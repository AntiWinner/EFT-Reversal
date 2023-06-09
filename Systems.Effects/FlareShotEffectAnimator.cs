using System.Runtime.CompilerServices;
using UnityEngine;

namespace Systems.Effects;

public sealed class FlareShotEffectAnimator : MonoBehaviour
{
	private static readonly int _E000 = Shader.PropertyToID(_ED3E._E000(35970));

	[SerializeField]
	private AnimationCurve _fadeCurve;

	[SerializeField]
	private ParticleSystem _flareParticleSystem;

	[SerializeField]
	private ParticleSystem _smokeParticleSystem;

	[SerializeField]
	private CullingLightObject _cullingLightObject;

	[SerializeField]
	private float _lightIntensity = 1f;

	private float _E001;

	private Material _E002;

	[CompilerGenerated]
	private float _E003;

	public float LightIntensity
	{
		set
		{
			_lightIntensity = value;
		}
	}

	public float Frequency
	{
		[CompilerGenerated]
		get
		{
			return _E003;
		}
		[CompilerGenerated]
		set
		{
			_E003 = value;
		}
	}

	private void OnEnable()
	{
		_E001 = 0f;
		if (!(_cullingLightObject == null))
		{
			float num = _fadeCurve.Evaluate(_E001);
			_cullingLightObject.IntensityMultiplier = num * _lightIntensity;
			_flareParticleSystem.Play(withChildren: false);
			_smokeParticleSystem.Play(withChildren: false);
		}
	}

	private void OnDisable()
	{
		_flareParticleSystem.Stop(withChildren: false, ParticleSystemStopBehavior.StopEmittingAndClear);
		_smokeParticleSystem.Stop(withChildren: false, ParticleSystemStopBehavior.StopEmittingAndClear);
	}

	private void OnDestroy()
	{
		OnDisable();
	}

	private void Update()
	{
		if (!(_cullingLightObject == null))
		{
			base.transform.rotation = Quaternion.identity;
			float num = _fadeCurve.Evaluate(_E001);
			_cullingLightObject.IntensityMultiplier = num * _lightIntensity;
			_E001 += Time.deltaTime * Frequency;
		}
	}
}
