using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using ChartAndGraph.Axis;
using UnityEngine;
using UnityEngine.UI;

namespace ChartAndGraph;

[Serializable]
public abstract class AnyChart : MonoBehaviour, IInternalUse
{
	private bool mGenerating;

	private Dictionary<int, string> mHorizontalValueToStringMap = new Dictionary<int, string>();

	private Dictionary<int, string> mVerticalValueToStringMap = new Dictionary<int, string>();

	[SerializeField]
	private bool keepOrthoSize;

	[SerializeField]
	private bool vRSpaceText;

	[SerializeField]
	private float vRSpaceScale = 0.1f;

	private HashSet<object> mHovered = new HashSet<object>();

	protected ItemLabels mItemLabels;

	protected VerticalAxis mVerticalAxis;

	protected HorizontalAxis mHorizontalAxis;

	protected CategoryLabels mCategoryLabels;

	protected GroupLabels mGroupLabels;

	protected GameObject VerticalMainDevisions;

	protected GameObject VerticalSubDevisions;

	protected GameObject HorizontalMainDevisions;

	protected GameObject HorizontalSubDevisions;

	private bool mGenerateOnNextUpdate;

	protected bool hideHierarchy = true;

	private List<_ED2E> mAxis = new List<_ED2E>();

	[CompilerGenerated]
	private Action ChartGenerated;

	public Dictionary<int, string> VerticalValueToStringMap => mVerticalValueToStringMap;

	public Dictionary<int, string> HorizontalValueToStringMap => mHorizontalValueToStringMap;

	protected virtual Camera TextCameraLink => null;

	protected virtual float TextIdleDistanceLink => 20f;

	public bool KeepOrthoSize
	{
		get
		{
			return keepOrthoSize;
		}
		set
		{
			keepOrthoSize = value;
			GenerateChart();
		}
	}

	public bool VRSpaceText
	{
		get
		{
			return vRSpaceText;
		}
		set
		{
			vRSpaceText = value;
			GenerateChart();
		}
	}

	public float VRSpaceScale
	{
		get
		{
			return vRSpaceScale;
		}
		set
		{
			vRSpaceScale = value;
			GenerateChart();
		}
	}

	protected bool IsUnderCanvas { get; private set; }

	protected bool CanvasChanged { get; private set; }

	protected abstract float TotalDepthLink { get; }

	protected abstract float TotalHeightLink { get; }

	protected abstract float TotalWidthLink { get; }

	bool IInternalUse.HideHierarchy => hideHierarchy;

	protected bool Invalidating => mGenerateOnNextUpdate;

	protected TextController TextController { get; private set; }

	ItemLabels IInternalUse.ItemLabels
	{
		get
		{
			return mItemLabels;
		}
		set
		{
			if (mItemLabels != null)
			{
				((IInternalSettings)mItemLabels).InternalOnDataUpdate -= _E002;
				((IInternalSettings)mItemLabels).InternalOnDataChanged -= _E001;
			}
			mItemLabels = value;
			if (mItemLabels != null)
			{
				((IInternalSettings)mItemLabels).InternalOnDataUpdate += _E002;
				((IInternalSettings)mItemLabels).InternalOnDataChanged += _E001;
			}
			OnLabelSettingsSet();
		}
	}

	VerticalAxis IInternalUse.VerticalAxis
	{
		get
		{
			return mVerticalAxis;
		}
		set
		{
			if (mVerticalAxis != null)
			{
				((IInternalSettings)mVerticalAxis).InternalOnDataChanged -= _E000;
				((IInternalSettings)mVerticalAxis).InternalOnDataUpdate -= _E000;
			}
			mVerticalAxis = value;
			if (mVerticalAxis != null)
			{
				((IInternalSettings)mVerticalAxis).InternalOnDataChanged += _E000;
				((IInternalSettings)mVerticalAxis).InternalOnDataUpdate += _E000;
			}
			OnAxisValuesChanged();
		}
	}

