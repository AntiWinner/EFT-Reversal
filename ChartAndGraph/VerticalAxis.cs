using System;
using System.Runtime.CompilerServices;

namespace ChartAndGraph;

public class VerticalAxis : AxisBase
{
	protected override Action<IInternalUse, bool> Assign => delegate(IInternalUse x, bool clear)
	{
		if (clear)
		{
			if (x.VerticalAxis == this)
			{
				x.VerticalAxis = null;
			}
		}
		else if (x.VerticalAxis != this)
		{
			x.VerticalAxis = this;
		}
	};

	[CompilerGenerated]
	private void _E000(IInternalUse x, bool clear)
	{
		if (clear)
		{
			if (x.VerticalAxis == this)
			{
				x.VerticalAxis = null;
			}
		}
		else if (x.VerticalAxis != this)
		{
			x.VerticalAxis = this;
		}
	}
}
