namespace EFT.Hideout;

public class ShootingRangeBehaviour : _E831
{
	private HideoutPlayerOwner _E013;

	public void ManualEnterLocation(HideoutPlayerOwner player)
	{
		_E013 = player;
		AreaData data = base.Data;
		if (data == null || data.CurrentLevel > 0)
		{
			_E013.EnterShootingRange();
		}
	}

	public override void BehaviourUpdate()
	{
		base.BehaviourUpdate();
		AreaData data = base.Data;
		if ((data == null || data.CurrentLevel > 0) && !(_E013 == null))
		{
			if (!_E013.InShootingRange)
			{
				_E013 = null;
			}
			else
			{
				_E013.DecidePatrolStatus();
			}
		}
	}

	public override void OnExitLocation()
	{
		base.OnExitLocation();
		if (!(_E013 == null))
		{
			if (!_E013.InShootingRange)
			{
				_E013 = null;
				return;
			}
			_E013.ExitShootingRange().HandleExceptions();
			_E013 = null;
		}
	}
}
