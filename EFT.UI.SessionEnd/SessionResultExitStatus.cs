using System;
using System.Runtime.CompilerServices;
using System.Threading;
using EFT.InputSystem;
using EFT.UI.Screens;
using UnityEngine;

namespace EFT.UI.SessionEnd;

public sealed class SessionResultExitStatus : EftScreen<SessionResultExitStatus._E000, SessionResultExitStatus>
{
	public new sealed class _E000 : _EC92._E000<_E000, SessionResultExitStatus>
	{
		[CompilerGenerated]
		private new Action m__E000;

		[CompilerGenerated]
		private Action _E001;

		public readonly Profile ActiveProfile;

		public readonly ESideType PlayerSide;

		public readonly ExitStatus ExitStatus;

		public readonly TimeSpan RaidTime;

		public readonly bool IsOnline;

		public readonly _E796 Session;

		public readonly PlayerVisualRepresentation LastPlayerState;

		public override EEftScreenType ScreenType => EEftScreenType.ExitStatus;

		protected override bool MainEnvironment => false;

		public override bool KeyScreen => true;

		protected override EStateSwitcher TaskBarButtonsVisibility => EStateSwitcher.Disabled;

		protected override EStateSwitcher MenuChatBarVisibility => EStateSwitcher.Disabled;

		public event Action OnShowNextScreen
		{
			[CompilerGenerated]
			add
			{
				Action action = m__E000;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Combine(action2, value);
					action = Interlocked.CompareExchange(ref m__E000, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action action = m__E000;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Remove(action2, value);
					action = Interlocked.CompareExchange(ref m__E000, value2, action2);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnGoToMainMenu
		{
			[CompilerGenerated]
			add
			{
				Action action = _E001;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Combine(action2, value);
					action = Interlocked.CompareExchange(ref _E001, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action action = _E001;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Remove(action2, value);
					action = Interlocked.CompareExchange(ref _E001, value2, action2);
				}
				while ((object)action != action2);
			}
		}

		public _E000(Profile activeProfile, PlayerVisualRepresentation lastPlayerState, ESideType side, ExitStatus exitStatus, TimeSpan raidTime, _E796 session, bool isOnline)
		{
			ActiveProfile = activeProfile;
			LastPlayerState = lastPlayerState;
			PlayerSide = side;
			IsOnline = isOnline;
			ExitStatus = exitStatus;
			RaidTime = raidTime;
			Session = session;
		}

		public void ShowNextScreen()
		{
			m__E000?.Invoke();
		}

		public void GoToMainMenu()
		{
			_E001?.Invoke();
		}
	}

	[SerializeField]
	private DefaultUIButton _nextButton;

	[SerializeField]
	private DefaultUIButton _mainMenuButton;

	[SerializeField]
	private PlayerLevelPanel _levelPanel;

	[SerializeField]
	private PlayerNamePanel _namePanel;

	[SerializeField]
	private PlayerNamePanel _killerNamePanel;

	[SerializeField]
	private CustomTextMeshProUGUI _bodyPartLabel;

	[SerializeField]
	private PlayerModelView _playerModelView;

	[SerializeField]
	private GameObject _survivedPanel;

	[SerializeField]
	private GameObject _leftPanel;

	[SerializeField]
	private GameObject _missingPanel;

	[SerializeField]
	private GameObject _killedPanel;

	[SerializeField]
	private GameObject _runnerPanel;

	[SerializeField]
	private GameObject _warningPanel;

	[SerializeField]
	private CustomTextMeshProUGUI _warningCaption;

	[SerializeField]
	private CustomTextMeshProUGUI _warningDescription;

	[SerializeField]
	private CustomTextMeshProUGUI _raidTime;

	[SerializeField]
	private CustomTextMeshProUGUI _experience;

	[SerializeField]
	private ReportPanel _reportPanel;

	private void Awake()
	{
		_nextButton.OnClick.AddListener(ScreenController.ShowNextScreen);
		_mainMenuButton.OnClick.AddListener(ScreenController.GoToMainMenu);
	}

	public override void Show(_E000 controller)
	{
		Show(controller.ActiveProfile, controller.LastPlayerState, controller.PlayerSide, controller.ExitStatus, controller.RaidTime, controller.Session, controller.IsOnline);
	}

	private void Show(Profile activeProfile, PlayerVisualRepresentation lastPlayerState, ESideType side, ExitStatus exitStatus, TimeSpan raidTime, _E796 session, bool isOnline)
	{
		_ECC9.ReleaseBeginSample(_ED3E._E000(254498), _ED3E._E000(85454));
		ShowGameObject();
		_levelPanel.Set(activeProfile.Info.Level, side);
		_namePanel.Set(activeProfile);
		AggressorStats aggressor = activeProfile.Stats.Aggressor;
		bool flag = aggressor != null && exitStatus == ExitStatus.Killed;
		_killerNamePanel.gameObject.SetActive(flag);
		_bodyPartLabel.gameObject.SetActive(flag);
		if (flag)
		{
			string text = ((aggressor.ProfileId != activeProfile.Id) ? aggressor.GetCorrectedNickname() : string.Empty);
			if (aggressor.Side == EPlayerSide.Savage && !string.IsNullOrEmpty(aggressor.MainProfileNickname))
			{
				string text2 = aggressor.MainProfileNickname;
				if (aggressor.Category == EMemberCategory.UniqueId)
				{
					Color iconColor = _E3A2.Load<ChatSpecialIconSettings>(_ED3E._E000(250600)).GetDataByMemberCategory(aggressor.Category).IconColor;
					text2 = _ED3E._E000(59472) + ColorUtility.ToHtmlStringRGBA(iconColor) + _ED3E._E000(59465) + text2 + _ED3E._E000(59467);
				}
				text = text + _ED3E._E000(54246) + text2 + _ED3E._E000(27308);
			}
			string text3 = aggressor.BodyPart.ToStringNoBox().Localized(EStringCase.Lower);
			if (aggressor.BodyPart == EBodyPart.Head && aggressor.HeadSegment.HasValue)
			{
				text3 = text3 + _ED3E._E000(10270) + (_ED3E._E000(254535) + aggressor.HeadSegment.Value).Localized().ToLower();
			}
			_bodyPartLabel.text = _ED3E._E000(27312) + text3 + _ED3E._E000(27308);
			_killerNamePanel.Set(aggressor.Side != EPlayerSide.Savage, aggressor.Category, text);
			if (session.ReportAvailable && !string.IsNullOrEmpty(aggressor.ProfileId))
			{
				_reportPanel.Show(session);
			}
		}
		_survivedPanel.SetActive(exitStatus == ExitStatus.Survived);
		_leftPanel.SetActive(exitStatus == ExitStatus.Left);
		_missingPanel.SetActive(exitStatus == ExitStatus.MissingInAction);
		_killedPanel.SetActive(exitStatus == ExitStatus.Killed);
		_runnerPanel.SetActive(exitStatus == ExitStatus.Runner);
		_warningPanel.SetActive(exitStatus != ExitStatus.Survived && isOnline);
		switch (exitStatus)
		{
		case ExitStatus.Left:
			_warningCaption.text = _ED3E._E000(254580).Localized();
			_warningDescription.text = _ED3E._E000(254630).Localized();
			break;
		case ExitStatus.Killed:
		case ExitStatus.MissingInAction:
			_warningCaption.text = _ED3E._E000(254725).Localized();
			_warningDescription.text = _ED3E._E000(254832).Localized();
			break;
		case ExitStatus.Runner:
			_warningCaption.text = _ED3E._E000(254883).Localized();
			_warningDescription.text = _ED3E._E000(254953).Localized();
			break;
		}
		_raidTime.text = string.Format(_ED3E._E000(59439), raidTime.Hours, raidTime.Minutes, raidTime.Seconds);
		_experience.text = activeProfile.Stats.TotalSessionExperience.ToThousandsString();
		if (lastPlayerState != null)
		{
			_playerModelView.Show(lastPlayerState).HandleExceptions();
		}
		else
		{
			_playerModelView.Show(activeProfile).HandleExceptions();
		}
		UI.AddDisposable(_playerModelView);
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		return InputNode.GetDefaultBlockResult(command);
	}
}
