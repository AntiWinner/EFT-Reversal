using BSG.CameraEffects;
using EFT.PostEffects;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityStandardAssets.ImageEffects;

namespace EFT.CameraControl;

public class OpticComponentUpdater : MonoBehaviour
{
	private Transform m__E000;

	private Camera _E001;

	private ChromaticAberration _E002;

	private BloomOptimized _E003;

	private ThermalVision _E004;

	private CC_FastVignette _E005;

	private UltimateBloom _E006;

	private Tonemapping _E007;

	private NightVision _E008;

	private Fisheye _E009;

	private OpticCullingMask _E00A;

	private CustomGlobalFog _E00B;

	private TOD_Scattering _E00C;

	private Undithering _E00D;

	private PostProcessLayer _E00E;

	private VolumetricLightRenderer _E00F;

	private CustomGlobalFog _E010;

	private TOD_Scattering _E011;

	private Undithering _E012;

	private PostProcessLayer _E013;

	private VolumetricLightRenderer _E014;

	private CameraLodBiasController _E015;

	private int _E016;

	public Camera MainCamera => _E8A8.Instance.Camera;

	private void Awake()
	{
		_E00B = GetComponent<CustomGlobalFog>();
		_E011 = GetComponent<TOD_Scattering>();
		_E012 = GetComponent<Undithering>();
		_E013 = GetComponent<PostProcessLayer>();
		_E014 = GetComponent<VolumetricLightRenderer>();
		_E00A = GetComponent<OpticCullingMask>();
		_E001 = GetComponent<Camera>();
		_E014 = GetComponent<VolumetricLightRenderer>();
		_E002 = GetComponent<ChromaticAberration>();
		_E003 = GetComponent<BloomOptimized>();
		_E004 = GetComponent<ThermalVision>();
		_E005 = GetComponent<CC_FastVignette>();
		_E006 = GetComponent<UltimateBloom>();
		_E007 = GetComponent<Tonemapping>();
		_E008 = GetComponent<NightVision>();
		_E009 = GetComponent<Fisheye>();
		_E015 = base.gameObject.GetOrAddComponent<CameraLodBiasController>();
		_E8A8.Instance.OnCameraChanged += _E000;
		_E000();
	}

	public void SetPivot(Transform pivot)
	{
		this.m__E000 = pivot;
	}

	public void CopyComponentFromOptic(OpticSight opticSight)
	{
		this.m__E000 = opticSight.TemplateCamera.transform;
		int instanceID = opticSight.gameObject.GetInstanceID();
		if (_E016 != instanceID)
		{
			bool flag = (bool)opticSight.ThermalVision && opticSight.ThermalVision.enabled;
			_E016 = instanceID;
			_E001.fieldOfView = opticSight.TemplateCamera.fieldOfView;
			_E001.nearClipPlane = opticSight.TemplateCamera.nearClipPlane;
			_E001.farClipPlane = (flag ? opticSight.TemplateCamera.farClipPlane : _E8A8.Instance.Camera.farClipPlane);
			_E00A.enabled = (bool)opticSight.OpticCullingMask && opticSight.OpticCullingMask.enabled;
			if (_E00A.enabled)
			{
				_E00A._maskScale = opticSight.OpticCullingMask._maskScale;
				_E00A.UpdateParameters();
			}
			_E002.enabled = (bool)opticSight.ChromaticAberration && opticSight.ChromaticAberration.enabled;
			if (_E002.enabled)
			{
				_E002.Aniso = opticSight.ChromaticAberration.Aniso;
				_E002.Shift = opticSight.ChromaticAberration.Shift;
			}
			_E003.enabled = (bool)opticSight.BloomOptimized && opticSight.BloomOptimized.enabled;
			if (_E003.enabled)
			{
				_E003.intensity = opticSight.BloomOptimized.intensity;
				_E003.threshold = opticSight.BloomOptimized.threshold;
				_E003.blurSize = opticSight.BloomOptimized.blurSize;
			}
			_E004.enabled = flag;
			if (_E004.enabled)
			{
				_E004.On = opticSight.ThermalVision.On;
				_E004.IsGlitch = opticSight.ThermalVision.IsGlitch;
				_E004.IsPixelated = opticSight.ThermalVision.IsPixelated;
				_E004.IsNoisy = opticSight.ThermalVision.IsNoisy;
				_E004.IsMotionBlurred = opticSight.ThermalVision.IsMotionBlurred;
				_E004.IsFpsStuck = opticSight.ThermalVision.IsFpsStuck;
				_E004.ThermalVisionUtilities = opticSight.ThermalVision.ThermalVisionUtilities;
				_E004.StuckFpsUtilities = opticSight.ThermalVision.StuckFpsUtilities;
				_E004.MotionBlurUtilities = opticSight.ThermalVision.MotionBlurUtilities;
				_E004.GlitchUtilities = opticSight.ThermalVision.GlitchUtilities;
				_E004.PixelationUtilities = opticSight.ThermalVision.PixelationUtilities;
				_E004.ChromaticAberrationThermalShift = opticSight.ThermalVision.ChromaticAberrationThermalShift;
				_E004.UnsharpBias = opticSight.ThermalVision.UnsharpBias;
				_E004.UnsharpRadiusBlur = opticSight.ThermalVision.UnsharpRadiusBlur;
			}
			_E005.enabled = (bool)opticSight.FastVignette && opticSight.FastVignette.enabled;
			_E006.enabled = (bool)opticSight.UltimateBloom && opticSight.UltimateBloom.enabled;
			_E009.enabled = (bool)opticSight.Fisheye && opticSight.Fisheye.enabled;
			_E007.enabled = (bool)opticSight.Tonemapping && opticSight.Tonemapping.enabled;
			if (_E007.enabled)
			{
				_E007.white = opticSight.Tonemapping.white;
				_E007.adaptionSpeed = opticSight.Tonemapping.adaptionSpeed;
				_E007.exposureAdjustment = opticSight.Tonemapping.exposureAdjustment;
				_E007.middleGrey = opticSight.Tonemapping.middleGrey;
				_E007.adaptiveTextureSize = opticSight.Tonemapping.adaptiveTextureSize;
				_E007.type = opticSight.Tonemapping.type;
			}
			_E008.enabled = (bool)opticSight.NightVision && opticSight.NightVision.enabled;
			if (_E008.enabled)
			{
				_E008.Intensity = opticSight.NightVision.Intensity;
				_E008.MaskSize = opticSight.NightVision.MaskSize;
				_E008.NoiseIntensity = opticSight.NightVision.NoiseIntensity;
				_E008.NoiseScale = opticSight.NightVision.NoiseScale;
				_E008.Color = opticSight.NightVision.Color;
			}
			_E015.enabled = (bool)opticSight.CameraLodBiasController && opticSight.CameraLodBiasController.enabled;
			if (_E015.enabled)
			{
				_E015.LodBiasFactor = opticSight.CameraLodBiasController.LodBiasFactor;
				_E015.SetMaxFov(_E001.fieldOfView);
			}
		}
	}

