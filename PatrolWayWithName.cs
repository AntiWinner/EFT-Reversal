using EFT;

public class PatrolWayWithName : PatrolWay
{
	public string NameId;

	public override bool Suitable(BotOwner bot, _E301 data)
	{
		return bot.PatrollingData.PointChooser.IsWaySuitableByNameId(NameId);
	}

	public override string Suitabledata()
	{
		return _ED3E._E000(8300) + NameId;
	}
}
