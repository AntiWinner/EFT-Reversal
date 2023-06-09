using System.Runtime.CompilerServices;
using AnimationSystem.RootMotionTable;
using Comfort.Common;
using EFT.AssetsManager;
using UnityEngine;
using UnityEngine.AI;

namespace EFT;

[DisallowMultipleComponent]
public abstract class PlayerSpiritBase : AssetPoolObject
{
	public Animator BodyAnimator;

	private IAnimator m__E01D;

	[HideInInspector]
	public IAnimator ArmsAnimator;

	[SerializeField]
	protected Transform _bodyTransform;

	[SerializeField]
	protected Transform _animatedTransform;

	[SerializeField]
	protected RootMotionBlendTable RootMotionTable;

	[HideInInspector]
	public Vector2 LookRotation;

	[HideInInspector]
	public Vector2 PreviousLookRotation;

	[SerializeField]
	protected CharacterControllerSpawner _characterControllerSpawner;

	protected _E33A _characterController;

	[SerializeField]
	public PlayerSpiritAura PlayerSpiritAura;

	public NavMeshAgent NavMeshAgent;

	public PlayerSpiritBones Bones;

	protected float _armsLength = 0.35f;

	public const int FOOTPRINTS_HISTORY_LENGTH = 300;

	[HideInInspector]
	public Player _player;

	protected bool _isActive;

	protected bool _isBodyAnimatorUpdating;

	protected bool _updateFastAnimator;

	private CharacterControllerSpawner.Mode m__E01E;

	private CharacterControllerSpawner.Mode m__E01F;

	private bool m__E020;

	private bool m__E021;

	private bool m__E022;

	public virtual IAnimator BodyAnimatorWrapper
	{
		get
		{
			if (this.m__E01D == null)
			{
				this.m__E01D = _E563.CreateAnimator(BodyAnimator);
			}
			return this.m__E01D;
		}
	}

	public _E33A CharacterController => _characterController;

	public TriggerColliderSearcher TriggerColliderSearcher => _characterControllerSpawner.TriggerColliderSearcher;

	public bool IsActive => _isActive;

	public CharacterControllerSpawner.Mode CharacterControllerMode
	{
		get
		{
			if (!this.m__E020)
			{
				return this.m__E01E;
			}
			return this.m__E01F;
		}
	}

