using System;
using Newtonsoft.Json;

namespace EFT;

[Serializable]
public sealed class TraderData
{
	[JsonProperty("salesSum")]
	private long? _salesSum;

	[JsonProperty("standing")]
	private double? _standing;

	[JsonProperty("loyalty")]
	private float? _loyaltyLevel;

	[JsonProperty("unlocked")]
	private bool? _unlocked;

	[JsonProperty("disabled")]
	private bool? _disabled;

	public void Apply(Profile._E001 traderInfo)
	{
		if (_salesSum.HasValue)
		{
			traderInfo.SetSalesSum(_salesSum.Value);
		}
		if (_standing.HasValue)
		{
			traderInfo.SetStanding(_standing.Value);
		}
		if (_unlocked.HasValue)
		{
			traderInfo.SetUnlocked(_unlocked.Value);
		}
		if (_disabled.HasValue)
		{
			traderInfo.SetDisabled(_disabled.Value);
		}
	}
}
