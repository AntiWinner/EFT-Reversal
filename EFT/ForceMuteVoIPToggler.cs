using EFT.GlobalEvents;
using UnityEngine;

namespace EFT;

public class ForceMuteVoIPToggler : MonoBehaviour
{
	[SerializeField]
	private BaseEventFilter _disableVoipEventFilter;

	[SerializeField]
	private BaseEventFilter _enableVoipEventFilter;

	private void Awake()
	{
		_disableVoipEventFilter.OnFilterPassed += _E000;
		_enableVoipEventFilter.OnFilterPassed += _E001;
	}

	private void _E000(BaseEventFilter filter, _EBAD passedEvent)
	{
		if (GamePlayerOwner.MyPlayer != null)
		{
			GamePlayerOwner.MyPlayer.VoipController.ForceMuteVoIP(enable: true);
		}
	}

	private void _E001(BaseEventFilter filter, _EBAD passedEvent)
	{
		if (GamePlayerOwner.MyPlayer != null)
		{
			GamePlayerOwner.MyPlayer.VoipController.ForceMuteVoIP(enable: false);
		}
	}
}
