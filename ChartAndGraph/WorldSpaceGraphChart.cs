using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ChartAndGraph;

public class WorldSpaceGraphChart : GraphChartBase
{
	[SerializeField]
	private float _textIdleDistance = 20f;

	[SerializeField]
	[Tooltip("If this value is set all the text in the chart will be rendered to this specific camera. otherwise text is rendered to the main camera")]
	private Camera _textCamera;

	private readonly Dictionary<string, List<BillboardText>> _E00F = new Dictionary<string, List<BillboardText>>();

	private float _E010;

	private GameObject _E011;

	private readonly HashSet<BillboardText> _E012 = new HashSet<BillboardText>();

	public Camera TextCamera
	{
		get
		{
			return _textCamera;
		}
		set
		{
			_textCamera = value;
			OnPropertyUpdated();
		}
	}

	protected override Camera TextCameraLink => TextCamera;

	public float TextIdleDistance
	{
		get
		{
			return _textIdleDistance;
		}
		set
		{
			_textIdleDistance = value;
			OnPropertyUpdated();
		}
	}

	protected override float TextIdleDistanceLink => TextIdleDistance;

	protected override float TotalDepthLink => _E010;

	protected override void OnPropertyUpdated()
	{
		base.OnPropertyUpdated();
		Invalidate();
	}

	private GameObject _E000(GraphData.CategoryData data)
	{
		GameObject gameObject = data.DotPrefab;
		if (gameObject == null)
		{
			if (_E011 == null)
			{
				_E011 = (GameObject)_E3A2.Load(_ED3E._E000(244695));
			}
			gameObject = _E011;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
		_ED15._E003(gameObject2, hideHierarchy);
		if (gameObject2.GetComponent<ChartItem>() == null)
		{
			gameObject2.AddComponent<ChartItem>();
		}
		gameObject2.transform.SetParent(base.transform);
		gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject2.transform.localPosition = Vector3.zero;
		gameObject2.transform.localRotation = Quaternion.identity;
		return gameObject2;
	}

	private FillPathGenerator _E001(GraphData.CategoryData data)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(data.FillPrefab.gameObject);
		_ED15._E003(gameObject, hideHierarchy);
		FillPathGenerator component = gameObject.GetComponent<FillPathGenerator>();
		if (gameObject.GetComponent<ChartItem>() == null)
		{
			gameObject.AddComponent<ChartItem>();
		}
		gameObject.transform.SetParent(base.transform);
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		return component;
	}

