using System;
using UnityEngine;

namespace EFT.Animations;

[Serializable]
public class RecoilSpring : Spring
{
	public float ReturnDtMult = 1f;

	public AnimationCurve ReturnSpeedCurve;

	private float[] _originalKeyValues;

	public void GatherValues()
	{
		if (_originalKeyValues == null)
		{
			_originalKeyValues = new float[ReturnSpeedCurve.length];
			float value = ReturnSpeedCurve[0].value;
			for (int i = 0; i < ReturnSpeedCurve.length; i++)
			{
				_originalKeyValues[i] = ReturnSpeedCurve[i].value - value;
			}
		}
	}

	public void SetCurveParameters(float mult)
	{
		float value = ReturnSpeedCurve[0].value;
		for (int i = 1; i < _originalKeyValues.Length; i++)
		{
			Keyframe key = ReturnSpeedCurve[i];
			key.value = value + _originalKeyValues[i] * mult;
			ReturnSpeedCurve.RemoveKey(i);
			ReturnSpeedCurve.AddKey(key);
		}
	}

	public override void FixedUpdate(float dt, int nFixedFrames = 1)
	{
		if (_update)
		{
			Velocity += (_E450.Limit(ForceAccumulator, AccelerationMax) + ForceAccumulatorLimitless) * InputIntensity;
			ForceAccumulator = Vector3.zero;
			ForceAccumulatorLimitless = Vector3.zero;
			_update = false;
			CurveTime = Mathf.Min(Current.sqrMagnitude, CurveTime);
		}
		CurveTime += dt;
		Velocity *= Damping;
		Velocity -= Current * (ReturnSpeedCurve.Evaluate(CurveTime) * ReturnSpeed);
		Current += Velocity;
	}
}
