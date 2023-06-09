using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using DG.Tweening;
using EFT.InputSystem;
using EFT.UI.Health;
using EFT.UI.Screens;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.Matchmaker;

public sealed class MatchMakerSideSelectionScreen : MatchmakerEftScreen<MatchMakerSideSelectionScreen._E000, MatchMakerSideSelectionScreen>
{
	private enum EScavLockReason
	{
		None,
		ScavNotReadySolo,
		ScavNotReadyGroup,
		OnlineBan,
		GroupTooBig
	}

	public new sealed class _E000 : _EC8F<_E000, MatchMakerSideSelectionScreen>
	{
		public readonly _E796 Session;

		public readonly RaidSettings RaidSettings;

		public readonly _E9C4 HealthController;

		public readonly _EAED InventoryController;

		public override EEftScreenType ScreenType => EEftScreenType.SelectRaidSide;

		public override bool KeyScreen => true;

		public _E000(_E796 session, ref RaidSettings raidSettings, _E9C4 healthController, _EAED inventoryController, _EC99 matchmakerController)
			: base(matchmakerController)
		{
			Session = session;
			RaidSettings = raidSettings;
			HealthController = healthController;
			InventoryController = inventoryController;
		}

		public void UpdateSideSelection(ESideType side)
		{
			RaidSettings.Side = side;
		}
	}

	private new const float m__E000 = 0.25f;

	private const float m__E001 = 0.3f;

	[SerializeField]
	private DefaultUIButton _nextButton;

	[SerializeField]
	private DefaultUIButton _backButton;

	[SerializeField]
	private CanvasGroup _savageBlocker;

	[SerializeField]
	private UIAnimatedToggleSpawner _randomButton;

	[SerializeField]
	private UIAnimatedToggleSpawner _savagesButton;

	[SerializeField]
	private UIAnimatedToggleSpawner _pmcButton;

	[SerializeField]
	private Button _savagesBigButton;

	[SerializeField]
	private Button _pmcBigButton;

	[SerializeField]
	private PlayerModelView _pmcModelView;

	[SerializeField]
	private InventoryScreenHealthPanel _healthPanel;

	[SerializeField]
	private PlayerModelView _savageModelView;

	[SerializeField]
	private TMP_Text _savageBlockMessage;

	[SerializeField]
	private TextMeshProUGUI _sideDescriptionText;

	[SerializeField]
	private CanvasGroup _lowHealthWarning;

	[SerializeField]
	private CanvasGroup _notFullHealthWarning;

	[SerializeField]
	private Image _randomIcon;

	[SerializeField]
	private Light _mainLight;

	[SerializeField]
	private Light _hairLight;

	[SerializeField]
	private Light _downLight;

	private DateTime m__E002;

	private Profile m__E003;

	private Profile m__E004;

	private ESideType m__E005;

	private _E9C4 m__E006;

	private _EAED m__E007;

	private bool m__E008;

	private bool m__E009;

	private CanvasGroup m__E00A;

	private bool m__E00B;

	private static Color32 _E00C => new Color32(byte.MaxValue, 210, 169, byte.MaxValue);

	private double _E00D => this.m__E004.Info.SavageLockTime - (double)_E5AD.UtcNowUnixInt;

	private bool _E00E
	{
		get
		{
			string playerName;
			double farScavTime;
			if (this._E00D < 0.0 && !this.m__E00B)
			{
				return !MatchmakerPlayersController.HasLockedScav(out playerName, out farScavTime);
			}
			return false;
		}
	}

	private double _E00F => MatchmakerPlayersController.GroupPlayers.Select((_EC9B player) => player.Info.SavageLockTime - (double)_E5AD.UtcNowUnixInt).Max();

	private void Awake()
	{
		this.m__E00A = _healthPanel.GetOrAddComponent<CanvasGroup>();
		_pmcButton.SpawnedObject.onValueChanged.AddListener(delegate(bool arg)
		{
			if (arg)
			{
				_E007(ESideType.Pmc);
			}
		});
		_savagesButton.SpawnedObject.onValueChanged.AddListener(delegate(bool arg)
		{
			if (arg)
			{
				_E007(ESideType.Savage);
			}
		});
		_randomButton.SpawnedObject.onValueChanged.AddListener(delegate(bool arg)
		{
			if (arg)
			{
				_E007(ESideType.Random);
			}
		});
		_savagesBigButton.onClick.AddListener(delegate
		{
			_savagesButton.SpawnedObject._E001 = true;
			_E007(ESideType.Savage);
		});
		_pmcBigButton.onClick.AddListener(delegate
		{
			_pmcButton.SpawnedObject._E001 = true;
			_E007(ESideType.Pmc);
		});
		EventTrigger orAddComponent = _pmcBigButton.GetOrAddComponent<EventTrigger>();
		EventTrigger orAddComponent2 = _pmcButton.GetOrAddComponent<EventTrigger>();
		EventTrigger.Entry entry = new EventTrigger.Entry
		{
			eventID = EventTriggerType.PointerEnter
		};
		EventTrigger.Entry entry2 = new EventTrigger.Entry
		{
			eventID = EventTriggerType.PointerExit
		};
		entry.callback.AddListener(_E001);
		entry2.callback.AddListener(_E002);
		orAddComponent.triggers.Add(entry);
		orAddComponent.triggers.Add(entry2);
		orAddComponent2.triggers.Add(entry);
		orAddComponent2.triggers.Add(entry2);
		_nextButton.OnClick.AddListener(delegate
		{
			ScreenController.UpdateSideSelection(this.m__E005);
			((_EC90<_E000, MatchMakerSideSelectionScreen>)ScreenController)._E000();
		});
		_backButton.OnClick.AddListener(delegate
		{
			ScreenController.CloseScreen();
		});
	}

