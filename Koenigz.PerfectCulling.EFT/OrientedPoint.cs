using System;
using UnityEngine;

namespace Koenigz.PerfectCulling.EFT;

[Serializable]
public struct OrientedPoint
{
	public Vector3 position;

	public Quaternion rotation;
}
