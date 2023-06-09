using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI;

public sealed class ScrollTrigger : MonoBehaviour, IScrollHandler, IEventSystemHandler
{
	[CompilerGenerated]
	private Action<PointerEventData> _E000;

	public event Action<PointerEventData> OnOnScroll
	{
		[CompilerGenerated]
		add
		{
			Action<PointerEventData> action = _E000;
			Action<PointerEventData> action2;
			do
			{
				action2 = action;
				Action<PointerEventData> value2 = (Action<PointerEventData>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E000, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<PointerEventData> action = _E000;
			Action<PointerEventData> action2;
			do
			{
				action2 = action;
				Action<PointerEventData> value2 = (Action<PointerEventData>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E000, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public void OnScroll(PointerEventData eventData)
	{
		_E000?.Invoke(eventData);
	}
}
