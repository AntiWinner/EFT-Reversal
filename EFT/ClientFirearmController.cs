using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.InventoryLogic;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT;

internal sealed class ClientFirearmController : Player.FirearmController
{
	public new class _E000 : _E01F
	{
		private readonly ClientFirearmController _E08D;

		public _E000(Player.FirearmController controller)
			: base(controller)
		{
			_E08D = controller as ClientFirearmController;
		}

		public override void SetTriggerPressed(bool pressed)
		{
			bool flag = _E06A;
			base.SetTriggerPressed(pressed);
			if (_E06A && !flag)
			{
				_E08D._E005(_E06B);
			}
		}

		protected override void SwitchToIdle()
		{
			_E08D._E006(_E06B);
			_E002();
			base.SwitchToIdle();
		}
	}

	public new class _E001 : _E023
	{
		private readonly ClientFirearmController _E08D;

		public _E001(Player.FirearmController controller)
			: base(controller)
		{
			_E08D = controller as ClientFirearmController;
		}

		public override void SetTriggerPressed(bool pressed)
		{
			bool flag = _E06A;
			base.SetTriggerPressed(pressed);
			if (_E06A && !flag)
			{
				_E08D._E005(_E06B);
			}
		}

		protected override void SwitchToIdle()
		{
			_E08D._E006(_E06B);
			base.SwitchToIdle();
		}
	}

	public new class _E002 : _E024
	{
		private readonly ClientFirearmController _E08D;

		public _E002(Player.FirearmController controller)
			: base(controller)
		{
			_E08D = controller as ClientFirearmController;
		}

		public override void SetTriggerPressed(bool pressed)
		{
			bool flag = _E06A;
			base.SetTriggerPressed(pressed);
			if (_E06A && !flag)
			{
				_E08D._E005(_E06B);
			}
		}

		protected override void SwitchToIdle()
		{
			_E08D._E006(_E06B);
			base.SwitchToIdle();
		}
	}

	[CompilerGenerated]
	private new sealed class _E003
	{
		public _EB22 gridItemAddress;

		public ClientFirearmController _003C_003E4__this;

		public _EA6A magazine;

		internal void _E000(IResult error)
		{
			_E677 obj = ((gridItemAddress == null) ? null : _E69E.FromGridItemAddress(gridItemAddress));
			using MemoryStream memoryStream = new MemoryStream();
			using BinaryWriter writer = new BinaryWriter(memoryStream);
			byte[] array = null;
			if (obj != null)
			{
				writer.Write(obj);
				array = memoryStream.ToArray();
			}
			else
			{
				array = new byte[0];
			}
			_003C_003E4__this.FirearmPacket.ReloadMagPacket = new _E6EF
			{
				Reload = true,
				MagId = magazine.Id,
				LocationDescription = array
			};
		}
	}

	[CompilerGenerated]
	private new sealed class _E004
	{
		public ClientFirearmController _003C_003E4__this;

		public bool quickReload;

		public string[] ammoIds;

		public _EB13 cylinderMagazine;

		internal void _E000(IResult error)
		{
			_003C_003E4__this.FirearmPacket.ReloadWithAmmoPacket = new _E6F4
			{
				Reload = true,
				FastReload = quickReload,
				ReloadWithAmmoStatus = _E6F4.EReloadWithAmmoStatus.StartReload,
				AmmoIds = ammoIds
			};
			_003C_003E4__this.FirearmPacket.CylinderMagStatusPacket = new _E6F3
			{
				StatusChanged = true,
				CamoraIndex = cylinderMagazine.CurrentCamoraIndex,
				HammerClosed = _003C_003E4__this.Item.CylinderHammerClosed
			};
		}
	}

	[CompilerGenerated]
	private new sealed class _E005
	{
		public _EB22 placeToPutContainedAmmoMagazine;

		public ClientFirearmController _003C_003E4__this;

		public _E9CF ammoPack;

		internal void _E000(IResult error)
		{
			_E677 obj = ((placeToPutContainedAmmoMagazine == null) ? null : _E69E.FromGridItemAddress(placeToPutContainedAmmoMagazine));
			using MemoryStream memoryStream = new MemoryStream();
			using BinaryWriter writer = new BinaryWriter(memoryStream);
			byte[] array = null;
			if (obj != null)
			{
				writer.Write(obj);
				array = memoryStream.ToArray();
			}
			else
			{
				array = new byte[0];
			}
			_003C_003E4__this.FirearmPacket.ReloadBarrelsPacket = new _E6F1
			{
				Reload = true,
				AmmoIds = ammoPack.GetReloadingAmmoIds(),
				LocationDescription = array
			};
		}
	}

