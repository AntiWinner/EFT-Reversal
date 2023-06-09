namespace EFT.InventoryLogic;

public interface IWeapon
{
	float Weight { get; }

	float RecoilForceBack { get; }

	float RecoilBase { get; }

	float SpeedFactor { get; }

	bool IsUnderbarrelWeapon { get; }

	float RecoilDelta { get; }

	Weapon.MalfunctionState MalfState { get; }

	Item Item { get; }

	WeaponTemplate WeaponTemplate { get; }

	int GetCurrentMagazineCount();
}
