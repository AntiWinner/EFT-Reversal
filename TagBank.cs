using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TagBank : ScriptableObject
{
	public EPhraseTrigger Trigger;

	public SpreadGroup[] SpreadGroups = Array.Empty<SpreadGroup>();

	public TaggedClip[] Clips;

	public Chain ChainEvent;

	public static int[] Sizes = new int[6] { 3, 2, 3, 2, 4, 3 };

	public int Importance;

	public float Blocker;

	public bool IgnoreTags;

	private List<TaggedClip> _E000;

	private List<TaggedClip> _E001;

	public TaggedClip Match(ETagStatus combat = ETagStatus.Unaware | ETagStatus.Aware | ETagStatus.Combat, ETagStatus speakerGroup = ETagStatus.Solo | ETagStatus.Coop, ETagStatus targetGroup = ETagStatus.TargetSolo | ETagStatus.TargetMultiple, ETagStatus health = ETagStatus.Healthy | ETagStatus.Injured | ETagStatus.BadlyInjured | ETagStatus.Dying, ETagStatus side = ETagStatus.Bear | ETagStatus.Usec | ETagStatus.Scav, ETagStatus exUsecBoss = ETagStatus.Birdeye | ETagStatus.Knight | ETagStatus.BigPipe)
	{
		return Match((int)(combat | speakerGroup | targetGroup | health | side | exUsecBoss));
	}

	public TaggedClip Match(int mask)
	{
		if (_E001 == null)
		{
			_E001 = new List<TaggedClip>(Clips.Length);
			_E000 = new List<TaggedClip>(Clips.Length);
		}
		else
		{
			_E001.Clear();
			_E000.Clear();
		}
		List<TaggedClip> list = _E001;
		List<TaggedClip> list2 = _E000;
		for (int i = 0; i < Clips.Length; i++)
		{
			TaggedClip taggedClip = Clips[i];
			if (IgnoreTags || Compare(taggedClip.Mask, mask))
			{
				if (taggedClip.Exclude)
				{
					list2.Add(taggedClip);
				}
				else
				{
					list.Add(taggedClip);
				}
			}
		}
		if (list.Count == 1)
		{
			for (int j = 0; j < list2.Count; j++)
			{
				list2[j].Exclude = false;
			}
		}
		else if (list.Count == 0)
		{
			for (int k = 0; k < list2.Count; k++)
			{
				list2[k].Exclude = false;
			}
			list = list2;
		}
		if (list.Count == 0)
		{
			return null;
		}
		int index = UnityEngine.Random.Range(0, list.Count);
		TaggedClip taggedClip2 = list[index];
		taggedClip2.Exclude = true;
		_E000.Clear();
		_E001.Clear();
		return taggedClip2;
	}

	public void OnValidate()
	{
		Clips = SpreadGroups.SelectMany((SpreadGroup g) => g.Clips).ToArray();
		for (int i = 0; i < Clips.Length; i++)
		{
			Clips[i].NetId = i;
			if (Clips[i].Clip != null)
			{
				Clips[i].Length = Clips[i].Clip.length;
			}
		}
	}

	public static bool Compare(int mask1, int @event)
	{
		int num = 0;
		for (int i = 0; i < Sizes.Length; i++)
		{
			int width = Sizes[i];
			if (i > 0)
			{
				num += Sizes[i - 1];
			}
			int bits = GetBits(mask1, num, width);
			int bits2 = GetBits(@event, num, width);
			if (bits != 0 && bits2 != 0 && (bits & bits2) == 0)
			{
				return false;
			}
		}
		return true;
	}

	public static int GetBits(int mask, int offset, int width)
	{
		return ((1 << width) - 1 << offset) & mask;
	}
}