	public virtual void Init(Player player, Vector3 position, bool isBodyAnimatorUpdating, CharacterControllerSpawner.Mode characterControllerMode, CharacterControllerSpawner.Mode disconnectedCharacterControllerMode)
	{
		_player = player;
		_isBodyAnimatorUpdating = isBodyAnimatorUpdating;
		this.m__E01E = characterControllerMode;
		this.m__E01F = disconnectedCharacterControllerMode;
		_E001(this.m__E01E);
		Bones.Init(this);
		_characterController.isEnabled = false;
		_bodyTransform.position = position;
		_characterController.isEnabled = true;
		Bones.SetupSimpleBones(_player.PlayerBones.BifacialTransforms);
		_player.PlayerBones.BodyTransform.SetImitators(() => _bodyTransform.position, delegate(Vector3 value)
		{
			_bodyTransform.position = value;
		}, () => _bodyTransform.rotation, delegate(Quaternion value)
		{
			_bodyTransform.rotation = value;
		}, delegate(Vector3 pos, Quaternion rot)
		{
			_bodyTransform.SetPositionAndRotation(pos, rot);
		}, () => _bodyTransform.localPosition, delegate(Vector3 value)
		{
			_bodyTransform.localPosition = value;
		}, () => _bodyTransform.localRotation, delegate(Quaternion value)
		{
			_bodyTransform.localRotation = value;
		}, () => _bodyTransform.forward, () => _bodyTransform.up, () => _bodyTransform.right, () => _bodyTransform.eulerAngles, (Vector3 value) => _bodyTransform.TransformPoint(value), (Vector3 value) => _bodyTransform.InverseTransformPoint(value), (Vector3 value) => _bodyTransform.TransformVector(value), (Vector3 value) => _bodyTransform.InverseTransformVector(value), delegate(Vector3 v1, Vector3 v2)
		{
			_bodyTransform.LookAt(v1, v2);
		});
		_player.PlayerBones.AnimatedTransform.SetImitators(() => _animatedTransform.position, delegate(Vector3 value)
		{
			_animatedTransform.position = value;
		}, () => _animatedTransform.rotation, delegate(Quaternion value)
		{
			_animatedTransform.rotation = value;
		}, delegate(Vector3 pos, Quaternion rot)
		{
			_animatedTransform.SetPositionAndRotation(pos, rot);
		}, () => _animatedTransform.localPosition, delegate(Vector3 value)
		{
			_animatedTransform.localPosition = value;
		}, () => _animatedTransform.localRotation, delegate(Quaternion value)
		{
			_animatedTransform.localRotation = value;
		}, () => _animatedTransform.forward, () => _animatedTransform.up, () => _animatedTransform.right, () => _animatedTransform.eulerAngles, (Vector3 value) => _animatedTransform.TransformPoint(value), (Vector3 value) => _animatedTransform.InverseTransformPoint(value), (Vector3 value) => _animatedTransform.TransformVector(value), (Vector3 value) => _animatedTransform.InverseTransformVector(value), delegate(Vector3 v1, Vector3 v2)
		{
			_animatedTransform.LookAt(v1, v2);
		});
		_player.PlayerBones.WeaponRoot.SetImitators(delegate
		{
			Vector3 position2 = _player.PlayerBones.LeftShoulder.position;
			Vector3 position3 = _player.PlayerBones.RightShoulder.position;
			return (position2 + position3) / 2f;
		}, delegate
		{
		}, () => Quaternion.FromToRotation(Vector3.forward, _player.LookDirection), delegate
		{
		}, delegate
		{
		}, () => NotImplementedPosition(), delegate
		{
			NotImplementedPosition();
		}, () => NotImplementedRotation(), delegate
		{
			NotImplementedRotation();
		}, () => Vector3.Cross(Vector3.right, _player.LookDirection).normalized, () => -_player.LookDirection, () => Vector3.Cross(Vector3.up, _player.LookDirection).normalized, NotImplementedPosition, (Vector3 value) => NotImplementedPosition(), (Vector3 value) => NotImplementedPosition(), (Vector3 value) => NotImplementedPosition(), (Vector3 value) => NotImplementedPosition(), delegate
		{
			NotImplementedRotation();
		});
		_player.PlayerBones.Fireport.CopyImitators(_player.PlayerBones.WeaponRoot);
		_player.PlayerBones.LeftMultiBarrelFireport.CopyImitators(_player.PlayerBones.WeaponRoot);
		_player.PlayerBones.RightMultiBarrelFireport.CopyImitators(_player.PlayerBones.WeaponRoot);
		EnableBoneImitation(IsActive);
	}

	public virtual void InitAfterPlayerInit()
	{
		_player.MovementContext.OnTiltChanged += OnTiltChanged;
		_player.MovementContext.StationaryTiltChanged += OnTiltChanged;
		_player.MovementContext.OnStepChanged += _E000;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (_characterController != null)
		{
			_characterController.OnHeightChanged -= OnCharacterControllerHeightChanged;
		}
		if (_player != null)
		{
			_player.MovementContext.OnTiltChanged -= OnTiltChanged;
			_player.MovementContext.OnStepChanged -= _E000;
			_player.MovementContext.StationaryTiltChanged -= OnTiltChanged;
		}
	}

	protected void EnableBoneImitation(bool useImitation)
	{
		_player.PlayerBones.EnableBoneImitation(useImitation);
	}

	protected Vector3 NotImplementedPosition()
	{
		_E36D.LogError(_ED3E._E000(137851));
		return Vector3.zero;
	}

	protected Quaternion NotImplementedRotation()
	{
		_E36D.LogError(_ED3E._E000(137851));
		return Quaternion.identity;
	}

	public void InitBodyAnimator(AnimatorUpdateMode updateMode, bool useFastAnimator)
	{
		if (useFastAnimator && _isBodyAnimatorUpdating)
		{
			RootMotionTable = Singleton<_ED0A>.Instance.GetAsset<RootMotionBlendTable>(_E5D2.PLAYER_ROOTMOTION_TABLE);
			RootMotionTable.LoadNodes();
			_E4EB fastAnimatorController = _E508.Deserialize(Singleton<_ED0A>.Instance.GetAsset<TextAsset>(_E5D2.PLAYER_FAST_ANIMATOR_CONTROLLER).bytes);
			this.m__E01D = _E563.CreateAnimator(fastAnimatorController, RootMotionTable._loadedNodes, _bodyTransform);
			this.m__E01D.updateMode = updateMode;
			_updateFastAnimator = true;
		}
		else if (BodyAnimator.runtimeAnimatorController == null)
		{
			BodyAnimator.runtimeAnimatorController = Singleton<_ED0A>.Instance.GetAsset<RuntimeAnimatorController>(_E5D2.PLAYER_DEFAULT_ANIMATOR_CONTROLLER_SPIRIT);
			BodyAnimator.updateMode = updateMode;
		}
	}

