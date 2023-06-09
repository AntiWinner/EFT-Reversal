using System;

namespace ChartAndGraph;

[Serializable]
public class ChartOrientedSize
{
	public float Breadth;

	public float Depth;

	public ChartOrientedSize()
	{
	}

	public ChartOrientedSize(float breadth)
	{
		Breadth = breadth;
		Depth = 0f;
	}

	public ChartOrientedSize(float breadth, float depth)
	{
		Breadth = breadth;
		Depth = depth;
	}

	public override bool Equals(object obj)
	{
		if (obj is ChartOrientedSize)
		{
			ChartOrientedSize chartOrientedSize = (ChartOrientedSize)obj;
			if (chartOrientedSize.Depth == Depth && chartOrientedSize.Breadth == Breadth)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return Breadth.GetHashCode() ^ Depth.GetHashCode();
	}
}
