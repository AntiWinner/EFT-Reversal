using System;
using System.Runtime.CompilerServices;
using EFT.Weather;
using JsonType;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Matchmaker;

public sealed class LocationConditionsPanelFactory : UIElement
{
	[SerializeField]
	private TextMeshProUGUI _currentPhaseTime;

	[SerializeField]
	private TextMeshProUGUI _nextPhaseTime;

	[SerializeField]
	private Image _weatherIcon;

	[SerializeField]
	private Toggle _amTimeToggle;

	[SerializeField]
	private Toggle _pmTimeToggle;

	private Coroutine _E2AC;

	private Action<EDateTime> _E2AD;

	[CompilerGenerated]
	private EDateTime _E2AE;

	private EDateTime _E000
	{
		[CompilerGenerated]
		get
		{
			return _E2AE;
		}
		[CompilerGenerated]
		set
		{
			_E2AE = value;
		}
	}

	private static DateTime _E001 => new DateTime(2016, 8, 4, 15, 28, 0);

	private static DateTime _E002 => new DateTime(2016, 8, 4, 3, 28, 0);

	private void Awake()
	{
		if (_amTimeToggle != null)
		{
			_amTimeToggle.onValueChanged.AddListener(delegate(bool arg)
			{
				if (arg)
				{
					this._E000 = EDateTime.CURR;
					_E2AD(EDateTime.CURR);
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
				this._E000 = EDateTime.PAST;
				_E2AD(EDateTime.PAST);
			}
		});
	}

	public void Set(EDateTime selectedTime, Action<EDateTime> onDateTimeSelected)
	{
		this._E000 = selectedTime;
		_E2AD = onDateTimeSelected;
		ShowGameObject();
		_weatherIcon.sprite = _E905.Pop<Sprite>(_ED3E._E000(233829) + EWeatherType.ClearDay);
		if (_amTimeToggle != null && _pmTimeToggle != null)
		{
			string text = LocationConditionsPanelFactory._E001.ToString(_ED3E._E000(148382));
			string text2 = _E002.ToString(_ED3E._E000(148382));
			_currentPhaseTime.SetMonospaceText(text);
			_nextPhaseTime.SetMonospaceText(text2);
			_amTimeToggle.isOn = selectedTime != EDateTime.PAST;
			_pmTimeToggle.isOn = selectedTime == EDateTime.PAST;
			onDateTimeSelected(this._E000);
		}
		else
		{
			_currentPhaseTime.text = ((selectedTime == EDateTime.CURR) ? LocationConditionsPanelFactory._E001.ToString(_ED3E._E000(148382)) : _E002.ToString(_ED3E._E000(148382)));
		}
	}

	[CompilerGenerated]
	private void _E000(bool arg)
	{
		if (arg)
		{
			this._E000 = EDateTime.CURR;
			_E2AD(EDateTime.CURR);
		}
	}

	[CompilerGenerated]
	private void _E001(bool arg)
	{
		if (arg)
		{
			this._E000 = EDateTime.PAST;
			_E2AD(EDateTime.PAST);
		}
	}
}
