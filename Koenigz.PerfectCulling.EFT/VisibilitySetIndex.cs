using System;

namespace Koenigz.PerfectCulling.EFT;

[Serializable]
public struct VisibilitySetIndex
{
	public int offset;

	public ushort numDeltaValues;

	public ushort dataLength;
}
