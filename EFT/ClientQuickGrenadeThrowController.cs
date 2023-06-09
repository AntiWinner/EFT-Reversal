using System;
using System.Threading.Tasks;
using UnityEngine;

namespace EFT;

internal sealed class ClientQuickGrenadeThrowController : Player.QuickGrenadeThrowController
{
	public _E953 GrenadePacket;

	internal new static ClientQuickGrenadeThrowController _E000(ClientPlayer player, _EADF item)
	{
		return Player.QuickGrenadeThrowController._E000<ClientQuickGrenadeThrowController>(player, item);
	}

	internal static Task<ClientQuickGrenadeThrowController> _E001(ClientPlayer player, string itemId)
	{
		_EADF obj = (string.IsNullOrEmpty(itemId) ? null : (player._E0DE.FindItem(itemId) as _EADF));
		if (obj == null)
		{
			throw new Exception(_ED3E._E000(192615));
		}
		return Player.QuickGrenadeThrowController._E001<ClientQuickGrenadeThrowController>(player, obj);
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

	protected override void _E00D()
	{
		GrenadePacket.Cook = true;
		base._E00D();
	}
}
