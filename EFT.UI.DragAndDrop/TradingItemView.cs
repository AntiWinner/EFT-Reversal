using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using Diz.Binding;
using EFT.InventoryLogic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.DragAndDrop;

public class TradingItemView : GridItemView
{
	[SerializeField]
	private Image _schemeIcon;

	[SerializeField]
	private TextMeshProUGUI _currency;

	[SerializeField]
	private TextMeshProUGUI _price;

	[SerializeField]
	private GameObject _selectedBorder;

	[SerializeField]
	private GameObject _selectedCorner;

	[SerializeField]
	private GameObject _prepareBorder;

	[SerializeField]
	private GameObject _barterIcon;

	[SerializeField]
	private GameObject _outOfStockPanel;

	[SerializeField]
	private TextMeshProUGUI _outOfStockText;

	protected bool ModSlotView;

	protected _E8B2 Trader;

	private _E8AF _E065;

	private bool _E059;

	private bool _E066;

	private TraderDealScreen.ETraderMode _E067;

	private ETradingItemViewType _E068;

	private EOwnerType _E069;

	private readonly _ECF5<bool> _E06A = new _ECF5<bool>();

	private readonly _ECF5<bool> _E06B = new _ECF5<bool>();

	private readonly _ECF5<bool> _E06C = new _ECF5<bool>();

	private readonly _ECF5<bool> _E06D = new _ECF5<bool>();

	protected virtual _E8B2._E000? ItemPrice => _E065.GetBarterPrice(base.Item);

	private bool _E000 => Transparency.Value < 1f;

	protected override IBindable<float> Transparency => _ECF3.Combine(base.Transparency, _E06A, _E06B, _E06C, _E06D, (float baseTransparent, bool isBeingSold, bool hasNoScheme, bool outOfStock, bool priceToLow) => (!(isBeingSold || hasNoScheme || priceToLow)) ? ((!outOfStock) ? baseTransparent : 0.5f) : 0.25f);

	protected override void InitInteractiveBinding()
	{
		IsInteractive = _ECF3.Combine(base.IsFilteredOut, base.IsBeingDragged, base.IsBeingAdded, base.IsBeingDrained, base.IsBeingRemoved, base.IsBeingSearched, _E06A, (bool filtered, bool dragged, bool added, bool drained, bool removed, bool searched, bool sold) => !filtered && !dragged && !added && !drained && !removed && !searched && !sold);
	}

	public static TradingItemView Create(Item item, _EB68 sourceContext, ItemRotation rotation, _E8B2 trader, _EB1E itemController, IItemOwner itemOwner, FilterPanel filterPanel, _EC9E container, TraderDealScreen.ETraderMode traderMode, bool canSelect, bool canDrag, ETradingItemViewType itemViewType, ItemUiContext itemUiContext, _ECB1 insurance, bool modSlotView)
	{
		TradingItemView tradingItemView = ItemViewFactory.CreateFromPool<TradingItemView>(_ED3E._E000(236453)).NewTradingItemView(item, sourceContext, rotation, trader, itemController, itemOwner, filterPanel, container, traderMode, canSelect, canDrag, itemViewType, itemUiContext, insurance, modSlotView);
		tradingItemView.Init();
		return tradingItemView;
	}

	protected TradingItemView NewTradingItemView(Item item, _EB68 sourceContext, ItemRotation rotation, _E8B2 trader, _EB1E itemController, IItemOwner itemOwner, FilterPanel filterPanel, _EC9E container, TraderDealScreen.ETraderMode traderMode, bool canSelect, bool canDrag, ETradingItemViewType itemViewType, ItemUiContext itemUiContext, _ECB1 insurance, bool modSlotView)
	{
		NewGridItemView(item, sourceContext, rotation, itemController, itemOwner, filterPanel, container, itemUiContext, insurance);
		_E069 = itemOwner.OwnerType;
		Trader = trader;
		_E065 = Trader.CurrentAssortment;
		_E067 = traderMode;
		_E059 = canSelect && traderMode == TraderDealScreen.ETraderMode.Purchase;
		_E066 = canDrag;
		ModSlotView = modSlotView;
		_E068 = itemViewType;
		if (_E065 == null)
		{
			return this;
		}
		CompositeDisposable.BindEvent(_E065.SelectedItemChanged, _E000);
		CompositeDisposable.BindEvent(_E065.AssortmentUpdated, _E001);
		CompositeDisposable.BindEvent(_E065.PreparedItemsChanged, delegate
		{
			_E06A.Value = _E065.IsBeingSold(base.Item);
		});
		CompositeDisposable.BindState(_E06A, delegate(bool x)
		{
			_barterIcon.SetActive(x);
		});
		return this;
	}

