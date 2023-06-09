using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TriangleCache
{
	public Vector3 centerPosition;

	public List<TriangleData> triangles = new List<TriangleData>();
}
