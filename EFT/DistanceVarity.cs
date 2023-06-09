using System;
using UnityEngine;

namespace EFT;

[Serializable]
public class DistanceVarity
{
	public AudioClip[] Clips = new AudioClip[1];

	public AudioClip this[int i]
	{
		get
		{
			return Clips[i];
		}
		set
		{
			Clips[i] = value;
		}
	}

	public int Length => Clips.Length;
}
