using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI;

public class HoverTooltipArea : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	[SerializeField]
	private string _message = string.Empty;

	[SerializeField]
	private float _delay;

	[SerializeField]
	private bool _limitTooltipWidth = true;

	[SerializeField]
	private bool _customOffset;

	[SerializeField]
	private Vector2 _offset;

	private Func<string> _E000;

	private SimpleTooltip _E001;

	private CanvasGroup _E002;

	private bool _E003;

	private bool _E004;

	private float? _E005
	{
		get
		{
			if (!_limitTooltipWidth)
			{
				return -1f;
			}
			return null;
		}
	}

	private string _E006 => _E000?.Invoke() ?? _message;

	private string _E007
	{
		get
		{
			if (!_E003)
			{
				return _E006.Localized();
			}
			return _E006;
		}
	}

	public CanvasGroup CanvasGroup
	{
		get
		{
			if (!_E002)
			{
				return _E002 = GetComponent<CanvasGroup>();
			}
			return _E002;
		}
	}

	private void Awake()
	{
		_E001 = ItemUiContext.Instance.Tooltip;
		_E001.Close();
		_E004 = false;
	}

	public void Init(SimpleTooltip tooltip, string text, bool rawText = false)
	{
		_E001 = tooltip;
		SetMessageText(text, rawText);
	}

	public void SetMessageText(string text, bool rawText = false)
	{
		_message = text;
		_E000 = null;
		_E003 = rawText;
		if (_E004)
		{
			Show();
		}
	}

	public void SetMessageText(Func<string> textGetter)
	{
		_message = string.Empty;
		_E003 = true;
		_E000 = textGetter;
		if (_E004)
		{
			Show();
		}
	}

	public void SetUnlockStatus(bool value)
	{
		base.enabled = !value;
		CanvasGroup.SetUnlockStatus(value, setRaycast: false);
	}

	void IPointerEnterHandler.OnPointerEnter([NotNull] PointerEventData eventData)
	{
		if (!string.IsNullOrEmpty(_E006))
		{
			Show();
			_E004 = true;
		}
	}

	void IPointerExitHandler.OnPointerExit([NotNull] PointerEventData eventData)
	{
		_E004 = false;
		_E001.Close();
	}

	private void Show()
	{
		Vector2? offset = (_customOffset ? new Vector2?(_offset) : null);
		_E001.Show(_E007, offset, _delay, _E005);
	}

	private void OnApplicationFocus(bool inFocus)
	{
		OnDisable();
	}

	private void OnDisable()
	{
		_E004 = false;
		if (_E001 != null && _E001.isActiveAndEnabled)
		{
			_E001.Close();
		}
	}
}
