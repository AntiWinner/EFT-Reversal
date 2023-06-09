using UnityEngine;

[ExecuteInEditMode]
public class CullingAdvancedLightObject : CullingObject
{
	[SerializeField]
	private BaseLight _light;

	[SerializeField]
	private float _fadeStartDistance = 50f;

	[SerializeField]
	private float _fadeEndDistance = 80f;

	[SerializeField]
	private AnimationCurve _fadeCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

	[SerializeField]
	private float _maxLightIntensity = -10f;

	[SerializeField]
	private bool _isControlledByLampController;

	private float m__E000;

	private float _E001 = 1f;

	private static int _E002;

	private int _E003;

	private int _E004;

	private const int _E005 = 20;

	private float _E006;

	private float _E007;

	private float _E008;

	public float IntensityMultiplier
	{
		set
		{
			_E001 = value;
		}
	}

	protected override void Awake()
	{
		_E003 = _E002;
		_E002 = (_E002 + 1) % 20;
		base.Awake();
		_E006 = _fadeStartDistance * _fadeStartDistance;
		_E007 = _fadeEndDistance * _fadeEndDistance;
		_E008 = _E007 - _E006;
		_E001 = 1f;
		_light = GetComponentInChildren<BaseLight>(includeInactive: true);
		if (_light == null)
		{
			Debug.LogError(_ED3E._E000(94781) + base.gameObject.name + _ED3E._E000(94754));
			base.enabled = false;
			return;
		}
		if (!_isControlledByLampController)
		{
			_light.enabled = true;
		}
		this.m__E000 = _maxLightIntensity;
		CullDistance = _fadeEndDistance;
	}

	public override void CustomUpdate()
	{
		_E004 = (_E004 + 1) % 20;
		if (_E004 == _E003)
		{
			base.CustomUpdate();
			_E000();
		}
	}

	public override void SetVisibility(bool isVisible)
	{
		if (base.IsVisible != isVisible)
		{
			base.SetVisibility(isVisible);
			_E000();
		}
	}

	private void _E000()
	{
		if (!(CullingManager.Instance == null))
		{
			float multiplier = CalculateMultiplayer();
			UpdateCullingObject(multiplier);
		}
	}

	protected void UpdateCullingObject(float multiplier)
	{
		bool num = CullingManager.Instance.IsOpticEnabled();
		bool flag = Mathf.Approximately(multiplier, 0f);
		float num2 = ((num && flag) ? 1f : multiplier);
		num2 *= _E001;
		if (_light != null)
		{
			_light.m_Intensity = this.m__E000 * num2;
		}
	}

	protected float CalculateMultiplayer()
	{
		float result = 0f;
		if (base.IsVisible)
		{
			float num = (CullingManager.Instance.GetCameraDistanceSqr(base.Index) - _E006) / _E008;
			if (num < 0f)
			{
				num = 0f;
			}
			else if (num > 1f)
			{
				num = 1f;
			}
			result = _fadeCurve.Evaluate(num);
		}
		return result;
	}

	protected virtual void OnDisable()
	{
		_E001 = 1f;
		if (_light != null)
		{
			_light.m_Intensity = this.m__E000;
		}
	}

	protected override void OnDestroy()
	{
		_E001 = 1f;
		base.OnDestroy();
		if (_light != null)
		{
			_light.m_Intensity = this.m__E000;
		}
	}
}
