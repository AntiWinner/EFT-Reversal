using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace ChartAndGraph;

[ExecuteInEditMode]
public abstract class GraphChartBase : AxisChart, ISerializationCallbackReceiver
{
	public class _E000
	{
		[CompilerGenerated]
		private float m__E000;

		[CompilerGenerated]
		private int _E001;

		[CompilerGenerated]
		private string _E002;

		[CompilerGenerated]
		private string _E003;

		[CompilerGenerated]
		private Vector3 _E004;

		[CompilerGenerated]
		private _ED17 _E005;

		[CompilerGenerated]
		private string _E006;

		public float Magnitude
		{
			[CompilerGenerated]
			get
			{
				return m__E000;
			}
			[CompilerGenerated]
			private set
			{
				m__E000 = value;
			}
		}

		public int Index
		{
			[CompilerGenerated]
			get
			{
				return _E001;
			}
			[CompilerGenerated]
			private set
			{
				_E001 = value;
			}
		}

		public string XString
		{
			[CompilerGenerated]
			get
			{
				return _E002;
			}
			[CompilerGenerated]
			private set
			{
				_E002 = value;
			}
		}

		public string YString
		{
			[CompilerGenerated]
			get
			{
				return _E003;
			}
			[CompilerGenerated]
			private set
			{
				_E003 = value;
			}
		}

		public Vector3 Position
		{
			[CompilerGenerated]
			get
			{
				return _E004;
			}
			[CompilerGenerated]
			private set
			{
				_E004 = value;
			}
		}

		public _ED17 Value
		{
			[CompilerGenerated]
			get
			{
				return _E005;
			}
			[CompilerGenerated]
			private set
			{
				_E005 = value;
			}
		}

		public string Category
		{
			[CompilerGenerated]
			get
			{
				return _E006;
			}
			[CompilerGenerated]
			private set
			{
				_E006 = value;
			}
		}

		public _E000(int index, Vector3 position, _ED17 value, float magnitude, string category, string xString, string yString)
		{
			Position = position;
			Value = value;
			Category = category;
			XString = xString;
			YString = yString;
			Index = index;
			Magnitude = magnitude;
		}
	}

	[Serializable]
	public class GraphEvent : UnityEvent<_E000>
	{
	}

	[SerializeField]
	protected float heightRatio = 300f;

	[SerializeField]
	protected float widthRatio = 600f;

	public GraphEvent PointClicked = new GraphEvent();

	public GraphEvent PointHovered = new GraphEvent();

	public UnityEvent NonHovered = new UnityEvent();

	[SerializeField]
	private bool scrollable = true;

	[HideInInspector]
	[SerializeField]
	private bool autoScrollHorizontally;

	[SerializeField]
	[HideInInspector]
	private double horizontalScrolling;

	[SerializeField]
	[HideInInspector]
	private bool autoScrollVertically;

	[SerializeField]
	[HideInInspector]
	private double verticalScrolling;

	[SerializeField]
	[HideInInspector]
	protected GraphData Data = new GraphData();

	public float HeightRatio
	{
		get
		{
			return heightRatio;
		}
		set
		{
			heightRatio = value;
			Invalidate();
		}
	}

	public float WidthRatio
	{
		get
		{
			return widthRatio;
		}
		set
		{
			widthRatio = value;
			Invalidate();
		}
	}

	public bool Scrollable
	{
		get
		{
			return scrollable;
		}
		set
		{
			scrollable = value;
			Invalidate();
		}
	}

	public bool AutoScrollHorizontally
	{
		get
		{
			return autoScrollHorizontally;
		}
		set
		{
			autoScrollHorizontally = value;
			GenerateRealtime();
		}
	}

	public double HorizontalScrolling
	{
		get
		{
			return horizontalScrolling;
		}
		set
		{
			horizontalScrolling = value;
			GenerateRealtime();
		}
	}

