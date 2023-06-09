using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using Koenigz.PerfectCulling.SamplingProviders;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Koenigz.PerfectCulling.EFT;

public sealed class PerfectCullingCrossSceneVolume : PerfectCullingBakingBehaviour, _E3B2._E000
{
	[Serializable]
	public sealed class BakeDescriptor
	{
		public enum LoadMode
		{
			Memory,
			Stream,
			AdaptiveGrid
		}

		public GuidReference crossSceneGroup;

		public int bakeCellSizeType;

		public int mergeIterations = 1;

		public StreamingVolumeBakeDataIndex streamingBakeData;

		[NonSerialized]
		public bool debugSamplingPoints;

		public LoadMode loadMode;

		public Vector3 bakeCellSize => Vector3.one * BAKE_CELL_SIZES[bakeCellSizeType];

		public PerfectCullingCrossSceneGroup GetGroup()
		{
			if ((bool)crossSceneGroup.gameObject)
			{
				return crossSceneGroup.gameObject.GetComponent<PerfectCullingCrossSceneGroup>();
			}
			return null;
		}

		public List<Renderer> GetAdditionalOccluders()
		{
			PerfectCullingCrossSceneGroup group = GetGroup();
			if (group == null)
			{
				return null;
			}
			if (group.sharedOccluder != null)
			{
				return group.sharedOccluder.GetRenderers();
			}
			return new List<Renderer>();
		}

		public void ValidateDescriptor()
		{
			if (GetGroup() == null)
			{
				throw new Exception(_ED3E._E000(66149) + crossSceneGroup.ObjectGuid.ToString());
			}
			if (streamingBakeData.IsValid)
			{
				return;
			}
			throw new Exception(_ED3E._E000(65965) + crossSceneGroup.ObjectGuid.ToString());
		}
	}

