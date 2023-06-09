using System;
using System.Runtime.CompilerServices;
using System.Threading;
using EFT.Weather;
using JsonType;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Matchmaker;

public sealed class LocationConditionsPanel : UIElement
{
	public const string DATE_TIME_FORMAT = "HH:mm:ss";

	[SerializeField]
	private TextMeshProUGUI _currentPhaseTime;

	[SerializeField]
	private TextMeshProUGUI _nextPhaseTime;

	[SerializeField]
	private Image _weatherIcon;

	[SerializeField]
	private CanvasGroup _canvasGroup;

	[SerializeField]
	private Toggle _amTimeToggle;

	[SerializeField]
	private Toggle _pmTimeToggle;

	[CompilerGenerated]
	private Action<EDateTime> _E2A8;

	private bool _E2A9;

	private _E796 _E17F;

	private EDateTime _E2AA;

	private bool _E2AB;

	private static DateTime _E000 => new DateTime(2016, 8, 4, 15, 28, 0);

	private static DateTime _E001 => new DateTime(2016, 8, 4, 3, 28, 0);

	public EDateTime SelectedDateTime
	{
		get
		{
			return _E2AA;
		}
		set
		{
			_E2AA = value;
			_E2A8?.Invoke(_E2AA);
		}
	}

	private DateTime _E002 => _E17F.GetCurrentLocationTime;

	public event Action<EDateTime> OnTimeChanged
	{
		[CompilerGenerated]
		add
		{
			Action<EDateTime> action = _E2A8;
			Action<EDateTime> action2;
			do
			{
				action2 = action;
				Action<EDateTime> value2 = (Action<EDateTime>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E2A8, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<EDateTime> action = _E2A8;
			Action<EDateTime> action2;
			do
			{
				action2 = action;
				Action<EDateTime> value2 = (Action<EDateTime>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E2A8, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	private void Awake()
	{
		if (_amTimeToggle != null)
		{
			_amTimeToggle.onValueChanged.AddListener(delegate(bool arg)
			{
				if (arg)
				{
					SelectedDateTime = EDateTime.CURR;
				}
			});
		}
		if (!(_pmTimeToggle != null))
		{
			return;
		}
		_pmTimeToggle.onValueChanged.AddListener(delegate(bool arg)
		{
			if (arg)
			{
				SelectedDateTime = EDateTime.PAST;
			}
		});
	}

	public void Set(_E796 session, RaidSettings raidSettings, bool takeFromCurrent)
	{
		ShowGameObject();
		_E17F = session;
		_E2AA = raidSettings.SelectedDateTime;
		_E2AB = takeFromCurrent;
		EWeatherType weatherTypeByNode = _E17F.Weather.GetWeatherTypeByNode();
		_E000();
		if (_canvasGroup != null)
		{
			_canvasGroup.SetUnlockStatus(value: true);
		}
		_weatherIcon.gameObject.SetActive(weatherTypeByNode != EWeatherType.None);
		string text = raidSettings.SelectedLocation?.Id;
		if (text == _ED3E._E000(124512) || text == _ED3E._E000(124565))
		{
			_weatherIcon.sprite = _E905.Pop<Sprite>(_ED3E._E000(233829) + EWeatherType.ClearDay);
			if (_amTimeToggle != null && _pmTimeToggle != null)
			{
				string text2 = LocationConditionsPanel._E000.ToString(_ED3E._E000(148382));
				string text3 = LocationConditionsPanel._E001.ToString(_ED3E._E000(148382));
				_currentPhaseTime.SetMonospaceText(text2);
				_nextPhaseTime.SetMonospaceText(text3);
			}
			else
			{
				_currentPhaseTime.text = ((raidSettings.SelectedDateTime == EDateTime.CURR) ? LocationConditionsPanel._E000.ToString(_ED3E._E000(148382)) : LocationConditionsPanel._E001.ToString(_ED3E._E000(148382)));
			}
			_E2A9 = false;
		}
		else
		{
			if (weatherTypeByNode != EWeatherType.None)
			{
				_weatherIcon.sprite = _E905.Pop<Sprite>(_ED3E._E000(233829) + weatherTypeByNode);
			}
			_E2A9 = true;
		}
		if (!(_amTimeToggle == null) || !(_pmTimeToggle == null))
		{
			_amTimeToggle.isOn = _E2AA != EDateTime.PAST;
			_pmTimeToggle.isOn = _E2AA == EDateTime.PAST;
		}
	}

	private void Update()
	{
		_E000();
	}

	private void _E000()
	{
		if (!_E2A9)
		{
			return;
		}
		if (_E2AB)
		{
			string text = this._E002.ToString(_ED3E._E000(148382));
			_currentPhaseTime.SetMonospaceText(text);
			if (!(_nextPhaseTime == null))
			{
				text = this._E002.AddHours(-12.0).ToString(_ED3E._E000(148382));
				_nextPhaseTime.SetMonospaceText(text);
			}
		}
		else
		{
			string text2 = ((_E2AA == EDateTime.CURR) ? this._E002.ToString(_ED3E._E000(148382)) : this._E002.AddHours(-12.0).ToString(_ED3E._E000(148382)));
			_currentPhaseTime.SetMonospaceText(text2);
		}
	}

	[CompilerGenerated]
	private void _E001(bool arg)
	{
		if (arg)
		{
			SelectedDateTime = EDateTime.CURR;
		}
	}

	[CompilerGenerated]
	private void _E002(bool arg)
	{
		if (arg)
		{
			SelectedDateTime = EDateTime.PAST;
		}
	}
}
