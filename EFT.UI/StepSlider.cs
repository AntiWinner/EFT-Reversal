using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class StepSlider : MonoBehaviour, _EC72<float>
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public int value;

		internal int _E000(int x, int y)
		{
			if (Math.Abs(x - value) >= Math.Abs(y - value))
			{
				return y;
			}
			return x;
		}
	}

	[SerializeField]
	private Slider _slider;

	[SerializeField]
	private CustomTextMeshProInputField _valueInput;

	[SerializeField]
	private GameObject _notchTemplate;

	[SerializeField]
	private RectTransform _notchContainer;

	private readonly List<GameObject> m__E000 = new List<GameObject>();

	private readonly List<int> m__E001 = new List<int>();

	private int m__E002;

	private int m__E003;

	private int m__E004;

	private int m__E005;

	private void Awake()
	{
		_slider.onValueChanged.AddListener(delegate(float arg)
		{
			int num = this.m__E001[(int)_slider.value - 1];
			if (this.m__E005 != num)
			{
				_E003(num, arg);
			}
		});
		_valueInput.onValueChanged.AddListener(delegate(string arg)
		{
			if (!int.TryParse(arg, out var result2))
			{
				result2 = this.m__E002;
			}
			if (this.m__E005 != result2)
			{
				if (result2 < this.m__E002)
				{
					result2 = this.m__E002;
				}
				else if (result2 > this.m__E003)
				{
					result2 = this.m__E003;
				}
				_E003(result2, _E002(this.m__E001, result2));
				_valueInput.text = result2.ToString();
			}
		});
		_valueInput.onEndEdit.AddListener(delegate(string value)
		{
			if (!int.TryParse(value, out var result))
			{
				_valueInput.text = CurrentValue().ToString(CultureInfo.InvariantCulture);
			}
			else if (result < this.m__E002)
			{
				_valueInput.text = this.m__E002.ToString();
			}
			else if (result > this.m__E003)
			{
				_valueInput.text = this.m__E003.ToString();
			}
		});
	}

	public void Focus()
	{
		_valueInput.ActivateInputField();
	}

	public void Show(int minValue, int maxValue, int currentValue)
	{
		Reset();
		int num = 24;
		this.m__E004 = _E001((decimal)maxValue / (decimal)num);
		if (maxValue / this.m__E004 < num)
		{
			num = _E001((decimal)maxValue / (decimal)this.m__E004);
		}
		bool flag = this.m__E004 == 1;
		for (int i = (flag ? 1 : 0); i < num; i++)
		{
			_E000(this.m__E004 * i);
		}
		this.m__E001.Add(_E001(maxValue));
		_slider.minValue = 1f;
		_slider.maxValue = (flag ? num : (num + 1));
		this.m__E002 = minValue;
		this.m__E003 = maxValue;
		_E003(currentValue, _E002(this.m__E001, currentValue));
	}

	private void _E000(int value)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(_notchTemplate, _notchContainer, worldPositionStays: false);
		gameObject.SetActive(value: true);
		this.m__E000.Add(gameObject);
		this.m__E001.Add(_E001(value));
	}

	private static int _E001(decimal value)
	{
		if (value <= 0m)
		{
			value = 1m;
		}
		return (int)Math.Ceiling(value);
	}

	private int _E002(IList<int> list, int value)
	{
		if (value >= this.m__E003)
		{
			return list.Count;
		}
		int item = list.Aggregate((int x, int y) => (Math.Abs(x - value) >= Math.Abs(y - value)) ? y : x);
		return list.IndexOf(item);
	}

	private void _E003(int realValue, float sliderValue)
	{
		if (sliderValue < _slider.minValue)
		{
			sliderValue = _slider.minValue;
		}
		if (sliderValue > _slider.maxValue)
		{
			sliderValue = _slider.maxValue;
		}
		if (Math.Abs(_slider.value - sliderValue) > Mathf.Epsilon)
		{
			UpdateValue(sliderValue);
		}
		if (realValue < this.m__E002)
		{
			realValue = this.m__E002;
		}
		if (realValue > this.m__E003)
		{
			realValue = this.m__E003;
		}
		if (this.m__E005 != realValue)
		{
			this.m__E005 = realValue;
			if (_valueInput.text != this.m__E005.ToString())
			{
				_valueInput.text = this.m__E005.ToString();
			}
		}
	}

	public void UpdateValue(float value, bool sendCallback = true, float? min = null, float? max = null)
	{
		_slider.value = value;
	}

	public void Bind(Action<float> valueChanged)
	{
	}

	public float CurrentValue()
	{
		return this.m__E005;
	}

	private void Reset()
	{
		foreach (GameObject item in this.m__E000)
		{
			UnityEngine.Object.DestroyImmediate(item);
		}
		this.m__E000.Clear();
		this.m__E001.Clear();
	}

	[CompilerGenerated]
	private void _E004(float arg)
	{
		int num = this.m__E001[(int)_slider.value - 1];
		if (this.m__E005 != num)
		{
			_E003(num, arg);
		}
	}

	[CompilerGenerated]
	private void _E005(string arg)
	{
		if (!int.TryParse(arg, out var result))
		{
			result = this.m__E002;
		}
		if (this.m__E005 != result)
		{
			if (result < this.m__E002)
			{
				result = this.m__E002;
			}
			else if (result > this.m__E003)
			{
				result = this.m__E003;
			}
			_E003(result, _E002(this.m__E001, result));
			_valueInput.text = result.ToString();
		}
	}

	[CompilerGenerated]
	private void _E006(string value)
	{
		if (!int.TryParse(value, out var result))
		{
			_valueInput.text = CurrentValue().ToString(CultureInfo.InvariantCulture);
		}
		else if (result < this.m__E002)
		{
			_valueInput.text = this.m__E002.ToString();
		}
		else if (result > this.m__E003)
		{
			_valueInput.text = this.m__E003.ToString();
		}
	}
}
