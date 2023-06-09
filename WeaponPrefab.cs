using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT;
using EFT.AssetsManager;
using EFT.InventoryLogic;
using EFT.Visual;
using JetBrains.Annotations;
using UnityEngine;

[DisallowMultipleComponent]
public class WeaponPrefab : AssetPoolObject, _E3A7
{
	[Serializable]
	public class AimPlane
	{
		public string Name;

		public float Depth;
	}

	[Serializable]
	public class MaterialConfig
	{
		public string renderer;

		public Material material;
	}

	[Serializable]
	public class LODConfig
	{
		public float screenRelativeTransitionHeight;

		public float fadeTransitionWidth;

		public string[] renderers = Array.Empty<string>();
	}

	public const string BONE_ALT_GRIP = "altpose";

	public const string BONE_SMOKEPORT = "smokeport";

	public const string BONE_FIREPORT = "fireport";

	public const string EXTRACTOR_GO_NAME = "extractor_smoke";

	private const string m__E007 = "HIDE_SHADOW";

	[SerializeField]
	public GameObject _weaponObject;

	[SerializeField]
	public GameObject _weaponObjectSimple;

	[SerializeField]
	public RuntimeAnimatorController _originalAnimatorController;

	[SerializeField]
	public RuntimeAnimatorController _animatorSimple;

	[SerializeField]
	public RuntimeAnimatorController _animatorSpirit;

	[SerializeField]
	public TextAsset _fastAnimatorControllerBinaryData;

	[SerializeField]
	private Avatar _avatar;

	[SerializeField]
	public RestSettings RestSettings;

	public GameObject DefaultMuzzlePrefab;

	public GameObject DefaultSmokeport;

	public GameObject DefaultHeatHazeEffect;

	public Vector3 RecoilCenter;

	public Vector3 RotationCenter;

	public Vector3 RotationCenterNoStock;

	public Vector2 DupletAccuracyPenaltyX;

	public Vector2 DupletAccuracyPenaltyY;

	public AimPlane FarPlane = new AimPlane
	{
		Name = _ED3E._E000(64474),
		Depth = 0.5f
	};

	public AimPlane DefaultAimPlane = new AimPlane
	{
		Name = _ED3E._E000(64467),
		Depth = 0f
	};

	public AimPlane[] CustomAimPlanes;

	[SerializeField]
	public GameObject _objectInstance;

	[SerializeField]
	private Transform _localWeaponRoot;

	private Player m__E008;

	private IAnimator m__E009;

	private IAnimator _E00A;

	private Animator _E00B;

	private FirearmsAnimator _E00C;

	private _E4EB _E00D;

	private _E570 _E00E;

	private _E508._E000 _E00F;

	private Weapon _E010;

	private Vector3 _E011;

	private Quaternion _E012;

	private Vector3 _E013;

	private bool _E014;

	private bool _E015;

	private float _E016;

	private List<HotObject> _E017 = new List<HotObject>();

	private List<Material> _E018 = new List<Material>();

	private GunShadowDisabler[] _E019 = Array.Empty<GunShadowDisabler>();

	private float _E01A = -1f;

	[Header("Extractor params")]
	public string[] RemoveChildrenOf;

	public string[] AnimatedBones;

	public TransformLinks Hierarchy;

	public _E6FB ObjectInHands;

	public int[] LayersDefaultStates;

	public MaterialConfig[] MaterialsConfig = Array.Empty<MaterialConfig>();

	[Header("Extractor params for LODs")]
	public LODConfig[] LodsConfig = Array.Empty<LODConfig>();

	public IAnimator Animator => this.m__E009;

	public FirearmsAnimator FirearmsAnimator => _E00C;

	public _E570 AnimationEventsEmitter => _E00E;

	public GameObject WeaponObject => _weaponObject;

	public RuntimeAnimatorController AnimatorController => _originalAnimatorController;

	public List<HotObject> HotObjects => _E017;