	HorizontalAxis IInternalUse.HorizontalAxis
	{
		get
		{
			return mHorizontalAxis;
		}
		set
		{
			if (mHorizontalAxis != null)
			{
				((IInternalSettings)mHorizontalAxis).InternalOnDataChanged -= _E000;
				((IInternalSettings)mHorizontalAxis).InternalOnDataUpdate -= _E000;
			}
			mHorizontalAxis = value;
			if (mHorizontalAxis != null)
			{
				((IInternalSettings)mHorizontalAxis).InternalOnDataChanged += _E000;
				((IInternalSettings)mHorizontalAxis).InternalOnDataUpdate += _E000;
			}
			OnAxisValuesChanged();
		}
	}

	CategoryLabels IInternalUse.CategoryLabels
	{
		get
		{
			return mCategoryLabels;
		}
		set
		{
			if (mCategoryLabels != null)
			{
				((IInternalSettings)mCategoryLabels).InternalOnDataUpdate -= _E002;
				((IInternalSettings)mCategoryLabels).InternalOnDataChanged -= _E001;
			}
			mCategoryLabels = value;
			if (mCategoryLabels != null)
			{
				((IInternalSettings)mCategoryLabels).InternalOnDataUpdate += _E002;
				((IInternalSettings)mCategoryLabels).InternalOnDataChanged += _E001;
			}
			OnLabelSettingsSet();
		}
	}

	GroupLabels IInternalUse.GroupLabels
	{
		get
		{
			return mGroupLabels;
		}
		set
		{
			if (mGroupLabels != null)
			{
				((IInternalSettings)mGroupLabels).InternalOnDataUpdate -= _E002;
				((IInternalSettings)mGroupLabels).InternalOnDataChanged -= _E001;
			}
			mGroupLabels = value;
			if (mGroupLabels != null)
			{
				((IInternalSettings)mGroupLabels).InternalOnDataUpdate += _E002;
				((IInternalSettings)mGroupLabels).InternalOnDataChanged += _E001;
			}
			OnLabelSettingsSet();
		}
	}

	TextController IInternalUse.InternalTextController => TextController;

	Camera IInternalUse.InternalTextCamera => TextCameraLink;

	float IInternalUse.InternalTextIdleDistance => TextIdleDistanceLink;

	float IInternalUse.InternalTotalDepth => TotalDepthLink;

	float IInternalUse.InternalTotalWidth => TotalWidthLink;

	float IInternalUse.InternalTotalHeight => TotalHeightLink;

	protected abstract bool SupportsCategoryLabels { get; }

	protected abstract bool SupportsItemLabels { get; }

	protected abstract bool SupportsGroupLables { get; }

	bool IInternalUse.InternalSupportsCategoryLables => SupportsCategoryLabels;

	bool IInternalUse.InternalSupportsGroupLabels => SupportsGroupLables;

	bool IInternalUse.InternalSupportsItemLabels => SupportsItemLabels;

