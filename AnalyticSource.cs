using UnityEngine;

[ExecuteInEditMode]
public class AnalyticSource : MonoBehaviour
{
	[_E3EC("If you want to update settings or transform wile plaing, you should call UpdateSettins manually")]
	public bool IsAdditional;

	public float LightIntensity = 7f;

	[Range(0f, 8f)]
	public float LightIntensityMultiplicator = 1f;

	public float FadeNormals;

	public float AddAmbient;

	public Vector2 DoorSize = new Vector2(0.25f, 0.25f);

	public Vector3 CubeScale = new Vector4(1f, 1f, 1f);

	public Vector3 CubeShift = new Vector4(0f, 0f, 0f);

	public Vector3 FadeOut = new Vector3(0.25f, 0.25f, 0.25f);

	public Transform CubeControlHelper;

	public AmbientLight.CullingSettings Culling;

	public StencilShadow LinkedStencilShadow;

	[HideInInspector]
	public Bounds Bounds;

	[HideInInspector]
	public MaterialPropertyBlock MaterialPropertyBlock;

	[HideInInspector]
	public Matrix4x4 LocalToWorldMatrix;

	[HideInInspector]
	public int ShaderPath;

	public int DesiredDrawPriority = -1;

	[HideInInspector]
	[SerializeField]
	private int _drawPriority = -1;

	private static readonly int _E000 = Shader.PropertyToID(_ED3E._E000(89716));

	private static readonly int _E001 = Shader.PropertyToID(_ED3E._E000(89711));

	private static readonly int _E002 = Shader.PropertyToID(_ED3E._E000(89698));

	private static readonly int _E003 = Shader.PropertyToID(_ED3E._E000(89748));

	private static readonly int _E004 = Shader.PropertyToID(_ED3E._E000(89741));

	private static readonly int _E005 = Shader.PropertyToID(_ED3E._E000(89730));

	private static readonly int _E006 = Shader.PropertyToID(_ED3E._E000(89778));

	private static readonly int _E007 = Shader.PropertyToID(_ED3E._E000(89815));

	private static readonly int _E008 = Shader.PropertyToID(_ED3E._E000(89803));

	public int ActualDrawPriority => _drawPriority;

	private void Awake()
	{
		AmbientLight.AddAnalyticSource(this);
	}

	protected virtual void Start()
	{
		UpdateSettings();
	}

	protected virtual void OnEnable()
	{
		AmbientLight.AddAnalyticSource(this);
		UpdateSettings();
	}

	protected virtual void OnDisable()
	{
		AmbientLight.RemoveAnalyticSource(this);
	}

	public void UpdateSettings()
	{
		if (CubeControlHelper != null)
		{
			CubeScale = CubeControlHelper.localScale * 0.5f;
			CubeShift = CubeControlHelper.localPosition;
			CubeControlHelper.localRotation = Quaternion.identity;
		}
		if (MaterialPropertyBlock == null)
		{
			MaterialPropertyBlock = new MaterialPropertyBlock();
		}
		MaterialPropertyBlock.SetVector(_E000, CubeScale);
		MaterialPropertyBlock.SetVector(_E001, CubeShift);
		MaterialPropertyBlock.SetVector(_E002, DoorSize);
		MaterialPropertyBlock.SetVector(_E003, new Vector4(1f / FadeOut.x, 1f / FadeOut.y, 1f / FadeOut.z));
		MaterialPropertyBlock.SetFloat(_E004, 1f / FadeNormals);
		MaterialPropertyBlock.SetFloat(_E005, LightIntensity);
		MaterialPropertyBlock.SetFloat(_E006, LightIntensityMultiplicator);
		MaterialPropertyBlock.SetFloat(_E007, AddAmbient);
		MaterialPropertyBlock.SetFloat(_E008, CubeShift.z - CubeScale.z);
		Vector3 vector = CubeScale * 2f + CubeShift;
		float num = Mathf.Max(vector.x, Mathf.Max(vector.y, vector.z)) * 0.5f;
		num += 0.2f;
		num = Mathf.Sqrt(num * num * 3f) * 2f;
		Bounds = new Bounds(base.transform.position, new Vector3(num, num, num));
		LocalToWorldMatrix = base.transform.localToWorldMatrix;
		if (Culling != null)
		{
			Culling.Update();
		}
	}
}
