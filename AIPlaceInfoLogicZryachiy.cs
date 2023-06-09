using EFT;

public class AIPlaceInfoLogicZryachiy : AIPlaceInfoLogic
{
	public bool CloseZone;

	public bool AttackPossible;

	public bool PermanentVision;

	public bool ControllableZone;

	public bool FollowersPositions;

	public bool BossPositions;

	public bool LookAtCenter;

	private int _E005 = -1;

	public bool CanTakeBy(BotOwner bot)
	{
		if (bot.Id != _E005)
		{
			return _E005 <= 0;
		}
		return true;
	}

	public void TakeBy(BotOwner bot)
	{
		_E005 = bot.Id;
	}

	public void ReleaseBy(BotOwner bot)
	{
		if (_E005 == bot.Id)
		{
			_E005 = -1;
		}
	}
}
