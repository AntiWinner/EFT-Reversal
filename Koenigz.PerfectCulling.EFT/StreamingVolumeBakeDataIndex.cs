using System.IO;
using UnityEngine;

namespace Koenigz.PerfectCulling.EFT;

[PreferBinarySerialization]
public sealed class StreamingVolumeBakeDataIndex : ScriptableObject
{
	public string volumeGuid;

	public string groupGuid;

	public Vector3 cellCount;

	public Vector3 cellSize;

	public Vector3 originalCellSize;

	public Quaternion orientation;

	public bool bakeCompleted;

	public int bakeHash;

	public int bakeDataVersion;

	public VisibilitySetIndex[] indices;

	public const string StreamingIndexFilePostfix = "_index";

	public const string StreamingRawDataFilePostfix = "_cull";

	public const string PlayerBuildCullingDataFolder = "Culling_Data";

	public bool IsValid
	{
		get
		{
			if (bakeCompleted && bakeHash != 0 && !string.IsNullOrEmpty(volumeGuid))
			{
				return !string.IsNullOrEmpty(groupGuid);
			}
			return false;
		}
	}

	public unsafe int IndexSizeBytes => sizeof(VisibilitySetIndex) * indices.Length;

	public static string GetStreamingDataFilePath(PerfectCullingCrossSceneVolume vol, PerfectCullingCrossSceneVolume.BakeDescriptor desc, bool isIndexFile = false)
	{
		string path = Path.Combine(Application.streamingAssetsPath, _ED3E._E000(67121));
		if (isIndexFile)
		{
			return Path.Combine(path, vol.VolumeGuid + _ED3E._E000(48793) + desc.crossSceneGroup.ObjectGuidString + _ED3E._E000(67110));
		}
		return Path.Combine(path, vol.VolumeGuid + _ED3E._E000(48793) + desc.crossSceneGroup.ObjectGuidString + _ED3E._E000(67163));
	}

	public static StreamingVolumeBakeDataIndex Convert(PerfectCullingCrossSceneVolume vol, PerfectCullingCrossSceneVolume.BakeDescriptor desc, PerfectCullingVolumeBakeData data)
	{
		return null;
	}
}