	public float CurrentOverheat
	{
		get
		{
			if (_E010 == null)
			{
				return 0f;
			}
			if (Math.Abs(_E010.MalfState.LastShotTime - _E016) < Mathf.Epsilon)
			{
				return 0f;
			}
			float modsCoolFactor;
			float currentOverheat = _E010.GetCurrentOverheat(_E62A.PastTime, Singleton<_E5CB>.Instance.Overheat, out modsCoolFactor);
			if (currentOverheat < Mathf.Epsilon)
			{
				_E016 = _E010.MalfState.LastShotTime;
			}
			return currentOverheat;
		}
	}

	public void SetUnderbarrelFastAnimator(Player player)
	{
		player._underbarrelFastAnimator = this.m__E009;
		this.m__E009.enabled = player.ArmsUpdateMode == Player.EUpdateMode.Auto;
		this.m__E009.updateMode = ((player.ArmsUpdateQueue == EUpdateQueue.FixedUpdate) ? AnimatorUpdateMode.AnimatePhysics : AnimatorUpdateMode.Normal);
	}

	public void ResetUnderbarrelFastAnimator(Player player)
	{
		player._underbarrelFastAnimator = null;
	}

	public Transform Init(Player player, bool parent)
	{
		this.m__E008 = player;
		CacheInternalObjects();
		if (!parent)
		{
			return null;
		}
		return _E001(player);
	}

	public void InitMalfunctionState(Weapon weapon, bool hasPlayer, bool malfunctionKnown, out AmmoPoolObject ammoPoolObject)
	{
		InitHotObjects(weapon);
		IAnimator animator = _E00A ?? this.m__E009;
		_E326.SetMalfunctionType(animator, (int)weapon.MalfState.State);
		if (!malfunctionKnown && (weapon.MalfState.State == Weapon.EMalfunctionState.Misfire || weapon.MalfState.State == Weapon.EMalfunctionState.SoftSlide || weapon.MalfState.State == Weapon.EMalfunctionState.HardSlide))
		{
			_E326.SetMisfireSlideUnknown(animator, malfunctionKnown);
		}
		ammoPoolObject = null;
		_E326.SetMalfunction(animator, (int)weapon.MalfState.State);
		animator.SetLayerWeight(animator.GetLayerIndex(_ED3E._E000(1219)), 1f);
		_EA12 malfunctionedAmmo = weapon.MalfState.MalfunctionedAmmo;
		GameObject gameObject = Singleton<_E760>.Instance.CreateItem(malfunctionedAmmo, isAnimated: true);
		Transform shellParent = _E38B.FindActiveTransformRecursive(_localWeaponRoot, _ED3E._E000(64030));
		ammoPoolObject = gameObject.GetComponent<AmmoPoolObject>();
		ammoPoolObject.SetUsed(weapon.MalfState.State != Weapon.EMalfunctionState.Feed);
		_E6F9.ParentShellToTransform(ammoPoolObject.gameObject, shellParent);
		if (!hasPlayer)
		{
			animator.SetLayerWeight(animator.GetLayerIndex(_ED3E._E000(1219)), 1f);
			if (weapon.GetFoldable() != null)
			{
				int layerIndex = animator.GetLayerIndex(_ED3E._E000(64016));
				if (layerIndex >= 0 && layerIndex < animator.layerCount)
				{
					animator.SetLayerWeight(layerIndex, weapon.Folded ? 1 : 0);
				}
				_E326.SetStockFolded(animator, weapon.Folded);
			}
			bool activeSelf = base.gameObject.activeSelf;
			base.gameObject.SetActive(value: true);
			animator.Play(_ED3E._E000(52028), 1, 0.1f);
			animator.Update(0.01f);
			base.gameObject.SetActive(activeSelf);
		}
		_E326.SetMalfunction(animator, -1);
	}

