namespace EFT.Interactive;

public interface IExfiltrationRequirement
{
	bool Met(Player player, ExfiltrationPoint point);

	void Enter(Player player, ExfiltrationPoint point);

	void Exit(Player player, ExfiltrationPoint point);

	void Start(ExfiltrationPoint point);

	void OnDestroy();
}
