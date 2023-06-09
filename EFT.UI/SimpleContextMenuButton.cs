using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public class SimpleContextMenuButton : UIElement, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	[SerializeField]
	private CanvasGroup _canvasGroup;

	[SerializeField]
	private Button _button;

	[SerializeField]
	private Image _backgroundImage;

	[SerializeField]
	private Color _defaultBackgroundColor;

	[SerializeField]
	private Color _selectedBackgroundColor;

	[SerializeField]
	private Image _subMenuArrow;

	private Action _E0F0;

	private Action _E0F1;

	private SimpleTooltip _E02A;

	private bool _E0F2;

	private bool _E0F3;

	private string _E0F4;

	private bool _E0F5;

	private bool _E0F6;

	public bool Blocked
	{
		get
		{
			return _E0F5;
		}
		set
		{
			_E0F5 = value;
			_backgroundImage.color = ((_E0F5 || _E0F6) ? _selectedBackgroundColor : _defaultBackgroundColor);
		}
	}

	private void Awake()
	{
		_button.onClick.AddListener(delegate
		{
			_E0F0?.Invoke();
		});
	}

	public virtual void Show(string caption, string imageText, Sprite sprite, Action onClicked, Action onHover, bool subMenu = false, bool hideIcon = false)
	{
		_E0F0 = onClicked;
		_E0F1 = onHover;
		_E02A = ItemUiContext.Instance.Tooltip;
		_E0F2 = _E0F0 == null && _E0F1 == null;
		_E0F5 = false;
		_E0F6 = false;
		_canvasGroup.interactable = !_E0F2;
		_canvasGroup.blocksRaycasts = !_E0F2;
		if (_subMenuArrow != null)
		{
			_subMenuArrow.enabled = subMenu;
		}
		ShowGameObject();
	}

	public void SetButtonInteraction((bool IsInteractive, string Error) interactiveStatus)
	{
		if (!_E0F2)
		{
			(bool IsInteractive, string Error) tuple = interactiveStatus;
			bool item = tuple.IsInteractive;
			string item2 = tuple.Error;
			bool flag = !string.IsNullOrEmpty(item2);
			_E0F3 = !item && flag && _E02A != null;
			_E0F4 = (flag ? item2.Localized() : string.Empty);
			_canvasGroup.SetUnlockStatus(item, setRaycast: false);
		}
	}

	public void OnPointerEnter([NotNull] PointerEventData eventData)
	{
		_E0F6 = true;
		if (!Blocked)
		{
			_backgroundImage.color = _selectedBackgroundColor;
		}
		if (_E0F3)
		{
			_E02A.Show(_E0F4);
		}
		_E0F1?.Invoke();
	}

	public void OnPointerExit([NotNull] PointerEventData eventData)
	{
		_E0F6 = false;
		if (!Blocked)
		{
			_backgroundImage.color = _defaultBackgroundColor;
		}
		if (_E0F3)
		{
			_E02A.Close();
		}
	}

	public override void Close()
	{
		if (_E0F3)
		{
			_E02A.Close();
		}
		base.Close();
	}

	[CompilerGenerated]
	private void _E000()
	{
		_E0F0?.Invoke();
	}
}
