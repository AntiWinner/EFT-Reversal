using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Bsg.GameSettings;
using Comfort.Common;
using UnityEngine;

namespace EFT.UI.Settings;

public sealed class SoundSettingsTab : SettingsTab
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public TaskCompletionSource<bool> source;

		public SoundSettingsTab _003C_003E4__this;

		public string voice;

		internal void _E000(IResult result)
		{
			source.SetResult(result.Succeed);
			if (result.Succeed)
			{
				_003C_003E4__this._E256.Voice = voice;
			}
			else
			{
				MonoBehaviourSingleton<PreloaderUI>.Instance.ShowErrorScreen(string.Empty, _ED3E._E000(234707).Localized());
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public SettingDropDown deviceDropDown;

		internal void _E000()
		{
			deviceDropDown.Close();
		}
	}

	private const string _E27E = "Error/VoiceChangingFailed";

	private readonly ReadOnlyCollection<int> _E27F = Array.AsReadOnly(new int[11]
	{
		0, 10, 20, 30, 40, 50, 60, 70, 80, 90,
		100
	});

	[SerializeField]
	private VoiceSelector _voiceSelector;

	[SerializeField]
	private Transform _slidersSection;

	[SerializeField]
	private Transform _togglesSection;

	[Space]
	[SerializeField]
	private SettingSelectSlider _selectSliderPrefab;

	[SerializeField]
	private SettingFloatSlider _floatSliderPrefab;

	[SerializeField]
	private SettingDropDown _dropDownPrefab;

	[SerializeField]
	private SettingToggle _togglePrefab;

	[SerializeField]
	[Space]
	private UiElementBlocker _voipEnableBlocker;

	[SerializeField]
	private UiElementBlocker _voipBanBlocker;

	[SerializeField]
	private CustomTextMeshProUGUI _voipBanMessageText;

	[SerializeField]
	private GameObject _voipBanMessage;

	private _E7E0 _E24F;

	private _E7DF _E0A3;

	private _E796 _E25F;

	private bool _E280;

	private _E72F _E256;

	private _E72E _E281;

	private int _E0A4;

	public void Show(_E7E0 soundSettings, _E72F profileInfo, _E796 session)
	{
		_E24F = soundSettings;
		_E0A3 = new _E7DF();
		_E0A3.BindTo(_E24F, previewMode: true);
		_E25F = session;
		_E256 = profileInfo;
		_E002(_E24F.OverallVolume);
		_E002(_E24F.InterfaceVolume);
		_E002(_E24F.ChatVolume);
		_E002(_E24F.MusicVolume);
		_E002(_E24F.HideoutVolume);
		_E003(_E24F.MusicOnRaidEnd);
		_E003(_E24F.BinauralSound);
		_E004();
	}

	private void Update()
	{
		_E006();
	}

	public override Task TakeSettingsFrom(_E7DE settingsManager)
	{
		_E24F.TakeSettingsFrom(settingsManager.Sound.Default);
		return Task.CompletedTask;
	}

	protected override void OnTabSelected()
	{
		if (!_E280)
		{
			_voiceSelector.Initialize(_E256.Voice, _E256.Side, _E000);
			_E280 = true;
		}
		VoiceSelector voiceSelector = _voiceSelector;
		_E725 activeProfileStatus = _E25F.ActiveProfileStatus;
		voiceSelector.Show(activeProfileStatus != null && activeProfileStatus.status == EProfileStatus.Free && !_E7A3.InRaid);
	}

	private Task<bool> _E000(string voice)
	{
		TaskCompletionSource<bool> source = new TaskCompletionSource<bool>();
		_E25F.ChangeVoice(voice, delegate(IResult result)
		{
			source.SetResult(result.Succeed);
			if (result.Succeed)
			{
				_E256.Voice = voice;
			}
			else
			{
				MonoBehaviourSingleton<PreloaderUI>.Instance.ShowErrorScreen(string.Empty, _ED3E._E000(234707).Localized());
			}
		});
		return source.Task;
	}

	private void _E001(GameSetting<string> setting, Transform container, ReadOnlyCollection<string> devices)
	{
		SettingDropDown deviceDropDown = CreateControl(_dropDownPrefab, container);
		deviceDropDown.BindTo(setting, devices, (string x) => (x == _ED3E._E000(30808) || x == _ED3E._E000(194336)) ? x.Localized() : x);
		UI.AddDisposable(delegate
		{
			deviceDropDown.Close();
		});
	}

	private void _E002(GameSetting<int> setting)
	{
		CreateControl(_selectSliderPrefab, _slidersSection).BindIndexTo(setting, _E27F, (int x) => x.ToString());
	}

	private void _E003(GameSetting<bool> setting)
	{
		CreateControl(_togglePrefab, _togglesSection).BindTo(setting);
	}

	private void _E004()
	{
		SettingToggle settingToggle = CreateControl(_togglePrefab, _voipBanBlocker.Container);
		settingToggle.transform.SetSiblingIndex(0);
		settingToggle.BindTo(_E24F.VoipEnabled);
		UI.BindState(_E24F.VoipEnabled, _E008);
		_E001(_E24F.VoipDevice, _voipEnableBlocker.Container, _E7E0.GetMicrophones());
		CreateControl(_floatSliderPrefab, _voipEnableBlocker.Container).BindTo(_E24F.VoipDeviceSensitivity, 0, 100);
		string voipDisabledReason = Singleton<_E7DE>.Instance.Sound.Controller.VoipDisabledReason;
		if (!string.IsNullOrEmpty(voipDisabledReason))
		{
			_voipEnableBlocker.RemoveBlock();
			_voipBanBlocker.StartBlock(voipDisabledReason + _ED3E._E000(234671));
			_E007(voipDisabledReason.Localized());
			return;
		}
		_E256.OnBanChanged += _E005;
		_E005();
		UI.AddDisposable(delegate
		{
			_E256.OnBanChanged -= _E005;
		});
	}

	private void _E005(EBanType banType = EBanType.Voip)
	{
		if (banType == EBanType.Voip)
		{
			_E72E ban = _E256.GetBan(EBanType.Voip);
			if (ban != null)
			{
				_voipEnableBlocker.RemoveBlock();
				_voipBanBlocker.StartBlock(_ED3E._E000(234663));
				_E281 = ban;
				_E0A4 = -1;
				_E006();
			}
			else
			{
				_E008(_E24F.VoipEnabled);
				_voipBanBlocker.RemoveBlock();
				_voipBanMessage.SetActive(value: false);
				_E281 = null;
			}
		}
	}

	private void _E006()
	{
		if (_E281 == null)
		{
			return;
		}
		TimeSpan timeLeft = _E281.TimeLeft;
		if (timeLeft.Seconds != _E0A4)
		{
			_E0A4 = timeLeft.Seconds;
			string text = _ED3E._E000(234663).Localized();
			if (timeLeft.TotalSeconds < 1.0)
			{
				_E007(text);
			}
			else
			{
				_E007(text + _ED3E._E000(54246) + timeLeft.TimeLeftShortFormat() + _ED3E._E000(27308));
			}
		}
	}

	private void _E007(string message)
	{
		_voipBanMessageText.text = message;
		_voipBanMessage.SetActive(value: true);
	}

	private void _E008(bool value)
	{
		_voipEnableBlocker.SetBlock(!value, _ED3E._E000(192208));
	}

	public override void Close()
	{
		_E0A3?.Dispose();
		_voiceSelector.Hide();
		base.Close();
		_E25F = null;
		_E256 = null;
		_E0A3 = null;
		_E281 = null;
	}

	[CompilerGenerated]
	private void _E009()
	{
		_E256.OnBanChanged -= _E005;
	}
}
