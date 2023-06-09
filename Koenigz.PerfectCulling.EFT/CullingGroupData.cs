using System;
using System.Collections.Generic;
using UnityEngine;

namespace Koenigz.PerfectCulling.EFT;

[Serializable]
[PreferBinarySerialization]
public sealed class CullingGroupData
{
	public ushort[] Indices;

	public ushort RuntimeCullingGroupIdx;

	public HashSet<ushort> BuildTimeIndices;
}
