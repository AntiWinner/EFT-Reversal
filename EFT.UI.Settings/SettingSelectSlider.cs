using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Bsg.GameSettings;
using UnityEngine;

namespace EFT.UI.Settings;

public sealed class SettingSelectSlider : SettingControl
{
	[CompilerGenerated]
	private sealed class _E000<_E077>
	{
		public GameSetting<int> setting;

		internal void _E000(int index)
		{
			setting.Value = index;
		}
	}

	[SerializeField]
	private SelectSlider Slider;

	protected override Component TargetComponent => Slider;

	public void BindIndexTo<T>(GameSetting<int> setting, ReadOnlyCollection<T> variants, Func<T, string> stringConverter)
	{
		InitSetting(setting);
		UI.AddDisposable(Slider);
		Slider.Show(variants.Select(stringConverter).ToArray);
		Slider.UpdateValue(Mathf.Clamp(setting, 0, variants.Count), sendCallback: false);
		Slider.Bind(delegate(int index)
		{
			setting.Value = index;
		});
	}

	public static bool UpdateSliderValue<T>(SelectSlider slider, T value, ReadOnlyCollection<T> variants)
	{
		int num = Mathf.Clamp(variants.IndexOf(value), 0, variants.Count);
		slider.UpdateValue(num, sendCallback: false);
		return num >= 0;
	}
}
