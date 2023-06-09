using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class ConditionCharacteristicsSlider : UIElement
{
	[SerializeField]
	private Image _border;

	[SerializeField]
	private Image _currentDurabilityImage;

	[SerializeField]
	private Image _selectedDurabilityImage;

	[SerializeField]
	private Image _maximumDirabilityImage;

	[SerializeField]
	private Image _maximumDurabilityRemovedImage;

	[SerializeField]
	private Image _maxDegradation;

	[SerializeField]
	private Slider _durabilitySlider;

	[SerializeField]
	private CustomTextMeshProUGUI _durabilityLabel;

	[SerializeField]
	private Button _maximumSliderValueButton;

	[SerializeField]
	private Color _criticalColor;

	[SerializeField]
	private Color _defaultColor;

	private float _E144;

	private int _E145 = 100;

	[CompilerGenerated]
	private Action<float> _E146;

	private _EB10 _E147;

	private _EB10 _E148;

	private Vector2 _E149;

	private float _E000 => Mathf.InverseLerp(0f, _E145, _E147?.Base() ?? 100f);

	private float _E001 => Mathf.InverseLerp(0f, _E145, _E148?.Base() ?? 100f);

	public float RealCurrentDurability => this._E000 * (float)_E145;

	public float Value
	{
		get
		{
			if (!(_E144 > 0f))
			{
				return 0f;
			}
			return _E144 * 100f;
		}
		set
		{
			_E000(Math.Max(value / 100f, 0f));
		}
	}

	public event Action<float> OnSliderValueChangedEvent
	{
		[CompilerGenerated]
		add
		{
			Action<float> action = _E146;
			Action<float> action2;
			do
			{
				action2 = action;
				Action<float> value2 = (Action<float>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E146, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<float> action = _E146;
			Action<float> action2;
			do
			{
				action2 = action;
				Action<float> value2 = (Action<float>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E146, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	private void Awake()
	{
		_durabilitySlider.onValueChanged.AddListener(_E000);
		_maximumSliderValueButton.onClick.AddListener(delegate
		{
			_durabilitySlider.value = _durabilitySlider.maxValue;
		});
	}

	public void Show(_EB10 durability, _EB10 maximumDurability, int peakDurability)
	{
		_E147 = durability;
		_E148 = maximumDurability;
		_E145 = peakDurability;
	}

	private void _E000(float value)
	{
		_E144 = value;
		SetSliderValues(setAnchors: false);
		_E146?.Invoke(value);
	}

	public void SetSliderValues(bool setAnchors = true)
	{
		if (setAnchors)
		{
			_currentDurabilityImage.rectTransform.anchorMax = new Vector2(this._E000, 0.5f);
			_maximumDirabilityImage.rectTransform.anchorMax = new Vector2(this._E001, 0.5f);
			_maxDegradation.rectTransform.anchorMax = new Vector2(Mathf.Clamp01(this._E001 - _E149.x / (float)_E145), 0.5f);
			_maxDegradation.rectTransform.anchorMin = new Vector2(Mathf.Clamp01(this._E001 - _E149.y / (float)_E145), 0.5f);
			Transform obj = _durabilitySlider.transform;
			((RectTransform)obj).anchorMin = new Vector2(0.01f + this._E000, 0.5f);
			((RectTransform)obj).anchorMax = new Vector2(this._E001 - _E149.x / (float)_E145, 0.5f);
			_durabilitySlider.minValue = 0.001f;
			float num = (this._E001 * (float)_E145 - RealCurrentDurability - _E149.x) / 100f;
			_durabilitySlider.maxValue = num;
			RectTransform rectTransform = _maximumDurabilityRemovedImage.rectTransform;
			rectTransform.anchorMin = new Vector2(this._E000 + num * 100f / (float)_E145, 0.5f);
			rectTransform.anchorMax = new Vector2(this._E001, 0.5f);
			bool flag = num < _durabilitySlider.minValue;
			if (flag)
			{
				_durabilitySlider.maxValue = 0f;
				_durabilitySlider.minValue = 0f;
				_durabilitySlider.value = 0f;
			}
			_durabilitySlider.gameObject.SetActive(!flag);
		}
		RectTransform rectTransform2 = _selectedDurabilityImage.rectTransform;
		rectTransform2.anchorMin = new Vector2(this._E000 / (float)_E145, 0.5f);
		rectTransform2.anchorMax = new Vector2(this._E000 + _E144 * 100f / (float)_E145, 0.5f);
		_durabilityLabel.text = Math.Round(RealCurrentDurability, 2) + _ED3E._E000(30703) + Math.Round(this._E001 * (float)_E145, 2) + string.Format(_ED3E._E000(247388), _E145);
	}

	public void SetSliderActive(bool value)
	{
		Color color = (value ? _criticalColor : _defaultColor);
		_durabilityLabel.color = color;
		_border.color = color;
		_currentDurabilityImage.color = color;
	}

	public void SetSliderInteractable(bool value)
	{
		_durabilitySlider.interactable = !value;
	}

	public void UpdateDegradationPrediction(Vector2 range)
	{
		_E149 = range;
		SetSliderValues();
	}

	[CompilerGenerated]
	private void _E001()
	{
		_durabilitySlider.value = _durabilitySlider.maxValue;
	}
}
