﻿using System.Collections.Generic;
using System.Linq;
using PathOfTerraria.Common.Systems.Affixes;
using PathOfTerraria.Core.Items;

namespace PathOfTerraria.Common.Systems.ModPlayers;
internal class UniversalBuffingPlayer : ModPlayer
{
	public EntityModifier UniversalModifier;
	public AffixTooltipsHandler AffixTooltipHandler = new();

	public override void PostUpdateEquips()
	{
		if (!Player.inventory[0].IsAir)
		{
			PoTItemHelper.ApplyAffixes(Player.inventory[0], UniversalModifier, Player);
		}

		UniversalModifier.ApplyTo(Player);

		Player.statLifeMax = Math.Min(400, Player.statLifeMax2);
	}

	public override void ResetEffects()
	{
		UniversalModifier = new EntityModifier();
		AffixTooltipHandler.Reset();
	}
	
	/// <summary>
	/// Used to apply on hit effects for affixes that have them
	/// </summary>
	/// <param name="target"></param>
	/// <param name="hit"></param>
	/// <param name="damageDone"></param>
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		foreach (KeyValuePair<int, Dictionary<int, StatModifier>> buff in UniversalModifier.Buffer)
		{
			int id = buff.Key;
			int time = 0;

			float roll = Main.rand.NextFloat();

			foreach (KeyValuePair<int, StatModifier> instance in buff.Value)
			{
				if (roll <= (instance.Value.ApplyTo(1f) - 1f))
				{
					time = Math.Max(time, instance.Key);
				}
			}

			if (time > 0)
			{
				target.AddBuff(id, time);
			}
		}
	}

	public void PrepareComparisonTooltips(List<TooltipLine> tooltips, Item item)
	{
		List<ItemAffix> affixes = item.GetInstanceData().Affixes;
		List<Type> removals = [];

		foreach (KeyValuePair<Type, AffixTooltip> line in AffixTooltipHandler.Tooltips)
		{
			if (!affixes.Any(x => x.GetType() == line.Key))
			{
				removals.Add(line.Key);
			}
		}

		foreach (Type type in removals)
		{
			AffixTooltipHandler.Tooltips.Remove(type);
		}

		AffixTooltipHandler.ModifyTooltips(tooltips);
	}
}
