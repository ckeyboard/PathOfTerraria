﻿namespace PathOfTerraria.Core.Systems.Affixes.ItemTypes.WeaponAffixes;

internal class IncreasedAttackSpeedAffix : ItemAffix
{
	public override void ApplyAffix(EntityModifier modifier, PoTItem item)
	{
		modifier.Attackspeed += Value;
	}
}

internal class AddedAttackSpeedAffix : ItemAffix
{
	public override void ApplyAffix(EntityModifier modifier, PoTItem item)
	{
		modifier.Attackspeed.Base += Value;
	}
}

internal class AddedDamageAffix : ItemAffix
{
	public override void ApplyAffix(EntityModifier modifier, PoTItem item)
	{
		modifier.Damage.Base += Value;
	}
}

internal class IncreasedDamageAffix : ItemAffix
{
	public override void ApplyAffix(EntityModifier modifier, PoTItem item)
	{
		modifier.Damage += Value;
	}
}

internal class ChargedItemAffix : ItemAffix
{
	public override void ApplyAffix(EntityModifier modifier, PoTItem item)
	{
		modifier.Damage *= Value;
	}
}