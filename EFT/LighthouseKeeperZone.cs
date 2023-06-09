using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Cutscene;
using EFT.Counters;
using EFT.Interactive;
using EFT.UI;
using EFT.UI.Screens;
using UnityEngine;
using UnityEngine.Rendering;

namespace EFT;

public class LighthouseKeeperZone : MonoBehaviour
{
	[SerializeField]
	private BaseCutsceneTrigger _enterZoneTrigger;

	[SerializeField]
	private BaseCutsceneTrigger _exitZoneTrigger;

	[SerializeField]
	private BufferGates _bufferGates;

	[SerializeField]
	private Door _keeperEntranceDoor;

	[SerializeField]
	private SmoothLookAt _lookAt;

	[SerializeField]
	private Transform _transformForFollowPlayerCam;

	[SerializeField]
	private float _fovInDialog;

	[SerializeField]
	private PlayerCameraFovChanger _playerCameraCamFovChanger;

	[SerializeField]
	private List<Renderer> _rendersOnlyForCutscene = new List<Renderer>();

	private Action m__E000;

	private bool m__E001;

	private bool m__E002;

	private bool m__E003 = true;

	private bool m__E004;

	private bool m__E005;

	private TraderDialogScreen._E000 m__E006;

	private _E8B4 m__E007;

	private bool m__E008;

	private bool m__E009;

	private string m__E00A;

	private readonly string m__E00B = _ED3E._E000(182589);

	private TraderDialogScreen._E000 _E000(string firstPhrase)
	{
		Player myPlayer = GamePlayerOwner.MyPlayer;
		Profile._E001 keeperTrader = myPlayer.Profile.TradersInfo[this.m__E00B];
		this.m__E007 = new _E8B4(keeperTrader, myPlayer._E0DC, firstPhrase);
		this.m__E006 = new TraderDialogScreen._E000(myPlayer.Profile, this.m__E00B, myPlayer._E0DC, this.m__E007);
		return this.m__E006;
	}

	private void Awake()
	{
		this.m__E004 = true;
		_enterZoneTrigger.OnPlayerCausesCutscene += _E006;
		_enterZoneTrigger.OnCutsceneEnded += _E007;
		_exitZoneTrigger.OnPlayerCausesCutscene += _E008;
		_exitZoneTrigger.OnCutsceneEnded += _E009;
		_bufferGates.OnDoorStateChanged += _E00D;
		_lookAt.SetTransformToLookAt(_transformForFollowPlayerCam);
		_keeperEntranceDoor.OnDoorStateChanged += _E00B;
		this.m__E000 = _EBAF.Instance.SubscribeOnEvent(delegate(_EBB2 invokedEvent)
		{
			_E005(invokedEvent);
		});
		this.m__E000 = (Action)Delegate.Combine(this.m__E000, _EBAF.Instance.SubscribeOnEvent(delegate(_EBBF invokedEvent)
		{
			_E004(invokedEvent);
		}));
		foreach (Renderer item in _rendersOnlyForCutscene)
		{
			item.enabled = true;
			item.forceRenderingOff = true;
		}
	}

	private void Start()
	{
		_keeperEntranceDoor.Operatable = false;
	}

	private void OnDestroy()
	{
		_EBAF.Instance.CreateCommonEvent<_EBC4>().Invoke(show: false);
		this.m__E000?.Invoke();
		this.m__E000 = null;
		if (this.m__E007 != null)
		{
			this.m__E007.Dispose();
		}
	}

	private void _E001(string greetingsPhrase)
	{
		if (0 == 0)
		{
			this.m__E00A = greetingsPhrase;
			this.m__E009 = true;
			_E00C();
		}
	}

	private void _E002()
	{
		if (0 == 0)
		{
			if (this.m__E007 != null)
			{
				this.m__E007.Dispose();
			}
			this.m__E006.CloseScreen();
		}
	}

	private void _E003()
	{
		if (0 == 0)
		{
			this.m__E006.HideDialog();
		}
	}

	private void _E004(_EBBF invokedEvent)
	{
		if (invokedEvent.InsideZone && invokedEvent.AfterReconnect && !this.m__E002)
		{
			_keeperEntranceDoor.Operatable = true;
		}
	}

