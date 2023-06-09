using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.AssetsManager;
using EFT.InventoryLogic;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT.UI;

public sealed class PlayerModelView : UIElement
{
	[CompilerGenerated]
	private sealed class _E002
	{
		public _E60E solver;

		public PlayerModelView _003C_003E4__this;

		internal ResourceKey _E000(string x)
		{
			return solver.GetBundle(x);
		}

		internal string _E001(KeyValuePair<EBodyModelPart, string> x)
		{
			return solver.GetWatchBundle(x.Value).WatchPrefab?.path;
		}

		internal void _E002(float p)
		{
			if (_003C_003E4__this._progressSpinner != null)
			{
				_003C_003E4__this._progressSpinner.SetProgress(p * 0.9f);
			}
		}
	}

	private const int EMPTY_HANDS = -1;

	private const int TWO_HANDED_FIREARM = 0;

	private const int ONE_HANDED_FIREARM = 1;

	private const int THROWABLE_WEAPON = 2;

	private const int MELEE_WEAPON = 3;

	[SerializeField]
	private ProgressSpinner _progressSpinner;

	private _ED0E<_ED08>._E002 _bundles;

	private CancellationTokenSource _loadingCancellation;

	private WeaponPrefab _weaponPrefab;

	private IAnimator _weaponAnimator;

	private Coroutine _weaponAnimationCoroutine;

	private _E60F _filter;

	private Vector3 _position;

	private bool _animateWeapon;

	public MenuPlayerPoser ModelPlayerPoser { get; private set; }

	public PlayerBody PlayerBody { get; set; }

	public bool LoadingComplete { get; private set; }

	public event Action LoadingCompletedEvent;

	public async Task Show(Profile profile, _EAE5 inventoryController = null, Action onCreated = null, float update = 0f, Vector3? position = null, bool animateWeapon = true)
	{
		await Show(profile.GetVisualEquipmentState(clone: false), inventoryController, onCreated, update, position, animateWeapon);
	}

	public async Task Show(PlayerVisualRepresentation playerVisual, _EAE5 inventoryController = null, Action onCreated = null, float update = 0f, Vector3? position = null, bool animateWeapon = true)
	{
		LoadingComplete = false;
		if (_loadingCancellation != null)
		{
			_E001();
		}
		_position = position ?? new Vector3(0f, -1f, 5f);
		_animateWeapon = animateWeapon;
		ShowGameObject();
		_loadingCancellation = new CancellationTokenSource();
		_progressSpinner.Show();
		_filter = _E611.Default;
		CancellationTokenRegistration cancellationTokenRegistration = Singleton<_E760>.Instance.PoolsCancellationToken.Register(delegate
		{
			if (_loadingCancellation != null)
			{
				_E001();
			}
		});
		await _E000(playerVisual, inventoryController, onCreated, update, _loadingCancellation.Token);
		cancellationTokenRegistration.Dispose();
	}