	public override void UpdateRemoveError(bool ignoreMalfunctions = true)
	{
		if (base.Item.Owner != null)
		{
			base.UpdateRemoveError(ignoreMalfunctions);
		}
	}

	public void SetPrepareBorder(bool set)
	{
		if (!(this == null) && _prepareBorder.activeSelf != set)
		{
			_prepareBorder.SetActive(set);
		}
	}

	private void _E000()
	{
		bool active = _E065.SelectedItem == base.Item && _E067 == TraderDealScreen.ETraderMode.Purchase;
		_selectedBorder.SetActive(active);
		_selectedCorner.SetActive(active);
	}

	private void _E001()
	{
		if (!IsKilled)
		{
			UpdateInfo();
		}
	}

	protected virtual void UpdateScheme()
	{
		SetPrice(ItemPrice);
	}

	protected void SetPrice(_E8B2._E000? price)
	{
		bool hasValue = price.HasValue;
		int num = price?.Amount ?? 0;
		_E06B.Value = !hasValue;
		_E06D.Value = hasValue && num < 1;
		bool flag = !hasValue || num < 1;
		_currency.gameObject.SetActive(!flag);
		_schemeIcon.gameObject.SetActive(!flag);
		_price.gameObject.SetActive(!flag);
		CanvasGroup.SetUnlockStatus(!this._E000, setRaycast: false);
		if (!flag)
		{
			_price.text = ((base.Item.CalculateRotatedSize(_itemRotation).X == 1) ? num.ToStringReduceThousands() : num.ToString());
			string currencyId = price.Value.CurrencyId;
			ECurrencyType type;
			bool flag2 = _EA10.TryGetCurrencyType(currencyId, out type);
			if (!flag2)
			{
				_schemeIcon.sprite = EFTHardSettings.Instance.StaticIcons.GetSmallCurrencySign(currencyId);
				_schemeIcon.SetNativeSize();
			}
			else
			{
				_currency.text = _EA10.GetCurrencyChar(type);
			}
			_currency.gameObject.SetActive(flag2);
			_schemeIcon.gameObject.SetActive(!flag2);
		}
	}

	protected override void UpdateStaticInfo()
	{
		base.UpdateStaticInfo();
		_prepareBorder.SetActive(value: false);
		UpdateScheme();
	}

	public override void UpdateInfo()
	{
		base.UpdateInfo();
		UpdateScheme();
		if (base.Item is _EA40 item)
		{
			_E002(item);
		}
		if (!(base.Item is Weapon) && !ModSlotView)
		{
			Caption.gameObject.SetActive(value: false);
		}
		GameObject gameObject = ItemValue.gameObject;
		gameObject.SetActive(value: false);
		if (!base.IsBeingExamined.Value)
		{
			gameObject.SetActive(value: true);
		}
		if (base.Item.StackObjectsCount > 1)
		{
			gameObject.SetActive(value: true);
		}
		ResourceComponent itemComponent = base.Item.GetItemComponent<ResourceComponent>();
		if (itemComponent != null && itemComponent.MaxResource > 0f)
		{
			gameObject.SetActive(value: true);
		}
		_E06C.Value = false;
		SetCountValue();
	}

