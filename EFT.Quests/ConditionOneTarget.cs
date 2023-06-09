using System.Collections.Generic;

namespace EFT.Quests;

public abstract class ConditionOneTarget : Condition
{
	public string target;

	public override string FormattedDescription => string.Format(base.FormattedDescription, target, base.value);

	protected override List<object> IdentityFields()
	{
		List<object> list = base.IdentityFields();
		list.Add(target);
		return list;
	}
}
