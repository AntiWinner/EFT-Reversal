namespace EFT;

internal sealed class ClientPortableRangeFinderController : PortableRangeFinderController
{
	public _E967 UsableItemPacket;

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

	public override void Hide()
	{
		base.Hide();
		UsableItemPacket.HideItem = true;
	}
}
