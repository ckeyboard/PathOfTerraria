﻿using PathOfTerraria.Content.Projectiles.Whip;
using PathOfTerraria.Common.Systems;
using ReLogic.Content;
using System.Collections.Generic;
using PathOfTerraria.Common.Systems;
using Terraria.DataStructures;
using Terraria.Localization;

namespace PathOfTerraria.Content.Items.Gear.Weapons.Whip;

internal abstract class Whip : Gear
{
	/// <summary>
	/// Defines the draw data for a particular whip.
	/// </summary>
	public readonly struct WhipDrawData(Point baseSize, Rectangle first, Rectangle second, Rectangle third, Rectangle tip, bool drawLine)
	{ 
		public readonly Point BaseSize = baseSize;
		public readonly Rectangle FirstSegmentSource = first;
		public readonly Rectangle SecondSegmentSource = second;
		public readonly Rectangle ThirdSegmentSource = third;
		public readonly Rectangle TipSource = tip;
		public readonly bool DrawLine = drawLine;
	}

	public override float DropChance => 1f;

	public abstract WhipDrawData DrawData { get; }
	public abstract WhipSettings WhipSettings { get; }
	protected override string GearLocalizationCategory => "Whip";
	public override string AltUseDescription => Language.GetTextValue("Mods.PathOfTerraria.Gear.Whip.AltUse");

	/// <summary>
	/// Stores a Whip's sprite asset automatically for use in <see cref="BowAnimationProjectile"/>.
	/// </summary>
	public static Dictionary<int, Asset<Texture2D>> WhipProjectileSpritesById = [];

	public override void SetStaticDefaults()
	{
		if (ModContent.HasAsset(Texture + "_Projectile"))
		{
			WhipProjectileSpritesById.Add(Type, ModContent.Request<Texture2D>(Texture + "_Projectile"));
		}
	}

	public override void Defaults()
	{
		Item.DefaultToWhip(ModContent.ProjectileType<WhipBaseProjectile>(), 5, 2, 4);
		Item.channel = true;
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		if (player.altFunctionUse == 2)
		{
			int proj = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
			Main.projectile[proj].ai[2] = 1;
			Item.useAnimation = 60;
			player.itemAnimation = player.itemAnimationMax = 60;

			player.GetModPlayer<AltUsePlayer>().SetAltCooldown(300);
			return false;
		}

		player.itemAnimation = player.itemAnimationMax = 30;
		Item.useAnimation = 30;
		return true;
	}
}