	public override void Show(_E000 controller)
	{
		base.Show(controller);
		Show(controller.Session, controller.RaidSettings, controller.HealthController, controller.InventoryController);
	}

	private void Show(_E796 session, RaidSettings raidSettings, _E9C4 healthController, _EAED inventoryController)
	{
		ShowGameObject();
		this.m__E003 = session.Profile;
		this.m__E004 = session.ProfileOfPet;
		this.m__E005 = raidSettings.Side;
		this.m__E006 = healthController;
		this.m__E007 = inventoryController;
		switch (raidSettings.Side)
		{
		case ESideType.Pmc:
			_pmcButton._E001 = true;
			break;
		case ESideType.Savage:
			_savagesButton._E001 = this._E00E;
			if (!this._E00E)
			{
				_pmcButton._E001 = true;
			}
			break;
		case ESideType.Random:
			_randomButton._E001 = true;
			break;
		default:
			throw new ArgumentOutOfRangeException(_ED3E._E000(181998), raidSettings.Side, null);
		}
		_healthPanel.SetParametersVisibility(value: false);
		_E003(visibility: false, instantly: true);
		_randomButton.gameObject.SetActive(value: false);
		_randomIcon.gameObject.SetActive(value: false);
		_E002();
		_E006();
		_E007(this.m__E005);
		_pmcModelView.Show(this.m__E003, null, delegate
		{
		}, 0.3f).HandleExceptions();
		UI.AddDisposable(_pmcModelView);
		_savageModelView.Show(this.m__E004, null, delegate
		{
		}, 5f).HandleExceptions();
		UI.AddDisposable(_savageModelView);
		_healthPanel.Show(this.m__E006, this.m__E007.Inventory, this.m__E003.Skills);
		UI.AddDisposable(_healthPanel);
		this.m__E003.Info.OnBanChanged += _E000;
		_E000();
		UI.AddDisposable(delegate
		{
			this.m__E003.Info.OnBanChanged -= _E000;
		});
	}

	private void _E000(EBanType banType = EBanType.Online)
	{
		if (banType == EBanType.Online)
		{
			_E72E ban = this.m__E003.Info.GetBan(EBanType.Online);
			this.m__E00B = ban != null;
		}
	}

	private void _E001(BaseEventData eventData = null)
	{
		_E003(this.m__E005 == ESideType.Pmc && this.m__E008);
	}

	private void _E002(BaseEventData eventData = null)
	{
		_E003(visibility: false);
	}

	private void Update()
	{
		if (this.m__E005 == ESideType.Pmc)
		{
			float normalized = this.m__E006.GetBodyPartHealth(EBodyPart.Common, rounded: true).Normalized;
			IEnumerable<_E992> source = from effect in this.m__E006.GetAllActiveEffects()
				where !(effect is _E99A) && !(effect is _E99C)
				select effect;
			bool flag = normalized <= 0.5f || source.Any();
			bool flag2 = (double)normalized <= 0.9 && !flag;
			this.m__E008 = flag || flag2;
			_E004(_lowHealthWarning, flag && !this.m__E009 && _pmcModelView.LoadingComplete);
			_E004(_notFullHealthWarning, flag2 && !this.m__E009 && _pmcModelView.LoadingComplete);
		}
		else
		{
			_E004(_lowHealthWarning, visibility: false);
			_E004(_notFullHealthWarning, visibility: false);
		}
		DateTime utcNow = _E5AD.UtcNow;
		DateTime dateTime = this.m__E002.AddSeconds(0.25);
		if (!(utcNow < dateTime) && !(utcNow == this.m__E002))
		{
			this.m__E002 = utcNow;
			_E006();
		}
	}

	private void _E003(bool visibility, bool instantly = false)
	{
		this.m__E009 = visibility;
		_E004(this.m__E00A, visibility, instantly);
	}

	private void _E004(CanvasGroup warning, bool visibility, bool instantly = false)
	{
		float num = (visibility ? 1f : 0f);
		float alpha = warning.alpha;
		if (!num.Equals(alpha))
		{
			warning.DOKill();
			if (!instantly)
			{
				warning.DOFade(num, 0.3f);
			}
			else
			{
				warning.alpha = num;
			}
		}
	}

