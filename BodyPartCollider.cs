using System;
using EFT;
using EFT.Ballistics;
using UnityEngine;

public class BodyPartCollider : BallisticCollider
{
	private const float m__E000 = 0.5f;

	private const float _E001 = 2f;

	private const float _E002 = 1f;

	private const float _E003 = 7.5f;

	private const float _E004 = 0.5f;

	private const float _E005 = 1f;

	private const float _E006 = 9999f;

	private float _E007;

	private float _E008;

	private float _E009;

	private Vector3 _E00A;

	public EBodyPart BodyPartType;

	public EBodyPartColliderType BodyPartColliderType;

	public Collider Collider;

	public Player Player;

	public Vector3 Center => Collider.bounds.center;

	private static Bounds _E000(Collider collider)
	{
		BoxCollider boxCollider = collider as BoxCollider;
		if (boxCollider != null)
		{
			return new Bounds(Vector3.zero, boxCollider.size);
		}
		SphereCollider sphereCollider = collider as SphereCollider;
		if (sphereCollider != null)
		{
			return new Bounds(Vector3.zero, Vector3.one * sphereCollider.radius * 2f);
		}
		CapsuleCollider capsuleCollider = collider as CapsuleCollider;
		if (capsuleCollider != null)
		{
			Vector3 size = Vector3.one * capsuleCollider.radius * 2f;
			size[capsuleCollider.direction] = capsuleCollider.height;
			return new Bounds(Vector3.zero, size);
		}
		Debug.LogError(_ED3E._E000(55253) + collider, collider);
		return collider.bounds;
	}

	public override void Awake()
	{
		base.TypeOfMaterial = MaterialType.Body;
		base.Awake();
	}

	public override _E6FF ApplyHit(_EC23 damageInfo, _EC22 shotID)
	{
		if (Player != null && damageInfo.IsForwardHit)
		{
			return Player.ApplyShot(damageInfo, BodyPartType, shotID);
		}
		return null;
	}

	public void ApplyEnvironmentalDamage(_EC23 damageInfo)
	{
		if (Player != null)
		{
			Player.ApplyDamageInfo(damageInfo, BodyPartType, 0f);
		}
	}

	public void ApplyInstantKill(_EC23 damageInfo)
	{
		if (Player != null)
		{
			Player.ApplyDamageInfo(damageInfo, BodyPartType, 0f);
		}
	}

	public bool ProceedBarb()
	{
		bool result = false;
		float num = Vector3.Distance(_E00A, base.transform.position);
		if (num < 0.5f)
		{
			_E007 += num * 2f;
			if (_E007 >= 1f)
			{
				_EC23 obj = default(_EC23);
				obj.DamageType = EDamageType.Barbed;
				obj.Damage = _E007;
				obj.Direction = Vector3.zero;
				obj.HitCollider = Collider;
				obj.HitNormal = Vector3.zero;
				obj.HitPoint = Vector3.zero;
				obj.HittedBallisticCollider = this;
				obj.Player = null;
				_EC23 damageInfo = obj;
				_E007 %= 1f;
				ApplyEnvironmentalDamage(damageInfo);
				result = true;
			}
		}
		else
		{
			_E007 = 0f;
		}
		_E00A = base.transform.position;
		return result;
	}

	public void ProceedFlame()
	{
		_E008 -= Time.deltaTime;
		if (_E008 <= 0f)
		{
			_EC23 obj = default(_EC23);
			obj.DamageType = EDamageType.Flame;
			obj.Damage = 7.5f;
			obj.Direction = Vector3.zero;
			obj.HitCollider = Collider;
			obj.HitNormal = Vector3.zero;
			obj.HitPoint = Vector3.zero;
			obj.HittedBallisticCollider = this;
			obj.Player = null;
			_EC23 damageInfo = obj;
			ApplyEnvironmentalDamage(damageInfo);
			_E008 = 0.5f;
		}
	}

	public void ProceedPlatformImpact(float damage)
	{
		if (!(_E009 > Time.time))
		{
			_EC23 obj = default(_EC23);
			obj.DamageType = EDamageType.Impact;
			obj.Damage = damage;
			obj.Direction = Vector3.zero;
			obj.HitCollider = Collider;
			obj.HitNormal = Vector3.zero;
			obj.HitPoint = Vector3.zero;
			obj.HittedBallisticCollider = this;
			obj.Player = null;
			_EC23 damageInfo = obj;
			ApplyEnvironmentalDamage(damageInfo);
			_E009 = Time.time + 1f;
		}
	}

	public void ProceedInstantKill()
	{
		_EC23 obj = default(_EC23);
		obj.Damage = 9999f;
		obj.Direction = Vector3.zero;
		obj.HitCollider = Collider;
		obj.HitNormal = Vector3.zero;
		obj.HitPoint = Vector3.zero;
		obj.DamageType = EDamageType.Undefined;
		obj.HittedBallisticCollider = this;
		obj.Player = null;
		_EC23 damageInfo = obj;
		ApplyInstantKill(damageInfo);
	}

	public override bool IsPenetrated(_EC26 shot, Vector3 hitPoint)
	{
		if (string.IsNullOrEmpty(shot.BlockedBy))
		{
			return base.IsPenetrated(shot, hitPoint);
		}
		return false;
	}

	public override bool Deflects(float _hitCosDirectionToNormal, _EC26 shot, Vector3 hitPoint, Vector3 shotNormal, Vector3 shotDirection)
	{
		if (!(Player == null))
		{
			return Player.SetShotStatus(this, shot, hitPoint, shotNormal, shotDirection);
		}
		return base.Deflects(_hitCosDirectionToNormal, shot, hitPoint, shotNormal, shotDirection);
	}

	public bool IsHitToArmor(Vector3 hitPoint, Vector3 shotNormal, Vector3 shotDirection)
	{
		if (Player == null)
		{
			return false;
		}
		return Player.CheckArmorHitByDirection(this, hitPoint, shotNormal, shotDirection);
	}

	public Vector3 GetRandomPointToCastLocal(Vector3 lookFromPoint)
	{
		Vector3 zero = Vector3.zero;
		Collider collider = Collider;
		if ((object)collider != null && collider is SphereCollider sphereCollider)
		{
			SphereCollider sphereCollider2 = sphereCollider;
			zero += sphereCollider2.center;
			Vector3 direction = lookFromPoint - base.transform.position;
			Vector3 forward = base.transform.InverseTransformDirection(direction);
			Vector2 vector = UnityEngine.Random.insideUnitCircle * sphereCollider2.radius;
			Vector3 vector2 = Quaternion.LookRotation(forward) * new Vector3(vector.x, vector.y, 0f);
			return zero + vector2;
		}
		throw new NotImplementedException();
	}
}
