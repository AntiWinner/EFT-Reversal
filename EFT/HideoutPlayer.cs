using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT;

public class HideoutPlayer : LocalPlayer
{
	private new sealed class _E000<_E077> : _E014<_E077> where _E077 : class, ITogglableComponentContainer<TogglableComponent>
	{
		private new readonly Func<_E077, _E077> m__E000;

		public override _E077 Component => _E000(base.Component);

		public _E000(Slot slot, Func<_E077, _E077> togglableComponentGetter, Func<_E077, Action, Action> subscriber)
			: base(slot, subscriber)
		{
			_E000 = togglableComponentGetter;
			Update();
		}
	}

	public new class _E001 : _E9E6
	{
		public float Intensity => 1.17f;

		public NightVisionComponent.EMask Mask => NightVisionComponent.EMask.Binocular;

		public float MaskSize => 1.25f;

		public float NoiseIntensity => 0.02f;

		public float NoiseScale => 5f;

		public Color32 Color => new Color32(121, 232, 121, byte.MaxValue);

		public float DiffuseIntensity => 0f;
	}

	[CompilerGenerated]
	private new sealed class _E003
	{
		public HideoutPlayer _003C_003E4__this;

		public Action nvUnsub;

		public Action tvUnsub;

		internal ThermalVisionComponent _E000(ThermalVisionComponent originalThermalComponent)
		{
			if (_003C_003E4__this.PointOfView != 0)
			{
				return null;
			}
			return originalThermalComponent;
		}

		internal NightVisionComponent _E001(NightVisionComponent originalNVComponent)
		{
			if (_003C_003E4__this.PointOfView != 0)
			{
				return _003C_003E4__this._E103;
			}
			if (originalNVComponent != null)
			{
				return originalNVComponent;
			}
			if (_003C_003E4__this.ThermalVisionObserver?.Component != null)
			{
				return null;
			}
			return _003C_003E4__this._E103;
		}

		internal void _E002()
		{
			nvUnsub();
			tvUnsub();
		}
	}

	[CompilerGenerated]
	private new sealed class _E004
	{
		public Action togglableSub;

		public Action hitSub;

		internal void _E000()
		{
			togglableSub?.Invoke();
			hitSub();
		}
	}

	[CompilerGenerated]
	private new sealed class _E005
	{
		public Action togglableSub;

		public Action hitSub;

		internal void _E000()
		{
			togglableSub?.Invoke();
			hitSub();
		}
	}

	[CompilerGenerated]
	private new sealed class _E006
	{
		public Slot playerSlot;

		internal bool _E000(Slot slot)
		{
			return slot.ID == playerSlot.ID;
		}
	}

	[CompilerGenerated]
	private new sealed class _E008
	{
		public bool handsAreEmpty;

		internal void _E000(Result<_E6C9> setEmptyHandsResult)
		{
			handsAreEmpty = true;
		}
	}

	private readonly NightVisionComponent _E103 = new NightVisionComponent(null, new _E001(), new TogglableComponent(null));

	private bool _E104;

	private _EAED _E105;

	private Action _E106;

	private NightVisionComponent _E107;

	[CompilerGenerated]
	private _EAED _E108;

	[CompilerGenerated]
	private bool _E109 = true;

	[CompilerGenerated]
	private bool _E10A;

	public _EAED OriginalInventory
	{
		[CompilerGenerated]
		get
		{
			return _E108;
		}
		[CompilerGenerated]
		private set
		{
			_E108 = value;
		}
	}

	public _EAED ShootingRangeInventory => _E105;

	public bool IsInPatrol
	{
		[CompilerGenerated]
		get
		{
			return _E109;
		}
		[CompilerGenerated]
		private set
		{
			_E109 = value;
		}
	}

	public bool IsUpdateHideoutPlayerInventoryInProgress
	{
		[CompilerGenerated]
		get
		{
			return _E10A;
		}
		[CompilerGenerated]
		private set
		{
			_E10A = value;
		}
	}

	public bool NightVisionActive
	{
		get
		{
			NightVisionComponent component = base.NightVisionObserver.Component;
			if (component == null || !component.Togglable.On)
			{
				return base.ThermalVisionObserver.Component?.Togglable.On ?? false;
			}
			return true;
		}
	}

	public override EPointOfView PointOfView
	{
		get
		{
			return _playerBody?.PointOfView.Value ?? EPointOfView.ThirdPerson;
		}
		set
		{
			base.PointOfView = value;
			base.ThermalVisionObserver.Update();
			base.NightVisionObserver.Update();
		}
	}

