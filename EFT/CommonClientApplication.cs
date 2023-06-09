using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Comfort.Common;
using Comfort.Serialization;
using DG.Tweening;
using EFT.InputSystem;
using EFT.UI;
using FilesChecker;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngineInternal;

namespace EFT;

public abstract class CommonClientApplication<T> : ClientApplication<T> where T : class, _E795
{
	[CompilerGenerated]
	private sealed class _E001
	{
		public IFilesChecker filesChecker;

		public ConsistencyEnsuranceMode ordinaryFileEnsuranceMode;

		public ConsistencyEnsuranceMode criticalFileEnsuranceMode;

		public CancellationToken token;

		internal Task<ICheckResult> _E000()
		{
			return filesChecker.EnsureConsistency(ordinaryFileEnsuranceMode, criticalFileEnsuranceMode, null, token);
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public TaskCompletionSource<bool> completionSource;

		internal void _E000()
		{
			MonoBehaviourSingleton<PreloaderUI>.Instance.CloseErrorScreen();
			completionSource.SetResult(result: true);
		}
	}

	[CompilerGenerated]
	private sealed class _E004
	{
		public IFilesChecker filesChecker;

		public string resultPath;

		public ConsistencyEnsuranceMode ordinaryFileEnsuranceMode;

		public ConsistencyEnsuranceMode criticalFileEnsuranceMode;

		internal Task<ICheckResult> _E000()
		{
			return filesChecker.EnsureConsistencySingle(resultPath, ordinaryFileEnsuranceMode, criticalFileEnsuranceMode, CancellationToken.None);
		}
	}

	[CompilerGenerated]
	private sealed class _E005
	{
		public TaskCompletionSource<object> completionSource;

		internal void _E000()
		{
			MonoBehaviourSingleton<PreloaderUI>.Instance.CloseErrorScreen();
			completionSource.SetResult(true);
		}
	}

	[SerializeField]
	protected bool _developMode;

	[SerializeField]
	protected int _fixedFrameRate = 60;

	[SerializeField]
	protected float _fixedDeltaTime = 1f / 60f;

	[SerializeField]
	protected string _localGameDate;

	[SerializeField]
	protected string _localGameTime;

	[SerializeField]
	protected float _localGameTimeFactor = 1f;

	[SerializeField]
	protected bool _resetSettings;

	[SerializeField]
	protected string _version;

	protected _E761 _objectsFactoryConfig;

	protected _E777 _commandLineArgs;

	protected string _backendUrl;

	protected string _buildVersion = "";

	private void OnValidate()
	{
		if (_fixedFrameRate < 20)
		{
			_fixedFrameRate = 20;
		}
		_fixedDeltaTime = 1f / (float)_fixedFrameRate;
		_E629.Validate(ref _localGameDate, ref _localGameTime, ref _localGameTimeFactor, init: false);
	}

	public override void Awake()
	{
		_E000();
		Application.targetFrameRate = 60;
		_commandLineArgs = new _E777();
		_backendUrl = _E2B6.BackendUrl;
		Binary.FindStoreableCode = false;
		_E5B8.Setup(LoggerMode.Replace);
		_E5DA.Setup();
		ConfigureApplication();
		_EA10.Init();
		_EB1B.Init();
		base.Awake();
		EDriveType eDriveType = _E770._E003();
		EDriveType eDriveType2 = _E770._E005();
		Logger.LogInfo(string.Format(_ED3E._E000(134007), eDriveType, eDriveType2));
		StaticManager.BeginCoroutine(CheckNVidiaReflexAvailability());
		if (SystemInfo.graphicsDeviceType != GraphicsDeviceType.Direct3D11)
		{
			Application.Quit();
		}
		DLSSWrapper.IsDLSSSupported();
	}

	protected override async Task Start()
	{
		await RunFilesChecking();
		Logger.LogTrace(_ED3E._E000(134077), Mathf.Epsilon);
		Logger.LogTrace(_ED3E._E000(134057), float.Epsilon);
		Logger.LogTrace(_ED3E._E000(134101), MathfInternal.IsFlushToZeroEnabled);
		Logger.LogTrace(_ED3E._E000(144411), MathfInternal.FloatMinDenormal);
		Logger.LogTrace(_ED3E._E000(144477), MathfInternal.FloatMinNormal);
		using (_E069.StartWithToken(_ED3E._E000(144485)))
		{
			DOTween.SetTweensCapacity(512, 50);
			await base.Start();
			using (_E069.StartWithToken(_ED3E._E000(144531)))
			{
				IOperation<_EC35, InputTree> operation = await _E77D.Execute(_resetSettings, PlayerUpdateQueue, BundleLock, BundleCheck);
				if (operation.Failed)
				{
					Logger.LogError(_ED3E._E000(144562) + operation.Error);
					MonoBehaviourSingleton<PreloaderUI>.Instance.ShowErrorScreen(_buildVersion, operation.Error, delegate
					{
						Start().HandleExceptions();
					});
					return;
				}
				Result<_EC35, InputTree> result = operation.Result;
				Init(result.Value0, result.Value1);
				_E7DE instance = Singleton<_E7DE>.Instance;
				Logger.LogInfo(_ED3E._E000(144581), instance.Game.Settings.ToPrettyJson());
				Logger.LogInfo(_ED3E._E000(144624), instance.Sound.Settings.ToPrettyJson());
				Logger.LogInfo(_ED3E._E000(144668), instance.PostFx.Settings.ToPrettyJson());
				Logger.LogInfo(_ED3E._E000(144649), instance.Graphics.Settings.ToPrettyJson());
				Logger.LogInfo(_ED3E._E000(144688), instance.Control.Settings.ToPrettyJson());
			}
			using (_E069.StartWithToken(_ED3E._E000(144734)))
			{
				await base.gameObject.AddComponent<UiPools>().Init();
			}
			Singleton<GUISounds>.Instance._E001();
			Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
			await LoadLoginScenes();
			Task task = CreateBackend();
			await task;
			task.HandleExceptions();
		}
	}