	public void RevertMalfunctionState(Weapon weapon, bool hasPlayer)
	{
		InitHotObjects(weapon);
		if (hasPlayer || this.m__E009.GetLayerIndex(_ED3E._E000(1219)) == -1)
		{
			return;
		}
		_E326.SetMalfunction(this.m__E009, 0);
		_E326.SetMalfunctionType(this.m__E009, 0);
		if (this.m__E009 == null || (this.m__E009 is _E564 obj && obj.Animator == null))
		{
			return;
		}
		if (weapon.GetFoldable() != null)
		{
			int layerIndex = this.m__E009.GetLayerIndex(_ED3E._E000(64016));
			if (layerIndex >= 0 && layerIndex < this.m__E009.layerCount)
			{
				this.m__E009.SetLayerWeight(layerIndex, weapon.Folded ? 1 : 0);
			}
			_E326.SetStockFolded(this.m__E009, weapon.Folded);
		}
		bool activeSelf = base.gameObject.activeSelf;
		base.gameObject.SetActive(value: true);
		_E000();
		this.m__E009.SetLayerWeight(this.m__E009.GetLayerIndex(_ED3E._E000(1219)), 0f);
		this.m__E009.Play(_ED3E._E000(52028), 1, 0.1f);
		this.m__E009.Update(0.01f);
		base.gameObject.SetActive(activeSelf);
	}

	private void _E000()
	{
		_E572[] behaviours = this.m__E009.GetBehaviours<_E572>();
		for (int i = 0; i < behaviours.Length; i++)
		{
			behaviours[i].EventsContainer.ResetCache();
		}
	}

	public void InitHotObjects(Weapon weapon)
	{
		_E010 = weapon;
		_E005();
		_E006();
	}

	private Transform _E001([CanBeNull] Player player)
	{
		if (player != null)
		{
			base.transform.parent = null;
		}
		base.gameObject.SetActive(value: true);
		if (ObjectInHands != null)
		{
			ObjectInHands.OnWeaponInit();
		}
		_objectInstance.transform.localPosition = Vector3.zero;
		_objectInstance.transform.localRotation = Quaternion.identity;
		_E00C = new FirearmsAnimator();
		_E00E = new _E570();
		if (player != null)
		{
			player._animators[1] = this.m__E009;
			this.m__E009.enabled = player.ArmsUpdateMode == Player.EUpdateMode.Auto;
			this.m__E009.updateMode = ((player.ArmsUpdateQueue == EUpdateQueue.FixedUpdate) ? AnimatorUpdateMode.AnimatePhysics : AnimatorUpdateMode.Normal);
			player.PlayerBones.WeaponRoot.Original = Hierarchy.GetTransform(ECharacterWeaponBones.Weapon_root);
			_E00E.SetAnimator(this.m__E009, _E570.EEmitType.EmitOnDemand, player.ProfileId + _ED3E._E000(64014) + player.Profile.Nickname);
			if (_E2B6.Config.UseSpiritPlayer)
			{
				_E002(player);
			}
			_E00C.SetAnimatorGetter(player.GetArmsAnimatorCommon);
		}
		else
		{
			this.m__E009.enabled = true;
			_E00C.SetAnimatorGetter(() => this.m__E009);
			_E00E.SetAnimator(this.m__E009, _E570.EEmitType.EmitOnDemand, _ED3E._E000(50105));
		}
		_E00E.OnEventAction += _E00C.AnimatorEventHandler;
		_E014 = true;
		return _objectInstance.transform;
	}

	private void _E002(Player player)
	{
		bool flag = true;
		flag = this.m__E009.enabled;
		player.Spirit.InitArmsAnimator(_E00A, _animatorSpirit, this.m__E009, flag);
		Dictionary<int, _E572> dictionary = this.m__E009.GetBehaviours<_E572>().ToDictionary((_E572 x) => x.FullNameHash, (_E572 x) => x);
		Dictionary<int, _E572> dictionary2 = player.Spirit.ArmsAnimator.GetBehaviours<_E572>().ToDictionary((_E572 x) => x.FullNameHash, (_E572 x) => x);
		foreach (KeyValuePair<int, _E572> item in dictionary)
		{
			if (dictionary2.ContainsKey(item.Key))
			{
				dictionary2[item.Key].EventsContainer = item.Value.EventsContainer;
				continue;
			}
			Debug.LogErrorFormat(_ED3E._E000(64008), item.Key, base.gameObject.name);
		}
		foreach (KeyValuePair<int, _E572> item2 in dictionary2)
		{
			_ = item2;
		}
	}

