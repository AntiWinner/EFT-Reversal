using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Comfort.Common;
using CommonAssets.Scripts.Game;
using EFT.Interactive;
using UnityEngine;
using UnityEngine.Networking;

namespace EFT.BufferZone;

public class BufferInnerZone : MonoBehaviour
{
	[SerializeField]
	private LampController availabilityShowingLamp;

	[SerializeField]
	private Collider _customerFilterCollider;

	[SerializeField]
	private PhysicsTriggerHandler enterZoneTriggerHandler;

	[SerializeField]
	private PhysicsTriggerHandler exitZoneTriggerHandler;

	[SerializeField]
	private List<PhysicsTriggerHandler> nearBufferZoneEntranceTriggerHandlers = new List<PhysicsTriggerHandler>();

	private const int m__E000 = 60;

	private const int m__E001 = 180;

	private const int m__E002 = 105;

	private readonly _ECEF<Player> m__E003 = new _ECEF<Player>();

	private readonly _ECEF<Player> m__E004 = new _ECEF<Player>();

	private Dictionary<string, float> m__E005 = new Dictionary<string, float>();

	private float m__E006;

	private bool m__E007;

	private bool m__E008;

	private bool m__E009;

	private bool m__E00A;

	private BufferOuterBattleZone m__E00B;

	private BufferGateSwitcher m__E00C;

	private BufferGateSwitcher _E00D;

	private BufferGates _E00E;

	private CancellationTokenSource _E00F = new CancellationTokenSource();

	private Dictionary<string, bool> _E010 = new Dictionary<string, bool>();

	private Dictionary<string, bool> _E011 = new Dictionary<string, bool>();

	[CompilerGenerated]
	private bool _E012 = true;

	[CompilerGenerated]
	private Action<string, bool> _E013;

	public _E5CB._E031 Settings => _EBEB.Instance.Settings;

	public bool HasCustomer => this.m__E003.Count > 0;

	public float CurrentPlayerRemainingUsageTime => Settings.CustomerAccessTime - (_E62A.PastTime - this.m__E006);

	public bool IsZoneAvailableForInteractions
	{
		[CompilerGenerated]
		get
		{
			return _E012;
		}
		[CompilerGenerated]
		private set
		{
			_E012 = value;
		}
	}

