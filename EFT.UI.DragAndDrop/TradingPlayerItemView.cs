using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.InventoryLogic;
using UnityEngine.EventSystems;

namespace EFT.UI.DragAndDrop;

public sealed class TradingPlayerItemView : TradingItemView
{
	protected override _E8B2._E000? ItemPrice => Trader.GetUserItemPrice(base.Item);

	public new static TradingPlayerItemView Create(Item item, _EB68 sourceContext, ItemRotation rotation, _E8B2 trader, _EB1E inventoryController, IItemOwner itemOwner, FilterPanel filterPanel, _EC9E container, TraderDealScreen.ETraderMode traderMode, bool canSelect, bool canDrag, ETradingItemViewType itemViewType, ItemUiContext itemUiContext, _ECB1 insurance, bool modSlotView)
	{
		TradingPlayerItemView tradingPlayerItemView = ItemViewFactory.CreateFromPool<TradingPlayerItemView>(_ED3E._E000(230407));
		tradingPlayerItemView.NewTradingItemView(item, sourceContext, rotation, trader, inventoryController, itemOwner, filterPanel, container, traderMode, canSelect, canDrag, itemViewType, itemUiContext, insurance, modSlotView);
		tradingPlayerItemView.Init();
		return tradingPlayerItemView;
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		HideTooltip();
		base.OnPointerEnter(eventData);
	}

	protected override void UpdateScheme()
	{
		SetPrice(Trader.GetUserItemPrice(base.Item));
	}

	protected override void RegisterItemView()
	{
		base.RegisterItemView();
		ItemController.AddItemEvent += _E000;
		ItemController.RemoveItemEvent += _E001;
		ItemController.RefreshItemEvent += _E002;
		CompositeDisposable.AddDisposable(delegate
		{
			ItemController.AddItemEvent -= _E000;
			ItemController.RemoveItemEvent -= _E001;
			ItemController.RefreshItemEvent -= _E002;
		});
	}

	private void _E000(_EAF2 args)
	{
		if (args.Status == CommandStatus.Succeed)
		{
			_E003(args.Item);
		}
	}

	private void _E001(_EAF3 args)
	{
		if (args.Status == CommandStatus.Succeed)
		{
			_E003(args.Item);
		}
	}

	private void _E002(_EAFF args)
	{
		_E003(args.Item);
	}

	private void _E003(Item item)
	{
		if (base.Item is ContainerCollection container && container.Contains(item))
		{
			UpdateStaticInfo();
		}
	}

	protected override void ShowTooltip()
	{
		string text = (Examined ? Singleton<_E63B>.Instance.BriefItemName(base.Item, base.Item.Name.Localized()) : _ED3E._E000(193009).Localized());
		if (Trader.Info.CanBuyRootItem(base.Item, out var unsellableParts) && unsellableParts.Count > 0)
		{
			ItemUiContext.MultiLineTooltip.Show(new _EC80(_E004(base.Item, unsellableParts)));
		}
		else if (ModSlotView)
		{
			ItemUiContext.Tooltip.Show(text);
		}
		else
		{
			base.ShowTooltip();
		}
	}

	private _EC7F[] _E004(Item rootItem, IEnumerable<Item> unsellableParts)
	{
		string label = ((ItemController == null || ItemController.Examined(rootItem)) ? Singleton<_E63B>.Instance.BriefItemName(rootItem, rootItem.Name.Localized()) : _ED3E._E000(193009).Localized());
		return unsellableParts.Select((Item item) => new _EC7F((ItemController == null || ItemController.Examined(item)) ? Singleton<_E63B>.Instance.BriefItemName(item, item.Name.Localized()) : _ED3E._E000(193009).Localized(), _ED3E._E000(103088) + _ED3E._E000(248286).Localized() + _ED3E._E000(59467))).Prepend(null).Prepend(new _EC7F(label, ""))
			.ToArray();
	}

	protected override void HideTooltip()
	{
		base.HideTooltip();
		ItemUiContext.MultiLineTooltip.Close();
	}

	[CompilerGenerated]
	private void _E005()
	{
		ItemController.AddItemEvent -= _E000;
		ItemController.RemoveItemEvent -= _E001;
		ItemController.RefreshItemEvent -= _E002;
	}

	[CompilerGenerated]
	private _EC7F _E006(Item item)
	{
		return new _EC7F((ItemController == null || ItemController.Examined(item)) ? Singleton<_E63B>.Instance.BriefItemName(item, item.Name.Localized()) : _ED3E._E000(193009).Localized(), _ED3E._E000(103088) + _ED3E._E000(248286).Localized() + _ED3E._E000(59467));
	}
}
