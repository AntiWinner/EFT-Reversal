using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Comfort.Common;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT.InventoryLogic;

[Serializable]
public class Weapon : _EA40, IWeapon
{
	public enum EMalfunctionState
	{
		None,
		Misfire,
		Jam,
		HardSlide,
		SoftSlide,
		Feed
	}

	[Flags]
	public enum EMalfunctionSource
	{
		Durability = 0,
		Ammo = 1,
		Magazine = 2,
		Overheat = 4,
		ConsoleCommand = 8
	}

	public class MalfunctionState
	{
		public _EA12 AmmoToFire;

		public _EA12 AmmoWillBeLoadedToChamber;

		public _EA12 MalfunctionedAmmo;

		public float LastShotOverheat;

		public float LastShotTime;

		public float LastMalfunctionTime;

		public float OverheatBarrelMoveMult;

		public Vector2 OverheatBarrelMoveDir;

		public float OverheatFirerateMult;

		public bool OverheatFirerateMultInited;

		public bool SlideOnOverheatReached;

		public bool AutoshotChanceInited;

		public float AutoshotTime;

		private EMalfunctionState _state;

		private readonly List<string> _playersWhoKnowAboutMalfunction = new List<string>(3);

		private readonly List<string> _playersWhoKnowMalfType = new List<string>(3);

		private readonly Dictionary<string, EMalfunctionSource> _playersReducedMalfChances = new Dictionary<string, EMalfunctionSource>(3);

		public IEnumerable<string> PlayersWhoKnowAboutMalfunction => _playersWhoKnowAboutMalfunction;

		public IEnumerable<string> PlayersWhoKnowMalfType => _playersWhoKnowMalfType;

		public IDictionary<string, EMalfunctionSource> PlayersReducedMalfChances => _playersReducedMalfChances;

		public bool IsAnyMalfExceptMisfire
		{
			get
			{
				if (_state != EMalfunctionState.Feed && _state != EMalfunctionState.Jam && _state != EMalfunctionState.HardSlide)
				{
					return _state == EMalfunctionState.SoftSlide;
				}
				return true;
			}
		}

		public EMalfunctionState State
		{
			get
			{
				return _state;
			}
			set
			{
				if (_state != value)
				{
					_state = value;
					this.OnStateChanged?.Invoke();
				}
			}
		}

		public EMalfunctionSource Source { get; set; }

		public event Action OnStateChanged;

		public void ChangeStateSilent(EMalfunctionState state)
		{
			_state = state;
		}

		public bool IsKnownMalfunction(string profileId)
		{
			if (State != 0)
			{
				return _playersWhoKnowAboutMalfunction.Contains(profileId);
			}
			return false;
		}

		public bool IsKnownMalfType(string profileId)
		{
			if (State != 0)
			{
				return _playersWhoKnowMalfType.Contains(profileId);
			}
			return false;
		}

		public void AddPlayerWhoKnowMalfunction(string playerId, bool clearRest = false)
		{
			if (clearRest)
			{
				_playersWhoKnowAboutMalfunction.Clear();
			}
			else if (_playersWhoKnowAboutMalfunction.Contains(playerId))
			{
				return;
			}
			_playersWhoKnowAboutMalfunction.Add(playerId);
			this.OnStateChanged?.Invoke();
		}

		public void AddPlayerWhoKnowMalfType(string playerId)
		{
			if (!_playersWhoKnowMalfType.Contains(playerId))
			{
				_playersWhoKnowMalfType.Add(playerId);
				if (!_playersWhoKnowAboutMalfunction.Contains(playerId))
				{
					_playersWhoKnowAboutMalfunction.Add(playerId);
				}
				this.OnStateChanged?.Invoke();
			}
		}

		public void ClearPlayersWhoKnow()
		{
			_playersWhoKnowAboutMalfunction.Clear();
			_playersWhoKnowMalfType.Clear();
			this.OnStateChanged?.Invoke();
		}

		public void Repair()
		{
			_state = EMalfunctionState.None;
			ClearPlayersWhoKnow();
		}

		public bool HasMalfReduceChance(string profileId, EMalfunctionSource malfSource)
		{
			if (_playersReducedMalfChances.TryGetValue(profileId, out var value))
			{
				return value.HasFlag(malfSource);
			}
			return false;
		}

		public void AddMalfReduceChance(string profileId, EMalfunctionSource malfSource)
		{
			if (!_playersReducedMalfChances.ContainsKey(profileId))
			{
				_playersReducedMalfChances.Add(profileId, malfSource);
			}
			else if ((_playersReducedMalfChances[profileId] & malfSource) != malfSource)
			{
				_playersReducedMalfChances[profileId] |= malfSource;
			}
		}

		public void CopyFrom(_E655 descriptor, _E63B itemFactory)
		{
			_state = (EMalfunctionState)descriptor.Malfunction;
			LastShotOverheat = descriptor.LastShotOverheat;
			SlideOnOverheatReached = descriptor.SlideOnOverheatReached;
			LastShotTime = descriptor.LastShotTime;
			_playersWhoKnowAboutMalfunction.Clear();
			_playersWhoKnowAboutMalfunction.AddRange(descriptor.PlayersWhoKnowAboutMalfunction);
			_playersWhoKnowMalfType.Clear();
			_playersWhoKnowMalfType.AddRange(descriptor.PlayersWhoKnowMalfType);
			_playersReducedMalfChances.Clear();
			foreach (var (key, value) in descriptor.PlayersReducedMalfChances)
			{
				_playersReducedMalfChances.Add(key, (EMalfunctionSource)value);
			}
			string ammoToFireTemplateId = descriptor.AmmoToFireTemplateId;
			string ammoWillBeLoadedToChamberTemplateId = descriptor.AmmoWillBeLoadedToChamberTemplateId;
			string ammoMalfunctionedTemplateId = descriptor.AmmoMalfunctionedTemplateId;
			if (!string.IsNullOrEmpty(ammoToFireTemplateId))
			{
				AmmoToFire = (_EA12)itemFactory.CreateItem(new MongoID(newProcessId: false), ammoToFireTemplateId, null);
			}
			if (!string.IsNullOrEmpty(ammoWillBeLoadedToChamberTemplateId))
			{
				AmmoWillBeLoadedToChamber = (_EA12)itemFactory.CreateItem(new MongoID(newProcessId: false), ammoWillBeLoadedToChamberTemplateId, null);
			}
			if (!string.IsNullOrEmpty(ammoMalfunctionedTemplateId))
			{
				MalfunctionedAmmo = (_EA12)itemFactory.CreateItem(new MongoID(newProcessId: false), ammoMalfunctionedTemplateId, null);
			}
			this.OnStateChanged?.Invoke();
		}
	}

