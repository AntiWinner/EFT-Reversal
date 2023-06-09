using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.InputSystem;
using EFT.UI;
using NVIDIA;
using UnityEngine;

namespace EFT;

[_E2E2(-800)]
public abstract class ClientApplication<T> : AbstractApplication where T : class, _E795
{
	protected _E3C3<T> ClientBackEnd;

	protected InputTree _inputTree;

	protected _ED0B BundleLock;

	private _EC29 _E004;

	private PerfHelperWrapper _E005;

	protected override EUpdateQueue PlayerUpdateQueue => EUpdateQueue.Update;

	protected T Session
	{
		get
		{
			_E3C3<T> clientBackEnd = ClientBackEnd;
			return ((clientBackEnd != null) ? clientBackEnd.Session : null) ?? null;
		}
	}

	protected override _E316 CreateLogConfigurator()
	{
		return _E778.Create();
	}

	public override void Awake()
	{
		base.Awake();
		Singleton<ClientApplication<T>>.Create(this);
	}

	protected virtual async Task Start()
	{
		_EC74.Start();
		if (!Singleton<AudioListenerConsistencyManager>.Instantiated)
		{
			GameObject obj = new GameObject(_ED3E._E000(133631), typeof(AudioListener), typeof(AudioListenerConsistencyManager));
			Singleton<AudioListenerConsistencyManager>.Create(obj.GetComponent<AudioListenerConsistencyManager>());
			UnityEngine.Object.DontDestroyOnLoad(obj);
		}
		AudioSource audioSource = UnityEngine.Object.Instantiate(new GameObject(), base.transform).AddComponent<AudioSource>();
		Singleton<GUISounds>.Create(base.gameObject.AddComponent<GUISounds>());
		await Singleton<GUISounds>.Instance._E000(audioSource);
		await _EC45.Init();
		_EC45.SetCursor(ECursorType.Idle);
		BundleLock = new _ED0B(int.MaxValue);
		if (!Singleton<_EC29>.Instantiated)
		{
			_E004 = _EC29._E000();
			Singleton<_EC29>.Instance = _E004;
		}
		try
		{
			if (!Singleton<PerfHelperWrapper>.Instantiated)
			{
				_E005 = PerfHelperWrapper.Create();
				Singleton<PerfHelperWrapper>.Instance = _E005;
				if (_E005.InitLibrary())
				{
					_E005.Run();
				}
			}
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogError(message);
		}
		_E763.RegisterKeysToReset();
	}

	public T GetClientBackEndSession()
	{
		return Session;
	}

	protected IEnumerator CheckNVidiaReflexAvailability()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		Reflex.NvReflex_Status reflexStatus;
		bool flag = _E8AE.IsReflexAvailable(out reflexStatus);
		Logger.LogInfo((!flag) ? string.Format(_ED3E._E000(133686), reflexStatus) : string.Format(_ED3E._E000(133613), reflexStatus));
		yield return new WaitForEndOfFrame();
		Reflex component = base.gameObject.GetComponent<Reflex>();
		if (component != null)
		{
			UnityEngine.Object.Destroy(component);
		}
	}

	protected virtual void Init(_EC35 assetsManager, InputTree inputTree)
	{
		_E5DB.Manager = assetsManager;
		_inputTree = inputTree;
		UnityEngine.Object.DontDestroyOnLoad(_inputTree.gameObject);
	}

	protected virtual void LateUpdate()
	{
		Singleton<_E7DE>.Instance?.Graphics.Controller.CheckAltEnterWindowMode();
	}

	public override void OnDestroy()
	{
		if (Singleton<ClientApplication<T>>.TryRelease(this))
		{
			base.OnDestroy();
			DestroyApplication();
		}
	}

	protected virtual void DestroyApplication()
	{
		_EC29 obj = Interlocked.Exchange(ref _E004, null);
		if (obj != null)
		{
			Singleton<_EC29>.Release(null);
			obj._E003().HandleExceptions();
		}
		PerfHelperWrapper perfHelperWrapper = Interlocked.Exchange(ref _E005, null);
		if (perfHelperWrapper != null)
		{
			Singleton<PerfHelperWrapper>.Release(null);
			perfHelperWrapper.ExitLib();
		}
	}

	public void OnApplicationFocus(bool hasFocus)
	{
		if (Singleton<_E90A>.Instantiated)
		{
			Singleton<_E90A>.Instance.PauseFrameRelatedMetrics(!hasFocus);
		}
		if (hasFocus)
		{
			Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
			Singleton<_E7DE>.Instance?.Graphics.Controller.CheckAltTabFullScreenMode().HandleExceptions();
		}
	}
}
