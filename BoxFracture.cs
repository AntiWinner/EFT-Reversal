using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Trigger))]
[RequireComponent(typeof(BoxCollider))]
[AddComponentMenu("Destruction/Box Fracture")]
[RequireComponent(typeof(Rigidbody))]
public class BoxFracture : BaseFracture
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public Vector3[] points;
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public int i;

		public _E000 CS_0024_003C_003E8__locals1;

		internal int _E000(Vector3 p1, Vector3 p2)
		{
			float sqrMagnitude = (p1 - CS_0024_003C_003E8__locals1.points[i]).sqrMagnitude;
			float sqrMagnitude2 = (p2 - CS_0024_003C_003E8__locals1.points[i]).sqrMagnitude;
			return sqrMagnitude.CompareTo(sqrMagnitude2);
		}
	}

	[CompilerGenerated]
	private new sealed class _E002
	{
		public Vector3 point;

		internal int _E000(Vector3 p1, Vector3 p2)
		{
			float sqrMagnitude = (p1 - point).sqrMagnitude;
			double num = (p2 - point).sqrMagnitude;
			return sqrMagnitude.CompareTo((float)num);
		}
	}

	private List<Vector3> m__E001;

	protected override void ImmediateFracture(Vector3[] points)
	{
		InitializeDestruction();
		this.m__E001 = new List<Vector3>(points);
		int i = 0;
		while (i < points.Length)
		{
			_E000(points[i]);
			this.m__E001.Sort(delegate(Vector3 p1, Vector3 p2)
			{
				float sqrMagnitude = (p1 - points[i]).sqrMagnitude;
				float sqrMagnitude2 = (p2 - points[i]).sqrMagnitude;
				return sqrMagnitude.CompareTo(sqrMagnitude2);
			});
			_E001(points[i]);
			if (Vertices.Count > 3)
			{
				NewVertices = Vertices.ToArray();
				NewTriangles = ConvexHull.ComputeIncremental(NewVertices);
				if (NewTriangles != null)
				{
					CreateShardMesh(points[i]);
				}
			}
			int num = i + 1;
			i = num;
		}
		FinalizeDestruction();
	}

	protected override IEnumerator SpreadFracture(Vector3[] points)
	{
		InitializeDestruction();
		this.m__E001 = new List<Vector3>(points);
		int num = 0;
		int num2 = points.Length;
		while (num2 > 0)
		{
			for (int i = 0; i < Mathf.Min(ShardsPerFrame, num2); i++)
			{
				int num3 = num2 - 1;
				num2 = num3;
				Vector3 point = points[num++];
				_E000(point);
				this.m__E001.Sort(delegate(Vector3 p1, Vector3 p2)
				{
					float sqrMagnitude = (p1 - point).sqrMagnitude;
					double num4 = (p2 - point).sqrMagnitude;
					return sqrMagnitude.CompareTo((float)num4);
				});
				_E001(point);
				if (Vertices.Count > 3)
				{
					NewVertices = Vertices.ToArray();
					NewTriangles = ConvexHull.ComputeIncremental(NewVertices);
					if (NewTriangles != null)
					{
						CreateShardMesh(point);
					}
				}
			}
			yield return null;
		}
		FinalizeDestruction();
	}

	private void _E000(Vector3 point)
	{
		Vector3 vector = point - MaxBounds;
		Vector3 vector2 = MinBounds - point;
		BaseFracture.InitPlanes[0].w = vector.x;
		BaseFracture.InitPlanes[1].w = vector.y;
		BaseFracture.InitPlanes[2].w = vector.z;
		BaseFracture.InitPlanes[3].w = vector2.x;
		BaseFracture.InitPlanes[4].w = vector2.y;
		BaseFracture.InitPlanes[5].w = vector2.z;
		Planes.Clear();
		Planes.AddRange(BaseFracture.InitPlanes);
	}

	private void _E001(Vector3 point)
	{
		float num = float.PositiveInfinity;
		for (int i = 1; i < this.m__E001.Count; i++)
		{
			Vector3 vector = this.m__E001[i] - point;
			float sqrMagnitude = vector.sqrMagnitude;
			if (!((double)sqrMagnitude > (double)num * (double)num))
			{
				Vector4 item = vector.normalized;
				item.w = (0f - Mathf.Sqrt(sqrMagnitude)) / 2f;
				Planes.Add(item);
				GetVerticesInPlane();
				if (Vertices.Count > 3)
				{
					_E003();
					num = _E002();
					continue;
				}
				break;
			}
			break;
		}
	}

	private new float _E002()
	{
		float num = Vertices[0].sqrMagnitude;
		int count = Vertices.Count;
		for (int i = 1; i < count; i++)
		{
			float sqrMagnitude = Vertices[i].sqrMagnitude;
			if ((double)num < (double)sqrMagnitude)
			{
				num = sqrMagnitude;
			}
		}
		return Mathf.Sqrt(num) * 2f;
	}

	private void _E003()
	{
		if (PlaneIndices.Count == Planes.Count)
		{
			return;
		}
		int num = 0;
		foreach (int planeIndex in PlaneIndices)
		{
			if (num != planeIndex)
			{
				Planes[num] = Planes[planeIndex];
			}
			num++;
		}
		int num2 = PlaneIndices.Count - num;
		if (num2 < 0)
		{
			Planes.RemoveRange(num - 1, num2);
		}
	}
}
