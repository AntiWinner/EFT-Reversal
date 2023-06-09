using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT.Interactive;

public class Minefield : BorderZone
{
	private const float _E004 = 1f;

	[SerializeField]
	private float _zoneDistortionPeriod = 0.21f;

	[SerializeField]
	private float _zoneDistortionPower = 2.5f;

	[SerializeField]
	private float _contusionStrength = 1f;

	[SerializeField]
	private float _contusionDuration = 20f;

	[SerializeField]
	private float _collateralContusionRange = 20f;

	[SerializeField]
	private float _collateralDamageRange = 10f;

	[SerializeField]
	private List<EBodyPart> _explosionTarget = new List<EBodyPart>();

	[SerializeField]
	private List<EBodyPart> _collateralDamageTarget = new List<EBodyPart>();

	[SerializeField]
	private float _firstExplosionDamage = 50f;

	[SerializeField]
	private float _minRangeToNextExplosion = 6f;

	[SerializeField]
	private float _maxRangeToNextExplosion = 10f;

	[SerializeField]
	private float _secondExplosionDamage = 200f;

	protected override bool IsInTriggerZone(Vector3 global)
	{
		Vector3 vector = base.transform.InverseTransformPoint(global);
		float num = Mathf.Cos((vector.x + vector.z) * _zoneDistortionPeriod) * _zoneDistortionPower;
		vector += new Vector3(num, 0f, num);
		Vector3 lossyScale = base.transform.lossyScale;
		if ((_extents.x + vector.x) * lossyScale.x < _triggerZoneSettings.x)
		{
			return false;
		}
		if ((_extents.x - vector.x) * lossyScale.x < _triggerZoneSettings.y)
		{
			return false;
		}
		if ((vector.z + _extents.z) * lossyScale.z < _triggerZoneSettings.z)
		{
			return false;
		}
		if ((_extents.z - vector.z) * lossyScale.z < _triggerZoneSettings.w)
		{
			return false;
		}
		return true;
	}

	protected override IEnumerator FireCoroutine(Player player)
	{
		InvokePlayerShotEvent(player, 0f);
		_E000(player, first: true);
		Vector3 position = player.Position;
		float num = Random.Range(_minRangeToNextExplosion, _maxRangeToNextExplosion);
		while (TargetedPlayers.Contains(player) && player.HealthController.IsAlive)
		{
			yield return new WaitForSeconds(1f);
			if (IsInTriggerZone(player.Position) && TargetedPlayers.Contains(player) && Vector3.Distance(position, player.Position) >= num)
			{
				InvokePlayerShotEvent(player, 0f);
				_E000(player, first: false);
				position = player.Position;
			}
		}
	}

	private void _E000(Player player, bool first)
	{
		Vector3 position = player.Position;
		foreach (Player item in TargetedPlayers.Concat(NotTargetedPlayers).ToList())
		{
			_E001(item, Vector3.Distance(position, item.Position), first, player != item);
		}
	}

	private void _E001(Player player, float distance, bool first, bool isCollateral)
	{
		if (isCollateral && distance > _collateralContusionRange)
		{
			return;
		}
		float num = (isCollateral ? Mathf.Clamp(1f - distance / _collateralContusionRange, 0.2f, 1f) : 1f);
		player.ActiveHealthController.DoContusion(_contusionDuration * num, _contusionStrength * num);
		if (isCollateral && distance > _collateralDamageRange)
		{
			return;
		}
		float num2 = 1f - distance / _collateralDamageRange;
		IEnumerable<BodyPartCollider> source = (isCollateral ? player.PlayerBones.BodyPartColliders.Where((BodyPartCollider x) => _collateralDamageTarget.Contains(x.BodyPartType)) : player.PlayerBones.BodyPartColliders.Where((BodyPartCollider x) => _explosionTarget.Contains(x.BodyPartType)));
		source = source.DistinctBy((BodyPartCollider x) => x.BodyPartType).ToArray();
		source = source.Randomize();
		int num3 = ((isCollateral || first) ? Random.Range(2, source.Count()) : int.MaxValue);
		float num4 = ((isCollateral || first) ? _firstExplosionDamage : _secondExplosionDamage);
		int num5 = 0;
		foreach (BodyPartCollider item in source)
		{
			_EC23 obj = default(_EC23);
			obj.DamageType = EDamageType.Landmine;
			obj.Damage = num4 * num2;
			obj.ArmorDamage = 0.5f;
			obj.StaminaBurnRate = 5f;
			obj.PenetrationPower = 30f;
			obj.Direction = Vector3.zero;
			obj.HitCollider = item.Collider;
			obj.HitNormal = Vector3.zero;
			obj.HitPoint = Vector3.zero;
			obj.HittedBallisticCollider = item;
			obj.Player = null;
			obj.IsForwardHit = true;
			_EC23 damageInfo = obj;
			item.ApplyHit(damageInfo, _EC22.EMPTY_SHOT_ID);
			if (++num5 >= num3)
			{
				break;
			}
		}
	}

	[CompilerGenerated]
	private bool _E002(BodyPartCollider x)
	{
		return _collateralDamageTarget.Contains(x.BodyPartType);
	}

	[CompilerGenerated]
	private bool _E003(BodyPartCollider x)
	{
		return _explosionTarget.Contains(x.BodyPartType);
	}
}
