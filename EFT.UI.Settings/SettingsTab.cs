using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Bsg.GameSettings;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Settings;

public abstract class SettingsTab : UIElement
{
	[Serializable]
	[Obsolete("Replace this with UiElementBlocker")]
	protected struct ElementBlocker
	{
		public Graphic Graphic;

		public CanvasGroup Group;

		public HoverTooltipArea Tooltip;

		public void StartBlock(string reason = null)
		{
			Graphic.enabled = true;
			Group.SetUnlockStatus(value: false);
			Tooltip.enabled = !string.IsNullOrEmpty(reason);
			Tooltip.SetMessageText(reason);
		}

		public void RemoveBlock()
		{
			Graphic.enabled = false;
			Group.SetUnlockStatus(value: true);
			Tooltip.enabled = false;
		}
	}

	[CompilerGenerated]
	private sealed class _E000<_E077> where _E077 : struct, Enum
	{
		public string localizationPrefix;

		public GameSetting<_E077> setting;

		public Action valueChanged;

		public Func<string, string> _003C_003E9__2;

		internal IEnumerable<string> _E000()
		{
			if (!string.IsNullOrEmpty(localizationPrefix))
			{
				return _E3A5<_E077>.Names.Select((string x) => (localizationPrefix + x).Localized()).ToArray();
			}
			return ((IReadOnlyList<string>)_E3A5<_E077>.Names).Localized(EStringCase.None);
		}

		internal string _E001(string x)
		{
			return (localizationPrefix + x).Localized();
		}

		internal void _E002(int index)
		{
			setting.Value = _E3A5<_E077>.Values[index];
			valueChanged?.Invoke();
		}
	}

	[CompilerGenerated]
	private sealed class _E001<_E077>
	{
		public GameSetting<_E077> setting;

		public ReadOnlyCollection<_E077> variants;

		public Action valueChanged;

		internal void _E000(int index)
		{
			setting.Value = variants[index];
			valueChanged?.Invoke();
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public GameSetting<int> setting;

		internal void _E000(float value)
		{
			setting.Value = (int)value;
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public GameSetting<float> setting;

		internal void _E000(float value)
		{
			setting.Value = value;
		}
	}

	[CompilerGenerated]
	private sealed class _E004
	{
		public GameSetting<bool> setting;

		public Action valueChanged;

		internal void _E000(bool arg)
		{
			setting.Value = arg;
			valueChanged?.Invoke();
		}
	}

	[CompilerGenerated]
	private Action<bool> _E27B;

	private static readonly string[] _E27C = new string[11]
	{
		_ED3E._E000(27314),
		_ED3E._E000(234628),
		_ED3E._E000(234631),
		_ED3E._E000(234626),
		_ED3E._E000(234685),
		_ED3E._E000(234680),
		_ED3E._E000(234683),
		_ED3E._E000(234678),
		_ED3E._E000(234673),
		_ED3E._E000(234668),
		_ED3E._E000(261918)
	};

	private List<SettingControl> _E27D;

	public bool IsSelected
	{
		set
		{
			base.gameObject.SetActive(value);
			if (value)
			{
				OnTabSelected();
			}
		}
	}

	public event Action<bool> OnLoadingInProgress
	{
		[CompilerGenerated]
		add
		{
			Action<bool> action = _E27B;
			Action<bool> action2;
			do
			{
				action2 = action;
				Action<bool> value2 = (Action<bool>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E27B, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<bool> action = _E27B;
			Action<bool> action2;
			do
			{
				action2 = action;
				Action<bool> value2 = (Action<bool>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E27B, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public abstract Task TakeSettingsFrom(_E7DE settingsManager);

	protected virtual void OnTabSelected()
	{
	}

	public override void Close()
	{
		if (_E27D != null)
		{
			foreach (SettingControl item in _E27D)
			{
				item.Close();
				UnityEngine.Object.Destroy(item.gameObject);
			}
			_E27D.Clear();
		}
		base.Close();
	}

	protected T CreateControl<T>(T prefab, Transform parent) where T : SettingControl
	{
		T val = UnityEngine.Object.Instantiate(prefab, parent);
		_E27D = _E27D ?? new List<SettingControl>(8);
		_E27D.Add(val);
		val.gameObject.SetActive(value: true);
		return val;
	}

	protected void SetLoadingStatus(bool inProgress)
	{
		_E27B?.Invoke(inProgress);
	}

	protected void RegisterDropDown(DropDownBox dropDownBox)
	{
		UI.AddDisposable(dropDownBox.Hide);
	}

	[Obsolete("Replace this with SettingDropDown")]
	protected void ShowEnumDropDown<T>(DropDownBox dropdown, GameSetting<T> setting, string localizationPrefix = null, Action valueChanged = null) where T : struct, Enum
	{
		dropdown.Show(() => (!string.IsNullOrEmpty(localizationPrefix)) ? _E3A5<T>.Names.Select((string x) => (localizationPrefix + x).Localized()).ToArray() : ((IReadOnlyList<string>)_E3A5<T>.Names).Localized(EStringCase.None));
		dropdown.UpdateValue(_E3A5<T>.IndexOf(setting), sendCallback: false);
		dropdown.Bind(delegate(int index)
		{
			setting.Value = _E3A5<T>.Values[index];
			valueChanged?.Invoke();
		});
		RegisterDropDown(dropdown);
	}

	[Obsolete("Replace this with SettingSelectSlider")]
	protected void BindSelectSliderToSetting<T>(SelectSlider slider, GameSetting<T> setting, ReadOnlyCollection<T> variants, Func<T, string> stringConverter, Action valueChanged = null)
	{
		slider.Show(variants.Select(stringConverter).ToArray());
		UI.AddDisposable(slider);
		slider.Bind(delegate(int index)
		{
			setting.Value = variants[index];
			valueChanged?.Invoke();
		});
		if (SettingSelectSlider.UpdateSliderValue(slider, setting, variants))
		{
			slider._E001(stringConverter(setting));
		}
	}

	protected void ShowFakeSlider(SelectSlider selectSlider)
	{
		selectSlider.Show(_E27C);
		selectSlider.UpdateValue(_E27C.Length - 1, sendCallback: false);
		UI.AddDisposable(selectSlider);
	}

	[Obsolete("Replace this with SettingFloatSlider")]
	protected static void BindFloatSliderToSetting(FloatSlider slider, GameSetting<int> setting, float minValue, float maxValue)
	{
		slider.Show(minValue, maxValue);
		slider.UpdateValue((int)setting, sendCallback: true, minValue, maxValue);
		slider.Bind(delegate(float value)
		{
			setting.Value = (int)value;
		});
	}

	[Obsolete("Replace this with SettingFloatSlider")]
	protected static void BindFloatSliderToSetting(FloatSlider slider, GameSetting<float> setting, float minValue, float maxValue)
	{
		slider.Show(minValue, maxValue);
		slider.UpdateValue(setting, sendCallback: true, minValue, maxValue);
		slider.Bind(delegate(float value)
		{
			setting.Value = value;
		});
	}

	[Obsolete("Replace this with SettingToggle")]
	protected static void BindToggleToSetting(UpdatableToggle toggle, GameSetting<bool> setting, Action valueChanged = null)
	{
		toggle.UpdateValue(setting, sendCallback: false);
		toggle.Bind(delegate(bool arg)
		{
			setting.Value = arg;
			valueChanged?.Invoke();
		});
	}

	protected static void ShowFakeToggle(UpdatableToggle toggle)
	{
		toggle.UpdateValue(value: false, sendCallback: false);
	}
}
