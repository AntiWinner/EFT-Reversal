using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class TracerSystem : MonoBehaviour
{
	private class _E000
	{
		public int Pos4;

		public Vector3[] Vertices = new Vector3[8];

		public Vector2 Uv2Val;

		public Color32 Color;

		public void Calc(Vector3 start, Vector3 end, float size, float time, Color32 color)
		{
			Vector3 vector = (start - end).normalized * size;
			Vector3 vector2 = new Vector3(0f - vector.z, 0f, vector.x);
			Vector3 vector3 = new Vector3(vector.y * vector2.z, vector.z * vector2.x - vector.x * vector2.z, (0f - vector.y) * vector2.x) * 3f;
			Vertices[0] = start + vector2;
			Vertices[1] = start - vector2;
			Vertices[2] = end - vector2;
			Vertices[3] = end + vector2;
			Vertices[4] = start + vector3;
			Vertices[5] = start - vector3;
			Vertices[6] = end - vector3;
			Vertices[7] = end + vector3;
			Uv2Val.x = Time.time;
			Uv2Val.y = time;
			Color = color;
		}

		public void Update(Vector3[] vertices, Vector2[] uv2, Color32[] colors)
		{
			for (int i = 0; i < 8; i++)
			{
				int num = Pos4 + i;
				vertices[num] = Vertices[i];
				uv2[num] = Uv2Val;
				colors[num] = Color;
			}
		}
	}

	public int Count = 128;

	public float Size = 0.3f;

	private int m__E000;

	private LinkedList<_E000> _E001;

	private _E000[] _E002;

	private int _E003;

	private Vector3[] _E004;

	private Vector3[] _E005;

	private Vector4[] _E006;

	private Vector2[] _E007;

	private Vector2[] _E008;

	private Color32[] _E009;

	private int[] _E00A;

	private Mesh _E00B;

	private Vector3 _E00C = Vector3.zero;

	private Vector3 _E00D = Vector3.zero;

	private void Awake()
	{
		if (SystemInfo.graphicsShaderLevel < 20)
		{
			base.enabled = false;
			return;
		}
		this.m__E000 = Count << 1;
		_E001 = new LinkedList<_E000>();
		_E002 = new _E000[Count];
		for (int i = 0; i < Count; i++)
		{
			_E002[i] = new _E000();
			_E002[i].Pos4 = i << 3;
		}
		int num = this.m__E000 << 2;
		_E004 = new Vector3[num];
		_E005 = new Vector3[num];
		_E006 = new Vector4[num];
		_E007 = new Vector2[num];
		_E008 = new Vector2[num];
		_E009 = new Color32[num];
		_E00A = new int[this.m__E000 * 6];
		int j = 0;
		int num2 = 0;
		for (; j < this.m__E000; j++)
		{
			int num3 = j << 2;
			_E00A[num2++] = num3;
			_E00A[num2++] = num3 + 1;
			_E00A[num2++] = num3 + 2;
			_E00A[num2++] = num3 + 2;
			_E00A[num2++] = num3 + 3;
			_E00A[num2++] = num3;
		}
		int k = 0;
		int num4 = 0;
		for (; k < this.m__E000; k++)
		{
			_E007[num4++] = new Vector2(0f, 0f);
			_E007[num4++] = new Vector2(0f, 1f);
			_E007[num4++] = new Vector2(1f, 1f);
			_E007[num4++] = new Vector2(1f, 0f);
		}
		_E00B = new Mesh
		{
			vertices = _E004,
			normals = _E005,
			tangents = _E006,
			uv = _E007,
			uv2 = _E008,
			triangles = _E00A,
			name = _ED3E._E000(88632)
		};
		_E00B.MarkDynamic();
		_E00B.bounds = new Bounds(Vector3.zero, Vector3.zero);
		GetComponent<MeshFilter>().mesh = _E00B;
	}

	private void LateUpdate()
	{
		if (_E001.Count == 0)
		{
			return;
		}
		foreach (_E000 item in _E001)
		{
			item.Update(_E004, _E008, _E009);
		}
		_E001.Clear();
		_E00B.vertices = _E004;
		_E00B.uv2 = _E008;
		_E00B.colors32 = _E009;
	}

	public void Add(Vector3 start, Vector3 end, Color32 color)
	{
		if (_E002 != null)
		{
			_E000 obj = _E002[_E003];
			if (++_E003 >= Count)
			{
				_E003 = 0;
			}
			_E001.AddLast(obj);
			obj.Calc(start, end, Size, 1f, color);
			_E000(start);
			_E000(end);
		}
	}

	public void Add(Vector3 start, Vector3 end, Color32 color, float size, float time = 1f)
	{
		if (_E002 != null)
		{
			_E000 obj = _E002[_E003];
			if (++_E003 >= Count)
			{
				_E003 = 0;
			}
			_E001.AddLast(obj);
			obj.Calc(start, end, size, time, color);
			_E000(start);
			_E000(end);
		}
	}

	private void _E000(Vector3 position)
	{
		if (!(position.x > _E00C.x) || !(position.y > _E00C.y) || !(position.z > _E00C.z) || !(position.x < _E00D.x) || !(position.y < _E00D.y) || !(position.z < _E00D.z))
		{
			if (position.x < _E00C.x)
			{
				_E00C.x = position.x - 50f;
			}
			if (position.y < _E00C.y)
			{
				_E00C.y = position.y - 50f;
			}
			if (position.z < _E00C.z)
			{
				_E00C.z = position.z - 50f;
			}
			if (position.x > _E00D.x)
			{
				_E00D.x = position.x + 50f;
			}
			if (position.y > _E00D.y)
			{
				_E00D.y = position.y + 50f;
			}
			if (position.z > _E00D.z)
			{
				_E00D.z = position.z + 50f;
			}
			_E00B.bounds = new Bounds
			{
				min = _E00C,
				max = _E00D
			};
		}
	}
}
