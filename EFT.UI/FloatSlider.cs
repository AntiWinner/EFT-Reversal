using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class FloatSlider : MonoBehaviour, _EC72<float>
{
	[SerializeField]
	private Slider _slider;

	[SerializeField]
	private TMP_InputField _valueInput;

	public string Format;

	private Action<float> m__E000;

	private void Awake()
	{
		_slider.onValueChanged.AddListener(_E000);
		if (_valueInput != null)
		{
			_valueInput.interactable = false;
		}
	}

	private void _E000(float value)
	{
		value = _E38D.RoundFloatValue(value, 2);
		string text = (string.IsNullOrEmpty(Format) ? value.ToString(CultureInfo.InvariantCulture) : value.ToString(Format));
		if (_valueInput != null && _valueInput.text != text)
		{
			_valueInput.text = text;
		}
		if (_slider.value.ToString(CultureInfo.InvariantCulture) != text)
		{
			_slider.value = value;
		}
		this.m__E000?.Invoke(value);
	}

	public void Show(float minValue, float maxValue)
	{
		_slider.minValue = minValue;
		_slider.maxValue = maxValue;
	}

	public void UpdateValue(float value, bool sendCallback = true, float? min = null, float? max = null)
	{
		if (!(Math.Abs(_slider.maxValue) < Mathf.Epsilon))
		{
			value = ((min.HasValue && max.HasValue) ? Mathf.Clamp(value, min.Value, max.Value) : Mathf.Clamp(value, 0f, _slider.maxValue));
			_E000(value);
		}
	}

	public void Bind(Action<float> valueChanged)
	{
		this.m__E000 = valueChanged;
	}

	public float CurrentValue()
	{
		return _slider.value;
	}
}
