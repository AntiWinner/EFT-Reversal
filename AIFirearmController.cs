using EFT;
using UnityEngine;

internal class AIFirearmController : Player.FirearmController
{
	public override Vector3 WeaponDirection => _player.LookDirection;
}
