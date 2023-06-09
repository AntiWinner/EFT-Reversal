using System.Linq;

namespace EFT.Interactive;

public class SharedExfiltrationPoint : ScavExfiltrationPoint
{
	public bool IsMandatoryForScavs;

	public override bool InfiltrationMatch(Player player)
	{
		if (player.Profile.Info.Side != EPlayerSide.Savage)
		{
			if (!string.IsNullOrEmpty(player.Profile.Info.EntryPoint))
			{
				return EligibleEntryPoints.Contains(player.Profile.Info.EntryPoint.ToLower());
			}
			return false;
		}
		return true;
	}
}
