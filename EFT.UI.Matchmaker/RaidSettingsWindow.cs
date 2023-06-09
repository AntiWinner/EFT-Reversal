using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.Bots;
using EFT.Weather;
using UnityEngine;
using UnityEngine.Events;

namespace EFT.UI.Matchmaker;

public sealed class RaidSettingsWindow : BaseUiWindow
{
	private sealed class _E000
	{
		public bool EnableBosses;

		public bool ScavWars;

		public bool TaggedAndCursed;

		public bool RandomWeather;

		public bool RandomTime;

		public bool MetabolismDisabled;

		public int AiDifficultyIndex;

		public int AiAmountIndex;

		public int PlayersSpawnPlaceIndex;

		public int CloudinessIndex;

		public int RainIndex;

		public int WindSpeedIndex;

		public int FogIndex;

		public int TimeFlowIndex;

		public int TimeOfDayIndex;
	}

	[HideInInspector]
	public readonly UnityEvent OnRaidSettingsChanged = new UnityEvent();

	[SerializeField]
	private UpdatableToggle _coopModeToggle;

	[SerializeField]
	private UiElementBlocker _coopModeBlocker;

	[SerializeField]
	[Header("Game settings")]
	private UpdatableToggle _metabolismDisabledToggle;

	[SerializeField]
	private UpdatableToggle _randomTimeToggle;

	[SerializeField]
	private UpdatableToggle _randomWeatherToggle;

	[SerializeField]
	private DropDownBox _playersSpawnPlaceDropdown;

	[SerializeField]
	private DropDownBox _cloudinessDropdown;

	[SerializeField]
	private DropDownBox _rainDropdown;

	[SerializeField]
	private DropDownBox _windSpeedDropdown;

	[SerializeField]
	private DropDownBox _fogDropdown;

	[SerializeField]
	private DropDownBox _timeFlowDropdown;

	[SerializeField]
	private DropDownBox _timeOfDayDropdown;

	[SerializeField]
	private List<CanvasGroup> _playersSpawnPlaceCanvasGroups;

	[SerializeField]
	private List<CanvasGroup> _randomTimeAndWeatherCanvasGroups;

	[SerializeField]
	private List<CanvasGroup> _waterAndFoodCanvasGroups;

	[SerializeField]
	private List<CanvasGroup> _timeCanvasGroups;

	[SerializeField]
	private List<CanvasGroup> _weatherCanvasGroups;

	[SerializeField]
	[Header("PVE settings")]
	private UpdatableToggle _enableBosses;

	[SerializeField]
	private UpdatableToggle _taggedAndCursed;

	[SerializeField]
	private UpdatableToggle _scavWars;

	[SerializeField]
	private DropDownBox _bossPickDropdown;

	[SerializeField]
	private DropDownBox _aiDifficultyDropdown;

	[SerializeField]
	private DropDownBox _aiAmountDropdown;

	[SerializeField]
	private CanvasGroup _aiAmountCanvasGroup;

	[SerializeField]
	private CanvasGroup _aiDifficultyCanvasGroup;

	[SerializeField]
	private List<CanvasGroup> _wavesCanvasGroups;

	private readonly List<CanvasGroup> _E2B5 = new List<CanvasGroup>();

	private RaidSettings _E29F;

	private readonly _E000 _E2B6 = new _E000();

	private _E72F _E256;

	private _EC99 m__E006;

	private _E72E _E2B7;

	protected override void Awake()
	{
		base.Awake();
		_coopModeToggle.onValueChanged.AddListener(delegate(bool value)
		{
			_E005(value);
		});
		_aiAmountDropdown.OnValueChanged.Subscribe(delegate
		{
			_E00A(_aiAmountDropdown.CurrentIndex != 1);
		});
		_E2B5.AddRange(_timeCanvasGroups);
		_E2B5.AddRange(_weatherCanvasGroups);
		_E2B5.AddRange(_wavesCanvasGroups);
		_E2B5.Add(_aiDifficultyCanvasGroup);
		_E2B5.Add(_aiAmountCanvasGroup);
		_E2B5.AddRange(_playersSpawnPlaceCanvasGroups);
		_E2B5.AddRange(_randomTimeAndWeatherCanvasGroups);
		_E2B5.AddRange(_waterAndFoodCanvasGroups);
	}

