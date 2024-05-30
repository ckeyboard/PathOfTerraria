﻿using PathOfTerraria.Core.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.IO;
using Terraria.WorldBuilding;

namespace PathOfTerraria.Core.Subworlds.Passes.CaveSystemPasses;
internal class FillPositionsPass() : GenPass("TerrainFill", 1)
{
	protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
	{
		CaveSystemWorld.AvailablePositions.Clear();
		CaveSystemWorld.Lines.Clear();
		CaveSystemWorld.Rooms.Clear();
		for (int x = 0 + CaveSystemWorld.Map.RoomSizeMax; x < Main.maxTilesX - CaveSystemWorld.Map.RoomSizeMax; x++)
		{
			progress.Set(x / (float)Main.maxTilesX);
			for (int y = 0 + CaveSystemWorld.Map.RoomSizeMax; y < Main.maxTilesY - CaveSystemWorld.Map.RoomSizeMax; y++)
			{
				CaveSystemWorld.AvailablePositions.Add(new(x, y));
			}
		}
	}
}
