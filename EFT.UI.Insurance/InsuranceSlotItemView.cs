using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.Insurance;

public class InsuranceSlotItemView : SlotItemView
{
	[SerializeField]
	private Image _itemToInsureBorder;

	private static _ECB1 _E034;

	private _ECB4 _E06E;

	private bool _E000 => _E034.Insured(base.Item.Id);

	private new bool _E001 => _E034.InInsuranceQueue(base.Item.Id);

	public new static SlotItemView Create(Item item, _EB68 sourceContext, _EAED inventoryController, IItemOwner itemOwner, ItemUiContext itemUiContext, _E74F skills, _ECB1 insurance)
	{
		InsuranceSlotItemView insuranceSlotItemView = ItemViewFactory.CreateFromPool<InsuranceSlotItemView>(_ED3E._E000(232981))._E000(_ECB4.FindOrCreate(item), sourceContext, ItemRotation.Horizontal, inventoryController, itemOwner, itemUiContext, skills, insurance);
		insuranceSlotItemView.Init();
		return insuranceSlotItemView;
	}

	private InsuranceSlotItemView _E000(_ECB4 insuredItem, _EB68 sourceContext, ItemRotation rotation, _EAED inventoryController, IItemOwner itemOwner, ItemUiContext itemUiContext, _E74F skills, _ECB1 insurance)
	{
		_E034 = insurance;
		_E06E = insuredItem;
		NewSlotItemView(insuredItem.Item, sourceContext, rotation, inventoryController, itemOwner, itemUiContext, skills, insurance);
		CompositeDisposable.SubscribeEvent(insurance.OnItemCovered, _E001);
		CompositeDisposable.SubscribeEvent(insurance.OnItemUncovered, _E001);
		_E001(_E06E);
		ChangeInsuredStatus(_E06E);
		ChangeRepairBuffStatus();
		CanvasGroup.SetUnlockStatus(insurance.ItemTypeAvailableForInsurance(insuredItem), setRaycast: false);
		return this;
	}

	protected override _EB68 CreateNewItemContext(_EB68 sourceContext)
	{
		return new _ECB2(_E06E, sourceContext);
	}

	protected override void OnClick(PointerEventData.InputButton button, Vector2 position, bool doubleClick)
	{
		switch (button)
		{
		case PointerEventData.InputButton.Left:
			if (Examined)
			{
				if (this._E001)
				{
					_E034.RemoveItemFromInsuranceQueue(_E06E);
				}
				else
				{
					_E034.AddItemToInsuranceQueue(_E06E);
				}
			}
			else
			{
				_E857.DisplayWarningNotification(_ED3E._E000(232936).Localized());
			}
			break;
		case PointerEventData.InputButton.Middle:
			ExecuteMiddleClick();
			break;
		case PointerEventData.InputButton.Right:
			ShowContextMenu(position);
			break;
		}
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		if (_E034.ItemTypeAvailableForInsurance(_E06E))
		{
			base.OnPointerEnter(eventData);
			if (ItemUiContext.Tooltip != null)
			{
				ItemUiContext.Tooltip.Show(Examined ? base.Item.Name.Localized() : _ED3E._E000(193009).Localized(), null, 0.5f);
			}
			else
			{
				Debug.LogWarning(_ED3E._E000(235956) + base.name + _ED3E._E000(261694));
			}
			if (!this._E001 && !this._E000)
			{
				_itemToInsureBorder.color = new Color(1f, 1f, 1f, 0.5f);
			}
		}
		else if (ItemUiContext.Tooltip != null)
		{
			ItemUiContext.Tooltip.Show(_ED3E._E000(232900).Localized());
		}
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		base.OnPointerExit(eventData);
		if (!this._E001 && !this._E000)
		{
			_itemToInsureBorder.color = new Color(1f, 1f, 1f, 0f);
		}
	}

	private void _E001(_ECB4 item)
	{
		if (item == _E06E)
		{
			_itemToInsureBorder.color = new Color(1f, 1f, 1f, (this._E001 || this._E000) ? 1f : 0f);
		}
	}
}
