using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Comfort.Common;
using UnityEngine;

public class TOD_Time : MonoBehaviour
{
	public bool LockCurrentTime;

	public _E629 GameDateTime;

	[_E058(1f)]
	[Tooltip("Length of one day in minutes.")]
	public float DayLengthInMinutes = 30f;

	[Tooltip("Set the date to the current device date on start.")]
	public bool UseDeviceDate;

	[Tooltip("Set the time to the current device time on start.")]
	public bool UseDeviceTime;

	[Tooltip("Apply the time curve when progressing time.")]
	public bool UseTimeCurve;

	[Tooltip("Time progression curve.")]
	public AnimationCurve TimeCurve = AnimationCurve.Linear(0f, 0f, 24f, 24f);

	[CompilerGenerated]
	private Action m__E000;

	[CompilerGenerated]
	private Action m__E001;

	[CompilerGenerated]
	private Action m__E002;

	[CompilerGenerated]
	private Action _E003;

	[CompilerGenerated]
	private Action _E004;

	private TOD_Sky _E005;

	private AnimationCurve _E006;

	private AnimationCurve _E007;

	private DateTime _E008;

	private const string _E009 = "d";

	public event Action OnMinute
	{
		[CompilerGenerated]
		add
		{
			Action action = this.m__E000;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = this.m__E000;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action OnHour
	{
		[CompilerGenerated]
		add
		{
			Action action = this.m__E001;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E001, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = this.m__E001;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E001, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action OnDay
	{
		[CompilerGenerated]
		add
		{
			Action action = this.m__E002;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E002, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = this.m__E002;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E002, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action OnMonth
	{
		[CompilerGenerated]
		add
		{
			Action action = _E003;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E003, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E003;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E003, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action OnYear
	{
		[CompilerGenerated]
		add
		{
			Action action = _E004;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E004, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E004;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E004, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public void RefreshTimeCurve()
	{
		TimeCurve.preWrapMode = WrapMode.Once;
		TimeCurve.postWrapMode = WrapMode.Once;
		_E001(TimeCurve, out _E006, out _E007);
		_E006.preWrapMode = WrapMode.Loop;
		_E006.postWrapMode = WrapMode.Loop;
		_E007.preWrapMode = WrapMode.Loop;
		_E007.postWrapMode = WrapMode.Loop;
	}

	public float ApplyTimeCurve(float deltaTime)
	{
		float num = _E007.Evaluate(_E005.Cycle.Hour) + deltaTime;
		deltaTime = _E006.Evaluate(num) - _E005.Cycle.Hour;
		if (num >= 24f)
		{
			deltaTime += (float)((int)num / 24 * 24);
		}
		else if (num < 0f)
		{
			deltaTime += (float)(((int)num / 24 - 1) * 24);
		}
		return deltaTime;
	}

	public void AddHours(float hours, bool adjust = true)
	{
		if (GameDateTime == null && UseTimeCurve && adjust)
		{
			hours = ApplyTimeCurve(hours);
		}
		DateTime dateTime = _E005.Cycle.DateTime;
		DateTime dateTime2 = ((GameDateTime != null) ? GameDateTime.CalculateTaxonomyDate(_E008) : dateTime.AddHours(hours));
		if (dateTime2.Year > dateTime.Year)
		{
			if (_E004 != null)
			{
				_E004();
			}
			if (_E003 != null)
			{
				_E003();
			}
			if (this.m__E002 != null)
			{
				this.m__E002();
			}
			if (this.m__E001 != null)
			{
				this.m__E001();
			}
			if (this.m__E000 != null)
			{
				this.m__E000();
			}
		}
		else if (dateTime2.Month > dateTime.Month)
		{
			if (_E003 != null)
			{
				_E003();
			}
			if (this.m__E002 != null)
			{
				this.m__E002();
			}
			if (this.m__E001 != null)
			{
				this.m__E001();
			}
			if (this.m__E000 != null)
			{
				this.m__E000();
			}
		}
		else if (dateTime2.Day > dateTime.Day)
		{
			if (this.m__E002 != null)
			{
				this.m__E002();
			}
			if (this.m__E001 != null)
			{
				this.m__E001();
			}
			if (this.m__E000 != null)
			{
				this.m__E000();
			}
		}
		else if (dateTime2.Hour > dateTime.Hour)
		{
			if (this.m__E001 != null)
			{
				this.m__E001();
			}
			if (this.m__E000 != null)
			{
				this.m__E000();
			}
		}
		else if (dateTime2.Minute > dateTime.Minute && this.m__E000 != null)
		{
			this.m__E000();
		}
		_E005.Cycle.DateTime = dateTime2;
	}

	public void AddSeconds(float seconds)
	{
		AddHours(seconds / 3600f);
	}

	private void _E000(Keyframe[] keys)
	{
		for (int i = 0; i < keys.Length; i++)
		{
			Keyframe keyframe = keys[i];
			if (i > 0)
			{
				Keyframe keyframe2 = keys[i - 1];
				keyframe.inTangent = (keyframe.value - keyframe2.value) / (keyframe.time - keyframe2.time);
			}
			if (i < keys.Length - 1)
			{
				Keyframe keyframe3 = keys[i + 1];
				keyframe.outTangent = (keyframe3.value - keyframe.value) / (keyframe3.time - keyframe.time);
			}
			keys[i] = keyframe;
		}
	}

	private void _E001(AnimationCurve source, out AnimationCurve approxCurve, out AnimationCurve approxInverse)
	{
		Keyframe[] array = new Keyframe[25];
		Keyframe[] array2 = new Keyframe[25];
		float num = -0.01f;
		for (int i = 0; i < 25; i++)
		{
			num = Mathf.Max(num + 0.01f, source.Evaluate(i));
			array[i] = new Keyframe(i, num);
			array2[i] = new Keyframe(num, i);
		}
		_E000(array);
		_E000(array2);
		approxCurve = new AnimationCurve(array);
		approxInverse = new AnimationCurve(array2);
	}

	private DateTime _E002()
	{
		try
		{
			return DateTime.Parse(DateTime.Parse(Singleton<_E5CB>.Instance.TODSkyDate).ToString(_ED3E._E000(18705)));
		}
		catch (Exception)
		{
			return _E5AD.Now;
		}
	}

	protected void Awake()
	{
		_E005 = GetComponent<TOD_Sky>();
		_E008 = _E002();
		DateTime now = _E5AD.Now;
		if (UseDeviceDate)
		{
			_E005.Cycle.Year = now.Year;
			_E005.Cycle.Month = now.Month;
			_E005.Cycle.Day = now.Day;
		}
		if (UseDeviceTime)
		{
			_E005.Cycle.Hour = (float)now.TimeOfDay.TotalHours;
		}
		RefreshTimeCurve();
	}

	protected void FixedUpdate()
	{
		float num = 1440f / DayLengthInMinutes;
		if (!LockCurrentTime)
		{
			AddSeconds(Time.deltaTime * num);
		}
	}
}
