using System;
using System.Threading.Tasks;
using Comfort.Common;
using UnityEngine;

namespace EFT;

internal sealed class ClientGrenadeController : Player.GrenadeController
{
	public _E953 GrenadePacket;

	internal new static ClientGrenadeController _E000(ClientPlayer player, _EADF item)
	{
		return Player.GrenadeController._E000<ClientGrenadeController>(player, item);
	}

	internal static Task<ClientGrenadeController> _E001(ClientPlayer player, string itemId)
	{
		_EADF obj = (string.IsNullOrEmpty(itemId) ? null : (player._E0DE.FindItem(itemId) as _EADF));
		if (obj == null)
		{
			throw new Exception(_ED3E._E000(192615));
		}
		return Player.GrenadeController._E001<ClientGrenadeController>(player, obj);
	}

	public override void ExamineWeapon()
	{
		GrenadePacket.ExamineWeapon = true;
		base.ExamineWeapon();
	}

	protected override void _E00D()
	{
		GrenadePacket.Cook = true;
		base._E00D();
	}

	public override void PullRingForHighThrow()
	{
		GrenadePacket.PullRingForHighThrow = true;
		base.PullRingForHighThrow();
	}

	public override void HighThrow()
	{
		GrenadePacket.HighThrow = true;
		base.HighThrow();
	}

	public override void PullRingForLowThrow()
	{
		GrenadePacket.PullRingForLowThrow = true;
		base.PullRingForLowThrow();
	}

	public override void LowThrow()
	{
		GrenadePacket.LowThrow = true;
		base.LowThrow();
	}

	protected override void CompassStateHandler(bool isActive)
	{
		GrenadePacket.CompassPacket = new _E94A(isActive);
		base.CompassStateHandler(isActive);
	}

	public override void ActualDrop(Result<_E6CC> controller, float animationSpeed, Action callback, bool fastDrop)
	{
		GrenadePacket.HideGrenade = true;
		base.ActualDrop(controller, animationSpeed, callback, fastDrop);
	}

	public override void SetInventoryOpened(bool opened)
	{
		GrenadePacket.EnableInventoryPacket.EnableInventory = true;
		GrenadePacket.EnableInventoryPacket.InventoryStatus = opened;
		base.SetInventoryOpened(opened);
	}

	protected override void _E00F(float timeSinceSafetyLevelRemoved, Vector3 position, Quaternion rotation, Vector3 force, bool lowThrow)
	{
		base._E00F(timeSinceSafetyLevelRemoved, position, rotation, force, lowThrow);
		GrenadePacket.GrenadeThrowData.HasThrowData = true;
		GrenadePacket.GrenadeThrowData.ThrowGrenadeRotation = rotation;
		GrenadePacket.GrenadeThrowData.ThrowGreanadePosition = position;
		GrenadePacket.GrenadeThrowData.ThrowForce = force;
		GrenadePacket.GrenadeThrowData.LowThrow = lowThrow;
	}

	public override bool CanThrow()
	{
		if ((_player as ClientPlayer).IsWaitingForNetworkCallback)
		{
			return false;
		}
		return base.CanThrow();
	}

	public override void ShowGesture(EGesture gesture)
	{
		GrenadePacket.Gesture = gesture;
		base.ShowGesture(gesture);
	}
}
