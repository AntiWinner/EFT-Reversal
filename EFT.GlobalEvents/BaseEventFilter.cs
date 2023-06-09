using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace EFT.GlobalEvents;

public abstract class BaseEventFilter : MonoBehaviour
{
	[CompilerGenerated]
	private Action<BaseEventFilter, _EBAD> m__E000;

	public event Action<BaseEventFilter, _EBAD> OnFilterPassed
	{
		[CompilerGenerated]
		add
		{
			Action<BaseEventFilter, _EBAD> action = this.m__E000;
			Action<BaseEventFilter, _EBAD> action2;
			do
			{
				action2 = action;
				Action<BaseEventFilter, _EBAD> value2 = (Action<BaseEventFilter, _EBAD>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<BaseEventFilter, _EBAD> action = this.m__E000;
			Action<BaseEventFilter, _EBAD> action2;
			do
			{
				action2 = action;
				Action<BaseEventFilter, _EBAD> value2 = (Action<BaseEventFilter, _EBAD>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	private void Awake()
	{
		_EBAF.OnEventInvoked += _E000;
	}

	private void OnDestroy()
	{
		_EBAF.OnEventInvoked -= _E000;
		this.m__E000 = null;
	}

	private void _E000(_EBAD invokedEvent)
	{
		if (IsFilterPassed(invokedEvent))
		{
			this.m__E000?.Invoke(this, invokedEvent);
		}
	}

	protected abstract bool IsFilterPassed(_EBAD eventToFilter);
}
