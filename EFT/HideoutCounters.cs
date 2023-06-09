using System;
using Newtonsoft.Json;

namespace EFT;

[Serializable]
public class HideoutCounters
{
	[JsonProperty("fuelCounter")]
	public float FuelCounter;

	[JsonProperty("airFilterCounter")]
	public float AirFilterCounter;

	[JsonProperty("waterFilterCounter")]
	public float WaterFilterCounter;

	[JsonProperty("craftingTimeCounter")]
	public float CraftingTimeCounter;

	public float GetCounter(EAreaType areaType)
	{
		return areaType switch
		{
			EAreaType.Generator => FuelCounter, 
			EAreaType.AirFilteringUnit => AirFilterCounter, 
			EAreaType.WaterCollector => WaterFilterCounter, 
			_ => throw new ArgumentOutOfRangeException(), 
		};
	}
}
