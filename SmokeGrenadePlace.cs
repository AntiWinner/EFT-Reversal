using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT;
using UnityEngine;

public class SmokeGrenadePlace : MonoBehaviour, IPhysicsTrigger
{
	[CompilerGenerated]
	private readonly string _E000 = _ED3E._E000(14200);

	public string Description
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
	}

	public void OnTriggerEnter(Collider col)
	{
		Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(col);
		if (!(playerByCollider == null) && playerByCollider.AIData != null && playerByCollider.AIData.IsAI)
		{
			playerByCollider.AIData.BotOwner.SmokeGrenade.SmokeEnter(this);
		}
	}

	public void OnTriggerExit(Collider col)
	{
		Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(col);
		if (!(playerByCollider == null) && playerByCollider.AIData != null && playerByCollider.AIData.IsAI)
		{
			playerByCollider.AIData.BotOwner.SmokeGrenade.SmokeExit(this);
		}
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