	public void RebindAnimator(Player player)
	{
		_E00E.RemoveBindedAnimator();
		this.m__E009.RebindBones();
		_E00E.SetAnimator(this.m__E009, _E570.EEmitType.EmitOnDemand, player.ProfileId + _ED3E._E000(64014) + player.Profile.Nickname);
		if (_E2B6.Config.UseSpiritPlayer)
		{
			_E002(player);
		}
	}

	protected override void ReturnToPool()
	{
		_E007();
		_E008();
		_E010 = null;
		if (_E00E != null)
		{
			_E00E.Dispose();
			_E00E = null;
		}
		this.m__E009.enabled = false;
		if (_E2B6.Config.UseSpiritPlayer)
		{
			_E00A.enabled = false;
		}
		_E003();
		if ((bool)Hierarchy)
		{
			Hierarchy.Self.localScale = new Vector3(1f, 1f, 1f);
			Hierarchy.ResetPositions();
		}
		if (base.transform.parent != null)
		{
			base.transform.parent = null;
		}
		_E00C = null;
		base.ReturnToPool();
		_E014 = false;
	}

	private void _E003()
	{
		if (_E00A != null && !_E00A.Reset() && _E014)
		{
			Debug.LogErrorFormat(_ED3E._E000(64088), GetHashCode());
		}
		if (!this.m__E009.Reset() && _E014)
		{
			Debug.LogErrorFormat(_ED3E._E000(64103), GetHashCode());
		}
		ResetStatesToDefault();
	}

	public void ResetStatesToDefault()
	{
		if (LayersDefaultStates == null)
		{
			Debug.LogError(_ED3E._E000(64186) + base.Name, this);
			return;
		}
		if (this.m__E009 is _E564 obj && !obj.Animator.gameObject.activeInHierarchy)
		{
			if (_E014)
			{
				Debug.LogError(_ED3E._E000(64216) + base.Name, this);
			}
			return;
		}
		if (LayersDefaultStates.Length != this.m__E009.layerCount)
		{
			if (_E014)
			{
				Debug.LogErrorFormat(this, _ED3E._E000(64282), LayersDefaultStates.Length, this.m__E009.layerCount, base.Name);
			}
			return;
		}
		for (int i = 0; i < this.m__E009.layerCount; i++)
		{
			int num = LayersDefaultStates[i];
			if (num != 0)
			{
				this.m__E009.Play(num, i, 0f);
			}
		}
		this.m__E009.Update(0f);
	}

	public override void OnCreatePoolRoleModel()
	{
		_E004();
		Hierarchy = _objectInstance.GetComponent<TransformLinks>();
		base.OnCreatePoolRoleModel();
	}

	public override void OnCreatePoolObject<TAssetPoolObject>([CanBeNull] _EC3A<TAssetPoolObject> assetsPoolParent)
	{
		CacheInternalObjects();
		if (ResourceType.ItemTemplate is WeaponTemplate)
		{
			ObjectInHands = new _E6F9();
		}
		else
		{
			ObjectInHands = new _E6FB();
		}
		RegisterComponent(ObjectInHands);
		ObjectInHands.OnCreatePoolObjectInit(this);
		base.OnCreatePoolObject(assetsPoolParent);
	}

	public override void InheritRoleModelData<TAssetPoolObject>(TAssetPoolObject roleModel)
	{
		base.InheritRoleModelData(roleModel);
		WeaponPrefab weaponPrefab = roleModel as WeaponPrefab;
		if (weaponPrefab._E00F != null)
		{
			_E00F = weaponPrefab._E00F;
		}
		CacheInternalObjects();
	}

