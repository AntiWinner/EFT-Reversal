using UnityEngine;

namespace EFT.UI;

public class ResizeTooltip : UIElement
{
	[SerializeField]
	private RectTransform _oldTiles;

	[SerializeField]
	private ColorBlinker _newTiles;

	[SerializeField]
	private RectTransform _upArrow;

	[SerializeField]
	private RectTransform _rightArrow;

	private const int _E17A = 10;

	private static readonly Color _E17B = new Color32(222, 0, 0, byte.MaxValue);

	private Vector2 _E17C;

	private RectTransform _E17D;

	public void Show(_E313 oldSize, _E313 newSize, Vector2 tooltipOffset, RectTransform parentTooltip)
	{
		ShowGameObject();
		_E17C = tooltipOffset;
		_E17D = parentTooltip;
		_E313 cellSize = newSize - oldSize;
		_E313 obj = _E000(cellSize);
		_rightArrow.gameObject.SetActive(cellSize.X > 0);
		_upArrow.gameObject.SetActive(cellSize.Y > 0);
		_rightArrow.anchoredPosition = new Vector2(10 - obj.X + 1, 0f);
		_upArrow.anchoredPosition = new Vector2(0f, obj.Y - 10 - 1);
		_E313 obj2 = _E000(oldSize);
		_E313 obj3 = _E000(newSize);
		_oldTiles.sizeDelta = obj2;
		_oldTiles.anchoredPosition = new Vector2(0f, (obj3 - obj2).Y);
		((RectTransform)base.transform).sizeDelta = obj3;
		_newTiles.EndColor = _E17B;
	}

	private static _E313 _E000(_E313 cellSize)
	{
		return cellSize * 10 + new _E313(1, 1);
	}

	private void Update()
	{
		base.transform.position = _E17D.position - new Vector3(((RectTransform)base.transform).sizeDelta.x, 0f) + (Vector3)_E17C;
	}
}
