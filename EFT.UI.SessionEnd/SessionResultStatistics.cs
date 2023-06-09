using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.InputSystem;
using EFT.UI.Screens;
using UnityEngine;

namespace EFT.UI.SessionEnd;

public sealed class SessionResultStatistics : EftScreen<SessionResultStatistics._E000, SessionResultStatistics>
{
	public new sealed class _E000 : _EC90<_E000, SessionResultStatistics>
	{
		public readonly Profile Profile;

		public readonly _E554.Location Location;

		public override EEftScreenType ScreenType => EEftScreenType.SessionStatistics;

		protected override bool MainEnvironment => false;

		public override bool KeyScreen => true;

		protected override EStateSwitcher TaskBarButtonsVisibility => EStateSwitcher.Disabled;

		protected override EStateSwitcher MenuChatBarVisibility => EStateSwitcher.Disabled;

		public _E000(Profile profile, _E554.Location location)
		{
			Profile = profile;
			Location = location;
		}
	}

	public sealed class _E001
	{
		public readonly List<_E34D._E000> StatItems = new List<_E34D._E000>();

		public string Caption;

		public Sprite Icon;

		public StatisticsSpawn.EStatGroupLayoutType LayoutType;
	}

	[SerializeField]
	private DefaultUIButton _nextButton;

	[SerializeField]
	private DefaultUIButton _backButton;

	[SerializeField]
	private StatisticsSpawn _statsSpawn;

	[SerializeField]
	private CustomTextMeshProUGUI _locationName;

	private void Awake()
	{
		_backButton.OnClick.AddListener(delegate
		{
			ScreenController.CloseScreen();
		});
		_nextButton.OnClick.AddListener(delegate
		{
			_nextButton.Interactable = false;
			_backButton.Interactable = false;
			ScreenController._E000();
		});
	}

	public override void Show(_E000 controller)
	{
		Show(controller.Profile, controller.Location);
	}

	private void Show(Profile profile, _E554.Location location)
	{
		ShowGameObject();
		_nextButton.Interactable = true;
		_backButton.Interactable = true;
		_locationName.text = (location._Id + _ED3E._E000(70087)).Localized();
		_statsSpawn._E000(profile, StatisticsSpawn.EStatisticsType.Session);
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (!command.IsCommand(ECommand.Escape))
		{
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuEscape);
			return InputNode.GetDefaultBlockResult(command);
		}
		ScreenController.CloseScreen();
		return ETranslateResult.BlockAll;
	}

	public override void Close()
	{
		_statsSpawn.Close();
		base.Close();
	}

	[CompilerGenerated]
	private void _E000()
	{
		ScreenController.CloseScreen();
	}

	[CompilerGenerated]
	private void _E001()
	{
		_nextButton.Interactable = false;
		_backButton.Interactable = false;
		ScreenController._E000();
	}
}
