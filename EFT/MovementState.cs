using System;
using EFT.Interactive;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT;

[Serializable]
public class MovementState
{
	protected _E70F MovementContext;

	public float NormalizedTime;

	public EPlayerState Name;

	public EStateType Type;

	public string AnimatorStateName;

	public int AnimatorStateHash;

	public EMovementDirection AdditionalDirectionInfo;

	public float StateLength;

	public float RotationSpeedClamp = 99f;

	public float StateSensitivity = 1f;

	public float AnimationAuthority;

	public float AuthoritySpeed = 4f;

	protected bool StickToGround = true;

	public bool PlantMultitool;

	public float PlantTime = 10f;

	public static Vector3 G = Physics.gravity;

	public static float G_y = Physics.gravity.y;

	protected Vector3? Destination;

	private Vector3 _currentDownForce;

	private Vector3 _motionShortage;

	private _E388 _sqrShortage = new _E388(3);

	public bool DisableRootMotion
	{
		set
		{
			MovementContext.IgnoreDeltaMovement = value;
		}
	}

	protected float FreefallTime
	{
		get
		{
			return MovementContext.FreefallTime;
		}
		set
		{
			MovementContext.FreefallTime = value;
		}
	}

	public virtual bool CanLoot => true;

	public bool CanInteract { get; set; }

	public virtual void Init(_E70F movementContext)
	{
		MovementContext = movementContext;
	}

	public void OnStateEnter(float normalizedTime)
	{
		NormalizedTime = normalizedTime;
		MovementContext?.ProcessStateEnter(this);
	}

	public void OnStateMove(float normalizedTime)
	{
		NormalizedTime = normalizedTime;
		if (MovementContext.OverridenState != null)
		{
			MovementContext.OverridenState.ManualAnimatorMoveUpdate(MovementContext.LastDeltaTime);
		}
		else if (MovementContext.CurrentState == this)
		{
			ManualAnimatorMoveUpdate(MovementContext.LastDeltaTime);
		}
	}

	public void OnStateExit(float normalizedTime)
	{
		NormalizedTime = normalizedTime;
	}

	public virtual void Enter(bool isFromSameState)
	{
	}

	public virtual void ReEnter()
	{
	}

	public virtual void Exit(bool toSameState)
	{
	}

	protected virtual void ManualAnimatorMoveUpdate(float deltaTime)
	{
		ProcessRotation(deltaTime);
		ProcessAnimatorMovement(deltaTime);
	}

	protected virtual void ProcessRotation(float deltaTime)
	{
		if (Mathf.Abs(MovementContext.HandsToBodyAngle) > MovementContext.TrunkRotationLimit)
		{
			ProcessUpperbodyRotation(deltaTime);
		}
		UpdateRotationSpeed(deltaTime);
		Debug.DrawRay(MovementContext.TransformPosition + 1.5f * Vector3.up, MovementContext.TransformForwardVector, Color.red);
		Debug.DrawRay(MovementContext.TransformPosition + 1.5f * Vector3.up, MovementContext.PlayerRealForward, Color.green);
	}

	protected virtual void ProcessUpperbodyRotation(float deltaTime)
	{
		float y = Mathf.DeltaAngle((float)Math.Sign(MovementContext.HandsToBodyAngle) * MovementContext.TrunkRotationLimit, MovementContext.HandsToBodyAngle);
		MovementContext.ApplyRotation(Quaternion.Lerp(MovementContext.TransformRotation, MovementContext.TransformRotation * Quaternion.Euler(0f, y, 0f), 30f * deltaTime));
	}

	protected void UpdateRotationSpeed(float deltaTime)
	{
		MovementContext.AddRotaionDelta(Mathf.DeltaAngle(MovementContext.PreviousYaw, MovementContext.Yaw) / deltaTime);
	}

	protected virtual void LimitMotion(ref Vector3 motion, float deltaTime)
	{
		MovementContext.LimitMotionXZ(ref motion, deltaTime);
	}

	protected virtual void ApplyGravity(ref Vector3 motion, float deltaTime)
	{
		MovementContext.ApplyGravity(ref motion, deltaTime, StickToGround);
	}

	public void CalculateMotionShortage()
	{
		if (Destination.HasValue)
		{
			_motionShortage = Destination.Value - MovementContext.TransformPosition;
			_sqrShortage.AddValue(_motionShortage.x * _motionShortage.x + _motionShortage.z * _motionShortage.z);
			Destination = null;
		}
	}

	protected virtual void ApplyMotion(ref Vector3 motion, float deltaTime)
	{
		BlendMotion(ref motion, deltaTime);
		MovementContext.LastBlendMotionDelta = motion / deltaTime;
		MovementContext.ProjectMotionToSurface(ref motion);
		ApplyGravity(ref motion, deltaTime);
		LimitMotion(ref motion, deltaTime);
		MovementContext.ApplyMotion(motion, deltaTime);
	}

