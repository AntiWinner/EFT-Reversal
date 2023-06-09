using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Audio.SpatialSystem;
using Comfort.Common;
using Diz.Jobs;
using EFT.Bots;
using EFT.HandBook;
using EFT.Hideout;
using EFT.InputSystem;
using EFT.UI;
using EFT.UI.Matchmaker;
using EFT.UI.Screens;
using FilesChecker;
using JsonType;
using Koenigz.PerfectCulling.EFT;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace EFT;

public sealed class TarkovApplication : CommonClientApplication<_E796>, _E8FD
{
	public sealed class _E000
	{
		[CompilerGenerated]
		private sealed class _E005
		{
			public _E63B itemFactory;

			public TaskCompletionSource<bool> source;

			internal string _E000(string id)
			{
				return itemFactory.GetPresetItem(id).Prefab.path;
			}

			internal void _E001()
			{
				source.SetResult(result: true);
			}
		}

		[CompilerGenerated]
		private sealed class _E007
		{
			public TarkovApplication._E000 _003C_003E4__this;

			public _E554.Location location;

			internal void _E000(IResult error)
			{
				_003C_003E4__this._E008();
			}
		}

		[CompilerGenerated]
		private sealed class _E008
		{
			public Profile savageProfile;

			public _E007 CS_0024_003C_003E8__locals1;

			internal void _E000(Result<ExitStatus, TimeSpan, _E907> result)
			{
				CS_0024_003C_003E8__locals1._003C_003E4__this.m__E000._E02A(CS_0024_003C_003E8__locals1._003C_003E4__this.m__E007.Id, savageProfile, CS_0024_003C_003E8__locals1.location, result);
			}
		}

		private TarkovApplication m__E000;

		private _E554.Location m__E001;

		private Task m__E002;

		private Task m__E003;

		private _E76A._E000 m__E004;

		private bool m__E005;

		private _ED0E<_ED08>._E002 m__E006;

		private Profile m__E007;

		private HideoutGame m__E008;

		[CompilerGenerated]
		private bool m__E009;

		public bool IsHideoutActive
		{
			[CompilerGenerated]
			get
			{
				return this.m__E009;
			}
			[CompilerGenerated]
			private set
			{
				this.m__E009 = value;
			}
		}

		public _E000(TarkovApplication application)
		{
			this.m__E000 = application;
		}

		public async Task HideoutSelectedHandler()
		{
			IsHideoutActive = true;
			if (!this.m__E005)
			{
				await _E000();
			}
			else
			{
				_E009();
			}
			Singleton<GUISounds>.Instance._E006(active: true);
		}

		private async Task _E000()
		{
			_E7A3.InHideOut = true;
			this.m__E001 = this.m__E000.Session.LocationSettings.locations.FirstOrDefault((KeyValuePair<string, _E554.Location> x) => x.Value.IsHideout).Value;
			this.m__E001.Scene = _E785.HideoutScenesPreset;
			using (_E069.StartWithToken(_ED3E._E000(143524)))
			{
				MonoBehaviourSingleton<PreloaderUI>.Instance.LoadingScreen.Show();
			}
			using (_E069.StartWithToken(_ED3E._E000(143614)))
			{
				await this.m__E000.Session.FlushOperationQueue();
			}
			using (_E069.StartWithToken(_ED3E._E000(143597)))
			{
				await _E003();
			}
			if ((await this.m__E000._E023(null, isRaid: false)).Succeed)
			{
				await _E007(this.m__E001, default(TimeAndWeatherSettings));
			}
		}

		public void StartLoadHideoutMap()
		{
			UnityEngine.Debug.Log(_ED3E._E000(143425));
			this.m__E002 = _E001();
			this.m__E002.HandleExceptions();
		}

		private async Task _E001()
		{
			int asyncUploadTimeSlice = QualitySettings.asyncUploadTimeSlice;
			int asyncUploadBufferSize = QualitySettings.asyncUploadBufferSize;
			QualitySettings.asyncUploadTimeSlice = _E2B6.Config.QualitySettingsAsyncUploadTimeSlice;
			QualitySettings.asyncUploadBufferSize = _E2B6.Config.QualitySettingsAsyncUploadBufferSize;
			try
			{
				this.m__E004 = await _E76A.Load(_E785.HideoutScenesPreset);
			}
			finally
			{
				QualitySettings.asyncUploadTimeSlice = asyncUploadTimeSlice;
				QualitySettings.asyncUploadBufferSize = asyncUploadBufferSize;
			}
		}

		private async Task _E002()
		{
			using (_E069.StartWithToken(_ED3E._E000(143644)))
			{
				if (this.m__E002 == null)
				{
					this.m__E002 = _E001();
				}
				await this.m__E002;
			}
			using (_E069.StartWithToken(_ED3E._E000(143622)))
			{
				if (this.m__E003 == null)
				{
					this.m__E003 = _E005(_ECE3.Low);
				}
				await this.m__E003;
			}
			using (_E069.StartWithToken(_ED3E._E000(143656)))
			{
				await _E004();
			}
		}

		private async Task _E003()
		{
			_E725 activeProfileStatus = this.m__E000.Session.ActiveProfileStatus;
			_E725 activePetStatus = this.m__E000.Session.ActivePetStatus;
			string currentProfileId = ((activeProfileStatus != null) ? activeProfileStatus.profileid : activePetStatus.profileid);
			using (_E069.StartWithToken(_ED3E._E000(143688)))
			{
				await _E002();
			}
			using (_E069.StartWithToken(_ED3E._E000(143733)))
			{
				GameWorld gameWorld = GameWorld.Create<HideoutGameWorld>(new GameObject(_ED3E._E000(143718)), Singleton<_E760>.Instance, this.m__E000.PlayerUpdateQueue, currentProfileId);
				try
				{
					await gameWorld.InitLevel(Singleton<_E63B>.Instance, this.m__E000._objectsFactoryConfig, loadBundlesAndCreatePools: false);
				}
				catch (OperationCanceledException)
				{
					gameWorld.Dispose();
				}
				catch (Exception e)
				{
					this.m__E000.Logger.LogException(e);
					gameWorld.Dispose();
				}
				Singleton<GameWorld>.Create(gameWorld);
				Singleton<_E5CE>.Create(gameWorld);
			}
			await Task.Yield();
		}

		private Task _E004()
		{
			IEnumerable<string> source2 = Singleton<_E815>.Instance.ProductionSchemes.Select((_E829 _) => _.endProduct);
			TaskCompletionSource<bool> source = new TaskCompletionSource<bool>();
			_E63B itemFactory = Singleton<_E63B>.Instance;
			IEnumerable<string> keys = from id in source2
				where !string.IsNullOrEmpty(id)
				select itemFactory.GetPresetItem(id).Prefab.path;
			this.m__E006 = Singleton<_ED0A>.Instance.Retain(keys);
			_E612.WaitForAllBundles(this.m__E006, delegate
			{
				source.SetResult(result: true);
			});
			return source.Task;
		}

		public void StartLoadHideoutBundles()
		{
			UnityEngine.Debug.Log(_ED3E._E000(143469));
			this.m__E003 = _E005(_ECE3.Low);
			this.m__E003.HandleExceptions();
		}

		private async Task _E005(_ECE1 yield)
		{
			using (_E069.StartWithToken(_ED3E._E000(143768)))
			{
				_E006();
				ResourceKey[] resources = this.m__E007.GetAllPrefabPaths().Concat(_E5D2.BUNDLES_TO_PRELOAD.Select((string x) => new ResourceKey
				{
					path = x
				})).ToArray();
				_E761 objectsFactoryConfig = this.m__E000._objectsFactoryConfig;
				_E761 config = new _E761(10, objectsFactoryConfig.WeaponsPoolSize, objectsFactoryConfig.MagsPoolSize, objectsFactoryConfig.ItemsPoolSize, 2);
				await Singleton<_E760>.Instance.RegisterLoadBundlesAndCreatePools(_E760.PoolsCategory.Raid, this.m__E000.transform, config, _E760.AssemblyType.Local, resources, yield);
			}
		}

		private void _E006()
		{
			this.m__E007 = this.m__E000.Session.Profile;
		}

		private async Task _E007(_E554.Location location, TimeAndWeatherSettings timeAndWeather)
		{
			using (_E069.StartWithToken(_ED3E._E000(143755)))
			{
				Profile savageProfile = this.m__E000.Session.ProfileOfPet;
				this.m__E008 = HideoutGame._E000(this.m__E000._inputTree, this.m__E007, this.m__E000.m__E002.InventoryController, this.m__E000._localGameDateTime, this.m__E000.Session.InsuranceCompany, MonoBehaviourSingleton<MenuUI>.Instance, MonoBehaviourSingleton<CommonUI>.Instance, MonoBehaviourSingleton<PreloaderUI>.Instance, MonoBehaviourSingleton<GameUI>.Instance, location, timeAndWeather, this.m__E000._raidSettings.WavesSettings, this.m__E000._raidSettings.SelectedDateTime, delegate(Result<ExitStatus, TimeSpan, _E907> result)
				{
					this.m__E000._E02A(this.m__E007.Id, savageProfile, location, result);
				}, this.m__E000._fixedDeltaTime, this.m__E000.PlayerUpdateQueue, this.m__E000.Session, this.m__E000.m__E002.HealthController);
				Singleton<AbstractGame>.Create(this.m__E008);
			}
			await this.m__E008._E002(this.m__E000._raidSettings.BotSettings, this.m__E000._backendUrl, this.m__E000.m__E002.InventoryController, delegate
			{
				_E008();
			});
		}

		private void _E008()
		{
			using (_E069.StartWithToken(_ED3E._E000(143509)))
			{
				_E814.SendAwakeEvents();
			}
			_E009();
			using (_E069.StartWithToken(_ED3E._E000(143489)))
			{
				MonoBehaviourSingleton<PreloaderUI>.Instance.LoadingScreen.LoadComplete();
				MonoBehaviourSingleton<CommonUI>.Instance.HideoutScreenOverlay.OnReturnToHomeScreen -= HideHideout;
				MonoBehaviourSingleton<CommonUI>.Instance.HideoutScreenOverlay.OnReturnToHomeScreen += HideHideout;
			}
			this.m__E005 = true;
		}

		public async Task UnloadHideout()
		{
			UnityEngine.Debug.Log(_ED3E._E000(143789));
			_E7A3.InHideOut = false;
			if (this.m__E002 != null && !this.m__E002.IsCompleted)
			{
				await this.m__E002;
			}
			if (this.m__E003 != null && !this.m__E003.IsCompleted)
			{
				await this.m__E003;
			}
			if ((object)this.m__E008 != null)
			{
				this.m__E008.Stop();
				this.m__E008.CleanUp();
				this.m__E008 = null;
			}
			this.m__E000._E029();
			if (MonoBehaviourSingleton<GameUI>.Instance != null)
			{
				UnityEngine.Object.Destroy(MonoBehaviourSingleton<GameUI>.Instance.gameObject);
			}
			if (this.m__E006 != null)
			{
				this.m__E006.Release();
				this.m__E006 = null;
			}
			if (this.m__E004 != null)
			{
				await _E76A.Unload(this.m__E004);
				this.m__E004 = null;
			}
			this.m__E005 = false;
			this.m__E003 = null;
			this.m__E002 = null;
			IsHideoutActive = false;
		}

		private void _E009()
		{
			AreaData[] areaDatas = Singleton<_E815>.Instance.AreaDatas.Where((AreaData x) => x.Enabled).ToArray();
			HideoutPlayerOwner playerOwner = this.m__E008.PlayerOwner;
			_E796 session = this.m__E000.Session;
			new HideoutScreenRear._E000(areaDatas, playerOwner, session).ShowScreenAsync(EScreenState.Root).HandleExceptions();
		}

