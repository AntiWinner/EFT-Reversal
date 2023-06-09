using System;

namespace EFT.InventoryLogic;

[Flags]
public enum EWeaponModType
{
	mod_mount = 1,
	mod_scope = 2,
	mod_tactical = 4,
	mod_stock = 8,
	mod_magazine = 0x10,
	mod_barrel = 0x20,
	mod_handguard = 0x40,
	mod_muzzle = 0x80,
	mod_sight_front = 0x100,
	mod_sight_rear = 0x200,
	mod_foregrip = 0x400,
	mod_reciever = 0x800,
	mod_charge = 0x1000,
	mod_pistol_grip = 0x2000,
	mod_launcher = 0x4000,
	mod_bipod = 0x8000,
	mod_mag_shaft = 0x10000,
	mod_silencer = 0x20000,
	mod_tactical_2 = 0x40000,
	chamber0 = 0x80000,
	chamber1 = 0x100000,
	patron_in_weapon = 0x200000,
	mod_gas_block = 0x400000,
	mod_equipment = 0x800000,
	mod_equipment_000 = 0x1000000,
	mod_equipment_001 = 0x2000000,
	mod_nvg = 0x4000000,
	mod_flashlight = 0x8000000,
	mod_muzzle_001 = 0x10000000
}
