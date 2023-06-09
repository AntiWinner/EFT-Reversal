using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

namespace ChartAndGraph;

public abstract class AxisBase : ChartSettingItemBase, ISerializationCallbackReceiver
{
	internal class _E000
	{
		public ChartDivisionInfo info;

		public float interp;

		public int fractionDigits;
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public double startValue;

		public double range;

		public double parentSize;

		internal double _E000(double x)
		{
			return (x - startValue) / range * parentSize;
		}
	}

	[SerializeField]
	private bool SimpleView = true;

	[Tooltip("the format of the axis labels. This can be either a number, time or date time. If the selected value is either DateTime or Time , user ChartDateUtillity to convert dates to double values that can be set to the graph")]
	[SerializeField]
	private AxisFormat format;

	[Tooltip("the depth of the axis reltive to the chart position")]
	[SerializeField]
	private AutoFloat depth;

	[FormerlySerializedAs("MainDivisions")]
	[Tooltip("The main divisions of the chart axis")]
	[SerializeField]
	private ChartMainDivisionInfo mainDivisions = new ChartMainDivisionInfo();

	[SerializeField]
	[FormerlySerializedAs("SubDivisions")]
	[Tooltip("The sub divisions of each main division")]
	private ChartSubDivisionInfo subDivisions = new ChartSubDivisionInfo();

	private Dictionary<double, string> m__E000 = new Dictionary<double, string>();

	private List<double> m__E001 = new List<double>();

	public AxisFormat Format
	{
		get
		{
			return format;
		}
		set
		{
			format = value;
			RaiseOnChanged();
		}
	}

	public AutoFloat Depth
	{
		get
		{
			return depth;
		}
		set
		{
			depth = value;
			RaiseOnChanged();
		}
	}

	public ChartDivisionInfo MainDivisions => mainDivisions;

	public ChartDivisionInfo SubDivisions => subDivisions;

	public AxisBase()
	{
		_E000();
	}

	private void _E000()
	{
		AddInnerItem(MainDivisions);
		AddInnerItem(SubDivisions);
	}

	public void ValidateProperties()
	{
		mainDivisions.ValidateProperites();
		subDivisions.ValidateProperites();
	}

	private void _E001(AnyChart parent, ChartOrientation orientation, float total, out float start, out float end)
	{
		start = 0f;
		end = total;
	}

	private void _E002(_ED20 mesh, float length, float offset)
	{
		if (length < 0f)
		{
			offset -= length;
		}
		mesh.Length = length;
		mesh.Offset = offset;
	}