	protected override void SetCountValue()
	{
		bool flag = false;
		if (base.Item.UnlimitedCount)
		{
			SetItemValue(EItemValueFormat.OneValue, display: true, GridItemView.GetStackColor(base.Item), _ED3E._E000(254994).Localized());
		}
		else if (base.Item.IsEmptyStack)
		{
			flag = true;
			SetItemValue(EItemValueFormat.OneValue, display: true, _ED3E._E000(236500), string.Empty);
			_E313 obj = base.Item.CalculateCellSize();
			int num = ((ItemRotation == ItemRotation.Horizontal) ? obj.X : obj.Y);
			_outOfStockText.text = ((num > 1) ? _ED3E._E000(236494).Localized() : _ED3E._E000(27314));
		}
		else if (base.Item.StackObjectsCount != 1)
		{
			SetItemValue(EItemValueFormat.OneValue, display: true, GridItemView.GetStackColor(base.Item), base.Item.StackObjectsCount);
		}
		_E06C.Value = flag;
		_outOfStockPanel.SetActive(flag);
	}

	private void _E002(_EA40 item)
	{
		if (!item.Grids.SelectMany((_E9EF grid) => grid.ContainedItems).All((KeyValuePair<Item, LocationInGrid> containedItem) => containedItem.Key == null))
		{
			CanvasGroup.SetUnlockStatus(value: false, setRaycast: false);
		}
	}

	protected override void OnClick(PointerEventData.InputButton button, Vector2 position, bool doubleClick)
	{
		switch (button)
		{
		case PointerEventData.InputButton.Middle:
			ExecuteMiddleClick();
			break;
		case PointerEventData.InputButton.Right:
			if (_E068 == ETradingItemViewType.TradingTable)
			{
				HideTooltip();
				_E065.UnprepareSellItem(base.Item);
			}
			else
			{
				ShowContextMenu(position);
			}
			break;
		case PointerEventData.InputButton.Left:
		{
			if (_E068 == ETradingItemViewType.TradingTable)
			{
				break;
			}
			bool flag = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
			if (!(!flag && doubleClick) || !base.NewContextInteractions.ExecuteInteraction(EItemInfoButton.Inspect))
			{
				if (flag && _E067 == TraderDealScreen.ETraderMode.Sale && _E065.QuickFindTradingAppropriatePlace(base.Item))
				{
					base.ItemContext.CloseDependentWindows();
					HideTooltip();
					Singleton<GUISounds>.Instance.PlayItemSound(base.Item.ItemSound, EInventorySoundType.pickup);
				}
				if (_E059)
				{
					_E065.SelectItem(base.Item);
				}
			}
			break;
		}
		}
	}

	public override void OnBeginDrag(PointerEventData eventData)
	{
		if (_E066)
		{
			base.OnBeginDrag(eventData);
		}
	}

	public override void OnDrag(PointerEventData eventData)
	{
		if (_E066)
		{
			base.OnDrag(eventData);
		}
	}

	public override void OnEndDrag(PointerEventData eventData)
	{
		if (_E066)
		{
			base.OnEndDrag(eventData);
		}
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		Highlight(highlight: true);
		base.OnPointerEnter(eventData);
	}

	protected override void ShowTooltip()
	{
		_E8B2._E000 obj = ItemPrice ?? default(_E8B2._E000);
		string text = (Examined ? Singleton<_E63B>.Instance.BriefItemName(base.Item, base.Item.Name.Localized()) : _ED3E._E000(193009).Localized());
		if (ModSlotView)
		{
			text = _ED3E._E000(103088) + _ED3E._E000(236483).Localized() + _ED3E._E000(247208) + text;
			ItemUiContext.Tooltip.Show(text);
			return;
		}
		if (_E06C.Value)
		{
			text = text + _ED3E._E000(236535) + _ED3E._E000(236494).Localized().ToLower() + _ED3E._E000(163322);
		}
		else if (_E06D.Value)
		{
			text = text + _ED3E._E000(236517) + _ED3E._E000(230426).Localized() + _ED3E._E000(59467);
		}
		ItemUiContext.PriceTooltip.Show(_E069, text, obj.Amount, obj.CurrencyId);
	}

	protected override void HideTooltip()
	{
		ItemUiContext.PriceTooltip.Close();
		ItemUiContext.Tooltip.Close();
	}

	[CompilerGenerated]
	private void _E003()
	{
		_E06A.Value = _E065.IsBeingSold(base.Item);
	}

	[CompilerGenerated]
	private void _E004(bool x)
	{
		_barterIcon.SetActive(x);
	}
}
