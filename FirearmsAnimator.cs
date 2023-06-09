using System;
using EFT;
using EFT.InventoryLogic;

public class FirearmsAnimator : ObjectInHandsAnimator
{
	public enum EGrenadeFire
	{
		Idle,
		Hold,
		Throw
	}

	public const string JAM_STATE_NAME = "JAM";

	public const string FIRE_STATE_NAME = "FIRE";

	public const string SEMIFIRE_STATE_NAME = "SA FIRE";

	public const string DOUBLE_ACTION_FIRE_STATE_NAME = "DOUBLE_ACTION_FIRE";

	public const string MISFIRE_STATE_NAME = "MISFIRE";

	public const string SOFT_SLIDE_STATE_NAME = "SOFT_SLIDE";

	public const string HARD_SLIDE_STATE_NAME = "HARD_SLIDE";

	public const string FEED_STATE_NAME = "FEED";

	public const string IDLE_STATE_NAME = "IDLE";

	public const string IDLE_UNDERBARREL_NAME = "IDLE WEAPON";

	public const string SPAWN_STATE_NAME = "SPAWN";

	public const string PATROL_STATE_NAME = "PATROL";

	public const string DRY_FIRE_STATE_NAME = "DRY FIRE";

	public const string DRY_FIRE_DISARMED_STATE_NAME = "Hands.DRY FIRE DISARMED";

	public const int HANDS_LAYER_INDEX = 1;

	public const string HANDS_LAYER_NAME = "Hands";

	public const string BOLT_CATCH_LAYER_NAME = "Catch";

	public const string HAMMER_LAYER_NAME = "Hammer";

	public const string ADDITIONAL_HANDS_LAYER_NAME = "Additional_Hands";

	public const string STOCK_LAYER = "Stock";

	public const string MALFUNCTION_LAYER = "Malfunction";

	public const string LHANDS_LAYER = "LActions";

	public int ADDITIONAL_HANDS_LAYER_INDEX;

	public int BOLT_CATCH_LAYER_INDEX;

	public int HAMMER_LAYER_INDEX;

	public int STOCK_LAYER_INDEX;

	public int MALFUNCTION_LAYER_INDEX;

	public new int LACTIONS_LAYER_INDEX;

	public string FullFireStateName { get; private set; }

	public string FullSemiFireStateName { get; private set; }

	public string FullDoubleActionFireStateName { get; private set; }

	public string FullIdleStateName { get; private set; }

	public event Action<bool> SetPatronInWeaponVisibleEvent;

	public void SetHammerArmed(bool isTriggerArmed)
	{
		_E326.SetArmed(base.Animator, isTriggerArmed);
		if (HAMMER_LAYER_INDEX >= 0)
		{
			base.Animator.SetLayerWeight(HAMMER_LAYER_INDEX, (!isTriggerArmed) ? 1 : 0);
		}
	}

	public void SetPatronInWeaponVisible(bool visible)
	{
		if (this.SetPatronInWeaponVisibleEvent != null)
		{
			this.SetPatronInWeaponVisibleEvent(visible);
		}
	}

	public override void SetAnimatorGetter(Func<IAnimator> getter)
	{
		base.SetAnimatorGetter(getter);
		BOLT_CATCH_LAYER_INDEX = base.Animator.GetLayerIndex("Catch");
		HAMMER_LAYER_INDEX = base.Animator.GetLayerIndex("Hammer");
		ADDITIONAL_HANDS_LAYER_INDEX = base.Animator.GetLayerIndex("Additional_Hands");
		LACTIONS_LAYER_INDEX = base.Animator.GetLayerIndex("LActions");
		STOCK_LAYER_INDEX = base.Animator.GetLayerIndex("Stock");
		MALFUNCTION_LAYER_INDEX = base.Animator.GetLayerIndex("Malfunction");
		LACTIONS_LAYER_INDEX = base.Animator.GetLayerIndex("LActions");
		FullFireStateName = base.Animator.GetLayerName(1) + ".FIRE";
		FullSemiFireStateName = base.Animator.GetLayerName(1) + ".SA FIRE";
		FullDoubleActionFireStateName = base.Animator.GetLayerName(1) + ".DOUBLE_ACTION_FIRE";
		FullIdleStateName = base.Animator.GetLayerName(1) + ".IDLE";
	}

