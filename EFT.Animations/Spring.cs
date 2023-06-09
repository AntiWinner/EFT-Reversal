using System;
using UnityEngine;

namespace EFT.Animations;

[Serializable]
public class Spring
{
	public enum VecComponent
	{
		X,
		Y,
		Z
	}

	public Vector3 Zero;

	public float InputIntensity = 1f;

	public float ReturnSpeed = 0.1f;

	public float Damping = 0.5f;

	public Vector3 Min = -Vector3.one * 180f;

	public Vector3 Max = Vector3.one * 180f;

	public float AccelerationMax = 10f;

	private AnimationCurve _curveLimitX;

	private AnimationCurve _curveLimitY;

	private AnimationCurve _curveLimitZ;

	protected float CurveTime;

	public bool Refrash;

	public float Softness = 4f;

	public VecComponent X;

	public VecComponent Y = VecComponent.Y;

	public VecComponent Z = VecComponent.Z;

	public Vector3 Velocity;

	public Vector3 Current;

	protected Vector3 ForceAccumulator;

	protected Vector3 ForceAccumulatorLimitless;

	public bool _update;

	public virtual void FixedUpdate(float dt, int nFixedFrames = 1)
	{
		if (_update)
		{
			Velocity += (_E450.Limit(ForceAccumulator, AccelerationMax) + ForceAccumulatorLimitless) * InputIntensity;
			ForceAccumulator = Vector3.zero;
			ForceAccumulatorLimitless = Vector3.zero;
			_update = false;
			CurveTime = 0f;
		}
		Velocity -= (Current - Zero) * ReturnSpeed;
		Velocity *= Damping;
		Current += Velocity;
	}

	public void Reset()
	{
		Current = Vector3.zero;
	}

	public Vector3 Get()
	{
		return Zero + Current;
	}

	public Vector3 GetRelative()
	{
		return Current;
	}

	public Vector3 GetWithRedirection()
	{
		Vector3 vector = Get();
		return new Vector3(vector[(int)X], vector[(int)Y], vector[(int)Z]);
	}

	private Vector3 _E000(Vector3 vec)
	{
		if (_curveLimitX == null || Refrash)
		{
			Refrash = false;
			_curveLimitX = _E001(Min.x, Max.x, Softness);
			_curveLimitY = _E001(Min.y, Max.y, Softness);
			_curveLimitZ = _E001(Min.z, Max.z, Softness);
		}
		vec.x = _curveLimitX.Evaluate(vec.x);
		vec.y = _curveLimitY.Evaluate(vec.y);
		vec.z = _curveLimitZ.Evaluate(vec.z);
		return vec;
	}

	private static AnimationCurve _E001(float min, float max, float softness = 1f)
	{
		float num = 2f / softness;
		AnimationCurve animationCurve = new AnimationCurve(new Keyframe(min * softness, min, 0f, 0f), new Keyframe(0f, 0f, num, num), new Keyframe(max * softness, max, 0f, 0f));
		WrapMode preWrapMode = (animationCurve.postWrapMode = WrapMode.Once);
		animationCurve.preWrapMode = preWrapMode;
		return animationCurve;
	}

	public void AddAcceleration(int comp, float val)
	{
		ForceAccumulator[comp] += val;
		_update = true;
	}

	public void AddZero()
	{
		_update = true;
	}

	public void AddAcceleration(Vector3 acceleration)
	{
		ForceAccumulator += acceleration;
		_update = true;
	}

	public void AddAccelerationLimitless(int comp, float val)
	{
		ForceAccumulatorLimitless[comp] += val;
		_update = true;
	}

	public void AddAccelerationLimitless(Vector3 acceleration)
	{
		ForceAccumulatorLimitless += acceleration;
		_update = true;
	}
}
