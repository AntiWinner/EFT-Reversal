using System;
using System.Diagnostics;
using System.Threading;
using Comfort.Common;
using UnityEngine;

namespace UnityDiagnostics;

public class CPULagSimulator : MonoBehaviour
{
	public enum ELoadType
	{
		Calculation,
		ThreadSleep
	}

	[Serializable]
	public class LagSimulator
	{
		public const int MIN_LAG = 0;

		public const int MAX_LAG = 5000;

		private readonly Stopwatch _stopwatch = new Stopwatch();

		public bool Enabled;

		public _E512 TimeMesurer;

		public KeyCode ToggleGameKey;

		[Range(0f, 5000f)]
		public int FramesBetweenSpikes;

		[Range(0f, 5000f)]
		public int LagDispersion;

		[Range(0f, 5000f)]
		public int ConstantLagMS;

		public ELoadType LoadType;

		private int _lastLagFrame;

		public int LastLag { get; set; }

		public void MakeLag()
		{
			if (_E001())
			{
				int num = UnityEngine.Random.Range(-LagDispersion, LagDispersion);
				LastLag = Mathf.Clamp(ConstantLagMS + num, 0, 5000);
				LastLag = Math.Max(0, LastLag - (int)TimeMesurer.CurrentMilliseconds);
				if (LoadType == ELoadType.Calculation)
				{
					_E000(LastLag);
				}
				else
				{
					Thread.Sleep(LastLag);
				}
				_lastLagFrame = Time.frameCount;
			}
		}

		private void _E000(long ms)
		{
			Quaternion rotation = UnityEngine.Random.rotationUniform;
			_stopwatch.Restart();
			while (_stopwatch.ElapsedMilliseconds < ms)
			{
				rotation = Quaternion.Inverse(rotation);
			}
			_stopwatch.Stop();
		}

		private bool _E001()
		{
			if (!Enabled)
			{
				return false;
			}
			return Time.frameCount > _lastLagFrame + FramesBetweenSpikes;
		}

		public void CheckToggleInput()
		{
			if (Input.GetKeyUp(ToggleGameKey))
			{
				Enabled = !Enabled;
			}
		}
	}

	private GUIStyle m__E000;

	public LagSimulator FixedUpdateLagSimulator = new LagSimulator
	{
		ToggleGameKey = KeyCode.Keypad2
	};

	public LagSimulator UpdateLagSimulator = new LagSimulator
	{
		ToggleGameKey = KeyCode.Keypad1
	};

	private void Awake()
	{
		this.m__E000 = new GUIStyle
		{
			fontStyle = FontStyle.Bold,
			fontSize = 14,
			normal = new GUIStyleState
			{
				textColor = Color.yellow
			}
		};
	}

	private void Start()
	{
		if (!Singleton<_E50D>.Instantiated)
		{
			_E2B6.LoadApplicationConfig(new _E3B7());
			Singleton<_E50D>.Create(_E50D.Create(LoggerMode.Add));
		}
		FixedUpdateLagSimulator.TimeMesurer = Singleton<_E50D>.Instance.SingleFixedUpdatesMeasurer;
		UpdateLagSimulator.TimeMesurer = Singleton<_E50D>.Instance.UpdateMeasurer;
	}

	private void Update()
	{
		UpdateLagSimulator.CheckToggleInput();
		FixedUpdateLagSimulator.CheckToggleInput();
		UpdateLagSimulator.MakeLag();
	}

	private void FixedUpdate()
	{
		FixedUpdateLagSimulator.MakeLag();
	}

	private void OnGUI()
	{
		Rect rect = new Rect(Screen.width - 150, 300f, 140f, 32f);
		Rect position = rect;
		position.height += 4f;
		Color color = GUI.color;
		GUI.color = new Color(0.5f, 0.5f, 0.5f, 0.7f);
		GUI.Box(position, "");
		GUI.color = Color.white;
		int num = 16;
		rect.yMin += 2f;
		rect.xMin += 2f;
		_E001(string.Format(_ED3E._E000(129055), UpdateLagSimulator.Enabled ? UpdateLagSimulator.LastLag.ToString() : UpdateLagSimulator.ToggleGameKey.ToString()), Color.white, rect);
		rect.yMin += num;
		rect.yMax += num;
		_E001(string.Format(_ED3E._E000(129039), FixedUpdateLagSimulator.Enabled ? FixedUpdateLagSimulator.LastLag.ToString() : FixedUpdateLagSimulator.ToggleGameKey.ToString()), Color.white, rect);
		rect.yMin += num;
		rect.yMax += num;
		GUI.color = color;
	}

	private bool _E000()
	{
		if (!UpdateLagSimulator.Enabled)
		{
			return FixedUpdateLagSimulator.Enabled;
		}
		return true;
	}

	private void _E001(string info, Color color, Rect rect)
	{
		this.m__E000.normal.textColor = color;
		GUI.Label(rect, info, this.m__E000);
	}

	public static CPULagSimulator Add()
	{
		return _E39A.Add<CPULagSimulator>(dontDestroyOnLoad: true);
	}

	public static void Remove()
	{
		_E39A.Remove<CPULagSimulator>();
	}
}
