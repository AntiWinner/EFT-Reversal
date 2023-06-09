using System;
using EFT.Utilities;

namespace EFT.Hideout;

[Serializable]
public sealed class Pattern<T>
{
	public T Value;

	public float Probability;

	public RandomBetweenFloats Delay;

	public Pattern()
	{
		Probability = 100f;
		Delay = new RandomBetweenFloats(0f, 0f);
	}
}