	private PathGenerator _E002(GraphData.CategoryData data)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(data.LinePrefab.gameObject);
		_ED15._E003(gameObject, hideHierarchy);
		PathGenerator component = gameObject.GetComponent<PathGenerator>();
		if (gameObject.GetComponent<ChartItem>() == null)
		{
			gameObject.AddComponent<ChartItem>();
		}
		gameObject.transform.SetParent(base.transform);
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		return component;
	}

	protected override void OnNonHoverted()
	{
		base.OnNonHoverted();
		foreach (BillboardText item in _E012)
		{
			if (!(item.UIText == null))
			{
				ChartItemEffect[] components = item.UIText.GetComponents<ChartItemEffect>();
				for (int i = 0; i < components.Length; i++)
				{
					components[i].TriggerOut(deactivateOnEnd: false);
				}
			}
		}
		_E012.Clear();
		if (NonHovered != null)
		{
			NonHovered.Invoke();
		}
	}

	protected override void OnItemHoverted(object userData)
	{
		base.OnItemHoverted(userData);
		if (!(userData is _E000 obj))
		{
			return;
		}
		foreach (BillboardText item in _E012)
		{
			if (!(item.UIText == null))
			{
				ChartItemEffect[] components = item.UIText.GetComponents<ChartItemEffect>();
				for (int i = 0; i < components.Length; i++)
				{
					components[i].TriggerOut(deactivateOnEnd: false);
				}
			}
		}
		_E012.Clear();
		if (!_E00F.TryGetValue(obj.Category, out var value) || obj.Index >= value.Count)
		{
			return;
		}
		BillboardText billboardText = value[obj.Index];
		_E012.Add(billboardText);
		Text uIText = billboardText.UIText;
		if (uIText != null)
		{
			ChartItemEffect[] components = uIText.GetComponents<ChartItemEffect>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].TriggerIn(deactivateOnEnd: false);
			}
		}
	}

	private void _E003(string cat, BillboardText text)
	{
		if (!_E00F.TryGetValue(cat, out var value))
		{
			value = new List<BillboardText>();
			_E00F.Add(cat, value);
		}
		value.Add(text);
	}

	protected override void ClearChart()
	{
		base.ClearChart();
		_E00F.Clear();
		_E012.Clear();
	}

	public override void GenerateRealtime()
	{
		base.GenerateRealtime();
		Debug.Log(_ED3E._E000(244724));
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
		double x2 = (float)((IInternalGraphData)Data).GetMinValue(0, dataValue: false);
		double y = (float)((IInternalGraphData)Data).GetMinValue(1, dataValue: false);
		double x3 = (float)((IInternalGraphData)Data).GetMaxValue(0, dataValue: false);
		double y2 = (float)((IInternalGraphData)Data).GetMaxValue(1, dataValue: false);
		_ED18 min = new _ED18(x2, y);
		_ED18 max = new _ED18(x3, y2);
		Rect rect = new Rect(0f, 0f, widthRatio, heightRatio);
		int num = 0;
		int num2 = ((IInternalGraphData)Data).TotalCategories + 1;
		double num3 = 0.0;
		double num4 = 0.0;
		bool flag = false;
		_E00F.Clear();
		_E012.Clear();
		foreach (GraphData.CategoryData category in ((IInternalGraphData)Data).Categories)
		{
			num4 = Math.Max(category.LineThickness, num4);
			_ED18[] array = category.getPoints().ToArray();
			TransformPoints(array, rect, min, max);
			if (array.Length == 0 && _ED15._E003)
			{
				flag = true;
				int num5 = num2 - 1 - num;
				float num6 = (float)num5 / (float)num2;
				float num7 = ((float)num5 + 1f) / (float)num2;
				_ED18 obj = interpolateInRect(rect, new _ED18(0.0, num6, -1.0)).ToDoubleVector3();
				_ED18 obj2 = interpolateInRect(rect, new _ED18(0.5, num7, -1.0)).ToDoubleVector3();
				_ED18 obj3 = interpolateInRect(rect, new _ED18(1.0, num6, -1.0)).ToDoubleVector3();
				array = new _ED18[3] { obj, obj2, obj3 };
				num++;
			}
			if (category.Depth > 0.0)
			{
				num3 = Math.Max(num3, category.Depth);
			}
			for (int i = 0; i < array.Length; i++)
			{
				_ED18 obj4 = array[i];
				if (!flag)
				{
					obj4 = Data.GetPoint(category.Name, i);
				}
				string text = StringFromAxisFormat(obj4.x, mHorizontalAxis);
				string text2 = StringFromAxisFormat(obj4.y, mVerticalAxis);
				_E000 userData = new _E000(i, (array[i] + new _ED18(0.0, 0.0, category.Depth)).ToVector3(), obj4.ToDoubleVector2(), (float)obj4.z, category.Name, text, text2);
				GameObject gameObject = _E000(category);
				ChartItemEvents[] componentsInChildren = gameObject.GetComponentsInChildren<ChartItemEvents>();
				foreach (ChartItemEvents chartItemEvents in componentsInChildren)
				{
					if (!(chartItemEvents == null))
					{
						((_ED1D)chartItemEvents).Parent = this;
						((_ED1D)chartItemEvents).UserData = userData;
					}
				}
				double num8 = array[i].z * category.PointSize;
				if (num8 < 0.0)
				{
					num8 = category.PointSize;
				}
				gameObject.transform.localScale = new _ED18(num8, num8, num8).ToVector3();
				if (category.PointMaterial != null)
				{
					Renderer component = gameObject.GetComponent<Renderer>();
					if (component != null)
					{
						component.material = category.PointMaterial;
					}
					ChartMaterialController component2 = gameObject.GetComponent<ChartMaterialController>();
					if (component2 != null && component2._E00C != null)
					{
						Color hover = component2._E00C.Hover;
						Color selected = component2._E00C.Selected;
						component2._E00C = new ChartDynamicMaterial(category.PointMaterial, hover, selected);
					}
				}
				_ED18 obj5 = array[i];
				obj5.z = category.Depth;
				gameObject.transform.localPosition = obj5.ToVector3();
				if (mItemLabels != null && mItemLabels.isActiveAndEnabled)
				{
					Vector3 vector = (array[i] + new _ED18(mItemLabels.Location.Breadth, mItemLabels.Seperation, (double)mItemLabels.Location.Depth + category.Depth)).ToVector3();
					if (mItemLabels.Alignment == ChartLabelAlignment.Base)
					{
						vector.y -= (float)array[i].y;
					}
					string text3 = mItemLabels.TextFormat.Format(string.Format(_ED3E._E000(71425), text, text2), category.Name, "");
					BillboardText billboardText = _ED15._E017(null, mItemLabels.TextPrefab, base.transform, text3, vector.x, vector.y, vector.z, 0f, null, hideHierarchy, mItemLabels.FontSize, mItemLabels.FontSharpness);
					base.TextController.AddText(billboardText);
					_E003(category.Name, billboardText);
				}
			}
			for (int k = 0; k < array.Length; k++)
			{
				array[k].z = 0.0;
			}
			Vector3[] path = array.Select((_ED18 x) => x.ToVector3()).ToArray();
			if (category.LinePrefab != null)
			{
				PathGenerator pathGenerator = _E002(category);
				pathGenerator.Generator(path, (float)category.LineThickness, closed: false);
				Vector3 localPosition = pathGenerator.transform.localPosition;
				localPosition.z = (float)category.Depth;
				pathGenerator.transform.localPosition = localPosition;
				if (category.LineMaterial != null)
				{
					Renderer component3 = pathGenerator.GetComponent<Renderer>();
					if (component3 != null)
					{
						component3.material = category.LineMaterial;
					}
					ChartMaterialController component4 = pathGenerator.GetComponent<ChartMaterialController>();
					if (component4 != null && component4._E00C != null)
					{
						Color hover2 = component4._E00C.Hover;
						Color selected2 = component4._E00C.Selected;
						component4._E00C = new ChartDynamicMaterial(category.LineMaterial, hover2, selected2);
					}
				}
			}
			_E010 = (float)(num3 + num4 * 2.0);
			if (!(category.FillPrefab != null))
			{
				continue;
			}
			FillPathGenerator fillPathGenerator = _E001(category);
			Vector3 localPosition2 = fillPathGenerator.transform.localPosition;
			localPosition2.z = (float)category.Depth;
			fillPathGenerator.transform.localPosition = localPosition2;
			if (category.LinePrefab == null || !(category.LinePrefab is SmoothPathGenerator))
			{
				fillPathGenerator.SetLineSmoothing(hasParent: false, 0, 0f);
			}
			else
			{
				SmoothPathGenerator smoothPathGenerator = (SmoothPathGenerator)category.LinePrefab;
				fillPathGenerator.SetLineSmoothing(hasParent: true, smoothPathGenerator.JointSmoothing, smoothPathGenerator.JointSize);
			}
			fillPathGenerator.SetGraphBounds(rect.yMin, rect.yMax);
			fillPathGenerator.SetStrechFill(category.StetchFill);
			fillPathGenerator.Generator(path, (float)category.LineThickness * 1.01f, closed: false);
			if (category.FillMaterial != null)
			{
				Renderer component5 = fillPathGenerator.GetComponent<Renderer>();
				if (component5 != null)
				{
					component5.material = category.FillMaterial;
				}
				ChartMaterialController component6 = fillPathGenerator.GetComponent<ChartMaterialController>();
				if (component6 != null && component6._E00C != null)
				{
					Color hover3 = component6._E00C.Hover;
					Color selected3 = component6._E00C.Selected;
					component6._E00C = new ChartDynamicMaterial(category.FillMaterial, hover3, selected3);
				}
			}
		}
		GenerateAxis(force: true);
	}
}
