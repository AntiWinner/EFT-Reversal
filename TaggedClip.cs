using System;
using UnityEngine;

[Serializable]
public class TaggedClip
{
	public AudioClip Clip;

	public int Mask;

	public float Volume;

	public int Falloff;

	public int NetId;

	public float Length;

	[NonSerialized]
	public bool Exclude;
}
