using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.Map;

public sealed class EntryPointView : UIElement, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	private Image _background;

	[SerializeField]
	private Image _glow;

	[SerializeField]
	private CanvasGroup _canvasGroup;

	[SerializeField]
	private CustomTextMeshProUGUI _nameLabel;

	[SerializeField]
	private CustomTextMeshProUGUI _indexLabel;

	[SerializeField]
	private Color _defaultColor;

	[SerializeField]
	private Color _selectedColor;

	[SerializeField]
	private Color _defaultTextColor;

	[SerializeField]
	private Color _selectedTextColor;

	[SerializeField]
	private Outline _outline;

	[SerializeField]
	private Color _defaultOutlineColor;

	[SerializeField]
	private Color _selectedOutlineColor;

	[SerializeField]
	private Image _iconImage;

	[SerializeField]
	private Image _allowDiableIcon;

	[SerializeField]
	private Sprite _defaultSprite;

	[SerializeField]
	private Sprite _selectedSprite;

	[SerializeField]
	private Sprite _lockedSprite;

	[SerializeField]
	private GameObject _lockedIcon;

	[SerializeField]
	private Image _textObject;

	private readonly Color32 _E284 = new Color32(36, 191, 229, byte.MaxValue);

	private bool _E31B;

	private EntryPoint _E31C;

	private Action<EntryPoint> _E31D;

	private bool _E2D3;

	public void Show(bool allowSelection, EntryPoint entryPoint, bool editPositions, Action<EntryPoint> onSelect, Action<EntryPoint> onDoubleClick)
	{
		_E31B = allowSelection;
		_E31C = entryPoint;
		_E31D = onSelect;
		ShowGameObject();
		_E000(entryPoint);
		((RectTransform)base.transform).anchoredPosition = entryPoint.PositionOnMap;
	}

	private void _E000(EntryPoint point)
	{
		if (!point.Locked)
		{
			string text = ((point.Index >= 10) ? point.Index.ToString() : (_ED3E._E000(27314) + point.Index));
			_nameLabel.text = point.Name.Localized();
			_indexLabel.text = text;
		}
		_iconImage.gameObject.SetActive(_E31B);
		_allowDiableIcon.gameObject.SetActive(!_E31B);
		if (_E31B)
		{
			_textObject.gameObject.SetActive(!point.Locked);
			_indexLabel.gameObject.SetActive(!point.Locked);
			_iconImage.sprite = (point.Locked ? _lockedSprite : _defaultSprite);
			_lockedIcon.SetActive(point.Locked);
			_background.color = _E284;
			_textObject.color = _defaultColor;
		}
		else
		{
			_allowDiableIcon.color = point.MainColor;
			_indexLabel.color = point.MainColor;
			_background.color = point.MainColor;
			_glow.gameObject.SetActive(value: false);
		}
	}

	public void Select()
	{
		if (_E31B && !_E31C.Locked)
		{
			_E2D3 = true;
			_canvasGroup.alpha = 1f;
			_background.gameObject.SetActive(_E31C.ShowRadius);
			_glow.gameObject.SetActive(value: true);
			_iconImage.sprite = _selectedSprite;
			_textObject.color = _selectedColor;
			_nameLabel.color = _selectedTextColor;
			_outline.effectColor = _selectedOutlineColor;
		}
	}

	public void Deselect()
	{
		if (!_E31C.Locked)
		{
			_E2D3 = false;
			_canvasGroup.alpha = 0.8f;
			_background.gameObject.SetActive(value: false);
			_glow.gameObject.SetActive(value: false);
			_iconImage.sprite = _defaultSprite;
			_textObject.color = _defaultColor;
			_nameLabel.color = _defaultTextColor;
			_outline.effectColor = _defaultOutlineColor;
		}
	}

	private void Update()
	{
		base.transform.localScale = new Vector3(1f / base.transform.parent.localScale.x, 1f / base.transform.parent.localScale.y, 1f);
		_background.transform.localScale = new Vector3(10f * _E31C.Radius / base.transform.localScale.x, 10f * _E31C.Radius / base.transform.localScale.y, 1f);
	}

	public void OnPointerClick([CanBeNull] PointerEventData eventData)
	{
		if (eventData != null && _E31B && !_E31C.Locked && eventData.button == PointerEventData.InputButton.Left)
		{
			_E31D(_E31C);
		}
	}

	public void OnPointerEnter([CanBeNull] PointerEventData eventData)
	{
		if (_E31B && !_E2D3)
		{
			_glow.gameObject.SetActive(value: true);
			_canvasGroup.alpha = 1f;
		}
	}

	public void OnPointerExit([CanBeNull] PointerEventData eventData)
	{
		if (_E31B && !_E2D3)
		{
			_glow.gameObject.SetActive(value: false);
			_canvasGroup.alpha = 0.8f;
		}
	}
}
