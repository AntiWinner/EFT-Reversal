using System;
using Comfort.Common;
using EFT;
using UnityEngine;

public class Watch : MonoBehaviour
{
	[SerializeField]
	private Transform _hourHand;

	[SerializeField]
	private Transform _minuteHand;

	[SerializeField]
	private Transform _secondHand;

	[SerializeField]
	private AnimationCurve _curve;

	private float _E000 = 1f;

	private DateTime _E001;

	private TimeSpan _E002;

	private DateTime _E003
	{
		get
		{
			TimeSpan timeSpan = TimeSpan.FromTicks((long)((float)(_E5AD.Now - _E001).Ticks * _E000));
			return _E001 + timeSpan + _E002;
		}
	}

	public void Init(TimeSpan timeDifference)
	{
		_E002 = timeDifference;
		if (Singleton<GameWorld>.Instantiated && Singleton<GameWorld>.Instance.GameDateTime != null)
		{
			_E629 gameDateTime = Singleton<GameWorld>.Instance.GameDateTime;
			_E000 = gameDateTime.TimeFactor;
			_E001 = _E5AD.Now;
			_E002 = gameDateTime.Calculate().TimeOfDay - _E001.TimeOfDay;
		}
	}

	private void Update()
	{
		TimeSpan timeOfDay = _E003.TimeOfDay;
		double num = timeOfDay.TotalSeconds % 60.0;
		float time = (float)(num - (double)(int)num);
		_hourHand.localRotation = Quaternion.Euler(0f, 0f, (float)timeOfDay.TotalHours * 30f);
		_minuteHand.localRotation = Quaternion.Euler(0f, 0f, (float)timeOfDay.TotalMinutes % 60f * 6f);
		_secondHand.localRotation = Quaternion.Euler(0f, 0f, ((float)(int)num + _curve.Evaluate(time)) * 6f);
	}
}
