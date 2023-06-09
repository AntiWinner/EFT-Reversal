using UnityEngine;

namespace EFT.GlobalEvents;

public class InteractWithKeeperZoneEventFilter : BaseEventFilter
{
	[SerializeField]
	private _EBBC.EInteractState _neededState;

	protected override bool IsFilterPassed(_EBAD eventToFilter)
	{
		if (eventToFilter is _EBBC obj && obj.InteractingPlayer.IsYourPlayer)
		{
			return obj.InteractState == _neededState;
		}
		return false;
	}
}
