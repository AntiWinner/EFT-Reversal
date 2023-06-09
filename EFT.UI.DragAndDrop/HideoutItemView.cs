using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI.DragAndDrop;

public sealed class HideoutItemView : StaticGridItemView
{
	private bool _E037;

	public override ItemRotation ItemRotation
	{
		get
		{
			return _itemRotation;
		}
		protected set
		{
			_itemRotation = value;
		}
	}

	public static HideoutItemView Create(Item item, _EB68 sourceContext, ItemRotation rotation, _EB1E itemController, ItemUiContext itemUiContext, _ECB1 insurance, bool modSlotView)
	{
		HideoutItemView hideoutItemView = ItemViewFactory.CreateFromPool<HideoutItemView>(_ED3E._E000(236046));
		hideoutItemView._E000(item, sourceContext, rotation, itemController, itemUiContext, insurance, modSlotView);
		return hideoutItemView;
	}

	private void _E000(Item item, _EB68 sourceContext, ItemRotation rotation, _EB1E itemController, ItemUiContext itemUiContext, _ECB1 insurance, bool modSlotView)
	{
		NewGridItemView(item, sourceContext, rotation, itemController, null, null, null, itemUiContext, insurance);
		_E037 = modSlotView;
		Init();
	}

	public override void UpdateRemoveError(bool ignoreMalfunctions = true)
	{
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

	public override void UpdateInfo()
	{
		base.UpdateInfo();
		Caption.gameObject.SetActive(_E037);
		ItemInscription.gameObject.SetActive(value: false);
		base.SecureIcon.gameObject.SetActive(value: false);
		base.LockedIcon.gameObject.SetActive(value: false);
		base.TogglableIcon.gameObject.SetActive(value: false);
		ItemValue.gameObject.SetActive(value: false);
		if (ItemOwner == null)
		{
			return;
		}
		foreach (_EAF6 item in ItemOwner.SelectEvents<_EAF6>(base.Item))
		{
			SetBeingExaminedState(item);
		}
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		Highlight(highlight: true);
		ShowTooltip();
	}

	protected override void OnClick(PointerEventData.InputButton button, Vector2 position, bool doubleClick)
	{
		switch (button)
		{
		case PointerEventData.InputButton.Left:
			if (doubleClick)
			{
				base.NewContextInteractions.ExecuteInteraction(EItemInfoButton.Inspect);
			}
			break;
		case PointerEventData.InputButton.Middle:
			base.NewContextInteractions.ExecuteInteraction(EItemInfoButton.Examine);
			break;
		case PointerEventData.InputButton.Right:
			ShowContextMenu(position);
			break;
		}
	}
}