		public void HideHideout()
		{
			Singleton<_E815>.Instance.ApplyChanges();
			IsHideoutActive = false;
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public string gameMap;

		internal bool _E000(_E554.Location v)
		{
			return v.Id.ToLower().Contains(gameMap.ToLower());
		}
	}

	[CompilerGenerated]
	private sealed class _E013
	{
		public bool isLeaving;

		public TarkovApplication _003C_003E4__this;

		public bool isPet;

		public bool isMatching;
	}

	[CompilerGenerated]
	private sealed class _E014
	{
		public bool refreshed;

		public _E013 CS_0024_003C_003E8__locals1;

		internal void _E000(Result<_E725[]> _)
		{
			refreshed = true;
			CS_0024_003C_003E8__locals1.isLeaving = CS_0024_003C_003E8__locals1._003C_003E4__this._E014(out CS_0024_003C_003E8__locals1.isPet, CS_0024_003C_003E8__locals1.isMatching);
		}
	}

	[CompilerGenerated]
	private sealed class _E016
	{
		public _E725 profileStatus;

		public TarkovApplication _003C_003E4__this;

		public bool isPet;

		public Profile savageProfile;

		public Action _003C_003E9__7;

		internal bool _E000(_E554.Location loc)
		{
			return loc.Id == profileStatus.location;
		}

		internal async void _E001()
		{
			_003C_003E4__this.m__E002.StopAfkMonitor();
			try
			{
				await _003C_003E4__this._E018(profileStatus.profileid, isPet, savageProfile);
			}
			catch (Exception e)
			{
				_003C_003E4__this._E01C(_ED3E._E000(146607), e);
			}
		}

		internal void _E002()
		{
			_E018 CS_0024_003C_003E8__locals0 = new _E018
			{
				CS_0024_003C_003E8__locals2 = this
			};
			MonoBehaviourSingleton<PreloaderUI>.Instance.StartBlackScreenShow(1f, 1f, delegate
			{
				if (_EC92.Instance.CheckCurrentScreen(EEftScreenType.Reconnect))
				{
					if (MonoBehaviourSingleton<LoginUI>.Instantiated)
					{
						MonoBehaviourSingleton<LoginUI>.Instance.gameObject.SetActive(value: true);
					}
					ProfileLoadingScreen._E000 obj = new ProfileLoadingScreen._E000();
					obj.ShowScreen(EScreenState.Root);
					obj.SetLivingStatus(isLiving: true);
					MonoBehaviourSingleton<PreloaderUI>.Instance.SetLoaderStatus(status: true);
				}
				MonoBehaviourSingleton<PreloaderUI>.Instance.FadeBlackScreen(1f, -1f);
			});
			CS_0024_003C_003E8__locals0.unloadHideout = _003C_003E4__this.HideoutControllerAccess.UnloadHideout();
			_003C_003E4__this.Session.AbortMatching(isPet, async delegate(IResult result)
			{
				try
				{
					await CS_0024_003C_003E8__locals0.unloadHideout;
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogException(exception);
				}
				await CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals2._003C_003E4__this._E015();
				if (MonoBehaviourSingleton<LoginUI>.Instantiated)
				{
					MonoBehaviourSingleton<LoginUI>.Instance.gameObject.SetActive(value: false);
				}
				CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals2._003C_003E4__this._E01D(result, delegate
				{
					CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals2._003C_003E4__this._E02C().HandleExceptions();
				});
			});
			_003C_003E4__this.m__E002.StopAfkMonitor();
		}

		internal void _E003()
		{
			_003C_003E4__this._E02C().HandleExceptions();
		}

		internal async void _E004()
		{
			await _003C_003E4__this.LoadLoginScenes();
		}
	}

	[CompilerGenerated]
	private sealed class _E017
	{
		public ReconnectionScreen._E000 reconnectionScreenController;

		public _E016 CS_0024_003C_003E8__locals1;

		internal void _E000()
		{
			reconnectionScreenController.ShowScreen(EScreenState.Queued);
		}
	}

	[CompilerGenerated]
	private sealed class _E018
	{
		public Task unloadHideout;

		public _E016 CS_0024_003C_003E8__locals2;

		internal async void _E000(IResult result)
		{
			try
			{
				await unloadHideout;
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
			await CS_0024_003C_003E8__locals2._003C_003E4__this._E015();
			if (MonoBehaviourSingleton<LoginUI>.Instantiated)
			{
				MonoBehaviourSingleton<LoginUI>.Instance.gameObject.SetActive(value: false);
			}
			CS_0024_003C_003E8__locals2._003C_003E4__this._E01D(result, delegate
			{
				CS_0024_003C_003E8__locals2._003C_003E4__this._E02C().HandleExceptions();
			});
		}
	}

	[CompilerGenerated]
	private sealed class _E01B
	{
		public TarkovApplication _003C_003E4__this;

		public bool isPet;

		public MatchmakerTimeHasCome._E000 controller;

		public CancellationTokenSource cancellationToken;

		public Callback _003C_003E9__1;

		internal void _E000()
		{
			if (!_003C_003E4__this._raidSettings.Local)
			{
				_003C_003E4__this.Session.AbortMatching(isPet, delegate(IResult result)
				{
					_003C_003E4__this._E01D(result, delegate
					{
					});
				});
			}
			controller.ChangeStatus(_ED3E._E000(146837));
			controller.ChangeCancelButtonVisibility(value: false);
			cancellationToken.Cancel();
		}

		internal void _E001(IResult result)
		{
			_003C_003E4__this._E01D(result, delegate
			{
			});
		}
	}

	[CompilerGenerated]
	private sealed class _E01D
	{
		public TarkovApplication _003C_003E4__this;

		public string matchingName;

		public bool handled;

		internal bool _E000(Exception x)
		{
			if (x != null)
			{
				if (!(x is OperationCanceledException))
				{
					if (x is _E2D4 obj)
					{
						_E2D4 obj2 = obj;
						_003C_003E4__this.Logger.LogInfo(string.Format(_ED3E._E000(146933), matchingName, obj2.Code));
						handled = true;
					}
				}
				else
				{
					_003C_003E4__this.Logger.LogInfo(matchingName + _ED3E._E000(146881));
					handled = true;
				}
			}
			return handled;
		}

		internal void _E001()
		{
			_003C_003E4__this._E02C().HandleExceptions();
		}
	}

	[CompilerGenerated]
	private sealed class _E01E
	{
		public MatchmakerTimeHasCome._E000 screenController;

		public CancellationTokenSource cancellationTokenSource;

		internal void _E000(_E5BB p)
		{
			screenController.ChangeStatus(_ED3E._E000(146969), p.Progress);
		}

		internal bool _E001(Exception x)
		{
			bool num = x is OperationCanceledException;
			if (num)
			{
				cancellationTokenSource.Cancel();
			}
			return num;
		}
	}

	[CompilerGenerated]
	private sealed class _E020
	{
		public CancellationTokenSource cancellationTokenSource;

		internal bool _E000(Exception x)
		{
			bool num = x is OperationCanceledException;
			if (num)
			{
				cancellationTokenSource.Cancel();
			}
			return num;
		}
	}

	[CompilerGenerated]
	private sealed class _E023
	{
		public MatchmakerTimeHasCome._E000 screenController;

		internal void _E000(_E5BB pr)
		{
			screenController.ChangeStatus(pr.Stage.ToString(), pr.Progress);
		}