	private void _E004()
	{
		if (!(_objectInstance == null))
		{
			return;
		}
		if (WeaponObject == null)
		{
			Debug.LogError(_ED3E._E000(64347) + base.name);
			return;
		}
		_objectInstance = UnityEngine.Object.Instantiate(WeaponObject);
		Animator component = _objectInstance.GetComponent<Animator>();
		if (component != null)
		{
			component.keepAnimatorControllerStateOnDisable = true;
			component.runtimeAnimatorController = AnimatorController;
			component.enabled = false;
		}
		_objectInstance.transform.SetParent(base.transform);
		_objectInstance.transform.localPosition = Vector3.zero;
		_objectInstance.transform.localRotation = Quaternion.identity;
		if (_localWeaponRoot == null)
		{
			_localWeaponRoot = _objectInstance.transform.FindTransform(_ED3E._E000(64323));
			if (_localWeaponRoot == null)
			{
				Debug.LogWarningFormat(_ED3E._E000(64375), base.gameObject.name);
			}
		}
	}

	protected virtual void CacheInternalObjects()
	{
		if (_E015)
		{
			return;
		}
		_E004();
		if (_objectInstance == null)
		{
			Debug.LogError(_ED3E._E000(64405) + base.name);
			return;
		}
		this.m__E009 = _E563.CreateAnimator(_objectInstance.GetComponent<Animator>());
		if (_E2B6.Config != null && _E2B6.Config.UseSpiritPlayer && _E00A == null)
		{
			GameObject gameObject = new GameObject(_ED3E._E000(64445));
			_E00A = _E563.CreateAnimator(gameObject.AddComponent<Animator>());
			gameObject.transform.ParentFake(_objectInstance.transform);
		}
		_E015 = true;
	}

	private void _E005()
	{
		_E017.Clear();
		GetComponentsInChildren(includeInactive: true, _E017);
	}

	private void _E006()
	{
		if (this.m__E008 == null || !this.m__E008.IsYourPlayer)
		{
			return;
		}
		_E019 = GetComponentsInChildren<GunShadowDisabler>(includeInactive: true);
		for (int i = 0; i < _E019.Length; i++)
		{
			_E019[i].DisableGunShadow();
		}
		if (!_E019.Any())
		{
			return;
		}
		_E018.Clear();
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>(includeInactive: true);
		foreach (Renderer renderer in componentsInChildren)
		{
			if (!(renderer == null) && renderer.materials != null)
			{
				for (int k = 0; k < renderer.materials.Length; k++)
				{
					Material material = renderer.materials[k];
					_E018.Add(material);
					material.EnableKeyword(_ED3E._E000(64422));
				}
			}
		}
	}

	private void _E007()
	{
		float temperatureCelsio = HotObject.ConvertHeat2Celsio(0f);
		for (int i = 0; i < _E017.Count; i++)
		{
			_E017[i].SetTemperatureToRenderer(temperatureCelsio, force: true);
		}
		_E017.Clear();
		_E01A = -1f;
	}

	private void _E008()
	{
		for (int i = 0; i < _E018.Count; i++)
		{
			_E018[i].DisableKeyword(_ED3E._E000(64422));
		}
		_E018.Clear();
		for (int j = 0; j < _E019.Length; j++)
		{
			_E019[j].EnableGunShadow();
		}
	}

	private void OnEnable()
	{
		_E3A3.RegisterInSystem(this);
	}

	private void OnDisable()
	{
		_E3A3.UnregisterInSystem(this);
	}

	public void ManualUpdate()
	{
		float currentOverheat = CurrentOverheat;
		if (!(Math.Abs(currentOverheat - _E01A) < 0.01f))
		{
			_E01A = currentOverheat;
			float temperatureCelsio = HotObject.ConvertHeat2Celsio(currentOverheat);
			for (int i = 0; i < _E017.Count; i++)
			{
				HotObject hotObject = _E017[i];
				hotObject.SetTemperatureToRenderer(temperatureCelsio);
				hotObject.PrepareForEffects();
			}
		}
	}

	[CompilerGenerated]
	private IAnimator _E009()
	{
		return this.m__E009;
	}
}
