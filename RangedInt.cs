using System;
using UnityEngine;

[Serializable]
public class RangedInt
{
	[SerializeField]
	private int min = 1;

	[SerializeField]
	private int max = 10;

	public int Min => min;

	public int Max => max;

	public RangedInt(int min, int max)
	{
		this.min = min;
		this.max = max;
	}

	public int Random()
	{
		return UnityEngine.Random.Range(min, max);
	}

	public int Clamp(int input)
	{
		return Mathf.Clamp(input, min, max);
	}
}
