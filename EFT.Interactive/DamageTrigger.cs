using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Comfort.Common;
using UnityEngine;

namespace EFT.Interactive;

[RequireComponent(typeof(Collider))]
public abstract class DamageTrigger : MonoBehaviour, _E31B, IPhysicsTrigger
{
	private Collider _E004;

	private Bounds _E005;

	private Collider[] _E006 = new Collider[16];

	private Dictionary<Player, int> _E007 = new Dictionary<Player, int>();

	[CompilerGenerated]
	private readonly string _E003 = _ED3E._E000(212655);

	public virtual string Description
	{
		[CompilerGenerated]
		get
		{
			return _E003;
		}
	}

	protected abstract bool IsStatic { get; }

	private void Awake()
	{
		base.gameObject.layer = LayerMask.NameToLayer(_ED3E._E000(25347));
		_E004 = GetComponent<Collider>();
	}

	private void Start()
	{
		_E000();
	}

	private void _E000()
	{
		_E005 = _E38D.GetColliderBoundsWithoutRotation(_E004);
	}

	public void OnTriggerEnter(Collider col)
	{
		Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(col);
		if (!(playerByCollider == null))
		{
			AddPenalty(playerByCollider);
			PlaySound();
		}
	}

	public void OnTriggerExit(Collider col)
	{
		Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(col);
		if (!(playerByCollider == null))
		{
			RemovePenalty(playerByCollider);
		}
	}

	public void OnTriggerStay(Collider col)
	{
		Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(col);
		if (playerByCollider == null || !playerByCollider.HealthController.IsAlive)
		{
			return;
		}
		if (!IsStatic)
		{
			_E000();
		}
		int num = Physics.OverlapBoxNonAlloc(_E005.center, _E005.extents, _E006, base.transform.rotation, _E37B.HitColliderMask, QueryTriggerInteraction.Ignore);
		for (int i = 0; i < num; i++)
		{
			BodyPartCollider component = _E006[i].GetComponent<BodyPartCollider>();
			if (component != null && component.Player == playerByCollider)
			{
				ProceedDamage(playerByCollider, component);
			}
		}
	}

	protected abstract void ProceedDamage(Player player, BodyPartCollider bodyPart);

	protected abstract void AddPenalty(Player player);

	protected abstract void RemovePenalty(Player player);

	protected abstract void PlaySound();

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
