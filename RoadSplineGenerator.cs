using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class RoadSplineGenerator : MonoBehaviour
{
	[Serializable]
	public class Road
	{
		public RoadSplineGenerator Parent;

		public Vector3[] Positins;

		public Vector3[] Normals;

		public int Parts;

		public Vector3[] Optimized;

		public float[] OptimizedVals;

		public float Width = 2f;

		public int WidthParts = 4;

		public float[] WidthVals = new float[5] { 0f, 0.25f, 0.5f, 0.75f, 1f };

		public bool UseWidthVals;

		public float TexDensity = 1f;

		public float TexXDensity = 1f;

		public Vector4 Borders;

		public int LodsCount = 5;

		[SerializeField]
		private bool _hold;

		public int LastVertId;

		public bool UseCurve;

		public AnimationCurve WidthCurve = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f));

		private int _verticesDelta;

		public bool Hold
		{
			get
			{
				return _hold;
			}
			set
			{
				if (_hold != value)
				{
					_hold = value;
					if (value)
					{
						Vector3[] points = Parent.GetPoints(this);
						float[] vals = Parent.GetVals(this);
						_E005(ref points, ref vals, Parent.Error, Parent.MaxOptimization);
						Optimized = points;
						OptimizedVals = vals;
					}
				}
			}
		}

		public void GeneratePoints(float error)
		{
			if (_hold)
			{
				Optimized = Parent.GetPointsHeld(this);
			}
			else
			{
				Optimized = _E003(Parent.GetPoints(this), error, Parent.MaxOptimization);
			}
		}

		public int GetTrianglesCount()
		{
			int num = Optimized.Length - 1;
			return WidthParts * 6 * num;
		}

		public int GetVerticesCount()
		{
			int num = Optimized.Length;
			return (WidthParts + 1) * num;
		}

		public void Generate(ref int vIndex, ref int tIndex, int[] triangles, Vector3[] positions, Vector4[] tangents, Vector2[] uv0, Color32[] colors, Color32[] oldColors, float error, bool lodGeneration)
		{
			_E000[] points = RoadSplineGenerator.GeneratePoints(Optimized, calcDistances: true);
			int num = vIndex;
			_E000(points, WidthVals, ref vIndex, ref tIndex, triangles, positions, tangents, uv0, colors);
			if (_hold && oldColors != null)
			{
				int num2 = vIndex - num;
				for (int i = 0; i < num2; i++)
				{
					int num3 = i + num;
					int num4 = i + LastVertId;
					colors[num3] = oldColors[num4];
				}
			}
			if (lodGeneration)
			{
				LastVertId += _verticesDelta;
				_verticesDelta = vIndex;
			}
			else
			{
				LastVertId = num;
			}
		}

		public float[] GetVals()
		{
			int num = WidthParts - 1;
			float[] array = new float[num];
			float num2 = 1f / (float)(num + 1);
			float num3 = num2;
			for (int i = 0; i < num; i++)
			{
				array[i] = num3;
				num3 += num2;
			}
			return array;
		}

		public void ResetVerticesDelta()
		{
			_verticesDelta = 0;
		}

		private void _E000(_E000[] points, float[] vals, ref int vIndex, ref int tIndex, int[] triangles, Vector3[] positions, Vector4[] tangents, Vector2[] uv0, Color32[] colors)
		{
			float num = Width * 0.5f;
			int num2 = WidthParts + 1;
			int num3 = vIndex;
			int num4 = tIndex;
			Vector2 zero = Vector2.zero;
			float num5 = TexDensity / Width;
			float texXDensity = TexXDensity;
			for (int i = 0; i < points.Length; i++)
			{
				_E000 obj = points[i];
				Vector3 vector;
				Vector3 vector2;
				float num8;
				float num9;
				if (UseCurve)
				{
					float num6 = WidthCurve.Evaluate(zero.y);
					float num7 = Width * num6;
					vector = obj.Right * num7;
					vector2 = obj.Position + obj.Right * num7 * 0.5f;
					num8 = (1f + num6) * 0.5f * texXDensity;
					num9 = 0f - num6;
				}
				else
				{
					vector = obj.Right * Width;
					vector2 = obj.Position + obj.Right * num;
					num8 = 1f * texXDensity;
					num9 = -1f;
				}
				Vector4 vector3 = obj.Right;
				vector3.w = 1f;
				zero.x = num8;
				tangents[num3] = vector3;
				positions[num3] = Parent.FixOnGround(vector2);
				uv0[num3] = zero;
				for (int j = 1; j < WidthParts; j++)
				{
					tangents[num3 + j] = vector3;
					positions[num3 + j] = Parent.FixOnGround(vector2 - vector * vals[j - 1]);
					zero.x = num8 + vals[j - 1] * num9 * texXDensity;
					uv0[num3 + j] = zero;
				}
				tangents[num3 + WidthParts] = vector3;
				positions[num3 + WidthParts] = Parent.FixOnGround(vector2 - vector);
				zero.x = num8 + num9 * texXDensity;
				uv0[num3 + WidthParts] = zero;
				zero.y += num5 * obj.NextDist;
				if (i != 0)
				{
					for (int k = 0; k < WidthParts; k++)
					{
						int num10 = num3 + k + 1;
						int num11 = num10 - num2;
						int num12 = num3 + k;
						int num13 = num12 - num2;
						int num14 = num4 + k * 6;
						triangles[num14] = num10;
						triangles[num14 + 1] = num11;
						triangles[num14 + 2] = num13;
						triangles[num14 + 3] = num13;
						triangles[num14 + 4] = num12;
						triangles[num14 + 5] = num10;
					}
					num4 += WidthParts * 6;
				}
				num3 += num2;
			}
			int num15 = points.Length * num2;
			float y = uv0[vIndex + num15 - 1].y;
			float num16 = y - Borders.y;
			float num17 = 1f - Borders.w;
			for (int l = 0; l < num15; l++)
			{
				Vector2 vector4 = uv0[l + vIndex];
				float num18 = 1f;
				if (Borders.x != 0f && vector4.y < Borders.x)
				{
					num18 -= 1f - vector4.y / Borders.x;
				}
				else if (Borders.y != 0f && vector4.y > num16)
				{
					num18 -= 1f - (y - vector4.y) / Borders.y;
				}
				if (Borders.z != 0f && vector4.x < Borders.z)
				{
					num18 -= 1f - vector4.x / Borders.z;
				}
				else if (Borders.w != 0f && vector4.x > num17)
				{
					num18 -= 1f - (1f - vector4.x) / Borders.w;
				}
				byte a = (byte)(Mathf.Clamp01(num18) * 255f);
				colors[l + vIndex] = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, a);
			}
			vIndex = num3;
			tIndex = num4;
		}
	}

	public struct _E000
	{
		public Vector3 Position;

		public Vector3 Right;

		public float NextDist;
	}

	[CompilerGenerated]
	private Action m__E000;

	public Road[] Roads;

	public float Error = 0.05f;

	public int MaxOptimization = 16;

	public float TerrainOffset = 0.01f;

	public LayerMask Mask = -1;

	public bool SplitForLods;

	public float CullingRate;

	public bool LockGeneration;

	private MeshFilter m__E001;

	public Mesh Mesh;

	public event Action OnGenerate
	{
		[CompilerGenerated]
		add
		{
			Action action = this.m__E000;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = this.m__E000;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public Mesh GetMesh()
	{
		if (Mesh != null)
		{
			return Mesh;
		}
		Mesh = GetComponent<MeshFilter>().sharedMesh;
		return Mesh;
	}

	public void Generate()
	{
		if (LockGeneration || Roads == null || Roads.Length == 0)
		{
			return;
		}
		GetComponent<MeshRenderer>().enabled = !SplitForLods;
		if (SplitForLods)
		{
			Color32[] oldColors = ((base.transform.childCount > 0 || Mesh == null) ? null : Mesh.colors32);
			for (int i = 0; i < Roads.Length; i++)
			{
				GenerateLods(i, oldColors);
			}
			SetCullingRate();
			if (this.m__E000 != null)
			{
				this.m__E000();
			}
			return;
		}
		if (this.m__E001 == null)
		{
			this.m__E001 = GetComponent<MeshFilter>() ?? base.gameObject.AddComponent<MeshFilter>();
		}
		Mesh = this.m__E001.sharedMesh;
		int num = 0;
		int num2 = 0;
		Road[] roads = Roads;
		foreach (Road road in roads)
		{
			road.Parent = this;
			road.GeneratePoints(Error);
			num += road.GetTrianglesCount();
			num2 += road.GetVerticesCount();
		}
		int[] triangles = new int[num];
		Vector3[] array = new Vector3[num2];
		Vector3[] array2 = new Vector3[num2];
		Vector4[] tangents = new Vector4[num2];
		Vector2[] array3 = new Vector2[num2];
		Color32[] array4 = new Color32[num2];
		Color32[] oldColors2 = ((Mesh == null) ? null : Mesh.colors32);
		if (base.transform.childCount > 0)
		{
			oldColors2 = GetColors();
			_E008();
		}
		int tIndex = 0;
		int vIndex = 0;
		roads = Roads;
		for (int j = 0; j < roads.Length; j++)
		{
			roads[j].Generate(ref vIndex, ref tIndex, triangles, array, tangents, array3, array4, oldColors2, Error, lodGeneration: false);
		}
		for (int k = 0; k < num2; k++)
		{
			array2[k] = Vector3.up;
		}
		if (Mesh == null)
		{
			MeshFilter meshFilter = this.m__E001;
			Mesh obj = new Mesh
			{
				vertices = array,
				normals = array2,
				tangents = tangents,
				triangles = triangles,
				uv = array3,
				colors32 = array4,
				name = _ED3E._E000(46038)
			};
			Mesh mesh = obj;
			Mesh = obj;
			meshFilter.mesh = mesh;
		}
		else
		{
			Mesh.Clear();
			Mesh.vertices = array;
			Mesh.normals = array2;
			Mesh.tangents = tangents;
			Mesh.triangles = triangles;
			Mesh.uv = array3;
			Mesh.colors32 = array4;
		}
		Mesh.RecalculateBounds();
		Mesh.RecalculateNormals();
		if (this.m__E000 != null)
		{
			this.m__E000();
		}
	}

	private static void _E000(Vector2[] uv, int widthVerts, int count, out int[] starts, out int[] lengths)
	{
		float num = uv[uv.Length - 1].y / (float)count;
		starts = new int[count];
		lengths = new int[count];
		float num2 = 0f;
		int i = 0;
		int num3 = 0;
		for (; i < uv.Length; i++)
		{
			int num4 = i * widthVerts;
			if (num4 >= uv.Length)
			{
				break;
			}
			if (!(uv[num4].y < num2))
			{
				starts[num3++] = i;
				num2 += num;
				if (num3 == count)
				{
					break;
				}
			}
		}
		for (int j = 0; j < count; j++)
		{
			if (j == count - 1)
			{
				lengths[j] = uv.Length / widthVerts - starts[j] - 1;
			}
			else
			{
				lengths[j] = starts[j + 1] - starts[j];
			}
		}
	}

	public void GenerateLods(int roadI, Color32[] oldColors)
	{
		if (Roads == null || Roads.Length == 0)
		{
			return;
		}
		Road road = Roads[roadI];
		MeshRenderer component = GetComponent<MeshRenderer>();
		road.Parent = this;
		road.GeneratePoints(Error);
		int trianglesCount = road.GetTrianglesCount();
		int verticesCount = road.GetVerticesCount();
		int[] array = new int[trianglesCount];
		Vector3[] array2 = new Vector3[verticesCount];
		Vector3[] array3 = new Vector3[verticesCount];
		Vector4[] array4 = new Vector4[verticesCount];
		Vector2[] array5 = new Vector2[verticesCount];
		Color32[] array6 = new Color32[verticesCount];
		if (oldColors == null)
		{
			oldColors = (road.Hold ? _E001(roadI) : null);
		}
		else if (!road.Hold)
		{
			oldColors = null;
		}
		int tIndex = 0;
		int vIndex = 0;
		road.Generate(ref vIndex, ref tIndex, array, array2, array4, array5, array6, oldColors, Error, lodGeneration: true);
		road.ResetVerticesDelta();
		for (int i = 0; i < verticesCount; i++)
		{
			array3[i] = Vector3.up;
		}
		int num = road.WidthParts + 1;
		_E000(array5, num, road.LodsCount, out var starts, out var lengths);
		for (int j = 0; j < road.LodsCount; j++)
		{
			int num2 = starts[j];
			int num3 = lengths[j];
			int num4 = num2 * road.WidthParts * 6;
			int num5 = num3 * road.WidthParts * 6;
			int num6 = num2 * num;
			int num7 = (num3 + 1) * num;
			int[] array7 = new int[num5];
			Vector3[] array8 = new Vector3[num7];
			Vector3[] array9 = new Vector3[num7];
			Vector4[] array10 = new Vector4[num7];
			Vector2[] array11 = new Vector2[num7];
			Color32[] array12 = new Color32[num7];
			for (int k = 0; k < num7; k++)
			{
				int num8 = num6 + k;
				array8[k] = array2[num8];
				array9[k] = array3[num8];
				array10[k] = array4[num8];
				array11[k] = array5[num8];
				array12[k] = array6[num8];
			}
			for (int l = 0; l < num5; l++)
			{
				int num9 = num4 + l;
				array7[l] = array[num9] - num6;
			}
			Mesh sharedMesh = new Mesh
			{
				vertices = array8,
				normals = array9,
				tangents = array10,
				triangles = array7,
				uv = array11,
				colors32 = array12,
				name = _ED3E._E000(46075)
			};
			string n = _ED3E._E000(48148) + roadI + _ED3E._E000(48146) + j + _ED3E._E000(11164);
			if (base.transform.Find(n) == null)
			{
				GameObject obj = new GameObject(n, typeof(MeshFilter), typeof(MeshRenderer), typeof(LODGroup))
				{
					isStatic = base.gameObject.isStatic
				};
				Transform obj2 = obj.transform;
				obj2.parent = base.transform;
				obj2.localPosition = Vector3.zero;
				obj2.gameObject.layer = component.gameObject.layer;
				obj2.GetComponent<MeshFilter>().sharedMesh = sharedMesh;
				MeshRenderer component2 = obj.GetComponent<MeshRenderer>();
				component2.shadowCastingMode = component.shadowCastingMode;
				component2.receiveShadows = component.receiveShadows;
				if (_E009(component2, component.sharedMaterial))
				{
					string materialName = component.gameObject.name + component.sharedMaterial.name + roadI;
					component2.sharedMaterial = _E3E6.Get(materialName, component);
					component2.sharedMaterial.renderQueue = component.sharedMaterial.renderQueue - 1;
					component2.sharedMaterial.name = materialName;
				}
				else
				{
					component2.sharedMaterial = component.sharedMaterial;
				}
				component2.lightProbeUsage = component.lightProbeUsage;
				component2.reflectionProbeUsage = component.reflectionProbeUsage;
				component2.probeAnchor = component.probeAnchor;
				component2.enabled = true;
				_E3E6.Add(component2);
			}
		}
	}

	private Color32[] _E001(int roadI)
	{
		Color32[] array = new Color32[Roads[roadI].GetVerticesCount()];
		int i = 0;
		_E002(roadI, array, ref i);
		return array;
	}

	private void _E002(int roadI, Color32[] colors, ref int i)
	{
		Road road = Roads[roadI];
		int num = road.WidthParts + 1;
		for (int j = 0; j < road.LodsCount; j++)
		{
			string n = _ED3E._E000(48148) + roadI + _ED3E._E000(48146) + j + _ED3E._E000(11164);
			Transform transform = base.transform.Find(n);
			if (transform == null)
			{
				break;
			}
			Color32[] colors2 = transform.GetComponent<MeshFilter>().sharedMesh.colors32;
			int num2 = ((j == road.LodsCount - 1) ? colors2.Length : (colors2.Length - num));
			for (int k = 0; k < num2; k++)
			{
				colors[i++] = colors2[k];
			}
		}
	}

	public Color32[] GetColors()
	{
		int num = 0;
		Road[] roads = Roads;
		foreach (Road road in roads)
		{
			num += road.GetVerticesCount();
		}
		Color32[] array = new Color32[num];
		int i2 = 0;
		for (int j = 0; j < Roads.Length; j++)
		{
			_E002(j, array, ref i2);
		}
		return array;
	}

	public void SetCullingRate()
	{
		LODGroup[] componentsInChildren = base.gameObject.GetComponentsInChildren<LODGroup>();
		foreach (LODGroup lODGroup in componentsInChildren)
		{
			lODGroup.SetLODs(new LOD[1]
			{
				new LOD(CullingRate, new Renderer[1] { lODGroup.GetComponent<MeshRenderer>() })
			});
		}
	}

	public Vector3 FixOnGround(Vector3 point)
	{
		Vector3 origin = point;
		origin.y += 100f;
		origin += base.transform.position;
		if (Physics.Raycast(origin, Vector3.down, out var hitInfo, 200f, Mask))
		{
			point = hitInfo.point;
			point.y += TerrainOffset;
			point -= base.transform.position;
		}
		return point;
	}

	public Vector3[] GetPoints(Road road)
	{
		int num = road.Parts + 1;
		Vector3[] array = new Vector3[num * (road.Positins.Length - 1) - (road.Positins.Length - 2)];
		float num2 = 1f / (float)road.Parts;
		Vector3[] positins = road.Positins;
		Vector3[] normals = road.Normals;
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
		for (int j = 0; j < array.Length; j++)
		{
			array[j] = FixOnGround(array[j]);
		}
		return array;
	}

	public Vector3[] GetPointsHeld(Road road)
	{
		Vector3[] positins = road.Positins;
		Vector3[] normals = road.Normals;
		float[] optimizedVals = road.OptimizedVals;
		int num = optimizedVals.Length;
		Vector3[] array = new Vector3[num];
		for (int i = 0; i < num; i++)
		{
			int num2 = (int)optimizedVals[i];
			float t = optimizedVals[i] - (float)num2;
			Vector3 p = positins[num2];
			Vector3 p2 = positins[num2 + 1];
			Vector3 n = normals[num2];
			Vector3 n2 = normals[num2 + 1];
			array[i] = GetCurve(t, p, n, n2, p2);
		}
		for (int j = 0; j < array.Length; j++)
		{
			array[j] = FixOnGround(array[j]);
		}
		return array;
	}

	public float[] GetVals(Road road)
	{
		int num = road.Parts + 1;
		float[] array = new float[num * (road.Positins.Length - 1) - (road.Positins.Length - 2)];
		float num2 = 1f / (float)road.Parts;
		int num3 = road.Positins.Length;
		int i = 1;
		int num4 = 0;
		for (; i < num3; i++)
		{
			float num5 = 0f;
			int num6 = 0;
			while (num6 < num)
			{
				array[num4] = (float)(i - 1) + num5;
				num5 += num2;
				num6++;
				num4++;
			}
			num4--;
		}
		array[array.Length - 1] -= 0.0001f;
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

	private static Vector3[] _E003(Vector3[] points, float error, int maxOptimization)
	{
		LinkedList<Vector3> linkedList = new LinkedList<Vector3>(points);
		int num = 0;
		int num2 = int.MaxValue;
		while (linkedList.Count < num2 && num++ < maxOptimization)
		{
			num2 = linkedList.Count;
			_E004(linkedList, error);
			error *= 0.5f;
		}
		points = new Vector3[linkedList.Count];
		linkedList.CopyTo(points, 0);
		return points;
	}

	private static void _E004(LinkedList<Vector3> points, float error)
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

	private static void _E005(ref Vector3[] points, ref float[] vals, float error, int maxOptimization)
	{
		LinkedList<int> linkedList = new LinkedList<int>();
		for (int i = 0; i < points.Length; i++)
		{
			linkedList.AddLast(i);
		}
		int num = int.MaxValue;
		int num2 = 0;
		while (linkedList.Count < num && num2++ < maxOptimization)
		{
			num = linkedList.Count;
			_E006(points, linkedList, error);
			error *= 0.5f;
		}
		Vector3[] array = new Vector3[linkedList.Count];
		float[] array2 = new float[linkedList.Count];
		LinkedListNode<int> linkedListNode = linkedList.First;
		int num3 = 0;
		while (num3 < array.Length)
		{
			int value = linkedListNode.Value;
			array[num3] = points[value];
			array2[num3] = vals[value];
			num3++;
			linkedListNode = linkedListNode.Next;
		}
		points = array;
		vals = array2;
	}

	private static void _E006(Vector3[] points, LinkedList<int> indices, float error)
	{
		LinkedListNode<int> linkedListNode = indices.First;
		while (linkedListNode.Next != null && linkedListNode.Next.Next != null)
		{
			Vector3 vector = points[linkedListNode.Value];
			Vector3 vector2 = points[linkedListNode.Next.Next.Value];
			Vector3 vector3 = points[linkedListNode.Next.Value];
			Vector3 onNormal = vector2 - vector;
			Vector3 vector4 = vector3 - vector;
			Vector3 vector5 = Vector3.Project(vector4, onNormal);
			if ((vector4 - vector5).magnitude / onNormal.magnitude < error)
			{
				indices.Remove(linkedListNode.Next);
			}
			linkedListNode = linkedListNode.Next;
		}
	}

	public static Vector3 GetCurve(float t, Vector3 p0, Vector3 n0, Vector3 n1, Vector3 p1)
	{
		return _E007(t, p0, n0, n1, p1);
	}

	private static Vector3 _E007(float t, Vector3 p0, Vector3 m0, Vector3 m1, Vector3 p1)
	{
		float num = t * t;
		float num2 = num * t;
		float num3 = 3f * num;
		float num4 = 2f * num2;
		return (num4 - num3 + 1f) * p0 + (num2 - 2f * num + t) * m0 + (num3 - num4) * p1 + (num2 - num) * m1;
	}

	public static _E000[] GeneratePoints(Vector3[] positions, bool calcDistances = true)
	{
		int num = positions.Length;
		int num2 = num - 1;
		_E000[] array = new _E000[num];
		Vector3[] array2 = new Vector3[num];
		Vector3[] array3 = new Vector3[num];
		array2[0] = (positions[1] - positions[0]).normalized;
		array2[num2] = (positions[num2] - positions[num2 - 1]).normalized;
		for (int i = 1; i < num2; i++)
		{
			array2[i] = (positions[i + 1] - positions[i - 1]).normalized;
		}
		for (int j = 0; j < num; j++)
		{
			array3[j] = CrossUp(array2[j]).normalized;
		}
		for (int k = 0; k < num; k++)
		{
			array[k] = new _E000
			{
				Position = positions[k],
				Right = array3[k]
			};
		}
		float num3 = num - 1;
		if (calcDistances)
		{
			for (int l = 0; (float)l < num3; l++)
			{
				array[l].NextDist = Vector3.Distance(positions[l], positions[l + 1]);
			}
		}
		return array;
	}

	public static Vector3 CrossUp(Vector3 lhs)
	{
		return new Vector3(0f - lhs.z, 0f, lhs.x);
	}

	public static void DropChilds(Transform transform)
	{
		if (transform.childCount == 0)
		{
			return;
		}
		LinkedList<GameObject> linkedList = new LinkedList<GameObject>();
		foreach (Transform item in transform)
		{
			linkedList.AddLast(item.gameObject);
		}
		foreach (GameObject item2 in linkedList)
		{
			UnityEngine.Object.DestroyImmediate(item2);
		}
	}

	private void _E008()
	{
		if (base.transform.childCount == 0)
		{
			return;
		}
		LinkedList<GameObject> linkedList = new LinkedList<GameObject>();
		foreach (Transform item in base.transform)
		{
			linkedList.AddLast(item.gameObject);
		}
		foreach (GameObject item2 in linkedList)
		{
			MeshRenderer component = item2.GetComponent<MeshRenderer>();
			if (component.sharedMaterial != null)
			{
				_E3E6.Remove(component.sharedMaterial.name, component);
			}
			UnityEngine.Object.DestroyImmediate(item2);
		}
	}

	private bool _E009(MeshRenderer lodMeshRenderer, Material lodMeshMaterial)
	{
		MeshCollider meshCollider = lodMeshRenderer.gameObject.AddComponent<MeshCollider>();
		foreach (Renderer lodRoad in _E3E6.LodRoads)
		{
			if (!(lodRoad == null))
			{
				MeshCollider meshCollider2 = lodRoad.gameObject.AddComponent<MeshCollider>();
				bool flag = meshCollider2.bounds.Intersects(meshCollider.bounds);
				UnityEngine.Object.DestroyImmediate(meshCollider2);
				bool flag2 = lodMeshRenderer.gameObject.transform.parent == lodRoad.gameObject.transform.parent;
				bool flag3 = lodMeshMaterial.renderQueue == lodRoad.sharedMaterial.renderQueue;
				if (flag && !flag2 && flag3)
				{
					UnityEngine.Object.DestroyImmediate(meshCollider);
					return true;
				}
			}
		}
		UnityEngine.Object.DestroyImmediate(meshCollider);
		return false;
	}
}
