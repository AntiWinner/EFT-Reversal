using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Koenigz.PerfectCulling;

[PreferBinarySerialization]
[CreateAssetMenu]
public class PerfectCullingVolumeBakeData : PerfectCullingBakeData
{
	[Serializable]
	public struct VisibilitySet
	{
		public byte[] compressed;

		public ushort numDeltaValues;
	}

	[Serializable]
	public struct RawData
	{
		public ushort[] uncompressed;
	}

	[CompilerGenerated]
	private sealed class _E000
	{
		public PerfectCullingVolumeBakeData _003C_003E4__this;

		public Vector3Int OptimizedCellSize;

		public Vector3Int optDim;

		public PerfectCullingVolumeBakeData tmpBakeData;

		public int processedElementCount;

		internal Task _E000(IGrouping<int, int> groups)
		{
			return Task.Run((Func<Task>)new _E001
			{
				CS_0024_003C_003E8__locals1 = this,
				groups = groups
			}._E000);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public IGrouping<int, int> groups;

		public PerfectCullingVolumeBakeData._E000 CS_0024_003C_003E8__locals1;

		internal async Task _E000()
		{
			HashSet<ushort> hashSet = new HashSet<ushort>();
			int num = 0;
			foreach (int group in groups)
			{
				_E49E.UnflattenToXYZ(group, out var x, out var y, out var z, CS_0024_003C_003E8__locals1._003C_003E4__this.cellCount);
				int x2 = x / CS_0024_003C_003E8__locals1.OptimizedCellSize.x;
				int y2 = y / CS_0024_003C_003E8__locals1.OptimizedCellSize.y;
				int z2 = z / CS_0024_003C_003E8__locals1.OptimizedCellSize.z;
				int num2 = _E49E.FlattenXYZDouble(x2, y2, z2, CS_0024_003C_003E8__locals1.optDim);
				hashSet.Clear();
				for (int i = -1; i <= 1; i++)
				{
					for (int j = -1; j <= 1; j++)
					{
						for (int k = -1; k <= 1; k++)
						{
							if (!_E49E.IsXYZInBounds(x + i, y + j, z + k, CS_0024_003C_003E8__locals1._003C_003E4__this.cellCount))
							{
								continue;
							}
							int num3 = _E49E.FlattenXYZDouble(x + i, y + j, z + k, CS_0024_003C_003E8__locals1._003C_003E4__this.cellCount);
							if (CS_0024_003C_003E8__locals1._003C_003E4__this.rawData[num3].uncompressed != null)
							{
								ushort[] uncompressed = CS_0024_003C_003E8__locals1._003C_003E4__this.rawData[num3].uncompressed;
								foreach (ushort item in uncompressed)
								{
									hashSet.Add(item);
								}
							}
						}
					}
				}
				if (CS_0024_003C_003E8__locals1.tmpBakeData.rawData[num2].uncompressed != null)
				{
					ushort[] uncompressed = CS_0024_003C_003E8__locals1.tmpBakeData.rawData[num2].uncompressed;
					foreach (ushort item2 in uncompressed)
					{
						hashSet.Add(item2);
					}
				}
				CS_0024_003C_003E8__locals1.tmpBakeData.rawData[num2].uncompressed = hashSet.ToArray();
				num++;
			}
			Interlocked.Add(ref CS_0024_003C_003E8__locals1.processedElementCount, num);
		}
	}

	public Vector3 cellCount;

	public Vector3 cellSize;

	public Vector3 originalCellSize;

	public Quaternion orientation;

	public VisibilitySet[] data;

	public RawData[] rawData;

	[NonSerialized]
	public int runtimeAssetSize = -1;

	public int maxStoredIndex = -1;

	public int numberOfGroups = -1;

	public string volumeGuid = "";

	public string groupGuid = "";

	public const int MaxValue = 15;

	public const int HeaderBitSize = 2;

	public static uint[] BITS = new uint[4] { 1u, 2u, 3u, 4u };

	private static _E4A7 m__E000 = new _E4A7();

	private static _E4A6 m__E001 = new _E4A6();

	public int RuntimeAssetSize
	{
		get
		{
			if (runtimeAssetSize == -1)
			{
				int num = 0;
				VisibilitySet[] array = data;
				for (int i = 0; i < array.Length; i++)
				{
					VisibilitySet visibilitySet = array[i];
					num += visibilitySet.compressed.Length;
					num += 2;
				}
				runtimeAssetSize = num;
			}
			return runtimeAssetSize;
		}
	}

	public void SetVolumeBakeData(Vector3 cellSize, Vector3 cellCount)
	{
		originalCellSize = cellSize;
		this.cellSize = cellSize;
		this.cellCount = cellCount;
	}

	public override void PrepareForBake(PerfectCullingBakingBehaviour bakingBehaviour)
	{
		orientation = bakingBehaviour.transform.rotation;
		data = new VisibilitySet[bakingBehaviour.GetSamplingPositions().Count];
		rawData = new RawData[data.Length];
		maxStoredIndex = -1;
		numberOfGroups = bakingBehaviour.bakeGroups.Length;
	}

