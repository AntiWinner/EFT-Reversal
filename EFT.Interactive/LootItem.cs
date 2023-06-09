using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.AssetsManager;
using EFT.InventoryLogic;
using EFT.MovingPlatforms;
using EFT.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

namespace EFT.Interactive;

public class LootItem : InteractableObject, MovingPlatform._E000
{
	protected const float _maxPhysicsTime = 15f;

	protected const float _maxPhysicsTimeForContinuousDetectionMode = 7.5f;

	protected const float _sqrSpeedToStopContinuousDetectionMode = 0.008f;

	protected const float DELAY_NEXT_CHECK_COLLISION = 0.5f;

	protected const float DEFAULT_ENERGY_VALUE = 50f;

	public Vector3 Shift;

	public _EB1E ItemOwner;

	[CompilerGenerated]
	private MovingPlatform _E02D;

	public string Name;

	public string StaticId;

	public string ItemId;

	public string TemplateId;

	public bool RandomRotation;

	public string[] ValidProfiles;

	[CompilerGenerated]
	private Player _E02E;

	[CompilerGenerated]
	private bool _E02F;

	protected Rigidbody _rigidBody;

	protected float _currentPhysicsTime;

	protected float _cullingRegisterRadius;

	protected BaseBallistic.ESurfaceSound _currentSurface;

	[_E368]
	[SerializeField]
	private List<Renderer> _renderers = new List<Renderer>(8);

	[_E368]
	[SerializeField]
	private List<LODGroup> _lodGroups = new List<LODGroup>();

	[_E368]
	[SerializeField]
	private List<ShadowCastingMode> _renderersShadowCastingMode = new List<ShadowCastingMode>(8);

	[_E368]
	[SerializeField]
	private BoxCollider _boundCollider;

	public DisablerCullingObject CullingObject;

	[_E368]
	private static Collider[] _E030 = new Collider[512];

	[_E368]
	private static Collider[] _E031 = new Collider[512];

	private float _E032;

	private Item _E033;

	private bool _E034;

	private IEnumerator _E035;

	private bool _E036;

	private const int _E037 = 8;

	private Vector3 _E038;

	public Item Item => _E033;

	public MovingPlatform Platform
	{
		[CompilerGenerated]
		get
		{
			return _E02D;
		}
		[CompilerGenerated]
		set
		{
			_E02D = value;
		}
	}

	public Rigidbody RigidBody => _rigidBody;

	public Player LastOwner
	{
		[CompilerGenerated]
		get
		{
			return _E02E;
		}
		[CompilerGenerated]
		set
		{
			_E02E = value;
		}
	}

	public bool IsPhysicsOn => _E035 != null;

	public bool PerformPickUpValidation
	{
		[CompilerGenerated]
		get
		{
			return _E02F;
		}
		[CompilerGenerated]
		private set
		{
			_E02F = value;
		}
	}

	private LayerMask _E000 => _E37B.AudioControllerStepLayerMask;

	public virtual float PhysicsQuality => 0f;

	public bool IsVisibilityEnabled => _renderers[0].enabled;

	public static T CreateStaticLoot<T>(GameObject gameObject, Item item, string name, GameWorld gameWorld, bool randomRotation, [CanBeNull] string[] validProfiles, string staticId = null, bool performPickUpValidation = true, Vector3 shift = default(Vector3)) where T : LootItem
	{
		T val = _E000<T>(gameObject, item, name, gameWorld, randomRotation, validProfiles, staticId, performPickUpValidation);
		val._E002(shift);
		List<Collider> colliders = gameObject.GetComponent<AssetPoolObject>().GetColliders(includeNestedAssetPoolObjects: true);
		if (colliders.Count == 0)
		{
			Debug.LogError(_ED3E._E000(212739) + gameObject.name);
		}
		else
		{
			_E001(gameObject, colliders, val._boundCollider);
		}
		val.OnRigidbodyStopped();
		return val;
	}

	private static _E077 _E000<_E077>(GameObject gameObject, Item item, string name, GameWorld gameWorld, bool randomRotation, [CanBeNull] string[] validProfiles, string staticId = null, bool performPickUpValidation = true) where _E077 : LootItem
	{
		_E077 component = gameObject.GetComponent<_E077>();
		if ((UnityEngine.Object)component == (UnityEngine.Object)null)
		{
			Debug.LogErrorFormat(_ED3E._E000(212768), item);
		}
		component.enabled = true;
		component.Init(item, name, gameWorld, randomRotation, validProfiles, staticId, performPickUpValidation);
		return component;
	}