	public void Show(RaidSettings raidSettings, _E72F profileInfo, _EC99 matchmaker)
	{
		ShowGameObject();
		_E29F = raidSettings;
		_E256 = profileInfo;
		this.m__E006 = matchmaker;
		_E002(_aiDifficultyDropdown, _E3A5<EBotDifficulty>.Names.Select((string difficulty) => (_ED3E._E000(48925) + difficulty).Localized()), (int)_E29F.WavesSettings.BotDifficulty);
		_E002(_aiAmountDropdown, _E3A5<EBotAmount>.Names.Select((string amount) => (_ED3E._E000(48895) + amount).Localized()), (int)_E29F.WavesSettings.BotDifficulty);
		_E002(_playersSpawnPlaceDropdown, _E3A5<EPlayersSpawnPlace>.Names.Select((string place) => (_ED3E._E000(48772) + place).Localized()), (int)_E29F.PlayersSpawnPlace);
		_E002(_cloudinessDropdown, _E3A5<ECloudinessType>.Names.Select((string clouds) => (_ED3E._E000(48823) + clouds).Localized()), (int)_E29F.TimeAndWeatherSettings.CloudinessType);
		_E002(_rainDropdown, _E3A5<ERainType>.Names.Select((string rain) => (_ED3E._E000(48807) + rain).Localized()), (int)_E29F.TimeAndWeatherSettings.RainType);
		_E002(_windSpeedDropdown, _E3A5<EWindSpeed>.Names.Select((string wind) => (_ED3E._E000(48857) + wind).Localized()), (int)_E29F.TimeAndWeatherSettings.WindType);
		_E002(_fogDropdown, _E3A5<EFogType>.Names.Select((string fog) => (_ED3E._E000(48844) + fog).Localized()), (int)_E29F.TimeAndWeatherSettings.FogType);
		_E002(_timeFlowDropdown, _E3A5<ETimeFlowType>.Names.Select((string timeFlow) => (_ED3E._E000(48882) + timeFlow).Localized()), (int)_E29F.TimeAndWeatherSettings.TimeFlowType);
		_E002(_bossPickDropdown, _E3A5<EBossType>.Names.Select((string bossType) => (_ED3E._E000(236791) + bossType).Localized()), (int)_E29F.WavesSettings.BotDifficulty);
		List<string> list = new List<string> { (_ED3E._E000(48925) + EBotDifficulty.AsOnline.ToStringNoBox()).Localized() };
		for (int i = 0; i < 24; i++)
		{
			list.Add(i.ToString());
		}
		_E002(_timeOfDayDropdown, list, _E29F.TimeAndWeatherSettings.HourOfDay + 1);
		_E001(_coopModeToggle, _E29F.RaidMode == ERaidMode.Coop);
		_E001(_metabolismDisabledToggle, _E29F.MetabolismDisabled);
		_E001(_randomTimeToggle, _E29F.TimeAndWeatherSettings.IsRandomTime);
		_E001(_randomWeatherToggle, _E29F.TimeAndWeatherSettings.IsRandomWeather);
		_E001(_enableBosses, _E29F.WavesSettings.IsBosses);
		_E001(_taggedAndCursed, _E29F.WavesSettings.IsTaggedAndCursed);
		_E001(_scavWars, _E29F.BotSettings.IsScavWars);
		_E005(_E29F.RaidMode == ERaidMode.Coop);
		_E256.OnBanChanged += _E000;
		_E000();
		UI.AddDisposable(delegate
		{
			_E256.OnBanChanged -= _E000;
		});
		UI.BindEvent(this.m__E006.GroupPlayers.ItemsChanged, _E003);
	}

	private void _E000(EBanType banType = EBanType.Online)
	{
		if (banType == EBanType.Online)
		{
			_E2B7 = _E256.GetBan(EBanType.Online);
			_coopModeBlocker.SetBlock(_E2B7 != null);
			if (_E2B7 != null)
			{
				_coopModeToggle.UpdateValue(value: false);
			}
		}
	}

	private void _E001(UpdatableToggle toggle, bool initValue)
	{
		toggle.UpdateValue(initValue);
		toggle.Bind(delegate
		{
			_E004();
		});
	}

