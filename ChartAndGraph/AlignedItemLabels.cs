using UnityEngine;

namespace ChartAndGraph;

public abstract class AlignedItemLabels : ItemLabelsBase
{
	[SerializeField]
	[Tooltip("Select the alignment of the label relative to the item")]
	private ChartLabelAlignment alignment;

	public ChartLabelAlignment Alignment
	{
		get
		{
			return alignment;
		}
		set
		{
			alignment = value;
			RaiseOnUpdate();
		}
	}
}
