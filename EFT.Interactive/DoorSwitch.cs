using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace EFT.Interactive;

public class DoorSwitch : Door
{
	public Switch SwitchToTrigger;

	public bool HasAuthority => !WorldInteractiveObject.InteractionShouldBeConfirmed;

	protected override IEnumerator SmoothDoorOpenCoroutine(EDoorState state, bool isLocalInteraction, float speed = 1f)
	{
		yield return base.SmoothDoorOpenCoroutine(state, isLocalInteraction, speed);
		if (state == EDoorState.Open && HasAuthority && SwitchToTrigger.DoorState != EDoorState.Interacting && SwitchToTrigger.DoorState != EDoorState.Locked)
		{
			SwitchToTrigger.LockForInteraction();
			SwitchToTrigger.Interact(new _EBFE(EInteractionType.Open));
		}
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private IEnumerator _E000(EDoorState state, bool isLocalInteraction, float speed)
	{
		return base.SmoothDoorOpenCoroutine(state, isLocalInteraction, speed);
	}
}
