#define BATTLEYE_ANTICHEAT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BattlEye;
using Comfort.Common;
using ComponentAce.Compression.Libs.zlib;
using CustomPlayerLoopSystem;
using Diz.Utils;
using EFT.Counters;
using EFT.UI.Matchmaker;
using Newtonsoft.Json;
using SaberClient;
using UnityEngine;
using UnityEngine.Networking;

namespace EFT;

internal sealed class NetworkGameSession : AbstractGameSession, _E7A5._E000, _E7A7, IBattlEyeClientRequestHandler
{
	private new sealed class _E000 : _E315
	{
		internal _E000(LoggerMode mode)
			: base(_ED3E._E000(182281), mode)
		{
		}
	}

	private new sealed class _E001 : _E315
	{
		internal _E001(LoggerMode mode)
			: base(_ED3E._E000(182275), mode)
		{
		}
	}

	[CompilerGenerated]
	private new sealed class _E003
	{
		public NetworkGameSession _003C_003E4__this;

		public AbstractGameSession._E004 response;

		internal void _E000()
		{
			_E000 obj = new _E000(LoggerMode.None);
			SClientWrapper.Load(serverIp: _003C_003E4__this.NetworkClient.serverIp, logDelegate: obj.Log, sessionId: response._E00A, antiCheatPort: response._E00C);
		}
	}

	[CompilerGenerated]
	private new sealed class _E005
	{
		public byte[] prefabsData;

		public byte[] customizationsData;

		public NetworkGameSession _003C_003E4__this;

		public int id;

		public float time;

		internal ResourceKey[] _E000()
		{
			_E006 CS_0024_003C_003E8__locals0 = new _E006();
			string text = SimpleZlib.Decompress(prefabsData);
			string text2 = SimpleZlib.Decompress(customizationsData);
			ResourceKey[] array = text.ParseJsonTo<ResourceKey[]>(Array.Empty<JsonConverter>());
			CS_0024_003C_003E8__locals0.solver = Singleton<_E60E>.Instance;
			string[] array2 = text2.ParseJsonTo<string[]>(Array.Empty<JsonConverter>());
			IEnumerable<ResourceKey> second = array2.Select((string x) => CS_0024_003C_003E8__locals0.solver.GetBundle(x));
			_003C_003E4__this.m__E052.LogTrace(_ED3E._E000(182436), id);
			_003C_003E4__this.m__E052.LogTrace(_ED3E._E000(182469), array.Length, text);
			_003C_003E4__this.m__E052.LogTrace(_ED3E._E000(182514), array2.Length, text2);
			return array.Concat(second).ToArray();
		}

		internal void _E001(float p)
		{
			if (!(Time.fixedTime - time < 1f))
			{
				time = Time.time;
				_003C_003E4__this._E00C(id, p);
			}
		}
	}

	[CompilerGenerated]
	private new sealed class _E006
	{
		public _E60E solver;

		internal ResourceKey _E000(string x)
		{
			return solver.GetBundle(x);
		}
	}

	private new _E315 m__E046;

	private new static float m__E047 = 0.7f;

	private new const string m__E048 = "anticheat";

	private new const string m__E049 = "resources";

	private new _E6A7 m__E04A;

	private new readonly _E3A4 m__E04B = new _E3A4();

	private new _E7A5 m__E04C;

	private new _E7A6 m__E04D;

	private new readonly List<int> m__E04E = new List<int>();

	private new DateTime m__E04F;

	private new readonly TimeSpan m__E050 = TimeSpan.FromMinutes(6.0);

	private new readonly Dictionary<int, NetworkWriter> m__E051 = new Dictionary<int, NetworkWriter>();

	private new _E001 m__E052;

	private new Action m__E053;

	[CompilerGenerated]
	private new bool m__E054;

	[CompilerGenerated]
	private new byte[] m__E055;

	[CompilerGenerated]
	private new int m__E056;

	[CompilerGenerated]
	private new NetworkClient m__E057;

	private new Dictionary<int, List<(int, byte[])>> m__E058 = new Dictionary<int, List<(int, byte[])>>();

	private new static readonly _E387 m__E059 = new _E387(20);

