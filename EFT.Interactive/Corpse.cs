using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.AssetsManager;
using EFT.InventoryLogic;
using EFT.MovingPlatforms;
using RootMotion.FinalIK;
using UnityEngine;

namespace EFT.Interactive;

public class Corpse : LootItem
{
	public const float MAX_TIME = 15f;

	private RigidbodySpawner[] _E01F;

	private CharacterJointSpawner[] _E020;

	private List<PlayerRigidbodySleepHierarchy> _E021;

	private Action _E022;

	[CompilerGenerated]
	private _E72D _E023;

	[CompilerGenerated]
	private EPlayerSide _E024;

	private _E53B[] _E025;

	protected Transform _pelvis;

	private Vector3 _E026;

	[CompilerGenerated]
	private bool _E027;

	protected PlayerBody PlayerBody;

	public _ECF5<Item> ItemInHands;

	public _EBFC Ragdoll;

	public _E72D Customization
	{
		[CompilerGenerated]
		get
		{
			return _E023;
		}
		[CompilerGenerated]
		private set
		{
			_E023 = value;
		}
	}

	public EPlayerSide Side
	{
		[CompilerGenerated]
		get
		{
			return _E024;
		}
		[CompilerGenerated]
		private set
		{
			_E024 = value;
		}
	}

	public _E53B[] TransformSyncs => _E025 ?? (_E025 = _E003());

	public _E53B[] TransformSyncsRelativeToPlatform
	{
		get
		{
			_E53B[] array = new _E53B[_E01F.Length];
			Transform transform = base.Platform.transform;
			for (int i = 0; i < _E01F.Length; i++)
			{
				RigidbodySpawner rigidbodySpawner = _E01F[i];
				array[i] = new _E53B
				{
					Position = transform.InverseTransformPoint(rigidbodySpawner.transform.position),
					Rotation = rigidbodySpawner.transform.localRotation * Quaternion.Inverse(transform.rotation)
				};
			}
			return array;
		}
	}

	public bool HasRagdoll
	{
		[CompilerGenerated]
		get
		{
			return _E027;
		}
		[CompilerGenerated]
		private set
		{
			_E027 = value;
		}
	}

	protected virtual CollisionDetectionMode CollisionDetectionMode => CollisionDetectionMode.Discrete;

	public override Transform TrackableTransform => _pelvis;

	public static T CreateCorpse<T>(GameObject gameObject, _EB0B equipment, _E72D customization, bool reinitBody, GameWorld gameWorld, EPlayerSide side, Vector3 velocity, Transform pelvis, _ECF5<Item> itemInHands, _E333 containerCollectionView, MongoID firstID = default(MongoID)) where T : Corpse
	{
		T val = gameObject.AddComponent<T>();
		val._E001(equipment, customization, reinitBody, gameWorld, side, velocity, pelvis, ragdollEnabled: true, itemInHands, containerCollectionView, firstID);
		return val;
	}