	internal void Reload(int currentMagType, int nextMagType, bool isFast)
	{
		SetMagTypeCurrent(currentMagType);
		SetMagTypeNew(nextMagType);
		if (isFast)
		{
			ReloadFast(b: true);
		}
		else
		{
			Reload(b: true);
		}
	}

	public bool IsIdling()
	{
		if (!CurrentStateNameIs(1, "IDLE") && !CurrentStateNameIs(1, FullIdleStateName))
		{
			return CurrentStateNameIs(1, "IDLE WEAPON");
		}
		return true;
	}

	public bool IsHandsProcessing()
	{
		return _E326.GetBoolUseLeftHand(base.Animator);
	}

	internal void SetSpeedParameters(float reload = 1f, float draw = 1f)
	{
		_E326.SetSpeedReload(base.Animator, reload);
		_E326.SetSpeedDraw(base.Animator, draw);
	}

	internal void SetMalfRepairSpeed(float fix = 1f)
	{
		_E326.SetSpeedFix(base.Animator, fix);
	}

	internal void Idle()
	{
		_E326.TriggerIdleTime(base.Animator);
	}

	public void SetIsExternalMag(bool isExternalMag)
	{
		if (HasParameter(_E326.BOOL_ISEXTERNALMAG))
		{
			_E326.SetIsExternalMag(base.Animator, isExternalMag);
		}
	}

	internal void SetMagFull(bool b)
	{
		_E326.SetMagFull(base.Animator, b);
	}

	public void SetFire(bool fire)
	{
		_E326.SetFire(base.Animator, fire);
		ResetLeftHand();
	}

	public void SetSprint(bool sprint)
	{
		_E326.SetSprint(base.Animator, sprint);
	}

	public void SetGrenadeFire(EGrenadeFire fire)
	{
		_E326.SetGrenadeFire(base.Animator, (int)fire);
		ResetLeftHand();
	}

	public void SetAlternativeFire(bool fire)
	{
		_E326.SetAltFire(base.Animator, fire);
		ResetLeftHand();
	}

	public void SetGrenadeAltFire(EGrenadeFire fire)
	{
		_E326.SetGrenadeAltFire(base.Animator, (int)fire);
		ResetLeftHand();
	}

	public void SetMagInWeapon(bool ok)
	{
		_E326.SetMagInWeapon(base.Animator, ok);
	}

	public void SetBoltActionReload(bool boltActionReload)
	{
		if (HasParameter(_E326.BOOL_BOLTACTIONRELOAD))
		{
			_E326.SetBoltActionReload(base.Animator, boltActionReload);
		}
	}

	public void SetBoltCatch(bool active)
	{
		if (HasParameter(_E326.BOOL_BOLTCATCH))
		{
			_E326.SetBoltCatch(base.Animator, active);
		}
		if (HasParameter(_E326.FLOAT_IDLEPOSITION))
		{
			_E326.SetIdlePosition(base.Animator, active ? 1 : 0);
		}
		if (BOLT_CATCH_LAYER_INDEX >= 0)
		{
			base.Animator.SetLayerWeight(BOLT_CATCH_LAYER_INDEX, active ? 1 : 0);
		}
	}

	public bool GetBoltCatch()
	{
		if (HasParameter(_E326.BOOL_BOLTCATCH))
		{
			return _E326.GetBoolBoltCatch(base.Animator);
		}
		return false;
	}

	public void SetCanReload(bool canReload)
	{
		_E326.SetCanReload(base.Animator, canReload);
	}

	public void SetAimAngle(float playerPitch)
	{
		if (HasParameter(_E326.FLOAT_AIM_ANGLE))
		{
			_E326.SetAim_angle(base.Animator, playerPitch);
		}
	}

	public void Malfunction(int val)
	{
		_E326.SetMalfunction(base.Animator, val);
		if (val != -1)
		{
			_E326.SetMalfunctionType(base.Animator, val);
		}
	}

	public void MalfunctionRepair(bool val)
	{
		_E326.SetMalfunctionRepair(base.Animator, val);
	}

