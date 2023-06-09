using System.Collections.Generic;
using UnityEngine;

namespace Systems.Effects;

public class TreePlaneLod : MonoBehaviour
{
	private static LinkedList<TreePlaneLod> _E000 = new LinkedList<TreePlaneLod>();

	private static TreePlaneLod[] _E001;

	private static Vector3[] _E002;

	private static Vector3[] _E003;

	private static Vector4[] _E004;

	private static Vector2[] _E005;

	private static Vector2[] _E006;

	private static int[] _E007;

	private static Mesh _E008;

	private static Vector3 _E009 = Vector3.zero;

	private static Vector3 _E00A = Vector3.zero;

	private static bool _E00B = false;

	public float Height;

	public float Width;

	public Material Mat;

	private int _E00C;

	private void Awake()
	{
		_E000.AddLast(this);
	}

	private void Start()
	{
		if (_E00B)
		{
			return;
		}
		_E00B = true;
		int count = _E000.Count;
		_E001 = new TreePlaneLod[count];
		int num = 0;
		foreach (TreePlaneLod item in _E000)
		{
			_E001[num] = item;
			item._E00C = num++;
		}
		int num2 = count << 2;
		_E002 = new Vector3[num2];
		_E003 = new Vector3[num2];
		_E004 = new Vector4[num2];
		_E005 = new Vector2[num2];
		_E006 = new Vector2[num2];
		_E007 = new int[count * 6];
		int i = 0;
		int num3 = 0;
		for (; i < count; i++)
		{
			int num4 = i << 2;
			_E007[num3++] = num4;
			_E007[num3++] = num4 + 1;
			_E007[num3++] = num4 + 2;
			_E007[num3++] = num4 + 2;
			_E007[num3++] = num4 + 3;
			_E007[num3++] = num4;
		}
		int j = 0;
		int num5 = 0;
		for (; j < count; j++)
		{
			TreePlaneLod treePlaneLod = _E001[j];
			int num6 = num5++;
			int num7 = num5++;
			int num8 = num5++;
			int num9 = num5++;
			_E005[num6] = new Vector2(0f, 0f);
			_E005[num7] = new Vector2(0f, 1f);
			_E005[num8] = new Vector2(1f, 1f);
			_E005[num9] = new Vector2(1f, 0f);
			float y = (treePlaneLod.GetComponent<Renderer>().isVisible ? 0f : 1f);
			_E006[num6] = new Vector2(0f - treePlaneLod.Width, y);
			_E006[num7] = new Vector2(0f - treePlaneLod.Width, y);
			_E006[num8] = new Vector2(treePlaneLod.Width, y);
			_E006[num9] = new Vector2(treePlaneLod.Width, y);
			Vector3 position = treePlaneLod.transform.position;
			Vector3 vector = new Vector3(position.x, position.y + treePlaneLod.Height, position.z);
			_E002[num6] = position;
			_E002[num7] = vector;
			_E002[num8] = vector;
			_E002[num9] = position;
			CalcBounds(position);
		}
		_E008 = new Mesh
		{
			vertices = _E002,
			normals = _E003,
			tangents = _E004,
			uv = _E005,
			uv2 = _E006,
			triangles = _E007,
			name = _ED3E._E000(92350)
		};
		_E008.MarkDynamic();
		_E008.bounds = new Bounds
		{
			min = _E009 - new Vector3(50f, 50f, 50f),
			max = _E00A + new Vector3(50f, 50f, 50f)
		};
		GameObject obj = new GameObject(_ED3E._E000(92329))
		{
			hideFlags = HideFlags.HideAndDontSave
		};
		MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
		meshFilter.mesh = _E008;
		meshRenderer.sharedMaterial = Mat;
	}

	private static void CalcBounds(Vector3 position)
	{
		if (position.x < _E009.x)
		{
			_E009.x = position.x;
		}
		if (position.y < _E009.y)
		{
			_E009.y = position.y;
		}
		if (position.z < _E009.z)
		{
			_E009.z = position.z;
		}
		if (position.x > _E00A.x)
		{
			_E00A.x = position.x;
		}
		if (position.y > _E00A.y)
		{
			_E00A.y = position.y;
		}
		if (position.z > _E00A.z)
		{
			_E00A.z = position.z;
		}
	}

	private static void UpdateMesh()
	{
		_E008.uv2 = _E006;
	}

	private void OnBecameVisible()
	{
		int num = _E00C << 2;
		_E006[num++] = new Vector2(0f - Width, 0f);
		_E006[num++] = new Vector2(0f - Width, 0f);
		_E006[num++] = new Vector2(Width, 0f);
		_E006[num] = new Vector2(Width, 0f);
		UpdateMesh();
	}

	private void OnBecameInvisible()
	{
		int num = _E00C << 2;
		_E006[num++] = new Vector2(0f - Width, 1f);
		_E006[num++] = new Vector2(0f - Width, 1f);
		_E006[num++] = new Vector2(Width, 1f);
		_E006[num] = new Vector2(Width, 1f);
		UpdateMesh();
	}
}
