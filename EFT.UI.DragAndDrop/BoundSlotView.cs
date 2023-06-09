using Comfort.Common;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT.UI.DragAndDrop;

public sealed class BoundSlotView : QuickSlotView, _E640, _E63F, _E641, _E64C, _E64D
{
	[SerializeField]
	private BoundSlotView _subWeaponView;

	private Slot m__E001;

	private EBoundItem m__E000;

	public Slot GetSlot()
	{
		return this.m__E001;
	}

	public void Show(EBoundItem index, Slot slot, _EAED inventoryController, ItemUiContext itemUiContext)
	{
		_E000(index, slot, inventoryController, itemUiContext);
		_E001(index, slot, inventoryController, itemUiContext);
	}

	private void _E000(EBoundItem index, Slot slot, _EAED inventoryController, ItemUiContext itemUiContext)
	{
		InventoryController?.UnregisterView(this);
		this.m__E001 = slot;
		InventoryController = inventoryController;
		ItemUiContext = itemUiContext;
		InventoryController.RegisterView(this);
		this.m__E000 = index;
		Item containedItem = this.m__E001.ContainedItem;
		if (containedItem == null)
		{
			SwitchVisualSelection(selected: false);
		}
		else if (ItemView == null)
		{
			SetItem(containedItem, inventoryController, itemUiContext);
		}
		ShowInfoPanel(containedItem);
		HotKey.text = Singleton<_E7DE>.Instance.Control.Settings.GetBoundItemNames(index);
		base.gameObject.SetActive(value: true);
	}

	protected override void SetIconScale()
	{
		ItemView.IconScale = ((RectTransform)InstallPlace.transform).sizeDelta;
	}

	private void _E001(EBoundItem index, Slot slot, _EAED inventoryController, ItemUiContext itemUiContext)
	{
		if (!(_subWeaponView == null))
		{
			if (slot.ContainedItem is Weapon weapon && weapon.GetUnderbarrelWeapon() != null)
			{
				_subWeaponView.Show(index, weapon.GetLauncherSlot(), inventoryController, itemUiContext);
			}
			else
			{
				_subWeaponView.gameObject.SetActive(value: false);
			}
		}
	}

	public void RefreshSelectView(Item itemInHands)
	{
		if (!(_subWeaponView == null))
		{
			bool flag = itemInHands == this.m__E001.ContainedItem;
			if (!flag)
			{
				SwitchVisualSelection(selected: false);
				_subWeaponView.SwitchVisualSelection(selected: false);
			}
			else if (this.m__E001.ContainedItem is Weapon weapon && weapon.GetUnderbarrelWeapon() != null)
			{
				bool flag2 = weapon.IsUnderBarrelDeviceActive && flag;
				SwitchVisualSelection(!flag2);
				_subWeaponView.SwitchVisualSelection(flag2);
			}
		}
	}

	public override void Hide()
	{
		InventoryController?.UnregisterView(this);
		base.Hide();
	}

	public void OnSetInHands(_EAFA args)
	{
		if (this.m__E001.ContainedItem == args.Item)
		{
			SwitchVisualSelection(selected: true);
		}
	}

	public void OnRemoveFromHands(_EAFB args)
	{
		if (this.m__E001.ContainedItem == args.Item)
		{
			SwitchVisualSelection(selected: false);
		}
	}

	public void OnItemRemoved(_EAF3 eventArgs)
	{
		if (eventArgs.Status != CommandStatus.Succeed || eventArgs.From.Container != this.m__E001)
		{
			return;
		}
		if (ItemView != null)
		{
			if (ItemView.Item == eventArgs.Item)
			{
				SwitchVisualSelection(selected: false);
			}
			RemoveItemView();
			_E001(this.m__E000, this.m__E001, InventoryController, ItemUiContext);
		}
		ShowInfoPanel(null);
	}

	public void OnItemAdded(_EAF2 eventArgs)
	{
		if (eventArgs.Status == CommandStatus.Succeed && eventArgs.To.Container == this.m__E001)
		{
			RemoveItemView();
			SetItem(eventArgs.Item, InventoryController, ItemUiContext);
			_E001(this.m__E000, this.m__E001, InventoryController, ItemUiContext);
			ShowInfoPanel(eventArgs.Item);
		}
	}

	protected override void OnDestroy()
	{
		InventoryController?.UnregisterView(this);
		this.m__E001 = null;
		base.OnDestroy();
	}

	public override bool CanAccept(_EB69 itemContext, _EB68 targetItemContext, out _ECD7 operation)
	{
		operation = default(_ECD7);
		return false;
	}

	public override bool CanDrag(_EB68 itemContext)
	{
		return false;
	}
}
