using System;

[Flags]
public enum EMemberCategory
{
	Default = 0,
	Developer = 1,
	UniqueId = 2,
	Trader = 4,
	Group = 8,
	System = 0x10,
	ChatModerator = 0x20,
	ChatModeratorWithPermanentBan = 0x40,
	UnitTest = 0x80,
	Sherpa = 0x100,
	Emissary = 0x200
}
