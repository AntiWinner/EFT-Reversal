using System;

namespace EFT;

[Flags]
public enum WildSpawnType
{
	marksman = 1,
	assault = 2,
	bossTest = 4,
	bossBully = 8,
	followerTest = 0x10,
	followerBully = 0x20,
	bossKilla = 0x40,
	bossKojaniy = 0x80,
	followerKojaniy = 0x100,
	pmcBot = 0x200,
	cursedAssault = 0x400,
	bossGluhar = 0x800,
	followerGluharAssault = 0x1000,
	followerGluharSecurity = 0x2000,
	followerGluharScout = 0x4000,
	followerGluharSnipe = 0x8000,
	followerSanitar = 0x10000,
	bossSanitar = 0x20000,
	test = 0x40000,
	assaultGroup = 0x80000,
	sectantWarrior = 0x100000,
	sectantPriest = 0x200000,
	bossTagilla = 0x400000,
	followerTagilla = 0x800000,
	exUsec = 0x1000000,
	gifter = 0x2000000,
	bossKnight = 0x4000000,
	followerBigPipe = 0x8000000,
	followerBirdEye = 0x10000000,
	bossZryachiy = 0x20000000,
	followerZryachiy = 0x40000000
}
