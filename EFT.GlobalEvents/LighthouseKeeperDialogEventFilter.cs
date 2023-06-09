using UnityEngine;

namespace EFT.GlobalEvents;

public class LighthouseKeeperDialogEventFilter : BaseEventFilter
{
	[SerializeField]
	private _EBBD.EDialogState _neededDialogOption;

	protected override bool IsFilterPassed(_EBAD eventToFilter)
	{
		if (eventToFilter is _EBBD obj)
		{
			return obj.invokedDialogOption == _neededDialogOption;
		}
		return false;
	}
}