	private void _E002(DropDownBox dropDown, IEnumerable<string> values, int initValue)
	{
		dropDown.Show(values);
		dropDown.UpdateValue(initValue, sendCallback: false);
		dropDown.Bind(delegate
		{
			_E004();
		});
		UI.AddDisposable(dropDown.Hide);
	}

	private void _E003()
	{
		ECoopBlock reason;
		bool coopBlockReason = this.m__E006.GetCoopBlockReason(out reason);
		bool flag = this.m__E006.GroupPlayers.Count == 1;
		string reason2 = ((reason == ECoopBlock.NoBlock) ? string.Empty : reason.LocalizedEnum());
		if (!coopBlockReason)
		{
			_coopModeToggle.UpdateValue(value: false);
		}
		_coopModeBlocker.SetBlock(!coopBlockReason || !flag, reason2);
	}

	private void _E004()
	{
		_E29F.RaidMode = ((!_coopModeToggle.isOn) ? ERaidMode.Local : ERaidMode.Coop);
		_E29F.PlayersSpawnPlace = (EPlayersSpawnPlace)_playersSpawnPlaceDropdown.CurrentIndex;
		_E29F.MetabolismDisabled = _metabolismDisabledToggle.isOn;
		_E29F.TimeAndWeatherSettings = new TimeAndWeatherSettings(_randomTimeToggle.isOn, _randomWeatherToggle.isOn, _cloudinessDropdown.CurrentIndex, _rainDropdown.CurrentIndex, _windSpeedDropdown.CurrentIndex, _fogDropdown.CurrentIndex, _timeFlowDropdown.CurrentIndex, _timeOfDayDropdown.CurrentIndex - 1);
		_E29F.BotSettings = new BotControllerSettings(_aiAmountDropdown.CurrentIndex != 1, _scavWars.isOn, (EBotAmount)_aiAmountDropdown.CurrentIndex);
		_E29F.WavesSettings = new WavesSettings((EBotAmount)_aiAmountDropdown.CurrentIndex, (EBotDifficulty)_aiDifficultyDropdown.CurrentIndex, _enableBosses.isOn, _taggedAndCursed.isOn);
		OnRaidSettingsChanged?.Invoke();
	}

	private void _E005(bool value)
	{
		if (value)
		{
			_E008();
			foreach (CanvasGroup item in _E2B5)
			{
				item.SetUnlockStatus(value: false);
			}
			_randomWeatherToggle.UpdateValue(value: false);
			_randomTimeToggle.UpdateValue(value: false);
			_aiDifficultyDropdown.UpdateValue(0);
			_aiAmountDropdown.UpdateValue(0);
			_enableBosses.UpdateValue(value: true);
			_scavWars.UpdateValue(value: false);
			_taggedAndCursed.UpdateValue(value: false);
			_E00A(value: false);
			foreach (CanvasGroup weatherCanvasGroup in _weatherCanvasGroups)
			{
				weatherCanvasGroup.SetUnlockStatus(value: true);
			}
			foreach (CanvasGroup timeCanvasGroup in _timeCanvasGroups)
			{
				timeCanvasGroup.SetUnlockStatus(value: true);
			}
			foreach (CanvasGroup randomTimeAndWeatherCanvasGroup in _randomTimeAndWeatherCanvasGroups)
			{
				randomTimeAndWeatherCanvasGroup.SetUnlockStatus(value: false);
			}
			foreach (CanvasGroup playersSpawnPlaceCanvasGroup in _playersSpawnPlaceCanvasGroups)
			{
				playersSpawnPlaceCanvasGroup.SetUnlockStatus(value: true);
			}
			{
				foreach (CanvasGroup waterAndFoodCanvasGroup in _waterAndFoodCanvasGroups)
				{
					waterAndFoodCanvasGroup.SetUnlockStatus(value: true);
				}
				return;
			}
		}
		_E009();
		_E006(_E29F.Local);
	}

	private void _E006(bool value)
	{
		foreach (CanvasGroup item in _E2B5)
		{
			item.SetUnlockStatus(value: false);
		}
		_E003();
		_E00A(value);
		_E007(value && !_coopModeToggle.isOn);
		foreach (CanvasGroup randomTimeAndWeatherCanvasGroup in _randomTimeAndWeatherCanvasGroups)
		{
			randomTimeAndWeatherCanvasGroup.SetUnlockStatus(value);
		}
	}

