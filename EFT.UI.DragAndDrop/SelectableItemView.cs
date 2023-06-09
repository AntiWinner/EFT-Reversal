using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.DragAndDrop;

public class SelectableItemView : BaseSelectableItemView
{
	[SerializeField]
	private Image _selectedMarkBack;

	[SerializeField]
	private GameObject _unavailableBorder;

	private bool _E063;

	private string _E064;

	protected Image SelectedMarkBack => _selectedMarkBack;

	public static SelectableItemView Create(Item item, _EB66 sourceContext, ItemRotation rotation, _EB1E itemController, IItemOwner itemOwner, FilterPanel filterPanel, _EC9E container, ItemUiContext itemUiContext, _ECB1 insurance)
	{
		SelectableItemView selectableItemView = ItemViewFactory.CreateFromPool<SelectableItemView>(_ED3E._E000(236352)).NewSelectableItemView(item, sourceContext, rotation, itemController, itemOwner, filterPanel, container, itemUiContext, insurance);
		selectableItemView.Init();
		return selectableItemView;
	}

	protected SelectableItemView NewSelectableItemView(Item item, _EB66 sourceContext, ItemRotation rotation, _EB1E itemController, IItemOwner itemOwner, FilterPanel filterPanel, _EC9E container, ItemUiContext itemUiContext, _ECB1 insurance)
	{
		NewBaseSelectableItemView(item, sourceContext, rotation, itemController, itemOwner, filterPanel, container, itemUiContext, insurance);
		return this;
	}

	protected override void SetItemSelected(bool selected)
	{
		if (_E063)
		{
			base.SetItemSelected(selected);
		}
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		if (!string.IsNullOrEmpty(_E064))
		{
			ItemUiContext.Tooltip.Show(_E064);
		}
		else
		{
			base.OnPointerEnter(eventData);
		}
	}

	protected override void OnClick(PointerEventData.InputButton button, Vector2 position, bool doubleClick)
	{
		if (_E063 || button != 0)
		{
			base.OnClick(button, position, doubleClick);
		}
		else
		{
			_E857.DisplaySingletonWarningNotification(_E064.Localized());
		}
	}

	protected override void SetAvailability(bool available, string tooltip)
	{
		if (base.Item.IsNotEmpty())
		{
			_E063 = false;
			_E064 = _ED3E._E000(225735).Localized();
		}
		else
		{
			_E063 = available;
			_E064 = tooltip;
		}
		CanvasGroup.SetUnlockStatus(_E063, setRaycast: false);
		_unavailableBorder.SetActive(!_E063);
		_selectedMarkBack.color = (_E063 ? Color.white : Color.red);
	}

	protected override void UpdateScale()
	{
		MainImage.preserveAspect = true;
		RectTransform rectTransform = MainImage.rectTransform;
		rectTransform.pivot = new Vector2(0.5f, 0.5f);
		rectTransform.anchoredPosition = new Vector2(0.5f, 0.5f);
		rectTransform.anchorMin = Vector2.zero;
		rectTransform.anchorMax = Vector2.one;
		rectTransform.offsetMin = Vector2.zero;
		rectTransform.offsetMax = Vector2.zero;
	}
}
