using System.Collections.Generic;
using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class ContainersPanel : UIElement
{
	private const int _E0E6 = 10;

	[SerializeField]
	private Transform _slotViewsContainer;

	[SerializeField]
	private SlotView _dogtagTemplate;

	[SerializeField]
	[FormerlySerializedAs("SlotTemplate")]
	private SlotView _defaultSlotTemplate;

	private readonly Dictionary<EquipmentSlot, SlotView> _E0E7 = new Dictionary<EquipmentSlot, SlotView>();

	private SlotView _E0E8;

	private static readonly EquipmentSlot[] _E0E9 = new EquipmentSlot[5]
	{
		EquipmentSlot.TacticalVest,
		EquipmentSlot.Pockets,
		EquipmentSlot.Backpack,
		EquipmentSlot.SecuredContainer,
		EquipmentSlot.Dogtag
	};

	public void Show(_EB63<_EB0B> parentContext, _EAED inventoryController, _E74F skills, _ECB1 insurance)
	{
		_EB0B castItem = parentContext.CastItem;
		ItemUiContext instance = ItemUiContext.Instance;
		ShowGameObject();
		EquipmentSlot[] array = _E0E9;
		foreach (EquipmentSlot equipmentSlot in array)
		{
			Slot slot = castItem.GetSlot(equipmentSlot);
			if (inventoryController.IsAllowedToSeeSlot(slot, equipmentSlot))
			{
				SlotView slotView = _E000(equipmentSlot);
				bool flag = equipmentSlot == EquipmentSlot.Dogtag;
				if (equipmentSlot == EquipmentSlot.Pockets)
				{
					slotView.gameObject.GetComponent<HorizontalLayoutGroup>().spacing += 10f;
				}
				slotView.transform.SetParent(flag ? _E0E7[EquipmentSlot.Pockets].transform : _slotViewsContainer, worldPositionStays: false);
				slotView.Show(slot, parentContext, inventoryController, instance, skills, insurance);
				if (flag && _E0E8 == null)
				{
					_E0E8 = slotView;
					((RectTransform)slotView.transform).anchoredPosition = Vector2.zero;
				}
				else
				{
					_E0E7.Add(equipmentSlot, slotView);
				}
			}
		}
	}

	private SlotView _E000(EquipmentSlot slotName)
	{
		if ((uint)(slotName - 13) <= 1u)
		{
			return Object.Instantiate(_dogtagTemplate);
		}
		return Object.Instantiate(_defaultSlotTemplate);
	}

	public SlotView GetSlotView(EquipmentSlot slotType)
	{
		return _E0E7[slotType];
	}

	public override void Close()
	{
		if (_E0E8 != null)
		{
			_E0E8.Hide();
			Object.DestroyImmediate(_E0E8.gameObject);
			_E0E8 = null;
		}
		foreach (SlotView value in _E0E7.Values)
		{
			value.Hide();
			Object.DestroyImmediate(value.gameObject);
		}
		_E0E7.Clear();
		base.Close();
	}
}
