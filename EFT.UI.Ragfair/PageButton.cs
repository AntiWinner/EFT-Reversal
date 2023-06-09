using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Ragfair;

public class PageButton : UIElement
{
	[SerializeField]
	private Image _buttonImage;

	[SerializeField]
	private Color _selectedColor;

	[SerializeField]
	private Color _nonSelectedColor;

	[SerializeField]
	private Button _button;

	[SerializeField]
	private TextMeshProUGUI _label;

	private bool _E377;

	private int _E378;

	private Action<int> _E06D;

	private void Awake()
	{
		_button.onClick.AddListener(delegate
		{
			if (!_E377 && _E06D != null)
			{
				_E06D(_E378);
			}
		});
	}

	public void Show(bool pageSelected, int page, Action<int> onClick)
	{
		_E377 = pageSelected;
		_E378 = page;
		_E06D = onClick;
		_label.text = (page + 1).ToString();
		_buttonImage.color = (pageSelected ? _selectedColor : _nonSelectedColor);
		ShowGameObject();
	}

	[CompilerGenerated]
	private void _E000()
	{
		if (!_E377 && _E06D != null)
		{
			_E06D(_E378);
		}
	}
}
