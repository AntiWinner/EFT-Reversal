namespace EFT.Quests;

public sealed class ConditionLevel : Condition
{
	public override string FormattedDescription => string.Format(base.FormattedDescription, base.value);
}
