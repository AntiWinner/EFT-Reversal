using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT.Interactive;

public class KeycardDoor : Door
{
	[SerializeField]
	private string[] _additionalKeys;

	[SerializeField]
	private bool _openOnUnlock;

	[SerializeField]
	private bool _lockOnShut;

	public AudioClip DeniedBeep;

	public AudioClip GrantedBeep;

	public AudioClip UnlockSound;

	private GripPose _E02C;

	public InteractiveProxy[] Proxies;

	public override void Interact(_EBFE interactionResult)
	{
		switch (interactionResult.InteractionType)
		{
		case EInteractionType.Open:
			if (DoorState != EDoorState.Locked)
			{
				Open();
			}
			break;
		case EInteractionType.Close:
			Close();
			break;
		case EInteractionType.Unlock:
			if (((_EBFF)interactionResult).Succeed)
			{
				Unlock();
			}
			else
			{
				StartCoroutine(_E000());
			}
			break;
		case EInteractionType.Breach:
			KickOpen(confirmed: false);
			break;
		}
	}

	private IEnumerator _E000()
	{
		DoorHandle handle = GetHandle();
		if (handle != null)
		{
			yield return handle.OpenCoroutine();
		}
		DoorState = base.FallbackState;
		yield return _E001(success: false);
	}

	protected override IEnumerator SmoothDoorOpenCoroutine(EDoorState state, bool isLocalInteraction, float speed = 1f)
	{
		yield return base.SmoothDoorOpenCoroutine(state, isLocalInteraction, speed);
		if (state == EDoorState.Shut && _lockOnShut)
		{
			Lock();
		}
	}

	protected override IEnumerator UnlockCoroutine()
	{
		DoorHandle handle = GetHandle();
		if (handle != null)
		{
			yield return handle.OpenCoroutine();
		}
		if (!_openOnUnlock)
		{
			DoorState = EDoorState.Shut;
		}
		CurrentAngle = GetAngle(EDoorState.Shut);
		yield return _E001(success: true);
		if (_openOnUnlock)
		{
			LockForInteraction();
			Interact(new _EBFE(EInteractionType.Open));
		}
	}

	private IEnumerator _E001(bool success)
	{
		yield return new WaitForSeconds(0.5f);
		AudioClip audioClip = (success ? GrantedBeep : DeniedBeep);
		if (audioClip != null)
		{
			MonoBehaviourSingleton<BetterAudio>.Instance.PlayAtPoint((_E02C != null) ? _E02C.transform.position : base.transform.position, audioClip, _E8A8.Instance.Distance(base.transform.position), BetterAudio.AudioSourceGroupType.Environment, 15, 0.7f, EOcclusionTest.Fast);
		}
		if (!success)
		{
			InteractiveProxy[] proxies = Proxies;
			for (int i = 0; i < proxies.Length; i++)
			{
				proxies[i].StartFlicker();
			}
		}
		else
		{
			float seconds = ((audioClip != null) ? audioClip.length : 0f);
			yield return new WaitForSeconds(seconds);
			MonoBehaviourSingleton<BetterAudio>.Instance.PlayAtPoint((_E02C != null) ? _E02C.transform.position : base.transform.position, UnlockSound, _E8A8.Instance.Distance(base.transform.position), BetterAudio.AudioSourceGroupType.Environment, 10, 0.7f, EOcclusionTest.Fast);
		}
	}

	public override void OnEnable()
	{
		base.OnEnable();
		InteractiveProxy[] proxies = Proxies;
		foreach (InteractiveProxy interactiveProxy in proxies)
		{
			Grips = Grips.Concat(interactiveProxy.Grips).ToArray();
		}
	}

	public DoorHandle GetHandle()
	{
		InteractiveProxy interactiveProxy = null;
		if (_E02C != null)
		{
			interactiveProxy = Proxies.FirstOrDefault((InteractiveProxy x) => x.Grips.Contains(_E02C));
		}
		if (interactiveProxy != null)
		{
			return interactiveProxy.Handle;
		}
		return Proxies[0].Handle;
	}

	public override WorldInteractiveObject._E001 GetInteractionParameters(Vector3 yourPosition)
	{
		if (DoorState != EDoorState.Locked)
		{
			return base.GetInteractionParameters(yourPosition);
		}
		_E02C = GetClosestGrip(yourPosition);
		InteractiveProxy interactiveProxy = Proxies.FirstOrDefault((InteractiveProxy p) => p.Grips.Contains(_E02C));
		DoorHandle handle = GetHandle();
		if (handle != null)
		{
			handle.DefPos();
		}
		if (interactiveProxy != null)
		{
			WorldInteractiveObject._E001 result = default(WorldInteractiveObject._E001);
			result.InteractionPosition = interactiveProxy.GetInteractionPosition(yourPosition);
			result.Grip = GetClosestGrip(yourPosition);
			result.AnimationId = 50;
			result.ViewTarget = interactiveProxy.GetViewDirection(yourPosition);
			result.Snap = true;
			result.InitialState = DoorState;
			result.RotationMode = ERotationInterpolationMode.ViewTarget;
			return result;
		}
		return base.GetInteractionParameters(yourPosition);
	}

	public override _ECD9<_EBFF> UnlockOperation(KeyComponent key, Player player)
	{
		_ECD1 canInteract = player.MovementContext.CanInteract;
		if (canInteract != null)
		{
			return canInteract;
		}
		bool num = key.Template.KeyId == KeyId || (_additionalKeys != null && _additionalKeys.Contains(key.Template.KeyId));
		_ECD8<_EB38> obj = default(_ECD8<_EB38>);
		if (!num)
		{
			return new _EBFF(key, null, succeed: false);
		}
		key.NumberOfUsages++;
		if (key.NumberOfUsages >= key.Template.MaximumNumberOfUsage && key.Template.MaximumNumberOfUsage > 0)
		{
			obj = _EB29.Discard(key.Item, (_EB1E)key.Item.Parent.GetOwner());
			if (obj.Failed)
			{
				return obj.Error;
			}
		}
		return new _EBFF(key, obj.Value, succeed: true);
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private IEnumerator _E002(EDoorState state, bool isLocalInteraction, float speed)
	{
		return base.SmoothDoorOpenCoroutine(state, isLocalInteraction, speed);
	}

	[CompilerGenerated]
	private bool _E003(InteractiveProxy x)
	{
		return x.Grips.Contains(_E02C);
	}

	[CompilerGenerated]
	private bool _E004(InteractiveProxy p)
	{
		return p.Grips.Contains(_E02C);
	}
}
