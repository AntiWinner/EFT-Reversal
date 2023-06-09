using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class SelectSlider : UIElement, _EC72<int>
{
	[SerializeField]
	private Slider _slider;

	[SerializeField]
	private TextMeshProUGUI _valueText;

	[SerializeField]
	private GameObject _notchTemplate;

	[SerializeField]
	private RectTransform _notchContainer;

	private readonly List<GameObject> _E098 = new List<GameObject>();

	private Action<int> _E20C;

	private Func<string[]> _E213;

	private bool _E214;

	private string[] _E20F;

	private void Awake()
	{
		_slider.onValueChanged.AddListener(delegate(float val)
		{
			if (!_E214)
			{
				UpdateValue((int)val);
			}
		});
		_slider.wholeNumbers = true;
	}

	public void Show(string[] values)
	{
		_E214 = true;
		_E20F = values;
		_slider.maxValue = _E20F.Length - 1;
		_E002();
		for (int i = 0; i < _E20F.Length - 1; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(_notchTemplate, _notchContainer, worldPositionStays: false);
			gameObject.SetActive(value: true);
			_E098.Add(gameObject);
		}
		_E214 = false;
		ShowGameObject();
	}

	public void Show(Func<string[]> values)
	{
		_E213 = values;
		_E000();
		UI.Dispose();
		UI.AddDisposable(_E7AD._E010.AddLocaleUpdateListener(_E000));
		ShowGameObject();
	}

	private void _E000()
	{
		Show(_E213());
		_E001(_E20F[CurrentValue()]);
	}

	public void UpdateValue(int value, bool sendCallback = true, int? min = null, int? max = null)
	{
		if (_E20F != null && _E20F.Length != 0)
		{
			value = ((min.HasValue && max.HasValue) ? Mathf.Clamp(value, min.Value, max.Value) : Mathf.Clamp(value, 0, _E20F.Length - 1));
			_slider.Set(value, sendCallback);
			_E001(_E20F[value]);
			if (_E20C != null && sendCallback)
			{
				_E20C(value);
			}
		}
	}

	internal void _E001(string text)
	{
		_valueText.text = text;
		_valueText.SetAllDirty();
	}

	public void Bind([CanBeNull] Action<int> valueChanged)
	{
		_E20C = valueChanged;
	}

	public int CurrentValue()
	{
		return (int)_slider.value;
	}

	private void _E002()
	{
		foreach (GameObject item in _E098)
		{
			UnityEngine.Object.DestroyImmediate(item);
		}
		_E098.Clear();
	}

	[CompilerGenerated]
	private void _E003(float val)
	{
		if (!_E214)
		{
			UpdateValue((int)val);
		}
	}
}
