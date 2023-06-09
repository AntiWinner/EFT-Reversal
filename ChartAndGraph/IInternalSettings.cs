using System;

namespace ChartAndGraph;

public interface IInternalSettings
{
	event EventHandler InternalOnDataUpdate;

	event EventHandler InternalOnDataChanged;
}
