using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Comfort.Common;
using CommonAssets.Scripts.Game;
using EFT.AssetsManager;
using EFT.Bots;
using EFT.CameraControl;
using EFT.EnvironmentEffect;
using EFT.Game.Spawning;
using EFT.Interactive;
using EFT.InventoryLogic;
using EFT.UI;
using EFT.UI.Matchmaker;
using EFT.UI.Screens;
using EFT.Weather;
using JsonType;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace EFT;

internal class BaseLocalGame<TPlayerOwner> : AbstractGame, EndByExitTrigerScenario._E001, _E61E, EndByTimerScenario._E000 where TPlayerOwner : GamePlayerOwner
{
	private class _E000
	{
	}

	[CompilerGenerated]
	private sealed class _E001<_E0A8> where _E0A8 : BaseLocalGame<TPlayerOwner>
	{
		public _E7FD inputTree;

		public _ECB1 insurance;

		public _E796 backEndSession;

		public CommonUI commonUI;

		public PreloaderUI preloaderUI;

		public GameUI gameUI;

		public _E0A8 game;

		public _E554.Location location;

		internal TPlayerOwner _E000(Player player)
		{
			return GamePlayerOwner.Create<TPlayerOwner>(player, inputTree, insurance, backEndSession, commonUI, preloaderUI, gameUI, game.GameDateTime, location);
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public _EBCC spawnSystem;

		public BaseLocalGame<TPlayerOwner> _003C_003E4__this;

		public _EAED inventoryController;

		public BotControllerSettings botsSettings;

		public Callback runCallback;

		internal async Task<Player> _E000()
		{
			ISpawnPoint spawnPoint = spawnSystem.SelectSpawnPoint(ESpawnCategory.Player, _003C_003E4__this._E001.Info.Side);
			_003C_003E4__this._E012 = spawnPoint.Infiltration;
			int playerId = ++_003C_003E4__this._E015;
			if (inventoryController == null)
			{
				inventoryController = new _EAED(_003C_003E4__this._E001, examined: true);
			}
			_E941 obj = new _E941(_003C_003E4__this._E001, inventoryController, _003C_003E4__this._E008, fromServer: true);
			obj.Run();
			Player.EUpdateMode armsUpdateMode = Player.EUpdateMode.Auto;
			if (_E2B6.Config.UseHandsFastAnimator)
			{
				armsUpdateMode = Player.EUpdateMode.Manual;
			}
			LocalPlayer obj2 = await _003C_003E4__this._E019(playerId, spawnPoint.Position, spawnPoint.Rotation, _ED3E._E000(60679), "", EPointOfView.FirstPerson, _003C_003E4__this._E001, aiControl: false, _003C_003E4__this.UpdateQueue, armsUpdateMode, Player.EUpdateMode.Auto, _E2B6.Config.CharacterController.ClientPlayerMode, () => Singleton<_E7DE>.Instance.Control.Settings.MouseSensitivity, () => Singleton<_E7DE>.Instance.Control.Settings.MouseAimingSensitivity, new _E75A(), obj);
			obj2.Location = _003C_003E4__this._E002.Id;
			obj2.OnEpInteraction += _003C_003E4__this.OnEpInteraction;
			return obj2;
		}

		internal void _E001()
		{
			_003C_003E4__this._E003(botsSettings, spawnSystem, runCallback);
		}
	}

	[CompilerGenerated]
	private sealed class _E007
	{
		public BaseLocalGame<TPlayerOwner> _003C_003E4__this;

		public ExitStatus exitStatus;

		public string exitName;

		public float delay;

		public Action _003C_003E9__1;

		internal void _E000()
		{
			_EC92 instance = _EC92.Instance;
			if (instance.CheckCurrentScreen(EEftScreenType.Reconnect))
			{
				instance.CloseAllScreensForced();
			}
			_003C_003E4__this._E00E.Player.OnGameSessionEnd(exitStatus, _003C_003E4__this.PastTime, _003C_003E4__this._E002.Id, exitName);
			_003C_003E4__this.CleanUp();
			_003C_003E4__this.Status = GameStatus.Stopped;
			TimeSpan timeSpan = _E5AD.Now - _003C_003E4__this._E005;
			_003C_003E4__this._E008.OfflineRaidEnded(exitStatus, exitName, timeSpan.TotalSeconds).HandleExceptions();
			StaticManager.Instance.WaitSeconds(delay, delegate
			{
				_003C_003E4__this._E009(new Result<ExitStatus, TimeSpan, _E907>(exitStatus, _E5AD.Now - _003C_003E4__this._E005, new _E907()));
				UIEventSystem.Instance.Enable();
			});
		}

