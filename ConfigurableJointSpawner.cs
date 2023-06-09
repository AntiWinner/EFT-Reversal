using System;
using UnityEngine;

[DisallowMultipleComponent]
public class ConfigurableJointSpawner : JointSpawner
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

	[Serializable]
	public struct JointDrive_
	{
		public float positionSpring;

		public float positionDamper;

		public float maximumForce;

		public static implicit operator JointDrive_(JointDrive jointDrive)
		{
			JointDrive_ result = default(JointDrive_);
			result.positionSpring = jointDrive.positionSpring;
			result.positionDamper = jointDrive.positionDamper;
			result.maximumForce = jointDrive.maximumForce;
			return result;
		}

		public static implicit operator JointDrive(JointDrive_ jointDrive)
		{
			JointDrive result = default(JointDrive);
			result.positionSpring = jointDrive.positionSpring;
			result.positionDamper = jointDrive.positionDamper;
			result.maximumForce = jointDrive.maximumForce;
			return result;
		}
	}

	public float projectionAngle;

	public float projectionDistance;

	[_E376(typeof(JointProjectionMode))]
	public JointProjectionMode projectionMode;

	public JointDrive_ slerpDrive;

	public JointDrive_ angularYZDrive;

	public JointDrive_ angularXDrive;

	[_E376(typeof(RotationDriveMode))]
	public RotationDriveMode rotationDriveMode;

	public Vector3 targetAngularVelocity;

	public Quaternion targetRotation;

	public JointDrive_ zDrive;

	public JointDrive_ yDrive;

	public JointDrive_ xDrive;

	public Vector3 targetVelocity;

	public Vector3 targetPosition;

	public SoftJointLimit_ angularZLimit;

	public SoftJointLimit_ angularYLimit;

	public SoftJointLimit_ highAngularXLimit;

	public SoftJointLimit_ lowAngularXLimit;

	public SoftJointLimit_ linearLimit;

	public SoftJointLimitSpring_ angularYZLimitSpring;

	public SoftJointLimitSpring_ angularXLimitSpring;

	public SoftJointLimitSpring_ linearLimitSpring;

	[_E376(typeof(ConfigurableJointMotion))]
	public ConfigurableJointMotion angularZMotion;

	[_E376(typeof(ConfigurableJointMotion))]
	public ConfigurableJointMotion angularYMotion;

	[_E376(typeof(ConfigurableJointMotion))]
	public ConfigurableJointMotion angularXMotion;

	[_E376(typeof(ConfigurableJointMotion))]
	public ConfigurableJointMotion zMotion;

	[_E376(typeof(ConfigurableJointMotion))]
	public ConfigurableJointMotion yMotion;

	[_E376(typeof(ConfigurableJointMotion))]
	public ConfigurableJointMotion xMotion;

	public Vector3 secondaryAxis;

	public bool configuredInWorldSpace;

	public bool swapBodies;

	public override Joint Create()
	{
		if (_joint == null)
		{
			ConfigurableJoint obj = base.Create() as ConfigurableJoint;
			obj.projectionAngle = projectionAngle;
			obj.projectionDistance = projectionDistance;
			obj.projectionMode = projectionMode;
			obj.slerpDrive = slerpDrive;
			obj.angularYZDrive = angularYZDrive;
			obj.angularXDrive = angularXDrive;
			obj.rotationDriveMode = rotationDriveMode;
			obj.targetAngularVelocity = targetAngularVelocity;
			obj.targetRotation = targetRotation;
			obj.zDrive = zDrive;
			obj.yDrive = yDrive;
			obj.xDrive = xDrive;
			obj.targetVelocity = targetVelocity;
			obj.targetPosition = targetPosition;
			obj.angularZLimit = angularZLimit;
			obj.angularYLimit = angularYLimit;
			obj.highAngularXLimit = highAngularXLimit;
			obj.lowAngularXLimit = lowAngularXLimit;
			obj.linearLimit = linearLimit;
			obj.angularYZLimitSpring = angularYZLimitSpring;
			obj.angularXLimitSpring = angularXLimitSpring;
			obj.linearLimitSpring = linearLimitSpring;
			obj.angularZMotion = angularZMotion;
			obj.angularYMotion = angularYMotion;
			obj.angularXMotion = angularXMotion;
			obj.zMotion = zMotion;
			obj.yMotion = yMotion;
			obj.xMotion = xMotion;
			obj.secondaryAxis = secondaryAxis;
			obj.configuredInWorldSpace = configuredInWorldSpace;
			obj.swapBodies = swapBodies;
		}
		return _joint;
	}

	protected override Joint CreateComponent()
	{
		return base.gameObject.AddComponent<ConfigurableJoint>();
	}
}
