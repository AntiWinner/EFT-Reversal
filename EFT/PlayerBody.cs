using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Comfort.Common;
using Diz.Binding;
using Diz.Skinning;
using EFT.AssetsManager;
using EFT.InventoryLogic;
using EFT.Visual;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

namespace EFT;

public class PlayerBody : MonoBehaviour
{
	public class _E000
	{
		[CompilerGenerated]
		private sealed class _E000
		{
			public PlayerBody._E000 _003C_003E4__this;

			public EquipmentSlot equipmentSlot;

			public Action<Item> _003C_003E9__3;

			internal void _E000(Item item)
			{
				_003C_003E4__this._E003();
				_003C_003E4__this._E002();
				if (equipmentSlot == EquipmentSlot.Scabbard && item is _EA60 obj && !obj.KnifeComponent.Template.DisplayOnModel)
				{
					item = null;
				}
				if (item != null)
				{
					_003C_003E4__this._E005 = item;
					_003C_003E4__this._E009 = item.ChildrenChanged.Subscribe(delegate
					{
						_003C_003E4__this._E003();
						_003C_003E4__this._E000();
					});
					_003C_003E4__this._E000();
				}
				else
				{
					_003C_003E4__this.DestroyCurrentModel();
				}
			}

			internal void _E001(Item child)
			{
				_003C_003E4__this._E003();
				_003C_003E4__this._E000();
			}

			internal void _E002(Item item)
			{
				if (_003C_003E4__this._E006 != null)
				{
					_003C_003E4__this._E004(_003C_003E4__this.m__E001, _003C_003E4__this._E006);
				}
			}
		}

		[CompilerGenerated]
		private sealed class _E001
		{
			public GameObject result;

			internal void _E000()
			{
				if (result != null)
				{
					AssetPoolObject.ReturnToPool(result);
				}
			}
		}

		private readonly Slot m__E000;

		private readonly PlayerBody m__E001;

		[CanBeNull]
		private readonly Transform m__E002;

		[CanBeNull]
		private readonly Slot m__E003;

		[CanBeNull]
		private readonly Transform m__E004;

		public Task LoadingJob;

		public Dress[] Dresses;

		public readonly _ECF5<Dress> MainDress = new _ECF5<Dress>();

		public readonly _ECF5<GameObject> ParentedModel = new _ECF5<GameObject>(null);

		private Item _E005;

		private GameObject _E006;

		private CancellationTokenSource _E007;

		private Action _E008;

		private Action _E009;

		private Action _E00A;

		[CompilerGenerated]
		private EquipmentSlot _E00B;

		public EquipmentSlot EquipmentSlot
		{
			[CompilerGenerated]
			get
			{
				return _E00B;
			}
			[CompilerGenerated]
			private set
			{
				_E00B = value;
			}
		}

		public _ECF5<Item> ContainedItem => this.m__E000.ReactiveContainedItem;

		public _E000(PlayerBody playerBody, Slot slot, [CanBeNull] Transform bone, EquipmentSlot equipmentSlot, [CanBeNull] Slot backpackSlot = null, [CanBeNull] Transform alternativeHolsterBone = null)
		{
			PlayerBody._E000 obj = this;
			this.m__E001 = playerBody;
			this.m__E000 = slot;
			this.m__E002 = bone;
			this.m__E003 = backpackSlot;
			this.m__E004 = alternativeHolsterBone;
			EquipmentSlot = equipmentSlot;
			_E008 = ((IBindable<Item>)((playerBody._itemInHands != null) ? ((_ECF4<Item>)_ECF3.Combine(ContainedItem, playerBody._itemInHands, (Item item, Item itemInHands) => (item != itemInHands) ? item : null)) : ((_ECF4<Item>)ContainedItem))).Bind((Action<Item>)delegate(Item item)
			{
				obj._E003();
				obj._E002();
				if (equipmentSlot == EquipmentSlot.Scabbard && item is _EA60 obj2 && !obj2.KnifeComponent.Template.DisplayOnModel)
				{
					item = null;
				}
				if (item != null)
				{
					obj._E005 = item;
					obj._E009 = item.ChildrenChanged.Subscribe(delegate
					{
						obj._E003();
						obj._E000();
					});
					obj._E000();
				}
				else
				{
					obj.DestroyCurrentModel();
				}
			});
			if (this.m__E003 == null || this.m__E003 == this.m__E000)
			{
				return;
			}
			_E00A = this.m__E003.ReactiveContainedItem.Bind(delegate
			{
				if (obj._E006 != null)
				{
					obj._E004(obj.m__E001, obj._E006);
				}
			});
		}