	[Flags]
	public enum EFireMode : byte
	{
		fullauto = 0,
		single = 1,
		doublet = 2,
		burst = 3,
		doubleaction = 4,
		semiauto = 5
	}

	public enum EReloadMode
	{
		ExternalMagazine,
		InternalMagazine,
		OnlyBarrel,
		ExternalMagazineWithInternalReloadSupport
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _E001
	{
		public _EB1E itemController;
	}

	public const string WEAPON_CLASS_PISTOL = "pistol";

	public const float MOA_ON_100_METERS = 2.9089f;

	private const float ERGONOMICS_MIN = 0f;

	private const float ERGONOMICS_MAX = 100f;

	[_E63D]
	public readonly RepairableComponent Repairable;

	[_E63D]
	public readonly FoldableComponent Foldable;

	[_E63D]
	public readonly FireModeComponent FireMode;

	[_E63D]
	public BuffComponent Buff;

	public Slot[] Chambers;

	private Vector3[] _opticCalibrationPoints;

	private _EC27 _opticTrajectoryInfosForAGS;

	public const float OVERHEAT_PROBLEMS_START = 100f;

	private static List<Mod> _preallocatedMods = new List<Mod>(63);

	public readonly MalfunctionState MalfState = new MalfunctionState();

	private Slot _magSlotCache;

	public _ECF5<int> AimIndex = new _ECF5<int>();

	public bool CompatibleAmmo;

	public bool Armed;

	public bool IsUnderBarrelDeviceActive;

	public bool CylinderHammerClosed;

	private AmmoTemplate[] _shellsInChambers;

	public EReloadMode ReloadMode
	{
		get
		{
			_EA6A currentMagazine = GetCurrentMagazine();
			EReloadMode reloadMode = GetTemplate<WeaponTemplate>().ReloadMode;
			if (currentMagazine == null)
			{
				return reloadMode;
			}
			EReloadMode reloadMagType = currentMagazine.ReloadMagType;
			if (reloadMagType == reloadMode)
			{
				return reloadMode;
			}
			if (reloadMode == EReloadMode.InternalMagazine && reloadMagType == EReloadMode.ExternalMagazine)
			{
				return EReloadMode.ExternalMagazineWithInternalReloadSupport;
			}
			throw new NotImplementedException(string.Format(_ED3E._E000(224855), reloadMode, reloadMagType));
		}
	}

	public string WeapClass => GetTemplate<WeaponTemplate>().weapClass;

	public EFireMode[] WeapFireType => GetTemplate<WeaponTemplate>().weapFireType;

	public float RecoilForceBack => GetTemplate<WeaponTemplate>().RecoilForceBack;

	public bool IsBoltCatch => GetTemplate<WeaponTemplate>().isBoltCatch;

	public bool MustBoltBeOpennedForExternalReload => GetTemplate<WeaponTemplate>().MustBoltBeOpennedForExternalReload;

	public bool MustBoltBeOpennedForInternalReload => GetTemplate<WeaponTemplate>().MustBoltBeOpennedForInternalReload;

	public bool BoltAction => GetTemplate<WeaponTemplate>().BoltAction;

	public float DoubleActionAccuracyPenalty => GetTemplate<WeaponTemplate>().DoubleActionAccuracyPenalty;

	public bool CompactHandling => GetTemplate<WeaponTemplate>().CompactHandling;

	public bool ManualBoltCatch => GetTemplate<WeaponTemplate>().ManualBoltCatch;

	public float SightingRange => GetTemplate<WeaponTemplate>().SightingRange;

	public int FireRate => GetTemplate<WeaponTemplate>().bFirerate;

	public bool AllowJam => GetTemplate<WeaponTemplate>().AllowJam;

	public bool AllowFeed => GetTemplate<WeaponTemplate>().AllowFeed;

	public bool AllowMisfire => GetTemplate<WeaponTemplate>().AllowMisfire;

	public bool AllowSlide => GetTemplate<WeaponTemplate>().AllowSlide;

	public bool AllowOverheat => GetTemplate<WeaponTemplate>().AllowOverheat;

	public bool AllowMalfunction
	{
		get
		{
			if (!AllowJam && !AllowFeed && !AllowMisfire)
			{
				return AllowSlide;
			}
			return true;
		}
	}

	public float BaseMalfunctionChance => GetTemplate<WeaponTemplate>().BaseMalfunctionChance;

	public float DurabilityBurnRatio => GetTemplate<WeaponTemplate>().DurabilityBurnRatio;

	public float HeatFactorGun => GetTemplate<WeaponTemplate>().HeatFactorGun;

	public float CoolFactorGun => GetTemplate<WeaponTemplate>().CoolFactorGun;

	public float HeatFactorByShot => GetTemplate<WeaponTemplate>().HeatFactorByShot;

	public bool IsFlareGun => GetTemplate<WeaponTemplate>().IsFlareGun;

	public bool IsOneOff => GetTemplate<WeaponTemplate>().IsOneoff;

	public bool IsGrenadeLauncher => GetTemplate<WeaponTemplate>().IsGrenadeLauncher;

	public bool NoFiremodeOnBoltcatch => GetTemplate<WeaponTemplate>().NoFiremodeOnBoltcatch;

	public string AmmoCaliber => GetTemplate<WeaponTemplate>().ammoCaliber.Replace(_ED3E._E000(227358), string.Empty);

	public int SingleFireRate
	{
		get
		{
			if (GetTemplate<WeaponTemplate>().SingleFireRate <= 0)
			{
				UnityEngine.Debug.LogErrorFormat(string.Format(_ED3E._E000(224955), GetTemplate<WeaponTemplate>().SingleFireRate, GetTemplate<WeaponTemplate>()._name, 240));
				GetTemplate<WeaponTemplate>().SingleFireRate = 240;
			}
			return Mathf.Max(GetTemplate<WeaponTemplate>().SingleFireRate, 240);
		}
	}

	public bool CanQueueSecondShot => GetTemplate<WeaponTemplate>().CanQueueSecondShot;

	public Slot FirstFreeChamberSlot
	{
		get
		{
			for (int i = 0; i < Chambers.Length; i++)
			{
				if (Chambers[i].ContainedItem == null)
				{
					return Chambers[i];
				}
			}
			return null;
		}
	}

