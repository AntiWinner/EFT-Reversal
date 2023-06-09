using System;
using Newtonsoft.Json;

namespace EFT;

[Serializable]
public class ExchangeRateDTO
{
	[JsonProperty("exchange_rate")]
	public double ExchangeRate;
}
