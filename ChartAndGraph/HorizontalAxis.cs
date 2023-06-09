using System;
using System.Runtime.CompilerServices;

namespace ChartAndGraph;

public class HorizontalAxis : AxisBase
{
	protected override Action<IInternalUse, bool> Assign => delegate(IInternalUse x, bool clear)
	{
		if (clear)
		{
			if (x.HorizontalAxis == this)
			{
				x.HorizontalAxis = null;
			}
		}
		else if (x.HorizontalAxis != this)
		{
			x.HorizontalAxis = this;
		}
	};

	[CompilerGenerated]
	private void _E000(IInternalUse x, bool clear)
	{
		if (clear)
		{
			if (x.HorizontalAxis == this)
			{
				x.HorizontalAxis = null;
			}
		}
		else if (x.HorizontalAxis != this)
		{
			x.HorizontalAxis = this;
		}
	}
}