	public void InitArmsAnimator(IAnimator animator, RuntimeAnimatorController animatorController, IAnimator animatorRoleModel, bool isAnimatorEnabled)
	{
		ArmsAnimator = animator;
		ArmsAnimator.runtimeAnimatorController = animatorController;
		ArmsAnimator.avatar = animatorRoleModel.avatar;
		ArmsAnimator.updateMode = animatorRoleModel.updateMode;
		ArmsAnimator.enabled = isAnimatorEnabled;
	}

	protected _E71E CreateFootprint()
	{
		return _E71E.Create(BodyAnimatorWrapper, ArmsAnimator, _characterController, _bodyTransform.localPosition, _bodyTransform.localRotation, _animatedTransform.localPosition, _animatedTransform.localRotation, LookRotation, PreviousLookRotation);
	}

	public abstract void PlayerSync();

	public void PlayerSync(_E71E footprint, bool updateAnimator = true)
	{
		if (!this.m__E022)
		{
			this.m__E022 = true;
			PlayerBodySync(footprint, updateAnimator);
			PlayerArmsSync(footprint, updateAnimator);
			_E320.SyncTransforms();
		}
	}

	protected void PlayerBodySync(_E71E footprint, bool updateAnimator = true)
	{
		if (updateAnimator)
		{
			TranslateAnimatorState(_player._animators[0], BodyAnimatorWrapper);
			SyncAnimators(_player._animators[0], footprint.bodyAnimator);
		}
		_player._characterController.CopyFields(footprint.characterController);
		_player.PlayerBones.BodyTransform.Original.localPosition = footprint.position;
		_player.PlayerBones.BodyTransform.Original.localRotation = footprint.rotation;
		_player.PlayerBones.AnimatedTransform.Original.localPosition = footprint.animatedPosition;
		_player.PlayerBones.AnimatedTransform.Original.localRotation = footprint.animatedRotation;
	}

	protected void PlayerArmsSync(_E71E footprint, bool updateAnimator = true)
	{
		if (updateAnimator)
		{
			TranslateAnimatorState(_player._animators[1], ArmsAnimator);
			SyncAnimators(_player._animators[1], footprint.armsAnimator);
		}
		_player.MovementContext.SetDirectlyLookRotations(footprint.lookRotation, footprint.previousLookRotation);
		_player.MouseLook(forceApplyToOriginalRibcage: true);
	}

	protected void SyncAnimators(IAnimator animatorRecipient, _E71F footprint)
	{
		if (animatorRecipient == null)
		{
			Debug.LogError(_ED3E._E000(137885));
			return;
		}
		if (animatorRecipient.layerCount > footprint.layersFootprint.Length)
		{
			Debug.LogErrorFormat(_ED3E._E000(137863), animatorRecipient.name);
			return;
		}
		for (int i = 0; i < animatorRecipient.layerCount; i++)
		{
			_E720 obj = footprint.layersFootprint[i];
			animatorRecipient.Play(obj.shortNameHash, i, obj.normalizedTime);
		}
		AnimatorUpdateMode updateMode = animatorRecipient.updateMode;
		animatorRecipient.updateMode = AnimatorUpdateMode.Normal;
		animatorRecipient.Update(0f);
		Physics.SyncTransforms();
		animatorRecipient.updateMode = updateMode;
	}

	protected void OnCharacterControllerHeightChanged(float value)
	{
		ChangePose();
	}

	public void OnTiltChanged(float value)
	{
		ChangePose();
	}

	private void _E000(int before, int after)
	{
		ChangePose();
	}