	public bool AutoScrollVertically
	{
		get
		{
			return autoScrollVertically;
		}
		set
		{
			autoScrollVertically = value;
			GenerateRealtime();
		}
	}

	public double VerticalScrolling
	{
		get
		{
			return verticalScrolling;
		}
		set
		{
			verticalScrolling = value;
			GenerateRealtime();
		}
	}

	public GraphData DataSource => Data;

	protected override bool SupportsCategoryLabels => false;

	protected override bool SupportsGroupLables => false;

	protected override bool SupportsItemLabels => true;

	protected override float TotalHeightLink => heightRatio;

	protected override float TotalWidthLink => widthRatio;

	protected override float TotalDepthLink => 0f;

	protected override double GetScrollOffset(int axis)
	{
		if (!scrollable)
		{
			return 0.0;
		}
		if ((autoScrollHorizontally && axis == 0) || (autoScrollVertically && axis == 1))
		{
			double maxValue = ((IInternalGraphData)Data).GetMaxValue(axis, dataValue: false);
			return ((IInternalGraphData)Data).GetMaxValue(axis, dataValue: true) - maxValue;
		}
		return axis switch
		{
			1 => verticalScrolling, 
			0 => horizontalScrolling, 
			_ => base.GetScrollOffset(axis), 
		};
	}

	private void _E000()
	{
		((IInternalGraphData)Data).InternalDataChanged -= _E002;
		((IInternalGraphData)Data).InternalRealTimeDataChanged -= _E001;
		((IInternalGraphData)Data).InternalDataChanged += _E002;
		((IInternalGraphData)Data).InternalRealTimeDataChanged += _E001;
	}

	private void _E001(object sender, EventArgs e)
	{
		GenerateRealtime();
	}

	private void _E002(object sender, EventArgs e)
	{
		Invalidate();
	}

	protected override void Start()
	{
		base.Start();
		if (!_ED15._E003)
		{
			_E000();
		}
		Invalidate();
	}

	protected override void OnValidate()
	{
		base.OnValidate();
		if (!_ED15._E003)
		{
			_E000();
		}
		Data.RestoreDataValues();
		Invalidate();
	}

	protected override void OnLabelSettingChanged()
	{
		base.OnLabelSettingChanged();
		Invalidate();
	}

	protected override void OnAxisValuesChanged()
	{
		base.OnAxisValuesChanged();
		Invalidate();
	}

	protected override void OnLabelSettingsSet()
	{
		base.OnLabelSettingsSet();
		Invalidate();
	}

	protected _ED19 interpolateInRect(Rect rect, _ED18 point)
	{
		double x = (double)rect.x + (double)rect.width * point.x;
		double y = (double)rect.y + (double)rect.height * point.y;
		return new _ED19(x, y, point.z, 0.0);
	}

	protected _ED19 TransformPoint(Rect viewRect, Vector3 point, _ED17 min, _ED17 range)
	{
		return interpolateInRect(viewRect, new _ED18(((double)point.x - min.x) / range.x, ((double)point.y - min.y) / range.y));
	}

	protected override void Update()
	{
		base.Update();
		((IInternalGraphData)Data).Update();
	}

	private void _E003(_ED18 point, ref double minX, ref double minY, ref double maxX, ref double maxY)
	{
		minX = Math.Min(minX, point.x);
		maxX = Math.Max(maxX, point.x);
		minY = Math.Min(minY, point.y);
		maxY = Math.Max(maxY, point.y);
	}

	private Rect _E004(Rect completeRect, Rect lineRect)
	{
		if (completeRect.width < 0.0001f || completeRect.height < 0.0001f)
		{
			return default(Rect);
		}
		if (float.IsInfinity(lineRect.xMax) || float.IsInfinity(lineRect.xMin) || float.IsInfinity(lineRect.yMin) || float.IsInfinity(lineRect.yMax))
		{
			return default(Rect);
		}
		float x = (lineRect.xMin - completeRect.xMin) / completeRect.width;
		float y = (lineRect.yMin - completeRect.yMin) / completeRect.height;
		float width = lineRect.width / completeRect.width;
		float height = lineRect.height / completeRect.height;
		return new Rect(x, y, width, height);
	}

