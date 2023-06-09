using System;
using System.Runtime.CompilerServices;
using EFT;

namespace Cutscene;

public class LighthouseKeeperExitCutsceneTrigger : BaseCutsceneTrigger
{
	private Action _E003;

	protected override void Awake()
	{
		base.Awake();
		_E003 = _EBAF.Instance.SubscribeOnEvent(delegate(_EBBD raisedEvent)
		{
			_E000(raisedEvent);
		});
	}

	private void OnDestroy()
	{
		_E003?.Invoke();
		_E003 = null;
	}

	private void _E000(_EBBD raisedEvent)
	{
		if (raisedEvent.invokedDialogOption == _EBBD.EDialogState.DialogClosed && GamePlayerOwner.MyPlayer != null)
		{
			CallStartCutscene(GamePlayerOwner.MyPlayer);
		}
	}

	[CompilerGenerated]
	private void _E001(_EBBD raisedEvent)
	{
		_E000(raisedEvent);
	}
}
