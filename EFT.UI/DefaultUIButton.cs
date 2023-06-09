using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

[RequireComponent(typeof(TweenAnimatedButton))]
public sealed class DefaultUIButton : ButtonFeedback
{
	[SerializeField]
	private Sprite _iconSprite;

	[SerializeField]
	private Sprite _iconIdleSprite;

	[SerializeField]
	private string _text;

	[SerializeField]
	private int _fontSize = 24;

	[SerializeField]
	private float _minWidth = -1f;

	[SerializeField]
	private bool _useEllipsis;

	[SerializeField]
	private string _enabledTooltip;

	[SerializeField]
	private string _disabledTooltip;

	[SerializeField]
	private CustomTextMeshProUGUI _headerLabel;

	[SerializeField]
	private CustomTextMeshProUGUI _sizeLabel;

	[SerializeField]
	private Image _iconImage;

	[SerializeField]
	private Image _iconIdleImage;

	[SerializeField]
	private GameObject _iconContainer;

	[HideInInspector]
	[SerializeField]
	private TweenAnimatedButton _button;

	[HideInInspector]
	[SerializeField]
	private LayoutElement _layoutElement;

	[SerializeField]
	private bool _rawText;

	public readonly UnityEvent OnClick = new UnityEvent();

	public readonly UnityEvent OnMouseOver = new UnityEvent();

	public readonly UnityEvent OnMouseOut = new UnityEvent();

	private Action _E0BC;

	public string HeaderText => _text;

	public int HeaderSize => _fontSize;

	public override bool Interactable
	{
		get
		{
			return base.Interactable;
		}
		set
		{
			base.Interactable = value;
			_button.Interactable = value;
		}
	}

	private void Awake()
	{
		if (Application.isPlaying)
		{
			_E008();
			_button.OnClick += delegate
			{
				OnClick?.Invoke();
			};
		}
	}

	private void OnEnable()
	{
		_E005();
		if (_E0BC == null)
		{
			_E0BC = _E7AD._E010.AddLocaleUpdateListener(_E005);
		}
	}

	private void OnDisable()
	{
		_E0BC?.Invoke();
		_E0BC = null;
	}

	public void SetRawText(string text, int fontSize)
	{
		_E000(text, fontSize, rawText: true);
	}

	public void SetHeaderText(string text, int fontSize)
	{
		_E000(text, fontSize, string.IsNullOrEmpty(text));
	}

	public void SetHeaderText(string text)
	{
		SetHeaderText(text, (int)_headerLabel.fontSize);
	}

	private void _E000(string text, int fontSize, bool rawText)
	{
		_text = text;
		_fontSize = fontSize;
		_rawText = rawText;
		_E003();
		_E005();
	}

	public void SetIcon(Sprite icon, Sprite iconIdle = null)
	{
		_iconSprite = icon;
		_iconIdleSprite = iconIdle;
		_E004();
	}

	private void _E001(string enabledTooltip, string disabledTooltip)
	{
		_enabledTooltip = enabledTooltip ?? string.Empty;
		_disabledTooltip = disabledTooltip ?? string.Empty;
		_E009(this);
	}

	private void _E002()
	{
		if (_minWidth.Positive())
		{
			_layoutElement = base.gameObject.GetOrAddComponent<LayoutElement>();
		}
		if (!(_layoutElement == null))
		{
			_layoutElement.minWidth = _minWidth;
			_E009(_layoutElement);
			_E009(this);
		}
	}

	private void _E003()
	{
		_headerLabel.fontSize = _fontSize;
		_E009(_headerLabel);
		_sizeLabel.fontSize = _fontSize;
		_E009(_sizeLabel);
	}

	private void _E004()
	{
		if (_iconImage != null)
		{
			_iconImage.sprite = _iconSprite;
		}
		_E009(_iconImage);
		if (_iconIdleImage != null)
		{
			_iconIdleImage.sprite = _iconIdleSprite;
		}
		_E009(_iconIdleImage);
		_iconContainer.SetActive(_iconSprite != null || _iconIdleSprite != null);
		_E006();
	}

	private void _E005()
	{
		_headerLabel.text = (_rawText ? _text : _text.Localized()).ToUpper();
		_headerLabel.SetAllDirty();
		_E009(_headerLabel);
		_E006();
	}

	private void _E006()
	{
		if (!(_sizeLabel == null))
		{
			string text = ((_iconSprite != null || _iconIdleSprite != null) ? _ED3E._E000(250777) : string.Empty);
			_sizeLabel.text = text + _headerLabel.text + text;
			_sizeLabel.SetAllDirty();
			_E009(_sizeLabel);
		}
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		base.OnPointerEnter(eventData);
		OnMouseOver?.Invoke();
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		base.OnPointerExit(eventData);
		OnMouseOut?.Invoke();
	}

	private void _E007()
	{
		_headerLabel.overflowMode = (_useEllipsis ? TextOverflowModes.Ellipsis : TextOverflowModes.Masking);
		_E009(_headerLabel);
		_sizeLabel.overflowMode = _headerLabel.overflowMode;
		_E009(_sizeLabel);
	}

	private void _E008()
	{
		_E004();
		_E003();
		_E005();
		_E007();
		_E002();
		_E001(_enabledTooltip, _disabledTooltip);
	}

	private void _E009(UnityEngine.Object unityObject)
	{
	}

	[CompilerGenerated]
	private void _E00A()
	{
		OnClick?.Invoke();
	}
}
