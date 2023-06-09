using EFT.BlitDebug;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/CandelaSSRRV3")]
public class CandelaSSRRV3 : MonoBehaviour
{
	[Header("QUALITY SETTINGS")]
	[Space]
	public int qualitySize = 1024;

	[Range(1f, 300f)]
	[Space]
	public int GlobalStepCount = 80;

	[Range(0.01f, 64f)]
	public float GlobalStepSize = 5f;

	[Space]
	public float Bias = 0.15f;

	[Header("FADE AND OFFSCREEN RENDER SETTINGS")]
	[Space]
	public float DistanceFade = 40f;

	[Tooltip("Off-Screen Render Camera FOV Set This Higher Than Current Camera FOV")]
	public float OffScreenFOV = 110f;

	[Range(0f, 0.5f)]
	[Tooltip("Cut Off Reflection Computation That  Is Off-Screen")]
	public float OffScreenCutoff = 0.28f;

	[Range(0f, 1f)]
	[Space]
	public float ToksvigPower = 0.064f;

	[Range(0f, 1f)]
	public float ContactBlurPower;

	private Shader m__E000;

	private Shader m__E001;

	private Camera m__E002;

	private Material _E003;

	private Material _E004;

	private Material _E005;

	private Material _E006;

	private Material _E007;

	private RenderTexture _E008;

	private RenderTexture _E009;

	private RenderTexture _E00A;

	private RenderTexture _E00B;

	private RenderTexture _E00C;

	private RenderTexture _E00D;

	private RenderTexture _E00E;

	private static readonly int _E00F = Shader.PropertyToID(_ED3E._E000(18910));

	private static readonly int _E010 = Shader.PropertyToID(_ED3E._E000(18894));

	private static readonly int _E011 = Shader.PropertyToID(_ED3E._E000(29958));

	private static readonly int _E012 = Shader.PropertyToID(_ED3E._E000(19338));

	private static readonly int _E013 = Shader.PropertyToID(_ED3E._E000(19397));

	private static readonly int _E014 = Shader.PropertyToID(_ED3E._E000(19448));

	private static readonly int _E015 = Shader.PropertyToID(_ED3E._E000(19434));

	private static readonly int _E016 = Shader.PropertyToID(_ED3E._E000(30002));

	private static readonly int _E017 = Shader.PropertyToID(_ED3E._E000(19426));

	private static readonly int _E018 = Shader.PropertyToID(_ED3E._E000(29993));

	private static readonly int _E019 = Shader.PropertyToID(_ED3E._E000(30040));

	private static readonly int _E01A = Shader.PropertyToID(_ED3E._E000(30030));

	private static readonly int _E01B = Shader.PropertyToID(_ED3E._E000(30075));

	private static readonly int _E01C = Shader.PropertyToID(_ED3E._E000(30051));

	private static readonly int _E01D = Shader.PropertyToID(_ED3E._E000(30090));

	private static readonly int _E01E = Shader.PropertyToID(_ED3E._E000(30135));

	private static readonly int _E01F = Shader.PropertyToID(_ED3E._E000(19095));

	private static readonly int _E020 = Shader.PropertyToID(_ED3E._E000(30173));

	private static readonly int _E021 = Shader.PropertyToID(_ED3E._E000(30152));

	private static readonly int _E022 = Shader.PropertyToID(_ED3E._E000(30198));

	private static readonly int _E023 = Shader.PropertyToID(_ED3E._E000(30233));

	private static void _E000(Material mat)
	{
		if ((bool)mat)
		{
			Object.DestroyImmediate(mat);
			mat = null;
		}
	}

	private void Awake()
	{
		this.m__E000 = _E3AC.Find(_ED3E._E000(29718));
		this.m__E001 = _E3AC.Find(_ED3E._E000(29759));
	}

	private void OnEnable()
	{
		if (!this.m__E002)
		{
			GameObject gameObject = new GameObject(_ED3E._E000(18759), typeof(Camera));
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			this.m__E002 = gameObject.GetComponent<Camera>();
			this.m__E002.CopyFrom(GetComponent<Camera>());
			this.m__E002.clearFlags = CameraClearFlags.Color;
			this.m__E002.renderingPath = RenderingPath.Forward;
			this.m__E002.backgroundColor = new Color(0f, 0f, 0f, 0f);
			this.m__E002.fieldOfView = OffScreenFOV;
			this.m__E002.enabled = false;
		}
		_E003 = new Material(_E3AC.Find(_ED3E._E000(29734)));
		_E003.hideFlags = HideFlags.HideAndDontSave;
		_E007 = new Material(_E3AC.Find(_ED3E._E000(29779)));
		_E007.hideFlags = HideFlags.HideAndDontSave;
		_E004 = new Material(_E3AC.Find(_ED3E._E000(29819)));
		_E004.hideFlags = HideFlags.HideAndDontSave;
		_E005 = new Material(_E3AC.Find(_ED3E._E000(29797)));
		_E005.hideFlags = HideFlags.HideAndDontSave;
		_E006 = new Material(_E3AC.Find(_ED3E._E000(29845)));
		_E006.hideFlags = HideFlags.HideAndDontSave;
	}