	private new static readonly _E361 m__E05A = new _E361(60);

	private new static double m__E05B;

	private new static float m__E05C;

	[CompilerGenerated]
	private static int _E05D;

	public bool ObserveOnly
	{
		[CompilerGenerated]
		get
		{
			return this.m__E054;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E054 = value;
		}
	}

	public byte[] OpenEncryptionKey
	{
		[CompilerGenerated]
		get
		{
			return this.m__E055;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E055 = value;
		}
	}

	public int OpenEncryptionKeyLength
	{
		[CompilerGenerated]
		get
		{
			return this.m__E056;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E056 = value;
		}
	}

	public NetworkClient NetworkClient
	{
		[CompilerGenerated]
		get
		{
			return this.m__E057;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E057 = value;
		}
	}

	public static double AverageRangeTrend => NetworkGameSession.m__E05A.Average;

	public static double RTT
	{
		get
		{
			return NetworkGameSession.m__E05B;
		}
		private set
		{
			NetworkGameSession.m__E05B = value;
			double val = NetworkGameSession.m__E05B / 1000.0;
			NetworkGameSession.m__E059.AddValue(val);
			double rangeDiff = NetworkGameSession.m__E059.RangeDiff;
			NetworkGameSession.m__E05A.AddValue(rangeDiff);
		}
	}

	public static bool ClientServerConnectionLags
	{
		get
		{
			return Time.time - NetworkGameSession.m__E05C < NetworkGameSession.m__E047;
		}
		set
		{
			NetworkGameSession.m__E05C = Time.time;
		}
	}

	public static int LossPercent
	{
		[CompilerGenerated]
		get
		{
			return _E05D;
		}
		[CompilerGenerated]
		private set
		{
			_E05D = value;
		}
	}

	public bool HasAuthority => GetComponent<NetworkIdentity>().hasAuthority;

	internal new static NetworkGameSession _E000(NetworkGame game, string profileId, string token, _E315 logger, Action onConnected = null)
	{
		UNetUpdate.OnUpdate += NetworkIdentity.ManualUnetUpdate;
		string text = _ED3E._E000(183487) + profileId + _ED3E._E000(27308);
		NetworkGameSession networkGameSession = AbstractGameSession._E000<NetworkGameSession>(game.gameObject.transform, text, profileId, token);
		NetworkClient networkClient3 = (networkGameSession.NetworkClient = (Singleton<NetworkClient>.Instance = new NetworkClient()));
		networkClient3.RegisterHandler(147, networkGameSession._E004);
		networkClient3.RegisterHandler(148, networkGameSession._E005);
		networkClient3.RegisterHandler(188, networkGameSession._E00A);
		networkClient3.RegisterHandler(189, networkGameSession._E006);
		networkClient3.RegisterHandler(168, networkGameSession._E009);
		networkGameSession.m__E04A = new _E6A7();
		networkClient3.RegisterHandler(185, networkGameSession.m__E04A.OnPartialCommand);
		networkGameSession.m__E04B.AddDisposable(_EBAF.Instance.SubscribeOnEvent<_EBB3>(networkGameSession._E00F));
		networkGameSession.m__E04B.AddDisposable(_EBAF.Instance.SubscribeOnEvent<_EBBE>(networkGameSession._E010));
		networkGameSession.m__E04C = _E7A5._E000(networkGameSession);
		networkGameSession.m__E04D = game;
		networkGameSession.m__E053 = onConnected;
		networkGameSession.m__E052 = new _E001(LoggerMode.Add);
		networkGameSession.ObserveOnly = false;
		networkGameSession.m__E046 = logger;
		return networkGameSession;
	}

	internal void _E001(HostTopology hostTopology, string address, int port)
	{
		this.m__E04C._E002(hostTopology, address, port);
	}

	internal void _E002(HostTopology hostTopology, string address, int port, int latency, float packetLoss)
	{
		this.m__E04C._E003(hostTopology, address, port, latency, packetLoss);
	}

	void _E7A5._E000.OnConnect(NetworkConnection connection)
	{
		base.Connection = connection;
		_E7A8._E002 = connection;
		_E008(NetworkClient.serverIp);
		_E003();
		this.m__E053?.Invoke();
	}

