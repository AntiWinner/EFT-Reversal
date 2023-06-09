using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class OnRenderObjectConnector : MonoBehaviour
{
	[CompilerGenerated]
	private static Action<OnRenderObjectConnector> _E032;

	[CompilerGenerated]
	private static Action<OnRenderObjectConnector> _E033;

	public static event Action<OnRenderObjectConnector> OnRenderObjectConnectorAdd
	{
		[CompilerGenerated]
		add
		{
			Action<OnRenderObjectConnector> action = _E032;
			Action<OnRenderObjectConnector> action2;
			do
			{
				action2 = action;
				Action<OnRenderObjectConnector> value2 = (Action<OnRenderObjectConnector>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E032, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<OnRenderObjectConnector> action = _E032;
			Action<OnRenderObjectConnector> action2;
			do
			{
				action2 = action;
				Action<OnRenderObjectConnector> value2 = (Action<OnRenderObjectConnector>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E032, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public static event Action<OnRenderObjectConnector> OnRenderObjectConnectorRemove
	{
		[CompilerGenerated]
		add
		{
			Action<OnRenderObjectConnector> action = _E033;
			Action<OnRenderObjectConnector> action2;
			do
			{
				action2 = action;
				Action<OnRenderObjectConnector> value2 = (Action<OnRenderObjectConnector>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E033, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<OnRenderObjectConnector> action = _E033;
			Action<OnRenderObjectConnector> action2;
			do
			{
				action2 = action;
				Action<OnRenderObjectConnector> value2 = (Action<OnRenderObjectConnector>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E033, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	protected virtual void Start()
	{
		_E032?.Invoke(this);
	}

	protected virtual void OnDestroy()
	{
		_E033?.Invoke(this);
	}

	public virtual void ManualOnRenderObject(Camera currentCamera)
	{
	}
}
