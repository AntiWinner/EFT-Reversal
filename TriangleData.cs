using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[Serializable]
public class TriangleData
{
	[SerializeField]
	public int Id;

	public float CenterX;

	public float CenterY;

	public float CenterZ;

	public int[] Ids;

	public float[] Dists;

	public int[] Closest;

	public int PointAIndex;

	public int PointBIndex;

	public int PointCIndex;

	private Dictionary<int, float> _distances = new Dictionary<int, float>();

	public static TriangleData CalcNavTriangle(Vector3 pos, ZoneTriangleData zoneTriangleData, [CanBeNull] TriangleData lastTriangle)
	{
		TriangleData triangleData = null;
		float num = float.MaxValue;
		if (lastTriangle == null)
		{
			List<TriangleData> nearestTriangles = zoneTriangleData.GetNearestTriangles(pos);
			for (int i = 0; i < nearestTriangles.Count; i++)
			{
				TriangleData triangleData2 = nearestTriangles[i];
				float num2 = triangleData2.SqrDist(pos);
				if (num2 < num)
				{
					num = num2;
					triangleData = triangleData2;
				}
			}
			lastTriangle = triangleData;
		}
		else
		{
			for (int j = 0; j < lastTriangle.Closest.Length; j++)
			{
				int key = lastTriangle.Closest[j];
				TriangleData triangleData3 = zoneTriangleData.TrianglesD[key];
				float num3 = triangleData3.SqrDist(pos);
				if (num > num3)
				{
					num = num3;
					triangleData = triangleData3;
				}
			}
			float num4 = lastTriangle.SqrDist(pos);
			if (num < num4)
			{
				lastTriangle = triangleData;
			}
		}
		return lastTriangle;
	}

	public TriangleData()
	{
	}

	public TriangleData(float centerX, float centerY, float centerZ)
	{
		CenterX = centerX;
		CenterY = centerY;
		CenterZ = centerZ;
	}

	public TriangleData(NavTriangle tr)
	{
		Id = tr.Index;
		PointAIndex = tr.Point1.Index;
		PointBIndex = tr.Point2.Index;
		PointCIndex = tr.Point3.Index;
		CenterX = tr.Center.x;
		CenterY = tr.Center.y;
		CenterZ = tr.Center.z;
		Ids = new int[tr.PathDistancesSave.Count];
		Dists = new float[tr.PathDistancesSave.Count];
		int num = 0;
		foreach (IntFloat item in tr.PathDistancesSave)
		{
			Ids[num] = item.Index;
			Dists[num] = item.Value;
			num++;
		}
	}

	public void ReworkToDictionary()
	{
		_distances = new Dictionary<int, float>();
		for (int i = 0; i < Ids.Length; i++)
		{
			int key = Ids[i];
			float value = Dists[i];
			_distances.Add(key, value);
		}
		Ids = null;
		Dists = null;
	}

	public bool TryGetValue(int id, out float dist)
	{
		return _distances.TryGetValue(id, out dist);
	}

	public float SqrDist(Vector3 position)
	{
		float num = CenterX - position.x;
		float num2 = CenterY - position.y;
		float num3 = CenterZ - position.z;
		return num * num + num2 * num2 + num3 * num3;
	}

	public Vector3 GetCenter()
	{
		return new Vector3(CenterX, CenterY, CenterZ);
	}
}
