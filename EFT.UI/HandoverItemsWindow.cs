using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Comfort.Common;
using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public abstract class HandoverItemsWindow : MessageWindow, _EB66._E000
{
	[CompilerGenerated]
	private Action<Item, bool> _E005;

	[SerializeField]
	private GridView _itemsToHandover;

	[SerializeField]
	private Button _autoSelectButton;

	[CompilerGenerated]
	private ItemUiContext _E006;

	protected readonly List<Item> ItemsList = new List<Item>();

	protected double CurrentValue;

	private _EAA0 _E007;

	private _EB1E _E008;

	[CompilerGenerated]
	private _EB66 _E009;

	protected ItemUiContext ItemUiContext
	{
		[CompilerGenerated]
		get
		{
			return _E006;
		}
		[CompilerGenerated]
		private set
		{
			_E006 = value;
		}
	}

	protected _EB66 SelectableItemContext
	{
		[CompilerGenerated]
		get
		{
			return _E009;
		}
		[CompilerGenerated]
		private set
		{
			_E009 = value;
		}
	}

	protected Item[] ItemsToHandover => ItemsList.ToArray();

	protected GridView GridView => _itemsToHandover;

	protected virtual int GridWidth => 10;

	protected virtual int GridHeight => 10;

	protected virtual bool CanStretchVertically => true;

	protected virtual bool CanStretchHorizontally => false;

	public event Action<Item, bool> OnItemSelected
	{
		[CompilerGenerated]
		add
		{
			Action<Item, bool> action = _E005;
			Action<Item, bool> action2;
			do
			{
				action2 = action;
				Action<Item, bool> value2 = (Action<Item, bool>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E005, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<Item, bool> action = _E005;
			Action<Item, bool> action2;
			do
			{
				action2 = action;
				Action<Item, bool> value2 = (Action<Item, bool>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E005, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	protected override void Awake()
	{
		base.Awake();
		_E007 = Singleton<_E63B>.Instance.CreateFakeStash();
		if (_autoSelectButton != null)
		{
			_autoSelectButton.onClick.AddListener(AutoSelectButtonPressedHandler);
		}
	}

	protected _EC7B Show(string title, string message, Profile profile, _EB1E itemController, _EB66 itemContext, Action cancelAction)
	{
		ItemUiContext = ItemUiContext.Instance;
		_E008 = itemController;
		_EC7B result = Show(title, message, delegate
		{
		}, cancelAction);
		SelectableItemContext = itemContext.CreateSelectableChild(_E007);
		SelectableItemContext.OnCloseWindow += Close;
		ClearSelectedList();
		new _EB1E(_E007, profile.Id, profile.Nickname, canBeLocalized: true, EOwnerType.Mail);
		UI.AddDisposable(_E000);
		return result;
	}

	protected virtual void AutoSelectButtonPressedHandler()
	{
		ClearSelectedList();
		foreach (Item item in _E007.Grid.Items)
		{
			TrySelectItemToHandover(item);
		}
	}

	protected virtual void UpdateItems(ICollection<Item> items)
	{
		if (_itemsToHandover.gameObject.activeSelf)
		{
			_itemsToHandover.Hide();
		}
		_E007.Grids[0]?.RemoveAll();
		_E007.Grids[0] = new _E9F5(_ED3E._E000(124538), GridWidth, GridHeight, CanStretchVertically, CanStretchHorizontally, Array.Empty<ItemFilter>(), _E007);
		foreach (Item item in items)
		{
			_E007.Grid.Add(item);
		}
		_itemsToHandover.Show(_E007.Grid, SelectableItemContext, _E008, ItemUiContext);
	}

	protected abstract void TrySelectItemToHandover(Item item);

	protected abstract bool IsSelected(Item item);

	void _EB66._E000.ToggleSelection(_EB66 context)
	{
		TrySelectItemToHandover(context.Item);
	}

	bool _EB66._E000.IsSelected(Item item)
	{
		return IsSelected(item);
	}

	protected abstract bool IsActive(Item item, out string tooltip);

	bool _EB66._E000.IsActive(_EB66 context, out string tooltip)
	{
		return IsActive(context.Item, out tooltip);
	}

	protected void SelectView(Item item)
	{
		_E005?.Invoke(item, arg2: true);
	}

	protected void DeselectView(Item item)
	{
		_E005?.Invoke(item, arg2: false);
	}

	protected virtual void ClearSelectedList()
	{
		ItemsList.Clear();
		SetAcceptActive(value: false);
	}

	private void _E000()
	{
		ClearSelectedList();
		if (SelectableItemContext != null)
		{
			SelectableItemContext.OnCloseWindow -= Close;
			SelectableItemContext.CloseDependentWindows();
			SelectableItemContext.Dispose();
			SelectableItemContext = null;
		}
		_itemsToHandover.Hide();
		_E007?.Grid?.RemoveAll();
	}
}
