using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class ContextMenuButton : SimpleContextMenuButton
{
	[SerializeField]
	private Image _icon;

	[SerializeField]
	private GameObject _iconContainer;

	[SerializeField]
	private TextMeshProUGUI _text;

	[SerializeField]
	private TextMeshProUGUI _iconLabel;

	[SerializeField]
	private GameObject _loader;

	public override void Show(string caption, string imageText, Sprite sprite, Action onClicked, Action onHover, bool subMenu = false, bool hideIcon = false)
	{
		base.Show(caption, imageText, sprite, onClicked, onHover, subMenu);
		_text.text = (caption ?? imageText).Localized(EStringCase.Upper);
		if (_loader != null)
		{
			_loader.SetActive(onClicked == null);
		}
		if (!(_icon == null))
		{
			if (sprite != null)
			{
				_icon.sprite = sprite;
			}
			if (_iconContainer != null)
			{
				_iconContainer.SetActive(!hideIcon);
			}
			_icon.gameObject.SetActive(sprite != null);
			if (_iconLabel != null)
			{
				_iconLabel.text = imageText;
			}
		}
	}
}
