using EFT.InputSystem;
using EFT.UI.Settings;
using UnityEngine;

namespace EFT.UI.Screens;

public sealed class PostFXPreviewScreen : UIScreen
{
	[SerializeField]
	private PostFXSettingsTab _postFXTab;

	[SerializeField]
	private DefaultUIButton _applyButton;

	private _EC94<EEftScreenType> _E0B2;

	private _E7E1 _E0B3;

	private void Awake()
	{
		_applyButton.OnClick.AddListener(_E000);
	}

	public void Show(SettingsScreen._E000 settingsController)
	{
		UIEventSystem.Instance.Enable();
		_E0B2 = settingsController;
		_postFXTab.gameObject.SetActive(value: true);
		_applyButton.gameObject.SetActive(value: true);
		_postFXTab.Show(settingsController.TempSettings.PostFx);
		GradingPostFX postFX = _E8A8.Instance.PostFX;
		if (postFX != null)
		{
			_E0B3 = new _E7E1(postFX);
			_E0B3.BindTo(settingsController.TempSettings.PostFx, previewMode: true);
		}
		ShowGameObject();
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (command.IsCommand(ECommand.Escape))
		{
			_E001();
		}
		return InputNode.GetDefaultBlockResult(command);
	}

	private void _E000()
	{
		_E001();
	}

	private void _E001()
	{
		UIEventSystem.Instance.Disable();
		_E0B2.RestoreScreen();
		Close();
	}

	public override void Close()
	{
		_E0B3?.Dispose();
		_E0B3 = null;
		_postFXTab.Close();
		base.Close();
	}
}
