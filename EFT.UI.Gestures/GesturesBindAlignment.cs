using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Gestures;

public abstract class GesturesBindAlignment : MonoBehaviour, PredefinedLayoutGroup._E000
{
	[SerializeField]
	protected RectTransform BindRoot;

	[SerializeField]
	protected Image HotkeyImage;

	[SerializeField]
	private CustomTextMeshProUGUI _textField;

	[SerializeField]
	private Sprite _leftHotkeySprite;

	[SerializeField]
	private Sprite _rightHotkeySprite;

	public virtual void AlignToCenter()
	{
		_textField.alignment = TextAlignmentOptions.Center;
		_E000();
	}

	public virtual void AlignToLeft()
	{
		_textField.alignment = TextAlignmentOptions.Left;
		_E001();
	}

	public virtual void AlignToRight()
	{
		_textField.alignment = TextAlignmentOptions.Right;
		_E000();
	}

	private void _E000()
	{
		RectTransform rectTransform = HotkeyImage.rectTransform;
		if (_E002(rectTransform))
		{
			_E003(rectTransform);
			HotkeyImage.sprite = _leftHotkeySprite;
		}
	}

	private void _E001()
	{
		RectTransform rectTransform = HotkeyImage.rectTransform;
		if (!_E002(rectTransform))
		{
			_E003(rectTransform);
			HotkeyImage.sprite = _rightHotkeySprite;
		}
	}

	private static bool _E002(RectTransform rectTransform)
	{
		return rectTransform.anchorMin.x > 0.5f;
	}

	private static void _E003(RectTransform rectTransform)
	{
		RectTransformUtility.FlipLayoutOnAxis(rectTransform, 0, keepPositioning: false, recursive: false);
	}
}
