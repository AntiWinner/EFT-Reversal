using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Comfort.Common;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class DropDownBox : InteractableElement, IPointerClickHandler, IEventSystemHandler, IPointerDownHandler, IPointerExitHandler, IPointerEnterHandler, _EC72<int>, IDisposable
{
	private struct _E000
	{
		public string Label;

		public bool Enabled;
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public DropDownBox _003C_003E4__this;

		public Func<IEnumerable<string>> values;

		public Func<int, bool> validator;

		internal void _E000()
		{
			_003C_003E4__this._E002();
			_003C_003E4__this.Show(values(), validator);
			_003C_003E4__this._currentValueText.text = _003C_003E4__this._E20F[_003C_003E4__this.CurrentIndex].Label;
			_003C_003E4__this._currentValueText.SetAllDirty();
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public int index;

		public DropDownBox _003C_003E4__this;

		internal void _E000()
		{
			_003C_003E4__this.UpdateValue(index);
			_003C_003E4__this._E001(state: false);
			_003C_003E4__this.OnValueChanged?.Invoke();
		}
	}

	[SerializeField]
	private TextMeshProUGUI _currentValueText;

	[SerializeField]
	private Button _button;

	[SerializeField]
	private RectTransform _openPanel;

	[SerializeField]
	private Image _background;

	[SerializeField]
	private Color _highlightedColor;

	[SerializeField]
	private RectTransform _elementsContainer;

	[SerializeField]
	private Button _elementTemplate;

	[SerializeField]
	private LayoutElement _scrollRectLayoutElement;

	[SerializeField]
	private RectTransform _scrollBar;

	[SerializeField]
	private Image _buttonOpen;

	[SerializeField]
	private Image _buttonClosed;

	[SerializeField]
	private float _maxVisibleHeight = 120f;

	[NonSerialized]
	public readonly _ECEC OnValueChanged = new _ECEC();

	private Vector2 _E207;

	private VerticalLayoutGroup _E208;

	private Vector3 _E209;

	private RectTransform _E20A;

	private readonly Color _E128 = Color.white;

	private bool _E20B;

	private Action<int> _E20C;

	private Action<int> _E20D;

	private bool _E20E;

	private readonly List<_E000> _E20F = new List<_E000>();

	private bool _E210;

	private Func<int, bool> _E211;

	[CompilerGenerated]
	private int _E212;

	public int CurrentIndex
	{
		[CompilerGenerated]
		get
		{
			return _E212;
		}
		[CompilerGenerated]
		private set
		{
			_E212 = value;
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
			_E004();
			if (!value)
			{
				_E001(state: false);
			}
		}
	}

	private void Awake()
	{
		_E000();
	}

	private void _E000()
	{
		if (!_E210)
		{
			_button.onClick.AddListener(delegate
			{
				_E001(!_E20B);
			});
			HoverTrigger hoverTrigger = _openPanel.gameObject.AddComponent<HoverTrigger>();
			hoverTrigger.OnHoverStart += delegate
			{
				_E003(value: true);
			};
			hoverTrigger.OnHoverEnd += delegate
			{
				_E003(value: false);
			};
			_openPanel.gameObject.SetActive(value: false);
			_E207 = _openPanel.anchoredPosition;
			_E208 = _openPanel.GetComponent<VerticalLayoutGroup>();
			_E002();
			_E210 = true;
		}
	}

	public void Show(Func<IEnumerable<string>> values, Func<int, bool> validator = null)
	{
		_E001 CS_0024_003C_003E8__locals0 = new _E001();
		CS_0024_003C_003E8__locals0._003C_003E4__this = this;
		CS_0024_003C_003E8__locals0.values = values;
		CS_0024_003C_003E8__locals0.validator = validator;
		CS_0024_003C_003E8__locals0._E000();
		UI.Dispose();
		UI.AddDisposable(_E7AD._E010.AddLocaleUpdateListener(delegate
		{
			CS_0024_003C_003E8__locals0._003C_003E4__this._E002();
			CS_0024_003C_003E8__locals0._003C_003E4__this.Show(CS_0024_003C_003E8__locals0.values(), CS_0024_003C_003E8__locals0.validator);
			CS_0024_003C_003E8__locals0._003C_003E4__this._currentValueText.text = CS_0024_003C_003E8__locals0._003C_003E4__this._E20F[CS_0024_003C_003E8__locals0._003C_003E4__this.CurrentIndex].Label;
			CS_0024_003C_003E8__locals0._003C_003E4__this._currentValueText.SetAllDirty();
		}));
	}

	public void Show(IEnumerable<string> values, Func<int, bool> validator = null)
	{
		_E000();
		_E211 = validator;
		if (ItemUiContext.Instance != null)
		{
			_E20A = ItemUiContext.Instance.ContextMenuArea;
		}
		_E20F.Clear();
		foreach (string value in values)
		{
			_E20F.Add(new _E000
			{
				Label = value,
				Enabled = true
			});
		}
	}

	private void _E001(bool state)
	{
		if (this == null || _E20B == state)
		{
			return;
		}
		_E20B = state;
		_buttonClosed.gameObject.SetActive(!state);
		_buttonOpen.gameObject.SetActive(state);
		_openPanel.gameObject.SetActive(state);
		_openPanel.SetParent(state ? _E20A : base.transform, worldPositionStays: true);
		if (state && Interactable)
		{
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuDropdown);
			for (int i = 0; i < _E20F.Count; i++)
			{
				Button button = UnityEngine.Object.Instantiate(_elementTemplate, _elementsContainer, worldPositionStays: false);
				HoverTooltipArea component = button.GetComponent<HoverTooltipArea>();
				bool flag = _E20F[i].Enabled && (_E211?.Invoke(i) ?? true);
				if (component != null)
				{
					component.SetUnlockStatus(flag);
				}
				TextMeshProUGUI textMeshProUGUI = button.GetComponentsInChildren<TextMeshProUGUI>(includeInactive: true)[0];
				textMeshProUGUI.text = _E20F[i].Label;
				CanvasGroup canvasGroup = textMeshProUGUI.gameObject.GetComponent<CanvasGroup>();
				if (!flag && canvasGroup == null)
				{
					canvasGroup = textMeshProUGUI.gameObject.AddComponent<CanvasGroup>();
				}
				if (canvasGroup != null)
				{
					canvasGroup.alpha = (flag ? 1f : 0.5f);
				}
				button.interactable = flag;
				button.gameObject.SetActive(value: true);
				if (flag)
				{
					int index = i;
					button.onClick.AddListener(delegate
					{
						UpdateValue(index);
						_E001(state: false);
						OnValueChanged?.Invoke();
					});
				}
			}
		}
		else
		{
			_openPanel.anchoredPosition = _E207;
			_E002();
			_E20D?.Invoke(CurrentIndex);
		}
	}

	internal void _E002()
	{
		for (int num = _elementsContainer.childCount - 1; num >= 0; num--)
		{
			UnityEngine.Object.Destroy(_elementsContainer.GetChild(num).gameObject);
		}
	}

	public void UpdateValue(int value, bool sendCallback = true, int? min = null, int? max = null)
	{
		if (_E20F != null && _E20F.Count != 0)
		{
			if (sendCallback)
			{
				Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuDropdownSelect);
			}
			value = ((min.HasValue && max.HasValue) ? Mathf.Clamp(value, min.Value, max.Value) : Mathf.Clamp(value, 0, _E20F.Count - 1));
			CurrentIndex = value;
			_currentValueText.text = _E20F[CurrentIndex].Label;
			if (_E20C != null && sendCallback)
			{
				_E20C(value);
			}
		}
	}

	public void Bind(Action<int> valueChanged)
	{
		_E20C = valueChanged;
	}

	public void DropdownClosedHandler(Action<int> onClosed)
	{
		_E20D = onClosed;
	}

	public int CurrentValue()
	{
		if (!_E20B)
		{
			return 0;
		}
		return 1;
	}

	private void Update()
	{
		_scrollRectLayoutElement.minHeight = Mathf.Min(_elementsContainer.sizeDelta.y, _maxVisibleHeight);
		if (_scrollBar != null)
		{
			_E208.padding.right = ((!_scrollBar.gameObject.activeSelf) ? 1 : (1 + (int)_scrollBar.rect.width));
		}
		if (_E209 != base.transform.position && Vector3.Distance(_E209, base.transform.position) > 0.01f)
		{
			_E209 = base.transform.position;
			if (_E20B)
			{
				_E001(state: false);
			}
		}
		if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) && !_E20E && _E20B)
		{
			_E001(state: false);
		}
	}

	private void _E003(bool value)
	{
		_E20E = value;
	}

	public void OnPointerClick([NotNull] PointerEventData eventData)
	{
		if (Interactable)
		{
			_E001(!_E20B);
		}
	}

	void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
	{
	}

	public void OnPointerEnter([NotNull] PointerEventData eventData)
	{
		if (Interactable)
		{
			_E003(value: true);
			_background?.CrossFadeColor(_highlightedColor, 0f, ignoreTimeScale: true, useAlpha: true);
		}
	}

	public void OnPointerExit([NotNull] PointerEventData eventData)
	{
		_E004();
	}

	private void _E004()
	{
		_E003(value: false);
		_background?.CrossFadeColor(_E128, 0f, ignoreTimeScale: true, useAlpha: true);
	}

	public void SetLabelText(string text)
	{
		_currentValueText.text = text;
	}

	public void Hide()
	{
		_E20C = null;
		_E001(state: false);
		UI.Dispose();
	}

	void IDisposable.Dispose()
	{
		Hide();
	}

	[CompilerGenerated]
	private void _E005()
	{
		_E001(!_E20B);
	}

	[CompilerGenerated]
	private void _E006(PointerEventData arg)
	{
		_E003(value: true);
	}

	[CompilerGenerated]
	private void _E007(PointerEventData arg)
	{
		_E003(value: false);
	}
}