	protected void ChangePose()
	{
		float height = _characterController.height;
		PlayerSpiritBones.PoseType poseType = PlayerSpiritBones.PoseType.Stand;
		if (height <= 0.3f)
		{
			poseType = PlayerSpiritBones.PoseType.Prone;
		}
		else if (1.2f <= height && height < 1.4000001f)
		{
			poseType = PlayerSpiritBones.PoseType.Crouch;
		}
		else if (height >= 1.4000001f)
		{
			poseType = PlayerSpiritBones.PoseType.Stand;
		}
		if (_player.MovementContext != null)
		{
			float num = _player.MovementContext.Step;
			float tilt = _player.MovementContext.Tilt;
			if (!Mathf.Approximately(num, 0f))
			{
				switch (poseType)
				{
				case PlayerSpiritBones.PoseType.Stand:
					if (num < 0f)
					{
						poseType = PlayerSpiritBones.PoseType.SideStepLeft;
					}
					else if (num > 0f)
					{
						poseType = PlayerSpiritBones.PoseType.SideStepRight;
					}
					break;
				case PlayerSpiritBones.PoseType.Crouch:
					if (num < 0f)
					{
						poseType = PlayerSpiritBones.PoseType.CrouchSideStepLeft;
					}
					else if (num > 0f)
					{
						poseType = PlayerSpiritBones.PoseType.CrouchSideStepRight;
					}
					break;
				case PlayerSpiritBones.PoseType.Prone:
					if (num < 0f)
					{
						poseType = PlayerSpiritBones.PoseType.ProneSideStepLeft;
					}
					else if (num > 0f)
					{
						poseType = PlayerSpiritBones.PoseType.ProneSideStepRight;
					}
					break;
				}
			}
			else
			{
				switch (poseType)
				{
				case PlayerSpiritBones.PoseType.Stand:
					if (tilt <= -5f)
					{
						poseType = PlayerSpiritBones.PoseType.TiltLeft;
					}
					else if (tilt >= 5f)
					{
						poseType = PlayerSpiritBones.PoseType.TiltRight;
					}
					break;
				case PlayerSpiritBones.PoseType.Crouch:
					if (tilt <= -5f)
					{
						poseType = PlayerSpiritBones.PoseType.CrouchTiltLeft;
					}
					else if (tilt >= 5f)
					{
						poseType = PlayerSpiritBones.PoseType.CrouchTiltRight;
					}
					break;
				case PlayerSpiritBones.PoseType.Prone:
					if (tilt <= -5f)
					{
						poseType = PlayerSpiritBones.PoseType.ProneTiltLeft;
					}
					else if (tilt >= 5f)
					{
						poseType = PlayerSpiritBones.PoseType.ProneTiltRight;
					}
					break;
				}
			}
		}
		Bones.SetPose(poseType);
		this.m__E021 = true;
	}

	protected void Update()
	{
		if (this.m__E021)
		{
			this.m__E021 = false;
			PlayerSpiritAura.UpdateBounds(Bones.GetLocalBounds());
		}
		Bones.SetAimingState(_player.HandsController.IsAiming);
	}

	protected void FixedUpdate()
	{
		this.m__E022 = false;
	}

	public void Die()
	{
		if (IsActive)
		{
			PlayerSync();
			SetActive(active: false);
		}
		_bodyTransform.gameObject.SetActive(value: false);
		base.enabled = false;
	}

	public void SetActive(bool active)
	{
		_isActive = active;
		EnableBoneImitation(IsActive);
		if (IsActive)
		{
			_player.EnabledAnimators = Player.EAnimatorMask.Thirdperson | Player.EAnimatorMask.Arms;
		}
		else
		{
			_player.EnabledAnimators = Player.EAnimatorMask.Thirdperson | Player.EAnimatorMask.Arms | Player.EAnimatorMask.Procedural | Player.EAnimatorMask.FBBIK | Player.EAnimatorMask.IK;
		}
	}

	protected virtual void TranslateAnimatorState(IAnimator animatorRecipient, IAnimator animatorDonor)
	{
		for (int i = 0; i < animatorRecipient.parameterCount; i++)
		{
			AnimatorParameterInfo parameter = animatorRecipient.GetParameter(i);
			if (!animatorRecipient.IsParameterControlledByCurve(parameter.nameHash))
			{
				switch (parameter.type)
				{
				case AnimatorControllerParameterType.Float:
					animatorRecipient.SetFloat(parameter.nameHash, animatorDonor.GetFloat(parameter.nameHash));
					break;
				case AnimatorControllerParameterType.Int:
					animatorRecipient.SetInteger(parameter.nameHash, animatorDonor.GetInteger(parameter.nameHash));
					break;
				case AnimatorControllerParameterType.Bool:
					animatorRecipient.SetBool(parameter.nameHash, animatorDonor.GetBool(parameter.nameHash));
					break;
				}
			}
		}
		for (int j = 0; j < animatorRecipient.layerCount; j++)
		{
			AnimatorStateInfoWrapper animatorStateInfoWrapper = (animatorDonor.IsInTransition(j) ? animatorDonor.GetNextAnimatorStateInfo(j) : animatorDonor.GetCurrentAnimatorStateInfo(j));
			animatorRecipient.Play(animatorStateInfoWrapper.shortNameHash, j, animatorStateInfoWrapper.normalizedTime);
		}
	}

