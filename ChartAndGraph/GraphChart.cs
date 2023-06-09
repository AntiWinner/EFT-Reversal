using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ChartAndGraph;

public class GraphChart : GraphChartBase, _ED1A
{
	private new class _E000
	{
		public _ED1E mItemLabels;

		public CanvasLines mLines;

		public CanvasLines mDots;

		public CanvasLines mFill;

		public Dictionary<int, string> mCahced = new Dictionary<int, string>();
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public string catName;

		public GraphChart _003C_003E4__this;

		internal void _E000(int idx, Vector2 pos)
		{
			_003C_003E4__this._E008(catName, idx, pos);
		}

		internal void _E001(int idx, Vector2 pos)
		{
			_003C_003E4__this._E007(catName, idx, pos);
		}

		internal void _E002()
		{
			_003C_003E4__this._E006(catName);
		}
	}

	private new Vector2 m__E000 = Vector2.zero;

	private HashSet<string> m__E001 = new HashSet<string>();

	private Dictionary<string, Dictionary<int, BillboardText>> m__E002 = new Dictionary<string, Dictionary<int, BillboardText>>();

	private HashSet<BillboardText> m__E003 = new HashSet<BillboardText>();

	private Dictionary<string, _E000> m__E004 = new Dictionary<string, _E000>();

	private List<_ED18> m__E005 = new List<_ED18>();

	private List<_ED19> m__E006 = new List<_ED19>();

	private List<Vector4> m__E007 = new List<Vector4>();

	private List<int> m__E008 = new List<int>();

	private GameObject m__E009;

	private GameObject m__E00A;

	private Vector2? _E00B;

	private GraphicRaycaster _E00C;

	private bool _E00D;

	private StringBuilder _E00E = new StringBuilder();

	public UnityEvent MousePan;

	[SerializeField]
	private bool horizontalPanning;

	[SerializeField]
	private bool verticalPanning;

	public bool HorizontalPanning
	{
		get
		{
			return horizontalPanning;
		}
		set
		{
			horizontalPanning = value;
			Invalidate();
		}
	}

	public bool VerticalPanning
	{
		get
		{
			return verticalPanning;
		}
		set
		{
			verticalPanning = value;
			Invalidate();
		}
	}

