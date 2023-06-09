using System;
using UnityEngine;

namespace AnimationSystem.RootMotionTable;

public sealed class CharacterClipsKeeper : ScriptableObject
{
	[Serializable]
	public struct LayerData
	{
		public AnimationClipData[] clips;
	}

	[Serializable]
	public struct AnimationClipData
	{
		public AnimationClip clip;

		public RootMotionBlendTable.ParameterRelatedCurve[] parameterRelatedCurves;
	}

	public LayerData[] Clips;

	public int GetClipIndex(int layerIndex, AnimationClip clip)
	{
		AnimationClipData[] clips = Clips[layerIndex].clips;
		for (int i = 0; i < clips.Length; i++)
		{
			if (clips[i].clip.Equals(clip))
			{
				return i;
			}
		}
		return -1;
	}
}
