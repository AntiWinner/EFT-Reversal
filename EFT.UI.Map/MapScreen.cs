using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT.UI.Map;

public sealed class MapScreen : UIElement, _EB66._E000
{
	public sealed class _E000 : _EC64<MapScreen>
	{
		private new readonly Profile m__E000;

		private readonly _EAE6 _E001;

		private readonly _EA40[] _E002;

		public _E000(MapScreen mapTab, _EAE6 itemController, _EA40[] collections)
			: base(mapTab)
		{
			_E001 = itemController;
			_E002 = collections;
		}

		public override void Show()
		{
			base._E000.Show(_E001, _E002);
		}
	}

	[CompilerGenerated]
	private Action<Item, bool> _E319;

	[SerializeField]
	private PocketMap _pocketMap;

	[SerializeField]
	private PocketMapMarkerManager _mapMarkerManager;

	[SerializeField]
	private MapMarkerWindow _markerWindow;

	[SerializeField]
	private GameObject _mapBlock;

	[SerializeField]
	private GameObject _emptyBlock;

	[SerializeField]
	private DropdownItemSelector _mapSelector;

	private _EAE6 _E19A;

	private MapComponent _E31A;

	private _EB66 _E133;

	public event Action<Item, bool> OnItemSelected
	{
		[CompilerGenerated]
		add
		{
			Action<Item, bool> action = _E319;
			Action<Item, bool> action2;
			do
			{
				action2 = action;
				Action<Item, bool> value2 = (Action<Item, bool>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E319, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<Item, bool> action = _E319;
			Action<Item, bool> action2;
			do
			{
				action2 = action;
				Action<Item, bool> value2 = (Action<Item, bool>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E319, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public void Init(MapComponent map)
	{
		_E31A = map;
	}

	public void Show(_EAE6 itemController, _EA40[] collections)
	{
		ItemUiContext instance = ItemUiContext.Instance;
		itemController.StopProcesses();
		instance.CloseAllWindows();
		ShowGameObject();
		_E19A = itemController;
		IEnumerable<Item> enumerable = itemController.Inventory.GetAllEquipmentItems();
		if (collections != null)
		{
			enumerable = enumerable.Concat(collections.GetAllItemsFromCollections());
		}
		List<MapComponent> list = enumerable.GetComponents<MapComponent>().ToList();
		bool flag = list.Count > 0;
		_mapBlock.SetActive(flag);
		_emptyBlock.SetActive(!flag);
		_E133 = new _EB66(EItemViewType.Dropdown, this).CreateSelectableChild(itemController.Inventory.Stash);
		UI.AddDisposable(_E133);
		if (flag)
		{
			if (_E31A == null)
			{
				_E31A = list[0];
			}
			_mapSelector.Init(list.Select((MapComponent x) => x.Item), itemController, _E133);
			_E000(_E31A);
		}
	}

	bool _EB66._E000.IsActive(_EB66 context, out string tooltip)
	{
		tooltip = string.Empty;
		return true;
	}

	bool _EB66._E000.IsSelected(Item item)
	{
		return false;
	}

	void _EB66._E000.ToggleSelection(_EB66 context)
	{
		if (_E31A.Item != context.Item)
		{
			_mapSelector.SelectItem(context.Item);
			_E002(context.Item);
		}
	}

	private void _E000(MapComponent map)
	{
		_mapSelector.SelectItem(map.Item);
		_E31A = map;
		_E001();
	}

	private void _E001()
	{
		if (_pocketMap.Shown)
		{
			_pocketMap.Close();
		}
		_pocketMap.Show(_E31A);
		if (_mapMarkerManager.Shown)
		{
			_mapMarkerManager.Hide();
		}
		_mapMarkerManager.Show(_E31A, _E19A, _markerWindow);
	}

	private void _E002(Item item)
	{
		_E31A = item.GetItemComponent<MapComponent>();
		_E001();
	}

	public override void Close()
	{
		base.Close();
		_E31A = null;
		if (_pocketMap.Shown)
		{
			_pocketMap.Close();
		}
		if (_mapMarkerManager.Shown)
		{
			_mapMarkerManager.Hide();
		}
	}
}