	private async Task _E000(PlayerVisualRepresentation playerVisual, [CanBeNull] _EAE5 inventoryController, [CanBeNull] Action onCreated, float update, CancellationToken ct)
	{
		if (ct.IsCancellationRequested)
		{
			return;
		}
		List<Slot> source = new List<Slot>
		{
			playerVisual.Equipment.GetSlot(EquipmentSlot.FirstPrimaryWeapon),
			playerVisual.Equipment.GetSlot(EquipmentSlot.SecondPrimaryWeapon),
			playerVisual.Equipment.GetSlot(EquipmentSlot.Holster),
			playerVisual.Equipment.GetSlot(EquipmentSlot.Scabbard)
		};
		Item item = ((inventoryController != null && (!(inventoryController is Player.PlayerInventoryController playerInventoryController) || !(playerInventoryController._E000.MovementContext.StationaryWeapon != null))) ? inventoryController.ItemInHands : source.Select((Slot x) => x.ContainedItem).FirstOrDefault((Item x) => x != null));
		_E60E solver = Singleton<_E60E>.Instance;
		_E72D obj = _filter.FilterCustomization(playerVisual.Customization);
		string[] keys = new string[1] { _ED3E._E000(250820) }.Concat(from x in obj.Values
			select solver.GetBundle(x) into x
			where x != null
			select x.path).Concat(from x in obj
			select solver.GetWatchBundle(x.Value).WatchPrefab?.path into x
			where !string.IsNullOrEmpty(x)
			select x).Concat(from x in playerVisual.Equipment.GetAllVisibleItems()
			select x.Prefab.path into x
			where !string.IsNullOrEmpty(x)
			select x)
			.ToArray();
		_ED0E<_ED08>._E002 obj2 = Singleton<_ED0A>.Instance.Retain(keys, new _ECCE<float>(delegate(float p)
		{
			if (_progressSpinner != null)
			{
				_progressSpinner.SetProgress(p * 0.9f);
			}
		}));
		await _E612.LoadBundles(obj2);
		if (ct.IsCancellationRequested)
		{
			obj2.Release();
			return;
		}
		_progressSpinner.SetProgress(0.9f);
		_bundles = obj2;
		ModelPlayerPoser = Singleton<_ED0A>.Instance.InstantiateAsset<MenuPlayerPoser>(_ED3E._E000(250820));
		PlayerBody = ModelPlayerPoser.GetComponent<PlayerBody>();
		UnityEngine.Object.DontDestroyOnLoad(ModelPlayerPoser);
		ModelPlayerPoser.gameObject.SetActive(value: false);
		await PlayerBody.Init(obj, playerVisual.Equipment, new _ECF5<Item>(item), _E37B.WeaponPreview, playerVisual.Info.Side);
		if (ct.IsCancellationRequested)
		{
			return;
		}
		LoadingComplete = true;
		this.LoadingCompletedEvent?.Invoke();
		_progressSpinner.Close();
		_weaponAnimator = null;
		TransformLinks transformLinks = null;
		ModelPlayerPoser.gameObject.SetActive(value: true);
		if (item != null && !(item is _EB15))
		{
			_weaponPrefab = Singleton<_E760>.Instance.CreateItem(item, isAnimated: true).GetComponent<WeaponPrefab>();
			if (_weaponPrefab != null)
			{
				Transform obj3 = _weaponPrefab.transform;
				obj3.SetParent(ModelPlayerPoser.PBones.Ribcage.Original, worldPositionStays: false);
				obj3.localPosition = Vector3.zero;
				obj3.localRotation = Quaternion.identity;
				_weaponPrefab.gameObject.SetActive(value: true);
				_weaponPrefab.Init(null, parent: true);
				transformLinks = _weaponPrefab.Hierarchy;
				Animator component = transformLinks.GetComponent<Animator>();
				_weaponAnimator = _E564.Create(component);
			}
		}
		Animator component2 = ModelPlayerPoser.GetComponent<Animator>();
		int num = ((item == null || item is _EB15) ? (-1) : ((item is _EACD) ? 1 : ((item is _EADF) ? 2 : ((item.GetItemComponent<KnifeComponent>() != null) ? 3 : 0))));
		component2.SetLayerWeight(11, 1f);
		component2.SetInteger(_E712.WEAPON_TYPE, num);
		component2.SetFloat(_E712.WEAPON_TYPE_FLOAT_HASH, num);
		component2.SetFloat(_E712.UI_WEAPON_TYPE, num);
		component2.Update(update * 5f);
		bool canReload = false;
		if (_weaponAnimator != null)
		{
			_weaponAnimator.enabled = true;
			_weaponAnimator.Play(_ED3E._E000(62585), 1, 0f);
			if (item is Weapon weapon)
			{
				Fold(_weaponAnimator, weapon.Folded);
				_E326.SetAmmoInMag(_weaponAnimator, weapon.GetMaxMagazineCount());
				_E326.SetMagInWeapon(_weaponAnimator, weapon.GetCurrentMagazine() != null);
				_EA6A currentMagazine = weapon.GetCurrentMagazine();
				canReload = currentMagazine != null && weapon.ReloadMode != Weapon.EReloadMode.OnlyBarrel && weapon.ReloadMode != Weapon.EReloadMode.InternalMagazine;
				if (currentMagazine != null)
				{
					_E326.SetRelTypeOld(_weaponAnimator, currentMagazine.magAnimationIndex);
					_E326.SetRelTypeNew(_weaponAnimator, currentMagazine.magAnimationIndex);
				}
			}
			_weaponAnimator.Update(3f);
		}
		UnityEngine.Object.DestroyImmediate(ModelPlayerPoser.GetComponent<CharacterController>());
		Transform obj4 = ModelPlayerPoser.transform;
		obj4.SetParent(base.transform);
		obj4.localPosition = _position;
		obj4.localRotation = new Quaternion(0f, 180f, 0f, 0f);
		obj4.localScale = Vector3.one;
		_E38B.SetLayersRecursively(ModelPlayerPoser.gameObject, _E37B.WeaponPreview);
		ModelPlayerPoser.ChangeWeapon((_weaponPrefab != null) ? _weaponPrefab.AnimationEventsEmitter : null, _weaponAnimator, transformLinks, canReload, _animateWeapon);
		base.transform.localScale = base.transform.localScale.Divide(base.transform.lossyScale);
		onCreated?.Invoke();
	}

	public void Fold(IAnimator animator, bool folded)
	{
		int layerIndex = animator.GetLayerIndex(_ED3E._E000(64016));
		if (layerIndex >= 0 && layerIndex < animator.layerCount)
		{
			animator.SetLayerWeight(layerIndex, folded ? 1 : 0);
		}
		_E326.SetStockFolded(_weaponAnimator, folded);
	}

	private void _E001()
	{
		if (_loadingCancellation != null)
		{
			if (!LoadingComplete)
			{
				_loadingCancellation?.Cancel();
			}
			_loadingCancellation = null;
			if (_progressSpinner.gameObject.activeSelf)
			{
				_progressSpinner.Close();
			}
			if (_weaponPrefab != null)
			{
				AssetPoolObject.ReturnToPool(_weaponPrefab.gameObject);
				_weaponPrefab = null;
			}
			if (PlayerBody != null)
			{
				PlayerBody.Dispose();
				UnityEngine.Object.DestroyImmediate(PlayerBody.gameObject);
				PlayerBody = null;
				ModelPlayerPoser = null;
			}
			if (_bundles != null)
			{
				_bundles.Release();
				_bundles = null;
			}
			if (_weaponAnimationCoroutine != null)
			{
				StopCoroutine(_weaponAnimationCoroutine);
				_weaponAnimationCoroutine = null;
			}
		}
	}

	public override void Close()
	{
		_E001();
		base.Close();
	}

	[CompilerGenerated]
	private void _E002()
	{
		if (_loadingCancellation != null)
		{
			_E001();
		}
	}
}