	private event Action _E000
	{
		[CompilerGenerated]
		add
		{
			Action action = ChartGenerated;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref ChartGenerated, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = ChartGenerated;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref ChartGenerated, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	event Action IInternalUse.Generated
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

	private void _E000(object sender, EventArgs e)
	{
		GenerateChart();
	}

	protected virtual void OnPropertyUpdated()
	{
		ValidateProperties();
	}

	private void _E001(object sender, EventArgs e)
	{
		OnLabelSettingsSet();
	}

	private void _E002(object sender, EventArgs e)
	{
		OnLabelSettingChanged();
	}

	protected virtual void OnLabelSettingChanged()
	{
	}

	protected virtual double GetScrollOffset(int axis)
	{
		return 0.0;
	}

	protected virtual void OnAxisValuesChanged()
	{
	}

	protected virtual void OnLabelSettingsSet()
	{
	}

	protected virtual void Start()
	{
		if (base.gameObject.activeInHierarchy)
		{
			_E003(start: true);
			_E005();
		}
	}

	private void _E003(bool start)
	{
		Canvas componentInParent = GetComponentInParent<Canvas>();
		bool isUnderCanvas = IsUnderCanvas;
		IsUnderCanvas = componentInParent != null;
		if (IsUnderCanvas && !start && IsUnderCanvas != isUnderCanvas)
		{
			CanvasChanged = true;
			GenerateChart();
			CanvasChanged = false;
		}
	}

	private void _E004()
	{
		GameObject gameObject = new GameObject(_ED3E._E000(245647), typeof(RectTransform));
		_ED15._E003(gameObject, hideHierarchy);
		gameObject.transform.SetParent(base.transform);
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localPosition = Vector3.zero;
		TextController = gameObject.AddComponent<TextController>();
		TextController._E002 = this;
	}

	private void _E005()
	{
		if (!(TextController != null))
		{
			_E004();
		}
	}

	protected virtual void Invalidate()
	{
		if (!mGenerating)
		{
			mGenerateOnNextUpdate = true;
		}
	}

	protected virtual void Update()
	{
		if (base.gameObject.activeInHierarchy)
		{
			_E003(start: false);
			if (mGenerateOnNextUpdate)
			{
				mGenerateOnNextUpdate = false;
				GenerateChart();
			}
		}
	}

	protected virtual void LateUpdate()
	{
	}

	protected virtual void OnValidate()
	{
		if (base.gameObject.activeInHierarchy)
		{
			ValidateProperties();
			_E003(start: true);
			_E005();
		}
	}

	protected abstract bool HasValues(AxisBase axis);

	protected abstract double MaxValue(AxisBase axis);

	protected abstract double MinValue(AxisBase axis);

	protected virtual void OnEnable()
	{
		ChartItem[] componentsInChildren = GetComponentsInChildren<ChartItem>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i] != null && componentsInChildren[i].gameObject != base.gameObject)
			{
				componentsInChildren[i].gameObject.SetActive(value: true);
			}
		}
	}

	protected virtual void OnDisable()
	{
		ChartItem[] componentsInChildren = GetComponentsInChildren<ChartItem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i] != null && componentsInChildren[i].gameObject != base.gameObject)
			{
				componentsInChildren[i].gameObject.SetActive(value: false);
			}
		}
	}

	public void GenerateChart()
	{
		if (mGenerating)
		{
			return;
		}
		mGenerating = true;
		InternalGenerateChart();
		Transform[] componentsInChildren = base.gameObject.GetComponentsInChildren<Transform>();
		foreach (Transform transform in componentsInChildren)
		{
			if (!(transform == null) && !(transform.gameObject == null))
			{
				transform.gameObject.layer = base.gameObject.layer;
			}
		}
		mGenerating = false;
	}

	public virtual void InternalGenerateChart()
	{
		if (base.gameObject.activeInHierarchy)
		{
			mGenerateOnNextUpdate = false;
			if (ChartGenerated != null)
			{
				ChartGenerated();
			}
		}
	}

	protected virtual void ClearChart()
	{
		mHovered.Clear();
		if (TextController != null)
		{
			TextController.DestroyAll();
			TextController.transform.SetParent(base.transform, worldPositionStays: false);
		}
		ChartItem[] componentsInChildren = GetComponentsInChildren<ChartItem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i] != null)
			{
				RectMask2D component = componentsInChildren[i].GetComponent<RectMask2D>();
				if (component != null)
				{
					Debug.Log(component.gameObject);
				}
				if ((!(TextController != null) || !(componentsInChildren[i].gameObject == TextController.gameObject)) && componentsInChildren[i].gameObject != base.gameObject)
				{
					_ED15.SafeDestroy(componentsInChildren[i].gameObject);
				}
			}
		}
		_E005();
		for (int j = 0; j < mAxis.Count; j++)
		{
			if (mAxis[j] != null && mAxis[j].This() != null)
			{
				_ED15.SafeDestroy(mAxis[j].GetGameObject());
			}
		}
		mAxis.Clear();
	}

	protected virtual void OnNonHoverted()
	{
	}

	protected virtual void OnItemLeave(object userData)
	{
		if (mHovered.Count != 0)
		{
			mHovered.Remove(userData);
			if (mHovered.Count == 0)
			{
				OnNonHoverted();
			}
		}
	}

	protected virtual void OnItemSelected(object userData)
	{
	}

	protected virtual void OnItemHoverted(object userData)
	{
		mHovered.Add(userData);
	}

	protected internal virtual _ED2E InternalUpdateAxis(ref GameObject axisObject, AxisBase axisBase, ChartOrientation axisOrientation, bool isSubDiv, bool forceRecreate, double scrollOffset)
	{
		_ED2E obj = null;
		if (axisObject == null || forceRecreate || CanvasChanged)
		{
			_ED15.SafeDestroy(axisObject);
			GameObject gameObject = null;
			gameObject = ((!IsUnderCanvas) ? _ED15._E002() : _ED15._E001());
			gameObject.transform.SetParent(base.transform, worldPositionStays: false);
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localPosition = default(Vector3);
			gameObject.layer = base.gameObject.layer;
			_ED15._E003(gameObject, hideHierarchy);
			axisObject = gameObject;
			obj = ((!IsUnderCanvas) ? ((_ED2E)gameObject.AddComponent<AxisGenerator>()) : ((_ED2E)gameObject.AddComponent<CanvasAxisGenerator>()));
		}
		else
		{
			obj = ((!IsUnderCanvas) ? ((_ED2E)axisObject.GetComponent<AxisGenerator>()) : ((_ED2E)axisObject.GetComponent<CanvasAxisGenerator>()));
		}
		obj.SetAxis(scrollOffset, this, axisBase, axisOrientation, isSubDiv);
		return obj;
	}

	protected virtual void ValidateProperties()
	{
		if (mItemLabels != null)
		{
			mItemLabels.ValidateProperties();
		}
		if (mCategoryLabels != null)
		{
			mCategoryLabels.ValidateProperties();
		}
		if (mGroupLabels != null)
		{
			mGroupLabels.ValidateProperties();
		}
		if (mHorizontalAxis != null)
		{
			mHorizontalAxis.ValidateProperties();
		}
		if (mVerticalAxis != null)
		{
			mVerticalAxis.ValidateProperties();
		}
	}

	protected void GenerateAxis(bool force)
	{
		mAxis.Clear();
		if ((bool)mVerticalAxis)
		{
			double scrollOffset = GetScrollOffset(1);
			_ED2E obj = InternalUpdateAxis(ref VerticalMainDevisions, mVerticalAxis, ChartOrientation.Vertical, isSubDiv: false, force, scrollOffset);
			_ED2E obj2 = InternalUpdateAxis(ref VerticalSubDevisions, mVerticalAxis, ChartOrientation.Vertical, isSubDiv: true, force, scrollOffset);
			if (obj != null)
			{
				mAxis.Add(obj);
			}
			if (obj2 != null)
			{
				mAxis.Add(obj2);
			}
		}
		if ((bool)mHorizontalAxis)
		{
			double scrollOffset2 = GetScrollOffset(0);
			_ED2E obj3 = InternalUpdateAxis(ref HorizontalMainDevisions, mHorizontalAxis, ChartOrientation.Horizontal, isSubDiv: false, force, scrollOffset2);
			_ED2E obj4 = InternalUpdateAxis(ref HorizontalSubDevisions, mHorizontalAxis, ChartOrientation.Horizontal, isSubDiv: true, force, scrollOffset2);
			if (obj3 != null)
			{
				mAxis.Add(obj3);
			}
			if (obj4 != null)
			{
				mAxis.Add(obj4);
			}
		}
	}

	void IInternalUse.InternalItemSelected(object userData)
	{
		OnItemSelected(userData);
	}

	void IInternalUse.InternalItemLeave(object userData)
	{
		OnItemLeave(userData);
	}

	void IInternalUse.InternalItemHovered(object userData)
	{
		OnItemHoverted(userData);
	}

	void IInternalUse.CallOnValidate()
	{
		OnValidate();
	}

	bool IInternalUse.InternalHasValues(AxisBase axis)
	{
		return HasValues(axis);
	}

	double IInternalUse.InternalMaxValue(AxisBase axis)
	{
		return MaxValue(axis);
	}

	double IInternalUse.InternalMinValue(AxisBase axis)
	{
		return MinValue(axis);
	}
}
