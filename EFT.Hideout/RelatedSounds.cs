using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EFT.Hideout;

[Serializable]
public sealed class RelatedSounds
{
	public Dictionary<EAreaActivityType, AudioClip> Data = new Dictionary<EAreaActivityType, AudioClip>();

	public AudioSequence WorkingSequence;

	[NonSerialized]
	public Dictionary<EAreaActivityType, AudioClip> FallbackSounds;

	public List<AudioClip> GetSounds(params EAreaActivityType[] soundTypes)
	{
		return soundTypes.Select(GetSound).ToList();
	}

	public AudioClip GetSound(EAreaActivityType soundType)
	{
		if (Data.TryGetValue(soundType, out var value))
		{
			return value;
		}
		if (!FallbackSounds.TryGetValue(soundType, out var value2))
		{
			return null;
		}
		return value2;
	}
}