	public void MisfireSlideUnknown(bool val)
	{
		_E326.SetMisfireSlideUnknown(base.Animator, val);
	}

	public void Rechamber(bool val)
	{
		_E326.SetRechamber(base.Animator, val);
	}

	public void SetAmmoOnMag(int count)
	{
		_E326.SetAmmoInMag(base.Animator, count);
	}

	public void SetMasteringReloadAborted(bool value)
	{
		_E326.SetMasteringReloadAborted(base.Animator, value);
	}

	public void SetAmmoCountForRemove(int count)
	{
		_E326.SetAmmoCountForRemove(base.Animator, count);
	}

	public void SetCamoraIndex(int camoraIndex)
	{
		_E326.SetCamoraIndex(base.Animator, camoraIndex);
	}

	public void SetCamoraFireIndex(int camoraFireIndex)
	{
		_E326.SetCamoraFireIndex(base.Animator, camoraFireIndex);
	}

	public void SetDoubleAction(float doubleAction)
	{
		_E326.SetDoubleAction(base.Animator, doubleAction);
	}

	public void SetCamoraIndexForLoadAmmo(int camoraIndex)
	{
		_E326.SetCamoraIndexForLoadAmmo(base.Animator, camoraIndex);
	}

	public void SetCamoraIndexWithShellForRemove(int camoraIdnex)
	{
		_E326.SetCamoraIndexWithShellForRemove(base.Animator, camoraIdnex);
	}

	public void SetCamoraIndexForUnloadAmmo(int camoraIndex)
	{
		_E326.SetCamoraIndexForUnloadAmmo(base.Animator, camoraIndex);
	}

	public void SetAmmoInChamber(float count)
	{
		_E326.SetAmmoInChamber(base.Animator, count);
	}

	public void SetShellsInWeapon(int count)
	{
		_E326.SetShellsInWeapon(base.Animator, count);
	}

	public void SetUnderbarrelSightingRange(float value)
	{
		_E326.SetUnderbarrelSightingRange(base.Animator, value);
	}

	public int GetSightingRange()
	{
		return _E326.GetSightingRange(base.Animator);
	}

	public void SetFireMode(Weapon.EFireMode fireMode, bool skipAnimation = false)
	{
		ResetLeftHand();
		_E326.SetFireMode(base.Animator, (int)fireMode);
		if (!skipAnimation)
		{
			_E326.TriggerFiremodeSwitch(base.Animator);
		}
	}

	public void TriggerFiremodeCheck()
	{
		_E326.TriggerFiremodeCheck(base.Animator);
	}

	public void SetMagTypeCurrent(int magType)
	{
		_E326.SetRelTypeOld(base.Animator, magType);
	}

	public void SetMagTypeNew(int magType)
	{
		_E326.SetRelTypeNew(base.Animator, magType);
	}

	public void SetWeaponLevel(float weaponLevel)
	{
		_E326.SetWeaponLevel(base.Animator, weaponLevel);
	}

	public void SetPickup(bool p)
	{
		if (p)
		{
			_E326.SetUseLeftHand(base.Animator, useLeftHand: true);
		}
		_E326.SetLActionIndex(base.Animator, 1);
	}

	public void SetInteract(bool p, int actionIndex)
	{
		if (p)
		{
			_E326.SetUseLeftHand(base.Animator, useLeftHand: true);
		}
		_E326.SetLActionIndex(base.Animator, actionIndex);
	}

	internal void WatchClock(EPlayerSide playerSide)
	{
		if (playerSide != EPlayerSide.Savage)
		{
			_E326.SetUseLeftHand(base.Animator, useLeftHand: true);
			_E326.SetLActionIndex(base.Animator, (playerSide == EPlayerSide.Bear) ? 301 : 302);
		}
	}

	public void Fold(bool folded)
	{
		if (STOCK_LAYER_INDEX >= 0 && STOCK_LAYER_INDEX < _animatorLayersCount)
		{
			base.Animator.SetLayerWeight(STOCK_LAYER_INDEX, folded ? 1 : 0);
		}
		if (HasParameter(_E326.BOOL_STOCKFOLDED))
		{
			_E326.SetStockFolded(base.Animator, folded);
		}
	}