	public bool VisorVisibility
	{
		set
		{
			if (_E8A8.Instance.VisorEffect != null)
			{
				_E8A8.Instance.VisorEffect.Visible = value;
			}
		}
	}

	internal override _EAED _E0DE
	{
		get
		{
			if (!_E104)
			{
				return base._E0DE;
			}
			return _E105;
		}
	}

	protected override void UpdateBreathStatus()
	{
		if (PointOfView == EPointOfView.FirstPerson)
		{
			base.UpdateBreathStatus();
		}
		else if (Speaker.Busy)
		{
			Speaker.Shut();
		}
	}

	public static async Task<HideoutPlayer> Create(int playerId, Vector3 position, Quaternion rotation, string layerName, string prefix, EPointOfView pointOfView, Profile profile, bool aiControl, EUpdateQueue updateQueue, EUpdateMode armsUpdateMode, EUpdateMode bodyUpdateMode, CharacterControllerSpawner.Mode characterControllerMode, Func<float> getSensitivity, Func<float> getAimingSensitivity, _E759 statisticsManager, _E935 questController, _E9C4 healthController, _EB61 inventoryController)
	{
		await _E003(profile, _ECE3.Low);
		HideoutPlayer hideoutPlayer = Player.Create<HideoutPlayer>(_E5D2.PLAYER_BUNDLE_NAME, playerId, position, updateQueue, armsUpdateMode, bodyUpdateMode, characterControllerMode, getSensitivity, getAimingSensitivity, prefix, aiControl);
		hideoutPlayer.OriginalInventory = inventoryController;
		hideoutPlayer._E105 = new SinglePlayerInventoryController(hideoutPlayer, profile.Clone());
		hideoutPlayer._E105.SetExamined(value: true);
		hideoutPlayer.IsYourPlayer = true;
		await hideoutPlayer.Init(rotation, layerName, pointOfView, profile, hideoutPlayer.OriginalInventory, healthController, statisticsManager, questController, new _E610(), EVoipState.NotAvailable, aiControl, async: false);
		foreach (_EA6A item in hideoutPlayer.Inventory.NonQuestItems.OfType<_EA6A>())
		{
			hideoutPlayer._E0DE.StrictCheckMagazine(item, status: true, hideoutPlayer.Profile.MagDrillsMastering, notify: false, useOperation: false);
		}
		hideoutPlayer._handsController = EmptyHandsController._E000<EmptyHandsController>(hideoutPlayer);
		hideoutPlayer._handsController.Spawn(1f, delegate
		{
		});
		hideoutPlayer.AIData = new _E279(null, hideoutPlayer);
		hideoutPlayer.AggressorFound = false;
		hideoutPlayer._animators[0].enabled = true;
		hideoutPlayer.PointOfViewChanged.Subscribe(hideoutPlayer.UpdateBreathStatus);
		return hideoutPlayer;
	}

	protected override void CreateSlotObservers()
	{
		base.ThermalVisionObserver = new _E000<ThermalVisionComponent>(base.Equipment.GetSlot(EquipmentSlot.Headwear), (ThermalVisionComponent originalThermalComponent) => (PointOfView != 0) ? null : originalThermalComponent, (ThermalVisionComponent tv, Action handler) => tv.Togglable.OnChanged.Subscribe(handler));
		base.NightVisionObserver = new _E000<NightVisionComponent>(base.Equipment.GetSlot(EquipmentSlot.Headwear), delegate(NightVisionComponent originalNVComponent)
		{
			if (PointOfView != 0)
			{
				return _E103;
			}
			if (originalNVComponent != null)
			{
				return originalNVComponent;
			}
			return (base.ThermalVisionObserver?.Component != null) ? null : _E103;
		}, (NightVisionComponent nv, Action handler) => nv.Togglable.OnChanged.Subscribe(handler));
		base.FaceShieldObserver = new _E014<FaceShieldComponent>(base.Equipment.GetSlot(EquipmentSlot.Headwear), delegate(FaceShieldComponent fs, Action handler)
		{
			Action togglableSub2 = fs.Togglable?.OnChanged.Subscribe(handler);
			Action hitSub2 = fs.HitsChanged.Subscribe(handler);
			return delegate
			{
				togglableSub2?.Invoke();
				hitSub2();
			};
		});
		base.FaceCoverObserver = new _E014<FaceShieldComponent>(base.Equipment.GetSlot(EquipmentSlot.FaceCover), delegate(FaceShieldComponent fs, Action handler)
		{
			Action togglableSub = fs.Togglable?.OnChanged.Subscribe(handler);
			Action hitSub = fs.HitsChanged.Subscribe(handler);
			return delegate
			{
				togglableSub?.Invoke();
				hitSub();
			};
		});
		Action tvUnsub = base.ThermalVisionObserver.Changed.Subscribe(_E001);
		Action nvUnsub = base.NightVisionObserver.Changed.Subscribe(_E000);
		_E106 = delegate
		{
			nvUnsub();
			tvUnsub();
		};
	}

