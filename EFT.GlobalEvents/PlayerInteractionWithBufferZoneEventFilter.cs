using UnityEngine;

namespace EFT.GlobalEvents;

public class PlayerInteractionWithBufferZoneEventFilter : BaseEventFilter
{
	[SerializeField]
	private _EBC1.EInteractState _neededState;

	protected override bool IsFilterPassed(_EBAD eventToFilter)
	{
		if (eventToFilter is _EBC1 obj)
		{
			return obj.InteractState == _neededState;
		}
		return false;
	}
}
