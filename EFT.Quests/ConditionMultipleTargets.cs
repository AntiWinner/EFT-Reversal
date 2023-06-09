using System.Collections.Generic;

namespace EFT.Quests;

public abstract class ConditionMultipleTargets : Condition
{
	public string[] target;

	protected override List<object> IdentityFields()
	{
		List<object> list = base.IdentityFields();
		list.AddRange(target);
		return list;
	}
}
