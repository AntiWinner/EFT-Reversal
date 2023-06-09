using System;
using System.Collections.Generic;

namespace ChartAndGraph;

internal interface IInternalGraphData
{
	int TotalCategories { get; }

	IEnumerable<GraphData.CategoryData> Categories { get; }

	event EventHandler InternalDataChanged;

	event EventHandler InternalRealTimeDataChanged;

	double GetMinValue(int axis, bool dataValue);

	double GetMaxValue(int axis, bool dataValue);

	void OnBeforeSerialize();

	void OnAfterDeserialize();

	void Update();
}
