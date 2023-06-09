using System;
using System.Runtime.CompilerServices;
using System.Threading;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI;

public sealed class HoverTrigger : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	[CompilerGenerated]
	private Action<PointerEventData> _E000;

	[CompilerGenerated]
	private Action<PointerEventData> _E001;

	public event Action<PointerEventData> OnHoverStart
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

	public event Action<PointerEventData> OnHoverEnd
	{
		[CompilerGenerated]
		add
		{
			Action<PointerEventData> action = _E001;
			Action<PointerEventData> action2;
			do
			{
				action2 = action;
				Action<PointerEventData> value2 = (Action<PointerEventData>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E001, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<PointerEventData> action = _E001;
			Action<PointerEventData> action2;
			do
			{
				action2 = action;
				Action<PointerEventData> value2 = (Action<PointerEventData>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E001, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public void Init([CanBeNull] Action<PointerEventData> onHoverStart, [CanBeNull] Action<PointerEventData> onHoverEnd)
	{
		_E000 = onHoverStart;
		_E001 = onHoverEnd;
	}

	public void OnPointerEnter([NotNull] PointerEventData eventData)
	{
		_E000?.Invoke(eventData);
	}

	public void OnPointerExit([NotNull] PointerEventData eventData)
	{
		_E001?.Invoke(eventData);
	}
}
