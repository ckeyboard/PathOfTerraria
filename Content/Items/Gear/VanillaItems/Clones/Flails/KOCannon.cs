﻿using Terraria.ID;

namespace PathOfTerraria.Content.Items.Gear.VanillaItems.Clones.Miscellaneous;

internal class KOCannon : VanillaClone
{
	protected override short VanillaItemId => ItemID.KOCannon;

	public override void Defaults()
	{
		ItemType = Core.ItemType.Melee;
		base.Defaults();
	}

	public override Vector2? HoldoutOffset()
	{
		return new Vector2(4, 0);
	}
}