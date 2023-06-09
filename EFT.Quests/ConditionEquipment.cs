using System.Collections.Generic;
using System.Linq;
using Comfort.Common;

namespace EFT.Quests;

public sealed class ConditionEquipment : Condition
{
	public bool IncludeNotEquippedItems;

	public string[][] equipmentInclusive;

	public string[][] equipmentExclusive;

	private bool? _targetIsCategory;

	public bool TargetIsCategory
	{
		get
		{
			if (!_targetIsCategory.HasValue)
			{
				_targetIsCategory = equipmentInclusive.Any((string[] group) => group.Any((string target) => Singleton<_EBA8>.Instance.IsCategory(target)));
			}
			return _targetIsCategory.Value;
		}
	}

	public override string FormattedDescription
	{
		get
		{
			if (!TargetIsCategory)
			{
				return base.FormattedDescription;
			}
			List<string> list = equipmentInclusive.SelectMany((string[] e) => e).ToList();
			if (list.Count == 0)
			{
				return base.FormattedDescription;
			}
			string arg = ((list.Count > 1) ? string.Join("StringSeparator/Or".Localized(), list.Select((string t) => t.Localized())) : list[0].Localized());
			return string.Format("QuestCondition/Inventory".Localized(), arg);
		}
	}
}
