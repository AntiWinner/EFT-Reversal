using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Comfort.Common;
using Dissonance;
using Dissonance.Networking;
using Dissonance.Networking.Client;
using Diz.Jobs;
using EFT.AssetsManager;
using EFT.BufferZone;
using EFT.CameraControl;
using EFT.Communications;
using EFT.Interactive;
using EFT.UI;
using EFT.UI.Matchmaker;
using EFT.UI.Screens;
using EFT.Weather;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.Networking;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

namespace EFT;

internal sealed class NetworkGame : AbstractGame, _E7A6, _E62D, IClientHearingTable
{
	private class _E000
	{
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public _E60E customizationSolver;

		public NetworkGame _003C_003E4__this;

		internal _EBDF _E000(string x)
		{
			return customizationSolver.GetItem(x);
		}

		internal ResourceKey _E001(_EBDF x)
		{
			return customizationSolver.GetWatchBundle(x.Id).WatchPrefab;
		}

		internal void _E002(_E5BB p)
		{
			_003C_003E4__this.SetMatchmakerStatus(_ED3E._E000(182222) + p.Stage, p.Progress);
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public byte channelId;

		public NetworkGame _003C_003E4__this;

		public Callback<int> callback;

		public int id;

		internal void _E000(byte channelIndex, in ArraySegment<byte> segment, PacketPriority priority)
		{
			byte channelIndex2 = channelIndex;
			if (channelIndex < _E6A8._E006)
			{
				UnityEngine.Debug.LogError(string.Format(_ED3E._E000(181083), channelIndex, channelId));
				channelIndex2 = channelId;
			}
			_003C_003E4__this._E02B._E005(channelIndex2, in segment, priority);
		}

		internal void _E001(IResult result)
		{
			_003C_003E4__this._E028.Profile.Stats.TotalInGameTime = _003C_003E4__this._E02F.TotalInGameTime;
			callback(new Result<int>(id, result.Error));
			_003C_003E4__this._E017(_003C_003E4__this._E028);
		}
	}

	[CompilerGenerated]
	private sealed class _E004
	{
		public NetworkGame _003C_003E4__this;

		public Callback<int> callback;

		public int id;

		internal void _E000(IResult result)
		{
			_003C_003E4__this._E03C = 0f;
			callback(new Result<int>(id, result.Error));
		}
	}

	[CompilerGenerated]
	private sealed class _E006
	{
		public byte channelId;

		internal bool _E000(NetworkPlayer value)
		{
			if (value.ChannelIndex != channelId)
			{
				return value.ChannelIndex == channelId - 1;
			}
			return true;
		}
	}

	[CompilerGenerated]
	private sealed class _E007
	{
		public ExfiltrationPoint point;

		public NetworkGame _003C_003E4__this;

		internal void _E000()
		{
			point.OnStatusChanged -= _003C_003E4__this._E004;
		}
	}

	[CompilerGenerated]
	private sealed class _E008
	{
		public ReconnectionScreen._E000 reconnectionScreenController;

		public NetworkGame _003C_003E4__this;

		internal void _E000()
		{
			reconnectionScreenController.OnLeave -= _003C_003E4__this._E026.StopGame;
		}
	}

	[CompilerGenerated]
	private sealed class _E00C
	{
		public Vector3 minePosition;

		internal bool _E000(MineDirectional mine)
		{
			return Vector3.Distance(mine.transform.position, minePosition) < Mathf.Epsilon;
		}
	}

	[CompilerGenerated]
	private sealed class _E00E
	{
		public bool complete;

		public NetworkGame _003C_003E4__this;

		internal void _E000()
		{
			_003C_003E4__this.m__E007.TimerPanel.Close();
			if (_003C_003E4__this.m__E00E != null)
			{
				_003C_003E4__this.m__E00E._E023();
			}
			_EC92.Instance.CloseAllScreensForced();
			complete = true;
		}
	}

	[CompilerGenerated]
	private sealed class _E010
	{
		public bool complete;

		internal void _E000()
		{
			complete = true;
		}
	}

	private const float m__E01C = 2f;

	private _E725 _E01D;

	private Profile _E01E;

	private Profile _E01F;

	private _E7FD _E020;

	private CommonUI _E021;

	private PreloaderUI _E022;

	private GameUI m__E007;

	private Action _E023;

	private Callback<ExitStatus, TimeSpan, _E907> m__E009;

	private Coroutine _E024;

	private _E554.Location _E025;

	private _E7A7 _E026;

	private bool _E027;

	private ClientPlayer _E028;

	private GamePlayerOwner m__E00E;

	private readonly Dictionary<int, NetworkPlayer> m__E013 = new Dictionary<int, NetworkPlayer>();

	private readonly Dictionary<byte, NetworkPlayer> _E029 = new Dictionary<byte, NetworkPlayer>();

	private readonly Dictionary<byte, _E6C3> _E02A = new Dictionary<byte, _E6C3>();

	private ChannelCombined _E02B;

	private ESideType _E02C;

	private ERaidMode _E02D;

	private _ECB1 _E02E;

	private _E796 _E02F;

	private _E90A _E030;

	private _E909 _E031;

	private _E912 _E032;

	private BufferZoneDataReciever _E033;

	[CanBeNull]
	private _E06B _E034;

	[CompilerGenerated]
	private _E629 m__E004;

	[CompilerGenerated]
	private ulong _E035;

	[CompilerGenerated]
	private double _E036;

	[CompilerGenerated]
	private Action m__E01B;

	private _E5B9 _E037;

	private readonly CancellationTokenSource _E038 = new CancellationTokenSource();

	private Result<ExitStatus, TimeSpan>? _E039;

	private bool _E03A;

	private bool _E03B;

	private float _E03C;

