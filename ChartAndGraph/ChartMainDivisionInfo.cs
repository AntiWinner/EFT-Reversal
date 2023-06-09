using System;

namespace ChartAndGraph;

[Serializable]
internal class ChartMainDivisionInfo : ChartDivisionInfo
{
	public DivisionMessure Messure
	{
		get
		{
			return messure;
		}
		set
		{
			messure = value;
			RaiseOnChanged();
		}
	}

	public float UnitsPerDivision
	{
		get
		{
			return unitsPerDivision;
		}
		set
		{
			unitsPerDivision = value;
			RaiseOnChanged();
		}
	}
}
