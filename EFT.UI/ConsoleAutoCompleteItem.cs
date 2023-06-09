using EFT.Console.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class ConsoleAutoCompleteItem : MonoBehaviour
{
	[SerializeField]
	private Color _activeColor;

	[SerializeField]
	private Color _inactiveColor;

	[SerializeField]
	private TMP_Text _text;

	[SerializeField]
	private Image _background;

	private bool _E000;

	public bool IsSelected
	{
		get
		{
			return _E000;
		}
		set
		{
			if (_E000 != value)
			{
				_E000 = value;
				_background.color = (_E000 ? _activeColor : _inactiveColor);
			}
		}
	}

	public void Setup(AutoCompleteItem item)
	{
		_text.text = item.Title;
	}
}
