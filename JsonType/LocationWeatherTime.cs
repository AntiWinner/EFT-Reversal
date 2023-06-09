using Newtonsoft.Json;

namespace JsonType;

public class LocationWeatherTime
{
	[JsonProperty("weather")]
	public _E8EB Weather;

	[JsonProperty("acceleration")]
	public float Acceleration;

	[JsonProperty("date")]
	public string Date;

	[JsonProperty("time")]
	public string Time;

	public static LocationWeatherTime CreateDefault()
	{
		return new LocationWeatherTime(_E8EB.CreateDefault(), 0f, "", "");
	}

	public LocationWeatherTime(_E8EB node, float acceleration, string date, string time)
	{
		Weather = node;
		Acceleration = acceleration;
		Date = date;
		Time = time;
	}
}