		public void Dispose()
		{
			_E003();
			_E002();
			DestroyCurrentModel();
			_E008?.Invoke();
			_E008 = null;
			_E00A?.Invoke();
			_E00A = null;
		}

		private void _E000()
		{
			LoadingJob = _E001(_E005);
		}

		private async Task _E001(Item item)
		{
			_E007 = new CancellationTokenSource();
			CancellationToken token = _E007.Token;
			GameObject result = await Singleton<_E760>.Instance.CreateItemAsync(item, null, isAnimated: false, _ECE3.General, token);
			CancellationTokenRegistration cancellationTokenRegistration = token.Register(delegate
			{
				if (result != null)
				{
					AssetPoolObject.ReturnToPool(result);
				}
			});
			_E007 = null;
			cancellationTokenRegistration.Dispose();
			if (!token.IsCancellationRequested)
			{
				DestroyCurrentModel();
				_E004(this.m__E001, result);
			}
		}

		private void _E002()
		{
			_E005 = null;
			if (_E009 != null)
			{
				_E009();
				_E009 = null;
			}
		}

		private void _E003()
		{
			if (_E007 != null)
			{
				_E007.Cancel();
				_E007 = null;
			}
		}

		public void DestroyCurrentModel()
		{
			if (Dresses != null)
			{
				Dress[] dresses = Dresses;
				for (int i = 0; i < dresses.Length; i++)
				{
					dresses[i].Unskin();
				}
				Dresses = null;
			}
			if (_E006 != null)
			{
				AssetPoolObject.ReturnToPool(_E006);
				_E006 = null;
			}
			ParentedModel.Value = _E006;
			MainDress.Value = null;
		}

