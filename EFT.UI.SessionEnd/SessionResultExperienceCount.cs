using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.InputSystem;
using EFT.UI.Screens;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.SessionEnd;

public sealed class SessionResultExperienceCount : EftScreen<SessionResultExperienceCount._E000, SessionResultExperienceCount>
{
	public new sealed class _E000 : _EC90<_E000, SessionResultExperienceCount>
	{
		public readonly Profile Profile;

		public readonly bool IsOnline;

		public readonly ExitStatus ExitStatus;

		public override EEftScreenType ScreenType => EEftScreenType.SessionExperience;

		protected override bool MainEnvironment => false;

		public override bool KeyScreen => true;

		protected override EStateSwitcher TaskBarButtonsVisibility => EStateSwitcher.Disabled;

		protected override EStateSwitcher MenuChatBarVisibility => EStateSwitcher.Disabled;

		public _E000(Profile profile, bool isOnline, ExitStatus exitStatus)
		{
			Profile = profile;
			IsOnline = isOnline;
			ExitStatus = exitStatus;
		}
	}

	[SerializeField]
	private DefaultUIButton _nextButton;

	[SerializeField]
	private DefaultUIButton _backButton;

	[SerializeField]
	private SessionExperiencePanel _experiencePanel;

	[SerializeField]
	private CustomTextMeshProUGUI _resultExperience;

	[SerializeField]
	private GameObject _sectionsScroller;

	[SerializeField]
	private Transform _sectionsContainer;

	[SerializeField]
	private ExperienceSectionView _sectionTemplate;

	[SerializeField]
	private SpriteMap _bonusGlow;

	[SerializeField]
	private SpriteMap _bonusIcons;

	[SerializeField]
	private CustomTextMeshProUGUI _bonusTitle;

	[SerializeField]
	private CustomTextMeshProUGUI _bonusValue;

	[SerializeField]
	private Image _bonusBack;

	[SerializeField]
	private Image _bonusIcon;

	[SerializeField]
	private Image _bonusHalo;

	private new _EC79<_EC8D, ExperienceSectionView> m__E000;

	private void Awake()
	{
		_nextButton.OnClick.AddListener(delegate
		{
			ScreenController._E000();
		});
		_backButton.OnClick.AddListener(delegate
		{
			ScreenController.CloseScreen();
		});
	}

	public override void Show(_E000 controller)
	{
		Show(controller.Profile, controller.IsOnline, controller.ExitStatus);
	}

