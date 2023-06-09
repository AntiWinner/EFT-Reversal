using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.Insurance;

public sealed class InsuranceItemView : GridItemView
{
	[SerializeField]
	private Image _itemToInsureBorder;

	private _ECB1 _E034;

	private _ECB4 _E06E;

	private bool _E000 => _E034.Insured(base.Item.Id);

	private new bool _E001 => _E034.InInsuranceQueue(base.Item.Id);

	public static InsuranceItemView Create(Item item, _EB68 sourceContext, ItemRotation rotation, _EB1E itemController, IItemOwner itemOwner, [CanBeNull] FilterPanel filterPanel, [CanBeNull] _EC9E container, ItemUiContext itemUiContext, _ECB1 insurance)
	{
		InsuranceItemView insuranceItemView = ItemViewFactory.CreateFromPool<InsuranceItemView>(_ED3E._E000(232923))._E000(_ECB4.FindOrCreate(item), sourceContext, rotation, itemOwner, itemController, filterPanel, container, itemUiContext, insurance);
		insuranceItemView.Init();
		return insuranceItemView;
	}

	private InsuranceItemView _E000(_ECB4 insuredItem, _EB68 sourceContext, ItemRotation rotation, IItemOwner itemOwner, _EB1E itemController, [CanBeNull] FilterPanel filterPanel, [CanBeNull] _EC9E container, ItemUiContext itemUiContext, _ECB1 insurance)
	{
		_E034 = insurance;
		_E06E = insuredItem;
		NewGridItemView(insuredItem.Item, sourceContext, rotation, itemController, itemOwner, filterPanel, container, itemUiContext, insurance);
		CompositeDisposable.SubscribeEvent(insurance.OnItemCovered, _E001);
		CompositeDisposable.SubscribeEvent(insurance.OnItemUncovered, _E001);
		_E001(insuredItem);
		CanvasGroup.SetUnlockStatus(_E034.ItemTypeAvailableForInsurance(insuredItem), setRaycast: false);
		return this;
	}

	protected override _EB68 CreateNewItemContext(_EB68 sourceContext)
	{
		return new _ECB2(_E06E, sourceContext);
	}

	private void _E001(_ECB4 item)
	{
		if (item == _E06E)
		{
			_itemToInsureBorder.color = new Color(1f, 1f, 1f, (this._E001 || this._E000) ? 1f : 0f);
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

	protected override void OnClick(PointerEventData.InputButton button, Vector2 position, bool doubleClick)
	{
		switch (button)
		{
		case PointerEventData.InputButton.Left:
			if (Examined)
			{
				if (!this._E001)
				{
					_E034.AddItemToInsuranceQueue(_E06E);
				}
				else
				{
					_E034.RemoveItemFromInsuranceQueue(_E06E);
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
}
