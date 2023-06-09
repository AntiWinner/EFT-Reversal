using System;

namespace FlyingWormConsole3.LiteNetLib;

[Flags]
public enum LocalAddrType
{
	IPv4 = 1,
	IPv6 = 2,
	All = 3
}
