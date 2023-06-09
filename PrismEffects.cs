using System.Collections.Generic;
using EFT.BlitDebug;
using EFT.EnvironmentEffect;
using Prism.Utils;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[AddComponentMenu("PRISM/Prism Effects")]
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class PrismEffects : MonoBehaviour
{
	public PrismPreset currentPrismPreset;

	private bool m__E000;

	public bool isParentPrism;

	public bool isChildPrism;

	private RenderTexture m__E001;

	public Material m_Material;

	public Shader m_Shader;

	public Material m_Material2;

	public Shader m_Shader2;

	public PrismEffects m_MasterEffectExposure;

	private List<PrismEffects> m__E002;

	public Material m_AOMaterial;

	public Shader m_AOShader;

	public Material m_Material3;

	public Shader m_Shader3;

	private Camera m__E003;

	private SSAA m__E004;

	public bool doBigPass = true;

	public bool useSeparableBlur = true;

	public Texture2D lensDirtTexture;

	public bool useLensDirt = true;

	[Space(10f)]
	public bool useBloom;

	public bool bloomUseScreenBlend;

	public BloomType bloomType = BloomType.HDR;

	public bool debugBloomTex;

	[Range(1f, 12f)]
	public int bloomDownsample = 2;

	[Range(0f, 12f)]
	public int bloomBlurPasses = 4;

	public float bloomIntensity = 0.15f;

	[Range(-2f, 2f)]
	public float bloomThreshold = 0.01f;

	public float dirtIntensity = 1f;

	public bool useBloomStability = true;

	private RenderTexture m__E005;

	public bool useUIBlur;

	public int uiBlurGrabTextureFromPassNumber = 2;

	private RenderTexture m__E006;

	[Space(10f)]
	public bool useVignette;

	public float vignetteStart = 0.9f;

	public float vignetteEnd = 0.4f;

	public float vignetteStrength = 1f;

	public Color vignetteColor = Color.black;

	[Space(10f)]
	public bool useNightVision;

	[Tooltip("The main color of the NV effect")]
	[SerializeField]
	public Color m_NVColor = new Color(0f, 1f, 0.1724138f, 0f);

	[SerializeField]
	[Tooltip("The color that the NV effect will 'bleach' towards (white = default)")]
	public Color m_TargetBleachColor = new Color(1f, 1f, 1f, 0f);

	[Range(0f, 0.1f)]
	[Tooltip("How much base lighting does the NV effect pick up")]
	public float m_baseLightingContribution = 0.025f;

	[Range(0f, 128f)]
	[Tooltip("The higher this value, the more bright areas will get 'bleached out'")]
	public float m_LightSensitivityMultiplier = 100f;

	[Space(10f)]
	public bool useNoise;

	public Texture2D noiseTexture;

	public float noiseScale = 1f;

	public float noiseIntensity = 0.2f;

	public NoiseType noiseType = NoiseType.RandomTimeNoise;

	[Space(10f)]
	public bool useChromaticAberration;

	public AberrationType aberrationType = AberrationType.Vertical;

	[Range(0f, 1f)]
	public float chromaticDistanceOne = 0.29f;

	[Range(0f, 1f)]
	public float chromaticDistanceTwo = 0.599f;

	public float chromaticIntensity = 0.03f;

	public float chromaticBlurWidth = 1f;

	public bool useChromaticBlur;

	[Space(10f)]
	public bool useTonemap;

	public TonemapType tonemapType = TonemapType.RomB;

	public Vector3 toneValues = new Vector3(-1f, 2.72f, 0.15f);

	public Vector3 secondaryToneValues = new Vector3(0.59f, 0.14f, 0.14f);

	public bool useExposure;

	public bool debugViewExposure;

	private RenderTexture m__E007;

	public float exposureMiddleGrey = 0.12f;

	public float exposureLowerLimit = -6f;

	public float exposureUpperLimit = 6f;

	public float exposureSpeed = 6f;

	public int histWidth = 1;

	public int histHeight = 1;

	private bool m__E008 = true;

	public bool useGammaCorrection;

	public float gammaValue = 1f;

	[Space(10f)]
	public bool useDof;

	public bool dofForceEnableMedian;

	public bool useNearDofBlur;

	public bool useFullScreenBlur;

	public float dofNearFocusDistance = 15f;

	public float dofFocusPoint = 5f;

	public float dofFocusDistance = 15f;

	public float dofRadius = 0.6f;

	public float dofBokehFactor = 60f;

	public DoFSamples dofSampleAmount = DoFSamples.Medium;

	public bool dofBlurSkybox = true;

	public bool debugDofPass;

	public Transform dofFocusTransform;

	[Space(10f)]
	public bool useLut;

	public Texture2D twoDLookupTex;

	public Texture3D threeDLookupTex;

	public string basedOnTempTex = "";

	public float lutLerpAmount = 0.6f;

	public bool useSecondLut;

	public Texture2D secondaryTwoDLookupTex;

	public Texture3D secondaryThreeDLookupTex;

	public string secondaryBasedOnTempTex = "";

	public float secondaryLutLerpAmount;

	[Space(10f)]
	public bool useSharpen;

	public float sharpenAmount = 1f;

	public bool useFog;

	public bool fogAffectSkybox;

	public float fogIntensity;

	public float fogStartPoint = 50f;

	public float fogDistance = 170f;

	public Color fogColor = Color.white;

	public Color fogEndColor = Color.gray;

	public float fogHeight = 2f;

	public bool useAmbientObscurance;

	public SampleCount aoSampleCount = SampleCount.Low;

	public bool useAODistanceCutoff;

	public float aoDistanceCutoffLength = 50f;

	public float aoDistanceCutoffStart = 500f;

	public float aoIntensity = 0.7f;

	public float aoMinIntensity;

	public float aoRadius = 1f;

	public bool aoDownsample;

	public AOBlurType aoBlurType = AOBlurType.Fast;

	[Range(0f, 3f)]
	public int aoBlurIterations = 1;

	public float aoBias = 0.1f;

	public float aoBlurFilterDistance = 1.25f;

	public float aoLightingContribution = 1f;

	public bool aoShowDebug;

	public bool useRays;

	public Transform rayTransform;

	public float rayWeight = 0.58767f;

	public Color rayColor = Color.white;

	public Color rayThreshold = new Color(0.87f, 0.74f, 0.65f);

	public bool raysShowDebug;

	[Space(10f)]
	public bool advancedVignette;

	public bool advancedAO;

	private static readonly int m__E009 = Shader.PropertyToID(_ED3E._E000(32907));

	private static readonly int m__E00A = Shader.PropertyToID(_ED3E._E000(32955));

	private static readonly int m__E00B = Shader.PropertyToID(_ED3E._E000(32939));

	private static readonly int m__E00C = Shader.PropertyToID(_ED3E._E000(32986));

	private static readonly int _E00D = Shader.PropertyToID(_ED3E._E000(32968));

	private static readonly int _E00E = Shader.PropertyToID(_ED3E._E000(33023));

	private static readonly int _E00F = Shader.PropertyToID(_ED3E._E000(33004));

	private static readonly int _E010 = Shader.PropertyToID(_ED3E._E000(33055));

	private static readonly int _E011 = Shader.PropertyToID(_ED3E._E000(33038));

	private static readonly int _E012 = Shader.PropertyToID(_ED3E._E000(33031));

	private static readonly int _E013 = Shader.PropertyToID(_ED3E._E000(33073));

	private static readonly int _E014 = Shader.PropertyToID(_ED3E._E000(33115));

	private static readonly int _E015 = Shader.PropertyToID(_ED3E._E000(33151));

	private static readonly int _E016 = Shader.PropertyToID(_ED3E._E000(33135));

	private static readonly int _E017 = Shader.PropertyToID(_ED3E._E000(33121));

	private static readonly int _E018 = Shader.PropertyToID(_ED3E._E000(33173));

	private static readonly int _E019 = Shader.PropertyToID(_ED3E._E000(33153));

	private static readonly int _E01A = Shader.PropertyToID(_ED3E._E000(33202));

	private static readonly int _E01B = Shader.PropertyToID(_ED3E._E000(33190));

	private static readonly int _E01C = Shader.PropertyToID(_ED3E._E000(33235));

	private static readonly int _E01D = Shader.PropertyToID(_ED3E._E000(33226));

	private static readonly int _E01E = Shader.PropertyToID(_ED3E._E000(33273));

	private static readonly int _E01F = Shader.PropertyToID(_ED3E._E000(33266));

	private static readonly int _E020 = Shader.PropertyToID(_ED3E._E000(33310));

	private static readonly int _E021 = Shader.PropertyToID(_ED3E._E000(33290));

	private static readonly int _E022 = Shader.PropertyToID(_ED3E._E000(33334));

	private static readonly int _E023 = Shader.PropertyToID(_ED3E._E000(33317));

	private static readonly int _E024 = Shader.PropertyToID(_ED3E._E000(33374));

	private static readonly int _E025 = Shader.PropertyToID(_ED3E._E000(33357));

	private static readonly int _E026 = Shader.PropertyToID(_ED3E._E000(39289));

	private static readonly int _E027 = Shader.PropertyToID(_ED3E._E000(39278));

	private static readonly int _E028 = Shader.PropertyToID(_ED3E._E000(39316));

	private static readonly int _E029 = Shader.PropertyToID(_ED3E._E000(39307));

	private static readonly int _E02A = Shader.PropertyToID(_ED3E._E000(33344));

	private static readonly int _E02B = Shader.PropertyToID(_ED3E._E000(33402));

	private static readonly int _E02C = Shader.PropertyToID(_ED3E._E000(33384));

	private static readonly int _E02D = Shader.PropertyToID(_ED3E._E000(33437));

	private static readonly int _E02E = Shader.PropertyToID(_ED3E._E000(33424));

	private static readonly int _E02F = Shader.PropertyToID(_ED3E._E000(33413));

	private static readonly int _E030 = Shader.PropertyToID(_ED3E._E000(33471));

	private static readonly int _E031 = Shader.PropertyToID(_ED3E._E000(33457));

	private static readonly int _E032 = Shader.PropertyToID(_ED3E._E000(33446));

	private static readonly int _E033 = Shader.PropertyToID(_ED3E._E000(33493));

	private static readonly int _E034 = Shader.PropertyToID(_ED3E._E000(39354));

	private static readonly int _E035 = Shader.PropertyToID(_ED3E._E000(39340));

	private static readonly int _E036 = Shader.PropertyToID(_ED3E._E000(39332));

	private static readonly int _E037 = Shader.PropertyToID(_ED3E._E000(39387));

	private static readonly int _E038 = Shader.PropertyToID(_ED3E._E000(39373));

	private static readonly int _E039 = Shader.PropertyToID(_ED3E._E000(39420));

	private static readonly int _E03A = Shader.PropertyToID(_ED3E._E000(39407));

	private static readonly int _E03B = Shader.PropertyToID(_ED3E._E000(33482));

	private static readonly int _E03C = Shader.PropertyToID(_ED3E._E000(33532));

	private static readonly int _E03D = Shader.PropertyToID(_ED3E._E000(33527));

	private static readonly int _E03E = Shader.PropertyToID(_ED3E._E000(33519));

	private static readonly int _E03F = Shader.PropertyToID(_ED3E._E000(33506));

	private static readonly int _E040 = Shader.PropertyToID(_ED3E._E000(33554));

	private static readonly int _E041 = Shader.PropertyToID(_ED3E._E000(33539));

	private static readonly int _E042 = Shader.PropertyToID(_ED3E._E000(33585));

	private static readonly int _E043 = Shader.PropertyToID(_ED3E._E000(33570));

	private static readonly int _E044 = Shader.PropertyToID(_ED3E._E000(33617));

	private static readonly int _E045 = Shader.PropertyToID(_ED3E._E000(33603));

	private static readonly int _E046 = Shader.PropertyToID(_ED3E._E000(33654));

	private static readonly int _E047 = Shader.PropertyToID(_ED3E._E000(33641));

	private static readonly int _E048 = Shader.PropertyToID(_ED3E._E000(33689));

	private static readonly int _E049 = Shader.PropertyToID(_ED3E._E000(33671));

	private static readonly int _E04A = Shader.PropertyToID(_ED3E._E000(33718));

	private static readonly int _E04B = Shader.PropertyToID(_ED3E._E000(33697));

	private static readonly int _E04C = Shader.PropertyToID(_ED3E._E000(33749));

	private static readonly int _E04D = Shader.PropertyToID(_ED3E._E000(33790));

	private static readonly int _E04E = Shader.PropertyToID(_ED3E._E000(33780));

	private static readonly int _E04F = Shader.PropertyToID(_ED3E._E000(33768));

	private static readonly int _E050 = Shader.PropertyToID(_ED3E._E000(35865));

	private static readonly int _E051 = Shader.PropertyToID(_ED3E._E000(35851));

	private static readonly int _E052 = Shader.PropertyToID(_ED3E._E000(35843));

	private static readonly int _E053 = Shader.PropertyToID(_ED3E._E000(35893));

	private static readonly int _E054 = Shader.PropertyToID(_ED3E._E000(35885));

	private static readonly int _E055 = Shader.PropertyToID(_ED3E._E000(35877));

	private static readonly int _E056 = Shader.PropertyToID(_ED3E._E000(39442));

	private static readonly int _E057 = Shader.PropertyToID(_ED3E._E000(39424));

	private static readonly int _E058 = Shader.PropertyToID(_ED3E._E000(35933));

	private bool _E059;

	public bool forceSecondChromaticPass
	{
		get
		{
			if ((useDof && useChromaticAberration) || useChromaticBlur)
			{
				return useChromaticAberration;
			}
			return false;
		}
	}

	public RenderTexture AdaptationTexture => this.m__E007;

	public bool useMedianDoF
	{
		get
		{
			if (useBloom || useExposure || useDof)
			{
				return dofForceEnableMedian;
			}
			return false;
		}
	}

	private RenderTextureFormat _E05A
	{
		get
		{
			if (_E059)
			{
				return RenderTextureFormat.R8;
			}
			return RenderTextureFormat.ARGB32;
		}
	}

	public int aoSampleCountValue
	{
		get
		{
			return aoSampleCount switch
			{
				SampleCount.Low => 10, 
				SampleCount.Medium => 14, 
				SampleCount.High => 18, 
				_ => Mathf.Clamp((int)aoSampleCount, 1, 256), 
			};
		}
		set
		{
			aoSampleCount = (SampleCount)value;
		}
	}

	public bool UsingTerrain
	{
		get
		{
			if ((bool)Terrain.activeTerrain)
			{
				return true;
			}
			return false;
		}
	}

	public bool IsGBufferAvailable => this.m__E003.actualRenderingPath == RenderingPath.DeferredShading;

	public void DontRenderPrismThisFrame()
	{
		this.m__E000 = true;
	}

	public Camera GetPrismCamera()
	{
		if (this.m__E003 == null)
		{
			this.m__E003 = GetComponent<Camera>();
			this.m__E004 = GetComponent<SSAA>();
		}
		return this.m__E003;
	}

	public void ResetToneParamsRomB()
	{
		toneValues = new Vector3(-1f, 2.72f, 0.15f);
		secondaryToneValues = new Vector3(0f, 0f, 0f);
	}

	public void ResetToneParamsFilmic()
	{
		toneValues = new Vector3(6.2f, 0.5f, 1.7f);
		secondaryToneValues = new Vector3(0.004f, 0.06f, 0f);
	}

	public void ResetToneParamsACES()
	{
		toneValues = new Vector3(2.51f, 0.03f, 2.43f);
		secondaryToneValues = new Vector3(0.59f, 0.14f, 0f);
	}

	public void SetGamma(float value)
	{
		gammaValue = value;
	}

	public void SetChromaticIntensity(float value)
	{
		chromaticIntensity = value;
	}

	public void SetNoiseIntensity(float value)
	{
		noiseIntensity = value;
	}

	public void SetVignetteStrength(float value)
	{
		vignetteStrength = value;
	}

	public void SetPrimaryLutStrength(float value)
	{
		lutLerpAmount = value;
	}

	public void SetSecondaryLutStrength(float value)
	{
		secondaryLutLerpAmount = value;
	}

	public void SetPrismPreset(PrismPreset preset)
	{
		if (!preset)
		{
			useBloom = false;
			useDof = false;
			useChromaticAberration = false;
			useVignette = false;
			useNoise = false;
			useTonemap = false;
			useFog = false;
			useNightVision = false;
			useExposure = false;
			useLut = false;
			useSecondLut = false;
			useGammaCorrection = false;
			useAmbientObscurance = false;
			useRays = false;
			useUIBlur = false;
			return;
		}
		if (preset.presetType == PrismPresetType.Full || preset.presetType == PrismPresetType.Bloom)
		{
			useBloom = preset.useBloom;
			bloomType = preset.bloomType;
			bloomDownsample = preset.bloomDownsample;
			bloomBlurPasses = preset.bloomBlurPasses;
			bloomIntensity = preset.bloomIntensity;
			bloomThreshold = preset.bloomThreshold;
			useBloomStability = preset.useBloomStability;
			bloomUseScreenBlend = preset.bloomUseScreenBlend;
			useLensDirt = preset.useBloomLensdirt;
			dirtIntensity = preset.bloomLensdirtIntensity;
			lensDirtTexture = preset.bloomLensdirtTexture;
			useFullScreenBlur = preset.useFullScreenBlur;
			useUIBlur = preset.useUIBlur;
			uiBlurGrabTextureFromPassNumber = preset.uiBlurGrabTextureFromPassNumber;
		}
		if (preset.presetType == PrismPresetType.Full || preset.presetType == PrismPresetType.DepthOfField)
		{
			useDof = preset.useDoF;
			dofRadius = preset.dofRadius;
			dofSampleAmount = preset.dofSampleCount;
			dofBokehFactor = preset.dofBokehFactor;
			dofFocusPoint = preset.dofFocusPoint;
			dofFocusDistance = preset.dofFocusDistance;
			useNearDofBlur = preset.useNearBlur;
			dofBlurSkybox = preset.dofBlurSkybox;
			dofNearFocusDistance = preset.dofNearFocusDistance;
			dofForceEnableMedian = preset.dofForceEnableMedian;
		}
		if (preset.presetType == PrismPresetType.Full || preset.presetType == PrismPresetType.ChromaticAberration)
		{
			useChromaticAberration = preset.useChromaticAb;
			aberrationType = preset.aberrationType;
			chromaticIntensity = preset.chromIntensity;
			chromaticDistanceOne = preset.chromStart;
			chromaticDistanceTwo = preset.chromEnd;
			useChromaticBlur = preset.useChromaticBlur;
			chromaticBlurWidth = preset.chromaticBlurWidth;
		}
		if (preset.presetType == PrismPresetType.Full || preset.presetType == PrismPresetType.Vignette)
		{
			useVignette = preset.useVignette;
			vignetteStrength = preset.vignetteIntensity;
			vignetteEnd = preset.vignetteEnd;
			vignetteStart = preset.vignetteStart;
			vignetteColor = preset.vignetteColor;
		}
		if (preset.presetType == PrismPresetType.Full || preset.presetType == PrismPresetType.Noise)
		{
			useNoise = preset.useNoise;
			noiseIntensity = preset.noiseIntensity;
		}
		if (preset.presetType == PrismPresetType.Full || preset.presetType == PrismPresetType.Tonemap)
		{
			useTonemap = preset.useTonemap;
			tonemapType = preset.toneType;
			toneValues = preset.toneValues;
			secondaryToneValues = preset.secondaryToneValues;
		}
		if (preset.presetType == PrismPresetType.Full || preset.presetType == PrismPresetType.Exposure)
		{
			useExposure = preset.useExposure;
			exposureMiddleGrey = preset.exposureMiddleGrey;
			exposureLowerLimit = preset.exposureLowerLimit;
			exposureUpperLimit = preset.exposureUpperLimit;
			exposureSpeed = preset.exposureSpeed;
		}
		if (preset.presetType == PrismPresetType.Full || preset.presetType == PrismPresetType.Gamma)
		{
			useGammaCorrection = preset.useGammaCorrection;
			gammaValue = preset.gammaValue;
		}
		if (preset.presetType == PrismPresetType.Full || preset.presetType == PrismPresetType.ColorCorrection)
		{
			useLut = preset.useLUT;
			lutLerpAmount = preset.lutIntensity;
			basedOnTempTex = preset.lutPath;
			twoDLookupTex = preset.twoDLookupTex;
			useSecondLut = preset.useSecondLut;
			secondaryLutLerpAmount = preset.secondaryLutLerpAmount;
			secondaryBasedOnTempTex = preset.secondaryLutPath;
			secondaryTwoDLookupTex = preset.secondaryTwoDLookupTex;
		}
		if (preset.presetType == PrismPresetType.Full || preset.presetType == PrismPresetType.Nightvision)
		{
			useNightVision = preset.useNV;
			m_NVColor = preset.nvColor;
			m_TargetBleachColor = preset.nvBleachColor;
			m_baseLightingContribution = preset.nvLightingContrib;
			m_LightSensitivityMultiplier = preset.nvLightSensitivity;
		}
		if (preset.presetType == PrismPresetType.Full || preset.presetType == PrismPresetType.Fog)
		{
			useFog = preset.useFog;
			fogIntensity = preset.fogIntensity;
			fogStartPoint = preset.fogStartPoint;
			fogDistance = preset.fogDistance;
			fogColor = preset.fogColor;
			fogEndColor = preset.fogEndColor;
			fogHeight = preset.fogHeight;
		}
		if (preset.presetType == PrismPresetType.Full || preset.presetType == PrismPresetType.AmbientObscurance)
		{
			useAmbientObscurance = preset.useAmbientObscurance;
			useAODistanceCutoff = preset.useAODistanceCutoff;
			aoIntensity = preset.aoIntensity;
			aoRadius = preset.aoRadius;
			aoDistanceCutoffStart = preset.aoDistanceCutoffStart;
			aoDownsample = preset.aoDownsample;
			aoBlurIterations = preset.aoBlurIterations;
			aoDistanceCutoffLength = preset.aoDistanceCutoffLength;
			aoSampleCount = preset.aoSampleCount;
			aoBias = preset.aoBias;
			aoBlurFilterDistance = preset.aoBlurFilterDistance;
			aoBlurType = preset.aoBlurType;
			aoLightingContribution = preset.aoLightingContribution;
		}
		if (preset.presetType == PrismPresetType.Full || preset.presetType == PrismPresetType.Godrays)
		{
			useRays = preset.useRays;
			rayWeight = preset.rayWeight;
			rayColor = preset.rayColor;
			rayThreshold = preset.rayThreshold;
		}
		if (preset.presetType == PrismPresetType.Full || !currentPrismPreset)
		{
			currentPrismPreset = preset;
		}
		Reset();
	}

	public void LerpToPreset(PrismPreset preset, float t)
	{
		t = Mathf.Clamp(t, 0f, 1f);
		if (preset.presetType == PrismPresetType.Full || preset.presetType == PrismPresetType.Bloom)
		{
			bloomDownsample = (int)Mathf.Lerp(currentPrismPreset.bloomDownsample, preset.bloomDownsample, t);
			bloomBlurPasses = (int)Mathf.Lerp(currentPrismPreset.bloomBlurPasses, preset.bloomBlurPasses, t);
			bloomIntensity = Mathf.Lerp(currentPrismPreset.bloomIntensity, preset.bloomIntensity, t);
			bloomThreshold = Mathf.Lerp(currentPrismPreset.bloomThreshold, preset.bloomThreshold, t);
			dirtIntensity = Mathf.Lerp(currentPrismPreset.bloomLensdirtIntensity, preset.bloomLensdirtIntensity, t);
		}
		if (preset.presetType == PrismPresetType.Full || preset.presetType == PrismPresetType.DepthOfField)
		{
			dofRadius = Mathf.Lerp(currentPrismPreset.dofRadius, preset.dofRadius, t);
			dofBokehFactor = Mathf.Lerp(currentPrismPreset.dofBokehFactor, preset.dofBokehFactor, t);
			dofFocusPoint = Mathf.Lerp(currentPrismPreset.dofFocusPoint, preset.dofFocusPoint, t);
			dofFocusDistance = Mathf.Lerp(currentPrismPreset.dofFocusDistance, preset.dofFocusDistance, t);
			dofNearFocusDistance = Mathf.Lerp(currentPrismPreset.dofNearFocusDistance, preset.dofNearFocusDistance, t);
		}
		if (preset.presetType == PrismPresetType.Full || preset.presetType == PrismPresetType.ChromaticAberration)
		{
			chromaticIntensity = Mathf.Lerp(currentPrismPreset.chromIntensity, preset.chromIntensity, t);
			chromaticDistanceOne = Mathf.Lerp(currentPrismPreset.chromStart, preset.chromStart, t);
			chromaticDistanceTwo = Mathf.Lerp(currentPrismPreset.chromEnd, preset.chromEnd, t);
			chromaticBlurWidth = Mathf.Lerp(currentPrismPreset.chromaticBlurWidth, preset.chromaticBlurWidth, t);
		}
		if (preset.presetType == PrismPresetType.Full || preset.presetType == PrismPresetType.Vignette)
		{
			vignetteStrength = Mathf.Lerp(currentPrismPreset.vignetteIntensity, preset.vignetteIntensity, t);
			vignetteEnd = Mathf.Lerp(currentPrismPreset.vignetteEnd, preset.vignetteEnd, t);
			vignetteStart = Mathf.Lerp(currentPrismPreset.vignetteStart, preset.vignetteStart, t);
		}
		if (preset.presetType == PrismPresetType.Full || preset.presetType == PrismPresetType.Noise)
		{
			noiseIntensity = Mathf.Lerp(currentPrismPreset.noiseIntensity, preset.noiseIntensity, t);
		}
		if (preset.presetType == PrismPresetType.Full || preset.presetType == PrismPresetType.Tonemap)
		{
			toneValues = Vector3.Lerp(currentPrismPreset.toneValues, preset.toneValues, t);
			secondaryToneValues = Vector3.Lerp(currentPrismPreset.secondaryToneValues, preset.secondaryToneValues, t);
		}
		if (preset.presetType == PrismPresetType.Full || preset.presetType == PrismPresetType.Exposure)
		{
			exposureMiddleGrey = Mathf.Lerp(currentPrismPreset.exposureMiddleGrey, preset.exposureMiddleGrey, t);
			exposureLowerLimit = Mathf.Lerp(currentPrismPreset.exposureLowerLimit, preset.exposureLowerLimit, t);
			exposureUpperLimit = Mathf.Lerp(currentPrismPreset.exposureUpperLimit, preset.exposureUpperLimit, t);
			exposureSpeed = Mathf.Lerp(currentPrismPreset.exposureSpeed, preset.exposureSpeed, t);
		}
		if (preset.presetType == PrismPresetType.Full || preset.presetType == PrismPresetType.Gamma)
		{
			gammaValue = Mathf.Lerp(currentPrismPreset.gammaValue, preset.gammaValue, t);
		}
		if (preset.presetType == PrismPresetType.Full || preset.presetType == PrismPresetType.ColorCorrection)
		{
			lutLerpAmount = Mathf.Lerp(currentPrismPreset.lutIntensity, preset.lutIntensity, t);
			secondaryLutLerpAmount = Mathf.Lerp(currentPrismPreset.secondaryLutLerpAmount, preset.secondaryLutLerpAmount, t);
		}
		if (preset.presetType == PrismPresetType.Full || preset.presetType == PrismPresetType.Fog)
		{
			fogIntensity = Mathf.Lerp(currentPrismPreset.fogIntensity, preset.fogIntensity, t);
			fogStartPoint = Mathf.Lerp(currentPrismPreset.fogStartPoint, preset.fogStartPoint, t);
			fogDistance = Mathf.Lerp(currentPrismPreset.fogDistance, preset.fogDistance, t);
			fogColor = Color.Lerp(currentPrismPreset.fogColor, preset.fogColor, t);
			fogEndColor = Color.Lerp(currentPrismPreset.fogEndColor, preset.fogEndColor, t);
		}
		if (preset.presetType == PrismPresetType.Full || preset.presetType == PrismPresetType.AmbientObscurance)
		{
			aoDistanceCutoffStart = Mathf.Lerp(currentPrismPreset.aoDistanceCutoffStart, preset.aoDistanceCutoffStart, t);
			aoIntensity = Mathf.Lerp(currentPrismPreset.aoIntensity, preset.aoIntensity, t);
			aoRadius = Mathf.Lerp(currentPrismPreset.aoRadius, preset.aoRadius, t);
			aoBias = Mathf.Lerp(currentPrismPreset.aoBias, preset.aoBias, t);
			aoDistanceCutoffLength = Mathf.Lerp(currentPrismPreset.aoDistanceCutoffLength, preset.aoDistanceCutoffLength, t);
			aoBlurIterations = (int)Mathf.Lerp(currentPrismPreset.aoBlurIterations, preset.aoBlurIterations, t);
			aoLightingContribution = (int)Mathf.Lerp(currentPrismPreset.aoLightingContribution, preset.aoLightingContribution, t);
		}
		if (preset.presetType == PrismPresetType.Full || preset.presetType == PrismPresetType.Godrays)
		{
			rayWeight = Mathf.Lerp(currentPrismPreset.rayWeight, preset.rayWeight, t);
			rayColor = Color.Lerp(currentPrismPreset.rayColor, preset.rayColor, t);
			rayThreshold = Color.Lerp(currentPrismPreset.rayThreshold, preset.rayThreshold, t);
		}
	}

	private void OnEnable()
	{
		_E059 = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.R8);
		this.m__E003 = GetComponent<Camera>();
		this.m__E004 = GetComponent<SSAA>();
		if (!m_Shader)
		{
			m_Shader = _E3AC.Find(_ED3E._E000(38274));
			if (!m_Shader)
			{
				Debug.LogError(_ED3E._E000(40852));
			}
		}
		if (!m_Shader2)
		{
			m_Shader2 = _E3AC.Find(_ED3E._E000(38318));
			if (!m_Shader2)
			{
				Debug.LogError(_ED3E._E000(39487));
			}
		}
		if (!m_Shader3)
		{
			m_Shader3 = _E3AC.Find(_ED3E._E000(38355));
			if (!m_Shader3)
			{
				Debug.LogError(_ED3E._E000(39543));
			}
		}
		if (!m_AOShader)
		{
			m_AOShader = _E3AC.Find(_ED3E._E000(38391));
			if (!m_AOShader)
			{
				Debug.LogError(_ED3E._E000(40922));
			}
		}
		if (!this.m__E007)
		{
			this.m__E007 = new RenderTexture(histWidth, histHeight, 0, RenderTextureFormat.ARGB32)
			{
				name = _ED3E._E000(39598)
			};
			this.m__E007.filterMode = FilterMode.Bilinear;
			this.m__E007.autoGenerateMips = false;
		}
		if (useUIBlur)
		{
			int num = (this.m__E004 ? this.m__E004.GetInputWidth() : Screen.width);
			int num2 = (this.m__E004 ? this.m__E004.GetInputHeight() : Screen.height);
			this.m__E006 = new RenderTexture(num / bloomDownsample, num2 / bloomDownsample, 0, RenderTextureFormat.ARGB32);
			this.m__E006.name = _ED3E._E000(39637);
			this.m__E007.filterMode = FilterMode.Bilinear;
			this.m__E007.autoGenerateMips = false;
		}
		if (useDof || useFog)
		{
			this.m__E003.depthTextureMode |= DepthTextureMode.Depth;
		}
		if (useAmbientObscurance && (!IsGBufferAvailable || UsingTerrain))
		{
			this.m__E003.depthTextureMode |= DepthTextureMode.Depth;
			this.m__E003.depthTextureMode |= DepthTextureMode.DepthNormals;
		}
		this.m__E008 = true;
	}

	[ContextMenu("DontRenderDepthTexture")]
	private void _E000()
	{
		this.m__E003.depthTextureMode = DepthTextureMode.None;
	}

	private void OnDestroy()
	{
		if (this.m__E002 != null)
		{
			foreach (PrismEffects item in this.m__E002)
			{
				item.m_MasterEffectExposure = null;
			}
		}
		if ((bool)threeDLookupTex)
		{
			Object.DestroyImmediate(threeDLookupTex);
		}
		threeDLookupTex = null;
		basedOnTempTex = "";
		twoDLookupTex = null;
		if ((bool)secondaryThreeDLookupTex)
		{
			Object.DestroyImmediate(secondaryThreeDLookupTex);
		}
		secondaryThreeDLookupTex = null;
		secondaryBasedOnTempTex = "";
		secondaryTwoDLookupTex = null;
	}

	private void OnDisable()
	{
		if ((bool)m_Material)
		{
			Object.DestroyImmediate(m_Material);
			m_Material = null;
		}
		if ((bool)m_Material2)
		{
			Object.DestroyImmediate(m_Material2);
			m_Material2 = null;
		}
		if ((bool)m_Material3)
		{
			Object.DestroyImmediate(m_Material3);
			m_Material3 = null;
		}
		if ((bool)m_AOMaterial)
		{
			Object.DestroyImmediate(m_AOMaterial);
			m_AOMaterial = null;
		}
		if (m_AOShader == m_Shader || m_Shader2 == m_Shader || m_Shader3 == m_Shader)
		{
			m_AOShader = null;
			m_Shader2 = null;
			m_Shader3 = null;
		}
		if ((bool)threeDLookupTex)
		{
			Object.DestroyImmediate(threeDLookupTex);
			basedOnTempTex = "";
		}
		if ((bool)secondaryThreeDLookupTex)
		{
			Object.DestroyImmediate(secondaryThreeDLookupTex);
			secondaryBasedOnTempTex = "";
		}
		if ((bool)this.m__E007)
		{
			Object.DestroyImmediate(this.m__E007);
			this.m__E007 = null;
		}
		if ((bool)this.m__E006)
		{
			Object.DestroyImmediate(this.m__E006);
			this.m__E006 = null;
		}
	}

	private Material _E001(Shader shader)
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

	public void Reset()
	{
		OnDisable();
		OnEnable();
	}

	private bool _E002()
	{
		if (m_Material == null && m_Shader != null && m_Shader.isSupported)
		{
			m_Material = _E001(m_Shader);
		}
		if (m_Material2 == null && m_Shader2 != null && m_Shader2.isSupported)
		{
			m_Material2 = _E001(m_Shader2);
		}
		if (m_Material3 == null && m_Shader3 != null && m_Shader3.isSupported)
		{
			m_Material3 = _E001(m_Shader3);
		}
		if (m_AOMaterial == null && m_AOShader != null && m_AOShader.isSupported)
		{
			m_AOMaterial = _E001(m_AOShader);
		}
		if (!m_Shader.isSupported)
		{
			Debug.LogError(_ED3E._E000(38939));
			base.enabled = false;
			return false;
		}
		if (!m_Shader2.isSupported)
		{
			Debug.LogError(_ED3E._E000(39625));
			base.enabled = false;
			return false;
		}
		if (!m_Shader3.isSupported)
		{
			Debug.LogError(_ED3E._E000(39680));
			m_Shader3 = m_Shader;
			useRays = false;
		}
		if (!m_AOShader.isSupported)
		{
			Debug.LogError(_ED3E._E000(39025));
			m_AOShader = m_Shader;
			useAmbientObscurance = false;
		}
		if (useLut && threeDLookupTex == null && (bool)twoDLookupTex)
		{
			Convert(twoDLookupTex);
			if (threeDLookupTex == null)
			{
				useLut = false;
				Debug.LogWarning(_ED3E._E000(39795));
			}
		}
		if (useLut && useSecondLut && secondaryThreeDLookupTex == null && (bool)secondaryTwoDLookupTex)
		{
			Convert(secondaryTwoDLookupTex, secondaryLut: true);
			if (secondaryThreeDLookupTex == null)
			{
				useSecondLut = false;
				Debug.LogWarning(_ED3E._E000(39818));
			}
		}
		return true;
	}

	public void UpdateShaderValues()
	{
		if (m_Material == null)
		{
			return;
		}
		m_Material.shaderKeywords = null;
		m_Material2.shaderKeywords = null;
		m_AOMaterial.shaderKeywords = null;
		if (useBloom)
		{
			if (bloomType == BloomType.Simple)
			{
				Shader.DisableKeyword(_ED3E._E000(39885));
				Shader.EnableKeyword(_ED3E._E000(39933));
				m_Material2.SetFloat(PrismEffects.m__E009, bloomIntensity);
				m_Material2.SetFloat(PrismEffects.m__E00A, bloomThreshold);
				m_Material.SetFloat(PrismEffects.m__E009, bloomIntensity);
				m_Material.SetFloat(PrismEffects.m__E00A, bloomThreshold);
				if (!this.m__E003.allowHDR)
				{
					Shader.EnableKeyword(_ED3E._E000(39912));
				}
				else
				{
					Shader.DisableKeyword(_ED3E._E000(39912));
				}
			}
			else if (bloomType == BloomType.HDR)
			{
				Shader.DisableKeyword(_ED3E._E000(39933));
				Shader.EnableKeyword(_ED3E._E000(39885));
				Shader.DisableKeyword(_ED3E._E000(39912));
				m_Material.SetFloat(PrismEffects.m__E00A, bloomThreshold);
				m_Material2.SetFloat(PrismEffects.m__E00A, bloomThreshold);
				m_Material2.SetFloat(PrismEffects.m__E009, bloomIntensity);
				m_Material.SetFloat(PrismEffects.m__E009, bloomIntensity);
			}
			float value = dirtIntensity * dirtIntensity;
			m_Material.SetFloat(PrismEffects.m__E00B, value);
			m_Material2.SetFloat(PrismEffects.m__E00B, value);
			if (useUIBlur)
			{
				Shader.EnableKeyword(_ED3E._E000(33808));
			}
			else
			{
				Shader.DisableKeyword(_ED3E._E000(33808));
			}
		}
		else
		{
			Shader.DisableKeyword(_ED3E._E000(39885));
			Shader.EnableKeyword(_ED3E._E000(39933));
			Shader.DisableKeyword(_ED3E._E000(39912));
			Shader.DisableKeyword(_ED3E._E000(33797));
			Shader.DisableKeyword(_ED3E._E000(33843));
			Shader.DisableKeyword(_ED3E._E000(33808));
		}
		_E003();
		if (useFog)
		{
			_E005(m_Material);
		}
		else
		{
			m_Material.SetFloat(PrismEffects.m__E00C, 0f);
		}
		if (useVignette)
		{
			m_Material.SetFloat(_E00D, vignetteStart);
			m_Material.SetFloat(_E00E, vignetteEnd);
			m_Material.SetFloat(_E00F, vignetteStrength);
			m_Material.SetColor(_E010, vignetteColor);
		}
		else
		{
			m_Material.SetFloat(_E00F, 0f);
		}
		if (useNightVision)
		{
			m_Material.SetVector(_E011, m_NVColor);
			m_Material.SetVector(_E012, m_TargetBleachColor);
			m_Material.SetFloat(_E013, m_baseLightingContribution);
			m_Material.SetFloat(_E014, m_LightSensitivityMultiplier);
		}
		if (useNoise)
		{
			Vector2 value2 = new Vector2(noiseIntensity * 0.01f, noiseIntensity);
			m_Material.SetVector(_E015, new Vector2(noiseIntensity * 0.01f, noiseIntensity));
			m_Material.SetTexture(_E016, noiseTexture);
			Vector2 vector = new Vector2(0.5f - Random.value, 0.5f - Random.value);
			m_Material.SetVector(_E017, vector);
			if (this.m__E004 != null)
			{
				this.m__E004.SetNoiseParams(enabled: true, noiseTexture, vector, value2);
			}
		}
		else if (this.m__E004 != null)
		{
			this.m__E004.SetNoiseParams(enabled: false, null, null, null);
		}
		if (useChromaticAberration)
		{
			float num = 0f;
			float value3 = chromaticIntensity * chromaticIntensity;
			num = ((aberrationType != AberrationType.Vertical) ? 0f : 1f);
			m_Material.SetFloat(_E018, value3);
			m_Material2.SetFloat(_E018, value3);
			m_Material.SetVector(_E019, new Vector4(chromaticDistanceOne, chromaticDistanceTwo, num, 0f));
			m_Material2.SetVector(_E019, new Vector4(chromaticDistanceOne, chromaticDistanceTwo, num, 0f));
		}
		else
		{
			m_Material.SetFloat(_E018, 0f);
		}
		if (useTonemap)
		{
			m_Material.SetVector(_E01A, new Vector4(toneValues.x, toneValues.y, toneValues.z, toneValues.z));
			m_Material.SetVector(_E01B, new Vector4(secondaryToneValues.x, secondaryToneValues.y, secondaryToneValues.z, secondaryToneValues.z));
		}
		if (useGammaCorrection)
		{
			m_Material.SetFloat(_E01C, gammaValue);
		}
		else
		{
			m_Material.SetFloat(_E01C, 0f);
		}
		if (useDof)
		{
			_E009(m_Material);
		}
		if (useSharpen)
		{
			m_Material2.SetFloat(_E01D, sharpenAmount);
		}
		if (useLensDirt)
		{
			m_Material.SetTexture(_E01E, lensDirtTexture);
			m_Material2.SetTexture(_E01E, lensDirtTexture);
		}
		else
		{
			m_Material.SetFloat(PrismEffects.m__E00B, 0f);
		}
		if (useExposure)
		{
			m_Material.SetFloat(_E01F, exposureLowerLimit);
			m_Material.SetFloat(_E020, exposureUpperLimit);
			m_Material.SetFloat(_E021, exposureMiddleGrey);
			m_Material2.SetFloat(_E022, exposureSpeed);
			m_Material2.SetFloat(_E01F, exposureLowerLimit);
			m_Material2.SetFloat(_E020, exposureUpperLimit);
			m_Material2.SetFloat(_E021, exposureMiddleGrey);
			Shader.EnableKeyword(_ED3E._E000(33827));
		}
		else
		{
			Shader.DisableKeyword(_ED3E._E000(33827));
		}
		Shader.DisableKeyword(_ED3E._E000(33870));
		Shader.DisableKeyword(_ED3E._E000(33914));
		if (useDof)
		{
			if (dofSampleAmount == DoFSamples.Low)
			{
				Shader.EnableKeyword(_ED3E._E000(33870));
			}
			else if (dofSampleAmount == DoFSamples.Medium)
			{
				Shader.EnableKeyword(_ED3E._E000(33914));
			}
			else
			{
				Shader.EnableKeyword(_ED3E._E000(33914));
				Debug.LogWarning(_ED3E._E000(33894));
				dofSampleAmount = DoFSamples.Medium;
			}
			if (useNearDofBlur)
			{
				Shader.EnableKeyword(_ED3E._E000(34058));
			}
			else
			{
				Shader.DisableKeyword(_ED3E._E000(34058));
			}
		}
		else
		{
			Shader.DisableKeyword(_ED3E._E000(34058));
			if (useFullScreenBlur)
			{
				m_Material.EnableKeyword(_ED3E._E000(34058));
				m_Material2.EnableKeyword(_ED3E._E000(34058));
			}
			else
			{
				m_Material.DisableKeyword(_ED3E._E000(34058));
				m_Material2.DisableKeyword(_ED3E._E000(34058));
			}
		}
		_E007();
		_E008();
		if (!useLut)
		{
			m_Material.DisableKeyword(_ED3E._E000(39155));
			m_Material.DisableKeyword(_ED3E._E000(39111));
		}
		else if (this.m__E003.allowHDR)
		{
			m_Material.EnableKeyword(_ED3E._E000(39111));
			m_Material.DisableKeyword(_ED3E._E000(39155));
		}
		else
		{
			m_Material.EnableKeyword(_ED3E._E000(39155));
			m_Material.DisableKeyword(_ED3E._E000(39111));
		}
		if (useTonemap && tonemapType == TonemapType.Filmic)
		{
			m_Material.EnableKeyword(_ED3E._E000(34096));
			Shader.EnableKeyword(_ED3E._E000(34096));
			Shader.DisableKeyword(_ED3E._E000(34141));
			Shader.DisableKeyword(_ED3E._E000(34120));
		}
		else if (useTonemap && tonemapType == TonemapType.RomB)
		{
			m_Material.EnableKeyword(_ED3E._E000(34141));
			Shader.EnableKeyword(_ED3E._E000(34141));
			Shader.DisableKeyword(_ED3E._E000(34096));
			Shader.DisableKeyword(_ED3E._E000(34120));
		}
		else if (useTonemap && tonemapType == TonemapType.ACES)
		{
			m_Material.EnableKeyword(_ED3E._E000(34120));
			Shader.EnableKeyword(_ED3E._E000(34120));
			Shader.DisableKeyword(_ED3E._E000(34096));
			Shader.DisableKeyword(_ED3E._E000(34141));
		}
		else
		{
			Shader.DisableKeyword(_ED3E._E000(34096));
			Shader.DisableKeyword(_ED3E._E000(34141));
			Shader.DisableKeyword(_ED3E._E000(34120));
		}
		if (useNoise)
		{
			if (this.m__E004 != null && this.m__E004.UsesDLSSUpscaler())
			{
				m_Material.SetFloat(_E023, 0f);
			}
			else
			{
				m_Material.SetFloat(_E023, 1f);
			}
		}
		else
		{
			m_Material.SetFloat(_E023, 0f);
		}
		if (useNightVision)
		{
			m_Material.SetFloat(_E024, 1f);
			Shader.EnableKeyword(_ED3E._E000(34171));
		}
		else
		{
			m_Material.SetFloat(_E024, 0f);
			Shader.DisableKeyword(_ED3E._E000(34171));
		}
		if (useRays)
		{
			_E004(m_Material3);
			_E004(m_Material);
		}
		else
		{
			m_Material.SetFloat(_E025, 0f);
		}
		if (useAmbientObscurance)
		{
			if (!IsGBufferAvailable || UsingTerrain)
			{
				this.m__E003.depthTextureMode |= DepthTextureMode.Depth;
				this.m__E003.depthTextureMode |= DepthTextureMode.DepthNormals;
			}
			m_Material.SetFloat(_E026, aoIntensity);
			m_Material.SetFloat(_E027, aoLightingContribution);
			if (useAODistanceCutoff || useDof)
			{
				m_AOMaterial.EnableKeyword(_ED3E._E000(39198));
			}
			else
			{
				m_AOMaterial.DisableKeyword(_ED3E._E000(39198));
			}
			if (IsGBufferAvailable && !UsingTerrain)
			{
				m_AOMaterial.EnableKeyword(_ED3E._E000(39187));
			}
			else
			{
				m_AOMaterial.DisableKeyword(_ED3E._E000(39187));
			}
			if (aoSampleCount == SampleCount.Low)
			{
				m_AOMaterial.EnableKeyword(_ED3E._E000(39171));
				m_AOMaterial.DisableKeyword(_ED3E._E000(39209));
				m_AOMaterial.SetInt(_E028, aoSampleCountValue);
			}
			else
			{
				m_AOMaterial.EnableKeyword(_ED3E._E000(39209));
				m_AOMaterial.DisableKeyword(_ED3E._E000(39171));
				m_AOMaterial.SetInt(_E028, aoSampleCountValue);
			}
			m_AOMaterial.SetInt(_E029, aoSampleCountValue);
		}
		else
		{
			m_Material.SetFloat(_E026, aoMinIntensity);
		}
	}

	private void _E003()
	{
		if ((useBloom && !useBloomStability) || (useBloom && !this.m__E005))
		{
			Shader.EnableKeyword(_ED3E._E000(33843));
			Shader.DisableKeyword(_ED3E._E000(33797));
		}
		else if (useBloom && useBloomStability)
		{
			Shader.EnableKeyword(_ED3E._E000(33797));
			Shader.DisableKeyword(_ED3E._E000(33843));
		}
		else
		{
			Shader.DisableKeyword(_ED3E._E000(33797));
			Shader.DisableKeyword(_ED3E._E000(33843));
		}
		if (bloomUseScreenBlend)
		{
			Shader.EnableKeyword(_ED3E._E000(39912));
		}
		else
		{
			Shader.DisableKeyword(_ED3E._E000(39912));
		}
	}

	private void _E004(Material raysMaterial)
	{
		raysMaterial.SetFloat(_E025, rayWeight);
		raysMaterial.SetColor(_E02A, rayColor);
		raysMaterial.SetColor(_E02B, rayThreshold);
		if (!rayTransform)
		{
			raysMaterial.SetVector(_E02C, new Vector4(base.transform.position.x, base.transform.position.y, base.transform.position.z, 1f));
			return;
		}
		Vector3 vector = this.m__E003.WorldToViewportPoint(rayTransform.position);
		raysMaterial.SetVector(_E02C, new Vector4(vector.x, vector.y, vector.z, rayWeight));
		if (vector.z >= 0f)
		{
			raysMaterial.SetColor(_E02A, rayColor);
		}
		else
		{
			raysMaterial.SetColor(_E02A, Color.black);
		}
	}

	private void _E005(Material fogMaterial)
	{
		fogMaterial.SetFloat(_E02D, fogHeight);
		fogMaterial.SetFloat(PrismEffects.m__E00C, 1f);
		fogMaterial.SetFloat(_E02E, fogDistance);
		fogMaterial.SetFloat(_E02F, fogStartPoint);
		fogMaterial.SetColor(_E030, fogColor);
		fogMaterial.SetColor(_E031, fogEndColor);
		if (fogAffectSkybox)
		{
			fogMaterial.SetFloat(_E032, 1f);
		}
		else
		{
			fogMaterial.SetFloat(_E032, 0.9999999f);
		}
		fogMaterial.SetMatrix(_E033, this.m__E003.cameraToWorldMatrix);
	}

	private void _E006(Material aoMaterial)
	{
		m_Material.SetFloat(_E027, aoLightingContribution);
		aoMaterial.SetFloat(_E026, aoIntensity);
		aoMaterial.SetFloat(_E034, aoRadius);
		aoMaterial.SetFloat(_E035, aoBias * 0.02f);
		aoMaterial.SetFloat(_E036, aoDownsample ? 0.5f : 1f);
		if (useDof)
		{
			aoMaterial.SetFloat(_E037, dofFocusPoint);
			aoMaterial.SetFloat(_E038, dofFocusDistance);
		}
		else
		{
			aoMaterial.SetFloat(_E037, aoDistanceCutoffStart);
			aoMaterial.SetFloat(_E038, aoDistanceCutoffLength);
		}
		aoMaterial.SetMatrix(_E039, this.m__E003.cameraToWorldMatrix);
		int num = (this.m__E004 ? this.m__E004.GetInputWidth() : Screen.width);
		int num2 = (this.m__E004 ? this.m__E004.GetInputHeight() : Screen.height);
		Matrix4x4 projectionMatrix = this.m__E003.projectionMatrix;
		aoMaterial.SetVector(value: new Vector4(-2f / ((float)num * projectionMatrix[0]), -2f / ((float)num2 * projectionMatrix[5]), (1f - projectionMatrix[2]) / projectionMatrix[0], (1f + projectionMatrix[6]) / projectionMatrix[5]), nameID: _E03A);
	}

	private void _E007()
	{
		if (useLut && (bool)twoDLookupTex && (bool)threeDLookupTex)
		{
			int width = threeDLookupTex.width;
			threeDLookupTex.wrapMode = TextureWrapMode.Clamp;
			m_Material.SetFloat(_E03B, (float)(width - 1) / (1f * (float)width));
			m_Material.SetFloat(_E03C, 1f / (2f * (float)width));
			m_Material.SetTexture(_E03D, threeDLookupTex);
			m_Material.SetFloat(_E03E, lutLerpAmount);
		}
		else
		{
			m_Material.SetFloat(_E03E, 0f);
		}
	}

	private void _E008()
	{
		if (useSecondLut && (bool)secondaryTwoDLookupTex && (bool)secondaryThreeDLookupTex)
		{
			int width = secondaryThreeDLookupTex.width;
			secondaryThreeDLookupTex.wrapMode = TextureWrapMode.Clamp;
			m_Material.SetFloat(_E03F, (float)(width - 1) / (1f * (float)width));
			m_Material.SetFloat(_E040, 1f / (2f * (float)width));
			m_Material.SetTexture(_E041, secondaryThreeDLookupTex);
			m_Material.SetFloat(_E042, secondaryLutLerpAmount);
		}
		else
		{
			m_Material.SetFloat(_E042, 0f);
		}
	}

	public void SetDofTransform(Transform target)
	{
		dofFocusTransform = target;
	}

	public void SetDofPoint(Vector3 point)
	{
		dofFocusPoint = Vector3.Distance(this.m__E003.transform.position, point);
	}

	public void ResetDofTransform()
	{
		dofFocusTransform = null;
	}

	private void _E009(Material targetMat)
	{
		if ((bool)dofFocusTransform)
		{
			targetMat.SetFloat(_E043, Vector3.Distance(base.transform.position, dofFocusTransform.position));
		}
		else
		{
			targetMat.SetFloat(_E043, dofFocusPoint);
		}
		targetMat.SetFloat(_E044, dofFocusDistance);
		targetMat.SetFloat(_E045, dofRadius);
		targetMat.SetFloat(_E046, dofBokehFactor);
		if (useNearDofBlur)
		{
			targetMat.SetFloat(_E047, 1f);
			targetMat.SetFloat(_E048, dofNearFocusDistance);
		}
		else
		{
			targetMat.SetFloat(_E047, 0f);
		}
		if (dofBlurSkybox)
		{
			targetMat.SetFloat(_E049, 1f);
		}
		else
		{
			targetMat.SetFloat(_E049, 0.9999999f);
		}
	}

	private void _E00A(RenderTexture source, RenderTextureFormat rtFormat)
	{
		RenderTexture renderTexture = RenderTexture.GetTemporary(source.width / 2, source.width / 2, 0, rtFormat);
		renderTexture.name = _ED3E._E000(34145);
		renderTexture.filterMode = FilterMode.Bilinear;
		DebugGraphics.Blit(source, renderTexture, m_Material2, 3);
		int num = histWidth;
		while (renderTexture.height > num || renderTexture.width > num)
		{
			int num2 = renderTexture.width / 2;
			if (num2 < num)
			{
				num2 = num;
			}
			int num3 = renderTexture.height / 2;
			if (num3 < num)
			{
				num3 = num;
			}
			RenderTexture temporary = RenderTexture.GetTemporary(num2, num3, 0, rtFormat);
			temporary.name = _ED3E._E000(34181);
			DebugGraphics.Blit(renderTexture, temporary, m_Material2, 2);
			RenderTexture.ReleaseTemporary(renderTexture);
			renderTexture = temporary;
		}
		this.m__E007.MarkRestoreExpected();
		int pass = 4;
		if (this.m__E008)
		{
			Graphics.Blit(renderTexture, this.m__E007);
		}
		else
		{
			DebugGraphics.Blit(renderTexture, this.m__E007, m_Material2, pass);
		}
		m_Material.SetTexture(_E04A, this.m__E007);
		m_Material2.SetTexture(_E04A, this.m__E007);
		RenderTexture.ReleaseTemporary(renderTexture);
	}

	private RenderTexture _E00B(RenderTexture tex, int iterations = 1)
	{
		for (int i = 0; i < iterations; i++)
		{
			Vector2 vector = new Vector2(0f, 1f);
			RenderTexture temporary = RenderTexture.GetTemporary(tex.width, tex.height, 0, RenderTextureFormat.ARGB32);
			temporary.name = _ED3E._E000(34221);
			m_Material2.SetVector(_E04B, vector);
			DebugGraphics.Blit(tex, temporary, m_Material2, 8);
			vector = new Vector2(1f, 0f);
			m_Material2.SetVector(_E04B, vector);
			DebugGraphics.Blit(temporary, tex, m_Material2, 8);
			RenderTexture.ReleaseTemporary(temporary);
		}
		RenderTexture.ReleaseTemporary(tex);
		return tex;
	}

	[ImageEffectTransformsToLDR]
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		bool flag = true;
		if (this.m__E000)
		{
			Graphics.CopyTexture(source, destination);
			this.m__E000 = false;
			return;
		}
		if (!_E002() || !flag)
		{
			Graphics.CopyTexture(source, destination);
			return;
		}
		EnvironmentManager instance = EnvironmentManager.Instance;
		if (instance != null)
		{
			exposureSpeed = instance.PrismExposureSpeed;
			exposureMiddleGrey = instance.PrismExposureOffset;
		}
		UpdateShaderValues();
		if (threeDLookupTex == null && useLut)
		{
			SetIdentityLut();
		}
		if (secondaryThreeDLookupTex == null && useSecondLut)
		{
			SetIdentityLut(secondary: true);
		}
		RenderTextureFormat defaultHDRRenderTextureFormat = RuntimeUtilities.defaultHDRRenderTextureFormat;
		defaultHDRRenderTextureFormat = (this.m__E003.allowHDR ? RuntimeUtilities.defaultHDRRenderTextureFormat : RenderTextureFormat.ARGB32);
		if (isParentPrism)
		{
			if (!this.m__E001 || this.m__E001.width != source.width || this.m__E001.height != source.height)
			{
				this.m__E001 = new RenderTexture(source.width, source.height, 16, RenderTextureFormat.RHalf, RenderTextureReadWrite.Linear);
				this.m__E001.name = _ED3E._E000(34267);
			}
			Graphics.SetRenderTarget(this.m__E001);
			GL.Clear(clearDepth: true, clearColor: true, Color.white);
			DebugGraphics.Blit(source, this.m__E001, m_Material3, 5);
			Shader.SetGlobalTexture(_E04C, this.m__E001);
			if (debugDofPass)
			{
				DebugGraphics.Blit(this.m__E001, destination, m_Material3, 6);
				return;
			}
		}
		int num = 1;
		if (aoDownsample)
		{
			num = 2;
		}
		int width = source.width / num;
		int height = source.height / num;
		int width2 = source.width / 4;
		int height2 = source.height / 4;
		RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, _E05A, RenderTextureReadWrite.Linear);
		temporary.name = _ED3E._E000(34241);
		RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, 0, defaultHDRRenderTextureFormat);
		temporary2.name = _ED3E._E000(34285);
		RenderTexture temporary3 = RenderTexture.GetTemporary(source.width, source.height, 0, defaultHDRRenderTextureFormat);
		temporary3.name = _ED3E._E000(34327);
		RenderTexture temporary4 = RenderTexture.GetTemporary(width2, height2, 0, RenderTextureFormat.ARGB32);
		temporary4.name = _ED3E._E000(34366);
		RenderTexture temporary5 = RenderTexture.GetTemporary(source.width / 2, source.height / 2, 0, defaultHDRRenderTextureFormat);
		temporary5.name = _ED3E._E000(34341);
		RenderTexture temporary6 = RenderTexture.GetTemporary(source.width, source.height, 0, defaultHDRRenderTextureFormat);
		temporary6.name = _ED3E._E000(34378);
		if (useAmbientObscurance)
		{
			_E006(m_AOMaterial);
			DebugGraphics.Blit(null, temporary, m_AOMaterial, 0);
			if (aoBlurType == AOBlurType.Fast)
			{
				for (int i = 0; i < aoBlurIterations; i++)
				{
					m_AOMaterial.SetVector(_E056, new Vector4(-1f, 0f, 0f, 0f));
					RenderTexture temporary7 = RenderTexture.GetTemporary(width, height, 0, _E05A, RenderTextureReadWrite.Linear);
					temporary7.name = _ED3E._E000(34776);
					DebugGraphics.Blit(temporary, temporary7, m_AOMaterial, (int)aoBlurType);
					RenderTexture.ReleaseTemporary(temporary);
					m_AOMaterial.SetVector(_E056, new Vector4(0f, 1f, 0f, 0f));
					temporary = RenderTexture.GetTemporary(width, height, 0, _E05A, RenderTextureReadWrite.Linear);
					temporary.name = _ED3E._E000(34756);
					DebugGraphics.Blit(temporary7, temporary, m_AOMaterial, (int)aoBlurType);
					RenderTexture.ReleaseTemporary(temporary7);
				}
			}
			else
			{
				for (int j = 0; j < aoBlurIterations; j++)
				{
					for (int k = 0; k < 2; k++)
					{
						m_AOMaterial.SetVector(_E056, new Vector4(-1f, 0f, 0f, 0f));
						RenderTexture temporary7 = RenderTexture.GetTemporary(width, height, 0, _E05A, RenderTextureReadWrite.Linear);
						temporary7.name = _ED3E._E000(34776);
						DebugGraphics.Blit(temporary, temporary7, m_AOMaterial, (int)(aoBlurType + k));
						RenderTexture.ReleaseTemporary(temporary);
						m_AOMaterial.SetVector(_E056, new Vector4(0f, 1f, 0f, 0f));
						temporary = RenderTexture.GetTemporary(width, height, 0, _E05A, RenderTextureReadWrite.Linear);
						temporary.name = _ED3E._E000(34756);
						DebugGraphics.Blit(temporary7, temporary, m_AOMaterial, (int)(aoBlurType + k));
						RenderTexture.ReleaseTemporary(temporary7);
					}
				}
			}
			if (aoShowDebug)
			{
				DebugGraphics.Blit(temporary, destination, m_AOMaterial, 2);
				goto IL_1084;
			}
			m_Material.SetTexture(_E057, temporary);
			if (isParentPrism)
			{
				Shader.SetGlobalFloat(_E026, aoIntensity);
				Shader.SetGlobalTexture(_E057, temporary);
			}
		}
		if (useMedianDoF)
		{
			DebugGraphics.Blit(source, temporary5, m_Material2, 6);
			m_Material.SetTexture(_E04D, temporary5);
			m_Material.SetFloat(_E04E, 1f);
		}
		else
		{
			m_Material.SetTexture(_E04D, source);
			m_Material.SetFloat(_E04E, 0f);
		}
		if (debugDofPass && useDof)
		{
			_E009(m_Material2);
			DebugGraphics.Blit(source, destination, m_Material2, 9);
		}
		else
		{
			if (useExposure)
			{
				bool flag2 = false;
				if (m_MasterEffectExposure != null)
				{
					flag2 = true;
					Graphics.Blit(m_MasterEffectExposure.AdaptationTexture, this.m__E007);
					m_Material.SetTexture(_E04A, this.m__E007);
					m_Material2.SetTexture(_E04A, this.m__E007);
				}
				if (!flag2)
				{
					if (useMedianDoF)
					{
						_E00A(temporary5, defaultHDRRenderTextureFormat);
					}
					else
					{
						_E00A(source, defaultHDRRenderTextureFormat);
					}
				}
				if (debugViewExposure)
				{
					DebugGraphics.Blit(source, destination, m_Material2, 11);
					goto IL_1084;
				}
			}
			if (useBloom)
			{
				if (bloomType == BloomType.HDR)
				{
					bloomDownsample = 2;
				}
				int num2 = source.width / bloomDownsample;
				int num3 = source.height / bloomDownsample;
				RenderTexture temporary8 = RenderTexture.GetTemporary(num2, num3, 0, defaultHDRRenderTextureFormat);
				temporary8.name = _ED3E._E000(34423);
				temporary8.filterMode = FilterMode.Bilinear;
				if (useMedianDoF)
				{
					if (useBloomStability)
					{
						DebugGraphics.Blit(temporary5, temporary8, m_Material2, 6);
					}
					else
					{
						DebugGraphics.Blit(temporary5, temporary8, m_Material2, 6);
					}
				}
				else
				{
					DebugGraphics.Blit(source, temporary8, m_Material2, 6);
				}
				if (useRays)
				{
					DebugGraphics.Blit(temporary8, temporary4, m_Material2, 2);
					_E00C(temporary4);
					if (raysShowDebug)
					{
						Graphics.Blit(temporary4, destination);
						RenderTexture.ReleaseTemporary(temporary8);
						goto IL_1084;
					}
				}
				RenderTexture renderTexture = RenderTexture.GetTemporary(num2, num3, 0, defaultHDRRenderTextureFormat);
				renderTexture.name = _ED3E._E000(34454);
				if (bloomType == BloomType.Simple)
				{
					int num4 = 0;
					DebugGraphics.Blit(temporary8, renderTexture, m_Material2, 5);
					for (int l = 0; l < bloomBlurPasses; l++)
					{
						RenderTexture temporary9 = RenderTexture.GetTemporary(num2, num3, 0, defaultHDRRenderTextureFormat);
						temporary9.name = _ED3E._E000(34490);
						m_Material2.SetInt(_E04F, l + num4);
						DebugGraphics.Blit(renderTexture, temporary9, m_Material2, 7);
						if (useUIBlur && uiBlurGrabTextureFromPassNumber == l && this.m__E006.width > 0 && this.m__E006.height > 0)
						{
							Graphics.Blit(temporary9, this.m__E006);
							Shader.SetGlobalTexture(_E050, this.m__E006);
						}
						RenderTexture.ReleaseTemporary(renderTexture);
						renderTexture = temporary9;
					}
					m_Material.SetTexture(_E051, renderTexture);
					if (debugBloomTex)
					{
						m_Material3.SetFloat(PrismEffects.m__E00A, bloomThreshold);
						m_Material3.SetFloat(PrismEffects.m__E009, bloomIntensity);
						DebugGraphics.Blit(renderTexture, destination, m_Material3, 4);
						RenderTexture.ReleaseTemporary(temporary8);
						RenderTexture.ReleaseTemporary(renderTexture);
						goto IL_1084;
					}
					if ((bool)this.m__E005 && useBloomStability)
					{
						m_Material.SetTexture(_E052, this.m__E005);
					}
					if ((bool)this.m__E005)
					{
						RenderTexture.ReleaseTemporary(this.m__E005);
						this.m__E005 = null;
					}
					if (useBloomStability)
					{
						this.m__E005 = RenderTexture.GetTemporary(num2, num3, 0, defaultHDRRenderTextureFormat);
						this.m__E005.name = _ED3E._E000(34470);
						this.m__E005.filterMode = FilterMode.Bilinear;
						Graphics.Blit(renderTexture, this.m__E005);
					}
					RenderTexture.ReleaseTemporary(temporary8);
				}
				else if (bloomType == BloomType.HDR)
				{
					RenderTexture temporary10 = RenderTexture.GetTemporary(num2 / 2, num3 / 2, 0, defaultHDRRenderTextureFormat);
					temporary10.name = _ED3E._E000(34503);
					temporary10.filterMode = FilterMode.Bilinear;
					RenderTexture temporary11 = RenderTexture.GetTemporary(num2 / 4, num3 / 4, 0, defaultHDRRenderTextureFormat);
					temporary11.name = _ED3E._E000(34529);
					temporary11.filterMode = FilterMode.Bilinear;
					RenderTexture temporary12 = RenderTexture.GetTemporary(num2 / 8, num3 / 8, 0, defaultHDRRenderTextureFormat);
					temporary12.name = _ED3E._E000(34561);
					temporary12.filterMode = FilterMode.Bilinear;
					DebugGraphics.Blit(source, temporary8, m_Material2, 10);
					DebugGraphics.Blit(temporary8, temporary10, m_Material2, 2);
					DebugGraphics.Blit(temporary10, temporary11, m_Material2, 2);
					DebugGraphics.Blit(temporary11, temporary12, m_Material2, 6);
					temporary8 = _E00B(temporary8);
					temporary10 = _E00B(temporary10);
					temporary11 = _E00B(temporary11, 2);
					temporary12 = _E00B(temporary12, 4);
					m_Material.SetTexture(_E051, temporary8);
					m_Material.SetTexture(_E053, temporary10);
					m_Material.SetTexture(_E054, temporary11);
					m_Material.SetTexture(_E055, temporary12);
					if ((bool)this.m__E005 && useBloomStability)
					{
						m_Material.SetTexture(_E052, this.m__E005);
					}
					if ((bool)this.m__E005)
					{
						RenderTexture.ReleaseTemporary(this.m__E005);
						this.m__E005 = null;
					}
					if (useBloomStability)
					{
						this.m__E005 = RenderTexture.GetTemporary(num2, num3, 0, defaultHDRRenderTextureFormat);
						this.m__E005.name = _ED3E._E000(34652);
						this.m__E005.filterMode = FilterMode.Bilinear;
						Graphics.Blit(temporary8, this.m__E005);
					}
					if (useUIBlur && this.m__E006.width > 0 && this.m__E006.height > 0)
					{
						Graphics.Blit(temporary8, this.m__E006);
						Shader.SetGlobalTexture(_E050, this.m__E006);
					}
					if (debugBloomTex)
					{
						m_Material3.EnableKeyword(_ED3E._E000(39885));
						m_Material3.EnableKeyword(_ED3E._E000(33843));
						m_Material3.SetTexture(_E051, temporary8);
						m_Material3.SetTexture(_E053, temporary10);
						m_Material3.SetTexture(_E054, temporary11);
						m_Material3.SetTexture(_E055, temporary12);
						m_Material3.SetFloat(PrismEffects.m__E009, bloomIntensity);
						DebugGraphics.Blit(temporary8, destination, m_Material3, 4);
						goto IL_1084;
					}
				}
				if (useSharpen)
				{
					DebugGraphics.Blit(source, temporary3, m_Material, 0);
					RenderTexture temporary13 = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.ARGB32);
					temporary13.name = _ED3E._E000(34684);
					DebugGraphics.Blit(temporary3, temporary13, m_Material2, 0);
					Graphics.CopyTexture(temporary13, destination);
					RenderTexture.ReleaseTemporary(temporary13);
				}
				else if (forceSecondChromaticPass)
				{
					DebugGraphics.Blit(source, temporary3, m_Material, 0);
					if (useChromaticBlur)
					{
						Vector2 vector = new Vector2(0f, 1f * chromaticBlurWidth);
						RenderTexture temporary14 = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.ARGB32);
						temporary14.name = _ED3E._E000(34667);
						m_Material2.SetVector(_E04B, vector);
						DebugGraphics.Blit(temporary3, temporary14, m_Material2, 12);
						vector = new Vector2(1f * chromaticBlurWidth, 0f);
						RenderTexture temporary15 = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.ARGB32);
						temporary15.name = _ED3E._E000(34706);
						m_Material2.SetVector(_E04B, vector);
						DebugGraphics.Blit(temporary14, temporary15, m_Material2, 12);
						RenderTexture temporary16 = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.ARGB32);
						temporary16.name = _ED3E._E000(34684);
						DebugGraphics.Blit(temporary15, temporary16, m_Material2, 1);
						Graphics.CopyTexture(temporary16, destination);
						RenderTexture.ReleaseTemporary(temporary14);
						RenderTexture.ReleaseTemporary(temporary15);
						RenderTexture.ReleaseTemporary(temporary16);
					}
					else
					{
						RenderTexture temporary17 = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.ARGB32);
						temporary17.name = _ED3E._E000(34684);
						DebugGraphics.Blit(temporary3, temporary17, m_Material2, 1);
						Graphics.CopyTexture(temporary17, destination);
						RenderTexture.ReleaseTemporary(temporary17);
					}
				}
				else
				{
					DebugGraphics.Blit(source, destination, m_Material, 0);
				}
				RenderTexture.ReleaseTemporary(renderTexture);
			}
			else
			{
				if (useRays)
				{
					int width3 = source.width / 2;
					int height3 = source.height / 2;
					RenderTexture temporary18 = RenderTexture.GetTemporary(width3, height3, 0, defaultHDRRenderTextureFormat);
					temporary18.name = _ED3E._E000(34746);
					temporary18.filterMode = FilterMode.Bilinear;
					if (useMedianDoF)
					{
						DebugGraphics.Blit(temporary5, temporary18, m_Material2, 2);
					}
					else
					{
						DebugGraphics.Blit(source, temporary18, m_Material2, 2);
					}
					DebugGraphics.Blit(temporary18, temporary4, m_Material2, 2);
					_E00C(temporary4);
					if (raysShowDebug)
					{
						Graphics.Blit(temporary4, destination);
						RenderTexture.ReleaseTemporary(temporary18);
						goto IL_1084;
					}
					RenderTexture.ReleaseTemporary(temporary18);
				}
				if (useSharpen)
				{
					DebugGraphics.Blit(source, temporary3, m_Material, 0);
					RenderTexture temporary19 = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.ARGB32);
					temporary19.name = _ED3E._E000(34684);
					DebugGraphics.Blit(temporary3, temporary19, m_Material2, 0);
					Graphics.CopyTexture(temporary19, destination);
					RenderTexture.ReleaseTemporary(temporary19);
				}
				else if (forceSecondChromaticPass)
				{
					DebugGraphics.Blit(source, temporary3, m_Material, 0);
					if (useChromaticBlur)
					{
						Vector2 vector2 = new Vector2(0f, 1f * chromaticBlurWidth);
						RenderTexture temporary20 = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.ARGB32);
						temporary20.name = _ED3E._E000(34667);
						m_Material2.SetVector(_E04B, vector2);
						DebugGraphics.Blit(temporary3, temporary20, m_Material2, 12);
						vector2 = new Vector2(1f * chromaticBlurWidth, 0f);
						RenderTexture temporary21 = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.ARGB32);
						temporary21.name = _ED3E._E000(34706);
						m_Material2.SetVector(_E04B, vector2);
						DebugGraphics.Blit(temporary20, temporary21, m_Material2, 12);
						RenderTexture temporary22 = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.ARGB32);
						temporary22.name = _ED3E._E000(34684);
						DebugGraphics.Blit(temporary21, temporary22, m_Material2, 1);
						Graphics.CopyTexture(temporary22, destination);
						RenderTexture.ReleaseTemporary(temporary20);
						RenderTexture.ReleaseTemporary(temporary21);
						RenderTexture.ReleaseTemporary(temporary22);
					}
					else
					{
						RenderTexture temporary23 = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.ARGB32);
						temporary23.name = _ED3E._E000(34684);
						DebugGraphics.Blit(temporary3, temporary23, m_Material2, 1);
						Graphics.CopyTexture(temporary23, destination);
						RenderTexture.ReleaseTemporary(temporary23);
					}
				}
				else
				{
					DebugGraphics.Blit(source, destination, m_Material, 0);
				}
			}
		}
		goto IL_1084;
		IL_1084:
		RenderTexture.ReleaseTemporary(temporary4);
		RenderTexture.ReleaseTemporary(temporary3);
		RenderTexture.ReleaseTemporary(temporary2);
		RenderTexture.ReleaseTemporary(temporary5);
		RenderTexture.ReleaseTemporary(temporary6);
		RenderTexture.ReleaseTemporary(temporary);
		this.m__E008 = false;
	}

	private void _E00C(RenderTexture quarterResMain)
	{
		RenderTexture temporary = RenderTexture.GetTemporary(quarterResMain.width, quarterResMain.height, 0, RenderTextureFormat.ARGB32);
		temporary.name = _ED3E._E000(34807);
		RenderTexture temporary2 = RenderTexture.GetTemporary(quarterResMain.width, quarterResMain.height, 0, RenderTextureFormat.ARGB32);
		temporary2.name = _ED3E._E000(32799);
		DebugGraphics.Blit(quarterResMain, temporary, m_Material3, 0);
		DebugGraphics.Blit(temporary, temporary2, m_Material3, 1);
		m_Material.SetTexture(_E058, temporary2);
		RenderTexture.ReleaseTemporary(temporary);
		RenderTexture.ReleaseTemporary(temporary2);
	}

	public void SetIdentityLut(bool secondary = false)
	{
		int num = 16;
		Color[] array = new Color[num * num * num];
		float num2 = 1f / (1f * (float)num - 1f);
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num; j++)
			{
				for (int k = 0; k < num; k++)
				{
					array[i + j * num + k * num * num] = new Color((float)i * 1f * num2, (float)j * 1f * num2, (float)k * 1f * num2, 1f);
				}
			}
		}
		if (!secondary)
		{
			if ((bool)threeDLookupTex)
			{
				Object.DestroyImmediate(threeDLookupTex);
			}
			threeDLookupTex = new Texture3D(num, num, num, TextureFormat.ARGB32, mipChain: false);
			threeDLookupTex.SetPixels(array);
			threeDLookupTex.Apply();
			basedOnTempTex = "";
		}
		else
		{
			if ((bool)secondaryThreeDLookupTex)
			{
				Object.DestroyImmediate(secondaryThreeDLookupTex);
			}
			secondaryThreeDLookupTex = new Texture3D(num, num, num, TextureFormat.ARGB32, mipChain: false);
			secondaryThreeDLookupTex.SetPixels(array);
			secondaryThreeDLookupTex.Apply();
			secondaryBasedOnTempTex = "";
		}
	}

	public bool ValidDimensions(Texture2D tex2d)
	{
		if (!tex2d)
		{
			return false;
		}
		if (tex2d.height != Mathf.FloorToInt(Mathf.Sqrt(tex2d.width)))
		{
			return false;
		}
		return true;
	}

	public void AddDependantEffectExposure(PrismEffects pe)
	{
		if (this.m__E002 == null)
		{
			this.m__E002 = new List<PrismEffects>();
		}
		this.m__E002.Add(pe);
		pe.m_MasterEffectExposure = this;
	}

	public void Convert(Texture2D temp2DTex, bool secondaryLut = false)
	{
		if ((bool)temp2DTex)
		{
			int num = temp2DTex.width * temp2DTex.height;
			num = temp2DTex.height;
			if (!ValidDimensions(temp2DTex))
			{
				Debug.LogWarning(_ED3E._E000(32828) + temp2DTex.name + _ED3E._E000(32810));
				if (!secondaryLut)
				{
					secondaryBasedOnTempTex = "";
				}
				else
				{
					basedOnTempTex = "";
				}
				return;
			}
			Color[] pixels = temp2DTex.GetPixels();
			Color[] array = new Color[pixels.Length];
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num; j++)
				{
					for (int k = 0; k < num; k++)
					{
						int num2 = num - j - 1;
						array[i + j * num + k * num * num] = pixels[k * num + i + num2 * num * num];
					}
				}
			}
			if (!secondaryLut)
			{
				if ((bool)threeDLookupTex)
				{
					Object.DestroyImmediate(threeDLookupTex);
				}
				threeDLookupTex = new Texture3D(num, num, num, TextureFormat.ARGB32, mipChain: false);
				threeDLookupTex.SetPixels(array);
				threeDLookupTex.Apply();
				basedOnTempTex = temp2DTex.name;
				twoDLookupTex = temp2DTex;
			}
			else
			{
				if ((bool)secondaryThreeDLookupTex)
				{
					Object.DestroyImmediate(secondaryThreeDLookupTex);
				}
				secondaryThreeDLookupTex = new Texture3D(num, num, num, TextureFormat.ARGB32, mipChain: false);
				secondaryThreeDLookupTex.SetPixels(array);
				secondaryThreeDLookupTex.Apply();
				secondaryBasedOnTempTex = temp2DTex.name;
				secondaryTwoDLookupTex = temp2DTex;
			}
		}
		else
		{
			Debug.LogError(_ED3E._E000(32847));
			base.enabled = false;
		}
	}
}
