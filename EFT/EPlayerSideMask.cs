using System;

namespace EFT;

[Flags]
public enum EPlayerSideMask
{
	None = 0,
	Usec = 1,
	Bear = 2,
	Savage = 4,
	Pmc = 3,
	All = 7
}
