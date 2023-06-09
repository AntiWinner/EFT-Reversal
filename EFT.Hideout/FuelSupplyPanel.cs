using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.InventoryLogic;
using EFT.UI;
using UnityEngine;

namespace EFT.Hideout;

public sealed class FuelSupplyPanel : AbstractPanel<List<_E837>>
{
	[CompilerGenerated]
	private sealed class _E001
	{
		public FuelSelectionCell selector;

		public FuelSupplyPanel _003C_003E4__this;

		internal void _E000()
		{
			selector.Close();
			UnityEngine.Object.Destroy(selector.gameObject);
			_003C_003E4__this.m__E002.Clear();
		}
	}

	[SerializeField]
	private GameObject _header;

	[SerializeField]
	private Transform _fuelTankListContainer;

	[SerializeField]
	private FuelSelectionCell _fuelTankTemplate;

	private _E831 m__E000;

	private bool m__E001;

	private readonly Dictionary<ItemSelectionCell, _EA1E> m__E002 = new Dictionary<ItemSelectionCell, _EA1E>();

	private ItemSelectionCell[] _E003;

	private _E819 _E004 => this.m__E000 as _E819;

	public override async Task ShowContents()
	{
		_E831 areaBehaviour = base.AreaData.Template.AreaBehaviour;
		if (areaBehaviour is _E819 obj)
		{
			this.m__E000 = areaBehaviour;
			obj.OnConsumableItemChanged += _E001;
			bool flag = areaBehaviour is GeneratorBehaviour;
			if (_header.activeSelf != flag)
			{
				_header.SetActive(flag);
			}
			await _E000(obj);
			await Task.Yield();
			return;
		}
		throw new ArgumentException(_ED3E._E000(165582) + areaBehaviour.GetType().Name);
	}

	private async Task _E000(_E819 consumer)
	{
		int numberOfSlots = consumer.Slots.NumberOfSlots;
		_E003 = new ItemSelectionCell[numberOfSlots];
		for (int i = 0; i < numberOfSlots; i++)
		{
			if (_fuelTankTemplate == null || this == null)
			{
				break;
			}
			_EA1E obj = null;
			if (consumer.UsingItems.Length > i)
			{
				obj = consumer.UsingItems[i];
			}
			FuelSelectionCell selector = UnityEngine.Object.Instantiate(_fuelTankTemplate, _fuelTankListContainer);
			selector.Show(obj, consumer, _E002);
			_E003[i] = selector;
			this.m__E002[selector] = obj;
			UI.AddDisposable(delegate
			{
				selector.Close();
				UnityEngine.Object.Destroy(selector.gameObject);
				this.m__E002.Clear();
			});
			await Task.Yield();
		}
	}

	public override void SetInfo()
	{
	}

	private void _E001(Item item, int index)
	{
		ItemSelectionCell itemSelectionCell = _E003[index];
		this.m__E002[itemSelectionCell] = item as _EA1E;
		itemSelectionCell.SetItem(item);
	}

	private bool _E002(ItemSelectionCell sender, Item selectedItem, bool selected)
	{
		if (selectedItem != null)
		{
			int num = Array.IndexOf(_E003, sender);
			if (num >= 0)
			{
				ItemSelectionCell itemSelectionCell = _E003[num];
				_EA1E obj = this.m__E002[itemSelectionCell];
				if (selected)
				{
					Singleton<_E815>.Instance.PutItemInAreaSlot(base.AreaData.Template.Type, num, selectedItem);
				}
				else if (!Singleton<_E815>.Instance.TakeItemsFromAreaSlot(base.AreaData.Template.Type, num, obj.TemplateId))
				{
					itemSelectionCell.SetItem(obj);
					return false;
				}
			}
		}
		if (this.m__E002.ContainsKey(sender))
		{
			this.m__E002[sender] = (selected ? (selectedItem as _EA1E) : null);
		}
		if (selected)
		{
			_EA1E[] installedSupplies = this.m__E002.Values.ToArray();
			_E004.InstallConsumableItems(installedSupplies);
		}
		return true;
	}

	public override void Close()
	{
		this.m__E002?.Clear();
		_E004.OnConsumableItemChanged -= _E001;
		base.Close();
	}
}