	protected LootItem Init(Item item, string itemName, GameWorld gameWorld, bool randomRotation, [CanBeNull] string[] validProfiles, string staticId = null, bool performPickUpValidation = true)
	{
		ItemOwner = (_EB1E)item.Parent.GetOwner();
		_E033 = item;
		Name = itemName;
		ItemId = item.Id;
		TemplateId = item.TemplateId;
		RandomRotation = randomRotation;
		ValidProfiles = validProfiles;
		StaticId = staticId;
		PerformPickUpValidation = performPickUpValidation;
		gameWorld.RegisterLoot(this);
		if (item.QuestItem)
		{
			base.gameObject.SetActive(validProfiles != null && (validProfiles.Contains(gameWorld.CurrentProfileId) || gameWorld.CurrentProfileId == null));
		}
		ItemOwner.RemoveItemEvent += RemoveLootItem;
		_E008();
		foreach (Transform item2 in base.gameObject.transform)
		{
			_E38B.SetLayersRecursively(item2.gameObject, _E37B.LootLayer);
		}
		base.gameObject.layer = LayerMask.NameToLayer(_ED3E._E000(60775));
		return this;
	}

	public override void OnCreatePoolRoleModel(AssetPoolObject assetPoolObject)
	{
		CacheComponents();
		_boundCollider = base.gameObject.AddComponent<BoxCollider>();
		_boundCollider.size = Vector3.zero;
		_boundCollider.isTrigger = true;
		_boundCollider.enabled = false;
		base.OnCreatePoolRoleModel(assetPoolObject);
	}

	public override void OnReturnToPool(AssetPoolObject assetPoolObject)
	{
		_E00B(isVisible: true);
		_boundCollider.enabled = false;
		base.OnReturnToPool(assetPoolObject);
	}

	public void CacheParameters()
	{
		_E038 = base.transform.position;
	}

	protected virtual void CacheComponents()
	{
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
		LODGroup[] componentsInChildren2 = GetComponentsInChildren<LODGroup>();
		AddCacheComponents(componentsInChildren, componentsInChildren2);
	}

	protected void AddCacheComponents(IEnumerable<Renderer> renderers, IEnumerable<LODGroup> lodGroups)
	{
		foreach (Renderer renderer in renderers)
		{
			_renderers.Add(renderer);
			_renderersShadowCastingMode.Add(renderer.shadowCastingMode);
		}
		_lodGroups.AddRange(lodGroups);
	}

	protected void RemoveCacheComponents(IEnumerable<Renderer> renderers, IEnumerable<LODGroup> lodGroups)
	{
		foreach (Renderer renderer in renderers)
		{
			int index = _renderers.IndexOf(renderer);
			_renderers.RemoveAt(index);
			_renderersShadowCastingMode.RemoveAt(index);
		}
		foreach (LODGroup lodGroup in lodGroups)
		{
			_lodGroups.Remove(lodGroup);
		}
	}

	public static void CreateLootContainer(LootableContainer lc, Item item, string name, GameWorld gameWorld, string staticId = null)
	{
		lc.Init(new _EB1E(item, item.Id, name));
		gameWorld.RegisterLoot(lc);
	}

	public static void CreateStationaryWeapon(StationaryWeapon stationaryWeapon, Item item, string name, GameWorld gameWorld, string staticId = null)
	{
		stationaryWeapon.Init(new _EB1E(item, item.Id, name));
		gameWorld.RegisterLoot(stationaryWeapon);
	}

	private static Bounds _E001(GameObject lootItemGameobject, IEnumerable<Collider> colliders, BoxCollider resultCollider)
	{
		Vector3 position = lootItemGameobject.transform.position;
		Quaternion rotation = lootItemGameobject.transform.rotation;
		Vector3 localScale = lootItemGameobject.transform.localScale;
		lootItemGameobject.transform.position = Vector3.zero;
		lootItemGameobject.transform.rotation = Quaternion.identity;
		lootItemGameobject.transform.localScale = Vector3.one;
		_E320.SyncTransforms();
		Bounds totalBounds = colliders.GetTotalBounds();
		resultCollider.enabled = true;
		resultCollider.center = totalBounds.center;
		resultCollider.size = Vector3.Max(totalBounds.size, Vector3.Min(totalBounds.size * 1.5f, Vector3.one * 0.125f));
		lootItemGameobject.transform.position = position;
		lootItemGameobject.transform.rotation = rotation;
		lootItemGameobject.transform.localScale = localScale;
		return resultCollider.bounds;
	}

