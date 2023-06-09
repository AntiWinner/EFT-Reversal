using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace EFT.Hideout;

[Serializable]
public sealed class TraderLoyaltyRequirement : _E83C
{
	[JsonProperty("loyaltyLevel")]
	public int LoyaltyLevel;

	[JsonProperty("traderId")]
	public string TraderId;

	public override ERequirementType Type => ERequirementType.TraderLoyalty;

	public override void Test(IEnumerable<Profile._E001> value)
	{
		base.Trader = value.First((Profile._E001 traderInfo) => traderInfo.Id == TraderId);
		if (base.Trader != null)
		{
			TestRequirement(base.Trader.LoyaltyLevel, LoyaltyLevel);
		}
	}

	[CompilerGenerated]
	private bool _E000(Profile._E001 traderInfo)
	{
		return traderInfo.Id == TraderId;
	}
}
