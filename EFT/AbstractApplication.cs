using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Comfort.Common;
using Diz.Jobs;
using Diz.Utils;
using EFT.Animals;
using EFT.Interactive;
using EFT.Visual;
using UnityEngine;
using UnityEngine.Assertions;

namespace EFT;

[_E2E2(-3000)]
public abstract class AbstractApplication : MonoBehaviour
{
	public _E5B5 Logger;

	private static bool m__E000;

	[CompilerGenerated]
	private static bool _E001;

	[CompilerGenerated]
	private bool _E002;

	protected abstract EUpdateQueue PlayerUpdateQueue { get; }

	protected static bool Initialized
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
		[CompilerGenerated]
		private set
		{
			_E001 = value;
		}
	}

	public bool Destroyed
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
		[CompilerGenerated]
		private set
		{
			_E002 = value;
		}
	}

	public virtual void Awake()
	{
		if (Initialized)
		{
			Debug.LogWarning(_ED3E._E000(105743));
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		Initialized = true;
		CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
		if (_E2B6.Config == null)
		{
			_E2B6.LoadApplicationConfig(new _E3B7());
		}
		ServicePointManager.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;
		if (!Singleton<_E316>.Instantiated)
		{
			Singleton<_E316>.Create(CreateLogConfigurator());
		}
		Logger = new _E5B5(LoggerMode.Add);
		JobScheduler jobScheduler = base.gameObject.AddComponent<JobScheduler>();
		if (Application.targetFrameRate > 0)
		{
			jobScheduler.SetTargetFrameRate(Application.targetFrameRate);
		}
		if (_E2B6.Config.LoadForceModeMultiplier > 0f)
		{
			jobScheduler.DefaultForceModeMultiplier = _E2B6.Config.LoadForceModeMultiplier;
		}
		Singleton<JobScheduler>.Create(jobScheduler);
		jobScheduler.Init(_E2B6.Config.Pools.ContinuationProfilerEnabled);
		Singleton<AsyncWorker>.Create(base.gameObject.AddComponent<AsyncWorker>());
		CreateTechnicalSystems();
		if (!Singleton<_E307>.Instantiated)
		{
			Singleton<_E307>.Create(new _E307());
		}
		_EBAF.Instance.Initialize();
		new _EBBB();
		_E000();
		Logger.LogInfo(_ED3E._E000(105768), PlayerUpdateQueue);
		Logger.LogInfo(_ED3E._E000(105798), Assert.raiseExceptions);
		UnityEngine.Object.DontDestroyOnLoad(this);
		if (_E2B6.Config.Physics.ManualUpdate)
		{
			_E320._E002.Enabled = true;
		}
		if (_E5D4.Validate())
		{
			Logger.LogInfo(_ED3E._E000(105835));
		}
		else
		{
			Logger.LogError(_ED3E._E000(105860));
		}
		_E5D3.Run();
	}

	public static void CreateTechnicalSystems()
	{
		if (AbstractApplication.m__E000)
		{
			return;
		}
		Dictionary<string, Action<GameObject>> obj = new Dictionary<string, Action<GameObject>>
		{
			{
				_ED3E._E000(105927),
				ComponentSystem<MuzzleManager, MuzzleSystem>.Register
			},
			{
				_ED3E._E000(105982),
				ComponentSystem<BaseLight, BaseLightSystem>.Register
			},
			{
				_ED3E._E000(105972),
				ComponentSystem<Flicker, FlickerSystem>.Register
			},
			{
				_ED3E._E000(105964),
				ComponentSystem<LampController, LampSystem>.Register
			},
			{
				_ED3E._E000(105961),
				ComponentSystem<WeaponPrefab, WeaponOverHeatSystem>.Register
			},
			{
				_ED3E._E000(105954),
				ComponentSystem<BirdCurveBrain, BirdCurveBrainSystem>.Register
			},
			{
				_ED3E._E000(106015),
				ComponentSystem<DisablerCullingObjectBase, HoboCullingManager>.Register
			},
			{
				_ED3E._E000(106004),
				ComponentSystem<FloatingObject, FloatingObjectManager>.Register
			}
		};
		GameObject gameObject = new GameObject(_ED3E._E000(105997));
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		foreach (KeyValuePair<string, Action<GameObject>> item in obj)
		{
			item.Value(gameObject);
		}
		AbstractApplication.m__E000 = true;
	}

	protected abstract _E316 CreateLogConfigurator();

	private static void _E000()
	{
		Singleton<_E50D>.Create(_E50D.Create(LoggerMode.Add));
	}

	public virtual void OnDestroy()
	{
		if (Singleton<_E50D>.Instantiated)
		{
			Singleton<_E50D>.Release(Singleton<_E50D>.Instance);
		}
		if (Singleton<_E316>.Instantiated)
		{
			Singleton<_E316>.Instance.Shutdown();
			Singleton<_E316>.Release(Singleton<_E316>.Instance);
		}
		Destroy();
	}

	protected virtual void Destroy()
	{
		Destroyed = true;
	}

	protected virtual void FixedUpdate()
	{
		_E320.FixedUpdate();
	}

	protected virtual void Update()
	{
		_E320.Update();
	}
}
