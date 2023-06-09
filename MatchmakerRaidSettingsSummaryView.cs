using System.Collections.Generic;
using EFT;
using EFT.Bots;
using EFT.UI;
using EFT.UI.Matchmaker;
using UnityEngine;

public sealed class MatchmakerRaidSettingsSummaryView : UIElement
{
	private const string _E000 = "Enabled";

	private const string _E001 = "Disabled";

	[SerializeField]
	private List<CanvasGroup> _labelsCanvasGroups;

	[SerializeField]
	private MatchmakerRaidSettingView _coopEnabledSetting;

	[SerializeField]
	private MatchmakerRaidSettingView _playerSpawnSetting;

	[SerializeField]
	private MatchmakerRaidSettingView _FoodAndWaterSetting;

	[SerializeField]
	private MatchmakerRaidSettingView _weatherSettings;

	[SerializeField]
	private MatchmakerRaidSettingView _dayTimeSettings;

	[SerializeField]
	private MatchmakerRaidSettingView _randomWeatherSettings;

	[SerializeField]
	private MatchmakerRaidSettingView _randomTimeSettings;

	[SerializeField]
	private MatchmakerRaidSettingView _timeFlowSettings;

	[SerializeField]
	private MatchmakerRaidSettingView _freeCameraSettings;

	[SerializeField]
	private MatchmakerRaidSettingView _openedDoorsSettings;

	[SerializeField]
	private MatchmakerRaidSettingView _healthSettings;

	[SerializeField]
	private MatchmakerRaidSettingView _overloadSettings;

	[SerializeField]
	private MatchmakerRaidSettingView _armsStaminaSettings;

	[SerializeField]
	private MatchmakerRaidSettingView _legsStaminaSettings;

	[SerializeField]
	private MatchmakerRaidSettingView _aiAmountSettings;

	[SerializeField]
	private MatchmakerRaidSettingView _aiDifficultySettings;

	[SerializeField]
	private MatchmakerRaidSettingView _silentBotsSettings;

	[SerializeField]
	private MatchmakerRaidSettingView _bossesEnabledSettings;

	[SerializeField]
	private MatchmakerRaidSettingView _bossPickSettings;

	[SerializeField]
	private MatchmakerRaidSettingView _friendlyScavsSettings;

	[SerializeField]
	private MatchmakerRaidSettingView _scavWarSettings;

	[SerializeField]
	private MatchmakerRaidSettingView _cursedSettings;

	public void Show(RaidSettings raidSettings)
	{
		ShowGameObject();
		string text = _ED3E._E000(48795).Localized();
		string text2 = _ED3E._E000(48787).Localized();
		ERaidMode raidMode = raidSettings.RaidMode;
		foreach (CanvasGroup labelsCanvasGroup in _labelsCanvasGroups)
		{
			labelsCanvasGroup.SetUnlockStatus(raidMode != ERaidMode.Online);
		}
		_coopEnabledSetting.Refresh((raidSettings.RaidMode == ERaidMode.Coop) ? text : text2, raidMode == ERaidMode.Coop);
		_playerSpawnSetting.Refresh((_ED3E._E000(48772) + raidSettings.PlayersSpawnPlace.ToStringNoBox()).Localized(), raidMode == ERaidMode.Coop);
		_FoodAndWaterSetting.Refresh(raidSettings.MetabolismDisabled ? text : text2, raidMode == ERaidMode.Coop);
		_weatherSettings.Refresh((_ED3E._E000(48823) + raidSettings.TimeAndWeatherSettings.CloudinessType.ToStringNoBox()).Localized() + _ED3E._E000(30703) + (_ED3E._E000(48807) + raidSettings.TimeAndWeatherSettings.RainType.ToStringNoBox()).Localized() + _ED3E._E000(30703) + (_ED3E._E000(48857) + raidSettings.TimeAndWeatherSettings.WindType.ToStringNoBox()).Localized() + _ED3E._E000(30703) + (_ED3E._E000(48844) + raidSettings.TimeAndWeatherSettings.FogType.ToStringNoBox()).Localized(), raidMode == ERaidMode.Coop);
		_dayTimeSettings.Refresh((raidSettings.TimeAndWeatherSettings.HourOfDay == -1) ? (_ED3E._E000(48895) + EBotAmount.AsOnline.ToStringNoBox()).Localized() : string.Format(_ED3E._E000(48837), raidSettings.TimeAndWeatherSettings.HourOfDay), raidMode == ERaidMode.Coop);
		_randomWeatherSettings.Refresh(raidSettings.TimeAndWeatherSettings.IsRandomWeather ? text : text2, raidMode == ERaidMode.Local);
		_randomTimeSettings.Refresh(raidSettings.TimeAndWeatherSettings.IsRandomTime ? text : text2, raidMode == ERaidMode.Local);
		_timeFlowSettings.Refresh((_ED3E._E000(48882) + raidSettings.TimeAndWeatherSettings.TimeFlowType.ToStringNoBox()).Localized(), isActive: false);
		_freeCameraSettings.Refresh(text2, isActive: false);
		_openedDoorsSettings.Refresh(text2, isActive: false);
		_overloadSettings.Refresh(text2, isActive: false);
		_armsStaminaSettings.Refresh(text2, isActive: false);
		_legsStaminaSettings.Refresh(text2, isActive: false);
		_healthSettings.Refresh(_ED3E._E000(48864), isActive: false);
		_aiAmountSettings.Refresh((_ED3E._E000(48895) + raidSettings.WavesSettings.BotAmount.ToStringNoBox()).Localized(), raidMode != ERaidMode.Online);
		_aiDifficultySettings.Refresh((_ED3E._E000(48925) + raidSettings.WavesSettings.BotDifficulty.ToStringNoBox()).Localized(), raidMode != 0 && raidSettings.WavesSettings.BotAmount != EBotAmount.NoBots);
		_silentBotsSettings.Refresh(text2, isActive: false);
		_bossesEnabledSettings.Refresh(raidSettings.WavesSettings.IsBosses ? text : text2, raidMode == ERaidMode.Local);
		_bossPickSettings.Refresh((_ED3E._E000(48895) + EBotAmount.AsOnline.ToStringNoBox()).Localized(), isActive: false);
		_friendlyScavsSettings.Refresh(text2, isActive: false);
		_scavWarSettings.Refresh(raidSettings.BotSettings.IsScavWars ? text : text2, raidMode == ERaidMode.Local);
		_cursedSettings.Refresh(raidSettings.WavesSettings.IsTaggedAndCursed ? text : text2, raidMode == ERaidMode.Local);
	}
}
