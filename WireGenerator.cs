using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class WireGenerator : MonoBehaviour
{
	[Serializable]
	public class Wire
	{
		public Vector3 A;

		public Vector3 B;

		public float Gravity;

		public int Parts;

		public int GetTrianglesCount(_E000 shape)
		{
			return shape.TrisA.Length * 6 * Parts;
		}

		public int GetVerticesCount(_E000 shape)
		{
			return shape.Vertices.Length * (Parts + 1);
		}

		public void Generate(_E000 shape, int vIndex, int tIndex, int[] triangles, Vector3[] positions, Vector2[] uv0, Vector2[] uv1)
		{
			Generate(shape, GenerateVecs(), vIndex, tIndex, triangles, positions, uv0);
			GenerateWindParams(vIndex, shape.Vertices.Length, uv1);
		}

		public Vector3[] GetPoints()
		{
			int parts = Parts;
			int num = parts + 1;
			Vector3[] array = new Vector3[num];
			Vector3 a = A;
			Vector3 vector = B - a;
			float num2 = 1f / (float)parts;
			Vector3 vector2 = vector * num2;
			Vector3 vector3 = a;
			for (int i = 0; i < num; i++)
			{
				array[i] = vector3;
				vector3 += vector2;
			}
			float num3 = 0f;
			for (int j = 0; j < num; j++)
			{
				float num4 = Mathf.Abs(0.5f - num3) * 2f;
				array[j].y -= (1f - num4 * num4) * Gravity;
				num3 += num2;
			}
			return array;
		}

		public void GenerateWindParams(int vIndex, int shapeVCount, Vector2[] uv1)
		{
			int parts = Parts;
			int num = parts + 1;
			float num2 = 1f / (float)parts;
			float y = 1f / Gravity;
			float num3 = 0f;
			int num4 = 0;
			while (num4 < num * shapeVCount)
			{
				float num5 = Mathf.Abs(0.5f - num3) * 2f;
				Vector2 vector = new Vector2((1f - num5 * num5) * Gravity, y);
				for (int i = 0; i < shapeVCount; i++)
				{
					int num6 = vIndex + num4++;
					uv1[num6] = vector;
				}
				num3 += num2;
			}
		}

		public _E001[] GenerateVecs()
		{
			int parts = Parts;
			int num = parts + 1;
			_E001[] array = new _E001[num];
			Vector3[] array2 = new Vector3[num];
			Vector3[] array3 = new Vector3[num];
			Vector3[] array4 = new Vector3[num];
			Vector3 a = A;
			Vector3 vector = B - a;
			float num2 = 1f / (float)parts;
			Vector3 vector2 = vector * num2;
			Vector3 normalized = new Vector3(vector.z, 0f, 0f - vector.x).normalized;
			Vector3 vector3 = a;
			for (int i = 0; i < num; i++)
			{
				array2[i] = vector3;
				vector3 += vector2;
			}
			float num3 = 0f;
			for (int j = 0; j < num; j++)
			{
				float num4 = Mathf.Abs(0.5f - num3) * 2f;
				array2[j].y -= (1f - num4 * num4) * Gravity;
				num3 += num2;
			}
			array3[0] = (array2[1] - array2[0]).normalized;
			array3[parts] = (array2[parts] - array2[parts - 1]).normalized;
			for (int k = 1; k < parts; k++)
			{
				array3[k] = (array2[k + 1] - array2[k - 1]).normalized;
			}
			for (int l = 0; l < num; l++)
			{
				array4[l] = Vector3.Cross(array3[l], normalized).normalized;
			}
			for (int m = 0; m < num; m++)
			{
				array[m] = new _E001
				{
					Position = array2[m],
					Right = normalized,
					Upward = array4[m]
				};
			}
			return array;
		}

		public static void Generate(_E000 shape, _E001[] points, int vIndex, int tIndex, int[] triangles, Vector3[] positions, Vector2[] uv0)
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
				num += wire.GetTrianglesCount(shape);
				num2 += wire.GetVerticesCount(shape);
			}
			int[] triangles = new int[num];
			Vector3[] array = new Vector3[num2];
			Vector2[] array2 = new Vector2[num2];
			Vector2[] array3 = new Vector2[num2];
			int num3 = 0;
			int num4 = 0;
			wires = Wires;
			foreach (Wire wire2 in wires)
			{
				wire2.Generate(shape, num4, num3, triangles, array, array2, array3);
				num3 += wire2.GetTrianglesCount(shape);
				num4 += wire2.GetVerticesCount(shape);
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
					uv2 = array3,
					name = _ED3E._E000(96650)
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
				Mesh.uv2 = array3;
			}
			Mesh.RecalculateBounds();
			Mesh.RecalculateNormals();
		}
	}
}
