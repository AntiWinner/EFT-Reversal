using EFT.Interactive;
using UnityEngine;

namespace EFT.GlobalEvents;

public class DoorInteractingEventFilter : BaseEventFilter
{
	[SerializeField]
	private Door _doorForTracking;

	[SerializeField]
	private bool _doorOpened;

	[SerializeField]
	private bool _doorClosed;

	protected override bool IsFilterPassed(_EBAD eventToFilter)
	{
		if (eventToFilter is _EBB7 obj && _doorForTracking != null && obj.Door.Id == _doorForTracking.Id)
		{
			return obj.Door.DoorState switch
			{
				EDoorState.Open => _doorOpened, 
				EDoorState.Shut => _doorClosed, 
				_ => false, 
			};
		}
		return false;
	}
}