	public Transform GetActiveTransform()
	{
		if (IsActive)
		{
			return _bodyTransform;
		}
		return _player.PlayerBones.BodyTransform.Original;
	}

	public void SwitchToDisconnectedMode()
	{
		if (!this.m__E020)
		{
			this.m__E020 = true;
			if (this.m__E01F != null)
			{
				_E001(this.m__E01F);
			}
		}
	}

	public void SwitchToNormalMode()
	{
		if (this.m__E020)
		{
			this.m__E020 = false;
			if (this.m__E01F != null)
			{
				_E001(this.m__E01E);
			}
		}
	}

	private void _E001(CharacterControllerSpawner.Mode characterControllerMode)
	{
		_E33A characterController = _characterController;
		_characterController = _characterControllerSpawner.Spawn(characterControllerMode, _player, (_characterControllerSpawner.CapsuleCollider != null) ? _characterControllerSpawner.CapsuleCollider.gameObject : _characterControllerSpawner.gameObject, isSpirit: true, isThirdPerson: false);
		if (_characterControllerSpawner.TriggerColliderSearcher != null)
		{
			_characterControllerSpawner.TriggerColliderSearcher.ConnectToCharacterController(_characterController);
			_characterControllerSpawner.TriggerColliderSearcher.IsEnabled = true;
		}
		_characterController.OnHeightChanged -= OnCharacterControllerHeightChanged;
		_characterController.OnHeightChanged += OnCharacterControllerHeightChanged;
		if (characterController != null)
		{
			_characterController.CopyFields(characterController);
		}
	}

	[CompilerGenerated]
	private Vector3 _E002()
	{
		return _bodyTransform.position;
	}

	[CompilerGenerated]
	private void _E003(Vector3 value)
	{
		_bodyTransform.position = value;
	}

	[CompilerGenerated]
	private Quaternion _E004()
	{
		return _bodyTransform.rotation;
	}

	[CompilerGenerated]
	private void _E005(Quaternion value)
	{
		_bodyTransform.rotation = value;
	}

	[CompilerGenerated]
	private void _E006(Vector3 pos, Quaternion rot)
	{
		_bodyTransform.SetPositionAndRotation(pos, rot);
	}

	[CompilerGenerated]
	private Vector3 _E007()
	{
		return _bodyTransform.localPosition;
	}

	[CompilerGenerated]
	private void _E008(Vector3 value)
	{
		_bodyTransform.localPosition = value;
	}

	[CompilerGenerated]
	private Quaternion _E009()
	{
		return _bodyTransform.localRotation;
	}

	[CompilerGenerated]
	private void _E00A(Quaternion value)
	{
		_bodyTransform.localRotation = value;
	}

	[CompilerGenerated]
	private Vector3 _E00B()
	{
		return _bodyTransform.forward;
	}

	[CompilerGenerated]
	private Vector3 _E00C()
	{
		return _bodyTransform.up;
	}

	[CompilerGenerated]
	private Vector3 _E00D()
	{
		return _bodyTransform.right;
	}

	[CompilerGenerated]
	private Vector3 _E00E()
	{
		return _bodyTransform.eulerAngles;
	}

	[CompilerGenerated]
	private Vector3 _E00F(Vector3 value)
	{
		return _bodyTransform.TransformPoint(value);
	}

	[CompilerGenerated]
	private Vector3 _E010(Vector3 value)
	{
		return _bodyTransform.InverseTransformPoint(value);
	}

	[CompilerGenerated]
	private Vector3 _E011(Vector3 value)
	{
		return _bodyTransform.TransformVector(value);
	}

	[CompilerGenerated]
	private Vector3 _E012(Vector3 value)
	{
		return _bodyTransform.InverseTransformVector(value);
	}

