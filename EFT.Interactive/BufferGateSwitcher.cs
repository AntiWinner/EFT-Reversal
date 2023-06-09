using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Comfort.Common;
using EFT.BufferZone;
using UnityEngine;

namespace EFT.Interactive;

public class BufferGateSwitcher : Switch
{
	[SerializeField]
	private bool _isOpeningEntranceGates;

	[SerializeField]
	private AudioClip _successfulInteractionSound;

	[SerializeField]
	private AudioClip _rejectedInteractionSound;

	private BufferGates _E016;

	private BufferInnerZone _E017;

	private Player _E018;

	private bool _E019;

	[CompilerGenerated]
	private Action<EInteractionType, bool, Player> _E01A;

	public bool Interactable => _E017.IsZoneAvailableForInteractions;

	public EDoorState BufferGatesState => _E016.DoorState;

	public event Action<EInteractionType, bool, Player> OnIntercatWithSwitch
	{
		[CompilerGenerated]
		add
		{
			Action<EInteractionType, bool, Player> action = _E01A;
			Action<EInteractionType, bool, Player> action2;
			do
			{
				action2 = action;
				Action<EInteractionType, bool, Player> value2 = (Action<EInteractionType, bool, Player>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E01A, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<EInteractionType, bool, Player> action = _E01A;
			Action<EInteractionType, bool, Player> action2;
			do
			{
				action2 = action;
				Action<EInteractionType, bool, Player> value2 = (Action<EInteractionType, bool, Player>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E01A, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public void SetUpReferences(BufferZoneContainer bufferZoneContainer)
	{
		_E016 = bufferZoneContainer.Gates;
		_E017 = bufferZoneContainer.InnerZone;
	}

	public void SetNextInteractionWithoutSound()
	{
		_E019 = true;
	}

	protected override bool CanStartInteraction(EDoorState state, bool logFalse = false)
	{
		return DoorState != state;
	}

	protected override IEnumerator SmoothDoorOpenCoroutine(EDoorState state, bool isLocalInteraction, float speed = 1f)
	{
		yield return base.SmoothDoorOpenCoroutine(state, isLocalInteraction, speed);
		if (Delay > 0f)
		{
			yield return new WaitForSeconds(Delay);
		}
		if (_isOpeningEntranceGates && !Interactable)
		{
			if (base.InteractingPlayer != null && base.InteractingPlayer.IsYourPlayer)
			{
				_EBEB.Instance.ShowGateNotAvailableNotification();
			}
			_E000(isSuccessful: false);
			yield break;
		}
		EInteractionType eInteractionType = EInteractionType.Open;
		if (_E016.DoorState == EDoorState.Interacting)
		{
			if (!_E016._E000)
			{
				_E000(isSuccessful: false);
				yield break;
			}
			if (_E018 != null && base.InteractingPlayer != _E018)
			{
				_E000(isSuccessful: false);
				yield break;
			}
			eInteractionType = EInteractionType.Close;
		}
		if (_isOpeningEntranceGates && isLocalInteraction)
		{
			if (base.InteractingPlayer != null && !_EBEB.Instance.IsPlayerCanAccessBufferZone(base.InteractingPlayer.ProfileId, out var receivedAccessStatus))
			{
				if (base.InteractingPlayer.IsYourPlayer)
				{
					switch (receivedAccessStatus)
					{
					case BufferAccessStatusType.UnavailableByTime:
						_EBEB.Instance.ShowUsageTimeEndedNotification();
						break;
					case BufferAccessStatusType.UnavailableByAccess:
						_EBEB.Instance.ShowAccessDeniedNotification();
						break;
					}
				}
				_E000(isSuccessful: false);
				yield break;
			}
			if (_E017.HasCustomer && (base.InteractingPlayer == null || !_E017.IsPlayerInZone(base.InteractingPlayer)))
			{
				if (base.InteractingPlayer.IsYourPlayer && eInteractionType == EInteractionType.Open)
				{
					_EBEB.Instance.ShowAlreadyHaveCustomersNotification();
				}
				_E000(isSuccessful: false);
				yield break;
			}
		}
		if (_E016.DoorState == EDoorState.Locked)
		{
			_E000(isSuccessful: false);
			yield break;
		}
		if (eInteractionType == EInteractionType.Open && isLocalInteraction)
		{
			_E017.AllowCustomerPassFilter(base.InteractingPlayer);
		}
		_E01A?.Invoke(eInteractionType, _isOpeningEntranceGates, base.InteractingPlayer);
		if (eInteractionType == EInteractionType.Open)
		{
			_E018 = (isLocalInteraction ? base.InteractingPlayer : null);
		}
		_E000(isSuccessful: true);
		if (base.HasAuthority)
		{
			_E016.LockForInteraction();
			_E016.Interact(new _EBFE(eInteractionType));
		}
	}

	private void _E000(bool isSuccessful)
	{
		if (_E019)
		{
			_E019 = false;
		}
		else
		{
			Singleton<BetterAudio>.Instance.PlayAtPoint(base.transform.position, isSuccessful ? _successfulInteractionSound : _rejectedInteractionSound, _E8A8.Instance.Distance(base.transform.position), BetterAudio.AudioSourceGroupType.Collisions, 35, UnityEngine.Random.Range(0.8f, 1f), EOcclusionTest.Regular);
		}
	}

	[DebuggerHidden]
	[CompilerGenerated]
	private IEnumerator _E001(EDoorState state, bool isLocalInteraction, float speed)
	{
		return base.SmoothDoorOpenCoroutine(state, isLocalInteraction, speed);
	}
}