	public static T CreateLootWithRigidbody<T>(GameObject gameObject, Item item, string name, GameWorld gameWorld, bool randomRotation, [CanBeNull] string[] validProfiles, out BoxCollider collider, bool forceLayerSetup = false, bool performPickUpValidation = true, float makeVisibleAfterDelay = 0f) where T : LootItem
	{
		T val = _E000<T>(gameObject, item, name, gameWorld, randomRotation, validProfiles, null, performPickUpValidation);
		AssetPoolObject component = val.GetComponent<AssetPoolObject>();
		val._rigidBody = val.gameObject.GetComponent<Rigidbody>();
		if (val._rigidBody == null)
		{
			val._rigidBody = val.gameObject.AddComponent<Rigidbody>();
		}
		if (gameObject.activeInHierarchy)
		{
			val._E004();
		}
		else
		{
			val._E036 = true;
		}
		if (component != null)
		{
			component.RegisteredComponentsToClean.Add(val._rigidBody);
		}
		val._rigidBody.mass = val._E033.GetSingleItemTotalWeight();
		val._rigidBody.isKinematic = false;
		val._currentPhysicsTime = 0f;
		val._E002(val._rigidBody.centerOfMass);
		List<Collider> colliders = component.GetColliders(includeNestedAssetPoolObjects: true);
		if (colliders.Count == 0)
		{
			Debug.LogError(_ED3E._E000(212739) + gameObject.name);
		}
		else
		{
			_E001(gameObject, colliders, val._boundCollider);
		}
		val._cullingRegisterRadius = 0.005f;
		Vector3 size = val._boundCollider.size;
		if (size.x * size.y * size.z <= EFTHardSettings.Instance.LootVolumeForHighQuallityPhysicsClient)
		{
			val._rigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
		}
		else
		{
			val._rigidBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
		}
		val.OnRigidbodyStarted();
		collider = val._boundCollider;
		if (makeVisibleAfterDelay > 0f)
		{
			val._E00B(isVisible: false);
			val.StartCoroutine(val._E00C(makeVisibleAfterDelay));
		}
		return val;
	}

	private void _E002(Vector3 shift)
	{
		_E003();
		if (shift == Vector3.zero)
		{
			return;
		}
		Shift = shift;
		foreach (Transform item in base.transform)
		{
			item.localPosition -= Shift;
		}
	}

	private void _E003()
	{
		if (Shift == Vector3.zero)
		{
			return;
		}
		foreach (Transform item in base.transform)
		{
			item.localPosition += Shift;
		}
		Shift = Vector3.zero;
	}

	private void OnEnable()
	{
		if (_E036)
		{
			_E004();
			_E036 = false;
		}
	}

	private void _E004()
	{
		_E034 = false;
		if (_rigidBody != null)
		{
			_E320._E002.SupportRigidbody(_rigidBody, visibilityChecker: GetVisibilityChecker(), quality: PhysicsQuality);
			_E035 = _E005();
			StartCoroutine(_E035);
		}
	}

	protected virtual _E383 GetVisibilityChecker()
	{
		return null;
	}

	protected virtual void OnRigidbodyStarted()
	{
	}

	protected virtual void OnRigidbodyStopped()
	{
		_E00B(isVisible: true);
		_E006();
	}

	protected virtual void RemoveLootItem(_EAF3 args)
	{
		if (args.Status == CommandStatus.Succeed && args.From is _EB1F && args.Item.Id == ItemId)
		{
			AudioClip itemClip = Singleton<GUISounds>.Instance.GetItemClip(ItemOwner.RootItem.ItemSound, EInventorySoundType.pickup);
			MonoBehaviourSingleton<BetterAudio>.Instance.PlayAtPoint(base.transform.position, itemClip, _E8A8.Instance.Distance(base.transform.position), BetterAudio.AudioSourceGroupType.Environment, 10);
			if (Singleton<GameWorld>.Instantiated)
			{
				Singleton<GameWorld>.Instance.DestroyLoot(this);
			}
		}
	}

