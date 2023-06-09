using EFT;

public class PatrolWayWithConditions : PatrolWay
{
	public SpawnTriggerType TriggerType;

	public string Id;

	public override bool Suitable(BotOwner bot, _E301 data)
	{
		_E2F2 spawnParams = data.SpawnParams;
		if (spawnParams == null)
		{
			return true;
		}
		if (spawnParams.TriggerType == SpawnTriggerType.none)
		{
			return true;
		}
		if (spawnParams.TriggerType == TriggerType)
		{
			if (spawnParams.Id_spawn.Length <= 0 || Id.Length <= 0)
			{
				return true;
			}
			if (spawnParams.Id_spawn == Id)
			{
				return true;
			}
		}
		return false;
	}
}
