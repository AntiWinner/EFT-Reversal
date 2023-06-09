using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Comfort.Common;
using EFT.CameraControl;
using EFT.InventoryLogic;
using EFT.UI;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT.Animations;

public class ProceduralWeaponAnimation : MonoBehaviour
{
	public class _E000
	{
		private const float _E000 = 0.3f;

		private const float _E001 = 0.1f;

		private const float _E002 = 1f;

		private static Vector3 _E003 = new Vector3(0.3f, 1f, 1f);

		private static Vector3 _E004 = new Vector3(1f, 0.1f, 1f);

		private static Vector3 _E005 = new Vector3(1f, 1f, 0f);

		private static Vector3 _E006 = Vector3.Cross(_E003 - _E004, _E005 - _E004);

		private static float _E007 = Vector3.Dot(_E006, _E003);

		public static float GetValue(float speed, float height)
		{
			return Mathf.Clamp01((_E007 - speed * _E006.x - height * _E006.y) / _E006.z);
		}
	}

	public class _E001
	{
		public SightComponent Mod;

		public Transform Bone;

		public ScopePrefabCache ScopePrefabCache;

		public int Depth;

		public float Rotation;

		public bool AlignIfAvailable;

		public bool BoneRelatesToOptics;

		public float EyeGuardShift;

