﻿using PathOfTerraria.Common.Enums;
using Terraria.DataStructures;
using Terraria.ID;

namespace PathOfTerraria.Content.Items.Gear.VanillaItems.Clones.Guns;

internal class Gatligator : VanillaClone
{
	protected override short VanillaItemId => ItemID.Gatligator;

	public override void Defaults()
	{
		ItemType = ItemType.Melee;
		base.Defaults();
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		float speed = velocity.Length();

		if (float.IsNaN(velocity.X) && float.IsNaN(velocity.Y) || velocity.X == 0f && velocity.Y == 0f)
		{
			velocity.X = player.direction;
			velocity.Y = 0f;
			speed = Item.shootSpeed;
		}
		else
		{
			speed = Item.shootSpeed / speed;
		}

		velocity.X += Main.rand.Next(-50, 51) * 0.03f / speed;
		velocity.Y += Main.rand.Next(-50, 51) * 0.03f / speed;

		Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);

		return false;
	}
}
