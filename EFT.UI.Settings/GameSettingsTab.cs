using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using UnityEngine;

namespace EFT.UI.Settings;

public sealed class GameSettingsTab : SettingsTab
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public GameSettingsTab _003C_003E4__this;

		public IDictionary<string, string> languages;

		internal void _E000(bool x)
		{
			_003C_003E4__this._E004();
		}

		internal void _E001(bool x)
		{
			_003C_003E4__this._E003(hideStreamerName: true);
		}

		internal void _E002(int index)
		{
			_003C_003E4__this._E25E.Language.Value = languages.ElementAt(index).Key;
		}

		internal void _E003(string _)
		{
			_003C_003E4__this._E001().HandleExceptions();
		}

		internal void _E004()
		{
			_003C_003E4__this.SetLoadingStatus(inProgress: false);
		}

		internal void _E005(EnvironmentUI.EEnvironmentUIType x)
		{
			_E006(x).HandleExceptions();
		}

		internal async Task _E006(EnvironmentUI.EEnvironmentUIType x)
		{
			_003C_003E4__this._interfaceEnvironmentType.Interactable = false;
			if (MonoBehaviourSingleton<EnvironmentUI>.Instantiated)
			{
				await MonoBehaviourSingleton<EnvironmentUI>.Instance.SetEnvironmentAsync(x);
			}
			_003C_003E4__this._interfaceEnvironmentType.Interactable = true;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public IDictionary<string, string> languages;

		internal IEnumerable<string> _E000()
		{
			return languages.Select((KeyValuePair<string, string> x) => x.Key.Localized()).ToArray();
		}
	}

	private const string _E259 = "NicknameCanBeChangedAfter";

	private const string _E25A = "Not available in alpha";

	private static readonly ReadOnlyCollection<int> _E25B = Array.AsReadOnly(Enumerable.Range(50, 26).ToArray());

	private static readonly ReadOnlyCollection<float> _E25C = Array.AsReadOnly(new float[9] { 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1f });

	[SerializeField]
	private DropDownBox _interfaceLanguage;

	[SerializeField]
	private DropDownBox _interfaceEnvironmentType;

	[SerializeField]
	private DropDownBox _quickSlotsDropdown;

	[SerializeField]
	private DropDownBox _staminaDropdown;

	[SerializeField]
	private DropDownBox _healthDropdown;

	[SerializeField]
	private DropDownBox _healthColorDropdown;

	[SerializeField]
	private DropDownBox _highlightScopeDropdown;

	[SerializeField]
	private DropDownBox _notificationTypeDropdown;

	[SerializeField]
	private DropDownBox _connectionTypeDropdown;

	[SerializeField]
	private UpdatableToggle _subtitles;

	[SerializeField]
	private UpdatableToggle _tutorialHints;

	[SerializeField]
	private UpdatableToggle _dontAllowToAddMe;

	[SerializeField]
	private UpdatableToggle _clearRAM;

	[SerializeField]
	private UpdatableToggle _setAffinityToLogicalCores;

	[SerializeField]
	private UpdatableToggle _enableHideoutPreload;

	[SerializeField]
	private UpdatableToggle _enableStreamerMode;

	[SerializeField]
	private UpdatableToggle _enableBlockInvites;

	[SerializeField]
	private UpdatableToggle _malfunctionVisability;

	[SerializeField]
	private SelectSlider _aimingDeadzone;

	[SerializeField]
	private SelectSlider _fov;

	[SerializeField]
	private SelectSlider _headbobbing;

	[SerializeField]
	private UpdatableToggle _blood;

	[SerializeField]
	private UpdatableToggle _badLanguage;

	[SerializeField]
	private DefaultUIButton _editInterfaceLayout;

	[SerializeField]
	private ValidationInputField _nicknameInput;

	[SerializeField]
	private DefaultUIButton _changeNicknameButton;

	[SerializeField]
	private GameObject _nicknameBlocker;

	[SerializeField]
	private ElementBlocker _streamerModeBlocker;

	[SerializeField]
	private ElementBlocker _subtitlesBlocker;

	[SerializeField]
	private ElementBlocker _tutorialHintsBlocker;

	[SerializeField]
	private ElementBlocker _bloodBlocker;

	private DateTime? _E25D;

	private _E7EF _E25E;

	private _E7EE _E0A3;

	private _E796 _E25F;

	private Profile _E0B7;

	private string _E260;

	private void Awake()
	{
		_editInterfaceLayout.OnClick.AddListener(delegate
		{
			Debug.Log(_ED3E._E000(256128));
		});
		_changeNicknameButton.OnClick.AddListener(_E002);
		_nicknameInput.onValueChanged.AddListener(delegate(string x)
		{
			_E260 = x;
			_E004();
		});
		_nicknameInput.onSelect.AddListener(delegate
		{
			_E003(hideStreamerName: false);
		});
		_nicknameInput.onDeselect.AddListener(delegate(string x)
		{
			_E260 = x;
			_E003(hideStreamerName: true);
		});
		_subtitlesBlocker.Group.gameObject.SetActive(value: false);
		_bloodBlocker.StartBlock(_ED3E._E000(257970));
		_tutorialHintsBlocker.StartBlock(_ED3E._E000(257970));
	}

	private void Update()
	{
		if (_E25D.HasValue && _E25D <= _E5AD.UtcNow)
		{
			_nicknameBlocker.SetActive(value: false);
			_nicknameInput.ValidateCurrentInput();
			_E25D = null;
		}
	}

	public void Show(_E7EF gameSettings, _E796 backEndSession)
	{
		_E000 CS_0024_003C_003E8__locals0 = new _E000();
		CS_0024_003C_003E8__locals0._003C_003E4__this = this;
		_E25F = backEndSession;
		_E25E = gameSettings;
		_E0B7 = _E25F.Profile;
		_E260 = _E0B7?.Nickname ?? _ED3E._E000(249938);
		_E0A3 = new _E7EE();
		_E0A3.BindTo(_E25E, previewMode: true);
		UI.BindState(_nicknameInput.HasError, delegate
		{
			CS_0024_003C_003E8__locals0._003C_003E4__this._E004();
		});
		if (_E0B7 != null)
		{
			_E0A3.SetLocalProfile(_E0B7);
			bool isStreamerModeAvailable = _E0B7.Info.IsStreamerModeAvailable;
			_streamerModeBlocker.Group.gameObject.SetActive(isStreamerModeAvailable);
			if (isStreamerModeAvailable)
			{
				UI.BindState(_E25E.StreamerModeEnabled, delegate
				{
					CS_0024_003C_003E8__locals0._003C_003E4__this._E003(hideStreamerName: true);
				});
			}
			else
			{
				_E003(hideStreamerName: false);
			}
			_E005(_E0B7.Info.NicknameChangeDate);
		}
		else
		{
			_E003(hideStreamerName: false);
			_nicknameBlocker.SetActive(value: true);
			Debug.LogError(_ED3E._E000(258009));
		}
		_E000();
		CS_0024_003C_003E8__locals0.languages = _E7AD._E010.AvailableLanguages;
		_interfaceLanguage.Bind(delegate(int index)
		{
			CS_0024_003C_003E8__locals0._003C_003E4__this._E25E.Language.Value = CS_0024_003C_003E8__locals0.languages.ElementAt(index).Key;
		});
		RegisterDropDown(_interfaceLanguage);
		UI.SubscribeState(_E25E.Language, delegate
		{
			CS_0024_003C_003E8__locals0._003C_003E4__this._E001().HandleExceptions();
		});
		ShowEnumDropDown(_interfaceEnvironmentType, _E25E.EnvironmentUiType, null, delegate
		{
			CS_0024_003C_003E8__locals0._003C_003E4__this.SetLoadingStatus(inProgress: false);
		});
		if (MonoBehaviourSingleton<EnvironmentUI>.Instantiated)
		{
			UI.SubscribeState(_E25E.EnvironmentUiType, delegate(EnvironmentUI.EEnvironmentUIType x)
			{
				CS_0024_003C_003E8__locals0._E006(x).HandleExceptions();
			});
		}
		ShowEnumDropDown(_quickSlotsDropdown, _E25E.QuickSlotsVisibility, _ED3E._E000(257987));
		ShowEnumDropDown(_staminaDropdown, _E25E.StaminaVisibility, _ED3E._E000(257987));
		ShowEnumDropDown(_healthDropdown, _E25E.HealthVisibility, _ED3E._E000(257987));
		ShowEnumDropDown(_healthColorDropdown, _E25E.HealthColor, _ED3E._E000(258027));
		ShowEnumDropDown(_highlightScopeDropdown, _E25E.HighlightScope, _ED3E._E000(256012));
		ShowEnumDropDown(_notificationTypeDropdown, _E25E.NotificationTransportType, _ED3E._E000(256048));
		ShowEnumDropDown(_connectionTypeDropdown, _E25E.ConnectionType, _ED3E._E000(256086));
		SettingsTab.ShowFakeToggle(_subtitles);
		SettingsTab.ShowFakeToggle(_tutorialHints);
		SettingsTab.ShowFakeToggle(_dontAllowToAddMe);
		SettingsTab.ShowFakeToggle(_blood);
		SettingsTab.ShowFakeToggle(_badLanguage);
		ShowFakeSlider(_aimingDeadzone);
		BindSelectSliderToSetting(_fov, _E25E.FieldOfView, _E25B, (int x) => x.ToString());
		BindSelectSliderToSetting(_headbobbing, _E25E.HeadBobbing, _E25C, (float x) => x.ToString(_ED3E._E000(164283)));
		SettingsTab.BindToggleToSetting(_clearRAM, _E25E.AutoEmptyWorkingSet);
		SettingsTab.BindToggleToSetting(_setAffinityToLogicalCores, _E25E.SetAffinityToLogicalCores);
		SettingsTab.BindToggleToSetting(_enableHideoutPreload, _E25E.EnableHideoutPreload);
		SettingsTab.BindToggleToSetting(_enableStreamerMode, _E25E.StreamerModeEnabled);
		SettingsTab.BindToggleToSetting(_enableBlockInvites, _E25E.BlockGroupInvites);
		SettingsTab.BindToggleToSetting(_malfunctionVisability, _E25E.MalfunctionVisability);
	}

	private void _E000()
	{
		IDictionary<string, string> languages = _E7AD._E010.AvailableLanguages;
		_interfaceLanguage.Show(() => languages.Select((KeyValuePair<string, string> x) => x.Key.Localized()).ToArray());
		int num = languages.Keys.IndexOf(_E25E.Language);
		if (num >= 0)
		{
			_interfaceLanguage.UpdateValue(num, sendCallback: false);
		}
		else
		{
			_interfaceLanguage.SetLabelText(_E25E.Language);
		}
	}

	private async Task _E001()
	{
		SetLoadingStatus(inProgress: true);
		await _E77F.ReloadLocale(_E25F);
		if (_E0B7 != null)
		{
			_E005(_E0B7.Info.NicknameChangeDate);
		}
		SetLoadingStatus(inProgress: false);
	}

	private void _E002()
	{
		if ((bool)_nicknameInput.HasError || _E0B7 == null || _E0B7.Nickname == _E260)
		{
			return;
		}
		_E25F.ChangeNickname(_E0B7.Id, _E260, delegate(Result<_E34B> changeNicknameResult)
		{
			_nicknameInput.DeSelect();
			if (string.IsNullOrEmpty(changeNicknameResult.Error))
			{
				_E0B7.Info.NicknameChangeDate = changeNicknameResult.Value.NicknameChangeDate;
				_E005(_E0B7.Info.NicknameChangeDate);
				Debug.Log(_ED3E._E000(256155) + _E260);
			}
			else if (EBackendErrorCode.NicknameChangeTimeout.EqualsToInt(changeNicknameResult.ErrorCode))
			{
				_E005(_E0B7.Info.NicknameChangeDate);
			}
			else
			{
				_nicknameInput.ShowErrorFromCode(changeNicknameResult.ErrorCode);
			}
		});
	}

	public override async Task TakeSettingsFrom(_E7DE settingsManager)
	{
		_E7EF @default = settingsManager.Game.Default;
		_E7AD._E010._E011 = @default.Language;
		await _E001();
		_E25E.TakeSettingsFrom(@default);
		_E000();
	}

	public override void Close()
	{
		_E0A3?.Dispose();
		_E0A3 = null;
		base.Close();
	}

	private void _E003(bool hideStreamerName)
	{
		if (_E0A3.IsStreamerModeActive && hideStreamerName)
		{
			_nicknameInput.HideSymbolCountLabel(needHide: true);
			_nicknameInput.SetTextWithoutNotify(_ED3E._E000(252985));
		}
		else
		{
			_nicknameInput.HideSymbolCountLabel(needHide: false);
			_nicknameInput.SetTextWithoutNotify(_E260);
		}
		_E004();
	}

	private void _E004()
	{
		_changeNicknameButton.Interactable = !_nicknameInput.HasError.Value && !_E0A3.IsStreamerModeActive && _E0B7 != null && _E260 != _E0B7.Nickname;
	}

	private void _E005(long date)
	{
		if (!(_E5AD.NowUnix > (double)date))
		{
			_E25D = _E5AD.UniversalDateTimeFromUnixTime(date);
			_nicknameInput.ShowMessage(string.Format(_ED3E._E000(256071).Localized(), _E25D.Value.ToString(_ED3E._E000(256105))), isError: true);
			_nicknameBlocker.SetActive(value: true);
		}
	}

	[CompilerGenerated]
	private void _E006(string x)
	{
		_E260 = x;
		_E004();
	}

	[CompilerGenerated]
	private void _E007(string x)
	{
		_E003(hideStreamerName: false);
	}

	[CompilerGenerated]
	private void _E008(string x)
	{
		_E260 = x;
		_E003(hideStreamerName: true);
	}

	[CompilerGenerated]
	private void _E009(Result<_E34B> changeNicknameResult)
	{
		_nicknameInput.DeSelect();
		if (string.IsNullOrEmpty(changeNicknameResult.Error))
		{
			_E0B7.Info.NicknameChangeDate = changeNicknameResult.Value.NicknameChangeDate;
			_E005(_E0B7.Info.NicknameChangeDate);
			Debug.Log(_ED3E._E000(256155) + _E260);
		}
		else if (EBackendErrorCode.NicknameChangeTimeout.EqualsToInt(changeNicknameResult.ErrorCode))
		{
			_E005(_E0B7.Info.NicknameChangeDate);
		}
		else
		{
			_nicknameInput.ShowErrorFromCode(changeNicknameResult.ErrorCode);
		}
	}
}
