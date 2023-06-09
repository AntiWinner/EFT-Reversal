using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Diz.Jobs;

public class JobScheduler : MonoBehaviour
{
	public class _E000
	{
		public long WorkCpuLoad;

		public float WorkCpuLoadPercent;

		public _ECE8 AvgWorkCpuLoad = new _ECE8();

		public float MaxWorkCpuLoadPercent;

		public override string ToString()
		{
			return string.Format(_ED3E._E000(245165), WorkCpuLoad, WorkCpuLoadPercent, AvgWorkCpuLoad.Count, AvgWorkCpuLoad.Average, MaxWorkCpuLoadPercent);
		}
	}

	public static int FreeTimeWindowFrame;

	public static float MaxAvgFrameLoad;

	private static _ECE9 m__E000 = new _ECE9(100);

	public static readonly _E000 WorkingStats = new _E000();

	private static bool m__E001;

	private const int m__E002 = 1024;

	private static List<_ECDD> m__E003 = new List<_ECDD>(1024);

	private static List<_ECDD> m__E004 = new List<_ECDD>(1024);

	private static List<_ECDD> m__E005 = new List<_ECDD>(1024);

	private static List<_ECDD> _E006 = new List<_ECDD>(1024);

	private static List<_ECDD> _E007 = new List<_ECDD>();

	private _ECDF _E008;

	private static _ECE0 _E009 = new _ECE0();

	internal static readonly _ECDC _E00A = new _ECDC();

	private static readonly _ECDB _E00B = new _ECDB();

	internal static _ECDE _E00C = new _ECDE();

	private static readonly WaitForEndOfFrame _E00D = new WaitForEndOfFrame();

	private readonly Stopwatch _E00E = new Stopwatch();

	private readonly Stopwatch _E00F = new Stopwatch();

	private bool _E010;

	public float DefaultForceModeMultiplier = 3f;

	private float _E011 = 3f;

	[CompilerGenerated]
	private long _E012 = 160000L;

	[CompilerGenerated]
	private byte _E013 = 6;

	private int _E014;

	private int _E015;

	public static int AvgTimeBufferSize
	{
		get
		{
			return JobScheduler.m__E000?.BufferSize ?? 0;
		}
		set
		{
			JobScheduler.m__E000 = new _ECE9(value);
		}
	}

	public long FrameTicks
	{
		[CompilerGenerated]
		get
		{
			return _E012;
		}
		[CompilerGenerated]
		set
		{
			_E012 = value;
		}
	}

	public long LoopTicks => FrameTicks / 2;

	public byte SlowFrames
	{
		[CompilerGenerated]
		get
		{
			return _E013;
		}
		[CompilerGenerated]
		set
		{
			_E013 = value;
		}
	}

	private bool _E016
	{
		get
		{
			if (_E00E.ElapsedTicks >= FrameTicks && _E014 <= SlowFrames)
			{
				return _E010;
			}
			return true;
		}
	}

	private static bool _E017
	{
		get
		{
			if (JobScheduler.m__E003.Count <= 0 && JobScheduler.m__E005.Count <= 0)
			{
				return _E007.Count > 0;
			}
			return true;
		}
	}

	private bool _E018 => (float)_E00F.ElapsedTicks <= (float)LoopTicks * (_E010 ? _E011 : 1f);

	public static int QueueLength => JobScheduler.m__E003.Count + JobScheduler.m__E005.Count;

	public bool IsForceModeEnabled => _E010;

	public void Init(bool enableContinuationProfiler)
	{
		_E00C.Enabled = enableContinuationProfiler;
		_E008 = new _ECDF((_ECDD continuation) => _E016 && !continuation.wasExecuted);
	}

	public void SetForceMode(bool enable, float forceModeMultiplier = -1f)
	{
		if (forceModeMultiplier < 0f)
		{
			forceModeMultiplier = DefaultForceModeMultiplier;
		}
		UnityEngine.Debug.Log(string.Format(_ED3E._E000(244986), _ED3E._E000(244971), enable, _ED3E._E000(245016), forceModeMultiplier));
		_E010 = enable;
		_E011 = forceModeMultiplier;
	}

