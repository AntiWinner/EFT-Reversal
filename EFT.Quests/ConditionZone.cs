namespace EFT.Quests;

public class ConditionZone : ConditionMultipleTargets
{
	public string zoneId;

	public override string FormattedDescription => string.Format(base.FormattedDescription, zoneId);
}
