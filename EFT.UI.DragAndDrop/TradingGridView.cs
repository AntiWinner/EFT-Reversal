using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using EFT.InventoryLogic;
using EFT.Trading;
using UnityEngine;

namespace EFT.UI.DragAndDrop;

public sealed class TradingGridView : GridView
{
	[CompilerGenerated]
	private Action _E00C;

	private _E8AF _E00D;

	private _EAA0 _E00E;

	private new IEnumerable<Item> _E00F;

	public event Action OnFilterChanged
	{
		[CompilerGenerated]
		add
		{
			Action action = _E00C;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E00C, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E00C;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E00C, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public void Show(_E9EF grid, _EB68 sourceContext, _E8AF assortment, _EAED inventoryController, ItemUiContext itemUiContext)
	{
		Grid = grid;
		_E000(grid.Items, assortment);
		_filterPanel.PreInit(_E00D, null);
		Show(Grid, sourceContext, inventoryController, itemUiContext);
		_E001();
	}

	public void Show(IEnumerable<Item> items, _EB68 sourceContext, _E8AF assortment, _EAED inventoryController, ItemUiContext itemUiContext, bool raiseEvents = true)
	{
		_E000(items, assortment);
		_E00E = (_EAA0)assortment.TraderController.RootItem;
		_filterPanel.PreInit(_E00D, _E00F);
		_filterPanel.Init();
		_E003();
		if (raiseEvents && _filterPanel.RememberChoice)
		{
			_E004();
		}
		Show(Grid, sourceContext, inventoryController, itemUiContext);
		_E001();
	}

	private void _E000(IEnumerable<Item> items, _E8AF assortment)
	{
		_E00F = items;
		_E00D = assortment;
		_filterPanel.CurrentFilterChanged += _E002;
		_filterPanel.CurrentFilterChanged += _E004;
	}

	private void _E001()
	{
		if (!_filterPanel.RememberChoice)
		{
			_filterPanel.RememberChoice = true;
		}
		_E005();
	}

	private new void _E002()
	{
		_E003();
		MagnifyIfPossible();
	}

	private void _E003()
	{
		if (_E00E == null)
		{
			return;
		}
		Grid = new _E9F5(_ED3E._E000(124538), 10, 25, canStretchVertically: true, Array.Empty<ItemFilter>(), _E00E);
		List<Item> list = _EB1B.Sort((_filterPanel != null) ? _filterPanel.GetFilteredItems(_E00F) : _E00F);
		foreach (Item item in list)
		{
			Grid.Add(item);
		}
		if (_E00E == null)
		{
			throw new Exception(_ED3E._E000(237099));
		}
		foreach (ItemView item2 in base._E002)
		{
			if (list.Contains(item2.Item))
			{
				item2.gameObject.SetActive(value: true);
				LocationInGrid locationInGrid = Grid.ItemCollection[item2.Item];
				if (item2.ItemRotation != locationInGrid.r)
				{
					item2.Rotate();
				}
				SetItemViewPosition(item2, Grid.ItemCollection[item2.Item]);
			}
			else
			{
				item2.gameObject.SetActive(value: false);
			}
		}
	}

	protected override void PrepareItems()
	{
		if (_filterPanel == null)
		{
			return;
		}
		foreach (Item item in _E00F)
		{
			_filterPanel.RegisterItem(item);
		}
	}

	private void _E004()
	{
		_E00C?.Invoke();
	}

	private void _E005()
	{
		HashSet<Item> hashSet = ((_E00D.CurrentRequisites != null) ? new HashSet<Item>(from x in _E00D.CurrentRequisites.SelectMany((_E8B1 x) => x.PreparedItems)
			select x.Item) : null);
		foreach (ItemView item in base._E002)
		{
			((TradingItemView)item).SetPrepareBorder(hashSet?.Contains(item.Item) ?? false);
		}
	}

	public override bool CanAccept(_EB69 itemContext, _EB68 targetItemContext, out _ECD7 operation)
	{
		if (base.SourceContext.ViewType == EItemViewType.TradingTrader)
		{
			operation = default(_ECD7);
			return false;
		}
		return base.CanAccept(itemContext, targetItemContext, out operation);
	}

	protected override Color GetHighlightColor(_EB69 itemContext, _ECD7 possibleOperation, _EB68 targetItemContext)
	{
		if (base.SourceContext.ViewType == EItemViewType.TradingTrader)
		{
			return GridView.InvalidOperationColor;
		}
		return base.GetHighlightColor(itemContext, possibleOperation, targetItemContext);
	}

	public void ClearFilterChoice()
	{
		if (_filterPanel != null)
		{
			_filterPanel.ClearChoice();
		}
	}

	public override void Hide()
	{
		if (_filterPanel != null)
		{
			_filterPanel.CurrentFilterChanged -= _E002;
			_filterPanel.CurrentFilterChanged -= _E004;
			_filterPanel.Hide();
		}
		base.Hide();
	}
}
