using System.Collections.Generic;
using UnityEngine;

namespace Koenigz.PerfectCulling.EFT;

public sealed class CrossSceneCullingTreePreProcess : PerfectCullingCrossSceneGroupPreProcess
{
	[SerializeField]
	[Header("Content setup")]
	private int _bakeHash;

	[SerializeField]
	private int _numBakeGroups;

	[SerializeField]
	private GuidReference[] _cullingGroups;

	public override int GetBakeHash()
	{
		return _bakeHash;
	}

	public override PerfectCullingBakeGroup[] PrepareRuntimeContent()
	{
		return GetBakeGroups();
	}

	public PerfectCullingBakeGroup[] GetBakeGroups()
	{
		List<PerfectCullingCrossSceneContentTrees> list = new List<PerfectCullingCrossSceneContentTrees>();
		GuidReference[] cullingGroups = _cullingGroups;
		foreach (GuidReference guidReference in cullingGroups)
		{
			list.Add(guidReference.gameObject.GetComponent<PerfectCullingCrossSceneContentTrees>());
		}
		list.Sort((PerfectCullingCrossSceneContentTrees x, PerfectCullingCrossSceneContentTrees y) => (x.ContentGroupId >= y.ContentGroupId) ? 1 : (-1));
		List<PerfectCullingBakeGroup> list2 = new List<PerfectCullingBakeGroup>();
		_numBakeGroups = 0;
		foreach (PerfectCullingCrossSceneContentTrees item in list)
		{
			list2.AddRange(item.BakeGroups);
			_numBakeGroups += item.BakeGroups.Length;
		}
		return list2.ToArray();
	}
}