	public Slot FirstLoadedChamberSlot
	{
		get
		{
			for (int i = 0; i < Chambers.Length; i++)
			{
				if (Chambers[i].ContainedItem != null)
				{
					return Chambers[i];
				}
			}
			return null;
		}
	}

	public Slot[] FreeChambersForLoading
	{
		get
		{
			List<Slot> list = new List<Slot>();
			for (int i = 0; i < Chambers.Length; i++)
			{
				if (Chambers[i].ContainedItem == null || (Chambers[i].ContainedItem is _EA12 obj && obj.IsUsed))
				{
					list.Add(Chambers[i]);
				}
			}
			return list.ToArray();
		}
	}

	public int FreeChamberSlotsCount => FreeChambersForLoading.Length;

	public bool IsMultiBarrel => Chambers.Length > 1;

	public bool SupportsInternalReload
	{
		get
		{
			if (ReloadMode != EReloadMode.InternalMagazine)
			{
				return ReloadMode == EReloadMode.ExternalMagazineWithInternalReloadSupport;
			}
			return true;
		}
	}

	public bool Folded => GetFoldable()?.Folded ?? false;

	public float DeviationCurve => this.GetItemComponentsInChildren<BarrelComponent>().FirstOrDefault()?.Template.DeviationCurve ?? Template.DeviationCurve;

	private float _E000
	{
		get
		{
			float num = this.GetItemComponentsInChildren<BarrelComponent>().FirstOrDefault()?.Template.DeviationMax ?? Template.DeviationMax;
			if (num != 0f)
			{
				return num;
			}
			return 100f;
		}
	}

	public float RecoilBase => Template.RecoilForceUp + Template.RecoilForceBack;

	public float RecoilDelta => (Folded ? Mods.Where((Mod mod) => !(mod is _EAA6)).Sum((Mod mod) => mod.Template.Recoil) : Mods.Sum((Mod mod) => mod.Template.Recoil)) / 100f;

	public float StockRecoilDelta => (Folded ? 0f : Mods.Where((Mod mod) => mod is _EAA6).Sum((Mod mod) => mod.Template.Recoil)) / 100f;

	public float RecoilTotal => RecoilBase + RecoilBase * RecoilDelta;

	public float ErgonomicsTotal => Template.Ergonomics * (1f + ErgonomicsDelta);

	public float ErgonomicsDelta => Mods.Sum((Mod mod) => mod.Template.Ergonomics) / Mathf.Max(1f, Template.Ergonomics);

	public int EmptyTacticalSlotCount => (from x in AllSlots
		where x.ID.Contains(_ED3E._E000(225028))
		where x.ContainedItem == null
		select x).Count();

	public bool CanReloadFast => Template.isFastReload;

	public bool CanLoadAmmoToChamber => Template.isChamberLoad;

	public float ShotgunDispersionBase => this.GetItemComponentsInChildren<BarrelComponent>().FirstOrDefault()?.Template.ShotgunDispersion ?? ((float)Template.ShotgunDispersion);

	public float TotalShotgunDispersion => ShotgunDispersionBase * (1f + CenterOfImpactDelta);

	public float CenterOfImpactBase => this.GetItemComponentsInChildren<BarrelComponent>().FirstOrDefault()?.Template.CenterOfImpact ?? Template.CenterOfImpact;

	public float CenterOfImpactDelta => (float)(-Mods.Sum((Mod mod) => mod.Accuracy)) / 100f;

	public float StockDoubleActionAccuracyPenaltyMult => Mods.FirstOrDefault((Mod mod) => mod is _EAA6)?.DoubleActionAccuracyPenaltyMult ?? 1f;

	public float TotalAccuracy => GetTotalCenterOfImpact(includeAmmo: true);

	public float VelocityBase => CurrentAmmoTemplate?.InitialSpeed ?? 0f;

	[CanBeNull]
	public AmmoTemplate CurrentAmmoTemplate
	{
		get
		{
			Slot slot = Chambers.FirstOrDefault();
			_EA12 obj = ((slot == null) ? null : (slot.ContainedItem as _EA12));
			if (obj != null)
			{
				return obj.Template as AmmoTemplate;
			}
			_EA6A currentMagazine = GetCurrentMagazine();
			if (currentMagazine != null && currentMagazine.Cartridges != null)
			{
				Item item = currentMagazine.FirstRealAmmo();
				if (item != null)
				{
					return item.Template as AmmoTemplate;
				}
			}
			AmmoTemplate defAmmoTemplate = Template.DefAmmoTemplate;
			if (defAmmoTemplate != null)
			{
				return defAmmoTemplate;
			}
			return null;
		}
	}

	public float VelocityDelta => (Mods.Sum((Mod mod) => mod.Template.Velocity) + Template.Velocity) / 100f;

	public float SpeedFactor => 1f + VelocityDelta;

	public float TotalVelocity => VelocityBase * SpeedFactor;

	public override IEnumerable<IContainer> Containers
	{
		get
		{
			foreach (IContainer container in base.Containers)
			{
				yield return container;
			}
			Slot[] chambers = Chambers;
			for (int i = 0; i < chambers.Length; i++)
			{
				yield return chambers[i];
			}
		}
	}

	public override List<EItemInfoButton> ItemInteractionButtons => base.ItemInteractionButtons.Concat(new List<EItemInfoButton>
	{
		EItemInfoButton.Modding,
		EItemInfoButton.EditBuild,
		EItemInfoButton.Reload,
		EItemInfoButton.Load,
		EItemInfoButton.Unload,
		EItemInfoButton.UnloadAmmo,
		EItemInfoButton.FindAmmo,
		EItemInfoButton.Equip,
		EItemInfoButton.Unequip,
		EItemInfoButton.Disassemble
	}).ToList();

	public Vector3[] OpticCalibrationPointsForAll => _opticCalibrationPoints;

	float IWeapon.Weight => base.Weight;

	float IWeapon.RecoilForceBack => RecoilForceBack;

	float IWeapon.RecoilBase => RecoilBase;

	float IWeapon.RecoilDelta => RecoilDelta;

	float IWeapon.SpeedFactor => SpeedFactor;

	bool IWeapon.IsUnderbarrelWeapon => false;

	MalfunctionState IWeapon.MalfState => MalfState;

	Item IWeapon.Item => this;

	WeaponTemplate IWeapon.WeaponTemplate => Template;

	public new WeaponTemplate Template => GetTemplate<WeaponTemplate>();

	public EFireMode SelectedFireMode => FireMode.FireMode;

	public bool HasChambers => Chambers.Length != 0;

