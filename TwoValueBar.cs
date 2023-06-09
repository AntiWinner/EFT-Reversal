using UnityEngine;

public class TwoValueBar : MonoBehaviour
{
	[SerializeField]
	private RectTransform _baseFill;

	[SerializeField]
	private RectTransform _newFill;

	[SerializeField]
	private RectTransform _newGlow;

	[SerializeField]
	private float _spacing;

	private float _E000;

	public void SetBase(float value)
	{
		_baseFill.anchorMax = new Vector2(value, _baseFill.anchorMax.y);
		_E000 = ((value > 0f) ? _spacing : 0f);
	}

	public void SetNew(float value)
	{
		_newFill.anchorMin = new Vector2(_baseFill.anchorMax.x, _newFill.anchorMin.y);
		_newFill.anchorMax = new Vector2(value, _newFill.anchorMax.y);
		_newFill.anchoredPosition = new Vector2(_E000, _newFill.anchoredPosition.y);
		_newFill.sizeDelta = new Vector2(0f - _E000, _newFill.sizeDelta.y);
		if (!(_newGlow == null))
		{
			_newGlow.anchorMin = new Vector2(value, _newFill.anchorMin.y);
			_newGlow.anchorMax = new Vector2(value, _newFill.anchorMax.y);
		}
	}
}
