using System.Runtime.CompilerServices;
using Comfort.Common;
using UnityEngine;

namespace EFT.Interactive;

public class TriggerWithId : MonoBehaviour, IPhysicsTrigger, _E633
{
	[SerializeField]
	private string _id;

	[CompilerGenerated]
	private readonly string _E000 = _ED3E._E000(210968);

	public string Id => _id;

	public string Description
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
	}

	private void Awake()
	{
		base.gameObject.layer = LayerMask.NameToLayer(_ED3E._E000(25347));
	}

	public void SetId(string id)
	{
		_id = id;
	}

	public void OnTriggerEnter(Collider col)
	{
		Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(col);
		if (!(playerByCollider == null))
		{
			TriggerEnter(playerByCollider);
		}
	}

	protected virtual void TriggerEnter(Player player)
	{
		player.AddTriggerZone(this);
	}

	public void OnTriggerExit(Collider col)
	{
		Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(col);
		if (!(playerByCollider == null))
		{
			TriggerExit(playerByCollider);
		}
	}

	protected virtual void TriggerExit(Player player)
	{
		player.RemoveTriggerZone(this);
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