	[CompilerGenerated]
	private void _E013(Vector3 v1, Vector3 v2)
	{
		_bodyTransform.LookAt(v1, v2);
	}

	[CompilerGenerated]
	private Vector3 _E014()
	{
		return _animatedTransform.position;
	}

	[CompilerGenerated]
	private void _E015(Vector3 value)
	{
		_animatedTransform.position = value;
	}

	[CompilerGenerated]
	private Quaternion _E016()
	{
		return _animatedTransform.rotation;
	}

	[CompilerGenerated]
	private void _E017(Quaternion value)
	{
		_animatedTransform.rotation = value;
	}

	[CompilerGenerated]
	private void _E018(Vector3 pos, Quaternion rot)
	{
		_animatedTransform.SetPositionAndRotation(pos, rot);
	}

	[CompilerGenerated]
	private Vector3 _E019()
	{
		return _animatedTransform.localPosition;
	}

	[CompilerGenerated]
	private void _E01A(Vector3 value)
	{
		_animatedTransform.localPosition = value;
	}

	[CompilerGenerated]
	private Quaternion _E01B()
	{
		return _animatedTransform.localRotation;
	}

	[CompilerGenerated]
	private void _E01C(Quaternion value)
	{
		_animatedTransform.localRotation = value;
	}

	[CompilerGenerated]
	private Vector3 _E01D()
	{
		return _animatedTransform.forward;
	}

	[CompilerGenerated]
	private Vector3 _E01E()
	{
		return _animatedTransform.up;
	}

	[CompilerGenerated]
	private Vector3 _E01F()
	{
		return _animatedTransform.right;
	}

	[CompilerGenerated]
	private Vector3 _E020()
	{
		return _animatedTransform.eulerAngles;
	}

	[CompilerGenerated]
	private Vector3 _E021(Vector3 value)
	{
		return _animatedTransform.TransformPoint(value);
	}

	[CompilerGenerated]
	private Vector3 _E022(Vector3 value)
	{
		return _animatedTransform.InverseTransformPoint(value);
	}

	[CompilerGenerated]
	private Vector3 _E023(Vector3 value)
	{
		return _animatedTransform.TransformVector(value);
	}

	[CompilerGenerated]
	private Vector3 _E024(Vector3 value)
	{
		return _animatedTransform.InverseTransformVector(value);
	}

	[CompilerGenerated]
	private void _E025(Vector3 v1, Vector3 v2)
	{
		_animatedTransform.LookAt(v1, v2);
	}

	[CompilerGenerated]
	private Vector3 _E026()
	{
		Vector3 position = _player.PlayerBones.LeftShoulder.position;
		Vector3 position2 = _player.PlayerBones.RightShoulder.position;
		return (position + position2) / 2f;
	}

	[CompilerGenerated]
	private Quaternion _E027()
	{
		return Quaternion.FromToRotation(Vector3.forward, _player.LookDirection);
	}

	[CompilerGenerated]
	private Vector3 _E028()
	{
		return NotImplementedPosition();
	}

	[CompilerGenerated]
	private void _E029(Vector3 value)
	{
		NotImplementedPosition();
	}

	[CompilerGenerated]
	private Quaternion _E02A()
	{
		return NotImplementedRotation();
	}

	[CompilerGenerated]
	private void _E02B(Quaternion value)
	{
		NotImplementedRotation();
	}

	[CompilerGenerated]
	private Vector3 _E02C()
	{
		return Vector3.Cross(Vector3.right, _player.LookDirection).normalized;
	}

	[CompilerGenerated]
	private Vector3 _E02D()
	{
		return -_player.LookDirection;
	}

	[CompilerGenerated]
	private Vector3 _E02E()
	{
		return Vector3.Cross(Vector3.up, _player.LookDirection).normalized;
	}

	[CompilerGenerated]
	private Vector3 _E02F(Vector3 value)
	{
		return NotImplementedPosition();
	}

	[CompilerGenerated]
	private Vector3 _E030(Vector3 value)
	{
		return NotImplementedPosition();
	}

	[CompilerGenerated]
	private Vector3 _E031(Vector3 value)
	{
		return NotImplementedPosition();
	}

	[CompilerGenerated]
	private Vector3 _E032(Vector3 value)
	{
		return NotImplementedPosition();
	}

	[CompilerGenerated]
	private void _E033(Vector3 v1, Vector3 v2)
	{
		NotImplementedRotation();
	}
}
