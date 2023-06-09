using System.Collections;
using System.Collections.Generic;
using Systems.Effects;
using Comfort.Common;
using UnityEngine;

namespace EFT.Interactive;

public class SniperImitator : MonoBehaviour
{
	private static readonly Quaternion m__E000 = Quaternion.Euler(-90f, 0f, 0f);

	[SerializeField]
	private FirearmsEffects FirearmsEffects;

	[SerializeField]
	private SniperFiringZone SniperFiringZone;

	[SerializeField]
	private SoundBank SoundBank;

	[SerializeField]
	private List<Transform> SniperPositions;

	[SerializeField]
	private Transform _effectsTransform;

	private bool m__E001;

	public Transform GetRandomSniperPosition => SniperPositions[Random.Range(0, SniperPositions.Count)];

	public SniperImitator(FirearmsEffects firearmsEffects, SniperFiringZone sniperFiringZone, SoundBank soundBank, List<Transform> sniperPositions)
	{
		FirearmsEffects = firearmsEffects;
		SniperFiringZone = sniperFiringZone;
		SoundBank = soundBank;
		SniperPositions = sniperPositions;
	}

	public void Awake()
	{
		SniperFiringZone.PlayerShotEvent += _E000;
	}

	private void _E000(Player player, BorderZone firingZone, float tMinus, bool willHit)
	{
		if (!this.m__E001)
		{
			this.m__E001 = true;
			FirearmsEffects.Init(base.transform);
		}
		Transform getRandomSniperPosition = GetRandomSniperPosition;
		Vector3 position = getRandomSniperPosition.position;
		float num = Vector3.Distance(position, player.Position) / SniperFiringZone.BulletSpeed;
		getRandomSniperPosition.rotation = Quaternion.LookRotation(player.Position + Vector3.up - position, Vector3.up) * SniperImitator.m__E000;
		StartCoroutine(_E001(tMinus - num, player, position, getRandomSniperPosition.rotation, willHit));
	}

	private IEnumerator _E001(float tMinus, Player player, Vector3 sniperPosition, Quaternion sniperRotation, bool willHit)
	{
		float num = _E8A8.Instance.SqrDistance(sniperPosition);
		float num2 = Mathf.Sqrt(num);
		float num3 = num2 / SniperFiringZone.BulletSpeed;
		yield return new WaitForSeconds(tMinus - num3);
		_effectsTransform.SetPositionAndRotation(sniperPosition, sniperRotation);
		if (Singleton<Effects>.Instantiated)
		{
			FirearmsEffects.StartFireEffects(isVisible: true, num);
		}
		if (Singleton<BetterAudio>.Instantiated)
		{
			MonoBehaviourSingleton<BetterAudio>.Instance.PlayAtPointDistant(sniperPosition, SoundBank, num2, 1f, 1f, EnvironmentType.Outdoor, EOcclusionTest.Fast);
		}
		yield return new WaitForSeconds(num3);
		if (willHit)
		{
			Collider collider = player.PlayerBones.BodyPartCollidersDictionary[EBodyPartColliderType.Ribcage].Collider;
			Transform transform = collider.transform;
			Vector3 normalized = (sniperPosition - transform.position).normalized;
			Vector3 vector = collider.ClosestPoint(transform.position + normalized);
			player.ShotReactions(new _EC23
			{
				DamageType = EDamageType.Sniper,
				HitCollider = collider,
				HitNormal = -normalized,
				HitPoint = vector
			}, EBodyPart.Chest);
			Singleton<Effects>.Instance.Emit(_ED3E._E000(205976), vector, normalized);
		}
		if (!(player is ObservedPlayer))
		{
			_E3BF.SniperZoneShoot(null);
			_E3BF.Shoot(new SonicBulletSoundPlayer._E001(SniperFiringZone.Ammo, sniperPosition, (player.Position - sniperPosition).normalized, _E8A8.Instance.Camera, SoundBank.Rolloff, 0f, isOccluded: false));
		}
	}

	private void OnDestroy()
	{
		SniperFiringZone.PlayerShotEvent -= _E000;
	}
}