	[CompilerGenerated]
	private sealed class _E006
	{
		public ClientFirearmController _003C_003E4__this;

		public string[] ammoIds;

		internal void _E000(IResult error)
		{
			_003C_003E4__this.FirearmPacket.ReloadWithAmmoPacket = new _E6F4
			{
				Reload = true,
				ReloadWithAmmoStatus = _E6F4.EReloadWithAmmoStatus.StartReload,
				AmmoIds = ammoIds
			};
		}
	}

	[CompilerGenerated]
	private sealed class _E007
	{
		public ClientFirearmController _003C_003E4__this;

		public _EB13 cylinderMagazine;

		internal void _E000(IResult finishCallback)
		{
			_003C_003E4__this.FirearmPacket.CylinderMagStatusPacket = new _E6F3
			{
				StatusChanged = true,
				CamoraIndex = cylinderMagazine.CurrentCamoraIndex,
				HammerClosed = _003C_003E4__this.Item.CylinderHammerClosed
			};
		}
	}

	private const int _E056 = 12;

	public _E6EE FirearmPacket;

	private readonly List<_EC26> _E057 = new List<_EC26>(20);

	private readonly List<_EC26> _E058 = new List<_EC26>(20);

	private readonly _EC26[] _E059 = new _EC26[64];

	private byte _E05A;

	private EShotType _E05B;

	protected override Dictionary<Type, OperationFactoryDelegate> GetOperationFactoryDelegates()
	{
		Dictionary<Type, OperationFactoryDelegate> operationFactoryDelegates = base.GetOperationFactoryDelegates();
		operationFactoryDelegates[typeof(_E022)] = delegate
		{
			if (base.Item.ReloadMode == Weapon.EReloadMode.InternalMagazine && base.Item.Chambers.Length == 0)
			{
				return new _E001(this);
			}
			return base.Item.MustBoltBeOpennedForInternalReload ? ((_E022)new _E002(this)) : ((_E022)new _E001(this));
		};
		operationFactoryDelegates[typeof(_E01F)] = () => new _E000(this);
		return operationFactoryDelegates;
	}

	internal static ClientFirearmController _E000(ClientPlayer player, Weapon weapon)
	{
		return Player.FirearmController._E000<ClientFirearmController>(player, weapon);
	}

	internal new static Task<ClientFirearmController> _E001(ClientPlayer player, string itemId, bool isStationaryWeapon)
	{
		Item item = (string.IsNullOrEmpty(itemId) ? null : (isStationaryWeapon ? Singleton<GameWorld>.Instance.FindStationaryWeaponByItemId(itemId).Item : player._E0DE.FindItem(itemId)));
		if (item == null)
		{
			throw new Exception(_ED3E._E000(192615));
		}
		return Player.FirearmController._E001<ClientFirearmController>(player, (Weapon)item);
	}

	protected override void InitBallisticCalculator()
	{
		BallisticsCalculator = Singleton<GameWorld>.Instance.CreateClientBallisticsCalculator();
	}

	protected override void RemoveBallisticCalculator()
	{
		Singleton<GameWorld>.Instance.RemoveClientBallisticsCalculator();
	}

	public override bool ChangeFireMode(Weapon.EFireMode fireMode)
	{
		bool num = base.ChangeFireMode(fireMode);
		if (num)
		{
			FirearmPacket.ChangeFireMode = new _E6E8
			{
				ChangeFireMode = true,
				FireMode = fireMode
			};
			_E003(base.Item);
		}
		return num;
	}

	public override void ChangeAimingMode()
	{
		base.ChangeAimingMode();
		FirearmPacket.ToggleAim = true;
		FirearmPacket.AimingIndex = (IsAiming ? base.Item.AimIndex.Value : (-1));
	}

	protected override void CompassStateHandler(bool isActive)
	{
		FirearmPacket.CompassPacket = new _E94A(isActive);
		base.CompassStateHandler(isActive);
	}

	public override void SetInventoryOpened(bool opened)
	{
		FirearmPacket.EnableInventoryPacket = new _E94F
		{
			EnableInventory = true,
			InventoryStatus = opened
		};
		base.SetInventoryOpened(opened);
	}

