using UnityEngine;

namespace EFT.Weather;

public interface IWeatherCurve
{
	Vector2 Wind { get; }

	Vector2 TopWind { get; }

	float Rain { get; }

	float Cloudiness { get; }

	float Fog { get; }

	float Temperature { get; }

	float LightningThunderProbability { get; }
}
