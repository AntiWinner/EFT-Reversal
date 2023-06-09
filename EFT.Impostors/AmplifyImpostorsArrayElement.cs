using System;
using System.Runtime.CompilerServices;
using System.Threading;
using EFT.SpeedTree;
using UnityEngine;

namespace EFT.Impostors;

[RequireComponent(typeof(TreeWind))]
[ExecuteInEditMode]
[DisallowMultipleComponent]
public sealed class AmplifyImpostorsArrayElement : MonoBehaviour
{
	[CompilerGenerated]
	private static Action<AmplifyImpostorsArrayElement> _E000;

	[CompilerGenerated]
	private static Action<AmplifyImpostorsArrayElement> _E001;

	[CompilerGenerated]
	private static Action<AmplifyImpostorsArrayElement, bool> _E002;

	private TreeWind _E003;

	public uint TypeId;

	public _E8DB ImpostorWind
	{
		get
		{
			TreeWind.Settings settings = _E003.settings;
			_E8DB result = default(_E8DB);
			result._ST_WindGlobal = settings.BaseMinWindData._ST_WindGlobal;
			result._ST_WindBranchAdherences = settings.BaseMinWindData._ST_WindBranchAdherences;
			return result;
		}
	}

	public static event Action<AmplifyImpostorsArrayElement> AddedEvent
	{
		[CompilerGenerated]
		add
		{
			Action<AmplifyImpostorsArrayElement> action = _E000;
			Action<AmplifyImpostorsArrayElement> action2;
			do
			{
				action2 = action;
				Action<AmplifyImpostorsArrayElement> value2 = (Action<AmplifyImpostorsArrayElement>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E000, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<AmplifyImpostorsArrayElement> action = _E000;
			Action<AmplifyImpostorsArrayElement> action2;
			do
			{
				action2 = action;
				Action<AmplifyImpostorsArrayElement> value2 = (Action<AmplifyImpostorsArrayElement>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E000, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public static event Action<AmplifyImpostorsArrayElement> RemovedEvent
	{
		[CompilerGenerated]
		add
		{
			Action<AmplifyImpostorsArrayElement> action = _E001;
			Action<AmplifyImpostorsArrayElement> action2;
			do
			{
				action2 = action;
				Action<AmplifyImpostorsArrayElement> value2 = (Action<AmplifyImpostorsArrayElement>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E001, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<AmplifyImpostorsArrayElement> action = _E001;
			Action<AmplifyImpostorsArrayElement> action2;
			do
			{
				action2 = action;
				Action<AmplifyImpostorsArrayElement> value2 = (Action<AmplifyImpostorsArrayElement>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E001, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public static event Action<AmplifyImpostorsArrayElement, bool> EnableChangedEvent
	{
		[CompilerGenerated]
		add
		{
			Action<AmplifyImpostorsArrayElement, bool> action = _E002;
			Action<AmplifyImpostorsArrayElement, bool> action2;
			do
			{
				action2 = action;
				Action<AmplifyImpostorsArrayElement, bool> value2 = (Action<AmplifyImpostorsArrayElement, bool>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E002, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<AmplifyImpostorsArrayElement, bool> action = _E002;
			Action<AmplifyImpostorsArrayElement, bool> action2;
			do
			{
				action2 = action;
				Action<AmplifyImpostorsArrayElement, bool> value2 = (Action<AmplifyImpostorsArrayElement, bool>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E002, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public static bool FilterTreeImposter(Renderer renderer)
	{
		if (renderer == null)
		{
			return false;
		}
		AmplifyImpostorsArrayElement component;
		return !renderer.gameObject.TryGetComponent<AmplifyImpostorsArrayElement>(out component);
	}

	private void Awake()
	{
		_E003 = base.gameObject.GetComponent<TreeWind>();
		_E000?.Invoke(this);
	}

	private void OnEnable()
	{
		_E002?.Invoke(this, arg2: true);
	}

	private void OnDisable()
	{
		_E002?.Invoke(this, arg2: false);
	}

	private void OnDestroy()
	{
		_E001?.Invoke(this);
	}
}
