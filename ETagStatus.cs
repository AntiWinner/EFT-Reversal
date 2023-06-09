using System;

[Flags]
public enum ETagStatus
{
	Unaware = 1,
	Aware = 2,
	Combat = 4,
	Solo = 8,
	Coop = 0x10,
	Bear = 0x20,
	Usec = 0x40,
	Scav = 0x80,
	TargetSolo = 0x100,
	TargetMultiple = 0x200,
	Healthy = 0x400,
	Injured = 0x800,
	BadlyInjured = 0x1000,
	Dying = 0x2000,
	Birdeye = 0x4000,
	Knight = 0x8000,
	BigPipe = 0x10000
}
