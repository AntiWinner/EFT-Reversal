using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class IntSlider : MonoBehaviour, _EC72<int>
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public int outValue;

		public IntSlider _003C_003E4__this;

		internal void _E000(string value)
		{
			if (!int.TryParse(value, out outValue))
			{
				_003C_003E4__this._valueInput.text = _003C_003E4__this.m__E001.ToString();
			}
			else if ((float)outValue < _003C_003E4__this._slider.minValue)
			{
				_003C_003E4__this._valueInput.text = _003C_003E4__this._slider.minValue.ToString(CultureInfo.InvariantCulture);
			}
			else if ((float)outValue > _003C_003E4__this._slider.maxValue)
			{
				_003C_003E4__this._valueInput.text = _003C_003E4__this._slider.maxValue.ToString(CultureInfo.InvariantCulture);
			}
		}
	}

	[SerializeField]
	private Slider _slider;

	[SerializeField]
	private CustomTextMeshProInputField _valueInput;

	[SerializeField]
	private GameObject _notchTemplate;

	[SerializeField]
	private GameObject _notchNotAvailableTemplate;

	[SerializeField]
	private RectTransform _notchContainer;

	private readonly List<GameObject> m__E000 = new List<GameObject>();

	private int m__E001;

	private int m__E002;

	private int _E003;

	private bool _E004;

	private int _E005
	{
		get
		{
			if (!_E004)
			{
				return 1;
			}
			return 0;
		}
	}

	private void Awake()
	{
		_valueInput.characterValidation = TMP_InputField.CharacterValidation.Integer;
		_slider.onValueChanged.AddListener(_E000);
		_valueInput.onValueChanged.AddListener(_E001);
		int outValue;
		_valueInput.onEndEdit.AddListener(delegate(string value)
		{
			if (!int.TryParse(value, out outValue))
			{
				_valueInput.text = this.m__E001.ToString();
			}
			else if ((float)outValue < _slider.minValue)
			{
				_valueInput.text = _slider.minValue.ToString(CultureInfo.InvariantCulture);
			}
			else if ((float)outValue > _slider.maxValue)
			{
				_valueInput.text = _slider.maxValue.ToString(CultureInfo.InvariantCulture);
			}
		});
	}

	private void _E000(float value)
	{
		UpdateValue((int)value);
	}

	private void _E001(string text)
	{
		if (int.TryParse(text, out var result))
		{
			result = result + this.m__E002 - _E005;
			UpdateValue(result);
		}
	}

	private void _E002()
	{
		foreach (GameObject item in this.m__E000)
		{
			UnityEngine.Object.DestroyImmediate(item);
		}
		this.m__E000.Clear();
	}

	public void Show(int minSliderValue, int maxSliderValue, int minValue, int maxValue, int currentValue, bool allowZero = false)
	{
		_slider.minValue = minSliderValue;
		_slider.maxValue = maxSliderValue;
		this.m__E002 = minValue;
		_E003 = maxValue;
		_E004 = allowZero;
		_E002();
		for (int i = 0; i < maxSliderValue - minSliderValue; i++)
		{
			GameObject gameObject = ((this.m__E002 - 1 > i || i > _E003) ? UnityEngine.Object.Instantiate(_notchNotAvailableTemplate) : UnityEngine.Object.Instantiate(_notchTemplate));
			gameObject.transform.SetParent(_notchContainer, worldPositionStays: false);
			gameObject.SetActive(value: true);
			this.m__E000.Add(gameObject);
		}
		UpdateValue(currentValue);
	}

	public void Focus()
	{
		_valueInput.ActivateInputField();
	}

	public void UpdateValue(int value, bool sendCallback = true, int? min = null, int? max = null)
	{
		if (value < this.m__E002)
		{
			value = this.m__E002;
		}
		if (value > _E003)
		{
			value = _E003;
		}
		string text = (value - this.m__E002 + _E005).ToString();
		if (text != _valueInput.text)
		{
			_valueInput.text = text;
		}
		if (!Mathf.Approximately(_slider.value, value))
		{
			_slider.value = value;
		}
		this.m__E001 = value;
	}

	public void Bind(Action<int> valueChanged)
	{
	}

	public int CurrentValue()
	{
		return this.m__E001 - this.m__E002 + _E005;
	}
}
