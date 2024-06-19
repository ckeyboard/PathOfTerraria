﻿using System.Collections.Generic;
using PathOfTerraria.Content.Projectiles.Melee;
using PathOfTerraria.Core.Systems;
using PathOfTerraria.Core.Systems.Affixes;
using PathOfTerraria.Core.Systems.Affixes.ItemTypes.WeaponAffixes;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;

namespace PathOfTerraria.Content.Items.Gear.Weapons.Sword;

internal class FireStarter : Sword
{
	public override string Texture => $"{PathOfTerraria.ModName}/Assets/Items/Gear/Weapons/Sword/FireStarter";
	public override float DropChance => 5f;
	public override int ItemLevel => 1;
	public override bool IsUnique => true;
	public override string Description => Language.GetTextValue("Mods.PathOfTerraria.Items.FireStarter.Description");
	public override string AltUseDescription => Language.GetTextValue("Mods.PathOfTerraria.Items.FireStarter.AltUseDescription");

	public override void Defaults()
	{
		base.Defaults();
		Item.damage = 4;
		Item.height = 52;
		Item.UseSound = SoundID.Item1;
	}
	
	public override string GenerateName()
	{
		return $"[c/FF0000:{Language.GetTextValue("Mods.PathOfTerraria.Items.FireStarter.DisplayName")}]";
	}
	
	public override List<ItemAffix> GenerateAffixes()
	{
		var sharpAffix = (ItemAffix)Affix.CreateAffix<PassiveAffixes.SharpItemAffix>();
		sharpAffix.MinValue = 1;
		sharpAffix.MaxValue = 4;
		
		var onFireAffix = (ItemAffix)Affix.CreateAffix<ModifyHitAffixes.ChanceToApplyOnFireGearAffix>();
		onFireAffix.Value = 0.1f;
		return [sharpAffix, onFireAffix];
	}
	
	public override bool AltFunctionUse(Player player)
	{
		AltUseSystem modPlayer = player.GetModPlayer<AltUseSystem>();

		if (modPlayer.OnCooldown)
		{
			return false;
		}
		
		if (Main.myPlayer == player.whoAmI)
		{
			Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<FireStarterProjectile>(), Item.damage, 0, player.whoAmI);
		}
		
		modPlayer.AltFunctionCooldown = 300;
		modPlayer.AltFunctionActiveTimer = 160;
		return true;
	}
	
	public override bool CanUseItem(Player player)
	{
		AltUseSystem modPlayer = player.GetModPlayer<AltUseSystem>();
		bool altFunctionActive = modPlayer.AltFunctionActive; // Prevent the item from being used if the alt function is active to spawn projectile instead

		if (!altFunctionActive)
		{
			Item.noUseGraphic = false;
			Item.noMelee = false;
			return true;
		}

		Item.noUseGraphic = true;
		Item.noMelee = true;

		if (player.ownedProjectileCounts[ModContent.ProjectileType<FireStarterProjectile>()] <= 0)
		{
			Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<FireStarterProjectile>(), Item.damage, Item.knockBack, player.whoAmI);
			SoundEngine.PlaySound(SoundID.Item1, player.Center);
		}
		
		return false;
	}
	
	public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
	{
		AltUseSystem modPlayer = player.GetModPlayer<AltUseSystem>();
		
		if (modPlayer.AltFunctionActive)
		{
			target.AddBuff(BuffID.OnFire, 180);
		}
		
		base.OnHitNPC(player, target, hit, damageDone);
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position,
		Vector2 velocity, int type, int damage, float knockback)
	{
		return false;
	}
}