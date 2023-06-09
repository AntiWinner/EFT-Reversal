using System;

namespace EFT.UI;

[Flags]
public enum EValidationType
{
	Numbers = 1,
	Latin = 2,
	AnyWordChars = 4,
	Space = 8,
	Hyphen = 0x10,
	Underscore = 0x20,
	Period = 0x40,
	Comma = 0x80,
	Slash = 0x100,
	Brackets = 0x200,
	Exclamation = 0x400,
	Question = 0x800,
	Quotes = 0x1000,
	At = 0x2000,
	Colons = 0x4000,
	Math = 0x8310,
	Separator = 0x10000,
	And = 0x20000,
	Dollar = 0x40000,
	NumSign = 0x80000,
	NewLine = 0x100000,
	Everything = -1,
	NickName = 0x33,
	BuildName = 0x3B,
	Punctuation = 0x5CC0,
	AllSymbols = 0xFFFF8,
	Notes = 0x1FFFFF,
	SearchField = 0xFFFFD
}
