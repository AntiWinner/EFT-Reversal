using System.Collections.Generic;

namespace EFT.Quests;

public sealed class ConditionSellItemToTrader : ConditionMultipleTargets
{
	public string trader;

	public override string FormattedDescription => string.Format(base.FormattedDescription, trader);

	protected override List<object> IdentityFields()
	{
		List<object> list = base.IdentityFields();
		list.Add(trader);
		return list;
	}
}
