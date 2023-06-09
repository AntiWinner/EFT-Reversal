using System.Linq;

namespace EFT.Quests;

public class ConditionItem : ConditionMultipleTargets
{
	public int minDurability;

	public int maxDurability;

	public int dogtagLevel;

	public bool onlyFoundInRaid;

	public bool isEncoded;

	public override string FormattedDescription => string.Format(base.FormattedDescription, (target.First() + " ShortName").Localized(), base.value, minDurability, maxDurability, dogtagLevel, onlyFoundInRaid, isEncoded);
}