	protected virtual void ProcessAnimatorMovement(float deltaTime)
	{
		Vector3 motion = MovementContext.PlayerAnimatorDeltaPosition;
		ApplyMotion(ref motion, deltaTime);
		Destination = MovementContext.TransformPosition + motion;
		MovementContext.ApplyRotation(MovementContext.TransformRotation * MovementContext.AnimatorDeltaRotation);
	}

	protected virtual void BlendMotion(ref Vector3 motion, float deltaTime)
	{
	}

	public virtual void Move(Vector2 direction)
	{
	}

	public virtual void Rotate(Vector2 deltaRotation, bool ignoreClamp = false)
	{
		if (!ignoreClamp)
		{
			deltaRotation = ClampRotation(deltaRotation);
		}
		MovementContext.Rotation += deltaRotation;
	}

	public Vector3 ClampRotation(Vector3 deltaRotation)
	{
		if (RotationSpeedClamp <= 0f)
		{
			return Vector3.zero;
		}
		deltaRotation = MovementContext.ApplyExternalSense(deltaRotation) * StateSensitivity;
		if (deltaRotation.magnitude > RotationSpeedClamp)
		{
			deltaRotation *= RotationSpeedClamp / deltaRotation.magnitude;
		}
		return deltaRotation;
	}

	public virtual void Loot(bool enabled)
	{
	}

	public virtual void Pickup(bool enabled, [CanBeNull] Action action)
	{
	}

	public virtual void Examine(bool enabled, [CanBeNull] Action action)
	{
	}

	public virtual void OnThrow(bool enable)
	{
	}

	public virtual void OnReload(bool enable)
	{
		MovementContext.PlayerAnimatorEnableProneReload(enable);
	}

	public virtual void OnInventory(bool enable)
	{
		MovementContext.PlayerAnimatorEnableProneReload(enable);
	}

	public virtual void ChangePose(float poseDelta)
	{
		MovementContext.SetPoseLevel(MovementContext.PoseLevel + poseDelta);
	}

	public virtual void ChangeSpeed(float speedDelta)
	{
		MovementContext.SetCharacterMovementSpeed(MovementContext.ClampedSpeed + speedDelta);
		MovementContext.RaiseChangeSpeedEvent();
	}

	public virtual void SetTilt(float tilt)
	{
		if (!tilt.IsZero() && _sqrShortage.Avarage > 0.0012f)
		{
			Vector3 rhs = ((tilt < 0f) ? (-MovementContext.PlayerRealRight) : MovementContext.PlayerRealRight);
			if (Vector3.Dot(_motionShortage.normalized, rhs) > 0.173f)
			{
				tilt = Mathf.Lerp(MovementContext.Tilt, 0f, Time.deltaTime * 5f);
			}
		}
		MovementContext.SetTilt(tilt);
	}

	public virtual void Kick()
	{
	}

	public virtual void Jump()
	{
	}

	public virtual void Prone()
	{
		if (MovementContext.StationaryWeapon == null)
		{
			MovementContext.IsInPronePose = true;
		}
	}

	public virtual void Plant(bool enabled, bool multitool, float plantTime, Action<bool> action)
	{
	}

	public virtual void EnableSprint(bool enable, bool isToggle = false)
	{
	}

	public virtual void EnableBreath(bool enable)
	{
	}

	public void StartDoorInteraction(WorldInteractiveObject interactive, _EBFE interactionResult, Action callback)
	{
		MovementContext.InteractWith(interactive, interactionResult, callback);
	}

	public virtual void ExecuteDoorInteraction(WorldInteractiveObject interactive, _EBFE interactionResult, [CanBeNull] Action callback, Player user)
	{
		interactive.SetUser(user);
		if (MovementContext.IsInPronePose)
		{
			MovementContext.IsInPronePose = false;
			MovementContext.SetPoseLevel(0f);
		}
		WorldInteractiveObject._E001 interactionParameters = interactive.GetInteractionParameters(MovementContext.TransformPosition);
		interactive.LockForInteraction();
		interactive.Interact(interactionResult);
		if (!interactive.interactWithoutAnimation)
		{
			MovementContext.SetInteractInHands(isInteracting: true, interactionParameters.AnimationId);
		}
	}

	public virtual void SetStep(int step)
	{
	}

	public virtual void Cancel()
	{
	}

	public virtual void OnInteraction()
	{
	}

	public virtual void BlindFire(int b)
	{
		MovementContext.SetBlindFire(0);
	}

	public virtual void DropStationary()
	{
	}

	public void AssignAnimatorPose(AnimatorPose animatorPose)
	{
		MovementContext.AssignAnimatorPose(animatorPose);
	}
}
