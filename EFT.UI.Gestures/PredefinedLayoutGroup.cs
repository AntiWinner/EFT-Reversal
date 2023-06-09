using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Gestures;

[RequireComponent(typeof(RectTransform))]
[ExecuteAlways]
public sealed class PredefinedLayoutGroup : LayoutGroup
{
	public interface _E000
	{
		void AlignToCenter();

		void AlignToLeft();

		void AlignToRight();
	}

	private static readonly Vector2 m__E000 = 0.5f * Vector2.one;

	[SerializeField]
	private Vector2[] PositionsToCenter = new Vector2[0];

	[SerializeField]
	private bool UpdateSideAlignment;

	public override void CalculateLayoutInputVertical()
	{
	}

	public override void SetLayoutHorizontal()
	{
		_E000();
	}

	public override void SetLayoutVertical()
	{
		_E000();
	}

	private void _E000()
	{
		if (PositionsToCenter.Length == 0)
		{
			return;
		}
		int count = base.rectChildren.Count;
		if (count > PositionsToCenter.Length)
		{
			Debug.LogError(string.Format(_ED3E._E000(229844), _ED3E._E000(229870), base.rectChildren.Count, _ED3E._E000(229859), PositionsToCenter.Length));
			return;
		}
		for (int i = 0; i < count; i++)
		{
			Vector2 anchoredPosition = PositionsToCenter[i];
			RectTransform rectTransform = base.rectChildren[i];
			m_Tracker.Add(this, rectTransform, DrivenTransformProperties.Anchors | DrivenTransformProperties.AnchoredPosition);
			rectTransform.anchorMin = PredefinedLayoutGroup.m__E000;
			rectTransform.anchorMax = PredefinedLayoutGroup.m__E000;
			rectTransform.anchoredPosition = anchoredPosition;
			if (UpdateSideAlignment)
			{
				_E001(rectTransform, anchoredPosition);
			}
		}
	}

	private static void _E001(RectTransform child, Vector2 anchoredPosition)
	{
		_E000 component = child.GetComponent<_E000>();
		if (component != null)
		{
			if (anchoredPosition.x > 0f)
			{
				component.AlignToLeft();
			}
			else if (anchoredPosition.x < 0f)
			{
				component.AlignToRight();
			}
			else
			{
				component.AlignToCenter();
			}
		}
	}
}
