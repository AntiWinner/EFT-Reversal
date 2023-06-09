using System;
using UnityEngine;

public class RoadsGeneratorFixed : MonoBehaviour
{
	[Serializable]
	public class RoadPoint
	{
		public Transform Point;

		public float Width;

		[HideInInspector]
		public Vector3[] Points;

		[HideInInspector]
		public Vector3[] Normals;
	}

	[SerializeField]
	private Material _material;

	[SerializeField]
	private float _textureLength = 0.5f;

	[SerializeField]
	private float _offsetAmount = 0.1f;

	[SerializeField]
	private float _halfRaycastLength = 10f;

	[SerializeField]
	public int OneSideAdditionalPointsAmount;

	[SerializeField]
	private LayerMask _mask;

	[SerializeField]
	private RoadPoint[] _roadPoints = new RoadPoint[0];

	private int m__E000;

	private void Awake()
	{
		this.m__E000 = OneSideAdditionalPointsAmount * 2 + 1;
		_E000(_roadPoints);
		RoadPoint[] roadPoints = _roadPoints;
		foreach (RoadPoint roadPoint in roadPoints)
		{
			_E001(roadPoint, roadPoint.Point.right);
			Vector3[] points = roadPoint.Points;
			for (int j = 0; j < points.Length; j++)
			{
				Debug.DrawRay(points[j], Vector3.up, Color.green, 900f);
			}
		}
		Mesh mesh = new Mesh
		{
			name = _ED3E._E000(48138)
		};
		mesh.vertices = _E002(_roadPoints);
		mesh.triangles = _E003(mesh.vertices);
		mesh.normals = _E004(_roadPoints);
		roadPoints = _roadPoints;
		foreach (RoadPoint roadPoint2 in roadPoints)
		{
			for (int k = 0; k < roadPoint2.Points.Length; k++)
			{
				Debug.DrawRay(roadPoint2.Points[k], roadPoint2.Normals[k], Color.yellow, 900f);
			}
		}
		mesh.uv = _E005(_roadPoints, mesh.vertexCount);
		GameObject obj = new GameObject(_ED3E._E000(48179));
		obj.transform.position = Vector3.zero;
		obj.transform.rotation = Quaternion.identity;
		obj.AddComponent<MeshFilter>().sharedMesh = mesh;
		obj.AddComponent<MeshRenderer>().material = _material;
	}

	private void _E000(RoadPoint[] points)
	{
	}

	private void _E001(RoadPoint roadPoint, Vector3 direction)
	{
		float num = roadPoint.Width / (float)(2 * OneSideAdditionalPointsAmount);
		roadPoint.Points = new Vector3[OneSideAdditionalPointsAmount * 2 + 1];
		roadPoint.Normals = new Vector3[roadPoint.Points.Length];
		roadPoint.Points[roadPoint.Points.Length / 2 + 1] = roadPoint.Point.position;
		for (int i = 0; i < OneSideAdditionalPointsAmount * 2 + 1; i++)
		{
			if (Physics.Raycast(roadPoint.Point.position + direction * num * (i - OneSideAdditionalPointsAmount) + new Vector3(0f, _halfRaycastLength), Vector3.down, out var hitInfo, _halfRaycastLength * 2f, _mask))
			{
				roadPoint.Points[i] = hitInfo.point + new Vector3(0f, _offsetAmount);
				roadPoint.Normals[i] = hitInfo.normal;
				if (i > 0)
				{
					Debug.DrawLine(roadPoint.Points[i - 1], roadPoint.Points[i], Color.red, 150f);
				}
			}
		}
	}

	private Vector3[] _E002(RoadPoint[] roadPoints)
	{
		Vector3[] array = new Vector3[_roadPoints.Length * OneSideAdditionalPointsAmount * 2 + _roadPoints.Length];
		int num = OneSideAdditionalPointsAmount * 2 + 1;
		for (int i = 0; i < roadPoints.Length; i++)
		{
			for (int j = 0; j < num; j++)
			{
				array[i * num + j] = roadPoints[i].Points[j];
			}
		}
		return array;
	}

	private int[] _E003(Vector3[] vert)
	{
		int[] array = new int[(vert.Length - this.m__E000) * 6];
		for (int i = 0; i < vert.Length - this.m__E000; i++)
		{
			if ((i + 1) % this.m__E000 != 0 || i == 0)
			{
				array[i * 6] = i;
				array[i * 6 + 1] = i + this.m__E000;
				array[i * 6 + 2] = i + 1;
				array[i * 6 + 3] = i + 1;
				array[i * 6 + 4] = i + this.m__E000;
				array[i * 6 + 5] = i + this.m__E000 + 1;
			}
		}
		return array;
	}

	private Vector3[] _E004(RoadPoint[] roadPoints)
	{
		Vector3[] array = new Vector3[_roadPoints.Length * OneSideAdditionalPointsAmount * 2 + _roadPoints.Length];
		int num = OneSideAdditionalPointsAmount * 2 + 1;
		for (int i = 0; i < roadPoints.Length; i++)
		{
			for (int j = 0; j < num; j++)
			{
				array[i * num + j] = roadPoints[i].Normals[j];
			}
		}
		return array;
	}

	private Vector2[] _E005(RoadPoint[] points, int vertLen)
	{
		int num = 0;
		float x = 0f;
		float num2 = points[0].Width / (float)(this.m__E000 - 1) / _textureLength;
		Vector2[] array = new Vector2[vertLen];
		int num3 = 0;
		for (int i = 0; i < vertLen; i++)
		{
			if (i % this.m__E000 == 0 && i != 0)
			{
				num3 = 0;
				num++;
				num2 = points[num].Width / (float)(this.m__E000 - 1) / _textureLength;
				if (num < points.Length && i != 0)
				{
					x = (points[num - 1].Point.position - points[num].Point.position).magnitude / _textureLength;
				}
			}
			array[i] = new Vector2(x, num2 * (float)num3);
			num3++;
		}
		return array;
	}
}
