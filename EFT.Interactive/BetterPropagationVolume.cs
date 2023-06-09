using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Comfort.Common;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT.Interactive;

public class BetterPropagationVolume : MonoBehaviour, IPhysicsTrigger
{
	public BoxCollider Collider;

	public bool MutuallyExclusive;

	public EVolumeRelations SelfRelation = EVolumeRelations.Connected;

	public bool IsIsolated;

	[Range(0f, 1f)]
	public float SelfOcclusionIntensity = 1f;

	[SerializeField]
	private bool _decayEnabled = true;

	[CompilerGenerated]
	private readonly string m__E000 = _ED3E._E000(205894);

	[HideInInspector]
	public EVolumeRelationsMask SelfRelationMask;

	public List<VolumeConnection> Connections;

	public Vector4 PropagationDistance = new Vector4(1f, 1f, 1f, 1f);

	public string Description
	{
		[CompilerGenerated]
		get
		{
			return this.m__E000;
		}
	}

	private void Start()
	{
		_E001();
		Singleton<BetterAudio>.Instance.RegisterVolume(this);
		base.gameObject.layer = LayerMask.NameToLayer(_ED3E._E000(25347));
	}

	public void OnTriggerEnter(Collider col)
	{
		Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(col);
		if ((bool)playerByCollider)
		{
			playerByCollider.AddVolume(this);
		}
		if (Singleton<_E482>.Instantiated && playerByCollider is ClientPlayer)
		{
			Singleton<_E482>.Instance.AddPlayerCurrentPropagationVolume(this);
		}
	}

	public void OnTriggerExit(Collider col)
	{
		Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(col);
		if ((bool)playerByCollider)
		{
			playerByCollider.RemoveVolume(this);
		}
		if (Singleton<_E482>.Instantiated && playerByCollider is ClientPlayer)
		{
			Singleton<_E482>.Instance.RemovePlayerCurrentVolume(this);
		}
	}

	public EVolumeRelationsMask GetRelation(BetterPropagationVolume volume)
	{
		if (volume == this)
		{
			return SelfRelationMask;
		}
		foreach (VolumeConnection connection in Connections)
		{
			if (connection.Volume == volume)
			{
				return connection.RelationsMask;
			}
		}
		return EVolumeRelationsMask.NotRelative;
	}

	public float Audibility(Vector3 globalPosition)
	{
		if (!_decayEnabled)
		{
			return 1f;
		}
		Vector3 localPosition = base.transform.InverseTransformPoint(globalPosition);
		return _E000(localPosition);
	}

	public float ConnectionAudibility(BetterPropagationVolume volume)
	{
		if (volume == this)
		{
			return 1f;
		}
		foreach (VolumeConnection connection in Connections)
		{
			if (connection.Volume == volume)
			{
				return connection.ConnectionAudibility;
			}
		}
		return 1f;
	}

	public bool UseManualAudibilitySettings(BetterPropagationVolume volume)
	{
		foreach (VolumeConnection connection in Connections)
		{
			if (!(connection.Volume == volume))
			{
				continue;
			}
			return connection.UseManualAudibilitySettings;
		}
		return false;
	}

	private float _E000(Vector3 localPosition)
	{
		Vector3 vector = Collider.size / 2f;
		Vector3 lossyScale = base.transform.lossyScale;
		float num = ((PropagationDistance.x > 0f) ? Mathf.InverseLerp(0f, PropagationDistance.x, (vector.x + localPosition.x) * lossyScale.x) : 1f);
		float num2 = ((PropagationDistance.y > 0f) ? Mathf.InverseLerp(0f, PropagationDistance.y, (vector.x - localPosition.x) * lossyScale.x) : 1f);
		float num3 = ((PropagationDistance.z > 0f) ? Mathf.InverseLerp(0f, PropagationDistance.z, (localPosition.z + vector.z) * lossyScale.z) : 1f);
		float num4 = ((PropagationDistance.w > 0f) ? Mathf.InverseLerp(0f, PropagationDistance.w, (vector.z - localPosition.z) * lossyScale.z) : 1f);
		return Mathf.Min(num, num2, num3, num4);
	}

