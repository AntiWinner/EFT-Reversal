using Comfort.Common;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI;

public class ButtonFeedback : InteractableElement, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	[SerializeField]
	private bool _onPointerEnterSound = true;

	[SerializeField]
	private bool _onPointerClickSound = true;

	public virtual void OnPointerEnter([NotNull] PointerEventData eventData)
	{
		if (Interactable)
		{
			if (_onPointerEnterSound && Singleton<GUISounds>.Instantiated)
			{
				Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.ButtonOver);
			}
			_EC45.SetCursor(ECursorType.Hover);
		}
	}

	public virtual void OnPointerExit([NotNull] PointerEventData eventData)
	{
		if (Interactable)
		{
			_EC45.SetCursor(ECursorType.Idle);
		}
	}

	public virtual void OnPointerClick([NotNull] PointerEventData eventData)
	{
		if (Interactable)
		{
			if (_onPointerClickSound && Singleton<GUISounds>.Instantiated)
			{
				Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.ButtonClick);
			}
			_EC45.SetCursor(ECursorType.Idle);
		}
	}
}