	void _E7A5._E000.OnDisconnect(NetworkConnection connection)
	{
		Debug.Log(_ED3E._E000(183469));
		_E7A8._E002 = null;
		base.Connection = null;
		this.m__E04C._E004();
		this.m__E04D.Stop();
		short[] array = NetworkClient.handlers.Keys.ToArray();
		foreach (short msgType in array)
		{
			NetworkClient.UnregisterHandler(msgType);
		}
		NetworkClient.UnregisterHandler(185);
		Singleton<NetworkClient>.Release(null);
		NetworkClient.Shutdown();
		NetworkClient = null;
		ClientScene.UnregisterSpawnHandler(AbstractSession._E000);
		ClientScene.DestroyAllClientObjects();
		AsyncWorker.RunOnBackgroundThread(delegate
		{
			SClientWrapper.Unload();
			BEClient.Stop();
		});
	}

	private void _E003()
	{
		AbstractGameSession._E000 msg = new AbstractGameSession._E000
		{
			_E000 = base.ProfileId,
			_E001 = base.Token,
			_E002 = ObserveOnly,
			_E003 = OpenEncryptionKey,
			_E004 = OpenEncryptionKeyLength,
			_E005 = this.m__E04D.LocationId
		};
		base.Connection.Send(147, msg);
	}

	private async void _E004(NetworkMessage message)
	{
		Debug.Log(_ED3E._E000(182325));
		_ = message.conn;
		NetworkClient.UnregisterHandler(147);
		AbstractGameSession._E004 response;
		try
		{
			response = message.ReadMessage<AbstractGameSession._E004>();
		}
		catch (Exception)
		{
			this.m__E04D.Abort(_ED3E._E000(183739));
			return;
		}
		try
		{
			await _E007(response);
		}
		catch (Exception)
		{
			this.m__E04D.Abort(_ED3E._E000(182310));
		}
	}

	private void _E005(NetworkMessage message)
	{
		Debug.Log(_ED3E._E000(183492));
		_ = message.conn;
		NetworkClient.UnregisterHandler(147);
		string error;
		try
		{
			error = message.ReadMessage<AbstractGameSession._E005>()._E004 switch
			{
				100501 => _ED3E._E000(183541), 
				100502 => _ED3E._E000(183568), 
				100500 => _ED3E._E000(183606), 
				100503 => _ED3E._E000(183618), 
				_ => _ED3E._E000(183702), 
			};
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			error = _ED3E._E000(183739);
		}
		this.m__E04D.Abort(error);
	}

	private void _E006(NetworkMessage message)
	{
		AbstractGameSession._E006 obj = message.ReadMessage<AbstractGameSession._E006>();
		if (_EC92.Instance.CurrentScreenController is MatchmakerTimeHasCome._E000 obj2 && !obj2.RaidSettings.Local)
		{
			obj2.ChangeStatus(_ED3E._E000(183755), obj._E000);
		}
	}

	private async Task _E007(AbstractGameSession._E004 response)
	{
		_E7A8.LogsLevel = response._E00D;
		base.MemberCategory = response._E007;
		string text = SimpleZlib.Decompress(response._E003);
		ResourceKey[] array = text.ParseJsonTo<ResourceKey[]>(Array.Empty<JsonConverter>());
		string text2 = SimpleZlib.Decompress(response._E004);
		string[] array2 = text2.ParseJsonTo<string[]>(Array.Empty<JsonConverter>());
		this.m__E052.LogTrace(_ED3E._E000(182347), _ED3E._E000(182325), array.Length, text);
		this.m__E052.LogTrace(_ED3E._E000(182381), _ED3E._E000(182325), array2.Length, text2);
		string text3 = SimpleZlib.Decompress(response._E005);
		_E8EB[] weathers = text3.ParseJsonTo<_E8EB[]>(Array.Empty<JsonConverter>());
		Debug.LogFormat(_ED3E._E000(182415), text);
		Debug.LogFormat(_ED3E._E000(182457), text3);
		float fixedDeltaTime = response._E008;
		ClientScene.RegisterSpawnHandler(AbstractSession._E000, _E011, _E012);
		Dictionary<string, int> interactables = SimpleZlib.Decompress(response._E009).ParseJsonTo<Dictionary<string, int>>(Array.Empty<JsonConverter>());
		_E5D9.SetupPositionQuantizer(response._E00B);
		await this.m__E04D.Run(this, response._E006, response._E002, interactables, array, array2, weathers, fixedDeltaTime, response._E00F, response._E010, response._E000, response._E001, response._E011);
		SClientWrapper.CacheStaticData();
		BEClient.CacheStaticData();
		await AsyncWorker.RunOnBackgroundThread(delegate
		{
			_E000 obj = new _E000(LoggerMode.None);
			SClientWrapper.Load(serverIp: NetworkClient.serverIp, logDelegate: obj.Log, sessionId: response._E00A, antiCheatPort: response._E00C);
		});
	}

