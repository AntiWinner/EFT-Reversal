using System.Globalization;
using EFT.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT;

public class RequiredItem : UIElement
{
	private const string _E00C = " ";

	[SerializeField]
	private TextMeshProUGUI _text;

	[SerializeField]
	private Image _image;

	[SerializeField]
	private Color _normalColor;

	[SerializeField]
	private Color _badColor;

	private NumberFormatInfo _E00D;

	private void Awake()
	{
		_E00D = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
		_E00D.NumberGroupSeparator = _ED3E._E000(18502);
	}

	public void Show(_EBE5 requirement, bool available, Sprite sprite)
	{
		ShowGameObject();
		_text.color = (available ? _normalColor : _badColor);
		_text.text = requirement.count.ToString(_ED3E._E000(189390), _E00D);
		_image.sprite = sprite;
	}
}
