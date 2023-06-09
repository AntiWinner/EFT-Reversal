using System;
using UnityEngine;
using UnityEngine.UI;

public class RadialLayoutGroup : LayoutGroup
{
	[SerializeField]
	private float _fDistance;

	[SerializeField]
	[Range(0f, 360f)]
	private float _minAngle;

	[Range(0f, 360f)]
	[SerializeField]
	private float _maxAngle;

	[Range(0f, 360f)]
	public float StartAngle;

	protected override void OnEnable()
	{
		base.OnEnable();
		_E000();
	}

	public override void SetLayoutHorizontal()
	{
		_E000();
	}

	public override void SetLayoutVertical()
	{
		_E000();
	}

	public override void CalculateLayoutInputVertical()
	{
		_E000();
	}

	public override void CalculateLayoutInputHorizontal()
	{
		_E000();
	}

	private void _E000()
	{
		m_Tracker.Clear();
		if (base.transform.ActiveChildCount() == 0)
		{
			return;
		}
		float num = (_maxAngle - _minAngle) / (float)(base.transform.ActiveChildCount() - 1);
		float num2 = StartAngle;
		for (int i = 0; i < base.transform.ActiveChildCount(); i++)
		{
			RectTransform rectTransform = (RectTransform)base.transform.GetChild(i);
			if (rectTransform != null)
			{
				m_Tracker.Add(this, rectTransform, DrivenTransformProperties.Anchors | DrivenTransformProperties.AnchoredPosition | DrivenTransformProperties.Pivot);
				Vector3 vector = new Vector3(Mathf.Cos(num2 * ((float)Math.PI / 180f)), Mathf.Sin(num2 * ((float)Math.PI / 180f)), 0f);
				rectTransform.localPosition = vector * _fDistance;
				Vector2 vector3 = (rectTransform.pivot = new Vector2(0.5f, 0.5f));
				Vector2 anchorMin = (rectTransform.anchorMax = vector3);
				rectTransform.anchorMin = anchorMin;
				num2 += num;
			}
		}
	}
}