	private void _E003(double scrollOffset, AnyChart parent, Transform parentTransform, ChartDivisionInfo info, _ED20 mesh, int group, ChartOrientation orientation, double gap, bool oppositeSide, double mainGap)
	{
		double parentSize = ((orientation == ChartOrientation.Vertical) ? ((IInternalUse)parent).InternalTotalHeight : ((IInternalUse)parent).InternalTotalWidth);
		_E004(parent, info, orientation, 0f, oppositeSide, out var startPosition, out var lengthDirection, out var advanceDirection);
		double num = _ED15._E005(parent, orientation, info);
		double num2 = _ED15._E006(parent, orientation, info);
		double num3 = ((orientation == ChartOrientation.Vertical) ? ((IInternalUse)parent).InternalTotalWidth : ((IInternalUse)parent).InternalTotalHeight);
		if (!info.MarkBackLength.Automatic)
		{
			num3 = info.MarkBackLength.Value;
		}
		double num4 = Math.Abs(num2);
		if (num3 != 0.0 && num > 0.0)
		{
			num4 += Math.Abs(num3) + Math.Abs(num);
		}
		_ED18 obj = advanceDirection * (info.MarkThickness * 0.5f);
		bool flag = ((IInternalUse)parent).InternalHasValues(this);
		double num5 = ((IInternalUse)parent).InternalMaxValue(this);
		double num6 = ((IInternalUse)parent).InternalMinValue(this);
		double range = num5 - num6;
		float num7 = Depth.Value;
		if (Depth.Automatic)
		{
			num7 = (float)((double)((IInternalUse)parent).InternalTotalDepth - num);
		}
		double startValue = scrollOffset + num6;
		double num8 = scrollOffset + num5 + double.Epsilon;
		Func<double, double> func = (double x) => (x - startValue) / range * parentSize;
		double num9 = gap - (scrollOffset - Math.Floor(scrollOffset / gap - double.Epsilon) * gap);
		double num10 = -1.0;
		double num11 = 0.0;
		if (mainGap > 0.0)
		{
			num10 = mainGap - (scrollOffset - Math.Floor(scrollOffset / mainGap - double.Epsilon) * mainGap);
			num11 = scrollOffset + num6 + num10;
		}
		int num12 = 0;
		this.m__E001.Clear();
		double num13 = startValue + num9;
		foreach (double key in this.m__E000.Keys)
		{
			if (key > num8 || key < num13)
			{
				this.m__E001.Add(key);
			}
		}
		for (int i = 0; i < this.m__E001.Count; i++)
		{
			this.m__E000.Remove(this.m__E001[i]);
		}
		for (double num14 = num13; num14 <= num8; num14 += gap)
		{
			num12++;
			if (num12 > 3000)
			{
				break;
			}
			if (mainGap > 0.0)
			{
				if (Math.Abs(num14 - num11) < 1E-05)
				{
					num11 += mainGap;
					continue;
				}
				if (num14 > num11)
				{
					num11 += mainGap;
				}
			}
			double num15 = func(num14);
			_ED18 obj2 = startPosition + advanceDirection * num15;
			_ED18 obj3 = obj + num2 * lengthDirection;
			obj2 -= obj;
			float num16 = 0f;
			Rect rect = _ED15._E00B(new Rect((float)obj2.x, (float)obj2.y, (float)obj3.x, (float)obj3.y));
			_E002(mesh, (float)((0.0 - num2) / num4), num16);
			num16 += Math.Abs(mesh.Length);
			mesh.AddXYRect(rect, group, num7);
			if (flag)
			{
				double num17 = Math.Round(num14 * 1000.0) / 1000.0;
				string value = "";
				int num18 = (int)Math.Round(num17);
				Dictionary<int, string> dictionary = ((orientation == ChartOrientation.Horizontal) ? parent.HorizontalValueToStringMap : parent.VerticalValueToStringMap);
				if (!(Math.Abs(num17 - (double)num18) < 0.001) || !dictionary.TryGetValue(num18, out value))
				{
					if (!this.m__E000.TryGetValue(num17, out value))
					{
						if (format == AxisFormat.Number)
						{
							value = ChartAdvancedSettings.Instance.FormatFractionDigits(info.FractionDigits, num17);
						}
						else
						{
							DateTime dateTime = _E5AD.Now.AddDays(num17);
							switch (format)
							{
							case AxisFormat.DateTime:
								value = _ED16.DateToDateTimeString(dateTime);
								break;
							case AxisFormat.Date:
								Debug.LogWarning(string.Concat(_ED16.DateToDateString(dateTime), _ED3E._E000(30288), dateTime, _ED3E._E000(245694), num17));
								value = _ED16.DateToDateString(dateTime);
								break;
							default:
								value = _ED16.DateToTimeString(dateTime);
								break;
							}
						}
						value = info.TextPrefix + value + info.TextSuffix;
						this.m__E000[num17] = value;
					}
				}
				else
				{
					value = info.TextPrefix + value + info.TextSuffix;
				}
				_ED18 obj4 = new _ED18(obj2.x, obj2.y);
				obj4 += lengthDirection * info.TextSeperation;
				_E000 obj5 = new _E000();
				obj5.interp = (float)(num15 / parentSize);
				obj5.info = info;
				obj5.fractionDigits = info.FractionDigits;
				mesh.AddText(parent, info.TextPrefab, parentTransform, info.FontSize, info.FontSharpness, value, (float)obj4.x, (float)obj4.y, num7 + info.TextDepth, 0f, obj5);
			}
			if (num > 0.0)
			{
				if (orientation == ChartOrientation.Horizontal)
				{
					_E002(mesh, (float)(num / num4), num16);
					rect = _ED15._E00B(new Rect((float)obj2.x, num7, (float)obj3.x, (float)num));
					mesh.AddXZRect(rect, group, (float)obj2.y);
				}
				else
				{
					_E002(mesh, (float)((0.0 - num) / num4), num16);
					rect = _ED15._E00B(new Rect((float)obj2.y, num7, (float)obj3.y, (float)num));
					mesh.AddYZRect(rect, group, (float)obj2.x);
				}
				num16 += Math.Abs(mesh.Length);
				if (num3 != 0.0)
				{
					_E002(mesh, (float)(num3 / num4), num16);
					num16 += Math.Abs(mesh.Length);
					_ED18 obj6 = obj + num3 * lengthDirection;
					Rect rect2 = _ED15._E00B(new Rect((float)obj2.x, (float)obj2.y, (float)obj6.x, (float)obj6.y));
					mesh.AddXYRect(rect2, group, (float)((double)num7 + num));
				}
			}
		}
	}

