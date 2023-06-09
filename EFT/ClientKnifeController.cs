using System;
using System.Threading.Tasks;
using EFT.Ballistics;
using EFT.InventoryLogic;

namespace EFT;

internal sealed class ClientKnifeController : Player.KnifeController
{
	public _E959 KnifePacket;

	internal new static ClientKnifeController _E000(ClientPlayer player, KnifeComponent knife)
	{
		return Player.KnifeController._E000<ClientKnifeController>(player, knife);
	}

	internal new static Task<ClientKnifeController> _E001(ClientPlayer player, string itemId)
	{
		KnifeComponent knifeComponent = (string.IsNullOrEmpty(itemId) ? null : player._E0DE.FindItem(itemId).GetItemComponent<KnifeComponent>());
		if (knifeComponent == null)
		{
			throw new Exception(_ED3E._E000(192615));
		}
		return Player.KnifeController._E001<ClientKnifeController>(player, knifeComponent);
	}

	public override void ExamineWeapon()
	{
		KnifePacket.ExamineWeapon = true;
		base.ExamineWeapon();
	}

	public override bool MakeKnifeKick()
	{
		bool flag = base.MakeKnifeKick();
		KnifePacket.MakeKnifeKick = flag;
		return flag;
	}

	public override void BrakeCombo()
	{
		KnifePacket.BrakeCombo = true;
		base.BrakeCombo();
	}

	public override bool MakeAlternativeKick()
	{
		bool flag = base.MakeAlternativeKick();
		KnifePacket.MakeKnifeKick = flag;
		KnifePacket.AlternativeKick = flag;
		return flag;
	}

	protected override void CompassStateHandler(bool isActive)
	{
		KnifePacket.CompassPacket = new _E94A(isActive);
		base.CompassStateHandler(isActive);
	}

	public override void ShowGesture(EGesture gesture)
	{
		KnifePacket.Gesture = gesture;
		base.ShowGesture(gesture);
	}

	public override void SetInventoryOpened(bool opened)
	{
		KnifePacket.EnableInventoryPacket = new _E94F
		{
			EnableInventory = true,
			InventoryStatus = opened
		};
		base.SetInventoryOpened(opened);
	}

	protected override _E6FF _E00B(Player._E00D hit, BallisticCollider ballisticCollider)
	{
		_E6FF obj = base._E00B(hit, ballisticCollider);
		KnifePacket.AddHit(hit, ballisticCollider, obj, LastKickType);
		return obj;
	}
}