	public static _EC11 GetRelationByPosition(Vector3 listenerPosition, Vector3 sourcePosition)
	{
		float audibility = 1f;
		EVolumeRelationsMask eVolumeRelationsMask = EVolumeRelationsMask.NotRelative;
		bool flag = false;
		_EC11 obj = default(_EC11);
		obj.UseManualAudibilitySettings = false;
		obj.RelationType = eVolumeRelationsMask;
		obj.Audibility = audibility;
		_EC11 result = obj;
		BetterPropagationVolume playerCurrentPropagationVolume = Singleton<_E482>.Instance.GetPlayerCurrentPropagationVolume();
		if ((object)playerCurrentPropagationVolume == null)
		{
			return result;
		}
		BetterPropagationVolume volumeByPosition = Singleton<_E482>.Instance.GetVolumeByPosition(sourcePosition);
		if ((object)volumeByPosition == null)
		{
			return result;
		}
		if (volumeByPosition.IsIsolated || playerCurrentPropagationVolume.IsIsolated)
		{
			eVolumeRelationsMask |= EVolumeRelationsMask.Isolated;
		}
		EVolumeRelationsMask relation = playerCurrentPropagationVolume.GetRelation(volumeByPosition);
		audibility = playerCurrentPropagationVolume.ConnectionAudibility(volumeByPosition);
		if (volumeByPosition.SelfRelation == EVolumeRelations.Stairs && playerCurrentPropagationVolume.SelfRelation == EVolumeRelations.Stairs)
		{
			flag = true;
		}
		eVolumeRelationsMask |= relation;
		if (flag)
		{
			eVolumeRelationsMask = EVolumeRelationsMask.Stairs;
		}
		result.RelationType = eVolumeRelationsMask;
		result.Audibility = audibility;
		result.UseManualAudibilitySettings = playerCurrentPropagationVolume.UseManualAudibilitySettings(volumeByPosition);
		return result;
	}

	public static _EC11 GetRelation([CanBeNull] Player player, [CanBeNull] Player sourcePlayer, Vector3 lPosition, Vector3 sPosition)
	{
		List<BetterPropagationVolume> list = ((player != null && player.PointOfView == EPointOfView.FirstPerson) ? player.GetPropagationVolume() : Singleton<_E482>.Instance.GetVolumesByPosition(lPosition));
		List<BetterPropagationVolume> list2 = ((list.Count <= 0) ? Singleton<_E482>.Instance.GetVolumesByPosition(sPosition) : ((sourcePlayer != null) ? sourcePlayer.GetPropagationVolume() : Singleton<_E482>.Instance.GetAdjustedAndIsolatedVolumes(sPosition, list)));
		float num = 1f;
		EVolumeRelationsMask eVolumeRelationsMask = EVolumeRelationsMask.NotRelative;
		bool flag = false;
		for (int i = 0; i < list2.Count; i++)
		{
			BetterPropagationVolume betterPropagationVolume = list2[i];
			if (betterPropagationVolume.IsIsolated)
			{
				eVolumeRelationsMask |= EVolumeRelationsMask.Isolated;
			}
			for (int j = 0; j < list.Count; j++)
			{
				BetterPropagationVolume betterPropagationVolume2 = list[j];
				EVolumeRelationsMask relation = betterPropagationVolume2.GetRelation(betterPropagationVolume);
				float num2 = betterPropagationVolume2.ConnectionAudibility(betterPropagationVolume);
				if (betterPropagationVolume.SelfRelation == EVolumeRelations.Stairs && betterPropagationVolume2.SelfRelation == EVolumeRelations.Stairs)
				{
					flag = true;
				}
				if (relation != EVolumeRelationsMask.NotRelative)
				{
					eVolumeRelationsMask |= relation;
					float num3 = betterPropagationVolume2.Audibility(lPosition) * betterPropagationVolume.Audibility(sPosition);
					if (num3 >= num)
					{
						num = num3 * num2;
					}
				}
			}
		}
		if (list2.Count < 1)
		{
			for (int k = 0; k < list.Count; k++)
			{
				if (list[k].IsIsolated)
				{
					eVolumeRelationsMask |= EVolumeRelationsMask.Isolated;
					break;
				}
			}
		}
		if (flag)
		{
			eVolumeRelationsMask = EVolumeRelationsMask.Stairs;
		}
		_EC11 result = default(_EC11);
		result.RelationType = eVolumeRelationsMask;
		result.Audibility = num;
		return result;
	}

	private void OnDestroy()
	{
		if (Singleton<BetterAudio>.Instance != null)
		{
			Singleton<BetterAudio>.Instance.RemoveVolume(this);
		}
	}

	[ContextMenu("ConvertEnums")]
	private void _E001()
	{
		if (SelfRelation == EVolumeRelations.Isolated)
		{
			IsIsolated = true;
			SelfRelationMask = EVolumeRelationsMask.Self;
		}
		else
		{
			SelfRelationMask = (EVolumeRelationsMask)Enum.Parse(typeof(EVolumeRelationsMask), SelfRelation.ToString());
		}
		foreach (VolumeConnection connection in Connections)
		{
			connection.RelationsMask = (EVolumeRelationsMask)Enum.Parse(typeof(EVolumeRelationsMask), connection.Relations.ToString());
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
