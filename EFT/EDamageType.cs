using System;

namespace EFT;

[Flags]
public enum EDamageType
{
	Undefined = 1,
	Fall = 2,
	Explosion = 4,
	Barbed = 8,
	Flame = 0x10,
	GrenadeFragment = 0x20,
	Impact = 0x40,
	Existence = 0x80,
	Medicine = 0x100,
	Bullet = 0x200,
	Melee = 0x400,
	Landmine = 0x800,
	Sniper = 0x1000,
	Blunt = 0x2000,
	LightBleeding = 0x4000,
	HeavyBleeding = 0x8000,
	Dehydration = 0x10000,
	Exhaustion = 0x20000,
	RadExposure = 0x40000,
	Stimulator = 0x80000,
	Poison = 0x100000,
	LethalToxin = 0x200000
}
