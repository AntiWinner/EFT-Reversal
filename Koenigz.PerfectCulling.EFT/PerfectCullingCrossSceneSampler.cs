using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Koenigz.PerfectCulling.EFT;

public sealed class PerfectCullingCrossSceneSampler : MonoBehaviour
{
	private sealed class _E000
	{
		public PerfectCullingCamera camera;

		public int threadId;

		public int numThreads;
	}

	private sealed class _E001
	{
		public HashSet<int> VolumeIds = new HashSet<int>();

		public HashSet<int> GroupIds = new HashSet<int>();

		public ManualResetEventSlim WorkThreadWaitHandle = new ManualResetEventSlim(initialState: false);

		public object LockUpdate = new object();

		public long WorkTime;

		public void Clear()
		{
			VolumeIds.Clear();
			GroupIds.Clear();
		}
	}

	private static class _E002
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct _E000 : IJobParallelFor
		{
			public void Execute(int index)
			{
				if (!TerminateToken.IsCancellationRequested)
				{
					_E009(TerminateToken.Token, JobParams.camera.ObservePosition, VolumeIds[index], PerfectCullingCrossSceneSampler._E002.m__E001[index], PerfectCullingCrossSceneSampler._E002.m__E000[index]);
				}
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct _E001 : IJobParallelFor
		{
			public void Execute(int index)
			{
				if (!TerminateToken.IsCancellationRequested)
				{
					_E00A(TerminateToken.Token, JobParams.camera, GroupIds[index]);
				}
			}
		}

		public static PerfectCullingCrossSceneSampler._E000 JobParams;

		public static CancellationTokenSource TerminateToken;

		public static readonly HashSet<int>[] VolumeIds = new HashSet<int>[2]
		{
			new HashSet<int>(),
			new HashSet<int>()
		};

		public static readonly HashSet<int>[] GroupIds = new HashSet<int>[2]
		{
			new HashSet<int>(),
			new HashSet<int>()
		};

		private static readonly List<int>[] m__E000 = new List<int>[2]
		{
			new List<int>(),
			new List<int>()
		};

		private static readonly List<int>[] m__E001 = new List<int>[2]
		{
			new List<int>(),
			new List<int>()
		};
	}

	private sealed class _E003
	{
		public PerfectCullingAdaptiveGrid grid;
	}

	private sealed class _E004
	{
		public bool IsActive;

		public Vector3 Position;

		public float FieldOfView;

		public Func<Camera> CameraGetter;

		public _E004(Func<Camera> cam)
		{
			CameraGetter = cam;
			Update();
		}

		public void Update()
		{
			Camera camera = CameraGetter();
			if (camera != null)
			{
				IsActive = camera.gameObject.activeInHierarchy;
				Position = camera.transform.position;
				FieldOfView = camera.fieldOfView;
			}
		}
	}

	private struct _E005 : IJob
	{
		[CompilerGenerated]
		private sealed class _E000
		{
			public int j;

			public Predicate<CullingGroupData> _003C_003E9__0;

			internal bool _E000(CullingGroupData x)
			{
				return x.RuntimeCullingGroupIdx == j;
			}
		}

		public Vector3 ObservePosition;

		public void Execute()
		{
			try
			{
				PerfectCullingAdaptiveGrid instance = PerfectCullingAdaptiveGrid.Instance;
				if (instance == null || Instance == null || !Instance.m__E006 || disableWorkThread)
				{
					return;
				}
				_E00C();
				CullingCellData cullingCellData = instance.RuntimeBVH.QueryNearest(ObservePosition, PerfectCullingCrossSceneSampler._E00F);
				if (PerfectCullingCrossSceneSampler.m__E012 == cullingCellData)
				{
					return;
				}
				CullingCellData cullingCellData2 = _E00B(cullingCellData);
				if (cullingCellData2 == null)
				{
					return;
				}
				Instance.m__E00D = cullingCellData2.RuntimeCellId;
				Instance.m__E00E = cullingCellData2.SpatialItemBounds;
				int i = 0;
				foreach (PerfectCullingCrossSceneGroup item in instance.RuntimeGroupMapping)
				{
					item._E00B(cullingCellData2.CullingData.Find((CullingGroupData x) => x.RuntimeCullingGroupIdx == i));
					i++;
				}
			}
			catch (Exception ex)
			{
				_E4BB.Throw(ex.Message + _ED3E._E000(29351) + ex.StackTrace);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E007
	{
		public int j;

		public Predicate<CullingGroupData> _003C_003E9__0;

		internal bool _E000(CullingGroupData x)
		{
			return x.RuntimeCullingGroupIdx == j;
		}
	}

	[CompilerGenerated]
	private static PerfectCullingCrossSceneSampler m__E000;

	[SerializeField]
	[Tooltip("Current culling camera.")]
	private PerfectCullingCamera _cullingCamera;

	[Tooltip("Number of work threads for presampling data. Typically 2 threads is enough. More threads may lead to poor performance.")]
	[SerializeField]
	private int _numWorkThreads = 2;

	[SerializeField]
	private PerfectCullingCrossSceneVolume[] _forceInitializeVolumes;

	private Thread[] m__E001;

	private float m__E002;

	private float m__E003;

	private int m__E004;

	private float m__E005;

	private volatile bool m__E006;

	private JobHandle? m__E007;

	private static CancellationTokenSource m__E008;

	private static CancellationToken m__E009;

	public static bool disableWorkThread;

	public static bool disableReuse;

	public static bool disableSampling;

	[CompilerGenerated]
	private bool m__E00A;

	private static readonly _E001[] m__E00B = Array.Empty<_E001>();

	private Task m__E00C;

	private int m__E00D;

	private Bounds m__E00E;

	internal static float _E00F = 5f;

	private const int m__E010 = 1;

	private const int m__E011 = 100;

	private static CullingCellData m__E012;

	private static float _E013 = 2f;

	private static object _E014 = new object();

	private static readonly List<_E004> _E015 = new List<_E004>();

	private JobHandle _E016;

	private static int _E017;

	private List<Vector3> _E018;

	public static PerfectCullingCrossSceneSampler Instance
	{
		[CompilerGenerated]
		get
		{
			return PerfectCullingCrossSceneSampler.m__E000;
		}
		[CompilerGenerated]
		private set
		{
			PerfectCullingCrossSceneSampler.m__E000 = value;
		}
	}

	public PerfectCullingCamera CullingCamera
	{
		get
		{
			return _cullingCamera;
		}
		set
		{
			_cullingCamera = value;
		}
	}

	public bool ShowStats
	{
		[CompilerGenerated]
		get
		{
			return this.m__E00A;
		}
		[CompilerGenerated]
		set
		{
			this.m__E00A = value;
		}
	}

	public static bool IsAutocullingExists
	{
		get
		{
			IReadOnlyCollection<PerfectCullingCrossSceneVolume> allCrossSceneVolumes = PerfectCullingCrossSceneVolume.AllCrossSceneVolumes;
			List<PerfectCullingCrossSceneGroup> allCrossGroups = PerfectCullingCrossSceneGroup.AllCrossGroups;
			if (allCrossSceneVolumes == null || allCrossGroups == null)
			{
				return false;
			}
			if (allCrossSceneVolumes != null && allCrossSceneVolumes.Count > 0)
			{
				if (allCrossGroups == null)
				{
					return false;
				}
				return allCrossGroups.Count > 0;
			}
			return false;
		}
	}

	private void Start()
	{
		this.m__E002 = Time.time;
		_E000();
		_E004();
	}

	private void OnDestroy()
	{
		_E001();
	}

	private void Update()
	{
		_E011();
	}

	private void LateUpdate()
	{
		_E012();
	}

	private void _E000()
	{
		if (_forceInitializeVolumes != null)
		{
			PerfectCullingCrossSceneVolume[] forceInitializeVolumes = _forceInitializeVolumes;
			for (int i = 0; i < forceInitializeVolumes.Length; i++)
			{
				forceInitializeVolumes[i].InitializeRuntimeVolumes();
			}
		}
	}

	private async void _E001()
	{
		_E4BB.Debug(_ED3E._E000(65754));
		if (PerfectCullingCrossSceneSampler.m__E008 != null)
		{
			PerfectCullingCrossSceneSampler.m__E008.Cancel();
		}
		AwaitWorkJobFinish();
		PerfectCullingCrossSceneSampler._E002.JobParams = null;
		if (this.m__E001 != null)
		{
			Thread[] array = this.m__E001;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Join();
			}
		}
		if (this.m__E00C != null)
		{
			await this.m__E00C;
			this.m__E00C = null;
		}
		if (!_E016.IsCompleted)
		{
			_E016.Complete();
		}
		this.m__E001 = null;
		if (PerfectCullingCrossSceneSampler.m__E008 != null)
		{
			PerfectCullingCrossSceneSampler.m__E008.Dispose();
			PerfectCullingCrossSceneSampler.m__E008 = null;
		}
		DeinitializeAutoCulling();
		if (Instance == this)
		{
			Instance = null;
		}
		_E4BB.Debug(_ED3E._E000(65821));
	}

	public static void InitializeAutoCulling()
	{
		_E4BB.Debug(_ED3E._E000(67328));
		if (PerfectCullingCrossSceneVolume.AllRuntimeCrossGroupVolumes.Count > 0)
		{
			_E4BB.Throw(_ED3E._E000(67369));
		}
		foreach (PerfectCullingCrossSceneVolume allCrossSceneVolume in PerfectCullingCrossSceneVolume.AllCrossSceneVolumes)
		{
			allCrossSceneVolume.InitializeRuntimeVolumes();
		}
		foreach (PerfectCullingCrossSceneGroup allCrossGroup in PerfectCullingCrossSceneGroup.AllCrossGroups)
		{
			allCrossGroup.OnPostLevelLoaded();
		}
		PerfectCullingAdaptiveGrid perfectCullingAdaptiveGrid = UnityEngine.Object.FindObjectOfType<PerfectCullingAdaptiveGrid>();
		if (perfectCullingAdaptiveGrid != null)
		{
			perfectCullingAdaptiveGrid._E000();
		}
		_E00E();
		ScreenDistanceSwitcher._E007 = true;
	}

	private static void _E002(Scene prev, Scene newScene)
	{
	}

	private static void _E003()
	{
		Instance.CullingCamera._E003();
		foreach (PerfectCullingCrossSceneGroup allCrossGroup in PerfectCullingCrossSceneGroup.AllCrossGroups)
		{
			allCrossGroup.UpdateSwitchQueuesMT();
		}
	}

	public static int GetRuntimeMemoryUsageMb()
	{
		int num = 0;
		foreach (PerfectCullingCrossSceneVolume._E000 allRuntimeCrossGroupVolume in PerfectCullingCrossSceneVolume.AllRuntimeCrossGroupVolumes)
		{
			num += allRuntimeCrossGroupVolume.streamer.GetRuntimeMemorySizeBytes();
		}
		num += _E4B4.RuntimeMemoryUsage;
		return num / 1048576;
	}

	public static void DeinitializeAutoCulling()
	{
		PerfectCullingBakeGroup.numPersistentShadowLods = 0;
		ScreenDistanceSwitcher._E007 = false;
	}

	internal void _E004()
	{
		_E4BB.Debug(_ED3E._E000(67455) + base.gameObject.name);
		if (Instance != null)
		{
			_E4BB.Throw(_ED3E._E000(67483) + Instance.gameObject.name);
		}
		Instance = this;
		this.m__E006 = true;
		if (_cullingCamera == null)
		{
			_cullingCamera = GetComponent<PerfectCullingCamera>();
		}
		if (this.m__E001 == null)
		{
			if (_cullingCamera == null)
			{
				_cullingCamera = UnityEngine.Object.FindObjectOfType<PerfectCullingCamera>();
			}
			PerfectCullingCrossSceneSampler.m__E008 = new CancellationTokenSource();
			PerfectCullingCrossSceneSampler.m__E009 = PerfectCullingCrossSceneSampler.m__E008.Token;
			if (IsAutocullingExists)
			{
				PerfectCullingCrossSceneSampler._E002.JobParams = new _E000
				{
					camera = _cullingCamera
				};
			}
		}
	}

	private void _E005()
	{
		if (!(Time.time - this.m__E002 >= 1f))
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		this.m__E002 = Time.time;
		foreach (PerfectCullingCrossSceneVolume._E000 allRuntimeCrossGroupVolume in PerfectCullingCrossSceneVolume.AllRuntimeCrossGroupVolumes)
		{
			num += allRuntimeCrossGroupVolume.streamer.bytesRead;
			num2 += allRuntimeCrossGroupVolume.streamer.readOps;
			allRuntimeCrossGroupVolume.streamer.bytesRead = 0;
			allRuntimeCrossGroupVolume.streamer.readOps = 0;
		}
		this.m__E003 = num;
		this.m__E004 = num2;
		if (this.m__E003 > this.m__E005)
		{
			this.m__E005 = this.m__E003;
		}
	}

	private void _E006()
	{
		if (!ShowStats)
		{
			return;
		}
		GUILayout.BeginArea(new Rect(0f, 0f, 400f, Screen.height));
		GUILayout.BeginVertical();
		GUILayout.Label(string.Format(_ED3E._E000(67516), _E017));
		GUILayout.Label(string.Format(_ED3E._E000(67492), this.m__E00D));
		GUILayout.Label(_ED3E._E000(67537) + (this.m__E003 / 1048576f).ToString(_ED3E._E000(29354)) + _ED3E._E000(67525) + (this.m__E005 / 1048576f).ToString(_ED3E._E000(29354)) + _ED3E._E000(67579));
		GUILayout.Label(string.Format(_ED3E._E000(67569), GetRuntimeMemoryUsageMb()));
		GUILayout.Label(string.Format(_ED3E._E000(67555), _E4B4.NumDebugAllocs));
		GUILayout.Label(string.Format(_ED3E._E000(65550), _E4B4.NumFreeBuffers));
		GUILayout.Label(string.Format(_ED3E._E000(65592), this.m__E004));
		GUILayout.Label(string.Format(_ED3E._E000(65576), PerfectCullingBakeGroup.numPersistentShadowLods));
		GUILayout.Space(10f);
		GUILayout.Label(_ED3E._E000(65609));
		int num = 0;
		_E001[] array = PerfectCullingCrossSceneSampler.m__E00B;
		foreach (_E001 obj in array)
		{
			GUILayout.Label(string.Format(_ED3E._E000(65657), num + 1, obj.WorkTime));
			num++;
		}
		if (PerfectCullingCrossSceneGroup.AllCrossGroups == null || PerfectCullingCrossSceneGroup.AllCrossGroups.Count == 0)
		{
			GUILayout.Label(_ED3E._E000(65646));
		}
		else
		{
			foreach (PerfectCullingCrossSceneGroup allCrossGroup in PerfectCullingCrossSceneGroup.AllCrossGroups)
			{
				if (!allCrossGroup.disableOnRuntime)
				{
					(int, int) switchStats = allCrossGroup.SwitchStats;
					GUILayout.Label(string.Format(_ED3E._E000(65688), allCrossGroup.gameObject.name, switchStats.Item1, switchStats.Item2));
				}
			}
		}
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

	public void ScheduleUpdate(_E4A0<int> inputRuntimeVolumeIds, _E4A0<int> inputGroupIds)
	{
		lock (PerfectCullingCrossSceneSampler.m__E00B[0].LockUpdate)
		{
			for (int i = 0; i < inputRuntimeVolumeIds.Count; i++)
			{
				PerfectCullingCrossSceneSampler.m__E00B[0].VolumeIds.Add(inputRuntimeVolumeIds.Buffer[i]);
			}
			for (int j = 0; j < inputGroupIds.Count; j++)
			{
				PerfectCullingCrossSceneSampler.m__E00B[0].GroupIds.Add(inputGroupIds.Buffer[j]);
			}
			PerfectCullingCrossSceneSampler.m__E00B[0].WorkThreadWaitHandle.Set();
		}
	}

	public JobHandle? ScheduleJob(_E4A0<int> inputRuntimeVolumeIds, _E4A0<int> inputGroupIds)
	{
		if (PerfectCullingCrossSceneSampler.m__E008 == null || PerfectCullingCrossSceneSampler.m__E008.IsCancellationRequested || PerfectCullingCrossSceneSampler._E002.JobParams == null)
		{
			return null;
		}
		PerfectCullingCrossSceneSampler._E002.VolumeIds[0].Clear();
		PerfectCullingCrossSceneSampler._E002.VolumeIds[1].Clear();
		PerfectCullingCrossSceneSampler._E002.GroupIds[0].Clear();
		PerfectCullingCrossSceneSampler._E002.GroupIds[1].Clear();
		int num = inputRuntimeVolumeIds.Count / 2;
		for (int i = 0; i < inputRuntimeVolumeIds.Count; i++)
		{
			int num2 = ((i > num) ? 1 : 0);
			PerfectCullingCrossSceneSampler._E002.VolumeIds[num2].Add(inputRuntimeVolumeIds.Buffer[i]);
		}
		int num3 = inputGroupIds.Count / 2;
		for (int j = 0; j < inputGroupIds.Count; j++)
		{
			int num4 = ((j > num3) ? 1 : 0);
			PerfectCullingCrossSceneSampler._E002.GroupIds[num4].Add(inputGroupIds.Buffer[j]);
		}
		if (PerfectCullingCrossSceneSampler._E002.TerminateToken != null)
		{
			PerfectCullingCrossSceneSampler._E002.TerminateToken.Dispose();
		}
		PerfectCullingCrossSceneSampler._E002.TerminateToken = new CancellationTokenSource();
		_E002._E001 jobData = default(_E002._E001);
		JobHandle dependsOn = default(_E002._E000).Schedule(2, 1);
		JobHandle value = jobData.Schedule(2, 1, dependsOn);
		this.m__E007 = value;
		return this.m__E007;
	}

	public bool FinishJob(JobHandle handle)
	{
		CancellationTokenSource terminateToken = PerfectCullingCrossSceneSampler._E002.TerminateToken;
		bool result = handle.IsCompleted && !terminateToken.IsCancellationRequested;
		if (!handle.IsCompleted)
		{
			terminateToken.Cancel();
		}
		handle.Complete();
		return result;
	}

	public void AwaitWorkJobFinish()
	{
		JobHandle? jobHandle = this.m__E007;
		JobHandle valueOrDefault = jobHandle.GetValueOrDefault();
		if (jobHandle.HasValue)
		{
			FinishJob(valueOrDefault);
		}
		if (PerfectCullingCrossSceneSampler._E002.TerminateToken != null)
		{
			PerfectCullingCrossSceneSampler._E002.TerminateToken.Dispose();
			PerfectCullingCrossSceneSampler._E002.TerminateToken = null;
		}
		this.m__E007 = null;
	}

	private static void _E007(object p)
	{
		_E000 obj = p as _E000;
		PerfectCullingCamera camera = obj.camera;
		int threadId = obj.threadId;
		List<int> sortedIndices = new List<int>();
		List<int> removeCells = new List<int>();
		_E001 obj2 = PerfectCullingCrossSceneSampler.m__E00B[threadId];
		Stopwatch stopwatch = Stopwatch.StartNew();
		while (!PerfectCullingCrossSceneSampler.m__E009.IsCancellationRequested)
		{
			try
			{
				obj2.WorkThreadWaitHandle.Wait(PerfectCullingCrossSceneSampler.m__E009);
				if (PerfectCullingCrossSceneSampler.m__E009.IsCancellationRequested)
				{
					break;
				}
				stopwatch.Restart();
				lock (obj2.LockUpdate)
				{
					Vector3 observePosition = camera.ObservePosition;
					if (disableWorkThread)
					{
						obj2.WorkTime = stopwatch.ElapsedMilliseconds;
						obj2.Clear();
						obj2.WorkThreadWaitHandle.Reset();
						continue;
					}
					if (_E008(PerfectCullingCrossSceneSampler.m__E009, camera, observePosition, obj2.VolumeIds, obj2.GroupIds, removeCells, sortedIndices))
					{
						Thread.Yield();
					}
					obj2.WorkTime = stopwatch.ElapsedMilliseconds;
					obj2.Clear();
					obj2.WorkThreadWaitHandle.Reset();
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				_E4BB.Error(ex2.Message + _ED3E._E000(29351) + ex2.StackTrace + _ED3E._E000(18502) + ex2.InnerException?.Message);
			}
		}
	}

	private static bool _E008(CancellationToken terminateToken, PerfectCullingCamera camera, Vector3 worldPos, HashSet<int> volumeIds, HashSet<int> groupIds, List<int> removeCells, List<int> sortedIndices)
	{
		_E009(terminateToken, worldPos, volumeIds, removeCells, sortedIndices);
		if (terminateToken.IsCancellationRequested)
		{
			return false;
		}
		return _E00A(terminateToken, camera, groupIds);
	}

	private static void _E009(CancellationToken terminateToken, Vector3 worldPos, HashSet<int> volumeIds, List<int> removeCells, List<int> sortedIndices)
	{
		foreach (int volumeId in volumeIds)
		{
			PerfectCullingCrossSceneVolume._E000 obj = PerfectCullingCrossSceneVolume.AllRuntimeCrossGroupVolumes[volumeId];
			if (obj == null)
			{
				_E4BB.Throw(_ED3E._E000(65668));
			}
			if (terminateToken.IsCancellationRequested)
			{
				break;
			}
			try
			{
				if (!Monitor.TryEnter(obj.lockSampleQueue))
				{
					continue;
				}
				bool isOutOfBounds;
				int indexForWorldPosNoOrientationMT = obj.GetIndexForWorldPosNoOrientationMT(worldPos, out isOutOfBounds);
				if (isOutOfBounds || obj.lastCellIndex == indexForWorldPosNoOrientationMT)
				{
					continue;
				}
				if (!disableReuse)
				{
					if (obj.runtimeBounds.SqrDistance(worldPos) > obj.reuseDistance2)
					{
						foreach (KeyValuePair<int, _E4A0<ushort>> syncSample in obj.syncSamples)
						{
							if (syncSample.Value != _E4A0<ushort>.Empty)
							{
								_E4B4.ReuseCellArray(syncSample.Value);
							}
						}
						obj.syncSamples.Clear();
					}
					else
					{
						removeCells.Clear();
						foreach (KeyValuePair<int, _E4A0<ushort>> syncSample2 in obj.syncSamples)
						{
							if (syncSample2.Value != _E4A0<ushort>.Empty && (obj.GetSamplingPositionAtMT(syncSample2.Key) - worldPos).sqrMagnitude > obj.reuseDistance2)
							{
								removeCells.Add(syncSample2.Key);
							}
						}
						foreach (int removeCell in removeCells)
						{
							_E4B4.ReuseCellArray(obj.syncSamples[removeCell]);
							obj.syncSamples.Remove(removeCell);
						}
					}
					Thread.Yield();
				}
				if (!disableSampling)
				{
					sortedIndices.Clear();
					Vector3[] visibilityOffsets = obj.visibilityOffsets;
					foreach (Vector3 vector in visibilityOffsets)
					{
						Vector3 pos = worldPos + vector;
						bool isOutOfBounds2;
						int indexForWorldPosMT = obj.GetIndexForWorldPosMT(pos, out isOutOfBounds2);
						if (!isOutOfBounds2 && !obj.syncSamples.ContainsKey(indexForWorldPosMT))
						{
							if (obj.streamer.StreamData.indices[indexForWorldPosMT].dataLength == 0)
							{
								obj.syncSamples[indexForWorldPosMT] = _E4A0<ushort>.Empty;
							}
							else
							{
								sortedIndices.Add(indexForWorldPosMT);
							}
						}
					}
					sortedIndices.Sort();
					foreach (int sortedIndex in sortedIndices)
					{
						_E4A0<ushort> obj2 = _E4B4.AllocateCellArray();
						if (obj.streamer.SampleByIndexFromDisk(sortedIndex, obj2) == 0)
						{
							throw new Exception(_ED3E._E000(65718));
						}
						if (!obj.syncSamples.ContainsKey(sortedIndex))
						{
							obj.syncSamples[sortedIndex] = obj2;
						}
						else
						{
							_E4B4.ReuseCellArray(obj2);
						}
					}
					obj.lastCellIndex = indexForWorldPosNoOrientationMT;
				}
				Thread.Yield();
			}
			catch (Exception ex)
			{
				_E4BB.Error(ex.Message + _ED3E._E000(29351) + ex.StackTrace + _ED3E._E000(2540) + ex?.InnerException?.StackTrace);
			}
			finally
			{
				if (Monitor.IsEntered(obj.lockSampleQueue))
				{
					Monitor.Exit(obj.lockSampleQueue);
				}
			}
		}
	}

	private static bool _E00A(CancellationToken terminateToken, PerfectCullingCamera camera, HashSet<int> groupIds)
	{
		if (!Instance.m__E006)
		{
			return false;
		}
		foreach (int groupId in groupIds)
		{
			if (terminateToken.IsCancellationRequested)
			{
				return false;
			}
			PerfectCullingCrossSceneGroup perfectCullingCrossSceneGroup = PerfectCullingCrossSceneGroup.AllCrossGroups[groupId];
			if (perfectCullingCrossSceneGroup.disableOnRuntime)
			{
				continue;
			}
			int counterMainThread = perfectCullingCrossSceneGroup.counterMainThread;
			try
			{
				if (counterMainThread != perfectCullingCrossSceneGroup.counterWorkThread)
				{
					Monitor.Enter(perfectCullingCrossSceneGroup.lockUpdateVisibilityQueues);
					if (perfectCullingCrossSceneGroup.UpdateVisibleSetsMT(camera))
					{
						perfectCullingCrossSceneGroup.counterWorkThread = counterMainThread;
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (Monitor.IsEntered(perfectCullingCrossSceneGroup.lockUpdateVisibilityQueues))
				{
					Monitor.Exit(perfectCullingCrossSceneGroup.lockUpdateVisibilityQueues);
				}
			}
		}
		return true;
	}

	public static void UpdateBakedLODVisibility(PerfectCullingCrossSceneGroup group, ScreenDistanceSwitcher lod)
	{
		_E00B(null);
	}

	internal static CullingCellData _E00B(CullingCellData newCell)
	{
		lock (_E014)
		{
			PerfectCullingCrossSceneSampler.m__E012 = newCell;
			return PerfectCullingCrossSceneSampler.m__E012;
		}
	}

	private static void _E00C()
	{
		List<ScreenDistanceSwitcher> list = ScreenDistanceSwitcher._E003;
		if (list == null || list.Count == 0)
		{
			return;
		}
		foreach (ScreenDistanceSwitcher item in list)
		{
			item.IsAutocullVisible = _E00D(item, _E015);
		}
	}

	private static bool _E00D(ScreenDistanceSwitcher sw, List<_E004> cameras)
	{
		foreach (_E004 camera in cameras)
		{
			if (!_E4B2._E000(camera.Position, sw.CenterPoint, camera.FieldOfView, sw.Size, sw.RelativeScreenSwitchHeight * ScreenDistanceSwitcher._E005, _E013))
			{
				continue;
			}
			return true;
		}
		return false;
	}

	private static void _E00E()
	{
		_E015.Clear();
		_E015.Add(new _E004(() => _E8A8.Instance?.Camera));
		_E015.Add(new _E004(() => _E8A8.Instance?.OpticCameraManager.Camera));
	}

	private void _E00F()
	{
		foreach (_E004 item in _E015)
		{
			item.Update();
		}
		_E013 = QualitySettings.lodBias;
	}

	[Obsolete("Left for reference code")]
	private async Task _E010(_E003 parameters)
	{
		PerfectCullingCrossSceneSampler.m__E012 = null;
		int num = 0;
		foreach (CullingCellData adaptiveCell in parameters.grid.AdaptiveCells)
		{
			adaptiveCell.RuntimeCellId = num;
			num++;
		}
		while (!PerfectCullingCrossSceneSampler.m__E009.IsCancellationRequested)
		{
			try
			{
				if (!Instance.m__E006 || disableWorkThread)
				{
					await Task.Delay(100);
					continue;
				}
				Vector3 observePosition = _cullingCamera.ObservePosition;
				_E00C();
				CullingCellData cullingCellData = parameters.grid.RuntimeBVH.QueryNearest(observePosition, PerfectCullingCrossSceneSampler._E00F);
				if (PerfectCullingCrossSceneSampler.m__E012 == cullingCellData)
				{
					await Task.Delay(1);
					continue;
				}
				CullingCellData cullingCellData2 = _E00B(cullingCellData);
				if (cullingCellData2 != null)
				{
					this.m__E00D = cullingCellData2.RuntimeCellId;
					this.m__E00E = cullingCellData2.SpatialItemBounds;
					int i = 0;
					foreach (PerfectCullingCrossSceneGroup item in parameters.grid.RuntimeGroupMapping)
					{
						item._E00B(cullingCellData2.CullingData.Find((CullingGroupData x) => x.RuntimeCullingGroupIdx == i));
						i++;
					}
				}
				await Task.Delay(1);
			}
			catch (Exception ex)
			{
				_E4BB.Throw(ex.Message + _ED3E._E000(29351) + ex.StackTrace);
			}
		}
	}

	private void _E011()
	{
		if (!(PerfectCullingAdaptiveGrid.Instance == null) && !(PerfectCullingAdaptiveGrid.Instance.GridData == null))
		{
			_E00F();
			_E005 jobData = default(_E005);
			jobData.ObservePosition = _cullingCamera.ObservePosition;
			_E016 = jobData.Schedule();
			JobHandle.ScheduleBatchedJobs();
		}
	}

	private void _E012()
	{
		_E016.Complete();
	}
}