	private _E75D _E03D;

	private float _E03E;

	private RaidSettings _E03F;

	private Profile _E000
	{
		get
		{
			if (_E02C != ESideType.Savage)
			{
				return _E01E;
			}
			return _E01F;
		}
	}

	public _E629 GameDateTime
	{
		[CompilerGenerated]
		get
		{
			return this.m__E004;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E004 = value;
		}
	}

	public ulong LocalIndex
	{
		[CompilerGenerated]
		get
		{
			return _E035;
		}
		[CompilerGenerated]
		set
		{
			_E035 = value;
		}
	}

	public double LocalTime
	{
		[CompilerGenerated]
		get
		{
			return _E036;
		}
		[CompilerGenerated]
		private set
		{
			_E036 = value;
		}
	}

	public string LocationId => _E025.Id;

	public override string LocationObjectId => _E025._Id;

	protected override string ProfileId => _E028.ProfileId;

	protected override GameUI GameUi => this.m__E007;

	public bool CoopEnabled => _E02D == ERaidMode.Coop;

	public event Action UpdateByUnity
	{
		[CompilerGenerated]
		add
		{
			Action action = this.m__E01B;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E01B, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = this.m__E01B;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E01B, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	bool IClientHearingTable.IsHeard()
	{
		if (base.Status != GameStatus.Started)
		{
			return false;
		}
		if (_E028 == null)
		{
			return true;
		}
		bool flag = _E4D0.IsTalkDetected();
		_E028.TalkDateTime = (flag ? _E5AD.UtcNow : default(DateTime));
		bool flag2;
		bool flag3;
		if (this.m__E013.Count == 1)
		{
			flag2 = true;
			flag3 = true;
		}
		else
		{
			flag2 = false;
			flag3 = false;
			Vector3 voicePos = _E028.Position;
			foreach (NetworkPlayer value in this.m__E013.Values)
			{
				if ((object)value != _E028)
				{
					(bool, bool) tuple = value.IsHeard(in voicePos, _E03E);
					bool item = tuple.Item1;
					bool item2 = tuple.Item2;
					flag2 = flag2 || item;
					flag3 = flag3 || item2;
					if (flag2 && flag3)
					{
						break;
					}
				}
			}
		}
		_E4D0.Blocked = !flag3;
		return flag2;
	}

	void IClientHearingTable.ReportAbuse()
	{
		_E026.ReportVoipAbuse();
	}

	internal static NetworkGame _E000(_E796 session, _E725 profileStatus, RaidSettings raidSettings, Profile savageProfile, _ECB1 insurance, _E7FD inputTree, CommonUI commonUI, PreloaderUI preloaderUI, GameUI gameUI, _E909 metricsEvents, _E90A metricsCollector, EUpdateQueue updateQueue, TimeSpan sessionTime, Action runCallback, Callback<ExitStatus, TimeSpan, _E907> callback)
	{
		NetworkGame networkGame = AbstractGame.Create<NetworkGame>(updateQueue, sessionTime, _ED3E._E000(182066));
		networkGame._E01E = session.Profile;
		networkGame._E01F = savageProfile;
		networkGame._E01D = profileStatus;
		networkGame._E02F = session;
		networkGame._E02E = insurance;
		networkGame._E02C = raidSettings.Side;
		networkGame._E020 = inputTree;
		networkGame._E021 = commonUI;
		networkGame._E022 = preloaderUI;
		networkGame.m__E007 = gameUI;
		networkGame._E023 = runCallback;
		networkGame.m__E009 = callback;
		networkGame._E025 = raidSettings.SelectedLocation;
		networkGame._E02D = raidSettings.RaidMode;
		networkGame._E031 = metricsEvents;
		networkGame._E030 = metricsCollector;
		networkGame._E032 = new _E912();
		networkGame._E033 = new BufferZoneDataReciever();
		ClientHearingTable.Instance = networkGame;
		TrafficCounters.Reset();
		Singleton<_E90A>.Instance = networkGame._E030;
		WorldInteractiveObject.InteractionShouldBeConfirmed = true;
		return networkGame;
	}

	public void BotUnspawn(BotOwner bot)
	{
	}

	public override void Dispose()
	{
		ClientHearingTable.Instance = null;
		base.Dispose();
	}

	private static void _E001()
	{
	}

	async Task _E7A6.Run(_E7A7 session, bool canRespawn, _E629 gameDateTime, IDictionary<string, int> interactables, ResourceKey[] prefabs, string[] customizations, _E8EB[] weathers, float fixedDeltaTime, bool speedLimitsEnabled, _E717.Config speedLimits, bool encryptionEnabled, bool decryptionEnabled, _E75D voipSettings)
	{
		UnityEngine.Debug.Log(_ED3E._E000(180974));
		base.Status = GameStatus.Running;
		_E026 = session;
		_E027 = canRespawn;
		GameDateTime = gameDateTime;
		base.FixedDeltaTime = fixedDeltaTime;
		_E03A = encryptionEnabled;
		_E03B = decryptionEnabled;
		_E03D = voipSettings;
		byte hearingDistance = voipSettings.PushToTalkSettings.HearingDistance;
		_E03E = hearingDistance * hearingDistance + 9;
		Singleton<_E3B5>.Create(new _E3B5(session, this.m__E013));
		_E60E customizationSolver = Singleton<_E60E>.Instance;
		_EBDF[] source = customizations.Select((string x) => customizationSolver.GetItem(x)).ToArray();
		IEnumerable<ResourceKey> second = (from x in source
			where x != null && x.Prefab != null
			select x.Prefab).Concat(from x in source
			select customizationSolver.GetWatchBundle(x.Id).WatchPrefab into x
			where !x.IsNullOrEmpty()
			select x);
		Dictionary<EFT.SynchronizableObjects.SynchronizableObjectType, ResourceKey>.ValueCollection values = _E5D2.SynchronizableObjectPath.Values;
		prefabs = prefabs.Concat(second).Concat(values).ToArray();
		bool flag = !Singleton<JobScheduler>.Instance.IsForceModeEnabled;
		if (flag)
		{
			Singleton<JobScheduler>.Instance.SetForceMode(enable: true);
		}
		PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
		_E2E1.FindParentPlayerLoopSystem(currentPlayerLoop, typeof(EarlyUpdate.UpdateTextureStreamingManager), out var playerLoopSystem, out var index);
		PlayerLoopSystem[] array = new PlayerLoopSystem[playerLoopSystem.subSystemList.Length];
		if (index != -1)
		{
			Array.Copy(playerLoopSystem.subSystemList, array, playerLoopSystem.subSystemList.Length);
			PlayerLoopSystem playerLoopSystem2 = default(PlayerLoopSystem);
			playerLoopSystem2.updateDelegate = _E001;
			playerLoopSystem2.type = typeof(_E000);
			PlayerLoopSystem playerLoopSystem3 = playerLoopSystem2;
			playerLoopSystem.subSystemList[index] = playerLoopSystem3;
			PlayerLoop.SetPlayerLoop(currentPlayerLoop);
		}
		await Singleton<_E760>.Instance.LoadBundlesAndCreatePools(_E760.PoolsCategory.Raid, _E760.AssemblyType.Online, prefabs, _ECE3.General, new _ECCE<_E5BB>(delegate(_E5BB p)
		{
			SetMatchmakerStatus(_ED3E._E000(182222) + p.Stage, p.Progress);
		}), _E038.Token);
		if (index != -1)
		{
			Array.Copy(array, playerLoopSystem.subSystemList, playerLoopSystem.subSystemList.Length);
			PlayerLoop.SetPlayerLoop(currentPlayerLoop);
		}
		if (flag)
		{
			Singleton<JobScheduler>.Instance.SetForceMode(enable: false);
		}
		_E031.SetGamePooled();
		if (_E038.IsCancellationRequested)
		{
			return;
		}
		Singleton<GameWorld>.Instance.GameDateTime = GameDateTime;
		Singleton<GameWorld>.Instance.LocationId = _E025.Id;
		Singleton<GameWorld>.Instance.SpeedLimitsEnabled = speedLimitsEnabled;
		Singleton<GameWorld>.Instance.SpeedLimits = speedLimits;
		_E57A.Instance.InitAllExfiltrationPoints(_E025.exits, justLoadSettings: true, _E025.DisabledScavExits, giveAuthority: false);
		if (WeatherController.Instance != null)
		{
			TOD_Sky.Instance.Components.Time.GameDateTime = GameDateTime;
			WeatherController.Instance._E000(weathers);
		}
		_E02B = ChannelCombined._E000(base.gameObject, null, _E026.Connection, _E007, new ChannelCombined._E001(), encryptionEnabled, decryptionEnabled, null, 0);
		_E026.NetworkClient.RegisterHandler(170, _E006);
		if (!Singleton<_E307>.Instantiated)
		{
			Singleton<_E307>.Create(new _E307());
		}
		base.Status = GameStatus.Runned;
		World._E000<ClientWorld>(interactables, null, _E03B);
		Singleton<GameWorld>.Instance.RegisterBorderZones();
		Singleton<GameWorld>.Instance.RegisterRestrictableZones();
		_E7DF controller = Singleton<_E7DE>.Instance.Sound.Controller;
		if (_E03D.VoipEnabled)
		{
			_E03D.VoipQualitySettings.Apply();
			_E03D.MicrophoneChecked = _E7E0.CheckMicrophone();
			if (_E03D.MicrophoneChecked)
			{
				controller.ResetVoipDisabledReason();
				DissonanceComms.ClientPlayerId = _E01D.profileid;
				await _E5DB.Manager.LoadScene(_E785.DissonanceSetupScene, LoadSceneMode.Additive);
			}
			else
			{
				UnityEngine.Debug.LogError(_ED3E._E000(181023));
				controller.VoipDisabledByInitializationFail();
			}
		}
		else
		{
			controller.VoipDisabledByServer();
		}
		SetMatchmakerStatus(_ED3E._E000(180999));
		ClientScene.Ready(session.Connection);
		UnityEngine.Debug.Log(_ED3E._E000(181033));
		_E031.SetGameRunned();
		await ApplyCoopSetting();
	}

	void _E7A6.Stop()
	{
		UnityEngine.Debug.Log(_ED3E._E000(180530));
		string error = ((_E037.DisconnectionCode == EDisconnectionCode.Unknown) ? _ED3E._E000(180574).Localized() : (_E037.DisconnectionCode.ToString().Localized() + _ED3E._E000(18502) + _E037.AdditionalInfo));
		Result<ExitStatus, TimeSpan> result = default(Result<ExitStatus, TimeSpan>);
		result.Error = error;
		Result<ExitStatus, TimeSpan> value = result;
		_E00E(value);
	}

	void _E7A6.Abort(string error)
	{
		Result<ExitStatus, TimeSpan> result = default(Result<ExitStatus, TimeSpan>);
		result.Error = error;
		Result<ExitStatus, TimeSpan> value = result;
		_E00E(value);
	}

	private void FixedUpdate()
	{
		LocalTime += Time.deltaTime;
	}

	private void LateUpdate()
	{
		_E030.LateUpdate();
	}

	Task _E7A6.WorldSpawn(NetworkMessage message)
	{
		return World._E019.OnDeserialize(message.reader);
	}

	void _E7A6.WorldUnspawn(NetworkMessage message)
	{
	}

	void _E7A6.SubWorldSpawn(NetworkMessage message)
	{
		World._E019._E003(message.reader);
	}

	void _E7A6.SubWorldUnspawn(NetworkMessage message)
	{
	}

	void _E7A6.PlayerSpawn(NetworkReader reader, Callback<int> callback)
	{
		_E031.SetPlayerSpawnEvent();
		int id = reader.ReadInt32();
		byte channelId = reader.ReadByte();
		string message = string.Format(_ED3E._E000(180544), id, channelId);
		if (channelId < _E6A8._E006)
		{
			UnityEngine.Debug.LogError(message);
		}
		else
		{
			UnityEngine.Debug.Log(message);
		}
		Vector3 position = reader.ReadVector3();
		int sendRateFramesLimit = (Singleton<_E5B7>.Instantiated ? Singleton<_E5B7>.Instance.ClientSendRateLimit : 100);
		_E028 = ClientPlayer._E002(id, position, this, delegate(byte channelIndex, in ArraySegment<byte> segment, PacketPriority priority)
		{
			byte channelIndex2 = channelIndex;
			if (channelIndex < _E6A8._E006)
			{
				UnityEngine.Debug.LogError(string.Format(_ED3E._E000(181083), channelIndex, channelId));
				channelIndex2 = channelId;
			}
			_E02B._E005(channelIndex2, in segment, priority);
		}, sendRateFramesLimit, base.UpdateQueue, Player.EUpdateMode.Auto, _E2B6.Config.UseBodyFastAnimator ? Player.EUpdateMode.Manual : Player.EUpdateMode.Auto, _E2B6.Config.CharacterController.ClientPlayerMode, _E02F, _E03D);
		this.m__E013.Add(_E028.Id, _E028);
		_E028.OnEpInteraction += base.OnEpInteraction;
		if (_E02A.TryGetValue(channelId, out var value))
		{
			_E02A.Remove(channelId);
		}
		else
		{
			value = _E6C3._E000(channelId);
		}
		_E028.OnDeserializeInitialState(reader, value, delegate(IResult result)
		{
			_E028.Profile.Stats.TotalInGameTime = _E02F.TotalInGameTime;
			callback(new Result<int>(id, result.Error));
			_E017(_E028);
		});
	}

	void _E7A6.PlayerUnspawn(NetworkReader reader, Callback<int> callback)
	{
		int value = reader.ReadInt32();
		byte key = reader.ReadByte();
		_E02A.Remove(key);
		string error = null;
		ClientPlayer clientPlayer = Interlocked.Exchange(ref _E028, null);
		if (clientPlayer != null)
		{
			this.m__E013.Remove(clientPlayer.Id);
			_E029.Remove(clientPlayer.ChannelIndex);
			AssetPoolObject.ReturnToPool(clientPlayer.gameObject);
		}
		else
		{
			error = _ED3E._E000(180578);
		}
		callback(new Result<int>(value, error));
	}

	void _E7A6.ObserverSpawn(NetworkReader reader, Callback<int> callback)
	{
		int playerId = reader.ReadInt32();
		byte b = reader.ReadByte();
		Vector3 position = reader.ReadVector3();
		bool voipEnabled = _E03D.VoipEnabled && _E03D.MicrophoneChecked;
		ObservedPlayer observedPlayer = ObservedPlayer._E016(playerId, position, this, EUpdateQueue.Update, voipEnabled);
		_E005(observedPlayer);
		if (_E02A.TryGetValue(b, out var value))
		{
			_E02A.Remove(b);
		}
		else
		{
			value = _E6C3._E000(b);
		}
		observedPlayer.DeferredData = value;
		this.m__E013[observedPlayer.Id] = observedPlayer;
		byte channelIndex = reader.ReadByte();
		observedPlayer._E018(channelIndex);
		_E002(observedPlayer, reader, callback).HandleExceptions();
	}

	private Task _E002(ObservedPlayer player, NetworkReader reader, Callback<int> callback)
	{
		int id = player.Id;
		return player._E026(reader, _E003, delegate(IResult result)
		{
			_E03C = 0f;
			callback(new Result<int>(id, result.Error));
		});
	}

	private async Task _E003(int playerId)
	{
		if (base.Status == GameStatus.Started)
		{
			while (Time.fixedTime - _E03C < 2f)
			{
				await Task.Yield();
			}
		}
		else
		{
			UnityEngine.Debug.Log(string.Format(_ED3E._E000(181094), playerId));
			await World._E019.AwaitSpawnCompletion();
			UnityEngine.Debug.Log(string.Format(_ED3E._E000(181121), playerId));
		}
		if (!this.m__E013.ContainsKey(playerId))
		{
			throw new OperationCanceledException(string.Format(_ED3E._E000(181212), playerId));
		}
		_E03C = Time.fixedTime;
	}

	void _E7A6.ObserverUnspawn(NetworkReader reader, Callback<int> callback)
	{
		int num = reader.ReadInt32();
		byte key = reader.ReadByte();
		_E02A.Remove(key);
		string error = null;
		if (this.m__E013.TryGetValue(num, out var value))
		{
			this.m__E013.Remove(num);
			_E029.Remove(value.ChannelIndex);
			value.Dispose();
			AssetPoolObject.ReturnToPool(value.gameObject);
		}
		else
		{
			error = _ED3E._E000(180613);
		}
		callback(new Result<int>(num, error));
	}

	void _E7A6.DeathInventorySync(NetworkMessage message)
	{
		NetworkReader reader = message.reader;
		int num = reader.ReadInt32();
		UnityEngine.Debug.Log(_ED3E._E000(180652) + num);
		int size = reader.ReadInt32();
		byte[] bytes = reader.SafeReadBytes(size);
		if (this.m__E013.TryGetValue(num, out var value) && value is ObservedPlayer)
		{
			((ObservedPlayer)value)._E022(bytes);
		}
	}

	private void _E004(ExfiltrationPoint point, EExfiltrationStatus prevStatus)
	{
		UpdateExfiltrationUi(point, point.Entered.Any((Player x) => x.ProfileId == ProfileId));
	}

	private void _E005(Player observedPlayer)
	{
		foreach (NetworkPlayer value in this.m__E013.Values)
		{
			ObservedPlayer observedPlayer2 = value as ObservedPlayer;
			Collider collider = observedPlayer.CharacterControllerCommon.GetCollider();
			if (observedPlayer2 != null)
			{
				Collider collider2 = observedPlayer2.CharacterControllerCommon.GetCollider();
				_E320.IgnoreCollision(collider, collider2);
			}
		}
	}

	private void _E006(NetworkMessage message)
	{
		_E02B._E00C(message.reader);
	}

	private bool _E007(byte channelId, _E524 reader, int rtt)
	{
		bool result = false;
		int num = reader.ReadLimitedInt32(0, 2);
		switch (num)
		{
		case 1:
		{
			if (!_E029.TryGetValue(channelId, out var value) && !_E029.TryGetValue((byte)(channelId - 1), out value))
			{
				value = _E009(channelId);
				if (value != null)
				{
					_E029.Add(value.ChannelIndex, value);
				}
			}
			if (value != null)
			{
				value.OnDeserializeFromServer(channelId, reader, rtt);
				result = true;
			}
			else
			{
				_E008(channelId, reader);
				result = true;
			}
			break;
		}
		case 0:
		{
			ClientWorld clientWorld = World._E019 as ClientWorld;
			if ((bool)clientWorld)
			{
				PacketPriority priority = ((channelId == _E6A8._E005) ? PacketPriority.Critical : PacketPriority.Low);
				clientWorld._E000(reader, priority);
			}
			else
			{
				UnityEngine.Debug.LogError(_ED3E._E000(180239) + clientWorld);
			}
			break;
		}
		case 2:
			_EBDB.Instance.Deserialize(ref reader);
			break;
		default:
			UnityEngine.Debug.LogError(_ED3E._E000(180699) + num);
			break;
		}
		return result;
	}

	private void _E008(byte channelId, _E524 reader)
	{
		if ((int)channelId % 2 != 0)
		{
			if (!_E02A.TryGetValue(channelId, out var value))
			{
				value = _E6C3._E000(channelId);
				_E02A[channelId] = value;
			}
			value._E002(reader.Buffer);
		}
	}

	private NetworkPlayer _E009(byte channelId)
	{
		return this.m__E013.Values.SingleOrDefault((NetworkPlayer value) => value.ChannelIndex == channelId || value.ChannelIndex == channelId - 1);
	}

	void _E7A6.Spawn()
	{
		UnityEngine.Debug.Log(_ED3E._E000(180676));
		_E031.SetGameSpawn();
		_E026.Spawn();
	}

	void _E7A6.GameSpawned()
	{
		_E031.SetGameSpawned();
		_E3AB.RunHeapPreAllocation();
		_E3AB.Collect(2, GCCollectionMode.Forced, isBlocking: true, compacting: true, force: true);
		if (_E3AB.Settings.OverrideRamCleanerSettings ? _E3AB.Settings.RamCleanerEnabled : ((bool)Singleton<_E7DE>.Instance.Game.Settings.AutoEmptyWorkingSet))
		{
			_E3AB.EmptyWorkingSet();
		}
		_E3AB.GCEnabled = false;
		Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
		if (base.Status == GameStatus.Runned)
		{
			UnityEngine.Debug.Log(_ED3E._E000(180732));
			_E026.StartGame();
		}
		else if (base.Status == GameStatus.Started)
		{
			_E026.RestartGame();
		}
	}

	void _E7A6.GameMatching(ushort activitiesCounter, ushort minCounter, int seconds)
	{
		SetMatchmakerStatus(_ED3E._E000(180726));
	}

	void _E7A6.GameStarting(int seconds)
	{
		_E031.SetGameStarting();
		base.Status = GameStatus.Starting;
		new MatchmakerFinalCountdown._E000(this._E000, _E5AD.Now.AddSeconds(seconds)).ShowScreen(EScreenState.Root);
		Singleton<GUISounds>.Instance._E004();
		Singleton<BetterAudio>.Instance.TweenAmbientVolume(0f, seconds);
		_E00A();
	}

	private void _E00A()
	{
		if (_E028 != null)
		{
			PlayerCameraController.Create(_E028);
			_E8A8.Instance.SetOcclusionCullingEnabled(_E025.OcculsionCullingEnabled);
		}
	}

	void _E7A6.GameStartingWithTeleport(Vector3 position, int exfiltrationId, string entryPoint)
	{
		_E031.SetGameStarting();
		_E028.Teleport(position);
		_E028.ScavExfilMask = exfiltrationId;
		_E028.Profile.Info.EntryPoint = entryPoint;
		_E026.StartGameAfterTeleport();
	}

	void _E7A6.GameStarted(float pastTime, int sessionSeconds)
	{
		_E031.SetGameStarted();
		if (Math.Abs(_E031.GameStarting) < 0.001f)
		{
			_E031.GameStarting = _E031.GameStarted;
		}
		base.Status = GameStatus.Started;
		base.GameTimer.Start(pastTime, sessionSeconds);
		Singleton<GUISounds>.Instance._E004();
		Singleton<BetterAudio>.Instance.ForceSetAmbientVolume(0f);
		ExfiltrationPoint[] array = ((_E028.Profile.Info.Side == EPlayerSide.Savage) ? _E57A.Instance.ScavExfiltrationClaim(_E028.ScavExfilMask, _E028.Profile.Id) : _E57A.Instance.EligiblePoints(_E028.Profile));
		this.m__E007.gameObject.SetActive(value: true);
		this.m__E007.TimerPanel.ProfileId = ProfileId;
		float seconds = base.GameTimer.EscapeTimeSeconds();
		this.m__E007.TimerPanel.SetTime(_E5AD.UtcNow, _E028.Side, seconds, array);
		ExfiltrationPoint[] array2 = array;
		foreach (ExfiltrationPoint point in array2)
		{
			point.OnStatusChanged += _E004;
			CompositeDisposable.AddDisposable(delegate
			{
				point.OnStatusChanged -= _E004;
			});
			UpdateExfiltrationUi(point, contains: false, initial: true);
		}
		if (_E023 != null)
		{
			_E023();
			_E023 = null;
		}
		_E00B();
		string text = null;
		if (_E028.Profile.Info.Side == EPlayerSide.Savage && _E02F.ActivePetStatus != null)
		{
			text = _E02F.ActivePetStatus.shortId;
		}
		else if (_E02F.ActiveProfileStatus != null)
		{
			text = _E02F.ActiveProfileStatus.shortId;
		}
		if (string.IsNullOrEmpty(text))
		{
			UnityEngine.Debug.LogError(_ED3E._E000(180706));
		}
		if (_E02D == ERaidMode.Coop)
		{
			text += _ED3E._E000(180744);
		}
		MonoBehaviourSingleton<PreloaderUI>.Instance.SetSessionId(text);
		ConsoleScreen.ApplyStartCommands();
	}

	void _E7A6.GameRestarted()
	{
		this.m__E007.BattleUiPanelDeath.Close();
		_E00B();
	}

	private void _E00B()
	{
		_E00A();
		if (_E028.HealthController.IsAlive)
		{
			this.m__E00E = GamePlayerOwner._E000(_E028, _E020, _E02E, _E02F, _E021, _E022, this.m__E007, GameDateTime, _E025);
			_E021.MenuScreen.OwnerOnLeaveGameEvent = _E00C;
			CompositeDisposable.AddDisposable(delegate
			{
				_E021.MenuScreen.OwnerOnLeaveGameEvent = null;
			});
			_E028.HealthController.DiedEvent += delegate
			{
				_E028.TranslateNetCommands = false;
				this.m__E00E._E023();
				if (_E027)
				{
					StartCoroutine(_E00D());
				}
			};
			CompositeDisposable.AddDisposable(delegate
			{
				_E028.HealthController.DiedEvent -= delegate
				{
					_E028.TranslateNetCommands = false;
					this.m__E00E._E023();
					if (_E027)
					{
						StartCoroutine(_E00D());
					}
				};
			});
			_E028.TranslateNetCommands = true;
			this.m__E00E._E022();
		}
		else if (_E027)
		{
			StartCoroutine(_E00D());
		}
		if (World._E019 is ClientWorld clientWorld)
		{
			clientWorld._E002();
		}
		else
		{
			UnityEngine.Debug.LogError(_ED3E._E000(180747));
		}
		_E026.GameStarted();
		_E030.Start();
	}

	private void _E00C()
	{
		ReconnectionScreen._E000 reconnectionScreenController = new ReconnectionScreen._E000(_E028.Profile, _E025, _E02C, returnAllowed: true, nextScreenAllowed: false, _E02F);
		reconnectionScreenController.OnLeave += _E026.StopGame;
		CompositeDisposable.AddDisposable(delegate
		{
			reconnectionScreenController.OnLeave -= _E026.StopGame;
		});
		reconnectionScreenController.ShowScreen(EScreenState.Queued);
	}

	private IEnumerator _E00D()
	{
		UnityEngine.Debug.Log(_ED3E._E000(181191));
		_E026.RestartGameInitiate();
		this.m__E007.BattleUiPanelDeath.Show(this._E000, ExitStatus.Killed, TimeSpan.Zero);
		yield return new WaitForSeconds(2f);
		_E028.PointOfView = EPointOfView.ThirdPerson;
	}

	void _E7A6.GameRestarting()
	{
		UnityEngine.Debug.Log(_ED3E._E000(180799));
		if (base.Status == GameStatus.Started)
		{
			UnityEngine.Debug.Log(_ED3E._E000(180820));
			if (this.m__E00E != null)
			{
				UnityEngine.Object.DestroyImmediate(this.m__E00E);
				this.m__E00E = null;
			}
			_E028 = null;
			_E026.Respawn();
		}
		UnityEngine.Debug.Log(_ED3E._E000(180860));
	}

	void _E7A6.GetRadioTransmitterData(string profileID)
	{
		_E026.CmdGetRadiotransmitterData(profileID);
	}

	void _E7A6.GameStopping()
	{
		_E00E(null);
	}

	void _E7A6.GameStopped(ExitStatus exitStatus, TimeSpan playTime)
	{
		_E039 = new Result<ExitStatus, TimeSpan>(exitStatus, playTime);
	}

	void _E7A6.SoftStopNotification(TimeSpan sessionTime)
	{
		base.GameTimer.ChangeSessionTime(sessionTime);
		float seconds = base.GameTimer.EscapeTimeSeconds();
		this.m__E007.TimerPanel.SetTime(_E5AD.UtcNow, _E028.Side, seconds, _E57A.Instance.EligiblePoints(_E01E));
		TimeSpan timeSpan = base.GameTimer.EscapeTimeTimeSpan();
		_E857.DisplayWarningNotification(string.Format(_ED3E._E000(180891).Localized(), timeSpan.Minutes, timeSpan.Seconds), ENotificationDurationType.Long);
		_E857.DisplayMessageNotification(_ED3E._E000(180923).Localized(), ENotificationDurationType.Long);
	}

	void _E7A6.DevelopSetBotData(byte[] data)
	{
		Singleton<_E3B5>.Instance._E002(data);
	}

	void _E7A6.DevelopSetBotDataZones(byte[] data)
	{
		Singleton<_E3B5>.Instance._E000(data);
	}

	void _E7A6.DevelopSetBotDataGroups(byte[] data)
	{
		Singleton<_E3B5>.Instance._E001(data);
	}

	private void _E00E(Result<ExitStatus, TimeSpan>? result)
	{
		_E039 = result;
		if (_E024 == null)
		{
			_E024 = StartCoroutine(_E00F());
		}
	}

	private IEnumerator _E00F()
	{
		using (_E069.StartWithToken(_ED3E._E000(183320)))
		{
			base.Status = GameStatus.Stopping;
			float time = Time.time;
			_E907 value = _E011();
			if (Singleton<_E90A>.Instantiated)
			{
				Singleton<_E90A>.Release(Singleton<_E90A>.Instance);
			}
			_E038.Cancel();
			if (_E028 != null)
			{
				_E028.TranslateNetCommands = false;
				_E010();
				_E028.OnEpInteraction -= base.OnEpInteraction;
			}
			using (_E069.StartWithToken(_ED3E._E000(183298)))
			{
				_EC92.Instance.CloseAllScreensForced();
			}
			yield return StartCoroutine(_E015());
			if (_E028 != null)
			{
				_E02F.LastPlayerState = _E028.Profile.GetVisualEquipmentState();
			}
			_E022.SetLoaderStatus(status: true);
			using (_E069.StartWithToken(_ED3E._E000(183328)))
			{
				while (!_E039.HasValue)
				{
					yield return null;
				}
			}
			if (_E026 != null)
			{
				_E026.Connection?.Close(_ED3E._E000(183370), isError: false);
			}
			if (_E02B != null)
			{
				_E02B._E001();
			}
			Result<ExitStatus, TimeSpan> value2 = _E039.Value;
			if (value2.Succeed)
			{
				yield return StartCoroutine(_E013(time, 5f));
				ExitStatus value3 = value2.Value0;
				this.m__E00E.Player.OnGameSessionEnd(value3, base.PastTime, _E025.Id, string.Empty);
				_E012();
				if (value3 == ExitStatus.Killed)
				{
					this.m__E007.BattleUiPanelDeath.Show(this._E000, value3, value2.Value1);
				}
			}
			else
			{
				yield return StartCoroutine(_E013(time, 1f));
				_E012();
				yield return StartCoroutine(_E016(value2.Error));
			}
			base.Status = GameStatus.Stopped;
			using (_E069.StartWithToken(_ED3E._E000(183420)))
			{
				Interlocked.Exchange(ref this.m__E009, null)?.Invoke(new Result<ExitStatus, TimeSpan, _E907>
				{
					Error = value2.Error,
					Value0 = value2.Value0,
					Value1 = value2.Value1,
					Value2 = value
				});
			}
			if (_E034 != null)
			{
				_E034.Dispose();
				_E034 = null;
			}
		}
	}

	private void _E010()
	{
		GameUi.BattleUiPmcCount.Close();
		GameUi.BattleUiPanelExitTrigger.Close();
	}

	private _E907 _E011()
	{
		using (_E069.StartWithToken(_ED3E._E000(180942)))
		{
			_E030.Stop();
			_E907 obj = new _E907();
			_E8EF metrics = _E030.Metrics;
			if (metrics != null)
			{
				obj.sid = _E01D.sid;
				obj.HardwareDescription = _E770._E001();
				obj.Location = _E025.Name;
				obj.Metrics = metrics;
				obj.ClientEvents = _E031;
				if (Singleton<_E7DE>.Instantiated)
				{
					_E7DE instance = Singleton<_E7DE>.Instance;
					obj.Settings = instance.Graphics.Settings.Clone();
					obj.SharedSettings = new _E906(instance.Game);
				}
			}
			if (_E034 != null)
			{
				obj.SpikeSamples = _E034.SpikeSamples;
			}
			return obj;
		}
	}

	private void _E012()
	{
		using (_E069.StartWithToken(_ED3E._E000(180989)))
		{
			NetworkPlayer[] array = this.m__E013.Values.ToArray();
			foreach (NetworkPlayer networkPlayer in array)
			{
				try
				{
					networkPlayer.Dispose();
					AssetPoolObject.ReturnToPool(networkPlayer.gameObject);
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogException(exception);
				}
			}
			if (Singleton<_E3B5>.Instantiated)
			{
				_E3B5 instance = Singleton<_E3B5>.Instance;
				instance.Stop();
				Singleton<_E3B5>.Release(instance);
			}
		}
	}

	private IEnumerator _E013(float stopTime, float seconds)
	{
		using (_E069.StartWithToken(_ED3E._E000(183411)))
		{
			yield return StartCoroutine(_E014(stopTime, seconds));
			if (_E026 != null && _E026.Connection != null && _E026.Connection.isConnected)
			{
				_E026.Connection.Close(_ED3E._E000(157750), isError: false);
				yield return StartCoroutine(_E014(Time.time, 1f));
			}
		}
	}

	public void OnDisconnectStatusAccepted(_E5B9 disconnectionReason)
	{
		_E037 = disconnectionReason;
	}

	public void VoipAbuseNotification(string reporterId)
	{
		_E028.VoipAbuseNotification(reporterId);
	}

	public void ParseMineExplosionData(byte[] data)
	{
		Vector3 minePosition = MineDirectional._E000.Deserialize(data);
		MineDirectional mineDirectional = Singleton<GameWorld>.Instance.MineManager.Mines.FirstOrDefault((MineDirectional mine) => Vector3.Distance(mine.transform.position, minePosition) < Mathf.Epsilon);
		if (!(mineDirectional == null))
		{
			mineDirectional.Explosion();
		}
	}

	public void ParseAirdropContainerData(byte[] data)
	{
		_E032.ParseAirdropContainerData(data).HandleExceptions();
	}

	public void AirdropFlareSuccessEvent(bool canSpawnAirdrop)
	{
		Singleton<GameWorld>.Instance.SynchronizableObjectLogicProcessor.AirdropManager.OnFlareSuccessEvent(canSpawnAirdrop);
	}

	public void ParseBufferZoneData(byte[] data)
	{
		_E033.ParseBufferZoneData(data);
	}

	public void RecieveClientRadioTransmitterData(_E638 data)
	{
		foreach (Player allPlayer in Singleton<GameWorld>.Instance.AllPlayers)
		{
			if (allPlayer.Id != data.PlayerID)
			{
				continue;
			}
			((ClientPlayer)allPlayer).OnChangeRadioTransmitterState(data.IsEncoded, data.Status, data.IsAgressor);
			break;
		}
	}

	public void RecieveObserverRadioTransmitterData(_E638 data)
	{
		foreach (Player allPlayer in Singleton<GameWorld>.Instance.AllPlayers)
		{
			if (allPlayer.Id != data.PlayerID)
			{
				continue;
			}
			((ObservedPlayer)allPlayer).OnChangeRadioTransmitterState(data.IsEncoded, data.Status, data.IsAgressor);
			break;
		}
	}

	public void RecieveLighthouseTraderZoneData(_E634 data)
	{
		foreach (Player allPlayer in Singleton<GameWorld>.Instance.AllPlayers)
		{
			if (!allPlayer.IsYourPlayer)
			{
				continue;
			}
			((ClientPlayer)allPlayer).SyncLighthouseTraderZoneData(data);
			break;
		}
	}

	private IEnumerator _E014(float stopTime, float seconds)
	{
		while (Time.time - stopTime < seconds && _E026 != null && _E026.Connection != null && _E026.Connection.isConnected)
		{
			yield return null;
		}
	}

	private IEnumerator _E015()
	{
		using (_E069.StartWithToken(_ED3E._E000(183395)))
		{
			bool complete = false;
			MonoBehaviourSingleton<PreloaderUI>.Instance.SetMenuChatBarVisibility(visible: false);
			MonoBehaviourSingleton<PreloaderUI>.Instance.StartBlackScreenShow(1f, 1f, delegate
			{
				this.m__E007.TimerPanel.Close();
				if (this.m__E00E != null)
				{
					this.m__E00E._E023();
				}
				_EC92.Instance.CloseAllScreensForced();
				complete = true;
			});
			float fixedTime = Time.fixedTime;
			while (!complete && Time.fixedTime - 5f < fixedTime)
			{
				yield return null;
			}
		}
	}

	private IEnumerator _E016(string error)
	{
		using (_E069.StartWithToken(_ED3E._E000(183436)))
		{
			UIEventSystem.Instance.Enable();
			bool complete = false;
			_E022.ShowCriticalErrorScreen(_ED3E._E000(8047), error, ErrorScreen.EButtonType.OkButton, 30f, delegate
			{
				complete = true;
			}, delegate
			{
				complete = true;
			});
			while (!complete)
			{
				yield return null;
			}
		}
	}

	public async Task ApplyCoopSetting()
	{
		if (CoopEnabled)
		{
			_E03F = await _E02F.ReceiveCoopRaidSettings();
			_E018();
		}
	}

	private void _E017(Player player)
	{
		if (CoopEnabled && _E03F.MetabolismDisabled)
		{
			player.HealthController.DisableMetabolism();
		}
	}

	private void _E018()
	{
		if (!(WeatherController.Instance == null))
		{
			DateTime dateTime = _E5AD.StartOfDay();
			DateTime dateTime2 = dateTime.AddDays(1.0);
			_E8EB obj = _E8EB.CreateDefault();
			_E8EB obj2 = _E8EB.CreateDefault();
			obj.Cloudness = (obj2.Cloudness = _E03F.TimeAndWeatherSettings.CloudinessType.ToValue());
			obj.Rain = (obj2.Rain = _E03F.TimeAndWeatherSettings.RainType.ToValue());
			obj.Wind = (obj2.Wind = _E03F.TimeAndWeatherSettings.WindType.ToValue());
			obj.ScaterringFogDensity = (obj2.ScaterringFogDensity = _E03F.TimeAndWeatherSettings.FogType.ToValue());
			obj.Time = dateTime.Ticks;
			obj2.Time = dateTime2.Ticks;
			WeatherController.Instance._E000(new _E8EB[2] { obj, obj2 });
		}
	}

	[CompilerGenerated]
	private bool _E019(Player x)
	{
		return x.ProfileId == ProfileId;
	}

	[CompilerGenerated]
	private void _E01A()
	{
		_E021.MenuScreen.OwnerOnLeaveGameEvent = null;
	}

	[CompilerGenerated]
	private void _E01B(EDamageType damageType)
	{
		_E028.TranslateNetCommands = false;
		this.m__E00E._E023();
		if (_E027)
		{
			StartCoroutine(_E00D());
		}
	}

	[CompilerGenerated]
	private void _E01C()
	{
		_E028.HealthController.DiedEvent -= delegate
		{
			_E028.TranslateNetCommands = false;
			this.m__E00E._E023();
			if (_E027)
			{
				StartCoroutine(_E00D());
			}
		};
	}
}
