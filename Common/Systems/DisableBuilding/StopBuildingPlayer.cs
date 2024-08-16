﻿using Mono.Cecil.Cil;
using MonoMod.Cil;
using PathOfTerraria.Content.Tiles.BossDomain;
using Terraria.ID;

namespace PathOfTerraria.Common.Systems.DisableBuilding;

internal class StopBuildingPlayer : ModPlayer
{
	/// <summary>
	/// Stops the player from building if true. This is reset every frame.
	/// </summary>
	public bool ConstantStopBuilding = false;

	/// <summary>
	/// I don't know why, but <see cref="DisableMining(ILContext)"/> doesn't work due to method order. /shrug<br/>
	/// Use this if <see cref="ConstantStopBuilding"/> doesn't work for some reason.
	/// </summary>
	public bool LastStopBuilding = false;

	public override void Load()
	{
		IL_Player.PickTile += DisableMining;
		IL_Player.PickWall += DisableMiningWall;
		IL_Player.ItemCheck_CutTiles += DisableCut;
	}

	private static void DisableMining(ILContext il)
	{
		ILCursor c = new(il);
		ILLabel label = c.DefineLabel();

		c.Emit(OpCodes.Ldarg_0);
		c.Emit(OpCodes.Ldarg_1);
		c.Emit(OpCodes.Ldarg_2);
		c.EmitDelegate((Player player, int x, int y) => player.GetModPlayer<StopBuildingPlayer>().CanDig(x, y, false));
		c.Emit(OpCodes.Brfalse, label);
		c.Emit(OpCodes.Ret);
		c.MarkLabel(label);
	}

	private static void DisableMiningWall(ILContext il)
	{
		ILCursor c = new(il);
		ILLabel label = c.DefineLabel();

		c.Emit(OpCodes.Ldarg_0);
		c.Emit(OpCodes.Ldarg_1);
		c.Emit(OpCodes.Ldarg_2);
		c.EmitDelegate((Player player, int x, int y) => player.GetModPlayer<StopBuildingPlayer>().CanDig(x, y, true));
		c.Emit(OpCodes.Brfalse, label);
		c.Emit(OpCodes.Ret);
		c.MarkLabel(label);
	}

	private static void DisableCut(ILContext il)
	{
		ILCursor c = new(il);
		ILLabel label = c.DefineLabel();

		c.Emit(OpCodes.Ldarg_0);
		c.EmitDelegate((Player player) => player.GetModPlayer<StopBuildingPlayer>().LastStopBuilding);
		c.Emit(OpCodes.Brfalse, label);
		c.Emit(OpCodes.Ret);
		c.MarkLabel(label);
	}

	private object CanDig(int x, int y, bool isWall)
	{
		if (!LastStopBuilding)
		{
			return false;
		}

		Tile tile = Main.tile[x, y];

		if (!isWall)
		{
			return tile.TileType == ModContent.TileType<WeakMalaise>();
		}

		return true;
	}

	public override void ResetEffects()
	{
		LastStopBuilding = ConstantStopBuilding;
		ConstantStopBuilding = false;
	}

	public override bool CanUseItem(Item item)
	{
		if (item.createTile >= TileID.Dirt || item.createWall > WallID.None || item.type == ItemID.IceRod || item.tileWand >= 0)
		{
			bool isRope = item.createTile >= TileID.Dirt && Main.tileRope[item.createTile];

			if (!isRope)
			{
				return !LastStopBuilding;
			}
		}

		return true;
	}
}
