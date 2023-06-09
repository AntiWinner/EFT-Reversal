using System;

namespace ChartAndGraph;

[Serializable]
public struct AutoFloat
{
	public bool Automatic;

	public float Value;

	public AutoFloat(bool automatic, float value)
	{
		Automatic = automatic;
		Value = value;
	}

	public override bool Equals(object obj)
	{
		if (obj is AutoFloat autoFloat)
		{
			if (autoFloat.Automatic && Automatic)
			{
				return true;
			}
			if (!autoFloat.Automatic && !Automatic && autoFloat.Value == Value)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	public override int GetHashCode()
	{
		if (Automatic)
		{
			return Automatic.GetHashCode();
		}
		return Value.GetHashCode();
	}
}
