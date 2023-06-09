using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.InputSystem;
using EFT.UI.Screens;
using TMPro;
using UnityEngine;

namespace EFT.UI.Matchmaker;

public sealed class MatchmakerTimeHasCome : EftScreen<MatchmakerTimeHasCome._E000, MatchmakerTimeHasCome>
{
	public new sealed class _E000 : _EC92._E000<_E000, MatchmakerTimeHasCome>
	{
		public readonly _E796 Session;

		public readonly RaidSettings RaidSettings;

		public bool SearchingForServer;

		[CompilerGenerated]
		private new Action m__E000;

		public override EEftScreenType ScreenType => EEftScreenType.TimeHasCome;

		public override bool KeyScreen => true;

		public bool LimitedServersAvailability
		{
			get
			{
				if (RaidSettings.RaidMode != ERaidMode.Coop)
				{
					return false;
				}
				return Session.LimitedServersAvailability;
			}
		}

		protected override EStateSwitcher MenuChatBarVisibility => EStateSwitcher.Disabled;

		public event Action OnAbortMatching
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

		public _E000(_E796 session, RaidSettings raidSettings)
		{
			Session = session;
			RaidSettings = raidSettings;
		}

		public void AbortMatching()
		{
			SearchingForServer = false;
			m__E000?.Invoke();
		}

		public void ChangeCancelButtonVisibility(bool value)
		{
			if (_E002 != null)
			{
				_E002._E002(value);
			}
		}

		public void ChangeStatus(string text, float? progress = null)
		{
			if (_E002 != null)
			{
				_E002._E003(text, progress);
			}
		}

		public void ShowPlayerModel()
		{
			if (_E002 != null)
			{
				_E002._E001().HandleExceptions();
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public _E857 notificationManager;

		public MatchmakerTimeHasCome _003C_003E4__this;

		internal void _E000()
		{
			notificationManager.OnNotificationReceived -= _003C_003E4__this._E000;
		}
	}

	private new const int m__E000 = 5;

	[SerializeField]
	private PlayerModelView _playerModelView;

	[SerializeField]
	private TextMeshProUGUI _locationName;

	[SerializeField]
	private PlayerLevelPanel _levelPanel;

	[SerializeField]
	private MatchmakerBannersPanel _bannersPanel;

	[SerializeField]
	private TextMeshProUGUI _deployingText;

	[SerializeField]
	private DefaultUIButton _cancelButton;

	private DateTime m__E001;

	private string m__E002;

	private bool m__E003;

	private readonly _EC76 m__E004 = new _EC76();

	private TimeSpan m__E005;

	private ERaidMode _E006;

	private RaidSettings _E007;

	private void Awake()
	{
		_cancelButton.OnClick.AddListener(_E004);
	}

	public override void Show(_E000 controller)
	{
		Show(controller.Session, controller.RaidSettings);
	}

	private void Show(_E796 session, RaidSettings raidSettings)
	{
		ESideType side = raidSettings.Side;
		_E554.Location selectedLocation = raidSettings.SelectedLocation;
		_E006 = raidSettings.RaidMode;
		_E007 = raidSettings;
		ShowGameObject(instant: true);
		if (selectedLocation != null)
		{
			_bannersPanel.Show(selectedLocation, side, session.Profile.Stats, session.LoadTextureMain);
		}
		UI.AddDisposable(_bannersPanel);
		_E002(value: true);
		this.m__E001 = _E5AD.Now;
		_locationName.text = ((selectedLocation != null) ? (selectedLocation._Id + _ED3E._E000(70087)) : _ED3E._E000(249071)).Localized();
		_levelPanel.Set(session.Profile.Info.Level, side);
		this.m__E002 = _ED3E._E000(260834) + _ED3E._E000(236629).Localized() + _ED3E._E000(47210);
		_E857 notificationManager = Singleton<_E857>.Instance;
		notificationManager.OnNotificationReceived += _E000;
		UI.AddDisposable(delegate
		{
			notificationManager.OnNotificationReceived -= _E000;
		});
	}

	private void _E000(_E856 notification)
	{
		if (notification != null && notification is _E886 && this.m__E003)
		{
			_E004();
		}
	}

	private async Task _E001()
	{
		await TasksExtensions.WaitUntil(() => Singleton<_E760>.Instance.IsPoolReady(_E760.PoolsCategory.Raid) || ScreenController.Closed);
		if (!ScreenController.Closed)
		{
			UI.AddDisposable(this.m__E004);
			this.m__E004.AddDisposable(_playerModelView);
			_playerModelView.Show(_E007.IsScav ? ScreenController.Session.ProfileOfPet : ScreenController.Session.Profile).HandleExceptions();
		}
	}

	private void _E002(bool value)
	{
		this.m__E003 = value;
		if (_cancelButton != null)
		{
			_cancelButton.gameObject.SetActive(this.m__E003);
		}
	}

	private void _E003(string status, float? progress = null)
	{
		this.m__E002 = (progress.HasValue ? string.Format(_ED3E._E000(236611), status.Localized(), Mathf.RoundToInt(progress.Value * 100f)) : (_ED3E._E000(260834) + status.Localized() + _ED3E._E000(47210)));
	}

	private void Update()
	{
		TimeSpan timeSpan = _E5AD.Now - this.m__E001;
		string text = this.m__E002 + string.Format(_ED3E._E000(236653), (int)timeSpan.TotalMinutes, timeSpan.Seconds);
		_deployingText.SetMonospaceText(text);
		if (!((timeSpan - this.m__E005).TotalSeconds < 5.0) && ScreenController.SearchingForServer && _E006 == ERaidMode.Coop)
		{
			this.m__E005 = timeSpan;
			this.m__E002 = (ScreenController.LimitedServersAvailability ? (_ED3E._E000(260834) + _ED3E._E000(236702).Localized() + _ED3E._E000(47210)) : (_ED3E._E000(260834) + _ED3E._E000(147239).Localized() + _ED3E._E000(47210)));
		}
	}

	private void _E004()
	{
		this.m__E004.Dispose();
		ScreenController.AbortMatching();
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (command.IsCommand(ECommand.Escape) && this.m__E003)
		{
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuEscape);
			_E004();
			return ETranslateResult.BlockAll;
		}
		return InputNode.GetDefaultBlockResult(command);
	}

	protected override void OnDestroy()
	{
		if (base.gameObject.activeSelf)
		{
			Debug.LogError(_ED3E._E000(236729));
		}
		base.OnDestroy();
	}

	[CompilerGenerated]
	private bool _E005()
	{
		if (!Singleton<_E760>.Instance.IsPoolReady(_E760.PoolsCategory.Raid))
		{
			return ScreenController.Closed;
		}
		return true;
	}
}
