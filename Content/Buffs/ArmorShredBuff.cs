﻿
namespace PathOfTerraria.Content.Buffs;

public sealed class ArmorShredBuff : ModBuff
{
	private sealed class ArmorShredGlobalNPCImpl : GlobalNPC
	{
		public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
		{
			base.ModifyHitByItem(npc, player, item, ref modifiers);

			if (!npc.HasBuff(ModContent.BuffType<ArmorShredBuff>()))
			{
				return;
			}

			modifiers.Defense *= _defenseMultiplier;
		}

		public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
		{
			if (!npc.HasBuff(ModContent.BuffType<ArmorShredBuff>()))
			{
				return;
			}

			modifiers.Defense *= _defenseMultiplier;
		}
	}

	private const float _defenseReductionPercent = 25f;
	private const float _defenseMultiplier = 1f - _defenseReductionPercent / 100f;
}