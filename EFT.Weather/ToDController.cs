using System;
using Comfort.Common;
using UnityEngine;
using UnityEngine.Rendering;

namespace EFT.Weather;

[Serializable]
public class ToDController
{
	public struct _E000
	{
		public SphericalHarmonicsL2 FullSH;

		public SphericalHarmonicsL2 SHWithoutTop;
	}

	[Header("midnight(-1) sunrize/sunset(0) midday(1)")]
	public Vector2 RayleighMultiplierMinMax;

	public AnimationCurve RayleighMultiplier;

	public Vector2 MieMultiplierMinMax;

	public AnimationCurve MieMultiplier;

	[Header("1 - DirectionalityMult * Directionality")]
	public float DirectionalityMult;

	public AnimationCurve Directionality;

	public Vector2 BrightnessMinMax;

	public AnimationCurve Brightness;

	public float ScatteringBrightnessMultiplier = 1f;

	public AnimationCurve ScatteringBrightness;

	public float BrightnessOvercast;

	public Vector2 ContrastMinMax;

	public AnimationCurve Contrast;

	[Space(16f)]
	[_E2BD(0f, 1f, -1f)]
	public Vector2 MoonLightMinMax;

	[Space(16f)]
	public Gradient LightColor;

	[Header("SHAmbient")]
	public AmbientLight AmbientLightScript;

	public AmbientHighlight AmbientHighlightScript;

	public bool HighlightWithoutTopHarmonics = true;

	[Space(16f)]
	public AnimationCurve TopHarmonicIntensity;

	public AnimationCurve HorizontHarmonicIntensity;

	public AnimationCurve BounceHarmonicIntensity;

	public AnimationCurve ForwarHarmonicIntensity;

	[Space(16f)]
	public Gradient ForwarHarmonicMultiplyer = new Gradient
	{
		colorKeys = new GradientColorKey[1]
		{
			new GradientColorKey(new Color(0.5f, 0.5f, 0.5f), 0f)
		}
	};

	public Gradient BackwardHarmonicMultiplyer = new Gradient
	{
		colorKeys = new GradientColorKey[1]
		{
			new GradientColorKey(new Color(0.5f, 0.5f, 0.5f), 0f)
		}
	};

	[Space(16f)]
	public Gradient AddTopAmbient;

	public Gradient AddAmbient;

	[Space(16f)]
	public AnimationCurve AmbientBrightness;

	public AnimationCurve AmbientSaturation;

	public AnimationCurve AmbientContrast;

	public Color CloudnessLightColor;

	private readonly Vector3[] _dirs = new Vector3[5]
	{
		Vector3.up,
		Vector3.zero,
		Vector3.zero,
		Vector3.zero,
		Vector3.zero
	};

	private readonly float[] _qualityUpdateIntervals = new float[4] { 1f, 0.5f, 0.1f, 0f };

	public SphericalHarmonicsL2 SH { get; private set; }

	public Color TopHorizontSkyColor { get; private set; }

	public Color EnvironmentColor { get; set; } = new Color(0.013f, 0.013f, 0.013f, 0f);


	public float HarmonicsDaylight { get; set; }

	public void Update()
	{
		TOD_Sky instance = TOD_Sky.Instance;
		if (instance == null)
		{
			Debug.LogWarning(_ED3E._E000(218567));
			return;
		}
		float y = instance.SunDirection.y;
		instance.Atmosphere.RayleighMultiplier = Mathf.LerpUnclamped(RayleighMultiplierMinMax.x, RayleighMultiplierMinMax.y, RayleighMultiplier.Evaluate(y));
		instance.Atmosphere.MieMultiplier = Mathf.LerpUnclamped(MieMultiplierMinMax.x, MieMultiplierMinMax.y, MieMultiplier.Evaluate(y));
		instance.Atmosphere.Directionality = 1f - Directionality.Evaluate(y) * DirectionalityMult;
		instance.Atmosphere.Brightness = Mathf.LerpUnclamped(BrightnessMinMax.x, BrightnessMinMax.y, Brightness.Evaluate(y));
		instance.Atmosphere.Brightness *= 1f - instance.Atmosphere.Fogginess * BrightnessOvercast;
		instance.Atmosphere.ScatteringBrightness = ScatteringBrightness.Evaluate(y) * ScatteringBrightnessMultiplier * instance.Atmosphere.Brightness;
		instance.Atmosphere.Contrast = Mathf.LerpUnclamped(ContrastMinMax.x, ContrastMinMax.y, Contrast.Evaluate(y));
		float num = y * 0.5f + 0.5f;
		instance.Components.LightSource.color = SaturateColor(LightColor.Evaluate(num), 1f - instance.Atmosphere.Fogginess);
		float t = 0.5f - 0.5f * Vector3.Dot(instance.MoonDirection, instance.SunDirection);
		instance.Night.ColorMultiplier = Mathf.LerpUnclamped(MoonLightMinMax.x, MoonLightMinMax.y, t);
		_E000 obj = CalculateSphericalHarmonics(instance, y, num);
		SH = obj.FullSH;
		AmbientLightScript.SetSH(SH);
		if (AmbientHighlightScript != null)
		{
			AmbientHighlightScript.SetSH(HighlightWithoutTopHarmonics ? obj.SHWithoutTop : SH);
		}
		if (Singleton<_E7DE>.Instantiated && !WeatherEventController.ChangeTimeInProgress)
		{
			_E7EB settings = Singleton<_E7DE>.Instance.Graphics.Settings;
			instance.Light.UpdateInterval = _qualityUpdateIntervals[(int)settings.ShadowsQuality];
		}
	}

