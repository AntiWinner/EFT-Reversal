using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT.Interactive;

public class Switch : WorldInteractiveObject
{
	[Serializable]
	public class SwitchAndOperation
	{
		public Switch Switch;

		public EInteractionType Operation;
	}

	[Header("-------------  Switch -----------")]
	public bool DontAnimateRotation;

	public string ContextMenuTip;

	public float Delay;

	[Header("Exfiltration Zone")]
	public ExfiltrationPoint ExfiltrationPoint;

	public EExfiltrationStatus TargetStatus;

	public EExfiltrationStatus[] ConditionStatus;

	public string ExtractionZoneTip;

	[Header("Other Switches")]
	public SwitchAndOperation[] NextSwitches;

	public Switch PreviousSwitch;

	[Header("Door")]
	public Door Door;

	public EInteractionType Interaction;

	[Header("Lamps")]
	public LampController[] Lamps;

	[Header("Curve is played 1.5x faster. Consider!")]
	public AnimationCurve CustomProgressCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

	public Vector3 ShutPosition;

	public Vector3 OpenPosition;

	public override float CurrentAngle
	{
		get
		{
			return base.CurrentAngle;
		}
		protected set
		{
			if (DontAnimateRotation)
			{
				_currentAngle = value;
			}
			else
			{
				base.CurrentAngle = value;
			}
			if (ShutPosition.sqrMagnitude > 0f || OpenPosition.sqrMagnitude > 0f)
			{
				ProcessAngleAsShift(value);
			}
		}
	}

	public bool HasAuthority => !WorldInteractiveObject.InteractionShouldBeConfirmed;

	public override string TypeKey => _ED3E._E000(212977);

	public override AnimationCurve ProgressCurve => CustomProgressCurve;

	public override float AngleSyncForSync => 1.5f;

	public override float MaxAllowedAngleDesync => 10f;

	public override void Interact(_EBFE interactionResult)
	{
		switch (interactionResult.InteractionType)
		{
		case EInteractionType.Open:
			Open();
			break;
		case EInteractionType.Close:
			Close();
			break;
		case EInteractionType.Unlock:
			Unlock();
			break;
		case EInteractionType.Lock:
			Lock();
			break;
		case EInteractionType.Breach:
			break;
		}
	}

	protected override IEnumerator SmoothDoorOpenCoroutine(EDoorState state, bool isLocalInteraction, float speed = 1f)
	{
		yield return base.SmoothDoorOpenCoroutine(state, isLocalInteraction, speed);
		if (Delay > 0f)
		{
			yield return new WaitForSeconds(Delay);
		}
		if (state != EDoorState.Open)
		{
			yield break;
		}
		SwitchAndOperation[] nextSwitches = NextSwitches;
		foreach (SwitchAndOperation switchAndOperation in nextSwitches)
		{
			if (switchAndOperation.Switch.DoorState != EDoorState.Interacting && (switchAndOperation.Operation != 0 || switchAndOperation.Switch.DoorState != EDoorState.Locked) && HasAuthority)
			{
				switchAndOperation.Switch.LockForInteraction();
				switchAndOperation.Switch.Interact(new _EBFE(switchAndOperation.Operation));
			}
		}
		if ((bool)Door && Door.DoorState != EDoorState.Interacting)
		{
			if (Door.DoorState == EDoorState.Locked)
			{
				Door.DoorState = EDoorState.Shut;
			}
			Door.LockForInteraction();
			Door.Interact(new _EBFE(Interaction));
		}
		if (ExfiltrationPoint != null && (ConditionStatus == null || ConditionStatus.Length < 1 || ConditionStatus.Contains(ExfiltrationPoint.Status)))
		{
			ExfiltrationPoint.ExternalSetStatus(TargetStatus);
		}
		if (Lamps == null)
		{
			yield break;
		}
		LampController[] lamps = Lamps;
		foreach (LampController lampController in lamps)
		{
			if (lampController.LampState != Turnable.EState.Destroyed)
			{
				lampController.Switch(Turnable.EState.On);
				yield return null;
			}
		}
	}

	public override _E001 GetInteractionParameters(Vector3 yourPosition)
	{
		_E001 result = default(_E001);
		result.InteractionPosition = base.transform.position;
		result.Grip = null;
		result.AnimationId = PushID;
		result.ViewTarget = GetViewDirection(yourPosition);
		result.Snap = (DoorState & Snap) != 0;
		result.InitialState = DoorState;
		result.RotationMode = ERotationInterpolationMode.ViewTarget;
		return result;
	}

	public string GetTip()
	{
		if (PreviousSwitch == null)
		{
			return ExtractionZoneTip;
		}
		if (DoorState == EDoorState.Locked || (DoorState == EDoorState.Shut && !Operatable) || PreviousSwitch.DoorState == EDoorState.Shut || PreviousSwitch.DoorState == EDoorState.Interacting)
		{
			return PreviousSwitch.GetTip();
		}
		return ExtractionZoneTip;
	}

	protected virtual void ProcessAngleAsShift(float value)
	{
		base.transform.localPosition = Vector3.Lerp(ShutPosition, OpenPosition, value / OpenAngle);
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private IEnumerator _E000(EDoorState state, bool isLocalInteraction, float speed)
	{
		return base.SmoothDoorOpenCoroutine(state, isLocalInteraction, speed);
	}
}
