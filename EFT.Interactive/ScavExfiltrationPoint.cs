using System.Collections.Generic;

namespace EFT.Interactive;

public class ScavExfiltrationPoint : ExfiltrationPoint
{
	public List<string> EligibleIds = new List<string>();

	public override bool InfiltrationMatch(Player player)
	{
		return EligibleIds.Contains(player.Profile.Id);
	}
}