	protected int ClipPoints(IList<_ED18> points, List<_ED19> res, out Rect uv)
	{
		double minValue = ((IInternalGraphData)Data).GetMinValue(0, dataValue: false);
		double minValue2 = ((IInternalGraphData)Data).GetMinValue(1, dataValue: false);
		double maxValue = ((IInternalGraphData)Data).GetMaxValue(0, dataValue: false);
		double maxValue2 = ((IInternalGraphData)Data).GetMaxValue(1, dataValue: false);
		double num = GetScrollOffset(0) + minValue;
		double num2 = GetScrollOffset(1) + minValue2;
		double num3 = maxValue - minValue;
		double num4 = maxValue2 - minValue2;
		double num5 = num + num3;
		bool flag = false;
		bool flag2 = false;
		double maxX = double.MinValue;
		double minX = double.MaxValue;
		double maxY = double.MinValue;
		double minY = double.MaxValue;
		minValue = double.MaxValue;
		minValue2 = double.MaxValue;
		maxValue = double.MinValue;
		maxValue2 = double.MinValue;
		int result = 0;
		for (int i = 0; i < points.Count; i++)
		{
			bool flag3 = flag;
			bool flag4 = flag2;
			flag = false;
			flag2 = false;
			_ED18 point = points[i];
			_E003(points[i], ref minValue, ref minValue2, ref maxValue, ref maxValue2);
			if (point.x < num || point.x > num5)
			{
				flag = true;
				if (flag4)
				{
					res.Add(point.ToDoubleVector4());
				}
				if (flag3 && point.x > num5 && points[i - 1].x < num)
				{
					_E003(points[i - 1], ref minX, ref minY, ref maxX, ref maxY);
					_E003(point, ref minX, ref minY, ref maxX, ref maxY);
					res.Add(points[i - 1].ToDoubleVector4());
					res.Add(point.ToDoubleVector4());
				}
			}
			else
			{
				flag2 = true;
				if (flag3)
				{
					result = i - 1;
					_E003(points[i - 1], ref minX, ref minY, ref maxX, ref maxY);
					res.Add(points[i - 1].ToDoubleVector4());
				}
				_E003(point, ref minX, ref minY, ref maxX, ref maxY);
				res.Add(point.ToDoubleVector4());
			}
		}
		for (int j = 0; j < res.Count; j++)
		{
			_ED19 obj = res[j];
			obj.w = obj.z;
			obj.z = 0.0;
			res[j] = obj;
		}
		uv = _E004(new Rect((float)minValue, (float)minValue2, (float)(maxValue - minValue), (float)(maxValue2 - minValue2)), new Rect((float)minX, (float)num2, (float)(maxX - minX), (float)num4));
		return result;
	}

	protected void TransformPoints(IList<_ED18> points, Rect viewRect, _ED18 min, _ED18 max)
	{
		_ED18 obj = max - min;
		if (!(obj.x <= 9.999999747378752E-05) && !(obj.y < 9.999999747378752E-05))
		{
			double num = Math.Min((double)viewRect.width / obj.x, (double)viewRect.height / obj.y);
			for (int i = 0; i < points.Count; i++)
			{
				_ED18 obj2 = points[i];
				_ED19 obj3 = interpolateInRect(viewRect, new _ED18((obj2.x - min.x) / obj.x, (obj2.y - min.y) / obj.y));
				obj3.z = obj2.z * num;
				points[i] = obj3.ToDoubleVector3();
			}
		}
	}

