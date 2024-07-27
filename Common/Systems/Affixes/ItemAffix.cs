﻿using System.Collections.Generic;
using System.Linq;
using PathOfTerraria.Common.Data;
using PathOfTerraria.Common.Data.Models;
using PathOfTerraria.Common.Enums;

namespace PathOfTerraria.Common.Systems.Affixes;

public abstract class ItemAffix : Affix
{
	public Influence RequiredInfluence => GetData().GetInfluences();
	public ItemType PossibleTypes => GetData().GetEquipTypes();

	public virtual void ApplyAffix(EntityModifier modifier, Item gear) { }

	/// <summary>
	/// Retrieves the affix data for the current <see cref="ItemAffix"/> instance.
	/// </summary>
	/// <returns></returns>
	public ItemAffixData GetData()
	{
		return AffixRegistry.TryGetAffixData(GetType());
	}

	public string GetTooltip(Item gear)
	{
		EntityModifier modifier = new();
		ApplyAffix(modifier, gear);

		string tooltip = "";

		List<string> affixes = EntityModifier.GetChangeOnlyStrings(modifier);

		if (affixes.Count != 0)
		{
			tooltip = affixes.First(); // idk if there will ever be more..?
		}

		return tooltip;
	}
}