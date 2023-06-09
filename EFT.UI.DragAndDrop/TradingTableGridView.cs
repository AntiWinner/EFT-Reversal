using System.Threading.Tasks;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT.UI.DragAndDrop;

public class TradingTableGridView : GridView
{
	private _E8AF _E00D;

	public void Show(_E9EF grid, _E8AF traderAssortment, _EAED inventoryController, ItemUiContext itemUiContext)
	{
		_E00D = traderAssortment;
		Show(grid, new _EB64(grid.ParentItem, EItemViewType.TradingSell), inventoryController, itemUiContext);
	}

	public override Task AcceptItem(_EB69 itemContext, _EB68 targetItemContext)
	{
		LocationInGrid location = CalculateItemLocation(itemContext);
		itemContext.DragCancelled();
		_E00D.PrepareToSell(itemContext.Item, location);
		itemContext.CloseDependentWindows();
		return Task.CompletedTask;
	}

	public override bool CanAccept(_EB69 itemContext, _EB68 targetItemContext, out _ECD7 operation)
	{
		if (itemContext == null || !_E00D.CanPrepareItemToSell(itemContext.Item))
		{
			operation = default(_ECD7);
			return false;
		}
		operation = _EB29.Move(itemContext.Item, new _EB22(Grid, CalculateItemLocation(itemContext)), _E00D.TraderController, simulate: true);
		return operation.Succeeded;
	}

	protected override Color GetHighlightColor(_EB69 itemContext, _ECD7 possibleOperation, _EB68 targetItemContext)
	{
		if (targetItemContext == null && _E00D.CanPrepareItemToSell(itemContext.Item))
		{
			return GridView.ValidMoveColor;
		}
		return GridView.InvalidOperationColor;
	}
}
