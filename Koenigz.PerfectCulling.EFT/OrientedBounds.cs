using System;
using UnityEngine;

namespace Koenigz.PerfectCulling.EFT;

[Serializable]
public struct OrientedBounds
{
	public Bounds bounds;

	public Quaternion rotation;

	public Bounds AABB
	{
		get
		{
			Bounds result = default(Bounds);
			result.center = bounds.center;
			Vector3 extents = bounds.extents;
			Vector3 vector = rotation * new Vector3(extents.x, extents.y, extents.z);
			result.Encapsulate(vector + bounds.center);
			result.Encapsulate(-vector + bounds.center);
			vector = rotation * new Vector3(extents.x, extents.y, 0f - extents.z);
			result.Encapsulate(vector + bounds.center);
			result.Encapsulate(-vector + bounds.center);
			vector = rotation * new Vector3(extents.x, 0f - extents.y, extents.z);
			result.Encapsulate(vector + bounds.center);
			result.Encapsulate(-vector + bounds.center);
			vector = rotation * new Vector3(extents.x, 0f - extents.y, 0f - extents.z);
			result.Encapsulate(vector + bounds.center);
			result.Encapsulate(-vector + bounds.center);
			return result;
		}
	}

	public bool Contains(Vector3 worldPosition)
	{
		return bounds.Contains(Quaternion.Inverse(rotation) * (worldPosition - bounds.center));
	}
}
