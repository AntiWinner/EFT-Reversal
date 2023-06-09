using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.InputSystem;
using EFT.UI.Screens;
using UnityEngine;

namespace EFT.UI.Matchmaker;

public sealed class MatchmakerOfflineRaidScreen : MatchmakerEftScreen<MatchmakerOfflineRaidScreen._E000, MatchmakerOfflineRaidScreen>
{
	public new sealed class _E000 : _EC8F<_E000, MatchmakerOfflineRaidScreen>
	{
		public readonly _E72F ProfileInfo;

		public readonly RaidSettings RaidSettings;

		public override EEftScreenType ScreenType => EEftScreenType.OfflineRaid;

		public override bool KeyScreen => true;

		public _E000(_E72F profileInfo, ref RaidSettings raidSettings, _EC99 matchmakerPlayersController)
			: base(matchmakerPlayersController)
		{
			ProfileInfo = profileInfo;
			RaidSettings = raidSettings;
		}
	}

	[SerializeField]
	private UpdatableToggle _offlineModeToggle;

	[SerializeField]
	private UiElementBlocker _onlineBlocker;

	[SerializeField]
	private RaidSettingsWindow _raidSettingsWindow;

	[SerializeField]
	private MatchmakerRaidSettingsSummaryView _raidSettingsSummaryView;

	[SerializeField]
	private DefaultUIButton _changeSettingsButton;

	[SerializeField]
	private DefaultUIButton _nextButtonSpawner;

	[SerializeField]
	private DefaultUIButton _backButtonSpawner;

	[SerializeField]
	private DefaultUIButton _readyButton;

	private new _E72F m__E000;

	private RaidSettings m__E001;

	private _E72E m__E002;

	private void Awake()
	{
		_offlineModeToggle.onValueChanged.AddListener(_E001);
		_nextButtonSpawner.OnClick.AddListener(delegate
		{
			((_EC90<_E000, MatchmakerOfflineRaidScreen>)ScreenController)._E000();
		});
		_backButtonSpawner.OnClick.AddListener(delegate
		{
			ScreenController.CloseScreen();
		});
		_readyButton.OnClick.AddListener(delegate
		{
			ScreenController._E000();
		});
		_changeSettingsButton.OnClick.AddListener(_E000);
	}

	public override void Show(_E000 controller)
	{
		base.Show(controller);
		Show(controller.ProfileInfo, controller.RaidSettings);
	}

	private void Show(_E72F profileInfo, RaidSettings raidSettings)
	{
		this.m__E000 = profileInfo;
		this.m__E001 = raidSettings;
		ShowGameObject();
		if (this.m__E001.RaidMode == ERaidMode.Coop)
		{
			_readyButton.gameObject.SetActive(value: false);
		}
		this.m__E000.OnBanChanged += _E004;
		_E004();
		_raidSettingsSummaryView.Show(raidSettings);
		UI.BindEvent(MatchmakerPlayersController.GroupPlayers.ItemsChanged, _E002);
		UI.SubscribeEvent(_raidSettingsWindow.OnRaidSettingsChanged, delegate
		{
			_raidSettingsSummaryView.Show(this.m__E001);
		});
		UI.AddDisposable(delegate
		{
			this.m__E000.OnBanChanged -= _E004;
		});
		UI.AddDisposable(_raidSettingsWindow.Close);
	}

	private void _E000()
	{
		_raidSettingsWindow.Show(this.m__E001, this.m__E000, MatchmakerPlayersController);
	}

	private void _E001(bool isToggleOn)
	{
		this.m__E001.RaidMode = ((MatchmakerPlayersController.GroupPlayers.Count != 1) ? (isToggleOn ? ERaidMode.Coop : ERaidMode.Online) : (isToggleOn ? ERaidMode.Local : ERaidMode.Online));
		_E003(isToggleOn);
	}

	private void _E002()
	{
		if (MatchmakerPlayersController.GroupPlayers.Count == 1)
		{
			_onlineBlocker.RemoveBlock();
			_E001(_offlineModeToggle.isOn);
			return;
		}
		ECoopBlock reason;
		bool coopBlockReason = MatchmakerPlayersController.GetCoopBlockReason(out reason);
		string reason2 = ((reason == ECoopBlock.NoBlock) ? string.Empty : reason.LocalizedEnum());
		if (!coopBlockReason)
		{
			_offlineModeToggle.UpdateValue(value: false);
		}
		_E003(coopBlockReason);
		_E001(_offlineModeToggle.isOn);
		_onlineBlocker.SetBlock(!coopBlockReason, reason2);
	}

	private void _E003(bool value)
	{
		_changeSettingsButton.Interactable = value;
		if (!value && _raidSettingsWindow.IsActive)
		{
			_raidSettingsWindow.Close();
		}
		_raidSettingsSummaryView.Show(this.m__E001);
	}

	private void _E004(EBanType banType = EBanType.Online)
	{
		if (banType == EBanType.Online)
		{
			this.m__E002 = this.m__E000.GetBan(EBanType.Online);
			if (this.m__E002 != null)
			{
				_offlineModeToggle.UpdateValue(value: true);
				_E003(value: true);
				_onlineBlocker.StartBlock(_ED3E._E000(140972));
			}
		}
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (command.IsCommand(ECommand.Escape))
		{
			if (_raidSettingsWindow.IsActive)
			{
				_raidSettingsWindow.Close();
				return ETranslateResult.BlockAll;
			}
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuEscape);
			ScreenController.CloseScreen();
			return ETranslateResult.BlockAll;
		}
		return InputNode.GetDefaultBlockResult(command);
	}

	[CompilerGenerated]
	private void _E005()
	{
		((_EC90<_E000, MatchmakerOfflineRaidScreen>)ScreenController)._E000();
	}

	[CompilerGenerated]
	private void _E006()
	{
		ScreenController.CloseScreen();
	}

	[CompilerGenerated]
	private void _E007()
	{
		ScreenController._E000();
	}

	[CompilerGenerated]
	private void _E008()
	{
		_raidSettingsSummaryView.Show(this.m__E001);
	}

	[CompilerGenerated]
	private void _E009()
	{
		this.m__E000.OnBanChanged -= _E004;
	}
}
