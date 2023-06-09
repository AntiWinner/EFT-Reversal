using System;
using UnityEngine;

namespace EFT.Utilities;

[Serializable]
public class RandomBetweenFloats
{
	public float From;

	public float To;

	public float Result => UnityEngine.Random.Range(From, To);

	public RandomBetweenFloats(float from, float to)
	{
		From = from;
		To = to;
	}
}
