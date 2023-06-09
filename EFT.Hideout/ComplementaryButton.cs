using System;
using System.Runtime.CompilerServices;
using EFT.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.Hideout;

public class ComplementaryButton : UIElement
{
	private const string _E06A = "{0} seconds left";

	[SerializeField]
	private HoverTrigger _hoverTrigger;

	[SerializeField]
	private CanvasGroup _canvasGroup;

	[SerializeField]
	private Image _background;

	[SerializeField]
	private GameObject _hoverObject;

	[SerializeField]
	private Image _icon;

	[SerializeField]
	private Sprite _defaultBackground;

	[SerializeField]
	private Sprite _selectedBackground;

	[SerializeField]
	private Sprite _defaultIcon;

	[SerializeField]
	private Sprite _selectedIcon;

	private SimpleTooltip _E02A;

	private Action<PointerEventData> _E06B;

	private Action _E06C;

	private Action<bool> _E06D;

	private bool _E06E;

	private int _E06F;

	private float _E070;

	private double _E071;

	private bool _E072;

	private Func<string> _E073 = () => string.Empty;

	private Func<string> _E074 = () => string.Empty;

	[CompilerGenerated]
	private bool _E075;

	public bool IsUnlocked
	{
		[CompilerGenerated]
		get
		{
			return _E075;
		}
		[CompilerGenerated]
		private set
		{
			_E075 = value;
		}
	}

	public Sprite Icon => _defaultIcon;

	private int _E000 => Mathf.CeilToInt((float)_E06F - _E070);

	private string _E001
	{
		get
		{
			string result = (_E06E ? _E073().Localized() : _E074().Localized());
			if (this._E000 > 0)
			{
				result = string.Format(_ED3E._E000(164634).Localized(), this._E000);
			}
			return result;
		}
	}

	public void Show([CanBeNull] Action<bool> onClick, Action<PointerEventData> onHoverStart = null, Action onHoverEnd = null, bool unlocked = true, int refreshCooldown = 0)
	{
		_E06D = onClick;
		IsUnlocked = unlocked;
		_E06B = onHoverStart;
		_E06C = onHoverEnd;
		_E06F = refreshCooldown;
		_E070 = refreshCooldown;
		_hoverTrigger.Init(_E000, delegate
		{
			PointerExit();
		});
		_hoverTrigger.GetOrAddComponent<ClickTrigger>().Init(delegate
		{
			_E001();
		});
		ShowGameObject();
	}

	public void SetTooltipMessages(Func<string> hoverOnEnabled, [CanBeNull] Func<string> hoverOnDisabled)
	{
		_E073 = hoverOnEnabled;
		_E074 = hoverOnDisabled ?? hoverOnEnabled;
	}

	private void Update()
	{
		if (IsUnlocked && _E06F > 0 && !_E072)
		{
			_E070 = (float)(_E5AD.NowUnix - _E071);
			if (!_E071.IsZero() && _E02A != null && !string.IsNullOrEmpty(this._E001))
			{
				_E02A.SetText(this._E001);
			}
			if (!(_E070 < (float)_E06F))
			{
				_E071 = 0.0;
				_E072 = true;
				_canvasGroup.alpha = 1f;
			}
		}
	}

	private void OnDisable()
	{
		if (_E02A != null)
		{
			_E02A.Close();
		}
	}

	private void _E000(PointerEventData eventData)
	{
		_E02A = ItemUiContext.Instance.Tooltip;
		if (_E02A != null && !string.IsNullOrEmpty(this._E001))
		{
			_E02A.Show(this._E001);
		}
		_hoverObject.SetActive(value: true);
		_E06B?.Invoke(eventData);
	}

	public void PointerExit()
	{
		CloseTooltip();
		_hoverObject.SetActive(value: false);
		_E06C?.Invoke();
	}

	public void PointerClick(bool value)
	{
		_E001();
	}

	public void CloseTooltip()
	{
		if (!(_E02A == null))
		{
			_E02A.Close();
			_E02A = null;
		}
	}

	private void _E001()
	{
		if (!IsUnlocked)
		{
			return;
		}
		if (_E06F > 0)
		{
			if (!_E072)
			{
				return;
			}
			_E070 = 0f;
			_E071 = _E5AD.NowUnix;
			_E072 = false;
			_canvasGroup.alpha = 0.3f;
		}
		_E06E = !_E06E;
		SetSelectedStatus(_E06E);
		_E06D?.Invoke(_E06E);
	}

	public virtual void SetSelectedStatus(bool selected)
	{
		_E06E = selected;
		_background.sprite = (selected ? _selectedBackground : _defaultBackground);
		SetIcon(selected ? _selectedIcon : _defaultIcon);
		_hoverObject.SetActive(value: false);
	}

	protected void SetIcon(Sprite sprite)
	{
		_icon.sprite = sprite;
		_icon.SetNativeSize();
	}

	public void SetUnlockStatus(bool isUnlocked)
	{
		IsUnlocked = isUnlocked;
		if (!(_canvasGroup == null))
		{
			_canvasGroup.alpha = (isUnlocked ? 1f : 0.3f);
		}
	}

	public override void Close()
	{
		CloseTooltip();
		base.Close();
	}

	[CompilerGenerated]
	private void _E002(PointerEventData arg)
	{
		PointerExit();
	}

	[CompilerGenerated]
	private void _E003(PointerEventData arg)
	{
		_E001();
	}
}
