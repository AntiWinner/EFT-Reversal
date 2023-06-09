using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace ChartAndGraph;

[Serializable]
public class GraphData : IInternalGraphData
{
	[Serializable]
	public class CategoryData
	{
		public string Name;

		public bool IsBezierCurve;

		public int SegmentsPerCurve = 10;

		public List<_ED18> mTmpCurveData = new List<_ED18>();

		public List<_ED18> Data = new List<_ED18>();

		public bool Regenerate = true;

		public double? MaxX;

		public double? MaxY;

		public double? MinX;

		public double? MinY;

		public double? MaxRadius;

		public ChartItemEffect LineHoverPrefab;

		public ChartItemEffect PointHoverPrefab;

		public Material LineMaterial;

		public MaterialTiling LineTiling;

		public double LineThickness = 1.0;

		public Material FillMaterial;

		public bool StetchFill;

		public Material PointMaterial;

		public double PointSize = 5.0;

		public PathGenerator LinePrefab;

		public FillPathGenerator FillPrefab;

		public GameObject DotPrefab;

		public double Depth;

		public List<_ED18> getPoints()
		{
			if (!IsBezierCurve)
			{
				return Data;
			}
			if (!Regenerate)
			{
				return mTmpCurveData;
			}
			Regenerate = false;
			mTmpCurveData.Clear();
			if (Data.Count <= 0)
			{
				return mTmpCurveData;
			}
			mTmpCurveData.Add(Data[0]);
			if (Data.Count < 4)
			{
				return mTmpCurveData;
			}
			int num = Data.Count - 1;
			for (int i = 0; i < num; i += 3)
			{
				AddInnerCurve(Data[i], Data[i + 1], Data[i + 2], Data[i + 3]);
				mTmpCurveData.Add(Data[i + 3]);
			}
			return mTmpCurveData;
		}

		public void AddInnerCurve(_ED18 p1, _ED18 c1, _ED18 c2, _ED18 p2)
		{
			for (int i = 0; i < SegmentsPerCurve; i++)
			{
				double num = (double)i / (double)SegmentsPerCurve;
				double num2 = 1.0 - num;
				_ED18 obj = num2 * num2 * num2 * p1 + 3.0 * num2 * num2 * num * c1 + 3.0 * num * num * num2 * c2 + num * num * num * p2;
				mTmpCurveData.Add(new _ED18(obj.x, obj.y, 0.0));
			}
		}
	}

	public class _E000 : IComparer<_ED18>
	{
		public int Compare(_ED18 x, _ED18 y)
		{
			if (x.x < y.x)
			{
				return -1;
			}
			if (!(x.x > y.x))
			{
				return 0;
			}
			return 1;
		}
	}

	[Serializable]
	public class SerializedCategory
	{
		[_ED13]
		public double Depth;

		[_ED13]
		public PathGenerator LinePrefab;

		[_ED13]
		public FillPathGenerator FillPrefab;

		[_ED13]
		public GameObject DotPrefab;

		[HideInInspector]
		public _ED18[] data;

		[HideInInspector]
		public double? MaxX;

		[HideInInspector]
		public double? MaxY;

		[HideInInspector]
		public double? MinX;

		[HideInInspector]
		public double? MinY;

		[HideInInspector]
		public double? MaxRadius;

		public string Name;

		public bool IsBezierCurve;

		public int SegmentsPerCurve = 10;

		public ChartItemEffect LineHoverPrefab;

		public ChartItemEffect PointHoverPrefab;

		public Material Material;

		public MaterialTiling LineTiling;

		public Material InnerFill;

		public double LineThickness = 1.0;

		public bool StetchFill;

		public Material PointMaterial;

		public double PointSize;
	}

	private class _E001
	{
		public string category;

		public int from;

		public _ED18 To;

		public _ED18 current;

		public int index;

		public double startTime;

		public double totalTime;
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public string category;

		internal bool _E000(_E001 x)
		{
			return x.category == category;
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public string category;

		internal bool _E000(_E001 x)
		{
			return x.category == category;
		}
	}

	[CompilerGenerated]
	private EventHandler DataChanged;

	[CompilerGenerated]
	private EventHandler RealtimeDataChanged;

	private bool _suspendEvents;

	private bool _sliderUpdated;

	private List<_ED18> mTmpDriv = new List<_ED18>();

	[SerializeField]
	private double automaticVerticalViewGap;

	[SerializeField]
	private bool automaticVerticallView = true;

	[SerializeField]
	private double automaticcHorizontaViewGap;

	[SerializeField]
	private bool automaticHorizontalView = true;

