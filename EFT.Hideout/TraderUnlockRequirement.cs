using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace EFT.Hideout;

[Serializable]
public sealed class TraderUnlockRequirement : _E83C
{
	[JsonProperty("traderId")]
	public string TraderId;

	public override ERequirementType Type => ERequirementType.TraderUnlock;

	public override void Test(IEnumerable<Profile._E001> value)
	{
		base.Trader = value.First((Profile._E001 traderInfo) => traderInfo.Id == TraderId);
		SetFulfillment(base.Trader?.Available ?? false);
	}

	[CompilerGenerated]
	private bool _E000(Profile._E001 traderInfo)
	{
		return traderInfo.Id == TraderId;
	}
}
