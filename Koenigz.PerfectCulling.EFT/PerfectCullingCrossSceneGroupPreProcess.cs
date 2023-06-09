using System;
using System.Collections.Generic;
using UnityEngine;

namespace Koenigz.PerfectCulling.EFT;

[DisallowMultipleComponent]
[RequireComponent(typeof(PerfectCullingCrossSceneGroup))]
public abstract class PerfectCullingCrossSceneGroupPreProcess : MonoBehaviour
{
	public enum RenderMode
	{
		TriangleFill,
		Wireframe
	}

	public PerfectCullingCrossSceneGroup Group => GetComponent<PerfectCullingCrossSceneGroup>();

	public virtual int GetBakeResolution()
	{
		return PerfectCullingSettings.Instance.bakeCameraResolutionWidth;
	}

	public virtual RenderMode GetGroupRenderMode()
	{
		return RenderMode.TriangleFill;
	}

	public virtual PerfectCullingBakeGroup[] CollectBakeGroups()
	{
		return Array.Empty<PerfectCullingBakeGroup>();
	}

	public virtual BakeBatch[] CreateBakeBatches(PerfectCullingBakeGroup[] inputGroups)
	{
		return new BakeBatch[1]
		{
			new BakeBatch
			{
				Groups = inputGroups
			}
		};
	}

	public virtual PerfectCullingBakeGroup[] PrebakeTransform(BakeBatch batch, ICollection<GameObject> tempObjects, out PerfectCullingBakingBehaviour._E001.Mode writeMode)
	{
		writeMode = PerfectCullingBakingBehaviour._E001.Mode.Overwrite;
		return batch.Groups;
	}

	public virtual int GetBakeHash()
	{
		return Group.GetBakeHashDefault();
	}

	public virtual BakeBatch[] GetBakeBatches()
	{
		return Group.bakeBatches;
	}

	public virtual void OnBeginContentCollect()
	{
	}

	public virtual void OnEndContentCollect()
	{
	}

	public virtual PerfectCullingBakeGroup[] PrepareRuntimeContent()
	{
		return Group.bakeGroups;
	}

	public virtual void RemapBatchResult(BakeBatch sourceBatch, PerfectCullingBakingBehaviour._E001 task)
	{
	}

	public virtual SharedOccluder GenerateSharedOccluder()
	{
		return _E4AE.GenerateSharedOccluder(null, Group);
	}

	public virtual LODGroup[] GetLODGroups()
	{
		return Group.GroupRoot.GetComponentsInChildren<LODGroup>();
	}
}