	public _E000 CalculateSphericalHarmonics(TOD_Sky todSky, float t, float t01)
	{
		_E000 obj = default(_E000);
		obj.FullSH = default(SphericalHarmonicsL2);
		obj.SHWithoutTop = default(SphericalHarmonicsL2);
		_E000 result = obj;
		Color color = ForwarHarmonicMultiplyer.Evaluate(t01) * 2f * ForwarHarmonicIntensity.Evaluate(t01);
		Color color2 = BackwardHarmonicMultiplyer.Evaluate(t01) * 2f;
		float num = TopHarmonicIntensity.Evaluate(t);
		float num2 = HorizontHarmonicIntensity.Evaluate(t01);
		float fogginess = todSky.Atmosphere.Fogginess;
		float num3 = (WeatherController.Instance ? WeatherController.Instance.CloudsController.Density : 0f);
		Vector3 sunDirection = todSky.SunDirection;
		sunDirection.y = 0f;
		sunDirection.Normalize();
		_dirs[1] = sunDirection;
		_dirs[2] = -sunDirection;
		_dirs[3] = new Vector3(sunDirection.z, 0f, 0f - sunDirection.x);
		_dirs[4] = new Vector3(0f - sunDirection.z, 0f, sunDirection.x);
		float t2 = AmbientContrast.Evaluate(t);
		float num4 = AmbientSaturation.Evaluate(t);
		float num5 = AmbientBrightness.Evaluate(t);
		float light = Mathf.Lerp(0.003f, 0.25f, Mathf.InverseLerp(0.07f, 1f, t));
		float t3 = Mathf.Lerp(1f, 0.3f, fogginess);
		num4 *= 1f - fogginess;
		float num6 = Mathf.InverseLerp(-0.4f, 0.2f, num3);
		float num7 = 1f + num6;
		num *= num7;
		float num8 = Mathf.Max(1f - (num3 + fogginess) * 1.3f, 0f);
		TopHorizontSkyColor = Color.clear;
		Color color3 = todSky.Components.LightSource.color;
		color3 = ContrastColor(color3, t2);
		color3 = _E001(light, color3, t3);
		color3 = SaturateColor(color3, num4);
		color3 *= num5;
		color3 = _E000(color3);
		Vector3 vector = -todSky.SunDirection;
		vector = (vector + Vector3.down).normalized;
		float intensity = BounceHarmonicIntensity.Evaluate(t) * num8;
		result.SHWithoutTop.AddDirectionalLight(vector, color3, intensity);
		Color color4 = AddAmbient.Evaluate(t01);
		color4 += CloudnessLightColor * Mathf.Clamp01(t) * num7;
		color4 = ContrastColor(color4, t2);
		color4 = _E001(light, color4, t3);
		color4 = SaturateColor(color4, num4);
		color4 *= num5;
		color4 = _E000(color4);
		result.SHWithoutTop.AddAmbientLight(color4);
		result.FullSH += result.SHWithoutTop;
		for (int i = 0; i < _dirs.Length; i++)
		{
			Vector3 direction = _dirs[i];
			Color color5 = todSky.SampleAtmosphereRaw(direction, directLight: false);
			if (i == 1)
			{
				color5 *= color;
			}
			if (i == 2)
			{
				color5 *= color2;
			}
			color5 = ContrastColor(color5, t2);
			color5 = _E001(light, color5, t3);
			color5 = SaturateColor(color5, num4);
			color5 *= num5;
			color5 = _E000(color5);
			float intensity2 = ((i == 0) ? num : num2);
			result.FullSH.AddDirectionalLight(direction, color5, intensity2);
			TopHorizontSkyColor += color5;
		}
		Color color6 = AddTopAmbient.Evaluate(t01);
		color6 = ContrastColor(color6, t2);
		color6 = _E001(light, color6, t3);
		color6 = SaturateColor(color6, num4);
		color6 *= num5;
		color6 = _E000(color6);
		result.FullSH.AddDirectionalLight(Vector3.up, color6, 1f);
		SphericalHarmonicsL2 sphericalHarmonicsL = default(SphericalHarmonicsL2);
		sphericalHarmonicsL.AddAmbientLight(EnvironmentColor);
		float num9 = 1f - HarmonicsDaylight;
		sphericalHarmonicsL *= HarmonicsDaylight;
		result.FullSH = result.FullSH * num9 + sphericalHarmonicsL;
		result.SHWithoutTop = result.SHWithoutTop * num9 + sphericalHarmonicsL;
		return result;
	}

	private static Color _E000(Color color)
	{
		return new Color(Mathf.Max(0f, color.r), Mathf.Max(0f, color.g), Mathf.Max(0f, color.b));
	}

	public static Color SaturateColor(Color color, float t)
	{
		float a = (color.r + color.g + color.b) * (1f / 3f);
		return new Color(Mathf.LerpUnclamped(a, color.r, t), Mathf.LerpUnclamped(a, color.g, t), Mathf.LerpUnclamped(a, color.b, t));
	}

	public static Color ContrastColor(Color color, float t)
	{
		return new Color(Mathf.LerpUnclamped(0.5f, color.r, t), Mathf.LerpUnclamped(0.5f, color.g, t), Mathf.LerpUnclamped(0.5f, color.b, t));
	}

	private static Color _E001(float light, Color color, float t)
	{
		return new Color(Mathf.LerpUnclamped(light, color.r, t), Mathf.LerpUnclamped(light, color.g, t), Mathf.LerpUnclamped(light, color.b, t));
	}
}