		internal void _E001()
		{
			_003C_003E4__this._E009(new Result<ExitStatus, TimeSpan, _E907>(exitStatus, _E5AD.Now - _003C_003E4__this._E005, new _E907()));
			UIEventSystem.Instance.Enable();
		}
	}

	private bool m__E003;

	[CompilerGenerated]
	private _E629 m__E004;

	protected DateTime _E005;

	private _E629 m__E006;

	private GameUI m__E007;

	protected _E796 _E008;

	private Callback<ExitStatus, TimeSpan, _E907> m__E009;

	private EndByExitTrigerScenario m__E00A;

	private EndByTimerScenario m__E00B;

	private Func<Task<Player>> m__E00C;

	private Func<Player, TPlayerOwner> m__E00D;

	protected TPlayerOwner _E00E;

	private Action m__E00F;

	private DateTime m__E010;

	private EDateTime m__E011;

	private string m__E012;

	protected readonly Dictionary<string, Player> _E013 = new Dictionary<string, Player>();

	protected _E626 _E014;

	private int _E015;

	private readonly Dictionary<string, DateTime> _E016 = new Dictionary<string, DateTime>
	{
		{
			_ED3E._E000(124512),
			new DateTime(2016, 8, 4, 15, 28, 0, DateTimeKind.Utc)
		},
		{
			_ED3E._E000(124565),
			new DateTime(2016, 8, 4, 3, 28, 0, DateTimeKind.Utc)
		}
	};

	protected readonly _E620 _E017 = new _E620();

	[CompilerGenerated]
	private MenuUI m__E018;

	[CompilerGenerated]
	private Profile m__E019;

	[CompilerGenerated]
	private _E554.Location m__E01A;

	[CompilerGenerated]
	private Action _E01B;

	public _E629 GameDateTime
	{
		[CompilerGenerated]
		get
		{
			return this._E004;
		}
		[CompilerGenerated]
		private set
		{
			this._E004 = value;
		}
	}

	public TPlayerOwner PlayerOwner => this._E00E;

	protected MenuUI _E000
	{
		[CompilerGenerated]
		get
		{
			return this._E018;
		}
		[CompilerGenerated]
		private set
		{
			this._E018 = value;
		}
	}

	protected Profile _E001
	{
		[CompilerGenerated]
		get
		{
			return this._E019;
		}
		[CompilerGenerated]
		private set
		{
			this._E019 = value;
		}
	}

	protected _E554.Location _E002
	{
		[CompilerGenerated]
		get
		{
			return this._E01A;
		}
		[CompilerGenerated]
		private set
		{
			this._E01A = value;
		}
	}

	public override string LocationObjectId => this._E002._Id;

	protected override GameUI GameUi => this._E007;

	protected override string ProfileId => this._E001.Id;

	public List<Player> AllPlayers
	{
		get
		{
			if ((UnityEngine.Object)this._E00E != (UnityEngine.Object)null && this._E00E.Player != null)
			{
				return new List<Player> { this._E00E.Player };
			}
			return new List<Player>();
		}
	}

