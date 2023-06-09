using System;

[Flags]
public enum EModClass
{
	None = 0,
	Functional = 1,
	Master = 2,
	Gear = 4,
	Auxiliary = 8,
	All = 7
}
