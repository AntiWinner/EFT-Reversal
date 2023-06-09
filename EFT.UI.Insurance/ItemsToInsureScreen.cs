using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Comfort.Common;
using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Insurance;

public sealed class ItemsToInsureScreen : UIElement, _E640, _E63F, _E641, _E647
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public ItemsToInsureScreen _003C_003E4__this;

		public List<_ECB4> allEquipmentItems;

		public Action<ItemToInsurePanel> _003C_003E9__4;

		internal void _E000(_ECB4[] result)
		{
			_003C_003E4__this._E18F.Logger.LogInfo(_ED3E._E000(233135), allEquipmentItems.Count);
		}

		internal void _E001()
		{
			if (_003C_003E4__this._E07E != null && _003C_003E4__this._E07E.Any())
			{
				_003C_003E4__this._E07E.UpdateItems(delegate(ItemToInsurePanel arg)
				{
					arg.UpdateInsuranceItemsPrices(_003C_003E4__this._E18F.AllItemsToInsure);
				});
				_003C_003E4__this.OnTraderChanged.Invoke();
			}
		}

		internal void _E002(ItemToInsurePanel arg)
		{
			arg.UpdateInsuranceItemsPrices(_003C_003E4__this._E18F.AllItemsToInsure);
		}

		internal void _E003(_ECB4 item, ItemToInsurePanel view)
		{
			_003C_003E4__this._scrollRect.verticalNormalizedPosition = 1f;
			_003C_003E4__this._scrollRect.horizontalNormalizedPosition = 0f;
			view.Show(item, ItemViewFactory.GetItemType(item.Type), _003C_003E4__this._E18F, _003C_003E4__this._itemToInsurePanel, _003C_003E4__this);
		}
	}

	[CompilerGenerated]
	private Action<bool> _E30A;

	[SerializeField]
	private ComplexStashPanel _itemsPanel;

	[SerializeField]
	private ItemToInsurePanel _itemToInsurePanel;

	[SerializeField]
	private RectTransform _itemsToInsureContainer;

	[SerializeField]
	private ScrollRect _scrollRect;

	[SerializeField]
	private TextMeshProUGUI _itemsToInsureCount;

	[SerializeField]
	private InsurerParametersPanel _insurerParametersPanel;

	[SerializeField]
	private Button _insureAllButton;

	[SerializeField]
	private UpdatableToggle _insureAllToggle;

	[NonSerialized]
	public _ECEC OnTraderChanged;

	private _ECB1 _E18F;

	private _EAED _E0A3;

	private _EC71<_ECB4, ItemToInsurePanel> _E07E;

	private _ECB1._E000 _E000 => _E18F.OverallPrice;

	public event Action<bool> OnInsuranceAvailableChanged
	{
		[CompilerGenerated]
		add
		{
			Action<bool> action = _E30A;
			Action<bool> action2;
			do
			{
				action2 = action;
				Action<bool> value2 = (Action<bool>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E30A, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<bool> action = _E30A;
			Action<bool> action2;
			do
			{
				action2 = action;
				Action<bool> value2 = (Action<bool>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E30A, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	private void Awake()
	{
		_insureAllToggle.onValueChanged.AddListener(delegate(bool arg)
		{
			if (arg)
			{
				IEnumerable<Item> enumerable = from item in _E0A3.Inventory.GetAllEquipmentItems()
					where !(item is _EB0B) && !(item is _EA80) && _E0A3.Inventory.Equipment.GetSlot(EquipmentSlot.Dogtag).ContainedItem != item && (!(item is _EA74 obj) || !obj.isSecured)
					select item;
				Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.InsuranceItemOnInsure);
				{
					foreach (Item item in enumerable)
					{
						_E18F.AddItemToInsuranceQueue(item, addChildren: true, playSound: false);
					}
					return;
				}
			}
			foreach (_ECB4 item2 in _E18F.AllItemsToInsure)
			{
				_E18F.RemoveItemFromInsuranceQueue(item2);
			}
		});
		_insureAllButton.onClick.AddListener(delegate
		{
			_insureAllToggle.isOn = !_insureAllToggle.isOn;
		});
		_insurerParametersPanel.OnInsuranceAvailableChanged += delegate(bool available)
		{
			_E30A?.Invoke(available);
		};
	}

	public void Show(Profile profile, _EAED controller, _EB0B equipment, _ECB1 insurance, ItemUiContext itemUiContext)
	{
		OnTraderChanged = new _ECEC();
		_E18F = insurance;
		_E0A3 = controller;
		_itemsPanel.Configure(controller, new _EB63<_EB0B>(equipment, EItemViewType.Insurance), profile.Skills, _E18F, itemUiContext);
		_itemsPanel.Show();
		ShowGameObject();
		_E0A3.RegisterView(this);
		List<_ECB4> allEquipmentItems = (from item in controller.Inventory.GetAllEquipmentItems()
			where !(item is _EB0B) && !(item is _EA80)
			select item).Select(_ECB4.FindOrCreate).Reverse().ToList();
		_E18F.UpdateInsuranceItemsPrices(allEquipmentItems, delegate
		{
			_E18F.Logger.LogInfo(_ED3E._E000(233135), allEquipmentItems.Count);
		});
		_insurerParametersPanel.Show(controller.Inventory, _E18F, delegate
		{
			if (_E07E != null && _E07E.Any())
			{
				_E07E.UpdateItems(delegate(ItemToInsurePanel arg)
				{
					arg.UpdateInsuranceItemsPrices(_E18F.AllItemsToInsure);
				});
				OnTraderChanged.Invoke();
			}
		}, profile.Skills);
		_ECB1._E000 obj = this._E000;
		_insurerParametersPanel.UpdateLabels(obj.Price);
		_insurerParametersPanel.UpdateInsureButtonStatus(obj.Error);
		_itemsToInsureCount.text = string.Format(_ED3E._E000(233098).Localized(), _E18F.AllItemsToInsure.Count);
		UI.AddDisposable(_E18F.OnItemUncovered.Subscribe(_E000));
		_E07E = UI.AddDisposable(new _EC71<_ECB4, ItemToInsurePanel>(_E18F.ItemsToInsure, _itemToInsurePanel, _itemsToInsureContainer, delegate(_ECB4 item, ItemToInsurePanel view)
		{
			_scrollRect.verticalNormalizedPosition = 1f;
			_scrollRect.horizontalNormalizedPosition = 0f;
			view.Show(item, ItemViewFactory.GetItemType(item.Type), _E18F, _itemToInsurePanel, this);
		}));
	}

	public void OnItemAdded(_EAF2 eventArgs)
	{
		_E18F.RemoveItemFromInsuranceQueue(eventArgs.Item);
	}

	public void OnItemRemoved(_EAF3 eventArgs)
	{
		_E18F.RemoveItemFromInsuranceQueue(eventArgs.Item);
	}

	private void _E000(_ECB4 obj)
	{
		_ECB1._E000 obj2 = this._E000;
		_insurerParametersPanel.UpdateLabels(obj2.Price, updateMoneyToPay: true, updateMoneySums: false);
		_insurerParametersPanel.UpdateInsureButtonStatus(obj2.Error);
		_itemsToInsureCount.text = string.Format(_ED3E._E000(233098).Localized(), _E18F.AllItemsToInsure.Count);
	}

	public void OnRefreshItem(_EAFF eventArgs)
	{
		if (eventArgs.Item is _EA76)
		{
			_insurerParametersPanel.UpdateLabels(0, updateMoneyToPay: false);
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F5))
		{
			_insureAllToggle.isOn = !_insureAllToggle.isOn;
		}
	}

	public override void Close()
	{
		_E0A3.UnregisterView(this);
		_insurerParametersPanel.Close();
		_itemsPanel.Close();
		_itemsPanel.UnConfigure();
		_E18F.ClearInsuranceQueue();
		_insureAllToggle.isOn = false;
		base.Close();
	}

	[CompilerGenerated]
	private void _E001(bool arg)
	{
		if (arg)
		{
			IEnumerable<Item> enumerable = from item in _E0A3.Inventory.GetAllEquipmentItems()
				where !(item is _EB0B) && !(item is _EA80) && _E0A3.Inventory.Equipment.GetSlot(EquipmentSlot.Dogtag).ContainedItem != item && (!(item is _EA74 obj) || !obj.isSecured)
				select item;
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.InsuranceItemOnInsure);
			{
				foreach (Item item in enumerable)
				{
					_E18F.AddItemToInsuranceQueue(item, addChildren: true, playSound: false);
				}
				return;
			}
		}
		foreach (_ECB4 item2 in _E18F.AllItemsToInsure)
		{
			_E18F.RemoveItemFromInsuranceQueue(item2);
		}
	}

	[CompilerGenerated]
	private bool _E002(Item item)
	{
		if (!(item is _EB0B) && !(item is _EA80) && _E0A3.Inventory.Equipment.GetSlot(EquipmentSlot.Dogtag).ContainedItem != item)
		{
			if (item is _EA74 obj)
			{
				return !obj.isSecured;
			}
			return true;
		}
		return false;
	}

	[CompilerGenerated]
	private void _E003()
	{
		_insureAllToggle.isOn = !_insureAllToggle.isOn;
	}

	[CompilerGenerated]
	private void _E004(bool available)
	{
		_E30A?.Invoke(available);
	}
}
