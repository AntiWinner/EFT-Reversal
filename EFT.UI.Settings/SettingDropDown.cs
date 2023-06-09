using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Bsg.GameSettings;
using UnityEngine;

namespace EFT.UI.Settings;

public sealed class SettingDropDown : SettingControl
{
	[CompilerGenerated]
	private sealed class _E000<_E077> where _E077 : struct, Enum
	{
		public GameSetting<_E077> setting;

		public Action customUpdater;

		internal void _E000(int index)
		{
			setting.Value = _E3A5<_E077>.Values[index];
			customUpdater?.Invoke();
		}
	}

	[CompilerGenerated]
	private sealed class _E001<_E077> where _E077 : struct, Enum
	{
		public GameSetting<int> setting;

		internal void _E000(int index)
		{
			setting.Value = index;
		}
	}

	[CompilerGenerated]
	private sealed class _E002<_E077>
	{
		public GameSetting<int> setting;

		internal void _E000(int index)
		{
			setting.Value = index;
		}
	}

	[CompilerGenerated]
	private sealed class _E003<_E077>
	{
		public GameSetting<_E077> setting;

		public DropDownBox dropdown;

		public ReadOnlyCollection<_E077> variants;

		public Func<_E077, string> stringConverter;

		internal async Task _E000(_E077 value)
		{
			await setting.SetValue(value);
			_E001();
		}

		internal void _E001()
		{
			if (UpdateDropDownValue(dropdown, setting, variants))
			{
				dropdown.SetLabelText(stringConverter(setting));
			}
		}

		internal void _E002(int index)
		{
			_E000(variants[index]).HandleExceptions();
		}
	}

	[CompilerGenerated]
	private sealed class _E004<_E077>
	{
		public DropDownBox dropdown;

		public ReadOnlyCollection<_E077> variants;

		internal void _E000(_E077 x)
		{
			UpdateDropDownValue(dropdown, x, variants);
		}
	}

	[SerializeField]
	private DropDownBox DropDown;

	[SerializeField]
	private UiElementBlocker _blocker;

	public UiElementBlocker Blocker => _blocker;

	protected override Component TargetComponent => DropDown;

	public override void Close()
	{
		base.Close();
		DropDown.Hide();
	}

	public void BindToEnum<T>(GameSetting<T> setting, bool twoWays = false, Func<int, bool> validator = null, Action customUpdater = null) where T : struct, Enum
	{
		InitSetting(setting);
		DropDown.Show(_E3A5<T>.GetLocalizedNames, validator);
		DropDown.UpdateValue(_E3A5<T>.IndexOf(setting), sendCallback: false);
		DropDown.Bind(delegate(int index)
		{
			setting.Value = _E3A5<T>.Values[index];
			customUpdater?.Invoke();
		});
		if (twoWays)
		{
			UI.AddDisposable(setting.Subscribe(UpdateDropDownEnumValue));
		}
	}

	public void BindToEnumIndex<T>(GameSetting<int> setting) where T : struct, Enum
	{
		InitSetting(setting);
		DropDown.Show(_E3A5<T>.GetLocalizedNames);
		DropDown.UpdateValue(setting, sendCallback: false);
		DropDown.Bind(delegate(int index)
		{
			setting.Value = index;
		});
	}

	public void BindIndexTo<T>(GameSetting<int> setting, ReadOnlyCollection<T> variants, Func<T, string> stringConverter)
	{
		InitSetting(setting);
		DropDown.Show(variants.Select(stringConverter));
		DropDown.UpdateValue(setting, sendCallback: false);
		DropDown.Bind(delegate(int index)
		{
			setting.Value = index;
		});
	}

	public void BindTo<T>(GameSetting<T> setting, ReadOnlyCollection<T> variants, Func<T, string> stringConverter)
	{
		InitSetting(setting);
		BindDropDownToSetting(DropDown, setting, variants, stringConverter);
	}

	public static void BindDropDownToSetting<T>(DropDownBox dropdown, GameSetting<T> setting, ReadOnlyCollection<T> variants, Func<T, string> stringConverter)
	{
		_E003<T> CS_0024_003C_003E8__locals0 = new _E003<T>();
		CS_0024_003C_003E8__locals0.setting = setting;
		CS_0024_003C_003E8__locals0.dropdown = dropdown;
		CS_0024_003C_003E8__locals0.variants = variants;
		CS_0024_003C_003E8__locals0.stringConverter = stringConverter;
		CS_0024_003C_003E8__locals0.dropdown.Show(CS_0024_003C_003E8__locals0.variants.Select(CS_0024_003C_003E8__locals0.stringConverter));
		CS_0024_003C_003E8__locals0.dropdown.Bind(delegate(int index)
		{
			CS_0024_003C_003E8__locals0._E000(CS_0024_003C_003E8__locals0.variants[index]).HandleExceptions();
		});
		CS_0024_003C_003E8__locals0._E001();
	}

	public void UpdateDropDownEnumValue<T>(T value) where T : struct, Enum
	{
		UpdateDropDownValue(DropDown, value, _E3A5<T>.Values);
	}

	public static bool UpdateDropDownValue<T>(DropDownBox dropdown, T value, ReadOnlyCollection<T> variants)
	{
		int num = Mathf.Clamp(variants.IndexOf(value), 0, variants.Count);
		dropdown.UpdateValue(num, sendCallback: false);
		return num >= 0;
	}

	public static _E3A4 BindTwoWaySettingDropDown<T>(DropDownBox dropdown, GameSetting<T> setting, ReadOnlyCollection<T> variants, Func<T, string> stringConverter)
	{
		_E3A4 obj = new _E3A4();
		BindDropDownToSetting(dropdown, setting, variants, stringConverter);
		obj.SubscribeState(setting, delegate(T x)
		{
			UpdateDropDownValue(dropdown, x, variants);
		});
		obj.AddDisposable(dropdown.Hide);
		return obj;
	}
}