	private void OnDisable()
	{
		Object.DestroyImmediate(this.m__E002);
		_E000(_E003);
		_E000(_E007);
		_E000(_E004);
		_E000(_E005);
		_E000(_E006);
		Object.DestroyImmediate(_E008);
		_E008 = null;
		Object.DestroyImmediate(_E009);
		_E009 = null;
		Object.DestroyImmediate(_E00A);
		_E00A = null;
		Object.DestroyImmediate(_E00B);
		_E00B = null;
		Object.DestroyImmediate(_E00D);
		_E00D = null;
		Object.DestroyImmediate(_E00C);
		_E00C = null;
		Object.DestroyImmediate(_E00E);
		_E00E = null;
	}

	private void _E001()
	{
		if ((bool)this.m__E002)
		{
			qualitySize = Mathf.Clamp(qualitySize, 512, 2048);
			float num = (float)Screen.width / (float)Screen.height;
			int num2 = (int)((float)qualitySize * num);
			int num3 = qualitySize;
			_E002(num2, num3, OffScreenFOV);
			this.m__E002.CopyFrom(GetComponent<Camera>());
			this.m__E002.renderingPath = RenderingPath.Forward;
			this.m__E002.clearFlags = CameraClearFlags.Color;
			this.m__E002.fieldOfView = OffScreenFOV;
			this.m__E002.backgroundColor = new Color(1f, 1f, 1f, 1f);
			if (_E009 == null || _E009.width != num2 || _E009.height != num3)
			{
				Object.DestroyImmediate(_E009);
				_E009 = new RenderTexture(num2, num3, 24, RenderTextureFormat.RFloat)
				{
					name = _ED3E._E000(29831)
				};
				_E009.filterMode = FilterMode.Point;
			}
			this.m__E002.targetTexture = _E009;
			this.m__E002.RenderWithShader(this.m__E000, "");
			_E003.SetTexture(_E00F, _E009);
			this.m__E002.backgroundColor = new Color(0f, 0f, 0f, 0f);
			if (_E00A == null || _E00A.width != num2 || _E00A.height != num3)
			{
				Object.DestroyImmediate(_E00A);
				_E00A = new RenderTexture(num2 / 2, num3 / 2, 24, RuntimeUtilities.defaultHDRRenderTextureFormat)
				{
					name = _ED3E._E000(29875)
				};
			}
			this.m__E002.targetTexture = _E00A;
			this.m__E002.RenderWithShader(this.m__E001, "");
			_E003.SetTexture(_E010, _E00A);
			this.m__E002.clearFlags = CameraClearFlags.Skybox;
			if (_E00B == null || _E00B.width != num2 || _E00B.height != num3)
			{
				Object.DestroyImmediate(_E00B);
				_E00B = new RenderTexture(num2, num3, 16, RuntimeUtilities.defaultHDRRenderTextureFormat)
				{
					name = _ED3E._E000(29913)
				};
			}
			this.m__E002.targetTexture = _E00B;
			this.m__E002.Render();
			_E003.SetTexture(_E011, _E00B);
			this.m__E002.targetTexture = null;
			RenderTextureFormat defaultHDRRenderTextureFormat = RuntimeUtilities.defaultHDRRenderTextureFormat;
			if (_E008 == null || _E008.width != num2 || _E008.height != num3)
			{
				Object.DestroyImmediate(_E008);
				_E008 = new RenderTexture(num2, num3, 0, defaultHDRRenderTextureFormat)
				{
					name = _ED3E._E000(29894)
				};
				_E008.hideFlags = HideFlags.HideAndDontSave;
				_E008.wrapMode = TextureWrapMode.Clamp;
			}
			Graphics.Blit(null, _E008, _E003);
			if (_E00D == null || _E00D.width != num2 || _E00D.height != num3)
			{
				Object.DestroyImmediate(_E00D);
				_E00D = new RenderTexture(num2, num3, 0, defaultHDRRenderTextureFormat)
				{
					name = _ED3E._E000(29933)
				};
				_E00D.hideFlags = HideFlags.HideAndDontSave;
				_E00D.wrapMode = TextureWrapMode.Clamp;
			}
			Graphics.Blit(_E008, _E00D, _E004);
			if (_E00C == null || _E00C.width != num2 || _E00C.height != num3)
			{
				Object.DestroyImmediate(_E00C);
				_E00C = new RenderTexture(num2, num3, 0, RenderTextureFormat.ARGB32)
				{
					name = _ED3E._E000(18949)
				};
				_E00C.hideFlags = HideFlags.HideAndDontSave;
				_E00C.wrapMode = TextureWrapMode.Clamp;
			}
			_E005.SetFloat(_E012, ToksvigPower);
			_E005.SetTexture(_E010, _E00A);
			Graphics.Blit(_E00B, _E00C, _E005);
			if (_E00E == null || _E00E.width != num2 || _E00E.height != num3)
			{
				Object.DestroyImmediate(_E00E);
				_E00E = new RenderTexture(num2, num3, 0, defaultHDRRenderTextureFormat)
				{
					name = _ED3E._E000(29974)
				};
				_E00E.hideFlags = HideFlags.HideAndDontSave;
				_E00E.wrapMode = TextureWrapMode.Clamp;
			}
			_E006.SetTexture(_E010, _E00A);
			_E006.SetTexture(_E013, _E00C);
			_E006.SetFloat(_E014, ContactBlurPower);
			float value = 1.3f;
			_E006.SetFloat(_E015, value);
			DebugGraphics.Blit(_E00D, _E00E, _E006, 0);
			DebugGraphics.Blit(_E00E, _E00D, _E006, 1);
			value = 0.3f;
			_E006.SetFloat(_E015, value);
			DebugGraphics.Blit(_E00D, _E00E, _E006, 0);
			DebugGraphics.Blit(_E00E, _E00D, _E006, 1);
			_E007.SetTexture(_E010, _E00A);
			_E007.SetTexture(_E016, _E00C);
			_E007.SetTexture(_E017, _E00D);
			float fieldOfView = GetComponent<Camera>().fieldOfView;
			_E007.SetFloat(_E018, fieldOfView);
			_E007.SetFloat(_E019, OffScreenFOV);
		}
	}

	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		_E001();
		Graphics.Blit(source, destination, _E007);
	}

	private void _E002(int sWidth, int sHeight, float OffscreenFOV)
	{
		if (!_E003)
		{
			return;
		}
		this.m__E002.CopyFrom(GetComponent<Camera>());
		this.m__E002.renderingPath = RenderingPath.Forward;
		this.m__E002.clearFlags = CameraClearFlags.Color;
		this.m__E002.fieldOfView = OffscreenFOV;
		_E003.SetFloat(_E01A, GlobalStepSize);
		_E003.SetFloat(_E01B, Bias);
		_E003.SetFloat(_E01C, GlobalStepCount);
		_E003.SetFloat(_E01D, DistanceFade);
		_E003.SetFloat(_E01E, OffScreenCutoff);
		Matrix4x4 matrix4x = Matrix4x4.TRS(new Vector3(0.5f, 0.5f, 0f), Quaternion.identity, new Vector3(0.5f, 0.5f, 1f));
		Matrix4x4 matrix4x2 = Matrix4x4.Scale(new Vector3(sWidth, sHeight, 1f));
		Matrix4x4 projectionMatrix = this.m__E002.projectionMatrix;
		Matrix4x4 projectionMatrix2 = this.m__E002.projectionMatrix;
		if (SystemInfo.graphicsDeviceVersion.IndexOf(_ED3E._E000(23402)) > -1)
		{
			for (int i = 0; i < 4; i++)
			{
				projectionMatrix2[2, i] = projectionMatrix2[2, i] * 0.5f + projectionMatrix2[3, i] * 0.5f;
			}
		}
		Matrix4x4 value = matrix4x2 * matrix4x * projectionMatrix;
		Matrix4x4 inverse = (projectionMatrix2 * GetComponent<Camera>().worldToCameraMatrix).inverse;
		Shader.SetGlobalMatrix(_E01F, inverse);
		_E003.SetMatrix(_E020, value);
		_E003.SetVector(_E021, new Vector4(1f / (float)sWidth, 1f / (float)sHeight, 0f, 0f));
		_E003.SetMatrix(_E022, projectionMatrix.inverse);
		_E003.SetMatrix(_E023, this.m__E002.worldToCameraMatrix);
	}
}
