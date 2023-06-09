namespace EFT.Quests;

public sealed class ConditionPlaceBeacon : ConditionZone
{
	public float plantTime;

	public override string FormattedDescription => string.Format(base.FormattedDescription, zoneId, plantTime);
}
