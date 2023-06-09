using System;

namespace FlyingWormConsole3.LiteNetLib;

[Flags]
public enum ConnectionState : byte
{
	Outgoing = 2,
	Connected = 4,
	ShutdownRequested = 8,
	Disconnected = 0x10,
	Any = 0xE
}
