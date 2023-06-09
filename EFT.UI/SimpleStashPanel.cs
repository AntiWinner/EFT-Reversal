using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Comfort.Common;
using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class SimpleStashPanel : UIElement, _EC60
{
	[CompilerGenerated]
	private Action _E182;

	[SerializeField]
	private SearchableItemView _simplePanel;

	[SerializeField]
	private TextMeshProUGUI _simpleGridName;

	[SerializeField]
	private FilterPanel _filterPanel;

	[SerializeField]
	private DisplayMoneyPanel _moneyCountPanel;

	[SerializeField]
	private GameObject _containerNamePanel;

	[SerializeField]
	private TextMeshProUGUI _containerName;

	[SerializeField]
	private GameObject _sortPanel;

	[SerializeField]
	private GridSortPanel _gridSortPanel;

	[SerializeField]
	private Tab _sortingTableTab;

	[SerializeField]
	private ScrollRect _stashScroll;

	private _EC61 _E183;

	private _EAED _E092;

	private _EA40 _E085;

	private _EB68 _E133;

	private ItemUiContext _E089;

	private _EAED _E184;

	private bool _E185;

	private EItemViewType _E186;

	public event Action OnSortingTableTabSelected
	{
		[CompilerGenerated]
		add
		{
			Action action = _E182;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E182, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E182;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E182, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public void Configure(_EA40 item, _EAED inventoryController, EItemViewType viewType)
	{
		_E186 = viewType;
		_E092 = inventoryController;
		_E085 = item;
		_E089 = ItemUiContext.Instance;
		_E185 = _stashScroll != null;
	}

	public void Show(_EAED leftSideInventoryController = null, ItemsPanel.EItemsTab tab = ItemsPanel.EItemsTab.Gear)
	{
		ShowGameObject();
		_E133 = new _EB64(_E085, _E186);
		_simplePanel.gameObject.SetActive(value: true);
		_simplePanel.Show(_E085, _E133, _E092, _filterPanel, _E089);
		_EAA0 stash = _E092.Inventory.Stash;
		IItemOwner owner = _E085.Parent.GetOwner();
		if (leftSideInventoryController != null && leftSideInventoryController != owner && owner is _EB1E obj)
		{
			_E184 = leftSideInventoryController;
			obj.RefreshItemEvent += _E184.RaiseEvent;
		}
		string correctedProfileNickname = Singleton<_E7DE>.Instance.Game.Controller.GetCorrectedProfileNickname(owner.ID, owner.ContainerName);
		_containerName.text = correctedProfileNickname;
		_containerNamePanel.SetActive(owner != _E092);
		if (_moneyCountPanel != null && stash != null && owner == _E092)
		{
			_E183 = new _EC61(_moneyCountPanel, stash, _E092);
		}
		else if (_moneyCountPanel != null)
		{
			_moneyCountPanel.gameObject.SetActive(value: false);
		}
		if (_E085 is _EA91 obj2 && _E092.GetSearchState(obj2).Value != SearchedState.FullySearched)
		{
			_E092.SearchContents(obj2);
		}
		_simpleGridName.text = ((_E085 is _EAA0) ? _ED3E._E000(258158) : _ED3E._E000(258161)).Localized();
		_E000(tab);
	}

	private void _E000(ItemsPanel.EItemsTab tab)
	{
		if (_sortPanel == null)
		{
			return;
		}
		if (!(_E085.Parent.GetOwner() is _EAED obj))
		{
			_sortPanel.SetActive(value: false);
		}
		else if (tab == ItemsPanel.EItemsTab.Gear && obj.IsAllowedToSort(_E085))
		{
			_sortPanel.SetActive(value: true);
			if (_gridSortPanel != null)
			{
				_gridSortPanel.Show(obj, _E085);
			}
			if (_sortingTableTab == null)
			{
				return;
			}
			if (obj.Inventory.SortingTable != null && _E182 != null)
			{
				_sortingTableTab.gameObject.SetActive(value: true);
				_sortingTableTab.OnSelectionChanged += _E001;
				UI.AddDisposable(delegate
				{
					_sortingTableTab.OnSelectionChanged -= _E001;
				});
				_sortingTableTab.Deselect().HandleExceptions();
				if (obj.SortingTableActive)
				{
					_E002();
				}
			}
			else
			{
				_sortingTableTab.gameObject.SetActive(value: false);
			}
		}
		else
		{
			_sortPanel.SetActive(value: false);
		}
	}

	private void _E001(Tab _, bool isVisible)
	{
		ChangeSortingTableTabState(isVisible);
	}

	public void ChangeSortingTableTabState(bool isVisible)
	{
		if (isVisible)
		{
			_E002();
		}
		else
		{
			_E003();
		}
	}

	private void _E002()
	{
		if (_gridSortPanel != null)
		{
			_gridSortPanel.SetUnlockStatus(value: false);
		}
		_sortingTableTab.Select();
		_E182?.Invoke();
	}

	private void _E003()
	{
		if (_gridSortPanel != null)
		{
			_gridSortPanel.SetUnlockStatus(value: true);
		}
		_sortingTableTab.Deselect().HandleExceptions();
	}

	private void Update()
	{
		if (_E185)
		{
			if (Input.GetKeyDown(KeyCode.PageUp))
			{
				_stashScroll.normalizedPosition = Vector2.up;
			}
			if (Input.GetKeyDown(KeyCode.PageDown))
			{
				_stashScroll.normalizedPosition = Vector2.zero;
			}
		}
	}

	public override void Close()
	{
		_simplePanel.Close();
		_simplePanel.gameObject.SetActive(value: false);
		if (_moneyCountPanel != null)
		{
			_moneyCountPanel.gameObject.SetActive(value: false);
		}
		if (_E183 != null)
		{
			_E183.Dispose();
			_E183 = null;
		}
		IItemOwner owner = _E085.Parent.GetOwner();
		if (_E184 != null)
		{
			if (owner is _EB1E obj)
			{
				obj.RefreshItemEvent -= _E184.RaiseEvent;
			}
			_E184 = null;
		}
		_E133?.Dispose();
		_E133 = null;
		base.Close();
	}

	[CompilerGenerated]
	private void _E004()
	{
		_sortingTableTab.OnSelectionChanged -= _E001;
	}
}
