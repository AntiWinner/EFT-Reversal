using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Comfort.Common;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using EFT.InputSystem;
using EFT.UI.Screens;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Matchmaker;

public sealed class MatchMakerAcceptScreen : MatchmakerEftScreen<MatchMakerAcceptScreen._E000, MatchMakerAcceptScreen>
{
	public new sealed class _E000 : _EC8F<_E000, MatchMakerAcceptScreen>
	{
		[CompilerGenerated]
		private new sealed class _E000
		{
			public _EC94<EEftScreenType> previousScreen;

			internal void _E000()
			{
				previousScreen.RestoreScreen();
				MonoBehaviourSingleton<PreloaderUI>.Instance.ShowErrorScreen(_ED3E._E000(234013).Localized(), _ED3E._E000(234005).Localized());
			}
		}

		[CompilerGenerated]
		private new Action<string, EMatchingType> m__E000;

		public readonly _E796 Session;

		public readonly RaidSettings RaidSettings;

		public override EEftScreenType ScreenType => EEftScreenType.MatchMakerAccept;

		public override bool KeyScreen => true;

		protected override EStateSwitcher MenuChatBarVisibility => EStateSwitcher.Disabled;

		protected override EStateSwitcher TaskBarButtonsVisibility => EStateSwitcher.Disabled;

		protected override EStateSwitcher ShowEnvironment => EStateSwitcher.Enabled;

		protected override EStateSwitcher ShowEnvironmentCamera => EStateSwitcher.Enabled;

		public event Action<string, EMatchingType> OnReadyToStartRaid
		{
			[CompilerGenerated]
			add
			{
				Action<string, EMatchingType> action = this.m__E000;
				Action<string, EMatchingType> action2;
				do
				{
					action2 = action;
					Action<string, EMatchingType> value2 = (Action<string, EMatchingType>)Delegate.Combine(action2, value);
					action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action<string, EMatchingType> action = this.m__E000;
				Action<string, EMatchingType> action2;
				do
				{
					action2 = action;
					Action<string, EMatchingType> value2 = (Action<string, EMatchingType>)Delegate.Remove(action2, value);
					action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
				}
				while ((object)action != action2);
			}
		}

		public _E000(_E796 session, ref RaidSettings raidSettings, _EC99 matchmakerPlayersController)
			: base(matchmakerPlayersController)
		{
			Session = session;
			RaidSettings = raidSettings;
		}

		public void ShowNextScreen(string groupId, EMatchingType matchingType)
		{
			this.m__E000?.Invoke(groupId, matchingType);
		}

		public void NavigateToRoot()
		{
			if (!(_E002 == null) && !base.Closed)
			{
				_EC94<EEftScreenType> previousScreen = base._E00A;
				while (previousScreen.ScreenType != EEftScreenType.SelectRaidSide && !previousScreen.Root)
				{
					previousScreen = previousScreen.PreviousScreen;
				}
				_E002._E010(delegate
				{
					previousScreen.RestoreScreen();
					MonoBehaviourSingleton<PreloaderUI>.Instance.ShowErrorScreen(_ED3E._E000(234013).Localized(), _ED3E._E000(234005).Localized());
				}).HandleExceptions();
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public MatchMakerAcceptScreen _003C_003E4__this;

		public _E000 screenController;

		public Func<bool> _003C_003E9__0;

		internal bool _E000()
		{
			if (!_003C_003E4__this.m__E005)
			{
				return screenController.Closed;
			}
			return true;
		}
	}

	[CompilerGenerated]
	private sealed class _E004
	{
		public TweenerCore<Quaternion, Vector3, QuaternionOptions> tween;

		internal void _E000()
		{
			tween.Kill();
		}
	}

	[CompilerGenerated]
	private sealed class _E007
	{
		public _E000 screenController;

		internal bool _E000()
		{
			return screenController.Closed;
		}
	}

	private new const string m__E000 = "Looking for group...";

	private const string m__E001 = "Stop looking for group";

	[SerializeField]
	private PlayersRaidReadyPanel _playersRaidReadyPanel;

	[SerializeField]
	private DefaultUIButton _acceptButton;

	[SerializeField]
	private DefaultUIButton _backButton;

	[SerializeField]
	private TextMeshProUGUI _locationName;

	[SerializeField]
	private TextMeshProUGUI _groupMembersCountLabel;

	[SerializeField]
	private GameObject _coopHighLoadPanel;

	[SerializeField]
	private Button _updateListButton;

	[SerializeField]
	private Button _coopSettingsButton;

	[SerializeField]
	private MatchMakerPlayerPreview _playerModelView;

	[SerializeField]
	private MatchMakerGroupPreview _groupPreview;

	[SerializeField]
	private LocationConditionsPanel _conditions;

	[SerializeField]
	private SimpleContextMenu _contextMenu;

	[SerializeField]
	private CanvasGroup _canvasGroup;

	[SerializeField]
	private CoopSettingsWindow _coopSettingsWindow;

	private _E796 m__E002;

	private Profile m__E003;

	private ERaidMode m__E004;

	private bool m__E005;

	private bool m__E006;

	private RaidSettings m__E007;

	private string m__E008;

	private bool m__E009;

	private static int _E00A
	{
		get
		{
			if (!Singleton<_E5B7>.Instantiated)
			{
				return 20;
			}
			return Math.Max(Singleton<_E5B7>.Instance.GroupStatusInterval, 20);
		}
	}

	private static int _E00B
	{
		get
		{
			if (!Singleton<_E5B7>.Instantiated)
			{
				return 7;
			}
			return Math.Max(Singleton<_E5B7>.Instance.GroupStatusButtonInterval, 7);
		}
	}

	private void Awake()
	{
		_acceptButton.OnClick.AddListener(delegate
		{
			_E00D().HandleExceptions();
		});
		_backButton.OnClick.AddListener(delegate
		{
			_E010(delegate
			{
				ScreenController.CloseScreen();
			}).HandleExceptions();
		});
		_updateListButton.onClick.AddListener(delegate
		{
			this.m__E005 = true;
			_E009().HandleExceptions();
		});
		_coopSettingsButton.onClick.AddListener(_E003);
	}

	public override void Show(_E000 controller)
	{
		UI.Dispose();
		base.Show(controller);
		Show(controller.Session, controller.RaidSettings);
	}

	public void ShowContextMenu(_EC9B player, Vector2 position)
	{
		if (this.m__E004 != ERaidMode.Local)
		{
			_contextMenu.Show(position, MatchmakerPlayersController.GetContextInteractions(player, inLobby: true));
		}
	}

	private void Show(_E796 session, RaidSettings raidSettings)
	{
		ShowGameObject();
		this.m__E009 = false;
		_E554.Location selectedLocation = raidSettings.SelectedLocation;
		MatchmakerPlayersController.UpdateMaxGroupCount();
		this.m__E004 = raidSettings.RaidMode;
		this.m__E007 = raidSettings;
		_coopSettingsButton.gameObject.SetActive(this.m__E004 == ERaidMode.Coop);
		this.m__E002 = session;
		Profile profile = this.m__E002.Profile;
		this.m__E003 = (raidSettings.IsScav ? this.m__E002.ProfileOfPet : profile);
		this.m__E008 = profile.AccountId;
		_E000().HandleExceptions();
		_locationName.text = (selectedLocation._Id + _ED3E._E000(70087)).Localized();
		_conditions.Set(this.m__E002, raidSettings, takeFromCurrent: false);
		if (this.m__E004 == ERaidMode.Local)
		{
			_acceptButton.Interactable = true;
			_playersRaidReadyPanel.Close();
			_groupPreview.Close();
			_E004(maxPveExceeded: false);
		}
		else
		{
			MatchmakerPlayersController.OnLookingForGroupStatusChanged += _E00B;
			MatchmakerPlayersController.OnMatchingAvailabilityChanged += _E007;
			MatchmakerPlayersController.OnLimitedServersAvailability += _E004;
			MatchmakerPlayersController.OnPlayerContextRequest += ShowContextMenu;
			MatchmakerPlayersController.OnRaidReadyStatusChanged += _E002;
			_E007(MatchmakerPlayersController.MatchingStatus);
			_E004(MatchmakerPlayersController.LimitedServersAvailability);
			_E005(MatchmakerPlayersController.CurrentProfileMatchingStatus);
			Singleton<_E857>.Instance.OnNotificationReceived += _E00C;
			UI.AddDisposable(delegate
			{
				MatchmakerPlayersController.OnLookingForGroupStatusChanged -= _E00B;
				MatchmakerPlayersController.OnMatchingAvailabilityChanged -= _E007;
				MatchmakerPlayersController.OnLimitedServersAvailability -= _E004;
				MatchmakerPlayersController.OnPlayerContextRequest -= ShowContextMenu;
				MatchmakerPlayersController.OnRaidReadyStatusChanged -= _E002;
				if (Singleton<_E857>.Instantiated)
				{
					Singleton<_E857>.Instance.OnNotificationReceived -= _E00C;
				}
			});
			UI.BindEvent(MatchmakerPlayersController.GroupPlayers.ItemsChanged, delegate
			{
				_groupMembersCountLabel.text = string.Format(_ED3E._E000(233980), MatchmakerPlayersController.GroupPlayers.Count, MatchmakerPlayersController.MaxGroupCount.Value);
			});
			UI.AddDisposable(_groupPreview);
			UI.AddDisposable(_playersRaidReadyPanel);
			_updateListButton.interactable = false;
			_E00E().HandleExceptions();
			if (_EC99.StartLookingForGroupByDefault)
			{
				MatchmakerPlayersController.StartLookingForGroup().HandleExceptions();
			}
		}
		_EC9B player = MatchmakerPlayersController?.CurrentPlayer ?? _EC99.CreateRaidPlayer(profile, this.m__E003);
		UI.AddDisposable(_playerModelView);
		_playerModelView.Show(MatchmakerPlayersController, this.m__E007, player, MatchmakerPlayersController.GetContextInteractions(player, inLobby: true)).HandleExceptions();
		_canvasGroup.blocksRaycasts = true;
	}

	private async Task _E000()
	{
		if (!this.m__E007.Local && !(await this.m__E002.SendRaidSettings(this.m__E007)).Failed)
		{
			MatchmakerPlayersController.SetCurrentPlayerReadyStatus(isReady: true);
		}
	}

	protected override void InviteAcceptedHandler()
	{
		if (!MatchmakerPlayersController.LeaderRaidReadyStatus || this.m__E007.Local)
		{
			_E001();
			ScreenController.CloseScreen();
		}
	}

	protected override void MatchingTypeUpdateHandler(EMatchingType matchingType)
	{
		_E005(matchingType);
	}

	private void _E001()
	{
		_EC94<EEftScreenType> previousScreen = ((_EC94<EEftScreenType>)ScreenController).PreviousScreen;
		while (!previousScreen.Root)
		{
			if (previousScreen.ScreenType == EEftScreenType.Insurance)
			{
				previousScreen.Disabled = true;
			}
			previousScreen = previousScreen.PreviousScreen;
		}
	}

	private void _E002(_EC9B player, bool status)
	{
		if (!(player.AccountId == this.m__E008) && !MatchmakerPlayersController.LeaderRaidReadyStatus)
		{
			_E001();
			ScreenController.CloseScreen();
		}
	}

	private void _E003()
	{
		if (_coopSettingsWindow.gameObject.activeSelf)
		{
			_coopSettingsWindow.Close();
		}
		_coopSettingsWindow.Show(this.m__E007);
	}

	private void _E004(bool maxPveExceeded)
	{
		if (this.m__E004 != ERaidMode.Coop)
		{
			_coopHighLoadPanel.SetActive(value: false);
		}
		else
		{
			_coopHighLoadPanel.SetActive(maxPveExceeded);
		}
	}

	private void _E005(EMatchingType matchingType)
	{
		if (matchingType == EMatchingType.GroupPlayer && !MatchmakerPlayersController.LeaderRaidReadyStatus)
		{
			_E001();
			ScreenController.CloseScreen();
			return;
		}
		_updateListButton.gameObject.SetActive(matchingType != EMatchingType.GroupPlayer);
		switch (matchingType)
		{
		case EMatchingType.Single:
			this.m__E006 = true;
			_groupPreview.Close();
			_playersRaidReadyPanel.Close();
			MatchmakerPlayersController.SetCurrentPlayerReadyStatus(isReady: true);
			break;
		case EMatchingType.GroupPlayer:
			_E008().HandleExceptions();
			_groupPreview.Show(this.m__E008, MatchmakerPlayersController, this.m__E007, MatchmakerPlayersController.GetContextInteractions);
			_playersRaidReadyPanel.Show(this.m__E008, this.m__E002.SocialNetwork.FriendsList, MatchmakerPlayersController, showRaidReady: false);
			_updateListButton.interactable = false;
			break;
		case EMatchingType.GroupLeader:
			_E008().HandleExceptions();
			_groupPreview.Show(this.m__E008, MatchmakerPlayersController, this.m__E007, MatchmakerPlayersController.GetContextInteractions);
			_playersRaidReadyPanel.Show(this.m__E008, this.m__E002.SocialNetwork.FriendsList, MatchmakerPlayersController);
			break;
		default:
			throw new ArgumentOutOfRangeException(_ED3E._E000(233878), matchingType, null);
		}
		_E006();
	}

	private void _E006()
	{
		if (this.m__E004 == ERaidMode.Coop && this.m__E007.PlayersSpawnPlace != 0)
		{
			_groupPreview.Close();
		}
	}

	private void _E007(EMatchingStatus matchingStatus)
	{
		_acceptButton.SetDisabledTooltip(matchingStatus.LocalizedEnum());
		_acceptButton.Interactable = matchingStatus == EMatchingStatus.Ready;
	}

	private async Task _E008()
	{
		_E000 screenController = ScreenController;
		this.m__E006 = false;
		while (!screenController.Closed && this.m__E004 != ERaidMode.Local && !this.m__E006)
		{
			Task task = Task.Delay(MatchMakerAcceptScreen._E00A * 1000);
			await _E00A();
			this.m__E005 = false;
			Task task2 = TasksExtensions.WaitUntil(() => this.m__E005 || screenController.Closed);
			await Task.WhenAny(task, task2);
		}
	}

	private async Task _E009()
	{
		_updateListButton.interactable = false;
		CancellationToken cancellationToken = UI.CancellationToken;
		TweenerCore<Quaternion, Vector3, QuaternionOptions> tween = _updateListButton.transform.DORotate(new Vector3(0f, 0f, (float)(-360 * MatchMakerAcceptScreen._E00B) / 2f), MatchMakerAcceptScreen._E00B, RotateMode.FastBeyond360);
		UI.AddDisposable(delegate
		{
			tween.Kill();
		});
		await tween.SetEase(Ease.Linear);
		if (!cancellationToken.IsCancellationRequested)
		{
			_updateListButton.transform.rotation = Quaternion.identity;
			_updateListButton.interactable = true;
		}
	}

	private Task _E00A()
	{
		_E009().HandleExceptions();
		return MatchmakerPlayersController.UpdateStatus();
	}

	private void _E00B(bool isLooking)
	{
		if (!MatchmakerPlayersController.Disposed)
		{
			_E857.DisplayMessageNotification((isLooking ? _ED3E._E000(233906) : _ED3E._E000(233867)).Localized());
		}
		if (isLooking)
		{
			_E008().HandleExceptions();
			_playersRaidReadyPanel.Show(this.m__E008, this.m__E002.SocialNetwork.FriendsList, MatchmakerPlayersController);
			return;
		}
		this.m__E006 = true;
		if (MatchmakerPlayersController.CurrentProfileMatchingStatus == EMatchingType.Single)
		{
			_playersRaidReadyPanel.Close();
		}
	}

	private void _E00C(_E856 notification)
	{
		if (notification is _E882 && !MatchmakerPlayersController.IsOwner)
		{
			Debug.Log(_ED3E._E000(233951));
			_E00D().HandleExceptions();
		}
	}

	private async Task _E00D()
	{
		if (!_canvasGroup.blocksRaycasts)
		{
			return;
		}
		_canvasGroup.blocksRaycasts = false;
		this.m__E009 = true;
		EMatchingType matchingType = EMatchingType.Single;
		string groupId = string.Empty;
		if (MatchmakerPlayersController.GroupPlayers.Count == 1)
		{
			ScreenController.ShowNextScreen(groupId, matchingType);
			return;
		}
		_EC99.StartLookingForGroupByDefault = false;
		if (MatchmakerPlayersController.Group == null)
		{
			await _E010();
		}
		else
		{
			matchingType = ((!MatchmakerPlayersController.IsOwner) ? EMatchingType.GroupPlayer : EMatchingType.GroupLeader);
			groupId = MatchmakerPlayersController.GroupId;
		}
		ScreenController.ShowNextScreen(groupId, matchingType);
	}

	private async Task _E00E()
	{
		int teamSearchingTimeout = Singleton<_E5CB>.Instance.TeamSearchingTimeout;
		if (teamSearchingTimeout > 0)
		{
			Task task = Task.Delay(teamSearchingTimeout * 1000 * 60);
			_E000 screenController = ScreenController;
			Task task2 = TasksExtensions.WaitUntil(() => screenController.Closed);
			await Task.WhenAny(task, task2);
			if (!task2.IsCompleted)
			{
				_E00F();
			}
		}
	}

	private void _E00F()
	{
		ScreenController.NavigateToRoot();
	}

	private async Task _E010(Action callback = null)
	{
		if (_canvasGroup.blocksRaycasts)
		{
			if (MatchmakerPlayersController == null)
			{
				callback?.Invoke();
				return;
			}
			_canvasGroup.blocksRaycasts = false;
			callback?.Invoke();
		}
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (command.IsCommand(ECommand.Escape))
		{
			if (_coopSettingsWindow.gameObject.activeSelf)
			{
				_coopSettingsWindow.Close();
				return ETranslateResult.Block;
			}
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuEscape);
			_E010(ScreenController.CloseScreen).HandleExceptions();
			return ETranslateResult.BlockAll;
		}
		return InputNode.GetDefaultBlockResult(command);
	}

	public override void Close()
	{
		if (!this.m__E009)
		{
			MatchmakerPlayersController.SetCurrentPlayerReadyStatus(isReady: false);
		}
		MatchmakerPlayersController.StopLookingForGroup().HandleExceptions();
		if (MatchmakerPlayersController.GroupPlayers.Count == 1)
		{
			MatchmakerPlayersController.ExitFromMatchMaker().HandleExceptions();
		}
		_coopSettingsWindow.Close();
		MatchmakerPlayersController.ResetMaxGroupCount();
		base.Close();
	}

	[CompilerGenerated]
	private void _E011()
	{
		_E00D().HandleExceptions();
	}

	[CompilerGenerated]
	private void _E012()
	{
		_E010(delegate
		{
			ScreenController.CloseScreen();
		}).HandleExceptions();
	}

	[CompilerGenerated]
	private void _E013()
	{
		ScreenController.CloseScreen();
	}

	[CompilerGenerated]
	private void _E014()
	{
		this.m__E005 = true;
		_E009().HandleExceptions();
	}

	[CompilerGenerated]
	private void _E015()
	{
		MatchmakerPlayersController.OnLookingForGroupStatusChanged -= _E00B;
		MatchmakerPlayersController.OnMatchingAvailabilityChanged -= _E007;
		MatchmakerPlayersController.OnLimitedServersAvailability -= _E004;
		MatchmakerPlayersController.OnPlayerContextRequest -= ShowContextMenu;
		MatchmakerPlayersController.OnRaidReadyStatusChanged -= _E002;
		if (Singleton<_E857>.Instantiated)
		{
			Singleton<_E857>.Instance.OnNotificationReceived -= _E00C;
		}
	}

	[CompilerGenerated]
	private void _E016()
	{
		_groupMembersCountLabel.text = string.Format(_ED3E._E000(233980), MatchmakerPlayersController.GroupPlayers.Count, MatchmakerPlayersController.MaxGroupCount.Value);
	}
}