	public int ChamberAmmoCount
	{
		get
		{
			int num = 0;
			for (int i = 0; i < Chambers.Length; i++)
			{
				num += ((Chambers[i].ContainedItem is _EA12 obj && !obj.IsUsed) ? 1 : 0);
			}
			return num;
		}
	}

	public int ShellsInWeaponCount
	{
		get
		{
			int num = 0;
			for (int i = 0; i < ShellsInChambers.Length; i++)
			{
				num += ((ShellsInChambers[i] != null) ? 1 : 0);
			}
			return num;
		}
	}

	public int ShellsInChamberCount
	{
		get
		{
			int num = 0;
			for (int i = 0; i < Chambers.Length; i++)
			{
				num += ((Chambers[i].ContainedItem is _EA12 obj && obj.IsUsed) ? 1 : 0);
			}
			return num;
		}
	}

	public Mod[] Mods => AllSlots.Select((Slot slot) => slot.ContainedItem).OfType<Mod>().ToArray();

	public override IEnumerable<Slot> AllSlots => base.AllSlots.Concat(Chambers);

	public AmmoTemplate[] ShellsInChambers
	{
		get
		{
			if (GetCurrentMagazine() is _EB13 obj && _shellsInChambers.Length < obj.MaxCount)
			{
				_shellsInChambers = new AmmoTemplate[obj.MaxCount];
			}
			return _shellsInChambers;
		}
		set
		{
			_shellsInChambers = value;
		}
	}

	public bool HasShellsInChamberBarrelOnlyWeapon
	{
		get
		{
			AmmoTemplate[] shellsInChambers = ShellsInChambers;
			for (int i = 0; i < shellsInChambers.Length; i++)
			{
				if (shellsInChambers[i] != null)
				{
					return true;
				}
			}
			return false;
		}
	}

	public event Func<EMalfunctionState, bool> OnMalfunctionValidate;

	public FoldableComponent GetFoldable()
	{
		return this.GetItemComponentsInChildren<FoldableComponent>().FirstOrDefault();
	}

	public Weapon(string id, WeaponTemplate template)
		: base(id, template)
	{
		Components.Add(Repairable = new RepairableComponent(this, template));
		if (template.Foldable || template.Retractable)
		{
			Components.Add(Foldable = new FoldableComponent(this, template));
		}
		Components.Add(Buff = new BuffComponent(this));
		Components.Add(FireMode = new FireModeComponent(this, template));
		Chambers = Array.ConvertAll(template.Chambers, (Slot x) => new Slot(x, this));
		ShellsInChambers = new AmmoTemplate[Chambers.Length];
		Attributes.Add(new _EB11(EItemAttributeId.Ergonomics)
		{
			Name = EItemAttributeId.Ergonomics.GetName(),
			Range = new Vector2(0f, 100f),
			Base = () => Mathf.RoundToInt(Template.Ergonomics),
			Delta = () => ErgonomicsDelta,
			StringValue = () => Mathf.Clamp(ErgonomicsTotal, 0f, 100f).ToString(_ED3E._E000(164283)),
			DisplayType = () => EItemAttributeDisplayType.FullBar
		});
		Attributes.Add(new _EB11(EItemAttributeId.CenterOfImpact)
		{
			Name = EItemAttributeId.CenterOfImpact.GetName(),
			Base = () => CenterOfImpactBase,
			Delta = delegate
			{
				float num = _E000(Repairable.TemplateDurability);
				float num2 = (GetBarrelDeviation() - num) / (this._E000 - num);
				return CenterOfImpactDelta + num2;
			},
			StringValue = () => (GetTotalCenterOfImpact(includeAmmo: true) * GetBarrelDeviation() * 100f / 2.9089f).ToString(_ED3E._E000(225047)) + _ED3E._E000(18502) + _ED3E._E000(227329).Localized(),
			Range = new Vector2(_EA1B.MaxCenterOfImpact + 0.1f, 0.001f),
			DisplayType = () => EItemAttributeDisplayType.FullBar,
			LessIsGood = true
		});
		Attributes.Add(new _EB11(EItemAttributeId.SightingRange)
		{
			Name = EItemAttributeId.SightingRange.GetName(),
			Base = () => GetSightingRange(),
			StringValue = () => (!(GetSightingRange() > 0f)) ? _ED3E._E000(225036) : GetSightingRange().ToString(),
			Range = new Vector2(0f, 5000f),
			DisplayType = () => EItemAttributeDisplayType.FullBar
		});
		Attributes.Add(new _EB11(EItemAttributeId.RecoilUp)
		{
			Name = EItemAttributeId.RecoilUp.GetName(),
			Range = new Vector2(0f, 1000f),
			LessIsGood = true,
			Base = () => Template.RecoilForceUp,
			Delta = () => RecoilDelta,
			StringValue = () => Mathf.RoundToInt(Template.RecoilForceUp + Template.RecoilForceUp * RecoilDelta).ToString(),
			DisplayType = () => EItemAttributeDisplayType.FullBar
		});
		Attributes.Add(new _EB11(EItemAttributeId.RecoilBack)
		{
			Name = EItemAttributeId.RecoilBack.GetName(),
			Range = new Vector2(0f, 1000f),
			LessIsGood = true,
			Base = () => Template.RecoilForceBack,
			Delta = () => RecoilDelta,
			StringValue = () => Mathf.RoundToInt(Template.RecoilForceBack + Template.RecoilForceBack * RecoilDelta).ToString(),
			DisplayType = () => EItemAttributeDisplayType.FullBar
		});
		Attributes.Add(new _EB11(EItemAttributeId.Velocity)
		{
			Name = EItemAttributeId.Velocity.GetName(),
			Range = new Vector2(0f, 1500f),
			Base = () => VelocityBase,
			Delta = () => VelocityDelta,
			StringValue = () => TotalVelocity.ToString(_ED3E._E000(27314)) + _ED3E._E000(18502) + _ED3E._E000(229348).Localized(),
			DisplayType = () => EItemAttributeDisplayType.FullBar
		});
		SafelyAddAttributeToList(new _EB11(EItemAttributeId.WeaponFireType)
		{
			Name = EItemAttributeId.WeaponFireType.GetName(),
			Base = () => WeapFireType.Length,
			StringValue = () => WeapFireType.CastToStringValue(_ED3E._E000(10270)),
			DisplayType = () => EItemAttributeDisplayType.Compact
		});
		SafelyAddAttributeToList(new _EB11(EItemAttributeId.AmmoCaliber)
		{
			Name = EItemAttributeId.AmmoCaliber.GetName(),
			Base = () => AmmoCaliber.Length,
			StringValue = () => AmmoCaliber.Localized(),
			DisplayType = () => EItemAttributeDisplayType.Compact
		});
		SafelyAddAttributeToList(new _EB11(EItemAttributeId.FireRate)
		{
			Name = EItemAttributeId.FireRate.GetName(),
			Base = () => Template.bFirerate,
			StringValue = () => Template.bFirerate + _ED3E._E000(18502) + _ED3E._E000(225032).Localized(),
			DisplayType = () => EItemAttributeDisplayType.Compact
		});
		SafelyAddAttributeToList(new _EB11(EItemAttributeId.EffectiveDist)
		{
			Name = EItemAttributeId.EffectiveDist.GetName(),
			Base = () => Template.bEffDist,
			StringValue = () => Template.bEffDist + _ED3E._E000(18502) + _ED3E._E000(215643).Localized(),
			DisplayType = () => EItemAttributeDisplayType.Compact
		});
	}

