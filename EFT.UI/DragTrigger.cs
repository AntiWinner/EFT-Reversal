using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI;

public class DragTrigger : MonoBehaviour, IDragHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler
{
	public event Action<PointerEventData> onDrag;

	public event Action<PointerEventData> onBeginDrag;

	public event Action<PointerEventData> onEndDrag;

	public virtual void OnDrag([NotNull] PointerEventData eventData)
	{
		this.onDrag?.Invoke(eventData);
	}

	public virtual void OnBeginDrag([NotNull] PointerEventData eventData)
	{
		this.onBeginDrag?.Invoke(eventData);
	}

	public virtual void OnEndDrag([NotNull] PointerEventData eventData)
	{
		this.onEndDrag?.Invoke(eventData);
	}
}
