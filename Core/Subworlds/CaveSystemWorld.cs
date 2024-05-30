﻿using Humanizer;
using PathOfTerraria.Content.Items.Consumables.Maps;
using PathOfTerraria.Core.Subworlds.Passes;
using PathOfTerraria.Core.Subworlds.Passes.CaveSystemPasses;
using System.Collections.Generic;
using System.Linq;
using Terraria.WorldBuilding;

namespace PathOfTerraria.Core.Subworlds;

public class CaveRoom
{
	public Vector2 Position;
	public int Size;
	public int Connections;
	public List<CaveRoom> AllConnections = new List<CaveRoom>();

	public CaveRoom(Vector2 position, int size, int connections)
	{
		Position = position;
		Size = size;
		Connections = connections;
	}
}

/// <summary>
/// This is a world that can be manipulated for testing new world generations through the "newworld" command
/// </summary>
public class CaveSystemWorld : MappingWorld
{
	public override List<GenPass> Tasks => [
		new FillWorldPass(),
		new FillPositionsPass(),
		new SpawnAndBossPositionPass(),
		new RoomPositionPass(),
		new SpawnToBossPass(),
		new RemainingRoomLinkingPass(),
		new CaveClearingPass(),
		new RoomCorrectionPass(),
	];

	// idk why im using tuples for everything, just too lazy to make a separate class/struct...
	public static List<Vector2> AvailablePositions = new();
	public static List<CaveRoom> Rooms = new();
	// <pos, size, connections>
	public static List<Tuple<Vector2, Vector2>> Lines = new();

	public static CaveRoom SpawnRoom => Rooms[0];
	public static CaveRoom BossRoom => Rooms[1];

	public static CaveMap Map = null;

	public static void AddLine(int r1, int r2)
	{
		Rooms[r1].Connections++;
		Rooms[r2].Connections++;

		Rooms[r1].AllConnections.Add(Rooms[r2]);
		Rooms[r2].AllConnections.Add(Rooms[r1]);

		Lines.Add(new(Rooms[r1].Position, Rooms[r2].Position));
	}
	public static void AddLine(CaveRoom r1, CaveRoom r2)
	{
		r1.Connections++;
		r2.Connections++;

		r1.AllConnections.Add(r2);
		r2.AllConnections.Add(r1);

		Lines.Add(new(r1.Position, r2.Position));
	}
	public static void AddRoom(Vector2 position, int size)
	{
		Rooms.Add(new(position, size, 0));

		AvailablePositions = AvailablePositions.Where(point => point.Distance(position) > size + Map.ExtraRoomDist + Map.RoomSizeMax).ToList();
	}
}