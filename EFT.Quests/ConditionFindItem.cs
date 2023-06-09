using System.Linq;
using Comfort.Common;

namespace EFT.Quests;

public sealed class ConditionFindItem : ConditionItem
{
	public bool countInRaid;

	private bool? _targetIsCategory;

	public bool TargetIsCategory
	{
		get
		{
			if (!_targetIsCategory.HasValue)
			{
				_targetIsCategory = target.Any((string t) => Singleton<_EBA8>.Instance.IsCategory(t));
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
			if (target.Length == 0)
			{
				return base.FormattedDescription;
			}
			string arg = ((target.Length > 1) ? string.Join("StringSeparator/Or".Localized(), target.Select((string t) => t.Localized())) : target[0].Localized());
			return string.Format("QuestCondition/Category".Localized(), arg);
		}
	}
}
