using System;
using UnityEngine;

[DisallowMultipleComponent]
public class CharacterJointSpawner : JointSpawner
{
	[Serializable]
	public struct SoftJointLimitSpring_
	{
		public float spring;

		public float damper;

		public static implicit operator SoftJointLimitSpring_(SoftJointLimitSpring softJointLimitSpring)
		{
			SoftJointLimitSpring_ result = default(SoftJointLimitSpring_);
			result.spring = softJointLimitSpring.spring;
			result.damper = softJointLimitSpring.damper;
			return result;
		}

		public static implicit operator SoftJointLimitSpring(SoftJointLimitSpring_ softJointLimitSpring)
		{
			SoftJointLimitSpring result = default(SoftJointLimitSpring);
			result.spring = softJointLimitSpring.spring;
			result.damper = softJointLimitSpring.damper;
			return result;
		}
	}

	[Serializable]
	public struct SoftJointLimit_
	{
		public float limit;

		public float bounciness;

		public float contactDistance;

		public static implicit operator SoftJointLimit_(SoftJointLimit softJointLimit)
		{
			SoftJointLimit_ result = default(SoftJointLimit_);
			result.limit = softJointLimit.limit;
			result.bounciness = softJointLimit.bounciness;
			result.contactDistance = softJointLimit.contactDistance;
			return result;
		}

		public static implicit operator SoftJointLimit(SoftJointLimit_ softJointLimit)
		{
			SoftJointLimit result = default(SoftJointLimit);
			result.limit = softJointLimit.limit;
			result.bounciness = softJointLimit.bounciness;
			result.contactDistance = softJointLimit.contactDistance;
			return result;
		}
	}

	public Vector3 swingAxis;

	public SoftJointLimitSpring_ twistLimitSpring;

	public SoftJointLimit_ lowTwistLimit;

	public SoftJointLimit_ highTwistLimit;

	public SoftJointLimitSpring_ swingLimitSpring;

	public SoftJointLimit_ swing1Limit;

	public SoftJointLimit_ swing2Limit;

	public bool enableProjection;

	public float projectionDistance;

	public float projectionAngle;

	[ContextMenu("Create")]
	public override Joint Create()
	{
		if (_joint == null)
		{
			CharacterJoint obj = base.Create() as CharacterJoint;
			obj.swingAxis = swingAxis;
			obj.twistLimitSpring = twistLimitSpring;
			obj.lowTwistLimit = lowTwistLimit;
			obj.highTwistLimit = highTwistLimit;
			obj.swingLimitSpring = swingLimitSpring;
			obj.swing1Limit = swing1Limit;
			obj.swing2Limit = swing2Limit;
			obj.enableProjection = enableProjection;
			obj.projectionDistance = projectionDistance;
			obj.projectionAngle = projectionAngle;
		}
		return _joint;
	}

	protected override Joint CreateComponent()
	{
		return base.gameObject.AddComponent<CharacterJoint>();
	}
}
