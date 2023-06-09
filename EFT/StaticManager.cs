using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT;

[ExecuteInEditMode]
public class StaticManager : MonoBehaviour
{
	[CompilerGenerated]
	private Action _E000;

	public _E2BB TimerManager;

	private static StaticManager _E001;

	private static bool _E002;

	public static StaticManager Instance
	{
		get
		{
			if (_E001 != null)
			{
				return _E001;
			}
			_E001 = _E3AA.FindUnityObjectOfType<StaticManager>();
			if (_E001 == null)
			{
				_E001 = new GameObject(typeof(StaticManager).ToString()).AddComponent<StaticManager>();
			}
			return _E001;
		}
	}

	public event Action StaticUpdate
	{
		[CompilerGenerated]
		add
		{
			Action action = _E000;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E000, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E000;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E000, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	private void Awake()
	{
		if (_E001 != null)
		{
			Debug.LogError(_ED3E._E000(133591), base.gameObject);
			UnityEngine.Object.DestroyImmediate(this);
		}
		base.gameObject.hideFlags = HideFlags.DontSave;
		_E001 = this;
		_E001.TimerManager = new _E2BB();
	}

	public static Coroutine BeginCoroutine(IEnumerator routine)
	{
		return Instance.StartCoroutine(routine);
	}

	public static void KillCoroutine([CanBeNull] Coroutine routine)
	{
		if (routine != null)
		{
			Instance.StopCoroutine(routine);
		}
	}

	public static void KillCoroutine([CanBeNull] ref Coroutine routine)
	{
		if (routine != null)
		{
			Instance.StopCoroutine(routine);
			routine = null;
		}
	}

	private void Update()
	{
		TimerManager?.Update();
		_E000?.Invoke();
	}

	private void OnDestroy()
	{
		if (!(_E001 == null))
		{
			_E000 = null;
			_E001.StopAllCoroutines();
			UnityEngine.Object.Destroy(_E001);
			_E001 = null;
		}
	}

	private void OnApplicationQuit()
	{
		OnDestroy();
	}
}