	private void _E005(_EBB2 zoneEvent)
	{
		if (this.m__E002)
		{
			return;
		}
		switch (zoneEvent.ZoneState)
		{
		case _EBB2.EZoneState.CantShowTimeEndingWarning:
			this.m__E005 = false;
			break;
		case _EBB2.EZoneState.TimeEndingWarning:
			if (!this.m__E001)
			{
				this.m__E005 = true;
			}
			else
			{
				_EBAF.Instance.CreateCommonEvent<_EBBD>().Invoke(_EBBD.EDialogState.TimeEndingWarning);
			}
			break;
		case _EBB2.EZoneState.KickSoon:
		case _EBB2.EZoneState.DisabledAfterZryachiyKilled:
		case _EBB2.EZoneState.DisabledAfterPlayerKilled:
			if (0 == 0)
			{
				_keeperEntranceDoor.Operatable = false;
				this.m__E002 = true;
			}
			if (this.m__E001)
			{
				_E003();
				_EBBD obj = _E84F.Instance.CreateEventWithActionOnEndFeedback<_EBBD>(delegate
				{
					_EBAF.Instance.CreateCommonEvent<_EBBD>().Invoke(_EBBD.EDialogState.DialogClosed);
				});
				switch (zoneEvent.ZoneState)
				{
				case _EBB2.EZoneState.KickSoon:
					obj.Invoke(_EBBD.EDialogState.TimeEndedFarewell);
					break;
				case _EBB2.EZoneState.DisabledAfterPlayerKilled:
					obj.Invoke(_EBBD.EDialogState.PlayerWithAccessKilled);
					break;
				case _EBB2.EZoneState.DisabledAfterZryachiyKilled:
					obj.Invoke(_EBBD.EDialogState.ZryachiyKilled);
					break;
				}
			}
			break;
		}
	}

	private void _E006(BaseCutsceneTrigger trigger, Player myPlayer)
	{
		foreach (Renderer item in _rendersOnlyForCutscene)
		{
			item.forceRenderingOff = false;
		}
		this.m__E001 = true;
		if (this.m__E004)
		{
			this.m__E003 = myPlayer.Profile.Stats.OverallCounters.GetInt(CounterTag.LightkeeperVisited) == 0;
		}
		this.m__E004 = false;
		_E7AE.Instance.PauseEffects();
		_EBAF.Instance.CreateCommonEvent<_EBBC>().Invoke(myPlayer, _EBBC.EInteractState.IsEntering);
		if (0 == 0)
		{
			_E000(null).ShowScreen(EScreenState.Queued);
		}
		if (this.m__E003)
		{
			_EBAF.Instance.CreateCommonEvent<_EBBD>().Invoke(_EBBD.EDialogState.FirstTimeGreetingsIdle);
			return;
		}
		_E84F.Instance.CreateEventWithActionOnEndFeedback<_EBBD>(delegate(string s)
		{
			if (!this.m__E002)
			{
				_E001(s);
				_EBAF.Instance.CreateCommonEvent<_EBC4>().Invoke(show: true);
				if (this.m__E005)
				{
					_EBAF.Instance.CreateCommonEvent<_EBBD>().Invoke(_EBBD.EDialogState.TimeEndingWarning);
					this.m__E005 = false;
				}
			}
		}).Invoke(_EBBD.EDialogState.CommonGreetigs);
	}

	private void _E007(Player myPlayer)
	{
		_EBAF.Instance.CreateCommonEvent<_EBBC>().Invoke(myPlayer, _EBBC.EInteractState.Entered);
		_keeperEntranceDoor.ForceCloseLocalDoor(playSound: true);
		_lookAt.SetTransformForRotation(myPlayer.CameraContainer.transform);
		_E00A(myPlayer).HandleExceptions();
		if (myPlayer.PlayerBody.BodySkins.TryGetValue(EBodyModelPart.Hands, out var value))
		{
			value.SetShadowCastingMode(ShadowCastingMode.ShadowsOnly);
		}
		this.m__E008 = true;
		_E00C();
		if (this.m__E003)
		{
			this.m__E003 = false;
			if (this.m__E005)
			{
				_EBAF.Instance.CreateCommonEvent<_EBBD>().Invoke(_EBBD.EDialogState.TimeEndingWarning);
				this.m__E005 = false;
			}
		}
	}

