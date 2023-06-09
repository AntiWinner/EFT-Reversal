using System;
using System.Collections.Generic;
using BSG.CameraEffects;
using EFT.BlitDebug;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class UltimateBloom : MonoBehaviour
{
	public enum BloomQualityPreset
	{
		Optimized,
		Standard,
		HighVisuals,
		Custom
	}

	public enum BloomSamplingQuality
	{
		VerySmallKernel,
		SmallKernel,
		MediumKernel,
		LargeKernel,
		LargerKernel,
		VeryLargeKernel
	}

	public enum BloomScreenBlendMode
	{
		Screen,
		Add
	}

	public enum HDRBloomMode
	{
		Auto,
		On,
		Off
	}

	public enum BlurSampleCount
	{
		Nine,
		Seventeen,
		Thirteen,
		TwentyThree,
		TwentySeven,
		ThrirtyOne,
		NineCurve,
		FourSimple
	}

	public enum FlareRendering
	{
		Sharp,
		Blurred,
		MoreBlurred
	}

	public enum SimpleSampleCount
	{
		Four,
		Nine,
		FourCurve,
		ThirteenTemporal,
		ThirteenTemporalCurve
	}

	public enum FlareType
	{
		Single,
		Double
	}

	public enum BloomIntensityManagement
	{
		FilmicCurve,
		Threshold
	}

	private enum FlareStripeType
	{
		Anamorphic,
		Star,
		DiagonalUpright,
		DiagonalUpleft
	}

	public enum AnamorphicDirection
	{
		Horizontal,
		Vertical
	}

	public enum BokehFlareQuality
	{
		Low,
		Medium,
		High,
		VeryHigh
	}

	public enum BlendMode
	{
		ADD,
		SCREEN
	}

	public enum SamplingMode
	{
		Fixed,
		HeightRelative
	}

	public enum FlareBlurQuality
	{
		Fast,
		Normal,
		High
	}

	public enum FlarePresets
	{
		ChoosePreset,
		GhostFast,
		Ghost1,
		Ghost2,
		Ghost3,
		Bokeh1,
		Bokeh2,
		Bokeh3
	}

	private delegate void _E000(RenderTexture source, RenderTexture destination, float horizontalBlur, float verticalBlur, RenderTexture additiveTexture, BlurSampleCount sampleCount, Color tint, float intensity);

	public float m_SamplingMinHeight = 400f;

	public float[] m_ResSamplingPixelCount = new float[6];

	public SamplingMode m_SamplingMode;

	public BlendMode m_BlendMode;

	public float m_ScreenMaxIntensity;

	public BloomQualityPreset m_QualityPreset;

	public HDRBloomMode m_HDR;

	public BloomScreenBlendMode m_ScreenBlendMode = BloomScreenBlendMode.Add;

	public float m_BloomIntensity = 1f;

	public float m_BloomThreshhold = 0.5f;

	public Color m_BloomThreshholdColor = Color.white;

	public int m_DownscaleCount = 5;

	public BloomIntensityManagement m_IntensityManagement;

	public float[] m_BloomIntensities;

	public Color[] m_BloomColors;

	public bool[] m_BloomUsages;

	[SerializeField]
	public DeluxeFilmicCurve m_BloomCurve = new DeluxeFilmicCurve();

	private int m__E000 = 5;

	public bool useTriangleBlit = true;

	private CommandBuffer m__E001;

	private List<RenderTexture> m__E002 = new List<RenderTexture>();

	private List<MaterialPropertyBlock> m__E003 = new List<MaterialPropertyBlock>();

	private int m__E004;

	public bool m_UseLensFlare;

	public float m_FlareTreshold = 0.8f;

	public float m_FlareIntensity = 0.25f;

	public Color m_FlareTint0 = new Color(0.5372549f, 0.32156864f, 0f);

	public Color m_FlareTint1 = new Color(0f, 21f / 85f, 42f / 85f);

	public Color m_FlareTint2 = new Color(24f / 85f, 0.5921569f, 0f);

	public Color m_FlareTint3 = new Color(38f / 85f, 7f / 51f, 0f);

	public Color m_FlareTint4 = new Color(0.47843137f, 0.34509805f, 0f);

	public Color m_FlareTint5 = new Color(0.5372549f, 0.2784314f, 0f);

	public Color m_FlareTint6 = new Color(0.38039216f, 0.54509807f, 0f);

	public Color m_FlareTint7 = new Color(8f / 51f, 0.5568628f, 0f);

	public float m_FlareGlobalScale = 1f;

	public Vector4 m_FlareScales = new Vector4(1f, 0.6f, 0.5f, 0.4f);

	public Vector4 m_FlareScalesNear = new Vector4(1f, 0.8f, 0.6f, 0.5f);

	public Texture2D m_FlareMask;

	public FlareRendering m_FlareRendering = FlareRendering.Blurred;

	public FlareType m_FlareType = FlareType.Double;

	public Texture2D m_FlareShape;

	public FlareBlurQuality m_FlareBlurQuality = FlareBlurQuality.High;

	private _E3B4 m__E005;

	private Mesh[] m__E006;

	public bool m_UseBokehFlare;

	public float m_BokehScale = 0.4f;

	public BokehFlareQuality m_BokehFlareQuality = BokehFlareQuality.Medium;

	public bool m_UseAnamorphicFlare;

	public float m_AnamorphicFlareTreshold = 0.8f;

	public float m_AnamorphicFlareIntensity = 1f;

	public int m_AnamorphicDownscaleCount = 3;

	public int m_AnamorphicBlurPass = 2;

	private int m__E007;

	private RenderTexture[] m__E008;

	public float[] m_AnamorphicBloomIntensities;

	public Color[] m_AnamorphicBloomColors;

	public bool[] m_AnamorphicBloomUsages;

	public bool m_AnamorphicSmallVerticalBlur = true;

	public AnamorphicDirection m_AnamorphicDirection;

	public float m_AnamorphicScale = 3f;

	public bool m_UseStarFlare;

	public float m_StarFlareTreshol = 0.8f;

	public float m_StarFlareIntensity = 1f;

	public float m_StarScale = 2f;

	public int m_StarDownscaleCount = 3;

	public int m_StarBlurPass = 2;

	private int m__E009;

	private RenderTexture[] m__E00A;

	public float[] m_StarBloomIntensities;

	public Color[] m_StarBloomColors;

	public bool[] m_StarBloomUsages;

	public bool m_UseLensDust;

	public float m_DustIntensity = 1f;

	public Texture2D m_DustTexture;

	public float m_DirtLightIntensity = 5f;

	public BloomSamplingQuality m_DownsamplingQuality;

	public BloomSamplingQuality m_UpsamplingQuality;

	public bool m_TemporalStableDownsampling = true;

	public bool m_InvertImage;

	private Material m__E00B;

	private Shader m__E00C;

	private Material m__E00D;

	private Shader m__E00E;

	private Material m__E00F;

	private Shader m__E010;

	private Material m__E011;

	private Material m__E012;

	private Shader m__E013;

	private Shader m__E014;

	private Material m__E015;

	private Shader m__E016;

	private Material m__E017;

	private Shader _E018;

	private Material _E019;

	private Shader _E01A;

	public bool m_DirectDownSample;

	public bool m_DirectUpsample;

	public bool m_UiShowBloomScales;

	public bool m_UiShowAnamorphicBloomScales;

	public bool m_UiShowStarBloomScales;

	public bool m_UiShowHeightSampling;

	public bool m_UiShowBloomSettings;

	public bool m_UiShowSampling;

	public bool m_UiShowIntensity;

	public bool m_UiShowOptimizations;

	public bool m_UiShowLensDirt;

	public bool m_UiShowLensFlare;

	public bool m_UiShowAnamorphic;

	public bool m_UiShowStar;

	private NightVision _E01B;

	private static readonly int _E01C = Shader.PropertyToID(_ED3E._E000(36139));

	private static readonly int _E01D = Shader.PropertyToID(_ED3E._E000(35970));

	private static readonly int _E01E = Shader.PropertyToID(_ED3E._E000(36188));

	private static readonly int _E01F = Shader.PropertyToID(_ED3E._E000(19813));

	private static readonly int _E020 = Shader.PropertyToID(_ED3E._E000(36172));

	private static readonly int _E021 = Shader.PropertyToID(_ED3E._E000(36162));

	private static readonly int _E022 = Shader.PropertyToID(_ED3E._E000(36211));

	private static readonly int _E023 = Shader.PropertyToID(_ED3E._E000(32939));

	private static readonly int _E024 = Shader.PropertyToID(_ED3E._E000(36194));

	private static readonly int _E025 = Shader.PropertyToID(_ED3E._E000(36238));

	private static readonly int _E026 = Shader.PropertyToID(_ED3E._E000(36282));

	private static readonly int _E027 = Shader.PropertyToID(_ED3E._E000(36271));

	private static readonly int _E028 = Shader.PropertyToID(_ED3E._E000(36312));

	private static readonly int _E029 = Shader.PropertyToID(_ED3E._E000(36300));

	private static readonly int _E02A = Shader.PropertyToID(_ED3E._E000(36288));

	private static readonly int _E02B = Shader.PropertyToID(_ED3E._E000(36340));

	private static readonly int _E02C = Shader.PropertyToID(_ED3E._E000(36328));

	private static readonly int _E02D = Shader.PropertyToID(_ED3E._E000(36380));

	private static readonly int _E02E = Shader.PropertyToID(_ED3E._E000(36368));

	private static readonly int _E02F = Shader.PropertyToID(_ED3E._E000(36356));

	private static readonly int _E030 = Shader.PropertyToID(_ED3E._E000(36408));

	private static readonly int _E031 = Shader.PropertyToID(_ED3E._E000(36399));

	private static readonly int _E032 = Shader.PropertyToID(_ED3E._E000(36394));

	private static readonly int _E033 = Shader.PropertyToID(_ED3E._E000(36445));

	private static readonly int _E034 = Shader.PropertyToID(_ED3E._E000(36442));

	private static readonly int _E035 = Shader.PropertyToID(_ED3E._E000(36428));

	private static readonly int _E036 = Shader.PropertyToID(_ED3E._E000(36422));

	private static readonly int _E037 = Shader.PropertyToID(_ED3E._E000(36474));

	private static readonly int _E038 = Shader.PropertyToID(_ED3E._E000(36463));

	private static readonly int _E039 = Shader.PropertyToID(_ED3E._E000(36453));

	private static readonly int _E03A = Shader.PropertyToID(_ED3E._E000(36505));

	private RenderTexture[] _E03B;

	private RenderTexture[] _E03C;

	private RenderTextureFormat _E03D;

	private bool[] _E03E;

	private RenderTexture _E03F;

	private bool _E040
	{
		get
		{
			if (_E01B != null)
			{
				return _E01B.On;
			}
			return false;
		}
	}

	private MaterialPropertyBlock _E000()
	{
		if (this.m__E004 >= this.m__E003.Count)
		{
			this.m__E003.Add(new MaterialPropertyBlock());
		}
		MaterialPropertyBlock materialPropertyBlock = this.m__E003[this.m__E004];
		this.m__E004++;
		materialPropertyBlock.Clear();
		return materialPropertyBlock;
	}

	private void _E001()
	{
		this.m__E004 = 0;
	}

	private void Start()
	{
		_E01B = base.gameObject.GetComponent<NightVision>();
	}

	private void _E002(Material mat)
	{
		if ((bool)mat)
		{
			UnityEngine.Object.DestroyImmediate(mat);
			mat = null;
		}
	}

	private void _E003(ref Material material, ref Shader shader, string shaderPath)
	{
		if (!(shader != null))
		{
			shader = _E3AC.Find(shaderPath);
			if (shader == null)
			{
				Debug.LogError(_ED3E._E000(36055) + shaderPath);
			}
			else if (!shader.isSupported)
			{
				Debug.LogError(_ED3E._E000(36034) + shaderPath + _ED3E._E000(36074));
			}
			else
			{
				material = _E004(shader);
			}
		}
	}

	public void CreateMaterials()
	{
		int num = 8;
		if (m_BloomIntensities == null || m_BloomIntensities.Length < num)
		{
			m_BloomIntensities = new float[num];
			for (int i = 0; i < 8; i++)
			{
				m_BloomIntensities[i] = 1f;
			}
		}
		if (m_BloomColors == null || m_BloomColors.Length < num)
		{
			m_BloomColors = new Color[num];
			for (int j = 0; j < 8; j++)
			{
				m_BloomColors[j] = Color.white;
			}
		}
		if (m_BloomUsages == null || m_BloomUsages.Length < num)
		{
			m_BloomUsages = new bool[num];
			for (int k = 0; k < 8; k++)
			{
				m_BloomUsages[k] = true;
			}
		}
		if (m_AnamorphicBloomIntensities == null || m_AnamorphicBloomIntensities.Length < num)
		{
			m_AnamorphicBloomIntensities = new float[num];
			for (int l = 0; l < 8; l++)
			{
				m_AnamorphicBloomIntensities[l] = 1f;
			}
		}
		if (m_AnamorphicBloomColors == null || m_AnamorphicBloomColors.Length < num)
		{
			m_AnamorphicBloomColors = new Color[num];
			for (int m = 0; m < 8; m++)
			{
				m_AnamorphicBloomColors[m] = Color.white;
			}
		}
		if (m_AnamorphicBloomUsages == null || m_AnamorphicBloomUsages.Length < num)
		{
			m_AnamorphicBloomUsages = new bool[num];
			for (int n = 0; n < 8; n++)
			{
				m_AnamorphicBloomUsages[n] = true;
			}
		}
		if (m_StarBloomIntensities == null || m_StarBloomIntensities.Length < num)
		{
			m_StarBloomIntensities = new float[num];
			for (int num2 = 0; num2 < 8; num2++)
			{
				m_StarBloomIntensities[num2] = 1f;
			}
		}
		if (m_StarBloomColors == null || m_StarBloomColors.Length < num)
		{
			m_StarBloomColors = new Color[num];
			for (int num3 = 0; num3 < 8; num3++)
			{
				m_StarBloomColors[num3] = Color.white;
			}
		}
		if (m_StarBloomUsages == null || m_StarBloomUsages.Length < num)
		{
			m_StarBloomUsages = new bool[num];
			for (int num4 = 0; num4 < 8; num4++)
			{
				m_StarBloomUsages[num4] = true;
			}
		}
		if (this.m__E005 == null && m_FlareShape != null && m_UseBokehFlare)
		{
			if (this.m__E005 != null)
			{
				this.m__E005.Clear(ref this.m__E006);
			}
			this.m__E005 = new _E3B4();
		}
		if (this.m__E00D == null)
		{
			_E03B = new RenderTexture[_E005()];
			_E03C = new RenderTexture[m_DownscaleCount];
			this.m__E008 = new RenderTexture[m_AnamorphicDownscaleCount];
			this.m__E00A = new RenderTexture[m_StarDownscaleCount];
		}
		string shaderPath = ((m_FlareType == FlareType.Single) ? _ED3E._E000(36907) : _ED3E._E000(38907));
		_E003(ref this.m__E00B, ref this.m__E00C, shaderPath);
		_E003(ref this.m__E00D, ref this.m__E00E, _ED3E._E000(36943));
		_E003(ref this.m__E011, ref this.m__E013, _ED3E._E000(38868));
		_E003(ref this.m__E012, ref this.m__E014, _ED3E._E000(38868));
		_E003(ref this.m__E015, ref this.m__E016, _ED3E._E000(36895));
		_E003(ref this.m__E017, ref _E018, _ED3E._E000(38762));
		_E003(ref _E019, ref _E01A, _ED3E._E000(36865));
		bool num5 = m_UseLensDust || m_UseLensFlare || m_UseAnamorphicFlare || m_UseStarFlare;
		string shaderPath2 = _ED3E._E000(38703);
		if (num5)
		{
			shaderPath2 = _ED3E._E000(38732);
		}
		_E003(ref this.m__E00F, ref this.m__E010, shaderPath2);
	}

	private Material _E004(Shader shader)
	{
		if (!shader)
		{
			return null;
		}
		return new Material(shader)
		{
			hideFlags = HideFlags.HideAndDontSave
		};
	}

	private void OnDisable()
	{
		if (base.gameObject.activeInHierarchy)
		{
			ForceShadersReload();
			if (this.m__E005 != null)
			{
				this.m__E005.Clear(ref this.m__E006);
				this.m__E005 = null;
			}
		}
	}

	public void ForceShadersReload()
	{
		_E002(this.m__E00B);
		this.m__E00B = null;
		this.m__E00C = null;
		_E002(this.m__E00D);
		this.m__E00D = null;
		this.m__E00E = null;
		_E002(this.m__E00F);
		this.m__E00F = null;
		this.m__E010 = null;
		_E002(this.m__E011);
		this.m__E011 = null;
		this.m__E013 = null;
		_E002(this.m__E012);
		this.m__E012 = null;
		this.m__E014 = null;
		_E002(_E019);
		_E019 = null;
		_E01A = null;
		_E002(this.m__E015);
		this.m__E015 = null;
		this.m__E016 = null;
		_E002(this.m__E017);
		this.m__E017 = null;
		_E018 = null;
	}

	private int _E005()
	{
		return Mathf.Max(Mathf.Max(Mathf.Max(m_DownscaleCount, m_UseAnamorphicFlare ? m_AnamorphicDownscaleCount : 0), m_UseLensFlare ? (_E007() + 1) : 0), m_UseStarFlare ? m_StarDownscaleCount : 0);
	}

	private void _E006()
	{
		if (_E03E == null)
		{
			_E03E = new bool[_E03B.Length];
		}
		if (_E03E.Length != _E03B.Length)
		{
			_E03E = new bool[_E03B.Length];
		}
		for (int i = 0; i < _E03E.Length; i++)
		{
			_E03E[i] = false;
		}
		for (int j = 0; j < _E03E.Length; j++)
		{
			_E03E[j] = m_BloomUsages[j] || _E03E[j];
		}
		if (m_UseAnamorphicFlare)
		{
			for (int k = 0; k < _E03E.Length; k++)
			{
				_E03E[k] = m_AnamorphicBloomUsages[k] || _E03E[k];
			}
		}
		if (m_UseStarFlare)
		{
			for (int l = 0; l < _E03E.Length; l++)
			{
				_E03E[l] = m_StarBloomUsages[l] || _E03E[l];
			}
		}
	}

	private int _E007()
	{
		if (m_UseBokehFlare && m_FlareShape != null)
		{
			if (m_BokehFlareQuality == BokehFlareQuality.VeryHigh)
			{
				return 1;
			}
			if (m_BokehFlareQuality == BokehFlareQuality.High)
			{
				return 2;
			}
			if (m_BokehFlareQuality == BokehFlareQuality.Medium)
			{
				return 3;
			}
			if (m_BokehFlareQuality == BokehFlareQuality.Low)
			{
				return 4;
			}
		}
		return 0;
	}

	private BlurSampleCount _E008()
	{
		if (m_SamplingMode == SamplingMode.Fixed)
		{
			BlurSampleCount result = BlurSampleCount.ThrirtyOne;
			if (m_UpsamplingQuality == BloomSamplingQuality.VerySmallKernel)
			{
				result = BlurSampleCount.Nine;
			}
			else if (m_UpsamplingQuality == BloomSamplingQuality.SmallKernel)
			{
				result = BlurSampleCount.Thirteen;
			}
			else if (m_UpsamplingQuality == BloomSamplingQuality.MediumKernel)
			{
				result = BlurSampleCount.Seventeen;
			}
			else if (m_UpsamplingQuality == BloomSamplingQuality.LargeKernel)
			{
				result = BlurSampleCount.TwentyThree;
			}
			else if (m_UpsamplingQuality == BloomSamplingQuality.LargerKernel)
			{
				result = BlurSampleCount.TwentySeven;
			}
			return result;
		}
		float num = Screen.height;
		int num2 = 0;
		float num3 = float.MaxValue;
		for (int i = 0; i < m_ResSamplingPixelCount.Length; i++)
		{
			float num4 = Math.Abs(num - m_ResSamplingPixelCount[i]);
			if (num4 < num3)
			{
				num3 = num4;
				num2 = i;
			}
		}
		return num2 switch
		{
			0 => BlurSampleCount.Nine, 
			1 => BlurSampleCount.Thirteen, 
			2 => BlurSampleCount.Seventeen, 
			3 => BlurSampleCount.TwentyThree, 
			4 => BlurSampleCount.TwentySeven, 
			_ => BlurSampleCount.ThrirtyOne, 
		};
	}

	public void ComputeResolutionRelativeData()
	{
		float num = m_SamplingMinHeight;
		float num2 = 9f;
		for (int i = 0; i < m_ResSamplingPixelCount.Length; i++)
		{
			m_ResSamplingPixelCount[i] = num;
			float num3 = num2 + 4f;
			float num4 = num3 / num2;
			num *= num4;
			num2 = num3;
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.m__E001 == null)
		{
			this.m__E001 = new CommandBuffer();
			this.m__E001.name = _ED3E._E000(36155);
		}
		this.m__E001.Clear();
		bool flag = false;
		flag = ((m_HDR != 0) ? (m_HDR == HDRBloomMode.On) : (source.format == RuntimeUtilities.defaultHDRRenderTextureFormat && GetComponent<Camera>().allowHDR));
		_E03D = (flag ? RuntimeUtilities.defaultHDRRenderTextureFormat : RenderTextureFormat.ARGB32);
		if (_E03B != null && _E03B.Length != _E005())
		{
			OnDisable();
		}
		if (this.m__E000 != m_DownscaleCount || this.m__E007 != m_AnamorphicDownscaleCount || this.m__E009 != m_StarDownscaleCount)
		{
			OnDisable();
		}
		this.m__E000 = m_DownscaleCount;
		this.m__E007 = m_AnamorphicDownscaleCount;
		this.m__E009 = m_StarDownscaleCount;
		CreateMaterials();
		if (m_DirectDownSample || m_DirectUpsample)
		{
			_E006();
		}
		bool flag2 = false;
		if (m_SamplingMode == SamplingMode.HeightRelative)
		{
			ComputeResolutionRelativeData();
		}
		RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0, _E03D);
		temporary.filterMode = FilterMode.Bilinear;
		if (m_IntensityManagement == BloomIntensityManagement.Threshold)
		{
			float num = (_E040 ? 0.5f : 1f);
			_E00F(source, temporary, m_BloomThreshhold * m_BloomThreshholdColor * num, useTriangleBlit ? _E000() : null);
		}
		else
		{
			m_BloomCurve.UpdateCoefficients();
			if (useTriangleBlit)
			{
				_E3A1.BlitOrCopy(this.m__E001, source, temporary);
			}
			else
			{
				_E3A1.BlitOrCopy(source, temporary);
			}
		}
		if (m_IntensityManagement == BloomIntensityManagement.Threshold)
		{
			_E00E(temporary, _E03B, null, flag);
		}
		else
		{
			_E00E(temporary, _E03B, m_BloomCurve, flag);
		}
		BlurSampleCount upsamplingCount = _E008();
		_E00D(_E03B, _E03C, source.width, source.height, upsamplingCount);
		Texture flareRT = Texture2D.blackTexture;
		RenderTexture renderTexture = null;
		if (m_UseLensFlare)
		{
			int num2 = _E007();
			int num3 = source.width / (int)Mathf.Pow(2f, num2);
			int num4 = source.height / (int)Mathf.Pow(2f, num2);
			if (m_FlareShape != null && m_UseBokehFlare)
			{
				float num5 = 15f;
				if (m_BokehFlareQuality == BokehFlareQuality.Medium)
				{
					num5 *= 2f;
				}
				if (m_BokehFlareQuality == BokehFlareQuality.High)
				{
					num5 *= 4f;
				}
				if (m_BokehFlareQuality == BokehFlareQuality.VeryHigh)
				{
					num5 *= 8f;
				}
				num5 *= m_BokehScale;
				this.m__E005.SetMaterial(_E019);
				this.m__E005.RebuildMeshIfNeeded(num3, num4, 1f / (float)num3 * num5, 1f / (float)num4 * num5, ref this.m__E006);
				this.m__E005.SetTexture(m_FlareShape);
				renderTexture = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, _E03D);
				int num6 = num2;
				RenderTexture temporary2 = RenderTexture.GetTemporary(source.width / (int)Mathf.Pow(2f, num6 + 1), source.height / (int)Mathf.Pow(2f, num6 + 1), 0, _E03D);
				_E00F(_E03B[num2], temporary2, m_FlareTreshold * Vector4.one, useTriangleBlit ? _E000() : null);
				this.m__E005.RenderFlare(useTriangleBlit ? this.m__E001 : null, temporary2, renderTexture, m_UseBokehFlare ? 1f : m_FlareIntensity, ref this.m__E006);
				this.m__E002.Add(temporary2);
				RenderTexture temporary3 = RenderTexture.GetTemporary(renderTexture.width, renderTexture.height, 0, _E03D);
				this.m__E015.SetTexture(_E01C, m_FlareMask);
				if (useTriangleBlit)
				{
					this.m__E001.BlitFullscreenTriangle(renderTexture, temporary3, this.m__E015);
				}
				else
				{
					DebugGraphics.Blit(renderTexture, temporary3, this.m__E015, 0);
				}
				this.m__E002.Add(renderTexture);
				renderTexture = null;
				_E00C(temporary3, source, ref flareRT);
				this.m__E002.Add(temporary3);
			}
			else
			{
				int num7 = _E007();
				RenderTexture renderTexture2 = _E03B[num7];
				RenderTexture temporary4 = RenderTexture.GetTemporary(renderTexture2.width, renderTexture2.height, 0, _E03D);
				_E010(_E03B[num7], temporary4, m_FlareTreshold * Vector4.one, m_FlareMask, useTriangleBlit ? _E000() : null);
				_E00C(temporary4, source, ref flareRT);
				this.m__E002.Add(temporary4);
			}
		}
		if (m_UseAnamorphicFlare)
		{
			RenderTexture renderTexture3 = _E00B(_E03B, upsamplingCount, source.width, source.height, FlareStripeType.Anamorphic);
			if (renderTexture3 != null)
			{
				if (m_UseLensFlare)
				{
					_E015(renderTexture3, (RenderTexture)flareRT, 1f, useTriangleBlit ? _E000() : null);
					this.m__E002.Add(renderTexture3);
				}
				else
				{
					flareRT = renderTexture3;
				}
			}
		}
		if (m_UseStarFlare)
		{
			RenderTexture renderTexture4 = null;
			if (m_StarBlurPass == 1)
			{
				renderTexture4 = _E00B(_E03B, upsamplingCount, source.width, source.height, FlareStripeType.Star);
				if (renderTexture4 != null)
				{
					if (m_UseLensFlare || m_UseAnamorphicFlare)
					{
						_E015(renderTexture4, (RenderTexture)flareRT, m_StarFlareIntensity, useTriangleBlit ? _E000() : null);
					}
					else
					{
						flareRT = RenderTexture.GetTemporary(source.width, source.height, 0, _E03D);
						_E016(renderTexture4, (RenderTexture)flareRT, m_StarFlareIntensity);
					}
					this.m__E002.Add(renderTexture4);
				}
			}
			else if (m_UseLensFlare || m_UseAnamorphicFlare)
			{
				renderTexture4 = _E00B(_E03B, upsamplingCount, source.width, source.height, FlareStripeType.DiagonalUpright);
				if (renderTexture4 != null)
				{
					_E015(renderTexture4, (RenderTexture)flareRT, m_StarFlareIntensity, useTriangleBlit ? _E000() : null);
					this.m__E002.Add(renderTexture4);
					renderTexture4 = _E00B(_E03B, upsamplingCount, source.width, source.height, FlareStripeType.DiagonalUpleft);
					_E015(renderTexture4, (RenderTexture)flareRT, m_StarFlareIntensity, useTriangleBlit ? _E000() : null);
					this.m__E002.Add(renderTexture4);
				}
			}
			else
			{
				renderTexture4 = _E00B(_E03B, upsamplingCount, source.width, source.height, FlareStripeType.DiagonalUpleft);
				if (renderTexture4 != null)
				{
					RenderTexture renderTexture5 = _E00B(_E03B, upsamplingCount, source.width, source.height, FlareStripeType.DiagonalUpright);
					_E017(renderTexture5, renderTexture4, m_StarFlareIntensity, m_StarFlareIntensity);
					this.m__E002.Add(renderTexture5);
					flareRT = renderTexture4;
				}
			}
		}
		if (m_DirectDownSample)
		{
			for (int i = 0; i < _E03B.Length; i++)
			{
				if (_E03E[i])
				{
					this.m__E002.Add(_E03B[i]);
				}
			}
		}
		else
		{
			for (int j = 0; j < _E03B.Length; j++)
			{
				this.m__E002.Add(_E03B[j]);
			}
		}
		MaterialPropertyBlock materialPropertyBlock = null;
		if (useTriangleBlit)
		{
			materialPropertyBlock = _E000();
			materialPropertyBlock.SetFloat(_E01D, m_BloomIntensity);
			materialPropertyBlock.SetFloat(_E01E, m_FlareIntensity);
			materialPropertyBlock.SetTexture(_E01F, source);
			materialPropertyBlock.SetTexture(_E020, flareRT);
			materialPropertyBlock.SetTexture(_E021, m_UseLensDust ? m_DustTexture : Texture2D.whiteTexture);
			materialPropertyBlock.SetTexture(_E022, temporary);
			if (m_UseLensDust)
			{
				materialPropertyBlock.SetFloat(_E023, m_DustIntensity);
				materialPropertyBlock.SetFloat(_E024, m_DirtLightIntensity);
			}
			else
			{
				materialPropertyBlock.SetFloat(_E023, 1f);
				materialPropertyBlock.SetFloat(_E024, 0f);
			}
			if (m_BlendMode == BlendMode.SCREEN)
			{
				materialPropertyBlock.SetFloat(_E025, m_ScreenMaxIntensity);
			}
		}
		else
		{
			this.m__E00F.SetFloat(_E01D, m_BloomIntensity);
			this.m__E00F.SetFloat(_E01E, m_FlareIntensity);
			this.m__E00F.SetTexture(_E01F, source);
			this.m__E00F.SetTexture(_E020, flareRT);
			this.m__E00F.SetTexture(_E021, m_UseLensDust ? m_DustTexture : Texture2D.whiteTexture);
			this.m__E00F.SetTexture(_E022, temporary);
			if (m_UseLensDust)
			{
				this.m__E00F.SetFloat(_E023, m_DustIntensity);
				this.m__E00F.SetFloat(_E024, m_DirtLightIntensity);
			}
			else
			{
				this.m__E00F.SetFloat(_E023, 1f);
				this.m__E00F.SetFloat(_E024, 0f);
			}
			if (m_BlendMode == BlendMode.SCREEN)
			{
				this.m__E00F.SetFloat(_E025, m_ScreenMaxIntensity);
			}
		}
		if (useTriangleBlit)
		{
			if (m_InvertImage)
			{
				this.m__E001.BlitFullscreenTriangle(_E03F, destination, this.m__E00F, 1, materialPropertyBlock);
			}
			else
			{
				this.m__E001.BlitFullscreenTriangle(_E03F, destination, this.m__E00F, 0, materialPropertyBlock);
			}
		}
		else if (m_InvertImage)
		{
			DebugGraphics.Blit(_E03F, destination, this.m__E00F, 1);
		}
		else
		{
			DebugGraphics.Blit(_E03F, destination, this.m__E00F, 0);
		}
		for (int k = 0; k < _E03C.Length; k++)
		{
			if (_E03C[k] != null)
			{
				this.m__E002.Add(_E03C[k]);
			}
		}
		if (useTriangleBlit)
		{
			Graphics.ExecuteCommandBuffer(this.m__E001);
		}
		if (flag2)
		{
			Graphics.Blit(renderTexture, destination);
		}
		if ((m_UseLensFlare || m_UseAnamorphicFlare || m_UseStarFlare) && flareRT != null && flareRT is RenderTexture)
		{
			this.m__E002.Add((RenderTexture)flareRT);
		}
		this.m__E002.Add(temporary);
		if (m_FlareShape != null && m_UseBokehFlare && renderTexture != null)
		{
			this.m__E002.Add(renderTexture);
		}
		if (!m_UseLensFlare && this.m__E005 != null)
		{
			this.m__E005.Clear(ref this.m__E006);
		}
		foreach (RenderTexture item in this.m__E002)
		{
			RenderTexture.ReleaseTemporary(item);
		}
		this.m__E002.Clear();
		_E001();
	}

	private RenderTexture _E009(RenderTexture[] sources, BlurSampleCount upsamplingCount, int sourceWidth, int sourceHeight)
	{
		for (int num = this.m__E00A.Length - 1; num >= 0; num--)
		{
			this.m__E00A[num] = RenderTexture.GetTemporary(sourceWidth / (int)Mathf.Pow(2f, num), sourceHeight / (int)Mathf.Pow(2f, num), 0, _E03D);
			this.m__E00A[num].filterMode = FilterMode.Bilinear;
			float num2 = 1f / (float)sources[num].width;
			float num3 = 1f / (float)sources[num].height;
			if (num < m_StarDownscaleCount - 1)
			{
				_E013(sources[num], this.m__E00A[num], num2 * m_StarScale, num3 * m_StarScale, this.m__E00A[num + 1], upsamplingCount, Color.white, 1f);
			}
			else
			{
				_E013(sources[num], this.m__E00A[num], num2 * m_StarScale, num3 * m_StarScale, null, upsamplingCount, Color.white, 1f);
			}
		}
		for (int i = 1; i < this.m__E00A.Length; i++)
		{
			if (this.m__E00A[i] != null)
			{
				this.m__E002.Add(this.m__E00A[i]);
			}
		}
		return this.m__E00A[0];
	}

	private void _E00A(Texture source, RenderTexture destination)
	{
		if (useTriangleBlit)
		{
			this.m__E001.BlitFullscreenTriangle(source, destination);
		}
		else
		{
			Graphics.Blit(source, destination);
		}
	}

	private RenderTexture _E00B(RenderTexture[] sources, BlurSampleCount upsamplingCount, int sourceWidth, int sourceHeight, FlareStripeType type)
	{
		RenderTexture[] array = this.m__E008;
		bool[] array2 = m_AnamorphicBloomUsages;
		float[] array3 = m_AnamorphicBloomIntensities;
		Color[] array4 = m_AnamorphicBloomColors;
		bool flag = m_AnamorphicSmallVerticalBlur;
		float num = m_AnamorphicBlurPass;
		float num2 = m_AnamorphicScale;
		float num3 = m_AnamorphicFlareIntensity;
		float num4 = 1f;
		float num5 = 0f;
		if (m_AnamorphicDirection == AnamorphicDirection.Vertical)
		{
			num4 = 0f;
			num5 = 1f;
		}
		if (type != 0)
		{
			array = this.m__E00A;
			array2 = m_StarBloomUsages;
			array3 = m_StarBloomIntensities;
			array4 = m_StarBloomColors;
			flag = false;
			num = m_StarBlurPass;
			num2 = m_StarScale;
			num3 = m_StarFlareIntensity;
			num5 = ((type != FlareStripeType.DiagonalUpleft) ? 1f : (-1f));
		}
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = null;
		}
		RenderTexture renderTexture = null;
		for (int num6 = array.Length - 1; num6 >= 0; num6--)
		{
			if ((!(sources[num6] == null) || !m_DirectUpsample) && (array2[num6] || !m_DirectUpsample))
			{
				array[num6] = RenderTexture.GetTemporary(sourceWidth / (int)Mathf.Pow(2f, num6), sourceHeight / (int)Mathf.Pow(2f, num6), 0, _E03D);
				array[num6].filterMode = FilterMode.Bilinear;
				float num7 = 1f / (float)array[num6].width;
				float num8 = 1f / (float)array[num6].height;
				RenderTexture source = sources[num6];
				RenderTexture renderTexture2 = array[num6];
				if (!array2[num6])
				{
					if (renderTexture != null)
					{
						if (flag)
						{
							_E012(renderTexture, renderTexture2, (m_AnamorphicDirection == AnamorphicDirection.Vertical) ? num7 : 0f, (m_AnamorphicDirection == AnamorphicDirection.Horizontal) ? num8 : 0f, null, BlurSampleCount.FourSimple, Color.white, 1f);
						}
						else
						{
							_E00A(renderTexture, renderTexture2);
						}
					}
					else
					{
						_E00A(Texture2D.blackTexture, renderTexture2);
					}
					renderTexture = array[num6];
				}
				else
				{
					RenderTexture renderTexture3 = null;
					if (flag && renderTexture != null)
					{
						renderTexture3 = RenderTexture.GetTemporary(renderTexture2.width, renderTexture2.height, 0, _E03D);
						_E012(renderTexture, renderTexture3, (m_AnamorphicDirection == AnamorphicDirection.Vertical) ? num7 : 0f, (m_AnamorphicDirection == AnamorphicDirection.Horizontal) ? num8 : 0f, null, BlurSampleCount.FourSimple, Color.white, 1f);
						renderTexture = renderTexture3;
					}
					if (num == 1f)
					{
						if (type != 0)
						{
							_E013(source, renderTexture2, num7 * num2 * num4, num8 * num2 * num5, renderTexture, upsamplingCount, array4[num6], array3[num6] * num3);
						}
						else
						{
							_E012(source, renderTexture2, num7 * num2 * num4, num8 * num2 * num5, renderTexture, upsamplingCount, array4[num6], array3[num6] * num3);
						}
					}
					else
					{
						RenderTexture temporary = RenderTexture.GetTemporary(renderTexture2.width, renderTexture2.height, 0, _E03D);
						bool flag2 = false;
						for (int j = 0; (float)j < num; j++)
						{
							RenderTexture additiveTexture = (((float)j == num - 1f) ? renderTexture : null);
							if (j == 0)
							{
								if (type != 0)
								{
									_E013(source, temporary, num7 * num2 * num4, num8 * num2 * num5, additiveTexture, upsamplingCount, array4[num6], array3[num6] * num3);
								}
								else
								{
									_E012(source, temporary, num7 * num2 * num4, num8 * num2 * num5, additiveTexture, upsamplingCount, array4[num6], array3[num6] * num3);
								}
								continue;
							}
							num7 = 1f / (float)renderTexture2.width;
							num8 = 1f / (float)renderTexture2.height;
							if (j % 2 == 1)
							{
								if (type != 0)
								{
									_E013(temporary, renderTexture2, num7 * num2 * num4 * 1.5f, num8 * num2 * num5 * 1.5f, additiveTexture, upsamplingCount, array4[num6], array3[num6] * num3);
								}
								else
								{
									_E012(temporary, renderTexture2, num7 * num2 * num4 * 1.5f, num8 * num2 * num5 * 1.5f, additiveTexture, upsamplingCount, array4[num6], array3[num6] * num3);
								}
								flag2 = false;
							}
							else
							{
								if (type != 0)
								{
									_E013(renderTexture2, temporary, num7 * num2 * num4 * 1.5f, num8 * num2 * num5 * 1.5f, additiveTexture, upsamplingCount, array4[num6], array3[num6] * num3);
								}
								else
								{
									_E012(renderTexture2, temporary, num7 * num2 * num4 * 1.5f, num8 * num2 * num5 * 1.5f, additiveTexture, upsamplingCount, array4[num6], array3[num6] * num3);
								}
								flag2 = true;
							}
						}
						if (flag2)
						{
							_E00A(temporary, renderTexture2);
						}
						if (renderTexture3 != null)
						{
							this.m__E002.Add(renderTexture3);
						}
						this.m__E002.Add(temporary);
					}
					renderTexture = array[num6];
				}
			}
		}
		RenderTexture renderTexture4 = null;
		for (int k = 0; k < array.Length; k++)
		{
			if (array[k] != null)
			{
				if (renderTexture4 == null)
				{
					renderTexture4 = array[k];
				}
				else
				{
					this.m__E002.Add(array[k]);
				}
			}
		}
		return renderTexture4;
	}

	private void _E00C(RenderTexture brightTexture, RenderTexture source, ref Texture flareRT)
	{
		flareRT = RenderTexture.GetTemporary(source.width, source.height, 0, _E03D);
		flareRT.filterMode = FilterMode.Bilinear;
		MaterialPropertyBlock materialPropertyBlock = null;
		if (useTriangleBlit)
		{
			materialPropertyBlock = _E000();
			materialPropertyBlock.Clear();
			materialPropertyBlock.SetVector(_E026, m_FlareScales * m_FlareGlobalScale);
			materialPropertyBlock.SetVector(_E027, m_FlareScalesNear * m_FlareGlobalScale);
			materialPropertyBlock.SetVector(_E028, m_FlareTint0);
			materialPropertyBlock.SetVector(_E029, m_FlareTint1);
			materialPropertyBlock.SetVector(_E02A, m_FlareTint2);
			materialPropertyBlock.SetVector(_E02B, m_FlareTint3);
			materialPropertyBlock.SetVector(_E02C, m_FlareTint4);
			materialPropertyBlock.SetVector(_E02D, m_FlareTint5);
			materialPropertyBlock.SetVector(_E02E, m_FlareTint6);
			materialPropertyBlock.SetVector(_E02F, m_FlareTint7);
			materialPropertyBlock.SetFloat(_E01D, m_FlareIntensity);
		}
		else
		{
			this.m__E00B.SetVector(_E026, m_FlareScales * m_FlareGlobalScale);
			this.m__E00B.SetVector(_E027, m_FlareScalesNear * m_FlareGlobalScale);
			this.m__E00B.SetVector(_E028, m_FlareTint0);
			this.m__E00B.SetVector(_E029, m_FlareTint1);
			this.m__E00B.SetVector(_E02A, m_FlareTint2);
			this.m__E00B.SetVector(_E02B, m_FlareTint3);
			this.m__E00B.SetVector(_E02C, m_FlareTint4);
			this.m__E00B.SetVector(_E02D, m_FlareTint5);
			this.m__E00B.SetVector(_E02E, m_FlareTint6);
			this.m__E00B.SetVector(_E02F, m_FlareTint7);
			this.m__E00B.SetFloat(_E01D, m_FlareIntensity);
		}
		if (m_FlareRendering == FlareRendering.Sharp)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(source.width / 2, source.height / 2, 0, _E03D);
			temporary.filterMode = FilterMode.Bilinear;
			_E011(brightTexture, temporary, 1f / (float)brightTexture.width, 1f / (float)brightTexture.height, SimpleSampleCount.Four, useTriangleBlit ? materialPropertyBlock : null);
			if (useTriangleBlit)
			{
				this.m__E001.BlitFullscreenTriangle(temporary, (RenderTexture)flareRT, this.m__E015, 0, materialPropertyBlock);
			}
			else
			{
				DebugGraphics.Blit(temporary, (RenderTexture)flareRT, this.m__E00B, 0);
			}
			this.m__E002.Add(temporary);
		}
		else if (m_FlareBlurQuality == FlareBlurQuality.Fast)
		{
			RenderTexture temporary2 = RenderTexture.GetTemporary(brightTexture.width / 2, brightTexture.height / 2, 0, _E03D);
			temporary2.filterMode = FilterMode.Bilinear;
			RenderTexture temporary3 = RenderTexture.GetTemporary(brightTexture.width / 4, brightTexture.height / 4, 0, _E03D);
			temporary3.filterMode = FilterMode.Bilinear;
			DebugGraphics.Blit(brightTexture, temporary2, this.m__E00B, 0);
			if (m_FlareRendering == FlareRendering.Blurred)
			{
				_E014(temporary2, temporary3, 1f / (float)temporary2.width, 1f / (float)temporary2.height, null, BlurSampleCount.Thirteen, Color.white, 1f, useTriangleBlit ? _E000() : null);
				_E011(temporary3, (RenderTexture)flareRT, 1f / (float)temporary3.width, 1f / (float)temporary3.height, SimpleSampleCount.Four, useTriangleBlit ? _E000() : null);
			}
			else if (m_FlareRendering == FlareRendering.MoreBlurred)
			{
				_E014(temporary2, temporary3, 1f / (float)temporary2.width, 1f / (float)temporary2.height, null, BlurSampleCount.ThrirtyOne, Color.white, 1f, useTriangleBlit ? _E000() : null);
				_E011(temporary3, (RenderTexture)flareRT, 1f / (float)temporary3.width, 1f / (float)temporary3.height, SimpleSampleCount.Four, useTriangleBlit ? _E000() : null);
			}
			this.m__E002.Add(temporary2);
			this.m__E002.Add(temporary3);
		}
		else if (m_FlareBlurQuality == FlareBlurQuality.Normal)
		{
			RenderTexture temporary4 = RenderTexture.GetTemporary(brightTexture.width / 2, brightTexture.height / 2, 0, _E03D);
			temporary4.filterMode = FilterMode.Bilinear;
			RenderTexture temporary5 = RenderTexture.GetTemporary(brightTexture.width / 4, brightTexture.height / 4, 0, _E03D);
			temporary5.filterMode = FilterMode.Bilinear;
			RenderTexture temporary6 = RenderTexture.GetTemporary(brightTexture.width / 4, brightTexture.height / 4, 0, _E03D);
			temporary6.filterMode = FilterMode.Bilinear;
			_E011(brightTexture, temporary4, 1f / (float)brightTexture.width, 1f / (float)brightTexture.height, SimpleSampleCount.Four, useTriangleBlit ? _E000() : null);
			_E011(temporary4, temporary5, 1f / (float)temporary4.width, 1f / (float)temporary4.height, SimpleSampleCount.Four, useTriangleBlit ? _E000() : null);
			DebugGraphics.Blit(temporary5, temporary6, this.m__E00B, 0);
			if (m_FlareRendering == FlareRendering.Blurred)
			{
				_E014(temporary6, temporary5, 1f / (float)temporary5.width, 1f / (float)temporary5.height, null, BlurSampleCount.Thirteen, Color.white, 1f, useTriangleBlit ? _E000() : null);
				_E011(temporary5, (RenderTexture)flareRT, 1f / (float)temporary5.width, 1f / (float)temporary5.height, SimpleSampleCount.Four, useTriangleBlit ? _E000() : null);
			}
			else if (m_FlareRendering == FlareRendering.MoreBlurred)
			{
				_E014(temporary6, temporary5, 1f / (float)temporary5.width, 1f / (float)temporary5.height, null, BlurSampleCount.ThrirtyOne, Color.white, 1f, useTriangleBlit ? _E000() : null);
				_E011(temporary5, (RenderTexture)flareRT, 1f / (float)temporary5.width, 1f / (float)temporary5.height, SimpleSampleCount.Four, useTriangleBlit ? _E000() : null);
			}
			this.m__E002.Add(temporary4);
			this.m__E002.Add(temporary5);
			this.m__E002.Add(temporary6);
		}
		else if (m_FlareBlurQuality == FlareBlurQuality.High)
		{
			RenderTexture temporary7 = RenderTexture.GetTemporary(brightTexture.width / 2, brightTexture.height / 2, 0, _E03D);
			temporary7.filterMode = FilterMode.Bilinear;
			RenderTexture temporary8 = RenderTexture.GetTemporary(temporary7.width / 2, temporary7.height / 2, 0, _E03D);
			temporary8.filterMode = FilterMode.Bilinear;
			RenderTexture temporary9 = RenderTexture.GetTemporary(temporary8.width / 2, temporary8.height / 2, 0, _E03D);
			temporary9.filterMode = FilterMode.Bilinear;
			RenderTexture temporary10 = RenderTexture.GetTemporary(temporary8.width / 2, temporary8.height / 2, 0, _E03D);
			temporary10.filterMode = FilterMode.Bilinear;
			_E011(brightTexture, temporary7, 1f / (float)brightTexture.width, 1f / (float)brightTexture.height, SimpleSampleCount.Four, useTriangleBlit ? _E000() : null);
			_E011(temporary7, temporary8, 1f / (float)temporary7.width, 1f / (float)temporary7.height, SimpleSampleCount.Four, useTriangleBlit ? _E000() : null);
			_E011(temporary8, temporary9, 1f / (float)temporary8.width, 1f / (float)temporary8.height, SimpleSampleCount.Four, useTriangleBlit ? _E000() : null);
			DebugGraphics.Blit(temporary9, temporary10, this.m__E00B, 0);
			if (m_FlareRendering == FlareRendering.Blurred)
			{
				_E014(temporary10, temporary9, 1f / (float)temporary9.width, 1f / (float)temporary9.height, null, BlurSampleCount.Thirteen, Color.white, 1f, useTriangleBlit ? _E000() : null);
				_E011(temporary9, (RenderTexture)flareRT, 1f / (float)temporary9.width, 1f / (float)temporary9.height, SimpleSampleCount.Four, useTriangleBlit ? _E000() : null);
			}
			else if (m_FlareRendering == FlareRendering.MoreBlurred)
			{
				_E014(temporary10, temporary9, 1f / (float)temporary9.width, 1f / (float)temporary9.height, null, BlurSampleCount.ThrirtyOne, Color.white, 1f, useTriangleBlit ? _E000() : null);
				_E011(temporary9, (RenderTexture)flareRT, 1f / (float)temporary9.width, 1f / (float)temporary9.height, SimpleSampleCount.Four, useTriangleBlit ? _E000() : null);
			}
			this.m__E002.Add(temporary7);
			this.m__E002.Add(temporary8);
			this.m__E002.Add(temporary9);
			this.m__E002.Add(temporary10);
		}
	}

	private void _E00D(RenderTexture[] sources, RenderTexture[] destinations, int originalWidth, int originalHeight, BlurSampleCount upsamplingCount)
	{
		RenderTexture renderTexture = null;
		for (int i = 0; i < _E03C.Length; i++)
		{
			_E03C[i] = null;
		}
		for (int num = destinations.Length - 1; num >= 0; num--)
		{
			if (m_BloomUsages[num] || !m_DirectUpsample)
			{
				_E03C[num] = RenderTexture.GetTemporary(originalWidth / (int)Mathf.Pow(2f, num), originalHeight / (int)Mathf.Pow(2f, num), 0, _E03D);
				_E03C[num].filterMode = FilterMode.Bilinear;
			}
			float num2 = 1f;
			if (m_BloomUsages[num])
			{
				float num3 = 1f / (float)sources[num].width;
				float verticalBlur = 1f / (float)sources[num].height;
				_E014(_E03B[num], _E03C[num], num3 * num2, verticalBlur, renderTexture, upsamplingCount, m_BloomColors[num], m_BloomIntensities[num], useTriangleBlit ? _E000() : null);
			}
			else if (num < m_DownscaleCount - 1)
			{
				if (!m_DirectUpsample)
				{
					_E011(renderTexture, _E03C[num], 1f / (float)_E03C[num].width, 1f / (float)_E03C[num].height, SimpleSampleCount.Four, useTriangleBlit ? _E000() : null);
				}
			}
			else if (useTriangleBlit)
			{
				this.m__E001.BlitFullscreenTriangle(Texture2D.blackTexture, _E03C[num]);
			}
			else
			{
				Graphics.Blit(Texture2D.blackTexture, _E03C[num]);
			}
			if (m_BloomUsages[num] || !m_DirectUpsample)
			{
				renderTexture = _E03C[num];
			}
		}
		_E03F = renderTexture;
	}

	private void _E00E(RenderTexture source, RenderTexture[] destinations, DeluxeFilmicCurve intensityCurve, bool hdr)
	{
		int num = destinations.Length;
		RenderTexture renderTexture = source;
		bool flag = false;
		for (int i = 0; i < num; i++)
		{
			if (m_DirectDownSample && !_E03E[i])
			{
				continue;
			}
			destinations[i] = RenderTexture.GetTemporary(source.width / (int)Mathf.Pow(2f, i + 1), source.height / (int)Mathf.Pow(2f, i + 1), 0, _E03D);
			destinations[i].filterMode = FilterMode.Bilinear;
			RenderTexture destination = destinations[i];
			float num2 = 1f;
			float num3 = 1f / (float)renderTexture.width;
			float num4 = 1f / (float)renderTexture.height;
			MaterialPropertyBlock materialPropertyBlock = _E000();
			if (intensityCurve != null && !flag)
			{
				intensityCurve.StoreK();
				if (useTriangleBlit)
				{
					materialPropertyBlock.SetFloat(_E030, intensityCurve.GetExposure());
					materialPropertyBlock.SetFloat(_E031, intensityCurve.m_k);
					materialPropertyBlock.SetFloat(_E032, intensityCurve.m_CrossOverPoint);
					materialPropertyBlock.SetVector(_E033, intensityCurve.m_ToeCoef);
					materialPropertyBlock.SetVector(_E034, intensityCurve.m_ShoulderCoef);
				}
				else
				{
					this.m__E00D.SetFloat(_E030, intensityCurve.GetExposure());
					this.m__E00D.SetFloat(_E031, intensityCurve.m_k);
					this.m__E00D.SetFloat(_E032, intensityCurve.m_CrossOverPoint);
					this.m__E00D.SetVector(_E033, intensityCurve.m_ToeCoef);
					this.m__E00D.SetVector(_E034, intensityCurve.m_ShoulderCoef);
				}
				float value = (hdr ? 2f : 1f);
				if (useTriangleBlit)
				{
					materialPropertyBlock.SetFloat(_E035, value);
				}
				else
				{
					this.m__E00D.SetFloat(_E035, value);
				}
				num3 = 1f / (float)renderTexture.width;
				num4 = 1f / (float)renderTexture.height;
				if (m_TemporalStableDownsampling)
				{
					_E011(renderTexture, destination, num3 * num2, num4 * num2, SimpleSampleCount.ThirteenTemporalCurve, useTriangleBlit ? materialPropertyBlock : null);
				}
				else
				{
					_E011(renderTexture, destination, num3 * num2, num4 * num2, SimpleSampleCount.FourCurve, useTriangleBlit ? materialPropertyBlock : null);
				}
				flag = true;
			}
			else if (m_TemporalStableDownsampling)
			{
				_E011(renderTexture, destination, num3 * num2, num4 * num2, SimpleSampleCount.ThirteenTemporal, useTriangleBlit ? materialPropertyBlock : null);
			}
			else
			{
				_E011(renderTexture, destination, num3 * num2, num4 * num2, SimpleSampleCount.Four, useTriangleBlit ? materialPropertyBlock : null);
			}
			renderTexture = destinations[i];
		}
	}

	private void _E00F(RenderTexture source, RenderTexture destination, Vector4 treshold, MaterialPropertyBlock props)
	{
		if (props != null)
		{
			props.SetTexture(_E01C, Texture2D.whiteTexture);
			props.SetVector(_E036, treshold);
		}
		else
		{
			this.m__E011.SetTexture(_E01C, Texture2D.whiteTexture);
			this.m__E011.SetVector(_E036, treshold);
		}
		if (useTriangleBlit)
		{
			this.m__E001.BlitFullscreenTriangle(source, destination, this.m__E011, 0, props);
		}
		else
		{
			DebugGraphics.Blit(source, destination, this.m__E011, 0);
		}
	}

	private void _E010(RenderTexture source, RenderTexture destination, Vector4 treshold, Texture mask, MaterialPropertyBlock props)
	{
		if (props != null)
		{
			props.SetTexture(_E01C, mask);
			props.SetVector(_E036, treshold);
		}
		else
		{
			this.m__E012.SetTexture(_E01C, mask);
			this.m__E012.SetVector(_E036, treshold);
		}
		if (useTriangleBlit)
		{
			this.m__E001.BlitFullscreenTriangle(source, destination, this.m__E012, 0, props);
		}
		else
		{
			DebugGraphics.Blit(source, destination, this.m__E012, 0);
		}
	}

	private void _E011(RenderTexture source, RenderTexture destination, float horizontalBlur, float verticalBlur, SimpleSampleCount sampleCount, MaterialPropertyBlock matBlock)
	{
		if (matBlock != null)
		{
			matBlock.SetVector(_E037, new Vector4(horizontalBlur, verticalBlur, 0f, 0f));
		}
		else
		{
			this.m__E00D.SetVector(_E037, new Vector4(horizontalBlur, verticalBlur, 0f, 0f));
		}
		if (matBlock == null)
		{
			switch (sampleCount)
			{
			case SimpleSampleCount.Four:
				DebugGraphics.Blit(source, destination, this.m__E00D, 0);
				break;
			case SimpleSampleCount.Nine:
				DebugGraphics.Blit(source, destination, this.m__E00D, 1);
				break;
			case SimpleSampleCount.FourCurve:
				DebugGraphics.Blit(source, destination, this.m__E00D, 5);
				break;
			case SimpleSampleCount.ThirteenTemporal:
				DebugGraphics.Blit(source, destination, this.m__E00D, 11);
				break;
			case SimpleSampleCount.ThirteenTemporalCurve:
				DebugGraphics.Blit(source, destination, this.m__E00D, 12);
				break;
			}
		}
		else
		{
			switch (sampleCount)
			{
			case SimpleSampleCount.Four:
				this.m__E001.BlitFullscreenTriangle(source, destination, this.m__E00D, 0, matBlock);
				break;
			case SimpleSampleCount.Nine:
				this.m__E001.BlitFullscreenTriangle(source, destination, this.m__E00D, 1, matBlock);
				break;
			case SimpleSampleCount.FourCurve:
				this.m__E001.BlitFullscreenTriangle(source, destination, this.m__E00D, 5, matBlock);
				break;
			case SimpleSampleCount.ThirteenTemporal:
				this.m__E001.BlitFullscreenTriangle(source, destination, this.m__E00D, 11, matBlock);
				break;
			case SimpleSampleCount.ThirteenTemporalCurve:
				this.m__E001.BlitFullscreenTriangle(source, destination, this.m__E00D, 12, matBlock);
				break;
			}
		}
	}

	private void _E012(RenderTexture source, RenderTexture destination, float horizontalBlur, float verticalBlur, RenderTexture additiveTexture, BlurSampleCount sampleCount, Color tint, float intensity)
	{
		int num = 2;
		if (sampleCount == BlurSampleCount.Seventeen)
		{
			num = 3;
		}
		if (sampleCount == BlurSampleCount.Nine)
		{
			num = 4;
		}
		if (sampleCount == BlurSampleCount.NineCurve)
		{
			num = 6;
		}
		if (sampleCount == BlurSampleCount.FourSimple)
		{
			num = 7;
		}
		if (sampleCount == BlurSampleCount.Thirteen)
		{
			num = 8;
		}
		if (sampleCount == BlurSampleCount.TwentyThree)
		{
			num = 9;
		}
		if (sampleCount == BlurSampleCount.TwentySeven)
		{
			num = 10;
		}
		Texture texture = null;
		texture = ((!(additiveTexture == null)) ? ((Texture)additiveTexture) : ((Texture)Texture2D.blackTexture));
		if (useTriangleBlit)
		{
			MaterialPropertyBlock materialPropertyBlock = _E000();
			materialPropertyBlock.SetTexture(_E021, texture);
			materialPropertyBlock.SetVector(_E037, new Vector4(horizontalBlur, verticalBlur, 0f, 0f));
			materialPropertyBlock.SetVector(_E038, tint);
			materialPropertyBlock.SetFloat(_E01D, intensity);
			this.m__E001.BlitFullscreenTriangle(source, destination, this.m__E00D, num, materialPropertyBlock);
		}
		else
		{
			this.m__E00D.SetTexture(_E021, texture);
			this.m__E00D.SetVector(_E037, new Vector4(horizontalBlur, verticalBlur, 0f, 0f));
			this.m__E00D.SetVector(_E038, tint);
			this.m__E00D.SetFloat(_E01D, intensity);
			DebugGraphics.Blit(source, destination, this.m__E00D, num);
		}
	}

	private void _E013(RenderTexture source, RenderTexture destination, float horizontalBlur, float verticalBlur, RenderTexture additiveTexture, BlurSampleCount sampleCount, Color tint, float intensity)
	{
		RenderTexture temporary = RenderTexture.GetTemporary(destination.width, destination.height, destination.depth, destination.format);
		temporary.filterMode = FilterMode.Bilinear;
		int num = 2;
		if (sampleCount == BlurSampleCount.Seventeen)
		{
			num = 3;
		}
		if (sampleCount == BlurSampleCount.Nine)
		{
			num = 4;
		}
		if (sampleCount == BlurSampleCount.NineCurve)
		{
			num = 6;
		}
		if (sampleCount == BlurSampleCount.FourSimple)
		{
			num = 7;
		}
		if (sampleCount == BlurSampleCount.Thirteen)
		{
			num = 8;
		}
		if (sampleCount == BlurSampleCount.TwentyThree)
		{
			num = 9;
		}
		if (sampleCount == BlurSampleCount.TwentySeven)
		{
			num = 10;
		}
		Texture texture = null;
		texture = ((!(additiveTexture == null)) ? ((Texture)additiveTexture) : ((Texture)Texture2D.blackTexture));
		if (useTriangleBlit)
		{
			MaterialPropertyBlock materialPropertyBlock = _E000();
			materialPropertyBlock.SetTexture(_E021, texture);
			materialPropertyBlock.SetVector(_E037, new Vector4(horizontalBlur, verticalBlur, 0f, 0f));
			materialPropertyBlock.SetVector(_E038, tint);
			materialPropertyBlock.SetFloat(_E01D, intensity);
			this.m__E001.BlitFullscreenTriangle(source, temporary, this.m__E00D, num, materialPropertyBlock);
		}
		else
		{
			this.m__E00D.SetTexture(_E021, texture);
			this.m__E00D.SetVector(_E037, new Vector4(horizontalBlur, verticalBlur, 0f, 0f));
			this.m__E00D.SetVector(_E038, tint);
			this.m__E00D.SetFloat(_E01D, intensity);
			DebugGraphics.Blit(source, temporary, this.m__E00D, num);
		}
		texture = temporary;
		if (useTriangleBlit)
		{
			MaterialPropertyBlock materialPropertyBlock2 = _E000();
			materialPropertyBlock2.SetTexture(_E021, texture);
			materialPropertyBlock2.SetVector(_E037, new Vector4(0f - horizontalBlur, verticalBlur, 0f, 0f));
			materialPropertyBlock2.SetVector(_E038, tint);
			materialPropertyBlock2.SetFloat(_E01D, intensity);
			this.m__E001.BlitFullscreenTriangle(source, destination, this.m__E00D, num, materialPropertyBlock2);
		}
		else
		{
			this.m__E00D.SetTexture(_E021, texture);
			this.m__E00D.SetVector(_E037, new Vector4(0f - horizontalBlur, verticalBlur, 0f, 0f));
			this.m__E00D.SetVector(_E038, tint);
			this.m__E00D.SetFloat(_E01D, intensity);
			DebugGraphics.Blit(source, destination, this.m__E00D, num);
		}
		this.m__E002.Add(temporary);
	}

	private void _E014(RenderTexture source, RenderTexture destination, float horizontalBlur, float verticalBlur, RenderTexture additiveTexture, BlurSampleCount sampleCount, Color tint, float intensity, MaterialPropertyBlock props)
	{
		RenderTexture temporary = RenderTexture.GetTemporary(destination.width, destination.height, destination.depth, destination.format);
		temporary.filterMode = FilterMode.Bilinear;
		int num = 2;
		if (sampleCount == BlurSampleCount.Seventeen)
		{
			num = 3;
		}
		if (sampleCount == BlurSampleCount.Nine)
		{
			num = 4;
		}
		if (sampleCount == BlurSampleCount.NineCurve)
		{
			num = 6;
		}
		if (sampleCount == BlurSampleCount.FourSimple)
		{
			num = 7;
		}
		if (sampleCount == BlurSampleCount.Thirteen)
		{
			num = 8;
		}
		if (sampleCount == BlurSampleCount.TwentyThree)
		{
			num = 9;
		}
		if (sampleCount == BlurSampleCount.TwentySeven)
		{
			num = 10;
		}
		if (props != null)
		{
			props.SetTexture(_E021, Texture2D.blackTexture);
			props.SetVector(_E037, new Vector4(0f, verticalBlur, 0f, 0f));
			props.SetVector(_E038, tint);
			props.SetFloat(_E01D, intensity);
			this.m__E001.BlitFullscreenTriangle(source, temporary, this.m__E00D, num, props);
		}
		else
		{
			this.m__E00D.SetTexture(_E021, Texture2D.blackTexture);
			this.m__E00D.SetVector(_E037, new Vector4(0f, verticalBlur, 0f, 0f));
			this.m__E00D.SetVector(_E038, tint);
			this.m__E00D.SetFloat(_E01D, intensity);
			DebugGraphics.Blit(source, temporary, this.m__E00D, num);
		}
		Texture texture = null;
		texture = ((!(additiveTexture == null)) ? ((Texture)additiveTexture) : ((Texture)Texture2D.blackTexture));
		if (props != null)
		{
			props.SetTexture(_E021, texture);
			props.SetVector(_E037, new Vector4(horizontalBlur, 0f, 1f / (float)destination.width, 1f / (float)destination.height));
			props.SetVector(_E038, Color.white);
			props.SetFloat(_E01D, 1f);
			this.m__E001.BlitFullscreenTriangle(temporary, destination, this.m__E00D, num, props);
		}
		else
		{
			this.m__E00D.SetTexture(_E021, texture);
			this.m__E00D.SetVector(_E037, new Vector4(horizontalBlur, 0f, 1f / (float)destination.width, 1f / (float)destination.height));
			this.m__E00D.SetVector(_E038, Color.white);
			this.m__E00D.SetFloat(_E01D, 1f);
			DebugGraphics.Blit(temporary, destination, this.m__E00D, num);
		}
		this.m__E002.Add(temporary);
	}

	private void _E015(RenderTexture source, RenderTexture destination, float intensity, MaterialPropertyBlock prop)
	{
		RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, source.depth, source.format);
		if (useTriangleBlit)
		{
			this.m__E001.BlitFullscreenTriangle(destination, temporary);
		}
		else
		{
			Graphics.Blit(destination, temporary);
		}
		if (prop != null)
		{
			prop.SetTexture(_E01F, temporary);
			prop.SetFloat(_E01D, intensity);
		}
		else
		{
			this.m__E017.SetTexture(_E01F, temporary);
			this.m__E017.SetFloat(_E01D, intensity);
		}
		if (useTriangleBlit)
		{
			this.m__E001.BlitFullscreenTriangle(source, destination, this.m__E017, 0, prop);
		}
		else
		{
			DebugGraphics.Blit(source, destination, this.m__E017, 0);
		}
		this.m__E002.Add(temporary);
	}

	private void _E016(RenderTexture source, RenderTexture destination, float intensity)
	{
		this.m__E017.SetFloat(_E01D, intensity);
		DebugGraphics.Blit(source, destination, this.m__E017, 2);
	}

	private void _E017(RenderTexture source, RenderTexture destination, float intensitySource, float intensityDestination)
	{
		RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, source.depth, source.format);
		_E00A(destination, temporary);
		if (useTriangleBlit)
		{
			MaterialPropertyBlock materialPropertyBlock = _E000();
			materialPropertyBlock.SetTexture(_E01F, temporary);
			materialPropertyBlock.SetFloat(_E039, intensitySource);
			materialPropertyBlock.SetFloat(_E03A, intensityDestination);
			this.m__E001.BlitFullscreenTriangle(source, destination, this.m__E017, 1, materialPropertyBlock);
		}
		else
		{
			this.m__E017.SetTexture(_E01F, temporary);
			this.m__E017.SetFloat(_E039, intensitySource);
			this.m__E017.SetFloat(_E03A, intensityDestination);
			DebugGraphics.Blit(source, destination, this.m__E017, 1);
		}
		this.m__E002.Add(temporary);
	}

	public void SetFilmicCurveParameters(float middle, float dark, float bright, float highlights)
	{
		m_BloomCurve.m_ToeStrength = -1f * dark;
		m_BloomCurve.m_ShoulderStrength = bright;
		m_BloomCurve.m_Highlights = highlights;
		m_BloomCurve.m_CrossOverPoint = middle;
		m_BloomCurve.UpdateCoefficients();
	}

	private void OnDestroy()
	{
		this.m__E001?.Dispose();
		this.m__E001 = null;
	}
}
