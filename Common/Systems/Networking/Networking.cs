﻿using System.IO;
using PathOfTerraria.Common.Systems.Networking.Handlers;
using PathOfTerraria.Common.Systems.Networking.Handlers.MapDevice;
using Terraria.ID;

namespace PathOfTerraria.Common.Systems.Networking;

internal static class Networking
{
	public enum Message : byte
	{
		/// <summary>
		/// Spawns experience. Signature:<br/>
		/// <c>byte target, int xpValue, Vector2 position, Vector2 velocity, bool spawnLocally = false</c>
		/// </summary>
		SpawnExperience,

		/// <summary>
		/// Syncs the usage of hotbar potions. Signature:<br/>
		/// <c>byte playerWhoAmI, bool isHealingPotion, byte newValue, bool runLocally = false</c>
		/// </summary>
		SetHotbarPotionUse,

		/// <summary>
		/// Syncs the rings of the Guardian Angel's hits. Signature:<br/>
		/// <c>byte playerWhoAmI, short npcWho, bool runLocally = false</c>
		/// </summary>
		SyncGuardianAngelHit,

		/// <summary>
		/// Spawns an NPC on the server. Signatures:<br/>
		/// <c>short npcId, Vector2 position</c><br/>
		/// <c>short npcId, Vector2 position, Vector2 velocity</c>
		/// </summary>
		SpawnNPCOnServer,

		/// <summary>
		/// Syncs placing an item in a map device. Signature:<br/>
		/// <c>byte fromWho, short itemId, Point16 entityKey</c>
		/// </summary>
		SyncMapDevicePlaceMap,

		/// <summary>
		/// 
		/// </summary>
		ConsumeMapOffOfDevice,

		/// <summary>
		/// Sets the index of a given Ravencrest structure. Signature:<br/>
		/// <c>string name, int index</c>
		/// </summary>
		SetRavencrestBuildingIndex,
		
		/// <summary>
		/// Syncs a condition drop to the server. Signature:<br/>
		/// <c>int id, bool add</c>
		/// </summary>
		SyncConditionalDrop,
	}

	internal static void HandlePacket(BinaryReader reader)
	{
		var message = (Message)reader.ReadByte();

		switch (message)
		{
			case Message.SpawnExperience:
				if (Main.netMode == NetmodeID.Server)
				{
					ExperienceHandler.ServerReceive(reader);
				}
				else
				{
					ExperienceHandler.ClientReceive(reader);
				}

				break;

			case Message.SetHotbarPotionUse:
				if (Main.netMode == NetmodeID.Server)
				{
					HotbarPotionHandler.ServerReceive(reader);
				}
				else
				{
					HotbarPotionHandler.ClientReceive(reader);
				}

				break;

			case Message.SyncGuardianAngelHit:
				if (Main.netMode == NetmodeID.Server)
				{
					SyncGuardianAngelHandler.ServerReceive(reader);
				}
				else
				{
					SyncGuardianAngelHandler.ClientReceive(reader);
				}

				break;

			case Message.SyncMapDevicePlaceMap:
				if (Main.netMode == NetmodeID.Server)
				{
					PlaceMapInDeviceHandler.ServerReceive(reader);
				}
				else
				{
					PlaceMapInDeviceHandler.ClientReceive(reader);
				}

				break;

			case Message.ConsumeMapOffOfDevice:
				if (Main.netMode == NetmodeID.Server)
				{
					ConsumeMapDeviceHandler.ServerReceive(reader);
				}
				else
				{
					ConsumeMapDeviceHandler.ClientReceive(reader);
				}

				break;

			case Message.SpawnNPCOnServer:
				if (Main.netMode == NetmodeID.Server)
				{
					SpawnNPCOnServerHandler.ServerRecieve(reader);
				}

				break;

			case Message.SetRavencrestBuildingIndex:
				if (Main.netMode == NetmodeID.Server)
				{
					RavencrestBuildingIndex.ServerRecieve(reader);
				}

				break;

			default:
				throw null;
		}
	}

	internal static ModPacket GetPacket(Message type)
	{
		ModPacket packet = PoTMod.Instance.GetPacket();
		packet.Write((byte)type);
		return packet;
	}
}