	private GameObject _E000(Rect viewRect)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(_E3A2.Load(_ED3E._E000(244042)) as GameObject);
		_ED15._E003(gameObject, hideHierarchy);
		gameObject.AddComponent<ChartItem>();
		gameObject.transform.SetParent(base.transform, worldPositionStays: false);
		RectTransform component = gameObject.GetComponent<RectTransform>();
		component.anchorMin = new Vector2(0f, 0f);
		component.anchorMax = new Vector2(0f, 0f);
		component.pivot = new Vector2(0f, 1f);
		component.sizeDelta = viewRect.size;
		component.anchoredPosition = new Vector2(0f, viewRect.size.y);
		this.m__E00A = gameObject;
		Mask component2 = this.m__E00A.GetComponent<Mask>();
		if (component2 != null)
		{
			component2.enabled = base.Scrollable;
		}
		return this.m__E00A;
	}

	private CanvasLines _E001(GraphData.CategoryData data, GameObject rectMask)
	{
		GameObject obj = new GameObject(_ED3E._E000(244083), typeof(RectTransform));
		_ED15._E003(obj, hideHierarchy);
		obj.AddComponent<ChartItem>();
		RectTransform component = obj.GetComponent<RectTransform>();
		obj.AddComponent<CanvasRenderer>();
		CanvasLines canvasLines = obj.AddComponent<CanvasLines>();
		canvasLines.maskable = true;
		component.SetParent(rectMask.transform, worldPositionStays: false);
		component.localScale = new Vector3(1f, 1f, 1f);
		component.anchorMin = new Vector2(0f, 0f);
		component.anchorMax = new Vector2(0f, 0f);
		component.anchoredPosition = Vector3.zero;
		component.localRotation = Quaternion.identity;
		return canvasLines;
	}

	protected override void Update()
	{
		base.Update();
		_E005();
		RectTransform component = GetComponent<RectTransform>();
		if (component != null && component.hasChanged && this.m__E000 != component.rect.size)
		{
			Invalidate();
		}
	}

	private void _E002(string cat, int index, BillboardText text)
	{
		if (text.UIText != null)
		{
			text.UIText.maskable = false;
		}
		if (!this.m__E002.TryGetValue(cat, out var value))
		{
			value = new Dictionary<int, BillboardText>(_ED15.DefaultIntComparer);
			this.m__E002.Add(cat, value);
		}
		value.Add(index, text);
	}

	protected override void ClearChart()
	{
		if (this.m__E00A != null)
		{
			this.m__E00A.GetComponent<Mask>().enabled = false;
		}
		base.ClearChart();
		this.m__E002.Clear();
		this.m__E003.Clear();
		this.m__E004.Clear();
	}

	private double _E003(double radius, double mag, double min, double max)
	{
		return (max - min) / mag * radius;
	}

	public override void GenerateRealtime()
	{
		if (_E00D)
		{
			return;
		}
		base.GenerateRealtime();
		double minValue = ((IInternalGraphData)Data).GetMinValue(0, dataValue: false);
		double minValue2 = ((IInternalGraphData)Data).GetMinValue(1, dataValue: false);
		double maxValue = ((IInternalGraphData)Data).GetMaxValue(0, dataValue: false);
		double maxValue2 = ((IInternalGraphData)Data).GetMaxValue(1, dataValue: false);
		double scrollOffset = GetScrollOffset(0);
		double scrollOffset2 = GetScrollOffset(1);
		double num = maxValue - minValue;
		double num2 = maxValue2 - minValue2;
		double x = minValue + scrollOffset + num;
		double y = minValue2 + scrollOffset2 + num2;
		_ED18 min = new _ED18(scrollOffset + minValue, scrollOffset2 + minValue2);
		_ED18 max = new _ED18(x, y);
		Rect rect = new Rect(0f, 0f, widthRatio, heightRatio);
		Transform parentTransform = base.transform;
		if (this.m__E009 != null)
		{
			parentTransform = this.m__E009.transform;
		}
		foreach (Dictionary<int, BillboardText> value3 in this.m__E002.Values)
		{
			value3.Clear();
		}
		foreach (GraphData.CategoryData category in ((IInternalGraphData)Data).Categories)
		{
			_E000 value = null;
			if (!this.m__E004.TryGetValue(category.Name, out value))
			{
				continue;
			}
			this.m__E006.Clear();
			this.m__E005.Clear();
			this.m__E005.AddRange(category.getPoints());
			Rect uv;
			int num3 = ClipPoints(this.m__E005, this.m__E006, out uv);
			TransformPoints(this.m__E006, this.m__E007, rect, min, max);
			this.m__E008.Clear();
			int num4 = num3 + this.m__E006.Count;
			foreach (int key2 in value.mCahced.Keys)
			{
				if (key2 < num3 || key2 > num4)
				{
					this.m__E008.Add(key2);
				}
			}
			for (int i = 0; i < this.m__E008.Count; i++)
			{
				value.mCahced.Remove(this.m__E008[i]);
			}
			if (this.m__E005.Count == 0)
			{
				continue;
			}
			if (mItemLabels != null && mItemLabels.isActiveAndEnabled && value.mItemLabels != null)
			{
				Rect rect2 = rect;
				rect2.xMin -= 1f;
				rect2.yMin -= 1f;
				rect2.xMax += 1f;
				rect2.yMax += 1f;
				_ED1E obj = value.mItemLabels;
				obj.Clear();
				for (int j = 0; j < this.m__E007.Count; j++)
				{
					if ((double)this.m__E007[j].w == 0.0)
					{
						continue;
					}
					Vector3 vector = (Vector3)this.m__E007[j] + new Vector3(mItemLabels.Location.Breadth, mItemLabels.Seperation, mItemLabels.Location.Depth);
					if (mItemLabels.Alignment == ChartLabelAlignment.Base)
					{
						vector.y -= this.m__E007[j].y;
					}
					if (rect2.Contains((Vector2)this.m__E007[j]))
					{
						string value2 = null;
						int key = j + num3;
						if (!value.mCahced.TryGetValue(key, out value2))
						{
							_ED18 obj2 = this.m__E005[j + num3];
							string arg = StringFromAxisFormat(obj2.x, mHorizontalAxis, mItemLabels.FractionDigits);
							string arg2 = StringFromAxisFormat(obj2.y, mVerticalAxis, mItemLabels.FractionDigits);
							mItemLabels.TextFormat.Format(_E00E, string.Format(_ED3E._E000(71425), arg, arg2), category.Name, "");
							value2 = _E00E.ToString();
							value.mCahced[key] = value2;
						}
						BillboardText text = obj.AddText(this, mItemLabels.TextPrefab, parentTransform, mItemLabels.FontSize, mItemLabels.FontSharpness, value2, vector.x, vector.y, vector.z, 0f, null);
						_E002(category.Name, j + num3, text);
					}
				}
				obj.DestoryRecycled();
				if (obj.TextObjects != null)
				{
					foreach (BillboardText textObject in obj.TextObjects)
					{
						((IInternalUse)this).InternalTextController.AddText(textObject);
					}
				}
			}
			if (value.mDots != null)
			{
				Rect r = rect;
				float num5 = (float)(category.PointSize * 0.5);
				r.xMin -= num5;
				r.yMin -= num5;
				r.xMax += num5;
				r.yMax += num5;
				value.mDots.SetViewRect(r, uv);
				value.mDots._E001(this.m__E007);
				value.mDots.refrenceIndex = num3;
			}
			if (value.mLines != null)
			{
				float num6 = 1f;
				if (category.LineTiling.EnableTiling && category.LineTiling.TileFactor > 0f)
				{
					float num7 = 0f;
					for (int k = 1; k < this.m__E007.Count; k++)
					{
						num7 += (this.m__E007[k - 1] - this.m__E007[k]).magnitude;
					}
					num6 = num7 / category.LineTiling.TileFactor;
				}
				if (num6 <= 0.0001f)
				{
					num6 = 1f;
				}
				value.mLines.Tiling = num6;
				value.mLines.SetViewRect(rect, uv);
				value.mLines._E001(this.m__E007);
				value.mLines.refrenceIndex = num3;
			}
			if (value.mFill != null)
			{
				value.mFill.SetViewRect(rect, uv);
				value.mFill._E001(this.m__E007);
				value.mFill.refrenceIndex = num3;
			}
		}
	}

	public override void InternalGenerateChart()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		base.InternalGenerateChart();
		ClearChart();
		if (Data == null)
		{
			return;
		}
		GenerateAxis(force: true);
		double minValue = ((IInternalGraphData)Data).GetMinValue(0, dataValue: false);
		double minValue2 = ((IInternalGraphData)Data).GetMinValue(1, dataValue: false);
		double maxValue = ((IInternalGraphData)Data).GetMaxValue(0, dataValue: false);
		double maxValue2 = ((IInternalGraphData)Data).GetMaxValue(1, dataValue: false);
		double scrollOffset = GetScrollOffset(0);
		double scrollOffset2 = GetScrollOffset(1);
		double num = maxValue - minValue;
		double num2 = maxValue2 - minValue2;
		double x2 = minValue + scrollOffset + num;
		double y = minValue2 + scrollOffset2 + num2;
		_ED18 min = new _ED18(scrollOffset + minValue, scrollOffset2 + minValue2);
		_ED18 max = new _ED18(x2, y);
		Rect rect = new Rect(0f, 0f, widthRatio, heightRatio);
		int num3 = 0;
		int num4 = ((IInternalGraphData)Data).TotalCategories + 1;
		bool flag = false;
		this.m__E002.Clear();
		this.m__E003.Clear();
		GameObject rectMask = _E000(rect);
		foreach (GraphData.CategoryData category in ((IInternalGraphData)Data).Categories)
		{
			this.m__E006.Clear();
			_ED18[] array = category.getPoints().ToArray();
			Rect uv;
			int num5 = ClipPoints(array, this.m__E006, out uv);
			TransformPoints(this.m__E006, this.m__E007, rect, min, max);
			if (array.Length == 0 && _ED15._E003)
			{
				flag = true;
				int num6 = num4 - 1 - num3;
				float num7 = (float)num6 / (float)num4;
				float num8 = ((float)num6 + 1f) / (float)num4;
				_ED18 obj = interpolateInRect(rect, new _ED18(0.0, num7, -1.0)).ToDoubleVector3();
				_ED18 obj2 = interpolateInRect(rect, new _ED18(0.5, num8, -1.0)).ToDoubleVector3();
				_ED18 obj3 = interpolateInRect(rect, new _ED18(1.0, num7, -1.0)).ToDoubleVector3();
				array = new _ED18[3] { obj, obj2, obj3 };
				this.m__E007.AddRange(((IEnumerable<_ED18>)array).Select((Func<_ED18, Vector4>)((_ED18 x) => x.ToVector3())));
				num3++;
			}
			List<CanvasLines._E001> list = new List<CanvasLines._E001>();
			list.Add(new CanvasLines._E001(this.m__E007));
			_E000 obj4 = new _E000();
			if (category.FillMaterial != null)
			{
				CanvasLines canvasLines = _E001(category, rectMask);
				canvasLines.material = category.FillMaterial;
				canvasLines.refrenceIndex = num5;
				canvasLines._E002(list);
				canvasLines.SetViewRect(rect, uv);
				canvasLines.MakeFillRender(rect, category.StetchFill);
				obj4.mFill = canvasLines;
			}
			if (category.LineMaterial != null)
			{
				CanvasLines canvasLines2 = _E001(category, rectMask);
				float num9 = 1f;
				if (category.LineTiling.EnableTiling && category.LineTiling.TileFactor > 0f)
				{
					float num10 = 0f;
					for (int i = 1; i < this.m__E007.Count; i++)
					{
						num10 += (this.m__E007[i - 1] - this.m__E007[i]).magnitude;
					}
					num9 = num10 / category.LineTiling.TileFactor;
				}
				if (num9 <= 0.0001f)
				{
					num9 = 1f;
				}
				canvasLines2.SetViewRect(rect, uv);
				canvasLines2.Thickness = (float)category.LineThickness;
				canvasLines2.Tiling = num9;
				canvasLines2.refrenceIndex = num5;
				canvasLines2.material = category.LineMaterial;
				canvasLines2.SetHoverPrefab(category.LineHoverPrefab);
				canvasLines2._E002(list);
				obj4.mLines = canvasLines2;
			}
			CanvasLines canvasLines3 = (obj4.mDots = _E001(category, rectMask));
			canvasLines3.material = category.PointMaterial;
			canvasLines3._E002(list);
			Rect r = rect;
			float num11 = (float)category.PointSize * 0.5f;
			r.xMin -= num11;
			r.yMin -= num11;
			r.xMax += num11;
			r.yMax += num11;
			canvasLines3.SetViewRect(r, uv);
			canvasLines3.refrenceIndex = num5;
			canvasLines3.SetHoverPrefab(category.PointHoverPrefab);
			if (category.PointMaterial != null)
			{
				canvasLines3.MakePointRender((float)category.PointSize);
			}
			else
			{
				canvasLines3.MakePointRender(0f);
			}
			if (mItemLabels != null && mItemLabels.isActiveAndEnabled)
			{
				_ED1E obj5 = new _ED1E(forText: true);
				obj5.RecycleText = true;
				obj4.mItemLabels = obj5;
				Rect rect2 = rect;
				rect2.xMin -= 1f;
				rect2.yMin -= 1f;
				rect2.xMax += 1f;
				rect2.yMax += 1f;
				for (int j = 0; j < this.m__E007.Count; j++)
				{
					if (this.m__E007[j].w == 0f)
					{
						continue;
					}
					Vector2 point = this.m__E007[j];
					if (rect2.Contains(point))
					{
						if (!flag)
						{
							point = Data.GetPoint(category.Name, j + num5).ToVector2();
						}
						string arg = StringFromAxisFormat(point.x, mHorizontalAxis, mItemLabels.FractionDigits);
						string arg2 = StringFromAxisFormat(point.y, mVerticalAxis, mItemLabels.FractionDigits);
						Vector3 vector = (Vector3)this.m__E007[j] + new Vector3(mItemLabels.Location.Breadth, mItemLabels.Seperation, mItemLabels.Location.Depth);
						if (mItemLabels.Alignment == ChartLabelAlignment.Base)
						{
							vector.y -= this.m__E007[j].y;
						}
						string text = mItemLabels.TextFormat.Format(string.Format(_ED3E._E000(71425), arg, arg2), category.Name, "");
						BillboardText billboardText = obj5.AddText(this, mItemLabels.TextPrefab, base.transform, mItemLabels.FontSize, mItemLabels.FontSharpness, text, vector.x, vector.y, vector.z, 0f, null);
						base.TextController.AddText(billboardText);
						_E002(category.Name, j + num5, billboardText);
					}
				}
			}
			string catName = category.Name;
			canvasLines3.Hover += delegate(int idx, Vector2 pos)
			{
				_E008(catName, idx, pos);
			};
			canvasLines3.Click += delegate(int idx, Vector2 pos)
			{
				_E007(catName, idx, pos);
			};
			canvasLines3.Leave += delegate
			{
				_E006(catName);
			};
			this.m__E004[catName] = obj4;
		}
		base.TextController.transform.SetAsLastSibling();
		_E009();
	}

	private void _E004(Vector2 delta)
	{
		bool flag = false;
		_E00D = true;
		if (VerticalPanning)
		{
			float num = (float)((IInternalGraphData)Data).GetMinValue(1, dataValue: false);
			float num2 = (float)((IInternalGraphData)Data).GetMaxValue(1, dataValue: false) - num;
			base.VerticalScrolling -= delta.y / heightRatio * num2;
			if (Mathf.Abs(delta.y) > 1f)
			{
				flag = true;
			}
		}
		if (HorizontalPanning)
		{
			float num3 = (float)((IInternalGraphData)Data).GetMinValue(0, dataValue: false);
			float num4 = (float)((IInternalGraphData)Data).GetMaxValue(0, dataValue: false) - num3;
			base.HorizontalScrolling -= delta.x / widthRatio * num4;
			if (Mathf.Abs(delta.x) > 1f)
			{
				flag = true;
			}
		}
		_E00D = false;
		if (flag)
		{
			GenerateRealtime();
			if (MousePan != null)
			{
				MousePan.Invoke();
			}
		}
	}

	private void _E005()
	{
		if (!verticalPanning && !horizontalPanning)
		{
			return;
		}
		_E00C = GetComponentInParent<GraphicRaycaster>();
		if (!(_E00C == null))
		{
			RectTransformUtility.ScreenPointToLocalPointInRectangle(base.transform as RectTransform, Input.mousePosition, _E00C.eventCamera, out var localPoint);
			bool flag = RectTransformUtility.RectangleContainsScreenPoint(base.transform as RectTransform, Input.mousePosition);
			if (Input.GetMouseButton(0) && flag && _E00B.HasValue)
			{
				Vector2 delta = localPoint - _E00B.Value;
				_E004(delta);
			}
			_E00B = localPoint;
		}
	}

	private void _E006(string category)
	{
		foreach (BillboardText item in this.m__E003)
		{
			if (item == null || item.UIText == null)
			{
				continue;
			}
			ChartItemEffect[] components = item.UIText.GetComponents<ChartItemEffect>();
			foreach (ChartItemEffect chartItemEffect in components)
			{
				if (chartItemEffect != null)
				{
					chartItemEffect.TriggerOut(deactivateOnEnd: false);
				}
			}
		}
		this.m__E003.Clear();
		OnItemLeave(new GraphChartBase._E000(0, Vector3.zero, new _ED17(0.0, 0.0), -1f, category, "", ""));
	}

	private void _E007(string category, int idx, Vector2 pos)
	{
		_ED18 point = Data.GetPoint(category, idx);
		if (!this.m__E002.TryGetValue(category, out var value) || !value.TryGetValue(idx, out var value2))
		{
			return;
		}
		foreach (BillboardText item in this.m__E003)
		{
			if (item == null || item.UIText == null || item.UIText == value2.UIText)
			{
				continue;
			}
			ChartItemEffect[] components = item.UIText.GetComponents<ChartItemEffect>();
			foreach (ChartItemEffect chartItemEffect in components)
			{
				if (chartItemEffect != null)
				{
					chartItemEffect.TriggerOut(deactivateOnEnd: false);
				}
			}
		}
		this.m__E003.Clear();
		Text uIText = value2.UIText;
		if (uIText != null)
		{
			ChartItemEvents component = uIText.GetComponent<ChartItemEvents>();
			if (component != null)
			{
				component.OnMouseDown();
				this.m__E003.Add(value2);
			}
		}
		string xString = StringFromAxisFormat(point.x, mHorizontalAxis);
		string yString = StringFromAxisFormat(point.y, mVerticalAxis);
		OnItemSelected(new GraphChartBase._E000(idx, pos, point.ToDoubleVector2(), (float)point.z, category, xString, yString));
	}

	private void _E008(string category, int idx, Vector2 pos)
	{
		_ED18 point = Data.GetPoint(category, idx);
		if (!this.m__E002.TryGetValue(category, out var value) || !value.TryGetValue(idx, out var value2))
		{
			return;
		}
		foreach (BillboardText item in this.m__E003)
		{
			if (item == null || item.UIText == null || item.UIText == value2.UIText)
			{
				continue;
			}
			ChartItemEffect[] components = item.UIText.GetComponents<ChartItemEffect>();
			foreach (ChartItemEffect chartItemEffect in components)
			{
				if (chartItemEffect != null)
				{
					chartItemEffect.TriggerOut(deactivateOnEnd: false);
				}
			}
		}
		this.m__E003.Clear();
		Text uIText = value2.UIText;
		if (uIText != null)
		{
			ChartItemEvents component = uIText.GetComponent<ChartItemEvents>();
			if (component != null)
			{
				component.OnMouseEnter();
				this.m__E003.Add(value2);
			}
		}
		string xString = StringFromAxisFormat(point.x, mHorizontalAxis);
		string yString = StringFromAxisFormat(point.y, mVerticalAxis);
		OnItemHoverted(new GraphChartBase._E000(idx, pos, point.ToDoubleVector2(), (float)point.z, category, xString, yString));
	}

	protected override void OnItemHoverted(object userData)
	{
		base.OnItemHoverted(userData);
		GraphChartBase._E000 obj = userData as GraphChartBase._E000;
		this.m__E001.Add(obj.Category);
	}

	protected override void OnItemSelected(object userData)
	{
		base.OnItemSelected(userData);
		GraphChartBase._E000 obj = userData as GraphChartBase._E000;
		this.m__E001.Add(obj.Category);
	}

	protected override void OnItemLeave(object userData)
	{
		if (userData is GraphChartBase._E000 obj)
		{
			string category = obj.Category;
			this.m__E001.Remove(category);
			this.m__E001.RemoveWhere((string x) => !Data.HasCategory(x));
			if (this.m__E001.Count == 0 && NonHovered != null)
			{
				NonHovered.Invoke();
			}
		}
	}

	private void _E009()
	{
		RectTransform component = GetComponent<RectTransform>();
		GameObject gameObject = (this.m__E009 = new GameObject());
		_ED15._E003(gameObject, hideHierarchy);
		gameObject.AddComponent<ChartItem>();
		gameObject.transform.position = base.transform.position;
		while (base.gameObject.transform.childCount > 0)
		{
			base.transform.GetChild(0).SetParent(gameObject.transform, worldPositionStays: false);
		}
		gameObject.transform.SetParent(base.transform, worldPositionStays: false);
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		float val = component.rect.size.x / base.WidthRatio;
		float val2 = component.rect.size.y / base.HeightRatio;
		float num = Math.Min(val, val2);
		gameObject.transform.localScale = new Vector3(num, num, num);
		gameObject.transform.localPosition = new Vector3((0f - base.WidthRatio) * num * 0.5f, (0f - base.HeightRatio) * num * 0.5f, 0f);
		this.m__E000 = component.rect.size;
	}

	[CompilerGenerated]
	private bool _E00A(string x)
	{
		return !Data.HasCategory(x);
	}
}
