using System;

namespace EFT;

[Flags]
public enum EReportType
{
	None = 0,
	[_E618("abuse")]
	Abuse = 1,
	[_E618("cheat")]
	Cheat = 2,
	[_E618("offensiveNickname")]
	OffensiveNickname = 4
}
