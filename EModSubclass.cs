using System;

[Flags]
public enum EModSubclass
{
	None = 0,
	Bipod = 1,
	IronSight = 2,
	Launcher = 4,
	LightLaser = 8,
	Mount = 0x10,
	MuzzleMod = 0x20,
	Railcovers = 0x40,
	SightMod = 0x80,
	TacticalCombo = 0x100,
	Magazine = 0x200,
	Gasblock = 0x400,
	Handguard = 0x800,
	Receiver = 0x1000,
	PistolGrip = 0x2000,
	Auxiliary = 0x4000,
	Charge = 0x8000,
	Stock = 0x10000,
	Barrel = 0x20000
}