	[SerializeField]
	private double horizontalViewSize = 100.0;

	[SerializeField]
	private double horizontalViewOrigin;

	[SerializeField]
	private double verticalViewSize = 100.0;

	[SerializeField]
	private double verticalViewOrigin;

	private List<_E001> _sliders = new List<_E001>();

	private _E000 _comparer = new _E000();

	private Dictionary<string, CategoryData> _categoryData = new Dictionary<string, CategoryData>();

	[SerializeField]
	private SerializedCategory[] mSerializedData = new SerializedCategory[0];

	public double AutomaticVerticallViewGap
	{
		get
		{
			return automaticVerticalViewGap;
		}
		set
		{
			automaticVerticalViewGap = value;
			RestoreDataValues();
			_E003();
		}
	}

	public bool AutomaticVerticallView
	{
		get
		{
			return automaticVerticallView;
		}
		set
		{
			automaticVerticallView = value;
			RestoreDataValues();
			_E003();
		}
	}

	public double AutomaticcHorizontaViewGap
	{
		get
		{
			return automaticcHorizontaViewGap;
		}
		set
		{
			automaticcHorizontaViewGap = value;
			RestoreDataValues();
			_E003();
		}
	}

	public bool AutomaticHorizontalView
	{
		get
		{
			return automaticHorizontalView;
		}
		set
		{
			automaticHorizontalView = value;
			RestoreDataValues();
			_E003();
		}
	}

	public double HorizontalViewSize
	{
		get
		{
			return horizontalViewSize;
		}
		set
		{
			horizontalViewSize = value;
			_E003();
		}
	}

	public double HorizontalViewOrigin
	{
		get
		{
			return horizontalViewOrigin;
		}
		set
		{
			horizontalViewOrigin = value;
			_E003();
		}
	}

	public double VerticalViewSize
	{
		get
		{
			return verticalViewSize;
		}
		set
		{
			verticalViewSize = value;
			_E003();
		}
	}

	public double VerticalViewOrigin
	{
		get
		{
			return verticalViewOrigin;
		}
		set
		{
			verticalViewOrigin = value;
			_E003();
		}
	}

	int IInternalGraphData.TotalCategories => _categoryData.Count;

	IEnumerable<CategoryData> IInternalGraphData.Categories => _categoryData.Values;

