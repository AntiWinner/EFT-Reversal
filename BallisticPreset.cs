using System;
using EFT.Ballistics;
using UnityEngine;

[Serializable]
public sealed class BallisticPreset
{
	public string Name;

	public MaterialType MaterialType;

	public float[] values = new float[6];

	[Tooltip("PenetrationLevel зависит от скейла")]
	public bool DependOnScale;

	[Tooltip("График множителя PenetrationLevel(value) от скейла(time)")]
	public AnimationCurve ScaleToPenetrationCurve;

	public float this[int i]
	{
		get
		{
			return values[i];
		}
		set
		{
			values[i] = value;
		}
	}

	public void ApplyScaleMult(BallisticCollider ballisticCollider, in Vector3 scale)
	{
		if (DependOnScale && ScaleToPenetrationCurve != null)
		{
			float time = Mathf.Max(scale.x, scale.y);
			ballisticCollider.PenetrationLevel *= ScaleToPenetrationCurve.Evaluate(time);
		}
	}

	public void ApplyPresetValues(BallisticCollider bCollider)
	{
		bCollider.TypeOfMaterial = MaterialType;
		bCollider.PenetrationLevel = this[0];
		bCollider.PenetrationChance = this[1];
		bCollider.RicochetChance = this[2];
		bCollider.FragmentationChance = this[3];
		bCollider.TrajectoryDeviationChance = this[4];
		bCollider.TrajectoryDeviation = this[5];
	}

	public bool IsValid(BallisticCollider bCollider)
	{
		if (bCollider.TypeOfMaterial == MaterialType && Math.Abs(bCollider.PenetrationLevel - this[0]) < 1E-05f && Math.Abs(bCollider.PenetrationChance - this[1]) < 1E-05f && Math.Abs(bCollider.RicochetChance - this[2]) < 1E-05f && Math.Abs(bCollider.FragmentationChance - this[3]) < 1E-05f && Math.Abs(bCollider.TrajectoryDeviationChance - this[4]) < 1E-05f)
		{
			return Math.Abs(bCollider.TrajectoryDeviation - this[5]) < 1E-05f;
		}
		return false;
	}
}