	public void SetTargetFrameRate(int frameRate)
	{
		int num = 1000 / frameRate;
		FrameTicks = TimeSpan.FromMilliseconds(num).Ticks;
		UnityEngine.Debug.Log(_ED3E._E000(244996) + FrameTicks + _ED3E._E000(245048) + frameRate);
	}

	private IEnumerator Start()
	{
		_E00E.Start();
		while (true)
		{
			yield return _E00D;
			_E00E.Restart();
		}
	}

	private void LateUpdate()
	{
		long elapsedTicks = _E00E.ElapsedTicks;
		JobScheduler.m__E000.AddValue(elapsedTicks);
		_E000();
		_E00E.Restart();
	}

	private void _E000()
	{
		if (!_E017)
		{
			return;
		}
		_E00F.Restart();
		_E015 = QueueLength;
		_E014++;
		while (_E016 && _E017)
		{
			_E001(_E008);
			if (_E014 > SlowFrames)
			{
				_E014 = (int)SlowFrames / 2;
			}
			if (!_E018)
			{
				break;
			}
		}
		if (_E015 <= 0)
		{
			_E014 = 0;
		}
	}

	private void _E001(_ECE6 executeCondition)
	{
		if (JobScheduler.m__E001)
		{
			UnityEngine.Debug.LogError(_ED3E._E000(245036));
			return;
		}
		JobScheduler.m__E001 = true;
		_E002(ref JobScheduler.m__E005, ref _E006, _E006, executeCondition);
		_E002(ref JobScheduler.m__E003, ref JobScheduler.m__E004, JobScheduler.m__E005, executeCondition);
		JobScheduler.m__E001 = false;
	}

	private void _E002(ref List<_ECDD> continuations, ref List<_ECDD> continuationsDoubleBuffer, List<_ECDD> delayedContinuations, _ECE6 executeCondition)
	{
		if (continuations.Count == 0)
		{
			return;
		}
		_E007 = continuations;
		continuations = continuationsDoubleBuffer;
		continuationsDoubleBuffer = _E007;
		for (int i = 0; i < _E007.Count; i++)
		{
			_ECDD continuation = _E007[i];
			if (executeCondition.CanExecute(continuation) && _E018)
			{
				_E015++;
				executeCondition.Execute(ref continuation);
				_E007[i] = continuation;
			}
			else if (!continuation.wasExecuted)
			{
				delayedContinuations.Add(continuation);
			}
		}
		_E007.Clear();
	}

	public static bool ForceExecuteContinuations(_ECE7 jobYield)
	{
		_E009.Init(jobYield);
		while (jobYield.IsInProgress && _E017)
		{
			_E009.HasExecution = false;
			_E003(_E007);
			_E003(JobScheduler.m__E005);
			_E003(JobScheduler.m__E003);
			if (!_E009.HasExecution)
			{
				UnityEngine.Debug.LogErrorFormat(_ED3E._E000(245104), jobYield.Name);
				return false;
			}
		}
		return true;
	}

	private static void _E003(List<_ECDD> continuations)
	{
		for (int i = 0; i < continuations.Count; i++)
		{
			_ECDD continuation = continuations[i];
			if (_E009.CanExecute(continuation))
			{
				_E009.Execute(ref continuation);
				continuations[i] = continuation;
			}
		}
	}

	internal static void _E004(Action action, _ECE7 jobYield)
	{
		JobScheduler.m__E003.Add(new _ECDD(action, jobYield));
	}

	public static _ECE4 Yield(EJobPriority priority = EJobPriority.General, _ECE7 jobYield = null)
	{
		switch (priority)
		{
		case EJobPriority.Low:
		case EJobPriority.General:
			return _E00B.Init(jobYield, runSynchronously: false);
		case EJobPriority.Immediate:
			return _E00B.Init(jobYield, runSynchronously: true);
		default:
			throw new NotImplementedException();
		}
	}

	public static string GetPerformanceReport()
	{
		return string.Format(_ED3E._E000(245143), WorkingStats, _E00C.GetReport());
	}

	[CompilerGenerated]
	private bool _E005(_ECDD continuation)
	{
		if (_E016)
		{
			return !continuation.wasExecuted;
		}
		return false;
	}
}
