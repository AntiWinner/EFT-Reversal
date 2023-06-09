using Comfort.Common;
using UnityEngine;

namespace EFT.InventoryLogic;

public class WeaponTemplate : _EA3F, _E9E7, _E9D9, _E9D8
{
	public const int SINGLE_FIRE_RATE_DEFAULT = 240;

	public const bool CAN_QUEUE_SECOND_SHOT_DEFAULT = true;

	public Weapon.EReloadMode ReloadMode;

	public string weapClass;

	public string weapUseType;

	public string ammoCaliber;

	public bool AdjustCollimatorsToTrajectory;

	public object[] weapAmmoTypes;

	public float Durability;

	public float MaxDurability;

	public float RepairComplexity;

	public float OperatingResource;

	public float RecoilForceUp;

	public float RecoilForceBack;

	public float Convergence;

	public int RecoilAngle;

	public int RecolDispersion;

	public float CameraSnap;

	public float Ergonomics;

	public float Velocity;

	public int durabSpawnMin;

	public int durabSpawnMax;

	public bool isFastReload;

	public bool isChamberLoad;

	public int ShotgunDispersion;

	public int bFirerate;

	public int SingleFireRate = 240;

	public bool CanQueueSecondShot = true;

	public int bEffDist;

	public int bHearDist;

	public bool isBoltCatch;

	public string defMagType;

	public string defAmmo;

	public float CameraRecoil;

	public float AimPlane;

	public Slot[] Chambers;

	public float CenterOfImpact;

	public float DoubleActionAccuracyPenalty;

	public float DeviationMax;

	public float DeviationCurve;

	public bool MustBoltBeOpennedForExternalReload;

	public bool MustBoltBeOpennedForInternalReload;

	public bool Foldable;

	public bool Retractable;

	public bool BoltAction;

	public bool ManualBoltCatch;

	public Vector3 TacticalReloadStiffnes;

	public float TacticalReloadFixation;

	public Vector3 RecoilCenter;

	public Vector3 RotationCenter;

	public Vector3 RotationCenterNoStock;

	public int IronSightRange;

	public float HipInnaccuracyGain;

	public float HipAccuracyRestorationDelay;

	public float HipAccuracyRestorationSpeed;

	private AmmoTemplate _defAmmoTemplate;

	public bool CompactHandling;

	public float SightingRange;

	public bool AllowJam;

	public bool AllowFeed;

	public bool AllowMisfire;

	public bool AllowSlide;

	public float BaseMalfunctionChance;

	public float AimSensitivity = 1f;

	public float DurabilityBurnRatio = 1f;

	public float HeatFactorGun;

	public float CoolFactorGun;

	public bool AllowOverheat;

	public float HeatFactorByShot = 1f;

	public float CoolFactorGunMods = 1f;

	public float RecoilPosZMult = 1f;

	public bool IsFlareGun;

	public bool IsOneoff;

	public bool IsGrenadeLauncher;

	public bool NoFiremodeOnBoltcatch;

	public float MinRepairDegradation;

	public float MaxRepairDegradation;

	public int SizeReduceRight;

	public string FoldedSlot;

	public Weapon.EFireMode[] weapFireType;

	public int BurstShotsCount = 3;

	public string MasteringLocalizationKey => weapClass + _ED3E._E000(224860);

	public AmmoTemplate DefAmmoTemplate
	{
		get
		{
			if (_defAmmoTemplate == null && Singleton<_E63B>.Instance.ItemTemplates.ContainsKey(defAmmo))
			{
				_defAmmoTemplate = Singleton<_E63B>.Instance.ItemTemplates[defAmmo] as AmmoTemplate;
			}
			return _defAmmoTemplate;
		}
	}

	int _E9E7.Durability => 100;

	int _E9E7.MaxDurability => 100;

	float _E9E7.MinRepairDegradation => MinRepairDegradation;

	float _E9E7.MaxRepairDegradation => MaxRepairDegradation;

	float _E9E7.MinRepairKitDegradation => MinRepairDegradation;

	float _E9E7.MaxRepairKitDegradation => MaxRepairDegradation;

	int _E9D9.SizeReduceRight => SizeReduceRight;

	string _E9D9.FoldedSlot => FoldedSlot;

	Weapon.EFireMode[] _E9D8.AvailablEFireModes => weapFireType;

	int _E9D8.BurstShotsCount => BurstShotsCount;
}