	protected void TransformPoints(IList<_ED19> points, List<Vector4> output, Rect viewRect, _ED18 min, _ED18 max)
	{
		output.Clear();
		_ED18 obj = max - min;
		if (!(obj.x <= 9.999999747378752E-05) && !(obj.y < 9.999999747378752E-05))
		{
			double num = Math.Min((double)viewRect.width / obj.x, (double)viewRect.height / obj.y);
			for (int i = 0; i < points.Count; i++)
			{
				_ED19 obj2 = points[i];
				_ED19 obj3 = interpolateInRect(viewRect, new _ED18((obj2.x - min.x) / obj.x, (obj2.y - min.y) / obj.y));
				obj3.z = 0.0;
				obj3.w = obj2.w * num;
				output.Add(obj3.ToVector4());
			}
		}
	}

	protected override void ValidateProperties()
	{
		base.ValidateProperties();
		if (heightRatio < 0f)
		{
			heightRatio = 0f;
		}
		if (widthRatio < 0f)
		{
			widthRatio = 0f;
		}
	}

	public virtual void GenerateRealtime()
	{
		GenerateAxis(force: false);
	}

	protected override bool HasValues(AxisBase axis)
	{
		return true;
	}

	protected override double MaxValue(AxisBase axis)
	{
		if (axis == null)
		{
			return 0.0;
		}
		if (axis == mHorizontalAxis)
		{
			return ((IInternalGraphData)Data).GetMaxValue(0, dataValue: false);
		}
		if (axis == mVerticalAxis)
		{
			return ((IInternalGraphData)Data).GetMaxValue(1, dataValue: false);
		}
		return 0.0;
	}

	protected override double MinValue(AxisBase axis)
	{
		if (axis == null)
		{
			return 0.0;
		}
		if (axis == mHorizontalAxis)
		{
			return ((IInternalGraphData)Data).GetMinValue(0, dataValue: false);
		}
		if (axis == mVerticalAxis)
		{
			return ((IInternalGraphData)Data).GetMinValue(1, dataValue: false);
		}
		return 0.0;
	}

	void ISerializationCallbackReceiver.OnBeforeSerialize()
	{
		if (Data != null)
		{
			((IInternalGraphData)Data).OnBeforeSerialize();
		}
	}

	void ISerializationCallbackReceiver.OnAfterDeserialize()
	{
		if (Data != null)
		{
			((IInternalGraphData)Data).OnAfterDeserialize();
		}
	}

	protected override void OnItemHoverted(object userData)
	{
		base.OnItemHoverted(userData);
		_E000 arg = userData as _E000;
		if (PointHovered != null)
		{
			PointHovered.Invoke(arg);
		}
	}

	protected string StringFromAxisFormat(double val, AxisBase axis)
	{
		if (axis == null)
		{
			return ChartAdvancedSettings.Instance.FormatFractionDigits(2, val);
		}
		return StringFromAxisFormat(val, axis, axis.MainDivisions.FractionDigits);
	}

	protected string StringFromAxisFormat(double val, AxisBase axis, int fractionDigits)
	{
		if (axis == null)
		{
			return ChartAdvancedSettings.Instance.FormatFractionDigits(fractionDigits, val);
		}
		string text = "";
		if (axis.Format == AxisFormat.Number)
		{
			return ChartAdvancedSettings.Instance.FormatFractionDigits(fractionDigits, val);
		}
		DateTime dateTime = _ED16.ValueToDate(val);
		if (axis.Format == AxisFormat.DateTime)
		{
			return _ED16.DateToDateTimeString(dateTime);
		}
		if (axis.Format == AxisFormat.Date)
		{
			return _ED16.DateToDateString(dateTime);
		}
		return _ED16.DateToTimeString(dateTime);
	}

	protected override void OnItemSelected(object userData)
	{
		base.OnItemSelected(userData);
		_E000 arg = userData as _E000;
		if (PointClicked != null)
		{
			PointClicked.Invoke(arg);
		}
	}
}
