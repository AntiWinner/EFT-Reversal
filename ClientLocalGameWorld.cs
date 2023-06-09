using EFT;

internal class ClientLocalGameWorld : ClientGameWorld
{
	protected override void Awake()
	{
		SpeedLimitsEnabled = true;
		base.Awake();
	}

	protected override bool IsLocalGame()
	{
		return true;
	}
}
