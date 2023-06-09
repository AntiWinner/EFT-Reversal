using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Cinemachine;
using Comfort.Common;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT.Hideout;

public sealed class AreaData
{
	[CompilerGenerated]
	private sealed class _E003
	{
		public EAreaType areaType;
	}

	[CompilerGenerated]
	private sealed class _E004
	{
		public int level;

		public _E003 CS_0024_003C_003E8__locals1;

		public Func<Requirement, bool> _003C_003E9__1;

		internal bool _E000(_E828 scheme)
		{
			return !scheme.requirements.Any((Requirement r) => r is AreaRequirement areaRequirement && areaRequirement.AreaType == CS_0024_003C_003E8__locals1.areaType && areaRequirement.RequiredLevel > level);
		}

		internal bool _E001(Requirement r)
		{
			if (r is AreaRequirement areaRequirement && areaRequirement.AreaType == CS_0024_003C_003E8__locals1.areaType)
			{
				return areaRequirement.RequiredLevel > level;
			}
			return false;
		}
	}

	private const string CONSTRUCT_LABEL = "CONSTRUCT";

	private const string CONSTRUCTING_LABEL = "CONSTRUCTING";

	private const string INSTALL_LABEL = "INSTALL";

	private const string UPGRADE_LABEL = "UPGRADE";

	private const string UPGRADING_LABEL = "UPGRADING";

	public readonly _ECEC LevelUpdated = new _ECEC();

	public readonly _ECEC StatusUpdated = new _ECEC();

	public readonly _ECEC AreaUpgraded = new _ECEC();

	public readonly _ECEC RecalculateAreas = new _ECEC();

	public readonly _ECED<ELightStatus> LightStatusChanged = new _ECED<ELightStatus>();

	public readonly _ECED<bool> ProductionStateChanged = new _ECED<bool>();

	public readonly _ECEC ImprovementsUpdated = new _ECEC();

	public readonly _ECED<bool> OnHover = new _ECED<bool>();

	public readonly _ECED<bool> OnSelected = new _ECED<bool>();

	public readonly _ECEC IconStatusUpdated = new _ECEC();

	public readonly _ECED<bool> VisibilityChanged = new _ECED<bool>();

	private HideoutArea _hideoutArea;

	private int _actionGoingStatus;

	private bool _isVisible = true;

	private int _currentLevel;

	private EAreaStatus _status;

	private ELightStatus _lightStatus;

	public AreaTemplate Template { get; }

	public Transform AreaIconPoint { get; private set; }

	public CinemachineVirtualCamera AreaCamera { get; private set; }

	public CinemachineVirtualCamera SpecialActionCamera { get; private set; }

	public bool Selected { get; private set; }

	public bool IsActive { get; set; }

	public bool HasActiveProduction { get; private set; }

	public Transform HighlightTransform => _hideoutArea.HighlightTransform;

	public Color HighlightColor => _hideoutArea.HighlightColor;

	public Stage CurrentStage => StageAt(CurrentLevel);

	public Stage NextStage => StageAt(CurrentLevel + 1);

	public bool IsVisible => _isVisible;

	public int CurrentLevel
	{
		get
		{
			return _currentLevel;
		}
		private set
		{
			if (_currentLevel != value)
			{
				_currentLevel = value;
			}
		}
	}

	public EAreaStatus Status
	{
		get
		{
			return _status;
		}
		private set
		{
			if (_status != value)
			{
				_status = value;
				StatusUpdated.Invoke();
			}
		}
	}

	public bool IsInstalled
	{
		get
		{
			if (Status != EAreaStatus.Upgrading && Status != EAreaStatus.LockedToUpgrade && Status != EAreaStatus.NoFutureUpgrades && Status != EAreaStatus.ReadyToUpgrade)
			{
				return Status == EAreaStatus.ReadyToInstallUpgrade;
			}
			return true;
		}
	}

	public bool ReadyForAction
	{
		get
		{
			if (Status != EAreaStatus.ReadyToConstruct && Status != EAreaStatus.ReadyToUpgrade && Status != EAreaStatus.ReadyToInstallConstruct)
			{
				return Status == EAreaStatus.ReadyToInstallUpgrade;
			}
			return true;
		}
	}

