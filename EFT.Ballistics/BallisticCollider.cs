using System;
using System.Runtime.CompilerServices;
using System.Threading;
using EFT.NetworkPackets;
using UnityEngine;

namespace EFT.Ballistics;

public class BallisticCollider : BaseBallistic
{
	public const int OLD_PRESET_ID = -900;

	[HideInInspector]
	[SerializeField]
	private int _presetId = -900;

	public int NetId;

	public EHitType HitType;

	[SerializeField]
	private MaterialType _typeOfMaterial;

	[CompilerGenerated]
	private Action<_EC23> _E014;

	public float PenetrationLevel;

	[Range(0f, 1f)]
	public float PenetrationChance;

	[Range(0f, 1f)]
	public float RicochetChance;

	[Range(0f, 1f)]
	public float FragmentationChance;

	[Range(0f, 1f)]
	public float TrajectoryDeviationChance;

	[Range(0f, 1f)]
	public float TrajectoryDeviation;

	public MaterialType TypeOfMaterial
	{
		get
		{
			return _typeOfMaterial;
		}
		set
		{
			_typeOfMaterial = value;
		}
	}

	public event Action<_EC23> OnHitAction
	{
		[CompilerGenerated]
		add
		{
			Action<_EC23> action = _E014;
			Action<_EC23> action2;
			do
			{
				action2 = action;
				Action<_EC23> value2 = (Action<_EC23>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E014, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<_EC23> action = _E014;
			Action<_EC23> action2;
			do
			{
				action2 = action;
				Action<_EC23> value2 = (Action<_EC23>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E014, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public virtual void Awake()
	{
		Associate(TypeOfMaterial);
	}

	public override BallisticCollider Get(Vector3 pos)
	{
		return this;
	}

	public virtual _E6FF ApplyHit(_EC23 damageInfo, _EC22 shotID)
	{
		_E014?.Invoke(damageInfo);
		return null;
	}

	public bool IsUnsetup()
	{
		if (Mathf.Approximately(PenetrationLevel, 0f) && Mathf.Approximately(PenetrationChance, 0f) && Mathf.Approximately(RicochetChance, 0f) && Mathf.Approximately(FragmentationChance, 0f) && Mathf.Approximately(TrajectoryDeviationChance, 0f))
		{
			return Mathf.Approximately(TrajectoryDeviation, 0f);
		}
		return false;
	}

	public virtual bool Deflects(float _hitCosDirectionToNormal, _EC26 shot, Vector3 hitPoint, Vector3 shotNormal, Vector3 shotDirection)
	{
		if (RicochetChance >= float.Epsilon && Vector3.Angle(-shotDirection, shotNormal) > 42.5f && shot.RicochetChance > 0f)
		{
			return (RicochetChance + shot.RicochetChance) * (1f - Mathf.Abs(_hitCosDirectionToNormal)) > shot.Randoms.GetRandomFloat(shot.RandomSeed);
		}
		return false;
	}

	public virtual bool IsPenetrated(_EC26 shot, Vector3 hitPoint)
	{
		if (PenetrationChance >= float.Epsilon && shot.PenetrationPower > PenetrationLevel)
		{
			return shot.PenetrationChance + PenetrationChance > shot.Randoms.GetRandomFloat(shot.RandomSeed);
		}
		return false;
	}

	public override void TakeSettingsFrom(BaseBallistic collider)
	{
		base.TakeSettingsFrom(collider);
		if (collider is BallisticCollider ballisticCollider)
		{
			TypeOfMaterial = ballisticCollider.TypeOfMaterial;
			PenetrationLevel = ballisticCollider.PenetrationLevel;
			PenetrationChance = ballisticCollider.PenetrationChance;
			RicochetChance = ballisticCollider.RicochetChance;
			FragmentationChance = ballisticCollider.FragmentationChance;
			TrajectoryDeviationChance = ballisticCollider.TrajectoryDeviationChance;
			TrajectoryDeviation = ballisticCollider.TrajectoryDeviation;
		}
	}

	public virtual void UnsubscribeHitAction()
	{
		_E014 = null;
	}

	public int FindPresetIndex()
	{
		return FindPresetIndex(this);
	}

	public static int FindPresetIndex(BallisticCollider collider)
	{
		if (collider._presetId != -900)
		{
			return collider._presetId;
		}
		BallisticPreset[] colliderPresets = EFTHardSettings.Instance.ColliderPresets;
		for (int i = 0; i < colliderPresets.Length; i++)
		{
			if (colliderPresets[i].MaterialType == collider.TypeOfMaterial)
			{
				return i;
			}
		}
		return 0;
	}
}
