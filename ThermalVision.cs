using System;
using System.Runtime.CompilerServices;
using BSG.CameraEffects;
using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class ThermalVision : MonoBehaviour, _E442
{
	public bool On;

	public bool IsNoisy;

	public bool IsFpsStuck;

	public bool IsMotionBlurred;

	public bool IsGlitch;

	public bool IsPixelated;

	public bool ZeroedUnsharpRadius;

	public ThermalVisionUtilities ThermalVisionUtilities;

	public StuckFPSUtilities StuckFpsUtilities;

	public MotionBlurUtilities MotionBlurUtilities;

	public GlitchUtilities GlitchUtilities;

	public PixelationUtilities PixelationUtilities;

	public TextureMask TextureMask;

	public MonoBehaviour[] SwitchComponentsOn;

	public MonoBehaviour[] SwitchComponentsOff;

	public float ChromaticAberrationThermalShift = 0.013f;

	public AnimationCurve BlackFlashGoingToOn;

	public AnimationCurve BlackFlashGoingToOff;

	public AudioClip SwitchOn;

	public AudioClip SwitchOff;

	[Header("Unsharp Mask")]
	public float UnsharpRadiusBlur = 5f;

	public float UnsharpBias = 2f;

	private _E426 m__E000;

	private bool? m__E001;

	private bool m__E002 = true;

	private CommandBuffer m__E003;

	private CommandBuffer m__E004;

	private Material m__E005;

	private Camera _E006;

	private _E418 _E007;

	private _E416 _E008;

	private _E415 _E009;

	private _E417 _E00A;

	private VolumetricLightRenderer _E00B;

	private SSAA _E00C;

	private SSAAOptic _E00D;

	private ChromaticAberration _E00E;

	private float _E00F;

	private float _E010;

	private static readonly int _E011 = Shader.PropertyToID(_ED3E._E000(86433));

	private static readonly int _E012 = Shader.PropertyToID(_ED3E._E000(16726));

	private static readonly int _E013 = Shader.PropertyToID(_ED3E._E000(83799));

	private static readonly int _E014 = Shader.PropertyToID(_ED3E._E000(83777));

	private static readonly int _E015 = Shader.PropertyToID(_ED3E._E000(42993));

	private static readonly int _E016 = Shader.PropertyToID(_ED3E._E000(16412));

	private static readonly int _E017 = Shader.PropertyToID(_ED3E._E000(83818));

	private static readonly int _E018 = Shader.PropertyToID(_ED3E._E000(83869));

	private static readonly int _E019 = Shader.PropertyToID(_ED3E._E000(83856));

	private static readonly int _E01A = Shader.PropertyToID(_ED3E._E000(83841));

	private static readonly int _E01B = Shader.PropertyToID(_ED3E._E000(83893));

	public bool InProcessSwitching => this.m__E000.InProcess;

	private void Awake()
	{
		_E000();
	}

	private void _E000()
	{
		this.m__E000 = new _E426
		{
			SwitchingOn = BlackFlashGoingToOn,
			SwitchingOff = BlackFlashGoingToOff
		};
		this.m__E000.AddCase(_E003(this.m__E000.SwitchingOn), delegate
		{
			_E001(on: true);
		}, switchingOn: true);
		this.m__E000.AddCase(_E003(this.m__E000.SwitchingOff), delegate
		{
			_E001(on: false);
		}, switchingOn: false);
		_E006 = GetComponent<Camera>();
		_E007 = new _E418(this);
		_E008 = new _E416(this);
		_E009 = new _E415(this);
		_E00A = new _E417(this, _E006);
		_E00C = GetComponent<SSAA>();
		_E00D = GetComponent<SSAAOptic>();
		_E00B = _E006.GetComponent<VolumetricLightRenderer>();
		_E00E = _E006.GetComponent<ChromaticAberration>();
		_E00F = _E00E.Shift;
		Shader shader = _E3AC.Find(_ED3E._E000(38602));
		this.m__E005 = new Material(shader);
		if (this.m__E003 == null)
		{
			this.m__E003 = new CommandBuffer
			{
				name = _ED3E._E000(83743)
			};
		}
		_E006.RemoveCommandBuffer(CameraEvent.AfterForwardAlpha, this.m__E003);
		_E006.AddCommandBuffer(CameraEvent.AfterForwardAlpha, this.m__E003);
		if (this.m__E004 == null)
		{
			this.m__E004 = new CommandBuffer
			{
				name = _ED3E._E000(83726)
			};
		}
		_E006.RemoveCommandBuffer(CameraEvent.BeforeForwardAlpha, this.m__E004);
		_E006.AddCommandBuffer(CameraEvent.BeforeForwardAlpha, this.m__E004);
	}

	private void Update()
	{
		if (_E006.enabled)
		{
			if (this.m__E000.InProcess)
			{
				this.m__E000.Process(Time.deltaTime);
			}
			else if (this.m__E001.HasValue)
			{
				this.m__E000.Switch(this.m__E001.Value);
				this.m__E001 = null;
			}
		}
	}

	public void FastForwardSwitch()
	{
		if (this.m__E000.InProcess)
		{
			this.m__E000.Process(float.PositiveInfinity);
		}
		Update();
	}

	private void OnDisable()
	{
		_E00B.IsOn = true;
		this.m__E003?.Clear();
		this.m__E004?.Clear();
		Shader.SetGlobalFloat(_E019, 0f);
	}

	private void OnPreCull()
	{
		if (_E00B.IsOn == On)
		{
			_E00B.IsOn = !On;
		}
		if (!On)
		{
			_E008.IsNeedClear = true;
			if (_E00C != null)
			{
				_E00C.SetThermalVisionNoiseParams(null, 0f, Vector4.one);
			}
			return;
		}
		RenderTargetIdentifier renderTargetIdentifier = (_E00D ? ((RenderTargetIdentifier)BuiltinRenderTextureType.CameraTarget) : (_E00C ? _E00C.GetRTIdentifier() : ((RenderTargetIdentifier)BuiltinRenderTextureType.CameraTarget)));
		this.m__E004.Blit(BuiltinRenderTextureType.GBuffer0, BuiltinRenderTextureType.CameraTarget);
		Shader.SetGlobalFloat(_E019, 1f);
		_E010 = QualitySettings.shadowDistance;
		QualitySettings.shadowDistance = 0f;
		SetMaterialProperties();
		this.m__E003.GetTemporaryRT(_E011, -1, -1);
		this.m__E003.SetRenderTarget(BuiltinRenderTextureType.CurrentActive, BuiltinRenderTextureType.CurrentActive);
		if (_E00C != null && (_E00C.IsUsingDLSS() || _E00C.IsUsingFSR2()))
		{
			this.m__E003.Blit(renderTargetIdentifier, _E011, this.m__E005, 0);
			this.m__E003.Blit(_E011, renderTargetIdentifier, this.m__E005, 2);
			_E00C.SetThermalVisionNoiseParams(ThermalVisionUtilities.NoiseParameters.NoiseTex, IsNoisy ? ThermalVisionUtilities.NoiseParameters.NoiseIntensity : 0f, Vector4.one);
		}
		else
		{
			this.m__E003.Blit(renderTargetIdentifier, _E011);
			this.m__E003.Blit(_E011, renderTargetIdentifier, this.m__E005, 0);
			this.m__E003.Blit(renderTargetIdentifier, _E011, this.m__E005, 2);
			this.m__E003.Blit(_E011, renderTargetIdentifier, this.m__E005, 1);
		}
		this.m__E003.ReleaseTemporaryRT(_E011);
		if (IsFpsStuck)
		{
			_E007.UpdateTexture(this.m__E003, _E006);
		}
		if (IsMotionBlurred)
		{
			_E008.UpdateTexture(this.m__E003, _E006, _E00C, _E00D, renderTargetIdentifier);
		}
		if (IsGlitch)
		{
			_E009.UpdateTexture(this.m__E003);
		}
		if (IsPixelated)
		{
			_E00A.UpdateTexture(this.m__E003);
		}
	}

	private void OnPostRender()
	{
		Shader.SetGlobalFloat(_E019, 0f);
		this.m__E004.Clear();
		this.m__E003.Clear();
		if (On)
		{
			QualitySettings.shadowDistance = _E010;
		}
	}

	public void SetMask(NightVisionComponent.EMask mask)
	{
		switch (mask)
		{
		case NightVisionComponent.EMask.Thermal:
			ThermalVisionUtilities.MaskDescription.Mask = ThermalVisionUtilities.MaskDescription.ThermalMaskTexture;
			break;
		case NightVisionComponent.EMask.Anvis:
			ThermalVisionUtilities.MaskDescription.Mask = ThermalVisionUtilities.MaskDescription.AnvisMaskTexture;
			break;
		case NightVisionComponent.EMask.Binocular:
			ThermalVisionUtilities.MaskDescription.Mask = ThermalVisionUtilities.MaskDescription.BinocularMaskTexture;
			break;
		case NightVisionComponent.EMask.GasMask:
			ThermalVisionUtilities.MaskDescription.Mask = ThermalVisionUtilities.MaskDescription.GasMaskTexture;
			break;
		case NightVisionComponent.EMask.OldMonocular:
			ThermalVisionUtilities.MaskDescription.Mask = ThermalVisionUtilities.MaskDescription.OldMonocularMaskTexture;
			break;
		default:
			throw new ArgumentOutOfRangeException(_ED3E._E000(83773), mask, null);
		}
	}

	private void _E001(bool on)
	{
		On = on;
		TextureMask.TryToEnable(this, on);
		MonoBehaviour[] switchComponentsOn = SwitchComponentsOn;
		for (int i = 0; i < switchComponentsOn.Length; i++)
		{
			switchComponentsOn[i].enabled = on;
		}
		switchComponentsOn = SwitchComponentsOff;
		for (int i = 0; i < switchComponentsOn.Length; i++)
		{
			switchComponentsOn[i].enabled = !on;
		}
		_E00E.Shift = (on ? ChromaticAberrationThermalShift : _E00F);
	}

	public void Toggle()
	{
		StartSwitch(!this.m__E000.On);
	}

	public void StartSwitch(bool on)
	{
		if (this.m__E000 == null)
		{
			Debug.LogError(_ED3E._E000(83770));
		}
		else if (this.m__E002)
		{
			this.m__E002 = false;
			_E001(on);
			this.m__E000.ForceSwitch(on);
		}
		else if (this.m__E000.InProcess)
		{
			this.m__E001 = on;
		}
		else
		{
			this.m__E000.Switch(on);
		}
	}

	public void SetMaterialProperties()
	{
		this.m__E005.SetTexture(_E012, _E002());
		this.m__E005.SetFloat(_E013, ThermalVisionUtilities.ValuesCoefs.MainTexColorCoef);
		this.m__E005.SetFloat(_E014, ThermalVisionUtilities.ValuesCoefs.MinimumTemperatureValue);
		this.m__E005.SetTexture(_E015, ThermalVisionUtilities.NoiseParameters.NoiseTex);
		this.m__E005.SetFloat(_E016, IsNoisy ? ThermalVisionUtilities.NoiseParameters.NoiseIntensity : 0f);
		this.m__E005.SetFloat(_E017, ThermalVisionUtilities.DepthFade);
		this.m__E005.SetFloat(_E018, ThermalVisionUtilities.ValuesCoefs.RampShift);
		float value = (ZeroedUnsharpRadius ? 0f : (UnsharpRadiusBlur / 2f));
		this.m__E005.SetFloat(_E01A, value);
		this.m__E005.SetFloat(_E01B, UnsharpBias);
		if (TextureMask != null)
		{
			TextureMask.Mask = ThermalVisionUtilities.MaskDescription.Mask;
			TextureMask.Size = ThermalVisionUtilities.MaskDescription.MaskSize;
			TextureMask.ApplySettings();
		}
	}

	private Texture _E002()
	{
		for (int i = 0; i < ThermalVisionUtilities.RampTexPalletteConnectors.Count; i++)
		{
			RampTexPalletteConnector rampTexPalletteConnector = ThermalVisionUtilities.RampTexPalletteConnectors[i];
			if (rampTexPalletteConnector.SelectablePalette == ThermalVisionUtilities.CurrentRampPalette)
			{
				return rampTexPalletteConnector.Texture;
			}
		}
		return null;
	}

	private void OnDestroy()
	{
		if (this.m__E003 != null)
		{
			_E006.RemoveCommandBuffer(CameraEvent.AfterForwardAlpha, this.m__E003);
		}
		if (this.m__E004 != null)
		{
			_E006.RemoveCommandBuffer(CameraEvent.BeforeForwardAlpha, this.m__E004);
		}
		_E007?.Destroy();
		_E008?.Dispose();
		_E007 = null;
		_E008 = null;
		Shader.SetGlobalFloat(_E019, 0f);
	}

	private static float _E003(AnimationCurve curve)
	{
		float result = 0f;
		float num = float.MinValue;
		Keyframe[] keys = curve.keys;
		for (int i = 0; i < keys.Length; i++)
		{
			Keyframe keyframe = keys[i];
			if (keyframe.value > num)
			{
				result = keyframe.time;
				num = keyframe.value;
			}
		}
		return result;
	}

	[CompilerGenerated]
	private void _E004()
	{
		_E001(on: true);
	}

	[CompilerGenerated]
	private void _E005()
	{
		_E001(on: false);
	}
}
