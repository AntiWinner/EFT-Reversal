using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI.DragAndDrop;

public sealed class AssembleBuildItemView : StaticGridItemView
{
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

	public void Show(Item item, ItemRotation rotation, _EAED inventoryController, ItemUiContext itemUiContext, _ECB1 insurance)
	{
		NewGridItemView(item, new _EB62(EItemViewType.WeaponModdingSimple), rotation, inventoryController, inventoryController, null, null, itemUiContext, insurance).Init();
		foreach (_EAF6 item2 in ItemOwner.SelectEvents<_EAF6>(base.Item))
		{
			SetBeingExaminedState(item2);
		}
	}

	protected override void UpdateScale()
	{
		Vector2 sizeDelta = MainImage.rectTransform.sizeDelta;
		float x = sizeDelta.x;
		float y = sizeDelta.y;
		float num = Mathf.Min(64f / x, 64f / y);
		MainImage.rectTransform.sizeDelta = new Vector2(x * num, y * num);
	}

	protected override string GetErrorText()
	{
		return null;
	}

	protected override void SetItemValue(EItemValueFormat format, bool display, string color, object arg1, object arg2 = null, string color2 = null)
	{
	}

	public override void UpdateInfo()
	{
		base.UpdateInfo();
		Caption.gameObject.SetActive(value: false);
		ItemInscription.gameObject.SetActive(value: false);
		base.SecureIcon.gameObject.SetActive(value: false);
		base.LockedIcon.gameObject.SetActive(value: false);
		base.TogglableIcon.gameObject.SetActive(value: false);
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
