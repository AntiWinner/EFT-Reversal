using System.Collections.Generic;
using System.Linq;
using CommonAssets.Scripts;
using EFT;
using FastAnimatorSystem;
using UnityEngine;

public class PlayerBones : MonoBehaviour
{
	public struct _E000
	{
		public ETransitionType type;

		public AnimationCurve c;

		public bool executed;
	}

	public enum ETransitionType
	{
		In,
		Out,
		None
	}

	public Player Player;

	public PlayableAnimator PlayableAnimator;

	public Transform Weapon_Root_Third;

	public Transform Weapon_Root_Anim;

	public Transform Neck;

	public Transform Weapon_reference_point;

	public Vector3 R_Neck;

	public Vector3 R_Head;

	public Transform[] Shoulders;

	public Transform[] Shoulders_Anim = new Transform[2];

	public Transform[] Upperarms;

	public Transform[] Forearms;

	public Transform LeftPalm;

	public Transform RightPalm;

	public Transform LootRaycastOrigin;

	public Transform RootJoint;

	public Transform HolsterPrimary;

	public Transform HolsterPrimaryAlternative;

	public Transform HolsterSecondary;

	public Transform HolsterSecondaryAlternative;

	public Transform ScabbardTagillaMelee;

	public Transform HolsterPistol;

	public Transform LeftLegHolsterPistol;

	public Transform[] BendGoals;

	public Transform KickingFoot;

	public Transform[] FovSpecialTransforms;

	public BifacialTransform WeaponRoot;

	public BifacialTransform Ribcage;

	public BifacialTransform Head;

	public BifacialTransform LeftShoulder;

	public BifacialTransform RightShoulder;

	public BifacialTransform LeftThigh2;

	public BifacialTransform RightThigh2;

	public BifacialTransform BodyTransform;

	public BifacialTransform AnimatedTransform;

	public BifacialTransform Pelvis;

	public BifacialTransform LeftThigh1;

	public BifacialTransform RightThigh1;

	public BifacialTransform Spine3;

	public Vector3 Offset = Vector3.zero;

	public Quaternion DeltaRotation = Quaternion.identity;

	public BodyPartCollider LeftHandCollider;

	[HideInInspector]
	public BifacialTransform Fireport = new BifacialTransform();

	public BifacialTransform LeftMultiBarrelFireport = new BifacialTransform();

	public BifacialTransform RightMultiBarrelFireport = new BifacialTransform();

	public BifacialTransform[] MultiBarrelsFireports;

	public readonly Dictionary<PlayerBoneType, BifacialTransform> BifacialTransforms = _E3A5<PlayerBoneType>.GetDictWith<BifacialTransform>();

	public readonly Dictionary<EBodyPartColliderType, BodyPartCollider> BodyPartCollidersDictionary = _E3A5<EBodyPartColliderType>.GetDictWith<BodyPartCollider>();

	public BodyPartCollider[] BodyPartColliders;

	public readonly HashSet<Collider> BodyPartCollidersHashSet = new HashSet<Collider>();

	public float ThirdPersonAuthority;

	private AnimationCurve m__E000;

	private float _E001;

	private float _E002;

	private ETransitionType _E003 = ETransitionType.None;

	public AnimationCurve HeadRotationCurve;

	public Transform Spine1;

	private _E000 _E004;

	public float HeadChain = 0.5f;

	public float NeckChain = 0.5f;

