using System;
using System.Threading.Tasks;
using EFT.InventoryLogic;

namespace EFT;

internal sealed class ClientUsableItemController : Player.UsableItemController
{
	public _E967 UsableItemPacket;

	internal new static ClientUsableItemController _E000(ClientPlayer player, Item item)
	{
		return Player.UsableItemController._E000<ClientUsableItemController>(player, item);
	}

	internal new static Task<ClientUsableItemController> _E001(ClientPlayer player, string itemId)
	{
		Item item = (string.IsNullOrEmpty(itemId) ? null : player._E0DE.FindItem(itemId));
		if (item == null || !(item is _EA82))
		{
			throw new Exception(_ED3E._E000(192615));
		}
		return Player.UsableItemController._E001<ClientUsableItemController>(player, item);
	}

	protected override void CompassStateHandler(bool isActive)
	{
		UsableItemPacket.CompassPacket = new _E94A(isActive);
		base.CompassStateHandler(isActive);
	}

	public override void ShowGesture(EGesture gesture)
	{
		UsableItemPacket.Gesture = gesture;
		base.ShowGesture(gesture);
	}

	public override bool ExamineWeapon()
	{
		bool num = base.ExamineWeapon();
		if (num)
		{
			UsableItemPacket.ExamineWeapon = true;
		}
		return num;
	}

	public override void SetInventoryOpened(bool opened)
	{
		UsableItemPacket.EnableInventoryPacket = new _E94F
		{
			EnableInventory = true,
			InventoryStatus = opened
		};
		base.SetInventoryOpened(opened);
	}

	public override void SetAim(bool value)
	{
		bool isAiming = IsAiming;
		base.SetAim(value);
		if (IsAiming != isAiming)
		{
			UsableItemPacket.ToggleAim = true;
			UsableItemPacket.IsAiming = IsAiming;
		}
	}
}
