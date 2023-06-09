using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace EFT.InputSystem;

[Serializable]
public sealed class AxisGroup
{
	[Serializable]
	public sealed class AxisPair
	{
		public InputSource positive;

		public InputSource negative;

		public AxisPair Clone()
		{
			return new AxisPair
			{
				positive = positive.Clone(),
				negative = negative.Clone()
			};
		}
	}

	public EAxis axisName;

	public List<AxisPair> pairs = new List<AxisPair>();

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public bool isInverted;

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public float gravity;

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public bool snapToZero;

	public AxisGroup Clone()
	{
		return new AxisGroup
		{
			axisName = axisName,
			pairs = pairs.ConvertAll((AxisPair x) => x.Clone()),
			isInverted = isInverted,
			gravity = gravity,
			snapToZero = snapToZero
		};
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		if (obj is AxisGroup other)
		{
			return Equals(other);
		}
		return false;
	}

	public bool Equals(AxisGroup other)
	{
		if (axisName != other.axisName || pairs.Count != other.pairs.Count || isInverted != other.isInverted || !(Math.Abs(gravity - other.gravity) <= float.Epsilon) || snapToZero != other.snapToZero)
		{
			return false;
		}
		for (int i = 0; i < pairs.Count; i++)
		{
			if (!pairs[i].positive.Equals(other.pairs[i].positive) || !pairs[i].negative.Equals(other.pairs[i].negative))
			{
				return false;
			}
		}
		return true;
	}

	public static bool EqualityCheck(AxisGroup x, AxisGroup y)
	{
		return x.Equals(y);
	}

	public static AxisGroup CopyItem(AxisGroup item)
	{
		return item.Clone();
	}
}
