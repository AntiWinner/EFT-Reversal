using System;
using System.Collections;
using EFT.InputSystem;
using EFT.UI.Screens;
using UnityEngine;

namespace EFT.UI;

public sealed class ProfileLoadingScreen : EftScreen<ProfileLoadingScreen._E000, ProfileLoadingScreen>
{
	public new sealed class _E000 : _EC92._E000<_E000, ProfileLoadingScreen>
	{
		public override EEftScreenType ScreenType => EEftScreenType.ProfileLoading;

		protected override bool MainEnvironment => true;

		protected override EShadingStateSwitcher ShadingType => EShadingStateSwitcher.Default;

		protected override EStateSwitcher MenuChatBarVisibility => EStateSwitcher.Disabled;

		protected override EStateSwitcher TaskBarButtonsVisibility => EStateSwitcher.Disabled;

		protected override EStateSwitcher ShowEnvironment => EStateSwitcher.Enabled;

		protected override EStateSwitcher EnvironmentOverlay => EStateSwitcher.Disabled;

		protected override EStateSwitcher ShowEnvironmentCamera => EStateSwitcher.Enabled;

		public void SetLivingStatus(bool isLiving)
		{
			_E002._E000(isLiving);
		}
	}

	private new const string m__E000 = "Profile data loading...";

	private const string m__E001 = "Leaving the game...";

	[SerializeField]
	private RectTransform _loadingPanel;

	[SerializeField]
	private RectTransform _errorPanel;

	[SerializeField]
	private CustomTextMeshProUGUI _statusField;

	public override void Show(_E000 controller)
	{
		Show(loading: true);
	}

	private void Show(bool loading)
	{
		_E000(isLiving: false);
		ShowGameObject(instant: true);
		_loadingPanel.gameObject.SetActive(loading);
		_errorPanel.gameObject.SetActive(!loading);
	}

	private void _E000(bool isLiving)
	{
		_statusField.text = (isLiving ? _ED3E._E000(146837) : _ED3E._E000(248296)).Localized();
	}

	private static IEnumerator _E001(Action autoHideAction)
	{
		yield return new WaitForSeconds(6f);
		autoHideAction();
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		return InputNode.GetDefaultBlockResult(command);
	}
}
