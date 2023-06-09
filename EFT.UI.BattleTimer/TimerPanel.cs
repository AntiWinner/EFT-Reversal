using System;
using System.Text;
using TMPro;
using UnityEngine;

namespace EFT.UI.BattleTimer;

public class TimerPanel : UIElement
{
	private const float _E28F = 1f;

	[SerializeField]
	private TextMeshProUGUI _timerText;

	protected TimeSpan TimeSpan;

	private DateTime _E290;

	private StringBuilder _E0D5;

	private string _E291;

	private double _E292;

	protected TextMeshProUGUI TimerText => _timerText;

	protected virtual bool DisplayTime => true;

	public void Show(DateTime dateTime, StringBuilder stringBuilder)
	{
		ShowGameObject();
		_E290 = dateTime;
		_E0D5 = stringBuilder;
	}

	protected virtual void Update()
	{
		double utcNowUnix = _E5AD.UtcNowUnix;
		if (!(utcNowUnix - _E292 < 1.0))
		{
			_E292 = utcNowUnix;
			UpdateTimer();
		}
	}

	protected virtual void UpdateTimer()
	{
		TimeSpan = _E290 - _E5AD.UtcNow;
		if (DisplayTime && _E0D5 != null)
		{
			SetTimerText(TimeSpan);
		}
	}

	protected virtual void SetTimerText(TimeSpan timeSpan)
	{
		if (timeSpan.TotalSeconds <= 0.0)
		{
			TimerText.text = string.Empty;
			return;
		}
		_E0D5.Length = 0;
		_E0D5.AppendDigitalValue(timeSpan.Hours, 1);
		_E0D5.Append(_ED3E._E000(30697));
		_E0D5.AppendDigitalValue(timeSpan.Minutes, 2);
		_E0D5.Append(_ED3E._E000(30697));
		_E0D5.AppendDigitalValue(timeSpan.Seconds, 2);
		for (int i = _E0D5.Length; i < _E0D5.Capacity; i++)
		{
			_E0D5.Append('\0');
		}
		TimerText.SetMonospaceText(_E0D5.ToString());
	}
}
