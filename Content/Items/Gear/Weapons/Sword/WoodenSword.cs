﻿using PathOfTerraria.Core.Systems;
using Terraria.DataStructures;
using Terraria.ID;

namespace PathOfTerraria.Content.Items.Gear.Weapons.Sword;

internal class WoodenSword : Sword
{
	public override string Texture => $"{PathOfTerraria.ModName}/Assets/Items/Gear/Weapons/Sword/WoodenSword";

	public override float DropChance => 1f;
	public override int ItemLevel => 1;

	public override void SetDefaults()
	{
		base.SetDefaults();
		Item.damage = 4;
		Item.UseSound = SoundID.Item1;
		GearType = GearType.Sword;
	}
}