	private void _E007(bool val)
	{
		foreach (CanvasGroup randomTimeAndWeatherCanvasGroup in _randomTimeAndWeatherCanvasGroups)
		{
			randomTimeAndWeatherCanvasGroup.SetUnlockStatus(val);
		}
	}

	private void _E008()
	{
		_E2B6.EnableBosses = _enableBosses.isOn;
		_E2B6.ScavWars = _scavWars.isOn;
		_E2B6.RandomTime = _randomTimeToggle.isOn;
		_E2B6.RandomWeather = _randomWeatherToggle.isOn;
		_E2B6.TaggedAndCursed = _taggedAndCursed.isOn;
		_E2B6.MetabolismDisabled = _metabolismDisabledToggle.isOn;
		_E2B6.AiDifficultyIndex = _aiDifficultyDropdown.CurrentIndex;
		_E2B6.AiAmountIndex = _aiAmountDropdown.CurrentIndex;
		_E2B6.PlayersSpawnPlaceIndex = _playersSpawnPlaceDropdown.CurrentIndex;
		_E2B6.CloudinessIndex = _cloudinessDropdown.CurrentIndex;
		_E2B6.RainIndex = _rainDropdown.CurrentIndex;
		_E2B6.WindSpeedIndex = _windSpeedDropdown.CurrentIndex;
		_E2B6.FogIndex = _fogDropdown.CurrentIndex;
		_E2B6.TimeFlowIndex = _timeFlowDropdown.CurrentIndex;
		_E2B6.TimeOfDayIndex = _timeOfDayDropdown.CurrentIndex;
	}

	private void _E009()
	{
		_enableBosses.UpdateValue(_E2B6.EnableBosses);
		_scavWars.UpdateValue(_E2B6.ScavWars);
		_randomTimeToggle.UpdateValue(_E2B6.RandomTime);
		_randomWeatherToggle.UpdateValue(_E2B6.RandomWeather);
		_taggedAndCursed.UpdateValue(_E2B6.TaggedAndCursed);
		_metabolismDisabledToggle.UpdateValue(_E2B6.MetabolismDisabled);
		_aiDifficultyDropdown.UpdateValue(_E2B6.AiDifficultyIndex);
		_aiAmountDropdown.UpdateValue(_E2B6.AiAmountIndex);
		_playersSpawnPlaceDropdown.UpdateValue(_E2B6.PlayersSpawnPlaceIndex);
		_cloudinessDropdown.UpdateValue(_E2B6.CloudinessIndex);
		_rainDropdown.UpdateValue(_E2B6.RainIndex);
		_windSpeedDropdown.UpdateValue(_E2B6.WindSpeedIndex);
		_fogDropdown.UpdateValue(_E2B6.FogIndex);
		_timeFlowDropdown.UpdateValue(_E2B6.TimeFlowIndex);
		_timeOfDayDropdown.UpdateValue(_E2B6.TimeOfDayIndex);
	}

	private void _E00A(bool value)
	{
		bool flag = value;
		if (_coopModeToggle.isOn || _aiAmountDropdown.CurrentIndex == 1)
		{
			flag = false;
		}
		foreach (CanvasGroup wavesCanvasGroup in _wavesCanvasGroups)
		{
			wavesCanvasGroup.interactable = flag && !_E2A0.Core.WAVE_ONLY_AS_ONLINE;
			wavesCanvasGroup.blocksRaycasts = flag;
			wavesCanvasGroup.alpha = ((flag && !_E2A0.Core.WAVE_ONLY_AS_ONLINE) ? 1f : 0.3f);
		}
		_aiDifficultyCanvasGroup.SetUnlockStatus(_aiAmountDropdown.CurrentIndex != 1);
		_aiAmountCanvasGroup.SetUnlockStatus(value: true);
	}

	[CompilerGenerated]
	private void _E00B(bool value)
	{
		_E005(value);
	}

	[CompilerGenerated]
	private void _E00C()
	{
		_E00A(_aiAmountDropdown.CurrentIndex != 1);
	}

	[CompilerGenerated]
	private void _E00D()
	{
		_E256.OnBanChanged -= _E000;
	}

	[CompilerGenerated]
	private void _E00E(bool _)
	{
		_E004();
	}

	[CompilerGenerated]
	private void _E00F(int _)
	{
		_E004();
	}
}
