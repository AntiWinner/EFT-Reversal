using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace EFT;

[Serializable]
public sealed class SupplyData : IBasePriceSource
{
	[JsonProperty("supplyNextTime")]
	public int SupplyNextTime;

	[JsonProperty("prices")]
	public Dictionary<string, double> MarketPrices;

	[JsonProperty("currencyCourses")]
	public Dictionary<string, double> CurrencyCourses;

	public double GetBasePrice(string itemId)
	{
		if (!MarketPrices.TryGetValue(itemId, out var value))
		{
			Debug.Log(string.Format(_ED3E._E000(181995), itemId, itemId.LocalizedName(), value));
		}
		return value;
	}
}
