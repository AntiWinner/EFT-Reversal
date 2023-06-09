using System.Collections.Generic;
using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using UnityEngine;

namespace EFT.UI;

public class InventoryScreenQuickAccessPanel : UIElement
{
	[SerializeField]
	private BoundSlotView slot0;

	[SerializeField]
	private BoundSlotView slot1;

	[SerializeField]
	private BoundSlotView slot2;

	[SerializeField]
	private BoundSlotView slot3;

	[SerializeField]
	private BoundItemView slot4;

	[SerializeField]
	private BoundItemView slot5;

	[SerializeField]
	private BoundItemView slot6;

	[SerializeField]
	private BoundItemView slot7;

	[SerializeField]
	private BoundItemView slot8;

	[SerializeField]
	private BoundItemView slot9;

	[SerializeField]
	private BoundItemView slot10;

	private BattleUIComponentAnimation _E093;

	private Dictionary<EBoundItem, BoundSlotView> _E167;

	private Dictionary<EBoundItem, BoundItemView> _E168;

	private void Awake()
	{
		_E167 = new Dictionary<EBoundItem, BoundSlotView>(4, _E3A5<EBoundItem>.EqualityComparer)
		{
			{
				EBoundItem.ItemV,
				slot0
			},
			{
				EBoundItem.Item1,
				slot1
			},
			{
				EBoundItem.Item2,
				slot2
			},
			{
				EBoundItem.Item3,
				slot3
			}
		};
		_E168 = new Dictionary<EBoundItem, BoundItemView>(7, _E3A5<EBoundItem>.EqualityComparer)
		{
			{
				EBoundItem.Item4,
				slot4
			},
			{
				EBoundItem.Item5,
				slot5
			},
			{
				EBoundItem.Item6,
				slot6
			},
			{
				EBoundItem.Item7,
				slot7
			},
			{
				EBoundItem.Item8,
				slot8
			},
			{
				EBoundItem.Item9,
				slot9
			},
			{
				EBoundItem.Item10,
				slot10
			}
		};
	}

	public void Show(_EAED inventoryController, ItemUiContext itemUiContext)
	{
		ShowGameObject();
		if (_E093 == null)
		{
			_E093 = base.gameObject.GetComponent<BattleUIComponentAnimation>();
		}
		if (_E167 == null || _E168 == null)
		{
			Awake();
		}
		foreach (KeyValuePair<EBoundItem, BoundSlotView> item in _E167)
		{
			item.Value.Show(item.Key, inventoryController.Inventory.FastAccess.BoundCells[item.Key], inventoryController, itemUiContext);
		}
		foreach (KeyValuePair<EBoundItem, BoundItemView> item2 in _E168)
		{
			item2.Value.Show(item2.Key, inventoryController, itemUiContext);
		}
		if (_E093 != null)
		{
			_E093.Close();
		}
	}

	public void RefreshSelection(Item itemInHands)
	{
		foreach (KeyValuePair<EBoundItem, BoundSlotView> item in _E167)
		{
			item.Value.RefreshSelection(itemInHands);
		}
		foreach (KeyValuePair<EBoundItem, BoundItemView> item2 in _E168)
		{
			item2.Value.RefreshSelection(itemInHands);
		}
	}

	public void RefreshBoundSlotSelectView(Item itemInHads)
	{
		foreach (KeyValuePair<EBoundItem, BoundSlotView> item in _E167)
		{
			item.Value.RefreshSelectView(itemInHads);
		}
	}

	public void AnimatedShow(bool autohide)
	{
		if (_E093 != null)
		{
			_E093.Show(autohide).HandleExceptions();
		}
	}

	public void AnimatedHide(float delaySeconds = 0f)
	{
		if (_E093 != null)
		{
			_E093.Hide(delaySeconds).HandleExceptions();
		}
	}

	public void Hide()
	{
		if (this == null || _E167 == null || _E168 == null)
		{
			return;
		}
		foreach (KeyValuePair<EBoundItem, BoundSlotView> item in _E167)
		{
			item.Value.Hide();
		}
		foreach (KeyValuePair<EBoundItem, BoundItemView> item2 in _E168)
		{
			item2.Value.Hide();
		}
		if (_E093 != null)
		{
			_E093.StopAnimation();
		}
	}

	private BoundSlotView _E000(Weapon weapon)
	{
		foreach (BoundSlotView value in _E167.Values)
		{
			Slot slot = value.GetSlot();
			if (slot == null || slot.ContainedItem != weapon)
			{
				continue;
			}
			return value;
		}
		return null;
	}

	public Vector2 ShowArrowForWeaponSlot(Weapon weapon, bool show)
	{
		BoundSlotView boundSlotView = _E000(weapon);
		if (boundSlotView != null)
		{
			return boundSlotView.ShowArrow(show);
		}
		Debug.LogError(_ED3E._E000(246158) + weapon.Name);
		return Vector2.zero;
	}

	public void ShowQuickdrawGlowForPistols(bool show, float duration)
	{
		foreach (BoundSlotView value in _E167.Values)
		{
			Slot slot = value.GetSlot();
			if (slot != null && slot.ContainedItem is Weapon weapon && weapon.WeapClass.Equals(_ED3E._E000(32735)))
			{
				value.ShowQuickdrawGlow(show, duration);
			}
		}
	}
}
