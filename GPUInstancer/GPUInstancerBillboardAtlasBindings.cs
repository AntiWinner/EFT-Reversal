using System.Collections.Generic;
using UnityEngine;

namespace GPUInstancer;

public class GPUInstancerBillboardAtlasBindings : ScriptableObject
{
	public List<BillboardAtlasBinding> billboardAtlasBindings;

	public BillboardAtlasBinding GetBillboardAtlasBinding(GameObject prefab, int atlasResolution, int frameCount)
	{
		foreach (BillboardAtlasBinding billboardAtlasBinding in billboardAtlasBindings)
		{
			if (!(billboardAtlasBinding.prefab == prefab) || billboardAtlasBinding.atlasResolution != atlasResolution || billboardAtlasBinding.frameCount != frameCount)
			{
				continue;
			}
			return billboardAtlasBinding;
		}
		return null;
	}

	public void ResetBillboardAtlases()
	{
		if (billboardAtlasBindings == null)
		{
			billboardAtlasBindings = new List<BillboardAtlasBinding>();
		}
		else
		{
			billboardAtlasBindings.Clear();
		}
	}

	public void ClearEmptyBillboardAtlases()
	{
		_ = billboardAtlasBindings;
	}

	public void AddBillboardAtlas(GameObject prefab, int atlasResolution, int frameCount, Texture2D albedoAtlasTexture, Texture2D normalAtlasTexture, float quadSize, float yPivotOffset)
	{
		billboardAtlasBindings.Add(new BillboardAtlasBinding(prefab, atlasResolution, frameCount, albedoAtlasTexture, normalAtlasTexture, quadSize, yPivotOffset));
	}

	public void RemoveBillboardAtlas(BillboardAtlasBinding billboardAtlasBinding)
	{
		billboardAtlasBindings.Remove(billboardAtlasBinding);
	}

	public bool IsBillboardAtlasExists(GameObject prefab)
	{
		foreach (BillboardAtlasBinding billboardAtlasBinding in billboardAtlasBindings)
		{
			if (billboardAtlasBinding.prefab == prefab)
			{
				return true;
			}
		}
		return false;
	}

	public bool DeleteBillboardTextures(GPUInstancerPrototype selectedPrototype)
	{
		return false;
	}
}
