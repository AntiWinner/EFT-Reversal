using System;

namespace EFT.Interactive;

[Flags]
public enum EDoorState : byte
{
	Locked = 1,
	Shut = 2,
	Open = 4,
	Interacting = 8,
	Breaching = 0x10
}