	private void _E008(string serverIp)
	{
		IPAddress.TryParse(serverIp, out var address);
		_E000 obj = new _E000(LoggerMode.None);
		uint serverAddress = 0u;
		if (address != null)
		{
			byte[] addressBytes = address.GetAddressBytes();
			serverAddress = (uint)((addressBytes[0] << 24) | (addressBytes[1] << 16) | (addressBytes[2] << 8) | addressBytes[3]);
		}
		BEClient.Start(_ED3E._E000(146106), serverAddress, (ushort)NetworkClient.serverPort, this, obj.Log, obj.IsEnabled);
		OpenEncryptionKey = BEClient.OpenEncryptionKey;
		OpenEncryptionKeyLength = BEClient.OpenEncryptionKeyLength;
	}

	void IBattlEyeClientRequestHandler.OnRequestRestart(BEClient.ERestartReason reason)
	{
	}

	void IBattlEyeClientRequestHandler.OnSendPacket(byte[] bePacket)
	{
		if (base.Connection != null)
		{
			base.Connection.Send(168, new AbstractGameSession._E007
			{
				_E000 = bePacket
			});
		}
	}

	private void _E009(NetworkMessage netMsg)
	{
		BEClient.AcceptServerPacket(netMsg.ReadMessage<AbstractGameSession._E007>()._E000);
	}

	private void _E00A(NetworkMessage message)
	{
		AbstractGameSession._E008 obj = message.ReadMessage<AbstractGameSession._E008>();
		_E00B(obj._E000, obj._E001, obj._E002).HandleExceptions();
	}

	private async Task _E00B(int id, byte[] prefabsData, byte[] customizationsData)
	{
		ResourceKey[] resources = await AsyncWorker.RunOnBackgroundThread(delegate
		{
			string text = SimpleZlib.Decompress(prefabsData);
			string text2 = SimpleZlib.Decompress(customizationsData);
			ResourceKey[] array = text.ParseJsonTo<ResourceKey[]>(Array.Empty<JsonConverter>());
			_E60E solver = Singleton<_E60E>.Instance;
			string[] array2 = text2.ParseJsonTo<string[]>(Array.Empty<JsonConverter>());
			IEnumerable<ResourceKey> second = array2.Select((string x) => solver.GetBundle(x));
			this.m__E052.LogTrace(_ED3E._E000(182436), id);
			this.m__E052.LogTrace(_ED3E._E000(182469), array.Length, text);
			this.m__E052.LogTrace(_ED3E._E000(182514), array2.Length, text2);
			return array.Concat(second).ToArray();
		});
		float time = 0f;
		IProgress<_E5BB> progress = new _ECCE<float>(delegate(float p)
		{
			if (!(Time.fixedTime - time < 1f))
			{
				time = Time.time;
				_E00C(id, p);
			}
		}).Select((_E5BB p) => (p.Stage != 0) ? (0.8f + p.Progress * 0.2f) : (p.Progress * 0.8f));
		try
		{
			await Singleton<_E760>.Instance.LoadBundlesAndCreatePools(_E760.PoolsCategory.Raid, _E760.AssemblyType.Online, resources, _ECE3.Low, progress);
		}
		catch (Exception exception)
		{
			_E7A8.TraceError(ETraceCode.LoadProfileResourcesException, id.ToString());
			Debug.LogException(exception);
		}
		_E00C(id, 1f);
	}

