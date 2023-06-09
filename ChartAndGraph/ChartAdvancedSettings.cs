using System;
using UnityEngine;

namespace ChartAndGraph;

[Serializable]
internal class ChartAdvancedSettings
{
	private static ChartAdvancedSettings mInstance;

	private static string[] FractionDigits = new string[8]
	{
		_ED3E._E000(245675),
		_ED3E._E000(245665),
		_ED3E._E000(259361),
		_ED3E._E000(245721),
		_ED3E._E000(245715),
		_ED3E._E000(245702),
		_ED3E._E000(245754),
		_ED3E._E000(245743)
	};

	[Range(0f, 7f)]
	public int ValueFractionDigits = 2;

	[Range(0f, 7f)]
	public int AxisFractionDigits = 2;

	public static ChartAdvancedSettings Instance
	{
		get
		{
			if (mInstance == null)
			{
				mInstance = new ChartAdvancedSettings();
			}
			return mInstance;
		}
	}

	private string _E000(string format, double val)
	{
		try
		{
			return string.Format(format, val);
		}
		catch
		{
		}
		return _ED3E._E000(18502);
	}

	private string _E001(int value)
	{
		value = Mathf.Clamp(value, 0, 7);
		return FractionDigits[value];
	}

	public string FormatFractionDigits(int digits, double val)
	{
		return _E000(_E001(digits), val);
	}
}
