using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Comfort.Common;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class TMP_FPSCounter : MonoBehaviour
{
	private sealed class _E000
	{
		public readonly float Width;

		private readonly MonoBehaviour m__E000;

		private readonly Func<IEnumerator> _E001;

		private Coroutine _E002;

		public _E000(MonoBehaviour parent, Func<IEnumerator> routineGenerator, float width)
		{
			m__E000 = parent;
			_E001 = routineGenerator;
			Width = width;
		}

		public void Start()
		{
			Stop();
			_E002 = m__E000.StartCoroutine(_E001());
		}

		public void Stop()
		{
			if (_E002 != null)
			{
				m__E000.StopCoroutine(_E002);
				_E002 = null;
			}
		}
	}

	[SerializeField]
	private LayoutElement _layoutElement;

	[SerializeField]
	private TextMeshProUGUI _limitFpsLabel;

	[SerializeField]
	private TextMeshProUGUI _fpsCounterLabel;

	[SerializeField]
	private TextMeshProUGUI _rttTimeCounterLabel;

	[SerializeField]
	private TextMeshProUGUI _serverFixedUpdateCounterLabel;

	[SerializeField]
	private TextMeshProUGUI _serverTimeDiffCounterLabel;

	[SerializeField]
	private GameObject _extendedLabelsPanel;

	[SerializeField]
	private TextMeshProUGUI _averageFpsLabel;

	[SerializeField]
	private TextMeshProUGUI _fixedUpdateFpsLabel;

	[SerializeField]
	private TextMeshProUGUI _averageFixedUpdateFpsLabel;

	[SerializeField]
	private TextMeshProUGUI _singleFixedUpdateFpsLabel;

	[SerializeField]
	private TextMeshProUGUI _fixedUpdateBetweenUpdatesLabel;

	[SerializeField]
	private TextMeshProUGUI _updateMeasurerLabel;

	[SerializeField]
	private TextMeshProUGUI _gameUpdateLabel;

	[SerializeField]
	private TextMeshProUGUI _renderMsLabel;

	[SerializeField]
	private TextMeshProUGUI _frameMsLabel;

	[SerializeField]
	private TextMeshProUGUI _distantShadowObjectsLabel;

	[SerializeField]
	private TextMeshProUGUI LocalVRamTotal;

	[SerializeField]
	private TextMeshProUGUI LocalVRamUsage;

	[SerializeField]
	private TextMeshProUGUI MaxLocalVRamUsage;

	[SerializeField]
	private TextMeshProUGUI TotalLocalVRam;

	[SerializeField]
	private TextMeshProUGUI DiskUsage;

	[SerializeField]
	private TextMeshProUGUI DiskRead;

	[SerializeField]
	private TextMeshProUGUI DiskWrite;

	[SerializeField]
	private TextMeshProUGUI VirtualMemoryBytes;

	[SerializeField]
	private TextMeshProUGUI GpuUtilization;

	[SerializeField]
	private TextMeshProUGUI _networkRttLabel;

	[SerializeField]
	private TextMeshProUGUI _networkLossLabel;

	[SerializeField]
	private TextMeshProUGUI _bufferCountLabel;

	[SerializeField]
	private TextMeshProUGUI _cameraPositionLabel;

	[SerializeField]
	private TextMeshProUGUI _desiredTextureMemory;

	[SerializeField]
	private TextMeshProUGUI _totalMemoryReserved;

	[SerializeField]
	private GameObject _webSocketLabelsPanel;

	[SerializeField]
	private TextMeshProUGUI _webSocketConnStatus;

	[SerializeField]
	private TextMeshProUGUI _webSocketCloseStatusCode;

	[SerializeField]
	private TextMeshProUGUI _webSocketConnIsAlive;

	[SerializeField]
	private TextMeshProUGUI _webSocketRequestsInProgressCount;

	private const int m__E000 = 4;

	private const int m__E001 = 1;

	private const float m__E002 = 0.1f;

	private const string m__E003 = "(inf)";

	private const string _E004 = "0.00";

	[CompilerGenerated]
	private bool _E005;

	public bool ShowGraphs;

	private readonly List<double> _E006 = new List<double>();

	private _E50D _E007;

	private _E50E _E008;

	private _E50E _E009;

	private _E50E _E00A;

	private _E50E _E00B;

	private _E50E _E00C;

	private _E50E _E00D;

	private _E50E _E00E;

	private _E50E _E00F;

	private _E50E _E010;

	private _E50E _E011;

	private _E50E _E012;

	private _E50E _E013;

	private _E50E _E014;

	private _E50E _E015;

	private bool _E016;

	private float _E017;

	private const float _E018 = 2f;

	private _E50A _E019;

	private _E50A _E01A;

	private _E50A _E01B;

	private _E50A _E01C;

	private _E50A _E01D;

	private _E50A _E01E;

	private Mutex _E01F = new Mutex();

	private _E000[] _E020;

	public bool Active
	{
		[CompilerGenerated]
		get
		{
			return this._E005;
		}
		[CompilerGenerated]
		private set
		{
			this._E005 = value;
		}
	}

	private void Awake()
	{
		_E020 = new _E000[4]
		{
			new _E000(this, _E000, 100f),
			new _E000(this, _E001, 100f),
			new _E000(this, _E002, 170f),
			new _E000(this, _E003, 170f)
		};
		if (!Singleton<_E50D>.Instantiated)
		{
			Singleton<_E50D>.Create(_E50D.Create(LoggerMode.Add));
		}
		_E007 = Singleton<_E50D>.Instance;
		_E009 = _E007.FixedUpdatesBetweenUpdateMeasurer.MeasureStatistics;
		_E008 = _E007.SingleFixedUpdatesMeasurer.MeasureStatistics;
		_E00A = _E007.UpdateMeasurer.MeasureStatistics;
		_E00D = _E007.GameUpdateMeasurer.MeasureStatistics;
		_E00B = _E007.RenderMeasurer.MeasureStatistics;
		_E00C = _E007.GameFrameMeasurer.MeasureStatistics;
		_E019 = _E50B.CreateAvgMeasurer(_ED3E._E000(252955), 200);
		_E01A = _E50B.CreateAvgMeasurer(_ED3E._E000(252951), 200);
		_E01B = _E50B.CreateAvgMeasurer(_ED3E._E000(252951), 200);
		_E01C = _E50B.CreateAvgMeasurer(_ED3E._E000(252951), 200);
		_E01D = _E50B.CreateAvgMeasurer(_ED3E._E000(252940), 200);
		_E01E = _E50B.CreateAvgMeasurer(_ED3E._E000(252940), 200);
		_E010 = _E019.MeasureStatistics;
		_E011 = _E01A.MeasureStatistics;
		_E012 = _E01B.MeasureStatistics;
		_E013 = _E01C.MeasureStatistics;
		_E014 = _E01D.MeasureStatistics;
		_E015 = _E01E.MeasureStatistics;
		_E00E = _E007.UpdateCountMeasurer.MeasureStatistics;
		_E00F = _E007.FixedUpdateCountMeasurer.MeasureStatistics;
	}

	public void Show(int level)
	{
		Active = true;
		base.gameObject.SetActive(value: true);
		_E000[] array = _E020;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Stop();
		}
		level = Mathf.Clamp(level, 0, _E020.Length);
		if (level >= 1)
		{
			float num = 0f;
			for (int j = 0; j < level; j++)
			{
				_E020[j].Start();
				num = Mathf.Max(num, _E020[j].Width);
			}
			_layoutElement.minWidth = num;
		}
	}

	private IEnumerator _E000()
	{
		WaitForSeconds waitForSeconds = new WaitForSeconds(1f);
		_networkRttLabel.gameObject.SetActive(value: false);
		_networkLossLabel.gameObject.SetActive(value: false);
		_rttTimeCounterLabel.gameObject.SetActive(value: false);
		_extendedLabelsPanel.gameObject.SetActive(value: false);
		_webSocketLabelsPanel.gameObject.SetActive(value: false);
		_serverFixedUpdateCounterLabel.gameObject.SetActive(value: false);
		_serverTimeDiffCounterLabel.gameObject.SetActive(value: false);
		_cameraPositionLabel.gameObject.SetActive(value: false);
		_bufferCountLabel.gameObject.SetActive(value: false);
		while (true)
		{
			int targetFrameRate = Application.targetFrameRate;
			string text = ((targetFrameRate > 0) ? targetFrameRate.ToString() : _ED3E._E000(252939));
			_limitFpsLabel.text = _ED3E._E000(252929) + text;
			double lastValue = _E00E.LastValue;
			_fpsCounterLabel.text = _ED3E._E000(40298) + lastValue;
			_fpsCounterLabel.color = lastValue.GetColor(60.0, 10.0);
			yield return waitForSeconds;
		}
	}

	private IEnumerator _E001()
	{
		WaitForSeconds waitForSeconds = new WaitForSeconds(1f);
		_networkRttLabel.gameObject.SetActive(value: true);
		_networkLossLabel.gameObject.SetActive(value: true);
		_rttTimeCounterLabel.gameObject.SetActive(value: true);
		while (true)
		{
			if (Singleton<_E7DE>.Instance.Game.Controller.IsStreamerModeActive)
			{
				_networkRttLabel.text = _ED3E._E000(252985);
				_networkLossLabel.text = _ED3E._E000(252978);
				_rttTimeCounterLabel.text = _ED3E._E000(252975);
			}
			else
			{
				double rTT = NetworkGameSession.RTT;
				_networkRttLabel.text = _ED3E._E000(252967) + rTT;
				_networkRttLabel.color = _E38D._E000.ToRTTColor(rTT);
				int lossPercent = NetworkGameSession.LossPercent;
				_networkLossLabel.text = _ED3E._E000(253021) + lossPercent + _ED3E._E000(149464);
				_networkLossLabel.color = _E38D._E000.ToLossColor(lossPercent);
				_rttTimeCounterLabel.text = _ED3E._E000(253012) + _E007.PlayerRTT;
				_rttTimeCounterLabel.color = _E38D._E000.ToPlayerRTTColor(rTT);
			}
			yield return waitForSeconds;
		}
	}

	private IEnumerator _E002()
	{
		WaitForSeconds waitForSeconds = new WaitForSeconds(1f);
		_extendedLabelsPanel.gameObject.SetActive(value: true);
		while (true)
		{
			double average = _E00E.Average;
			_averageFpsLabel.text = _ED3E._E000(253000) + average.ToString(_ED3E._E000(29354));
			_averageFpsLabel.color = average.GetColor(60.0, 10.0);
			_fixedUpdateFpsLabel.text = _ED3E._E000(253054) + _E00F.LastValue.ToString(_ED3E._E000(29354));
			_averageFixedUpdateFpsLabel.text = _ED3E._E000(253046) + _E00F.Average.ToString(_ED3E._E000(29354));
			float num = Time.fixedDeltaTime * 1000f;
			double average2 = _E008.Average;
			_singleFixedUpdateFpsLabel.text = _ED3E._E000(253030) + average2.ToString(_ED3E._E000(29354));
			_singleFixedUpdateFpsLabel.color = average2.GetColor(num * 0.3f, num);
			double average3 = _E009.Average;
			_fixedUpdateBetweenUpdatesLabel.text = _ED3E._E000(253077) + average3.ToString(_ED3E._E000(29354));
			_fixedUpdateBetweenUpdatesLabel.color = average3.GetColor(num * 0.3f, num);
			double average4 = _E00A.Average;
			_updateMeasurerLabel.text = _ED3E._E000(253068) + average4.ToString(_ED3E._E000(29354));
			_updateMeasurerLabel.color = average4.GetColor(num * 0.3f, num);
			double average5 = _E00D.Average;
			_gameUpdateLabel.text = _ED3E._E000(253061) + average5.ToString(_ED3E._E000(29354));
			_gameUpdateLabel.color = average5.GetColor(num * 0.6f, num);
			double average6 = _E00B.Average;
			_renderMsLabel.text = _ED3E._E000(253114) + average6.ToString(_ED3E._E000(29354));
			_renderMsLabel.color = average6.GetColor(num * 0.6f, num);
			double average7 = _E00C.Average;
			_frameMsLabel.text = _ED3E._E000(253103) + average7.ToString(_ED3E._E000(29354));
			_frameMsLabel.color = average7.GetColor(num * 1.3f, 5f * num);
			_distantShadowObjectsLabel.text = "";
			_distantShadowObjectsLabel.color = Color.green;
			bool num2 = _E8A8.Instance.Camera != null;
			string text = _ED3E._E000(225036);
			string text2 = _ED3E._E000(225036);
			string text3 = _ED3E._E000(225036);
			string text4 = _ED3E._E000(225036);
			if (num2)
			{
				_E8A8.Instance.GetVRamUsage(out var totalVRam, out var localBudget, out var localCurrentUsage);
				text = (localBudget / 1048576uL).ToString();
				text2 = (localCurrentUsage / 1048576uL).ToString();
				text4 = (totalVRam / 1048576uL).ToString();
				text3 = (VRamUsageWrapper.maxLocalUsage / 1048576uL).ToString();
			}
			LocalVRamTotal.text = _ED3E._E000(253091) + text;
			LocalVRamUsage.text = _ED3E._E000(253137) + text2;
			MaxLocalVRamUsage.text = _ED3E._E000(253126) + text3;
			TotalLocalVRam.text = _ED3E._E000(253175) + text4;
			if (PerfHelperWrapper.IsRunning)
			{
				float num3 = PerfHelperWrapper.DiskIdlePercent;
				float num4 = PerfHelperWrapper.DiskWriteBytesPerSecond;
				float num5 = PerfHelperWrapper.DiskReadBytesPerSecond;
				float num6 = (float)PerfHelperWrapper.ApplicationPrivateBytes / 1024f / 1024f;
				float num7 = PerfHelperWrapper.GPUUtilization;
				if (_E016)
				{
					num3 = (float)_E011.Average;
					num4 = (float)_E013.Average;
					num5 = (float)_E012.Average;
					num6 = (float)(_E014.Average / 1024.0 / 1024.0);
					num7 = (float)_E010.Average;
				}
				DiskUsage.text = _ED3E._E000(253156) + num3.ToString(_ED3E._E000(29354)) + _ED3E._E000(149464);
				GpuUtilization.text = _ED3E._E000(253208) + num7.ToString(_ED3E._E000(29354)) + _ED3E._E000(149464);
				float num8 = num5 / 1024f;
				float num9 = num4 / 1024f;
				DiskRead.text = _ED3E._E000(253196) + (_E016 ? _ED3E._E000(253190) : _ED3E._E000(12201)) + num8.ToString(_ED3E._E000(32702)) + _ED3E._E000(253245);
				DiskWrite.text = _ED3E._E000(253236) + (_E016 ? _ED3E._E000(253190) : _ED3E._E000(12201)) + num9.ToString(_ED3E._E000(32702)) + _ED3E._E000(253245);
				float num10 = num6 / (float)SystemInfo.systemMemorySize * 100f;
				VirtualMemoryBytes.text = _ED3E._E000(253231) + num6.ToString(_ED3E._E000(32702)) + _ED3E._E000(253266) + SystemInfo.systemMemorySize + _ED3E._E000(253257) + num10.ToString(_ED3E._E000(48633)) + _ED3E._E000(253252);
			}
			else
			{
				DiskUsage.text = _ED3E._E000(253255);
				DiskRead.text = _ED3E._E000(253302);
				DiskWrite.text = _ED3E._E000(253285);
				VirtualMemoryBytes.text = _ED3E._E000(253333);
				GpuUtilization.text = _ED3E._E000(253326);
			}
			if (_E016)
			{
				_desiredTextureMemory.text = _ED3E._E000(253373) + (int)(_E015.Average / 1024.0 / 1024.0) + _ED3E._E000(253359);
			}
			else
			{
				_desiredTextureMemory.text = _ED3E._E000(253373) + (int)(Texture.desiredTextureMemory / 1024uL / 1024uL) + _ED3E._E000(253359);
			}
			_totalMemoryReserved.text = string.Format(_ED3E._E000(253354), Profiler.GetTotalReservedMemoryLong() / 1024 / 1024);
			yield return waitForSeconds;
		}
	}

	private IEnumerator _E003()
	{
		WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);
		while (true)
		{
			yield return waitForSeconds;
		}
	}

	private void LateUpdate()
	{
		if (_E016)
		{
			_E01F.WaitOne();
			_E01A.AddValue(PerfHelperWrapper.DiskIdlePercent);
			_E01B.AddValue(PerfHelperWrapper.DiskReadBytesPerSecond);
			_E01C.AddValue(PerfHelperWrapper.DiskWriteBytesPerSecond);
			_E01D.AddValue(PerfHelperWrapper.ApplicationPrivateBytes);
			_E01E.AddValue(Texture.desiredTextureMemory);
			_E019.AddValue(PerfHelperWrapper.GPUUtilization);
			_E01A.Commit();
			_E01B.Commit();
			_E01C.Commit();
			_E01D.Commit();
			_E01E.Commit();
			_E019.Commit();
			_E017 += Time.deltaTime;
			if (_E017 > 2f)
			{
				_E016 = false;
			}
			_E01F.ReleaseMutex();
		}
		if (ShowGraphs && GraphManager.Graph != null)
		{
			bool mark = false;
			Rect rectangle = new Rect(0f, 0f, 0.75f * (float)Screen.width, 0.2f * (float)Screen.height);
			GraphManager.Graph.Plot(_ED3E._E000(218820), (float)Singleton<_E50D>.Instance.GetCurrentFrameTime(), Color.green, mark, Color.red, rectangle);
		}
	}

	public void GetPerformanceState(out double fpsMeasure, out double gameUpdate, out double renderMeasurer, out double frameMeasurer, out double diskIdleMeasurer, out double diskReadBytesMeasurer, out double diskWriteBytesMeasurer, out double localVRAMUsageMBytes, out double privateMemMBytes, out double desiredTextureMemoryMBytes, out double totalReservedMemory, out double gpuUtilization)
	{
		fpsMeasure = _E00E.LastValue;
		gameUpdate = _E00D.Average;
		renderMeasurer = _E00B.Average;
		frameMeasurer = _E00C.Average;
		diskIdleMeasurer = _E011.Average;
		diskReadBytesMeasurer = _E012.Average;
		diskWriteBytesMeasurer = _E013.Average;
		privateMemMBytes = _E014.Average / 1024.0 / 1024.0;
		gpuUtilization = _E010.Average;
		if (_E8A8.Instance.Camera != null)
		{
			_E8A8.Instance.GetVRamUsage(out var _, out var _, out var localCurrentUsage);
			localVRAMUsageMBytes = (double)localCurrentUsage / 1048576.0;
		}
		else
		{
			localVRAMUsageMBytes = 0.0;
		}
		if (!_E016)
		{
			_E016 = true;
		}
		_E017 = 0f;
		desiredTextureMemoryMBytes = _E015.Average / 1024.0 / 1024.0;
		totalReservedMemory = Profiler.GetTotalReservedMemoryLong() / 1024 / 1024;
	}

	public IEnumerator PerformanceTest(int overall, int every, string label = "label", Action onTestComplete = null)
	{
		_E016 = true;
		_E006.Clear();
		float num = Time.time + (float)overall;
		yield return new WaitForEndOfFrame();
		WaitForSeconds waitForSeconds = new WaitForSeconds(every);
		while (Time.time < num)
		{
			GetPerformanceState(out var fpsMeasure, out var gameUpdate, out var renderMeasurer, out var frameMeasurer, out var diskIdleMeasurer, out var diskReadBytesMeasurer, out var diskWriteBytesMeasurer, out var localVRAMUsageMBytes, out var privateMemMBytes, out var desiredTextureMemoryMBytes, out var totalReservedMemory, out var gpuUtilization);
			_E006.Add(fpsMeasure);
			_E006.Add(gameUpdate);
			_E006.Add(renderMeasurer);
			_E006.Add(frameMeasurer);
			_E006.Add(diskIdleMeasurer);
			_E006.Add(diskReadBytesMeasurer);
			_E006.Add(diskWriteBytesMeasurer);
			_E006.Add(localVRAMUsageMBytes);
			_E006.Add(privateMemMBytes);
			_E006.Add(desiredTextureMemoryMBytes);
			_E006.Add(totalReservedMemory);
			_E006.Add(gpuUtilization);
			yield return waitForSeconds;
		}
		string text = string.Empty;
		for (int i = 0; i < _E006.Count; i++)
		{
			text += _E006[i];
			if (i < _E006.Count - 1)
			{
				text += (((i + 1) % 4 == 0) ? _ED3E._E000(2540) : _ED3E._E000(97232));
			}
		}
		_E36F.NLog(_ED3E._E000(253389), string.Format(_ED3E._E000(253382), label, every, text));
		onTestComplete?.Invoke();
	}

	public void Hide()
	{
		Active = false;
		base.gameObject.SetActive(value: false);
	}
}
