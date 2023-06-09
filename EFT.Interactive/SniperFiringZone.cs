using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using UnityEngine;

namespace EFT.Interactive;

public class SniperFiringZone : BorderZone
{
	protected const float IMITATOR_NOTIFICATION_DELAY = 2f;

	[SerializeField]
	private float _shotFrequency = 3f;

	[SerializeField]
	private float _triggerZoneHitProbability = 1f;

	[SerializeField]
	private float _bufferZoneHitProbability = 0.66f;

	[SerializeField]
	private float _firstShotHitProbability = 0.5f;

	[SerializeField]
	private int _becomeLethalIfShotsCountExceed = 3;

	[SerializeField]
	private string _ammoTemplate = _ED3E._E000(212949);

	[SerializeField]
	private List<EBodyPart> _triggerZoneTarget = new List<EBodyPart>();

	[SerializeField]
	private List<EBodyPart> _bufferZoneTarget = new List<EBodyPart>();

	[SerializeField]
	private PlayersWithImmuneToSniperFireCollector _playersWithImmuneToFire;

	private _EA12 _E005;

	public _EA12 Ammo => _E005 ?? (_E005 = (_EA12)Singleton<_E63B>.Instance.CreateItem(new MongoID(newProcessId: false), _ammoTemplate, null));

	public float BulletSpeed => Ammo.InitialSpeed;

	private float _E000 => Ammo.Damage;

	public override float Delay => 2f;

	private bool _E000(Player player)
	{
		if (player.HealthController.IsAlive)
		{
			if (!(_playersWithImmuneToFire == null))
			{
				return !_playersWithImmuneToFire.IsPlayerImmuneForFire(player.ProfileId);
			}
			return true;
		}
		return false;
	}

	protected override IEnumerator FireCoroutine(Player player)
	{
		bool flag = true;
		int num = 0;
		while (TargetedPlayers.Contains(player) && _E000(player))
		{
			bool flag2 = IsInTriggerZone(player.Position);
			float num2 = (flag ? _firstShotHitProbability : (flag2 ? _triggerZoneHitProbability : _bufferZoneHitProbability));
			bool flag3 = Random.Range(0f, 1f) <= num2;
			InvokePlayerShotEvent(player, 2f, flag3);
			num = (flag2 ? (num + 1) : 0);
			yield return new WaitForSeconds(2f);
			if (flag3)
			{
				Shoot(player, flag2, flag, num > _becomeLethalIfShotsCountExceed);
			}
			flag = false;
			yield return new WaitForSeconds(_shotFrequency - 2f);
		}
	}

	protected virtual void Shoot(Player player, bool isInTriggerZone, bool first, bool lethal)
	{
		if (!(player == null) && TargetedPlayers.Contains(player))
		{
			BodyPartCollider[] array = (lethal ? player.PlayerBones.BodyPartColliders.Where((BodyPartCollider x) => x.BodyPartType == EBodyPart.Head).ToArray() : (isInTriggerZone ? player.PlayerBones.BodyPartColliders.Where((BodyPartCollider x) => _triggerZoneTarget.Contains(x.BodyPartType)).ToArray() : player.PlayerBones.BodyPartColliders.Where((BodyPartCollider x) => _bufferZoneTarget.Contains(x.BodyPartType)).ToArray()));
			if (array.Length >= 1)
			{
				BodyPartCollider bodyPartCollider = array[Random.Range(0, array.Length)];
				_EC23 obj = default(_EC23);
				obj.DamageType = EDamageType.Sniper;
				obj.Damage = this._E000;
				obj.ArmorDamage = (float)Ammo.ArmorDamage / 100f;
				obj.StaminaBurnRate = Ammo.StaminaBurnRate;
				obj.PenetrationPower = (lethal ? 100 : _E005.PenetrationPower);
				obj.Direction = Random.onUnitSphere;
				obj.HitCollider = bodyPartCollider.Collider;
				obj.HitNormal = Vector3.zero;
				obj.HitPoint = bodyPartCollider.Collider.transform.position;
				obj.HittedBallisticCollider = bodyPartCollider;
				obj.Player = null;
				obj.IsForwardHit = true;
				_EC23 damageInfo = obj;
				bodyPartCollider.ApplyHit(damageInfo, _EC22.EMPTY_SHOT_ID);
			}
		}
	}

	[CompilerGenerated]
	private bool _E001(BodyPartCollider x)
	{
		return _triggerZoneTarget.Contains(x.BodyPartType);
	}

	[CompilerGenerated]
	private bool _E002(BodyPartCollider x)
	{
		return _bufferZoneTarget.Contains(x.BodyPartType);
	}
}
