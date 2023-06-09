using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using JetBrains.Annotations;
using RootMotion.FinalIK;
using UnityEngine;

namespace EFT.AssetsManager;

[DisallowMultipleComponent]
public class PlayerPoolObject : AssetPoolObject
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public EBodyPartColliderType parent;

		public EBodyPartColliderType child;

		internal bool _E000(PlayerRigidbodySleepHierarchy hierarchy)
		{
			return hierarchy.BodyPart == parent;
		}

		internal bool _E001(PlayerRigidbodySleepHierarchy hierarchy)
		{
			return hierarchy.BodyPart == child;
		}
	}

	public GrounderFBBIK GrounderFbbik;

	public FullBodyBipedIK FullBodyBipedIk;

	public HitReaction HitReaction;

	public LimbIK[] LimbIks;

	public Animator Animator;

	public CharacterControllerSpawner CharacterControllerSpawner;

	public PlayerBones PlayerBones;

	public PlayerOverlapManager PlayerOverlapManager;

	public AnimatorDefaultStateCache AnimatorDefaultStateCache;

	public RigidbodySpawner[] RigidbodySpawners;

	public CharacterJointSpawner[] JointSpawners;

	public List<PlayerRigidbodySleepHierarchy> PlayerRigidbodySleepHierarchy;

	public override void OnCreatePoolRoleModel()
	{
		base.OnCreatePoolRoleModel();
		Animator = GetComponentInChildren<Animator>();
		SetupAnimator();
		JointSpawners = base.gameObject.GetComponentsInChildren<CharacterJointSpawner>();
		RigidbodySpawners = base.gameObject.GetComponentsInChildren<RigidbodySpawner>();
	}

	public static List<PlayerRigidbodySleepHierarchy> CreatePlayerRigidbodySleepHierarchy(RigidbodySpawner[] rigidbodySpawners)
	{
		List<PlayerRigidbodySleepHierarchy> list = new List<PlayerRigidbodySleepHierarchy>();
		foreach (RigidbodySpawner rigidbodySpawner in rigidbodySpawners)
		{
			BodyPartCollider component = rigidbodySpawner.GetComponent<BodyPartCollider>();
			PlayerRigidbodySleepHierarchy item = new PlayerRigidbodySleepHierarchy
			{
				BodyPart = component.BodyPartColliderType,
				RigidbodySpawner = rigidbodySpawner
			};
			list.Add(item);
		}
		_E000(list, EBodyPartColliderType.Ribcage, EBodyPartColliderType.Head);
		_E000(list, EBodyPartColliderType.Ribcage, EBodyPartColliderType.LeftUpperArm);
		_E000(list, EBodyPartColliderType.Ribcage, EBodyPartColliderType.RightUpperArm);
		_E000(list, EBodyPartColliderType.Pelvis, EBodyPartColliderType.LeftThigh);
		_E000(list, EBodyPartColliderType.Pelvis, EBodyPartColliderType.RightThigh);
		_E000(list, EBodyPartColliderType.LeftUpperArm, EBodyPartColliderType.LeftForearm);
		_E000(list, EBodyPartColliderType.RightUpperArm, EBodyPartColliderType.RightForearm);
		_E000(list, EBodyPartColliderType.LeftThigh, EBodyPartColliderType.LeftCalf);
		_E000(list, EBodyPartColliderType.RightThigh, EBodyPartColliderType.RightCalf);
		return list;
	}

	private static void _E000(List<PlayerRigidbodySleepHierarchy> playerRigidbodySleepHierarchy, EBodyPartColliderType parent, EBodyPartColliderType child)
	{
		PlayerRigidbodySleepHierarchy parent2 = playerRigidbodySleepHierarchy.First((PlayerRigidbodySleepHierarchy hierarchy) => hierarchy.BodyPart == parent);
		playerRigidbodySleepHierarchy.First((PlayerRigidbodySleepHierarchy hierarchy) => hierarchy.BodyPart == child).Parent = parent2;
	}

	public override void OnCreatePoolObject<TAssetPoolObject>([CanBeNull] _EC3A<TAssetPoolObject> assetsPoolParent)
	{
		base.OnCreatePoolObject(assetsPoolParent);
		_E001();
		FullBodyBipedIk.enabled = false;
		HitReaction.enabled = false;
		LimbIK[] limbIks = LimbIks;
		for (int i = 0; i < limbIks.Length; i++)
		{
			limbIks[i].enabled = false;
		}
		foreach (Collider collider in Colliders)
		{
			collider.enabled = false;
		}
		if (CharacterControllerSpawner.CapsuleCollider != null)
		{
			CharacterControllerSpawner.CapsuleCollider.enabled = false;
		}
		PlayerRigidbodySleepHierarchy = CreatePlayerRigidbodySleepHierarchy(RigidbodySpawners);
	}

	private void _E001()
	{
		CharacterControllerSpawner = GetComponent<CharacterControllerSpawner>();
		PlayerOverlapManager = GetComponentInChildren<PlayerOverlapManager>();
		PlayerBones = GetComponentInChildren<PlayerBones>();
		GrounderFbbik = GetComponentInChildren<GrounderFBBIK>();
		FullBodyBipedIk = GetComponentInChildren<FullBodyBipedIK>();
		HitReaction = GetComponentInChildren<HitReaction>();
		LimbIks = GetComponentsInChildren<LimbIK>();
		AnimatorDefaultStateCache = GetComponentInChildren<AnimatorDefaultStateCache>();
		_E002();
	}

	protected virtual void SetupAnimator()
	{
		ResourceKey pLAYER_DEFAULT_ANIMATOR_CONTROLLER = _E5D2.PLAYER_DEFAULT_ANIMATOR_CONTROLLER;
		if (!_E2B6.Config.UseBodyFastAnimator)
		{
			Animator.runtimeAnimatorController = Singleton<_ED0A>.Instance.GetAsset<RuntimeAnimatorController>(pLAYER_DEFAULT_ANIMATOR_CONTROLLER);
		}
		Animator.keepAnimatorControllerStateOnDisable = true;
		Animator.enabled = false;
	}

	public override void OnGetFromPool()
	{
		base.OnGetFromPool();
		Animator.enabled = true;
	}

	protected override void ReturnToPool()
	{
		base.ReturnToPool();
		FullBodyBipedIk.enabled = false;
		HitReaction.enabled = false;
		LimbIK[] limbIks = LimbIks;
		for (int i = 0; i < limbIks.Length; i++)
		{
			limbIks[i].enabled = false;
		}
		foreach (Collider collider in Colliders)
		{
			collider.enabled = false;
		}
		if (CharacterControllerSpawner.CapsuleCollider != null)
		{
			CharacterControllerSpawner.CapsuleCollider.enabled = false;
		}
		if (CharacterControllerSpawner.TriggerColliderSearcher != null)
		{
			CharacterControllerSpawner.TriggerColliderSearcher.IsEnabled = false;
		}
		if (AnimatorDefaultStateCache != null)
		{
			AnimatorDefaultStateCache.SetupDefaultParameter();
		}
		foreach (PlayerRigidbodySleepHierarchy item in PlayerRigidbodySleepHierarchy)
		{
			item.Reset();
		}
	}

	private void _E002()
	{
		CharacterJointSpawner[] jointSpawners = JointSpawners;
		for (int i = 0; i < jointSpawners.Length; i++)
		{
			jointSpawners[i].Create();
		}
		RigidbodySpawner[] rigidbodySpawners = RigidbodySpawners;
		for (int i = 0; i < rigidbodySpawners.Length; i++)
		{
			rigidbodySpawners[i].Create().isKinematic = true;
		}
	}
}
