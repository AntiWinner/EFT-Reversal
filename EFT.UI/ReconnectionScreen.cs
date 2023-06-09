using System;
using System.Runtime.CompilerServices;
using System.Threading;
using EFT.InputSystem;
using EFT.UI.Matchmaker;
using EFT.UI.Screens;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT.UI;

public sealed class ReconnectionScreen : EftScreen<ReconnectionScreen._E000, ReconnectionScreen>
{
	public new sealed class _E000 : _EC92._E000<_E000, ReconnectionScreen>
	{
		[CompilerGenerated]
		private new Action m__E000;

		[CompilerGenerated]
		private Action _E001;

		public readonly Profile ActiveProfile;

		public readonly _E554.Location Location;

		public readonly ESideType Side;

		public readonly bool ReturnAllowed;

		public readonly bool NextScreenAllowed;

		public readonly _E796 Session;

		public override EEftScreenType ScreenType => EEftScreenType.Reconnect;

		public override bool KeyScreen => true;

		protected override EStateSwitcher TaskBarButtonsVisibility => EStateSwitcher.Disabled;

		public event Action OnReconnectAction
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

		public event Action OnLeave
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

		public _E000(Profile activeProfile, _E554.Location location, ESideType side, bool returnAllowed, bool nextScreenAllowed, _E796 session)
		{
			ActiveProfile = activeProfile;
			Location = location;
			Side = side;
			ReturnAllowed = returnAllowed;
			NextScreenAllowed = nextScreenAllowed;
			Session = session;
		}

		public void Reconnect()
		{
			m__E000?.Invoke();
		}

		public void Leave()
		{
			MonoBehaviourSingleton<PreloaderUI>.Instance.SetMenuChatBarVisibility(visible: false);
			_E001?.Invoke();
		}
	}

	[SerializeField]
	private DefaultUIButton _reconnectButton;

	[SerializeField]
	private DefaultUIButton _backButton;

	[SerializeField]
	private DefaultUIButton _exitButton;

	[SerializeField]
	private CustomTextMeshProUGUI _locationName;

	[SerializeField]
	private PlayerLevelPanel _levelPanel;

	[SerializeField]
	private PlayerModelView _playerModelView;

	[SerializeField]
	private MatchmakerBannersPanel _bannersPanel;

	public override void Show(_E000 controller)
	{
		Show(controller.ActiveProfile, controller.Location, controller.Side, controller.ReturnAllowed, controller.NextScreenAllowed, controller.Session);
	}

	private void Awake()
	{
		_reconnectButton.OnClick.AddListener(delegate
		{
			ScreenController.Reconnect();
		});
		_backButton.OnClick.AddListener(delegate
		{
			ScreenController.CloseScreen();
		});
		_exitButton.OnClick.AddListener(delegate
		{
			_E000(available: false);
			ScreenController.Leave();
		});
	}

	private void Show(Profile activeProfile, [CanBeNull] _E554.Location location, ESideType side, bool returnAllowed, bool nextScreenAllowed, _E796 session)
	{
		Display();
		_levelPanel.Set(activeProfile.Info.Level, side);
		_locationName.text = ((location != null) ? (location._Id + _ED3E._E000(70087)) : _ED3E._E000(249071)).Localized();
		_reconnectButton.gameObject.SetActive(nextScreenAllowed);
		_backButton.gameObject.SetActive(returnAllowed);
		_exitButton.gameObject.SetActive(value: true);
		_E000(available: true);
		_playerModelView.Show(activeProfile).HandleExceptions();
		UI.AddDisposable(_playerModelView);
		if (location != null)
		{
			_bannersPanel.Show(location, side, activeProfile.Stats, session.LoadTextureMain);
		}
	}

	private void _E000(bool available)
	{
		_reconnectButton.Interactable = available;
		_backButton.Interactable = available;
		_exitButton.Interactable = available;
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		return InputNode.GetDefaultBlockResult(command);
	}

	protected override void TranslateAxes(ref float[] axes)
	{
		axes = null;
	}

	protected override ECursorResult ShouldLockCursor()
	{
		return ECursorResult.ShowCursor;
	}

	public override void Close()
	{
		_bannersPanel.Close();
		base.Close();
	}

	[CompilerGenerated]
	private void _E001()
	{
		ScreenController.Reconnect();
	}

	[CompilerGenerated]
	private void _E002()
	{
		ScreenController.CloseScreen();
	}

	[CompilerGenerated]
	private void _E003()
	{
		_E000(available: false);
		ScreenController.Leave();
	}
}