	public void SetAmmoCompatible(bool compatible)
	{
		if (HasParameter(_E326.BOOL_INCOMPATIBLEAMMO))
		{
			_E326.SetIncompatibleAmmo(base.Animator, !compatible);
		}
	}

	public void SetAnimationVariant(int animationVariant)
	{
		if (HasParameter(_E326.INT_ANIMATIONVARIANT))
		{
			_E326.SetAnimationVariant(base.Animator, animationVariant);
		}
	}

	public void SetUseTimeMultiplier(float speed)
	{
		if (HasParameter(_E326.FLOAT_USETIMEMULTIPLIER))
		{
			_E326.SetUseTimeMultiplier(base.Animator, speed);
		}
	}

	public void SetLooting(bool b)
	{
		if (b)
		{
			_E326.SetUseLeftHand(base.Animator, useLeftHand: true);
		}
	}

	public void ReloadFast(bool b)
	{
		ResetLeftHand();
		_E326.TriggerReloadFast(base.Animator);
	}

	public void RollToZeroCamora(bool roll)
	{
		ResetLeftHand();
		_E326.SetRollToZeroCamora(base.Animator, roll);
	}

	public void SetRollCylinder(bool roll)
	{
		ResetLeftHand();
		_E326.SetRollCylinder(base.Animator, roll);
	}

	public void LoadOneTrigger(bool loadOne)
	{
		ResetLeftHand();
		_E326.SetLoadOne(base.Animator, loadOne);
	}

	public void HammerArmed()
	{
		ResetLeftHand();
		_E326.TriggerHammerArmed(base.Animator);
	}

	public void SetChamberIndexForLoadUnloadAmmo(float chamberIndex)
	{
		ResetLeftHand();
		_E326.SetChamberIndexForLoadAmmo(base.Animator, chamberIndex);
	}

	public void SetChamberIndexWithShell(float chamberIndex)
	{
		ResetLeftHand();
		_E326.SetChamberIndexWithShell(base.Animator, chamberIndex);
	}

	public void Reload(bool b)
	{
		ResetLeftHand();
		_E326.TriggerReload(base.Animator);
	}

	public void ResetReload()
	{
		ResetLeftHand();
		_E326.ResetTriggerReload(base.Animator);
	}

	public void SetButterFingers()
	{
	}

	public void TriggerFold()
	{
		_E326.TriggerStockSwitch(base.Animator);
	}

	public void LookTrigger()
	{
		_E326.TriggerLook(base.Animator);
	}

	public void InsertMagInInventoryMode()
	{
		_E326.TriggerMagInFromInv(base.Animator);
	}

	public void ResetInsertMagInInventoryMode()
	{
		_E326.ResetTriggerMagInFromInv(base.Animator);
	}

	public void ResetGestureTrigger()
	{
		_E326.ResetTriggerGesture(base.Animator);
	}

	public void ResetHandReadyTrigger()
	{
		_E326.ResetTriggerHandReady(base.Animator);
	}

	public void PullOutMagInInventoryMode()
	{
		_E326.TriggerMagOutFromInv(base.Animator);
	}

	public void SetupMod(bool modSet)
	{
		_E326.SetModSet(base.Animator, modSet);
	}

	public void CheckAmmo()
	{
		_E326.TriggerCheckAmmo(base.Animator);
	}

	public void CheckChamber()
	{
		_E326.TriggerCheckChamber(base.Animator);
	}

	public void ResetCheckChamberTrigger()
	{
		_E326.ResetTriggerCheckChamber(base.Animator);
	}

	public void SetLauncher(bool isLauncherEnabled)
	{
		_E326.SetLauncherReady(base.Animator, isLauncherEnabled);
		SetLauncherId(-1);
	}

	public void SetLauncherId(int launcherId)
	{
		_E326.SetLauncherID(base.Animator, launcherId);
	}

	public void Discharge(bool discharge)
	{
		_E326.SetDischarge(base.Animator, discharge);
	}

	public void ModToggleTrigger()
	{
		_E326.TriggerModSwitch(base.Animator);
	}
}
