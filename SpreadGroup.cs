using System;

[Serializable]
public class SpreadGroup
{
	public int Falloff = 50;

	public float Volume = 1f;

	public TaggedClip[] Clips = Array.Empty<TaggedClip>();
}
