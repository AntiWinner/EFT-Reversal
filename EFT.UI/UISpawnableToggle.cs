using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class UISpawnableToggle : ButtonFeedback
{
	[SerializeField]
	private CustomTextMeshProUGUI _headerLabel;

	[SerializeField]
	private CustomTextMeshProUGUI _sizeLabel;

	[SerializeField]
	private Image _iconSprite;

	[SerializeField]
	private bool _isBoldOnHover = true;

	private GameObject _E0E1;

	public AnimatedToggle Toggle;

	private FontStyles _E0E2;

	internal bool _E000
	{
		get
		{
			return Toggle._E001;
		}
		set
		{
			Toggle._E001 = value;
		}
	}

	public override bool Interactable
	{
		get
		{
			return base.Interactable;
		}
		set
		{
			base.Interactable = value;
			Toggle.interactable = value;
		}
	}

	internal void _E000(ToggleGroup group)
	{
		_E0E2 = _headerLabel.fontStyle;
		Toggle.group = group;
	}

	internal void _E001(string headerText, int headerSize, [CanBeNull] Sprite sprite, [CanBeNull] GameObject hoverImage)
	{
		_E0E1 = hoverImage;
		_headerLabel.text = headerText.Localized().ToUpper();
		_headerLabel.fontSize = headerSize;
		if (!(_iconSprite == null))
		{
			bool flag = sprite != null;
			if (_sizeLabel != null)
			{
				string text = (flag ? _ED3E._E000(250777) : string.Empty);
				_sizeLabel.text = text + _headerLabel.text + text;
				_sizeLabel.fontSize = headerSize;
			}
			_iconSprite.gameObject.SetActive(flag);
			if (flag)
			{
				_iconSprite.sprite = sprite;
			}
		}
	}

	internal void _E002(bool useEllipsis)
	{
		if (useEllipsis)
		{
			_headerLabel.overflowMode = TextOverflowModes.Ellipsis;
			if (_sizeLabel != null)
			{
				_sizeLabel.overflowMode = TextOverflowModes.Ellipsis;
			}
		}
	}

	internal void _E003(float minWidth)
	{
		LayoutElement component = base.gameObject.GetComponent<LayoutElement>();
		if (component != null)
		{
			component.minWidth = minWidth;
		}
	}

	public void Highlighted()
	{
		if (_isBoldOnHover)
		{
			_headerLabel.fontStyle = _E0E2 | FontStyles.Bold;
		}
		if (_E0E1 != null)
		{
			_E0E1.SetActive(value: true);
		}
	}

	public void Default()
	{
		_headerLabel.fontStyle = _E0E2;
		if (_E0E1 != null)
		{
			_E0E1.SetActive(value: false);
		}
	}
}
