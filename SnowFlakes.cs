using UnityEngine;
using UnityEngine.Rendering;

public class SnowFlakes : MonoBehaviour
{
	public float CloseSize;

	public float FarSize;

	public Material Close;

	public Material Far;

	private MeshRenderer[] m__E000;

	private bool m__E001;

	private bool m__E002;

	private static readonly int m__E003 = Shader.PropertyToID(_ED3E._E000(44102));

	private const int _E004 = 16383;

	private const int _E005 = 8191;

	private const int _E006 = 4095;

	private string _E000(string n, object t)
	{
		return string.Format(_ED3E._E000(95558), n, t);
	}

	private void Start()
	{
		this.m__E002 = SystemInfo.systemMemorySize < 2000;
		if (!this.m__E002)
		{
			this.m__E000 = new MeshRenderer[4];
			this.m__E000[0] = _E002(8191, Close);
			this.m__E000[1] = _E002(16383, Close);
			this.m__E000[2] = _E002(16383, Far);
			this.m__E000[3] = _E002(16383, Far);
		}
		else
		{
			this.m__E000 = new MeshRenderer[2];
			this.m__E000[0] = _E002(8191, Close);
			this.m__E000[1] = _E002(16383, Close);
		}
		_E41F.OnScreenChange += _E001;
		_E001();
	}

	private void _E001()
	{
		Close.SetVector(SnowFlakes.m__E003, new Vector2((float)Screen.height / (float)Screen.width, 1f) * CloseSize);
		Far.SetVector(SnowFlakes.m__E003, new Vector2((float)Screen.height / (float)Screen.width, 1f) * FarSize);
	}

	private void Update()
	{
	}

	private MeshRenderer _E002(int particlesCount, Material material)
	{
		GameObject obj = new GameObject(_ED3E._E000(95603) + particlesCount + _ED3E._E000(27308));
		obj.transform.parent = base.transform;
		obj.AddComponent<MeshFilter>().mesh = _E003(particlesCount);
		MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
		meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
		meshRenderer.material = material;
		return meshRenderer;
	}

	private static Mesh _E003(int count)
	{
		int num = count << 2;
		Vector3[] array = new Vector3[num];
		Vector4[] array2 = new Vector4[num];
		Vector3[] array3 = new Vector3[num];
		Vector2[] array4 = new Vector2[num];
		Vector2[] array5 = new Vector2[num];
		int[] array6 = new int[count * 6];
		int i = 0;
		int num2 = 0;
		for (; i < count; i++)
		{
			int num3 = i << 2;
			array6[num2++] = num3;
			array6[num2++] = num3 + 1;
			array6[num2++] = num3 + 2;
			array6[num2++] = num3 + 2;
			array6[num2++] = num3 + 3;
			array6[num2++] = num3;
		}
		int j = 0;
		int num4 = 0;
		for (; j < count; j++)
		{
			int num5 = num4++;
			int num6 = num4++;
			int num7 = num4++;
			int num8 = num4++;
			array4[num5] = new Vector2(0f, 1f);
			array4[num6] = new Vector2(0f, 0f);
			array4[num7] = new Vector2(1f, 0f);
			array4[num8] = new Vector2(1f, 1f);
			Vector2 vector = new Vector2(Random.value - 0.5f, 0f);
			float num9 = (Random.value - 0.5f) * 0.3f;
			vector.y = ((vector.x > 0f) ? (1f - vector.x) : (-1f - vector.x));
			Vector2 vector2 = new Vector2(0f - vector.y, vector.x + num9);
			array5[num5] = vector;
			array5[num6] = vector2;
			vector.y -= num9;
			array5[num7] = -vector;
			vector2.x += num9;
			array5[num8] = -vector2;
			array[num5] = (array[num6] = (array[num7] = (array[num8] = new Vector3(Random.value, Random.value, Random.value))));
			array2[num5] = (array2[num6] = (array2[num7] = (array2[num8] = new Vector4((Random.value - 0.5f) * 100f, (Random.value - 0.5f) * 100f, (Random.value - 0.5f) * 100f, (Random.value - 0.5f) * 100f))));
			array3[num5] = (array3[num6] = (array3[num7] = (array3[num8] = Vector3.zero)));
		}
		return new Mesh
		{
			vertices = array,
			normals = array3,
			tangents = array2,
			uv = array4,
			uv2 = array5,
			triangles = array6,
			bounds = new Bounds(Vector3.zero, Vector3.one * float.MaxValue),
			name = _ED3E._E000(95591)
		};
	}
}
