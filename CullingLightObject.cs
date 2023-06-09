using EFT.Visual;
using UnityEngine;

[ExecuteInEditMode]
public class CullingLightObject : CullingObject
{
	[SerializeField]
	private Light _light;

	[SerializeField]
	private float _fadeStartDistance = 50f;

	[SerializeField]
	private float _fadeEndDistance = 80f;

	[SerializeField]
	private AnimationCurve _fadeCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

	[SerializeField]
	private bool _useLightIntensityFromEditor;

	[SerializeField]
	[Range(0f, 8f)]
	private float _maxLightIntensity = 5f;

	[SerializeField]
	private bool _takeFlareParametersFromThisScript;

	private GUIStyle _E009 = new GUIStyle();

	[SerializeField]
	[Header("Shadows")]
	private bool _cullShadowsByDistance = true;

	[SerializeField]
	private float _shadowsFadeStartDistance = 12f;

	[SerializeField]
	private float _shadowsFadeEndDistance = 17f;

	private float m__E000;

	private bool _E00A = true;

	private float m__E001 = 1f;

	private static int m__E002;

	private int m__E003;

	private int _E004;

	private const int _E005 = 20;

	private float _E006;

	private float _E007;

	private float _E008;

	private LightFlicker _E00B;

	private float _E00C;

	private float _E00D;

	private float _E00E;

	[SerializeField]
	private LightShadows _initialShadowsMode;

	public float IntensityMultiplier
	{
		get
		{
			return this.m__E001;
		}
		set
		{
			this.m__E001 = value;
		}
	}

	public float CurrentLightIntensity => _maxLightIntensity * this.m__E001;

	public bool IsLightEnabled => _E00A;

	public bool TakeFlareParametersFromCullingLight => _takeFlareParametersFromThisScript;

	public bool CullShadowsByDistance => _cullShadowsByDistance;

	protected override void Awake()
	{
		this.m__E003 = CullingLightObject.m__E002;
		CullingLightObject.m__E002 = (CullingLightObject.m__E002 + 1) % 20;
		base.Awake();
		_E000();
		_E003();
		this.m__E001 = 1f;
		_light = GetComponent<Light>();
		if (_light == null)
		{
			Debug.LogError(_ED3E._E000(94843) + base.gameObject.name + _ED3E._E000(94823));
			base.enabled = false;
			return;
		}
		_light.enabled = true;
		_E00B = GetComponent<LightFlicker>();
		this.m__E000 = (_useLightIntensityFromEditor ? _maxLightIntensity : _light.intensity);
		if (_initialShadowsMode == LightShadows.None)
		{
			_initialShadowsMode = _light.shadows;
		}
		CullDistance = _fadeEndDistance;
		if (_light.type == LightType.Point)
		{
			base.Radius = _light.range;
		}
	}

	public float getMaxIntensityFinal()
	{
		return this.m__E000;
	}

	public LightFlicker GetFlicker()
	{
		return _E00B;
	}

	public Light GetLight()
	{
		return _light;
	}

	public bool GetCullShadowsByDistance()
	{
		return _cullShadowsByDistance;
	}

	public LightShadows GetInitialShadowsMode()
	{
		return _initialShadowsMode;
	}

	public float GetFadeStartDistance()
	{
		return _fadeStartDistance;
	}

	public float GetFadEndDistance()
	{
		return _fadeEndDistance;
	}

	private void _E000()
	{
		_E006 = _fadeStartDistance * _fadeStartDistance;
		_E007 = _fadeEndDistance * _fadeEndDistance;
		_E008 = _E007 - _E006;
	}

	public override void SetVisibility(bool isVisible)
	{
		if (base.IsVisible != isVisible)
		{
			base.SetVisibility(isVisible);
			_E001();
		}
	}

	public override void CustomUpdate()
	{
		if (!(CullingManager.Instance == null))
		{
			_E004 = (_E004 + 1) % 20;
			if (_E004 == this.m__E003)
			{
				base.CustomUpdate();
				_E001();
			}
		}
	}

	private void _E001()
	{
		if (!(CullingManager.Instance == null))
		{
			float multiplier = _E002();
			float shadowMultiplier = CalculateShadowMultiplier();
			UpdateCullingObject(multiplier, shadowMultiplier);
		}
	}

	protected virtual void UpdateCullingObject(float multiplier, float shadowMultiplier)
	{
		bool num = CullingManager.Instance.IsOpticEnabled();
		bool flag = Mathf.Approximately(multiplier, 0f);
		float num2 = ((num && flag) ? 1f : multiplier);
		num2 *= this.m__E001;
		if (_E00B != null && _E00B.enabled)
		{
			_E00B.CullingCoef = num2;
		}
		else
		{
			_light.intensity = this.m__E000 * num2;
		}
		if (_cullShadowsByDistance)
		{
			_light.shadowStrength = shadowMultiplier;
			bool flag2 = Mathf.Approximately(shadowMultiplier, 0f);
			if (flag2 && _light.shadows == _initialShadowsMode)
			{
				_light.shadows = LightShadows.None;
			}
			else if (!flag2 && _light.shadows == LightShadows.None)
			{
				_light.shadows = _initialShadowsMode;
			}
		}
	}

	private float _E002()
	{
		float result = 0f;
		if (base.IsVisible)
		{
			float time = Mathf.Clamp01((CullingManager.Instance.GetCameraDistanceSqr(base.Index) - _E006) / _E008);
			result = _fadeCurve.Evaluate(time);
		}
		return result;
	}

	protected float CalculateShadowMultiplier()
	{
		if (!_cullShadowsByDistance)
		{
			return 1f;
		}
		float result = 0f;
		if (base.IsVisible)
		{
			float time = Mathf.Clamp01((CullingManager.Instance.GetCameraDistanceSqr(base.Index) - _E00C) / _E00E);
			result = _fadeCurve.Evaluate(time);
		}
		return result;
	}

	private void _E003()
	{
		_E00C = _shadowsFadeStartDistance * _shadowsFadeStartDistance;
		_E00D = _shadowsFadeEndDistance * _shadowsFadeEndDistance;
		_E00E = _E00D - _E00C;
	}

	public void Switch(bool enable)
	{
		_E00A = enable;
	}

	protected virtual void OnDisable()
	{
		this.m__E001 = 1f;
		if (_light != null)
		{
			_light.intensity = this.m__E000;
			_light.shadows = _initialShadowsMode;
			_light.shadowStrength = 1f;
		}
	}

	protected override void OnDestroy()
	{
		this.m__E001 = 1f;
		base.OnDestroy();
		if (_light != null)
		{
			_light.intensity = this.m__E000;
			_light.shadows = _initialShadowsMode;
			_light.shadowStrength = 1f;
		}
	}

	public Light GetEditorLight()
	{
		return _light;
	}
}
