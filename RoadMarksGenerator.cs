using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class RoadMarksGenerator : MonoBehaviour
{
	public RoadSplineGenerator Parent;

	public int Road;

	public float Shift;

	public float Width = 0.1f;

	public float MarkLength = 0.4f;

	public float MarkDist = 0.2f;

	[Range(0f, 1f)]
	public float Start;

	[Range(0f, 1f)]
	public float End = 1f;

	public Rect UvRect = new Rect(0f, 0f, 1f, 1f);

	public float RoadOffset = 0.01f;

	public int LodParts;

	[Range(0f, 100f)]
	public float CullingRate;

	private MeshFilter m__E000;

	private float m__E001;

	public string Culled;

	public LayerMask Mask = -1;

	private float m__E002;

	private bool m__E003;

	private void Awake()
	{
		this.m__E002 = Start;
	}

	private void OnValidate()
	{
		if (Parent != null)
		{
			Road = Mathf.Clamp(Road, 0, Parent.Roads.Length);
		}
		LodParts = Math.Max(0, LodParts);
		this.m__E001 = 1f - CullingRate * 0.01f;
		this.m__E003 = !Mathf.Approximately(this.m__E002, Start);
		this.m__E002 = Start;
		Culled = this.m__E001.ToString(_ED3E._E000(45972));
		_E000();
	}

	private void _E000()
	{
		if (Parent == null || Parent.Roads == null || Parent.Roads.Length == 0)
		{
			return;
		}
		Parent.OnGenerate -= OnValidate;
		if (!this)
		{
			return;
		}
		Parent.OnGenerate += OnValidate;
		base.transform.position = Parent.transform.position + new Vector3(0f, RoadOffset, 0f);
		Vector3[] array = _E005(Parent.Roads[Road], Shift, MarkLength, MarkDist, Start, End);
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = FixOnGround(array[i]);
		}
		if (LodParts > 1)
		{
			_E001(array, LodParts, this.m__E001);
			GetComponent<Renderer>().enabled = false;
			return;
		}
		GetComponent<Renderer>().enabled = true;
		if (this.m__E000 == null)
		{
			this.m__E000 = GetComponent<MeshFilter>() ?? base.gameObject.AddComponent<MeshFilter>();
		}
		this.m__E000.sharedMesh = _E002(array, this.m__E000.sharedMesh);
	}

	private void _E001(Vector3[] marksPos, int parts, float cullingRate)
	{
		Vector3[][] array = _E003(marksPos, parts);
		MeshRenderer component = GetComponent<MeshRenderer>();
		for (int i = 0; i < parts; i++)
		{
			Mesh mesh = null;
			string n = _ED3E._E000(45975) + i + _ED3E._E000(11164);
			Transform transform = base.transform.Find(n);
			if (transform == null)
			{
				GameObject obj = new GameObject(n, typeof(MeshFilter), typeof(MeshRenderer), typeof(LODGroup))
				{
					isStatic = base.gameObject.isStatic
				};
				transform = obj.transform;
				transform.parent = base.transform;
				transform.localPosition = Vector3.zero;
				MeshRenderer component2 = obj.GetComponent<MeshRenderer>();
				component2.shadowCastingMode = component.shadowCastingMode;
				component2.receiveShadows = component.receiveShadows;
				component2.sharedMaterials = component.sharedMaterials;
				component2.lightProbeUsage = component.lightProbeUsage;
				component2.reflectionProbeUsage = component.reflectionProbeUsage;
				component2.probeAnchor = component.probeAnchor;
			}
			else
			{
				mesh = transform.GetComponent<MeshFilter>().sharedMesh;
			}
			transform.GetComponent<MeshFilter>().sharedMesh = _E002(array[i], mesh);
			transform.GetComponent<LODGroup>().SetLODs(new LOD[1]
			{
				new LOD(cullingRate, new Renderer[1] { transform.GetComponent<MeshRenderer>() })
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
			point.y += RoadOffset;
			point -= base.transform.position;
		}
		return point;
	}

	private Mesh _E002(Vector3[] marksPos, Mesh mesh = null)
	{
		int num = marksPos.Length * 2;
		Vector3[] array = new Vector3[num];
		Vector3[] normals = new Vector3[num];
		Vector4[] tangents = new Vector4[num];
		Vector2[] uv = new Vector2[num];
		Color[] colors = new Color[num];
		int[] array2 = new int[marksPos.Length * 3];
		Color[] array3 = null;
		if (mesh != null)
		{
			array3 = mesh.colors;
		}
		_E004(marksPos, array2, array, normals, tangents, uv, UvRect, Width, colors, array3, this.m__E003);
		if (mesh == null)
		{
			mesh = new Mesh
			{
				vertices = array,
				normals = normals,
				tangents = tangents,
				triangles = array2,
				uv = uv,
				name = _ED3E._E000(45964)
			};
		}
		else
		{
			mesh.Clear();
			mesh.vertices = array;
			mesh.normals = normals;
			mesh.tangents = tangents;
			mesh.triangles = array2;
			mesh.uv = uv;
			if (array3 != null)
			{
				mesh.colors = colors;
			}
		}
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		return mesh;
	}

	private Vector3[][] _E003(Vector3[] marksPos, int parts)
	{
		int num = marksPos.Length;
		Vector3[][] array = new Vector3[parts][];
		int num2 = (num >> 1) / parts << 1;
		for (int i = 0; i < parts; i++)
		{
			int num3 = i * num2;
			int num4 = ((i == parts - 1) ? (num - num3) : num2);
			array[i] = new Vector3[num4];
			Array.Copy(marksPos, num3, array[i], 0, num4);
		}
		return array;
	}

	private void _E004(Vector3[] marksPos, int[] tris, Vector3[] verts, Vector3[] normals, Vector4[] tangents, Vector2[] uv, Rect uvRect, float width, Color[] colors, Color[] oldColors, bool copyColorsInReverseOrder)
	{
		width *= 0.5f;
		Vector3 up = Vector3.up;
		Vector2 vector = new Vector2(uvRect.xMin, uvRect.yMin);
		Vector2 vector2 = new Vector2(uvRect.xMax, uvRect.yMin);
		Vector2 vector3 = new Vector2(uvRect.xMin, uvRect.yMax);
		Vector2 vector4 = new Vector2(uvRect.xMax, uvRect.yMax);
		int i = 0;
		int num = 0;
		int num2 = 0;
		for (; i < marksPos.Length; i += 2)
		{
			Vector3 vector5 = marksPos[i];
			Vector3 vector6 = marksPos[i + 1];
			Vector3 normalized = RoadSplineGenerator.CrossUp(vector6 - vector5).normalized;
			Vector3 vector7 = normalized * width;
			Vector4 vector8 = new Vector4(normalized.x, normalized.y, normalized.z, 1f);
			int num3 = num2++;
			int num4 = num2++;
			int num5 = num2++;
			int num6 = num2++;
			verts[num3] = vector5 - vector7;
			verts[num4] = vector5 + vector7;
			verts[num5] = vector6 - vector7;
			verts[num6] = vector6 + vector7;
			normals[num3] = up;
			normals[num4] = up;
			normals[num5] = up;
			normals[num6] = up;
			tangents[num3] = vector8;
			tangents[num4] = vector8;
			tangents[num5] = vector8;
			tangents[num6] = vector8;
			if (oldColors != null && oldColors.Length > num6 && !copyColorsInReverseOrder)
			{
				colors[num3] = oldColors[num3];
				colors[num4] = oldColors[num4];
				colors[num5] = oldColors[num5];
				colors[num6] = oldColors[num6];
			}
			uv[num3] = vector;
			uv[num4] = vector2;
			uv[num5] = vector3;
			uv[num6] = vector4;
			tris[num++] = num3;
			tris[num++] = num4;
			tris[num++] = num6;
			tris[num++] = num6;
			tris[num++] = num5;
			tris[num++] = num3;
		}
		if (oldColors != null && copyColorsInReverseOrder)
		{
			for (int j = 0; j < oldColors.Length && colors.Length - 1 - j >= 0; j++)
			{
				colors[colors.Length - 1 - j] = oldColors[oldColors.Length - 1 - j];
			}
		}
	}

	private static Vector3[] _E005(RoadSplineGenerator.Road road, float shift, float markLength, float markDist, float start, float end)
	{
		Vector3[] array = new Vector3[road.Optimized.Length];
		road.Optimized.CopyTo(array, 0);
		float[] array2 = new float[array.Length];
		GeneratePoints(array, array2, shift);
		float num = array2[array2.Length - 1];
		start = start * (num + markLength) - markLength;
		end *= num;
		start = Math.Max(0f - markLength, start);
		end = Math.Min(num, end);
		Vector3[] array3 = new Vector3[(int)((end - start + (markLength + markDist)) / (markLength + markDist)) << 1];
		bool flag = false;
		float num2 = start;
		int num3 = array.Length - 1;
		int num4 = 0;
		int num5 = 0;
		while (num4 < num3 && num5 < array3.Length)
		{
			if (num2 > array2[num4 + 1])
			{
				num4++;
				continue;
			}
			num2 = Math.Min(num2, end);
			float val = (num2 - array2[num4]) / (array2[num4 + 1] - array2[num4]);
			val = Math.Max(val, 0f);
			array3[num5++] = Lerp(array[num4], array[num4 + 1], val);
			flag = !flag;
			num2 += (flag ? markLength : markDist);
		}
		if (flag)
		{
			array3[array3.Length - 1] = array[array.Length - 1];
		}
		return array3;
	}

	public static Vector3 Lerp(Vector3 from, Vector3 to, float t)
	{
		return new Vector3(from.x + (to.x - from.x) * t, from.y + (to.y - from.y) * t, from.z + (to.z - from.z) * t);
	}

	public static void GeneratePoints(Vector3[] positions, float[] lengths, float shift)
	{
		int num = positions.Length;
		int num2 = num - 1;
		Vector3[] array = new Vector3[num];
		Vector3[] array2 = new Vector3[num];
		array[0] = positions[1] - positions[0];
		array[num2] = positions[num2] - positions[num2 - 1];
		for (int i = 1; i < num2; i++)
		{
			array[i] = positions[i + 1] - positions[i - 1];
		}
		for (int j = 0; j < num; j++)
		{
			array2[j] = RoadSplineGenerator.CrossUp(array[j]).normalized;
		}
		for (int k = 0; k < num; k++)
		{
			positions[k] += array2[k] * shift;
		}
		float num3 = 0f;
		lengths[0] = 0f;
		for (int l = 1; l < num; l++)
		{
			num3 = (lengths[l] = num3 + Vector3.Distance(positions[l - 1], positions[l]));
		}
	}
}
