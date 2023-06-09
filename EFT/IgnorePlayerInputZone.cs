using EFT.GlobalEvents;
using UnityEngine;

namespace EFT;

public class IgnorePlayerInputZone : MonoBehaviour
{
	[SerializeField]
	private BaseEventFilter _setIgnoreInputFilter;

	[SerializeField]
	private BaseEventFilter _giveInputBackFilter;

	[SerializeField]
	private bool canRotateHead = true;

	private void Awake()
	{
		_setIgnoreInputFilter.OnFilterPassed += _E000;
		_giveInputBackFilter.OnFilterPassed += _E001;
	}

	private void _E000(BaseEventFilter filter, _EBAD passedEvent)
	{
		if (canRotateHead)
		{
			GamePlayerOwner.SetIgnoreInputInNPCDialog(ignore: true);
		}
		else
		{
			GamePlayerOwner.SetIgnoreInput(ignore: true);
		}
	}

	private void _E001(BaseEventFilter filter, _EBAD passedEvent)
	{
		if (canRotateHead)
		{
			GamePlayerOwner.SetIgnoreInputInNPCDialog(ignore: false);
		}
		else
		{
			GamePlayerOwner.SetIgnoreInput(ignore: false);
		}
	}
}
