using System;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.Weather;
using UnityEngine;

public class WeatherEventController : MonoBehaviour
{
	public static WeatherEventController instance;

	public const string WEATHER_EVENT = "weatherEvent";

	private bool m__E000;

	[CompilerGenerated]
	private static bool _E001;

	private int _E002;

	private float _E003;

	[CompilerGenerated]
	private int _E004 = 21;

	[CompilerGenerated]
	private int _E005 = 40;

	private int _E006 = 45;

	private DateTime _E007;

	[CompilerGenerated]
	private readonly _E8EB _E008 = new _E8EB();

	public float currentPercantage;

	private long _E009;

	private float _E00A;

	public bool EventActive => this.m__E000;

	public static bool ChangeTimeInProgress
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
		[CompilerGenerated]
		private set
		{
			_E001 = value;
		}
	}

	public int DesiredHour
	{
		[CompilerGenerated]
		get
		{
			return _E004;
		}
		[CompilerGenerated]
		private set
		{
			_E004 = value;
		}
	}

	public int DesiredMinute
	{
		[CompilerGenerated]
		get
		{
			return _E005;
		}
		[CompilerGenerated]
		private set
		{
			_E005 = value;
		}
	}

	public _E8EB DestWeatherNode
	{
		[CompilerGenerated]
		get
		{
			return _E008;
		}
	}

	private _E629 _E00B => TOD_Sky.Instance.Components.Time.GameDateTime;

	private void Awake()
	{
		instance = this;
	}

	private void OnDestroy()
	{
		instance = null;
	}

	public void SetWeather(int hours, int minutes, float Cloudness, int WindDirection, float Wind, float Rain, float RainRandomness, float ScaterringFogDensity, Vector2 TopWindDirection)
	{
		DesiredHour = hours;
		DesiredMinute = minutes;
		DestWeatherNode.Cloudness = Cloudness;
		DestWeatherNode.WindDirection = WindDirection;
		DestWeatherNode.Wind = Wind;
		DestWeatherNode.Rain = Rain;
		DestWeatherNode.RainRandomness = RainRandomness;
		DestWeatherNode.Temperature = 20f;
		DestWeatherNode.AtmospherePressure = 760f;
		DestWeatherNode.ScaterringFogDensity = ScaterringFogDensity;
		DestWeatherNode.TopWindDirection = TopWindDirection;
	}

	private void _E000()
	{
		Activate(val: true);
	}

	public void Activate(bool val, int fromPercent = 0)
	{
		if (this.m__E000)
		{
			return;
		}
		this.m__E000 = val;
		if (this.m__E000)
		{
			float num = (float)fromPercent / 100f;
			_E003 = _E00B.TimeFactor;
			DateTime dateTime = _E00B.Calculate();
			_E007 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, DesiredHour, DesiredMinute, dateTime.Second, DateTimeKind.Utc);
			if (_E007 < dateTime)
			{
				_E007 = _E007.AddDays(1.0);
			}
			int num2 = Mathf.CeilToInt((float)_E006 * (1f - num));
			TimeSpan timeSpan = _E007.Subtract(dateTime);
			TimeSpan timeSpan2 = DateTime.Now.AddSeconds(num2).Subtract(DateTime.Now);
			if (num2 > 0)
			{
				float timeFactorMod = (float)timeSpan.Ticks / (float)timeSpan2.Ticks / _E003;
				_E00B.TimeFactorMod = timeFactorMod;
			}
			long value = (long)((float)timeSpan.Ticks * num);
			_E00B.Look();
			_E00B.ResetForce(dateTime.AddTicks(value));
			DestWeatherNode.Time = _E5AD.MoscowNow.AddSeconds(Mathf.Max(num2, 1)).Ticks;
			WeatherController.Instance.SetWeatherForce(DestWeatherNode);
			_E00A = TOD_Sky.Instance.Light.UpdateInterval;
			TOD_Sky.Instance.Light.UpdateInterval = 0f;
			ChangeTimeInProgress = true;
			_E009 = dateTime.Ticks;
		}
	}

	private void Update()
	{
		if (this.m__E000)
		{
			DateTime gameDateTime = _E00B.Calculate();
			currentPercantage = Mathf.Clamp01((float)(gameDateTime.Ticks - _E009) / (float)(_E007.Ticks - _E009));
			if (currentPercantage >= 1f && ChangeTimeInProgress)
			{
				ChangeTimeInProgress = false;
				_E00B.ResetForce(gameDateTime);
				_E00B.TimeFactorMod = 1f;
				Singleton<_E307>.Instance.AnyEvent(_ED3E._E000(54075));
				TOD_Sky.Instance.Light.UpdateInterval = _E00A;
				TOD_Sky.Instance.Components.Time.LockCurrentTime = true;
			}
		}
	}
}
