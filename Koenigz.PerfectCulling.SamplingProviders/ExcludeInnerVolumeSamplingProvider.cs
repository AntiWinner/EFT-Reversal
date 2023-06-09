using System.Collections.Generic;
using Koenigz.PerfectCulling.EFT;
using UnityEngine;

namespace Koenigz.PerfectCulling.SamplingProviders;

[DisallowMultipleComponent]
[RequireComponent(typeof(PerfectCullingBakingBehaviour))]
[ExecuteAlways]
public sealed class ExcludeInnerVolumeSamplingProvider : SamplingProviderBase
{
	[SerializeField]
	private PerfectCullingCrossSceneVolume[] _innerCrossVolumes;

	public override string Name => _ED3E._E000(66733);

	public override void InitializeSamplingProvider()
	{
	}

	public void RefreshInnerVolumes()
	{
		List<PerfectCullingCrossSceneVolume> list = new List<PerfectCullingCrossSceneVolume>();
		PerfectCullingCrossSceneVolume component = GetComponent<PerfectCullingCrossSceneVolume>();
		PerfectCullingCrossSceneVolume[] componentsInChildren = base.transform.GetComponentsInChildren<PerfectCullingCrossSceneVolume>();
		foreach (PerfectCullingCrossSceneVolume perfectCullingCrossSceneVolume in componentsInChildren)
		{
			if (!(perfectCullingCrossSceneVolume == component))
			{
				list.Add(perfectCullingCrossSceneVolume);
			}
		}
		_innerCrossVolumes = list.ToArray();
	}

	public override bool IsSamplingPositionActive(PerfectCullingBakingBehaviour bakingBehaviour, Vector3 pos)
	{
		return true;
	}

	public bool IsSamplingPositionActiveMT(PerfectCullingBakingBehaviour bakingBehaviour, Vector3 pos)
	{
		PerfectCullingCrossSceneVolume[] innerCrossVolumes = _innerCrossVolumes;
		for (int i = 0; i < innerCrossVolumes.Length; i++)
		{
			if (innerCrossVolumes[i].runtimeVolumeBounds.Contains(pos))
			{
				return false;
			}
		}
		return true;
	}
}
