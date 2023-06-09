using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Systems.Effects;
using Comfort.Common;
using EFT;
using EFT.Ballistics;
using EFT.CameraControl;
using EFT.EnvironmentEffect;
using EFT.HealthSystem;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public sealed class EffectsController : MonoBehaviour
{
	private abstract class _E000
	{
		public List<_E992> ActiveEffects = new List<_E992>();

		public Type[] ValidTypes;

		public float MaxEffectValue;

		public bool Enabled;

		protected EffectsController _E000;

		public virtual void AddEffect(_E992 effect)
		{
			if (ValidTypes.Contains(effect.Type))
			{
				ActiveEffects.Add(effect);
				Toggle(value: true);
			}
		}

		public virtual void DeleteEffect(_E992 effect)
		{
			if (ActiveEffects.Remove(effect) && ActiveEffects.Count == 0)
			{
				UpdateAmount();
				Toggle(value: false);
			}
		}

		public virtual void StimulatorBuff(_E986 buff)
		{
		}

		protected virtual float _E003()
		{
			float num = 0f;
			foreach (_E992 activeEffect in ActiveEffects)
			{
				num += activeEffect.CurrentStrength;
			}
			return num;
		}

		public virtual void Toggle(bool value)
		{
			Enabled = value;
		}

		public abstract void UpdateAmount();
	}

	private sealed class _E001 : _E000
	{
		private CC_FastVignette m__E001;

		public _E001(EffectsController effectsController, CC_FastVignette cameraEffect, float maxEffectValue, params Type[] types)
		{
			_E000 = effectsController;
			MaxEffectValue = maxEffectValue;
			m__E001 = cameraEffect;
			ValidTypes = types;
		}

		public override void UpdateAmount()
		{
			float value = _E003();
			m__E001.darkness = Mathf.Clamp01(value) * MaxEffectValue * _E000.FastVineteFlicker.Evaluate(Time.realtimeSinceStartup);
		}

		public override void Toggle(bool value)
		{
			base.Toggle(value);
			m__E001.enabled = value;
		}
	}

	private sealed class _E002 : _E000
	{
		private CC_DoubleVision _E001;

		public _E002(EffectsController effectsController, CC_DoubleVision cameraEffect, float maxEffectValue, params Type[] types)
		{
			_E000 = effectsController;
			MaxEffectValue = maxEffectValue;
			_E001 = cameraEffect;
			ValidTypes = types;
		}

		public override void UpdateAmount()
		{
			float value = _E003();
			_E001.amount = Mathf.Clamp01(value) * MaxEffectValue;
		}

		public override void Toggle(bool value)
		{
			base.Toggle(value);
			_E001.enabled = value;
		}
	}

	private sealed class _E003 : _E000
	{
		private CC_HueFocus _E001;

		public _E003(EffectsController effectsController, CC_HueFocus cameraEffect, float maxEffectValue, params Type[] types)
		{
			_E000 = effectsController;
			MaxEffectValue = maxEffectValue;
			_E001 = cameraEffect;
			ValidTypes = types;
		}

		public override void UpdateAmount()
		{
			float value = _E003();
			_E001.amount = Mathf.Clamp01(value) * MaxEffectValue;
		}

		public override void Toggle(bool value)
		{
			_E001.enabled = value;
		}
	}

	private sealed class _E004 : _E000
	{
		private CC_RadialBlur _E001;

		private float _E002;

		private new List<_E992> _E003 = new List<_E992>();

		private float m__E004 = 1f;

		public _E004(EffectsController effectsController, CC_RadialBlur cameraEffect, float maxEffectValue, params Type[] types)
		{
			_E000 = effectsController;
			MaxEffectValue = maxEffectValue;
			_E001 = cameraEffect;
			ValidTypes = types;
		}

		public override void UpdateAmount()
		{
			bool flag = false;
			foreach (_E992 activeEffect in ActiveEffects)
			{
				if (!(activeEffect is _E9B0) || activeEffect.State == EEffectState.Residued)
				{
					continue;
				}
				flag = true;
				break;
			}
			float value = _E003();
			value = Mathf.Clamp01(value);
			value = ((!flag) ? Mathf.Lerp(_E001.amount / MaxEffectValue, value, 0.15f) : (value + (Mathf.Cos(Time.time * 7f) + Mathf.Sin(Time.time * 3f)) * 0.2f));
			if (_E003.Count > 0)
			{
				value *= 1f - _E003.Max((_E992 x) => x.CurrentStrength);
			}
			_E001.amount = value * MaxEffectValue * m__E004;
		}

		public override void AddEffect(_E992 effect)
		{
			if (effect is _E9B1)
			{
				_E003.Add(effect);
			}
			else
			{
				base.AddEffect(effect);
			}
		}

		public override void DeleteEffect(_E992 effect)
		{
			if (effect is _E9B1)
			{
				_E003.Remove(effect);
			}
			else
			{
				base.DeleteEffect(effect);
			}
		}

		public override void Toggle(bool value)
		{
			base.Toggle(value);
			_E001.enabled = value;
			m__E004 = 1f - (float)_E000._E01C.Skills.StressPain;
		}
	}

	private sealed class _E005 : _E000
	{
		private CC_Sharpen _E001;

		private float m__E005;

		private bool _E006;

		public _E005(EffectsController effectsController, CC_Sharpen cameraEffect, float maxEffectValue, float durationTime, params Type[] types)
		{
			_E000 = effectsController;
			MaxEffectValue = maxEffectValue;
			_E001 = cameraEffect;
			ValidTypes = types;
			UpdateAmount();
		}

		public override void UpdateAmount()
		{
			float value = _E003();
			_E001.strength = m__E005 + Mathf.Clamp(value, 0f, 2f) * (MaxEffectValue - m__E005);
			_E001.enabled = (double)_E001.strength >= 0.1;
		}

		public void ChangeDefaultSharpenValue(float defaultValue)
		{
			m__E005 = defaultValue;
			UpdateAmount();
		}
	}

	private sealed class _E006 : _E000
	{
		private static readonly Dictionary<Type, float> _E007 = new Dictionary<Type, float>
		{
			{
				typeof(_E9BF),
				-0.15f
			},
			{
				typeof(_E9A4),
				0.15f
			},
			{
				typeof(_E9B0),
				0.3f
			}
		};

		private const float _E008 = 0f;

		private readonly CC_Sharpen _E009;

		private new static bool _E000(_E992 effect)
		{
			if (!(effect is _E9BF obj) || !(obj.MedItem is _EA70))
			{
				return effect is _E9A4;
			}
			return true;
		}

		public _E006(EffectsController effectsController, CC_Sharpen cameraEffect)
		{
			base._E000 = effectsController;
			_E009 = cameraEffect;
			_E001();
			Enabled = true;
		}

		public override void AddEffect(_E992 effect)
		{
			if (_E000(effect))
			{
				ActiveEffects.Add(effect);
				_E001();
			}
		}

		public override void DeleteEffect(_E992 effect)
		{
			int num = ActiveEffects.IndexOf(effect);
			if (num != -1)
			{
				ActiveEffects.RemoveAt(num);
				_E001();
			}
		}

		private void _E001()
		{
			MaxEffectValue = ((ActiveEffects.Count > 0) ? ActiveEffects.Max((_E992 x) => _E007[x.Type]) : 0f);
		}

		public override void UpdateAmount()
		{
			_E009.HealthDesaturate = Mathf.MoveTowards(_E009.HealthDesaturate, MaxEffectValue, Time.deltaTime * 0.1f);
		}
	}

	private sealed class _E007 : _E000
	{
		private CC_Blend _E001;

		private float _E00A;

		private float _E00B;

		private bool _E00C;

		private float _E00D;

		public _E007(EffectsController effectsController, CC_Blend cameraEffect, float maxEffectValue, params Type[] types)
		{
			_E000 = effectsController;
			MaxEffectValue = maxEffectValue;
			_E001 = cameraEffect;
			ValidTypes = types;
		}

		public override void AddEffect(_E992 effect)
		{
			if (ValidTypes.Contains(effect.Type))
			{
				if (ActiveEffects.Count == 0)
				{
					_E00A = Time.time;
					_E00B = Time.time + 10f;
				}
				ActiveEffects.Add(effect);
				Toggle(value: true);
				_E00C = false;
			}
		}

		public override void DeleteEffect(_E992 effect)
		{
			int num = ActiveEffects.IndexOf(effect);
			if (num != -1)
			{
				ActiveEffects.RemoveAt(num);
				if (ActiveEffects.Count == 0)
				{
					_E00A = Time.time;
					_E00B = Time.time + 10f;
					_E00C = true;
				}
			}
		}

		public override void UpdateAmount()
		{
			_E00D = (_E00C ? Mathf.InverseLerp(_E00B, _E00A, Time.time) : Mathf.InverseLerp(_E00A, _E00B, Time.time));
			_E001.amount = _E00D * MaxEffectValue;
			if (_E00D <= 0f && ActiveEffects.Count < 1)
			{
				Toggle(value: false);
			}
		}

		public override void Toggle(bool value)
		{
			base.Toggle(value);
			_E001.enabled = value;
			_E001.amount = 0f;
		}
	}

	private sealed class _E008 : _E000
	{
		private GrenadeFlashScreenEffect _E00E;

		private List<_E992> _E00F = new List<_E992>();

		public _E008(EffectsController effectsController, GrenadeFlashScreenEffect screenScreenEffect, params Type[] types)
		{
			ValidTypes = types;
			base._E000 = effectsController;
			_E00E = screenScreenEffect;
		}

		public override void AddEffect(_E992 effect)
		{
			if (ValidTypes.Contains(effect.Type))
			{
				ActiveEffects.Add(effect);
				Toggle(value: true);
				if (EnvironmentManager.Instance.Environment == EnvironmentType.Indoor)
				{
					_E00F.Add(effect);
				}
				_E00E.Explode(effect.Strength);
			}
		}

		public override void DeleteEffect(_E992 effect)
		{
			int num = ActiveEffects.IndexOf(effect);
			if (num != -1)
			{
				ActiveEffects.RemoveAt(num);
				if (ActiveEffects.Count == 0)
				{
					Toggle(value: false);
				}
				_E00F.Remove(effect);
			}
		}

		public override void Toggle(bool value)
		{
			base.Toggle(value);
			_E00E.enabled = value;
		}

		public override void UpdateAmount()
		{
			_E00E.EffectStrength = (float)Math.Sqrt(ActiveEffects.Max((_E992 e) => (!_E00F.Contains(e)) ? e.CurrentStrength : Mathf.Clamp01(e.CurrentStrength * 1.25f)));
		}

		[CompilerGenerated]
		private new float _E000(_E992 e)
		{
			if (!_E00F.Contains(e))
			{
				return e.CurrentStrength;
			}
			return Mathf.Clamp01(e.CurrentStrength * 1.25f);
		}
	}

	private sealed class _E009 : _E000
	{
		private float _E010 = 17f;

		private float _E011 = 5f;

		private float _E00A;

		public override void UpdateAmount()
		{
			float num = _E003();
			_E8A8.Instance.Camera.fieldOfView = _E8A8.Instance.Fov + num * num * num + (Mathf.Cos(Time.time * _E010) + Mathf.Cos(Time.time * _E011)) * (Mathf.Clamp01(num - 1.5f) / 3f);
		}

		public _E009(EffectsController effectsController, params Type[] types)
		{
			_E000 = effectsController;
			ValidTypes = types;
		}

		public override void Toggle(bool value)
		{
			base.Toggle(value);
			if (!value)
			{
				_E8A8.Instance.Camera.fieldOfView = _E8A8.Instance.Fov;
			}
		}
	}

	private sealed class _E00A : _E000
	{
		private CC_Wiggle m__E001;

		private float _E012;

		private float _E013;

		private List<_E986> _E014;

		private Dictionary<Type, float> _E015 = new Dictionary<Type, float>
		{
			{
				typeof(_E9AC),
				0.8f
			},
			{
				typeof(_E9AF),
				0.4f
			},
			{
				typeof(_E9AA),
				4f
			}
		};

		private Dictionary<Type, float> _E016 = new Dictionary<Type, float>
		{
			{
				typeof(_E9AC),
				8f
			},
			{
				typeof(_E9AF),
				5f
			},
			{
				typeof(_E9AA),
				1.5f
			}
		};

		public _E00A(EffectsController effectsController, CC_Wiggle cameraEffect, float maxEffectValue, params Type[] types)
		{
			base._E000 = effectsController;
			MaxEffectValue = maxEffectValue;
			this.m__E001 = cameraEffect;
			ValidTypes = types;
			_E014 = new List<_E986>();
		}

		public override void AddEffect(_E992 effect)
		{
			if (ValidTypes.Contains(effect.Type))
			{
				ActiveEffects.Add(effect);
				Toggle(value: true);
				_E012 = 0f;
				_E013 = 0f;
				_E000();
				this.m__E001.str = 1f;
			}
		}

		public override void DeleteEffect(_E992 effect)
		{
			int num = ActiveEffects.IndexOf(effect);
			if (num != -1)
			{
				ActiveEffects.RemoveAt(num);
				_E000();
			}
		}

		public override void StimulatorBuff(_E986 buff)
		{
			if (buff.Settings.BuffType == EStimulatorBuffType.ContusionWiggle)
			{
				if (buff.Active)
				{
					_E014.Add(buff);
				}
				else
				{
					_E014.Remove(buff);
				}
				Toggle(ActiveEffects.Count > 0 || _E014.Count > 0);
				_E000();
			}
		}

		private new void _E000()
		{
			_E012 = 0f;
			_E013 = 0f;
			if (ActiveEffects.Any((_E992 e) => e is _E9AA))
			{
				_E012 = _E015[typeof(_E9AA)];
				_E013 = _E016[typeof(_E9AA)];
			}
			else if (ActiveEffects.Count > 0)
			{
				_E012 = ActiveEffects.Max((_E992 e) => _E015[e.Type]);
				_E013 = ActiveEffects.Max((_E992 e) => _E016[e.Type]);
			}
			if (_E014.Count > 0)
			{
				_E012 = Mathf.Max(_E012, _E014.Max((_E986 b) => b.Value) * _E015[typeof(_E9AC)]);
				_E013 = Mathf.Max(_E013, _E014.Max((_E986 b) => b.Value) * _E016[typeof(_E9AC)]);
			}
		}

		public override void UpdateAmount()
		{
			float value = _E003();
			this.m__E001.speed = Mathf.Clamp01(value) * _E012;
			this.m__E001.scale = Mathf.Clamp01(value) * _E013;
			if (ActiveEffects.Count + _E014.Count == 0)
			{
				this.m__E001.str = Mathf.Lerp(this.m__E001.str, 0f, Time.deltaTime);
				if (this.m__E001.str < 0.01f)
				{
					Toggle(value: false);
				}
			}
		}

		public override void Toggle(bool value)
		{
			base.Toggle(value);
			this.m__E001.enabled = value;
		}

		protected override float _E003()
		{
			return base._E003() + _E014.Sum((_E986 buff) => buff.Value);
		}

		[CompilerGenerated]
		private float _E001(_E992 e)
		{
			return _E015[e.Type];
		}

		[CompilerGenerated]
		private float _E002(_E992 e)
		{
			return _E016[e.Type];
		}
	}

	private sealed class _E00B : _E000
	{
		private MotionBlur _E001;

		private Dictionary<Type, float> _E017 = new Dictionary<Type, float>
		{
			{
				typeof(_E9AC),
				0.8f
			},
			{
				typeof(_E9AF),
				0.3f
			},
			{
				typeof(_E9AA),
				1.4f
			}
		};

		private float _E018;

		private List<_E986> _E014;

		public _E00B(EffectsController effectsController, MotionBlur cameraEffect, float maxEffectValue, params Type[] types)
		{
			base._E000 = effectsController;
			MaxEffectValue = maxEffectValue;
			_E001 = cameraEffect;
			ValidTypes = types;
			_E014 = new List<_E986>();
		}

		protected override float _E003()
		{
			return base._E003() + _E014.Sum((_E986 buff) => buff.Value);
		}

		public override void UpdateAmount()
		{
			float value = _E003();
			MaxEffectValue = Mathf.Lerp(MaxEffectValue, _E018, Time.deltaTime);
			_E001.blurAmount = Mathf.Clamp01(value) * MaxEffectValue;
		}

		public override void AddEffect(_E992 effect)
		{
			base.AddEffect(effect);
			_E000();
		}

		public override void DeleteEffect(_E992 effect)
		{
			base.DeleteEffect(effect);
			_E000();
		}

		public override void StimulatorBuff(_E986 buff)
		{
			if (buff.Settings.BuffType == EStimulatorBuffType.ContusionBlur)
			{
				if (buff.Active)
				{
					_E014.Add(buff);
				}
				else
				{
					_E014.Remove(buff);
				}
				Toggle(ActiveEffects.Count > 0 || _E014.Count > 0);
				_E000();
			}
		}

		private new void _E000()
		{
			_E018 = 0f;
			foreach (_E992 activeEffect in ActiveEffects)
			{
				if (_E018 <= _E017[activeEffect.Type])
				{
					_E018 = _E017[activeEffect.Type];
				}
			}
			if (_E014.Count > 0)
			{
				_E018 = Mathf.Max(_E018, _E014.Max((_E986 b) => b.Value) * _E017[typeof(_E9AC)]);
			}
		}

		public override void Toggle(bool value)
		{
			base.Toggle(value);
			_E001.enabled = value;
		}
	}

	private const float m__E000 = 5.2f;

	private const float m__E001 = 15f;

	[SerializeField]
	private GameObject _effectsPrefab;

	[SerializeField]
	public AnimationCurve FastVineteFlicker;

	[CompilerGenerated]
	private RainScreenDrops m__E002;

	[CompilerGenerated]
	private ScreenWater m__E003;

	private CC_FastVignette m__E004;

	private CC_DoubleVision m__E005;

	private CC_HueFocus m__E006;

	private CC_RadialBlur m__E007;

	private CC_Sharpen m__E008;

	private CC_Blend m__E009;

	private CC_Blend m__E00A;

	private CC_Wiggle m__E00B;

	private MotionBlur m__E00C;

	private BloodOnScreen m__E00D;

	private GrenadeFlashScreenEffect _E00E;

	private EyeBurn _E00F;

	private FastBlur _E010;

	private DepthOfField _E011;

	private readonly List<_E000> _E012 = new List<_E000>();

	private _E005 _E013;

	private float _E014;

	private float _E015;

	private float _E016;

	private float _E017;

	private float _E018;

	private float _E019;

	private float _E01A;

	private DeathFade _E01B;

	private Player _E01C;

	private Action _E01D;

	private Action _E01E;

	private static readonly MaterialType[] _E01F = new MaterialType[3]
	{
		MaterialType.GlassVisor,
		MaterialType.Helmet,
		MaterialType.HelmetRicochet
	};

	public RainScreenDrops RainScreenDrops
	{
		[CompilerGenerated]
		get
		{
			return this.m__E002;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E002 = value;
		}
	}

	public ScreenWater ScreenWater
	{
		[CompilerGenerated]
		get
		{
			return this.m__E003;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E003 = value;
		}
	}

	private void Awake()
	{
		_E003();
		_E01D = Singleton<_E7DE>.Instance.Graphics.Settings.Sharpen.Bind(_E009);
		PlayerCameraController.OnPlayerCameraControllerCreated += _E000;
		PlayerCameraController.OnPlayerCameraControllerDestroyed += _E002;
		_E01E = _EBAF.Instance.SubscribeOnEvent(delegate(_EBB3 invokedEvent)
		{
			_E001(invokedEvent);
		});
	}

	private void _E000(PlayerCameraController playerCameraController, Camera camera)
	{
		_E01C = playerCameraController.Player;
		foreach (_E992 allActiveEffect in _E01C.HealthController.GetAllActiveEffects())
		{
			foreach (_E000 item in _E012)
			{
				item.AddEffect(allActiveEffect);
			}
		}
		_E01C.HealthController.EffectStartedEvent += _E00A;
		_E01C.HealthController.EffectRemovedEvent += _E00B;
		_E01C.OnDamageReceived += _E007;
		_E01C.HealthController.ApplyDamageEvent += _E006;
		_E01C.HealthController.DiedEvent += _E008;
		_E01C.HealthController.BurnEyesEvent += _E005;
		_E01C.HealthController.StimulatorBuffEvent += _E00C;
		_E01C.OnGlassesChanged += _E004;
	}

	private void _E001(_EBB3 invokedEvent)
	{
		if (invokedEvent.PlayerProfileID == _E01C.ProfileId)
		{
			_E00F.IsPaused = invokedEvent.IsPaused;
			if (_E00F.IsPaused)
			{
				_E00F.ClearEyeBurnRT();
			}
		}
	}

	private void _E002()
	{
		_E01C = null;
	}

	private void _E003()
	{
		EffectsCurveData component = _effectsPrefab.GetComponent<EffectsCurveData>();
		if (component != null)
		{
			FastVineteFlicker = component.FastVineteFlicker;
		}
		else
		{
			Debug.LogError(_ED3E._E000(88694));
		}
		RainScreenDrops = GetComponent<RainScreenDrops>();
		this.m__E00D = GetComponent<BloodOnScreen>();
		ScreenWater = GetComponent<ScreenWater>();
		FastVineteFlicker.postWrapMode = WrapMode.Loop;
		_E010 = GetComponent<FastBlur>();
		_E01B = GetComponent<DeathFade>();
		if (_E01B != null)
		{
			_E01B.enabled = false;
		}
		DepthOfField component2 = _effectsPrefab.GetComponent<DepthOfField>();
		_E011 = base.gameObject.GetComponent<DepthOfField>() ?? base.gameObject.AddComponentCopy(component2);
		_E011.aperture = component2.aperture;
		_E011.blurSampleCount = component2.blurSampleCount;
		_E011.blurType = component2.blurType;
		_E011.focalLength = component2.focalLength;
		_E011.focalSize = component2.focalSize;
		_E011.focalTransform = component2.focalTransform;
		_E011.foregroundOverlap = component2.foregroundOverlap;
		_E011.highResolution = component2.highResolution;
		_E011.maxBlurSize = component2.maxBlurSize;
		_E011.nearBlur = component2.nearBlur;
		CC_FastVignette component3 = _effectsPrefab.GetComponent<CC_FastVignette>();
		this.m__E004 = base.gameObject.GetComponent<CC_FastVignette>() ?? base.gameObject.AddComponentCopy(component3);
		this.m__E004.sharpness = component3.sharpness;
		this.m__E004.center = component3.center;
		this.m__E004.shader = component3.shader;
		_E017 = component3.darkness;
		this.m__E004.enabled = false;
		CC_DoubleVision component4 = _effectsPrefab.GetComponent<CC_DoubleVision>();
		this.m__E005 = base.gameObject.GetComponent<CC_DoubleVision>() ?? base.gameObject.AddComponentCopy(component4);
		this.m__E005.shader = component4.shader;
		this.m__E005.enabled = false;
		this.m__E005.displace = component4.displace;
		_E016 = component4.amount;
		CC_HueFocus component5 = _effectsPrefab.GetComponent<CC_HueFocus>();
		this.m__E006 = base.gameObject.GetComponent<CC_HueFocus>() ?? base.gameObject.AddComponentCopy(component5);
		this.m__E006.hue = component5.hue;
		this.m__E006.range = component5.range;
		this.m__E006.boost = component5.boost;
		this.m__E006.shader = component5.shader;
		this.m__E006.enabled = false;
		_E00E = base.gameObject.GetComponent<GrenadeFlashScreenEffect>();
		_E00F = base.gameObject.GetComponent<EyeBurn>();
		CC_RadialBlur component6 = _effectsPrefab.GetComponent<CC_RadialBlur>();
		this.m__E007 = base.gameObject.GetComponent<CC_RadialBlur>() ?? base.gameObject.AddComponentCopy(component6);
		this.m__E007.shader = component6.shader;
		this.m__E007.enabled = false;
		this.m__E007.quality = component6.quality;
		this.m__E007.center = component6.center;
		_E014 = component6.amount;
		CC_Blend cC_Blend = _effectsPrefab.GetComponents<CC_Blend>()[0];
		this.m__E00A = base.gameObject.AddComponentCopy(cC_Blend);
		this.m__E00A.shader = cC_Blend.shader;
		this.m__E00A.enabled = false;
		_E015 = cC_Blend.amount;
		CC_Blend cC_Blend2 = _effectsPrefab.GetComponents<CC_Blend>()[1];
		this.m__E009 = base.gameObject.AddComponentCopy(cC_Blend2);
		this.m__E009.shader = cC_Blend2.shader;
		this.m__E009.enabled = false;
		_E015 = cC_Blend2.amount;
		CC_Wiggle component7 = _effectsPrefab.GetComponent<CC_Wiggle>();
		this.m__E00B = base.gameObject.GetComponent<CC_Wiggle>() ?? base.gameObject.AddComponentCopy(component7);
		this.m__E00B.shader = component7.shader;
		this.m__E00B.enabled = false;
		_E018 = component7.scale;
		MotionBlur component8 = _effectsPrefab.GetComponent<MotionBlur>();
		this.m__E00C = base.gameObject.GetComponent<MotionBlur>() ?? base.gameObject.AddComponentCopy(component8);
		this.m__E00C.shader = component8.shader;
		this.m__E00C.enabled = false;
		_E019 = component8.blurAmount;
		CC_Sharpen component9 = _effectsPrefab.GetComponent<CC_Sharpen>();
		this.m__E008 = base.gameObject.GetComponent<CC_Sharpen>() ?? base.gameObject.AddComponentCopy(component9);
		this.m__E008.shader = component9.shader;
		this.m__E008.enabled = false;
		_E01A = component9.strength;
		_E013 = new _E005(this, this.m__E008, _E01A, 15f, typeof(_E9B1), typeof(_E9AA));
		_E012.Clear();
		_E012.Add(_E013);
		_E012.Add(new _E006(this, this.m__E008));
		_E012.Add(new _E004(this, this.m__E007, _E014, typeof(_E9A3), typeof(_E9B0), typeof(_E9B1)));
		_E012.Add(new _E007(this, this.m__E009, _E015, typeof(_E9AF)));
		_E012.Add(new _E001(this, this.m__E004, _E017, typeof(_E9A4), typeof(_E9B6)));
		_E012.Add(new _E00A(this, this.m__E00B, _E018, typeof(_E9AC), typeof(_E9AF)));
		_E012.Add(new _E00B(this, this.m__E00C, _E019, typeof(_E9AC), typeof(_E9AF)));
		_E012.Add(new _E002(this, this.m__E005, _E016, typeof(_E9AC), typeof(_E9B3)));
		_E012.Add(new _E009(this, typeof(_E9AA)));
		_E012.Add(new _E008(this, _E00E, typeof(_E9B3)));
		base.gameObject.AddComponent<SSAAImpl>();
	}

	private void _E004(bool isActive)
	{
		if ((bool)RainScreenDrops)
		{
			RainScreenDrops.ChangeGlassesState(isActive);
		}
		if ((bool)this.m__E00D)
		{
			this.m__E00D.ChangeGlassesState(isActive);
		}
		if ((bool)ScreenWater)
		{
			ScreenWater.ChangeGlassesState(isActive);
		}
	}

	public void SetDoFFocalDistance(int fov, float minFov, float maxFov)
	{
		_E011.focalLength = 5.2f - Mathf.InverseLerp(minFov, maxFov, fov) * (minFov / (maxFov - minFov));
	}

	private void _E005(Vector3 position, float str, float time)
	{
		_E00F.enabled = true;
		_E00F.Burn(position, str, _E01C.MovementContext.PreviousRotation - _E01C.MovementContext.Rotation);
	}

	private void _E006(EBodyPart bodyPart, float damage, _EC23 damageInfo)
	{
		if (damageInfo.DamageType.IsBleeding())
		{
			Vector3 vector = UnityEngine.Random.insideUnitSphere / 3f;
			vector += EFTHardSettings.Instance.BleedingCenters[(int)bodyPart];
			Vector3 direction = new Vector3(vector.x, 0f, vector.z);
			this.m__E00D.HitBleeding(direction, (damageInfo.DamageType & EDamageType.HeavyBleeding) != 0);
		}
	}

	private void _E007(float damage, EBodyPart bodyPart, EDamageType type, float damageReducedByArmor, MaterialType special = MaterialType.None)
	{
		if (!base.enabled || !base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (_E01F.Contains(special) && Singleton<Effects>.Instantiated)
		{
			Singleton<Effects>.Instance.EmitSoundOnly(special, _E01C.Transform.position, EPointOfView.FirstPerson);
		}
		if (type.IsSelfInflicted() || type == EDamageType.Undefined)
		{
			return;
		}
		Vector3 insideUnitSphere = UnityEngine.Random.insideUnitSphere;
		Vector3 direction = new Vector3(insideUnitSphere.x, 0f, insideUnitSphere.z);
		direction = base.transform.InverseTransformDirection(direction).normalized;
		float num = damage + damageReducedByArmor;
		float strength = Mathf.Sqrt(num) / 10f;
		float hands = 0.05f;
		float camera = 0.4f;
		switch (bodyPart)
		{
		case EBodyPart.Head:
			hands = 0.1f;
			camera = 1.3f;
			break;
		case EBodyPart.LeftArm:
		case EBodyPart.RightArm:
			hands = 0.15f;
			camera = 0.5f;
			break;
		case EBodyPart.LeftLeg:
		case EBodyPart.RightLeg:
			camera = 0.3f;
			break;
		}
		_E01C.ProceduralWeaponAnimation.ForceReact.AddForce(strength, hands, camera);
		float power = (_E01C.MovementContext.PhysicalConditionIs(EPhysicalCondition.OnPainkillers) ? num : ((bodyPart == EBodyPart.Head) ? (num * 6f) : (num * 3f)));
		_E010.enabled = true;
		_E010.Hit(power);
		if (type != EDamageType.Fall && damage >= 0.1f)
		{
			this.m__E00D.Hit(direction);
			if (bodyPart == EBodyPart.Head)
			{
				insideUnitSphere = UnityEngine.Random.insideUnitSphere / 1.5f;
				insideUnitSphere += EFTHardSettings.Instance.BleedingCenters[(int)bodyPart];
				direction = new Vector3(insideUnitSphere.x, 0f, insideUnitSphere.z);
				this.m__E00D.Hit(direction);
			}
		}
	}

	private void _E008(EDamageType damageType)
	{
		if (base.enabled && base.gameObject.activeInHierarchy)
		{
			_E01B.enabled = true;
			_E01B.EnableEffect();
			_E010.enabled = true;
			_E010.Die();
		}
	}

	private void OnDisable()
	{
		foreach (_E000 item in _E012)
		{
			item.ActiveEffects.Clear();
		}
	}

	private void OnDestroy()
	{
		PlayerCameraController.OnPlayerCameraControllerCreated -= _E000;
		PlayerCameraController.OnPlayerCameraControllerDestroyed -= _E002;
		_E01D?.Invoke();
		_E01D = null;
		_E01E?.Invoke();
		_E01E = null;
		if (!(_E01C == null))
		{
			_E01C.HealthController.EffectStartedEvent -= _E00A;
			_E01C.HealthController.EffectRemovedEvent -= _E00B;
			_E01C.OnDamageReceived -= _E007;
			_E01C.HealthController.ApplyDamageEvent -= _E006;
			_E01C.HealthController.DiedEvent -= _E008;
			_E01C.HealthController.BurnEyesEvent -= _E005;
			_E01C.HealthController.StimulatorBuffEvent -= _E00C;
			_E01C.OnGlassesChanged -= _E004;
			_E01C = null;
		}
	}

	private void _E009(float value)
	{
		_E013?.ChangeDefaultSharpenValue(value);
	}

	private void _E00A(_E992 effect)
	{
		foreach (_E000 item in _E012)
		{
			item.AddEffect(effect);
		}
	}

	private void _E00B(_E992 effect)
	{
		foreach (_E000 item in _E012)
		{
			item.DeleteEffect(effect);
		}
	}

	private void _E00C(_E986 buff)
	{
		foreach (_E000 item in _E012)
		{
			item.StimulatorBuff(buff);
		}
	}

	private void Update()
	{
		for (int i = 0; i < _E012.Count; i++)
		{
			if (_E012[i].Enabled)
			{
				_E012[i].UpdateAmount();
			}
		}
	}

	[CompilerGenerated]
	private void _E00D(_EBB3 invokedEvent)
	{
		_E001(invokedEvent);
	}
}
