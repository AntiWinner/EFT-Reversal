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

internal sealed class ObservedFirearmController : Player.FirearmController, ObservedPlayer._E004
{
	private new class _E000 : Player.FirearmController._E002
	{
		private _E00D _E083;

		private readonly ObservedFirearmController _E084;

		private _E6EF _E085;

		private _E6F0 _E086;

		private _E6F4 _E087;

		private float _E088;

		private new int _E000
		{
			get
			{
				ObservedFirearmController observedFirearmController = (ObservedFirearmController)((Player.FirearmController._E00E)this)._E002;
				FireModeComponent fireMode = ((Player.FirearmController._E00E)this)._E002.Item.FireMode;
				int result = observedFirearmController.ShotsInBurst;
				if (fireMode.FireMode == Weapon.EFireMode.burst)
				{
					observedFirearmController.ShotsInBurst++;
					if (Time.time - _E088 > 1f)
					{
						result = 0;
						observedFirearmController.ShotsInBurst = 1;
					}
					else if (observedFirearmController.ShotsInBurst == fireMode.BurstShotsCount)
					{
						observedFirearmController.ShotsInBurst = 0;
					}
					_E088 = Time.time;
				}
				else
				{
					observedFirearmController.ShotsInBurst = 0;
				}
				return result;
			}
		}

		public _E000(Player.FirearmController controller)
			: base(controller)
		{
			_E084 = controller as ObservedFirearmController;
		}

		public override void FastForward()
		{
			if (_E020 >= 0f && _E020 < 1f)
			{
				_E000(_E020, 1f);
			}
			int num = 100;
			while (_E084._E051.Count > 0 && --num > 0)
			{
				_E000(0f, 1f);
			}
			if (num <= 0)
			{
				Debug.LogError(_ED3E._E000(190385));
			}
			if (State != Player.EOperationState.Finished)
			{
				base.FastForward();
			}
		}

		protected override void InternalOnFireEvent()
		{
			_E083 = _E084._E051.Dequeue();
			Weapon.EMalfunctionState malfunctionState = _E083.MalfunctionState;
			_E01D = _E002(malfunctionState);
			_E084._E053 = _E083.ShotDirection;
			_E084._E052 = _E083.FireportPosition;
			_E039.MalfState.State = malfunctionState;
			if (malfunctionState != 0)
			{
				_E039.MalfState.LastMalfunctionTime = _E62A.PastTime;
				_E037.MisfireSlideUnknown(val: false);
				((Player.FirearmController._E00E)this)._E001._E0DE.ExamineMalfunction(_E039);
			}
			switch (malfunctionState)
			{
			case Weapon.EMalfunctionState.Misfire:
				((Player.FirearmController._E00E)this)._E002.Malfunction = true;
				_E003();
				return;
			case Weapon.EMalfunctionState.None:
				((Player.FirearmController._E00E)this)._E002.Malfunction = false;
				_E037.SetFire(((Player.FirearmController._E00E)this)._E002.IsTriggerPressed);
				break;
			default:
				((Player.FirearmController._E00E)this)._E002.Malfunction = true;
				_E037.Malfunction((int)malfunctionState);
				break;
			}
			FireModeComponent fireMode = ((Player.FirearmController._E00E)this)._E002.Item.FireMode;
			((Player.FirearmController._E00E)this)._E002.IsBirstOf2Start = fireMode.FireMode == Weapon.EFireMode.burst && this._E000 == 0 && fireMode.BurstShotsCount == 2;
			MakeShot(_E01D.AmmoToFire);
			_E01A++;
			((Player.FirearmController._E00E)this)._E002.IsBirstOf2Start = false;
			if (_E039.HasChambers)
			{
				if (_E039.MalfState.State == Weapon.EMalfunctionState.Feed)
				{
					((Player.FirearmController._E00E)this)._E002._E00C.SetRoundIntoWeapon(_E01D.FedAmmo);
					((Player.FirearmController._E00E)this)._E002._E00C.MoveAmmoFromChamberToShellPort(ammoIsUsed: false);
				}
				else
				{
					((Player.FirearmController._E00E)this)._E002._E00C.MoveAmmoFromChamberToShellPort(_E01D.AmmoToFire.IsUsed);
				}
			}
			if (_E039.MalfState.State == Weapon.EMalfunctionState.Jam || _E039.MalfState.State == Weapon.EMalfunctionState.SoftSlide || _E039.MalfState.State == Weapon.EMalfunctionState.HardSlide || _E039.MalfState.State == Weapon.EMalfunctionState.Feed)
			{
				_E037.SetAmmoOnMag(_E01D.AmmoCountInMagAfterShot);
				_E003();
			}
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);
			if (_E084._E00B())
			{
				FastForward();
				_E084.CurrentOperation.Update(0.01f);
			}
		}

		protected override void MakeShot(_EA12 ammo, int chamberIndex = 0, bool multiBarrelShot = false)
		{
			if (_E2B6.Config.UseSpiritPlayer && _E084._player.Spirit.IsActive)
			{
				_E084._player.Spirit.PlayerSync();
			}
			base.MakeShot(ammo, chamberIndex, multiBarrelShot);
		}

		protected override void InternalOnFireEndEvent()
		{
			if (_E084._E051.Count > 0)
			{
				((Player.FirearmController._E00E)this)._E002.IsTriggerPressed = true;
			}
			else if (_E084._E04E.Count > 0)
			{
				_E6EE obj = _E084._E04E.Peek();
				((Player.FirearmController._E00E)this)._E002.IsTriggerPressed = obj.HasShotsRealShots();
			}
			else
			{
				((Player.FirearmController._E00E)this)._E002.IsTriggerPressed = false;
			}
			if (!((Player.FirearmController._E00E)this)._E002.IsTriggerPressed)
			{
				_E001();
				if (_E085.Reload)
				{
					_E084._E005(_E085.MagId, _E085.LocationDescription, null);
				}
				else if (_E086.Reload)
				{
					_E084._E006(_E086.MagId, null);
				}
				else if (_E087.Reload)
				{
					_E084._E00A(_E087.AmmoIds, null);
				}
				return;
			}
			if (_E084._E051.Count <= 0)
			{
				_E6EE firearmPacket = _E084._E04E.Dequeue();
				_E000(firearmPacket);
				_E6E7? obj2 = firearmPacket.FiredShotInfos;
				while (obj2.HasValue)
				{
					_E6E7 value = obj2.Value;
					((Player.FirearmController._E00E)this)._E002.CurrentChamberIndex = value.ChamberIndex;
					Vector3 position = ((Player.FirearmController._E00E)this)._E002.CurrentFireport.position;
					Vector3 weaponDirection = ((Player.FirearmController._E00E)this)._E002.WeaponDirection;
					EShotType eShotType = value.EShotType;
					_E084._E003(position, weaponDirection, eShotType.ToWeaponMalfunctionState());
					obj2 = obj2.Value.GetNested();
				}
			}
			((Player.FirearmController._E00E)this)._E002._E001.SetFire(((Player.FirearmController._E00E)this)._E002.IsTriggerPressed);
		}

		private void _E000(_E6EE firearmPacket)
		{
			_E6EF reloadMagPacket = firearmPacket.ReloadMagPacket;
			if (reloadMagPacket.Reload)
			{
				_E085 = reloadMagPacket;
			}
			else
			{
				_E6F0 quickReloadMagPacket = firearmPacket.QuickReloadMagPacket;
				if (quickReloadMagPacket.Reload)
				{
					_E086 = quickReloadMagPacket;
				}
				else
				{
					_E6F4 reloadWithAmmoPacket = firearmPacket.ReloadWithAmmoPacket;
					if (reloadWithAmmoPacket.Reload)
					{
						_E087 = reloadWithAmmoPacket;
					}
				}
			}
			if (firearmPacket.LauncherReloadInfo.Reload)
			{
				string text = _ED3E._E000(190430);
				Debug.LogFormat(_ED3E._E000(190403), text);
			}
		}

