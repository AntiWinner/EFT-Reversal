using System.Runtime.CompilerServices;
using Comfort.Common;
using UnityEngine;

namespace EFT.BufferZone;

public class BufferOuterBattleZone : MonoBehaviour, IPhysicsTrigger
{
	private readonly _ECEF<Player> m__E000 = new _ECEF<Player>();

	private readonly _ECEF<Player> _E001 = new _ECEF<Player>();

	public string Description => _ED3E._E000(209376);

	void IPhysicsTrigger.OnTriggerEnter(Collider col)
	{
		Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(col);
		if (!(playerByCollider == null) && !this.m__E000.Contains(playerByCollider))
		{
			this.m__E000.Add(playerByCollider);
			playerByCollider.OnPlayerDeadOrUnspawn += _E000;
		}
	}

	void IPhysicsTrigger.OnTriggerExit(Collider col)
	{
		if (base.enabled)
		{
			Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(col);
			if (!(playerByCollider == null))
			{
				_E000(playerByCollider);
			}
		}
	}

	public bool IsPlayerInZone(Player player)
	{
		return this.m__E000.Contains(player);
	}

	public bool IsPlayerDyingInZone(Player player)
	{
		if (!_E001.Contains(player))
		{
			return this.m__E000.Contains(player);
		}
		return true;
	}

	private void _E000(Player player)
	{
		if (!player.HealthController.IsAlive)
		{
			_E001.Add(player);
		}
		this.m__E000.Remove(player);
		player.OnPlayerDeadOrUnspawn -= _E000;
	}

	[SpecialName]
	bool IPhysicsTrigger.get_enabled()
	{
		return base.enabled;
	}

	[SpecialName]
	void IPhysicsTrigger.set_enabled(bool value)
	{
		base.enabled = value;
	}
}