	public override void SetAim(bool value)
	{
		bool isAiming = IsAiming;
		bool aimingInterruptedByOverlap = AimingInterruptedByOverlap;
		base.SetAim(value);
		if (IsAiming != isAiming || aimingInterruptedByOverlap)
		{
			FirearmPacket.ToggleAim = true;
			FirearmPacket.AimingIndex = (IsAiming ? base.Item.AimIndex.Value : (-1));
		}
	}

	public override void ReloadMag(_EA6A magazine, [CanBeNull] _EB22 gridItemAddress, [CanBeNull] Callback callback)
	{
		if (!CanStartReload())
		{
			return;
		}
		base.CurrentOperation.ReloadMag(magazine, gridItemAddress, callback, delegate
		{
			_E677 obj = ((gridItemAddress == null) ? null : _E69E.FromGridItemAddress(gridItemAddress));
			using MemoryStream memoryStream = new MemoryStream();
			using BinaryWriter writer = new BinaryWriter(memoryStream);
			byte[] array = null;
			if (obj != null)
			{
				writer.Write(obj);
				array = memoryStream.ToArray();
			}
			else
			{
				array = new byte[0];
			}
			FirearmPacket.ReloadMagPacket = new _E6EF
			{
				Reload = true,
				MagId = magazine.Id,
				LocationDescription = array
			};
		});
	}

	public override void QuickReloadMag(_EA6A magazine, Callback callback)
	{
		if (CanStartReload())
		{
			base.QuickReloadMag(magazine, callback);
			FirearmPacket.QuickReloadMagPacket = new _E6F0
			{
				Reload = true,
				MagId = magazine.Id
			};
		}
	}

	public override void ReloadRevolverDrum(_E9CF ammoPack, Callback callback, bool quickReload = false)
	{
		if (base.Item.GetCurrentMagazine() != null && CanStartReload())
		{
			string[] ammoIds = ammoPack.GetReloadingAmmoIds();
			_EB13 cylinderMagazine = base.Item.GetCurrentMagazine() as _EB13;
			base.CurrentOperation.ReloadRevolverDrum(ammoPack, callback, delegate
			{
				FirearmPacket.ReloadWithAmmoPacket = new _E6F4
				{
					Reload = true,
					FastReload = quickReload,
					ReloadWithAmmoStatus = _E6F4.EReloadWithAmmoStatus.StartReload,
					AmmoIds = ammoIds
				};
				FirearmPacket.CylinderMagStatusPacket = new _E6F3
				{
					StatusChanged = true,
					CamoraIndex = cylinderMagazine.CurrentCamoraIndex,
					HammerClosed = base.Item.CylinderHammerClosed
				};
			}, quickReload);
		}
	}

	public override void ReloadBarrels(_E9CF ammoPack, _EB22 placeToPutContainedAmmoMagazine, Callback callback)
	{
		if (!CanStartReload())
		{
			return;
		}
		if (CanStartReload() && ammoPack.AmmoCount > 0)
		{
			base.CurrentOperation.ReloadBarrels(ammoPack, placeToPutContainedAmmoMagazine, callback, delegate
			{
				_E677 obj = ((placeToPutContainedAmmoMagazine == null) ? null : _E69E.FromGridItemAddress(placeToPutContainedAmmoMagazine));
				using MemoryStream memoryStream = new MemoryStream();
				using BinaryWriter writer = new BinaryWriter(memoryStream);
				byte[] array = null;
				if (obj != null)
				{
					writer.Write(obj);
					array = memoryStream.ToArray();
				}
				else
				{
					array = new byte[0];
				}
				FirearmPacket.ReloadBarrelsPacket = new _E6F1
				{
					Reload = true,
					AmmoIds = ammoPack.GetReloadingAmmoIds(),
					LocationDescription = array
				};
			});
		}
		else
		{
			callback?.Fail(_ED3E._E000(163838));
		}
	}

	public override void ReloadWithAmmo(_E9CF ammoPack, [CanBeNull] Callback callback)
	{
		if (base.Item.GetCurrentMagazine() != null && CanStartReload())
		{
			string[] ammoIds = ammoPack.GetReloadingAmmoIds();
			base.CurrentOperation.ReloadWithAmmo(ammoPack, callback, delegate
			{
				FirearmPacket.ReloadWithAmmoPacket = new _E6F4
				{
					Reload = true,
					ReloadWithAmmoStatus = _E6F4.EReloadWithAmmoStatus.StartReload,
					AmmoIds = ammoIds
				};
			});
		}
	}