	public float GetSightingRange()
	{
		float a = 0f;
		if (Mods.Count() > 0)
		{
			a = Mods.Max((Mod x) => x.SightingRange);
		}
		return Mathf.Max(a, SightingRange);
	}

	public float GetTotalCenterOfImpact(bool includeAmmo)
	{
		float num = CenterOfImpactBase * (1f + CenterOfImpactDelta);
		if (!includeAmmo)
		{
			return num;
		}
		return num * (CurrentAmmoTemplate?.AmmoFactor ?? 1f);
	}

	public float GetBarrelDeviation()
	{
		return _E000(Repairable.Durability) * (float)Buff.WeaponSpread;
	}

	private float _E000(float durability)
	{
		float deviationCurve = DeviationCurve;
		float num = 2f * deviationCurve;
		float num2 = ((100f - num == 0f) ? (durability / num) : ((0f - deviationCurve + Mathf.Sqrt((0f - num + 100f) * durability + deviationCurve * deviationCurve)) / (0f - num + 100f)));
		float num3 = 1f - num2;
		float num4 = this._E000;
		return num3 * num3 * num4 + 2f * num2 * num3 * deviationCurve + num2 * num2;
	}

	public bool IsModSuitable(Mod mod)
	{
		Slot[] array = AllSlots.Where((Slot x) => !x.ID.StartsWith(_ED3E._E000(225081))).ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].CanAccept(mod))
			{
				return true;
			}
		}
		return false;
	}

	public override Item FindItem(string itemId)
	{
		if (Id == itemId)
		{
			return this;
		}
		for (int i = 0; i < Chambers.Length; i++)
		{
			Item item = Chambers[i].FindItem(itemId);
			if (item != null)
			{
				return item;
			}
		}
		return base.FindItem(itemId);
	}

	public override IContainer GetContainer(string containerId)
	{
		for (int i = 0; i < Chambers.Length; i++)
		{
			if (Chambers[i].ID == containerId)
			{
				return Chambers[i];
			}
		}
		return base.GetContainer(containerId);
	}

	[OnDeserializing]
	private void _E001(StreamingContext context)
	{
		Buff.DisableComponent();
	}

	public void CreateOpticCalibrationPoints(SightComponent sight)
	{
		for (int i = 0; i < sight.ScopesCount; i++)
		{
			if (sight.OpticCalibrationPoints == null)
			{
				sight.OpticCalibrationPoints = new Vector3[sight.ScopesCount][];
			}
			if (sight.GetScopeCalibrationDistances(i) != null)
			{
				if (sight.OpticCalibrationPoints == null)
				{
					RecalculateOpticCalibrationPoints();
				}
			}
			else if (sight.OpticCalibrationPoints[i] == null)
			{
				sight.OpticCalibrationPoints[i] = new Vector3[0];
			}
		}
	}

	public void RecalculateOpticCalibrationPoints()
	{
		SightComponent[] array = this.GetAllItemsFromCollection().GetComponents<SightComponent>().ToArray();
		foreach (SightComponent sightComponent in array)
		{
			if (sightComponent.OpticCalibrationPoints == null)
			{
				sightComponent.OpticCalibrationPoints = new Vector3[sightComponent.ScopesCount][];
			}
			for (int j = 0; j < sightComponent.ScopesCount; j++)
			{
				_E002(sightComponent, j);
			}
		}
	}

	private void _E002(SightComponent sight, int scopeIndex)
	{
		List<int> list = new List<int>();
		int[] scopeCalibrationDistances = sight.GetScopeCalibrationDistances(scopeIndex);
		if (scopeCalibrationDistances != null)
		{
			foreach (int item in scopeCalibrationDistances)
			{
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
		}
		list.Sort();
		AmmoTemplate defAmmoTemplate = Template.DefAmmoTemplate;
		if (defAmmoTemplate != null)
		{
			_opticCalibrationPoints = CreateOpticCalibrationData(list.ToArray(), defAmmoTemplate, SpeedFactor, 0.001f);
			Vector3[] array = new Vector3[scopeCalibrationDistances.Length];
			for (int j = 0; j < scopeCalibrationDistances.Length; j++)
			{
				int item2 = scopeCalibrationDistances[j];
				int num = list.IndexOf(item2);
				array[j] = _opticCalibrationPoints[num];
			}
			sight.OpticCalibrationPoints[scopeIndex] = array;
		}
		else
		{
			UnityEngine.Debug.LogError(_ED3E._E000(225017) + base.Name);
		}
	}

	public Vector3[] CreateOpticCalibrationData(int[] opticCalibrationDistances, AmmoTemplate ammoTemplate, float speedFactor, float deltaTime)
	{
		Vector3[] array = new Vector3[opticCalibrationDistances.Length];
		Vector3 zero = Vector3.zero;
		Vector3 forward = Vector3.forward;
		float num = ammoTemplate.InitialSpeed * speedFactor;
		Vector3 vector = forward * num;
		Vector3 currentPosition = zero;
		Vector3 velocity = vector;
		float num2 = 0f;
		float num3 = 0f;
		_EC26.FormTrajectory(zero, vector, ammoTemplate.BulletMassGram, ammoTemplate.BulletDiameterMilimeters, ammoTemplate.BallisticCoeficient, out var trajectoryInfo);
		for (int i = 0; i < opticCalibrationDistances.Length; i++)
		{
			float num4 = opticCalibrationDistances[i] * opticCalibrationDistances[i];
			while (num2 < num4)
			{
				_EC26.PredictedTrajectoryCalculation(out currentPosition, out velocity, trajectoryInfo, num3);
				num2 = Vector3.SqrMagnitude(zero - currentPosition);
				num3 += deltaTime;
			}
			array[i] = currentPosition;
		}
		return array;
	}

	public Vector3 CalculateShotDirectionForIron(Vector3 localPosition, float CachedSpeedFactor, int calibrationRange)
	{
		AmmoTemplate defAmmoTemplate = Template.DefAmmoTemplate;
		float x = defAmmoTemplate.InitialSpeed * CachedSpeedFactor;
		_EC26.FormTrajectory(Vector3.zero, new Vector3(x, 0f, 0f), defAmmoTemplate.BulletMassGram, defAmmoTemplate.BulletDiameterMilimeters, defAmmoTemplate.BallisticCoeficient, out var trajectoryInfo);
		float num = 0f;
		int num2 = 0;
		int num3 = trajectoryInfo.MaxAllowedLength - 1;
		while (num2 <= num3)
		{
			int num4 = (num2 + num3) / 2;
			if (trajectoryInfo[num4].position.magnitude < (float)calibrationRange && trajectoryInfo[num4 + 1].position.magnitude > (float)calibrationRange)
			{
				num = trajectoryInfo[num4].position.y;
				break;
			}
			if (trajectoryInfo[num4].position.magnitude > (float)calibrationRange && trajectoryInfo[num4 + 1].position.magnitude > (float)calibrationRange)
			{
				num3 = num4 - 1;
			}
			else
			{
				num2 = num4 + 1;
			}
		}
		trajectoryInfo.Reset();
		num2 = 0;
		num3 = trajectoryInfo.MaxAllowedLength - 1;
		while (num2 <= num3)
		{
			int num5 = (num2 + num3) / 2;
			if (trajectoryInfo[num5].position.x < (float)calibrationRange && trajectoryInfo[num5 + 1].position.x > (float)calibrationRange)
			{
				num = trajectoryInfo[num5].position.y;
				break;
			}
			if (trajectoryInfo[num5].position.x > (float)calibrationRange && trajectoryInfo[num5 + 1].position.x > (float)calibrationRange)
			{
				num3 = num5 - 1;
			}
			else
			{
				num2 = num5 + 1;
			}
		}
		return new Vector3(0f, -1f, (localPosition.y - num) / localPosition.z);
	}

	public Vector3 ZeroLevelPosition(Vector3 direction, Vector3 startPosition, float height, out float time)
	{
		AmmoTemplate defAmmoTemplate = Template.DefAmmoTemplate;
		float initialSpeed = defAmmoTemplate.InitialSpeed;
		Vector3 vector = direction * initialSpeed;
		Vector3 velocity = vector;
		float num = _EC26.TimeToLevel(height, vector.y);
		_EC26.FormTrajectory(startPosition, vector, defAmmoTemplate.BulletMassGram, defAmmoTemplate.BulletDiameterMilimeters, defAmmoTemplate.BallisticCoeficient, out _opticTrajectoryInfosForAGS);
		time = num;
		_EC26.PredictedTrajectoryCalculation(out var currentPosition, out velocity, _opticTrajectoryInfosForAGS, num);
		return currentPosition;
	}

	public void OnShot(float ammoBurnRatio, float ammoHeatFactor, float skillWeaponTreatmentFactor, _E5CB._E02D overheatSettings, float pastTime)
	{
		if (AllowOverheat)
		{
			float modsCoolFactor;
			float currentOverheat = GetCurrentOverheat(pastTime, overheatSettings, out modsCoolFactor);
			float modsHeatFactor;
			float shotOverheat = GetShotOverheat(ammoHeatFactor, overheatSettings.ModHeatFactor, out modsHeatFactor);
			MalfState.OverheatBarrelMoveDir = GetCurrentOverheatBarrelMove(pastTime, overheatSettings.BarrelMoveRndDuration);
			MalfState.LastShotOverheat = Mathf.Clamp(currentOverheat + shotOverheat, overheatSettings.MinOverheat, overheatSettings.MaxOverheat);
			MalfState.LastShotTime = pastTime;
		}
		else
		{
			MalfState.LastShotOverheat = 0f;
		}
		float num = Mathf.Clamp(MalfState.LastShotOverheat, overheatSettings.OverheatProblemsStart, overheatSettings.MaxOverheat) / 100f;
		float overheatFactor = Mathf.Lerp(overheatSettings.DurReduceMinMult, overheatSettings.DurReduceMaxMult, num - 1f);
		MalfState.OverheatBarrelMoveMult = (AllowOverheat ? Mathf.Lerp(0f, overheatSettings.BarrelMoveMaxMult, num - 1f) : 0f);
		if (Repairable.Durability > 0f)
		{
			Repairable.Durability -= GetDurabilityLossOnShot(ammoBurnRatio, overheatFactor, skillWeaponTreatmentFactor, out var _);
			Repairable.Durability = Mathf.Max(Repairable.Durability, 0f);
			Buff.TryDisableComponent(Repairable.Durability);
		}
		if (Repairable.Durability < 0f)
		{
			Repairable.Durability = 0f;
		}
		if (MalfState.LastShotOverheat >= overheatSettings.OverheatProblemsStart && Repairable.MaxDurability / 100f >= overheatSettings.OverheatWearLimit)
		{
			num -= 1f;
			float num2 = 0f;
			num2 = ((!(Mathf.Abs(MalfState.LastShotOverheat - overheatSettings.MaxOverheat) <= Mathf.Epsilon)) ? Mathf.Lerp(overheatSettings.MinWearOnOverheat, overheatSettings.MaxWearOnOverheat, num) : UnityEngine.Random.Range(overheatSettings.MinWearOnMaxOverheat, overheatSettings.MaxWearOnMaxOverheat));
			if (Repairable.Durability < Repairable.MaxDurability - num2)
			{
				Repairable.MaxDurability = Mathf.Max(Repairable.MaxDurability - num2, overheatSettings.OverheatWearLimit * 100f);
			}
		}
		if (overheatSettings.EnableSlideOnMaxOverheat)
		{
			if (!MalfState.SlideOnOverheatReached && MalfState.LastShotOverheat >= overheatSettings.StartSlideOverheat)
			{
				MalfState.SlideOnOverheatReached = true;
			}
			if (MalfState.LastShotOverheat <= overheatSettings.FixSlideOverheat)
			{
				MalfState.SlideOnOverheatReached = false;
			}
		}
		if (MalfState.LastShotOverheat > overheatSettings.FirerateOverheatBorder)
		{
			if (!MalfState.OverheatFirerateMultInited)
			{
				MalfState.OverheatFirerateMultInited = true;
				MalfState.OverheatFirerateMult = UnityEngine.Random.Range(overheatSettings.FirerateReduceMinMult, overheatSettings.FirerateReduceMaxMult);
			}
		}
		else
		{
			MalfState.OverheatFirerateMult = 0f;
			MalfState.OverheatFirerateMultInited = false;
		}
		if (MalfState.LastShotOverheat >= overheatSettings.AutoshotMinOverheat)
		{
			if (!MalfState.AutoshotChanceInited)
			{
				float num3 = UnityEngine.Random.Range(0.05f, overheatSettings.AutoshotPossibilityDuration);
				bool flag = UnityEngine.Random.Range(0f, 1f) <= overheatSettings.AutoshotChance;
				MalfState.AutoshotTime = (flag ? (MalfState.LastShotTime + num3) : (-1f));
				MalfState.AutoshotChanceInited = true;
			}
		}
		else
		{
			MalfState.AutoshotChanceInited = false;
			MalfState.AutoshotTime = -1f;
		}
	}

	public bool CanQuickdrawPistolAfterMalf(float pastTime, _E5CB._E02C malfSettings)
	{
		return pastTime - MalfState.LastMalfunctionTime < malfSettings.TimeToQuickdrawPistol;
	}

	public float GetCurrentOverheat(float pastTime, _E5CB._E02D overheatSettings, out float modsCoolFactor)
	{
		if (MalfState.LastShotOverheat <= 0f)
		{
			modsCoolFactor = 1f;
			return 0f;
		}
		modsCoolFactor = Template.CoolFactorGunMods;
		_preallocatedMods.Clear();
		this.GetAllItemsNonAlloc(_preallocatedMods);
		foreach (Mod preallocatedMod in _preallocatedMods)
		{
			modsCoolFactor *= preallocatedMod.CoolFactor;
		}
		_preallocatedMods.Clear();
		float num = modsCoolFactor * overheatSettings.ModCoolFactor;
		float num2 = pastTime - MalfState.LastShotTime;
		float lastShotOverheat = MalfState.LastShotOverheat;
		return Mathf.Clamp(lastShotOverheat - num2 * lastShotOverheat * CoolFactorGun * num / (lastShotOverheat - Mathf.Pow(lastShotOverheat / overheatSettings.MaxOverheat + overheatSettings.MaxOverheatCoolCoef * 0.0338f + 0.2925f, 9.667f - overheatSettings.MaxOverheatCoolCoef * -0.0668f)), overheatSettings.MinOverheat, overheatSettings.MaxOverheat);
	}

	public Vector2 GetCurrentOverheatBarrelMove(float pastTime, float timeToChangeDir)
	{
		if (!(pastTime - MalfState.LastShotTime < timeToChangeDir))
		{
			return UnityEngine.Random.insideUnitCircle;
		}
		return MalfState.OverheatBarrelMoveDir;
	}

	public float GetDurabilityLossOnShot(float ammoBurnRatio, float overheatFactor, float skillWeaponTreatmentFactor, out float modsBurnRatio)
	{
		modsBurnRatio = 1f;
		Mod[] mods = Mods;
		foreach (Mod mod in mods)
		{
			modsBurnRatio *= mod.DurabilityBurnModificator;
		}
		return (float)Repairable.TemplateDurability / Template.OperatingResource * DurabilityBurnRatio * (modsBurnRatio * ammoBurnRatio) * overheatFactor * (1f - skillWeaponTreatmentFactor);
	}

	public float GetShotOverheat(float ammoHeatFactor, float globalModHeatFactor, out float modsHeatFactor)
	{
		modsHeatFactor = ammoHeatFactor;
		Mod[] mods = Mods;
		foreach (Mod mod in mods)
		{
			modsHeatFactor *= mod.HeatFactor;
		}
		return Template.HeatFactorByShot * modsHeatFactor * Template.HeatFactorGun * globalModHeatFactor;
	}

	public bool ValidateMalfunction(EMalfunctionState malfState)
	{
		if (this.OnMalfunctionValidate == null)
		{
			return true;
		}
		return this.OnMalfunctionValidate(malfState);
	}

	int IWeapon.GetCurrentMagazineCount()
	{
		return GetCurrentMagazineCount();
	}

	public override int GetHashSum()
	{
		int num = base.GetHashSum();
		if (Chambers == null)
		{
			return num;
		}
		for (int i = 0; i < Chambers.Length; i++)
		{
			num = num * 23 + Chambers[i].GetHashSum();
		}
		return num;
	}

	public int GetModsHashSumWithoutMag()
	{
		int num = 0;
		Slot[] slots = Slots;
		foreach (Slot slot in slots)
		{
			if (slot.ContainedItem != null && !(slot.ContainedItem is _EA6A) && !(slot.ContainedItem is _EA12))
			{
				num += slot.ContainedItem.GetHashSum();
			}
		}
		return num;
	}

	[CanBeNull]
	public T GetFirstOrDefaultMod<T>()
	{
		return Mods.OfType<T>().FirstOrDefault();
	}

	public void GetShellsIndexes(List<int> shellsIndexes)
	{
		shellsIndexes.Clear();
		for (int i = 0; i < _shellsInChambers.Length; i++)
		{
			if (_shellsInChambers[i] != null)
			{
				shellsIndexes.Add(i);
			}
		}
	}

	public int GetShellsInWeaponCount()
	{
		int num = 0;
		for (int i = 0; i < _shellsInChambers.Length; i++)
		{
			if (_shellsInChambers[i] != null)
			{
				num++;
			}
		}
		return num;
	}

	[CanBeNull]
	public Slot GetMagazineSlot()
	{
		return _magSlotCache ?? (_magSlotCache = Array.Find(Slots, (Slot x) => x.ID == EWeaponModType.mod_magazine.ToString()));
	}

	public override _EA6A GetCurrentMagazine()
	{
		return (_EA6A)(GetMagazineSlot()?.ContainedItem);
	}

	public int GetCurrentMagazineCount()
	{
		return GetCurrentMagazine()?.Count ?? 0;
	}

	public int GetMaxMagazineCount()
	{
		return GetCurrentMagazine()?.MaxCount ?? 0;
	}

	[CanBeNull]
	public _EA62 GetUnderbarrelWeapon()
	{
		return AllSlots.FirstOrDefault((Slot slot) => slot.ContainedItem is _EA62)?.ContainedItem as _EA62;
	}

	[CanBeNull]
	public Slot GetLauncherSlot()
	{
		return AllSlots.FirstOrDefault((Slot slot) => slot.ContainedItem is _EA62);
	}

	public override _ECD7 Apply([NotNull] _EB1E itemController, [NotNull] Item item, int count, bool simulate)
	{
		_E001 obj = default(_E001);
		obj.itemController = itemController;
		if (!obj.itemController.Examined(item))
		{
			return new _E9FC(item);
		}
		if (!obj.itemController.Examined(this))
		{
			return new _E9FC(this);
		}
		Slot magazineSlot = GetMagazineSlot();
		_ECD1 error;
		if (item is _EA6A item2 && magazineSlot != null)
		{
			if (!magazineSlot.CanAccept(item2))
			{
				return new Slot._E008(item2, magazineSlot);
			}
			_EB20 obj2 = new _EB20(magazineSlot);
			IResult result = _E01E(obj2, ref obj);
			if (result.Failed)
			{
				return new _ECD2(result.Error);
			}
			_ECD8<_EB3B> obj3 = _EB29.Move(item2, obj2, obj.itemController, simulate);
			if (obj3.Succeeded)
			{
				return obj3;
			}
			error = obj3.Error;
			Item containedItem = magazineSlot.ContainedItem;
			if (!_E38D.DisabledForNow && containedItem != null && _EB2A.CanSwap(item2, magazineSlot))
			{
				return new _ECD7((_EB2D)null);
			}
		}
		else
		{
			if (item is _EA12 && IsMultiBarrel)
			{
				_ECD7 result2 = base.Apply(obj.itemController, item, count, simulate);
				if (result2.Succeeded)
				{
					return result2;
				}
				return result2.Error;
			}
			_ECD7 result3 = base.Apply(obj.itemController, item, count, simulate);
			if (result3.Succeeded)
			{
				return result3;
			}
			error = result3.Error;
			if (!(item is _EA12 obj4) || !(magazineSlot?.ContainedItem is _EA6A obj5) || !SupportsInternalReload || obj5.MaxCount <= obj5.Count || this.Contains(obj4))
			{
				return error;
			}
			IResult result4 = _E01E(new _EB20(magazineSlot), ref obj);
			if (result4.Failed)
			{
				return new _ECD2(result4.Error);
			}
			_ECD7 result5 = obj5.ApplyWithoutRestrictions(obj.itemController, obj4, count, simulate);
			if (result5.Succeeded)
			{
				return result5;
			}
			error = result5.Error;
		}
		return error;
	}

	[CompilerGenerated]
	private Slot _E003(Slot x)
	{
		return new Slot(x, this);
	}

	[CompilerGenerated]
	private float _E004()
	{
		return Mathf.RoundToInt(Template.Ergonomics);
	}

	[CompilerGenerated]
	private float _E005()
	{
		return ErgonomicsDelta;
	}

	[CompilerGenerated]
	private string _E006()
	{
		return Mathf.Clamp(ErgonomicsTotal, 0f, 100f).ToString(_ED3E._E000(164283));
	}

	[CompilerGenerated]
	private float _E007()
	{
		return CenterOfImpactBase;
	}

	[CompilerGenerated]
	private float _E008()
	{
		float num = _E000(Repairable.TemplateDurability);
		float num2 = (GetBarrelDeviation() - num) / (this._E000 - num);
		return CenterOfImpactDelta + num2;
	}

	[CompilerGenerated]
	private string _E009()
	{
		return (GetTotalCenterOfImpact(includeAmmo: true) * GetBarrelDeviation() * 100f / 2.9089f).ToString(_ED3E._E000(225047)) + _ED3E._E000(18502) + _ED3E._E000(227329).Localized();
	}

	[CompilerGenerated]
	private float _E00A()
	{
		return GetSightingRange();
	}

	[CompilerGenerated]
	private string _E00B()
	{
		if (!(GetSightingRange() > 0f))
		{
			return _ED3E._E000(225036);
		}
		return GetSightingRange().ToString();
	}

	[CompilerGenerated]
	private float _E00C()
	{
		return Template.RecoilForceUp;
	}

	[CompilerGenerated]
	private float _E00D()
	{
		return RecoilDelta;
	}

	[CompilerGenerated]
	private string _E00E()
	{
		return Mathf.RoundToInt(Template.RecoilForceUp + Template.RecoilForceUp * RecoilDelta).ToString();
	}

	[CompilerGenerated]
	private float _E00F()
	{
		return Template.RecoilForceBack;
	}

	[CompilerGenerated]
	private float _E010()
	{
		return RecoilDelta;
	}

	[CompilerGenerated]
	private string _E011()
	{
		return Mathf.RoundToInt(Template.RecoilForceBack + Template.RecoilForceBack * RecoilDelta).ToString();
	}

	[CompilerGenerated]
	private float _E012()
	{
		return VelocityBase;
	}

	[CompilerGenerated]
	private float _E013()
	{
		return VelocityDelta;
	}

	[CompilerGenerated]
	private string _E014()
	{
		return TotalVelocity.ToString(_ED3E._E000(27314)) + _ED3E._E000(18502) + _ED3E._E000(229348).Localized();
	}

	[CompilerGenerated]
	private float _E015()
	{
		return WeapFireType.Length;
	}

	[CompilerGenerated]
	private string _E016()
	{
		return WeapFireType.CastToStringValue(_ED3E._E000(10270));
	}

	[CompilerGenerated]
	private float _E017()
	{
		return AmmoCaliber.Length;
	}

	[CompilerGenerated]
	private string _E018()
	{
		return AmmoCaliber.Localized();
	}

	[CompilerGenerated]
	private float _E019()
	{
		return Template.bFirerate;
	}

	[CompilerGenerated]
	private string _E01A()
	{
		return Template.bFirerate + _ED3E._E000(18502) + _ED3E._E000(225032).Localized();
	}

	[CompilerGenerated]
	private float _E01B()
	{
		return Template.bEffDist;
	}

	[CompilerGenerated]
	private string _E01C()
	{
		return Template.bEffDist + _ED3E._E000(18502) + _ED3E._E000(215643).Localized();
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private IEnumerable<IContainer> _E01D()
	{
		return base.Containers;
	}

	[CompilerGenerated]
	internal static IResult _E01E(_EB20 slotItemAddress, ref _E001 P_1)
	{
		if (P_1.itemController is _EAED obj && obj.Inventory.Equipment.GetContainerSlots().Contains(slotItemAddress.Slot) && P_1.itemController.SelectEvents(null).Any())
		{
			return new FailedResult(_ED3E._E000(213804));
		}
		return SuccessfulResult.New;
	}
}
