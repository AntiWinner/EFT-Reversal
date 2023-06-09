using System;
using EFT.Weather;
using Newtonsoft.Json;

namespace EFT;

[Serializable]
public struct TimeAndWeatherSettings
{
	[JsonProperty("isRandomTime")]
	public bool IsRandomTime;

	[JsonProperty("isRandomWeather")]
	public bool IsRandomWeather;

	[JsonProperty("cloudinessType")]
	public ECloudinessType CloudinessType;

	[JsonProperty("rainType")]
	public ERainType RainType;

	[JsonProperty("windType")]
	public EWindSpeed WindType;

	[JsonProperty("fogType")]
	public EFogType FogType;

	[JsonProperty("timeFlowType")]
	public ETimeFlowType TimeFlowType;

	[JsonProperty("hourOfDay")]
	public int HourOfDay;

	public TimeAndWeatherSettings(bool randomTime, bool randomWeather, int cloudinessType, int rainType, int windSpeed, int fogType, int timeFlowType, int hourOfDay = -1)
	{
		IsRandomTime = randomTime;
		IsRandomWeather = randomWeather;
		CloudinessType = (ECloudinessType)cloudinessType;
		RainType = (ERainType)rainType;
		WindType = (EWindSpeed)windSpeed;
		FogType = (EFogType)fogType;
		TimeFlowType = (ETimeFlowType)timeFlowType;
		HourOfDay = hourOfDay;
	}
}
