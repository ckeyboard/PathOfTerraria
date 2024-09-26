﻿using PathOfTerraria.Content.Projectiles;
using PathOfTerraria.Common.Systems;
using System.Collections.Generic;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.WorldBuilding;
using PathOfTerraria.Common.Systems.DisableBuilding;
using Terraria.Enums;
using Terraria.Localization;
using PathOfTerraria.Common.World.Generation;
using Terraria.DataStructures;
using PathOfTerraria.Common.World.Passes;

namespace PathOfTerraria.Common.Subworlds.BossDomains;

public class QueenBeeDomain : BossDomainSubworld
{
	public override int Width => 800;
	public override int Height => 400;
	public override int[] WhitelistedCutTiles => [TileID.BeeHive, TileID.JungleVines];

	public bool BossSpawned = false;
	public bool ReadyToExit = false;

	public override List<GenPass> Tasks => [new PassLegacy("Reset", ResetStep), new PassLegacy("Tiles", GenTiles), new PassLegacy("Polish", Polish),
		new PassLegacy("Settle Liquids", SettleLiquidsStep.Generation)];

	private void Polish(GenerationProgress progress, GameConfiguration configuration)
	{
		Dictionary<Point, OpenFlags> grasses = [];

		for (int i = 1; i < Width - 1; ++i)
		{
			for (int j = 1; j < Height - 1; ++j)
			{
				Tile tile = Main.tile[i, j];

				if (tile.TileType == TileID.Mud)
				{
					OpenFlags flags = OpenExtensions.GetOpenings(i, j, false, false);

					if (flags != OpenFlags.None)
					{
						tile.TileType = TileID.JungleGrass;
						grasses.Add(new Point(i, j), flags);
					}
				}

				if (tile.WallType == WallID.None)
				{
					tile.WallType = WallID.MudUnsafe;
				}

				WorldGen.SquareTileFrame(i, j);
			}
		}

		foreach ((Point grass, OpenFlags flags) in grasses)
		{
			AddGrassDecor(grass.X, grass.Y, flags);
		}
	}

	private static void AddGrassDecor(int i, int j, OpenFlags flags)
	{
		if (flags.HasFlag(OpenFlags.Above))
		{
			WorldGen.PlaceTile(i, j - 1, TileID.JunglePlants, true, false, style: WorldGen.genRand.Next(24));
		}

		if (flags.HasFlag(OpenFlags.Below) && !WorldGen.genRand.NextBool(5))
		{
			int length = WorldGen.genRand.Next(2, 7);

			for (int k = 1; k < length; ++k)
			{
				if (Main.tile[i, j + k].HasTile)
				{
					break;
				}

				WorldGen.PlaceTile(i, j + k, TileID.JungleVines, true);
			}
		}
	}

	private void GenTiles(GenerationProgress progress, GameConfiguration configuration)
	{
		Main.spawnTileX = Width / 2;
		Main.spawnTileY = Height / 2 - 20;
		Main.worldSurface = 230;
		Main.rockLayer = 299;

		progress.Message = Language.GetTextValue($"Mods.{PoTMod.ModName}.Generation.Terrain");

		for (int x = 0; x < Main.maxTilesX; ++x)
		{
			for (int y = 0; y < Main.maxTilesY; ++y)
			{
				Tile tile = Main.tile[x, y];
				tile.TileType = TileID.Mud;
				tile.HasTile = true;
				tile.WallType = WallID.MudUnsafe;
			}

			progress.Value = (float)x / Main.maxTilesX;
		}

		StructureTools.PlaceByOrigin("Assets/Structures/BeeDomain/Arena_" + WorldGen.genRand.Next(2), new Point16(Width / 2, Height / 2), new(0.5f));

		// Replace hive with unsafe hive wall so it's not destroyed by the tunnel
		for (int i = 1; i < Width - 1; ++i)
		{
			for (int j = 1; j < Height - 1; ++j)
			{
				Tile tile = Main.tile[i, j];

				if (tile.WallType == WallID.Hive)
				{
					tile.WallType = WallID.HiveUnsafe;
				}
			}
		}

		DigTunnel(Width / 4, Height / 2 - WorldGen.genRand.Next(-50, 50), true);
		DigTunnel((int)(Width * 0.75f), Height / 2 - WorldGen.genRand.Next(-50, 50), false);
	}

	private void DigTunnel(int x, int y, bool left)
	{
		var original = new Point16(x, y);

		Vector2[] positions = Tunnel.GeneratePoints([new(x, y), Vector2.Lerp(new(x, y), new(Width / 2, Height / 2), 0.5f) 
			+ new Vector2(WorldGen.genRand.Next(-12, 12), WorldGen.genRand.Next(-12, 12)), new(Width / 2, Height / 2)], 8, 6);
		
		FastNoiseLite noise = new(WorldGen._genRandSeed);
		int breakTime = -1;

		foreach (Vector2 pos in positions)
		{
			if (Main.tile[pos.ToPoint()].WallType != WallID.MudUnsafe && breakTime == -1)
			{
				breakTime = 5;
			}

			if (breakTime > -1)
			{
				if (--breakTime == 0) // Makes sure the opening continues a little bit into the hive
				{
					break;
				}
			}

			WorldGen.digTunnel(pos.X, pos.Y, 0, 0, 5, (int)(noise.GetNoise(pos.X, pos.Y) * 4) + 8);
		}

		StructureTools.PlaceByOrigin($"Assets/Structures/BeeDomain/Mini_{(left ? "" : "R_")}{WorldGen.genRand.Next(2)}", original, new(left ? 1 : 0, 0.5f));
	}

	public override void OnEnter()
	{
		BossSpawned = false;
		ReadyToExit = false;
	}

	public override void Update()
	{
		Liquid.UpdateLiquid();
		Wiring.UpdateMech();

		TileEntity.UpdateStart();
		foreach (TileEntity te in TileEntity.ByID.Values)
		{
			te.Update();
		}

		TileEntity.UpdateEnd();

		Main.dayTime = true;
		Main.time = Main.dayLength / 2;
		Main.moonPhase = (int)MoonPhase.Full;

		foreach (Player player in Main.ActivePlayers)
		{
			player.GetModPlayer<StopBuildingPlayer>().ConstantStopBuilding = true;
		}

		if (!BossSpawned && NPC.AnyNPCs(NPCID.QueenBee))
		{
			BossSpawned = true;
		}

		if (BossSpawned && !NPC.AnyNPCs(NPCID.QueenBee) && !ReadyToExit)
		{
			Vector2 pos = new Vector2(Width / 2, Height / 2 - 8) * 16;
			Projectile.NewProjectile(Entity.GetSource_NaturalSpawn(), pos, Vector2.Zero, ModContent.ProjectileType<ExitPortal>(), 0, 0, Main.myPlayer);

			BossTracker.CachedBossesDowned.Add(NPCID.QueenBee);
			ReadyToExit = true;
		}
	}
}