	private IEnumerator _E005()
	{
		while (!(_rigidBody == null))
		{
			if (IsRigidbodyDone())
			{
				StopPhysics();
			}
			_currentPhysicsTime += Time.deltaTime;
			yield return null;
		}
	}

	private void _E006()
	{
		int num = _E320._E003.OverlapSphereNonAlloc(_E320._E003.EWorldType.DisablerCullingObjectTriggers, base.transform.position, _cullingRegisterRadius, _E030, _E031, _E37B.DisablerCullingObjectLayerMask, QueryTriggerInteraction.Collide);
		if (num <= 0)
		{
			return;
		}
		float num2 = float.MaxValue;
		for (int i = 0; i < num; i++)
		{
			ColliderReporter component = _E030[i].GetComponent<ColliderReporter>();
			if (!(component != null))
			{
				continue;
			}
			for (int j = 0; j < component.Owners.Count; j++)
			{
				DisablerCullingObject disablerCullingObject = component.Owners[j] as DisablerCullingObject;
				if (!(disablerCullingObject != null) || !(CullingObject != disablerCullingObject) || !disablerCullingObject.enabled || !disablerCullingObject.gameObject.activeInHierarchy || !disablerCullingObject.AllowLootCulling)
				{
					continue;
				}
				Bounds bounds = disablerCullingObject.GetBounds();
				if (bounds.Contains(base.transform.position))
				{
					float num3 = Vector3.SqrMagnitude(bounds.center - base.transform.position);
					if (num3 < num2)
					{
						num2 = num3;
						CullingObject = disablerCullingObject;
					}
				}
			}
		}
		RegisterInCullingObject(_renderers, _lodGroups);
		Array.Clear(_E030, 0, num);
		Array.Clear(_E031, 0, _E031.Length);
	}

	protected void RegisterInCullingObject(IEnumerable<Renderer> renderers, IEnumerable<LODGroup> lodGroups)
	{
		if (CullingObject != null)
		{
			CullingObject.RegisterComponents(renderers, ignoreInverseColliders: true);
			CullingObject.RegisterComponents(lodGroups, ignoreInverseColliders: true);
			Debug.LogFormat(CullingObject, _ED3E._E000(212806), _E033, CullingObject.gameObject.name);
		}
	}

	protected virtual bool IsRigidbodyDone()
	{
		if (_rigidBody.IsSleeping())
		{
			return true;
		}
		if (_rigidBody.collisionDetectionMode == CollisionDetectionMode.Continuous && _rigidBody.velocity.sqrMagnitude <= 0.008f)
		{
			return _currentPhysicsTime >= 7.5f;
		}
		return _currentPhysicsTime >= 15f;
	}

	protected void StopPhysics()
	{
		if (_rigidBody != null)
		{
			UnityEngine.Object.Destroy(_rigidBody);
			_rigidBody = null;
			OnRigidbodyStopped();
		}
		if (_E035 != null)
		{
			StopCoroutine(_E035);
			_E035 = null;
		}
	}

	public override void Kill()
	{
		base.Kill();
		_E003();
		ItemOwner.RemoveItemEvent -= RemoveLootItem;
		_E00A();
		_E007();
		CullingObject = null;
		AssetPoolObject.ReturnToPool(base.gameObject);
	}

	private void _E007()
	{
		UnregisterFromCullingObject(_renderers, _lodGroups);
	}

	protected void UnregisterFromCullingObject(IEnumerable<Renderer> renderers, IEnumerable<LODGroup> lodGroups)
	{
		if (!(CullingObject != null))
		{
			return;
		}
		CullingObject.UnregisterComponents(renderers, ignoreInverseColliders: true);
		CullingObject.UnregisterComponents(lodGroups, ignoreInverseColliders: true);
		foreach (Renderer renderer in renderers)
		{
			if (_renderers.Contains(renderer))
			{
				renderer.enabled = true;
			}
		}
		foreach (LODGroup lodGroup in lodGroups)
		{
			if (_lodGroups.Contains(lodGroup))
			{
				lodGroup.enabled = true;
			}
		}
	}

	private void _E008()
	{
		TryDisableShadow(_renderers);
	}