	public override void ReloadGrenadeLauncher(_E9CF ammoPack, [CanBeNull] Callback callback)
	{
		if (CanStartReload())
		{
			string[] reloadingAmmoIds = ammoPack.GetReloadingAmmoIds();
			FirearmPacket.LauncherReloadInfo = new _E95A
			{
				Reload = true,
				AmmoIds = reloadingAmmoIds
			};
			base.CurrentOperation.ReloadGrenadeLauncher(ammoPack, callback);
		}
	}

	public override bool ExamineWeapon()
	{
		bool num = base.ExamineWeapon();
		if (num)
		{
			FirearmPacket.ExamineWeapon = true;
		}
		return num;
	}

	public override void RollCylinder(bool rollToZeroCamora)
	{
		if (!IsAiming && base.CurrentOperation is _E011 && !_player._E0DE.HasAnyHandsAction() && base.Item is _EAD1)
		{
			FirearmPacket.RollCylinderPacket = new _E6F2
			{
				RollCylinder = true,
				RollToZeroCamora = rollToZeroCamora
			};
			_EB13 cylinderMagazine = base.Item.GetCurrentMagazine() as _EB13;
			base.CurrentOperation.RollCylinder(delegate
			{
				FirearmPacket.CylinderMagStatusPacket = new _E6F3
				{
					StatusChanged = true,
					CamoraIndex = cylinderMagazine.CurrentCamoraIndex,
					HammerClosed = base.Item.CylinderHammerClosed
				};
			}, rollToZeroCamora);
		}
	}

	public override void ShowGesture(EGesture gesture)
	{
		FirearmPacket.Gesture = gesture;
		base.ShowGesture(gesture);
	}

	public override bool CheckAmmo()
	{
		bool num = base.CheckAmmo();
		if (num)
		{
			FirearmPacket.CheckAmmo = true;
		}
		return num;
	}

	public override bool CheckChamber()
	{
		bool num = base.CheckChamber();
		if (num)
		{
			FirearmPacket.CheckChamber = true;
		}
		return num;
	}

	public override bool CheckFireMode()
	{
		bool num = base.CheckFireMode();
		if (num)
		{
			FirearmPacket.CheckFireMode = true;
		}
		return num;
	}

	public override void SetLightsState(_E6C5[] lightsStates, bool force = false)
	{
		if (force || base.CurrentOperation.CanChangeLightState(lightsStates))
		{
			_E6E9 obj = default(_E6E9);
			obj.ToggleTacticalCombo = true;
			obj.TacticalComboStatuses = new _E6EA[lightsStates.Length];
			_E6E9 toggleTacticalCombo = obj;
			for (int i = 0; i < lightsStates.Length; i++)
			{
				_E6C5 obj2 = lightsStates[i];
				toggleTacticalCombo.TacticalComboStatuses[i] = new _E6EA
				{
					Id = obj2.Id,
					IsActive = obj2.IsActive,
					SelectedMode = obj2.LightMode
				};
			}
			FirearmPacket.ToggleTacticalCombo = toggleTacticalCombo;
		}
		base.SetLightsState(lightsStates, force);
	}

	public override void SetScopeMode(_E6C6[] scopeStates)
	{
		_E002(scopeStates);
		base.SetScopeMode(scopeStates);
	}

	public override void UnderbarrelSightingRangeUp()
	{
		base.UnderbarrelSightingRangeUp();
		_E6EB obj = default(_E6EB);
		obj.ChangeLauncherRange = true;
		obj.RangeIndex = UnderbarrelWeapon.RangeIndex;
		_E6EB launcherRangeStatePacket = obj;
		FirearmPacket.LauncherRangeStatePacket = launcherRangeStatePacket;
	}

	public override void UnderbarrelSightingRangeDown()
	{
		base.UnderbarrelSightingRangeDown();
		_E6EB obj = default(_E6EB);
		obj.ChangeLauncherRange = true;
		obj.RangeIndex = UnderbarrelWeapon.RangeIndex;
		_E6EB launcherRangeStatePacket = obj;
		FirearmPacket.LauncherRangeStatePacket = launcherRangeStatePacket;
	}

	public override void OpticCalibrationSwitchUp(_E6C6[] scopeStates)
	{
		_E002(scopeStates);
		base.OpticCalibrationSwitchUp(scopeStates);
	}

	public override void OpticCalibrationSwitchDown(_E6C6[] scopeStates)
	{
		_E002(scopeStates);
		base.OpticCalibrationSwitchDown(scopeStates);
	}

