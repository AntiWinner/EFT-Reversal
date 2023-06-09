using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT.UI.DragAndDrop;

public class SearchableSlotView : SlotView
{
	[SerializeField]
	private SearchableItemView _searchableItemView;

	[SerializeField]
	private Transform _specSlotsPanel;

	[SerializeField]
	private SlotView _specSlotTemplate;

	[SerializeField]
	private CustomTextMeshProUGUI _specSlotHeader;

	private List<SlotView> m__E002;

	private const string m__E003 = "Pockets";

	private const string m__E004 = "InventoryScreen/SpecialSlotsHeader";

	private Action _E005;

	public override void Show(Slot slot, _EB68 parentItemContext, _EAED inventoryController, ItemUiContext itemUiContext, _E74F skills, _ECB1 insurance)
	{
		base.Show(slot, parentItemContext, inventoryController, itemUiContext, skills, insurance);
		_E000(base.Slot.ContainedItem);
		if (inventoryController == slot.ContainedItem?.Owner)
		{
			_E001(base.Slot.ContainedItem);
		}
		if (base.Slot.ContainedItem is _EA91 obj)
		{
			_E005 = obj.SearchOperations.CountChanged.Bind(delegate(int searchOperationsCount)
			{
				_E002(searchOperationsCount > 0);
			});
		}
		if (!(slot.ID == _ED3E._E000(202277)))
		{
			Sprite sprite = _E905.Pop<Sprite>(_ED3E._E000(236939) + slot.Name);
			if (sprite == null)
			{
				Debug.LogWarning(_ED3E._E000(236906) + slot.Name + _ED3E._E000(111488));
			}
			else
			{
				SetSlotBackImage(sprite);
			}
		}
	}

	private void _E000(Item item)
	{
		if (item is _EA40 obj)
		{
			_searchableItemView.Show(obj, base.ParentItemContext.CreateChild(obj), InventoryController, null, ItemUiContext);
		}
	}

	private void _E001(Item item)
	{
		_EA40 obj = item as _EA40;
		if (!(_specSlotsPanel == null) && obj != null && obj.Slots.Length != 0)
		{
			this.m__E002 = new List<SlotView>(obj.Slots.Length);
			_specSlotsPanel.gameObject.SetActive(value: true);
			_specSlotsPanel.SetAsLastSibling();
			if (_specSlotHeader != null)
			{
				_specSlotHeader.text = _ED3E._E000(236980).Localized().ToUpper();
			}
			Slot[] slots = obj.Slots;
			foreach (Slot slot in slots)
			{
				SlotView slotView = UnityEngine.Object.Instantiate(_specSlotTemplate);
				this.m__E002.Add(slotView);
				slotView.transform.SetParent(_specSlotsPanel, worldPositionStays: false);
				slotView.Show(slot, base.ParentItemContext, InventoryController, ItemUiContext, Skills, InsuranceCompany);
			}
		}
	}

	private void _E002(bool value)
	{
		if (_E00E != null)
		{
			_E00E.IsBeingSearched.Value = value;
		}
		SearchIcon.SetActive(value);
	}

	public override void DragStarted()
	{
		_searchableItemView.HideContents();
		base.DragStarted();
	}

	public override void DragCancelled()
	{
		_searchableItemView.ShowContents();
		base.DragCancelled();
	}

	protected override void OnAddToSlot(Item item, _EAF2 args)
	{
		base.OnAddToSlot(item, args);
		if (args.Status == CommandStatus.Succeed)
		{
			_E000(item);
			_E001(item);
		}
	}

	protected override void OnRemoveFromSlot(Item item, _EAF3 args)
	{
		base.OnRemoveFromSlot(item, args);
		if (args.Status == CommandStatus.Succeed)
		{
			if (item is _EA40)
			{
				_searchableItemView.Hide();
			}
			_E003();
		}
		else
		{
			_searchableItemView.HideContents();
		}
	}

	public override void Hide()
	{
		if (base.Slot?.ContainedItem is _EA40)
		{
			_searchableItemView.Hide();
		}
		_E003();
		_E005?.Invoke();
		_E005 = null;
		base.Hide();
	}

	private void _E003()
	{
		if (_specSlotsPanel != null)
		{
			_specSlotsPanel.gameObject.SetActive(value: false);
		}
		if (this.m__E002 == null)
		{
			return;
		}
		foreach (SlotView item in this.m__E002)
		{
			item.Hide();
			UnityEngine.Object.Destroy(item);
		}
		this.m__E002 = null;
	}

	[CompilerGenerated]
	private void _E004(int searchOperationsCount)
	{
		_E002(searchOperationsCount > 0);
	}
}
