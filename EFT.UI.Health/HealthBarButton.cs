using EFT.UI.DragAndDrop;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI.Health;

public sealed class HealthBarButton : UIElement, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IDropHandler, IPointerClickHandler
{
	private static readonly int _E2F5 = Animator.StringToHash(_ED3E._E000(229523));

	private static readonly int _E2F6 = Animator.StringToHash(_ED3E._E000(229511));

	[SerializeField]
	private Animator _animator;

	private EBodyPart _E0AF;

	private _E9C4 _E0AE;

	private ItemUiContext _E2F7;

	public void Show(_E9C4 healthController, EBodyPart bodyPart)
	{
		ShowGameObject();
		_E0AF = bodyPart;
		_E0AE = healthController;
		_E2F7 = ItemUiContext.Instance;
	}

	public void OnPointerEnter([NotNull] PointerEventData eventData)
	{
		ItemView itemView = ((eventData.pointerDrag == null) ? null : eventData.pointerDrag.GetComponent<ItemView>());
		if (!(itemView == null) && _E0AE.CanApplyItem(itemView.Item, _E0AF))
		{
			_animator.SetTrigger(_E2F5);
		}
	}

	public void OnDrop([NotNull] PointerEventData eventData)
	{
		ItemView component = eventData.pointerDrag.GetComponent<ItemView>();
		if (!(component == null))
		{
			_E000();
			_E0AE.ApplyItem(component.Item, _E0AF);
		}
	}

	private void _E000()
	{
		_animator.SetTrigger(_E2F6);
	}

	public void OnPointerExit([NotNull] PointerEventData eventData)
	{
		_E000();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		_ = eventData.button;
		_ = 1;
	}
}