	private void _E00C(int id, float progressValue)
	{
		if (base.Connection != null)
		{
			base.Connection.Send(190, new _E009
			{
				_E000 = base.ProfileId,
				_E001 = id,
				_E002 = progressValue
			});
		}
		else
		{
			Debug.LogError(_ED3E._E000(183780));
		}
	}

	public override void OnStartAuthority()
	{
		Debug.Log(_ED3E._E000(183840));
		this.m__E04D.Spawn();
	}

	void _E7A7.Spawn()
	{
		Debug.Log(_ED3E._E000(183891));
		NetworkClient.RegisterHandler(151, _E00D);
		NetworkClient.RegisterHandler(152, this.m__E04D.WorldUnspawn);
		NetworkClient.RegisterHandler(153, this.m__E04D.SubWorldSpawn);
		NetworkClient.RegisterHandler(154, this.m__E04D.SubWorldUnspawn);
		NetworkClient.RegisterHandler(156, _E014);
		NetworkClient.RegisterHandler(158, _E016);
		NetworkClient.RegisterHandler(160, this.m__E04D.DeathInventorySync);
		this.m__E04A.AddHandler(155, _E013);
		this.m__E04A.AddHandler(157, _E015);
		CallCmdSpawn();
	}

	private void _E00D(NetworkMessage message)
	{
		_E00E(message).HandleExceptions();
	}

	private async Task _E00E(NetworkMessage message)
	{
		await this.m__E04D.WorldSpawn(message);
		CallCmdWorldSpawnConfirm();
		Debug.Log(_ED3E._E000(182551));
	}

	void _E7A7.Respawn()
	{
		Debug.Log(_ED3E._E000(183924));
		CallCmdRespawn();
	}

	void _E7A7.RequestBotDevelopData(string profileId)
	{
		CallCmdDevelopRequestBot(profileId);
	}

	void _E7A7.RequestBotDevelopDataZones()
	{
		CallCmdDevelopRequestBotZones();
	}

	void _E7A7.RequestBotDevelopDataGroups()
	{
		CallCmdDevelopRequestBotGroups();
	}

	void _E7A7.StartGame()
	{
		Debug.Log(_ED3E._E000(183957));
		CallCmdStartGame();
	}

	void _E7A7.StartGameAfterTeleport()
	{
		Debug.Log(_ED3E._E000(183988));
		CallCmdStartGameAfterTeleport();
	}

	void _E7A7.RestartGameInitiate()
	{
		Debug.Log(_ED3E._E000(184008));
		CallCmdRestartGameInitiate();
	}

	void _E7A7.RestartGame()
	{
		Debug.Log(_ED3E._E000(184033));
		CallCmdRestartGame();
	}

	void _E7A7.StopGame()
	{
		Debug.Log(_ED3E._E000(184066));
		CallCmdStopGame();
	}

	void _E7A7.GameStarted()
	{
		Debug.Log(_ED3E._E000(184096));
		CallCmdGameStarted();
	}

	void _E7A7.ReportVoipAbuse()
	{
		CallCmdReportVoipAbuse();
	}

	void _E7A7.PlayerEffectsPause(string playerProfileID, bool isPaused)
	{
		CallCmdPlayerEffectsPause(playerProfileID, isPaused);
	}

	private void _E00F(_EBB3 invokedEvent)
	{
		CallCmdPlayerEffectsPause(invokedEvent.PlayerProfileID, invokedEvent.IsPaused);
	}

	void _E7A7.OnPlayerKeeperStatisticsChanged(string playerProfileID, CounterTag statisticsType, int valueToSet)
	{
		CallCmdOnPlayerKeeperStatisticsChanged(playerProfileID, statisticsType, valueToSet);
	}

	private void _E010(_EBBE invokedEvent)
	{
		CallCmdOnPlayerKeeperStatisticsChanged(invokedEvent.PlayerProfileID, invokedEvent.KeeperStatisticsType, invokedEvent.ValueToSet);
	}

