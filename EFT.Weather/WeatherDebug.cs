using System;
using UnityEngine;

namespace EFT.Weather;

[Serializable]
public class WeatherDebug : IWeatherCurve
{
	public enum Direction
	{
		East = 1,
		North,
		West,
		South,
		SE,
		SW,
		NW,
		NE
	}

	[SerializeField]
	private bool isEnabled;

	public bool IsDynamicSunWeatherDebug;

	[Range(0f, 1f)]
	public float WindMagnitude;

	public Direction WindDirection = Direction.North;

	public Vector2 TopWindDirection;

	[Range(-1f, 1f)]
	public float CloudDensity;

	[Range(0f, 1f)]
	public float Fog;

	[Range(0f, 1f)]
	public float Rain;

	[Range(0f, 1f)]
	public float LightningThunderProbability;

	[Range(-50f, 50f)]
	public float Temperature;

	[SerializeField]
	internal TOD_CycleParameters Date;

	private static WeatherDebug cachedWeather;

	Vector2 IWeatherCurve.Wind => _E8EB.WindDirections[(int)WindDirection] * WindMagnitude;

	Vector2 IWeatherCurve.TopWind => TopWindDirection;

	float IWeatherCurve.Rain => Rain;

	float IWeatherCurve.Cloudiness => CloudDensity;

	float IWeatherCurve.Fog => Fog;

	float IWeatherCurve.Temperature => Temperature;

	float IWeatherCurve.LightningThunderProbability => LightningThunderProbability;

	public bool Enabled
	{
		get
		{
			return isEnabled;
		}
		set
		{
			isEnabled = value;
		}
	}

	public void CopyParams(IWeatherCurve curve)
	{
		WindMagnitude = curve.Wind.magnitude;
		WindDirection = Direction.North;
		TopWindDirection = curve.TopWind;
		CloudDensity = curve.Cloudiness;
		Fog = curve.Fog;
		Rain = curve.Rain;
		Temperature = curve.Temperature;
	}

	public void CopyParams(WeatherDebug from)
	{
		IsDynamicSunWeatherDebug = from.IsDynamicSunWeatherDebug;
		WindMagnitude = from.WindMagnitude;
		WindDirection = from.WindDirection;
		TopWindDirection = from.TopWindDirection;
		CloudDensity = from.CloudDensity;
		Fog = from.Fog;
		Rain = from.Rain;
		LightningThunderProbability = from.LightningThunderProbability;
		Temperature = from.Temperature;
		Date.DateTime = from.Date.DateTime;
	}

	public void SavePreset()
	{
		cachedWeather = _E000(this);
	}

	public void LoadPreset()
	{
		CopyParams(cachedWeather);
	}

	private WeatherDebug _E000(WeatherDebug from)
	{
		return new WeatherDebug
		{
			IsDynamicSunWeatherDebug = from.IsDynamicSunWeatherDebug,
			WindMagnitude = from.WindMagnitude,
			WindDirection = from.WindDirection,
			TopWindDirection = from.TopWindDirection,
			CloudDensity = from.CloudDensity,
			Fog = from.Fog,
			Rain = from.Rain,
			LightningThunderProbability = from.LightningThunderProbability,
			Temperature = from.Temperature,
			Date = new TOD_CycleParameters(from.Date)
		};
	}
}