	public event Action<string, bool> OnPlayerInZoneStatusChanged
	{
		[CompilerGenerated]
		add
		{
			Action<string, bool> action = _E013;
			Action<string, bool> action2;
			do
			{
				action2 = action;
				Action<string, bool> value2 = (Action<string, bool>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E013, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<string, bool> action = _E013;
			Action<string, bool> action2;
			do
			{
				action2 = action;
				Action<string, bool> value2 = (Action<string, bool>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E013, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	private void Awake()
	{
		availabilityShowingLamp.Switch(Turnable.EState.SmoothOff);
		enterZoneTriggerHandler.OnTriggerEnter += _E007;
		exitZoneTriggerHandler.OnTriggerEnter += _E008;
		exitZoneTriggerHandler.OnTriggerExit += _E008;
		foreach (PhysicsTriggerHandler nearBufferZoneEntranceTriggerHandler in nearBufferZoneEntranceTriggerHandlers)
		{
			nearBufferZoneEntranceTriggerHandler.OnTriggerEnter += _E004;
		}
	}

	private void Update()
	{
		if (!HasCustomer)
		{
			return;
		}
		Player player = this.m__E003[0];
		if (!player.IsYourPlayer)
		{
			return;
		}
		float currentPlayerRemainingUsageTime = CurrentPlayerRemainingUsageTime;
		_E005(player.ProfileId, currentPlayerRemainingUsageTime);
		if (currentPlayerRemainingUsageTime <= 0f)
		{
			EndByExitTrigerScenario._E001 obj = Singleton<AbstractGame>.Instance as EndByExitTrigerScenario._E001;
			_E00B(player);
			if (Singleton<AbstractGame>.Instance is LocalGame)
			{
				obj.StopSession(player.ProfileId, ExitStatus.MissingInAction, string.Empty);
			}
			this.m__E003.Clear();
			return;
		}
		if (!this.m__E007)
		{
			this.m__E007 = true;
			if (currentPlayerRemainingUsageTime < Settings.CustomerKickNotifTime)
			{
				_E000();
			}
			else
			{
				_EBEB.Instance.ShowTimerRemindNotification();
			}
		}
		if (!this.m__E008 && currentPlayerRemainingUsageTime < Settings.CustomerKickNotifTime)
		{
			_E000();
		}
		if (!this.m__E009 && !this.m__E008 && currentPlayerRemainingUsageTime <= 180f)
		{
			if (currentPlayerRemainingUsageTime > 105f)
			{
				_EBAF.Instance.CreateCommonEvent<_EBB2>().Invoke(_EBB2.EZoneState.TimeEndingWarning);
			}
			else
			{
				this.m__E00A = true;
			}
			this.m__E009 = true;
		}
		if (!this.m__E00A && currentPlayerRemainingUsageTime <= 105f)
		{
			_EBAF.Instance.CreateCommonEvent<_EBB2>().Invoke(_EBB2.EZoneState.CantShowTimeEndingWarning);
			this.m__E00A = true;
		}
	}

	private void OnDestroy()
	{
		enterZoneTriggerHandler.OnTriggerEnter -= _E007;
		exitZoneTriggerHandler.OnTriggerEnter -= _E008;
		exitZoneTriggerHandler.OnTriggerExit -= _E008;
		_E013 = null;
		foreach (PhysicsTriggerHandler nearBufferZoneEntranceTriggerHandler in nearBufferZoneEntranceTriggerHandlers)
		{
			nearBufferZoneEntranceTriggerHandler.OnTriggerEnter -= _E004;
		}
		_E00F.Cancel();
	}

	public void SetUpReferences(BufferZoneContainer bufferZoneContainer)
	{
		this.m__E00B = bufferZoneContainer.BattleOuterZone;
		this.m__E00C = bufferZoneContainer.OuterSwitcher;
		_E00D = bufferZoneContainer.InnerSwitcher;
		_E00E = bufferZoneContainer.Gates;
		_E00D.OnIntercatWithSwitch += _E003;
		this.m__E00C.OnIntercatWithSwitch += _E003;
		_E00E.OnDoorStateChanged += _E002;
	}

	private void _E000()
	{
		this.m__E008 = true;
		_EBEB.Instance.ShowGateKickAlarmNotification();
		_EBAF.Instance.CreateCommonEvent<_EBB2>().Invoke(_EBB2.EZoneState.KickSoon);
	}

	public bool IsPlayerHaveAccess(string profileID)
	{
		if (_E010.TryGetValue(profileID, out var value))
		{
			return value;
		}
		return false;
	}

	public void ChangePlayerAccessStatus(string profileID, bool status)
	{
		_E010[profileID] = status;
		if (!status)
		{
			float userRemainingUsageTime = GetUserRemainingUsageTime(profileID);
			userRemainingUsageTime = Mathf.Clamp(userRemainingUsageTime, 0f, 60f);
			_E005(profileID, userRemainingUsageTime);
			if (HasCustomer && string.Equals(this.m__E003[0].ProfileId, profileID))
			{
				_E006();
			}
		}
	}

	private bool _E001(Player player)
	{
		if (!player.HandsIsEmpty)
		{
			return player.HandsController is Player.MedsController;
		}
		return true;
	}

	private void _E002(WorldInteractiveObject gates, EDoorState stateBefore, EDoorState stateAfter)
	{
		if (stateAfter != EDoorState.Shut)
		{
			return;
		}
		UnallowAllCustomersToPassFilter();
		if (!HasCustomer)
		{
			return;
		}
		foreach (Player item in this.m__E003)
		{
			AllowCustomerPassFilter(item);
			item.CanManipulateWithHandsInBufferZone = false;
			if (item.IsYourPlayer)
			{
				_E00A(item);
			}
		}
	}

	private void _E003(EInteractionType interactionType, bool isOuterGateSwitcher, Player interactingPlayer)
	{
		if (!(interactingPlayer == null) && interactionType == EInteractionType.Open && !isOuterGateSwitcher)
		{
			interactingPlayer.CanManipulateWithHandsInBufferZone = true;
			_E009(interactingPlayer).HandleExceptions();
		}
	}

	private void _E004(Collider col)
	{
		Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(col);
		if (!(playerByCollider == null) && playerByCollider.IsInBufferZone)
		{
			playerByCollider.CanManipulateWithHandsInBufferZone = false;
			if (playerByCollider.IsYourPlayer)
			{
				_E00A(playerByCollider);
			}
		}
	}

	public bool IsPlayerInZone(Player player)
	{
		return this.m__E003.Contains(player);
	}

	public bool IsPlayerDyingInZone(Player player)
	{
		if (!this.m__E004.Contains(player))
		{
			return this.m__E003.Contains(player);
		}
		return true;
	}

	public void AllowCustomerPassFilter(Player player, bool allow = true)
	{
		if (!(player == null))
		{
			_E320.IgnoreCollision(player.CharacterControllerCommon.GetCollider(), _customerFilterCollider, allow);
			Physics.IgnoreCollision(player.POM.Collider, _customerFilterCollider, allow);
			player.POM.IgnoreCollider(_customerFilterCollider, allow);
		}
	}

	public void UnallowAllCustomersToPassFilter()
	{
		GameWorld instance = Singleton<GameWorld>.Instance;
		if (instance == null)
		{
			return;
		}
		foreach (Player allPlayer in instance.AllPlayers)
		{
			if (!(allPlayer == null) && allPlayer.HealthController != null && allPlayer.HealthController.IsAlive)
			{
				AllowCustomerPassFilter(allPlayer, allow: false);
			}
		}
	}

	public void ChangeZoneInteractionAvailability(bool isAvailable, EBufferZoneData changesDataType)
	{
		bool isZoneAvailableForInteractions = IsZoneAvailableForInteractions;
		IsZoneAvailableForInteractions = isAvailable;
		if (isZoneAvailableForInteractions && !isAvailable && HasCustomer)
		{
			_E006();
			if (this.m__E003[0].IsYourPlayer)
			{
				switch (changesDataType)
				{
				case EBufferZoneData.DisableByPlayerDead:
					_EBAF.Instance.CreateCommonEvent<_EBB2>().Invoke(_EBB2.EZoneState.DisabledAfterPlayerKilled);
					break;
				case EBufferZoneData.DisableByZryachiyDead:
					_EBAF.Instance.CreateCommonEvent<_EBB2>().Invoke(_EBB2.EZoneState.DisabledAfterZryachiyKilled);
					break;
				}
			}
		}
		if (!isAvailable)
		{
			availabilityShowingLamp.Switch(Turnable.EState.On);
		}
	}

	public float GetUserRemainingUsageTime(string playerProfileID)
	{
		if (!this.m__E005.ContainsKey(playerProfileID))
		{
			_E005(playerProfileID, Settings.CustomerAccessTime);
		}
		return this.m__E005[playerProfileID];
	}

	private void _E005(string playerProfileID, float time)
	{
		this.m__E005[playerProfileID] = time;
	}

	private void _E006()
	{
		if (!(CurrentPlayerRemainingUsageTime <= 60f))
		{
			this.m__E006 = 60f - Settings.CustomerAccessTime + _E62A.PastTime;
			float currentPlayerRemainingUsageTime = CurrentPlayerRemainingUsageTime;
			_E005(this.m__E003[0].ProfileId, currentPlayerRemainingUsageTime);
		}
	}

	public void Serialize(NetworkWriter writer)
	{
		writer.Write(IsZoneAvailableForInteractions);
	}

	public async Task HandlePlayerReconnected(bool isInBufferZone, Player player, int bufferZoneUsageTimeLeft, Task onCurrentHandsControllerSpawned)
	{
		_E005(player.ProfileId, bufferZoneUsageTimeLeft);
		if (isInBufferZone)
		{
			player.IsInBufferZone = true;
			player.CanManipulateWithHandsInBufferZone = false;
			while (Singleton<AbstractGame>.Instance.Status != GameStatus.Started)
			{
				await Task.Yield();
			}
			availabilityShowingLamp.Switch(Turnable.EState.On);
			if (!this.m__E003.Contains(player))
			{
				this.m__E003.Add(player);
			}
			_EBAF.Instance.CreateCommonEvent<_EBC1>().Invoke(player, entered: true);
			if (player.IsYourPlayer)
			{
				await onCurrentHandsControllerSpawned;
				_E00A(player);
				this.m__E007 = false;
				_EBAF.Instance.CreateCommonEvent<_EBBF>().Invoke(insideZone: true, afterReconnect: true);
			}
			this.m__E006 = _E62A.PastTime - (Settings.CustomerAccessTime - GetUserRemainingUsageTime(player.ProfileId));
			AllowCustomerPassFilter(player);
			player.OnPlayerDeadOrUnspawn += _E00B;
			_E013?.Invoke(player.ProfileId, arg2: true);
		}
	}

	private void _E007(Collider col)
	{
		Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(col);
		if (!(playerByCollider == null) && !this.m__E003.Contains(playerByCollider))
		{
			this.m__E003.Add(playerByCollider);
			_E013?.Invoke(playerByCollider.ProfileId, arg2: true);
			playerByCollider.IsInBufferZone = true;
			playerByCollider.CanManipulateWithHandsInBufferZone = false;
			_EBAF.Instance.CreateCommonEvent<_EBC1>().Invoke(playerByCollider, entered: true);
			if (playerByCollider.IsYourPlayer)
			{
				_E00C(_E00F.Token).HandleExceptions();
				_EBAF.Instance.CreateCommonEvent<_EBBF>().Invoke(insideZone: true);
				_E00A(playerByCollider);
			}
			this.m__E006 = _E62A.PastTime - (Settings.CustomerAccessTime - GetUserRemainingUsageTime(playerByCollider.ProfileId));
			if ((!_EBEB.Instance.IsPlayerHaveAccessToInnerZone(playerByCollider.ProfileId) || !IsZoneAvailableForInteractions) && CurrentPlayerRemainingUsageTime > 60f)
			{
				this.m__E006 = 60f - Settings.CustomerAccessTime + _E62A.PastTime;
				float currentPlayerRemainingUsageTime = CurrentPlayerRemainingUsageTime;
				_E005(playerByCollider.ProfileId, currentPlayerRemainingUsageTime);
			}
			playerByCollider.OnPlayerDeadOrUnspawn += _E00B;
			availabilityShowingLamp.Switch(Turnable.EState.On);
		}
	}

	private void _E008(Collider col)
	{
		if (!base.enabled)
		{
			return;
		}
		Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(col);
		if (!(playerByCollider == null) && playerByCollider.IsInBufferZone)
		{
			_E00B(playerByCollider);
			if (playerByCollider.IsYourPlayer)
			{
				this.m__E007 = false;
				_E00C(_E00F.Token).HandleExceptions();
				_E009(playerByCollider).HandleExceptions();
				_EBAF.Instance.CreateCommonEvent<_EBBF>().Invoke(insideZone: false);
			}
		}
	}

	private async Task _E009(Player player)
	{
		if (!player.IsYourPlayer || !(_E011.TryGetValue(player.ProfileId, out var value) && value))
		{
			return;
		}
		_E011[player.ProfileId] = false;
		if (!_E001(player))
		{
			while (player.ProcessStatus != 0)
			{
				await Task.Yield();
			}
		}
		if (!_E011[player.ProfileId])
		{
			player.TrySetLastEquippedWeapon();
		}
	}

	private void _E00A(Player player)
	{
		if (player.IsYourPlayer)
		{
			_E011[player.ProfileId] = true;
			if (!_E001(player))
			{
				player.SetEmptyHands(delegate
				{
				});
			}
		}
	}

	public void HandlePlayerLeftZoneFromServer(string profileID)
	{
		if (!HasCustomer)
		{
			return;
		}
		Player player = null;
		foreach (Player item in this.m__E003)
		{
			if (string.Equals(item.ProfileId, profileID))
			{
				player = item;
				break;
			}
		}
		if (!(player == null))
		{
			_E00B(player);
		}
	}

	private void _E00B(Player player)
	{
		if (this.m__E003.Remove(player))
		{
			_E005(player.ProfileId, CurrentPlayerRemainingUsageTime);
		}
		player.IsInBufferZone = false;
		_EBAF.Instance.CreateCommonEvent<_EBC1>().Invoke(player, entered: false);
		if (!player.HealthController.IsAlive)
		{
			this.m__E004.Add(player);
		}
		if (IsZoneAvailableForInteractions)
		{
			availabilityShowingLamp.Switch(Turnable.EState.SmoothOff);
		}
		player.OnPlayerDeadOrUnspawn -= _E00B;
		_E013?.Invoke(player.ProfileId, arg2: false);
	}

	private async Task _E00C(CancellationToken cancellationToken)
	{
		if (_E00E.DoorState == EDoorState.Interacting)
		{
			while (!_E00E._E000)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return;
				}
				await Task.Yield();
			}
		}
		if (!cancellationToken.IsCancellationRequested && _E00E.DoorState != EDoorState.Shut)
		{
			_E00D.SetNextInteractionWithoutSound();
			if (_E00D.DoorState == EDoorState.Shut)
			{
				_E00D.Interact(new _EBFE(EInteractionType.Open));
			}
			else
			{
				_E00D.Interact(new _EBFE(EInteractionType.Close));
			}
		}
	}
}
