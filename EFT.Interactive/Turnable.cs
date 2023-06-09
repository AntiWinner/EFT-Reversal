using Comfort.Common;
using EFT.Ballistics;
using EFT.NetworkPackets;
using UnityEngine;

namespace EFT.Interactive;

public abstract class Turnable : MonoBehaviour, _EC07
{
	public enum EState
	{
		TurningOn,
		TurningOff,
		On,
		Off,
		Destroyed,
		ConstantFlickering,
		SmoothOff
	}

	public string Id;

	public int NetId;

	public EState LampState;

	public BallisticCollider BallisticCollider;

	string _EC07.IdEditable
	{
		get
		{
			return Id;
		}
		set
		{
			Id = value;
		}
	}

	GameObject _EC07.GameObject => base.gameObject;

	string _EC07.TypeKey => _ED3E._E000(105964);

	public bool IsDestroyed => LampState == EState.Destroyed;

	protected void CheckUniqueIdOnDuplicateEvent()
	{
	}

	public virtual void Switch(EState switchTo)
	{
		LampState = switchTo;
	}

	public void RegisterForNetwork()
	{
		NetId = Id.GetHashCode();
		if (BallisticCollider != null && !IsDestroyed)
		{
			BallisticCollider.NetId = NetId;
			BallisticCollider.OnHitAction += _E000;
			if (this is LampController)
			{
				BallisticCollider.HitType = EHitType.Lamp;
			}
		}
		if (Singleton<GameWorld>.Instantiated)
		{
			Singleton<GameWorld>.Instance.RegisterInteractiveObject(this);
		}
	}

	private void _E000(_EC23 dInfo)
	{
		if (!IsDestroyed)
		{
			Singleton<GameWorld>.Instance.ChangeLampState(this, EState.Destroyed);
		}
		BallisticCollider.OnHitAction -= _E000;
	}
}
