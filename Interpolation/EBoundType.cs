using System;

namespace Interpolation;

[Flags]
public enum EBoundType
{
	Unknown = 0,
	Less = 1,
	LessOrEqual = 3,
	Equals = 6,
	GreaterOrEqual = 0xD,
	Greater = 0x1B
}