	public void RotateHead(float v, Vector3 offset)
	{
		if (!(v <= 0f) || !(offset.sqrMagnitude < 0.01f))
		{
			float t = HeadRotationCurve.Evaluate(v);
			Quaternion quaternion = Quaternion.Euler(offset * NeckChain);
			Quaternion quaternion2 = Quaternion.Euler(offset * HeadChain);
			Neck.transform.localRotation *= Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(R_Neck), t) * quaternion;
			Head.Original.transform.localRotation *= Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(R_Head), t) * quaternion2;
		}
	}

	public void Awake()
	{
		CreateBifacialTransformsCollection();
		BodyPartCollidersHashSet.Clear();
		BodyPartCollidersDictionary.Clear();
		BodyPartCollider[] bodyPartColliders = BodyPartColliders;
		foreach (BodyPartCollider bodyPartCollider in bodyPartColliders)
		{
			BodyPartCollidersHashSet.Add(bodyPartCollider.Collider);
			BodyPartCollidersDictionary[bodyPartCollider.BodyPartColliderType] = bodyPartCollider;
		}
		_E004.executed = true;
	}

	public void CreateBifacialTransformsCollection()
	{
		BifacialTransforms.Clear();
		BifacialTransforms.Add(PlayerBoneType.WeaponRoot, WeaponRoot);
		BifacialTransforms.Add(PlayerBoneType.Ribcage, Ribcage);
		BifacialTransforms.Add(PlayerBoneType.Head, Head);
		BifacialTransforms.Add(PlayerBoneType.LeftShoulder, LeftShoulder);
		BifacialTransforms.Add(PlayerBoneType.RightShoulder, RightShoulder);
		BifacialTransforms.Add(PlayerBoneType.Body, BodyTransform);
		BifacialTransforms.Add(PlayerBoneType.LeftThigh2, LeftThigh2);
		BifacialTransforms.Add(PlayerBoneType.RightThigh2, RightThigh2);
		BifacialTransforms.Add(PlayerBoneType.Fireport, Fireport);
		BifacialTransforms.Add(PlayerBoneType.Pelvis, Pelvis);
		BifacialTransforms.Add(PlayerBoneType.LeftThigh1, LeftThigh1);
		BifacialTransforms.Add(PlayerBoneType.RightThigh1, RightThigh1);
		BifacialTransforms.Add(PlayerBoneType.Spine, Spine3);
		BifacialTransforms.Add(PlayerBoneType.LeftFirePort, LeftMultiBarrelFireport);
		BifacialTransforms.Add(PlayerBoneType.RightFirePort, RightMultiBarrelFireport);
	}

	public void EnableBoneImitation(bool useImitation)
	{
		foreach (BifacialTransform value in BifacialTransforms.Values)
		{
			value.UseImitation = useImitation;
		}
	}

	public void AdjustPistolHolster(Transform holsterTransform, bool isRightLeg)
	{
		if (isRightLeg && HolsterPistol != null)
		{
			HolsterPistol.localPosition = holsterTransform.localPosition;
			HolsterPistol.localRotation = holsterTransform.localRotation;
		}
		else if (!isRightLeg && LeftLegHolsterPistol != null)
		{
			LeftLegHolsterPistol.localPosition = holsterTransform.localPosition;
			LeftLegHolsterPistol.localRotation = holsterTransform.localRotation;
		}
	}

	public void UpdateImportantBones(TransformLinks tLinks)
	{
		Weapon_Root_Anim = tLinks.GetTransform(ECharacterWeaponBones.Weapon_root_anim);
		Shoulders_Anim[0] = tLinks.GetTransform(ECharacterWeaponBones.HumanLUpperarm);
		Shoulders_Anim[1] = tLinks.GetTransform(ECharacterWeaponBones.HumanRUpperarm);
	}

	public void UpdateLauncherRightHand()
	{
		AlternativePropBone[] array = WeaponRoot.Original.GetComponentsInChildren<AlternativePropBone>().ToArray();
		if (array.Length != 0)
		{
			Weapon_Root_Anim = array[0].transform;
		}
	}

	public void SetShoulders(float str, float strRight)
	{
		Shoulders[0].LerpPositionAndRotation(Shoulders_Anim[0].position, Shoulders_Anim[0].rotation, str);
		Shoulders[1].LerpPositionAndRotation(Shoulders_Anim[1].position, Shoulders_Anim[1].rotation, strRight);
	}

	public void Kinematics(Transform target, float weight)
	{
		Quaternion quaternion = Quaternion.Lerp(RightPalm.rotation, target.rotation, weight);
		Vector3 vector = Vector3.Lerp(RightPalm.position, target.position, weight);
		Weapon_Root_Anim.transform.SetPositionAndRotation(vector, quaternion);
		Quaternion quaternion2 = Quaternion.Inverse(target.rotation) * quaternion;
		Transform obj = Weapon_Root_Anim.transform;
		obj.rotation *= quaternion2;
		obj.position += vector - target.position;
	}

	public void ShiftWeaponRoot(float t, EPointOfView pv, float str, bool armsupdated = false)
	{
		ThirdPersonAuthority = str;
		if (pv != 0)
		{
			if (Weapon_Root_Anim != null && Weapon_Root_Third != null)
			{
				Vector3 position = Vector3.Lerp(Weapon_Root_Anim.position, Weapon_Root_Third.position + Weapon_Root_Third.rotation * Offset, str);
				Quaternion rotation = Quaternion.Lerp(Weapon_Root_Anim.rotation, Weapon_Root_Third.rotation * DeltaRotation, str);
				Weapon_Root_Anim.SetPositionAndRotation(position, rotation);
			}
		}
		else if (Weapon_Root_Anim != null && ThirdPersonAuthority > 0f)
		{
			Weapon_Root_Anim.SetPositionAndRotation(Vector3.Lerp(Weapon_Root_Anim.position, Weapon_Root_Third.position, ThirdPersonAuthority), Quaternion.Lerp(Weapon_Root_Anim.rotation, Weapon_Root_Third.rotation, ThirdPersonAuthority));
		}
	}

	public void ExecuteTransition(ref _E000 trans)
	{
		trans.executed = true;
		ETransitionType type = trans.type;
		AnimationCurve c = trans.c;
		if (this.m__E000 == null || (_E001 == _E002 && type != _E003))
		{
			_E001 = 0f;
		}
		else if (type != _E003)
		{
			_E001 = 1f - _E001 / _E002;
			_E001 = c[c.length - 1].time * _E001;
		}
		_E003 = type;
		_E002 = c[c.length - 1].time;
		this.m__E000 = c;
	}

	public void Transit(AnimationCurve c, ETransitionType type)
	{
		if (!_E004.executed)
		{
			Debug.Log(string.Format(_ED3E._E000(64459), c[0].value, c[1].value, type.ToString()));
		}
		_E004.c = c;
		_E004.type = type;
		_E004.executed = false;
	}

	public void IKTransit(ETransitionType eTransitionType, float speed = 4f)
	{
		Player.ThirdIkWeight.Target = ((eTransitionType == ETransitionType.In) ? 1 : 0);
	}

	public BifacialTransform FindFireport()
	{
		Fireport.Original = _E38B.FindTransformRecursive(WeaponRoot.Original, _ED3E._E000(64493));
		return Fireport;
	}

	public BifacialTransform[] FindMultiBarrelsFireports(bool isMultiBarrel)
	{
		if (!isMultiBarrel)
		{
			return null;
		}
		LeftMultiBarrelFireport.Original = _E38B.FindTransformRecursive(WeaponRoot.Original, _ED3E._E000(64486));
		RightMultiBarrelFireport.Original = _E38B.FindTransformRecursive(WeaponRoot.Original, _ED3E._E000(58393));
		MultiBarrelsFireports = new BifacialTransform[2] { LeftMultiBarrelFireport, RightMultiBarrelFireport };
		return MultiBarrelsFireports;
	}
}
