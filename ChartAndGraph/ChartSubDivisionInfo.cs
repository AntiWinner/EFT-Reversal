using System;
using UnityEngine;

namespace ChartAndGraph;

[Serializable]
internal class ChartSubDivisionInfo : ChartDivisionInfo
{
	protected override float ValidateTotal(float total)
	{
		return Mathf.Round(total);
	}
}
