namespace EFT.Quests;

public sealed class ConditionLeaveItemAtLocation : ConditionZone
{
	public float plantTime;

	public override string FormattedDescription => string.Format(base.FormattedDescription, zoneId, plantTime);
}