		internal void _E001(float pr)
		{
			screenController.ChangeStatus(_ED3E._E000(147139), pr);
			if (pr > 0.85f)
			{
				_E782.ReadyToMatching = true;
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E026
	{
		public TarkovApplication _003C_003E4__this;

		public string profileId;

		public Profile savageProfile;

		public _E554.Location location;

		public MatchmakerTimeHasCome._E000 timeHasComeScreenController;

		internal void _E000()
		{
			_003C_003E4__this._E02A(profileId, savageProfile, location, new Result<ExitStatus, TimeSpan, _E907>
			{
				Error = _ED3E._E000(145423)
			}, timeHasComeScreenController);
		}
	}

	[CompilerGenerated]
	private sealed class _E027
	{
		public TaskCanceledException e;

		public _E026 CS_0024_003C_003E8__locals1;

		internal void _E000()
		{
			CS_0024_003C_003E8__locals1._003C_003E4__this._E02A(CS_0024_003C_003E8__locals1.profileId, CS_0024_003C_003E8__locals1.savageProfile, CS_0024_003C_003E8__locals1.location, new Result<ExitStatus, TimeSpan, _E907>
			{
				Error = e.Message
			}, CS_0024_003C_003E8__locals1.timeHasComeScreenController);
		}
	}

	[CompilerGenerated]
	private sealed class _E029
	{
		public string profileId;

		internal bool _E000(_E725 status)
		{
			return status.profileid == profileId;
		}
	}

	[CompilerGenerated]
	private sealed class _E02B
	{
		public TarkovApplication _003C_003E4__this;

		public _E725 profileStatus;

		public Profile savageProfile;

		public _E554.Location location;

		public MatchmakerTimeHasCome._E000 timeHasComeScreenController;

		internal void _E000()
		{
			_003C_003E4__this.Logger.LogDebug(_ED3E._E000(145583));
			_EC92.Instance.CloseAllScreensForced();
			UnityEngine.Object.DestroyImmediate(MonoBehaviourSingleton<MenuUI>.Instance.gameObject);
			_003C_003E4__this.m__E002?.Unsubscribe();
			_003C_003E4__this.BundleLock.MaxConcurrentOperations = 1;
			Singleton<GameWorld>.Instance.OnGameStarted();
		}

		internal void _E001(Result<ExitStatus, TimeSpan, _E907> result)
		{
			_003C_003E4__this._E02A(profileStatus.profileid, savageProfile, location, result, timeHasComeScreenController);
		}

		internal void _E002()
		{
			timeHasComeScreenController.ChangeStatus(_ED3E._E000(145617));
		}
	}

	[CompilerGenerated]
	private sealed class _E02C
	{
		public TarkovApplication _003C_003E4__this;

		public Profile profile;

		public Profile savageProfile;

		public _E554.Location location;

		public MatchmakerTimeHasCome._E000 timeHasComeScreenController;

		internal void _E000(Result<ExitStatus, TimeSpan, _E907> result)
		{
			_003C_003E4__this._E02A(profile.Id, savageProfile, location, result, timeHasComeScreenController);
		}

		internal void _E001(IResult error)
		{
			using (_E069.StartWithToken(_ED3E._E000(143489)))
			{
				UnityEngine.Object.DestroyImmediate(MonoBehaviourSingleton<MenuUI>.Instance.gameObject);
				_003C_003E4__this.m__E002?.Unsubscribe();
				_003C_003E4__this.BundleLock.MaxConcurrentOperations = 1;
				Singleton<GameWorld>.Instance.OnGameStarted();
			}
		}
	}

	[SerializeField]
	private SplashScreenPanel _splashScreenPanel;

	[SerializeField]
	private Camera _temporaryCamera;

	[HideInInspector]
	[SerializeField]
	private _E629 _localGameDateTime;

	[Header("Locations debug settings")]
	public bool UnlockAndShowAllLocations;

	[SerializeField]
	private bool _customRaidSettings;

	[SerializeField]
	private RaidSettings _raidSettings = new RaidSettings
	{
		Side = ESideType.Pmc,
		SelectedDateTime = EDateTime.CURR,
		RaidMode = ERaidMode.Online
	};

	public FastGameInfo FastGameInfo_EXPERIMENTAL = new FastGameInfo();

	private _E815 m__E000;

	private UIInputRoot m__E001;

	private _E781 m__E002;

	private string m__E003;

	private bool m__E004 = true;

	private Task m__E005;

	[CompilerGenerated]
	private int m__E006;

	[CompilerGenerated]
	private int m__E007;

	[CompilerGenerated]
	private _E000 m__E008;

	[CompilerGenerated]
	private Action m__E009;

	private static readonly WaitForSeconds m__E00A = new WaitForSeconds(1f);

	private Coroutine m__E00B;

	public int CurrentRaidNum
	{
		[CompilerGenerated]
		get
		{
			return this.m__E006;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E006 = value;
		}
	}

	public int CurrentTotalRaidNum
	{
		[CompilerGenerated]
		get
		{
			return this.m__E007;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E007 = value;
		}
	}

	public _E000 HideoutControllerAccess
	{
		[CompilerGenerated]
		get
		{
			return this.m__E008;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E008 = value;
		}
	}

	public event Action AfterApplicationLoaded
	{
		[CompilerGenerated]
		add
		{
			Action action = this.m__E009;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E009, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = this.m__E009;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E009, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	protected override bool ConfigureApplication()
	{
		bool flag = base.ConfigureApplication();
		_E3B7 config = _E2B6.Config;
		if (flag)
		{
			_raidSettings.RaidMode = config.RaidMode;
		}
		if (!_raidSettings.Local)
		{
			return flag;
		}
		if (!string.IsNullOrEmpty(_localGameDate) || !string.IsNullOrEmpty(_localGameTime) || Math.Abs(_localGameTimeFactor - 1f) > 0.001f)
		{
			_localGameDateTime = _E629._E000(_E5AD.UtcNow, ref _localGameDate, ref _localGameTime, ref _localGameTimeFactor, debug: true);
		}
		return flag;
	}

	protected override async Task Start()
	{
		await base.Start();
		_EC92.Instance.InitChatScreen(delegate
		{
			_E000(on: true);
		});
	}

	protected override void Update()
	{
		this.m__E000?.Update(Time.deltaTime);
		base.Update();
	}

	protected override void LateUpdate()
	{
		base.Session?.TrySendCommands();
		base.LateUpdate();
	}

	protected override void DestroyApplication()
	{
		base.Session?.FlushOperationQueue();
		base.DestroyApplication();
	}

	protected override async Task LoadLoginScenes()
	{
		using (_E069.StartWithToken(_ED3E._E000(143779)))
		{
			await _E5DB.Manager.LoadScene(_E785.EnvironmentUIScene);
			UnityEngine.Object.DontDestroyOnLoad(MonoBehaviourSingleton<EnvironmentUI>.Instance.gameObject);
			await _E5DB.Manager.LoadScene(_E785.LoginUIScene, LoadSceneMode.Additive);
			UnityEngine.Object.DontDestroyOnLoad(MonoBehaviourSingleton<LoginUI>.Instance.gameObject);
			await _E5DB.Manager.LoadScene(_E785.PreloaderUIScene, LoadSceneMode.Additive);
			_splashScreenPanel.Hide();
			UnityEngine.Object.DestroyImmediate(_temporaryCamera);
		}
	}

	protected override Task BundleCheck(string bundlePath)
	{
		return DefaultBundleCheck(bundlePath);
	}

	public async Task InternalStartGame(string gameMap, bool isLocalGame, bool isBotEnabled)
	{
		_raidSettings.SelectedLocation = base.Session.LocationSettings.locations.Values.FirstOrDefault((_E554.Location v) => v.Id.ToLower().Contains(gameMap.ToLower()));
		if (_raidSettings.SelectedLocation == null)
		{
			throw new ArgumentException(_ED3E._E000(143827) + gameMap + _ED3E._E000(135679));
		}
		_raidSettings.RaidMode = (isLocalGame ? ERaidMode.Local : ERaidMode.Online);
		_raidSettings.BotSettings = new BotControllerSettings(isBotEnabled, isScavWars: false);
		if (this.m__E002 != null)
		{
			this.m__E002.IsInSession = true;
		}
		TimeAndWeatherSettings timeAndWeather = default(TimeAndWeatherSettings);
		try
		{
			Singleton<JobScheduler>.Instance.SetForceMode(enable: true);
			CurrentTotalRaidNum++;
			await (_raidSettings.Local ? _E01E(timeAndWeather) : _E01F(string.Empty, EMatchingType.Single));
		}
		catch (Exception e)
		{
			_E01C(_raidSettings.Local ? _ED3E._E000(143859) : _ED3E._E000(143813), e);
		}
		Singleton<JobScheduler>.Instance.SetForceMode(enable: false);
	}

	private void _E000(bool on)
	{
		this.m__E002.ShowScreen(EMenuType.Chat, on);
	}

	private async Task _E001()
	{
		_E77C obj = new _E77C();
		await obj.RunValidation(Logger.Log, Logger.IsEnabled);
		if (!obj.Succeed)
		{
			Logger.LogError(_ED3E._E000(143903), obj.Error);
			MonoBehaviourSingleton<PreloaderUI>.Instance.ShowErrorScreen(_buildVersion, obj.Error.Localized(), delegate
			{
				MonoBehaviourSingleton<PreloaderUI>.Instance.CloseErrorScreen();
				Application.Quit();
			});
		}
	}

	protected override void Init(_EC35 assetsManager, InputTree inputTree)
	{
		base.Init(assetsManager, inputTree);
		this.m__E001 = base.gameObject.AddComponent<UIInputRoot>();
		this.m__E001.enabled = true;
		_inputTree.Add(this.m__E001);
	}

	protected override async Task CreateBackend()
	{
		try
		{
			this.m__E001._E000(MonoBehaviourSingleton<PreloaderUI>.Instance.ErrorScreenInputNode);
			this.m__E001._E000(MonoBehaviourSingleton<PreloaderUI>.Instance.Console);
			string sessionId = _commandLineArgs.SessionId;
			bool developMode = _developMode || _E2B6.Config.DevelopMode;
			using (_E069.StartWithToken(_ED3E._E000(143923)))
			{
				ClientBackEnd = _E002(developMode, sessionId);
				await ClientBackEnd.RegenerateToken();
			}
			using (_E069.StartWithToken(_ED3E._E000(143905)))
			{
				await _E77F.LoadMainMenuLocale(ClientBackEnd);
			}
			using (_E069.StartWithToken(_ED3E._E000(143948)))
			{
				await ClientBackEnd.ValidateVersion();
			}
			using (_E069.StartWithToken(_ED3E._E000(143988)))
			{
				await _E003();
			}
			if (string.IsNullOrEmpty(sessionId))
			{
				await _E004();
			}
			else
			{
				using (_E069.StartWithToken(_ED3E._E000(143971)))
				{
					new ProfileLoadingScreen._E000().ShowScreen(EScreenState.Root);
					await ClientBackEnd.CreateClientSession();
				}
			}
		}
		catch (_E2D4 obj2)
		{
			if (obj2.Code != 232)
			{
				_E006(obj2);
			}
			return;
		}
		await _E00A();
	}

	private _E3C3<_E796> _E002(bool developMode, string sessionId)
	{
		return _E791._E000(_E5E9.Create(_ED3E._E000(149342), _version, _ED3E._E000(149325), _ED3E._E000(149327)), _backendUrl, _E2B6.BackendCacheDir, null, developMode, sessionId, _E790.HandleError);
	}

	private async Task _E003()
	{
		_E7AD obj = _E7AD._E010;
		obj.Init(await ClientBackEnd.GetAvailableLanguages());
	}

	private Task _E004()
	{
		throw new _E2D4(201, _ED3E._E000(145107));
	}

	private async Task _E005()
	{
		using (_E069.StartWithToken(_ED3E._E000(144007)))
		{
			Login login = _E77A._E002();
			_E780 obj = _E780.Execute(ClientBackEnd, MonoBehaviourSingleton<LoginUI>.Instance, login);
			await obj;
			if (obj.Failed)
			{
				_E006(new _E2D4(0, obj.Error), _ED3E._E000(144045));
				return;
			}
			_E77A._E003(login);
		}
	}

	private void _E006(_E2D4 ex, string debilniiOverride = null)
	{
		string id = (string.IsNullOrEmpty(debilniiOverride) ? ex.Code.ToString() : debilniiOverride);
		MonoBehaviourSingleton<PreloaderUI>.Instance.ShowCriticalErrorScreen(id.Localized(), ex.Message.Localized(), ErrorScreen.EButtonType.QuitButton, 30f, delegate
		{
			Application.Quit();
		}, delegate
		{
			Application.Quit();
		});
	}

	private async Task _E007()
	{
		_EC92.Instance.CloseAllScreensForced();
		Task unloadHideoutTask = HideoutControllerAccess.UnloadHideout();
		await ClientBackEnd.Logout();
		await _E008(unloadHideoutTask);
	}

	private async Task _E008(Task unloadHideoutTask)
	{
		_EC92.Instance.CloseAllScreensForced();
		if (MonoBehaviourSingleton<CommonUI>.Instantiated)
		{
			MonoBehaviourSingleton<CommonUI>.Instance.ChatScreen.Close();
			UnityEngine.Object.DestroyImmediate(MonoBehaviourSingleton<CommonUI>.Instance.gameObject);
		}
		if (MonoBehaviourSingleton<PreloaderUI>.Instantiated)
		{
			UnityEngine.Object.DestroyImmediate(MonoBehaviourSingleton<PreloaderUI>.Instance.gameObject);
		}
		if (MonoBehaviourSingleton<MenuUI>.Instantiated)
		{
			UnityEngine.Object.DestroyImmediate(MonoBehaviourSingleton<MenuUI>.Instance.gameObject);
		}
		_E00D();
		if (Singleton<BonusController>.Instantiated)
		{
			Singleton<BonusController>.Release(null);
		}
		_E79D.AllMembers.Clear();
		_E79D.AllMessages.Clear();
		await unloadHideoutTask;
		await _E5DB.Manager.LoadScene(_E785.PreloaderUIScene);
		_E009();
	}

	private async void _E009()
	{
		MonoBehaviourSingleton<LoginUI>.Instance.gameObject.SetActive(value: true);
		await _E005();
		await _E00A();
	}

	private async Task _E00A()
	{
		Task task = _E00B();
		await task;
		task.HandleExceptions();
	}

	private async Task _E00B()
	{
		if (!MonoBehaviourSingleton<PreloaderUI>.Instantiated)
		{
			_EC36 obj = _E5DB.Manager.LoadScene(_E785.PreloaderUIScene);
			await obj;
			if (obj.Failed)
			{
				_E030(_ED3E._E000(144034), obj.Error);
				return;
			}
		}
		UnityEngine.Object.DontDestroyOnLoad(MonoBehaviourSingleton<PreloaderUI>.Instance.gameObject);
		MenuTaskBar menuTaskBar = MonoBehaviourSingleton<PreloaderUI>.Instance.MenuTaskBar;
		menuTaskBar._E000 += delegate(EMenuType menuType, bool isOn)
		{
			this.m__E002.ShowScreen(menuType, isOn);
		};
		this.m__E001._E000(menuTaskBar);
		_EC36 obj2 = _E5DB.Manager.LoadScene(_E785.CommonUIScene);
		await obj2;
		if (obj2.Failed)
		{
			_E030(_ED3E._E000(144086), obj2.Error);
			return;
		}
		UnityEngine.Object.DontDestroyOnLoad(MonoBehaviourSingleton<CommonUI>.Instance.gameObject);
		this.m__E001._E000(MonoBehaviourSingleton<CommonUI>.Instance.MenuScreen);
		this.m__E001._E000(MonoBehaviourSingleton<CommonUI>.Instance.ReconnectionScreen);
		this.m__E001._E000(MonoBehaviourSingleton<CommonUI>.Instance.InventoryScreen);
		this.m__E001._E000(MonoBehaviourSingleton<CommonUI>.Instance.ScavengerInventoryScreen);
		this.m__E001._E000(MonoBehaviourSingleton<CommonUI>.Instance.HideoutScreenRear);
		this.m__E001._E000(MonoBehaviourSingleton<CommonUI>.Instance.HideoutScreenOverlay);
		this.m__E001._E000(MonoBehaviourSingleton<CommonUI>.Instance.TraderDialogScreen);
		this.m__E001._E000(MonoBehaviourSingleton<CommonUI>.Instance.WeaponModdingScreen);
		this.m__E001._E000(MonoBehaviourSingleton<CommonUI>.Instance.EditBuildScreen);
		this.m__E001._E000(MonoBehaviourSingleton<CommonUI>.Instance.SettingsScreen);
		this.m__E001._E000(MonoBehaviourSingleton<CommonUI>.Instance.HandbookScreen);
		this.m__E001._E000(MonoBehaviourSingleton<CommonUI>.Instance.TransferItemsScreen);
		obj2 = _E5DB.Manager.LoadScene(_E785.MenuUIScene, LoadSceneMode.Additive);
		await obj2;
		if (obj2.Failed)
		{
			_E030(_ED3E._E000(144079), obj2.Error);
			return;
		}
		UnityEngine.Object.DontDestroyOnLoad(MonoBehaviourSingleton<MenuUI>.Instance.gameObject);
		this.m__E001._E000(MonoBehaviourSingleton<MenuUI>.Instance.MatchMakerAcceptScreen);
		this.m__E001._E000(MonoBehaviourSingleton<MenuUI>.Instance.MatchMakerSelectionLocationScreen);
		this.m__E001._E000(MonoBehaviourSingleton<MenuUI>.Instance.MatchMakerSideSelectionScreen);
		this.m__E001._E000(MonoBehaviourSingleton<MenuUI>.Instance.MatchmakerFinalCountdown);
		this.m__E001._E000(MonoBehaviourSingleton<MenuUI>.Instance.MatchmakerMapPoints);
		this.m__E001._E000(MonoBehaviourSingleton<MenuUI>.Instance.MatchmakerOfflineRaidScreen);
		this.m__E001._E000(MonoBehaviourSingleton<MenuUI>.Instance.MatchmakerInsuranceScreen);
		this.m__E001._E000(MonoBehaviourSingleton<MenuUI>.Instance.MatchmakerTimeHasCome);
		this.m__E001._E000(MonoBehaviourSingleton<MenuUI>.Instance.MatchmakerKeyAccessScreen);
		this.m__E001.CheckDuplicateChildren();
		MonoBehaviourSingleton<MenuUI>.Instance.OperationQueueIndicator.Show(base.Session);
		_E00F().HandleExceptions();
	}

	private void _E00C()
	{
		_E00D();
		_E857 obj = new _E857();
		Singleton<_E857>.Create(obj);
		obj.Activate(transportType: Singleton<_E7DE>.Instance.Game.Settings.NotificationTransportType.Value, backEndSession: base.Session);
		obj.OnNotificationReceived += _E00E;
		MonoBehaviourSingleton<PreloaderUI>.Instance.NotifierView.Init();
		base.Session.RefreshProfileStatuses(isPet: false);
	}

	private void _E00D()
	{
		if (Singleton<_E857>.Instantiated)
		{
			_E857 instance = Singleton<_E857>.Instance;
			instance.OnNotificationReceived -= _E00E;
			instance.Deactivate();
			Singleton<_E857>.Release(null);
		}
	}

	private void _E00E(_E856 notification)
	{
		if (_E7A3.InRaid)
		{
			return;
		}
		if (notification == null)
		{
			return;
		}
		if (!(notification is _E88B))
		{
			if (!(notification is _E86C obj))
			{
				if (!(notification is _E86E obj2))
				{
					if (!(notification is _E86F obj3))
					{
						if (notification is _E86D)
						{
							_E790.HandleWrongMajorVersion();
						}
					}
					else
					{
						_E790.HandleMessage(obj3.Description);
					}
				}
				else
				{
					_E86E obj4 = obj2;
					base.Session.ChangeProfileStatus(obj4.Status);
				}
			}
			else
			{
				obj.Apply(base.Session);
			}
		}
		else
		{
			ClientBackEnd.Destroy().HandleExceptions();
			_E790.ForceLogout();
		}
	}

	private async Task _E00F()
	{
		try
		{
			using (_E069.StartWithToken(_ED3E._E000(144067)))
			{
				await _E010();
			}
		}
		catch (Exception exception)
		{
			MonoBehaviourSingleton<PreloaderUI>.Instance.ShowErrorScreen(_ED3E._E000(8047), exception, Application.Quit);
		}
	}

	private async Task _E010()
	{
		MonoBehaviourSingleton<PreloaderUI>.Instance.SetLoaderStatus(status: true);
		Singleton<GUISounds>.Instance._E003();
		if (Singleton<_E63E>.Instantiated)
		{
			Singleton<_E63E>.Instance.Clear();
		}
		else
		{
			Singleton<_E63E>.Create(new _E63E());
		}
		_E63B obj;
		using (_E069.StartWithToken(_ED3E._E000(144103)))
		{
			Task<_E63B> task = base.Session.CreateItemFactory(Singleton<_E63E>.Instance, Singleton<_E63B>.Instance);
			HideoutControllerAccess = new _E000(this);
			if (Singleton<_E7DE>.Instantiated && (bool)Singleton<_E7DE>.Instance.Game.Settings.EnableHideoutPreload)
			{
				HideoutControllerAccess.StartLoadHideoutMap();
			}
			obj = await task;
		}
		await base.Session.LoadCustomization();
		base.Session.LastPlayerState = null;
		this.m__E001._E001(MonoBehaviourSingleton<PreloaderUI>.Instance.ErrorScreenInputNode);
		this.m__E001._E001(MonoBehaviourSingleton<PreloaderUI>.Instance.Console);
		this.m__E001._E000(MonoBehaviourSingleton<MenuUI>.Instance.TradingScreen);
		this.m__E001._E000(MonoBehaviourSingleton<MenuUI>.Instance.TradingScreen.TraderScreensGroup);
		this.m__E001._E000(MonoBehaviourSingleton<CommonUI>.Instance.ChatScreen);
		this.m__E001._E000(MonoBehaviourSingleton<PreloaderUI>.Instance.ErrorScreenInputNode);
		this.m__E001._E000(MonoBehaviourSingleton<PreloaderUI>.Instance.Console);
		this.m__E001.CheckDuplicateChildren();
		this.m__E001.enabled = true;
		if (Singleton<_E63B>.Instantiated)
		{
			Singleton<_E63B>.Release(Singleton<_E63B>.Instance);
		}
		Singleton<_E63B>.Create(obj);
		_ED0A instance = Singleton<_ED0A>.Instance;
		if (!Singleton<_E760>.Instantiated)
		{
			using (_E069.StartWithToken(_ED3E._E000(144950)))
			{
				Singleton<_E760>.Create(new _E760(instance, obj.ItemTemplates, this));
			}
		}
		if (!Singleton<_E3E4>.Instantiated)
		{
			using (_E069.StartWithToken(_ED3E._E000(144130)))
			{
				_E3E4 obj4 = new _E3E4(instance, Singleton<_E760>.Instance);
				await obj4.Init();
				Singleton<_E3E4>.Create(obj4);
			}
		}
		if (!Singleton<_E3DE>.Instantiated)
		{
			using (_E069.StartWithToken(_ED3E._E000(144178)))
			{
				_E3DE obj5 = new _E3DE(instance);
				await obj5.Init();
				Singleton<_E3DE>.Create(obj5);
			}
		}
		using (_E069.StartWithToken(_ED3E._E000(144222)))
		{
			_E55C obj6 = await base.Session.GetGlobalConfig();
			base.Session.SetBotSettings(obj6);
			if (Singleton<_E5CB>.Instantiated)
			{
				Singleton<_E5CB>.Release(Singleton<_E5CB>.Instance);
			}
			Singleton<_E5CB>.Create(obj6.Config);
			MonoBehaviourSingleton<EnvironmentUI>.Instance.Events = obj6.Config.EventType;
			obj.SetItemPresets(obj6.ItemPresets.Values.ToArray());
		}
		using (_E069.StartWithToken(_ED3E._E000(144255)))
		{
			_E55D obj7 = await base.Session.GetClientSettingsConfig();
			if (obj7.ClientSettings.TurnOffLogging)
			{
				_E372.DebugLogsEnabled(enabled: false);
			}
			if (Singleton<_E5B7>.Instantiated)
			{
				Singleton<_E5B7>.Release(Singleton<_E5B7>.Instance);
			}
			Singleton<_E5B7>.Create(obj7.ClientSettings);
			base.Session.ConfigureLobbyRelatedParameters(obj7.ClientSettings);
			base.Session.StartKeepAliveCoroutine(obj7.ClientSettings.KeepAliveInterval);
			_E3AB.Init(obj7.ClientSettings.MemoryManagementSettings);
			MonoBehaviourSingleton<PreloaderUI>.Instance.SetNetworkViewSettings(obj7.ClientSettings.NetworkStateView);
			_E7E9.SetFramerateLimits(obj7.ClientSettings.FramerateLimit);
			_E2C7.FirstCycleDelaySeconds = Math.Max(_E2C7.FirstCycleDelaySeconds, obj7.ClientSettings.FirstCycleDelaySeconds);
			_E2C7.SecondCycleDelaySeconds = Math.Max(_E2C7.SecondCycleDelaySeconds, obj7.ClientSettings.SecondCycleDelaySeconds);
			_E2C7.NextCycleDelaySeconds = Math.Max(_E2C7.NextCycleDelaySeconds, obj7.ClientSettings.NextCycleDelaySeconds);
			_E2C7.AdditionalRandomDelaySeconds = Math.Max(_E2C7.AdditionalRandomDelaySeconds, obj7.ClientSettings.AdditionalRandomDelaySeconds);
			_E2C7.DefaultRetries = Math.Max(_E2C7.DefaultRetries, (byte)obj7.ClientSettings.DefaultRetriesCount);
			_E2C7.MaximumRetries = Math.Max(_E2C7.MaximumRetries, (byte)obj7.ClientSettings.CriticalRetriesCount);
			if (obj7.ClientSettings.Mark502and504AsNonImportant)
			{
				_E5E4.RemoveCodeFromImportantErrorsList(EBackendErrorCode.HTTPBadGateway);
				_E5E4.RemoveCodeFromImportantErrorsList(EBackendErrorCode.HTTPServiceUnavailable);
				_E5E4.RemoveCodeFromImportantErrorsList(EBackendErrorCode.HTTPGatewayTimeout);
			}
			else
			{
				_E5E4.AddCodeToImportantErrorsList(EBackendErrorCode.HTTPBadGateway);
				_E5E4.AddCodeToImportantErrorsList(EBackendErrorCode.HTTPServiceUnavailable);
				_E5E4.AddCodeToImportantErrorsList(EBackendErrorCode.HTTPGatewayTimeout);
			}
			ClientBackEnd.EnableDiagnostics(obj7.ClientSettings.WebDiagnosticsEnabled);
		}
		using (_E069.StartWithToken(_ED3E._E000(144272)))
		{
			await base.Session.GetProfiles();
		}
		if (base.Session.AllProfiles.IsNullOrEmpty())
		{
			await _E011();
		}
		await _E77F.SelectProfile(base.Session);
		if (Singleton<_E5B7>.Instance.ShouldEstablishLobbyConnection)
		{
			await base.Session.IssueWSToken();
			await base.Session.EstablishWSConnection();
		}
		if (_E014(out var _))
		{
			using (_E069.StartWithToken(_ED3E._E000(144309)))
			{
				ProfileLoadingScreen._E000 obj8 = _EC92.Instance.CurrentScreenController as ProfileLoadingScreen._E000;
				obj8?.SetLivingStatus(isLiving: true);
				await _E015();
				obj8?.SetLivingStatus(isLiving: false);
				await base.Session.GetProfiles();
			}
		}
		MonoBehaviourSingleton<PreloaderUI>.Instance.CloseErrorScreen();
		if (!string.IsNullOrEmpty(this.m__E003))
		{
			MonoBehaviourSingleton<PreloaderUI>.Instance.ShowErrorScreen(_ED3E._E000(18502), this.m__E003);
			this.m__E003 = null;
		}
		await _E012();
		Singleton<_E7DE>.Instance.Sound.Controller.CheckVoipInitialization();
	}

	private async Task _E011()
	{
		MonoBehaviourSingleton<EnvironmentUI>.Instance.SetAsMain(isMain: true);
		_E762.ResetPrefs();
		_E77E obj = _E77E.Execute(base.Session);
		this.m__E001._E000(MonoBehaviourSingleton<LoginUI>.Instance.WelcomeScreen);
		this.m__E001._E000(MonoBehaviourSingleton<LoginUI>.Instance.SetNicknameScreen);
		this.m__E001._E000(MonoBehaviourSingleton<LoginUI>.Instance.SideSelectionScreen);
		await obj;
		if (obj.Failed)
		{
			Logger.LogError(_ED3E._E000(144296) + obj.Error);
			MonoBehaviourSingleton<PreloaderUI>.Instance.ShowErrorScreen(_buildVersion, obj.Error, Application.Quit);
		}
		await base.Session.GetProfiles();
	}

	private async Task _E012()
	{
		try
		{
			using (_E069.StartWithToken(_ED3E._E000(144336)))
			{
				await _E013();
				_E3AB.Collect(2, GCCollectionMode.Forced, isBlocking: true, compacting: true, force: true);
			}
		}
		catch (Exception ex)
		{
			Logger.LogError(_ED3E._E000(144372) + ex.Message + _ED3E._E000(18502) + ex.StackTrace);
			MonoBehaviourSingleton<PreloaderUI>.Instance.ShowErrorScreen(_buildVersion, ex, delegate
			{
				_E00F().HandleExceptions();
			});
		}
	}

	private async Task _E013()
	{
		if (!Singleton<BonusController>.Instantiated)
		{
			Singleton<BonusController>.Create(new BonusController());
			MonoBehaviourSingleton<PreloaderUI>.Instance.MenuTaskBar.InitBonusController();
		}
		else
		{
			Singleton<BonusController>.Instance.Reset();
		}
		_E629 localGameDateTime;
		using (_E069.StartWithToken(_ED3E._E000(146448)))
		{
			localGameDateTime = await _E77F.Run(base.Session);
		}
		if (_localGameDateTime == null)
		{
			_localGameDateTime = localGameDateTime;
		}
		Task<_EBA6<HandbookData>> task = base.Session.RequestHandbookInfo();
		if (Singleton<_E7DE>.Instantiated && (bool)Singleton<_E7DE>.Instance.Game.Settings.EnableHideoutPreload)
		{
			HideoutControllerAccess.StartLoadHideoutBundles();
		}
		_EBA6<HandbookData> obj2;
		using (_E069.StartWithToken(_ED3E._E000(146521)))
		{
			obj2 = await task;
		}
		Profile profile = base.Session.Profile;
		await Singleton<_E3C1>.Instance.LoadVoice(profile.Info.Voice);
		Singleton<BonusController>.Instance.InitSkillManager(profile.Skills);
		using (_E069.StartWithToken(_ED3E._E000(146554)))
		{
			Singleton<_EBA8>.Create(new _EBA8(obj2.Categories, obj2.Items, base.Session, profile.Encyclopedia, profile.Stats.CarriedQuestItems));
		}
		using (_E069.StartWithToken(_ED3E._E000(146536)))
		{
			if (!Singleton<_E815>.Instantiated)
			{
				this.m__E000 = new _E815();
				Singleton<_E815>.Create(this.m__E000);
			}
			await this.m__E000.Init(base.Session);
			MonoBehaviourSingleton<PreloaderUI>.Instance.MenuTaskBar.InitHideout(this.m__E000);
		}
		using (_E069.StartWithToken(_ED3E._E000(146590)))
		{
			List<_ECAC> builds = await base.Session.RequestBuilds();
			_E63B instance = Singleton<_E63B>.Instance;
			base.Session.CreateWeaponStorage(builds);
			instance.SetWeaponBuildsStorage(base.Session.WeaponBuildsStorage);
		}
		if (MonoBehaviourSingleton<LoginUI>.Instantiated)
		{
			MonoBehaviourSingleton<LoginUI>.Instance.gameObject.SetActive(value: false);
		}
		_E00C();
		_ECBD ragFair = base.Session.RagFair;
		ragFair.UpdateSettings(Singleton<_E5CB>.Instance.RagFair, profile.WishList, profile.RagfairInfo.Offers);
		ragFair.SetMyRating(profile.RagfairInfo.Rating, profile.RagfairInfo.IsRatingGrowing);
		ragFair.AddMyOffers(profile.RagfairInfo.Offers);
		MonoBehaviourSingleton<PreloaderUI>.Instance.InitConsole(profile);
		_E016();
		using (_E069.StartWithToken(_ED3E._E000(146621)))
		{
			await _E001();
		}
	}

	private bool _E014(out bool isPet, bool isMatching = false)
	{
		Profile profileOfPet = base.Session.ProfileOfPet;
		_E725 activePetStatus = base.Session.ActivePetStatus;
		if (profileOfPet != null && activePetStatus != null && (activePetStatus.status == EProfileStatus.Leaving || (isMatching && activePetStatus.status == EProfileStatus.MatchWait)))
		{
			isPet = true;
			return true;
		}
		Profile profile = base.Session.Profile;
		activePetStatus = base.Session.ActiveProfileStatus;
		if (profile != null && activePetStatus != null && (activePetStatus.status == EProfileStatus.Leaving || (isMatching && activePetStatus.status == EProfileStatus.MatchWait)))
		{
			isPet = false;
			return true;
		}
		isPet = false;
		return false;
	}

	private async Task _E015(bool isMatching = false)
	{
		int num = 0;
		bool isPet;
		bool isLeaving = _E014(out isPet, isMatching);
		while (isLeaving)
		{
			float fixedTime = Time.fixedTime;
			bool refreshed = false;
			base.Session.RefreshProfileStatuses(isPet, delegate
			{
				refreshed = true;
				isLeaving = _E014(out isPet, isMatching);
			});
			if (num < 15)
			{
				num += 5;
			}
			else if (num < 60)
			{
				num += 15;
			}
			while (!refreshed || (isLeaving && Time.fixedTime - fixedTime < (float)num))
			{
				await Task.Yield();
			}
		}
	}

	internal void _E016()
	{
		Profile profileOfPet = base.Session.ProfileOfPet;
		_E725 activePetStatus = base.Session.ActivePetStatus;
		if (profileOfPet != null && activePetStatus != null && activePetStatus.status != 0 && activePetStatus.status != EProfileStatus.Transfer)
		{
			_E017(profileOfPet, activePetStatus, isPet: true, profileOfPet).HandleExceptions();
			return;
		}
		Profile profile = base.Session.Profile;
		_E725 activeProfileStatus = base.Session.ActiveProfileStatus;
		if (profile != null && activeProfileStatus != null)
		{
			_E017(profile, activeProfileStatus, isPet: false, profileOfPet).HandleExceptions();
			return;
		}
		throw new NotImplementedException(_ED3E._E000(145145));
	}

	private async Task _E017(Profile profile, _E725 profileStatus, bool isPet, Profile savageProfile)
	{
		if (!_customRaidSettings)
		{
			_raidSettings = new RaidSettings
			{
				Side = ESideType.Pmc,
				SelectedDateTime = EDateTime.CURR,
				RaidMode = ERaidMode.Online,
				TimeAndWeatherSettings = new TimeAndWeatherSettings(randomTime: false, randomWeather: false, 0, 0, 0, 0, 4)
			};
		}
		if (profileStatus.status == EProfileStatus.Leaving)
		{
			UnityEngine.Debug.LogError(_ED3E._E000(146640));
		}
		if (profileStatus.status == EProfileStatus.Free)
		{
			base.Session.SocialNetwork._E001(base.Session, _version);
			string[] availableSuites = await base.Session.GetAvailableSuites();
			Singleton<_E60E>.Instance.SetAvailableSuites(availableSuites);
			_E01A().HandleExceptions();
			return;
		}
		if (_raidSettings.Local)
		{
			throw new NotImplementedException(_ED3E._E000(146660) + profileStatus.status);
		}
		base.Session.SocialNetwork._E001(base.Session, _version);
		string gameMode = profileStatus.gameMode;
		_raidSettings.RaidMode = profileStatus.raidMode;
		_raidSettings.Side = ((profile.Info.Side == EPlayerSide.Savage) ? ESideType.Savage : ESideType.Pmc);
		_raidSettings.SelectedLocation = base.Session.LocationSettings.locations.Values.SingleOrDefault((_E554.Location loc) => loc.Id == profileStatus.location);
		if (_raidSettings.SelectedLocation != null)
		{
			switch (profileStatus.status)
			{
			case EProfileStatus.Busy:
			{
				ReconnectionScreen._E000 reconnectionScreenController = new ReconnectionScreen._E000(profile, _raidSettings.SelectedLocation, _raidSettings.Side, returnAllowed: false, nextScreenAllowed: true, base.Session);
				reconnectionScreenController.OnReconnectAction += async delegate
				{
					this.m__E002.StopAfkMonitor();
					try
					{
						await _E018(profileStatus.profileid, isPet, savageProfile);
					}
					catch (Exception e2)
					{
						_E01C(_ED3E._E000(146607), e2);
					}
				};
				reconnectionScreenController.OnLeave += delegate
				{
					MonoBehaviourSingleton<PreloaderUI>.Instance.StartBlackScreenShow(1f, 1f, delegate
					{
						if (_EC92.Instance.CheckCurrentScreen(EEftScreenType.Reconnect))
						{
							if (MonoBehaviourSingleton<LoginUI>.Instantiated)
							{
								MonoBehaviourSingleton<LoginUI>.Instance.gameObject.SetActive(value: true);
							}
							ProfileLoadingScreen._E000 obj = new ProfileLoadingScreen._E000();
							obj.ShowScreen(EScreenState.Root);
							obj.SetLivingStatus(isLiving: true);
							MonoBehaviourSingleton<PreloaderUI>.Instance.SetLoaderStatus(status: true);
						}
						MonoBehaviourSingleton<PreloaderUI>.Instance.FadeBlackScreen(1f, -1f);
					});
					Task unloadHideout = HideoutControllerAccess.UnloadHideout();
					base.Session.AbortMatching(isPet, async delegate(IResult result)
					{
						try
						{
							await unloadHideout;
						}
						catch (Exception exception)
						{
							UnityEngine.Debug.LogException(exception);
						}
						await _E015();
						if (MonoBehaviourSingleton<LoginUI>.Instantiated)
						{
							MonoBehaviourSingleton<LoginUI>.Instance.gameObject.SetActive(value: false);
						}
						_E01D(result, delegate
						{
							_E02C().HandleExceptions();
						});
					});
					this.m__E002.StopAfkMonitor();
				};
				_E01A(delegate
				{
					reconnectionScreenController.ShowScreen(EScreenState.Queued);
				}).HandleExceptions();
				return;
			}
			case EProfileStatus.MatchWait:
				if (this.m__E002 != null)
				{
					this.m__E002.IsInSession = true;
				}
				try
				{
					Singleton<JobScheduler>.Instance.SetForceMode(enable: true);
					await _E01F(gameMode, EMatchingType.Single);
				}
				catch (Exception e)
				{
					_E01C(_ED3E._E000(146700), e);
				}
				Singleton<JobScheduler>.Instance.SetForceMode(enable: false);
				return;
			}
			Logger.LogError(_ED3E._E000(146689) + profileStatus.status);
		}
		else
		{
			Logger.LogError(_ED3E._E000(146730));
		}
		MonoBehaviourSingleton<PreloaderUI>.Instance.ShowErrorScreen(_buildVersion, _ED3E._E000(146805), async delegate
		{
			await LoadLoginScenes();
		});
	}

	private async Task _E018(string profileId, bool isPet, Profile savageProfile)
	{
		CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
		_E909 obj = new _E909(Logger);
		MatchmakerTimeHasCome._E000 obj2 = _E019(isPet, cancellationTokenSource);
		_E76B scenePreset = new _E76B(_raidSettings.SelectedLocation.Scene, disableServerScenes: true);
		await _E021(scenePreset, cancellationTokenSource.Token, obj2);
		cancellationTokenSource.Token.ThrowIfCancellationRequested();
		obj.SetLocationLoaded();
		if ((await _E023(obj2)).Succeed)
		{
			await _E024(profileId, savageProfile, _raidSettings.SelectedLocation, obj, obj2);
		}
	}

	private MatchmakerTimeHasCome._E000 _E019(bool isPet, CancellationTokenSource cancellationToken)
	{
		MatchmakerTimeHasCome._E000 controller = new MatchmakerTimeHasCome._E000(base.Session, _raidSettings);
		controller.ShowScreen(EScreenState.Root);
		controller.OnAbortMatching += delegate
		{
			if (!_raidSettings.Local)
			{
				base.Session.AbortMatching(isPet, delegate(IResult result)
				{
					_E01D(result, delegate
					{
					});
				});
			}
			controller.ChangeStatus(_ED3E._E000(146837));
			controller.ChangeCancelButtonVisibility(value: false);
			cancellationToken.Cancel();
		};
		return controller;
	}

	private async Task _E01A(Action reconnectAction = null)
	{
		base.Session.RefreshPings(Singleton<_E5B7>.Instance.PingServerResultSendInterval, Singleton<_E5B7>.Instance.PingServersInterval);
		this.m__E002?.Unsubscribe();
		using (_E069.StartWithToken(_ED3E._E000(146817)))
		{
			this.m__E002 = await _E781.Execute(ClientBackEnd, MonoBehaviourSingleton<EnvironmentUI>.Instance, MonoBehaviourSingleton<MenuUI>.Instance, MonoBehaviourSingleton<CommonUI>.Instance, MonoBehaviourSingleton<PreloaderUI>.Instance, _raidSettings, HideoutControllerAccess, delegate
			{
				_E007().HandleExceptions();
			}, reconnectAction);
		}
		_E01B();
		await this.m__E002;
		if (this.m__E002.Failed)
		{
			Logger.LogError(_ED3E._E000(146855) + this.m__E002.Error);
			MonoBehaviourSingleton<PreloaderUI>.Instance.ShowErrorScreen(_buildVersion, this.m__E002.Error, delegate
			{
				_E01A().HandleExceptions();
			});
			return;
		}
		_raidSettings.SelectedDateTime = (_E554.Location.NightTimeAllowedLocations.Contains(_raidSettings.SelectedLocation.Id) ? _raidSettings.SelectedDateTime : EDateTime.CURR);
		this.m__E002.StoreProfile();
		this.m__E002.IsInSession = true;
		try
		{
			Singleton<JobScheduler>.Instance.SetForceMode(enable: true);
			CurrentTotalRaidNum++;
			if (!_raidSettings.Local)
			{
				await _E01F(this.m__E002.GroupId, this.m__E002.MatchingType);
			}
			else
			{
				await _E01E(_raidSettings.TimeAndWeatherSettings);
			}
		}
		catch (Exception e)
		{
			_E01C(_raidSettings.Local ? _ED3E._E000(143859) : _ED3E._E000(143813), e);
		}
		Singleton<JobScheduler>.Instance.SetForceMode(enable: false);
	}

	private void _E01B()
	{
		this.m__E009?.Invoke();
		this.m__E004 = false;
	}

	private void _E01C(string matchingName, Exception e)
	{
		_E01D CS_0024_003C_003E8__locals0 = new _E01D();
		CS_0024_003C_003E8__locals0._003C_003E4__this = this;
		CS_0024_003C_003E8__locals0.matchingName = matchingName;
		CS_0024_003C_003E8__locals0.handled = false;
		if (e is AggregateException ex)
		{
			ex.Handle(delegate(Exception x)
			{
				if (x != null)
				{
					if (!(x is OperationCanceledException))
					{
						if (x is _E2D4 obj)
						{
							_E2D4 obj2 = obj;
							CS_0024_003C_003E8__locals0._003C_003E4__this.Logger.LogInfo(string.Format(_ED3E._E000(146933), CS_0024_003C_003E8__locals0.matchingName, obj2.Code));
							CS_0024_003C_003E8__locals0.handled = true;
						}
					}
					else
					{
						CS_0024_003C_003E8__locals0._003C_003E4__this.Logger.LogInfo(CS_0024_003C_003E8__locals0.matchingName + _ED3E._E000(146881));
						CS_0024_003C_003E8__locals0.handled = true;
					}
				}
				return CS_0024_003C_003E8__locals0.handled;
			});
		}
		else if (!CS_0024_003C_003E8__locals0._E000(e))
		{
			Logger.LogException(e);
			Logger.LogError(CS_0024_003C_003E8__locals0.matchingName + _ED3E._E000(145167) + e.Message);
		}
		if (this.m__E002 != null)
		{
			this.m__E002.IsInSession = false;
		}
		else
		{
			UnityEngine.Debug.LogError(_ED3E._E000(145153));
		}
		if (CS_0024_003C_003E8__locals0.handled)
		{
			_E02C().HandleExceptions();
			return;
		}
		Logger.LogError(CS_0024_003C_003E8__locals0.matchingName + _ED3E._E000(145167) + e.Message);
		MonoBehaviourSingleton<PreloaderUI>.Instance.ShowErrorScreen(_buildVersion, e, delegate
		{
			CS_0024_003C_003E8__locals0._003C_003E4__this._E02C().HandleExceptions();
		});
	}

	private void _E01D(IResult result, Action callback)
	{
		if (result.Failed)
		{
			UnityEngine.Debug.LogError(_ED3E._E000(145192) + result.Error);
			MonoBehaviourSingleton<PreloaderUI>.Instance.ShowErrorScreen("", _ED3E._E000(145239) + result.Error, callback);
		}
		else
		{
			callback();
		}
	}

	private async Task _E01E(TimeAndWeatherSettings timeAndWeather)
	{
		if (MonoBehaviourSingleton<PreloaderUI>.Instantiated)
		{
			MonoBehaviourSingleton<PreloaderUI>.Instance.MenuTaskBar.PreparingRaid();
		}
		CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
		MatchmakerTimeHasCome._E000 screenController = _E019(isPet: false, cancellationTokenSource);
		await base.Session.FlushOperationQueue();
		try
		{
			await RunFilesChecking(ConsistencyEnsuranceMode.Fast, ConsistencyEnsuranceMode.Full, cancellationTokenSource.Token);
			_E76B scenePreset = new _E76B(_raidSettings.SelectedLocation.Scene, disableServerScenes: false);
			await _E021(scenePreset, cancellationTokenSource.Token, screenController);
			await Singleton<_E760>.Instance.LoadBundlesAndCreatePools(_E760.PoolsCategory.Raid, _E760.AssemblyType.Local, base.Session.Profile.GetAllPrefabPaths().ToArray(), _ECE3.General, new _ECCE<_E5BB>(delegate(_E5BB p)
			{
				screenController.ChangeStatus(_ED3E._E000(146969), p.Progress);
			}), cancellationTokenSource.Token);
		}
		catch (_E783)
		{
			this.m__E003 = _ED3E._E000(146948).Localized();
			cancellationTokenSource.Cancel();
		}
		catch (OperationCanceledException)
		{
			cancellationTokenSource.Cancel();
		}
		catch (AggregateException ex2)
		{
			ex2.Handle(delegate(Exception x)
			{
				bool num = x is OperationCanceledException;
				if (num)
				{
					cancellationTokenSource.Cancel();
				}
				return num;
			});
		}
		catch (Exception e)
		{
			Logger.LogException(e);
			cancellationTokenSource.Cancel();
		}
		if (cancellationTokenSource.Token.IsCancellationRequested)
		{
			screenController.SearchingForServer = false;
			screenController.ChangeStatus(_ED3E._E000(146837));
			screenController.ChangeCancelButtonVisibility(value: false);
			screenController.CloseScreen();
		}
		cancellationTokenSource.Token.ThrowIfCancellationRequested();
		screenController.ChangeCancelButtonVisibility(value: false);
		if ((await _E023(screenController)).Succeed)
		{
			await _E027(timeAndWeather, screenController);
		}
	}

	private async Task _E01F(string groupId, EMatchingType type)
	{
		_E7A3.IsNetworkGame = true;
		Logger.LogDebug(_ED3E._E000(146997) + groupId);
		if (MonoBehaviourSingleton<PreloaderUI>.Instantiated)
		{
			MonoBehaviourSingleton<PreloaderUI>.Instance.MenuTaskBar.PreparingRaid();
		}
		_E909 metricsEvents = new _E909(Logger);
		CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
		MatchmakerTimeHasCome._E000 obj = _E019(_raidSettings.IsScav, cancellationTokenSource);
		obj.ChangeStatus(_ED3E._E000(147038));
		await base.Session.FlushOperationQueue();
		if (!(await base.Session.IsMatchingAvailable()))
		{
			throw new Exception(_ED3E._E000(147013).Localized());
		}
		_E782.ReadyToMatching = false;
		Task<_E542> task = null;
		try
		{
			await RunFilesChecking(ConsistencyEnsuranceMode.Fast, ConsistencyEnsuranceMode.Full, cancellationTokenSource.Token);
			task = _E782.Execute(base.Session, _raidSettings, groupId, type, cancellationTokenSource.Token, metricsEvents, Singleton<_E5CB>.Instance.GameSearchingTimeout);
			_E76B scenePreset = new _E76B(_raidSettings.SelectedLocation.Scene, disableServerScenes: true);
			Task task2 = _E020(scenePreset, cancellationTokenSource, metricsEvents, obj);
			await Task.WhenAll(task, task2);
		}
		catch (_E783)
		{
			this.m__E003 = _ED3E._E000(146948).Localized();
			cancellationTokenSource.Cancel();
		}
		catch (OperationCanceledException)
		{
			cancellationTokenSource.Cancel();
		}
		catch (AggregateException ex2)
		{
			ex2.Handle(delegate(Exception x)
			{
				bool num = x is OperationCanceledException;
				if (num)
				{
					cancellationTokenSource.Cancel();
				}
				return num;
			});
		}
		catch (Exception e)
		{
			Logger.LogException(e);
			cancellationTokenSource.Cancel();
		}
		if (_E014(out var _, isMatching: true) || cancellationTokenSource.Token.IsCancellationRequested)
		{
			obj.ChangeStatus(_ED3E._E000(146837));
			obj.ChangeCancelButtonVisibility(value: false);
			await _E015(isMatching: true);
			obj.CloseScreen();
		}
		cancellationTokenSource.Token.ThrowIfCancellationRequested();
		obj.ChangeCancelButtonVisibility(value: false);
		Profile profileOfPet = base.Session.ProfileOfPet;
		_E554.Location selectedLocation = _raidSettings.SelectedLocation;
		if ((await _E023(obj)).Succeed)
		{
			await _E024(task.Result.ProfileId, profileOfPet, selectedLocation, metricsEvents, obj);
		}
	}

	private async Task _E020(_E76B scenePreset, CancellationTokenSource cancellationTokenSource, _E909 metricsEvents, MatchmakerTimeHasCome._E000 screenController)
	{
		Logger.LogInfo(_ED3E._E000(147125) + scenePreset.key.path + _ED3E._E000(147104) + scenePreset.key.rcid);
		Logger.LogDebug(_ED3E._E000(147167));
		await _E021(scenePreset, cancellationTokenSource.Token, screenController);
		metricsEvents.SetLocationLoaded();
	}

	private async Task _E021(_E76B scenePreset, CancellationToken cancellationToken, MatchmakerTimeHasCome._E000 screenController)
	{
		_E725 activeProfileStatus = base.Session.ActiveProfileStatus;
		_E725 activePetStatus = base.Session.ActivePetStatus;
		string currentProfileId = ((activeProfileStatus != null) ? activeProfileStatus.profileid : activePetStatus.profileid);
		using (_E069.StartWithToken(_ED3E._E000(143789)))
		{
			await HideoutControllerAccess.UnloadHideout();
		}
		cancellationToken.ThrowIfCancellationRequested();
		screenController.ShowPlayerModel();
		GameWorld gameWorld;
		using (_E069.StartWithToken(_ED3E._E000(143733)))
		{
			gameWorld = ((!_raidSettings.Local) ? ((ClientGameWorld)GameWorld.Create<ClientNetworkGameWorld>(new GameObject(_ED3E._E000(143718)), Singleton<_E760>.Instance, PlayerUpdateQueue, currentProfileId)) : ((ClientGameWorld)GameWorld.Create<ClientLocalGameWorld>(new GameObject(_ED3E._E000(143718)), Singleton<_E760>.Instance, PlayerUpdateQueue, currentProfileId)));
			try
			{
				_ECCE<_E5BB> progress = new _ECCE<_E5BB>(delegate(_E5BB pr)
				{
					screenController.ChangeStatus(pr.Stage.ToString(), pr.Progress);
				});
				await gameWorld.InitLevel(Singleton<_E63B>.Instance, _objectsFactoryConfig, loadBundlesAndCreatePools: true, null, progress, cancellationToken);
			}
			catch (OperationCanceledException)
			{
				gameWorld.Dispose();
			}
			catch (Exception e)
			{
				Logger.LogException(e);
				gameWorld.Dispose();
			}
		}
		Singleton<GameWorld>.Create(gameWorld);
		Singleton<_E5CE>.Create(gameWorld);
		cancellationToken.ThrowIfCancellationRequested();
		Logger.LogDebug(_ED3E._E000(147186));
		screenController.ChangeStatus(_ED3E._E000(147139));
		int asyncUploadTimeSlice = QualitySettings.asyncUploadTimeSlice;
		int asyncUploadBufferSize = QualitySettings.asyncUploadBufferSize;
		QualitySettings.asyncUploadTimeSlice = _E2B6.Config.QualitySettingsAsyncUploadTimeSlice;
		QualitySettings.asyncUploadBufferSize = _E2B6.Config.QualitySettingsAsyncUploadBufferSize;
		GraphicsSettings.logWhenShaderIsCompiled = _E2B6.Config.LogWhenShaderIsCompiled;
		using (_E069.StartWithToken(_ED3E._E000(147222)))
		{
			_E769 obj2 = _E5DB.Manager.LoadScenesFromPreset(scenePreset, loadFirstAsSingle: true, loadInParallelMode: false, allowSceneActivation: true, cancellationToken, new _ECCE<float>(delegate(float pr)
			{
				screenController.ChangeStatus(_ED3E._E000(147139), pr);
				if (pr > 0.85f)
				{
					_E782.ReadyToMatching = true;
				}
			}));
			Logger.LogDebug(_ED3E._E000(147203));
			try
			{
				await obj2;
				if (obj2.Succeed)
				{
					Singleton<LevelSettings>.Instance.OnPostLoadingScene();
				}
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
			finally
			{
				QualitySettings.asyncUploadTimeSlice = asyncUploadTimeSlice;
				QualitySettings.asyncUploadBufferSize = asyncUploadBufferSize;
			}
			cancellationToken.ThrowIfCancellationRequested();
			if (obj2.Failed)
			{
				if (obj2.IsCanceled)
				{
					throw new OperationCanceledException();
				}
				throw new Exception(obj2.Error);
			}
		}
		_E782.ReadyToMatching = true;
		await Task.Yield();
		_E320._E003.Create();
		PerfectCullingCrossSceneSampler.InitializeAutoCulling();
		StaticDeferredDecalRenderer.Instance?.UpdateInstancesBuffers();
		if (MonoBehaviourSingleton<SpatialAudioSystem>.Instantiated)
		{
			MonoBehaviourSingleton<SpatialAudioSystem>.Instance.Initialize();
		}
		else
		{
			_E022();
		}
		screenController.SearchingForServer = true;
		screenController.ChangeStatus(_ED3E._E000(147239));
	}

	private void _E022()
	{
		if (!MonoBehaviourSingleton<BetterAudio>.Instantiated)
		{
			UnityEngine.Debug.LogError(_ED3E._E000(145278));
			return;
		}
		GameObject obj = new GameObject(_ED3E._E000(145255));
		obj.transform.SetParent(Singleton<BetterAudio>.Instance.gameObject.transform);
		SpatialAudioSystem spatialAudioSystem = obj.AddComponent<SpatialAudioSystem>();
		Singleton<SpatialAudioSystem>.Create(spatialAudioSystem);
		spatialAudioSystem.UseNewPropagationSystem = false;
		spatialAudioSystem.Initialize();
		UnityEngine.Debug.LogError(_ED3E._E000(145298));
	}

	private async Task<IResult> _E023(MatchmakerTimeHasCome._E000 timeHasComeScreenController, bool isRaid = true)
	{
		using (_E069.StartWithToken(_ED3E._E000(147291)))
		{
			if (isRaid)
			{
				CurrentRaidNum++;
				_E7A3.InRaid = true;
			}
			await Task.Yield();
			Logger.LogTrace(_ED3E._E000(147320));
			if (timeHasComeScreenController != null)
			{
				timeHasComeScreenController.SearchingForServer = false;
				timeHasComeScreenController.ChangeStatus(_ED3E._E000(147300));
			}
			IOperation operation = await _E5DB.Manager.LoadScene(_E785.GameUIScene, LoadSceneMode.Additive);
			Logger.LogTrace(_ED3E._E000(147346));
			if (operation.Succeed)
			{
				Logger.LogDebug(_ED3E._E000(147390));
				MonoBehaviourSingleton<GameUI>.Instance.gameObject.SetActive(value: false);
				UnityEngine.Object.DontDestroyOnLoad(MonoBehaviourSingleton<GameUI>.Instance.gameObject);
				if (this.m__E002 != null)
				{
					this.m__E002.IsInSession = isRaid;
				}
				else
				{
					UnityEngine.Debug.LogError(_ED3E._E000(147370));
				}
				Logger.LogTrace(_ED3E._E000(147408));
				return SuccessfulResult.New;
			}
			Logger.LogTrace(_ED3E._E000(147452));
			Logger.LogError(_ED3E._E000(147432) + operation.Error);
			MonoBehaviourSingleton<PreloaderUI>.Instance.ShowErrorScreen(_buildVersion, operation.Error, delegate
			{
				Logger.LogTrace(_ED3E._E000(143445));
				_E02C().HandleExceptions();
			});
			return new FailedResult(_ED3E._E000(147432) + operation.Error);
		}
	}

	private async Task _E024(string profileId, Profile savageProfile, _E554.Location location, _E909 metricsEvents, MatchmakerTimeHasCome._E000 timeHasComeScreenController)
	{
		metricsEvents.SetGamePrepared();
		try
		{
			await _E025(profileId, savageProfile, metricsEvents, timeHasComeScreenController);
		}
		catch (TaskCanceledException ex)
		{
			TaskCanceledException ex2 = ex;
			TaskCanceledException e = ex2;
			MonoBehaviourSingleton<PreloaderUI>.Instance.ShowErrorScreen(_buildVersion, e.Message, delegate
			{
				_E02A(profileId, savageProfile, location, new Result<ExitStatus, TimeSpan, _E907>
				{
					Error = e.Message
				}, timeHasComeScreenController);
			});
		}
		catch (Exception ex3)
		{
			Logger.LogError(_ED3E._E000(145471) + ex3.Message);
			MonoBehaviourSingleton<PreloaderUI>.Instance.ShowErrorScreen(_buildVersion, ex3, delegate
			{
				_E02A(profileId, savageProfile, location, new Result<ExitStatus, TimeSpan, _E907>
				{
					Error = _ED3E._E000(145423)
				}, timeHasComeScreenController);
			});
		}
	}

	private async Task _E025(string profileId, Profile savageProfile, _E909 metricsEvents, MatchmakerTimeHasCome._E000 timeHasComeScreenController)
	{
		Logger.LogDebug(_ED3E._E000(145502));
		if (Singleton<_E857>.Instantiated)
		{
			Singleton<_E857>.Instance.Deactivate();
		}
		Logger.LogDebug(_ED3E._E000(145472));
		_E725 obj = base.Session.AllProfileStatus?.SingleOrDefault((_E725 status) => status.profileid == profileId);
		if (obj != null && obj.status == EProfileStatus.Free)
		{
			throw new TaskCanceledException(_ED3E._E000(145514).Localized());
		}
		if (obj == null || obj.status != EProfileStatus.Busy)
		{
			throw new Exception(_ED3E._E000(145548));
		}
		_E026(obj, savageProfile, metricsEvents, await base.Session.GetMetricsConfig(), timeHasComeScreenController);
	}

	private void _E026(_E725 profileStatus, Profile savageProfile, _E909 metricsEvents, _E908 metricsConfig, MatchmakerTimeHasCome._E000 timeHasComeScreenController)
	{
		Logger.LogDebug(_ED3E._E000(145333));
		Logger.LogDebug(_ED3E._E000(145375), profileStatus.ToString());
		_E554.Location location = _raidSettings.SelectedLocation;
		TimeSpan sessionTime = TimeSpan.FromMinutes(location.EscapeTimeLimit);
		NetworkGame networkGame = NetworkGame._E000(base.Session, profileStatus, _raidSettings, savageProfile, base.Session.InsuranceCompany, _inputTree, MonoBehaviourSingleton<CommonUI>.Instance, MonoBehaviourSingleton<PreloaderUI>.Instance, MonoBehaviourSingleton<GameUI>.Instance, metricsEvents, new _E90A(metricsConfig, this), PlayerUpdateQueue, sessionTime, delegate
		{
			Logger.LogDebug(_ED3E._E000(145583));
			_EC92.Instance.CloseAllScreensForced();
			UnityEngine.Object.DestroyImmediate(MonoBehaviourSingleton<MenuUI>.Instance.gameObject);
			this.m__E002?.Unsubscribe();
			BundleLock.MaxConcurrentOperations = 1;
			Singleton<GameWorld>.Instance.OnGameStarted();
		}, delegate(Result<ExitStatus, TimeSpan, _E907> result)
		{
			_E02A(profileStatus.profileid, savageProfile, location, result, timeHasComeScreenController);
		});
		Singleton<AbstractGame>.Create(networkGame);
		Logger.LogDebug(_ED3E._E000(145388));
		NetworkGameSession networkGameSession = NetworkGameSession._E000(networkGame, profileStatus.profileid, profileStatus.profileToken, Logger, delegate
		{
			timeHasComeScreenController.ChangeStatus(_ED3E._E000(145617));
		});
		metricsEvents.SetGameCreated();
		HostTopology hostTopology = new HostTopology(_E6A8._E000(), 1);
		Logger.LogDebug(_ED3E._E000(143382));
		networkGameSession._E001(hostTopology, profileStatus.ip, profileStatus.port);
		timeHasComeScreenController.ChangeStatus(_ED3E._E000(143416));
	}

	private async Task _E027(TimeAndWeatherSettings timeAndWeather, MatchmakerTimeHasCome._E000 timeHasComeScreenController)
	{
		if (Singleton<_E857>.Instantiated)
		{
			Singleton<_E857>.Instance.Deactivate();
		}
		Profile profile = base.Session.Profile;
		Profile savageProfile = base.Session.ProfileOfPet;
		_E554.Location location = _raidSettings.SelectedLocation;
		profile.Inventory.Stash = null;
		profile.Inventory.QuestStashItems = null;
		profile.Inventory.DiscardLimits = Singleton<_E63B>.Instance.GetDiscardLimits();
		timeHasComeScreenController.ChangeStatus(_ED3E._E000(145653));
		base.Session.SendRaidSettings(_raidSettings).HandleExceptions();
		TimeSpan sessionTime = _E028(location.EscapeTimeLimit);
		LocalGame localGame = LocalGame._E000(_inputTree, profile, _localGameDateTime, base.Session.InsuranceCompany, MonoBehaviourSingleton<MenuUI>.Instance, MonoBehaviourSingleton<CommonUI>.Instance, MonoBehaviourSingleton<PreloaderUI>.Instance, MonoBehaviourSingleton<GameUI>.Instance, location, timeAndWeather, _raidSettings.WavesSettings, _raidSettings.SelectedDateTime, delegate(Result<ExitStatus, TimeSpan, _E907> result)
		{
			_E02A(profile.Id, savageProfile, location, result, timeHasComeScreenController);
		}, _fixedDeltaTime, PlayerUpdateQueue, base.Session, sessionTime);
		Singleton<AbstractGame>.Create(localGame);
		await localGame._E002(_raidSettings.BotSettings, _backendUrl, null, delegate
		{
			using (_E069.StartWithToken(_ED3E._E000(143489)))
			{
				UnityEngine.Object.DestroyImmediate(MonoBehaviourSingleton<MenuUI>.Instance.gameObject);
				this.m__E002?.Unsubscribe();
				BundleLock.MaxConcurrentOperations = 1;
				Singleton<GameWorld>.Instance.OnGameStarted();
			}
		});
	}

	private TimeSpan _E028(int defaultMinutes)
	{
		return TimeSpan.FromSeconds(60 * defaultMinutes);
	}

	private void _E029()
	{
		if (Singleton<AbstractGame>.Instantiated)
		{
			Singleton<AbstractGame>.Instance.Dispose();
			Singleton<AbstractGame>.Release(Singleton<AbstractGame>.Instance);
		}
		if (Singleton<GameWorld>.Instantiated)
		{
			GameWorld instance = Singleton<GameWorld>.Instance;
			instance.Dispose();
			Singleton<GameWorld>.Release(instance);
			Singleton<_E5CE>.Release(instance);
			UnityEngine.Object.Destroy(instance.gameObject);
		}
		if (Singleton<BetterAudio>.Instantiated)
		{
			Singleton<BetterAudio>.Release(Singleton<BetterAudio>.Instance);
		}
		Singleton<_E760>.Instance.UnloadTemporaryPools(cleanUselessPools: false);
		Singleton<_E760>.Instance.UnloadBundles();
	}

	private async void _E02A(string profileId, Profile savageProfile, _E554.Location location, Result<ExitStatus, TimeSpan, _E907> result, MatchmakerTimeHasCome._E000 timeHasComeScreenController = null)
	{
		AudioListener.pause = true;
		_E029();
		_E7A3.InRaid = false;
		_E7A3.IsNetworkGame = false;
		MonoBehaviourSingleton<PreloaderUI>.Instance.SetLoaderStatus(status: true);
		MonoBehaviourSingleton<PreloaderUI>.Instance.FadeBlackScreen(1f, -1f);
		using (_E069.StartWithToken(_ED3E._E000(145692)))
		{
			int sceneCount = SceneManager.sceneCount;
			string[] array = new string[sceneCount];
			for (int i = 0; i < sceneCount; i++)
			{
				array[i] = SceneManager.GetSceneAt(i).name;
			}
			if (MonoBehaviourSingleton<EnvironmentUI>.Instantiated)
			{
				MonoBehaviourSingleton<EnvironmentUI>.Instance.RefreshEnvironmentAsync().HandleExceptions();
			}
			await _E5DB.Manager.LoadScene(_E785.EmptyScene, LoadSceneMode.Additive);
			this.m__E005 = Task.WhenAll(array.Select((string x) => _E76A.UnloadScene(x)));
		}
		BundleLock.MaxConcurrentOperations = int.MaxValue;
		using (_E069.StartWithToken(_ED3E._E000(145679)))
		{
			if (timeHasComeScreenController != null && !timeHasComeScreenController.Closed)
			{
				timeHasComeScreenController.CloseSelf(forced: true).HandleExceptions();
			}
			if (MonoBehaviourSingleton<MenuUI>.Instantiated)
			{
				UnityEngine.Object.Destroy(MonoBehaviourSingleton<MenuUI>.Instance.gameObject);
			}
			if (Singleton<_E857>.Instantiated)
			{
				Singleton<_E857>.Instance.Activate();
			}
		}
		using (_E069.StartWithToken(_ED3E._E000(145726)))
		{
			UnityEngine.Object.Destroy(MonoBehaviourSingleton<GameUI>.Instance.gameObject);
		}
		if (this.m__E002 != null)
		{
			this.m__E002.IsInSession = false;
		}
		else
		{
			UnityEngine.Debug.LogError(_ED3E._E000(145153));
		}
		using (_E069.StartWithToken(_ED3E._E000(145709)))
		{
			_E3AB.Collect(2, GCCollectionMode.Forced, isBlocking: true, compacting: true, force: true);
		}
		AudioListener.pause = false;
		using (_E069.StartWithToken(_ED3E._E000(145754)))
		{
			if ((bool)Singleton<_E7DE>.Instance.Sound.Settings.MusicOnRaidEnd)
			{
				Singleton<GUISounds>.Instance._E002(result.Value0);
			}
		}
		if (result.Succeed)
		{
			try
			{
				await _E02B(profileId, savageProfile, location, result.Value0, result.Value1, result.Value2);
				return;
			}
			catch (Exception ex)
			{
				Logger.LogError(_ED3E._E000(145734) + ex.Message);
				MonoBehaviourSingleton<PreloaderUI>.Instance.ShowErrorScreen(_buildVersion, ex, delegate
				{
					_E02C().HandleExceptions();
				});
				MonoBehaviourSingleton<PreloaderUI>.Instance.SetLoaderStatus(status: false);
				return;
			}
		}
		MonoBehaviourSingleton<PreloaderUI>.Instance.SetLoaderStatus(status: false);
		_E02C().HandleExceptions();
	}

	private async Task _E02B(string profileId, Profile savageProfile, _E554.Location location, ExitStatus exitStatus, TimeSpan exitTime, _E907 clientMetrics = null)
	{
		using (_E069.StartWithToken(_ED3E._E000(145771)))
		{
			using (_E069.StartWithToken(_ED3E._E000(145813)))
			{
				if ((await _E5DB.Manager.LoadScene(_E785.SessionEndUIScene, LoadSceneMode.Additive)).Failed)
				{
					throw new Exception(_ED3E._E000(145849));
				}
			}
			_E34D stats = null;
			if (base.Session.Destroyed)
			{
				return;
			}
			if (base.Session.Profile == null)
			{
				throw new NullReferenceException(_ED3E._E000(145887));
			}
			if (_raidSettings.Local)
			{
				stats = base.Session.Profile.Stats;
			}
			using (_E069.StartWithToken(_ED3E._E000(145871)))
			{
				if (clientMetrics?.Metrics != null)
				{
					await base.Session.SendMetrics(clientMetrics);
				}
			}
			using (_E069.StartWithToken(_ED3E._E000(145859)))
			{
				await base.Session.GetProfiles();
				if (base.Session.Profile == null)
				{
					throw new NullReferenceException(_ED3E._E000(145887));
				}
				if (_raidSettings.Local)
				{
					base.Session.Profile.Stats = stats;
				}
				await _E77F.SelectProfile(base.Session);
			}
			MonoBehaviourSingleton<PreloaderUI>.Instance.SetLoaderStatus(status: false);
			_E784 obj2 = _E784._E000(base.Session, profileId, savageProfile, location, exitStatus, exitTime, _raidSettings.RaidMode);
			await obj2;
			if (obj2.Succeed)
			{
				_E02C().HandleExceptions();
			}
		}
	}

	private async Task _E02C()
	{
		_E7A3.IsNetworkGame = false;
		if (this.m__E005 != null)
		{
			using (_E069.StartWithToken(_ED3E._E000(145911)))
			{
				await this.m__E005;
				this.m__E005 = null;
			}
		}
		_EC92.Instance.CloseAllScreensForced();
		_E029();
		if (Singleton<MenuUI>.Instantiated)
		{
			UnityEngine.Object.DestroyImmediate(MonoBehaviourSingleton<MenuUI>.Instance.gameObject);
		}
		this.m__E002?.Unsubscribe();
		await Resources.UnloadUnusedAssets().Await();
		_E3AB.Collect(2, GCCollectionMode.Forced, isBlocking: true, compacting: true, force: true);
		IOperation operation;
		using (_E069.StartWithToken(_ED3E._E000(145947)))
		{
			operation = await _E5DB.Manager.LoadScene(_E785.MenuUIScene);
		}
		if (operation.Succeed)
		{
			if (MonoBehaviourSingleton<MenuUI>.Instantiated)
			{
				UnityEngine.Object.DontDestroyOnLoad(MonoBehaviourSingleton<MenuUI>.Instance.gameObject);
				this.m__E001._E000(MonoBehaviourSingleton<MenuUI>.Instance.MatchMakerAcceptScreen);
				this.m__E001._E000(MonoBehaviourSingleton<MenuUI>.Instance.MatchMakerSelectionLocationScreen);
				this.m__E001._E000(MonoBehaviourSingleton<MenuUI>.Instance.MatchMakerSideSelectionScreen);
				this.m__E001._E000(MonoBehaviourSingleton<MenuUI>.Instance.MatchmakerFinalCountdown);
				this.m__E001._E000(MonoBehaviourSingleton<MenuUI>.Instance.MatchmakerMapPoints);
				this.m__E001._E000(MonoBehaviourSingleton<MenuUI>.Instance.MatchmakerOfflineRaidScreen);
				this.m__E001._E000(MonoBehaviourSingleton<MenuUI>.Instance.MatchmakerInsuranceScreen);
				this.m__E001._E000(MonoBehaviourSingleton<MenuUI>.Instance.MatchmakerTimeHasCome);
				this.m__E001._E000(MonoBehaviourSingleton<MenuUI>.Instance.MatchmakerKeyAccessScreen);
				this.m__E001.CheckDuplicateChildren();
				await _E00F();
			}
		}
		else
		{
			Logger.LogError(_ED3E._E000(145921) + operation.Error);
			MonoBehaviourSingleton<PreloaderUI>.Instance.ShowErrorScreen(_buildVersion, operation.Error, delegate
			{
				_E02C().HandleExceptions();
			});
		}
	}

	private void OnApplicationQuit()
	{
		_E00D();
		_E90C.SetAffinityToLogicalCoresEnabled(enabled: false);
	}

	[CompilerGenerated]
	private void _E02D()
	{
		_E000(on: true);
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task _E02E()
	{
		return base.Start();
	}

	[CompilerGenerated]
	private void _E02F(EMenuType menuType, bool isOn)
	{
		this.m__E002.ShowScreen(menuType, isOn);
	}

	[CompilerGenerated]
	private void _E030(string operationName, string error)
	{
		Logger.LogError(_ED3E._E000(143397) + operationName + _ED3E._E000(143395) + error);
		MonoBehaviourSingleton<PreloaderUI>.Instance.ShowErrorScreen(_buildVersion, error, delegate
		{
			_E00A();
		});
	}

	[CompilerGenerated]
	private void _E031()
	{
		_E00A();
	}

	[CompilerGenerated]
	private void _E032()
	{
		_E00F().HandleExceptions();
	}

	[CompilerGenerated]
	private void _E033()
	{
		_E007().HandleExceptions();
	}

	[CompilerGenerated]
	private void _E034()
	{
		_E01A().HandleExceptions();
	}

	[CompilerGenerated]
	private void _E035()
	{
		Logger.LogTrace(_ED3E._E000(143445));
		_E02C().HandleExceptions();
	}

	[CompilerGenerated]
	private void _E036()
	{
		_E02C().HandleExceptions();
	}

	[CompilerGenerated]
	private void _E037()
	{
		_E02C().HandleExceptions();
	}
}
