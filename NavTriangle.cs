using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class NavTriangle
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public CustomNavigationPoint place;

		internal bool _E000(IntFloat x)
		{
			return x.Index == place.Id;
		}
	}

	public Vector3 Center;

	public int Index;

	public Dictionary<int, float> PathDistances = new Dictionary<int, float>();

	public List<IntFloat> PathDistancesSave = new List<IntFloat>();

	public NavPoint Point1;

	public NavPoint Point2;

	public NavPoint Point3;

	public static float CalculatePathLength(Vector3[] path)
	{
		Vector3 vector = path[0];
		float num = 0f;
		for (int i = 1; i < path.Length; i++)
		{
			Vector3 vector2 = path[i];
			Vector3 vector3 = vector - vector2;
			num += Mathf.Sqrt(vector3.x * vector3.x + vector3.y * vector3.y + vector3.z * vector3.z);
			vector = vector2;
		}
		return num;
	}

	public NavTriangle(NavPoint point1, NavPoint point2, NavPoint point3, int index)
	{
		Index = index;
		Point1 = point1;
		Point2 = point2;
		Point3 = point3;
		Center = (point1.Pos + point2.Pos + point3.Pos) / 3f;
	}

	public void ReworkToDictionary()
	{
		PathDistances = new Dictionary<int, float>();
		foreach (IntFloat item in PathDistancesSave)
		{
			PathDistances.Add(item.Index, item.Value);
		}
		PathDistancesSave = null;
	}

	public void AddDistance(CustomNavigationPoint place)
	{
		NavMeshPath navMeshPath = new NavMeshPath();
		if ((Center - place.CovPointsPlaceSerializable.Origin).magnitude < 20f && NavMesh.CalculatePath(Center, place.CovPointsPlaceSerializable.Origin, -1, navMeshPath))
		{
			float val = CalculatePathLength(navMeshPath.corners);
			if (PathDistancesSave.FirstOrDefault((IntFloat x) => x.Index == place.Id) == null)
			{
				PathDistancesSave.Add(new IntFloat(place.Id, val));
			}
		}
	}
}