	private void _E002(_E6C6[] scopeStates)
	{
		if (base.CurrentOperation.CanChangeScopeStates(scopeStates))
		{
			_E6EC obj = default(_E6EC);
			obj.ChangeSightMode = true;
			obj.SightModeStatuses = new _E6ED[scopeStates.Length];
			_E6EC changeSightsMode = obj;
			for (int i = 0; i < scopeStates.Length; i++)
			{
				_E6C6 obj2 = scopeStates[i];
				changeSightsMode.SightModeStatuses[i] = new _E6ED
				{
					Id = obj2.Id,
					SelectedMode = obj2.ScopeMode,
					ScopeIndexInsideSight = obj2.ScopeIndexInsideSight,
					ScopeCalibrationIndex = obj2.ScopeCalibrationIndex
				};
			}
			FirearmPacket.ChangeSightsMode = changeSightsMode;
		}
	}

	public override bool ToggleLauncher()
	{
		bool num = base.ToggleLauncher();
		if (num)
		{
			FirearmPacket.ToggleLauncher = true;
		}
		return num;
	}

	protected override void InitiateShot(IWeapon weapon, _EA12 ammo, Vector3 shotPosition, Vector3 shotDirection, Vector3 fireportPosition, int chamberIndex, float overheat)
	{
		_E05A++;
		if (_E05A == 0)
		{
			_E05A++;
		}
		switch (weapon.MalfState.State)
		{
		case Weapon.EMalfunctionState.None:
			_E05B = EShotType.RegularShot;
			break;
		case Weapon.EMalfunctionState.SoftSlide:
			_E05B = EShotType.SoftSlidedShot;
			break;
		case Weapon.EMalfunctionState.HardSlide:
			_E05B = EShotType.HardSlidedShot;
			break;
		case Weapon.EMalfunctionState.Feed:
			_E05B = EShotType.Feed;
			break;
		case Weapon.EMalfunctionState.Misfire:
			_E05B = EShotType.Misfire;
			break;
		case Weapon.EMalfunctionState.Jam:
			_E05B = EShotType.JamedShot;
			break;
		}
		FirearmPacket.Add(new _E6E7
		{
			IsPrimaryActive = (weapon == base.Item),
			EShotType = _E05B,
			AmmoAfterShot = weapon.GetCurrentMagazineCount(),
			ShotPosition = shotPosition,
			ShotDirection = shotDirection,
			FireportPosition = fireportPosition,
			ChamberIndex = chamberIndex,
			Overheat = overheat,
			UnderbarrelShot = weapon.IsUnderbarrelWeapon
		});
		base.InitiateShot(weapon, ammo, shotPosition, shotDirection, fireportPosition, chamberIndex, overheat);
	}

	private void _E003(Weapon weapon)
	{
		if (weapon is _EAD1 && weapon.GetCurrentMagazine() is _EB13 obj)
		{
			FirearmPacket.CylinderMagStatusPacket = new _E6F3
			{
				StatusChanged = true,
				CamoraIndex = obj.CurrentCamoraIndex,
				HammerClosed = weapon.CylinderHammerClosed
			};
		}
	}

	protected override void SendStartOneShotFire()
	{
		FirearmPacket.FlareShotInfo.StartOneShotFire = true;
		FirearmPacket.FlareShotInfo.WasShot = false;
	}

	protected override void CreateFlareShot(_EA12 ammo, Vector3 shotPosition, Vector3 shotForward)
	{
		FirearmPacket.FlareShotInfo.WasShot = true;
		FirearmPacket.FlareShotInfo.ShotPosition = shotPosition;
		FirearmPacket.FlareShotInfo.ShotForward = shotForward;
		base.CreateFlareShot(ammo, shotPosition, shotForward);
	}

	protected override void ShotMisfired(_EA12 ammo, Weapon.EMalfunctionState malfunctionState, float overheat)
	{
		_E05A++;
		if (_E05A == 0)
		{
			_E05A++;
		}
		switch (malfunctionState)
		{
		case Weapon.EMalfunctionState.SoftSlide:
			_E05B = EShotType.SoftSlidedShot;
			break;
		case Weapon.EMalfunctionState.HardSlide:
			_E05B = EShotType.HardSlidedShot;
			break;
		case Weapon.EMalfunctionState.Feed:
			_E05B = EShotType.Feed;
			break;
		case Weapon.EMalfunctionState.Misfire:
			_E05B = EShotType.Misfire;
			break;
		case Weapon.EMalfunctionState.Jam:
			_E05B = EShotType.JamedShot;
			break;
		default:
			_E05B = EShotType.RegularShot;
			break;
		}
		FirearmPacket.Add(new _E6E7
		{
			IsPrimaryActive = true,
			EShotType = _E05B,
			AmmoAfterShot = base.Item.GetCurrentMagazineCount(),
			Overheat = overheat
		});
	}

