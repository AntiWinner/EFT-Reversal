using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI;

public sealed class ClickTrigger : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	private Action<PointerEventData> _E000;

	public void Init(Action<PointerEventData> onClick)
	{
		_E000 = onClick;
	}

	public void OnPointerClick([NotNull] PointerEventData eventData)
	{
		_E000?.Invoke(eventData);
	}
}