	public override ushort[] GetRawData(int index)
	{
		if (rawData[index].uncompressed == null)
		{
			return Array.Empty<ushort>();
		}
		return rawData[index].uncompressed;
	}

	public override void SetRawData(int index, ushort[] indices)
	{
		if (indices == null || indices.Length == 0)
		{
			rawData[index] = new RawData
			{
				uncompressed = Array.Empty<ushort>()
			};
			maxStoredIndex = 0;
			return;
		}
		rawData[index] = new RawData
		{
			uncompressed = indices
		};
		ushort[] uncompressed = rawData[index].uncompressed;
		foreach (ushort a in uncompressed)
		{
			maxStoredIndex = Mathf.Max(a, maxStoredIndex);
		}
	}

	public unsafe override void SetRawDataMT(int index, int* buffer, int count)
	{
		if (count == 0)
		{
			rawData[index] = new RawData
			{
				uncompressed = Array.Empty<ushort>()
			};
			maxStoredIndex = 0;
			return;
		}
		ushort[] array = new ushort[count];
		for (int i = 0; i < count; i++)
		{
			maxStoredIndex = Mathf.Max(array[i] = (ushort)buffer[i], maxStoredIndex);
		}
		rawData[index] = new RawData
		{
			uncompressed = array
		};
	}

	public override void SetRawData(int index, _E493 indices)
	{
		if (indices == null)
		{
			rawData[index] = new RawData
			{
				uncompressed = Array.Empty<ushort>()
			};
			maxStoredIndex = 0;
			return;
		}
		ushort[] bufferCopy = indices.BufferCopy;
		rawData[index] = new RawData
		{
			uncompressed = bufferCopy
		};
		ushort[] array = bufferCopy;
		foreach (ushort a in array)
		{
			maxStoredIndex = Mathf.Max(a, maxStoredIndex);
		}
	}

	private void _E000(int index, ushort[] indices)
	{
		if (indices.Length == 0)
		{
			return;
		}
		List<int> list = new List<int>();
		_E001(indices[0], list);
		for (int i = 1; i < indices.Length; i++)
		{
			_E001(indices[i] - indices[i - 1], list);
		}
		PerfectCullingVolumeBakeData.m__E001.Reset();
		for (int j = 0; j < list.Count; j++)
		{
			int num;
			if (list[j] <= 1)
			{
				num = 0;
			}
			else if (list[j] <= 3)
			{
				num = 1;
			}
			else if (list[j] <= 7)
			{
				num = 2;
			}
			else
			{
				if (list[j] > 15)
				{
					throw new Exception(_ED3E._E000(72684));
				}
				num = 3;
			}
			PerfectCullingVolumeBakeData.m__E001.Write((uint)num, 2);
			PerfectCullingVolumeBakeData.m__E001.Write((uint)list[j], (int)BITS[num]);
		}
		PerfectCullingVolumeBakeData.m__E001.Flush();
		byte[] array = new byte[PerfectCullingVolumeBakeData.m__E001.Length];
		Array.Copy(PerfectCullingVolumeBakeData.m__E001.Buffer, array, PerfectCullingVolumeBakeData.m__E001.Length);
		data[index] = new VisibilitySet
		{
			compressed = array,
			numDeltaValues = (ushort)list.Count
		};
		List<ushort> list2 = new List<ushort>();
		SampleAtIndex(index, list2);
		if (indices.Length != list2.Count)
		{
			throw new Exception(_ED3E._E000(72680) + indices.Length + _ED3E._E000(66586) + list2.Count);
		}
		for (int k = 0; k < indices.Length; k++)
		{
			if (indices[k] != list2[k])
			{
				throw new Exception(_ED3E._E000(66583) + indices[k] + _ED3E._E000(18502) + list2[k] + _ED3E._E000(66569) + index + _ED3E._E000(29184) + k + _ED3E._E000(66563));
			}
		}
	}

	public override void CompleteBake()
	{
		if (rawData != null && rawData.Length != 0)
		{
			data = new VisibilitySet[rawData.Length];
			for (int i = 0; i < rawData.Length; i++)
			{
				_E4A2.ListUshort.Clear();
				_E4A2.ListUshort.AddRange(rawData[i].uncompressed);
				_E4A2.ListUshort.Sort();
				_E000(i, _E4A2.ListUshort.ToArray());
			}
			rawData = null;
		}
	}

