using System;
using UnityEngine;

[Serializable]
public class TOD_CycleParameters
{
	[Tooltip("Current hour of the day.")]
	[Range(0f, 24f)]
	public float Hour = 12f;

	[Tooltip("Current day of the month.")]
	[Range(0f, 31f)]
	public int Day = 15;

	[Range(0f, 11f)]
	[Tooltip("Current month of the year.")]
	public int Month = 6;

	[Tooltip("Current year.")]
	[_E05A(1f, 9999f)]
	public int Year = 2000;

	public DateTime DateTime
	{
		get
		{
			return new DateTime(0L, DateTimeKind.Utc).AddYears(Year - 1).AddMonths(Month - 1).AddDays(Day - 1)
				.AddHours(Hour);
		}
		set
		{
			Year = value.Year;
			Month = value.Month;
			Day = value.Day;
			Hour = (float)value.Hour + (float)value.Minute / 60f + (float)value.Second / 3600f + (float)value.Millisecond / 3600000f;
		}
	}

	public long Ticks
	{
		get
		{
			return DateTime.Ticks;
		}
		set
		{
			DateTime = new DateTime(value, DateTimeKind.Utc);
		}
	}

	public TOD_CycleParameters()
	{
		DateTime = DateTime.UtcNow;
	}

	public TOD_CycleParameters(TOD_CycleParameters from)
	{
		DateTime = from.DateTime;
	}
}
