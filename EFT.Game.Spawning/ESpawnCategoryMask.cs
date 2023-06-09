using System;

namespace EFT.Game.Spawning;

[Flags]
public enum ESpawnCategoryMask
{
	None = 0,
	Player = 1,
	Bot = 2,
	Boss = 4,
	Coop = 8,
	Group = 0x10,
	Opposite = 0x20,
	All = 7
}