		public bool IsOptic
		{
			get
			{
				if (ScopePrefabCache != null && ScopePrefabCache.CurrentModHasOptics && BoneRelatesToOptics)
				{
					return string.IsNullOrEmpty(Mod.CustomAimPlane);
				}
				return false;
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public Transform scope;

		internal bool _E000(_E001 x)
		{
			return x.Bone == scope;
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public Transform c;

		internal int _E000(Transform x)
		{
			return HandPoser.NumParents(x, c);
		}

		internal int _E001(Transform x)
		{
			return HandPoser.NumParents(x, c);
		}
	}

	[CompilerGenerated]
	private sealed class _E004
	{
		public _E001 scope;

		internal bool _E000(WeaponPrefab.AimPlane x)
		{
			return x.Name == scope.Mod.CustomAimPlane;
		}
	}

	private static readonly WaitForEndOfFrame m__E000 = new WaitForEndOfFrame();

	private const float m__E001 = 0.1f;

	public const string MOD_CAMERA_BONE = "mod_aim_camera";

	public const string LAUNCHER_CAMERA_BONE = "launcher_0_aim_camera";

	public const string LINE_OF_SIGHT_P0 = "mod_align_rear";

	public const string LINE_OF_SIGHT_P1 = "mod_align_front";

	public const string CAMERA_BONE = "aim_camera";

	private const int m__E002 = 55;

	private const float m__E003 = 0.5f;

	public PlayerSpring HandsContainer;

	public GameObject CameraContainer;

	[_E376(typeof(EProceduralAnimationMask))]
	public EProceduralAnimationMask Mask;

	public BreathEffector Breath;

	public WalkEffector Walk;

	public MotionEffector MotionReact;

	public ForceEffector ForceReact;

	public ShotEffector Shootingg;

	public TurnAwayEffector TurnAway;

	public CustomEffector CustomEffector;

	public Vector3 Offset;

	public Vector2 AgsDeltaHeightRange = new Vector2(-10f, 10f);

	private float m__E004;

	private readonly Quaternion m__E005 = Quaternion.Euler(-90f, 0f, 0f);

	private _E333 m__E006;

	private Transform m__E007;

	private Transform m__E008;

	private Transform m__E009;

	private float m__E00A;

	private float m__E00B;

	private float m__E00C;

	private float m__E00D;

	public Vector2 _cameraShiftToLineOfSight;

	private Transform m__E00E;

	private Transform m__E00F;

	private bool m__E010;

	private const float m__E011 = 0.05f;

	public List<_E001> ScopeAimTransforms = new List<_E001>();

	public float WeaponFlipSpeed = 1f;

	public float CameraSmoothTime = 8f;

	private Vector3 m__E012;

	private Quaternion m__E013 = Quaternion.identity;

	private Quaternion m__E014;

	private float m__E015;

	private Player.FirearmController m__E016;

	[SerializeField]
	private bool _crankRecoil;

	private bool _E017;

	private bool _E018;

	public bool _shouldMoveWeaponCloser;

	[SerializeField]
	private Vector3 _vCameraTarget;

	public Vector3 RotationCameraOffset;

	public EPointOfView _pointOfView = EPointOfView.ThirdPerson;

	private float _E019;

	private float _E01A;

	public WeaponPrefab.AimPlane _currentAimingPlane;

	public WeaponPrefab.AimPlane _farPlane;

	private readonly List<_E001> _E01B = new List<_E001>(5);

	private Vector3 _E01C = Vector3.zero;

	public float _fovCompensatoryDistance;

	private float _E01D;

	private float _E01E;

	public float _agsDeltaHeight;

	private Quaternion _E01F = Quaternion.Euler(-180f, 0f, 0f);

	private Quaternion _E020;

	private float _E021 = 1f;

	public Vector3 _shotDirection = Vector3.down;

	private float _E022 = 1f;

	public Vector3 TacticalReloadStiffnes = new Vector3(0.95f, 0.3f, 0.95f);

	public float TacticalReloadPosition = 0.95f;

	private Player.ValueBlenderDelay _E023 = new Player.ValueBlenderDelay
	{
		Speed = 4f,
		Target = 0f
	};

	public float CameraSmoothSteady = 8f;

	public float CameraSmoothRecoil = 3f;

	private const float _E024 = 0.2f;

	public float CameraSmoothOut = 6f;

	public Action AvailableScopesChanged;

	public Player.BetterValueBlender CameraSmoothBlender = new Player.BetterValueBlender
	{
		Speed = 5f
	};

	public Vector3 AimSwayMax = new Vector3(-1f, 3f, -3f);

	public Vector3 AimSwayMin = new Vector3(-0.2f, -1f, -1f);

	public float SwayFalloff = 1f;

	public float AimSwayStartsThreshold = 3f;

	public float AimSwayMaxThreshold = 7f;

	private float _E025;

	private Vector3 _E026;

	private Player.ValueBlender _E027 = new Player.ValueBlender
	{
		Target = 0f
	};

	private Vector3 _E028;

	private Quaternion _E029;

	private Quaternion _E02A;

	private Vector3 _E02B;

	private float _E02C;

	private int _E02D = 2;

	private bool _E02E;

	private readonly Player.ValueBlender _E02F = new Player.ValueBlender
	{
		Speed = 4f,
		Target = 0f
	};

	public float SmoothedTilt;

	public float PossibleTilt;

	public float _launcherZeroZ = -0.01f;

	public float _launcherRotationZeroX = -5f;

	public Vector3 RotationZeroSum = Vector3.zero;

	public Vector3 PositionZeroSum = Vector3.zero;

	public Player.BetterValueBlender BlindfireBlender = new Player.BetterValueBlender
	{
		Speed = 5f,
		Target = 0f
	};

	public Player.ValueBlender TiltBlender = new Player.BetterValueBlender
	{
		Speed = 5f,
		Target = 0f
	};

	public Vector3 BlindFireOffset = new Vector3(0.02f, 0.07f, 0.15f);

	public Vector3 BlindFireRotation = new Vector3(2f, -20f, -1f);

	public Vector3 BlindFireCamera;

	public Vector3 SideFireOffset = new Vector3(0.08f, 0.02f, 0f);

	public Vector3 SideFireRotation = new Vector3(0f, 10f, 1f);

	public Vector3 SideFireCamera = new Vector3(-2f, -18f, 0f);

	public float Pitch;

	public float AimingDisplacementStr = 0.25f;

	private float _E030;

	private float _E031;

	private float _E032;

	private float _E033;

	private Vector3 _E034;

	private Vector3 _E035;

	public Vector3 BlindFireEndPosition;

	private bool _E036;

	private bool _E037;

	[CompilerGenerated]
	private Action _E038;

	private Vector3 _E039;

	private _E74F._E000 _E03A = new _E74F._E000();

	private Vector3 _E03B;

	private _E3CB _E03C;

	private bool _E03D;

	private Action _E03E;

	private Vector3 _E03F = Vector3.zero;

	[CompilerGenerated]
	private bool _E040;

	[CompilerGenerated]
	private bool _E041 = true;

	private float _E042;

	public List<(AnimatorPose Pose, float Time, bool Active)> ActiveBlends = new List<(AnimatorPose, float, bool)>(3);

	private const float _E043 = 1f;

	private float _E044 => HandsContainer.FarPlane.Depth;

	public Vector3 ShotDirection => _shotDirection;

	private float _E045 => Singleton<_E7DE>.Instance.Game.Settings.HeadBobbing;

	[CanBeNull]
	public SightComponent CurrentAimingMod
	{
		get
		{
			if (ScopeAimTransforms.Count <= 0)
			{
				return null;
			}
			return CurrentScope.Mod;
		}
	}

	protected int AimIndex
	{
		get
		{
			if (!this.m__E016)
			{
				return 0;
			}
			return this.m__E016.Item.AimIndex.Value;
		}
	}

	public Transform AimPointParent
	{
		get
		{
			if (!_crankRecoil)
			{
				return HandsContainer.WeaponRootAnim;
			}
			return HandsContainer.WeaponRoot;
		}
	}

	public float AimingSpeed => _E019;

	public bool CrankRecoil
	{
		get
		{
			return _crankRecoil;
		}
		set
		{
			_crankRecoil = value;
		}
	}

	public bool WalkEffectorEnabled
	{
		set
		{
			if (value)
			{
				Mask |= EProceduralAnimationMask.Walking;
				return;
			}
			Mask &= ~EProceduralAnimationMask.Walking;
			Walk.OnStop();
		}
	}

	public bool DrawEffectorEnabled
	{
		set
		{
			if (value)
			{
				Mask |= EProceduralAnimationMask.DrawDown;
			}
			else
			{
				Mask &= ~EProceduralAnimationMask.DrawDown;
			}
		}
	}

	private Vector3 _E046 => new Vector3(1f, _E021, 1f);

	private float _E047 => Singleton<_E7DE>.Instance.Game.Settings.FieldOfView.Value;

	public _E5CB._E024 Aiming => Singleton<_E5CB>.Instance.Aiming;

	public bool Sprint
	{
		get
		{
			return _E018;
		}
		set
		{
			_E018 = value;
			if (PointOfView == EPointOfView.FirstPerson)
			{
				_E8A8.Instance.SetFov(_E018 ? (_E047 - 10f) : _E047, _E018 ? 15 : 6);
			}
		}
	}

	public bool IsAiming
	{
		get
		{
			if (_E017)
			{
				return Mathf.Abs(BlindfireBlender.Target) < 1f;
			}
			return false;
		}
		set
		{
			_E017 = value;
			CameraSmoothBlender.ChangeValue(CameraSmoothSteady, 0f);
			if (_E025 > 0f)
			{
				_E027.Value = 1f;
				_E026 = _E025 * IntensityByPoseLevel * new Vector3(UnityEngine.Random.Range(AimSwayMin.x, AimSwayMax.x), UnityEngine.Random.Range(AimSwayMin.y, AimSwayMax.y), UnityEngine.Random.Range(AimSwayMin.z, AimSwayMax.z));
			}
			if (_E017 && _E03A.StiffDraw)
			{
				Breath.StiffUntill = Time.time + 3f;
			}
			_E014();
			if (_E017)
			{
				_E00C();
				_E00D();
				_E001();
			}
			else
			{
				_E00B();
			}
		}
	}

	public int Pose
	{
		get
		{
			return _E02D;
		}
		set
		{
			_E02D = value;
			_E014();
		}
	}

	public float ExternalHeadRotation => CustomEffector.PoseBlender.Value;

	public float IntensityByPoseLevel => Aiming.ProceduralIntensityByPose[Pose];

	public float IntensityByAiming
	{
		get
		{
			if (!IsAiming)
			{
				return 1f;
			}
			return Aiming.AimProceduralIntensity;
		}
	}

	public EPointOfView PointOfView
	{
		get
		{
			return _pointOfView;
		}
		set
		{
			_pointOfView = value;
			TurnAway.PointOfView = value;
			_E000();
			if (!(_E03C is _E3CE))
			{
				SetStrategy(value);
			}
		}
	}

	public bool IsGrenadeLauncher
	{
		get
		{
			return _E02E;
		}
		set
		{
			if (_E02E != value)
			{
				_E02F.Target = (value ? 1 : 0);
				_E02E = value;
				FindAimTransforms();
				_E014();
			}
		}
	}

	public bool OverlappingAllowsBlindfire
	{
		[CompilerGenerated]
		get
		{
			return _E040;
		}
		[CompilerGenerated]
		set
		{
			_E040 = value;
		}
	}

	private float _E048 => OverlappingAllowsBlindfire ? 1 : 0;

	public _E001 CurrentScope => ScopeAimTransforms[(ScopeAimTransforms.Count >= 1) ? (AimIndex % ScopeAimTransforms.Count) : 0];

	public float Overweight
	{
		get
		{
			return this.m__E004;
		}
		set
		{
			this.m__E004 = value;
			Breath.Overweight = value;
			_E01A = Mathf.Lerp(1f, Singleton<_E5CB>.Instance.Stamina.AimingSpeedMultiplier, value);
			Walk.Overweight = Mathf.Lerp(0f, Singleton<_E5CB>.Instance.Stamina.WalkVisualEffectMultiplier, value);
		}
	}

	public bool TacticalReload
	{
		get
		{
			return _E036;
		}
		set
		{
			_E036 = value;
			UpdateTacticalReload();
		}
	}

	public float VisorStepFrequency
	{
		get
		{
			if (!(_E03C is _E3CE))
			{
				return Walk.StepFrequency;
			}
			return 0f;
		}
	}

	public bool ShotNeedsFovAdjustments
	{
		[CompilerGenerated]
		get
		{
			return _E041;
		}
		[CompilerGenerated]
		set
		{
			_E041 = value;
		}
	}

	public event Action OnPreCollision
	{
		[CompilerGenerated]
		add
		{
			Action action = _E038;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E038, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E038;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E038, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	private void _E000()
	{
		if (HandsContainer.WeaponRootAnim == null || _E01B == null || _E01B.Count == 0)
		{
			return;
		}
		bool flag = false;
		foreach (_E001 item in _E01B)
		{
			if (!IsAiming || _pointOfView != 0 || CurrentScope != item)
			{
				ScopePrefabCache scopePrefabCache = item.ScopePrefabCache;
				for (int i = 0; i != scopePrefabCache.ModesCount; i++)
				{
					OpticSight opticSight = scopePrefabCache.GetOpticSight(i);
					if (!(opticSight == null))
					{
						bool flag3 = (opticSight.enabled = IsAiming && CurrentScope.ScopePrefabCache != null && CurrentScope.ScopePrefabCache.CurrentModOpticSight == opticSight && item.ScopePrefabCache == CurrentScope.ScopePrefabCache && i == CurrentScope.ScopePrefabCache.CurrentModeId);
						opticSight.LensFade(!flag3);
					}
				}
			}
			else
			{
				if (CurrentScope != item || !(CurrentScope.ScopePrefabCache != null))
				{
					continue;
				}
				for (int j = 0; j != CurrentScope.ScopePrefabCache.ModesCount; j++)
				{
					bool flag4 = CurrentScope.ScopePrefabCache.CurrentModeId == j;
					OpticSight opticSight2 = CurrentScope.ScopePrefabCache.GetOpticSight(j);
					if (!(opticSight2 == null))
					{
						opticSight2.enabled = flag4;
						flag = flag || flag4;
						opticSight2.LensFade(!flag4);
					}
				}
			}
		}
		if (flag)
		{
			return;
		}
		foreach (_E001 item2 in _E01B)
		{
			if (item2.ScopePrefabCache.CurrentModIgnoreOpticsForCameraPlane)
			{
				item2.ScopePrefabCache.CurrentModOpticSight.enabled = true;
				item2.ScopePrefabCache.CurrentModOpticSight.LensFade(isHide: false);
			}
		}
	}

	private void _E001()
	{
		_E037 = true;
	}

	public void PreCalibrate()
	{
		if (_E037)
		{
			CalculateLocalSightTarget();
		}
	}

	public void Calibrate()
	{
		if (_E037 || (_E017 && _E021 < 1f))
		{
			if (!_E03D)
			{
				_E004(_E03B);
			}
			_E006(_E03B);
			_E037 = false;
		}
	}

	public void CalculateLocalSightTarget()
	{
		if (this.m__E016 == null || this.m__E016.Item == null)
		{
			return;
		}
		SightComponent currentAimingMod = CurrentAimingMod;
		if (currentAimingMod == null)
		{
			return;
		}
		this.m__E016.Item.CreateOpticCalibrationPoints(currentAimingMod);
		if (currentAimingMod.HasOpticCalibrationPoints(currentAimingMod.SelectedScopeIndex))
		{
			_E03B = currentAimingMod.GetCurrentOpticCalibrationPoint();
			if (Singleton<GameUI>.Instantiated && Singleton<GameUI>.Instance.BattleUiScreen != null)
			{
				Singleton<GameUI>.Instance.BattleUiScreen.ShowAmmoCountZeroingPanel(currentAimingMod.GetCurrentOpticCalibrationDistance().ToString() ?? "");
			}
			_E00C();
			_E002(currentAimingMod.GetCurrentOpticCalibrationDistance());
		}
	}

	private void _E002(float distance)
	{
		if (CurrentScope != null && !(CurrentScope.ScopePrefabCache == null))
		{
			CurrentScope.ScopePrefabCache.RotateToAngleByDistance(distance);
		}
	}

	private Vector3 _E003(Vector3 point)
	{
		Quaternion quaternion = Quaternion.Euler(90f, 0f - this.m__E015, 0f);
		_ = HandsContainer.Fireport.localScale;
		return HandsContainer.Fireport.position + HandsContainer.Fireport.TransformDirection(quaternion * point);
	}

	private void _E004(Vector3 point)
	{
		ScopePrefabCache scopePrefabCache = CurrentScope.ScopePrefabCache;
		if (!(scopePrefabCache == null) && scopePrefabCache.HasOptics)
		{
			point = _E003(point);
			scopePrefabCache.LookAt(point, HandsContainer.Fireport.forward);
			ShotNeedsFovAdjustments = false;
		}
	}

	private void _E005(Vector3 point)
	{
		_E001 currentScope = CurrentScope;
		ScopePrefabCache scopePrefabCache = currentScope.ScopePrefabCache;
		if (!(scopePrefabCache == null) && scopePrefabCache.HasOptics)
		{
			Vector3 position = currentScope.Bone.position;
			Vector3 localOpticCameraTarget = scopePrefabCache.GetLocalOpticCameraTarget(position);
			scopePrefabCache.LookAt(point, HandsContainer.Fireport.forward);
			Vector3 opticsWorldCameraPosition = scopePrefabCache.GetOpticsWorldCameraPosition(localOpticCameraTarget);
			ScopeAimTransforms[AimIndex].Bone.position = opticsWorldCameraPosition;
			ShotNeedsFovAdjustments = false;
		}
	}

	private void _E006(Vector3 point)
	{
		ScopePrefabCache scopePrefabCache = CurrentScope.ScopePrefabCache;
		if (!(scopePrefabCache == null) && scopePrefabCache.HasCollimators)
		{
			point = _E003(point);
			if (scopePrefabCache.HasCollimators)
			{
				scopePrefabCache.LookAtCollimatorOnly(point, scopePrefabCache.GetLensTransformForward());
			}
		}
	}

	public void OpticCalibrationSwitchUp()
	{
		_E03C.OpticCalibration(this, calibrate: true);
	}

	public void OpticCalibrationSwitchDown()
	{
		_E03C.OpticCalibration(this, calibrate: false);
	}

	public void OpticCalibrationSwitch(bool isUp)
	{
		if (!IsAiming || this.m__E016.Item == null)
		{
			return;
		}
		SightComponent currentAimingMod = CurrentAimingMod;
		if (currentAimingMod != null && currentAimingMod.HasOpticCalibrationPoints(currentAimingMod.SelectedScopeIndex))
		{
			bool flag = false;
			if ((!isUp) ? currentAimingMod.OpticCalibrationPointDown() : currentAimingMod.OpticCalibrationPointUp())
			{
				_E001();
			}
		}
	}

	public void OnScopesModeUpdated()
	{
		_E014();
		_E001();
		_E013();
		_E011();
		_E00B();
		_E00C();
		_E00D();
	}

	public void ClearPreviousWeapon()
	{
		_E00B();
		this.m__E016 = null;
		_shouldMoveWeaponCloser = false;
		_E032 = 0.5f;
		_E033 = 0.5f;
		_E037 = false;
		ShotNeedsFovAdjustments = true;
		_E03D = false;
		this.m__E010 = false;
		_E03E?.Invoke();
	}

	internal void _E007(Player.FirearmController firearms, IWeapon weapon, WeaponPrefab wp, _E74F._E000 buffInfo)
	{
		this.m__E016 = firearms;
		_E03A = buffInfo;
		WeaponTemplate weaponTemplate = weapon.WeaponTemplate;
		Shootingg._E000(weapon, buffInfo, firearms.Item);
		HandsContainer.Recoil.ReturnSpeed = weaponTemplate.Convergence * Aiming.RecoilConvergenceMult;
		HandsContainer.Recoil.Damping = Aiming.RecoilDamping;
		HandsContainer.HandsPosition.Damping = Aiming.RecoilHandDamping;
		CrankRecoil = Aiming.RecoilCrank;
		HandsContainer.Recoil.GatherValues();
		HandsContainer.Recoil.SetCurveParameters(Aiming.RecoilScaling);
		TacticalReloadStiffnes = weaponTemplate.TacticalReloadStiffnes;
		TacticalReloadPosition = weaponTemplate.TacticalReloadFixation;
		Breath.Delay = weaponTemplate.HipAccuracyRestorationDelay;
		Breath.AmplitudeGain.Speed = weaponTemplate.HipAccuracyRestorationSpeed;
		Breath.AmplitudeGainPerShot = weaponTemplate.HipInnaccuracyGain;
		if (weaponTemplate.CameraSnap > 0f)
		{
			CameraSmoothRecoil = weaponTemplate.CameraSnap;
		}
		CameraSmoothBlender.Target = CameraSmoothSteady;
		if (wp != null)
		{
			HandsContainer.RotationCenter = wp.RotationCenter;
			HandsContainer.RecoilPivot = wp.RecoilCenter;
			HandsContainer.RotationCenterWoStock = wp.RotationCenterNoStock;
			_E008(wp);
		}
		UpdateWeaponVariables();
		TurnAway.IsPistol = firearms.Item is _EACD;
		firearms.Item.RecalculateOpticCalibrationPoints();
	}

	internal void _E008(WeaponPrefab wp)
	{
		if (!(wp == null))
		{
			HandsContainer.DefaultAimPlane = wp.DefaultAimPlane;
			HandsContainer.FarPlane = wp.FarPlane;
			HandsContainer.CustomAimPlanes = new WeaponPrefab.AimPlane[wp.CustomAimPlanes.Length + 1];
			HandsContainer.CustomAimPlanes[0] = wp.DefaultAimPlane;
			if (HandsContainer.CustomAimPlanes.Length > 1)
			{
				Array.Copy(wp.CustomAimPlanes, 0, HandsContainer.CustomAimPlanes, 1, wp.CustomAimPlanes.Length);
			}
		}
	}

	public void UpdateSwaySettings()
	{
		HandsContainer.SwaySpring._dampingRatio = Mathf.Lerp(EFTHardSettings.Instance.SWAY_DAMPING_NORMAL_DAMAGED.x, EFTHardSettings.Instance.SWAY_DAMPING_NORMAL_DAMAGED.y, _E042 + Overweight);
		HandsContainer.SwaySpring._angularFrequency = Mathf.Lerp(EFTHardSettings.Instance.SWAY_FREQ_NORMAL_DAMAGED.x, EFTHardSettings.Instance.SWAY_FREQ_NORMAL_DAMAGED.y, _E042 + Overweight);
	}

	public void PhysicalConditionUpdated(EPhysicalCondition updated, EPhysicalCondition full)
	{
		bool flag = (full & EPhysicalCondition.LeftArmDamaged) != 0;
		bool flag2 = (full & EPhysicalCondition.RightArmDamaged) != 0;
		_E042 = ((flag && flag2) ? 1f : ((flag || flag2) ? 0.5f : 0f));
		UpdateSwaySettings();
		Breath.Fracture = flag || flag2;
		if ((full & EPhysicalCondition.OnPainkillers) != 0)
		{
			Walk.CurrentWalkPreset = WalkEffector.EWalkPreset.normal;
			Breath.TremorOn = false;
		}
		else
		{
			Breath.TremorOn = (full & EPhysicalCondition.Tremor) != 0;
			Walk.CurrentWalkPreset = (((full & EPhysicalCondition.LeftLegDamaged) != 0 || (full & EPhysicalCondition.RightLegDamaged) != 0) ? WalkEffector.EWalkPreset.lame : WalkEffector.EWalkPreset.normal);
		}
	}

	public void UpdateWeaponVariables()
	{
		_shouldMoveWeaponCloser = false;
		if (!(this.m__E016 == null) && this.m__E016.Item != null)
		{
			this.m__E016.RecalculateErgonomic();
			Weapon item = this.m__E016.Item;
			_E03D = item.Template.AdjustCollimatorsToTrajectory;
			_E031 = this.m__E016.ErgonomicWeight;
			float singleItemTotalWeight = this.m__E016.Item.GetSingleItemTotalWeight();
			float num = Mathf.Clamp01(this.m__E016.TotalErgonomics / 100f);
			float t = Mathf.InverseLerp(Aiming.LightWeight, Aiming.HeavyWeight, singleItemTotalWeight);
			float a = Mathf.Lerp(Aiming.MaxTimeLight, Aiming.MaxTimeHeavy, t);
			float b = Mathf.Lerp(Aiming.MinTimeLight, Aiming.MinTimeHeavy, t);
			float t2 = ((num < 0.25f) ? (0.25f + 3f * num * num) : (2f * num - num * num));
			_E019 = 1f / Mathf.Lerp(a, b, t2) * (1f + _E03A.AimSpeed);
			_E027.Speed = SwayFalloff / _E019;
			_E025 = Mathf.InverseLerp(AimSwayStartsThreshold, AimSwayMaxThreshold, singleItemTotalWeight * (1f - num));
			UpdateSwayFactors();
			CheckShouldMoveWeaponCloser();
			item.RecalculateOpticCalibrationPoints();
			Shootingg.OnWeaponParametersChanged();
		}
	}

	public void CheckShouldMoveWeaponCloser()
	{
		Weapon weapon = ((this.m__E016 != null) ? this.m__E016.Item : null);
		if (_E017 || weapon == null || !weapon.CompactHandling)
		{
			_shouldMoveWeaponCloser = false;
			return;
		}
		bool flag = false;
		IEnumerable<ProtrudableComponent> enumerable = Enumerable.Empty<ProtrudableComponent>();
		if (_EB29.CanFold(weapon, out var foldable))
		{
			if (foldable.FoldedSlot == null)
			{
				flag |= !foldable.Folded;
			}
			else if (foldable.FoldedSlot.ContainedItem != null)
			{
				enumerable = foldable.FoldedSlot.ContainedItem.GetItemComponentsInChildren<ProtrudableComponent>().ToArray();
				flag |= !foldable.Folded && enumerable.Any((ProtrudableComponent x) => x.IsProtruding());
			}
		}
		IEnumerable<ProtrudableComponent> source = weapon.GetItemComponentsInChildren<ProtrudableComponent>().Except(enumerable);
		flag |= source.Any((ProtrudableComponent x) => x.IsProtruding());
		_shouldMoveWeaponCloser = !flag;
		if (!EFTHardSettings.Instance.OFFSET_FOLDED_WEAPON)
		{
			_shouldMoveWeaponCloser = false;
		}
	}

	public void ManualSetVariables(float aimingSpeed, float aimSwayStrength, float overweight, float ergonomicWeight)
	{
		_E019 = aimingSpeed;
		_E027.Speed = SwayFalloff / _E019;
		_E025 = aimSwayStrength;
		this.m__E004 = overweight;
		_E031 = ergonomicWeight;
	}

	public void SetLauncherWeaponBone(Transform weaponRoot, Transform propBone)
	{
		this.m__E007 = weaponRoot;
		this.m__E008 = propBone;
		this.m__E009 = _E38B.FindTransformRecursive(this.m__E007, _ED3E._E000(219813));
		this.m__E00A = -85f;
	}

	public void UpdateWeaponBoneByLauncherWeaponBone()
	{
		HandsContainer.Weapon.transform.position = this.m__E008.position;
		HandsContainer.Weapon.transform.rotation = this.m__E008.rotation;
	}

	public void InitTransforms(TransformLinks hierarchy, _E333 ccv = null)
	{
		_E03E?.Invoke();
		if (PointOfView == EPointOfView.FirstPerson && !Sprint && _E8A8.Instance.Camera != null)
		{
			_E8A8.Instance.SetFov(_E047, 2f);
		}
		_E020 = Quaternion.Euler(RotationCameraOffset);
		this.m__E006 = ccv;
		HandsContainer.TrackingTransform = hierarchy.Self;
		HandsContainer.CameraTransform = _E38B.FindTransformRecursive(CameraContainer.transform, _ED3E._E000(161643));
		HandsContainer.WeaponRootAnim = hierarchy.GetTransform(ECharacterWeaponBones.Weapon_root_anim);
		HandsContainer.CameraAnimatedFP = hierarchy.GetTransform(ECharacterWeaponBones.Camera_animated);
		HandsContainer.WeaponRoot = hierarchy.GetTransform(ECharacterWeaponBones.Weapon_root);
		HandsContainer.Weapon = hierarchy.GetTransform(ECharacterWeaponBones.weapon);
		HandsContainer.Fireport = _E38B.FindTransformRecursive(HandsContainer.WeaponRootAnim, _ED3E._E000(64493));
		MotionReact.Initialize(HandsContainer);
		ForceReact.Initialize(HandsContainer);
		Breath.Initialize(HandsContainer);
		Walk.Initialize(HandsContainer);
		Shootingg.Initialize(HandsContainer);
		TurnAway.Initialize(HandsContainer);
		CustomEffector.Initialize(HandsContainer);
		FindAimTransforms();
		if (this.m__E016 != null)
		{
			_E03E = this.m__E016.Item.AimIndex.Bind(_E016);
		}
		HandsContainer.CameraTransform.localPosition = HandsContainer.CameraOffset + _E01C;
		Reset();
	}

	public void FindAimTransformsWithoutSights()
	{
		Transform transform = _E38B.FindTransformRecursive(HandsContainer.WeaponRootAnim, _ED3E._E000(219866));
		if ((bool)transform)
		{
			ScopeAimTransforms.Add(new _E001
			{
				Bone = transform,
				Mod = null,
				AlignIfAvailable = true
			});
		}
		_E013();
		_E011();
	}

	public void FindUnderbarrelWeaponSight()
	{
		if (!(this.m__E007 == null))
		{
			Transform transform = _E38B.FindTransformRecursive(this.m__E007, _ED3E._E000(134736));
			if ((bool)transform)
			{
				ScopeAimTransforms.Add(new _E001
				{
					Bone = transform,
					Mod = null,
					AlignIfAvailable = true
				});
			}
		}
	}

	public void ResetScopeRotation()
	{
		this.m__E014 = Quaternion.identity;
		this.m__E015 = 0f;
	}

	public void FindAimTransforms()
	{
		ScopeAimTransforms.Clear();
		this.m__E00E = (this.m__E00F = null);
		_E00B();
		if (this.m__E006 == null)
		{
			return;
		}
		Slot[] array = (from x in this.m__E006.ContainerBones.Keys.OfType<Slot>()
			where x.ContainedItem != null && x.ContainedItem.GetItemComponent<SightComponent>() != null
			orderby x.ContainedItem.Name
			select x).ToArray();
		bool flag = false;
		AutoFoldableSight[] componentsInChildren = HandsContainer.Weapon.GetComponentsInChildren<AutoFoldableSight>(includeInactive: true);
		if (_E02E)
		{
			_E009();
			return;
		}
		Slot[] array2 = array;
		foreach (Slot slot2 in array2)
		{
			if (slot2.ContainedItem is _EA58 && !(slot2.ParentItem is _EAD5))
			{
				continue;
			}
			Transform boneForSlot = this.m__E006.GetBoneForSlot(slot2);
			List<Transform> list = _E38B._E004(boneForSlot, _ED3E._E000(134736), true, _ED3E._E000(134727));
			ScopePrefabCache scopePrefabCache = null;
			try
			{
				scopePrefabCache = boneForSlot.GetChild(0).GetComponent<ScopePrefabCache>();
			}
			catch (Exception)
			{
				Debug.LogError(string.Format(_ED3E._E000(219853), slot2, this.m__E016.Item.ShortName.Localized(), this.m__E016.IsTriggerPressed, boneForSlot.name, boneForSlot.parent.name));
			}
			for (int j = 0; j < list.Count; j++)
			{
				Transform scope = list[j];
				_E001 obj = ScopeAimTransforms.FirstOrDefault((_E001 x) => x.Bone == scope);
				if (obj != null)
				{
					if (HandPoser.NumParents(scope, boneForSlot) < obj.Depth)
					{
						obj.Mod = slot2.ContainedItem.GetItemComponent<SightComponent>();
					}
				}
				else
				{
					if (!(scope != null) || !_E00F(scope))
					{
						continue;
					}
					float num = 180f - _E38B.GetRotationRelativeToParent(HandsContainer.Weapon, scope).eulerAngles.y;
					if (Mathf.Abs(num) < 1f)
					{
						flag = true;
					}
					bool flag2 = scopePrefabCache != null && scopePrefabCache.IsOpticBone(scope);
					float eyeGuardShift = 0f;
					if (flag2)
					{
						EyeGuardComponent eyeGuardComponent = slot2.ContainedItem.GetItemComponentsInChildren<EyeGuardComponent>(onlyMerged: false).FirstOrDefault();
						if (eyeGuardComponent != null)
						{
							eyeGuardShift = eyeGuardComponent.Template.ShiftsAimCamera;
						}
					}
					ScopeAimTransforms.Add(new _E001
					{
						Bone = scope,
						Mod = slot2.ContainedItem.GetItemComponent<SightComponent>(),
						Depth = HandPoser.NumParents(scope, boneForSlot),
						ScopePrefabCache = scopePrefabCache,
						Rotation = num,
						EyeGuardShift = eyeGuardShift,
						BoneRelatesToOptics = flag2
					});
				}
			}
		}
		AutoFoldableSight[] array3 = componentsInChildren;
		foreach (AutoFoldableSight autoFoldableSight in array3)
		{
			autoFoldableSight.gameObject.SetActive((autoFoldableSight.Mode == EAutoFoldableSightMode.On) ^ flag);
		}
		if (!flag)
		{
			foreach (Slot item in array.Where((Slot slot) => slot.ContainedItem is _EA58))
			{
				Transform transform = this.m__E006.GetBoneForSlot(item).FindTransform(_ED3E._E000(134736));
				if (!(transform == null) && _E00F(transform))
				{
					ScopeAimTransforms.Add(new _E001
					{
						Bone = transform,
						Mod = item.ContainedItem.GetItemComponent<SightComponent>(),
						AlignIfAvailable = true
					});
					break;
				}
			}
			if (PointOfView == EPointOfView.FirstPerson)
			{
				_E00E();
			}
		}
		if (ScopeAimTransforms.Count == 0)
		{
			Transform transform2 = _E38B.FindTransformRecursive(HandsContainer.WeaponRootAnim, _ED3E._E000(134736));
			if ((bool)transform2 && _E00F(transform2))
			{
				ScopeAimTransforms.Add(new _E001
				{
					Bone = transform2,
					Mod = null,
					AlignIfAvailable = true
				});
			}
		}
		if (ScopeAimTransforms.Count == 0)
		{
			Transform transform3 = _E38B.FindTransformRecursive(HandsContainer.WeaponRootAnim, _ED3E._E000(219866));
			if ((bool)transform3)
			{
				ScopeAimTransforms.Add(new _E001
				{
					Bone = transform3,
					Mod = null,
					AlignIfAvailable = true
				});
			}
		}
		if (ScopeAimTransforms.Count < 1)
		{
			Debug.LogErrorFormat(_ED3E._E000(219937), (this.m__E016 != null) ? this.m__E016.Item.ShortName.Localized() : _ED3E._E000(29690), (this.m__E016 != null) ? string.Join(_ED3E._E000(10270), this.m__E016.Item.Mods.Select((Mod x) => x.ShortName.Localized()).ToArray()) : _ED3E._E000(29690));
		}
		_E00A();
	}

	private void _E009()
	{
		FindUnderbarrelWeaponSight();
		_E00A();
	}

	private void _E00A()
	{
		if (PointOfView == EPointOfView.FirstPerson)
		{
			_E013();
			_E01B.Clear();
			if (ScopeAimTransforms != null)
			{
				_E01B.AddRange(ScopeAimTransforms.Where((_E001 sat) => sat.ScopePrefabCache != null && sat.ScopePrefabCache.HasOptics));
			}
			_E011();
			_E000();
		}
		_E015();
		AvailableScopesChanged?.Invoke();
	}

	private void _E00B()
	{
		ShotNeedsFovAdjustments = true;
		_shotDirection = Vector3.down;
		_cameraShiftToLineOfSight = Vector2.zero;
		this.m__E00C = 0f;
	}

	[ContextMenu("align collimator")]
	private void _E00C()
	{
		if (!_E03D)
		{
			return;
		}
		_E001 currentScope = CurrentScope;
		if (currentScope != null && !currentScope.IsOptic && !(currentScope.ScopePrefabCache == null) && currentScope.ScopePrefabCache.HasCollimators)
		{
			Transform lensCenter = currentScope.ScopePrefabCache.GetLensCenter();
			Vector3 position = _E003(_E03B);
			Transform weapon = HandsContainer.Weapon;
			Vector3 vector = weapon.InverseTransformPoint(lensCenter.position);
			Vector3 vector2 = weapon.InverseTransformPoint(position);
			Vector3 vector3 = weapon.InverseTransformPoint(currentScope.Bone.position);
			Vector2 vector4 = new Vector2(0f - vector.y, vector.z);
			Vector2 vector5 = new Vector2(0f - vector2.y, vector2.z);
			Vector2 vector6 = new Vector2(0f - vector3.y, vector3.z);
			float num = vector5.y - vector4.y;
			float num2 = vector5.x - vector4.x;
			float num3 = Mathf.Atan(num / num2);
			this.m__E00C = num3 * 57.29578f;
			if (PointOfView == EPointOfView.FirstPerson)
			{
				Vector2 normalized = (vector4 - vector5).normalized;
				float num4 = vector4.x - vector6.x;
				Vector2 cameraShiftToLineOfSight = vector4 + normalized * num4 - vector6;
				_cameraShiftToLineOfSight = cameraShiftToLineOfSight;
			}
		}
	}

	private void _E00D()
	{
		if (CurrentScope.AlignIfAvailable && !(this.m__E00E == null) && !(this.m__E00F == null))
		{
			Transform weapon = HandsContainer.Weapon;
			Vector3 vector = weapon.InverseTransformPoint(this.m__E00E.position);
			Vector3 vector2 = weapon.InverseTransformPoint(this.m__E00F.position);
			Vector3 vector3 = weapon.InverseTransformPoint(this.m__E016.CurrentFireport.position);
			Vector3 vector4 = weapon.InverseTransformPoint(CurrentScope.Bone.position);
			Vector2 vector5 = new Vector2(0f - vector.y, vector.z);
			Vector2 vector6 = new Vector2(0f - vector2.y, vector2.z);
			Vector2 vector7 = new Vector2(0f - vector4.y, vector4.z);
			float num = vector6.y - vector5.y;
			float num2 = vector6.x - vector5.x;
			float num3 = Mathf.Atan(num / num2);
			this.m__E00C = num3 * 57.29578f;
			Vector3 normalized = new Vector3(0f, num, num2).normalized;
			Vector3 localPosition = vector2 + normalized * Mathf.Max(25, this.m__E016.Item.Template.IronSightRange) - vector3;
			Vector2 normalized2 = (vector5 - vector6).normalized;
			float num4 = vector5.x - vector7.x;
			Vector2 cameraShiftToLineOfSight = vector5 + normalized2 * num4 - vector7;
			_cameraShiftToLineOfSight = cameraShiftToLineOfSight;
			_shotDirection = this.m__E016.Item.CalculateShotDirectionForIron(localPosition, this.m__E016.Item.SpeedFactor, this.m__E016.Item.Template.IronSightRange);
			ShotNeedsFovAdjustments = false;
		}
	}

	[ContextMenu("Align")]
	private void _E00E()
	{
		Transform c = HandsContainer.Weapon;
		this.m__E00E = (from x in _E38B._E004(c, _ED3E._E000(220025), true)
			orderby HandPoser.NumParents(x, c) descending
			select x).FirstOrDefault();
		this.m__E00F = (from x in _E38B._E004(c, _ED3E._E000(220008), true)
			orderby HandPoser.NumParents(x, c) descending
			select x).FirstOrDefault();
		this.m__E010 = this.m__E00E != null && this.m__E00F != null;
	}

	private bool _E00F(Transform t)
	{
		return Mathf.Abs(180f - _E38B.GetRotationRelativeToParent(HandsContainer.Weapon, t).eulerAngles.y) < 55f;
	}

	public void UpdateTacticalReload()
	{
		bool flag = IsAiming && TacticalReload;
		_E023.Delay = (flag ? 0f : 0.2f);
		_E023.Speed = (flag ? 4 : 2);
		_E023.Target = (flag ? 1 : 0);
	}

	public void ZeroAdjustments()
	{
		PositionZeroSum.y = (_shouldMoveWeaponCloser ? 0.05f : 0f);
		RotationZeroSum.y = SmoothedTilt * PossibleTilt;
		float value = BlindfireBlender.Value;
		float num = Mathf.Abs(value);
		if (num > 0f)
		{
			_E022 = ((Mathf.Abs(Pitch) < 45f) ? 1f : ((90f - Mathf.Abs(Pitch)) / 45f));
			_E035 = ((value > 0f) ? (BlindFireRotation * num) : (SideFireRotation * num));
			_E034 = ((value > 0f) ? (BlindFireOffset * num) : (SideFireOffset * num));
			BlindFireEndPosition = ((value > 0f) ? BlindFireOffset : SideFireOffset);
			BlindFireEndPosition *= _E022;
		}
		else
		{
			_E034 = Vector3.zero;
		}
		HandsContainer.HandsPosition.Zero = PositionZeroSum + _E022 * _E048 * _E034;
		HandsContainer.HandsRotation.Zero = RotationZeroSum;
	}

	public void BlendAnimatorPose(float dt)
	{
		for (int num = ActiveBlends.Count - 1; num >= 0; num--)
		{
			(AnimatorPose Pose, float Time, bool Active) tuple = ActiveBlends[num];
			AnimatorPose item = tuple.Pose;
			float item2 = tuple.Time;
			bool item3 = tuple.Active;
			dt *= WeaponFlipSpeed;
			item2 += (item3 ? dt : (0f - dt));
			if (item2 < 0f)
			{
				ActiveBlends.RemoveAt(num);
				_E010();
			}
			else
			{
				float num2 = item.Blend.Evaluate(item2);
				HandsContainer.HandsPosition.Zero += item.Position * num2;
				HandsContainer.HandsRotation.Zero += item.Rotation * num2;
				ActiveBlends[num] = (item, Mathf.Min(item2, item.Blend.GetDuration()), item3);
			}
		}
	}

	private void _E010()
	{
		CustomEffector.Aim = IsAiming && ActiveBlends.Count < 1;
	}

	public void ApplyPosition()
	{
		HandsContainer.WeaponRootAnim.localPosition = HandsContainer.HandsPosition.Get();
		if (IsGrenadeLauncher)
		{
			UpdateWeaponBoneByLauncherWeaponBone();
		}
	}

	public void ApplyStationaryWeaponPosition()
	{
		HandsContainer.WeaponRootAnim.localPosition += HandsContainer.HandsPosition.Get();
	}

	public void RotateAround(Transform t, Vector3 worldPivot, Vector3 rotation)
	{
		Quaternion quaternion = Quaternion.Euler(0f, rotation.y, 0f);
		Quaternion quaternion2 = Quaternion.Euler(rotation.x, 0f, rotation.z);
		Quaternion quaternion3 = HandsContainer.WeaponRootAnim.parent.rotation * quaternion2 * quaternion * t.localRotation;
		Quaternion quaternion4 = quaternion3 * Quaternion.Inverse(t.rotation);
		Vector3 vector = t.position - worldPivot;
		vector = quaternion4 * vector;
		t.SetPositionAndRotation(worldPivot + vector, quaternion3);
	}

	public void DeferredRotateWithCustomOrder(Transform t, Vector3 worldPivot, Vector3 rotation)
	{
		_E029 = Quaternion.Euler(rotation.x, 0f, rotation.z) * Quaternion.Euler(0f, rotation.y, 0f) * _E029;
		Quaternion quaternion = HandsContainer.WeaponRootAnim.parent.rotation * _E029;
		Quaternion quaternion2 = quaternion * Quaternion.Inverse(_E02A);
		Vector3 vector = _E02B - worldPivot;
		vector = quaternion2 * vector;
		_E02A = quaternion;
		_E02B = worldPivot + vector;
	}

	public void DeferredRotate(Transform t, Vector3 worldPivot, Vector3 rotation)
	{
		Quaternion quaternion = Quaternion.Euler(rotation);
		Quaternion quaternion2 = quaternion * _E02A;
		Vector3 vector = _E02B - worldPivot;
		vector = quaternion * vector;
		_E02A = quaternion2;
		_E02B = worldPivot + vector;
	}

	public void ApplyComplexRotation(float dt)
	{
		Vector3 rotation = HandsContainer.HandsRotation.Get();
		Vector3 value = HandsContainer.SwaySpring.Value;
		rotation += _E033 * (_E017 ? AimingDisplacementStr : 1f) * new Vector3(value.x, 0f, value.z);
		rotation += value;
		Vector3 position = (_shouldMoveWeaponCloser ? HandsContainer.RotationCenterWoStock : HandsContainer.RotationCenter);
		Vector3 worldPivot = HandsContainer.WeaponRootAnim.TransformPoint(position);
		_E02B = HandsContainer.WeaponRootAnim.position;
		_E029 = HandsContainer.WeaponRootAnim.localRotation;
		_E02A = HandsContainer.WeaponRootAnim.rotation;
		DeferredRotateWithCustomOrder(HandsContainer.WeaponRootAnim, worldPivot, rotation);
		Vector3 vector = HandsContainer.Recoil.Get();
		if (vector.magnitude > float.Epsilon)
		{
			if (_E021 < 1f && ShotNeedsFovAdjustments)
			{
				vector.x = Mathf.Atan(Mathf.Tan(vector.x * ((float)Math.PI / 180f)) * _E021) * 57.29578f;
				vector.z = Mathf.Atan(Mathf.Tan(vector.z * ((float)Math.PI / 180f)) * _E021) * 57.29578f;
			}
			Vector3 worldPivot2 = _E02B + _E02A * HandsContainer.RecoilPivot;
			DeferredRotate(HandsContainer.WeaponRootAnim, worldPivot2, _E02A * vector);
		}
		ApplyAimingAlignment(dt);
		this.m__E013 = Quaternion.Lerp(this.m__E013, IsAiming ? this.m__E014 : Quaternion.identity, CameraSmoothTime * _E019 * dt);
		Quaternion quaternion = Quaternion.Euler(_E022 * _E048 * _E035);
		HandsContainer.WeaponRootAnim.SetPositionAndRotation(_E02B, _E02A * quaternion * this.m__E013);
	}

	public void ApplyAimingAlignment(float dt)
	{
		if (_E03D || this.m__E010)
		{
			this.m__E00D = Mathf.Lerp(this.m__E00D, _E017 ? this.m__E00C : 0f, dt * 5f);
			if (Mathf.Abs(this.m__E00D) > 0.002f)
			{
				DeferredRotate(HandsContainer.WeaponRootAnim, HandsContainer.Fireport.position, _E02A * new Vector3(this.m__E00D, 0f, 0f));
			}
		}
	}

	public void ApplySimpleRotation(float dt)
	{
		_E02B = HandsContainer.WeaponRootAnim.position;
		_E029 = HandsContainer.WeaponRootAnim.localRotation;
		_E02A = HandsContainer.WeaponRootAnim.rotation;
		Vector3 worldPivot = HandsContainer.WeaponRootAnim.TransformPoint(HandsContainer.RecoilPivot);
		Vector3 rotation = HandsContainer.HandsRotation.Get();
		DeferredRotateWithCustomOrder(HandsContainer.WeaponRootAnim, worldPivot, rotation);
		ApplyAimingAlignment(dt);
		Quaternion quaternion = Quaternion.Euler(_E022 * _E048 * _E035);
		this.m__E013 = Quaternion.Lerp(this.m__E013, IsAiming ? this.m__E014 : Quaternion.identity, CameraSmoothTime * _E019 * dt);
		HandsContainer.WeaponRootAnim.SetPositionAndRotation(_E02B, _E02A * quaternion * this.m__E013);
	}

	public void ApplyTacticalReloadTransformations()
	{
		float value = _E023.Value;
		if (value > Mathf.Epsilon)
		{
			Transform weapon = HandsContainer.Weapon;
			Quaternion localRotation = weapon.localRotation;
			Vector3 localPosition = weapon.localPosition;
			Vector3 a = localPosition;
			float x = Mathf.Lerp((localRotation.eulerAngles.x > 180f) ? (localRotation.eulerAngles.x - 360f) : localRotation.eulerAngles.x, 0f, TacticalReloadStiffnes.x * value);
			float angle = Mathf.Lerp((localRotation.eulerAngles.y > 180f) ? (localRotation.eulerAngles.y - 360f) : localRotation.eulerAngles.y, 0f, TacticalReloadStiffnes.y * value);
			float z = Mathf.Lerp((localRotation.eulerAngles.z > 180f) ? (localRotation.eulerAngles.z - 360f) : localRotation.eulerAngles.z, 0f, TacticalReloadStiffnes.z * value);
			weapon.localRotation = Quaternion.Euler(x, 0f, z);
			localPosition = Vector3.Lerp(localPosition, Vector3.zero, TacticalReloadPosition * value);
			weapon.localPosition = localPosition;
			Vector3 point = HandsContainer.WeaponRootAnim.TransformPoint(new Vector3(0f, 0f, HandsContainer.RecoilPivot.z * value));
			weapon.RotateAround(point, weapon.parent.transform.up, angle);
			weapon.localPosition = Vector3.Lerp(a, weapon.localPosition, value);
		}
	}

	public void AvoidObstacles()
	{
		_E038?.Invoke();
		if (TurnAway.IsDirty)
		{
			HandsContainer.WeaponRootAnim.localPosition += TurnAway.Position;
			HandsContainer.WeaponRootAnim.RotateAround(HandsContainer.WeaponRootAnim.TransformPoint(new Vector3(0f, -0.2f, 0f)), TurnAway.Rotation);
		}
		Offset = HandsContainer.WeaponRootAnim.localPosition;
	}

	public void SetStrategy(EPointOfView pointOfView)
	{
		_E3CB strategy;
		if (pointOfView != 0)
		{
			_E3CB obj = new _E3CD();
			strategy = obj;
		}
		else
		{
			_E3CB obj = new _E3CC();
			strategy = obj;
		}
		SetStrategy(strategy);
	}

	public void SetStrategy(_E3CB strategy)
	{
		_E03C = strategy;
	}

	public void SetHeadRotation(Vector3 headRot)
	{
		_E039 = headRot;
	}

	public Vector3 GetHeadRotation()
	{
		return new Vector3(_E039.y, 0f, 0f - _E039.x);
	}

	public void StationaryCamera(float dt)
	{
		if ((bool)HandsContainer.WeaponRootAnim)
		{
			if (IsAiming)
			{
				_vCameraTarget = HandsContainer.CameraTransform.parent.InverseTransformPoint(CurrentScope.Bone.position);
				_vCameraTarget.z += _cameraShiftToLineOfSight.x;
				_vCameraTarget.y += _cameraShiftToLineOfSight.y;
			}
			else
			{
				Vector3 position = HandsContainer.WeaponRoot.parent.TransformPoint(HandsContainer.CameraOffset);
				_vCameraTarget = HandsContainer.CameraTransform.parent.InverseTransformPoint(position);
			}
			Vector3 localPosition = HandsContainer.CameraTransform.localPosition;
			Vector2 a = new Vector2(localPosition.x, localPosition.y);
			Vector2 b = new Vector2(_vCameraTarget.x, _vCameraTarget.y);
			float num = _E019 * CameraSmoothBlender.Value * dt;
			Vector2 vector = Vector2.Lerp(a, b, num);
			float z = Mathf.Lerp(localPosition.z, _vCameraTarget.z, num / 4f);
			Vector3 localPosition2 = new Vector3(vector.x, vector.y, z) + HandsContainer.CameraPosition.GetRelative();
			HandsContainer.CameraTransform.localPosition = localPosition2;
			Quaternion quaternion = HandsContainer.CameraAnimatedFP.localRotation * HandsContainer.CameraAnimatedTP.localRotation;
			HandsContainer.CameraTransform.localRotation = quaternion * Quaternion.Euler(HandsContainer.CameraRotation.Get() + _E039) * _E020;
		}
	}

	public void AdjustOpticsPAG17()
	{
		if (Math.Abs(Pitch - _E01E) > 0.1f)
		{
			Singleton<GameUI>.Instance.BattleUiScreen.ShowAmmoCountZeroingPanel(string.Format(_ED3E._E000(220056), 0f - Pitch, _agsDeltaHeight));
			_E01E = Pitch;
		}
		if (IsAiming)
		{
			float num = HandsContainer.Fireport.position.y - HandsContainer.RootJoint.position.y;
			float time = 0f;
			Vector3 vector = this.m__E016.Item.ZeroLevelPosition(-HandsContainer.Fireport.up, HandsContainer.Fireport.position, num + _agsDeltaHeight, out time);
			if (time > 0f)
			{
				_E028 = vector;
				_E005(_E028);
			}
		}
	}

	public void CalculateCameraPosition()
	{
		if (!HandsContainer.WeaponRootAnim)
		{
			return;
		}
		Vector3 vector = ((BlindfireBlender.Value > 0f) ? (BlindFireCamera * Mathf.Abs(BlindfireBlender.Value)) : (SideFireCamera * Mathf.Abs(BlindfireBlender.Value)));
		HandsContainer.CameraRotation.Zero = new Vector3(0f, 0f, SmoothedTilt * PossibleTilt) + vector * _E022;
		Vector3 zero = Vector3.zero;
		foreach (var activeBlend in ActiveBlends)
		{
			float num = activeBlend.Pose.Blend.Evaluate(activeBlend.Time);
			zero += activeBlend.Pose.CameraPosition * num;
			HandsContainer.CameraRotation.Zero += activeBlend.Pose.CameraRotation * num;
		}
		if (IsAiming && Mathf.Approximately(BlindfireBlender.Value, 0f) && ScopeAimTransforms.Count > 0)
		{
			if (_E023.Value > Mathf.Epsilon)
			{
				_vCameraTarget = this.m__E005 * _E38B.GetPositionRelativeToParent(HandsContainer.Weapon, CurrentScope.Bone) + HandsContainer.WeaponRoot.localPosition + this.m__E005 * HandsContainer.WeaponRootAnim.localPosition;
			}
			else
			{
				_vCameraTarget = HandsContainer.WeaponRoot.parent.InverseTransformPoint(CurrentScope.Bone.position);
			}
			if (_currentAimingPlane != null)
			{
				Transform aimPointParent = AimPointParent;
				Matrix4x4 matrix4x = Matrix4x4.TRS(aimPointParent.position, aimPointParent.rotation, _E046);
				float num2 = Mathf.Min(_currentAimingPlane.Depth, _farPlane.Depth - HandsContainer.Weapon.localPosition.y);
				_E03F.y = (_crankRecoil ? (0f - num2 + PositionZeroSum.y * 2f) : (0f - num2));
				Vector3 point = matrix4x.MultiplyPoint3x4(_E03F);
				Transform parent = HandsContainer.WeaponRoot.parent;
				matrix4x = Matrix4x4.TRS(parent.position, parent.rotation, _E046).inverse;
				_vCameraTarget.z = matrix4x.MultiplyPoint3x4(point).z + _fovCompensatoryDistance - TurnAway.Position.y + _cameraShiftToLineOfSight.x;
			}
			_vCameraTarget.y += _cameraShiftToLineOfSight.y;
		}
		else
		{
			_vCameraTarget = HandsContainer.CameraOffset + _E01C + TurnAway.CameraShift;
			_vCameraTarget = ((_E02C > 0f) ? (_vCameraTarget + zero) : _vCameraTarget);
		}
	}

	public void LerpCamera(float dt)
	{
		Vector3 localPosition = HandsContainer.CameraTransform.localPosition;
		Vector2 a = new Vector2(localPosition.x, localPosition.y);
		Vector2 b = new Vector2(_vCameraTarget.x, _vCameraTarget.y);
		float num = (IsAiming ? (_E019 * CameraSmoothBlender.Value * _E01A) : CameraSmoothOut);
		Vector2 vector = Vector2.Lerp(a, b, dt * num);
		float z = localPosition.z;
		float num2 = CameraSmoothTime * dt;
		float num3 = (IsAiming ? (1f + HandsContainer.HandsPosition.GetRelative().y * 100f + TurnAway.Position.y * 10f) : CameraSmoothOut);
		z = Mathf.Lerp(z, _vCameraTarget.z, num2 * num3);
		Vector3 localPosition2 = new Vector3(vector.x, vector.y, z) + HandsContainer.CameraPosition.GetRelative();
		if (_E025 > 0f)
		{
			float value = _E027.Value;
			if (IsAiming && value > 0f)
			{
				HandsContainer.SwaySpring.ApplyVelocity(_E026 * value);
			}
		}
		HandsContainer.CameraTransform.localPosition = localPosition2;
		Quaternion b2 = HandsContainer.CameraAnimatedFP.localRotation * HandsContainer.CameraAnimatedTP.localRotation;
		HandsContainer.CameraTransform.localRotation = Quaternion.Lerp(_E01F, b2, _E045 * (1f - _E023.Value)) * Quaternion.Euler(HandsContainer.CameraRotation.Get() + _E039) * _E020;
	}

	private void _E011()
	{
		if (ScopeAimTransforms.Count < 1)
		{
			return;
		}
		_currentAimingPlane = null;
		_E001 scope = CurrentScope;
		if (scope.Mod != null && !string.IsNullOrEmpty(scope.Mod.CustomAimPlane))
		{
			_currentAimingPlane = HandsContainer.CustomAimPlanes.FirstOrDefault((WeaponPrefab.AimPlane x) => x.Name == scope.Mod.CustomAimPlane);
		}
		if (_currentAimingPlane == null)
		{
			_currentAimingPlane = (scope.IsOptic ? HandsContainer.FarPlane : HandsContainer.DefaultAimPlane);
		}
		ScopePrefabCache scopePrefabCache = CurrentScope.ScopePrefabCache;
		if (!CurrentScope.IsOptic)
		{
			_fovCompensatoryDistance = (1f - _E021) * 0.075f;
		}
		else
		{
			_fovCompensatoryDistance = (scopePrefabCache.GetAnyOpticsDistanceToCamera() + _E01D) * (_E021 - 1f);
		}
	}

	private float _E012(Transform t)
	{
		if (t == null)
		{
			return 0f;
		}
		return 0f - HandsContainer.Weapon.InverseTransformPoint(t.position).y;
	}

	private void _E013()
	{
		if (ScopeAimTransforms.Count < 1)
		{
			_farPlane = HandsContainer.FarPlane;
			return;
		}
		float num = float.PositiveInfinity;
		_E01D = 0f;
		foreach (_E001 scopeAimTransform in ScopeAimTransforms)
		{
			if (!(scopeAimTransform.ScopePrefabCache == null) && scopeAimTransform.IsOptic)
			{
				float num2 = _E012(scopeAimTransform.Bone);
				num = Mathf.Min(num, num2 - scopeAimTransform.EyeGuardShift);
				_E01D = Mathf.Max(0f, num - HandsContainer.FarPlane.Depth);
			}
		}
		_farPlane = new WeaponPrefab.AimPlane
		{
			Depth = Mathf.Min(num, HandsContainer.FarPlane.Depth),
			Name = _ED3E._E000(220041)
		};
	}

	private void _E014()
	{
		UpdateTacticalReload();
		_E010();
		if (PointOfView == EPointOfView.FirstPerson)
		{
			if (!Sprint && AimIndex < ScopeAimTransforms.Count)
			{
				float x = ((!IsAiming) ? _E047 : (CurrentScope.IsOptic ? 35f : (_E047 - 15f)));
				if (this.m__E016 != null && !this.m__E016._player.MouseLookControl)
				{
					_E8A8.Instance.SetFov(x, 1f, !_E017);
				}
			}
			_E000();
		}
		Shootingg.Pose = Pose;
		Shootingg.Intensity = IntensityByAiming;
		Breath.Intensity = IntensityByPoseLevel * IntensityByAiming;
		UpdateSwayFactors();
		Breath.IsAiming = IsAiming;
		HandsContainer.HandsRotation.InputIntensity = (HandsContainer.HandsPosition.InputIntensity = IntensityByAiming * IntensityByAiming);
		TurnAway.IsInPronePose = _E02D == 0;
		Walk.AdjustPose();
	}

	public void UpdateSwayFactors()
	{
		_E032 = EFTHardSettings.Instance.SWAY_STRENGTH_PER_KG.Evaluate(_E031 * (1f + Overweight));
		_E033 = EFTHardSettings.Instance.DISPLACEMENT_STRENGTH_PER_KG.Evaluate(_E031 * (1f + Overweight));
		MotionReact.SwayFactors = new Vector3(_E032, IsAiming ? (_E032 * 0.3f) : _E032, _E032) * IntensityByAiming;
	}

	public void ProcessEffectors(float deltaTime, int nFixedFrames, Vector3 motion, Vector3 velocity)
	{
		MotionReact.Motion = motion;
		MotionReact.Velocity = velocity;
		_E03C.ProcessEffectors(this, deltaTime, nFixedFrames);
		_E03C.ApplyTransformations(this, deltaTime);
	}

	public void LateTransformations(float dt)
	{
		_E03C.LateTransformations(this, dt);
	}

	public void Shoot(float str = 1f)
	{
		if ((Mask & EProceduralAnimationMask.Shooting) != 0)
		{
			CameraSmoothBlender.ChangeValue(CameraSmoothRecoil * Aiming.CameraSnapGlobalMult, 0.2f);
			Shootingg.Process(str);
			if (PointOfView == EPointOfView.FirstPerson)
			{
				Breath.OnShot();
			}
		}
	}

	public void Reset()
	{
		HandsContainer.HandsRotation.Reset();
		HandsContainer.HandsPosition.Reset();
		TurnAway.OverlapDepth = 0f;
		IsGrenadeLauncher = false;
		ShotNeedsFovAdjustments = true;
		MotionReact.Reset();
	}

	private void _E015()
	{
		if (ScopeAimTransforms.Count < 1)
		{
			this.m__E014 = Quaternion.identity;
			this.m__E015 = 0f;
			return;
		}
		_E001 currentScope = CurrentScope;
		if ((bool)currentScope.Bone && Mathf.Abs(currentScope.Rotation) >= 1f)
		{
			this.m__E014 = Quaternion.Euler(0f, currentScope.Rotation, 0f);
			this.m__E015 = currentScope.Rotation;
		}
		else
		{
			this.m__E014 = Quaternion.identity;
			this.m__E015 = 0f;
		}
	}

	public void SetFovParams(float scale)
	{
		_E021 = scale;
		TurnAway.FovScale = scale;
		_E013();
		_E011();
	}

	internal void _E016(int currentScopeIndex)
	{
		if (currentScopeIndex >= ScopeAimTransforms.Count)
		{
			return;
		}
		IEnumerable<SightComponent> components = this.m__E016.Item.AllSlots.Select((Slot x) => x.ContainedItem).GetComponents<SightComponent>();
		int num = 0;
		foreach (SightComponent item in components)
		{
			if (currentScopeIndex == num)
			{
				if (ScopeAimTransforms[num].Mod == null)
				{
					item.SelectedScopeIndex = 0;
				}
				else
				{
					ScopeAimTransforms[num].Mod.SelectedScopeIndex = 0;
				}
				break;
			}
			if (currentScopeIndex > num && currentScopeIndex < num + item.ScopesCount)
			{
				if (ScopeAimTransforms[num].Mod == null)
				{
					item.SelectedScopeIndex = currentScopeIndex - num;
				}
				else
				{
					ScopeAimTransforms[num].Mod.SelectedScopeIndex = currentScopeIndex - num;
				}
				break;
			}
			num += item.ScopesCount;
		}
		_E00C();
		_E00D();
		_E015();
		_E011();
		_E014();
		_E001();
	}

	public void StartBlindFire(int blindfire)
	{
		BlindfireBlender.Target = blindfire;
	}

	public IEnumerator ApplyFovAdjustments(Player player)
	{
		_E03C.ApplyFovAdjustments(this, player);
		yield return ProceduralWeaponAnimation.m__E000;
		_E03C.ResetFovAdjustments(this, player);
	}

	public void ResetFovAdjustments(Player player)
	{
		_E03C.ResetFovAdjustments(this, player);
	}

	public void AgsCalibrate(bool calibrate)
	{
		_E001 currentScope = CurrentScope;
		if (!(currentScope.ScopePrefabCache == null) && currentScope.ScopePrefabCache.HasOptics)
		{
			_agsDeltaHeight = Mathf.Clamp(calibrate ? (_agsDeltaHeight + 0.5f) : (_agsDeltaHeight - 0.5f), AgsDeltaHeightRange.x, AgsDeltaHeightRange.y);
			Singleton<GameUI>.Instance.BattleUiScreen.ShowAmmoCountZeroingPanel(string.Format(_ED3E._E000(220092), 0f - Pitch, _agsDeltaHeight));
		}
	}

	public void UpdatePossibleTilt(float smoothedCharacterMovementSpeed, float smoothedPoseLevel)
	{
		PossibleTilt = _E03C.UpdatePossibleTilt(this, smoothedCharacterMovementSpeed, smoothedPoseLevel);
	}

	public void AssignAnimatorPose(AnimatorPose animatorPose)
	{
		bool flag = animatorPose == null;
		for (int i = 0; i < ActiveBlends.Count; i++)
		{
			bool flag2 = ActiveBlends[i].Pose == animatorPose;
			flag = flag || flag2;
			ActiveBlends[i] = (ActiveBlends[i].Pose, ActiveBlends[i].Time, flag2);
		}
		if (!flag)
		{
			ActiveBlends.Add((animatorPose, 0f, true));
			CustomEffector.Aim = false;
		}
	}

	public void ObservedCalibration()
	{
		if (!_E03D || this.m__E016 == null || this.m__E016.Item == null)
		{
			return;
		}
		SightComponent currentAimingMod = CurrentAimingMod;
		if (currentAimingMod != null)
		{
			this.m__E016.Item.CreateOpticCalibrationPoints(currentAimingMod);
			if (currentAimingMod.HasOpticCalibrationPoints(currentAimingMod.SelectedScopeIndex))
			{
				_E03B = currentAimingMod.GetCurrentOpticCalibrationPoint();
				_E00C();
			}
		}
	}

	public void StartFovCoroutine(Player player)
	{
		StartCoroutine(ApplyFovAdjustments(player));
	}
}
