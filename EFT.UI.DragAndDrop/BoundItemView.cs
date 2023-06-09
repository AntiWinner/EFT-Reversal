using System.Collections.Generic;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT.UI.DragAndDrop;

public sealed class BoundItemView : QuickSlotView, _E64A, _E63F, _E64B, _E64C, _E64D
{
	private EBoundItem _E000;

	public void Show(EBoundItem boundItemIndex, _EAED inventoryController, ItemUiContext itemUiContext)
	{
		InventoryController?.UnregisterView(this);
		InventoryController = inventoryController;
		ItemUiContext = itemUiContext;
		InventoryController.RegisterView(this);
		_E000 = boundItemIndex;
		Dictionary<EBoundItem, Item> boundItems = inventoryController.Inventory.FastAccess.BoundItems;
		if (boundItems.ContainsKey(boundItemIndex))
		{
			Item item = boundItems[boundItemIndex];
			ShowInfoPanel(item);
			if (item != null && ItemView == null)
			{
				SetItem(item, inventoryController, itemUiContext);
			}
		}
		HotKey.text = Singleton<_E7DE>.Instance.Control.Settings.GetBoundItemNames(boundItemIndex);
		base.gameObject.SetActive(value: true);
	}

	public override void Hide()
	{
		InventoryController?.UnregisterView(this);
		base.Hide();
	}

	protected override void OnDestroy()
	{
		Hide();
		base.OnDestroy();
	}

	public void OnSetInHands(_EAFA args)
	{
		if (ItemView != null && ItemView.Item == args.Item)
		{
			SwitchVisualSelection(selected: true);
		}
	}

	public void OnRemoveFromHands(_EAFB args)
	{
		if (ItemView != null && ItemView.Item == args.Item)
		{
			SwitchVisualSelection(selected: false);
		}
		else if (args.Item == null)
		{
			SwitchVisualSelection(selected: true);
		}
	}

	public void OnBindItem(_EAFC eventArgs)
	{
		if (eventArgs.Index == _E000 && eventArgs.Status == CommandStatus.Succeed)
		{
			RemoveItemView();
			SetItem(eventArgs.Item, InventoryController, ItemUiContext);
			ShowInfoPanel(eventArgs.Item);
		}
	}

	public void OnUnbindItem(_EAFD eventArgs)
	{
		if (eventArgs.Index != _E000 || eventArgs.Status != CommandStatus.Succeed)
		{
			return;
		}
		if (ItemView != null)
		{
			if (ItemView.Item == eventArgs.Item)
			{
				SwitchVisualSelection(selected: false);
			}
			RemoveItemView();
		}
		else
		{
			Debug.LogError(_ED3E._E000(236825));
		}
		ShowInfoPanel(null);
	}

	public override Task AcceptItem(_EB69 itemContext, _EB68 targetItemContext)
	{
		InventoryController.TryRunNetworkTransaction(_EB4F.Run(InventoryController, itemContext.Item, _E000, simulate: true));
		return Task.CompletedTask;
	}

	public override bool CanDrag(_EB68 itemContext)
	{
		return true;
	}

	public override void RemoveItemViewForced()
	{
		InventoryController.UnbindItem(_E000);
	}

	public override bool CanAccept(_EB69 itemContext, _EB68 targetItemContext, out _ECD7 operation)
	{
		operation = (InventoryController.IsAtBindablePlace(itemContext.Item) ? _EB4F.Run(InventoryController, itemContext.Item, _E000, simulate: true) : ((_ECD8<_EB4F>)new _ECD2(_ED3E._E000(236859))));
		return operation.Succeeded;
	}
}
