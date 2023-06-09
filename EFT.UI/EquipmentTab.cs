using System.Collections.Generic;
using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using UnityEngine;

namespace EFT.UI;

public sealed class EquipmentTab : UIElement
{
	[SerializeField]
	private SlotView _scabbardSlot;

	[SerializeField]
	private SlotView _holsterSlot;

	[SerializeField]
	private SlotView _primaryWeaponSlot;

	[SerializeField]
	private SlotView _seconaryWeaponSlot;

	[SerializeField]
	private SlotView _armorSlot;

	[SerializeField]
	private SlotView _eyewearSlot;

	[SerializeField]
	private SlotView _faceCoverSlot;

	[SerializeField]
	private SlotView _headwearSlot;

	[SerializeField]
	private SlotView _earpieceSlot;

	[SerializeField]
	private SlotView _armbandSlot;

	private _EAED _E092;

	private _EB0B _E18C;

	private Dictionary<EquipmentSlot, SlotView> _E0E7;

	public bool AnyItemViews
	{
		get
		{
			if (!(_scabbardSlot.ContainedItemView != null) && !(_holsterSlot.ContainedItemView != null) && !(_primaryWeaponSlot.ContainedItemView != null) && !(_seconaryWeaponSlot.ContainedItemView != null) && !(_armorSlot.ContainedItemView != null) && !(_eyewearSlot.ContainedItemView != null) && !(_faceCoverSlot.ContainedItemView != null) && !(_headwearSlot.ContainedItemView != null))
			{
				return _earpieceSlot.ContainedItemView != null;
			}
			return true;
		}
	}

	private void Awake()
	{
		_E0E7 = new Dictionary<EquipmentSlot, SlotView>
		{
			{
				EquipmentSlot.Scabbard,
				_scabbardSlot
			},
			{
				EquipmentSlot.Holster,
				_holsterSlot
			},
			{
				EquipmentSlot.FirstPrimaryWeapon,
				_primaryWeaponSlot
			},
			{
				EquipmentSlot.SecondPrimaryWeapon,
				_seconaryWeaponSlot
			},
			{
				EquipmentSlot.Eyewear,
				_eyewearSlot
			},
			{
				EquipmentSlot.FaceCover,
				_faceCoverSlot
			},
			{
				EquipmentSlot.Headwear,
				_headwearSlot
			},
			{
				EquipmentSlot.Earpiece,
				_earpieceSlot
			},
			{
				EquipmentSlot.ArmorVest,
				_armorSlot
			},
			{
				EquipmentSlot.ArmBand,
				_armbandSlot
			}
		};
	}

	public void Show(_EB63<_EB0B> equipmentContext, _EAED inventoryController, _E74F skills, _ECB1 insurance)
	{
		ItemUiContext instance = ItemUiContext.Instance;
		_E092 = inventoryController;
		_E18C = equipmentContext.CastItem;
		ShowGameObject();
		foreach (KeyValuePair<EquipmentSlot, SlotView> item in _E0E7)
		{
			_E39D.Deconstruct(item, out var key, out var value);
			EquipmentSlot slotName = key;
			SlotView slotView = value;
			Slot slot = _E18C.GetSlot(slotName);
			if (!_E092.IsAllowedToSeeSlot(slot, slotName))
			{
				slotView.gameObject.SetActive(value: false);
			}
			else
			{
				slotView.Show(slot, equipmentContext, _E092, instance, skills, insurance);
			}
		}
	}

	public SlotView GetSlotView(EquipmentSlot slotType)
	{
		if (_E0E7 == null || _E0E7.Count < 1)
		{
			return null;
		}
		return _E0E7[slotType];
	}

	public void Hide()
	{
		if (_E0E7 == null)
		{
			return;
		}
		foreach (KeyValuePair<EquipmentSlot, SlotView> item in _E0E7)
		{
			item.Value.Hide();
		}
		HideGameObject();
	}
}