	protected internal override void DryShot(int chamberIndex = 0, bool underbarrelShot = false)
	{
		base.DryShot();
		_E05A++;
		if (_E05A == 0)
		{
			_E05A++;
		}
		_E05B = EShotType.DryFire;
		FirearmPacket.Add(new _E6E7
		{
			IsPrimaryActive = true,
			EShotType = _E05B,
			AmmoAfterShot = ((!underbarrelShot) ? base.Item.GetCurrentMagazineCount() : 0),
			ChamberIndex = chamberIndex,
			UnderbarrelShot = underbarrelShot
		});
		_E003(base.Item);
	}

	public override void BallisticUpdate(float deltaTime)
	{
		BallisticsCalculator.ManualUpdate(deltaTime);
		_E004();
	}

	protected override void RegisterShot(Item weapon, _EC26 shot)
	{
		base.RegisterShot(weapon, shot);
		_E057.Add(shot);
	}

	private void _E004()
	{
		for (int i = 0; i < _E057.Count; i++)
		{
			_EC26 obj = _E057[i];
			if (obj.IsShotFinished)
			{
				_E058.Add(obj);
			}
		}
		int count = _E058.Count;
		for (int j = 0; j < count; j++)
		{
			_EC26 obj2 = _E058[j];
			_E057.Remove(obj2);
			int allFragmentsRecursively = obj2.GetAllFragmentsRecursively(_E059);
			for (int k = 0; k < allFragmentsRecursively; k++)
			{
				_EC26 obj3 = _E059[k];
				if (obj2 != obj3)
				{
					if (obj3.IsShotFinished)
					{
						_E058.Add(obj3);
					}
					else
					{
						_E057.Add(obj3);
					}
				}
			}
		}
		if (_E058.Count <= 0)
		{
			return;
		}
		_E95C<_E95F> shotsForApprovement = default(_E95C<_E95F>);
		for (int l = 0; l < _E058.Count; l++)
		{
			_EC26 obj4 = _E058[l];
			_E95F shotInfo = obj4.GetShotInfo();
			if (shotInfo.HasHitSomething && obj4.IsForwardHit && !obj4.AvoidAdditionalDamage && (shotInfo.HittedId != _player.OwnerId || obj4.FragmentIndex > 0) && shotsForApprovement.Length < 12)
			{
				shotsForApprovement.Add(shotInfo);
			}
		}
		FirearmPacket.ShotsForApprovement = shotsForApprovement;
		_E058.Clear();
	}

	private void _E005(int ammoToLoadIntoMag)
	{
		FirearmPacket.ReloadWithAmmoPacket = new _E6F4
		{
			Reload = true,
			ReloadWithAmmoStatus = _E6F4.EReloadWithAmmoStatus.AbortReload,
			AmmoLoadedToMag = ammoToLoadIntoMag
		};
	}

	private void _E006(int ammoToLoadIntoMag)
	{
		FirearmPacket.ReloadWithAmmoPacket = new _E6F4
		{
			Reload = true,
			ReloadWithAmmoStatus = _E6F4.EReloadWithAmmoStatus.EndReload,
			AmmoLoadedToMag = ammoToLoadIntoMag
		};
	}

	public override bool CanStartReload()
	{
		if (((ClientPlayer)_player).IsWaitingForNetworkCallback)
		{
			return false;
		}
		return base.CanStartReload();
	}

	public override bool CanPressTrigger()
	{
		if (((ClientPlayer)_player).IsWaitingForNetworkCallback)
		{
			return false;
		}
		return base.CanPressTrigger();
	}

	protected override void LightAndSoundShot(Vector3 point, Vector3 direction, AmmoTemplate ammoTemplate)
	{
	}

	[CompilerGenerated]
	private Player._E00E _E007()
	{
		if (base.Item.ReloadMode == Weapon.EReloadMode.InternalMagazine && base.Item.Chambers.Length == 0)
		{
			return new _E001(this);
		}
		if (base.Item.MustBoltBeOpennedForInternalReload)
		{
			return new _E002(this);
		}
		return new _E001(this);
	}

	[CompilerGenerated]
	private Player._E00E _E008()
	{
		return new _E000(this);
	}
}
