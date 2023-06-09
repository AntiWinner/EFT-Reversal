using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using AnimationEventSystem;
using EFT;
using JetBrains.Annotations;
using RootMotion.FinalIK;
using UnityEngine;

public class MenuPlayerPoser : MonoBehaviour, _E324
{
	private enum EMenuPlayerAnimationType
	{
		Idle,
		RandomAnimation,
		WeaponAnimation,
		GestureAnimation
	}

	private const int MIN_AMMO_COUNT = 2;

	private const int MAX_AMMO_COUNT = 10;

	public LimbIK[] Limbs;

	public PlayerBones PBones;

	public TwistRelax[] Twists;

	public HandPoser LeftHandPoser;

	public HandPoser RightHandPoser;

	private bool _initialized;

	private Transform[] _markers = new Transform[2];

	private readonly Transform[] _gripReferences = new Transform[2];

	private HandPoser[] _handPosers;

	private Quaternion _ikRotation;

	private Vector3 _ikPosition;

	private Transform[] _elbowBends = new Transform[2];

	private IAnimator _weaponAnimator;

	private int _randomAnimationsCount;

	private bool _patrolDelayed;

	private bool _canReload;

	private int _ammoToLoad = -1;

	private int _weaponAnimationType = -1;

	private EGesture _gestureAnimationType;

	private _E570 _animEventEmitter;

	private List<_E324> _eventConsumers = new List<_E324>();

	private _E30E[] _grips = new _E30E[2];

	private _E30F _blender;

	private Player.ValueBlender _crutchBlender = new Player.ValueBlender(1)
	{
		Speed = 2f
	};

	private List<EGesture> _gestures = (from EGesture g in Enum.GetValues(typeof(EGesture))
		where g != EGesture.None
		select g).ToList();

	private List<(int Parameter, Action<IAnimator> AnimateAction)> _weaponAnimations = new List<(int, Action<IAnimator>)>();

	private bool _animate;

	public Animator PlayerAnimatorController { get; private set; }

	private bool _E000
	{
		set
		{
			_E326.SetPatrol(_weaponAnimator, value);
			if (_blender != null)
			{
				_blender.Blender.Target = ((!value) ? 1 : 0);
			}
			_crutchBlender.Target = (value ? 1 : 0);
		}
	}

	private static int _E001 => UnityEngine.Random.Range(2, 11);

	public void Init(Animator playerAnimatorController)
	{
		_eventConsumers.Add(this);
		LimbIK obj = Limbs[0];
		bool flag2 = (Limbs[1].enabled = false);
		obj.enabled = flag2;
		_handPosers = new HandPoser[2] { LeftHandPoser, RightHandPoser };
		LeftHandPoser.enabled = false;
		RightHandPoser.enabled = false;
		PlayerAnimatorController = playerAnimatorController;
		_randomAnimationsCount = PlayerAnimatorController.GetInteger(_E712.UI_RANDOM_ANIMATIONS_COUNT);
		_initialized = true;
	}

	public void ChangeWeapon([CanBeNull] _E570 eventsEmitter, [CanBeNull] IAnimator animator, [CanBeNull] TransformLinks links, bool canReload, bool animate = true)
	{
		if (!_initialized)
		{
			Init(GetComponent<Animator>());
		}
		_E006();
		_canReload = canReload;
		_weaponAnimationType = -1;
		_gestureAnimationType = EGesture.None;
		_animate = animate;
		_weaponAnimations = new List<(int, Action<IAnimator>)>
		{
			(_E326.TRIGGER_CHECKCHAMBER, _E326.TriggerCheckChamber),
			(_E326.TRIGGER_LOOK, _E326.TriggerLook)
		};
		if (_canReload)
		{
			_weaponAnimations.Add((_E326.TRIGGER_CHECKAMMO, _E326.TriggerCheckAmmo));
			_weaponAnimations.Add((_E326.TRIGGER_RELOAD, _E326.TriggerReload));
		}
		if (links != null)
		{
			PBones.UpdateImportantBones(links);
		}
		_animEventEmitter = eventsEmitter;
		if (_animEventEmitter != null)
		{
			_animEventEmitter.OnEventAction += _E004;
		}
		if (animator != null)
		{
			_weaponAnimator = animator;
			_weaponAnimator.enabled = true;
			_E326.SetActive(_weaponAnimator, active: true);
			this._E000 = true;
			_weaponAnimator.Play(_ED3E._E000(62585), 1, 0f);
			_E326.SetCanReload(_weaponAnimator, _canReload);
		}
		if (links != null)
		{
			links.GatherIK(_markers, _gripReferences, _elbowBends);
		}
		_E005(links);
		LateUpdate();
	}