	private void _E000()
	{
		_E010 = MainCamera.GetComponent<CustomGlobalFog>();
		_E00C = MainCamera.GetComponent<TOD_Scattering>();
		_E00D = MainCamera.GetComponent<Undithering>();
		_E00E = MainCamera.GetComponent<PostProcessLayer>();
		_E00F = MainCamera.GetComponent<VolumetricLightRenderer>();
	}

	private void LateUpdate()
	{
		if (!(MainCamera == null) && !(this.m__E000 == null))
		{
			base.transform.position = this.m__E000.position;
			base.transform.rotation = this.m__E000.rotation;
			_E001.useOcclusionCulling = MainCamera.useOcclusionCulling;
			if (_E012 != null && _E00D != null)
			{
				_E012.enabled = _E00D.enabled;
				_E012.shader = _E00D.shader;
			}
			if (_E014 != null && _E00F != null)
			{
				_E014.enabled = _E00F.enabled;
				_E014.DefaultSpotCookie = _E00F.DefaultSpotCookie;
				_E014.Resolution = _E00F.Resolution;
			}
			if (_E011 != null && _E00C != null)
			{
				_E011.enabled = _E00C.enabled;
				_E011.DitheringTexture = _E00C.DitheringTexture;
				_E011.GlobalDensity = _E00C.GlobalDensity;
				_E011.HeightFalloff = _E00C.HeightFalloff;
				_E011.SunrizeGlow = _E00C.SunrizeGlow;
				_E011.ZeroLevel = _E00C.ZeroLevel;
			}
			if (_E00B != null && _E010 != null)
			{
				_E00B.enabled = _E010.enabled;
				_E00B.shader = _E010.shader;
				_E00B.BlendMode = _E010.BlendMode;
				_E00B.FogColor = _E010.FogColor;
				_E00B.FogStart = _E010.FogStart;
				_E00B.FogStrength = _E010.FogStrength;
				_E00B.FogToplength = _E010.FogToplength;
				_E00B.FogY = _E010.FogY;
				_E00B.FuncSoftness = _E010.FuncSoftness;
				_E00B.FuncStart = _E010.FuncStart;
				_E00B.DirectionDifferenceThreshold = _E010.DirectionDifferenceThreshold;
				_E00B.FogMaxDistance = _E010.FogMaxDistance;
				_E00B.FogTopIntensity = _E010.FogTopIntensity;
			}
			if (_E013 != null && _E00E != null)
			{
				_E013.enabled = _E00E.enabled;
				_E013.antialiasingMode = _E00E.antialiasingMode;
				_E013.temporalAntialiasing.jitterSpread = _E00E.temporalAntialiasing.jitterSpread;
				_E013.temporalAntialiasing.stationaryBlending = _E00E.temporalAntialiasing.stationaryBlending;
				_E013.temporalAntialiasing.motionBlending = _E00E.temporalAntialiasing.motionBlending;
				_E013.temporalAntialiasing.sharpness = _E00E.temporalAntialiasing.sharpness;
			}
		}
	}

	private void OnDestroy()
	{
		_E8A8.Instance.OnCameraChanged -= _E000;
	}
}
