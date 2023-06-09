using System;
using System.Collections.Generic;
using UnityEngine;

namespace Koenigz.PerfectCulling.EFT;

[DisallowMultipleComponent]
public sealed class CrossSceneCullingGroupPreProcess : PerfectCullingCrossSceneGroupPreProcess
{
	[SerializeField]
	private int _numBakeGroups;

	[SerializeField]
	private GuidReference[] _cullingGroups;

	[SerializeField]
	private int _bakeHash;

	public GuidReference[] CullingGroups => _cullingGroups;

	internal void _E000(GuidReference[] newCullingGroups)
	{
		_cullingGroups = newCullingGroups;
	}

	public override int GetBakeHash()
	{
		return _bakeHash;
	}

	public override void OnEndContentCollect()
	{
		_E001();
	}

	public override PerfectCullingBakeGroup[] CollectBakeGroups()
	{
		return Array.Empty<PerfectCullingBakeGroup>();
	}

	public override BakeBatch[] CreateBakeBatches(PerfectCullingBakeGroup[] inputGroups)
	{
		return Array.Empty<BakeBatch>();
	}

	public override BakeBatch[] GetBakeBatches()
	{
		BakeBatch bakeBatch = new BakeBatch();
		bakeBatch.Groups = GetBakeGroups();
		return new BakeBatch[1] { bakeBatch };
	}

	public override PerfectCullingBakeGroup[] PrepareRuntimeContent()
	{
		GuidReference[] cullingGroups = _cullingGroups;
		foreach (GuidReference obj in cullingGroups)
		{
			PerfectCullingCrossSceneGroup component = GetComponent<PerfectCullingCrossSceneGroup>();
			if (component == null)
			{
				_E4BB.Throw(_ED3E._E000(66959));
			}
			obj.GetComponent<PerfectCullingCrossSceneContentMeshes>().RuntimeCullingGroup = component;
		}
		return GetBakeGroups();
	}

	public override LODGroup[] GetLODGroups()
	{
		List<LODGroup> list = new List<LODGroup>();
		GuidReference[] cullingGroups = _cullingGroups;
		foreach (GuidReference guidReference in cullingGroups)
		{
			list.AddRange(guidReference.gameObject.GetComponentsInChildren<LODGroup>());
		}
		return list.ToArray();
	}

	private void _E001()
	{
	}

	public PerfectCullingBakeGroup[] GetBakeGroups()
	{
		List<PerfectCullingCrossSceneContentMeshes> list = new List<PerfectCullingCrossSceneContentMeshes>();
		GuidReference[] cullingGroups = _cullingGroups;
		foreach (GuidReference guidReference in cullingGroups)
		{
			list.Add(guidReference.gameObject.GetComponent<PerfectCullingCrossSceneContentMeshes>());
		}
		list.Sort((PerfectCullingCrossSceneContentMeshes x, PerfectCullingCrossSceneContentMeshes y) => (x.ContentGroupId >= y.ContentGroupId) ? 1 : (-1));
		List<PerfectCullingBakeGroup> list2 = new List<PerfectCullingBakeGroup>();
		_numBakeGroups = 0;
		foreach (PerfectCullingCrossSceneContentMeshes item in list)
		{
			list2.AddRange(item.BakeGroups);
			_numBakeGroups += item.BakeGroups.Length;
		}
		return list2.ToArray();
	}

	public override SharedOccluder GenerateSharedOccluder()
	{
		return _E4AE.GenerateSharedOccluder(null, base.Group, (IReadOnlyCollection<PerfectCullingBakeGroup>)(object)GetBakeGroups());
	}
}
