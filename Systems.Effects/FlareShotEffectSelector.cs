using System;
using EFT.Airdrop;
using EFT.PrefabSettings;
using UnityEngine;

namespace Systems.Effects;

public sealed class FlareShotEffectSelector : MonoBehaviour
{
	[Serializable]
	private struct FlareParameters
	{
		public Color32 Color;

		public float LightIntensity;

		public float LightRange;

		public bool CastShadows;
	}

	[SerializeField]
	private FlareParameters _lightFlareParameters;

	[SerializeField]
	private FlareParameters _redAirdropFlareParameters;

	[SerializeField]
	private FlareParameters _greenExitActivateFlareParameters;

	[SerializeField]
	private FlareParameters _yellowQuestFlareParameters;

	[SerializeField]
	private FlareParameters _acidGreenFlareParameters;

	[Space(10f)]
	[SerializeField]
	private ParticleSystem _flareParticleSystem;

	[SerializeField]
	private ParticleSystem _smokeParticleSystem;

	[SerializeField]
	private Light _flareLight;

	[SerializeField]
	private FlareShotEffectAnimator _flareAnimator;

	[SerializeField]
	[Space(10f)]
	private FlarePatronSound _flarePatronSound;

	public void SetFlareEffect(FlareColorType flareColorType, float lifetime)
	{
		_flareParticleSystem.Stop(withChildren: false, ParticleSystemStopBehavior.StopEmittingAndClear);
		_smokeParticleSystem.Stop(withChildren: false, ParticleSystemStopBehavior.StopEmittingAndClear);
		_flarePatronSound.Init(lifetime);
		FlareParameters flareParameters = flareColorType switch
		{
			FlareColorType.LightFlare => _lightFlareParameters, 
			FlareColorType.RedFlare => _redAirdropFlareParameters, 
			FlareColorType.GreenFlare => _greenExitActivateFlareParameters, 
			FlareColorType.YellowFlare => _yellowQuestFlareParameters, 
			FlareColorType.AcidGreen => _acidGreenFlareParameters, 
			_ => _lightFlareParameters, 
		};
		if (_flareLight != null && _flareAnimator != null)
		{
			_flareLight.intensity = flareParameters.LightIntensity;
			_flareAnimator.LightIntensity = flareParameters.LightIntensity;
			_flareLight.range = flareParameters.LightRange;
			_flareLight.shadows = (flareParameters.CastShadows ? LightShadows.Soft : LightShadows.None);
			_flareLight.color = flareParameters.Color;
		}
		ParticleSystem.MainModule main = _smokeParticleSystem.main;
		main.duration = lifetime;
		main.duration -= main.startLifetime.constantMax;
		main = _flareParticleSystem.main;
		main.duration = lifetime;
		main.startColor = Color.Lerp(flareParameters.Color, Color.white, 0.25f);
		ParticleSystem.MinMaxCurve startLifetime = main.startLifetime;
		startLifetime.mode = ParticleSystemCurveMode.Constant;
		startLifetime.constant = lifetime;
		if (Mathf.Abs(lifetime) > Mathf.Epsilon && _flareAnimator != null)
		{
			_flareAnimator.Frequency = 1f / lifetime;
		}
	}
}