	private EScavLockReason _E005()
	{
		EScavLockReason result = EScavLockReason.None;
		string playerName;
		double farScavTime;
		if (this.m__E00B)
		{
			result = EScavLockReason.OnlineBan;
		}
		else if (MatchmakerPlayersController.GroupPlayers.Count > 4)
		{
			result = EScavLockReason.GroupTooBig;
		}
		else if (MatchmakerPlayersController.HasLockedScav(out playerName, out farScavTime))
		{
			result = ((MatchmakerPlayersController.GroupPlayers.Count == 1) ? EScavLockReason.ScavNotReadySolo : EScavLockReason.ScavNotReadyGroup);
		}
		return result;
	}

	private void _E006()
	{
		bool flag = this._E00E;
		_savageBlocker.alpha = (flag ? 1f : 0.3f);
		_savageBlocker.interactable = flag;
		if (!this._E00E)
		{
			EScavLockReason eScavLockReason = _E005();
			TimeSpan.FromSeconds(this._E00F).Deconstruct(out var hours, out var minutes, out var seconds);
			int num = hours;
			int num2 = minutes;
			int num3 = seconds;
			string empty = string.Empty;
			switch (eScavLockReason)
			{
			case EScavLockReason.OnlineBan:
			case EScavLockReason.GroupTooBig:
				empty = eScavLockReason.LocalizedEnum();
				break;
			case EScavLockReason.ScavNotReadyGroup:
				empty = eScavLockReason.LocalizedEnum() + string.Format(_ED3E._E000(234034), num, num2, num3);
				break;
			default:
				empty = string.Format(_ED3E._E000(59439), num, num2, num3);
				break;
			}
			_savageBlockMessage.SetMonospaceText(empty);
			_savageBlockMessage.SetMonospaceText(this.m__E00B ? _ED3E._E000(140972).Localized() : empty);
		}
		_savageBlockMessage.gameObject.SetActive(!flag);
		_savagesButton.SpawnedObject.interactable = flag;
		_hairLight.color = (flag ? MatchMakerSideSelectionScreen._E00C : new Color32(byte.MaxValue, 0, 0, byte.MaxValue));
		_mainLight.intensity = ((!flag) ? 1 : 8);
		_downLight.intensity = ((!flag) ? 1 : 8);
		_savagesBigButton.gameObject.SetActive(flag);
		if (!flag && _savagesButton._E001)
		{
			_savagesButton._E001 = false;
			_pmcButton._E001 = true;
		}
	}

	private void _E007(ESideType side)
	{
		this.m__E005 = side;
		switch (side)
		{
		case ESideType.Pmc:
			_sideDescriptionText.text = ((this.m__E003.Info.Side == EPlayerSide.Bear) ? _ED3E._E000(234057).Localized() : _ED3E._E000(234073).Localized());
			break;
		case ESideType.Savage:
			_sideDescriptionText.text = _ED3E._E000(234105).Localized();
			break;
		default:
			throw new ArgumentOutOfRangeException(_ED3E._E000(181998), side, null);
		case ESideType.Random:
			break;
		}
		_E001();
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

	protected override void TranslateAxes(ref float[] axes)
	{
		axes = null;
	}

	protected override ECursorResult ShouldLockCursor()
	{
		return ECursorResult.ShowCursor;
	}

	public override void Close()
	{
		_E003(visibility: false, instantly: true);
		_E004(_notFullHealthWarning, visibility: false, instantly: true);
		_E004(_lowHealthWarning, visibility: false, instantly: true);
		base.Close();
	}

	[CompilerGenerated]
	private void _E008(bool arg)
	{
		if (arg)
		{
			_E007(ESideType.Pmc);
		}
	}

	[CompilerGenerated]
	private void _E009(bool arg)
	{
		if (arg)
		{
			_E007(ESideType.Savage);
		}
	}

	[CompilerGenerated]
	private void _E00A(bool arg)
	{
		if (arg)
		{
			_E007(ESideType.Random);
		}
	}

	[CompilerGenerated]
	private void _E00B()
	{
		_savagesButton.SpawnedObject._E001 = true;
		_E007(ESideType.Savage);
	}

	[CompilerGenerated]
	private void _E00C()
	{
		_pmcButton.SpawnedObject._E001 = true;
		_E007(ESideType.Pmc);
	}

	[CompilerGenerated]
	private void _E00D()
	{
		ScreenController.UpdateSideSelection(this.m__E005);
		((_EC90<_E000, MatchMakerSideSelectionScreen>)ScreenController)._E000();
	}

	[CompilerGenerated]
	private void _E00E()
	{
		ScreenController.CloseScreen();
	}

	[CompilerGenerated]
	private void _E00F()
	{
		this.m__E003.Info.OnBanChanged -= _E000;
	}
}
