using System.Runtime.CompilerServices;
using Bsg.GameSettings;
using UnityEngine;

namespace EFT.UI.Settings;

public sealed class SettingToggle : SettingControl
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public GameSetting<bool> setting;

		internal void _E000(bool value)
		{
			setting.Value = value;
		}
	}

	[SerializeField]
	private UpdatableToggle Toggle;

	protected override Component TargetComponent => Toggle;

	public void BindTo(GameSetting<bool> setting)
	{
		InitSetting(setting);
		Toggle.UpdateValue(setting, sendCallback: false);
		Toggle.Bind(delegate(bool value)
		{
			setting.Value = value;
		});
	}
}
