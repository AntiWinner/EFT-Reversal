using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class DecalSystem : MonoBehaviour
{
	private class _E000
	{
		public int Pos4;

		public Vector3[] Vertices = new Vector3[4];

		public Vector3 Normal;

		public Vector4 Tangent;

		public Vector2[] Uv;

		public void FastCalcWithoutRotation(Vector3 position, Vector3 normal, float radius)
		{
			Vector3 vector;
			Vector3 vector2;
			float num;
			if (normal.x < 0.95f && normal.x > -0.95f)
			{
				vector = new Vector3(0f, normal.z, 0f - normal.y);
				Debug.DrawRay(position, normal, Color.green, 5f);
				Debug.DrawRay(position, vector, Color.red, 5f);
				num = radius / Mathf.Sqrt(vector.y * vector.y + vector.z * vector.z);
				vector2 = new Vector3(normal.y * (0f - normal.y) - normal.z * normal.z, (0f - normal.x) * (0f - normal.y), normal.x * normal.z);
			}
			else
			{
				vector = new Vector3(0f - normal.z, 0f, normal.x);
				num = radius / Mathf.Sqrt(vector.x * vector.x + vector.z * vector.z);
				vector2 = new Vector3(normal.y * vector.z, normal.z * vector.x - normal.x * vector.z, (0f - normal.y) * vector.x);
			}
			vector *= num;
			vector2 *= num;
			Vertices[0] = position - vector;
			Vertices[1] = position - vector2;
			Vertices[2] = position + vector;
			Vertices[3] = position + vector2;
			num = 0.7071f / radius;
			Tangent = new Vector4((Vertices[3].x - Vertices[0].x) * num, (Vertices[3].y - Vertices[0].y) * num, (Vertices[3].z - Vertices[0].z) * num, -1f);
			Normal = normal;
		}

		public void Calc(Vector3 position, Vector3 normal, float radius)
		{
			Vector3 vector = Vector3.Cross(normal, _E8EE.VectorNormalized());
			vector *= radius / vector.magnitude;
			Vector3 vector2 = Vector3.Cross(normal, vector);
			Vertices[0] = position - vector;
			Vertices[1] = position - vector2;
			Vertices[2] = position + vector;
			Vertices[3] = position + vector2;
			float num = 0.7071f / radius;
			Tangent = new Vector4((Vertices[3].x - Vertices[0].x) * num, (Vertices[3].y - Vertices[0].y) * num, (Vertices[3].z - Vertices[0].z) * num, -1f);
			Normal = normal;
		}

		public void Update(Vector3[] vertices, Vector3[] normals, Vector4[] tangents)
		{
			for (int i = 0; i < 4; i++)
			{
				int num = Pos4 + i;
				vertices[num] = Vertices[i];
				normals[num] = Normal;
				tangents[num] = Tangent;
			}
		}

		public void Update(Vector3[] vertices, Vector3[] normals, Vector4[] tangents, Vector2[] uv)
		{
			for (int i = 0; i < 4; i++)
			{
				int num = Pos4 + i;
				vertices[num] = Vertices[i];
				normals[num] = Normal;
				tangents[num] = Tangent;
				uv[num] = Uv[i];
			}
		}
	}

	public int Count = 2048;

	public float SizeMin = 1f;

	public float SizeRandomRange;

	private bool m__E000;

	public int TileSheetRows = 1;

	public int TileSheetColumns = 1;

	private Vector2[][] _E001;

	private int _E002 = 1;

	private bool _E003;

	private LinkedList<_E000> _E004;

	private int _E005;

	private _E000[] _E006;

	private Vector3[] _E007;

	private Vector3[] _E008;

	private Vector4[] _E009;

	private Vector2[] _E00A;

	private Vector2[] _E00B;

	private int[] _E00C;

	private Mesh _E00D;

	private Vector3 _E00E = Vector3.zero;

	private Vector3 _E00F = Vector3.zero;

	private void Awake()
	{
		this.m__E000 = SizeRandomRange >= float.Epsilon;
		_E004 = new LinkedList<_E000>();
		_E006 = new _E000[Count];
		for (int i = 0; i < Count; i++)
		{
			_E006[i] = new _E000();
			_E006[i].Pos4 = i << 2;
		}
		int num = Count << 2;
		_E007 = new Vector3[num];
		_E008 = new Vector3[num];
		_E009 = new Vector4[num];
		_E00A = new Vector2[num];
		_E00B = new Vector2[num];
		_E00C = new int[Count * 6];
		int j = 0;
		int num2 = 0;
		for (; j < Count; j++)
		{
			int num3 = j << 2;
			_E00C[num2++] = num3;
			_E00C[num2++] = num3 + 1;
			_E00C[num2++] = num3 + 2;
			_E00C[num2++] = num3 + 2;
			_E00C[num2++] = num3 + 3;
			_E00C[num2++] = num3;
		}
		int k = 0;
		int num4 = 0;
		for (; k < Count; k++)
		{
			_E00A[num4++] = new Vector2(0f, 0f);
			_E00A[num4++] = new Vector2(0f, 1f);
			_E00A[num4++] = new Vector2(1f, 1f);
			_E00A[num4++] = new Vector2(1f, 0f);
		}
		_E002 = TileSheetRows * TileSheetColumns;
		_E003 = _E002 > 1;
		if (_E003)
		{
			_E001 = new Vector2[_E002][];
			float num5 = 1f / (float)TileSheetRows;
			float num6 = 1f / (float)TileSheetColumns;
			int l = 0;
			int num7 = 0;
			int num8 = 0;
			for (; l < _E002; l++)
			{
				float x = num5 * (float)num7;
				float x2 = num5 * (float)(num7 + 1);
				float y = num6 * (float)num8;
				float y2 = num6 * (float)(num8 + 1);
				_E001[l] = new Vector2[4];
				_E001[l][0] = new Vector2(x, y);
				_E001[l][1] = new Vector2(x, y2);
				_E001[l][2] = new Vector2(x2, y2);
				_E001[l][3] = new Vector2(x2, y);
				num7++;
				if (num7 >= TileSheetRows)
				{
					num8++;
					num7 = 0;
				}
			}
		}
		_E00D = new Mesh
		{
			vertices = _E007,
			normals = _E008,
			tangents = _E009,
			uv = _E00A,
			uv2 = _E00B,
			triangles = _E00C,
			name = _ED3E._E000(88507)
		};
		_E00D.MarkDynamic();
		_E00D.bounds = new Bounds(Vector3.zero, Vector3.zero);
		GetComponent<MeshFilter>().mesh = _E00D;
	}

	private void LateUpdate()
	{
		if (_E004.Count == 0)
		{
			return;
		}
		if (_E003)
		{
			foreach (_E000 item in _E004)
			{
				item.Update(_E007, _E008, _E009, _E00A);
			}
		}
		else
		{
			foreach (_E000 item2 in _E004)
			{
				item2.Update(_E007, _E008, _E009);
			}
		}
		_E004.Clear();
		_E00D.vertices = _E007;
		_E00D.normals = _E008;
		if (_E003)
		{
			_E00D.uv = _E00A;
		}
		_E00D.tangents = _E009;
	}

	public void Clear()
	{
		_E007 = new Vector3[Count << 2];
		_E00D.vertices = _E007;
		_E00D.bounds = new Bounds(Vector3.zero, Vector3.zero);
		_E00E = Vector3.zero;
		_E00F = Vector3.zero;
		_E004.Clear();
	}

	public void Add(Vector3 position, Vector3 normal)
	{
		float radius = (this.m__E000 ? (SizeMin + _E8EE.Float() * SizeRandomRange) : SizeMin);
		_E000 obj = _E006[_E005];
		if (++_E005 >= Count)
		{
			_E005 = 0;
		}
		position = base.transform.InverseTransformPoint(position);
		normal = base.transform.InverseTransformDirection(normal);
		_E004.AddLast(obj);
		obj.Calc(position, normal, radius);
		if (_E003)
		{
			obj.Uv = _E001[_E8EE.Int(_E002)];
		}
		_E000(position);
	}

	public void Add(Vector3 position, Vector3 normal, int tile)
	{
		float radius = (this.m__E000 ? (SizeMin + _E8EE.Float() * SizeRandomRange) : SizeMin);
		_E000 obj = _E006[_E005];
		if (++_E005 >= Count)
		{
			_E005 = 0;
		}
		position = base.transform.InverseTransformPoint(position);
		normal = base.transform.InverseTransformDirection(normal);
		_E004.AddLast(obj);
		obj.Calc(position, normal, radius);
		obj.Uv = _E001[tile];
		_E000(position);
	}

	private void _E000(Vector3 position)
	{
		if (!(position.x > _E00E.x) || !(position.y > _E00E.y) || !(position.z > _E00E.z) || !(position.x < _E00F.x) || !(position.y < _E00F.y) || !(position.z < _E00F.z))
		{
			if (position.x < _E00E.x)
			{
				_E00E.x = position.x - 50f;
			}
			if (position.y < _E00E.y)
			{
				_E00E.y = position.y - 50f;
			}
			if (position.z < _E00E.z)
			{
				_E00E.z = position.z - 50f;
			}
			if (position.x > _E00F.x)
			{
				_E00F.x = position.x + 50f;
			}
			if (position.y > _E00F.y)
			{
				_E00F.y = position.y + 50f;
			}
			if (position.z > _E00F.z)
			{
				_E00F.z = position.z + 50f;
			}
			_E00D.bounds = new Bounds
			{
				min = _E00E,
				max = _E00F
			};
		}
	}
}