	public void OnWeaponOperationStart()
	{
		if (_initialized)
		{
			if (_weaponAnimationType >= 0)
			{
				_weaponAnimations[_weaponAnimationType].AnimateAction(_weaponAnimator);
				return;
			}
			if (_gestureAnimationType != 0)
			{
				_E001(_gestureAnimationType);
				return;
			}
			_patrolDelayed = false;
			PlayerAnimatorController.SetBool(_E712.UI_WEAPON_OPERATION, value: false);
			OnWeaponOperationEnd();
		}
	}

	public void OnWeaponOperationEnd()
	{
		if (_initialized)
		{
			this._E000 = true;
			_E326.SetCanReload(_weaponAnimator, _canReload);
		}
	}

	public void OnRandomizeAnimation()
	{
		if (!_initialized)
		{
			return;
		}
		switch ((EMenuPlayerAnimationType)((_animate && _weaponAnimator != null) ? UnityEngine.Random.Range(0, 3) : 0))
		{
		case EMenuPlayerAnimationType.WeaponAnimation:
		{
			_weaponAnimationType = UnityEngine.Random.Range(0, _weaponAnimations.Count);
			(int, Action<IAnimator>) tuple = _weaponAnimations[_weaponAnimationType];
			if (!_weaponAnimator.HasParameter(tuple.Item1))
			{
				_weaponAnimationType = -1;
				PlayerAnimatorController.SetInteger(_E712.UI_RANDOM_TRANSITION, 0);
			}
			else
			{
				_E000();
			}
			break;
		}
		case EMenuPlayerAnimationType.GestureAnimation:
			_gestureAnimationType = _gestures[UnityEngine.Random.Range(1, _gestures.Count)];
			_E000();
			break;
		case EMenuPlayerAnimationType.Idle:
			PlayerAnimatorController.SetInteger(_E712.UI_RANDOM_TRANSITION, 0);
			break;
		case EMenuPlayerAnimationType.RandomAnimation:
			PlayerAnimatorController.SetInteger(_E712.UI_RANDOM_TRANSITION, UnityEngine.Random.Range(1, _randomAnimationsCount + 1));
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	private void _E000()
	{
		PlayerAnimatorController.SetBool(_E712.UI_WEAPON_OPERATION, value: true);
		PlayerAnimatorController.SetInteger(_E712.UI_RANDOM_TRANSITION, 0);
		this._E000 = false;
		_patrolDelayed = true;
	}

	private void _E001(EGesture index)
	{
		_E326.TriggerGesture(_weaponAnimator);
		_E326.SetUseLeftHand(_weaponAnimator, useLeftHand: true);
		_E326.SetLActionIndex(_weaponAnimator, 0);
		_E326.SetGestureIndex(_weaponAnimator, (float)(index - 1));
	}

	private void _E002()
	{
		if (_ammoToLoad < 0)
		{
			_ammoToLoad = MenuPlayerPoser._E001;
		}
		_ammoToLoad--;
		if (_ammoToLoad == 0)
		{
			_E326.SetCanReload(_weaponAnimator, canReload: false);
			_ammoToLoad = MenuPlayerPoser._E001;
		}
	}

	private void _E003()
	{
		if (_patrolDelayed)
		{
			_patrolDelayed = false;
		}
		else
		{
			PlayerAnimatorController.SetBool(_E712.UI_WEAPON_OPERATION, value: false);
		}
	}

	private void _E004(int functionNameHash, AnimationEventParameter parameter)
	{
		_E325.AnimatorEventHandler(_eventConsumers, functionNameHash, parameter);
	}

	private void _E005(TransformLinks transforms = null)
	{
		_handPosers[0].GripWeight = 0f;
		if (transforms == null)
		{
			_handPosers[0].weight = (_handPosers[1].weight = 0f);
			return;
		}
		_handPosers[0].MapGrip(transforms.GetTransform(ECharacterWeaponBones.HumanLPalm));
		_handPosers[1].MapGrip(transforms.GetTransform(ECharacterWeaponBones.HumanRPalm));
		_handPosers[0].weight = (_handPosers[1].weight = 1f);
		GripPose[] source = transforms.Self.GetComponentsInChildren<GripPose>().ToArray();
		GripPose[] source2 = (from x in source
			where x.Hand == GripPose.EHand.Left
			orderby x.GripType == GripPose.EGripType.UnderbarrelWeapon descending, HandPoser.NumParents(x.transform, PBones.WeaponRoot.Original) descending
			select x).ToArray();
		GripPose gripPose = source2.FirstOrDefault((GripPose x) => x.GripType != GripPose.EGripType.Common);
		GripPose gripPose2 = source2.FirstOrDefault((GripPose x) => x.GripType == GripPose.EGripType.Common);
		GripPose gripPose3 = (from x in source
			where x.Hand == GripPose.EHand.Right
			orderby x.GripType != GripPose.EGripType.Alternative, HandPoser.NumParents(x.transform, PBones.WeaponRoot.Original) descending
			select x).FirstOrDefault();
		if (gripPose != null && gripPose2 != null)
		{
			if (gripPose.GripType == GripPose.EGripType.UnderbarrelWeapon)
			{
				gripPose2 = gripPose;
			}
			_blender = new _E30F(gripPose, gripPose2, 2f, 0f);
			_grips = new _E30E[2] { _blender, gripPose3 };
		}
		else
		{
			_grips = new _E30E[2]
			{
				gripPose ? gripPose : gripPose2,
				gripPose3
			};
		}
		_handPosers[0].SetGrip(_grips[0]);
		_handPosers[1].SetGrip(_grips[1]);
	}

	private void LateUpdate()
	{
		if (!_initialized)
		{
			return;
		}
		_animEventEmitter?.EmitEvents();
		if (_weaponAnimator == null || _weaponAnimator.runtimeAnimatorController == null)
		{
			return;
		}
		float num = 1f - PlayerAnimatorController.GetFloat(_E712.RIGHT_HAND_WEIGHT);
		float weight = 1f - PlayerAnimatorController.GetFloat(_E712.LEFT_HAND_WEIGHT);
		float @float = PlayerAnimatorController.GetFloat(_E712.WEAPON_ROOT_3RD);
		float num2 = 1f - PlayerAnimatorController.GetFloat(_E712.ELBOW_LEFT_WEIGHT);
		float num3 = 1f - PlayerAnimatorController.GetFloat(_E712.ELBOW_RIGHT_WEIGHT);
		if (num < 1f && _markers[1] != null)
		{
			PBones.Kinematics(_markers[1], 0f);
		}
		else if (@float > 0.01f)
		{
			PBones.ShiftWeaponRoot(Time.deltaTime, EPointOfView.ThirdPerson, @float);
		}
		else
		{
			PBones.WeaponRoot.Original.localPosition = Vector3.zero;
			PBones.WeaponRoot.Original.localRotation = Quaternion.identity;
		}
		Limbs[0].solver.IKRotationWeight = (Limbs[0].solver.IKPositionWeight = (_handPosers[0].weight = weight));
		Limbs[1].solver.IKRotationWeight = (Limbs[1].solver.IKPositionWeight = num);
		if (_elbowBends[0] != null && _elbowBends[1] != null)
		{
			for (int i = 0; i < PBones.BendGoals.Length; i++)
			{
				Transform obj = PBones.BendGoals[i];
				float t = ((i == 0) ? num2 : num3);
				obj.position = Vector3.Lerp(obj.position, _elbowBends[i].position, t);
			}
		}
		for (int j = 0; j < 2; j++)
		{
			if (_markers[j] == null || Math.Abs(Limbs[j].solver.IKPositionWeight) < float.Epsilon)
			{
				continue;
			}
			if (_grips[j] != null)
			{
				float num4 = Vector3.Distance(_markers[j].position, _gripReferences[j].position);
				if (_grips[j].IsAlternative)
				{
					num4 *= 1f - _crutchBlender.Value;
				}
				float num5 = Mathf.InverseLerp(0.1f, 0f, num4);
				_handPosers[j].GripWeight = num5;
				_ikPosition = Vector3.Lerp(_markers[j].position, _grips[j].Position, num5);
				_ikRotation = Quaternion.Lerp(_markers[j].rotation, _grips[j].Rotation, num5);
			}
			else
			{
				_ikPosition = _markers[j].position;
				_ikRotation = _markers[j].rotation;
			}
			Limbs[j].solver.SetIKPosition(_ikPosition);
			Limbs[j].solver.SetIKRotation(_ikRotation);
		}
		LimbIK[] limbs = Limbs;
		for (int k = 0; k < limbs.Length; k++)
		{
			limbs[k].solver.Update();
		}
		_handPosers[0].ManualUpdate();
		_handPosers[1].ManualUpdate();
		_blender?.Update();
		TwistRelax[] twists = Twists;
		for (int k = 0; k < twists.Length; k++)
		{
			twists[k].Relax();
		}
	}

	private void OnDestroy()
	{
		_E006();
	}

	private void _E006()
	{
		if (_animEventEmitter != null)
		{
			_animEventEmitter.OnEventAction -= _E004;
		}
	}

	void _E324.OnAddAmmoInChamber()
	{
		_E002();
	}

	void _E324.OnAddAmmoInMag()
	{
		_E002();
	}

	void _E324.OnArm()
	{
	}

	void _E324.OnCook()
	{
	}

	void _E324.OnDelAmmoChamber()
	{
	}

	void _E324.OnDelAmmoFromMag()
	{
	}

	void _E324.OnDisarm()
	{
	}

	void _E324.OnFireEnd()
	{
	}

	void _E324.OnFiringBullet()
	{
	}

	void _E324.OnFoldOff()
	{
	}

	void _E324.OnFoldOn()
	{
	}

	void _E324.OnIdleStart()
	{
		_E003();
	}

	void _E324.OnMagHide()
	{
	}

	void _E324.OnMagIn()
	{
	}

	void _E324.OnMagOut()
	{
	}

	void _E324.OnMagShow()
	{
	}

	void _E324.OnMessageName()
	{
	}

	void _E324.OnMalfunctionOff()
	{
	}

	void _E324.OnModChanged()
	{
	}

	void _E324.OnOffBoltCatch()
	{
	}

	void _E324.OnOnBoltCatch()
	{
	}

	void _E324.OnPutMagToRig()
	{
	}

	void _E324.OnRemoveShell()
	{
	}

	void _E324.OnReplaceSecondMag()
	{
	}

	void _E324.OnShellEject()
	{
	}

	void _E324.OnShowAmmo(bool BoolParam)
	{
	}

	void _E324.OnSound(string StringParam)
	{
	}

	void _E324.OnSoundAtPoint(string StringParam)
	{
	}

	void _E324.OnStartUtilityOperation()
	{
	}

	void _E324.OnThirdAction(int IntParam)
	{
	}

	void _E324.OnUseSecondMagForReload()
	{
	}

	void _E324.OnWeapIn()
	{
	}

	void _E324.OnWeapOut()
	{
	}

	void _E324.OnLauncherAppeared()
	{
	}

	void _E324.OnLauncherDisappeared()
	{
	}

	void _E324.OnShowMag()
	{
	}

	void _E324.OnSliderOut()
	{
	}

	void _E324.OnUseProp(bool BoolParam)
	{
	}

	void _E324.OnBackpackDrop()
	{
	}

	void _E324.OnComboPlanning()
	{
	}

	void _E324.OnCurrentAnimStateEnded()
	{
	}

	void _E324.OnSetActiveObject(int objectID)
	{
	}

	void _E324.OnDeactivateObject(int objectID)
	{
	}

	[CompilerGenerated]
	private int _E007(GripPose x)
	{
		return HandPoser.NumParents(x.transform, PBones.WeaponRoot.Original);
	}

	[CompilerGenerated]
	private int _E008(GripPose x)
	{
		return HandPoser.NumParents(x.transform, PBones.WeaponRoot.Original);
	}
}
