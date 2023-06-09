using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using Comfort.Common;
using EFT.Counters;
using EFT.Utilities;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class OverallScreen : UIElement
{
	public sealed class _E000 : _EC64<OverallScreen>
	{
		private new readonly Profile m__E000;

		[CanBeNull]
		private readonly _EAED _E001;

		public _E000(OverallScreen overallTab, Profile profile, [CanBeNull] _EAED inventoryController)
			: base(overallTab)
		{
			m__E000 = profile;
			_E001 = inventoryController;
		}

		public override void Show()
		{
			base._E000.Show(m__E000, _E001);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public Profile profile;

		public OverallScreen _003C_003E4__this;

		public _EAED inventoryController;

		internal void _E000(_EBE6 suite)
		{
			suite.SetClothingsToProfile(profile);
			if (_003C_003E4__this._playerModelView.gameObject.activeSelf)
			{
				_003C_003E4__this._playerModelView.Close();
			}
			_003C_003E4__this._E001(profile, inventoryController);
			_003C_003E4__this._E19D?.Invoke(suite);
		}
	}

	[SerializeField]
	private StatisticsSpawn _statsSpawn;

	[SerializeField]
	private CustomTextMeshProUGUI _nicknameLabel;

	[SerializeField]
	private ChatSpecialIcon _specialIcon;

	[SerializeField]
	private CustomTextMeshProUGUI _experienceLabel;

	[SerializeField]
	private PlayerModelView _playerModelView;

	[SerializeField]
	private XCoordRotation _rotator;

	[SerializeField]
	private PlayerLevelPanel _playerLevelPanel;

	[SerializeField]
	private DefaultUIButton _backButton;

	[SerializeField]
	private ShortStatsPanel _shortStatsPanel;

	[SerializeField]
	private Image _sideImage;

	[SerializeField]
	private Sprite _bearSprite;

	[SerializeField]
	private Sprite _usecSprite;

	[SerializeField]
	private CustomTextMeshProUGUI _raidsLabel;

	[SerializeField]
	private CustomTextMeshProUGUI _runsLabel;

	[SerializeField]
	private CustomTextMeshProUGUI _survivedLabel;

	[SerializeField]
	private CustomTextMeshProUGUI _survivalRate;

	[SerializeField]
	private CustomTextMeshProUGUI _kiaLabel;

	[SerializeField]
	private CustomTextMeshProUGUI _averageLifetimeLabel;

	[SerializeField]
	private CustomTextMeshProUGUI _miaLabel;

	[SerializeField]
	private CustomTextMeshProUGUI _rowSLabel;

	[SerializeField]
	private CustomTextMeshProUGUI _awolLabel;

	[SerializeField]
	private CustomTextMeshProUGUI _leaveRateLabel;

	[SerializeField]
	private CustomTextMeshProUGUI _killsLabel;

	[SerializeField]
	private CustomTextMeshProUGUI _kdRatioLabel;

	[SerializeField]
	private LocalizedText _onlineTimeLabel;

	[SerializeField]
	private LocalizedText _overallLifetimeLabel;

	[SerializeField]
	private UIAnimatedToggleSpawner _overallToggle;

	[SerializeField]
	private UIAnimatedToggleSpawner _pmcToggle;

	[SerializeField]
	private UIAnimatedToggleSpawner _scavToggle;

	[SerializeField]
	private DragTrigger _dragTrigger;

	[SerializeField]
	private InventoryClothingSelectionPanel _clothingPanel;

	[CompilerGenerated]
	private Action<_EBE6> _E19D;

	[CompilerGenerated]
	private Action _E18D;

	private Profile _E0B7;

	public event Action<_EBE6> OnCustomizationChanged
	{
		[CompilerGenerated]
		add
		{
			Action<_EBE6> action = _E19D;
			Action<_EBE6> action2;
			do
			{
				action2 = action;
				Action<_EBE6> value2 = (Action<_EBE6>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E19D, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<_EBE6> action = _E19D;
			Action<_EBE6> action2;
			do
			{
				action2 = action;
				Action<_EBE6> value2 = (Action<_EBE6>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E19D, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action OnBackButtonClick
	{
		[CompilerGenerated]
		add
		{
			Action action = _E18D;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E18D, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E18D;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E18D, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	private void Awake()
	{
		_overallToggle.SpawnedObject.onValueChanged.AddListener(delegate(bool value)
		{
			_E003(_overallToggle, value);
		});
		_pmcToggle.SpawnedObject.onValueChanged.AddListener(delegate(bool value)
		{
			_E003(_pmcToggle, value);
		});
		_scavToggle.SpawnedObject.onValueChanged.AddListener(delegate(bool value)
		{
			_E003(_scavToggle, value);
		});
		_backButton.OnClick.AddListener(delegate
		{
			_E18D?.Invoke();
		});
	}

	private void Show(Profile profile, [CanBeNull] _EAED inventoryController)
	{
		inventoryController?.StopProcesses();
		ItemUiContext.Instance.CloseAllWindows();
		ShowGameObject();
		_E0B7 = profile;
		_E003(_overallToggle, value: true);
		_overallToggle._E001 = true;
		_playerLevelPanel.Set(profile.Info.Level, ESideType.Pmc);
		_nicknameLabel.text = profile.GetCorrectedNickname();
		_specialIcon.Show((profile.Info.Side != EPlayerSide.Savage) ? profile.Info.MemberCategory : EMemberCategory.Default);
		NumberFormatInfo numberFormatInfo = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
		numberFormatInfo.NumberGroupSeparator = _ED3E._E000(18502);
		_experienceLabel.text = profile.Info.Experience.ToString(_ED3E._E000(258812), numberFormatInfo);
		_E000(profile, inventoryController);
		_E001(profile, inventoryController);
		UI.AddDisposable(_clothingPanel);
		UI.AddDisposable(_playerModelView);
		_statsSpawn._E000(profile, StatisticsSpawn.EStatisticsType.Overall);
		UI.AddDisposable(_statsSpawn);
		_sideImage.gameObject.SetActive(profile.Info.Side != EPlayerSide.Savage);
		switch (profile.Info.Side)
		{
		case EPlayerSide.Bear:
			_sideImage.sprite = _bearSprite;
			break;
		case EPlayerSide.Usec:
			_sideImage.sprite = _usecSprite;
			break;
		case EPlayerSide.Savage:
			_sideImage.sprite = null;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		long allLong = _E0B7.Stats.OverallCounters.GetAllLong(CounterTag.Sessions);
		long allLong2 = _E0B7.Stats.OverallCounters.GetAllLong(CounterTag.ExitStatus, ExitStatus.Left);
		long allLong3 = _E0B7.Stats.OverallCounters.GetAllLong(CounterTag.ExitStatus, ExitStatus.Survived);
		long allLong4 = _E0B7.Stats.OverallCounters.GetAllLong(CounterTag.Deaths);
		long allLong5 = _E0B7.Stats.OverallCounters.GetAllLong(CounterTag.Kills);
		long totalInGameTime = _E0B7.Stats.TotalInGameTime;
		ShortStatsPanel shortStatsPanel = _shortStatsPanel;
		float kills = allLong5;
		shortStatsPanel.SetStats(allLong, kills, (allLong > 0) ? ((int)((float)allLong2 / (float)allLong * 100f)) : 0, (allLong > 0) ? ((int)((float)allLong3 / (float)allLong * 100f)) : 0, (allLong4 > 0) ? ((float)Math.Round((double)allLong5 / (double)allLong4, 2)) : 0f, _E004(totalInGameTime));
	}

	private void _E000(Profile profile, _EAED inventoryController)
	{
		bool flag = !_E7A3.InRaid;
		_clothingPanel.gameObject.SetActive(flag);
		if (!flag)
		{
			return;
		}
		IEnumerable<_EBE6> availableSuites = Singleton<_E60E>.Instance.AvailableSuites;
		List<_EBE6> list = new List<_EBE6>();
		List<_EBE6> list2 = new List<_EBE6>();
		string text = profile.Customization[EBodyModelPart.Body];
		string text2 = profile.Customization[EBodyModelPart.Feet];
		_EBE6 selectedUpperSuite = null;
		_EBE6 selectedLowerSuite = null;
		foreach (_EBE6 item3 in availableSuites)
		{
			_EBE6 obj = item3;
			if (obj != null)
			{
				if (!(obj is _EBE7 obj2))
				{
					if (obj is _EBE8 obj3)
					{
						_EBE8 item = obj3;
						list2.Add(item);
					}
				}
				else
				{
					_EBE7 item2 = obj2;
					list.Add(item2);
				}
			}
			if (text == item3.MainBodyPartItem)
			{
				selectedUpperSuite = item3;
			}
			else if (text2 == item3.MainBodyPartItem)
			{
				selectedLowerSuite = item3;
			}
		}
		_clothingPanel.Show(list, selectedUpperSuite, list2, selectedLowerSuite, delegate(_EBE6 suite)
		{
			suite.SetClothingsToProfile(profile);
			if (_playerModelView.gameObject.activeSelf)
			{
				_playerModelView.Close();
			}
			_E001(profile, inventoryController);
			_E19D?.Invoke(suite);
		});
	}

	private void _E001(Profile profile, _EAED inventoryController)
	{
		_playerModelView.Show(profile, inventoryController, delegate
		{
			_rotator.Init(_playerModelView.ModelPlayerPoser.transform);
			_dragTrigger.onDrag += _E002;
			UI.AddDisposable(delegate
			{
				_dragTrigger.onDrag -= _E002;
			});
		}).HandleExceptions();
	}

	private void _E002(PointerEventData pointerData)
	{
		_rotator.Rotate(pointerData.delta.x);
	}

	private void _E003(UnityEngine.Object toggle, bool value)
	{
		if (value)
		{
			if (toggle == _pmcToggle || toggle == _scavToggle)
			{
				CounterTag counterTag = ((toggle == _scavToggle) ? CounterTag.Scav : CounterTag.Pmc);
				long @long = _E0B7.Stats.OverallCounters.GetLong(CounterTag.Sessions, counterTag);
				_raidsLabel.text = @long.ToString();
				_runsLabel.text = _E0B7.Stats.OverallCounters.GetLong(CounterTag.ExitStatus, ExitStatus.Runner, counterTag).ToString();
				long long2 = _E0B7.Stats.OverallCounters.GetLong(CounterTag.ExitStatus, ExitStatus.Survived, counterTag);
				_survivedLabel.text = long2.ToString();
				long long3 = _E0B7.Stats.OverallCounters.GetLong(CounterTag.Deaths, counterTag);
				_kiaLabel.text = long3.ToString();
				_miaLabel.text = _E0B7.Stats.OverallCounters.GetLong(CounterTag.ExitStatus, ExitStatus.MissingInAction, counterTag).ToString();
				long long4 = _E0B7.Stats.OverallCounters.GetLong(CounterTag.ExitStatus, ExitStatus.Left, counterTag);
				_awolLabel.text = long4.ToString();
				_survivalRate.text = ((@long > 0) ? ((int)((float)long2 / (float)@long * 100f) + _ED3E._E000(215182)) : _ED3E._E000(29690));
				_leaveRateLabel.text = ((@long > 0) ? ((int)((float)long4 / (float)@long * 100f) + _ED3E._E000(215182)) : _ED3E._E000(29690));
				long totalInGameTime = _E0B7.Stats.TotalInGameTime;
				TimeSpan timeSpan = TimeSpan.FromSeconds(totalInGameTime);
				_onlineTimeLabel.SetFormatValues((int)timeSpan.TotalHours, timeSpan.Minutes);
				_averageLifetimeLabel.text = ((@long > 0) ? _E005((long)((float)totalInGameTime / (float)@long)) : _ED3E._E000(29690));
				_E006();
				_rowSLabel.text = _E0B7.Stats.OverallCounters.GetLong(CounterTag.LongestWinStreak, counterTag).ToString();
				long long5 = _E0B7.Stats.OverallCounters.GetLong(CounterTag.Kills, counterTag);
				_killsLabel.text = long5.ToString();
				_kdRatioLabel.text = ((long3 > 0) ? string.Format(_ED3E._E000(258808), (float)long5 / (float)long3) : _ED3E._E000(29690));
			}
			else if (toggle == _overallToggle)
			{
				long allLong = _E0B7.Stats.OverallCounters.GetAllLong(CounterTag.Sessions);
				_raidsLabel.text = allLong.ToString();
				_runsLabel.text = _E0B7.Stats.OverallCounters.GetAllLong(CounterTag.ExitStatus, ExitStatus.Runner).ToString();
				long allLong2 = _E0B7.Stats.OverallCounters.GetAllLong(CounterTag.ExitStatus, ExitStatus.Survived);
				_survivedLabel.text = allLong2.ToString();
				long allLong3 = _E0B7.Stats.OverallCounters.GetAllLong(CounterTag.ExitStatus, ExitStatus.Killed);
				_kiaLabel.text = allLong3.ToString();
				_miaLabel.text = _E0B7.Stats.OverallCounters.GetAllLong(CounterTag.ExitStatus, ExitStatus.MissingInAction).ToString();
				long allLong4 = _E0B7.Stats.OverallCounters.GetAllLong(CounterTag.ExitStatus, ExitStatus.Left);
				_awolLabel.text = allLong4.ToString();
				_survivalRate.text = ((allLong > 0) ? ((int)((float)allLong2 / (float)allLong * 100f) + _ED3E._E000(215182)) : _ED3E._E000(29690));
				_leaveRateLabel.text = ((allLong > 0) ? ((int)((float)allLong4 / (float)allLong * 100f) + _ED3E._E000(215182)) : _ED3E._E000(29690));
				long totalInGameTime2 = _E0B7.Stats.TotalInGameTime;
				TimeSpan timeSpan2 = TimeSpan.FromSeconds(totalInGameTime2);
				_onlineTimeLabel.SetFormatValues((int)timeSpan2.TotalHours, timeSpan2.Minutes);
				_averageLifetimeLabel.text = ((allLong > 0) ? _E005((long)((float)totalInGameTime2 / (float)allLong)) : _ED3E._E000(29690));
				_E006();
				_rowSLabel.text = _E0B7.Stats.OverallCounters.GetAllLong(CounterTag.LongestWinStreak).ToString();
				long allLong5 = _E0B7.Stats.OverallCounters.GetAllLong(CounterTag.Kills);
				_killsLabel.text = allLong5.ToString();
				_kdRatioLabel.text = ((allLong3 > 0) ? string.Format(_ED3E._E000(258808), (float)allLong5 / (float)allLong3) : _ED3E._E000(29690));
			}
		}
	}

	private static string _E004(long seconds)
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
		return string.Format(_ED3E._E000(259361), timeSpan.TotalHours);
	}

	private static string _E005(long seconds)
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
		return string.Format(_ED3E._E000(258802), timeSpan.Minutes, timeSpan.Seconds);
	}

	private void _E006()
	{
		TimeSpan timeSpan = _E5AD.UtcNow - _E5AD.LocalDateTimeFromUnixTime(_E0B7.Info.RegistrationDate);
		if ((int)timeSpan.TotalMinutes <= 0)
		{
			_overallLifetimeLabel.LocalizationKey = _ED3E._E000(258784);
		}
		else if (timeSpan <= TimeSpan.FromHours(48.0))
		{
			_overallLifetimeLabel.LocalizationKey = _ED3E._E000(258841);
			_overallLifetimeLabel.SetFormatValues((int)timeSpan.TotalHours, timeSpan.Minutes);
		}
		else if (timeSpan <= TimeSpan.FromDays(365.0))
		{
			_overallLifetimeLabel.LocalizationKey = _ED3E._E000(258829);
			_overallLifetimeLabel.SetFormatValues((int)timeSpan.TotalDays);
		}
		else
		{
			int num = (int)(timeSpan.TotalDays / 365.0);
			int num2 = (int)(timeSpan.TotalDays - (double)(365 * num));
			_overallLifetimeLabel.LocalizationKey = _ED3E._E000(258826);
			_overallLifetimeLabel.SetFormatValues(num, num2);
		}
	}

	[CompilerGenerated]
	private void _E007(bool value)
	{
		_E003(_overallToggle, value);
	}

	[CompilerGenerated]
	private void _E008(bool value)
	{
		_E003(_pmcToggle, value);
	}

	[CompilerGenerated]
	private void _E009(bool value)
	{
		_E003(_scavToggle, value);
	}

	[CompilerGenerated]
	private void _E00A()
	{
		_E18D?.Invoke();
	}

	[CompilerGenerated]
	private void _E00B()
	{
		_rotator.Init(_playerModelView.ModelPlayerPoser.transform);
		_dragTrigger.onDrag += _E002;
		UI.AddDisposable(delegate
		{
			_dragTrigger.onDrag -= _E002;
		});
	}

	[CompilerGenerated]
	private void _E00C()
	{
		_dragTrigger.onDrag -= _E002;
	}
}
