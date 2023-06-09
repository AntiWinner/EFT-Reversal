using System.Runtime.CompilerServices;
using Bsg.GameSettings;
using UnityEngine;

namespace EFT.UI.Settings;

public sealed class SettingFloatSlider : SettingControl
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public GameSetting<int> setting;

		internal void _E000(float value)
		{
			setting.Value = (int)value;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public GameSetting<float> setting;

		internal void _E000(float value)
		{
			setting.Value = value;
		}
	}

	[SerializeField]
	private FloatSlider Slider;

	protected override Component TargetComponent => Slider;

	public string Format
	{
		set
		{
			Slider.Format = value;
		}
	}

	public void BindTo(GameSetting<int> setting, int minValue, int maxValue)
	{
		InitSetting(setting);
		Format = _ED3E._E000(27314);
		Slider.Show(minValue, maxValue);
		Slider.UpdateValue((int)setting, sendCallback: true, minValue, maxValue);
		Slider.Bind(delegate(float value)
		{
			setting.Value = (int)value;
		});
	}

	public void BindTo(GameSetting<float> setting, float minValue, float maxValue)
	{
		InitSetting(setting);
		Slider.Show(minValue, maxValue);
		Slider.UpdateValue(setting, sendCallback: true, minValue, maxValue);
		Slider.Bind(delegate(float value)
		{
			setting.Value = value;
		});
	}
}
