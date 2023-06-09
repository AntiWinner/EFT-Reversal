using UnityEngine;
using UnityEngine.Rendering;

public class RainFallDrops : MonoBehaviour
{
	[SerializeField]
	[Range(0f, 1f)]
	private float _intensity = 1f;

	[Range(0f, 1f)]
	[SerializeField]
	private float _intensityThreshold = 0.001f;

	[Range(128f, 16383f)]
	public int Count = 8191;

	public Vector2 MinSize = new Vector2(0.04f, 0.15f);

	public Vector2 MaxSize = new Vector2(0.04f, 0.15f);

	[_E2BD(0f, 1f, -1f)]
	public Vector2 MinMaxSideSpeed = new Vector2(0f, 1f);

	[_E2BD(0f, 2f, -1f)]
	public Vector2 MinMaxDensity = new Vector2(0f, 1f);

	[SerializeField]
	private Material _close;

	[Range(0f, 1f)]
	[SerializeField]
	public float DropsAlphaClose = 0.116f;

	[SerializeField]
	[Range(0f, 1f)]
	public float MinAmbient = 0.2f;

	[SerializeField]
	[Range(0f, 1f)]
	public float MinAmbientAddition = 0.4f;

	[SerializeField]
	[Range(0f, 1f)]
	public float MinAmbientAdditionCoef;

	public float SideSpeed;

	private const int m__E000 = 16383;

	private MeshRenderer m__E001;

	private GameObject m__E002;

	private Material m__E003;

	private static readonly int _E004 = Shader.PropertyToID(_ED3E._E000(84197));

	private static readonly int _E005 = Shader.PropertyToID(_ED3E._E000(84248));

	private static readonly int _E006 = Shader.PropertyToID(_ED3E._E000(35970));

	private static readonly int _E007 = Shader.PropertyToID(_ED3E._E000(84239));

	private static readonly int _E008 = Shader.PropertyToID(_ED3E._E000(84226));

	private static readonly int _E009 = Shader.PropertyToID(_ED3E._E000(44102));

	private static readonly int _E00A = Shader.PropertyToID(_ED3E._E000(84279));

	public float Intensity
	{
		get
		{
			return _intensity;
		}
		set
		{
			_intensity = Mathf.Clamp01(value);
		}
	}

	public void Init()
	{
		this.m__E003 = _close.CopyToPreventMaterialChangeInEditor();
		this.m__E001 = _E001(Count, this.m__E003);
		_E41F.OnScreenChange += _E000;
		_E000();
	}

	private void _E000()
	{
		this.m__E003.SetFloat(_E004, DropsAlphaClose);
	}

	private void Update()
	{
		if (!(this.m__E001 == null))
		{
			this.m__E001.enabled = Intensity > _intensityThreshold;
			_E002();
		}
	}

	private void OnDestroy()
	{
		if (this.m__E002 != null)
		{
			Object.Destroy(this.m__E002);
		}
	}

	private MeshRenderer _E001(int particlesCount, Material material)
	{
		this.m__E002 = new GameObject(_ED3E._E000(84165) + particlesCount + _ED3E._E000(27308));
		this.m__E002.transform.parent = base.transform;
		this.m__E002.AddComponent<MeshFilter>().mesh = _E003(particlesCount);
		MeshRenderer meshRenderer = this.m__E002.AddComponent<MeshRenderer>();
		meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
		meshRenderer.material = material;
		return meshRenderer;
	}

	private void _E002()
	{
		this.m__E003.SetVector(_E005, RainController.FallingVectorV3);
		this.m__E003.SetFloat(_E006, Intensity);
		this.m__E003.SetFloat(_E007, SideSpeed * Mathf.LerpUnclamped(MinMaxSideSpeed.x, MinMaxSideSpeed.y, Intensity));
		this.m__E003.SetFloat(_E008, Mathf.LerpUnclamped(MinMaxDensity.x, MinMaxDensity.y, Intensity));
		this.m__E003.SetVector(_E009, Vector2.Scale(new Vector2((float)Screen.height / (float)Screen.width, 1f), Vector2.LerpUnclamped(MinSize, MaxSize, Intensity)));
		this.m__E003.SetFloat(_E00A, MinAmbient + MinAmbientAddition * MinAmbientAdditionCoef);
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
			float num9 = Random.Range(0.1f, 0.6f);
			float num10 = Random.Range(num9, 1f);
			array5[num5] = new Vector2(0f - num9, num10);
			array5[num6] = new Vector2(0f - num9, 0f - num10);
			array5[num7] = new Vector2(num9, 0f - num10);
			array5[num8] = new Vector2(num9, num10);
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
			name = _ED3E._E000(84223)
		};
	}
}
