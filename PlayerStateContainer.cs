using System;
using System.Runtime.CompilerServices;
using EFT;
using UnityEngine;

[_E506(typeof(_E576))]
public class PlayerStateContainer : StateMachineBehaviour, _E342, IStateBehaviour
{
	public bool IsDefaultState;

	public EPlayerState Name;

	public EStateType Type;

	public EMovementDirection AdditionalDirectionInfo;

	public float RotationSpeedClamp = 99f;

	public float StateSensitivity = 1f;

	public bool CanInteract;

	public bool DisableRootMotion;

	public bool CreateUniqueMovementStateObject;

	public float AnimationAuthority;

	public float AuthoritySpeed = 4f;

	[HideInInspector]
	public string StateFullName;

	[HideInInspector]
	public int StateFullNameHash;

	[HideInInspector]
	public float StateLength;

	public AnimatorPose FirstPersonPose;

	[CompilerGenerated]
	private MovementState _E000;

	public bool DefaultState
	{
		get
		{
			return IsDefaultState;
		}
		set
		{
			IsDefaultState = value;
		}
	}

	public EPlayerState PlayerStateName
	{
		get
		{
			return Name;
		}
		set
		{
			Name = value;
		}
	}

	public MovementState EncapsulatedState
	{
		[CompilerGenerated]
		get
		{
			return _E000;
		}
		[CompilerGenerated]
		set
		{
			_E000 = value;
		}
	}

	public int FullNameHash => StateFullNameHash;

	public virtual void Init(_E70F movementContext, Func<EPlayerState, int, bool, MovementState> newStateFunc)
	{
		if (StateFullNameHash == 0)
		{
			throw new Exception(_ED3E._E000(58530) + Name);
		}
		EncapsulatedState = newStateFunc(Name, StateFullNameHash, CreateUniqueMovementStateObject);
		if (EncapsulatedState != null)
		{
			EncapsulatedState.Init(movementContext);
			EncapsulatedState.RotationSpeedClamp = RotationSpeedClamp;
			EncapsulatedState.StateSensitivity = StateSensitivity;
			EncapsulatedState.CanInteract = CanInteract;
			EncapsulatedState.AnimationAuthority = AnimationAuthority;
			EncapsulatedState.AuthoritySpeed = AuthoritySpeed;
			EncapsulatedState.Name = Name;
			EncapsulatedState.AnimatorStateName = StateFullName;
			EncapsulatedState.AnimatorStateHash = StateFullNameHash;
			EncapsulatedState.AdditionalDirectionInfo = AdditionalDirectionInfo;
			EncapsulatedState.StateLength = StateLength;
			EncapsulatedState.Type = Type;
		}
	}

	public void Deinit()
	{
		EncapsulatedState = null;
	}

	public sealed override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (EncapsulatedState != null)
		{
			EncapsulatedState.AnimatorStateHash = StateFullNameHash;
			EncapsulatedState.AnimatorStateName = StateFullName;
			EncapsulatedState.AdditionalDirectionInfo = AdditionalDirectionInfo;
			EncapsulatedState.RotationSpeedClamp = RotationSpeedClamp;
			EncapsulatedState.StateSensitivity = StateSensitivity;
			EncapsulatedState.CanInteract = CanInteract;
			EncapsulatedState.Type = Type;
			EncapsulatedState.OnStateEnter(stateInfo.normalizedTime);
			EncapsulatedState.AssignAnimatorPose(FirstPersonPose);
			if (DisableRootMotion)
			{
				EncapsulatedState.DisableRootMotion = true;
			}
		}
	}

	public sealed override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		EncapsulatedState?.OnStateMove(stateInfo.normalizedTime);
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (EncapsulatedState != null)
		{
			EncapsulatedState.OnStateExit(stateInfo.normalizedTime);
			if (DisableRootMotion)
			{
				EncapsulatedState.DisableRootMotion = false;
			}
		}
	}
}