	protected void TryDisableShadow(IEnumerable<Renderer> renderers)
	{
		if (_E033 is Weapon || _E033 is Mod)
		{
			_E009(renderers);
		}
	}

	private void _E009(IEnumerable<Renderer> renderers)
	{
		foreach (Renderer renderer in renderers)
		{
			if (renderer.shadowCastingMode != 0)
			{
				renderer.shadowCastingMode = ShadowCastingMode.Off;
			}
		}
	}

	private void _E00A()
	{
		RestoreShadows(_renderers);
	}

	protected void RestoreShadows(IEnumerable<Renderer> renderers)
	{
		foreach (Renderer renderer in renderers)
		{
			int num = _renderers.IndexOf(renderer);
			if (num >= 0)
			{
				ShadowCastingMode shadowCastingMode = _renderersShadowCastingMode[num];
				renderer.shadowCastingMode = shadowCastingMode;
			}
		}
	}

	public int GetNetId()
	{
		return _E033.Id.GetHashCode();
	}

	public override string ToString()
	{
		return base.ToString() + _ED3E._E000(29351) + _E033;
	}

	public virtual void Board(MovingPlatform platform)
	{
		Platform = platform;
		base.transform.parent = platform.transform;
	}

	public virtual void GetOff(MovingPlatform platform)
	{
		if (!(Platform != platform))
		{
			Platform = null;
			base.transform.parent = null;
		}
	}

	private void _E00B(bool isVisible)
	{
		foreach (LODGroup lodGroup in _lodGroups)
		{
			lodGroup.enabled = isVisible;
		}
		foreach (Renderer renderer in _renderers)
		{
			renderer.enabled = isVisible;
		}
	}

	private IEnumerator _E00C(float delay)
	{
		yield return new WaitForSeconds(delay);
		_E00B(isVisible: true);
	}

	public void SetItemAndRigidbody(Item item, Rigidbody rigidbody)
	{
		_E033 = item;
		_rigidBody = rigidbody;
	}

	protected void PlayDropSound()
	{
		if (_E033 != null && _E033.DropSoundType != 0 && !_E034)
		{
			Vector3 center = base.transform.TransformPoint(_boundCollider.center);
			if (CheckSurfaceRayCast(center, 0.5f))
			{
				Singleton<BetterAudio>.Instance.PlayDropItem(_currentSurface, _E033.DropSoundType, base.transform.position, 50f);
			}
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (_E033 != null && _E033.DropSoundType != 0)
		{
			_E034 = true;
			if (_E00E(collision, 0.5f))
			{
				float energy = (_rigidBody ? _E00D() : 50f);
				Singleton<BetterAudio>.Instance.PlayDropItem(_currentSurface, _E033.DropSoundType, base.transform.position, energy);
				_E038 = base.transform.position;
			}
		}
	}

	private float _E00D()
	{
		float mass = _rigidBody.mass;
		float num = Mathf.Abs(_E038.y - base.transform.position.y);
		float num2 = mass * Physics.gravity.magnitude * num;
		float num3 = mass * Mathf.Pow(_rigidBody.velocity.magnitude, 2f);
		return num2 + num3;
	}

	private bool _E00E(Collision collision, float delayToNextCheck = 1f)
	{
		if (_E032 > Time.time)
		{
			return false;
		}
		_E032 = Time.time + delayToNextCheck;
		BaseBallistic component = collision.collider.GetComponent<BaseBallistic>();
		if (component != null)
		{
			_currentSurface = component.GetSurfaceSound(collision.contacts[0].point);
			return true;
		}
		return false;
	}

	protected bool CheckSurfaceRayCast(Vector3 center, float delayToNextCheck = 1f)
	{
		if (_E032 > Time.time)
		{
			return false;
		}
		_E032 = Time.time + delayToNextCheck;
		return _E00F(center);
	}

	private bool _E00F(Vector3 center)
	{
		if (!Physics.Raycast(new Ray(center, Vector3.down), out var hitInfo, 10f, this._E000))
		{
			return false;
		}
		_currentSurface = BaseBallistic.ESurfaceSound.Concrete;
		BaseBallistic component = hitInfo.collider.GetComponent<BaseBallistic>();
		if (component != null)
		{
			_currentSurface = component.GetSurfaceSound(hitInfo.point);
		}
		return true;
	}
}