	void _E7A7.CmdGetRadiotransmitterData(string playerProfileID)
	{
		CallCmdGetRadiotransmitterData(playerProfileID);
	}

	void _E7A7.DevelopmentSpawn(SpawnedInstance instance, EPlayerSide side)
	{
		if (base.MemberCategory.Is(EMemberCategory.Developer))
		{
			Debug.Log(_ED3E._E000(184129) + instance);
			if (instance == SpawnedInstance.BotOnServer)
			{
				CallCmdDevelopmentSpawnBotOnServer(side);
			}
			else
			{
				CallCmdDevelopmentSpawnBotRequest(side);
			}
		}
	}

	private GameObject _E011(Vector3 position, NetworkHash128 id)
	{
		Debug.Log(_ED3E._E000(184200));
		return base.gameObject;
	}

	private static void _E012(GameObject sessionObject)
	{
		Debug.Log(_ED3E._E000(184253));
	}

	private void _E013(NetworkConnection connection, NetworkReader reader)
	{
		this.m__E04D.PlayerSpawn(reader, delegate(Result<int> result)
		{
			if (result.Failed)
			{
				Debug.LogErrorFormat(_ED3E._E000(184233), result.Value, result.Error);
			}
			CallCmdSpawnConfirm(result.Value);
		});
	}

	private void _E014(NetworkMessage message)
	{
		this.m__E04D.PlayerUnspawn(message.reader, delegate(Result<int> result)
		{
			if (!this.m__E051.Remove(result.Value) && result.Failed)
			{
				Debug.LogError(result.Error);
			}
		});
	}

	private void _E015(NetworkConnection connection, NetworkReader reader)
	{
		this.m__E04D.ObserverSpawn(reader, delegate(Result<int> result)
		{
			if (result.Failed)
			{
				Debug.LogErrorFormat(_ED3E._E000(184312), result.Value, result.Error);
			}
			CallCmdSpawnConfirm(result.Value);
		});
	}

	private void _E016(NetworkMessage message)
	{
		this.m__E04D.ObserverUnspawn(message.reader, delegate(Result<int> result)
		{
			if (!this.m__E051.Remove(result.Value) && result.Failed)
			{
				Debug.LogError(result.Error);
			}
		});
	}

	protected override void _E045()
	{
		this.m__E04D.GameSpawned();
	}

	protected override void _E046(ushort activitiesCounter, ushort minCounter, int seconds)
	{
		this.m__E04D.GameMatching(activitiesCounter, minCounter, seconds);
	}

	protected override void _E047(int seconds)
	{
		this.m__E04D.GameStarting(seconds);
	}

	protected override void _E048(Vector3 position, int exfiltrationId, string entryPoint)
	{
		this.m__E04D.GameStartingWithTeleport(position, exfiltrationId, entryPoint);
	}

	protected override void _E049(float pastTime, int sessionSeconds)
	{
		this.m__E04D.GameStarted(pastTime, sessionSeconds);
	}

	protected override void _E04A()
	{
		this.m__E04D.GameRestarting();
	}

	protected override void _E04F(byte[] data)
	{
		this.m__E04D.DevelopSetBotData(data);
	}

	protected override void _E050(byte[] data)
	{
		this.m__E04D.DevelopSetBotDataZones(data);
	}

	protected override void _E051(byte[] data)
	{
		this.m__E04D.DevelopSetBotDataGroups(data);
	}

	protected override void _E04B()
	{
		this.m__E04D.GameRestarted();
	}

	protected override void _E04C()
	{
		this.m__E04D.GameStopping();
	}

	protected override void _E04D(ExitStatus exitStatus, int playSeconds)
	{
		this.m__E04D.GameStopped(exitStatus, TimeSpan.FromSeconds(playSeconds));
	}

	protected override void _E04E(long game)
	{
		this.m__E04F = _E5AD.UtcNow;
		this.m__E04D.GameDateTime.Reset(DateTime.FromBinary(game));
	}

	protected override void _E052(EPlayerSide side, int instanceId)
	{
		if (base.MemberCategory.Is(EMemberCategory.Developer))
		{
			this.m__E04E.Add(instanceId);
			CallCmdDevelopmentSpawnBotOnClient(side, instanceId);
		}
	}