	private void _E000()
	{
		NightVisionComponent component = base.NightVisionObserver.Component;
		bool flag = component?.Togglable.On ?? false;
		if (PointOfView != 0)
		{
			TogglableComponent togglableComponent = base.NightVisionObserver.GetItemComponent()?.Togglable ?? base.ThermalVisionObserver.GetItemComponent()?.Togglable;
			if (togglableComponent == null || togglableComponent.On != flag)
			{
				togglableComponent?.ToggleSilent();
			}
		}
		else
		{
			if (base.ThermalVisionObserver.Component == null && ((component != _E103 && _E103.Togglable.On != flag) || (_E107 != _E103 && _E107 != null && _E103.Togglable.On)))
			{
				_E103.Togglable.ToggleSilent();
			}
			_E107 = component;
		}
	}

	private void _E001()
	{
		if (PointOfView != 0)
		{
			return;
		}
		ThermalVisionComponent component = base.ThermalVisionObserver.Component;
		bool flag = component?.Togglable.On ?? false;
		if (_E103.Togglable.On != flag)
		{
			if (component == null)
			{
				_E103.Togglable.Toggle();
			}
			else
			{
				_E103.Togglable.ToggleSilent();
			}
		}
		base.NightVisionObserver.Update();
	}

	public void ToggleNightVision()
	{
		ITogglableComponentContainer togglableComponentContainer = base.ThermalVisionObserver?.Component;
		(togglableComponentContainer ?? base.NightVisionObserver?.Component)?.Togglable.Toggle();
	}

	public override void OnGameSessionEnd(ExitStatus exitStatus, float pastTime, string locationId, string exitName)
	{
		_E106?.Invoke();
		base.MovementContext.PhysicalConditionChanged -= base.ProceduralWeaponAnimation.PhysicalConditionUpdated;
		_healthController.DiedEvent -= OnDead;
		OriginalInventory.UnregisterView(this);
		ExfilUnsubscribe();
		base.NightVisionObserver.Dispose();
		base.ThermalVisionObserver.Dispose();
		base.FaceShieldObserver.Dispose();
		base.FaceCoverObserver.Dispose();
		base.OnGameSessionEnd(exitStatus, pastTime, locationId, exitName);
	}

	public async Task UpdateHideoutPlayerInventory()
	{
		if (IsUpdateHideoutPlayerInventoryInProgress)
		{
			return;
		}
		_E104 = true;
		while (base.MovementContext.BlockFirearms && _E104)
		{
			await Task.Yield();
		}
		IsUpdateHideoutPlayerInventoryInProgress = true;
		if (!_E104)
		{
			IsUpdateHideoutPlayerInventoryInProgress = false;
			return;
		}
		Item item = null;
		using (_E069.StartWithToken(_ED3E._E000(192742)))
		{
			using (_E069.StartWithToken(_ED3E._E000(192779)))
			{
				await _E003(base.Profile, _ECE3.Low);
			}
			_E105.Inventory.FastAccess.BoundItems.Clear();
			IEnumerable<Slot> allSlots = _E105.Inventory.Equipment.GetAllSlots();
			IEnumerable<Slot> allSlots2 = OriginalInventory.Inventory.Equipment.GetAllSlots();
			Item itemInHands = _E105.ItemInHands;
			foreach (Slot playerSlot in allSlots)
			{
				Slot slot2 = allSlots2.First((Slot slot) => slot.ID == playerSlot.ID);
				if (slot2.ContainedItem != null)
				{
					_EB20 to = new _EB20(playerSlot);
					Item item2 = slot2.ContainedItem.CloneItem();
					if (playerSlot.ContainedItem == itemInHands)
					{
						item = item2;
					}
					if (item2 is _EA91 obj2)
					{
						obj2.UncoverAll(base.ProfileId);
					}
					_ECD8<_EB37> obj3 = _EB29.AddWithoutRestrictions(item2, to, _E105);
					if (obj3.Failed)
					{
						UnityEngine.Debug.LogError(obj3.Error);
					}
				}
			}
		}
		await Task.Yield();
		if (item != null)
		{
			await _E002();
			SetItemInHands(item, delegate(Result<_E6C7> result)
			{
				if (result.Failed)
				{
					UnityEngine.Debug.LogError(result.Error);
				}
				IsUpdateHideoutPlayerInventoryInProgress = false;
			});
			return;
		}
		_EB0B equipment = _E105.Inventory.Equipment;
		Item item3 = equipment.GetSlot(EquipmentSlot.FirstPrimaryWeapon).ContainedItem ?? equipment.GetSlot(EquipmentSlot.SecondPrimaryWeapon).ContainedItem ?? equipment.GetSlot(EquipmentSlot.Holster).ContainedItem;
		if (item3 != null)
		{
			await _E002();
			SetItemInHands(item3, delegate(Result<_E6C7> result)
			{
				if (result.Failed)
				{
					UnityEngine.Debug.LogError(result.Error);
				}
				IsUpdateHideoutPlayerInventoryInProgress = false;
			});
		}
		else
		{
			IsUpdateHideoutPlayerInventoryInProgress = false;
		}
	}

