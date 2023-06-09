using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

[UsedImplicitly]
[ExecuteInEditMode]
public sealed class ToggleButton : UIElement, IPointerClickHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField]
	private Image _image;

	[SerializeField]
	private Sprite _onSprite;

	[SerializeField]
	private Sprite _offSprite;

	[SerializeField]
	private Graphic _labelOn;

	[SerializeField]
	private Graphic _labelOff;

	[SerializeField]
	private bool _isOn;

	[NonSerialized]
	public readonly _ECED<bool> OnToggle = new _ECED<bool>();

	public bool IsOn
	{
		get
		{
			return _isOn;
		}
		set
		{
			if (_isOn != value)
			{
				_isOn = value;
				_E000(notify: true);
			}
		}
	}

	public void SetValueSilently(bool isOn)
	{
		_isOn = isOn;
		_E000();
	}

	private void _E000(bool notify = false)
	{
		_image.sprite = (IsOn ? _offSprite : _onSprite);
		_labelOn.color = _labelOn.color.SetAlpha((!IsOn) ? 1 : 0);
		_labelOff.color = _labelOff.color.SetAlpha(IsOn ? 1 : 0);
		if (notify)
		{
			OnToggle?.Invoke(_isOn);
		}
	}

	private void OnValidate()
	{
		_E000();
	}

	private void OnEnable()
	{
		_E000();
	}

	void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
	{
		IsOn = !IsOn;
	}

	void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
	{
	}

	void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
	{
	}
}