	public event Action UpdateByUnity
	{
		[CompilerGenerated]
		add
		{
			Action action = _E01B;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E01B, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E01B;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E01B, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	protected static _E0A8 _E000<_E0A8>(_E7FD inputTree, Profile profile, _E629 backendDateTime, _ECB1 insurance, MenuUI menuUI, CommonUI commonUI, PreloaderUI preloaderUI, GameUI gameUI, _E554.Location location, TimeAndWeatherSettings timeAndWeather, WavesSettings wavesSettings, EDateTime dateTime, Callback<ExitStatus, TimeSpan, _E907> callback, float fixedDeltaTime, EUpdateQueue updateQueue, _E796 backEndSession, TimeSpan? sessionTime) where _E0A8 : BaseLocalGame<TPlayerOwner>
	{
		float num = 1.5f;
		WildSpawnWave[] waves = location.waves;
		foreach (WildSpawnWave obj in waves)
		{
			obj.slots_min = (int)((float)obj.slots_min * num);
			obj.slots_max = (int)((float)obj.slots_max * num);
		}
		_E0A8 game = AbstractGame.Create<_E0A8>(updateQueue, sessionTime, _ED3E._E000(182066));
		((BaseLocalGame<TPlayerOwner>)game)._E003 = _E2B6.Config.UseSpiritPlayer;
		_E2B6.Config.UseSpiritPlayer = false;
		((BaseLocalGame<TPlayerOwner>)game)._E000 = menuUI;
		((BaseLocalGame<TPlayerOwner>)game)._E008 = backEndSession;
		((BaseLocalGame<TPlayerOwner>)game)._E007 = gameUI;
		((BaseLocalGame<TPlayerOwner>)game)._E009 = callback;
		((BaseLocalGame<TPlayerOwner>)game)._E001 = profile;
		((BaseLocalGame<TPlayerOwner>)game)._E002 = location;
		((BaseLocalGame<TPlayerOwner>)game)._E011 = dateTime;
		game.FixedDeltaTime = fixedDeltaTime;
		if (location.NewSpawn)
		{
			location.OldSpawn = true;
			location.NewSpawn = false;
		}
		if (!Singleton<_E307>.Instantiated)
		{
			Singleton<_E307>.Create(new _E307());
		}
		((BaseLocalGame<TPlayerOwner>)game)._E00A = EndByExitTrigerScenario.Create(game);
		((BaseLocalGame<TPlayerOwner>)game)._E00B = EndByTimerScenario._E000(game);
		((BaseLocalGame<TPlayerOwner>)game)._E006 = backendDateTime;
		game._E001(timeAndWeather);
		((BaseLocalGame<TPlayerOwner>)game)._E00D = (Player player) => GamePlayerOwner.Create<TPlayerOwner>(player, inputTree, insurance, backEndSession, commonUI, preloaderUI, gameUI, game.GameDateTime, location);
		commonUI.MenuScreen.OwnerOnLeaveGameEvent = game._E01A;
		WorldInteractiveObject.InteractionShouldBeConfirmed = false;
		game._E017();
		return game;
	}

	private void Update()
	{
		_E01B?.Invoke();
	}

	protected virtual void _E017()
	{
	}

	private void _E001(TimeAndWeatherSettings timeAndWeather)
	{
		System.Random random = new System.Random();
		if (timeAndWeather.IsRandomTime)
		{
			this._E010 = new DateTime(2016, 4, 30, random.Next(1, 24), random.Next(1, 59), 0, DateTimeKind.Utc);
		}
		else if (!_E016.TryGetValue(this._E002.Id, out this._E010))
		{
			this._E010 = ((this._E011 == EDateTime.CURR) ? this._E006.Calculate() : this._E006.Calculate().AddHours(12.0));
		}
		GameDateTime = new _E629(this._E006._E009, this._E010, this._E006.TimeFactor, this._E006._E008);
		if (!Singleton<GameWorld>.Instantiated)
		{
			return;
		}
		Singleton<GameWorld>.Instance.GameDateTime = GameDateTime;
		if (WeatherController.Instance != null)
		{
			TOD_Sky.Instance.Components.Time.GameDateTime = GameDateTime;
			_E8EB[] randomTestWeatherNodes = _E8EB.GetRandomTestWeatherNodes();
			if (!timeAndWeather.IsRandomWeather)
			{
				long time = randomTestWeatherNodes[0].Time;
				randomTestWeatherNodes[0] = this._E008.Weather;
				randomTestWeatherNodes[0].Time = time;
			}
			WeatherController.Instance._E000(randomTestWeatherNodes);
		}
	}

	internal async Task _E002(BotControllerSettings botsSettings, string backendUrl, _EAED inventoryController, Callback runCallback)
	{
		base.Status = GameStatus.Running;
		UnityEngine.Random.InitState((int)_E5AD.Now.Ticks);
		_E554.Location location;
		if (this._E002.IsHideout)
		{
			location = this._E002;
		}
		else
		{
			using (_E069.StartWithToken(_ED3E._E000(182271)))
			{
				int variantId = UnityEngine.Random.Range(1, 6);
				_E004(backendUrl, this._E002.Id, variantId);
				location = await this._E008.LoadLocationLoot(this._E002.Id, variantId);
			}
		}
		_EBD2 spawnPoints = _EBD2.CreateFromScene(_E5AD.LocalDateTimeFromUnixTime(location.UnixDateTime), location.SpawnPointParams);
		int spawnSafeDistance = ((location.SpawnSafeDistanceMeters > 0) ? location.SpawnSafeDistanceMeters : 100);
		_EBD6 settings = new _EBD6(location.MinDistToFreePoint, location.MaxDistToFreePoint, location.MaxBotPerZone, spawnSafeDistance);
		_EBCC spawnSystem = _EBD5.CreateSpawnSystem(settings, () => Time.time, Singleton<GameWorld>.Instance, this._E017, spawnPoints);
		_E3B7 config = _E2B6.Config;
		if (config.FixedFrameRate > 0f)
		{
			base.FixedDeltaTime = 1f / config.FixedFrameRate;
		}
		this._E00C = async delegate
		{
			ISpawnPoint spawnPoint = spawnSystem.SelectSpawnPoint(ESpawnCategory.Player, this._E001.Info.Side);
			this._E012 = spawnPoint.Infiltration;
			int playerId = ++_E015;
			if (inventoryController == null)
			{
				inventoryController = new _EAED(this._E001, examined: true);
			}
			_E941 obj2 = new _E941(this._E001, inventoryController, this._E008, fromServer: true);
			obj2.Run();
			Player.EUpdateMode armsUpdateMode = Player.EUpdateMode.Auto;
			if (_E2B6.Config.UseHandsFastAnimator)
			{
				armsUpdateMode = Player.EUpdateMode.Manual;
			}
			LocalPlayer obj3 = await _E019(playerId, spawnPoint.Position, spawnPoint.Rotation, _ED3E._E000(60679), "", EPointOfView.FirstPerson, this._E001, aiControl: false, base.UpdateQueue, armsUpdateMode, Player.EUpdateMode.Auto, _E2B6.Config.CharacterController.ClientPlayerMode, () => Singleton<_E7DE>.Instance.Control.Settings.MouseSensitivity, () => Singleton<_E7DE>.Instance.Control.Settings.MouseAimingSensitivity, new _E75A(), obj2);
			obj3.Location = this._E002.Id;
			obj3.OnEpInteraction += base.OnEpInteraction;
			return obj3;
		};
		using (_E069.StartWithToken(_ED3E._E000(182252)))
		{
			Player player = await this._E00C();
			_E013.Add(player.ProfileId, player);
			this._E00E = this._E00D(player);
			PlayerCameraController.Create(this._E00E.Player);
			_E8A8.Instance.SetOcclusionCullingEnabled(this._E002.OcculsionCullingEnabled);
			_E8A8.Instance.IsActive = false;
		}
		await _E00B(location, delegate
		{
			_E003(botsSettings, spawnSystem, runCallback);
		});
	}

	private void _E003(BotControllerSettings botsSettings, _EBCC spawnSystem, Callback runCallback)
	{
		int timeBeforeDeployLocal = Singleton<_E5CB>.Instance.TimeBeforeDeployLocal;
		_E3AB.RunHeapPreAllocation();
		_E3AB.Collect(force: true);
		if (_E3AB.Settings.OverrideRamCleanerSettings ? _E3AB.Settings.RamCleanerEnabled : ((bool)Singleton<_E7DE>.Instance.Game.Settings.AutoEmptyWorkingSet))
		{
			_E3AB.EmptyWorkingSet();
		}
		_E3AB.GCEnabled = false;
		Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
		_E018(timeBeforeDeployLocal);
		_E8A8.Instance.IsActive = true;
		StartCoroutine(_E01C(timeBeforeDeployLocal, botsSettings, spawnSystem, runCallback));
	}

	protected virtual void _E018(float timeBeforeDeploy)
	{
		new MatchmakerFinalCountdown._E000(this._E001, _E5AD.Now.AddSeconds(timeBeforeDeploy)).ShowScreen(EScreenState.Root);
		Singleton<GUISounds>.Instance._E004();
		Singleton<BetterAudio>.Instance.TweenAmbientVolume(0f, timeBeforeDeploy);
		this._E007.gameObject.SetActive(value: true);
		this._E007.TimerPanel.ProfileId = ProfileId;
	}

	protected virtual async Task<LocalPlayer> _E019(int playerId, Vector3 position, Quaternion rotation, string layerName, string prefix, EPointOfView pointOfView, Profile profile, bool aiControl, EUpdateQueue updateQueue, Player.EUpdateMode armsUpdateMode, Player.EUpdateMode bodyUpdateMode, CharacterControllerSpawner.Mode characterControllerMode, Func<float> getSensitivity, Func<float> getAimingSensitivity, _E759 statisticsManager, _E935 questController)
	{
		profile.SetSpawnedInSession(value: false);
		return await LocalPlayer.Create(playerId, position, rotation, _ED3E._E000(60679), "", EPointOfView.FirstPerson, profile, aiControl: false, base.UpdateQueue, armsUpdateMode, Player.EUpdateMode.Auto, _E2B6.Config.CharacterController.ClientPlayerMode, () => Singleton<_E7DE>.Instance.Control.Settings.MouseSensitivity, () => Singleton<_E7DE>.Instance.Control.Settings.MouseAimingSensitivity, new _E75A(), new _E610(), questController, isYourPlayer: true);
	}

	private void _E004(string backendUrl, string locationId, int variantId)
	{
		_E2C3 cache = this._E008.Cache;
		if (cache == null)
		{
			return;
		}
		string text = backendUrl + string.Format(_ED3E._E000(182063), locationId, variantId);
		if (cache.Exists(text))
		{
			return;
		}
		string path = _ED3E._E000(182130) + this._E002.Id + variantId;
		_E554._E000 obj;
		try
		{
			obj = _E3A2.Load<TextAsset>(path).text.ParseJsonTo<_E554._E000>(Array.Empty<JsonConverter>());
			if (obj.BackendUrl != backendUrl)
			{
				return;
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
			return;
		}
		_E2C0 obj2 = new _E2C0
		{
			data = obj.Location,
			crc = obj.crc
		};
		try
		{
			string data = obj2.ToPrettyJson();
			cache.Save(text, data);
			UnityEngine.Debug.Log(string.Format(_ED3E._E000(182117), locationId, variantId, obj.crc, text));
		}
		catch (Exception exception2)
		{
			UnityEngine.Debug.LogException(exception2);
		}
	}

	private Task<_E554.Location> _E005(string backendUrl)
	{
		int num = UnityEngine.Random.Range(1, 6);
		return _E00E(_E3A2.Load<TextAsset>(_ED3E._E000(182130) + this._E002.Id + num).text.ParseJsonTo<_E554._E000>(Array.Empty<JsonConverter>()).Location);
	}

	private void _E006(ExfiltrationPoint point, EExfiltrationStatus prevStatus)
	{
		UpdateExfiltrationUi(point, point.Entered.Any((Player x) => x.ProfileId == this._E001.Id));
	}

	protected virtual void _E01A()
	{
		ReconnectionScreen._E000 obj = new ReconnectionScreen._E000(this._E001, this._E002, ESideType.Pmc, returnAllowed: true, nextScreenAllowed: false, this._E008);
		obj.OnLeave += delegate
		{
			Stop(this._E001.Id, ExitStatus.Left, null);
		};
		obj.ShowScreen(EScreenState.Queued);
	}

	protected virtual IEnumerator _E01C(float startDelay, BotControllerSettings controllerSettings, _EBCC spawnSystem, Callback runCallback)
	{
		yield return new WaitForEndOfFrame();
		using (_E069.StartWithToken(_ED3E._E000(182242)))
		{
			_E01C();
		}
		runCallback.Succeed();
	}

	protected virtual void _E01C()
	{
		base.GameTimer.Start();
		_E007();
		if (this._E012 != null)
		{
			this._E001.Info.EntryPoint = this._E012;
			_E57A.Instance.InitAllExfiltrationPoints(this._E002.exits, justLoadSettings: false, this._E002.DisabledScavExits);
			ExfiltrationPoint[] array = _E57A.Instance.EligiblePoints(this._E001);
			this._E007.TimerPanel.SetTime(_E5AD.UtcNow, this._E001.Info.Side, base.GameTimer.SessionSeconds(), array);
			ExfiltrationPoint[] array2 = array;
			foreach (ExfiltrationPoint exfiltrationPoint in array2)
			{
				exfiltrationPoint.OnStatusChanged += _E006;
				UpdateExfiltrationUi(exfiltrationPoint, contains: false, initial: true);
			}
		}
		this._E00A.Run();
		this._E005 = _E5AD.Now;
		base.Status = GameStatus.Started;
		ConsoleScreen.ApplyStartCommands();
	}

	protected void _E007()
	{
		this._E00E.Player.HealthController.DiedEvent += delegate
		{
			// Found self-referencing delegate construction. Abort transformation to avoid stack overflow.
			this._E00E._E023();
			this._E00E.Player.HealthController.DiedEvent -= _E011;
			_E008();
		};
		this._E00E._E022();
	}

	private void _E008()
	{
		this._E007.BattleUiPanelDeath.Show(this._E001, ExitStatus.Killed, _E5AD.Now - this._E005);
		Stop(this._E001.Id, ExitStatus.Killed, null, 5f);
	}

	private static bool _E009<_E077>(ref PlayerLoopSystem system, PlayerLoopSystem replacement)
	{
		if (system.type == typeof(_E077))
		{
			system = replacement;
			return true;
		}
		if (system.subSystemList != null)
		{
			for (int i = 0; i < system.subSystemList.Length; i++)
			{
				if (_E009<_E077>(ref system.subSystemList[i], replacement))
				{
					return true;
				}
			}
		}
		return false;
	}

	private static void _E00A()
	{
	}

	private async Task _E00B(_E554.Location location, Action callback)
	{
		using (_E069.StartWithToken(_ED3E._E000(180245)))
		{
			if (_E2B6.Config.NoLootForLocalGame)
			{
				foreach (_E545 item in location.Loot.Where((_E545 x) => x.Item is _EA68).ToList())
				{
					_EA68 obj = item.Item as _EA68;
					_E9EF[] grids = obj.Grids;
					for (int i = 0; i < grids.Length; i++)
					{
						grids[i].RemoveAll();
					}
					Slot[] slots = obj.Slots;
					for (int i = 0; i < slots.Length; i++)
					{
						slots[i].RemoveItem();
					}
				}
			}
			Item[] source = location.Loot.Select((_E545 x) => x.Item).ToArray();
			ResourceKey[] array = source.OfType<ContainerCollection>().GetAllItemsFromCollections().Concat(source.Where((Item x) => !(x is ContainerCollection)))
				.SelectMany((Item x) => x.Template.AllResources)
				.ToArray();
			if (array.Length != 0)
			{
				PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
				_E2E1.FindParentPlayerLoopSystem(currentPlayerLoop, typeof(EarlyUpdate.UpdateTextureStreamingManager), out var playerLoopSystem, out var index);
				PlayerLoopSystem[] array2 = new PlayerLoopSystem[playerLoopSystem.subSystemList.Length];
				if (index != -1)
				{
					Array.Copy(playerLoopSystem.subSystemList, array2, playerLoopSystem.subSystemList.Length);
					PlayerLoopSystem playerLoopSystem2 = default(PlayerLoopSystem);
					playerLoopSystem2.updateDelegate = _E00A;
					playerLoopSystem2.type = typeof(_E000);
					PlayerLoopSystem playerLoopSystem3 = playerLoopSystem2;
					playerLoopSystem.subSystemList[index] = playerLoopSystem3;
					PlayerLoop.SetPlayerLoop(currentPlayerLoop);
				}
				await Singleton<_E760>.Instance.LoadBundlesAndCreatePools(_E760.PoolsCategory.Raid, _E760.AssemblyType.Local, array, _ECE3.General, new _ECCE<_E5BB>(delegate(_E5BB p)
				{
					SetMatchmakerStatus(_ED3E._E000(182222) + p.Stage, p.Progress);
				}));
				if (index != -1)
				{
					Array.Copy(array2, playerLoopSystem.subSystemList, playerLoopSystem.subSystemList.Length);
					PlayerLoop.SetPlayerLoop(currentPlayerLoop);
				}
			}
			_E548 lootItems = Singleton<GameWorld>.Instance._E004(location.Loot);
			Singleton<GameWorld>.Instance._E005(lootItems, initial: true);
			this._E00E.Player.ManageGameQuests();
		}
		callback();
	}

	void EndByTimerScenario._E000.StopGame()
	{
		UnityEngine.Debug.Log(_ED3E._E000(182178));
		Stop(this._E001.Id, ExitStatus.Survived, null);
	}

	public void ItemPlaced(DroppedItem droppedItem, string profileId)
	{
	}

	public void BotUnspawn(BotOwner botOwner)
	{
		Player getPlayer = botOwner.GetPlayer;
		this._E017.BotDied(botOwner);
		this._E017.DestroyInfo(getPlayer);
		AssetPoolObject.ReturnToPool(botOwner.gameObject);
	}

	public void ItemRemoved(DroppedItem droppedItem)
	{
	}

	void EndByExitTrigerScenario._E001.StopSession(string profileId, ExitStatus exitStatus, string exitName)
	{
		Stop(profileId, exitStatus, exitName);
	}

	protected virtual void Stop(string profileId, ExitStatus exitStatus, string exitName, float delay = 0f)
	{
		if (profileId != this._E001.Id || base.Status == GameStatus.Stopped || base.Status == GameStatus.Stopping)
		{
			return;
		}
		if (base.Status == GameStatus.Starting || base.Status == GameStatus.Started)
		{
			this._E00B._E003 = GameStatus.SoftStopping;
		}
		base.Status = GameStatus.Stopping;
		base.GameTimer.TryStop();
		this._E00A.Stop();
		this._E007.TimerPanel.Close();
		this._E017.Stop();
		this._E017.DestroyInfo(this._E00E.Player);
		if (EnvironmentManager.Instance != null)
		{
			EnvironmentManager.Instance.Stop();
		}
		MonoBehaviourSingleton<PreloaderUI>.Instance.StartBlackScreenShow(1f, 1f, delegate
		{
			_EC92 instance = _EC92.Instance;
			if (instance.CheckCurrentScreen(EEftScreenType.Reconnect))
			{
				instance.CloseAllScreensForced();
			}
			this._E00E.Player.OnGameSessionEnd(exitStatus, base.PastTime, this._E002.Id, exitName);
			CleanUp();
			base.Status = GameStatus.Stopped;
			TimeSpan timeSpan = _E5AD.Now - this._E005;
			this._E008.OfflineRaidEnded(exitStatus, exitName, timeSpan.TotalSeconds).HandleExceptions();
			StaticManager.Instance.WaitSeconds(delay, delegate
			{
				this._E009(new Result<ExitStatus, TimeSpan, _E907>(exitStatus, _E5AD.Now - this._E005, new _E907()));
				UIEventSystem.Instance.Enable();
			});
		});
		_E2B6.Config.UseSpiritPlayer = this._E003;
	}

	public virtual void CleanUp()
	{
		_E00C(_E013);
	}

	protected static void _E00C(IDictionary<string, Player> players)
	{
		foreach (Player value in players.Values)
		{
			try
			{
				value.Dispose();
				AssetPoolObject.ReturnToPool(value.gameObject);
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
		}
		players.Clear();
	}

	protected int _E00D()
	{
		_E015++;
		return _E015;
	}

	[SpecialName]
	GameObject _E61E.get_gameObject()
	{
		return base.gameObject;
	}

	[CompilerGenerated]
	internal static Task<_E554.Location> _E00E(_E391 unparsedData)
	{
		return Task.FromResult(unparsedData.ParseJsonTo<_E554.Location>(Array.Empty<JsonConverter>()));
	}

	[CompilerGenerated]
	private bool _E00F(Player x)
	{
		return x.ProfileId == this._E001.Id;
	}

	[CompilerGenerated]
	private void _E010()
	{
		Stop(this._E001.Id, ExitStatus.Left, null);
	}

	[CompilerGenerated]
	private void _E011(EDamageType controller)
	{
		// Found self-referencing delegate construction. Abort transformation to avoid stack overflow.
		this._E00E._E023();
		this._E00E.Player.HealthController.DiedEvent -= _E011;
		_E008();
	}

	[CompilerGenerated]
	private void _E012(_E5BB p)
	{
		SetMatchmakerStatus(_ED3E._E000(182222) + p.Stage, p.Progress);
	}
}
