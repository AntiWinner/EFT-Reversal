using Bsg.GameSettings;
using UnityEngine;

namespace EFT.UI.Settings;

public abstract class SettingControl : UIElement
{
	[SerializeField]
	private LocalizedText Text;

	private HoverTooltipArea _E02A;

	private UiElementBlocker _E258;

	protected abstract Component TargetComponent { get; }

	protected void InitSetting(IGameSetting setting)
	{
		UI.Dispose();
		Text.LocalizationKey = setting.Key;
	}

	public HoverTooltipArea GetOrCreateTooltip()
	{
		if (_E02A != null)
		{
			return _E02A;
		}
		_E02A = Text.gameObject.AddComponent<HoverTooltipArea>();
		return _E02A;
	}
}
