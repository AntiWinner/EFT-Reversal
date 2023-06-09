using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace EFT.UI.Settings;

public sealed class PostFXSettingsTab : SettingsTab
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public SettingsScreen._E000 screenController;

		internal async void _E000()
		{
			screenController.IgnoreScreenInterruptionCheck = true;
			if (await _EC92.Instance.TryReturnToRootScreen())
			{
				MonoBehaviourSingleton<GameUI>.Instance.PostFXPreview.Show(screenController);
			}
		}
	}

	[SerializeField]
	private UpdatableToggle _overallToggle;

	[SerializeField]
	private FloatSlider _brightness;

	[SerializeField]
	private FloatSlider _saturation;

	[SerializeField]
	private FloatSlider _clarity;

	[SerializeField]
	private FloatSlider _colorfulness;

	[SerializeField]
	private FloatSlider _lumaSharpen;

	[SerializeField]
	private FloatSlider _adaptiveSharpen;

	[SerializeField]
	private FloatSlider _filterIntensity;

	[SerializeField]
	private FloatSlider _colorblindnessIntensity;

	[SerializeField]
	private DropDownBox _colorGrading;

	[SerializeField]
	private DropDownBox _colorblindnessType;

	[SerializeField]
	private DefaultUIButton _visualizeButton;

	[SerializeField]
	private CanvasGroup[] _toggleRelatedCanvases;

	private _E7E3 _E27A;

	public void Show(_E7E3 settings, SettingsScreen._E000 screenController = null)
	{
		_E27A = settings;
		_E000(_E27A);
		_visualizeButton.gameObject.SetActive(_E7A3.InRaid && screenController != null);
		if (screenController == null || !_E7A3.InRaid)
		{
			return;
		}
		_visualizeButton.OnClick.AddListener(async delegate
		{
			screenController.IgnoreScreenInterruptionCheck = true;
			if (await _EC92.Instance.TryReturnToRootScreen())
			{
				MonoBehaviourSingleton<GameUI>.Instance.PostFXPreview.Show(screenController);
			}
		});
		UI.AddDisposable(_visualizeButton.OnClick.RemoveAllListeners);
	}

	private void _E000(_E7E3 settings)
	{
		SettingsTab.BindFloatSliderToSetting(_clarity, settings.Clarity, -100f, 100f);
		SettingsTab.BindFloatSliderToSetting(_brightness, settings.Brightness, -100f, 100f);
		SettingsTab.BindFloatSliderToSetting(_saturation, settings.Saturation, -100f, 100f);
		SettingsTab.BindFloatSliderToSetting(_colorfulness, settings.Colorfulness, 0f, 100f);
		SettingsTab.BindFloatSliderToSetting(_lumaSharpen, settings.LumaSharpen, 0f, 100f);
		SettingsTab.BindFloatSliderToSetting(_adaptiveSharpen, settings.AdaptiveSharpen, 0f, 100f);
		SettingsTab.BindFloatSliderToSetting(_filterIntensity, settings.Intensity, 0f, 100f);
		SettingsTab.BindFloatSliderToSetting(_colorblindnessIntensity, settings.ColorBlindnessIntensity, 0f, 100f);
		ShowEnumDropDown(_colorGrading, settings.ColorFilterType, _ED3E._E000(234594));
		ShowEnumDropDown(_colorblindnessType, settings.ColorBlindnessType, _ED3E._E000(234640));
		SettingsTab.BindToggleToSetting(_overallToggle, settings.EnablePostFx);
		UI.BindState(settings.EnablePostFx, _E001);
	}

	public override Task TakeSettingsFrom(_E7DE settingsManager)
	{
		_E27A.TakeSettingsFrom(settingsManager.PostFx.Default);
		return Task.CompletedTask;
	}

	private void _E001(bool value)
	{
		CanvasGroup[] toggleRelatedCanvases = _toggleRelatedCanvases;
		for (int i = 0; i < toggleRelatedCanvases.Length; i++)
		{
			toggleRelatedCanvases[i].SetUnlockStatus(value);
		}
	}
}