	private event EventHandler _E000
	{
		[CompilerGenerated]
		add
		{
			EventHandler eventHandler = DataChanged;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref DataChanged, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			EventHandler eventHandler = DataChanged;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref DataChanged, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	private event EventHandler _E001
	{
		[CompilerGenerated]
		add
		{
			EventHandler eventHandler = RealtimeDataChanged;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref RealtimeDataChanged, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			EventHandler eventHandler = RealtimeDataChanged;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref RealtimeDataChanged, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	event EventHandler IInternalGraphData.InternalRealTimeDataChanged
	{
		add
		{
			this._E001 += value;
		}
		remove
		{
			this._E001 -= value;
		}
	}

	event EventHandler IInternalGraphData.InternalDataChanged
	{
		add
		{
			this._E000 += value;
		}
		remove
		{
			this._E000 -= value;
		}
	}

	public void RestoreDataValues()
	{
		if (automaticHorizontalView)
		{
			_E000(0);
		}
		if (AutomaticVerticallView)
		{
			_E000(1);
		}
	}

	private void _E000(int axis)
	{
		if (axis == 0)
		{
			double maxValue = ((IInternalGraphData)this).GetMaxValue(0, dataValue: true);
			horizontalViewSize = maxValue - (horizontalViewOrigin = ((IInternalGraphData)this).GetMinValue(0, dataValue: true));
		}
		else
		{
			double maxValue2 = ((IInternalGraphData)this).GetMaxValue(1, dataValue: true);
			verticalViewSize = maxValue2 - (verticalViewOrigin = ((IInternalGraphData)this).GetMinValue(1, dataValue: true));
		}
	}

	private static void _E001(CategoryData data, _ED18 point)
	{
		if (!data.MaxRadius.HasValue || data.MaxRadius.Value < point.z)
		{
			data.MaxRadius = point.z;
		}
		if (!data.MaxX.HasValue || data.MaxX.Value < point.x)
		{
			data.MaxX = point.x;
		}
		if (!data.MinX.HasValue || data.MinX.Value > point.x)
		{
			data.MinX = point.x;
		}
		if (!data.MaxY.HasValue || data.MaxY.Value < point.y)
		{
			data.MaxY = point.y;
		}
		if (!data.MinY.HasValue || data.MinY.Value > point.y)
		{
			data.MinY = point.y;
		}
	}

	public void Update()
	{
		_sliderUpdated = false;
		_sliders.RemoveAll(delegate(_E001 x)
		{
			if (!_categoryData.TryGetValue(x.category, out var value))
			{
				return true;
			}
			if (value.IsBezierCurve)
			{
				return false;
			}
			List<_ED18> data = value.Data;
			if (x.from >= data.Count || x.index >= data.Count)
			{
				return true;
			}
			_ED18 a = data[x.from];
			_ED18 to = x.To;
			double num = Time.time;
			num -= x.startTime;
			num = ((!(x.totalTime <= 9.999999747378752E-05)) ? (num / x.totalTime) : 1.0);
			_ED18 obj = (x.current = _ED18.Lerp(a, to, num));
			data[x.index] = obj;
			_sliderUpdated = true;
			if (num >= 1.0)
			{
				_E001(value, obj);
				return true;
			}
			return false;
		});
		if (_sliderUpdated)
		{
			_E002();
		}
	}

	private void _E002()
	{
		if (!_suspendEvents && RealtimeDataChanged != null)
		{
			RealtimeDataChanged(this, EventArgs.Empty);
		}
	}

	private void _E003()
	{
		if (!_suspendEvents && DataChanged != null)
		{
			DataChanged(this, EventArgs.Empty);
		}
	}

	public void StartBatch()
	{
		_suspendEvents = true;
	}

	public void EndBatch()
	{
		_suspendEvents = false;
		_E003();
	}

	public void ClearAndMakeBezierCurve(string category)
	{
		if (!_categoryData.ContainsKey(category))
		{
			Debug.LogWarning(_ED3E._E000(244073));
			return;
		}
		_categoryData[category].IsBezierCurve = true;
		ClearCategory(category);
	}

	public void ClearAndMakeLinear(string category)
	{
		if (!_categoryData.ContainsKey(category))
		{
			Debug.LogWarning(_ED3E._E000(244073));
			return;
		}
		_categoryData[category].IsBezierCurve = false;
		ClearCategory(category);
	}

	public void RenameCategory(string prevName, string newName)
	{
		if (!(prevName == newName))
		{
			if (_categoryData.ContainsKey(newName))
			{
				throw new ArgumentException(string.Format(_ED3E._E000(244135), newName));
			}
			CategoryData categoryData = _categoryData[prevName];
			_categoryData.Remove(prevName);
			categoryData.Name = newName;
			_categoryData.Add(newName, categoryData);
			_E003();
		}
	}

	public void AddCategory(string category, Material lineMaterial, double lineThickness, MaterialTiling lineTiling, Material innerFill, bool strechFill, Material pointMaterial, double pointSize)
	{
		if (_categoryData.ContainsKey(category))
		{
			throw new ArgumentException(string.Format(_ED3E._E000(244135), category));
		}
		CategoryData categoryData = new CategoryData();
		_categoryData.Add(category, categoryData);
		categoryData.Name = category;
		categoryData.LineMaterial = lineMaterial;
		categoryData.LineHoverPrefab = null;
		categoryData.PointHoverPrefab = null;
		categoryData.FillMaterial = innerFill;
		categoryData.LineThickness = lineThickness;
		categoryData.LineTiling = lineTiling;
		categoryData.StetchFill = strechFill;
		categoryData.PointMaterial = pointMaterial;
		categoryData.PointSize = pointSize;
		_E003();
	}

	public void Set2DCategoryPrefabs(string category, ChartItemEffect lineHover, ChartItemEffect pointHover)
	{
		if (!_categoryData.ContainsKey(category))
		{
			Debug.LogWarning(_ED3E._E000(244073));
			return;
		}
		CategoryData categoryData = _categoryData[category];
		categoryData.LineHoverPrefab = lineHover;
		categoryData.PointHoverPrefab = pointHover;
	}

	public void AddCategory3DGraph(string category, PathGenerator linePrefab, Material lineMaterial, double lineThickness, MaterialTiling lineTiling, FillPathGenerator fillPrefab, Material innerFill, bool strechFill, GameObject pointPrefab, Material pointMaterial, double pointSize, double depth, bool isCurve, int segmentsPerCurve)
	{
		if (_categoryData.ContainsKey(category))
		{
			throw new ArgumentException(string.Format(_ED3E._E000(244135), category));
		}
		if (depth < 0.0)
		{
			depth = 0.0;
		}
		CategoryData categoryData = new CategoryData();
		_categoryData.Add(category, categoryData);
		categoryData.Name = category;
		categoryData.LineMaterial = lineMaterial;
		categoryData.FillMaterial = innerFill;
		categoryData.LineThickness = lineThickness;
		categoryData.LineTiling = lineTiling;
		categoryData.StetchFill = strechFill;
		categoryData.PointMaterial = pointMaterial;
		categoryData.PointSize = pointSize;
		categoryData.LinePrefab = linePrefab;
		categoryData.FillPrefab = fillPrefab;
		categoryData.DotPrefab = pointPrefab;
		categoryData.Depth = depth;
		categoryData.IsBezierCurve = isCurve;
		categoryData.SegmentsPerCurve = segmentsPerCurve;
		_E003();
	}

	public void SetCategoryLine(string category, Material lineMaterial, double lineThickness, MaterialTiling lineTiling)
	{
		if (!_categoryData.ContainsKey(category))
		{
			Debug.LogWarning(_ED3E._E000(244073));
			return;
		}
		CategoryData categoryData = _categoryData[category];
		categoryData.LineMaterial = lineMaterial;
		categoryData.LineThickness = lineThickness;
		categoryData.LineTiling = lineTiling;
		_E003();
	}

	public void Clear()
	{
		_sliders.Clear();
		_categoryData.Clear();
	}

	public bool HasCategory(string category)
	{
		return _categoryData.ContainsKey(category);
	}

	public bool RemoveCategory(string category)
	{
		_sliders.RemoveAll((_E001 x) => x.category == category);
		return _categoryData.Remove(category);
	}

	public void SetCategoryPoint(string category, Material pointMaterial, double pointSize)
	{
		if (!_categoryData.ContainsKey(category))
		{
			Debug.LogWarning(_ED3E._E000(244073));
			return;
		}
		CategoryData categoryData = _categoryData[category];
		categoryData.PointMaterial = pointMaterial;
		categoryData.PointSize = pointSize;
		_E003();
	}

	public void Set3DCategoryPrefabs(string category, PathGenerator linePrefab, FillPathGenerator fillPrefab, GameObject dotPrefab)
	{
		if (!_categoryData.ContainsKey(category))
		{
			Debug.LogWarning(_ED3E._E000(244073));
			return;
		}
		CategoryData categoryData = _categoryData[category];
		categoryData.LinePrefab = linePrefab;
		categoryData.DotPrefab = dotPrefab;
		categoryData.FillPrefab = fillPrefab;
		_E003();
	}

	public void Set3DCategoryDepth(string category, double depth)
	{
		if (!_categoryData.ContainsKey(category))
		{
			Debug.LogWarning(_ED3E._E000(244073));
			return;
		}
		if (depth < 0.0)
		{
			depth = 0.0;
		}
		_categoryData[category].Depth = depth;
		_E003();
	}

	public void SetCategoryFill(string category, Material fillMaterial, bool strechFill)
	{
		if (!_categoryData.ContainsKey(category))
		{
			Debug.LogWarning(_ED3E._E000(244073));
			return;
		}
		CategoryData categoryData = _categoryData[category];
		categoryData.FillMaterial = fillMaterial;
		categoryData.StetchFill = strechFill;
		_E003();
	}

	public void ClearCategory(string category)
	{
		if (!_categoryData.ContainsKey(category))
		{
			Debug.LogWarning(_ED3E._E000(244073));
			return;
		}
		_sliders.RemoveAll((_E001 x) => x.category == category);
		_categoryData[category].MaxX = null;
		_categoryData[category].MaxY = null;
		_categoryData[category].MinX = null;
		_categoryData[category].MinY = null;
		_categoryData[category].MaxRadius = null;
		_categoryData[category].Data.Clear();
		_categoryData[category].Regenerate = true;
		_E003();
	}

	public void AddPointToCategory(string category, DateTime x, DateTime y, double pointSize = -1.0)
	{
		double num = _ED16.DateToValue(x);
		double num2 = _ED16.DateToValue(y);
		AddPointToCategory(category, num, num2, pointSize);
	}

	public _ED18 GetPoint(string category, int index)
	{
		List<_ED18> points = _categoryData[category].getPoints();
		if (points.Count == 0)
		{
			return _ED18.zero;
		}
		if (index < 0)
		{
			return points[0];
		}
		if (index < points.Count)
		{
			return points[index];
		}
		return points[points.Count - 1];
	}

	public void AddPointToCategory(string category, DateTime x, double y, double pointSize = -1.0)
	{
		double num = _ED16.DateToValue(x);
		AddPointToCategory(category, num, y, pointSize);
	}

	public void AddPointToCategory(string category, double x, DateTime y, double pointSize = -1.0)
	{
		double num = _ED16.DateToValue(y);
		AddPointToCategory(category, x, num, pointSize);
	}

	public void AddPointToCategoryRealtime(string category, DateTime x, DateTime y, double slideTime = 0.0, double pointSize = -1.0)
	{
		double num = _ED16.DateToValue(x);
		double num2 = _ED16.DateToValue(y);
		AddPointToCategoryRealtime(category, num, num2, slideTime, pointSize);
	}

	public void AddPointToCategoryRealtime(string category, DateTime x, double y, double slideTime = 0.0, double pointSize = -1.0)
	{
		double num = _ED16.DateToValue(x);
		AddPointToCategoryRealtime(category, num, y, slideTime, pointSize);
	}

	public void AddPointToCategoryRealtime(string category, double x, DateTime y, double slideTime = 0.0, double pointSize = -1.0)
	{
		double num = _ED16.DateToValue(y);
		AddPointToCategoryRealtime(category, x, num, slideTime, pointSize);
	}

	public void AddPointToCategoryRealtime(string category, double x, double y, double slideTime = 0.0, double pointSize = -1.0)
	{
		if (!_categoryData.ContainsKey(category))
		{
			Debug.LogWarning(_ED3E._E000(244073));
			return;
		}
		CategoryData categoryData = _categoryData[category];
		if (categoryData.IsBezierCurve)
		{
			Debug.LogWarning(_ED3E._E000(244163));
			return;
		}
		_ED18 obj = new _ED18(x, y, pointSize);
		List<_ED18> data = categoryData.Data;
		if (data.Count > 0 && data[data.Count - 1].x > obj.x)
		{
			Debug.LogWarning(_ED3E._E000(244229));
			return;
		}
		if (slideTime <= 0.0 || data.Count == 0)
		{
			data.Add(obj);
			_E001(categoryData, obj);
		}
		else
		{
			_E001 obj2 = new _E001
			{
				category = category,
				from = data.Count - 1,
				index = data.Count,
				startTime = Time.time,
				totalTime = slideTime,
				To = obj
			};
			_sliders.Add(obj2);
			obj2.current = data[data.Count - 1];
			data.Add(obj2.current);
		}
		_E002();
	}

	public void SetCurveInitialPoint(string category, DateTime x, double y, double pointSize = -1.0)
	{
		SetCurveInitialPoint(category, _ED16.DateToValue(x), y, pointSize);
	}

	public void SetCurveInitialPoint(string category, DateTime x, DateTime y, double pointSize = -1.0)
	{
		SetCurveInitialPoint(category, _ED16.DateToValue(x), _ED16.DateToValue(y), pointSize);
	}

	public void SetCurveInitialPoint(string category, double x, DateTime y, double pointSize = -1.0)
	{
		SetCurveInitialPoint(category, x, _ED16.DateToValue(y), pointSize);
	}

	public void SetCurveInitialPoint(string category, double x, double y, double pointSize = -1.0)
	{
		if (!_categoryData.ContainsKey(category))
		{
			Debug.LogWarning(_ED3E._E000(244073));
			return;
		}
		CategoryData categoryData = _categoryData[category];
		if (!categoryData.IsBezierCurve)
		{
			Debug.LogWarning(_ED3E._E000(244303));
			return;
		}
		if (categoryData.Data.Count > 0)
		{
			Debug.LogWarning(_ED3E._E000(244365));
			return;
		}
		categoryData.Regenerate = true;
		if (!categoryData.MaxRadius.HasValue || categoryData.MaxRadius.Value < pointSize)
		{
			categoryData.MaxRadius = pointSize;
		}
		if (!categoryData.MaxX.HasValue || categoryData.MaxX.Value < x)
		{
			categoryData.MaxX = x;
		}
		if (!categoryData.MinX.HasValue || categoryData.MinX.Value > x)
		{
			categoryData.MinX = x;
		}
		if (!categoryData.MaxY.HasValue || categoryData.MaxY.Value < y)
		{
			categoryData.MaxY = y;
		}
		if (!categoryData.MinY.HasValue || categoryData.MinY.Value > y)
		{
			categoryData.MinY = y;
		}
		_ED18 item = new _ED18(x, y, pointSize);
		categoryData.Data.Add(item);
		_E003();
	}

	private double _E004(double a, double b, double c)
	{
		return Math.Min(a, Math.Min(b, c));
	}

	private double _E005(double a, double b, double c)
	{
		return Math.Max(a, Math.Max(b, c));
	}

	private _ED17 _E006(_ED17 a, _ED17 b, _ED17 c)
	{
		return new _ED17(_E005(a.x, b.x, c.x), _E005(a.y, b.y, c.y));
	}

	private _ED17 _E007(_ED17 a, _ED17 b, _ED17 c)
	{
		return new _ED17(_E004(a.x, b.x, c.x), _E004(a.y, b.y, c.y));
	}

	public void MakeCurveCategorySmooth(string category)
	{
		if (!_categoryData.ContainsKey(category))
		{
			Debug.LogWarning(_ED3E._E000(244073));
			return;
		}
		CategoryData categoryData = _categoryData[category];
		if (!categoryData.IsBezierCurve)
		{
			Debug.LogWarning(_ED3E._E000(244303));
			return;
		}
		List<_ED18> data = categoryData.Data;
		categoryData.Regenerate = true;
		mTmpDriv.Clear();
		for (int i = 0; i < data.Count; i += 3)
		{
			_ED18 obj = data[Mathf.Max(i - 3, 0)];
			_ED18 obj2 = data[Mathf.Min(i + 3, data.Count - 1)] - obj;
			mTmpDriv.Add(obj2 * 0.25);
		}
		for (int j = 3; j < data.Count; j += 3)
		{
			int num = j / 3;
			_ED18 value = data[j - 3] + mTmpDriv[num - 1];
			_ED18 value2 = data[j] - mTmpDriv[num];
			data[j - 2] = value;
			data[j - 1] = value2;
		}
		_E003();
	}

	public void AddLinearCurveToCategory(string category, _ED17 toPoint, double pointSize = -1.0)
	{
		if (!_categoryData.ContainsKey(category))
		{
			Debug.LogWarning(_ED3E._E000(244073));
			return;
		}
		CategoryData categoryData = _categoryData[category];
		if (!categoryData.IsBezierCurve)
		{
			Debug.LogWarning(_ED3E._E000(244303));
			return;
		}
		if (categoryData.Data.Count == 0)
		{
			Debug.LogWarning(_ED3E._E000(244452));
			return;
		}
		List<_ED18> data = categoryData.Data;
		_ED18 a = data[data.Count - 1];
		_ED18 obj = _ED18.Lerp(a, toPoint.ToDoubleVector3(), 0.3333333432674408);
		_ED18 obj2 = _ED18.Lerp(a, toPoint.ToDoubleVector3(), 0.6666666865348816);
		AddCurveToCategory(category, obj.ToDoubleVector2(), obj2.ToDoubleVector2(), toPoint, pointSize);
	}

	public void AddCurveToCategory(string category, _ED17 controlPointA, _ED17 controlPointB, _ED17 toPoint, double pointSize = -1.0)
	{
		if (!_categoryData.ContainsKey(category))
		{
			Debug.LogWarning(_ED3E._E000(244073));
			return;
		}
		CategoryData categoryData = _categoryData[category];
		if (!categoryData.IsBezierCurve)
		{
			Debug.LogWarning(_ED3E._E000(244303));
			return;
		}
		if (categoryData.Data.Count == 0)
		{
			Debug.LogWarning(_ED3E._E000(244452));
			return;
		}
		List<_ED18> data = categoryData.Data;
		if (data.Count > 0 && data[data.Count - 1].x > toPoint.x)
		{
			Debug.LogWarning(_ED3E._E000(244544));
			return;
		}
		categoryData.Regenerate = true;
		_ED17 obj = _E007(controlPointA, controlPointB, toPoint);
		_ED17 obj2 = _E006(controlPointA, controlPointB, toPoint);
		if (!categoryData.MaxRadius.HasValue || categoryData.MaxRadius.Value < pointSize)
		{
			categoryData.MaxRadius = pointSize;
		}
		if (!categoryData.MaxX.HasValue || categoryData.MaxX.Value < obj2.x)
		{
			categoryData.MaxX = obj2.x;
		}
		if (!categoryData.MinX.HasValue || categoryData.MinX.Value > obj.x)
		{
			categoryData.MinX = obj.x;
		}
		if (!categoryData.MaxY.HasValue || categoryData.MaxY.Value < obj2.y)
		{
			categoryData.MaxY = obj2.y;
		}
		if (!categoryData.MinY.HasValue || categoryData.MinY.Value > obj.y)
		{
			categoryData.MinY = obj.y;
		}
		data.Add(controlPointA.ToDoubleVector3());
		data.Add(controlPointB.ToDoubleVector3());
		data.Add(new _ED18(toPoint.x, toPoint.y, pointSize));
		_E003();
	}

	public void AddPointToCategory(string category, double x, double y, double pointSize = -1.0)
	{
		if (!_categoryData.ContainsKey(category))
		{
			Debug.LogWarning(_ED3E._E000(244073));
			return;
		}
		CategoryData categoryData = _categoryData[category];
		if (categoryData.IsBezierCurve)
		{
			Debug.LogWarning(_ED3E._E000(244163));
			return;
		}
		_ED18 item = new _ED18(x, y, pointSize);
		List<_ED18> data = categoryData.Data;
		if (!categoryData.MaxRadius.HasValue || categoryData.MaxRadius.Value < pointSize)
		{
			categoryData.MaxRadius = pointSize;
		}
		if (!categoryData.MaxX.HasValue || categoryData.MaxX.Value < item.x)
		{
			categoryData.MaxX = item.x;
		}
		if (!categoryData.MinX.HasValue || categoryData.MinX.Value > item.x)
		{
			categoryData.MinX = item.x;
		}
		if (!categoryData.MaxY.HasValue || categoryData.MaxY.Value < item.y)
		{
			categoryData.MaxY = item.y;
		}
		if (!categoryData.MinY.HasValue || categoryData.MinY.Value > item.y)
		{
			categoryData.MinY = item.y;
		}
		if (data.Count > 0 && data[data.Count - 1].x <= item.x)
		{
			data.Add(item);
			return;
		}
		int num = data.BinarySearch(item, _comparer);
		if (num < 0)
		{
			num = ~num;
		}
		data.Insert(num, item);
		_E003();
	}

	double IInternalGraphData.GetMaxValue(int axis, bool dataValue)
	{
		if (!dataValue)
		{
			if (axis == 0 && !automaticHorizontalView)
			{
				return HorizontalViewOrigin + Math.Max(0.0010000000474974513, horizontalViewSize);
			}
			if (axis == 1 && !AutomaticVerticallView)
			{
				return VerticalViewOrigin + Math.Max(0.0010000000474974513, verticalViewSize);
			}
		}
		double? num = null;
		double num2 = 0.0;
		foreach (CategoryData value in _categoryData.Values)
		{
			if (value.MaxRadius.HasValue && num2 < value.MaxRadius)
			{
				num2 = value.MaxRadius.Value;
			}
			if (axis == 0)
			{
				if (!num.HasValue || (value.MaxX.HasValue && num.Value < value.MaxX))
				{
					num = value.MaxX;
				}
			}
			else if (!num.HasValue || (value.MaxY.HasValue && num.Value < value.MaxY))
			{
				num = value.MaxY;
			}
		}
		foreach (_E001 slider in _sliders)
		{
			if (axis == 0)
			{
				if (!num.HasValue || num.Value < slider.current.x)
				{
					num = slider.current.x;
				}
			}
			else if (!num.HasValue || num.Value < slider.current.y)
			{
				num = slider.current.y;
			}
		}
		if (!num.HasValue)
		{
			return 10.0;
		}
		double num3 = ((axis == 0) ? automaticcHorizontaViewGap : automaticVerticalViewGap);
		return num.Value + num2 + num3;
	}

	double IInternalGraphData.GetMinValue(int axis, bool dataValue)
	{
		if (!dataValue)
		{
			if (axis == 0 && !automaticHorizontalView)
			{
				return horizontalViewOrigin;
			}
			if (axis == 1 && !AutomaticVerticallView)
			{
				return verticalViewOrigin;
			}
		}
		double? num = null;
		double num2 = 0.0;
		foreach (CategoryData value in _categoryData.Values)
		{
			if (value.MaxRadius.HasValue && num2 < value.MaxRadius)
			{
				num2 = value.MaxRadius.Value;
			}
			if (axis == 0)
			{
				if (!num.HasValue || (value.MinX.HasValue && num.Value > value.MinX))
				{
					num = value.MinX;
				}
			}
			else if (!num.HasValue || (value.MinY.HasValue && num.Value > value.MinY))
			{
				num = value.MinY;
			}
		}
		foreach (_E001 slider in _sliders)
		{
			if (axis == 0)
			{
				if (!num.HasValue || num.Value > slider.current.x)
				{
					num = slider.current.x;
				}
			}
			else if (!num.HasValue || num.Value > slider.current.y)
			{
				num = slider.current.y;
			}
		}
		if (!num.HasValue)
		{
			return 0.0;
		}
		if (((IInternalGraphData)this).GetMaxValue(axis, dataValue) == num.Value)
		{
			if (num.Value == 0.0)
			{
				return -10.0;
			}
			return 0.0;
		}
		double num3 = ((axis == 0) ? automaticcHorizontaViewGap : automaticVerticalViewGap);
		return num.Value - num2 - num3;
	}

	void IInternalGraphData.OnAfterDeserialize()
	{
		if (mSerializedData == null)
		{
			return;
		}
		_categoryData.Clear();
		_suspendEvents = true;
		SerializedCategory[] array = mSerializedData;
		foreach (SerializedCategory serializedCategory in array)
		{
			if (serializedCategory.Depth < 0.0)
			{
				serializedCategory.Depth = 0.0;
			}
			string name = serializedCategory.Name;
			AddCategory3DGraph(name, serializedCategory.LinePrefab, serializedCategory.Material, serializedCategory.LineThickness, serializedCategory.LineTiling, serializedCategory.FillPrefab, serializedCategory.InnerFill, serializedCategory.StetchFill, serializedCategory.DotPrefab, serializedCategory.PointMaterial, serializedCategory.PointSize, serializedCategory.Depth, serializedCategory.IsBezierCurve, serializedCategory.SegmentsPerCurve);
			Set2DCategoryPrefabs(name, serializedCategory.LineHoverPrefab, serializedCategory.PointHoverPrefab);
			CategoryData categoryData = _categoryData[name];
			if (categoryData.Data == null)
			{
				categoryData.Data = new List<_ED18>();
			}
			else
			{
				categoryData.Data.Clear();
			}
			if (serializedCategory.data != null)
			{
				categoryData.Data.AddRange(serializedCategory.data);
			}
			categoryData.MaxX = serializedCategory.MaxX;
			categoryData.MaxY = serializedCategory.MaxY;
			categoryData.MinX = serializedCategory.MinX;
			categoryData.MinY = serializedCategory.MinY;
			categoryData.MaxRadius = serializedCategory.MaxRadius;
		}
		_suspendEvents = false;
	}

	void IInternalGraphData.OnBeforeSerialize()
	{
		List<SerializedCategory> list = new List<SerializedCategory>();
		foreach (KeyValuePair<string, CategoryData> categoryDatum in _categoryData)
		{
			SerializedCategory serializedCategory = new SerializedCategory
			{
				Name = categoryDatum.Key,
				MaxX = categoryDatum.Value.MaxX,
				MinX = categoryDatum.Value.MinX,
				MaxY = categoryDatum.Value.MaxY,
				MaxRadius = categoryDatum.Value.MaxRadius,
				MinY = categoryDatum.Value.MinY,
				LineThickness = categoryDatum.Value.LineThickness,
				StetchFill = categoryDatum.Value.StetchFill,
				Material = categoryDatum.Value.LineMaterial,
				LineHoverPrefab = categoryDatum.Value.LineHoverPrefab,
				PointHoverPrefab = categoryDatum.Value.PointHoverPrefab,
				LineTiling = categoryDatum.Value.LineTiling,
				InnerFill = categoryDatum.Value.FillMaterial,
				data = categoryDatum.Value.Data.ToArray(),
				PointSize = categoryDatum.Value.PointSize,
				IsBezierCurve = categoryDatum.Value.IsBezierCurve,
				SegmentsPerCurve = categoryDatum.Value.SegmentsPerCurve,
				PointMaterial = categoryDatum.Value.PointMaterial,
				LinePrefab = categoryDatum.Value.LinePrefab,
				Depth = categoryDatum.Value.Depth,
				DotPrefab = categoryDatum.Value.DotPrefab,
				FillPrefab = categoryDatum.Value.FillPrefab
			};
			if (serializedCategory.Depth < 0.0)
			{
				serializedCategory.Depth = 0.0;
			}
			list.Add(serializedCategory);
		}
		mSerializedData = list.ToArray();
	}

	[CompilerGenerated]
	private bool _E008(_E001 x)
	{
		if (!_categoryData.TryGetValue(x.category, out var value))
		{
			return true;
		}
		if (value.IsBezierCurve)
		{
			return false;
		}
		List<_ED18> data = value.Data;
		if (x.from >= data.Count || x.index >= data.Count)
		{
			return true;
		}
		_ED18 a = data[x.from];
		_ED18 to = x.To;
		double num = Time.time;
		num -= x.startTime;
		num = ((!(x.totalTime <= 9.999999747378752E-05)) ? (num / x.totalTime) : 1.0);
		_ED18 obj = (x.current = _ED18.Lerp(a, to, num));
		data[x.index] = obj;
		_sliderUpdated = true;
		if (num >= 1.0)
		{
			_E001(value, obj);
			return true;
		}
		return false;
	}
}
