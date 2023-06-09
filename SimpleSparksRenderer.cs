using System;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class SimpleSparksRenderer : MonoBehaviour
{
	public int Segments;

	public int Count = 250;

	private Vector3[] _E000;

	private Vector3[] _E001;

	private Vector4[] _E002;

	private Color32[] _E003;

	private Mesh _E004;

	private Bounds _E005;

	private bool _E006;

	private int _E007;

	private float _E008;

	private Renderer _E009;

	private MaterialPropertyBlock _E00A;

	private static readonly int _E00B = Shader.PropertyToID(_ED3E._E000(88654));

	private static readonly int _E00C = Shader.PropertyToID(_ED3E._E000(88640));

	private void OnValidate()
	{
		Segments = Math.Max(Segments, 2);
		Count = Math.Max(Count, 1);
		int num = Segments + 1 << 1;
		int val = 65535 / num;
		Count = Math.Min(Count, val);
	}

	public void Awake()
	{
		int num = Segments + 1 << 1;
		int num2 = Count * num;
		int num3 = Count * Segments * 6;
		_E000 = new Vector3[num2];
		int[] triangles = new int[num3];
		Vector2[] array = new Vector2[num2];
		Vector2[] array2 = new Vector2[num2];
		Vector2[] array3 = new Vector2[num2];
		_E001 = new Vector3[num2];
		_E002 = new Vector4[num2];
		_E003 = new Color32[num2];
		for (int i = 0; i < Count; i++)
		{
			Fill(i, array, array2, triangles);
		}
		float[] array4 = new float[num];
		for (int j = 0; j < Segments + 1; j++)
		{
			float num4 = (float)Mathf.Clamp(j - 1, 0, Segments - 2) / (float)(Segments - 2);
			array4[j << 1] = (array4[(j << 1) + 1] = num4);
		}
		int k = 0;
		int num5 = 0;
		for (; k < Count; k++)
		{
			float value = UnityEngine.Random.value;
			int num6 = 0;
			while (num6 < num)
			{
				array3[num5] = new Vector2(array4[num6], value);
				num6++;
				num5++;
			}
		}
		if (_E004 != null)
		{
			UnityEngine.Object.Destroy(_E004);
		}
		_E005 = new Bounds(Vector3.zero, Vector3.zero);
		_E004 = new Mesh
		{
			vertices = _E000,
			triangles = triangles,
			uv = array,
			uv2 = array2,
			uv3 = array3,
			normals = _E001,
			tangents = _E002,
			colors32 = _E003,
			name = _ED3E._E000(88619)
		};
		GetComponent<MeshFilter>().mesh = _E004;
		_E009 = GetComponent<Renderer>();
		_E00A = new MaterialPropertyBlock();
	}

	private void OnDestroy()
	{
		if (_E004 != null)
		{
			UnityEngine.Object.Destroy(_E004);
		}
	}

	public void Fill(int pos, Vector2[] uv0, Vector2[] uv1, int[] triangles)
	{
		int num = Segments + 1;
		int num2 = num << 1;
		int num3 = pos * num2;
		int num4 = num - 1;
		for (int i = 1; i < num4; i++)
		{
			int num5 = num3 + (i << 1);
			int num6 = num5 + 1;
			uv0[num5] = new Vector2(0.5f, 0f);
			uv0[num6] = new Vector2(0.5f, 1f);
			uv1[num5] = new Vector2(0f, -1f);
			uv1[num6] = new Vector2(0f, 1f);
		}
		int num7 = num3;
		int num8 = num3 + 1;
		uv0[num7] = new Vector2(0f, 0f);
		uv0[num8] = new Vector2(0f, 1f);
		uv1[num7] = new Vector2(-1f, -1f);
		uv1[num8] = new Vector2(-1f, 1f);
		int num9 = num3 + num2 - 2;
		int num10 = num9 + 1;
		uv0[num9] = new Vector2(1f, 0f);
		uv0[num10] = new Vector2(1f, 1f);
		uv1[num9] = new Vector2(1f, -1f);
		uv1[num10] = new Vector2(1f, 1f);
		int num11 = pos * Segments * 6;
		for (int j = 0; j < Segments; j++)
		{
			int num12 = num3 + (j << 1);
			int num13 = num12++;
			int num14 = num12++;
			int num15 = num12++;
			triangles[num11++] = num13;
			triangles[num11++] = num15;
			triangles[num11++] = num12;
			triangles[num11++] = num12;
			triangles[num11++] = num14;
			triangles[num11++] = num13;
		}
	}

	public void EmitSeg(Vector3 position, Vector3 velocity, float time, float gravity, float drag, float lifeTime, byte emission = byte.MaxValue, byte size = byte.MaxValue, byte turbulence = byte.MaxValue, byte frequency = byte.MaxValue)
	{
		int num = Segments + 1 << 1;
		int i = _E007 * num;
		int num2 = i + num;
		int num3 = 0;
		for (; i < num2; i++)
		{
			_E000[i] = position;
			_E001[i] = velocity;
			_E002[i] = new Vector4(time, lifeTime, gravity, drag);
			_E003[i] = new Color32(emission, size, turbulence, frequency);
			num3++;
		}
		_E007++;
		if (_E007 >= Count)
		{
			_E007 = 0;
		}
		float size2 = (velocity.x + velocity.y + velocity.z) * lifeTime + gravity * lifeTime * lifeTime;
		ExpandBoundsFast(ref _E005, position, size2);
		_E006 = true;
	}

	public static void ExpandBoundsFast(ref Bounds bounds, Vector3 position, float size)
	{
		position.x = ((position.x > 0f) ? position.x : (0f - position.x));
		position.y = ((position.y > 0f) ? position.y : (0f - position.y));
		position.z = ((position.z > 0f) ? position.z : (0f - position.z));
		position.x += size;
		position.y += size;
		position.z += size;
		Vector3 extents = bounds.extents;
		extents.x = ((extents.x > position.x) ? extents.x : position.x);
		extents.y = ((extents.y > position.y) ? extents.y : position.y);
		extents.z = ((extents.z > position.z) ? extents.z : position.z);
		bounds.extents = extents;
	}

	private void LateUpdate()
	{
		if (_E006)
		{
			_E004.vertices = _E000;
			_E004.normals = _E001;
			_E004.tangents = _E002;
			_E004.colors32 = _E003;
			_E004.bounds = _E005;
			_E006 = false;
		}
	}

	private void Update()
	{
		_E00A.SetFloat(_E00B, Time.time);
		_E00A.SetFloat(_E00C, _E008);
		_E009.SetPropertyBlock(_E00A);
		_E008 = Time.time;
	}
}