		private void _E004(PlayerBody playerBody, GameObject model)
		{
			Animator componentInChildren = model.GetComponentInChildren<Animator>();
			if (componentInChildren != null)
			{
				componentInChildren.enabled = false;
			}
			_E006 = model;
			_E38B.SetLayersRecursively(_E006, playerBody._layer, _ED3E._E000(60752));
			WeaponPrefab component = _E006.GetComponent<WeaponPrefab>();
			TransformLinks transformLinks = ((component != null) ? component._objectInstance.GetComponent<TransformLinks>() : null);
			Transform transform = ((transformLinks != null) ? transformLinks.GetTransform(ECharacterWeaponBones.Weapon_root) : null);
			if (transform != null)
			{
				Transform transform2 = ((this.m__E003 != null && this.m__E004 != null && this.m__E003.ContainedItem == null) ? this.m__E004 : this.m__E002);
				_E006.transform.SetParent(transform2, worldPositionStays: false);
				_E006.transform.localRotation = Quaternion.identity;
				_E006.transform.localPosition = Vector3.zero;
				if (transform2 != null)
				{
					Quaternion quaternion = Quaternion.Inverse(transform.rotation) * transform2.rotation;
					_E006.transform.localRotation *= quaternion;
					Vector3 vector = transform2.position - transform.position;
					_E006.transform.position += vector;
				}
				else
				{
					Debug.LogError(string.Concat(_ED3E._E000(189076), _E006, _ED3E._E000(63757), this.m__E000));
				}
				_E006.SetActive(value: true);
			}
			else
			{
				DressItem component2 = _E006.GetComponent<DressItem>();
				if (component2 != null)
				{
					_E006.transform.SetParent(playerBody._meshTransform, worldPositionStays: false);
					_E006.SetActive(value: true);
					Dress[] componentsInChildren = component2.DressPrefab.GetComponentsInChildren<Dress>(includeInactive: true);
					component2.EnableLoot(on: false);
					Dress[] array = componentsInChildren;
					foreach (Dress obj in array)
					{
						obj.Init(playerBody);
						obj.Skin(playerBody.PlayerBones.RootJoint, playerBody._meshTransform);
					}
					Dresses = componentsInChildren;
					MainDress.Value = componentsInChildren.FirstOrDefault();
				}
				else
				{
					Dress[] array2 = (Dresses = _E006.GetComponentsInChildren<Dress>(includeInactive: true));
					MainDress.Value = array2.FirstOrDefault();
					Dress[] array = array2;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].Init(playerBody);
					}
					_E006.transform.SetParent(this.m__E002, worldPositionStays: false);
					_E006.transform.localRotation = new Quaternion(0.5f, -0.5f, -0.5f, -0.5f);
					_E006.transform.localPosition = Vector3.zero;
					_E006.SetActive(value: true);
				}
			}
			ParentedModel.Value = _E006;
		}
	}

	[SerializeField]
	private Transform _meshTransform;

	public PlayerBones PlayerBones;

	public Skeleton SkeletonRootJoint;

	public Skeleton SkeletonHands;

	private int _layer;

	private EPlayerSide _side;

	private bool _active;

	public readonly Dictionary<EBodyModelPart, LoddedSkin> BodySkins = _E3A5<EBodyModelPart>.GetDictWith<LoddedSkin>();

	private PluggableBone _watches;

	private _E3D2[] _bodyRenderers;

	private static readonly EquipmentSlot[] SlotNames = new EquipmentSlot[11]
	{
		EquipmentSlot.ArmorVest,
		EquipmentSlot.TacticalVest,
		EquipmentSlot.Backpack,
		EquipmentSlot.Earpiece,
		EquipmentSlot.Eyewear,
		EquipmentSlot.Headwear,
		EquipmentSlot.FaceCover,
		EquipmentSlot.FirstPrimaryWeapon,
		EquipmentSlot.SecondPrimaryWeapon,
		EquipmentSlot.ArmBand,
		EquipmentSlot.Scabbard
	};

	public readonly _E373<EquipmentSlot, _E000> SlotViews = new _E373<EquipmentSlot, _E000>(SlotNames.Length);

	private _ECF5<Item> _itemInHands;

	public _ECF5<string> BodyCustomizationId = new _ECF5<string>();

	public _ECF5<EPlayerSide> PlayerSide = new _ECF5<EPlayerSide>();

	public _ECF5<EPointOfView> PointOfView = new _ECF5<EPointOfView>(EPointOfView.ThirdPerson);

	public Transform MeshTransform => _meshTransform;

	public bool HasIntergratedArmor { get; private set; }

	public Task Init(_E72D customization, _EB0B equipment, [CanBeNull] _ECF5<Item> itemInHands, int layer, EPlayerSide playerSide)
	{
		_active = true;
		_layer = layer;
		_itemInHands = itemInHands;
		BodyCustomizationId.Value = customization[EBodyModelPart.Body];
		PlayerSide.Value = playerSide;
		EquipmentSlot[] slotNames = SlotNames;
		foreach (EquipmentSlot equipmentSlot in slotNames)
		{
			Transform bone = equipmentSlot switch
			{
				EquipmentSlot.Scabbard => PlayerBones.ScabbardTagillaMelee, 
				EquipmentSlot.ArmBand => PlayerBones.RightShoulder.Original, 
				EquipmentSlot.SecondPrimaryWeapon => PlayerBones.HolsterSecondary, 
				EquipmentSlot.FirstPrimaryWeapon => PlayerBones.HolsterPrimary, 
				_ => PlayerBones.Head.Original, 
			};
			Transform alternativeHolsterBone = equipmentSlot switch
			{
				EquipmentSlot.Scabbard => PlayerBones.ScabbardTagillaMelee, 
				EquipmentSlot.SecondPrimaryWeapon => PlayerBones.HolsterSecondaryAlternative, 
				EquipmentSlot.FirstPrimaryWeapon => PlayerBones.HolsterPrimaryAlternative, 
				_ => null, 
			};
			_E000 value = new _E000(this, equipment.GetSlot(equipmentSlot), bone, equipmentSlot, equipment.GetSlot(EquipmentSlot.Backpack), alternativeHolsterBone);
			SlotViews.AddOrReplace(equipmentSlot, value)?.Dispose();
		}
		_E60E instance = Singleton<_E60E>.Instance;
		HasIntergratedArmor = instance.HasIntegratedArmor(customization[EBodyModelPart.Body]);
		foreach (KeyValuePair<EBodyModelPart, string> item in customization)
		{
			ResourceKey bundle = instance.GetBundle(item.Value);
			if (bundle != null)
			{
				SetSkin(new KeyValuePair<EBodyModelPart, ResourceKey>(item.Key, bundle), (item.Key == EBodyModelPart.Hands) ? SkeletonHands : SkeletonRootJoint);
				continue;
			}
			Debug.LogErrorFormat(_ED3E._E000(188933), item.Key, item.Value);
		}
		_bodyRenderers = new _E3D2[1]
		{
			new _E3D2
			{
				DecalType = EDecalTextureType.Blood,
				Renderers = BodySkins.Values.SelectMany((LoddedSkin x) => x.GetRenderers()).ToArray()
			}
		};
		if (BodySkins.TryGetValue(EBodyModelPart.Hands, out var value2))
		{
			_EBE0 watchBundle = instance.GetWatchBundle(customization[EBodyModelPart.Hands]);
			if (watchBundle.HasValidPath())
			{
				_E000(watchBundle, SkeletonHands);
			}
			value2.SetShadowCastingMode(ShadowCastingMode.Off);
		}
		if (BodySkins.TryGetValue(EBodyModelPart.Feet, out var value3))
		{
			LegsView component = value3.GetComponent<LegsView>();
			if (component != null)
			{
				component.SetHolster(this);
				SlotViews.AddOrReplace(EquipmentSlot.Holster, new _E000(this, equipment.GetSlot(EquipmentSlot.Holster), component.IsRightLegHolster ? PlayerBones.HolsterPistol : PlayerBones.LeftLegHolsterPistol, EquipmentSlot.Holster))?.Dispose();
			}
		}
		return Task.WhenAll(from x in SlotViews.Where(delegate(_E000 x)
			{
				Task loadingJob = x.LoadingJob;
				return loadingJob != null && !loadingJob.IsCompleted;
			})
			select x.LoadingJob);
	}

	private void _E000(_EBE0 watchBundleInfo, Skeleton handsSkeleton)
	{
		_watches = Singleton<_ED0A>.Instance.InstantiateAsset<PluggableBone>(watchBundleInfo.WatchPrefab);
		_watches.Plug(handsSkeleton, watchBundleInfo.WatchPosition, watchBundleInfo.WatchRotation);
		_watches.GetComponent<Watch>().Init(new TimeSpan(0L));
	}

	public void UpdatePlayerRenders(EPointOfView pointOfView, EPlayerSide side)
	{
		foreach (var (eBodyModelPart2, loddedSkin2) in BodySkins)
		{
			switch (eBodyModelPart2)
			{
			case EBodyModelPart.Hands:
				loddedSkin2.EnableRenderers(pointOfView == EPointOfView.FirstPerson);
				break;
			default:
				loddedSkin2.SetShadowCastingMode((pointOfView == EPointOfView.ThirdPerson) ? ShadowCastingMode.On : ShadowCastingMode.ShadowsOnly);
				break;
			case EBodyModelPart.Feet:
				break;
			}
		}
		if (_watches != null)
		{
			_watches.gameObject.SetActive(pointOfView == EPointOfView.FirstPerson);
		}
		PointOfView.Value = pointOfView;
		PlayerSide.Value = side;
	}

	public bool IsVisible()
	{
		if (!BodySkins[EBodyModelPart.Body].IsVisible())
		{
			return BodySkins[EBodyModelPart.Feet].IsVisible();
		}
		return true;
	}

	public void SetSkin(KeyValuePair<EBodyModelPart, ResourceKey> part, Skeleton skeleton)
	{
		LoddedSkin loddedSkin = Singleton<_ED0A>.Instance.InstantiateAsset<LoddedSkin>(part.Value);
		loddedSkin.Init(skeleton, this);
		loddedSkin.Skin();
		loddedSkin.SetLayer(_layer);
		loddedSkin.transform.SetParent(_meshTransform, worldPositionStays: false);
		if (BodySkins.ContainsKey(part.Key))
		{
			BodySkins[part.Key].Unskin();
			UnityEngine.Object.DestroyImmediate(BodySkins[part.Key].gameObject);
		}
		BodySkins[part.Key] = loddedSkin;
	}

	public void GetBodyRenderersNonAlloc(List<_E3D2> preAllocatedRenederersList)
	{
		foreach (_E000 item2 in SlotViews.GetValuesEnumerator())
		{
			if (item2.Dresses != null)
			{
				Dress[] dresses = item2.Dresses;
				foreach (Dress dress in dresses)
				{
					preAllocatedRenederersList.Add(dress.GetBodyRenderer());
				}
			}
		}
		_E3D2[] bodyRenderers = _bodyRenderers;
		foreach (_E3D2 item in bodyRenderers)
		{
			preAllocatedRenederersList.Add(item);
		}
	}

	public void SetTemperatureForBody(float tempCelsio)
	{
		if (BodySkins.TryGetValue(EBodyModelPart.Head, out var value))
		{
			value.SetTemperature(tempCelsio);
		}
		if (BodySkins.TryGetValue(EBodyModelPart.Feet, out var value2))
		{
			value2.SetTemperature(tempCelsio);
		}
		if (BodySkins.TryGetValue(EBodyModelPart.Body, out var value3))
		{
			value3.SetTemperature(tempCelsio);
		}
		if (BodySkins.TryGetValue(EBodyModelPart.Hands, out var value4))
		{
			value4.SetTemperature(tempCelsio);
		}
	}

	public void Dispose()
	{
		if (_watches != null)
		{
			UnityEngine.Object.Destroy(_watches.gameObject);
		}
		foreach (_E000 item in SlotViews.GetValuesEnumerator())
		{
			item.Dispose();
		}
		SlotViews.Clear();
		foreach (KeyValuePair<EBodyModelPart, LoddedSkin> bodySkin in BodySkins)
		{
			bodySkin.Value.Unskin();
			UnityEngine.Object.DestroyImmediate(bodySkin.Value.gameObject);
		}
		BodySkins.Clear();
		_active = false;
	}

	private void OnApplicationQuit()
	{
		Dispose();
	}

	private void OnDestroy()
	{
		if (_active)
		{
			Debug.LogError(_ED3E._E000(188968));
		}
	}

	public _E000 GetSlotViewByItem(Item item)
	{
		foreach (_E000 item2 in SlotViews.GetValuesEnumerator())
		{
			if (item2.ContainedItem.Value == item)
			{
				return item2;
			}
		}
		return null;
	}

	public string GetSlotViewsDebugString()
	{
		string text = "";
		EquipmentSlot[] slotNames = SlotNames;
		foreach (EquipmentSlot equipmentSlot in slotNames)
		{
			if (SlotViews.ContainsKey(equipmentSlot))
			{
				text += string.Format(_ED3E._E000(32068), equipmentSlot, SlotViews.GetByKey(equipmentSlot).ContainedItem.Value);
			}
		}
		return text;
	}
}
