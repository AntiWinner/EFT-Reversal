using System.Collections.Generic;
using Comfort.Common;
using EFT;
using EFT.Ballistics;
using UnityEngine;

namespace Systems.Effects;

public class EffectsCommutator : MonoBehaviour
{
	[SerializeField]
	private Vector2 _minMaxBleedingSpawnDelta = new Vector2(1f, 3f);

	private Effects.Effect[] _E000;

	private List<Vector3> _E001 = new List<Vector3>(16);

	private readonly List<KeyValuePair<Player, float>> _E002 = new List<KeyValuePair<Player, float>>(3);

	public void PlayerMeshesHit(List<_E3D2> renderers, Vector3 point, Vector3 direction)
	{
		Singleton<Effects>.Instance.PlayerMeshesHit(renderers, point, direction);
	}

	public void PlayHitEffect(_EC26 info, _E6FF playerHitInfo)
	{
		if (IsHitPointAlreadyProcessed(info.HitPoint))
		{
			return;
		}
		_E001.Add(info.HitPoint);
		float num = ((info.FragmentIndex > 0) ? (0.5f / (float)info.FragmentIndex) : 1f);
		if (info.HittedBallisticCollider == null)
		{
			return;
		}
		Vector3 position = info.HitPoint + info.HitNormal * EFTHardSettings.Instance.DECAL_SHIFT;
		float num2 = Mathf.InverseLerp(64f, 256f, info.VelocitySqrMagnitude);
		MaterialType material = info.HittedBallisticCollider.TypeOfMaterial;
		EPointOfView pov = EPointOfView.ThirdPerson;
		if (playerHitInfo != null)
		{
			material = playerHitInfo.Material;
			pov = playerHitInfo.PoV;
			if (playerHitInfo.Penetrated)
			{
				CheckEnvironmentHitAfterBodyHit(info);
			}
			else if (playerHitInfo.Silent)
			{
				num2 = 0f;
			}
		}
		else if (info.BulletState == _EC26.EBulletState.RicochetHit)
		{
			Singleton<BetterAudio>.Instance.PlayAtPoint(position, Singleton<Effects>.Instance.AdditionalSoundEffects[0], _E8A8.Instance.Distance(position), 1f, -1f, EnvironmentType.Outdoor, EOcclusionTest.Fast);
		}
		bool flag = info.FragmentIndex == 0 && info.Player != null && info.Player.PointOfView == EPointOfView.FirstPerson;
		if (!flag)
		{
			Vector3 vector = _E8A8.Instance.Camera.WorldToViewportPoint(info.HitPoint);
			flag = vector.z > 0f && vector.x > -0.01f && vector.x < 1.01f && vector.y > -0.01f && vector.y < 1.01f;
		}
		Singleton<Effects>.Instance.Emit(material, info.HittedBallisticCollider, position, info.HitNormal, info.IsForwardHit ? (num * num2) : 0f, isKnife: false, flag, pov);
	}

	private bool IsHitPointAlreadyProcessed(Vector3 hitPoint)
	{
		for (int i = 0; i < _E001.Count; i++)
		{
			Vector3 vector = _E001[i];
			if (Mathf.Abs(hitPoint.x - vector.x) < 0.025f && Mathf.Abs(hitPoint.y - vector.y) < 0.025f && Mathf.Abs(hitPoint.z - vector.z) < 0.025f)
			{
				return true;
			}
		}
		return false;
	}

	private void Update()
	{
		UpdatePlayersBleedings();
	}

	private void LateUpdate()
	{
		_E001.Clear();
	}

	private static void CheckEnvironmentHitAfterBodyHit(_EC26 fragment)
	{
		if (Physics.Raycast(fragment.HitPoint, fragment.Direction, out var hitInfo, EFTHardSettings.Instance.DRAW_BLOOD_ON_WALLS_MAX_DISTANCE, EFTHardSettings.Instance.ENVIRONMENT_HIT_MASK))
		{
			BallisticCollider component = hitInfo.collider.gameObject.GetComponent<BallisticCollider>();
			if (component != null && !(component is BodyPartCollider))
			{
				Singleton<Effects>.Instance.EmitBloodOnEnvironment(hitInfo.point, hitInfo.normal);
			}
		}
	}

	public void PlayKnifeHitEffect(_EC23 info)
	{
		if (info.HittedBallisticCollider == null)
		{
			return;
		}
		float volume = 1f;
		Vector3 position = info.HitPoint + info.HitNormal * EFTHardSettings.Instance.DECAL_SHIFT;
		if (info.Weapon is _EA60 obj && obj.KnifeComponent.Template.DisplayOnModel)
		{
			volume = 0f;
			int num = 4;
			if (info.HittedBallisticCollider.TypeOfMaterial == MaterialType.Body || info.HittedBallisticCollider.TypeOfMaterial == MaterialType.BodyArmor)
			{
				num = 3;
			}
			Singleton<BetterAudio>.Instance.PlayAtPoint(position, Singleton<Effects>.Instance.AdditionalSoundEffects[num], _E8A8.Instance.Distance(position), 1f, -1f, EnvironmentType.Outdoor, EOcclusionTest.Fast);
		}
		Singleton<Effects>.Instance.Emit(info.HittedBallisticCollider.TypeOfMaterial, info.HittedBallisticCollider, position, info.HitNormal, volume, isKnife: true);
	}

	public void StartBleedingForPlayer(Player player)
	{
		bool flag = false;
		foreach (KeyValuePair<Player, float> item in _E002)
		{
			flag = item.Key == player;
			if (flag)
			{
				break;
			}
		}
		if (!flag)
		{
			_E002.Add(new KeyValuePair<Player, float>(player, 0f));
		}
	}

	public void StopBleedingForPlayer(Player player)
	{
		int num = -1;
		for (int i = 0; i < _E002.Count; i++)
		{
			if (_E002[i].Key == player)
			{
				num = i;
				break;
			}
		}
		if (num >= 0)
		{
			_E002.RemoveAt(num);
		}
	}

	public void UpdatePlayersBleedings()
	{
		float time = Time.time;
		for (int i = 0; i < _E002.Count; i++)
		{
			KeyValuePair<Player, float> keyValuePair = _E002[i];
			float value = keyValuePair.Value;
			if (time > value && Physics.Raycast(keyValuePair.Key.Position, Vector3.down, out var hitInfo, EFTHardSettings.Instance.DRAW_BLEEDING_MAX_DISTANCE, EFTHardSettings.Instance.ENVIRONMENT_HIT_MASK))
			{
				Singleton<Effects>.Instance.EmitBleeding(hitInfo.point, hitInfo.normal);
				float value2 = time + Random.Range(_minMaxBleedingSpawnDelta.x, _minMaxBleedingSpawnDelta.y);
				_E002[i] = new KeyValuePair<Player, float>(keyValuePair.Key, value2);
			}
		}
	}
}
