using System;
using System.Collections.Generic;
using uLipSync;
using UnityEngine;

namespace Cutscene;

[Serializable]
public class AnimationTrack
{
	public BakedData lipSyncBackedData;

	public LipSyncBackedDataRandomVariants lipSyncBackedDataRandomRange;

	private List<BakedData> randomLipSyncData;

	private BakedData selectedLipSyncData;

	public bool HaveData
	{
		get
		{
			if (lipSyncBackedData != null || lipSyncBackedDataRandomRange != null)
			{
				return true;
			}
			return false;
		}
	}

	public float duration
	{
		get
		{
			if (selectedLipSyncData != null)
			{
				return selectedLipSyncData.duration;
			}
			if (lipSyncBackedData != null)
			{
				return lipSyncBackedData.duration;
			}
			if (lipSyncBackedDataRandomRange != null)
			{
				return lipSyncBackedDataRandomRange.BakedData[0].duration;
			}
			return 0f;
		}
	}

	public string hint
	{
		get
		{
			if (lipSyncBackedData != null)
			{
				return _ED3E._E000(71374) + lipSyncBackedData.name;
			}
			if (lipSyncBackedDataRandomRange != null)
			{
				return _ED3E._E000(71370) + lipSyncBackedDataRandomRange.name;
			}
			return _ED3E._E000(71383);
		}
	}

	public BakedData GetBackedDataForPlay()
	{
		if (lipSyncBackedData != null)
		{
			return lipSyncBackedData;
		}
		if (lipSyncBackedDataRandomRange != null)
		{
			if (randomLipSyncData == null || randomLipSyncData.Count == 0)
			{
				randomLipSyncData = new List<BakedData>(lipSyncBackedDataRandomRange.BakedData);
			}
			selectedLipSyncData = randomLipSyncData[UnityEngine.Random.Range(0, randomLipSyncData.Count)];
			if (randomLipSyncData.Count == 1)
			{
				randomLipSyncData = new List<BakedData>(lipSyncBackedDataRandomRange.BakedData);
			}
			randomLipSyncData.Remove(selectedLipSyncData);
			return selectedLipSyncData;
		}
		return null;
	}
}