	private void Show(Profile profile, bool isOnline, ExitStatus exitStatus)
	{
		ShowGameObject();
		_EC8E obj = new _EC8E();
		_EC8D obj2 = new _EC8D(_ED3E._E000(257105).Localized(), EStatGroupId.BattleCategory)
		{
			{
				_ED3E._E000(257151).Localized() + _ED3E._E000(54246) + profile.Stats.SessionCounters.GetInt(_E944.Kills) + _ED3E._E000(27308),
				profile.Stats.SessionCounters.GetInt(_E944.ExpKillBase)
			},
			{
				_ED3E._E000(257128).Localized() + _ED3E._E000(54246) + profile.Stats.SessionCounters.GetInt(_E944.HeadShots) + _ED3E._E000(27308),
				profile.Stats.SessionCounters.GetInt(_E944.ExpKillBodyPartBonus)
			},
			{
				_ED3E._E000(257182).Localized(),
				profile.Stats.SessionCounters.GetInt(_E944.ExpKillStreakBonus)
			},
			{
				_ED3E._E000(257170).Localized(),
				profile.Stats.SessionCounters.GetInt(_E944.ExpDamage)
			}
		};
		if (obj2.Articles.Count > 0)
		{
			obj.Sections.Add(obj2);
		}
		_EC8D obj3 = new _EC8D(_ED3E._E000(257209).Localized(), EStatGroupId.CommonStats)
		{
			{
				_ED3E._E000(257192).Localized(),
				profile.Stats.SessionCounters.GetInt(_E944.ExpExitStatus)
			},
			{
				_ED3E._E000(257244).Localized(),
				profile.Stats.SessionCounters.GetInt(_E944.ExpTrigger)
			}
		};
		if (obj3.Articles.Count > 0)
		{
			obj.Sections.Add(obj3);
		}
		_EC8D obj4 = new _EC8D(_ED3E._E000(257228).Localized(), EStatGroupId.HealthAndPhysicalCondition)
		{
			{
				_ED3E._E000(257219).Localized(),
				profile.Stats.SessionCounters.GetInt(_E944.ExpHeal)
			},
			{
				_ED3E._E000(257271).Localized(),
				profile.Stats.SessionCounters.GetInt(_E944.ExpEnergy)
			},
			{
				_ED3E._E000(257259).Localized(),
				profile.Stats.SessionCounters.GetInt(_E944.ExpHydration)
			}
		};
		if (obj4.Articles.Count > 0)
		{
			obj.Sections.Add(obj4);
		}
		_EC8D obj5 = new _EC8D(_ED3E._E000(257306).Localized(), EStatGroupId.LootingCategory)
		{
			{
				_ED3E._E000(257295).Localized(),
				profile.Stats.SessionCounters.GetInt(_E944.ExpItemLooting)
			},
			{
				_ED3E._E000(257282).Localized(),
				profile.Stats.SessionCounters.GetInt(_E944.ExpDoorUnlocked)
			},
			{
				_ED3E._E000(257325).Localized(),
				profile.Stats.SessionCounters.GetInt(_E944.ExpDoorBreached)
			},
			{
				_ED3E._E000(257295).Localized(),
				profile.Stats.SessionCounters.GetInt(_E944.ExpStationaryContainer)
			}
		};
		if (obj5.Articles.Count > 0)
		{
			obj.Sections.Add(obj5);
		}
		int experience = profile.Info.Experience;
		int totalSessionExperience = profile.Stats.TotalSessionExperience;
		_experiencePanel.Set(isOnline ? (experience - totalSessionExperience) : experience, totalSessionExperience);
		this.m__E000?.Dispose();
		this.m__E000 = UI.AddViewList(obj.Sections, _sectionTemplate, _sectionsContainer, delegate(_EC8D section, ExperienceSectionView view)
		{
			view.Show(section);
		});
		_sectionsScroller.SetActive(obj.Sections.Count > 0);
		_bonusIcon.sprite = _bonusIcons[exitStatus];
		_bonusHalo.sprite = _bonusGlow[exitStatus];
		_bonusTitle.text = (_ED3E._E000(257368) + exitStatus).Localized();
		Color color = Color.red;
		switch (exitStatus)
		{
		case ExitStatus.Survived:
			color = new Color(0.48046875f, 0.6875f, 0.01953125f);
			break;
		case ExitStatus.Killed:
			color = new Color(43f / 64f, 0.05859375f, 0.05859375f);
			break;
		case ExitStatus.Left:
			color = new Color(0.71484375f, 0.29296875f, 0.0625f);
			break;
		case ExitStatus.Runner:
			color = new Color(87f / 128f, 87f / 128f, 87f / 128f);
			break;
		case ExitStatus.MissingInAction:
			color = new Color(43f / 64f, 0.05859375f, 0.05859375f);
			break;
		}
		float multiplier = Singleton<_E5CB>.Instance.Experience.MatchEnd.GetMultiplier(exitStatus);
		double num = Singleton<BonusController>.Instance.Calculate(EBonusType.ExperienceRate, 1.0);
		int num2 = (int)((double)((float)totalSessionExperience / multiplier) / num);
		_bonusBack.color = color;
		_bonusValue.text = ((totalSessionExperience > 0) ? string.Format(_ED3E._E000(257361), num2, multiplier, totalSessionExperience, num) : _ED3E._E000(27314));
		_resultExperience.text = totalSessionExperience.ToThousandsString();
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (command.IsCommand(ECommand.Escape))
		{
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuEscape);
			ScreenController.CloseScreen();
			return ETranslateResult.BlockAll;
		}
		return InputNode.GetDefaultBlockResult(command);
	}

	[CompilerGenerated]
	private void _E000()
	{
		ScreenController._E000();
	}

	[CompilerGenerated]
	private void _E001()
	{
		ScreenController.CloseScreen();
	}
}
