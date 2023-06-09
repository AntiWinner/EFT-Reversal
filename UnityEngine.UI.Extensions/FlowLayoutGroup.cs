using System.Collections.Generic;

namespace UnityEngine.UI.Extensions;

[AddComponentMenu("Layout/Extensions/Flow Layout Group")]
public class FlowLayoutGroup : LayoutGroup
{
	public float SpacingX;

	public float SpacingY;

	public bool ExpandHorizontalSpacing;

	public bool ChildForceExpandWidth;

	public bool ChildForceExpandHeight;

	private float m__E000;

	private readonly IList<RectTransform> _E001 = new List<RectTransform>();

	protected bool IsCenterAlign
	{
		get
		{
			if (base.childAlignment != TextAnchor.LowerCenter && base.childAlignment != TextAnchor.MiddleCenter)
			{
				return base.childAlignment == TextAnchor.UpperCenter;
			}
			return true;
		}
	}

	protected bool IsRightAlign
	{
		get
		{
			if (base.childAlignment != TextAnchor.LowerRight && base.childAlignment != TextAnchor.MiddleRight)
			{
				return base.childAlignment == TextAnchor.UpperRight;
			}
			return true;
		}
	}

	protected bool IsMiddleAlign
	{
		get
		{
			if (base.childAlignment != TextAnchor.MiddleLeft && base.childAlignment != TextAnchor.MiddleRight)
			{
				return base.childAlignment == TextAnchor.MiddleCenter;
			}
			return true;
		}
	}

	protected bool IsLowerAlign
	{
		get
		{
			if (base.childAlignment != TextAnchor.LowerLeft && base.childAlignment != TextAnchor.LowerRight)
			{
				return base.childAlignment == TextAnchor.LowerCenter;
			}
			return true;
		}
	}

	public override void CalculateLayoutInputHorizontal()
	{
		base.CalculateLayoutInputHorizontal();
		float totalMin = GetGreatestMinimumChildWidth() + (float)base.padding.left + (float)base.padding.right;
		SetLayoutInputForAxis(totalMin, -1f, -1f, 0);
	}

	public override void SetLayoutHorizontal()
	{
		SetLayout(base.rectTransform.rect.width, 0, layoutInput: false);
	}

	public override void SetLayoutVertical()
	{
		SetLayout(base.rectTransform.rect.width, 1, layoutInput: false);
	}

	public override void CalculateLayoutInputVertical()
	{
		this.m__E000 = SetLayout(base.rectTransform.rect.width, 1, layoutInput: true);
	}

	public float SetLayout(float width, int axis, bool layoutInput)
	{
		float height = base.rectTransform.rect.height;
		float num = base.rectTransform.rect.width - (float)base.padding.left - (float)base.padding.right;
		float num2 = (IsLowerAlign ? ((float)base.padding.bottom) : ((float)base.padding.top));
		float num3 = 0f;
		float num4 = 0f;
		for (int i = 0; i < base.rectChildren.Count; i++)
		{
			int index = (IsLowerAlign ? (base.rectChildren.Count - 1 - i) : i);
			RectTransform rectTransform = base.rectChildren[index];
			float preferredSize = LayoutUtility.GetPreferredSize(rectTransform, 0);
			float preferredSize2 = LayoutUtility.GetPreferredSize(rectTransform, 1);
			preferredSize = Mathf.Min(preferredSize, num);
			if (num3 + preferredSize > num)
			{
				num3 -= SpacingX;
				if (!layoutInput)
				{
					float yOffset = _E000(height, num2, num4);
					LayoutRow(_E001, num3, num4, num, base.padding.left, yOffset, axis);
				}
				_E001.Clear();
				num2 += num4;
				num2 += SpacingY;
				num4 = 0f;
				num3 = 0f;
			}
			num3 += preferredSize;
			_E001.Add(rectTransform);
			if (preferredSize2 > num4)
			{
				num4 = preferredSize2;
			}
			if (i < base.rectChildren.Count - 1)
			{
				num3 += SpacingX;
			}
		}
		if (!layoutInput)
		{
			float yOffset2 = _E000(height, num2, num4);
			num3 -= SpacingX;
			LayoutRow(_E001, num3, num4, num - ((_E001.Count > 1) ? SpacingX : 0f), base.padding.left, yOffset2, axis);
		}
		_E001.Clear();
		num2 += num4;
		num2 += (float)(IsLowerAlign ? base.padding.top : base.padding.bottom);
		if (layoutInput && axis == 1)
		{
			SetLayoutInputForAxis(num2, num2, -1f, axis);
		}
		return num2;
	}

	private float _E000(float groupHeight, float yOffset, float currentRowHeight)
	{
		if (IsLowerAlign)
		{
			return groupHeight - yOffset - currentRowHeight;
		}
		if (IsMiddleAlign)
		{
			return groupHeight * 0.5f - this.m__E000 * 0.5f + yOffset;
		}
		return yOffset;
	}

	protected void LayoutRow(IList<RectTransform> contents, float rowWidth, float rowHeight, float maxWidth, float xOffset, float yOffset, int axis)
	{
		float num = xOffset;
		if (!ChildForceExpandWidth && IsCenterAlign)
		{
			num += (maxWidth - rowWidth) * 0.5f;
		}
		else if (!ChildForceExpandWidth && IsRightAlign)
		{
			num += maxWidth - rowWidth;
		}
		float num2 = 0f;
		float num3 = 0f;
		if (ChildForceExpandWidth)
		{
			num2 = (maxWidth - rowWidth) / (float)_E001.Count;
		}
		else if (ExpandHorizontalSpacing)
		{
			num3 = (maxWidth - rowWidth) / (float)(_E001.Count - 1);
			if (_E001.Count > 1)
			{
				if (IsCenterAlign)
				{
					num -= num3 * 0.5f * (float)(_E001.Count - 1);
				}
				else if (IsRightAlign)
				{
					num -= num3 * (float)(_E001.Count - 1);
				}
			}
		}
		for (int i = 0; i < _E001.Count; i++)
		{
			int index = (IsLowerAlign ? (_E001.Count - 1 - i) : i);
			RectTransform rect = _E001[index];
			float a = LayoutUtility.GetPreferredSize(rect, 0) + num2;
			float num4 = LayoutUtility.GetPreferredSize(rect, 1);
			if (ChildForceExpandHeight)
			{
				num4 = rowHeight;
			}
			a = Mathf.Min(a, maxWidth);
			float num5 = yOffset;
			if (IsMiddleAlign)
			{
				num5 += (rowHeight - num4) * 0.5f;
			}
			else if (IsLowerAlign)
			{
				num5 += rowHeight - num4;
			}
			if (ExpandHorizontalSpacing && i > 0)
			{
				num += num3;
			}
			if (axis == 0)
			{
				SetChildAlongAxis(rect, 0, num, a);
			}
			else
			{
				SetChildAlongAxis(rect, 1, num5, num4);
			}
			if (i < _E001.Count - 1)
			{
				num += a + SpacingX;
			}
		}
	}

	public float GetGreatestMinimumChildWidth()
	{
		float num = 0f;
		for (int i = 0; i < base.rectChildren.Count; i++)
		{
			num = Mathf.Max(LayoutUtility.GetMinWidth(base.rectChildren[i]), num);
		}
		return num;
	}
}
