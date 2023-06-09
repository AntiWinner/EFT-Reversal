using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI.DragAndDrop;

public sealed class EmptyItemView : UIElement, IPointerDownHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	private CanvasGroup _highlight;

	private _EB65 _E133;

	private bool _E2BA;

	public bool Interactable
	{
		get
		{
			return _E2BA;
		}
		set
		{
			_E2BA = value;
			_highlight.interactable = _E2BA;
			_highlight.blocksRaycasts = _E2BA;
		}
	}

	public void Show(_EB65 itemContext)
	{
		_E133 = itemContext.CreateModdingChild(null);
		UI.AddDisposable(_E133);
	}

	void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
	{
		_E133.ToggleSelection();
	}

	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
	{
		_E133.Highlight(selected: true);
		_highlight.alpha = 1f;
	}

	void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
	{
		_E133.Highlight(selected: false);
		_highlight.alpha = 0.8f;
	}

	public override void Close()
	{
		base.Close();
		Object.Destroy(base.gameObject);
	}
}
