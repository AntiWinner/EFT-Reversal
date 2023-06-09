using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using UnityEngine;

namespace EFT.Interactive;

public class ObstacleCollider : MonoBehaviour, IPhysicsTrigger
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public Collider parentCollider;

		internal bool _E000(RaycastHit c)
		{
			return c.collider == parentCollider;
		}
	}

	[Tooltip("Запрещено ложиться")]
	[SerializeField]
	private bool _disablesProne = true;

	[Tooltip("Запрещено ползать")]
	[SerializeField]
	private bool _disablesProneMove = true;

	[Tooltip("Запрещено прыгать")]
	[SerializeField]
	private bool _disablesJump = true;

	[Tooltip("Запрещено спринтовать")]
	[SerializeField]
	private bool _disablesSprint = true;

	[SerializeField]
	[Tooltip("Ограничение скорости (болото)")]
	private bool _hasSwampSpeedLimit = true;

	public string Description => _ED3E._E000(205909);

	public bool HasSwampSpeedLimit => _hasSwampSpeedLimit;

	public EPhysicalCondition ConditionsMask
	{
		get
		{
			EPhysicalCondition ePhysicalCondition = EPhysicalCondition.None;
			if (_disablesProne)
			{
				ePhysicalCondition |= EPhysicalCondition.ProneDisabled;
			}
			if (_disablesProneMove)
			{
				ePhysicalCondition |= EPhysicalCondition.ProneMovementDisabled;
			}
			if (_disablesJump)
			{
				ePhysicalCondition |= EPhysicalCondition.JumpDisabled;
			}
			if (_disablesSprint)
			{
				ePhysicalCondition |= EPhysicalCondition.SprintDisabled;
			}
			return ePhysicalCondition;
		}
	}

	public void OnTriggerEnter(Collider col)
	{
		Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(col);
		if (playerByCollider != null)
		{
			playerByCollider.MovementContext.OnEnterObstacle(this);
		}
	}

	public void OnTriggerExit(Collider col)
	{
		Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(col);
		if (playerByCollider != null)
		{
			playerByCollider.MovementContext.OnExitObstacle(this);
		}
	}

	public void BreakInToPieces()
	{
		Collider parentCollider = base.gameObject.GetComponentInParent<Collider>();
		float num = parentCollider.bounds.min.x;
		float num2 = parentCollider.bounds.min.z;
		while (num < parentCollider.bounds.max.x)
		{
			for (; num2 < parentCollider.bounds.max.z; num2 += 1f)
			{
				RaycastHit[] array = Physics.RaycastAll(new Ray(new Vector3(num, 100f, num2), Vector3.down), 300f);
				if (array.Length != 0 && array.Any((RaycastHit c) => c.collider == parentCollider))
				{
					BoxCollider boxCollider = base.gameObject.AddComponent<BoxCollider>();
					boxCollider.isTrigger = true;
					boxCollider.center = base.transform.InverseTransformPoint(num, 0f, num2);
					boxCollider.size = Vector3.one;
				}
			}
			num += 1f;
			num2 = parentCollider.bounds.min.z;
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
