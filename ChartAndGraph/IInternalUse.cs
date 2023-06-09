using System;
using UnityEngine;

namespace ChartAndGraph;

public interface IInternalUse
{
	CategoryLabels CategoryLabels { get; set; }

	ItemLabels ItemLabels { get; set; }

	GroupLabels GroupLabels { get; set; }

	HorizontalAxis HorizontalAxis { get; set; }

	VerticalAxis VerticalAxis { get; set; }

	Camera InternalTextCamera { get; }

	float InternalTextIdleDistance { get; }

	TextController InternalTextController { get; }

	float InternalTotalWidth { get; }

	float InternalTotalDepth { get; }

	float InternalTotalHeight { get; }

	bool InternalSupportsItemLabels { get; }

	bool InternalSupportsCategoryLables { get; }

	bool InternalSupportsGroupLabels { get; }

	bool HideHierarchy { get; }

	event Action Generated;

	bool InternalHasValues(AxisBase axis);

	double InternalMaxValue(AxisBase axis);

	double InternalMinValue(AxisBase axis);

	void InternalItemSelected(object userData);

	void InternalItemLeave(object userData);

	void InternalItemHovered(object userData);

	void CallOnValidate();
}
