using System;
using System.Globalization;
using EFT.HealthSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Health;

public class HealthParameterPanel : MonoBehaviour
{
	[SerializeField]
	private TMP_Text _currentValue;

	[SerializeField]
	private TMP_Text _maxValue;

	[SerializeField]
	private Image _glow;

	[SerializeField]
	private Color _warningColor;

	[SerializeField]
	private Color _alternativeWarningColor;

	[SerializeField]
	private int _minimalStringLength = -1;

	[SerializeField]
	private Color _additionalZerosColor = new Color(0.6f, 0.6f, 0.6f, 0.2f);

	private float _E008 = float.MinValue;

	private float _E009 = float.MinValue;

	private float _E00A = float.MinValue;

	private float _E00B = float.MinValue;

	private Color _E00C;

	protected virtual void Awake()
	{
		_E00C = _currentValue.color;
	}

	public void SetParameterValue(ValueStruct value, float warningThreshold, int roundValue, bool countFromTop = false)
	{
		if (!_E000(value, warningThreshold, roundValue))
		{
			return;
		}
		_E00B = value.Maximum;
		_E00A = value.Current;
		_E008 = warningThreshold;
		_E009 = roundValue;
		string text = ((roundValue <= 0) ? ((value.Normalized > 0.5f) ? Math.Floor(value.Current) : Math.Ceiling(value.Current)).ToString(_ED3E._E000(229566), CultureInfo.InvariantCulture) : Math.Round(value.Current, roundValue).ToString(CultureInfo.InvariantCulture));
		int num = _minimalStringLength - text.Length;
		if (num > 0)
		{
			string text2 = ColorUtility.ToHtmlStringRGBA(_additionalZerosColor);
			string value2 = _ED3E._E000(59472) + text2 + _ED3E._E000(59465) + new string('0', num) + _ED3E._E000(59467);
			text = text.Insert(0, value2);
		}
		_currentValue.SetMonospaceText(text);
		if (_maxValue != null)
		{
			_maxValue.SetMonospaceText(string.Format(_ED3E._E000(229561), value.Maximum));
		}
		if (warningThreshold >= 0f)
		{
			bool flag = (countFromTop ? ((float)(int)value.Current >= warningThreshold) : ((float)(int)value.Current <= warningThreshold));
			_currentValue.color = (flag ? _warningColor : _E00C);
			if (_glow != null && _glow.gameObject.activeSelf != flag)
			{
				_glow.gameObject.SetActive(flag);
			}
		}
	}

	public void SetWarningColor(bool display, bool useAlternative)
	{
		_currentValue.color = ((!display) ? _E00C : (useAlternative ? _alternativeWarningColor : _warningColor));
	}

	private bool _E000(ValueStruct value, float warningThreshold, int roundValue)
	{
		if (!(Math.Abs(value.Maximum - _E00B) >= float.Epsilon) && !(Math.Abs(value.Current - _E00A) >= float.Epsilon) && !(Math.Abs(warningThreshold - _E008) >= float.Epsilon))
		{
			return Math.Abs((float)roundValue - _E009) >= float.Epsilon;
		}
		return true;
	}
}
