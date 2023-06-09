using EFT.BlitDebug;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[AddComponentMenu("Image Effects/CandelaSSRR")]
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CandelaSSRR : MonoBehaviour
{
	[Range(0.1f, 40f)]
	[HideInInspector]
	public float GlobalScale = 7.5f;

	[Range(1f, 120f)]
	[HideInInspector]
	public int maxGlobalStep = 85;

	[Range(1f, 40f)]
	[HideInInspector]
	public int maxFineStep = 12;

	[HideInInspector]
	[Range(0f, 0.001f)]
	public float bias = 0.0065f;

	[HideInInspector]
	[Range(0f, 10f)]
	public float fadePower = 0.3f;

	[HideInInspector]
	[Range(1f, 10f)]
	public float fresfade = 4.3f;

	[Range(0.001f, 1.5f)]
	[HideInInspector]
	public float fresrange = 0.55f;

	[HideInInspector]
	[Range(0f, 1f)]
	public float maxDepthCull = 1f;

	[HideInInspector]
	[Range(0f, 1f)]
	public float reflectionMultiply = 1f;

	[Range(0f, 1f)]
	[HideInInspector]
	public float ToksvigPower = 0.14f;

	[Range(0f, 1f)]
	[HideInInspector]
	public float ContactBlurPower = 0.4f;

	[HideInInspector]
	[Range(0f, 10f)]
	public float GlobalBlurRadius = 0.8f;

	[Range(0f, 8f)]
	[HideInInspector]
	public float DistanceBlurRadius = 0.8f;

	[Range(0f, 10f)]
	[HideInInspector]
	public float DistanceBlurStart = 3.5f;

	[HideInInspector]
	[Range(0f, 1f)]
	public float GrazeBlurPower;

	[HideInInspector]
	public bool BlurQualityHigh = true;

	[Range(1f, 5f)]
	[HideInInspector]
	public int HQ_BlurIterations = 2;

	[HideInInspector]
	public float HQ_DepthSensetivity = 1.15f;

	[HideInInspector]
	public float HQ_NormalsSensetivity = 1.07f;

	[HideInInspector]
	public float DebugScreenFade;

	[HideInInspector]
	[Range(0f, 10f)]
	public float ScreenFadePower = 9.47f;

	[Range(0f, 3f)]
	[HideInInspector]
	public float ScreenFadeSpread;

	[HideInInspector]
	[Range(0f, 4f)]
	public float ScreenFadeEdge = 0.03f;

	[HideInInspector]
	public float UseEdgeTexture;

	[HideInInspector]
	public Texture2D EdgeFadeTexture;

	[HideInInspector]
	public float SSRRcomposeMode = 1f;

	[HideInInspector]
	public bool HDRreflections = true;

	[HideInInspector]
	public bool UseCustomDepth;

	[HideInInspector]
	public bool InvertRoughness;

	[HideInInspector]
	public bool UseLayerMask;

	public LayerMask cullingmask = 0;

	[HideInInspector]
	public bool renderCustomColorMap;

	[HideInInspector]
	public float alphaBiasControlSSRR = 1f;

	[HideInInspector]
	public bool enableSkyReflections = true;

	[HideInInspector]
	public int convolutionMode = 2;

	[HideInInspector]
	public float convolutionSamples = 8f;

	[HideInInspector]
	public int qualityWidth = 1024;

	[HideInInspector]
	public int qualityHeight = 1024;

	[HideInInspector]
	public int qualityIndex = 2;

	public bool UseSourceSize;

	[HideInInspector]
	public Camera attachedCamera;

	private Shader m__E000;

	private Shader m__E001;

	private Camera _E002;

	private Material _E003;

	private Material _E004;

	private Material _E005;

	private RenderTexture _E006;

	private RenderTexture _E007;

	private RenderTexture _E008;

	private RenderTexture _E009;

	private Material _E00A;

	private Material _E00B;

	private bool _E00C;

	private static readonly int _E00D = Shader.PropertyToID(_ED3E._E000(18996));

	private static readonly int _E00E = Shader.PropertyToID(_ED3E._E000(18988));

	private static readonly int _E00F = Shader.PropertyToID(_ED3E._E000(19037));

	private static readonly int _E010 = Shader.PropertyToID(_ED3E._E000(19035));

	private static readonly int _E011 = Shader.PropertyToID(_ED3E._E000(19020));

	private static readonly int _E012 = Shader.PropertyToID(_ED3E._E000(19009));

	private static readonly int _E013 = Shader.PropertyToID(_ED3E._E000(19063));

	private static readonly int _E014 = Shader.PropertyToID(_ED3E._E000(19050));

	private static readonly int _E015 = Shader.PropertyToID(_ED3E._E000(19100));

	private static readonly int _E016 = Shader.PropertyToID(_ED3E._E000(19095));

	private static readonly int _E017 = Shader.PropertyToID(_ED3E._E000(19075));

	private static readonly int _E018 = Shader.PropertyToID(_ED3E._E000(19119));

	private static readonly int _E019 = Shader.PropertyToID(_ED3E._E000(19167));

	private static readonly int _E01A = Shader.PropertyToID(_ED3E._E000(19146));

	private static readonly int _E01B = Shader.PropertyToID(_ED3E._E000(19195));

	private static readonly int _E01C = Shader.PropertyToID(_ED3E._E000(19168));

	private static readonly int _E01D = Shader.PropertyToID(_ED3E._E000(19220));

	private static readonly int _E01E = Shader.PropertyToID(_ED3E._E000(19211));

	private static readonly int _E01F = Shader.PropertyToID(_ED3E._E000(19263));

	private static readonly int _E020 = Shader.PropertyToID(_ED3E._E000(19249));

	private static readonly int _E021 = Shader.PropertyToID(_ED3E._E000(19237));

	private static readonly int _E022 = Shader.PropertyToID(_ED3E._E000(19281));

	private static readonly int _E023 = Shader.PropertyToID(_ED3E._E000(19265));

	private static readonly int _E024 = Shader.PropertyToID(_ED3E._E000(19314));

	private static readonly int _E025 = Shader.PropertyToID(_ED3E._E000(19358));

	private static readonly int _E026 = Shader.PropertyToID(_ED3E._E000(19338));

	private static readonly int _E027 = Shader.PropertyToID(_ED3E._E000(19384));

	private static readonly int _E028 = Shader.PropertyToID(_ED3E._E000(19371));

	private static readonly int _E029 = Shader.PropertyToID(_ED3E._E000(19415));

	private static readonly int _E02A = Shader.PropertyToID(_ED3E._E000(19397));

	private static readonly int _E02B = Shader.PropertyToID(_ED3E._E000(19448));

	private static readonly int _E02C = Shader.PropertyToID(_ED3E._E000(19434));

	private static readonly int _E02D = Shader.PropertyToID(_ED3E._E000(19426));

	private static void _E000(Material mat)
	{
		if ((bool)mat)
		{
			Object.DestroyImmediate(mat);
			mat = null;
		}
	}

	private static Material _E001(string shadername)
	{
		if (string.IsNullOrEmpty(shadername))
		{
			return null;
		}
		return new Material(_E3AC.Find(shadername))
		{
			hideFlags = HideFlags.HideAndDontSave
		};
	}

	private void Awake()
	{
		this.m__E000 = _E3AC.Find(_ED3E._E000(18742));
		this.m__E001 = _E3AC.Find(_ED3E._E000(18781));
		_E00C = SystemInfo.graphicsDeviceVersion.IndexOf(_ED3E._E000(23402)) > -1;
	}

	private void OnEnable()
	{
		if (!_E002)
		{
			GameObject gameObject = new GameObject(_ED3E._E000(18759), typeof(Camera));
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			_E002 = gameObject.GetComponent<Camera>();
			_E002.CopyFrom(GetComponent<Camera>());
			_E002.clearFlags = CameraClearFlags.Color;
			_E002.renderingPath = RenderingPath.Forward;
			_E002.backgroundColor = new Color(0f, 0f, 0f, 0f);
			_E002.enabled = false;
		}
		_E003 = new Material(_E3AC.Find(_ED3E._E000(18804)));
		_E003.hideFlags = HideFlags.HideAndDontSave;
		_E00A = new Material(_E3AC.Find(_ED3E._E000(18785)));
		_E00A.hideFlags = HideFlags.HideAndDontSave;
		_E005 = new Material(_E3AC.Find(_ED3E._E000(18831)));
		_E005.hideFlags = HideFlags.HideAndDontSave;
		_E004 = new Material(_E3AC.Find(_ED3E._E000(18878)));
		_E004.hideFlags = HideFlags.HideAndDontSave;
		if (_E00B == null)
		{
			_E00B = _E001(_ED3E._E000(18862));
		}
	}

	private void OnPreRender()
	{
		if (GetComponent<Camera>().renderingPath == RenderingPath.DeferredLighting)
		{
			attachedCamera = GetComponent<Camera>();
			attachedCamera.depthTextureMode |= DepthTextureMode.Depth;
			attachedCamera.depthTextureMode |= DepthTextureMode.DepthNormals;
		}
		if ((bool)_E002)
		{
			_E002.CopyFrom(GetComponent<Camera>());
			_E002.renderingPath = RenderingPath.Forward;
			_E002.clearFlags = CameraClearFlags.Color;
			if (GetComponent<Camera>().renderingPath == RenderingPath.Forward)
			{
				attachedCamera = GetComponent<Camera>();
				attachedCamera.depthTextureMode |= DepthTextureMode.Depth;
				attachedCamera.depthTextureMode |= DepthTextureMode.DepthNormals;
				_E002.backgroundColor = new Color(1f, 1f, 1f, 1f);
				RenderTexture temporary = RenderTexture.GetTemporary(Screen.width, Screen.height, 16, RenderTextureFormat.RHalf);
				temporary.filterMode = FilterMode.Point;
				_E002.targetTexture = temporary;
				_E002.RenderWithShader(this.m__E000, "");
				temporary.SetGlobalShaderProperty(_ED3E._E000(18910));
				RenderTexture.ReleaseTemporary(temporary);
				_E002.backgroundColor = new Color(0f, 0f, 0f, 0f);
				_E002.renderingPath = RenderingPath.Forward;
				RenderTexture temporary2 = RenderTexture.GetTemporary(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
				_E002.targetTexture = temporary2;
				_E002.RenderWithShader(this.m__E001, "");
				temporary2.SetGlobalShaderProperty(_ED3E._E000(18894));
				RenderTexture.ReleaseTemporary(temporary2);
			}
		}
	}

	private void OnDisable()
	{
		Object.DestroyImmediate(_E002);
		_E000(_E003);
		_E000(_E00A);
		_E000(_E004);
		_E000(_E00B);
		_E000(_E005);
		Object.DestroyImmediate(_E006);
		_E006 = null;
		Object.DestroyImmediate(_E007);
		_E007 = null;
		Object.DestroyImmediate(_E008);
		_E008 = null;
		Object.DestroyImmediate(_E009);
		_E009 = null;
	}

	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		int width = qualityWidth;
		int height = qualityHeight;
		if (UseSourceSize)
		{
			width = source.width;
			height = source.height;
		}
		RenderTextureFormat format = RenderTextureFormat.ARGB32;
		if (HDRreflections)
		{
			format = RuntimeUtilities.defaultHDRRenderTextureFormat;
		}
		if (_E006 == null || _E006.width != width || _E006.height != height)
		{
			Object.DestroyImmediate(_E006);
			_E006 = new RenderTexture(width, height, 0, format)
			{
				name = _ED3E._E000(18932)
			};
			_E006.hideFlags = HideFlags.HideAndDontSave;
			_E006.wrapMode = TextureWrapMode.Clamp;
		}
		int num = 1;
		if (_E007 == null || _E007.width != width / num || _E007.height != height / num)
		{
			Object.DestroyImmediate(_E007);
			_E007 = new RenderTexture(width / num, height / num, 0, format)
			{
				name = _ED3E._E000(18915)
			};
			_E007.hideFlags = HideFlags.HideAndDontSave;
			_E007.wrapMode = TextureWrapMode.Clamp;
		}
		if (_E008 == null || _E008.width != width / num || _E008.height != height / num)
		{
			Object.DestroyImmediate(_E008);
			_E008 = new RenderTexture(width / num, height / num, 0, format)
			{
				name = _ED3E._E000(18958)
			};
			_E008.hideFlags = HideFlags.HideAndDontSave;
			_E008.wrapMode = TextureWrapMode.Clamp;
		}
		if (_E009 == null || _E009.width != width || _E009.height != height)
		{
			Object.DestroyImmediate(_E009);
			_E009 = new RenderTexture(width, height, 0, RuntimeUtilities.defaultHDRRenderTextureFormat);
			_E009.name = _ED3E._E000(18949);
			_E009.hideFlags = HideFlags.HideAndDontSave;
			_E009.wrapMode = TextureWrapMode.Clamp;
		}
		_E00A.SetFloat(_E00D, width);
		_E003.SetFloat(_E00E, GlobalScale);
		_E003.SetFloat(_E00F, bias);
		_E003.SetFloat(_E010, maxGlobalStep);
		_E003.SetFloat(_E011, maxFineStep);
		_E003.SetFloat(_E012, maxDepthCull);
		_E003.SetFloat(_E013, fadePower);
		_E00A.SetFloat(_E013, fadePower);
		_E00A.SetFloat(_E014, fresfade);
		_E00A.SetFloat(_E015, fresrange);
		Matrix4x4 projectionMatrix = GetComponent<Camera>().projectionMatrix;
		if (_E00C)
		{
			for (int i = 0; i < 4; i++)
			{
				projectionMatrix[2, i] = projectionMatrix[2, i] * 0.5f + projectionMatrix[3, i] * 0.5f;
			}
		}
		Matrix4x4 inverse = (projectionMatrix * GetComponent<Camera>().worldToCameraMatrix).inverse;
		Shader.SetGlobalMatrix(_E016, inverse);
		Shader.SetGlobalFloat(_E017, DistanceBlurRadius);
		Shader.SetGlobalFloat(_E018, GrazeBlurPower);
		Shader.SetGlobalFloat(_E019, DistanceBlurStart);
		Shader.SetGlobalFloat(_E01A, SSRRcomposeMode);
		Shader.SetGlobalFloat(_E01B, (QualitySettings.antiAliasing > 0 && GetComponent<Camera>().renderingPath == RenderingPath.Forward) ? 1f : 0f);
		_E003.SetMatrix(_E01C, projectionMatrix);
		_E003.SetMatrix(_E01D, projectionMatrix.inverse);
		_E003.SetMatrix(_E01E, GetComponent<Camera>().worldToCameraMatrix.inverse.transpose);
		_E003.SetMatrix(_E01F, GetComponent<Camera>().cameraToWorldMatrix);
		_E00A.SetMatrix(_E01F, GetComponent<Camera>().cameraToWorldMatrix);
		_E005.SetMatrix(_E01F, GetComponent<Camera>().cameraToWorldMatrix);
		_E00A.SetMatrix(_E016, inverse);
		float value = 0f;
		if (enableSkyReflections)
		{
			value = 1f;
		}
		_E003.SetFloat(_E020, value);
		_E00A.SetVector(_E021, new Vector4(DebugScreenFade, ScreenFadePower, ScreenFadeSpread, ScreenFadeEdge));
		_E00A.SetFloat(_E022, UseEdgeTexture);
		_E00A.SetTexture(_E023, EdgeFadeTexture);
		_E00A.SetFloat(_E024, reflectionMultiply);
		_E00A.SetFloat(_E025, convolutionSamples);
		_E005.SetFloat(_E026, ToksvigPower);
		float value2 = 0f;
		float value3 = 0f;
		float value4 = 0f;
		if (GetComponent<Camera>().renderingPath == RenderingPath.Forward)
		{
			value2 = 1f;
		}
		_E00A.SetFloat(_E027, value2);
		_E003.SetFloat(_E027, value2);
		_E005.SetFloat(_E027, value2);
		_E00B.SetFloat(_E027, value2);
		if (GetComponent<Camera>().renderingPath == RenderingPath.DeferredLighting)
		{
			value3 = 1f;
		}
		_E00A.SetFloat(_E028, value3);
		_E003.SetFloat(_E028, value3);
		_E005.SetFloat(_E028, value3);
		_E00B.SetFloat(_E028, value3);
		if (GetComponent<Camera>().renderingPath == RenderingPath.DeferredShading)
		{
			value4 = 1f;
		}
		_E00A.SetFloat(_E029, value4);
		_E003.SetFloat(_E029, value4);
		_E005.SetFloat(_E029, value4);
		_E00B.SetFloat(_E029, value4);
		DebugGraphics.Blit(source, _E006, _E003, 0);
		Graphics.Blit(_E006, _E007, _E004);
		Graphics.Blit(source, _E009, _E005);
		_E00B.SetTexture(_E02A, _E009);
		_E00B.SetFloat(_E02B, ContactBlurPower);
		_E00B.SetFloat(_E013, fadePower);
		float value5 = 1.3f;
		_E00B.SetFloat(_E02C, value5);
		DebugGraphics.Blit(_E007, _E008, _E00B, 0);
		DebugGraphics.Blit(_E008, _E007, _E00B, 1);
		value5 = 0.3f;
		_E00B.SetFloat(_E02C, value5);
		DebugGraphics.Blit(_E007, _E008, _E00B, 0);
		DebugGraphics.Blit(_E008, _E007, _E00B, 1);
		_E00A.SetTexture(_E02A, _E009);
		_E00A.SetTexture(_E02D, _E007);
		Graphics.Blit(source, destination, _E00A);
	}
}
