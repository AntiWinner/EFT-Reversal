using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class RequiredSkillOrLevel : UIElement
{
	[SerializeField]
	private TextMeshProUGUI _text;

	[SerializeField]
	private Image _icon;

	[SerializeField]
	private Color _goodColor;

	[SerializeField]
	private Color _badColor;

	public TMP_Text Text => _text;

	public void Show(string text, bool available, Sprite icon = null, float textWidth = -1f)
	{
		ShowGameObject();
		_text.color = (available ? _goodColor : _badColor);
		_text.text = text;
		if (icon != null)
		{
			_icon.sprite = icon;
		}
		if (textWidth > 0f)
		{
			Vector2 sizeDelta = ((RectTransform)_text.transform).sizeDelta;
			sizeDelta.x = textWidth;
			((RectTransform)_text.transform).sizeDelta = sizeDelta;
			_text.overflowMode = TextOverflowModes.Ellipsis;
			Vector2 sizeDelta2 = ((RectTransform)base.transform).sizeDelta;
			sizeDelta2.x = textWidth + ((RectTransform)_icon.transform).sizeDelta.x + 10f;
			((RectTransform)base.transform).sizeDelta = sizeDelta2;
		}
	}
}
