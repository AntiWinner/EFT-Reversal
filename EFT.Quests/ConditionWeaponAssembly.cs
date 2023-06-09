namespace EFT.Quests;

public sealed class ConditionWeaponAssembly : ConditionItem, _E921
{
	public _E923 durability;

	public _E923 ergonomics;

	public _E923 baseAccuracy;

	public _E923 recoil;

	public _E923 muzzleVelocity;

	public _E923 height;

	public _E923 magazineCapacity;

	public _E923 width;

	public _E923 weight;

	public _E923 effectiveDistance;

	public _E923 emptyTacticalSlot;

	public string[] hasItemFromCategory;

	public string[] containsItems;

	public override string FormattedDescription => string.Format(base.FormattedDescription, durability, ergonomics, baseAccuracy, recoil, muzzleVelocity, height, magazineCapacity, width, weight, effectiveDistance, emptyTacticalSlot);
}