	protected virtual bool ConfigureApplication()
	{
		_E2B6.Config = new _E3B7();
		int num = 1 | (_E2B6.Config.PatchConfig(_commandLineArgs.ConfigPatch) ? 1 : 0);
		_E3B7 config = _E2B6.Config;
		_E315.UnityDebugLogsEnabled = config.Log.UnityDebugLogsEnabled;
		_E315.IsLogsEnabled = !config.Log.DisableAllLogs;
		if (config.Log.DisableAllLogs)
		{
			UnityEngine.Debug.unityLogger.logEnabled = !config.Log.DisableAllLogs;
			Application.SetStackTraceLogType(LogType.Assert, StackTraceLogType.None);
			Application.SetStackTraceLogType(LogType.Error, StackTraceLogType.None);
			Application.SetStackTraceLogType(LogType.Exception, StackTraceLogType.None);
			Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.None);
			Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
		}
		if (num != 0)
		{
			_backendUrl = config.BackendUrl;
			_version = config.Version;
			_buildVersion = config.BuildVersion;
			_resetSettings = config.ResetSettings;
		}
		_objectsFactoryConfig = _E761.Create(config.Pools.AmmoPoolSize, config.Pools.WeaponsPoolSize, config.Pools.MagsPoolSize, config.Pools.ItemsPoolSize, config.Pools.PlayersPoolSize);
		return (byte)num != 0;
	}

	protected async Task RunFilesChecking(ConsistencyEnsuranceMode ordinaryFileEnsuranceMode = ConsistencyEnsuranceMode.Fast, ConsistencyEnsuranceMode criticalFileEnsuranceMode = ConsistencyEnsuranceMode.Full, CancellationToken token = default(CancellationToken))
	{
		_E7DB loggerFactory = new _E7DB();
		IFilesChecker filesChecker = FilesCheckerFactory.CreateFilesChecker(loggerFactory);
		ICheckResult result;
		using (_E069.StartWithToken(_ED3E._E000(144726)))
		{
			result = await Task.Run(() => filesChecker.EnsureConsistency(ordinaryFileEnsuranceMode, criticalFileEnsuranceMode, null, token), CancellationToken.None);
		}
		if (result.Succeed() || result.Cancelled())
		{
			return;
		}
		if (MonoBehaviourSingleton<PreloaderUI>.Instance != null)
		{
			TaskCompletionSource<bool> completionSource = new TaskCompletionSource<bool>();
			MonoBehaviourSingleton<PreloaderUI>.Instance.ShowErrorScreen(_buildVersion, _ED3E._E000(144708).Localized(), delegate
			{
				MonoBehaviourSingleton<PreloaderUI>.Instance.CloseErrorScreen();
				completionSource.SetResult(result: true);
			});
			await completionSource.Task;
		}
		if (Singleton<_EC96>.Instantiated)
		{
			Singleton<_EC96>.Instance.CloseAllScreensForced();
		}
		Application.Quit();
		throw new OperationCanceledException();
	}

	protected async Task DefaultBundleCheck(string bundlePath)
	{
		ConsistencyEnsuranceMode ordinaryFileEnsuranceMode = ConsistencyEnsuranceMode.Fast;
		ConsistencyEnsuranceMode criticalFileEnsuranceMode = ConsistencyEnsuranceMode.Full;
		string path = _ED3E._E000(144754);
		string resultPath = Path.Combine(path, bundlePath.Replace(_ED3E._E000(30703), _ED3E._E000(93636)));
		_E7DB loggerFactory = new _E7DB();
		IFilesChecker filesChecker = FilesCheckerFactory.CreateFilesChecker(loggerFactory);
		ICheckResult result;
		using (_E069.StartWithToken(_ED3E._E000(144726)))
		{
			result = await Task.Run(() => filesChecker.EnsureConsistencySingle(resultPath, ordinaryFileEnsuranceMode, criticalFileEnsuranceMode, CancellationToken.None), CancellationToken.None);
		}
		if (result.Succeed() || result.Cancelled())
		{
			return;
		}
		if (MonoBehaviourSingleton<PreloaderUI>.Instance != null)
		{
			TaskCompletionSource<object> completionSource = new TaskCompletionSource<object>();
			MonoBehaviourSingleton<PreloaderUI>.Instance.ShowErrorScreen(_buildVersion, _ED3E._E000(144708).Localized(), delegate
			{
				MonoBehaviourSingleton<PreloaderUI>.Instance.CloseErrorScreen();
				completionSource.SetResult(true);
			});
			await completionSource.Task;
		}
		Application.Quit();
		throw new OperationCanceledException();
	}

	protected abstract Task BundleCheck(string bundlePath);

	protected abstract Task LoadLoginScenes();

	protected abstract Task CreateBackend();

	private void _E000()
	{
		AudioConfiguration configuration = AudioSettings.GetConfiguration();
		configuration.dspBufferSize = 0;
		if (!AudioSettings.Reset(configuration))
		{
			UnityEngine.Debug.LogWarning(_ED3E._E000(134039));
		}
	}

	[CompilerGenerated]
	private void _E001()
	{
		Start().HandleExceptions();
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task _E002()
	{
		return base.Start();
	}
}