	protected override void _E053(int sessionSeconds)
	{
		this.m__E04D.SoftStopNotification(TimeSpan.FromSeconds(sessionSeconds));
	}

	protected override void _E055(string reporterId)
	{
		this.m__E04D.VoipAbuseNotification(reporterId);
	}

	protected override void _E056(byte[] data)
	{
		this.m__E04D.ParseAirdropContainerData(data);
	}

	protected override void _E058(bool canSendAirdrop)
	{
		this.m__E04D.AirdropFlareSuccessEvent(canSendAirdrop);
	}

	protected override void _E057(byte[] data)
	{
		this.m__E04D.ParseMineExplosionData(data);
	}

	protected override void _E059(byte[] data)
	{
		this.m__E04D.ParseBufferZoneData(data);
	}

	protected override void _E05A(_E638 data)
	{
		this.m__E04D.RecieveClientRadioTransmitterData(data);
	}

	protected override void _E05B(_E638 data)
	{
		this.m__E04D.RecieveObserverRadioTransmitterData(data);
	}

	protected override void _E05C(_E634 data)
	{
		this.m__E04D.RecieveLighthouseTraderZoneData(data);
	}

	private void FixedUpdate()
	{
		if (NetworkClient != null)
		{
			RTT = NetworkClient.GetRTT();
			if (NetworkClient.connection != null)
			{
				LossPercent = NetworkTransport.GetOutgoingPacketNetworkLossPercent(NetworkClient.connection.hostId, NetworkClient.connection.connectionId, out var _);
			}
			else
			{
				LossPercent = 0;
			}
		}
	}

	private void Update()
	{
		BEClient.Update();
		this.m__E04A?.ManualUpdate();
		if (this.m__E04D.GameDateTime != null)
		{
			DateTime utcNow = _E5AD.UtcNow;
			if ((utcNow - this.m__E04F > this.m__E050 || utcNow <= this.m__E04F) && HasAuthority)
			{
				this.m__E04F = utcNow;
				CallCmdSyncGameTime();
			}
		}
	}

	protected override void _E054(int disconnectionCode, string additionalInfo, string technicalMessage)
	{
		_E5B9 obj = default(_E5B9);
		obj.DisconnectionCode = (EDisconnectionCode)disconnectionCode;
		obj.AdditionalInfo = additionalInfo;
		obj.OriginalTechnicalMessage = technicalMessage;
		_E5B9 obj2 = obj;
		Debug.Log(obj2);
		this.m__E04D.OnDisconnectStatusAccepted(obj2);
		CallCmdDisconnectAcceptedOnClient();
	}

	protected override void OnDestroy()
	{
		this.m__E04B.Dispose();
		this.m__E04A.Cleanup();
		BEClient.Stop();
		UNetUpdate.OnUpdate -= NetworkIdentity.ManualUnetUpdate;
	}

	[CompilerGenerated]
	private void _E017(Result<int> result)
	{
		if (result.Failed)
		{
			Debug.LogErrorFormat(_ED3E._E000(184233), result.Value, result.Error);
		}
		CallCmdSpawnConfirm(result.Value);
	}

	[CompilerGenerated]
	private void _E018(Result<int> result)
	{
		if (!this.m__E051.Remove(result.Value) && result.Failed)
		{
			Debug.LogError(result.Error);
		}
	}

	[CompilerGenerated]
	private void _E019(Result<int> result)
	{
		if (result.Failed)
		{
			Debug.LogErrorFormat(_ED3E._E000(184312), result.Value, result.Error);
		}
		CallCmdSpawnConfirm(result.Value);
	}

	[CompilerGenerated]
	private void _E01A(Result<int> result)
	{
		if (!this.m__E051.Remove(result.Value) && result.Failed)
		{
			Debug.LogError(result.Error);
		}
	}

	private void _E01B()
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		bool flag = base.OnSerialize(writer, forceAll);
		bool flag2 = default(bool);
		return flag2 || flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		base.OnDeserialize(reader, initialState);
	}

	public override void PreStartClient()
	{
		base.PreStartClient();
	}
}