	public new sealed class _E000
	{
		public enum EUpdateResult
		{
			OutOfBounds,
			Ok,
			NoData
		}

		public PerfectCullingCrossSceneGroup group;

		public int groupIndex;

		public StreamingVolumeBakeDataIndex volumeBakeData;

		public Vector3[] visibilityOffsets;

		public Vector3 volumeSize;

		public Vector3 volumePosition;

		public Quaternion volumeRotation;

		public string volumeName;

		public Bounds runtimeBounds;

		public int lastCellIndex;

		public float reuseDistance2;

		public object lockSampleQueue;

		public object lockRemoveCells;

		public object lockReuse;

		public _E4B5 streamer;

		public ExcludeInnerVolumeSamplingProvider excludeInnerVolumeSamplingProvider;

		public Dictionary<int, _E4A0<ushort>> syncSamples;

		public string parentVolumeName;

		public string groupName;

		public Vector3 runtimeCellCount;

		public bool IsFineVolume => excludeInnerVolumeSamplingProvider == null;

		public _E000(BakeDescriptor desc, PerfectCullingCrossSceneVolume parentVolume)
		{
			group = desc.GetGroup();
			group._runtimeSharedVolumes.Add(this);
			groupIndex = PerfectCullingCrossSceneGroup.AllCrossGroups.IndexOf(group);
			runtimeBounds = parentVolume.volumeBakeBounds;
			excludeInnerVolumeSamplingProvider = parentVolume.GetComponent<ExcludeInnerVolumeSamplingProvider>();
			streamer = new _E4B5(desc.streamingBakeData, StreamingVolumeBakeDataIndex.GetStreamingDataFilePath(parentVolume, desc), desc.loadMode);
			volumeName = parentVolume.name;
			volumePosition = parentVolume.transform.position;
			volumeRotation = parentVolume.transform.rotation;
			volumeSize = parentVolume.volumeSize;
			volumeBakeData = desc.streamingBakeData;
			lastCellIndex = int.MaxValue;
			lockSampleQueue = new object();
			lockRemoveCells = new object();
			syncSamples = new Dictionary<int, _E4A0<ushort>>();
			visibilityOffsets = _E4A3.GenerateOffsets(1, volumeBakeData.cellSize).ToArray();
			float num = volumeBakeData.cellSize.x * 2f;
			reuseDistance2 = num * num;
			runtimeCellCount = _E49E.CalculateCellCount(volumeSize, volumeBakeData.cellSize);
			parentVolumeName = parentVolume.gameObject.name;
			groupName = group.gameObject.name;
		}

		public Vector3 GetSamplingPositionAtMT(int index)
		{
			Vector3 cellSize = volumeBakeData.cellSize;
			Vector3 cellCount = volumeBakeData.cellCount;
			Vector3 vector = cellCount * 0.5f;
			_E49E.UnflattenToXYZ(index, out var x, out var y, out var z, cellCount);
			Vector3 vector2 = cellSize / 2f;
			vector2.x += (float)x * cellSize.x;
			vector2.y += (float)y * cellSize.y;
			vector2.z += (float)z * cellSize.z;
			vector2.x -= vector.x * cellSize.x;
			vector2.y -= vector.y * cellSize.y;
			vector2.z -= vector.z * cellSize.z;
			return volumePosition + volumeRotation * vector2;
		}

		private void _E000(_E4A0<ushort> indices, int newCounter)
		{
			PerfectCullingBakeGroup[] bakeGroups = group.bakeGroups;
			int count = indices.Count;
			ushort[] buffer = indices.Buffer;
			if (bakeGroups.Length < indices.Count)
			{
				_E4BB.Error(string.Format(_ED3E._E000(66199), group.bakeGroups.Length, count));
				return;
			}
			for (int i = 0; i < count; i++)
			{
				bakeGroups[buffer[i]].updateCounter = newCounter;
			}
		}

		public EUpdateResult UpdateVisibleRenderersAtPointMT(Vector3 pos, int newCounter)
		{
			if (!runtimeBounds.Contains(pos))
			{
				return EUpdateResult.OutOfBounds;
			}
			if (lastCellIndex == int.MaxValue)
			{
				return EUpdateResult.OutOfBounds;
			}
			_E4A0<ushort> precomputedIndices = GetPrecomputedIndices(lastCellIndex);
			if (precomputedIndices == _E4A0<ushort>.Empty)
			{
				return EUpdateResult.Ok;
			}
			if (precomputedIndices != null)
			{
				_E000(precomputedIndices, newCounter);
				return EUpdateResult.Ok;
			}
			_E4BB.Warning(_ED3E._E000(66265) + parentVolumeName + _ED3E._E000(66006) + groupName);
			return EUpdateResult.NoData;
		}

		public _E4A0<ushort> GetPrecomputedIndices(int index)
		{
			if (Monitor.TryEnter(lockSampleQueue))
			{
				syncSamples.TryGetValue(index, out var value);
				Monitor.Exit(lockSampleQueue);
				return value;
			}
			return null;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetIndexForWorldPosNoOrientationMT(Vector3 pos, out bool isOutOfBounds)
		{
			return _E49E.GetIndexForWorldPosNoOrientation(pos, volumePosition, volumeSize, runtimeCellCount, volumeBakeData.cellSize, out isOutOfBounds);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetIndexForWorldPosMT(Vector3 pos, out bool isOutOfBounds)
		{
			return GetIndexForWorldPosMT(pos, runtimeCellCount, volumeBakeData.cellSize, out isOutOfBounds);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetIndexForWorldPosMT(Vector3 pos, Vector3 cellCount, Vector3 cellSize, out bool isOutOfBounds)
		{
			Quaternion gridBakeOrientation = ((volumeBakeData == null) ? volumeRotation : volumeBakeData.orientation);
			return _E49E.GetIndexForWorldPos(pos, volumePosition, volumeRotation, volumeSize, gridBakeOrientation, cellCount, cellSize, out isOutOfBounds);
		}
	}

	public static readonly float[] BAKE_CELL_SIZES = new float[5] { 0.25f, 0.5f, 1f, 2f, 3f };

	public static readonly string[] BAKE_CELL_DISPLAY_VALUES = new string[5]
	{
		_ED3E._E000(66134),
		_ED3E._E000(66124),
		_ED3E._E000(66121),
		_ED3E._E000(66116),
		_ED3E._E000(66119)
	};

	public static readonly string[] BAKE_CELL_DISPLAY_FILTER_VALUES = new string[6]
	{
		_ED3E._E000(66114),
		_ED3E._E000(66134),
		_ED3E._E000(66124),
		_ED3E._E000(66121),
		_ED3E._E000(66116),
		_ED3E._E000(66119)
	};

	public static readonly string[] LOAD_MODES = new string[3]
	{
		_ED3E._E000(66174),
		_ED3E._E000(66165),
		_ED3E._E000(66156)
	};

	private static readonly Color _E003 = new Color(0f, 1f, 0f, 0.25f);

	public const int MAX_RUNTIME_VOLUMES = 4096;

	public static readonly List<_E000> AllRuntimeCrossGroupVolumes = new List<_E000>();

	private static readonly List<PerfectCullingCrossSceneVolume> _E004 = new List<PerfectCullingCrossSceneVolume>();

	private static List<int> _E005;

	private List<_E000> _E006 = new List<_E000>();

	[SerializeField]
	private string _volumeGuid;

	[SerializeField]
	public Vector3 volumeSize = Vector3.one * 2f;

	public Bounds runtimeVolumeBounds;

	[SerializeField]
	public List<BakeDescriptor> bakeDescriptors = new List<BakeDescriptor>();

	private static BakeDescriptor _E007;

	public static IReadOnlyCollection<PerfectCullingCrossSceneVolume> AllCrossSceneVolumes => _E004;

	public bool IsFineVolume => GetComponent<ExcludeInnerVolumeSamplingProvider>() == null;

	public string VolumeGuid => _volumeGuid;

	[SerializeField]
	public Bounds volumeBakeBounds
	{
		get
		{
			return new Bounds(base.transform.position, volumeSize);
		}
		set
		{
			base.transform.position = value.center;
			volumeSize = new Vector3(Mathf.Max(1f, value.size.x), Mathf.Max(1f, value.size.y), Mathf.Max(1f, value.size.z));
		}
	}

	public Vector3 HandleSized
	{
		get
		{
			return volumeBakeBounds.size;
		}
		set
		{
			volumeBakeBounds = new Bounds(base.transform.position, value);
		}
	}

	public float MaxCellSize
	{
		get
		{
			float num = 0f;
			foreach (BakeDescriptor bakeDescriptor in bakeDescriptors)
			{
				if (BAKE_CELL_SIZES[bakeDescriptor.bakeCellSizeType] > num)
				{
					num = BAKE_CELL_SIZES[bakeDescriptor.bakeCellSizeType];
				}
			}
			return num;
		}
	}

	public static BakeDescriptor ActiveBakeDescriptor => _E007;

	public override PerfectCullingBakeData BakeData => PerfectCullingResourcesLocator.Instance.tempVolumeBakeData;

	public override PerfectCullingBakeGroup[] bakeGroups
	{
		get
		{
			return _E007.GetGroup().bakeGroups;
		}
		set
		{
			throw new InvalidOperationException();
		}
	}

	public override List<Renderer> additionalOccluders
	{
		get
		{
			return _E007.GetAdditionalOccluders();
		}
		set
		{
			throw new InvalidOperationException();
		}
	}

	public PerfectCullingVolumeBakeData volumeBakeData => PerfectCullingResourcesLocator.Instance.tempVolumeBakeData;

	public int CellCount => _E49E.CalculateNumberOfCells(volumeSize, _E007.bakeCellSize);

	public BakeDescriptor AddBakeDescriptor()
	{
		BakeDescriptor bakeDescriptor = new BakeDescriptor();
		bakeDescriptors.Add(bakeDescriptor);
		return bakeDescriptor;
	}

	public bool HasGroup(PerfectCullingCrossSceneGroup group)
	{
		foreach (BakeDescriptor bakeDescriptor in bakeDescriptors)
		{
			if (!(bakeDescriptor.crossSceneGroup != null) || !(bakeDescriptor.GetGroup() == group))
			{
				continue;
			}
			return true;
		}
		return false;
	}

	public void FixVolumeSize()
	{
		Bounds bounds = volumeBakeBounds;
		bounds.size = _E4A3.Snap(bounds.size, MaxCellSize);
		volumeBakeBounds = bounds;
	}

	public void ScheduleBake(List<int> bakeDescriptors)
	{
		if (bakeDescriptors.Count == 0)
		{
			_E007 = null;
			return;
		}
		FixVolumeSize();
		_E005 = new List<int>(bakeDescriptors);
		for (int i = 0; i < _E005.Count; i++)
		{
			_E49A.ScheduleBake(new _E490
			{
				BakingBehaviour = this,
				AdditionalOccluders = null
			});
		}
		_E49A.BakeAllScheduled();
	}

	public override void SetBakeData(PerfectCullingBakeData bakeData)
	{
		throw new InvalidOperationException(_ED3E._E000(65798));
	}

	public override void InitializeBake()
	{
		if (_E005.Count == 0)
		{
			throw new InvalidOperationException(_ED3E._E000(65850));
		}
		int index = _E005[0];
		_E007 = bakeDescriptors[index];
		_E005.RemoveAt(0);
		Debug.Log(_ED3E._E000(65834) + base.gameObject.name + _ED3E._E000(91199) + _E007.crossSceneGroup.gameObject.name);
	}

	public override bool PreBake()
	{
		SceneManager.SetActiveScene(_E007.crossSceneGroup.gameObject.scene);
		volumeSize = new Vector3((int)volumeSize.x, (int)volumeSize.y, (int)volumeSize.z);
		Vector3 bakeCellSize = _E007.bakeCellSize;
		Vector3 cellCount = new Vector3(volumeSize.x / bakeCellSize.x, volumeSize.y / bakeCellSize.y, volumeSize.z / bakeCellSize.z);
		volumeBakeData.volumeGuid = _volumeGuid;
		volumeBakeData.SetVolumeBakeData(bakeCellSize, cellCount);
		if ((int)volumeBakeData.cellCount.x == 0 || (int)volumeBakeData.cellCount.y == 0 || (int)volumeBakeData.cellCount.z == 0)
		{
			return false;
		}
		return true;
	}

	public override void PostBake()
	{
		int mergeIterations = _E007.mergeIterations;
		for (int i = 0; i < mergeIterations; i++)
		{
			volumeBakeData.MergeDownsample();
		}
		volumeBakeData.groupGuid = _E007.crossSceneGroup.ObjectGuid.ToString(_ED3E._E000(40858));
		Debug.Log(_ED3E._E000(65881) + base.gameObject.name + _ED3E._E000(91199) + _E007.crossSceneGroup.gameObject.name);
	}

	internal List<Vector3> _E000(Vector3 overrideBakeCellSize, Space space = Space.World, int maxPoints = 0)
	{
		InitializeAllSamplingProviders();
		int num = _E49E.CalculateNumberOfCells(volumeSize, overrideBakeCellSize);
		int num2 = ((maxPoints > 0) ? maxPoints : num);
		List<Vector3> list = new List<Vector3>();
		int num3 = 0;
		for (int i = 0; i < num; i++)
		{
			if (num3 >= num2)
			{
				break;
			}
			Vector3 samplingPositionAt = GetSamplingPositionAt(i, overrideBakeCellSize, space);
			if (SamplingProvidersIsPositionActive(samplingPositionAt))
			{
				list.Add(samplingPositionAt);
				num3++;
			}
		}
		return list;
	}

	public override List<Vector3> GetSamplingPositions(Space space = Space.Self)
	{
		List<Vector3> list = new List<Vector3>(CellCount);
		for (int i = 0; i < CellCount; i++)
		{
			list.Add(GetSamplingPositionAt(i, _E007.bakeCellSize, space));
		}
		return list;
	}

	public IEnumerable<Vector3?> GetSamplingPositionsEnum(Vector3 cellSize, Space space = Space.Self)
	{
		int num = _E49E.CalculateNumberOfCells(volumeSize, cellSize);
		int num2 = 0;
		while (num2 < num)
		{
			yield return GetSamplingPositionAt(num2, cellSize, space);
			int num3 = num2 + 1;
			num2 = num3;
		}
		yield return null;
	}

	public List<Vector3> GetSamplingPositions(Vector3 cellSize, Space space = Space.Self)
	{
		int num = _E49E.CalculateNumberOfCells(volumeSize, cellSize);
		List<Vector3> list = new List<Vector3>(num);
		for (int i = 0; i < num; i++)
		{
			list.Add(GetSamplingPositionAt(i, cellSize, space));
		}
		return list;
	}

	public override int GetIndexForWorldPos(Vector3 worldPos, out bool isOutOfBounds)
	{
		return _E49E.GetIndexForWorldPos(worldPos, base.transform.position, base.transform.rotation, volumeSize, base.transform.rotation, _E49E.CalculateCellCount(volumeSize, _E007.bakeCellSize), _E007.bakeCellSize, out isOutOfBounds);
	}

	public override Vector3 GetWorldPositionForIndex(int index)
	{
		return GetSamplingPositionAt(index, _E007.bakeCellSize, Space.World);
	}

	public Vector3 GetSamplingPositionAt(int index, Vector3 cellSize, Space space = Space.Self)
	{
		Vector3 gridSize = new Vector3(volumeSize.x / cellSize.x, volumeSize.y / cellSize.y, volumeSize.z / cellSize.z);
		return GetSamplingPositionAt(index, gridSize, cellSize, space);
	}

	public Vector3 GetSamplingPositionAt(int index, Vector3 gridSize, Vector3 cellSize, Space space = Space.Self)
	{
		Vector3 vector = gridSize * 0.5f;
		_E49E.UnflattenToXYZ(index, out var x, out var y, out var z, gridSize);
		Vector3 vector2 = cellSize / 2f + new Vector3((float)x * cellSize.x, (float)y * cellSize.y, (float)z * cellSize.z) - new Vector3(vector.x * cellSize.x, vector.y * cellSize.y, vector.z * cellSize.z);
		if (space == Space.World)
		{
			return base.transform.position + base.transform.rotation * vector2;
		}
		return vector2;
	}

	public override int GetBakeHash()
	{
		return _E007.GetGroup().GetBakeHash();
	}

	protected override void CullAdditionalOccluders(ref HashSet<Renderer> additionalOccluders)
	{
		if (additionalOccluders == null)
		{
			return;
		}
		Bounds bounds = new Bounds(base.transform.position, volumeSize);
		HashSet<Renderer> hashSet = new HashSet<Renderer>();
		foreach (Renderer additionalOccluder in additionalOccluders)
		{
			if (bounds.Intersects(additionalOccluder.bounds))
			{
				hashSet.Add(additionalOccluder);
			}
		}
		additionalOccluders = hashSet;
	}

	public override void PostProcessBakeData(PerfectCullingBakeData data)
	{
	}

	private void OnDestroy()
	{
		_E001();
	}

	public override void Start()
	{
		_E002();
	}

	internal void _E001()
	{
		_E004.Remove(this);
		if (_E006 == null)
		{
			return;
		}
		if (PerfectCullingCrossSceneSampler.Instance != null)
		{
			PerfectCullingCrossSceneSampler.Instance.AwaitWorkJobFinish();
		}
		foreach (_E000 item in _E006)
		{
			AllRuntimeCrossGroupVolumes.Remove(item);
			foreach (_E4A0<ushort> value in item.syncSamples.Values)
			{
				if (value != _E4A0<ushort>.Empty)
				{
					_E4B4.ReuseCellArray(value);
				}
			}
			item.syncSamples.Clear();
			item.streamer.Dispose();
			item.streamer = null;
		}
		_E006.Clear();
		_E006 = null;
	}

	internal void _E002()
	{
		_E004.Add(this);
		runtimeVolumeBounds = volumeBakeBounds;
	}

	public void InitializeRuntimeVolumes()
	{
		foreach (BakeDescriptor bakeDescriptor in bakeDescriptors)
		{
			PerfectCullingCrossSceneGroup group = bakeDescriptor.GetGroup();
			if (group == null || !group.gameObject.activeInHierarchy)
			{
				_E4BB.Error(_ED3E._E000(65864));
				continue;
			}
			if (bakeDescriptor.streamingBakeData == null)
			{
				_E4BB.Warning(string.Format(_ED3E._E000(65932), VolumeGuid, bakeDescriptor.crossSceneGroup.ObjectGuid, group.gameObject.name));
				continue;
			}
			if (!bakeDescriptor.streamingBakeData.IsValid)
			{
				_E4BB.Error(_ED3E._E000(65965) + VolumeGuid + _ED3E._E000(66006) + bakeDescriptor.crossSceneGroup.ObjectGuid.ToString());
				continue;
			}
			if (group.GetBakeHash() != bakeDescriptor.streamingBakeData.bakeHash)
			{
				_E4BB.Error(string.Format(_ED3E._E000(66003), VolumeGuid, bakeDescriptor.crossSceneGroup.ObjectGuid, group.gameObject.name, group.GetBakeHash(), bakeDescriptor.streamingBakeData.bakeHash));
				continue;
			}
			string streamingDataFilePath = StreamingVolumeBakeDataIndex.GetStreamingDataFilePath(this, bakeDescriptor);
			if (!File.Exists(streamingDataFilePath))
			{
				_E4BB.Warning(_ED3E._E000(66069) + streamingDataFilePath);
			}
			else
			{
				if (group.disableOnRuntime)
				{
					continue;
				}
				if (bakeDescriptor.loadMode == BakeDescriptor.LoadMode.AdaptiveGrid)
				{
					group.disableOnRuntime = true;
					continue;
				}
				_E000 item = new _E000(bakeDescriptor, this);
				if (_E006 == null)
				{
					_E006 = new List<_E000>();
				}
				_E006.Add(item);
				AllRuntimeCrossGroupVolumes.Add(item);
				_E4BB.Debug(string.Format(_ED3E._E000(66105), base.gameObject.name, group.gameObject.name, bakeDescriptor.streamingBakeData.cellSize, bakeDescriptor.streamingBakeData.originalCellSize));
			}
		}
	}

	public void ReplaceGroups(List<PerfectCullingCrossSceneGroup> newGroups)
	{
		bakeDescriptors = new List<BakeDescriptor>();
		foreach (PerfectCullingCrossSceneGroup newGroup in newGroups)
		{
			AddBakeDescriptor().crossSceneGroup = new GuidReference(newGroup.GetComponent<GuidComponent>());
		}
	}

	public static int GetFrameHashNoOrientation(Vector3 pos, _E4A0<int> runtimeVolumesToUpdate = null, _E4A0<int> runtimeGroupsToUpdate = null)
	{
		int num = 13;
		int num2 = 0;
		foreach (_E000 allRuntimeCrossGroupVolume in AllRuntimeCrossGroupVolumes)
		{
			bool isOutOfBounds;
			int indexForWorldPosNoOrientationMT = allRuntimeCrossGroupVolume.GetIndexForWorldPosNoOrientationMT(pos, out isOutOfBounds);
			if (!isOutOfBounds)
			{
				num = num * 17 + indexForWorldPosNoOrientationMT;
				runtimeVolumesToUpdate?.Add(num2);
				runtimeGroupsToUpdate?.Add(allRuntimeCrossGroupVolume.groupIndex);
			}
			num2++;
		}
		return num;
	}
}
