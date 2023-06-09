using System;

namespace EFT;

[Flags]
public enum ResourceType
{
	Ammo = 1,
	Weapon = 2,
	Magazine = 4,
	Item = 8,
	UsablePrefab = 0x10,
	Player = 0x20,
	WeaponModItem = 0x40,
	SynchronizableObject = 0x80,
	Other = 0x100
}
