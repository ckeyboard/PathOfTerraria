﻿using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ObjectData;

namespace PathOfTerraria.Content.Tiles.BossDomain;

internal class EmbeddedSlimes : ModTile
{
	public override void SetStaticDefaults()
	{
		Main.tileFrameImportant[Type] = true;

		TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
		TileObjectData.newTile.CoordinateHeights = [16, 18];
		TileObjectData.newTile.StyleHorizontal = true;
		TileObjectData.addTile(Type);

		DustType = DustID.Stone;

		AddMapEntry(new Color(128, 128, 128));
	}

	public override void NearbyEffects(int i, int j, bool closer)
	{
		if (closer && Main.LocalPlayer.DistanceSQ(new Vector2(i, j).ToWorldCoordinates()) < 200 * 200)
		{
			WorldGen.KillTile(i, j);
		}
	}

	public override void KillMultiTile(int i, int j, int frameX, int frameY)
	{
		short type = frameX < 36 ? NPCID.GreenSlime : (frameX < 72 ? NPCID.BlueSlime : NPCID.RedSlime);
		NPC.NewNPC(new EntitySource_TileBreak(i, j), (i + 1) * 16, (j + 1) * 16, type, 0);

		for (int k = 0; k < 16; k++)
		{
			short dust = Main.rand.NextBool() ? DustID.t_Slime : DustID.Stone;
			Dust.NewDust(new Vector2(i, j + 1).ToWorldCoordinates(0, 0), 32, 16, dust, Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-4, -1));
		}
	}
}