	public ELightStatus LightStatus
	{
		get
		{
			return _lightStatus;
		}
		set
		{
			if (_lightStatus != value)
			{
				_lightStatus = value;
				LightStatusChanged.Invoke(_lightStatus);
			}
		}
	}

	private TimeSpan _E000
	{
		get
		{
			if (!NextStage.ConstructionTime.IsActive)
			{
				return TimeSpan.Zero;
			}
			DateTime startTime = CurrentStage.StartTime;
			float data = NextStage.ConstructionTime.Data;
			DateTime utcNow = _E5AD.UtcNow;
			return startTime.AddSeconds(data) - utcNow;
		}
	}

	public string TimeLeftFormatted
	{
		get
		{
			var (num4, num5, num6) = this._E000;
			return string.Format(_ED3E._E000(59439), num4, num5, num6);
		}
	}

	public (EButtonDisplayStatus, string) ActionButtonStatus
	{
		get
		{
			switch (Status)
			{
			case EAreaStatus.LockedToConstruct:
				return (EButtonDisplayStatus.GrayedOut, _ED3E._E000(165115));
			case EAreaStatus.ReadyToConstruct:
				return (EButtonDisplayStatus.Enabled, _ED3E._E000(165115));
			case EAreaStatus.Constructing:
				return (EButtonDisplayStatus.GrayedOut, _ED3E._E000(165101));
			case EAreaStatus.ReadyToInstallConstruct:
				return (EButtonDisplayStatus.Enabled, _ED3E._E000(165090));
			case EAreaStatus.LockedToUpgrade:
				return (EButtonDisplayStatus.GrayedOut, _ED3E._E000(165146));
			case EAreaStatus.ReadyToUpgrade:
				return (EButtonDisplayStatus.Enabled, _ED3E._E000(165146));
			case EAreaStatus.Upgrading:
			case EAreaStatus.AutoUpgrading:
				return (EButtonDisplayStatus.GrayedOut, _ED3E._E000(165138));
			case EAreaStatus.ReadyToInstallUpgrade:
				return (EButtonDisplayStatus.Enabled, _ED3E._E000(165090));
			case EAreaStatus.NotSet:
			case EAreaStatus.NoFutureUpgrades:
				return (EButtonDisplayStatus.Disabled, string.Empty);
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
	}

	public bool DisplayOutOfFuelIcon
	{
		get
		{
			if (!Template.NeedsFuel)
			{
				return false;
			}
			if (CurrentStage.Production.IsActive && Template.Type != EAreaType.ScavCase)
			{
				return true;
			}
			if (CurrentStage.FuelSupply.IsActive)
			{
				return true;
			}
			if (Template.Type == EAreaType.Generator || Template.Type == EAreaType.Illumination)
			{
				return true;
			}
			return false;
		}
	}

	public bool DisplayInterface => CurrentStage.DisplayInterface;

	public bool DisplayLevel => Template.DisplayLevel;

	public bool DisplayNotification
	{
		get
		{
			Stage nextStage = NextStage;
			if (nextStage == null)
			{
				return true;
			}
			return !nextStage.AutoUpgrade;
		}
	}

	public RelatedRequirements Requirements => Template.Requirements;

	public bool Enabled => Template.Enabled;

	public void SetProductionState(bool active)
	{
		if (HasActiveProduction != active)
		{
			HasActiveProduction = active;
			ProductionStateChanged.Invoke(HasActiveProduction);
		}
	}

	public AreaData(AreaTemplate template)
	{
		template.Init();
		Template = template;
		_isVisible = true;
	}

	public void InitArea(HideoutArea area)
	{
		_hideoutArea = area;
		AreaIconPoint = area.AreaIconPoint;
		AreaCamera = area.AreaCamera;
		SpecialActionCamera = area.SpecialActionCamera;
	}

	public Stage StageAt(int level)
	{
		if (!Template.Stages.TryGetValue(level, out var value))
		{
			return new Stage();
		}
		return value;
	}

	public void InitProfileData(AreaInfo areaInfo, _E74F skills)
	{
		IsActive = areaInfo.active;
		_E001(areaInfo.level);
		if (areaInfo.constructing)
		{
			InitConstructing(areaInfo.completeTime.GetValueOrDefault()).HandleExceptions();
		}
		Template.AreaBehaviour.Init(this, areaInfo, skills);
	}

	public void ReleaseSubscriptions()
	{
		Template.AreaBehaviour.ReleaseSubscriptions();
	}

	public async Task InitConstructing(int timestamp)
	{
		Stage currentStage = CurrentStage;
		int num = timestamp - _E5AD.UtcNowUnixInt;
		if (currentStage.Waiting)
		{
			if (Status == EAreaStatus.NotSet)
			{
				Status = ((CurrentLevel <= 0) ? EAreaStatus.Constructing : (NextStage.AutoUpgrade ? EAreaStatus.AutoUpgrading : EAreaStatus.Upgrading));
			}
			if (num > 0)
			{
				await TasksExtensions.Delay((float)num + 0.5f);
			}
			currentStage.Waiting = false;
			Status = ((CurrentLevel > 0) ? EAreaStatus.ReadyToInstallUpgrade : EAreaStatus.ReadyToInstallConstruct);
			currentStage.ActionGoing = false;
			currentStage.ActionReady = true;
			return;
		}
		currentStage.ActionReady = false;
		currentStage.ActionGoing = true;
		currentStage.Waiting = true;
		float data = NextStage.ConstructionTime.Data;
		currentStage.StartTime = _E5AD.UtcNow.AddSeconds((float)num - data);
		Status = ((CurrentLevel <= 0) ? EAreaStatus.Constructing : (NextStage.AutoUpgrade ? EAreaStatus.AutoUpgrading : EAreaStatus.Upgrading));
		if (num > 0)
		{
			await TasksExtensions.Delay((float)num + 0.5f);
		}
		currentStage.Waiting = false;
		DecideStatus(CurrentLevel + 1);
		currentStage.ActionGoing = false;
	}

	public async Task UpgradeAction()
	{
		if (Interlocked.CompareExchange(ref _actionGoingStatus, 1, 0) == 1)
		{
			return;
		}
		switch (Status)
		{
		case EAreaStatus.ReadyToInstallConstruct:
		case EAreaStatus.ReadyToInstallUpgrade:
			Singleton<_E815>.Instance.CompleteUpgradeZone(Template.Type);
			_E000();
			break;
		case EAreaStatus.ReadyToConstruct:
		case EAreaStatus.ReadyToUpgrade:
		{
			Stage nextStage = NextStage;
			Stage currentStage = CurrentStage;
			currentStage.StartTime = _E5AD.UtcNow;
			Status = ((Status != EAreaStatus.ReadyToUpgrade) ? EAreaStatus.Constructing : (NextStage.AutoUpgrade ? EAreaStatus.AutoUpgrading : EAreaStatus.Upgrading));
			float data = nextStage.ConstructionTime.Data;
			currentStage.ActionGoing = true;
			currentStage.Waiting = true;
			Singleton<_E815>.Instance.UpgradeZone(Template.Type, nextStage.Requirements);
			if (data < float.Epsilon)
			{
				Singleton<_E815>.Instance.CompleteUpgradeZone(Template.Type);
			}
			await TasksExtensions.Delay(data + 0.5f);
			currentStage.Waiting = false;
			DecideStatus(CurrentLevel + 1);
			currentStage.ActionGoing = false;
			break;
		}
		}
		_actionGoingStatus = 0;
	}

	public async void StartAutoUpgrade()
	{
		Stage nextStage = NextStage;
		Stage currentStage = CurrentStage;
		float data = nextStage.ConstructionTime.Data;
		Status = EAreaStatus.AutoUpgrading;
		currentStage.StartTime = _E5AD.UtcNow;
		currentStage.ActionGoing = true;
		currentStage.Waiting = true;
		await TasksExtensions.Delay(data + 0.5f);
		currentStage.Waiting = false;
		DecideStatus(CurrentLevel + 1);
		currentStage.ActionGoing = false;
	}

	private void _E000()
	{
		int num = CurrentLevel + 1;
		if (num > Template.MaxLevel)
		{
			Debug.LogErrorFormat(string.Format(_ED3E._E000(165124), num, Template.Name));
			return;
		}
		CurrentStage.ActionReady = false;
		_E001(num);
	}

	private void _E001(int level)
	{
		CurrentLevel = level;
		RecalculateAreas.Invoke();
		AreaUpgraded.Invoke();
		DecideStatus(level);
		LevelUpdated.Invoke();
	}

	public void SetProductionSchemes(IEnumerable<_E828> productionSchemes)
	{
		if (productionSchemes.IsNullOrEmpty())
		{
			return;
		}
		EAreaType areaType = Template.Type;
		foreach (KeyValuePair<int, Stage> stage3 in Template.Stages)
		{
			var (level, stage2) = stage3;
			if (stage2.Production == null)
			{
				stage2.Production = new RelatedProduction();
			}
			stage2.Production.Data = productionSchemes.Where((_E828 scheme) => !scheme.requirements.Any((Requirement r) => r is AreaRequirement areaRequirement && areaRequirement.AreaType == areaType && areaRequirement.RequiredLevel > level)).ToArray();
		}
	}

	public void DecideStatus(int levelToSet)
	{
		if (CurrentStage.ActionReady || CurrentStage.Waiting)
		{
			return;
		}
		if (CurrentStage.ActionGoing)
		{
			if (!(CurrentStage.StartTime.AddSeconds(NextStage.ConstructionTime.Data) > _E5AD.UtcNow) && (Status == EAreaStatus.Upgrading || Status == EAreaStatus.AutoUpgrading || Status == EAreaStatus.Constructing))
			{
				Status = ((Status == EAreaStatus.Upgrading || Status == EAreaStatus.AutoUpgrading) ? EAreaStatus.ReadyToInstallUpgrade : EAreaStatus.ReadyToInstallConstruct);
				CurrentStage.ActionReady = true;
				if (NextStage.ConstructionTime.Data < float.Epsilon || NextStage.AutoUpgrade)
				{
					_E000();
				}
				if (NextStage.AutoUpgrade)
				{
					StartAutoUpgrade();
				}
			}
		}
		else if (levelToSet <= 0)
		{
			if (!_E002(1))
			{
				Status = EAreaStatus.LockedToConstruct;
			}
			else if (Status != EAreaStatus.ReadyToInstallConstruct)
			{
				Status = EAreaStatus.ReadyToConstruct;
			}
		}
		else if (!_E003(NextStage))
		{
			Status = EAreaStatus.LockedToUpgrade;
		}
		else if (levelToSet >= Template.MaxLevel)
		{
			Status = EAreaStatus.NoFutureUpgrades;
		}
		else
		{
			Status = EAreaStatus.ReadyToUpgrade;
		}
	}

	private bool _E002(int level)
	{
		return _E003(StageAt(level));
	}

	private bool _E003(Stage stage)
	{
		RelatedRequirements requirements = stage.Requirements;
		if (requirements == null)
		{
			return true;
		}
		if (!requirements.Any())
		{
			return true;
		}
		return requirements.All((Requirement requirement) => requirement.Fulfilled);
	}

	public void Hover(bool value)
	{
		OnHover.Invoke(value);
	}

	public override string ToString()
	{
		return string.Format(_ED3E._E000(165195), Template.Name, CurrentLevel);
	}

	public void SetSelectedStatus(bool isSelected)
	{
		Selected = isSelected;
		Hover(value: false);
		OnSelected.Invoke(isSelected);
	}

	public void SetProducedItemsHandler(Item[] items, _E827 _)
	{
		if (_hideoutArea != null && items != null)
		{
			_hideoutArea.SpawnProducedItems(items).HandleExceptions();
		}
	}

	public void GetProducedItemsHandler(IEnumerable<Item> items)
	{
		if (_hideoutArea != null && items != null)
		{
			_hideoutArea.RemoveProducedItems(items);
		}
	}

	public void AttachVideoCard(int count)
	{
		if (_hideoutArea != null)
		{
			_hideoutArea.AttachVideocard(count);
		}
	}

	public void DetachVideoCard()
	{
		if (_hideoutArea != null)
		{
			_hideoutArea.DetachVideocard();
		}
	}

	public void SetImprovements()
	{
		ImprovementsUpdated.Invoke();
	}

	public void CheckVisibility()
	{
		bool flag = Requirements.All((Requirement requirement) => requirement.Fulfilled);
		if (_isVisible != flag)
		{
			_isVisible = flag;
			StatusUpdated?.Invoke();
			VisibilityChanged?.Invoke(flag);
		}
	}
}
