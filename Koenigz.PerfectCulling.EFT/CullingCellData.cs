using System;
using System.Collections.Generic;
using UnityEngine;

namespace Koenigz.PerfectCulling.EFT;

[Serializable]
public sealed class CullingCellData : ISpatialItem
{
	public OrientedBounds CellBounds;

	public List<CullingGroupData> CullingData;

	public bool UserModified;

	public bool Ignored;

	[NonSerialized]
	public int RuntimeCellId;

	public Bounds SpatialItemBounds => CellBounds.AABB;
}