	private void _E004(AnyChart parent, ChartDivisionInfo info, ChartOrientation orientation, float scrollOffset, bool oppositeSide, out _ED18 startPosition, out _ED18 lengthDirection, out _ED18 advanceDirection)
	{
		if (orientation == ChartOrientation.Horizontal)
		{
			advanceDirection = new _ED18(1.0, 0.0);
			if (oppositeSide)
			{
				startPosition = new _ED18(scrollOffset, ((IInternalUse)parent).InternalTotalHeight);
				lengthDirection = new _ED18(0.0, -1.0);
			}
			else
			{
				startPosition = new _ED18(0.0, 0.0);
				lengthDirection = new _ED18(0.0, 1.0);
			}
		}
		else
		{
			advanceDirection = new _ED18(0.0, 1.0);
			if (oppositeSide)
			{
				startPosition = new _ED18(0.0, 0.0);
				lengthDirection = new _ED18(1.0, 0.0);
			}
			else
			{
				startPosition = new _ED18(((IInternalUse)parent).InternalTotalWidth, scrollOffset);
				lengthDirection = new _ED18(-1.0, 0.0);
			}
		}
	}

	internal void _E005(double scrollOffset, AnyChart parent, Transform parentTransform, _ED20 mesh, ChartOrientation orientation)
	{
		int total = SubDivisions.Total;
		if (total <= 1)
		{
			return;
		}
		double num = ((IInternalUse)parent).InternalMaxValue(this);
		double num2 = ((IInternalUse)parent).InternalMinValue(this);
		double range = num - num2;
		double? num3 = _E006(parent, range);
		if (num3.HasValue)
		{
			double gap = num3.Value / (double)total;
			mesh.Tile = _ED15._E010(SubDivisions.MaterialTiling);
			if ((SubDivisions.Alignment & ChartDivisionAligment.Opposite) == ChartDivisionAligment.Opposite)
			{
				_E003(scrollOffset, parent, parentTransform, SubDivisions, mesh, 0, orientation, gap, oppositeSide: false, num3.Value);
			}
			if ((SubDivisions.Alignment & ChartDivisionAligment.Standard) == ChartDivisionAligment.Standard)
			{
				_E003(scrollOffset, parent, parentTransform, SubDivisions, mesh, 0, orientation, gap, oppositeSide: true, num3.Value);
			}
		}
	}

	private double? _E006(AnyChart parent, double range)
	{
		double value = ((ChartMainDivisionInfo)MainDivisions).UnitsPerDivision;
		if (((ChartMainDivisionInfo)MainDivisions).Messure == ChartDivisionInfo.DivisionMessure.TotalDivisions)
		{
			int total = ((ChartMainDivisionInfo)MainDivisions).Total;
			if (total <= 0)
			{
				return null;
			}
			value = range / (double)total;
		}
		return value;
	}

	internal void _E007(double scrollOffset, AnyChart parent, Transform parentTransform, _ED20 mesh, ChartOrientation orientation)
	{
		double num = ((IInternalUse)parent).InternalMaxValue(this);
		double num2 = ((IInternalUse)parent).InternalMinValue(this);
		double range = num - num2;
		double? num3 = _E006(parent, range);
		if (num3.HasValue)
		{
			mesh.Tile = _ED15._E010(MainDivisions.MaterialTiling);
			if ((MainDivisions.Alignment & ChartDivisionAligment.Opposite) == ChartDivisionAligment.Opposite)
			{
				_E003(scrollOffset, parent, parentTransform, MainDivisions, mesh, 0, orientation, num3.Value, oppositeSide: false, -1.0);
			}
			if ((MainDivisions.Alignment & ChartDivisionAligment.Standard) == ChartDivisionAligment.Standard)
			{
				_E003(scrollOffset, parent, parentTransform, MainDivisions, mesh, 0, orientation, num3.Value, oppositeSide: true, -1.0);
			}
		}
	}

	void ISerializationCallbackReceiver.OnBeforeSerialize()
	{
	}

	void ISerializationCallbackReceiver.OnAfterDeserialize()
	{
		_E000();
	}
}