	private void _E008(BaseCutsceneTrigger trigger, Player myPlayer)
	{
		_EBAF.Instance.CreateCommonEvent<_EBBC>().Invoke(myPlayer, _EBBC.EInteractState.IsExiting);
		_playerCameraCamFovChanger.ReturnFov(1f);
		_EBAF.Instance.CreateCommonEvent<_EBC4>().Invoke(show: false);
		_keeperEntranceDoor.Interact(new _EBFE(EInteractionType.Open));
		_lookAt.ToggleWorking(enable: false);
		_E002();
	}

	private void _E009(Player myPlayer)
	{
		_EBAF.Instance.CreateCommonEvent<_EBBC>().Invoke(myPlayer, _EBBC.EInteractState.Exited);
		_keeperEntranceDoor.ForceCloseLocalDoor(playSound: true);
		_E7AE.Instance.UnpauseEffects();
		if (myPlayer.PlayerBody.BodySkins.TryGetValue(EBodyModelPart.Hands, out var value))
		{
			value.SetShadowCastingMode(ShadowCastingMode.On);
		}
		this.m__E001 = false;
		foreach (Renderer item in _rendersOnlyForCutscene)
		{
			item.forceRenderingOff = true;
		}
		this.m__E008 = false;
		this.m__E009 = false;
	}

	private async Task _E00A(Player myPlayer)
	{
		await Task.Delay(100);
		myPlayer.MovementContext.ToggleBlockInputPlayerRotation(block: true);
		await Task.Delay(200);
		_lookAt.ToggleWorking(enable: true);
		_playerCameraCamFovChanger.ChangeFov(_fovInDialog, _lookAt.GetTimeToLookAt());
	}

	private void _E00B(WorldInteractiveObject obj, EDoorState prevState, EDoorState nextState)
	{
		if (nextState != EDoorState.Open || !this.m__E003)
		{
			return;
		}
		_E84F.Instance.CreateEventWithActionOnEndFeedback<_EBBD>(delegate(string s)
		{
			if (!this.m__E002)
			{
				_E001(s);
				_EBAF.Instance.CreateCommonEvent<_EBC4>().Invoke(show: true);
			}
		}).Invoke(_EBBD.EDialogState.FirstTimeGreetings);
	}

	private void _E00C()
	{
		if (this.m__E009 && this.m__E008)
		{
			this.m__E007.OnGreetingsComplete(this.m__E00A);
		}
	}

	private void _E00D(WorldInteractiveObject obj, EDoorState prevState, EDoorState nextState)
	{
		switch (nextState)
		{
		case EDoorState.Open:
		case EDoorState.Interacting:
			_keeperEntranceDoor.Operatable = false;
			break;
		case EDoorState.Shut:
			if (!this.m__E002)
			{
				_keeperEntranceDoor.Operatable = true;
			}
			break;
		}
	}

	[CompilerGenerated]
	private void _E00E(_EBB2 invokedEvent)
	{
		_E005(invokedEvent);
	}

	[CompilerGenerated]
	private void _E00F(_EBBF invokedEvent)
	{
		_E004(invokedEvent);
	}

	[CompilerGenerated]
	private void _E010(string s)
	{
		if (!this.m__E002)
		{
			_E001(s);
			_EBAF.Instance.CreateCommonEvent<_EBC4>().Invoke(show: true);
			if (this.m__E005)
			{
				_EBAF.Instance.CreateCommonEvent<_EBBD>().Invoke(_EBBD.EDialogState.TimeEndingWarning);
				this.m__E005 = false;
			}
		}
	}

	[CompilerGenerated]
	private void _E011(string s)
	{
		if (!this.m__E002)
		{
			_E001(s);
			_EBAF.Instance.CreateCommonEvent<_EBC4>().Invoke(show: true);
		}
	}
}
