﻿namespace PathOfTerraria.Core.Systems.Affixes.ItemTypes;

internal class NoFallDamageAffix : ItemAffix
{
	public override void ApplyAffix(Player player, EntityModifier modifier, PoTItem gear)
	{
		if (player != null)
		{
			player.noFallDmg = true;
		}
	}
}

internal class FetidCarapaceAffix : ItemAffix
{
	public override void OnLoad()
	{
		OnSwapPlayer.OnSwapMainItem += EnableFetidCarapaceIfOpen;
	}

	private void EnableFetidCarapaceIfOpen(Player self, Item newItem, Item oldItem)
	{
		if (newItem.type == ModContent.ItemType<Rottenbone>())
		{
			SkillPlayer skillPlayer = self.GetModPlayer<SkillPlayer>();
			skillPlayer.TryAddSkill(new FetidCarapace());
		}
	}
}