	public void MergeDownsample()
	{
		if (cellCount.x == 1f && cellCount.y == 1f && cellCount.z == 1f)
		{
			Debug.LogWarning(_ED3E._E000(66652));
			return;
		}
		PerfectCullingVolumeBakeData tmpBakeData = ScriptableObject.CreateInstance<PerfectCullingVolumeBakeData>();
		Vector3Int OptimizedCellSize = new Vector3Int(2, 2, 2);
		Vector3Int vector3Int = new Vector3Int((int)((cellCount.x % (float)OptimizedCellSize.x == 0f) ? cellCount.x : (cellCount.x + (float)OptimizedCellSize.x - cellCount.x % (float)OptimizedCellSize.x)), (int)((cellCount.y % (float)OptimizedCellSize.y == 0f) ? cellCount.y : (cellCount.y + (float)OptimizedCellSize.y - cellCount.y % (float)OptimizedCellSize.y)), (int)((cellCount.z % (float)OptimizedCellSize.z == 0f) ? cellCount.z : (cellCount.z + (float)OptimizedCellSize.z - cellCount.z % (float)OptimizedCellSize.z)));
		Vector3Int optDim = new Vector3Int(vector3Int.x / OptimizedCellSize.x, vector3Int.y / OptimizedCellSize.y, vector3Int.z / OptimizedCellSize.z);
		tmpBakeData.rawData = new RawData[optDim.x * optDim.y * optDim.z];
		tmpBakeData.cellCount = optDim;
		tmpBakeData.cellSize = new Vector3(cellSize.x * (float)OptimizedCellSize.x, cellSize.y * (float)OptimizedCellSize.y, cellSize.z * (float)OptimizedCellSize.z);
		int num = Mathf.CeilToInt(cellCount.x * cellCount.y * cellCount.z);
		IEnumerable<IGrouping<int, int>> source = from val in Enumerable.Range(0, num)
			group val by val % 32;
		int processedElementCount = 0;
		Task task = Task.WhenAll(source.Select((IGrouping<int, int> groups) => Task.Run(async delegate
		{
			HashSet<ushort> hashSet = new HashSet<ushort>();
			int num3 = 0;
			foreach (int group in groups)
			{
				_E49E.UnflattenToXYZ(group, out var x, out var y, out var z, cellCount);
				int x2 = x / OptimizedCellSize.x;
				int y2 = y / OptimizedCellSize.y;
				int z2 = z / OptimizedCellSize.z;
				int num4 = _E49E.FlattenXYZDouble(x2, y2, z2, optDim);
				hashSet.Clear();
				for (int i = -1; i <= 1; i++)
				{
					for (int j = -1; j <= 1; j++)
					{
						for (int k = -1; k <= 1; k++)
						{
							if (_E49E.IsXYZInBounds(x + i, y + j, z + k, cellCount))
							{
								int num5 = _E49E.FlattenXYZDouble(x + i, y + j, z + k, cellCount);
								if (rawData[num5].uncompressed != null)
								{
									ushort[] uncompressed = rawData[num5].uncompressed;
									foreach (ushort item in uncompressed)
									{
										hashSet.Add(item);
									}
								}
							}
						}
					}
				}
				if (tmpBakeData.rawData[num4].uncompressed != null)
				{
					ushort[] uncompressed = tmpBakeData.rawData[num4].uncompressed;
					foreach (ushort item2 in uncompressed)
					{
						hashSet.Add(item2);
					}
				}
				tmpBakeData.rawData[num4].uncompressed = hashSet.ToArray();
				num3++;
			}
			Interlocked.Add(ref processedElementCount, num3);
		})));
		TaskAwaiter awaiter = task.GetAwaiter();
		int num2 = 0;
		while (num2 != num && !task.Wait(100))
		{
			num2 = Interlocked.CompareExchange(ref processedElementCount, 0, 0);
		}
		awaiter.GetResult();
		rawData = tmpBakeData.rawData;
		cellCount = tmpBakeData.cellCount;
		cellSize = tmpBakeData.cellSize;
		UnityEngine.Object.DestroyImmediate(tmpBakeData);
	}

	public override void SampleAtIndex(int index, List<ushort> indices)
	{
		PerfectCullingVolumeBakeData.m__E000.Reset(data[index].compressed);
		ushort num = 0;
		int numDeltaValues = data[index].numDeltaValues;
		for (int i = 0; i < numDeltaValues; i++)
		{
			uint num2 = PerfectCullingVolumeBakeData.m__E000.Read(2);
			uint bits = BITS[num2];
			uint num3 = PerfectCullingVolumeBakeData.m__E000.Read((int)bits);
			num = (ushort)(num + (ushort)num3);
			if (num3 < 15)
			{
				indices.Add(num);
			}
		}
	}

	public void SampleAtIndexFast(int index, _E4A0<ushort> indices)
	{
		PerfectCullingVolumeBakeData.m__E000.Reset(data[index].compressed);
		ushort num = 0;
		int numDeltaValues = data[index].numDeltaValues;
		for (int i = 0; i < numDeltaValues; i++)
		{
			uint num2 = PerfectCullingVolumeBakeData.m__E000.Read(2);
			uint bits = BITS[num2];
			uint num3 = PerfectCullingVolumeBakeData.m__E000.Read((int)bits);
			num = (ushort)(num + (ushort)num3);
			if (num3 < 15)
			{
				indices.Add(num);
			}
		}
	}

	public override void DrawInspectorGUI()
	{
	}

	[CompilerGenerated]
	internal static void _E001(int value, List<int> values)
	{
		while (value >= 15)
		{
			values.Add(15);
			value -= 15;
		}
		values.Add(value);
	}
}