		protected override void UncheckOnShot()
		{
		}
	}

	private new sealed class _E001 : Player.FirearmController._E00A
	{
		private _E00D _E01D;

		private readonly ObservedFirearmController _E084;

		private _E6F4 _E085;

		private _E6F3 _E089;

		public _E001(Player.FirearmController controller)
			: base(controller)
		{
			_E084 = controller as ObservedFirearmController;
		}

		protected override void PrepareShot()
		{
			_E01D = _E084._E051.Dequeue();
			_E084._E053 = _E01D.ShotDirection;
			_E084._E052 = _E01D.FireportPosition;
			if (_E01D.MalfunctionState != 0)
			{
				throw new NotImplementedException();
			}
			base.PrepareShot();
		}

		protected override void MakeShot(_EA12 ammo, int chamberIndex = 0, bool multiBarrelShot = false)
		{
			if (_E2B6.Config.UseSpiritPlayer && ((Player.FirearmController._E00E)this)._E001.Spirit.IsActive)
			{
				((Player.FirearmController._E00E)this)._E001.Spirit.PlayerSync();
			}
			base.MakeShot(ammo);
		}

		public override void Reset()
		{
			base.Reset();
			_E085 = default(_E6F4);
			_E089 = default(_E6F3);
		}

		public override void OnAddAmmoInChamber()
		{
			_E039.CylinderHammerClosed = _E039.FireMode.FireMode == Weapon.EFireMode.doubleaction;
			_E037.SetDoubleAction(Convert.ToSingle(_E039.CylinderHammerClosed));
		}

		protected override void UncheckOnShot()
		{
		}

		private void _E000(_E6EE firearmPacket)
		{
			_E6F4 reloadWithAmmoPacket = firearmPacket.ReloadWithAmmoPacket;
			if (reloadWithAmmoPacket.Reload)
			{
				_E085 = reloadWithAmmoPacket;
			}
			if (firearmPacket.CylinderMagStatusPacket.StatusChanged)
			{
				_E089 = firearmPacket.CylinderMagStatusPacket;
			}
		}

		public override void OnFireEndEvent()
		{
			if (_E02B)
			{
				_E037.SetDoubleAction(Convert.ToSingle(_E039.CylinderHammerClosed));
			}
			if (_E084._E051.Count > 0)
			{
				((Player.FirearmController._E00E)this)._E002.IsTriggerPressed = true;
			}
			else if (_E084._E04E.Count > 0)
			{
				((Player.FirearmController._E00E)this)._E002.IsTriggerPressed = _E084._E04E.Peek().HasShotsRealShots();
			}
			else
			{
				((Player.FirearmController._E00E)this)._E002.IsTriggerPressed = false;
			}
			if (!((Player.FirearmController._E00E)this)._E002.IsTriggerPressed)
			{
				_E001();
				if (_E089.StatusChanged)
				{
					_E084._E008(_E089.CamoraIndex, _E089.HammerClosed);
				}
				if (_E085.Reload)
				{
					_E084._E009(_E085.AmmoIds, _E085.FastReload, null);
				}
				return;
			}
			if (_E084._E051.Count <= 0)
			{
				_E6EE firearmPacket = _E084._E04E.Dequeue();
				_E000(firearmPacket);
				_E6E7? obj = firearmPacket.FiredShotInfos;
				while (obj.HasValue)
				{
					_E6E7 value = obj.Value;
					Vector3 shotPosition = value.ShotPosition;
					Vector3 shotDirection = value.ShotDirection;
					EShotType eShotType = value.EShotType;
					_E084._E003(shotPosition, shotDirection, eShotType.ToWeaponMalfunctionState());
					obj = obj.Value.GetNested();
				}
				PrepareShot();
			}
			_E037.SetFire(((Player.FirearmController._E00E)this)._E002.IsTriggerPressed);
		}
	}

	private new sealed class _E002 : Player.FirearmController._E00B
	{
		private _E00D _E01D;

		private readonly ObservedFirearmController _E084;

		private _E6F1 _E085;

		public _E002(Player.FirearmController controller)
			: base(controller)
		{
			_E084 = controller as ObservedFirearmController;
		}

		protected override void PrepareShot()
		{
			foreach (_E00D item in _E084._E051)
			{
				if (item.MalfunctionState != 0)
				{
					throw new NotImplementedException();
				}
			}
			base.PrepareShot();
		}

		protected override void MakeShot(_EA12 ammo, int chamberIndex = 0, bool multiBarrelShot = false)
		{
			if (_E2B6.Config.UseSpiritPlayer && ((Player.FirearmController._E00E)this)._E001.Spirit.IsActive)
			{
				((Player.FirearmController._E00E)this)._E001.Spirit.PlayerSync();
			}
			if (_E084._E051.Count == 0)
			{
				Debug.LogError(_ED3E._E000(190447));
				return;
			}
			_E01D = _E084._E051.Dequeue();
			_E084._E053 = _E01D.ShotDirection;
			_E084._E052 = _E01D.FireportPosition;
			base.MakeShot(ammo, chamberIndex, multiBarrelShot);
		}

		protected override void UncheckOnShot()
		{
		}

		private void _E000(_E6EE firearmPacket)
		{
			_E6F1 reloadBarrelsPacket = firearmPacket.ReloadBarrelsPacket;
			if (reloadBarrelsPacket.Reload)
			{
				_E085 = reloadBarrelsPacket;
			}
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);
			if (_E084._E00B())
			{
				FastForward();
				_E084.CurrentOperation.Update(0.01f);
			}
		}

		public override void OnFireEndEvent()
		{
			if (_E084._E051.Count > 0)
			{
				((Player.FirearmController._E00E)this)._E002.IsTriggerPressed = true;
			}
			else if (_E084._E04E.Count > 0)
			{
				((Player.FirearmController._E00E)this)._E002.IsTriggerPressed = _E084._E04E.Peek().HasShotsRealShots();
			}
			else
			{
				((Player.FirearmController._E00E)this)._E002.IsTriggerPressed = false;
			}
			if (!((Player.FirearmController._E00E)this)._E002.IsTriggerPressed)
			{
				_E037.Animator.Play(_E037.FullIdleStateName, 1, 0.1f);
				_E000();
				if (_E085.Reload)
				{
					_E084._E004(_E085.AmmoIds, _E085.LocationDescription);
				}
				return;
			}
			if (_E084._E051.Count <= 0)
			{
				_E6EE firearmPacket = _E084._E04E.Dequeue();
				_E000(firearmPacket);
				_E6E7? obj = firearmPacket.FiredShotInfos;
				while (obj.HasValue)
				{
					_E6E7 value = obj.Value;
					Vector3 shotPosition = value.ShotPosition;
					Vector3 shotDirection = value.ShotDirection;
					EShotType eShotType = value.EShotType;
					_E084._E003(shotPosition, shotDirection, eShotType.ToWeaponMalfunctionState());
					obj = obj.Value.GetNested();
				}
				PrepareShot();
			}
			_E037.SetFire(((Player.FirearmController._E00E)this)._E002.IsTriggerPressed);
		}
	}

	private new sealed class _E003 : Player.FirearmController._E00C
	{
		private _E00D _E01D;

		private readonly ObservedFirearmController _E084;

		private _E6EF _E085;

		private _E6F0 _E086;

		private _E6F4 _E087;

		private _E6F3 _E08A;

		public _E003(Player.FirearmController controller)
			: base(controller)
		{
			_E084 = controller as ObservedFirearmController;
		}

		public override void Start()
		{
			base.Start();
			if (_E039.MalfState.State != Weapon.EMalfunctionState.Misfire)
			{
				string stateName = _E037.FullFireStateName;
				if (_E039.FireMode.FireMode == Weapon.EFireMode.semiauto)
				{
					stateName = _E037.FullSemiFireStateName;
				}
				_E037.Animator.Play(stateName, 1, 0.2f);
			}
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);
			if (_E084._E00B())
			{
				FastForward();
				_E084.CurrentOperation.Update(0.01f);
			}
		}

		protected override void PrepareShot()
		{
			_E01D = _E084._E051.Dequeue();
			_E084._E053 = _E01D.ShotDirection;
			_E084._E052 = _E01D.FireportPosition;
			Weapon.EMalfunctionState malfunctionState = _E01D.MalfunctionState;
			_E02F = _E002(malfunctionState);
			_E039.MalfState.State = malfunctionState;
			if (malfunctionState != 0)
			{
				_E039.MalfState.LastMalfunctionTime = _E62A.PastTime;
				_E037.MisfireSlideUnknown(val: false);
				((Player.FirearmController._E00E)this)._E001._E0DE.ExamineMalfunction(_E039);
			}
			switch (malfunctionState)
			{
			case Weapon.EMalfunctionState.None:
				((Player.FirearmController._E00E)this)._E002.Malfunction = false;
				_E037.SetFire(((Player.FirearmController._E00E)this)._E002.IsTriggerPressed);
				break;
			case Weapon.EMalfunctionState.Misfire:
				((Player.FirearmController._E00E)this)._E002.Malfunction = true;
				_E001();
				break;
			case Weapon.EMalfunctionState.Jam:
			case Weapon.EMalfunctionState.HardSlide:
			case Weapon.EMalfunctionState.SoftSlide:
			case Weapon.EMalfunctionState.Feed:
				((Player.FirearmController._E00E)this)._E002.Malfunction = true;
				_E037.Malfunction((int)malfunctionState);
				_E037.SetFire(((Player.FirearmController._E00E)this)._E002.IsTriggerPressed);
				break;
			}
		}

		public override void Reset()
		{
			base.Reset();
			_E085 = default(_E6EF);
			_E086 = default(_E6F0);
			_E087 = default(_E6F4);
			_E08A = default(_E6F3);
		}

		public override void OnFireEndEvent()
		{
			if (_E084._E051.Count > 0)
			{
				((Player.FirearmController._E00E)this)._E002.IsTriggerPressed = true;
			}
			else if (_E084._E04E.Count > 0)
			{
				((Player.FirearmController._E00E)this)._E002.IsTriggerPressed = _E084._E04E.Peek().HasShotsRealShots();
			}
			else
			{
				((Player.FirearmController._E00E)this)._E002.IsTriggerPressed = false;
			}
			if (!((Player.FirearmController._E00E)this)._E002.IsTriggerPressed)
			{
				_E037.Animator.Play(_E037.FullIdleStateName, 1, 0.1f);
				_E000();
				if (_E08A.StatusChanged)
				{
					_E084._E008(_E08A.CamoraIndex, _E08A.HammerClosed);
				}
				if (_E085.Reload)
				{
					_E084._E005(_E085.MagId, _E085.LocationDescription, null);
				}
				else if (_E086.Reload)
				{
					_E084._E006(_E086.MagId, null);
				}
				else if (_E087.Reload)
				{
					if (_E039 is _EAD1)
					{
						_E084._E009(_E087.AmmoIds, _E087.FastReload, null);
					}
					else
					{
						_E084._E00A(_E087.AmmoIds, null);
					}
				}
				return;
			}
			if (_E084._E051.Count <= 0)
			{
				_E6EE firearmPacket = _E084._E04E.Dequeue();
				_E000(firearmPacket);
				_E6E7? obj = firearmPacket.FiredShotInfos;
				while (obj.HasValue)
				{
					_E6E7 value = obj.Value;
					Vector3 shotPosition = value.ShotPosition;
					Vector3 shotDirection = value.ShotDirection;
					EShotType eShotType = value.EShotType;
					_E084._E003(shotPosition, shotDirection, eShotType.ToWeaponMalfunctionState());
					obj = obj.Value.GetNested();
				}
				PrepareShot();
			}
			_E037.SetFire(((Player.FirearmController._E00E)this)._E002.IsTriggerPressed);
		}

		protected override void UncheckOnShot()
		{
		}

		private void _E000(_E6EE firearmPacket)
		{
			_E6F3 cylinderMagStatusPacket = firearmPacket.CylinderMagStatusPacket;
			if (cylinderMagStatusPacket.StatusChanged)
			{
				_E08A = cylinderMagStatusPacket;
			}
			_E6EF reloadMagPacket = firearmPacket.ReloadMagPacket;
			if (reloadMagPacket.Reload)
			{
				_E085 = reloadMagPacket;
			}
			else
			{
				_E6F0 quickReloadMagPacket = firearmPacket.QuickReloadMagPacket;
				if (quickReloadMagPacket.Reload)
				{
					_E086 = quickReloadMagPacket;
				}
				else
				{
					_E6F4 reloadWithAmmoPacket = firearmPacket.ReloadWithAmmoPacket;
					if (reloadWithAmmoPacket.Reload)
					{
						_E087 = reloadWithAmmoPacket;
					}
				}
			}
			if (firearmPacket.LauncherReloadInfo.Reload)
			{
				string text = _ED3E._E000(190430);
				Debug.LogErrorFormat(_ED3E._E000(190403), text);
			}
		}

		protected override void MakeShot(_EA12 ammo, int chamberIndex = 0, bool multiBarrelShot = false)
		{
			if (_E2B6.Config.UseSpiritPlayer && ((Player.FirearmController._E00E)this)._E001.Spirit.IsActive)
			{
				((Player.FirearmController._E00E)this)._E001.Spirit.PlayerSync();
			}
			base.MakeShot(ammo, chamberIndex, multiBarrelShot);
		}
	}

	private new sealed class _E004 : Player.FirearmController._E011
	{
		private readonly ObservedFirearmController _E084;

		public _E004(Player.FirearmController controller)
			: base(controller)
		{
			_E084 = controller as ObservedFirearmController;
		}

		public override void ShowUncompatibleNotification()
		{
		}

		protected override void RunUtilityOperation(_E034.EUtilityType utilityType)
		{
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);
			_E00E obj = _E084._E04E;
			if (_E002.IsTriggerPressed)
			{
				SetTriggerPressed(pressed: false);
			}
			if (obj.Count == 0)
			{
				return;
			}
			_E6EE obj2 = obj.Dequeue();
			if (obj2.ChangeFireMode.ChangeFireMode)
			{
				_E002.ChangeFireMode(obj2.ChangeFireMode.FireMode);
			}
			if (obj2.ToggleAim)
			{
				_E002.SetAim(obj2.AimingIndex);
			}
			if (obj2.ExamineWeapon && _E002.ExamineWeapon())
			{
				((Player.FirearmController._E00E)this)._E001.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 1f);
			}
			if (obj2.CheckAmmo && _E002.CheckAmmo())
			{
				((Player.FirearmController._E00E)this)._E001.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 1f);
			}
			if (obj2.CheckChamber && _E002.CheckChamber())
			{
				((Player.FirearmController._E00E)this)._E001.BodyAnimatorCommon.SetFloat(_E712.RELOAD_FLOAT_PARAM_HASH, 1f);
			}
			if (obj2.CheckFireMode)
			{
				_E002.CheckFireMode();
			}
			if (obj2.RollCylinderPacket.RollCylinder)
			{
				_E002.RollCylinder(obj2.RollCylinderPacket.RollToZeroCamora);
			}
			if (obj2.FlareShotInfo.StartOneShotFire)
			{
				SetTriggerPressed(pressed: true);
			}
			if (obj2.FlareShotInfo.WasShot)
			{
				_E084._E04F = obj2.FlareShotInfo;
				if (_E039.IsFlareGun)
				{
					SetTriggerPressed(pressed: true);
				}
			}
			if (obj2.HideWeaponPacket.HideWeapon)
			{
				Item item = null;
				string itemId = obj2.HideWeaponPacket.ItemId;
				if (!string.IsNullOrEmpty(itemId))
				{
					item = ((Player.FirearmController._E00E)this)._E001._E0DE.Inventory.Equipment.FindItem(itemId);
					if (item == null && Singleton<GameWorld>.Instance.FindStationaryWeaponByItemId(itemId) == null)
					{
						Debug.LogError(_ED3E._E000(188472) + obj2.HideWeaponPacket.ItemId + _ED3E._E000(188493) + ((Player.FirearmController._E00E)this)._E001.Profile.Nickname + _ED3E._E000(31756));
					}
				}
				HideWeapon(null, obj2.HideWeaponPacket.FastDrop, item);
			}
			if (obj2.ToggleTacticalCombo.ToggleTacticalCombo)
			{
				_E6EA[] tacticalComboStatuses = obj2.ToggleTacticalCombo.TacticalComboStatuses;
				_E6C5[] array = new _E6C5[tacticalComboStatuses.Length];
				for (int i = 0; i < tacticalComboStatuses.Length; i++)
				{
					_E6EA obj3 = tacticalComboStatuses[i];
					array[i] = new _E6C5
					{
						Id = obj3.Id,
						IsActive = obj3.IsActive,
						LightMode = obj3.SelectedMode
					};
				}
				_E002.SetLightsState(array, force: true);
			}
			if (obj2.ChangeSightsMode.ChangeSightMode)
			{
				_E6ED[] sightModeStatuses = obj2.ChangeSightsMode.SightModeStatuses;
				_E6C6[] array2 = new _E6C6[sightModeStatuses.Length];
				for (int j = 0; j < sightModeStatuses.Length; j++)
				{
					_E6ED obj4 = sightModeStatuses[j];
					array2[j] = new _E6C6
					{
						Id = obj4.Id,
						ScopeMode = obj4.SelectedMode,
						ScopeIndexInsideSight = obj4.ScopeIndexInsideSight,
						ScopeCalibrationIndex = obj4.ScopeCalibrationIndex
					};
				}
				_E002.SetScopeMode(array2);
			}
			if (obj2.ToggleLauncher)
			{
				_E002.ToggleLauncher();
			}
			if (obj2.Gesture != 0)
			{
				((Player.FirearmController._E00E)this)._E001._E073(obj2.Gesture);
			}
			_E6E7? obj5 = obj2.FiredShotInfos;
			if (obj5.HasValue)
			{
				if (obj5.Value.Nested == null && obj5.Value.EShotType == EShotType.DryFire)
				{
					_E084._E002();
					if (_E084.Item.ChamberAmmoCount > 0 && _E002.Item.MalfState.State == Weapon.EMalfunctionState.None)
					{
						Debug.LogError(_ED3E._E000(188482));
					}
				}
				else
				{
					bool flag = false;
					while (obj5.HasValue)
					{
						_E6E7 value = obj5.Value;
						if (value.EShotType != EShotType.DryFire)
						{
							Vector3 shotPosition = value.ShotPosition;
							Vector3 shotDirection = value.ShotDirection;
							EShotType eShotType = value.EShotType;
							_E084._E003(shotPosition, shotDirection, eShotType.ToWeaponMalfunctionState());
							flag = true;
						}
						obj5 = value.GetNested();
					}
					if (flag)
					{
						SetTriggerPressed(pressed: true);
					}
				}
			}
			if (obj2.EnableInventoryPacket.EnableInventory)
			{
				SetInventoryOpened(obj2.EnableInventoryPacket.InventoryStatus);
			}
			if (obj2.ReloadMagPacket.Reload)
			{
				_E084._E005(obj2.ReloadMagPacket.MagId, obj2.ReloadMagPacket.LocationDescription, null);
			}
			if (obj2.QuickReloadMagPacket.Reload)
			{
				_E084._E006(obj2.QuickReloadMagPacket.MagId, null);
			}
			if (obj2.ReloadBarrelsPacket.Reload)
			{
				_E084._E004(obj2.ReloadBarrelsPacket.AmmoIds, obj2.ReloadBarrelsPacket.LocationDescription);
			}
			if (obj2.CylinderMagStatusPacket.StatusChanged)
			{
				_E084._E008(obj2.CylinderMagStatusPacket.CamoraIndex, obj2.CylinderMagStatusPacket.HammerClosed);
			}
			if (obj2.ReloadWithAmmoPacket.Reload)
			{
				if (obj2.ReloadWithAmmoPacket.ReloadWithAmmoStatus == _E6F4.EReloadWithAmmoStatus.StartReload)
				{
					if (_E039 is _EAD1)
					{
						_E084._E009(obj2.ReloadWithAmmoPacket.AmmoIds, obj2.ReloadWithAmmoPacket.FastReload, null);
					}
					else
					{
						_E084._E00A(obj2.ReloadWithAmmoPacket.AmmoIds, null);
					}
				}
				else
				{
					if (obj2.ReloadWithAmmoPacket.ReloadWithAmmoStatus == _E6F4.EReloadWithAmmoStatus.EndReload)
					{
						List<_EA12> foundAmmo = _E084.FindAmmoByIds(obj2.ReloadWithAmmoPacket.AmmoIds);
						if (_E039.MustBoltBeOpennedForInternalReload)
						{
							_E024.CommitReloadWithAmmo(obj2.ReloadWithAmmoPacket.AmmoLoadedToMag, new _E9CF(foundAmmo), ((Player.FirearmController._E00E)this)._E001, _E039.GetCurrentMagazine(), _E039);
						}
						else if (_E039 is _EAD1)
						{
							_EB13 obj6 = _E039.GetCurrentMagazine() as _EB13;
							_E01F.CommitReloadWithAmmo(obj2.ReloadWithAmmoPacket.AmmoLoadedToMag, new _E9CF(foundAmmo), ((Player.FirearmController._E00E)this)._E001, obj6, _E039, obj6.GetFreeCamorasIndexesFromCurrentActiveIndex(obj2.ReloadWithAmmoPacket.FastReload, !_E039.CylinderHammerClosed));
						}
						else
						{
							_E023.CommitReloadWithAmmo(obj2.ReloadWithAmmoPacket.AmmoLoadedToMag, new _E9CF(foundAmmo), ((Player.FirearmController._E00E)this)._E001, _E039.GetCurrentMagazine(), _E039);
						}
					}
					_E037.SetAmmoOnMag(_E039.GetMaxMagazineCount());
					_E037.SetAmmoInChamber(_E039.ChamberAmmoCount);
				}
			}
			if (obj2.LauncherReloadInfo.Reload)
			{
				Debug.LogError(_ED3E._E000(188515));
				_E084._E007(obj2.LauncherReloadInfo.AmmoIds, null);
			}
			_E002.ApplyCompassPacket(obj2.CompassPacket);
		}

		public override void SetTriggerPressed(bool pressed)
		{
			_E002.IsTriggerPressed = pressed;
			if (_E002.IsTriggerPressed && (_E039.ChamberAmmoCount > 0 || _E039.IsOneOff || (!_E039.HasChambers && _E039.GetCurrentMagazineCount() > 0 && !(_E039 is _EAD1))))
			{
				State = Player.EOperationState.Finished;
				if (_E002.Item.SelectedFireMode == Weapon.EFireMode.single || _E002.Item.SelectedFireMode == Weapon.EFireMode.doublet || _E002.Item.SelectedFireMode == Weapon.EFireMode.semiauto)
				{
					_E002.InitiateOperation<Player.FirearmController._E00C>().Start();
				}
				else
				{
					_E002.InitiateOperation<Player.FirearmController._E002>().Start();
				}
			}
			else if (_E002.IsTriggerPressed && _E039 is _EAD1)
			{
				State = Player.EOperationState.Finished;
				_E002.InitiateOperation<Player.FirearmController._E00C>().Start();
			}
			else
			{
				_E002._E001.SetFire(pressed);
				if (pressed)
				{
					_E002.DryShot();
				}
			}
		}

		protected override void ProcessAutoshot()
		{
		}

		protected override void ProcessRemoveOneOffWeapon()
		{
		}
	}

	private new sealed class _E005 : _E014
	{
		private _E00D _E01D;

		private readonly ObservedFirearmController _E084;

		private _E95A _E085;

		public _E005(Player.FirearmController controller)
			: base(controller)
		{
			_E084 = controller as ObservedFirearmController;
		}

		protected override void PrepareShot()
		{
			_E01D = _E084._E051.Dequeue();
			_E084._E053 = _E01D.ShotDirection;
			_E084._E052 = _E01D.FireportPosition;
			base.PrepareShot();
		}

		protected override void UncheckOnShot()
		{
		}

		public override void OnFireEndEvent()
		{
			if (_E084._E051.Count > 0)
			{
				((Player.FirearmController._E00E)this)._E002.IsTriggerPressed = true;
			}
			else if (_E084._E04E.Count > 0)
			{
				((Player.FirearmController._E00E)this)._E002.IsTriggerPressed = _E084._E04E.Peek().HasShotsRealShots();
			}
			else
			{
				((Player.FirearmController._E00E)this)._E002.IsTriggerPressed = false;
			}
			if (!((Player.FirearmController._E00E)this)._E002.IsTriggerPressed)
			{
				_E037.Animator.Play(_E037.FullIdleStateName, 1, 0.1f);
				_E000();
				if (_E085.Reload)
				{
					_E084._E007(_E085.AmmoIds, null);
				}
				return;
			}
			if (_E084._E051.Count <= 0)
			{
				_E6EE firearmPacket = _E084._E04E.Dequeue();
				_E000(firearmPacket);
				_E6E7? obj = firearmPacket.FiredShotInfos;
				while (obj.HasValue)
				{
					_E6E7 value = obj.Value;
					Vector3 shotPosition = value.ShotPosition;
					Vector3 shotDirection = value.ShotDirection;
					EShotType eShotType = value.EShotType;
					_E084._E003(shotPosition, shotDirection, eShotType.ToWeaponMalfunctionState());
					obj = obj.Value.GetNested();
				}
				PrepareShot();
			}
			_E037.SetFire(((Player.FirearmController._E00E)this)._E002.IsTriggerPressed);
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);
			if (_E084._E00B())
			{
				FastForward();
				_E084.CurrentOperation.Update(0.01f);
			}
		}

		private void _E000(_E6EE firearmPacket)
		{
			_E95A launcherReloadInfo = firearmPacket.LauncherReloadInfo;
			if (launcherReloadInfo.Reload)
			{
				_E085 = launcherReloadInfo;
			}
		}
	}

	private class _E006 : _E015
	{
		private readonly ObservedFirearmController _E084;

		public _E006(Player.FirearmController controller)
			: base(controller)
		{
			_E084 = controller as ObservedFirearmController;
		}

		public override void HideWeapon(Action onHidden, bool fastDrop, Item nextControllerItem = null)
		{
			_E003(onHidden, fastDrop, nextControllerItem);
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);
			_E00E obj = _E084._E04E;
			if (_E002.IsTriggerPressed)
			{
				SetTriggerPressed(pressed: false);
			}
			if (obj.Count == 0)
			{
				return;
			}
			_E6EE obj2 = obj.Dequeue();
			if (obj2.ChangeFireMode.ChangeFireMode)
			{
				_E002.ChangeFireMode(obj2.ChangeFireMode.FireMode);
			}
			if (obj2.ToggleAim)
			{
				_E002.SetAim(obj2.AimingIndex);
			}
			if (obj2.ExamineWeapon)
			{
				_E002.ExamineWeapon();
			}
			if (obj2.CheckAmmo)
			{
				_E002.CheckAmmo();
			}
			if (obj2.CheckChamber)
			{
				_E002.CheckChamber();
			}
			if (obj2.CheckFireMode)
			{
				_E002.CheckFireMode();
			}
			if (obj2.HideWeaponPacket.HideWeapon)
			{
				Item item = null;
				string itemId = obj2.HideWeaponPacket.ItemId;
				if (!string.IsNullOrEmpty(itemId))
				{
					item = ((Player.FirearmController._E00E)this)._E001._E0DE.Inventory.Equipment.FindItem(itemId);
					if (item == null && Singleton<GameWorld>.Instance.FindStationaryWeaponByItemId(itemId) == null)
					{
						Debug.LogError(_ED3E._E000(188472) + obj2.HideWeaponPacket.ItemId + _ED3E._E000(188493) + ((Player.FirearmController._E00E)this)._E001.Profile.Nickname + _ED3E._E000(31756));
					}
				}
				HideWeapon(null, obj2.HideWeaponPacket.FastDrop, item);
			}
			if (obj2.LauncherRangeStatePacket.ChangeLauncherRange)
			{
				_E047.ForceSetSightRangeIndex(obj2.LauncherRangeStatePacket.RangeIndex);
			}
			if (obj2.ToggleTacticalCombo.ToggleTacticalCombo)
			{
				_E6EA[] tacticalComboStatuses = obj2.ToggleTacticalCombo.TacticalComboStatuses;
				_E6C5[] array = new _E6C5[tacticalComboStatuses.Length];
				for (int i = 0; i < tacticalComboStatuses.Length; i++)
				{
					_E6EA obj3 = tacticalComboStatuses[i];
					array[i] = new _E6C5
					{
						Id = obj3.Id,
						IsActive = obj3.IsActive,
						LightMode = obj3.SelectedMode
					};
				}
				_E002.SetLightsState(array, force: true);
			}
			if (obj2.ChangeSightsMode.ChangeSightMode)
			{
				_E6ED[] sightModeStatuses = obj2.ChangeSightsMode.SightModeStatuses;
				_E6C6[] array2 = new _E6C6[sightModeStatuses.Length];
				for (int j = 0; j < sightModeStatuses.Length; j++)
				{
					_E6ED obj4 = sightModeStatuses[j];
					array2[j] = new _E6C6
					{
						Id = obj4.Id,
						ScopeMode = obj4.SelectedMode,
						ScopeIndexInsideSight = obj4.ScopeIndexInsideSight,
						ScopeCalibrationIndex = obj4.ScopeCalibrationIndex
					};
				}
				_E002.SetScopeMode(array2);
			}
			if (obj2.Gesture != 0)
			{
				((Player.FirearmController._E00E)this)._E001._E073(obj2.Gesture);
			}
			if (obj2.ToggleLauncher)
			{
				_E002.ToggleLauncher();
			}
			_E6E7? obj5 = obj2.FiredShotInfos;
			if (obj5.HasValue)
			{
				if (obj5.Value.Nested == null && obj5.Value.EShotType == EShotType.DryFire)
				{
					_E084._E002();
				}
				else
				{
					bool flag = false;
					while (obj5.HasValue)
					{
						_E6E7 value = obj5.Value;
						if (value.EShotType != EShotType.DryFire)
						{
							Vector3 shotPosition = value.ShotPosition;
							Vector3 shotDirection = value.ShotDirection;
							EShotType eShotType = value.EShotType;
							_E084._E003(shotPosition, shotDirection, eShotType.ToWeaponMalfunctionState());
							flag = true;
						}
						obj5 = value.GetNested();
					}
					if (flag)
					{
						SetTriggerPressed(pressed: true);
					}
				}
			}
			if (obj2.LauncherReloadInfo.Reload)
			{
				_E084._E007(obj2.LauncherReloadInfo.AmmoIds, null);
			}
			if (obj2.EnableInventoryPacket.EnableInventory)
			{
				SetInventoryOpened(obj2.EnableInventoryPacket.InventoryStatus);
			}
			_E002.ApplyCompassPacket(obj2.CompassPacket);
		}
	}

	private class _E007 : _E01F
	{
		private readonly ObservedFirearmController _E084;

		private int _E08B;

		public _E007(Player.FirearmController controller)
			: base(controller)
		{
			_E084 = controller as ObservedFirearmController;
		}

		public override void Update(float deltaTime)
		{
			_E00E obj = _E084._E04E;
			bool needSwitchToIdle = false;
			bool needAbort = false;
			ObservedFirearmController._E009.Update(obj, _E084, _E039, ref needAbort, ref _E06B, ref needSwitchToIdle);
			if (needAbort)
			{
				_E06A = true;
				base.SetTriggerPressed(pressed: true);
				if (_E05A)
				{
					_E001();
				}
			}
			if (needSwitchToIdle)
			{
				_E06A = true;
				try
				{
					_E000();
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
					obj.Dequeue();
				}
			}
			base.Update(deltaTime);
		}

		protected override void AddAmmoInHighMasteringReload()
		{
			if (_E08B < _E05B.Count && _E08B < _E04A.AmmoCount)
			{
				_E08B++;
				base.AddAmmoInHighMasteringReload();
			}
		}

		protected override void SetCamoraIndexForLoadAmmo()
		{
			_E06B = _E08B;
			base.SetCamoraIndexForLoadAmmo();
			_E08B++;
		}

		protected override void RemoveExtraPatronsInHandsAfterMasteringReload()
		{
			if (_E05A)
			{
				_E037.SetMasteringReloadAborted(_E06A);
				for (int i = _E08B; i < _E05B.Count && i < _E04A.AmmoCount; i++)
				{
					_E038.DestroyPatronInWeapon(_E05B[i]);
				}
			}
		}

		protected override void SwitchToIdle()
		{
		}

		public override void OnMagPuttedToRig()
		{
		}

		public override void Reset()
		{
			base.Reset();
			_E08B = 0;
		}

		public override void SetTriggerPressed(bool pressed)
		{
		}

		private new void _E000()
		{
			_E002();
			base._E002();
			base.SwitchToIdle();
		}

		private new void _E001()
		{
			_E037.SetCanReload(canReload: false);
			_E037.SetAmmoOnMag(_E08B);
			_E037.SetCamoraIndex(_E025.CurrentCamoraIndex);
			_E037.SetMasteringReloadAborted(_E06A);
		}

		private new void _E002()
		{
			if (_E08B < _E06B)
			{
				_E037.SetCamoraIndex(_E025.CurrentCamoraIndex);
				_E037.SetCamoraIndexForLoadAmmo(_E05B[_E08B]);
				_EA12 ammoToReload = _E04A.GetAmmoToReload(_E08B);
				_E038.SetRoundIntoWeapon(ammoToReload, _E05B[_E08B]);
			}
		}
	}

	private new sealed class _E008 : _E020
	{
		private readonly ObservedFirearmController _E084;

		public _E008(Player.FirearmController controller)
			: base(controller)
		{
			_E084 = controller as ObservedFirearmController;
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);
			_E00E obj = _E084._E04E;
			if (obj.Count == 0)
			{
				return;
			}
			_E6EE obj2 = obj.Peek();
			bool flag = true;
			if (obj2.ToggleTacticalCombo.ToggleTacticalCombo)
			{
				_E6EA[] tacticalComboStatuses = obj2.ToggleTacticalCombo.TacticalComboStatuses;
				_E6C5[] array = new _E6C5[tacticalComboStatuses.Length];
				for (int i = 0; i < tacticalComboStatuses.Length; i++)
				{
					_E6EA obj3 = tacticalComboStatuses[i];
					array[i] = new _E6C5
					{
						Id = obj3.Id,
						IsActive = obj3.IsActive,
						LightMode = obj3.SelectedMode
					};
				}
				((Player.FirearmController._E00E)this)._E002.SetLightsState(array, force: true);
				obj2.ToggleTacticalCombo = default(_E6E9);
			}
			if (obj2.ChangeSightsMode.ChangeSightMode)
			{
				_E6ED[] sightModeStatuses = obj2.ChangeSightsMode.SightModeStatuses;
				_E6C6[] array2 = new _E6C6[sightModeStatuses.Length];
				for (int j = 0; j < sightModeStatuses.Length; j++)
				{
					_E6ED obj4 = sightModeStatuses[j];
					array2[j] = new _E6C6
					{
						Id = obj4.Id,
						ScopeMode = obj4.SelectedMode,
						ScopeIndexInsideSight = obj4.ScopeIndexInsideSight,
						ScopeCalibrationIndex = obj4.ScopeCalibrationIndex
					};
				}
				((Player.FirearmController._E00E)this)._E002.SetScopeMode(array2);
				obj2.ChangeSightsMode = default(_E6EC);
			}
			if (obj2.EnableInventoryPacket.EnableInventory)
			{
				SetInventoryOpened(obj2.EnableInventoryPacket.InventoryStatus);
				obj2.EnableInventoryPacket = default(_E94F);
			}
			if (obj2._E002)
			{
				flag = false;
				FastForward();
			}
			if (flag)
			{
				obj.Dequeue();
			}
		}
	}

	private new static class _E009
	{
		public static void Update(_E00E scheduledPackets, ObservedFirearmController controller, Weapon weapon, ref bool needAbort, ref int ammoToLoadIntoMag, ref bool needSwitchToIdle)
		{
			if (scheduledPackets.Count <= 0)
			{
				return;
			}
			_E6EE obj = scheduledPackets.Peek();
			bool flag = true;
			if (obj.ReloadWithAmmoPacket.Reload)
			{
				if (obj.ReloadWithAmmoPacket.ReloadWithAmmoStatus == _E6F4.EReloadWithAmmoStatus.AbortReload)
				{
					ammoToLoadIntoMag = obj.ReloadWithAmmoPacket.AmmoLoadedToMag;
					needAbort = true;
				}
				else if (obj.ReloadWithAmmoPacket.ReloadWithAmmoStatus == _E6F4.EReloadWithAmmoStatus.EndReload)
				{
					ammoToLoadIntoMag = obj.ReloadWithAmmoPacket.AmmoLoadedToMag;
					needSwitchToIdle = true;
				}
				obj.ReloadWithAmmoPacket = default(_E6F4);
			}
			if (obj.ToggleTacticalCombo.ToggleTacticalCombo)
			{
				_E6EA[] tacticalComboStatuses = obj.ToggleTacticalCombo.TacticalComboStatuses;
				_E6C5[] array = new _E6C5[tacticalComboStatuses.Length];
				for (int i = 0; i < tacticalComboStatuses.Length; i++)
				{
					_E6EA obj2 = tacticalComboStatuses[i];
					array[i] = new _E6C5
					{
						Id = obj2.Id,
						IsActive = obj2.IsActive,
						LightMode = obj2.SelectedMode
					};
				}
				controller.SetLightsState(array, force: true);
				obj.ToggleTacticalCombo = default(_E6E9);
			}
			if (obj.ChangeSightsMode.ChangeSightMode)
			{
				_E6ED[] sightModeStatuses = obj.ChangeSightsMode.SightModeStatuses;
				_E6C6[] array2 = new _E6C6[sightModeStatuses.Length];
				for (int j = 0; j < sightModeStatuses.Length; j++)
				{
					_E6ED obj3 = sightModeStatuses[j];
					array2[j] = new _E6C6
					{
						Id = obj3.Id,
						ScopeMode = obj3.SelectedMode,
						ScopeIndexInsideSight = obj3.ScopeIndexInsideSight,
						ScopeCalibrationIndex = obj3.ScopeCalibrationIndex
					};
				}
				controller.SetScopeMode(array2);
				obj.ChangeSightsMode = default(_E6EC);
			}
			if (obj.EnableInventoryPacket.EnableInventory)
			{
				controller.SetInventoryOpened(obj.EnableInventoryPacket.InventoryStatus);
				obj.EnableInventoryPacket = default(_E94F);
			}
			_E6E7? firedShotInfos = obj.FiredShotInfos;
			if (firedShotInfos.HasValue)
			{
				needSwitchToIdle = true;
				flag = false;
			}
			if (flag)
			{
				scheduledPackets.Dequeue();
			}
		}
	}

	private new class _E00A : _E023
	{
		private readonly ObservedFirearmController _E084;

		public _E00A(Player.FirearmController controller)
			: base(controller)
		{
			_E084 = controller as ObservedFirearmController;
		}

		public override void Update(float deltaTime)
		{
			_E00E obj = _E084._E04E;
			bool needSwitchToIdle = false;
			bool needAbort = false;
			ObservedFirearmController._E009.Update(obj, _E084, _E039, ref needAbort, ref _E06B, ref needSwitchToIdle);
			if (needAbort)
			{
				_E06A = true;
				base.SetTriggerPressed(pressed: true);
			}
			if (needSwitchToIdle)
			{
				_E06A = true;
				try
				{
					base.SwitchToIdle();
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
					obj.Dequeue();
				}
			}
			base.Update(deltaTime);
		}

		protected override void SwitchToIdle()
		{
		}

		public override void SetTriggerPressed(bool pressed)
		{
		}

		public override void AddAmmoToMag()
		{
			_E06B++;
			_E037.SetAmmoOnMag(_E069.Count + _E06B);
		}
	}

	private new class _E00B : _E024
	{
		private readonly ObservedFirearmController _E084;

		public _E00B(Player.FirearmController controller)
			: base(controller)
		{
			_E084 = controller as ObservedFirearmController;
		}

		public override void Update(float deltaTime)
		{
			_E00E obj = _E084._E04E;
			bool needSwitchToIdle = false;
			bool needAbort = false;
			ObservedFirearmController._E009.Update(obj, _E084, _E039, ref needAbort, ref _E06B, ref needSwitchToIdle);
			if (needAbort)
			{
				_E06A = true;
				base.SetTriggerPressed(pressed: true);
			}
			if (needSwitchToIdle)
			{
				_E06A = true;
				try
				{
					if (_E039.MalfState.State == Weapon.EMalfunctionState.Misfire)
					{
						_E000();
						_E037.SetAmmoInChamber(_E039.ChamberAmmoCount);
					}
					base.SwitchToIdle();
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
					obj.Dequeue();
				}
			}
			base.Update(deltaTime);
		}

		protected override void SwitchToIdle()
		{
		}

		public override void OnAddAmmoInChamber()
		{
			if (_E039.MalfState.State == Weapon.EMalfunctionState.Misfire)
			{
				_E000();
				_E037.SetAmmoInChamber(_E039.ChamberAmmoCount);
			}
		}

		public override void SetTriggerPressed(bool pressed)
		{
		}
	}

	private new sealed class _E00C : _E028
	{
		private readonly ObservedFirearmController _E084;

		public _E00C(Player.FirearmController controller)
			: base(controller)
		{
			_E084 = controller as ObservedFirearmController;
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);
			_E00E obj = _E084._E04E;
			if (obj.Count == 0)
			{
				return;
			}
			_E6EE obj2 = obj.Peek();
			bool flag = true;
			if (obj2.ToggleTacticalCombo.ToggleTacticalCombo)
			{
				_E6EA[] tacticalComboStatuses = obj2.ToggleTacticalCombo.TacticalComboStatuses;
				_E6C5[] array = new _E6C5[tacticalComboStatuses.Length];
				for (int i = 0; i < tacticalComboStatuses.Length; i++)
				{
					_E6EA obj3 = tacticalComboStatuses[i];
					array[i] = new _E6C5
					{
						Id = obj3.Id,
						IsActive = obj3.IsActive,
						LightMode = obj3.SelectedMode
					};
				}
				((Player.FirearmController._E00E)this)._E002.SetLightsState(array, force: true);
				obj2.ToggleTacticalCombo = default(_E6E9);
			}
			if (obj2.ChangeSightsMode.ChangeSightMode)
			{
				_E6ED[] sightModeStatuses = obj2.ChangeSightsMode.SightModeStatuses;
				_E6C6[] array2 = new _E6C6[sightModeStatuses.Length];
				for (int j = 0; j < sightModeStatuses.Length; j++)
				{
					_E6ED obj4 = sightModeStatuses[j];
					array2[j] = new _E6C6
					{
						Id = obj4.Id,
						ScopeMode = obj4.SelectedMode
					};
				}
				((Player.FirearmController._E00E)this)._E002.SetScopeMode(array2);
				obj2.ChangeSightsMode = default(_E6EC);
			}
			if (obj2.EnableInventoryPacket.EnableInventory)
			{
				SetInventoryOpened(obj2.EnableInventoryPacket.InventoryStatus);
				obj2.EnableInventoryPacket = default(_E94F);
			}
			if (obj2._E002)
			{
				flag = false;
				FastForward();
			}
			if (flag)
			{
				obj.Dequeue();
			}
		}
	}

	private new struct _E00D
	{
		public Vector3 FireportPosition;

		public Vector3 ShotDirection;

		public Weapon.EMalfunctionState MalfunctionState;
	}

	private new sealed class _E00E : Queue<_E6EE>
	{
		public _E00E(int capacity)
			: base(capacity)
		{
		}
	}

	private new sealed class _E00F : Player.FirearmController._E00F
	{
		private readonly ObservedFirearmController _E084;

		public _E00F(Player.FirearmController controller)
			: base(controller)
		{
			_E084 = controller as ObservedFirearmController;
		}

		public override void OnFireEvent()
		{
			_E03A = _E039.FirstLoadedChamberSlot.ContainedItem as _EA12;
			if (_E03A != null && !_E03A.IsUsed)
			{
				_E03A.IsUsed = true;
				Vector3 shotPosition = _E084._E04F.ShotPosition;
				Vector3 shotForward = _E084._E04F.ShotForward;
				((Player.FirearmController._E00E)this)._E002._E00C.MoveAmmoFromChamberToShellPort(_E03A.IsUsed);
				_E039.FirstLoadedChamberSlot.RemoveItem().OrElse(elseValue: false);
				_E084.InitiateFlare(_E03A, shotPosition, shotForward);
			}
		}
	}

	private sealed class _E010 : _E01C
	{
		private readonly ObservedFirearmController _E084;

		public _E010(Player.FirearmController controller)
			: base(controller)
		{
			_E084 = controller as ObservedFirearmController;
		}

		public override void OnFireEvent()
		{
			_E03A = new _EA12(Guid.NewGuid().ToString(), _E039.Template.DefAmmoTemplate);
			if (_E03A != null)
			{
				_E03A.IsUsed = true;
				Vector3 shotPosition = _E084._E04F.ShotPosition;
				Vector3 shotForward = _E084._E04F.ShotForward;
				((Player.FirearmController._E00E)this)._E002._E00C.MoveAmmoFromChamberToShellPort(_E03A.IsUsed);
				_E084.InitiateFlare(_E03A, shotPosition, shotForward);
				_E03A = null;
				_E039.Repairable.Durability = 0f;
			}
		}
	}

	[CompilerGenerated]
	private new sealed class _E011
	{
		public string magId;

		internal bool _E000(_EA6A mag)
		{
			return mag.Id == magId;
		}
	}

	[CompilerGenerated]
	private new sealed class _E012
	{
		public string magId;

		internal bool _E000(_EA6A mag)
		{
			return mag.Id == magId;
		}
	}

	private _E952 _E04F;

	private static readonly List<_EA6A> _E050 = new List<_EA6A>(10);

	private readonly Queue<_E00D> _E051 = new Queue<_E00D>(20);

	private readonly _E00E _E04E = new _E00E(100);

	private Vector3 _E052;

	private Vector3 _E053;

	[CompilerGenerated]
	private int _E054;

	public override Vector3 WeaponDirection => -base.CurrentFireport.up;

	public int ShotsInBurst
	{
		[CompilerGenerated]
		get
		{
			return _E054;
		}
		[CompilerGenerated]
		set
		{
			_E054 = value;
		}
	}

	protected override Dictionary<Type, OperationFactoryDelegate> GetOperationFactoryDelegates()
	{
		Dictionary<Type, OperationFactoryDelegate> operationFactoryDelegates = base.GetOperationFactoryDelegates();
		operationFactoryDelegates[typeof(Player.FirearmController._E00C)] = () => (!base.Item.IsFlareGun) ? ((!base.Item.IsOneOff) ? ((base.Item.ReloadMode != Weapon.EReloadMode.OnlyBarrel) ? ((!(base.Item is _EAD1)) ? ((Player.FirearmController._E00C)new _E003(this)) : ((Player.FirearmController._E00C)new _E001(this))) : new _E002(this)) : new _E010(this)) : new _E00F(this);
		operationFactoryDelegates[typeof(Player.FirearmController._E002)] = () => new _E000(this);
		operationFactoryDelegates[typeof(Player.FirearmController._E011)] = () => new _E004(this);
		operationFactoryDelegates[typeof(_E022)] = delegate
		{
			if (base.Item.ReloadMode == Weapon.EReloadMode.InternalMagazine && base.Item.Chambers.Length == 0)
			{
				return new _E00A(this);
			}
			return base.Item.MustBoltBeOpennedForInternalReload ? ((_E022)new _E00B(this)) : ((_E022)new _E00A(this));
		};
		operationFactoryDelegates[typeof(_E01F)] = () => new _E007(this);
		operationFactoryDelegates[typeof(_E020)] = () => new _E008(this);
		operationFactoryDelegates[typeof(_E015)] = () => new _E006(this);
		operationFactoryDelegates[typeof(_E014)] = () => new _E005(this);
		operationFactoryDelegates[typeof(Player.FirearmController._E00F)] = () => new _E00F(this);
		operationFactoryDelegates[typeof(_E01C)] = () => new _E010(this);
		operationFactoryDelegates[typeof(_E028)] = () => new _E00C(this);
		return operationFactoryDelegates;
	}

	internal static ObservedFirearmController _E000(ObservedPlayer player, Weapon item)
	{
		return Player.FirearmController._E000<ObservedFirearmController>(player, item);
	}

	internal new static Task<ObservedFirearmController> _E001(ObservedPlayer player, Weapon item)
	{
		return Player.FirearmController._E001<ObservedFirearmController>(player, item);
	}

	private void _E002()
	{
		if (base.Item.MalfState.State != 0)
		{
			base.Item.MalfState.AddPlayerWhoKnowMalfunction(_player.Profile.Id);
		}
		if (base.Item.ChamberAmmoCount == 0)
		{
			SetTriggerPressed(pressed: true);
		}
	}

	public override void OnPlayerDead()
	{
		base.OnPlayerDead();
		int num = 100;
		if (_E04E.Count > 0)
		{
			Logger.LogError(_ED3E._E000(189802), _player.Profile.Id, _E04E.Count);
		}
		while (_E04E.Count > 0 && --num > 0)
		{
			_player.HandsAnimator.Animator.Update(Time.fixedDeltaTime);
			Player.FirearmController._E00E currentOperation = base.CurrentOperation;
			ManualUpdate(Time.fixedDeltaTime);
			if (currentOperation.State != Player.EOperationState.Finished)
			{
				currentOperation.FastForward();
			}
		}
		if (num < 0)
		{
			Logger.LogError(_ED3E._E000(189840), _E04E.Count);
		}
		foreach (_E6EE item in _E04E)
		{
			if (item._E002)
			{
				if (item.ReloadMagPacket.Reload)
				{
					Logger.LogError(_ED3E._E000(189861), _player.Profile.Id, base.CurrentOperation);
				}
				else if (item.HasShotsRealShots())
				{
					Logger.LogError(_ED3E._E000(189945), _player.Profile.Id, base.CurrentOperation);
				}
			}
		}
	}

	public override void SetScopeMode(_E6C6[] scopeStates)
	{
		base._E00C.ProceduralWeaponAnimation.ObservedCalibration();
		base.SetScopeMode(scopeStates);
	}

	public override void AdjustShotVectors(ref Vector3 position, ref Vector3 direction)
	{
	}

	protected override bool CanChangeCompassState(bool newState)
	{
		return false;
	}

	public override bool CanRemove()
	{
		return true;
	}

	protected override void OnCanUsePropChanged(bool canUse)
	{
	}

	public override void SetCompassState(bool active)
	{
	}

	private void _E003(Vector3 fireportPosition, Vector3 direction, Weapon.EMalfunctionState malfunction)
	{
		_E051.Enqueue(new _E00D
		{
			FireportPosition = fireportPosition,
			ShotDirection = direction,
			MalfunctionState = malfunction
		});
	}

	private void _E004(string[] ammoIds, byte[] locationDescriptorBytes)
	{
		List<_EA12> list = new List<_EA12>();
		foreach (string text in ammoIds)
		{
			_ECD9<Item> obj = _player.FindItemById(text, checkDistance: false);
			if (obj.Failed)
			{
				Debug.LogErrorFormat(_ED3E._E000(189956) + text + _ED3E._E000(189998));
			}
			else if (obj.Value is _EA12 item)
			{
				list.Add(item);
			}
			else
			{
				Debug.LogErrorFormat(_ED3E._E000(190040) + text + _ED3E._E000(190030));
			}
		}
		_EB22 placeToPutContainedAmmoMagazine = null;
		if (locationDescriptorBytes != null && locationDescriptorBytes.Length != 0)
		{
			using MemoryStream input = new MemoryStream(locationDescriptorBytes);
			using BinaryReader reader = new BinaryReader(input);
			try
			{
				if (locationDescriptorBytes.Length != 0)
				{
					_E677 descriptor = reader.ReadEFTGridItemAddressDescriptor();
					placeToPutContainedAmmoMagazine = _player._E0DE.ToGridItemAddress(descriptor);
				}
			}
			catch (_E69D exception)
			{
				Debug.LogException(exception);
			}
		}
		ReloadBarrels(new _E9CF(list), placeToPutContainedAmmoMagazine, null);
	}

	private void _E005(string magId, byte[] locationDescriptorBytes, [CanBeNull] string error)
	{
		_E050.Clear();
		_player._E0DE.GetAcceptableItemsNonAlloc(_EB0B.AllSlotNames, _E050, (_EA6A mag) => mag.Id == magId);
		_EA6A obj = ((_E050.Count > 0) ? _E050[0] : null);
		_E050.Clear();
		if (obj == null)
		{
			Debug.LogErrorFormat(_ED3E._E000(190062), magId);
			try
			{
				Item item = _player._E0DE.FindItem(magId);
				obj = item as _EA6A;
				if (obj == null)
				{
					Debug.LogErrorFormat(_ED3E._E000(190082), magId, item.ShortName);
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				Debug.LogErrorFormat(_ED3E._E000(190157), _player.ProfileId, magId);
			}
		}
		if (obj == null)
		{
			Debug.LogError(_ED3E._E000(190235));
			return;
		}
		_EB22 gridItemAddress = null;
		if (locationDescriptorBytes != null && locationDescriptorBytes.Length != 0)
		{
			using MemoryStream input = new MemoryStream(locationDescriptorBytes);
			using BinaryReader reader = new BinaryReader(input);
			try
			{
				if (locationDescriptorBytes.Length != 0)
				{
					_E677 descriptor = reader.ReadEFTGridItemAddressDescriptor();
					gridItemAddress = _player._E0DE.ToGridItemAddress(descriptor);
				}
			}
			catch (_E69D exception2)
			{
				Debug.LogException(exception2);
			}
		}
		ReloadMag(obj, gridItemAddress, delegate
		{
		});
	}

	private void _E006(string magId, [CanBeNull] string error)
	{
		_E050.Clear();
		_player._E0DE.GetAcceptableItemsNonAlloc(_EB0B.AllSlotNames, _E050, (_EA6A mag) => mag.Id == magId);
		_EA6A obj = ((_E050.Count > 0) ? _E050[0] : null);
		_E050.Clear();
		if (obj == null)
		{
			Debug.LogError(string.Format(_ED3E._E000(190062), magId));
			try
			{
				Item item = _player._E0DE.FindItem(magId);
				obj = item as _EA6A;
				if (obj == null)
				{
					Debug.LogError(string.Format(_ED3E._E000(190082), magId, item.ShortName));
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				Debug.LogError(string.Format(_ED3E._E000(190254), _player.ProfileId, magId));
			}
		}
		if (obj == null)
		{
			Debug.LogError(_ED3E._E000(190235));
		}
		else
		{
			QuickReloadMag(obj, delegate
			{
			});
		}
	}

	private void _E007(string[] ammoIds, string error)
	{
		List<_EA12> list = FindAmmoByIds(ammoIds);
		if (!list.TrueForAll((_EA12 ammo) => ammo.CheckAction(null)))
		{
			Debug.LogError(_ED3E._E000(190331));
		}
		if (list.Count != 0)
		{
			ReloadGrenadeLauncher(new _E9CF(list), delegate
			{
			});
		}
	}

	private void _E008(int startCamoraIndex, bool hammerClosed)
	{
		base.Item.CylinderHammerClosed = hammerClosed;
		if (base.Item.GetCurrentMagazine() is _EB13 obj)
		{
			obj.SetCurrentCamoraIndex(startCamoraIndex);
		}
	}

	private void _E009(string[] ammoIds, bool fastReload, [CanBeNull] string error)
	{
		List<_EA12> list = FindAmmoByIds(ammoIds);
		if (!list.TrueForAll((_EA12 ammo) => ammo.CheckAction(null)))
		{
			Debug.LogError(_ED3E._E000(190331));
		}
		if (list.Count != 0)
		{
			ReloadRevolverDrum(new _E9CF(list), delegate
			{
			}, fastReload);
		}
	}

	private void _E00A(string[] ammoIds, [CanBeNull] string error)
	{
		List<_EA12> list = FindAmmoByIds(ammoIds);
		if (!list.TrueForAll((_EA12 ammo) => ammo.CheckAction(null)))
		{
			Debug.LogError(_ED3E._E000(190331));
		}
		if (list.Count != 0)
		{
			ReloadWithAmmo(new _E9CF(list), delegate
			{
			});
		}
	}

	private new bool _E00B()
	{
		_E00E obj = _E04E;
		if (obj.Count <= 0)
		{
			return false;
		}
		foreach (_E6EE item in obj)
		{
			if (!item._E003)
			{
				continue;
			}
			return true;
		}
		return false;
	}

	void ObservedPlayer._E004.ProcessPlayerPacket(_E733 framePlayerInfo)
	{
		_E6EE firearmPacket = framePlayerInfo.FirearmPacket;
		_E04E.Enqueue(firearmPacket);
	}

	bool ObservedPlayer._E004.IsInIdleState()
	{
		return base.CurrentOperation is _E004;
	}

	protected override void InitiateShot(IWeapon weapon, _EA12 ammo, Vector3 shotPosition, Vector3 shotDirection, Vector3 fireportPosition, int chamberIndex, float overheat)
	{
		base.InitiateShot(weapon, ammo, _E052, _E053, fireportPosition, chamberIndex, overheat);
	}

	protected override void LightAndSoundShot(Vector3 point, Vector3 direction, AmmoTemplate ammoTemplate)
	{
	}

	public override void WeaponOverlapping()
	{
		if (!(base.CurrentOperation is _E02B))
		{
			WeaponOverlapView();
		}
	}

	[CompilerGenerated]
	private new Player._E00E _E00C()
	{
		if (!base.Item.IsFlareGun)
		{
			if (!base.Item.IsOneOff)
			{
				if (base.Item.ReloadMode != Weapon.EReloadMode.OnlyBarrel)
				{
					if (!(base.Item is _EAD1))
					{
						return new _E003(this);
					}
					return new _E001(this);
				}
				return new _E002(this);
			}
			return new _E010(this);
		}
		return new _E00F(this);
	}

	[CompilerGenerated]
	private Player._E00E _E00D()
	{
		return new _E000(this);
	}

	[CompilerGenerated]
	private Player._E00E _E00E()
	{
		return new _E004(this);
	}

	[CompilerGenerated]
	private Player._E00E _E00F()
	{
		if (base.Item.ReloadMode == Weapon.EReloadMode.InternalMagazine && base.Item.Chambers.Length == 0)
		{
			return new _E00A(this);
		}
		if (base.Item.MustBoltBeOpennedForInternalReload)
		{
			return new _E00B(this);
		}
		return new _E00A(this);
	}

	[CompilerGenerated]
	private Player._E00E _E010()
	{
		return new _E007(this);
	}

	[CompilerGenerated]
	private Player._E00E _E011()
	{
		return new _E008(this);
	}

	[CompilerGenerated]
	private Player._E00E _E012()
	{
		return new _E006(this);
	}

	[CompilerGenerated]
	private Player._E00E _E013()
	{
		return new _E005(this);
	}

	[CompilerGenerated]
	private Player._E00E _E014()
	{
		return new _E00F(this);
	}

	[CompilerGenerated]
	private Player._E00E _E015()
	{
		return new _E010(this);
	}

	[CompilerGenerated]
	private Player._E00E _E016()
	{
		return new _E00C(this);
	}
}