	public async Task ReleaseShootingRangeInventory()
	{
		while (IsUpdateHideoutPlayerInventoryInProgress)
		{
			await Task.Yield();
		}
		IsUpdateHideoutPlayerInventoryInProgress = true;
		(HandsController as _E6CB)?.SetTriggerPressed(pressed: false);
		FastForwardCurrentOperations();
		if (!(HandsController is EmptyHandsController))
		{
			bool handsAreEmpty = false;
			await _E002();
			SetEmptyHands(delegate
			{
				handsAreEmpty = true;
			});
			if (!IsInPatrol)
			{
				while (!handsAreEmpty)
				{
					if (PointOfView != 0)
					{
						FastForwardCurrentOperations();
					}
					await Task.Yield();
				}
			}
			else
			{
				FastForwardCurrentOperations();
			}
		}
		foreach (Slot allSlot in _E105.Inventory.Equipment.GetAllSlots())
		{
			if (allSlot.ContainedItem != null)
			{
				_ECD8<_EB3A> obj = _EB29.Remove(allSlot.ContainedItem, _E105);
				if (obj.Failed)
				{
					UnityEngine.Debug.LogError(obj.Error);
				}
			}
		}
		Singleton<GameWorld>.Instance.DestroyAllLoot();
		base.MovementContext.RestorePreviousPatrol();
		IsUpdateHideoutPlayerInventoryInProgress = false;
		_E104 = false;
	}

	public void SetPatrol(bool patrol)
	{
		IsInPatrol = patrol || !_E104;
		(HandsController as _E6CB)?.SetTriggerPressed(pressed: false);
		base.MovementContext.BlockFirearms = IsInPatrol;
		if (IsInPatrol && HandsController is _E6CB obj)
		{
			obj.SetAim(value: false);
		}
	}

	private async Task _E002()
	{
		if (IsInPatrol)
		{
			HandsController.FirearmsAnimator.SetPatrol(b: false);
			await Task.Yield();
			FastForwardCurrentOperations();
		}
	}

	private static async Task _E003(Profile profile, _ECE1 yield)
	{
		ResourceKey[] resources = profile.GetAllPrefabPaths().ToArray();
		await Singleton<_E760>.Instance.LoadBundlesAndCreatePools(_E760.PoolsCategory.Raid, _E760.AssemblyType.Local, resources, yield);
	}

	public override void ExecuteSkill(Action action)
	{
	}

	public override void ExecuteShotSkill(Item weapon)
	{
	}

	protected override void OnSkillLevelChanged(_E74E skill)
	{
	}

	protected override void OnWeaponMastered(_E750 masterSkill)
	{
	}

	[Conditional("DEBUG")]
	[Conditional("CONSOLE")]
	private async void _E004()
	{
		for (int i = 0; i < 220; i++)
		{
			if (!IsUpdateHideoutPlayerInventoryInProgress)
			{
				return;
			}
			await Task.Yield();
		}
		UnityEngine.Debug.LogError(_ED3E._E000(192816));
	}

	[CompilerGenerated]
	private void _E005(Result<_E6C7> result)
	{
		if (result.Failed)
		{
			UnityEngine.Debug.LogError(result.Error);
		}
		IsUpdateHideoutPlayerInventoryInProgress = false;
	}

	[CompilerGenerated]
	private void _E006(Result<_E6C7> result)
	{
		if (result.Failed)
		{
			UnityEngine.Debug.LogError(result.Error);
		}
		IsUpdateHideoutPlayerInventoryInProgress = false;
	}
}