	public static T CreateStillCorpse<T>(GameWorld gameWorld, _E546 corpseJson, MovingPlatform platform) where T : Corpse
	{
		_ECC9.ReleaseBeginSample(_ED3E._E000(212494), _ED3E._E000(212538));
		_EB0B equipment = (_EB0B)corpseJson.Item;
		_ECF5<Item> itemInHands = new _ECF5<Item>(null);
		_E72D customization = corpseJson.Customization;
		EPlayerSide side = corpseJson.Side;
		_E53B[] bones = corpseJson.Bones;
		Vector3 position = corpseJson.Position;
		Quaternion quaternion = Quaternion.Euler(corpseJson.Rotation);
		GameObject gameObject = Singleton<_E760>.Instance.CreatePlayerObject(_E5D2.PLAYER_BUNDLE_NAME);
		gameObject.name = _ED3E._E000(212516) + gameObject.name;
		gameObject.transform.position = position;
		gameObject.transform.rotation = quaternion;
		PlayerBones componentInChildren = gameObject.GetComponentInChildren<PlayerBones>();
		IK[] componentsInChildren = gameObject.GetComponentsInChildren<IK>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = false;
		}
		gameObject.SetActive(value: true);
		Animator[] componentsInChildren2 = gameObject.GetComponentsInChildren<Animator>();
		foreach (Animator obj in componentsInChildren2)
		{
			obj.Update(0f);
			obj.enabled = false;
		}
		T val = gameObject.AddComponent<T>();
		if (platform != null)
		{
			val.Board(platform);
			val.transform.localPosition = position;
			val.transform.localRotation = quaternion;
		}
		val._E025 = bones;
		val._E001(equipment, customization, reinitBody: true, gameWorld, side, Vector3.zero, componentInChildren.Pelvis.Original, ragdollEnabled: false, itemInHands);
		if (val.PlayerBody != null)
		{
			val.PlayerBody.UpdatePlayerRenders(EPointOfView.ThirdPerson, side);
		}
		val.ApplyTransformSync(bones);
		foreach (Collider collider in gameObject.GetComponent<PlayerPoolObject>().Colliders)
		{
			collider.enabled = true;
		}
		return val;
	}

	private void Awake()
	{
		PlayerPoolObject component = base.gameObject.GetComponent<PlayerPoolObject>();
		_E020 = component.JointSpawners;
		_E01F = component.RigidbodySpawners;
		_E021 = component.PlayerRigidbodySleepHierarchy;
	}

	private void _E000()
	{
		if (HasRagdoll)
		{
			Ragdoll = new _EBFC(_E01F, _E020, _E021, _E026, EFTHardSettings.Instance.CorpseMaxDepenetrationVelocity, CollisionDetectionMode, this, CheckCorpseIsStill, PlayerBody.IsVisible, OnRigidbodyStopped, keepRigidbody: false, !EFTHardSettings.Instance.DEBUG_CORPSE_PHYSICS);
			OnRigidbodyStarted();
			_E002();
		}
	}

	private void _E001(_EB0B equipment, _E72D customization, bool reinitBody, GameWorld gameWorld, EPlayerSide side, Vector3 velocity, Transform pelvis, bool ragdollEnabled, _ECF5<Item> itemInHands, _E333 containerCollectionView = null, MongoID firstId = default(MongoID))
	{
		_ECC9.ReleaseBeginSample(_ED3E._E000(212568), _ED3E._E000(212556));
		_E026 = velocity;
		HasRagdoll = ragdollEnabled;
		_pelvis = pelvis;
		_pelvis.gameObject.AddComponent<ProxyTransportee>().Parent = this;
		new _EAEC(equipment, side, equipment.Id, canBeLocalized: true, EOwnerType.Profile, firstId, null, null, containerCollectionView);
		Init(equipment, _ED3E._E000(225276), gameWorld, randomRotation: false, null);
		Customization = customization;
		Side = side;
		ItemInHands = itemInHands;
		PlayerBody = GetComponentInChildren<PlayerBody>();
		if (reinitBody)
		{
			_E611 @default = _E611.Default;
			PlayerBody.Init(@default.FilterCustomization(customization), equipment, null, LayerMask.NameToLayer(_ED3E._E000(60679)), side);
		}
		_cullingRegisterRadius = 0.005f;
		int layer = LayerMask.NameToLayer(_ED3E._E000(55338));
		_E38B.SetLayersRecursively(base.gameObject, layer, _ED3E._E000(60752));
		_E000();
	}

	public override void Kill()
	{
		if (PlayerBody != null)
		{
			PlayerBody.Dispose();
		}
		base.Kill();
	}

	public void SetItemInHandsLootedCallback(Action itemInHandsLooted)
	{
		_E022 = itemInHandsLooted;
	}

	protected override void RemoveLootItem(_EAF3 args)
	{
		if (args.Status == CommandStatus.Succeed && ItemInHands.Value == args.Item)
		{
			Ragdoll?.ClearWeapon();
			_E022?.Invoke();
			_E022 = null;
			ItemInHands.Value = null;
		}
		base.RemoveLootItem(args);
	}

	private void _E002()
	{
		if (CheckSurfaceRayCast(base.transform.position, 0.5f))
		{
			Sounds asset = Singleton<_ED0A>.Instance.GetAsset<Sounds>(_ED3E._E000(109909));
			if (!(asset == null))
			{
				SurfaceSet surfaceSet = asset.GetSurfaceSet(_currentSurface) ?? asset.GetSurfaceSet(BaseBallistic.ESurfaceSound.Concrete);
				Singleton<BetterAudio>.Instance.PlayDropItem(surfaceSet.ProneDropSoundBank, base.transform.position, 50f);
			}
		}
	}

	protected virtual bool CheckCorpseIsStill(bool sleeping, float timePass)
	{
		if (!sleeping)
		{
			return timePass >= 15f;
		}
		return true;
	}

	protected override void CacheComponents()
	{
	}

	private _E53B[] _E003()
	{
		_E53B[] array = new _E53B[_E01F.Length];
		for (int i = 0; i < _E01F.Length; i++)
		{
			RigidbodySpawner rigidbodySpawner = _E01F[i];
			array[i] = new _E53B
			{
				Position = rigidbodySpawner.transform.position,
				Rotation = rigidbodySpawner.transform.rotation
			};
		}
		return array;
	}

	protected void ApplyTransformSync(_E53B[] transformSyncs)
	{
		if (_E01F.Length != transformSyncs.Length)
		{
			Debug.LogErrorFormat(_ED3E._E000(212553), _E01F.Length, transformSyncs.Length);
			Kill();
			return;
		}
		if (base.Platform != null)
		{
			Transform transform = base.Platform.transform;
			for (int i = 0; i < transformSyncs.Length; i++)
			{
				transformSyncs[i] = new _E53B
				{
					Position = transform.TransformPoint(transformSyncs[i].Position),
					Rotation = transform.rotation * transformSyncs[i].Rotation
				};
			}
		}
		for (int j = 0; j < _E01F.Length; j++)
		{
			RigidbodySpawner obj = _E01F[j];
			_E53B obj2 = transformSyncs[j];
			obj.transform.position = obj2.Position;
			obj.transform.rotation = obj2.Rotation;
		}
	}
}
