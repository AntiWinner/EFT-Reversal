using System;

namespace EFT.Interactive;

[Flags]
public enum EVolumeRelationsMask
{
	Self = 1,
	Connected = 2,
	NotRelative = 4,
	Vertical = 8,
	Isolated = 0x10,
	Stairs = 0x20
}
