using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class WireSplineGenerator : MonoBehaviour
{
	[Serializable]
	public class Wire
	{
		public Vector3[] Positins;

		public Vector3[] Normals;

		public int Parts;

		private Vector3[] _optimized;

		public void GeneratePoints(float error)
		{
			_optimized = _E002(_E001(this), error);
		}

		public int GetTrianglesCount(_E000 shape)
		{
			int num = _optimized.Length - 1;
			return shape.TrisA.Length * 6 * num;
		}

		public int GetVerticesCount(_E000 shape)
		{
			int num = _optimized.Length;
			return shape.Vertices.Length * num;
		}

		public void Generate(_E000 shape, ref int vIndex, ref int tIndex, int[] triangles, Vector3[] positions, Vector2[] uv0, float error)
		{
			Vector3[] positions2 = _E002(_E001(this), error);
			_E001[] points = GeneratePoints(positions2);
			Generate(shape, points, ref vIndex, ref tIndex, triangles, positions, uv0);
		}

		public _E001[] GeneratePoints(Vector3[] positions)
		{
			int num = positions.Length;
			int num2 = num - 1;
			_E001[] array = new _E001[num];
			Vector3[] array2 = new Vector3[num];
			Vector3[] array3 = new Vector3[num];
			Vector3[] array4 = new Vector3[num];
			array2[0] = (positions[1] - positions[0]).normalized;
			array2[num2] = (positions[num2] - positions[num2 - 1]).normalized;
			for (int i = 1; i < num2; i++)
			{
				array2[i] = (positions[i + 1] - positions[i - 1]).normalized;
			}
			if (array2[0].y > 0.9f || array2[0].y < -0.9f)
			{
				array3[0] = new Vector3(1f, 0f, 0f);
			}
			else
			{
				array3[0] = new Vector3(0f, 1f, 0f);
			}
			array4[0] = Vector3.Cross(array2[0], array3[0]).normalized;
			array3[0] = Vector3.Cross(array2[0], array4[0]).normalized;
			for (int j = 1; j < num; j++)
			{
				array4[j] = Vector3.Cross(array2[j], -array3[j - 1]).normalized;
				array3[j] = Vector3.Cross(array2[j], array4[j]).normalized;
			}
			for (int k = 0; k < num; k++)
			{
				array[k] = new _E001
				{
					Position = positions[k],
					Right = array4[k],
					Upward = array3[k]
				};
			}
			return array;
		}

		public static void Generate(_E000 shape, _E001[] points, ref int vIndex, ref int tIndex, int[] triangles, Vector3[] positions, Vector2[] uv0)
		{
			int num = vIndex;
			int num2 = tIndex;
			for (int i = 0; i < points.Length; i++)
			{
				_E001 obj = points[i];
				for (int j = 0; j < shape.Vertices.Length; j++)
				{
					positions[num + j] = obj.Position + shape.Vertices[j].x * obj.Right + shape.Vertices[j].y * obj.Upward;
				}
				if (shape.Uv != null)
				{
					for (int k = 0; k < shape.Vertices.Length; k++)
					{
						uv0[num + k] = new Vector2(shape.Uv[k], 0f);
					}
				}
				if (i != 0)
				{
					for (int l = 0; l < shape.TrisA.Length; l++)
					{
						int num3 = num + shape.TrisA[l];
						int num4 = num3 - shape.Vertices.Length;
						int num5 = num + shape.TrisB[l];
						int num6 = num5 - shape.Vertices.Length;
						int num7 = num2 + l * 6;
						triangles[num7] = num3;
						triangles[num7 + 1] = num4;
						triangles[num7 + 2] = num6;
						triangles[num7 + 3] = num6;
						triangles[num7 + 4] = num5;
						triangles[num7 + 5] = num3;
					}
					num2 += shape.TrisA.Length * 6;
				}
				num += shape.Vertices.Length;
			}
			vIndex = num;
			tIndex = num2;
		}
	}

	public class _E000
	{
		public Vector2[] Vertices;

		public float[] Uv;

		public int[] TrisA;

		public int[] TrisB;
	}

	public struct _E001
	{
		public Vector3 Position;

		public Vector3 Upward;

		public Vector3 Right;
	}

	public Wire[] Wires;

	public int ShapeDetails = 5;

	public float ShapeSize = 0.1f;

	public float Error = 0.05f;

	private MeshFilter m__E000;

	public Mesh Mesh;

	private static _E000 _E000(int details, float size)
	{
		_E000 obj = new _E000();
		if (details > 2)
		{
			obj.Vertices = new Vector2[details];
			float num = (float)Math.PI * 2f / (float)details;
			float num2 = 0f;
			for (int i = 0; i < details; i++)
			{
				obj.Vertices[i] = new Vector2(Mathf.Sin(num2), Mathf.Cos(num2)) * size;
				num2 += num;
			}
			obj.TrisA = new int[details];
			obj.TrisB = new int[details];
			for (int j = 0; j < details; j++)
			{
				obj.TrisA[j] = j;
				obj.TrisB[j] = ((j > 0) ? (j - 1) : (details - 1));
			}
		}
		else if (details == 2)
		{
			obj.Vertices = new Vector2[4]
			{
				new Vector2(0f, size),
				new Vector2(0f, 0f - size),
				new Vector2(size, 0f),
				new Vector2(0f - size, 0f)
			};
			obj.Uv = new float[4] { 0f, 1f, 0f, 1f };
			obj.TrisA = new int[2] { 0, 2 };
			obj.TrisB = new int[2] { 1, 3 };
		}
		else if (details == 1)
		{
			obj.Vertices = new Vector2[2]
			{
				new Vector2(0f, size),
				new Vector2(0f, 0f - size)
			};
			obj.Uv = new float[2] { 0f, 1f };
			obj.TrisA = new int[1];
			obj.TrisB = new int[1] { 1 };
		}
		else if (details < 1)
		{
			obj.Vertices = new Vector2[2]
			{
				new Vector2(size, 0f),
				new Vector2(0f - size, 0f)
			};
			obj.Uv = new float[2] { 0f, 1f };
			obj.TrisA = new int[1];
			obj.TrisB = new int[1] { 1 };
		}
		return obj;
	}

	public void Generate()
	{
		if (Wires != null && Wires.Length != 0)
		{
			_E000 shape = _E000(ShapeDetails, ShapeSize);
			int num = 0;
			int num2 = 0;
			Wire[] wires = Wires;
			foreach (Wire wire in wires)
			{
				wire.GeneratePoints(Error);
				num += wire.GetTrianglesCount(shape);
				num2 += wire.GetVerticesCount(shape);
			}
			int[] triangles = new int[num];
			Vector3[] array = new Vector3[num2];
			Vector2[] array2 = new Vector2[num2];
			int tIndex = 0;
			int vIndex = 0;
			wires = Wires;
			for (int i = 0; i < wires.Length; i++)
			{
				wires[i].Generate(shape, ref vIndex, ref tIndex, triangles, array, array2, Error);
			}
			if (this.m__E000 == null)
			{
				this.m__E000 = GetComponent<MeshFilter>() ?? base.gameObject.AddComponent<MeshFilter>();
			}
			if (Mesh == null)
			{
				MeshFilter meshFilter = this.m__E000;
				Mesh obj = new Mesh
				{
					vertices = array,
					triangles = triangles,
					uv = array2,
					name = _ED3E._E000(96693)
				};
				Mesh mesh = obj;
				Mesh = obj;
				meshFilter.mesh = mesh;
			}
			else
			{
				Mesh.Clear();
				Mesh.vertices = array;
				Mesh.triangles = triangles;
				Mesh.uv = array2;
			}
			Mesh.RecalculateBounds();
			Mesh.RecalculateNormals();
		}
	}

	private static Vector3[] _E001(Wire wire)
	{
		int num = wire.Parts + 1;
		Vector3[] array = new Vector3[num * (wire.Positins.Length - 1) - (wire.Positins.Length - 2)];
		float num2 = 1f / (float)wire.Parts;
		Vector3[] positins = wire.Positins;
		Vector3[] normals = wire.Normals;
		int i = 1;
		int num3 = 0;
		for (; i < positins.Length; i++)
		{
			Vector3 p = positins[i - 1];
			Vector3 p2 = positins[i];
			Vector3 n = normals[i - 1];
			Vector3 n2 = normals[i];
			float num4 = 0f;
			int num5 = 0;
			while (num5 < num)
			{
				array[num3] = GetCurve(num4, p, n, n2, p2);
				num4 += num2;
				num5++;
				num3++;
			}
			num3--;
		}
		return array;
	}

	public static Vector3[] GetPoints(Vector3 p0, Vector3 p1, Vector3 n0, Vector3 n1, int parts)
	{
		int num = parts + 1;
		float num2 = 1f / (float)parts;
		Vector3[] array = new Vector3[num];
		float num3 = 0f;
		for (int i = 0; i < num; i++)
		{
			array[i] = GetCurve(num3, p0, n0, n1, p1);
			num3 += num2;
		}
		return array;
	}

	private static Vector3[] _E002(Vector3[] points, float error)
	{
		LinkedList<Vector3> linkedList = new LinkedList<Vector3>(points);
		int num = int.MaxValue;
		while (linkedList.Count < num)
		{
			num = linkedList.Count;
			_E003(linkedList, error);
			error *= 0.5f;
		}
		points = new Vector3[linkedList.Count];
		linkedList.CopyTo(points, 0);
		return points;
	}

	private static void _E003(LinkedList<Vector3> points, float error)
	{
		LinkedListNode<Vector3> linkedListNode = points.First;
		while (linkedListNode.Next != null && linkedListNode.Next.Next != null)
		{
			Vector3 value = linkedListNode.Value;
			Vector3 value2 = linkedListNode.Next.Next.Value;
			Vector3 value3 = linkedListNode.Next.Value;
			Vector3 onNormal = value2 - value;
			Vector3 vector = value3 - value;
			Vector3 vector2 = Vector3.Project(vector, onNormal);
			if ((vector - vector2).magnitude / onNormal.magnitude < error)
			{
				points.Remove(linkedListNode.Next);
			}
			linkedListNode = linkedListNode.Next;
		}
	}

	public static Vector3 GetCurve(float t, Vector3 p0, Vector3 n0, Vector3 n1, Vector3 p1)
	{
		return _E004(t, p0, n0, n1, p1);
	}

	private static Vector3 _E004(float t, Vector3 p0, Vector3 m0, Vector3 m1, Vector3 p1)
	{
		float num = t * t;
		float num2 = num * t;
		float num3 = 3f * num;
		float num4 = 2f * num2;
		return (num4 - num3 + 1f) * p0 + (num2 - 2f * num + t) * m0 + (num3 - num4) * p1 + (num2 - num) * m1;
	}

	private static Vector3 _E005(float t, Vector3 p0, Vector3 n0, Vector3 n1, Vector3 p1)
	{
		float num = 1f - t;
		float num2 = num * num;
		float num3 = num2 * num;
		float num4 = t * t;
		float num5 = num4 * t;
		return num3 * p0 + num2 * t * n0 + num * num4 * n1 + num5 * p1;
	}
}
