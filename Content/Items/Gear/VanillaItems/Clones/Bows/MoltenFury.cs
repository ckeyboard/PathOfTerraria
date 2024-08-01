﻿using Terraria.DataStructures;
using Terraria.ID;

namespace PathOfTerraria.Content.Items.Gear.VanillaItems.Clones.Bows;

internal class MoltenFury : VanillaClone
{
	protected override short VanillaItemId => ItemID.MoltenFury;

	public override void Defaults()
	{
		ItemType = Core.ItemType.Melee;
		base.Defaults();
	}

	public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
	{
		if (type == ProjectileID.WoodenArrowFriendly)
		{
			type = ProjectileID.FireArrow;
		}
	}
}