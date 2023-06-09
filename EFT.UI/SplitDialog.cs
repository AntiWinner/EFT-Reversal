using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class SplitDialog : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	public enum ESplitDialogType
	{
		Int,
		Float,
		Step
	}

	[SerializeField]
	private RectTransform _window;

	[SerializeField]
	private GameObject _captionPanel;

	[SerializeField]
	private Button _closeButton;

	[SerializeField]
	private Button _acceptButton;

	[SerializeField]
	private StepSlider _stepSlider;

	[SerializeField]
	private IntSlider _intSlider;

	[SerializeField]
	private CustomTextMeshProUGUI _title;

	private Action<int> m__E000;

	private Action m__E001;

	private Action m__E002;

	public void Awake()
	{
		_captionPanel.AddComponent<UIDragComponent>().Init(_window, putOnTop: false);
		_closeButton.onClick.AddListener(delegate
		{
			this.m__E001();
		});
	}

	public void Show(string title, int maxValue, Vector2 position, Action<int> acceptAction, Action cancelAction, ESplitDialogType type = ESplitDialogType.Step)
	{
		Show(title, 1, maxValue, 1, maxValue, maxValue / 2 + 1, position, acceptAction, cancelAction, type);
	}

	public void Show(string title, int minSliderValue, int maxSliderValue, int minValue, int maxValue, int currentValue, Vector2 position, Action<int> acceptAction, Action cancelAction, ESplitDialogType type = ESplitDialogType.Step, bool allowZero = false)
	{
		base.gameObject.SetActive(value: true);
		base.transform.SetAsLastSibling();
		_title.text = title;
		_window.position = position;
		_E001();
		this.m__E000 = acceptAction;
		this.m__E001 = cancelAction;
		switch (type)
		{
		case ESplitDialogType.Int:
		case ESplitDialogType.Float:
			_E000(delegate
			{
				this.m__E000(_intSlider.CurrentValue());
			});
			_stepSlider.gameObject.SetActive(value: false);
			_intSlider.gameObject.SetActive(value: true);
			_intSlider.Show(minSliderValue, maxSliderValue, minValue, maxValue, currentValue, allowZero);
			_intSlider.Focus();
			break;
		case ESplitDialogType.Step:
			_E000(delegate
			{
				this.m__E000((int)_stepSlider.CurrentValue());
			});
			_intSlider.gameObject.SetActive(value: false);
			_stepSlider.gameObject.SetActive(value: true);
			_stepSlider.Show(minValue, maxValue, currentValue);
			_stepSlider.Focus();
			break;
		default:
			throw new ArgumentOutOfRangeException(_ED3E._E000(124643), type, null);
		}
	}

	public void Accept()
	{
		this.m__E002?.Invoke();
	}

	private void _E000(Action action)
	{
		this.m__E002 = action;
		_acceptButton.onClick.RemoveAllListeners();
		_acceptButton.onClick.AddListener(delegate
		{
			this.m__E002();
		});
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		base.transform.SetAsLastSibling();
	}

	private void _E001()
	{
		_window.CorrectPositionResolution();
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}

	[CompilerGenerated]
	private void _E002()
	{
		this.m__E001();
	}

	[CompilerGenerated]
	private void _E003()
	{
		this.m__E000(_intSlider.CurrentValue());
	}

	[CompilerGenerated]
	private void _E004()
	{
		this.m__E000((int)_stepSlider.CurrentValue());
	}

	[CompilerGenerated]
	private void _E005()
	{
		this.m__E002();
	}
}
