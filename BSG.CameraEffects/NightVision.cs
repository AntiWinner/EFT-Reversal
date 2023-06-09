using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.InventoryLogic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

namespace BSG.CameraEffects;

[RequireComponent(typeof(Camera))]
public class NightVision : MonoBehaviour, _E442
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public float v;

		public Func<Keyframe, bool> _003C_003E9__0;

		internal bool _E000(Keyframe key)
		{
			return key.value > v;
		}
	}

	public Shader Shader;

	public float Intensity;

	public Texture Noise;

	public Texture Mask;

	public float MaskSize = 1f;

	public float NoiseIntensity;

	public float NoiseScale;

	[ColorUsage(false)]
	public Color Color;

	public TextureMask TextureMask;

	public MonoBehaviour[] SwitchComponentsOn;

	public MonoBehaviour[] SwitchComponentsOff;

	public AnimationCurve BlackFlashGoingToOn;

	public AnimationCurve BlackFlashGoingToOff;

	public AudioClip SwitchOn;

	public AudioClip SwitchOff;

	public Texture ThermalMaskTexture;

	public Texture AnvisMaskTexture;

	public Texture BinocularMaskTexture;

	public Texture GasMaskTexture;

	public Texture OldMonocularMaskTexture;

	public float ambientFactor = 1.2f;

	[SerializeField]
	private bool _on;

	private static readonly int m__E000 = Shader.PropertyToID(_ED3E._E000(36528));

	private static readonly int m__E001 = Shader.PropertyToID(_ED3E._E000(35970));

	private static readonly int m__E002 = Shader.PropertyToID(_ED3E._E000(42993));

	private static readonly int m__E003 = Shader.PropertyToID(_ED3E._E000(92674));

	private static readonly int m__E004 = Shader.PropertyToID(_ED3E._E000(16412));

	private static readonly int _E005 = Shader.PropertyToID(_ED3E._E000(92726));

	private static readonly int _E006 = Shader.PropertyToID(_ED3E._E000(92709));

	private static readonly string _E007 = _ED3E._E000(92760);

	private Material _E008;

	private Material _E009;

	private int _E00A;

	private int _E00B;

	private _E426 _E00C;

	[CanBeNull]
	private SSAA _E00D;

	private bool? _E00E;

	private bool _E00F = true;

	private CommandBuffer _E010;

	private Camera _E011;

	private bool _E012;

	private Vector4 _E013;

	private Color _E014;

	private Material _E015
	{
		get
		{
			if (!(_E008 == null))
			{
				return _E008;
			}
			return _E008 = new Material(Shader);
		}
	}

	public bool On => _on;

	public bool InProcessSwitching => _E00C.InProcess;

	private Color _E016 => Color * (1f - 2f * _E00C.Value);

	private void Awake()
	{
		_E000();
	}

	private void _E000()
	{
		_E00C = new _E426
		{
			SwitchingOn = BlackFlashGoingToOn,
			SwitchingOff = BlackFlashGoingToOff
		};
		_E00D = GetComponent<SSAA>();
		_E008 = new Material(Shader);
		_E00C.AddCase(_E002(_E00C.SwitchingOn), delegate
		{
			_E001(on: true);
		}, switchingOn: true);
		_E00C.AddCase(_E002(_E00C.SwitchingOff), delegate
		{
			_E001(on: false);
		}, switchingOn: false);
		_E010 = new CommandBuffer
		{
			name = _ED3E._E000(92690)
		};
		_E011 = GetComponent<Camera>();
		_E011.RemoveCommandBuffer(CameraEvent.BeforeImageEffects, _E010);
		_E011.AddCommandBuffer(CameraEvent.BeforeImageEffects, _E010);
		_E012 = _E00D != null;
		ApplySettings();
	}

	public void SetMask(NightVisionComponent.EMask mask)
	{
		switch (mask)
		{
		case NightVisionComponent.EMask.Thermal:
			Mask = ThermalMaskTexture;
			break;
		case NightVisionComponent.EMask.Anvis:
			Mask = AnvisMaskTexture;
			break;
		case NightVisionComponent.EMask.Binocular:
			Mask = BinocularMaskTexture;
			break;
		case NightVisionComponent.EMask.GasMask:
			Mask = GasMaskTexture;
			break;
		case NightVisionComponent.EMask.OldMonocular:
			Mask = OldMonocularMaskTexture;
			break;
		default:
			throw new ArgumentOutOfRangeException(_ED3E._E000(83773), mask, null);
		}
	}

	public void ApplySettings()
	{
		float num = NoiseScale * (float)Screen.height / (float)Noise.height;
		_E013 = new Vector4(num * (float)Screen.width / (float)Screen.height, num, 0f, 0f);
		_E015.SetColor(NightVision.m__E000, _E016);
		_E015.SetFloat(NightVision.m__E001, Intensity);
		_E015.SetFloat(NightVision.m__E004, NoiseIntensity);
		_E015.SetVector(NightVision.m__E003, _E013);
		_E015.SetTexture(NightVision.m__E002, Noise);
		_E015.EnableKeyword(_E007);
		if (_E012)
		{
			_E00D.SetNightVisionNoiseParams(Noise, NoiseIntensity, _E013, _E016);
			_E015.DisableKeyword(_E007);
		}
		if (!(TextureMask == null))
		{
			TextureMask.Mask = Mask;
			TextureMask.Size = MaskSize;
			TextureMask.ApplySettings();
		}
	}

	public void StartSwitch(bool on)
	{
		if (_E00C == null)
		{
			Debug.LogError(_ED3E._E000(83770));
		}
		else if (_E00F)
		{
			_E00F = false;
			_E001(on);
			_E00C.ForceSwitch(on);
		}
		else if (_E00C.InProcess)
		{
			_E00E = on;
		}
		else
		{
			_E00C.Switch(on);
		}
	}

	public void FastForwardSwitch()
	{
		if (_E00C.InProcess)
		{
			_E00C.Process(float.PositiveInfinity);
		}
		Update();
	}

	private void OnDisable()
	{
		_E010.Clear();
		Shader.SetGlobalFloat(_E005, 0f);
		if (_E012)
		{
			_E00D.SetNightVisionEnabled(_on);
			_E015.SetKeyword(_E007, !_E00D.IsUsingDLSS() && !_E00D.IsUsingFSR2());
		}
	}

	private void Update()
	{
		if (_E00C.InProcess)
		{
			_E00C.Process(Time.deltaTime);
		}
		else if (_E00E.HasValue)
		{
			_E00C.Switch(_E00E.Value);
			_E00E = null;
		}
		_E015.SetColor(NightVision.m__E000, _E016);
		if (_E012)
		{
			_E00D.SetNightVisionEnabled(_on);
			_E015.SetKeyword(_E007, !_E00D.IsUsingDLSS() && !_E00D.IsUsingFSR2());
		}
	}

	private void OnPreCull()
	{
		Shader.SetGlobalFloat(_E005, On ? 1 : 0);
		_E010.Clear();
		if (On)
		{
			RenderTargetIdentifier renderTargetIdentifier = (_E00D ? _E00D.GetRTIdentifier() : ((RenderTargetIdentifier)BuiltinRenderTextureType.CameraTarget));
			Singleton<LevelSettings>.Instance.AmbientType = AmbientType.NightVision;
			_E010.GetTemporaryRT(_E006, -1, -1);
			_E010.Blit(renderTargetIdentifier, _E006, _E015);
			_E010.Blit(_E006, renderTargetIdentifier);
			_E010.ReleaseTemporaryRT(_E006);
		}
	}

	private void OnPostRender()
	{
		Singleton<LevelSettings>.Instance.AmbientType = AmbientType.Default;
	}

	private void _E001(bool on)
	{
		_on = on;
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
	}

	private static float _E002(AnimationCurve curve)
	{
		float result = 0f;
		float v = float.MinValue;
		foreach (Keyframe item in curve.keys.Where((Keyframe key) => key.value > v))
		{
			result = item.time;
			v = item.value;
		}
		return result;
	}

	private void OnDestroy()
	{
		Shader.SetGlobalFloat(_E005, 0f);
	}

	[CompilerGenerated]
	private void _E003()
	{
		_E001(on: true);
	}

	[CompilerGenerated]
	private void _E004()
	{
		_E001(on: false);
	}
}
