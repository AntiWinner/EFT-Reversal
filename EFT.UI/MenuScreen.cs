using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Comfort.Common;
using EFT.InputSystem;
using EFT.UI.Matchmaker;
using EFT.UI.Screens;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class MenuScreen : EftScreen<MenuScreen._E000, MenuScreen>
{
	public new abstract class _E000 : _EC92._E000<_E000, MenuScreen>
	{
		public _EC99 Matchmaker;

		[CompilerGenerated]
		private new Action<EMenuType, bool> m__E000;

		public override EEftScreenType ScreenType => EEftScreenType.MainMenu;

		protected override EShadingStateSwitcher ShadingType => EShadingStateSwitcher.Default;

		protected override bool MainEnvironment => true;

		public override bool KeyScreen => true;

		protected override EStateSwitcher MenuChatBarVisibility => EStateSwitcher.Enabled;

		protected override EStateSwitcher ShowEnvironmentCamera => EStateSwitcher.Enabled;

		protected override EStateSwitcher EnvironmentOverlay => EStateSwitcher.Disabled;

		protected override EStateSwitcher ShowEnvironment => EStateSwitcher.Enabled;

		protected override EStateSwitcher CameraBlur => EStateSwitcher.Disabled;

		public event Action<EMenuType, bool> OnMenuItemSelected
		{
			[CompilerGenerated]
			add
			{
				Action<EMenuType, bool> action = m__E000;
				Action<EMenuType, bool> action2;
				do
				{
					action2 = action;
					Action<EMenuType, bool> value2 = (Action<EMenuType, bool>)Delegate.Combine(action2, value);
					action = Interlocked.CompareExchange(ref m__E000, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action<EMenuType, bool> action = m__E000;
				Action<EMenuType, bool> action2;
				do
				{
					action2 = action;
					Action<EMenuType, bool> value2 = (Action<EMenuType, bool>)Delegate.Remove(action2, value);
					action = Interlocked.CompareExchange(ref m__E000, value2, action2);
				}
				while ((object)action != action2);
			}
		}

		protected _E000(_EC99 matchmaker = null)
		{
			Matchmaker = matchmaker;
		}

		protected override void ShowAction(MenuScreen screen)
		{
			base.ShowAction(screen);
			screen._E004(minimized: false);
			GUISounds instance = Singleton<GUISounds>.Instance;
			instance._E005(isActive: true);
			instance._E006(active: false);
		}

		public void SelectMenuItem(EMenuType itemType)
		{
			m__E000?.Invoke(itemType, arg2: true);
		}
	}

	public sealed class _E001 : _E000
	{
		protected override EStateSwitcher TaskBarButtonsVisibility => EStateSwitcher.Enabled;

		public _E001(_EC99 matchmaker)
			: base(matchmaker)
		{
		}

		protected override void ShowAction(MenuScreen screen)
		{
			base.ShowAction(screen);
			_E002._E000(reconnectAvailable: false);
		}
	}

	public sealed class _E002 : _E000
	{
		protected override EStateSwitcher TaskBarButtonsVisibility => EStateSwitcher.Disabled;

		protected override void ShowAction(MenuScreen screen)
		{
			base.ShowAction(screen);
			_E002._E000(reconnectAvailable: true);
		}
	}

	public sealed class _E003 : _E000
	{
		protected override EStateSwitcher UnrestrictedFrameRate => EStateSwitcher.Enabled;

		protected override EStateSwitcher TaskBarButtonsVisibility => EStateSwitcher.Disabled;

		protected override void ShowAction(MenuScreen screen)
		{
			_E002._E000(reconnectAvailable: false);
			screen._E002();
		}
	}

	[SerializeField]
	private DefaultUIButton _playButton;

	[SerializeField]
	private DefaultUIButton _playerButton;

	[SerializeField]
	private DefaultUIButton _tradeButton;

	[SerializeField]
	private DefaultUIButton _exitButton;

	[SerializeField]
	private DefaultUIButton _disconnectButton;

	[SerializeField]
	private DefaultUIButton _hideoutButton;

	[SerializeField]
	private Button _logoutButton;

	[SerializeField]
	private DefaultUIButton _hideScreenButton;

	[SerializeField]
	private GameObject _warningGameObject;

	[SerializeField]
	private GameObject _alphaWarningGameObject;

	private new bool m__E000;

	private EnvironmentUI m__E001;

	private DateTime m__E002;

	public Action OwnerOnLeaveGameEvent;

	private bool m__E003;

	private _EC99 m__E004;

	private void _E000(bool reconnectAvailable)
	{
		this.m__E000 = reconnectAvailable;
		_tradeButton.gameObject.SetActive(!this.m__E000);
		_hideoutButton.gameObject.SetActive(!this.m__E000);
		_playerButton.gameObject.SetActive(!this.m__E000);
		_alphaWarningGameObject.SetActive(!this.m__E000);
		_warningGameObject.SetActive(this.m__E000);
	}

	private void Awake()
	{
		_playButton.OnClick.AddListener(delegate
		{
			_E003(this.m__E000 ? EMenuType.Reconnect : EMenuType.Play);
		});
		_playerButton.OnClick.AddListener(delegate
		{
			_E003(EMenuType.Player);
		});
		_disconnectButton.OnClick.AddListener(delegate
		{
			OwnerOnLeaveGameEvent?.Invoke();
		});
		_tradeButton.OnClick.AddListener(delegate
		{
			_E003(EMenuType.Trade);
		});
		_hideoutButton.OnClick.AddListener(delegate
		{
			_E003(EMenuType.Hideout);
		});
		_exitButton.OnClick.AddListener(delegate
		{
			_E003(EMenuType.Exit);
		});
		_logoutButton.onClick.AddListener(delegate
		{
			_E003(EMenuType.Logout);
		});
		_hideScreenButton.OnClick.AddListener(delegate
		{
			ScreenController.CloseScreen();
		});
		_logoutButton.gameObject.SetActive(value: false);
	}

	public void Init(EnvironmentUI environment)
	{
		this.m__E001 = environment;
	}

	private void _E001(EMatchingType matchingType)
	{
		bool flag = matchingType != EMatchingType.GroupPlayer || this.m__E000 || this.m__E003;
		if (!flag)
		{
			_playButton.SetDisabledTooltip(EMatchingStatus.GroupPlayer.LocalizedEnum());
		}
		_playButton.Interactable = flag;
	}

	private void _E002()
	{
		this.m__E001.ResetRotation();
		_E004(minimized: true);
	}

	private void _E003(EMenuType menuType)
	{
		ScreenController.SelectMenuItem(menuType);
	}

	internal void _E004(bool minimized)
	{
		this.m__E003 = minimized;
		_playButton.gameObject.SetActive(!minimized);
		_playerButton.gameObject.SetActive(!minimized);
		_tradeButton.gameObject.SetActive(!minimized);
		_hideoutButton.gameObject.SetActive(!minimized);
		_exitButton.gameObject.SetActive(!minimized);
		_disconnectButton.gameObject.SetActive(minimized);
		_hideScreenButton.gameObject.SetActive(minimized);
	}

	public override void Show(_E000 controller)
	{
		Show(controller.Matchmaker);
	}

	private void Show(_EC99 matchmaker)
	{
		ShowGameObject();
		this.m__E004 = matchmaker;
		this.WaitFrames(3, delegate
		{
			if (_E7A3.InRaid)
			{
				_E3AB.Collect();
			}
			else
			{
				_E3AB.GCEnabled = true;
				_E3AB.Collect();
			}
			if (_E3AB.Settings.OverrideRamCleanerSettings ? _E3AB.Settings.RamCleanerEnabled : ((bool)Singleton<_E7DE>.Instance.Game.Settings.AutoEmptyWorkingSet))
			{
				_E3AB.EmptyWorkingSet();
			}
		});
		_playButton.Interactable = true;
		if (this.m__E004 != null)
		{
			this.m__E004.OnMatchingTypeUpdate += _E001;
			_E001(this.m__E004.CurrentProfileMatchingStatus);
			UI.AddDisposable(delegate
			{
				this.m__E004.OnMatchingTypeUpdate -= _E001;
			});
		}
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (command.IsCommand(ECommand.ToggleInventory))
		{
			if (!this.m__E003 && !this.m__E000)
			{
				_E003(EMenuType.Player);
			}
			return ETranslateResult.BlockAll;
		}
		if (command.IsCommand(ECommand.Escape) && this.m__E003)
		{
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuEscape);
			ScreenController.CloseScreen();
			return ETranslateResult.BlockAll;
		}
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

	[CompilerGenerated]
	private void _E005()
	{
		_E003(this.m__E000 ? EMenuType.Reconnect : EMenuType.Play);
	}

	[CompilerGenerated]
	private void _E006()
	{
		_E003(EMenuType.Player);
	}

	[CompilerGenerated]
	private void _E007()
	{
		OwnerOnLeaveGameEvent?.Invoke();
	}

	[CompilerGenerated]
	private void _E008()
	{
		_E003(EMenuType.Trade);
	}

	[CompilerGenerated]
	private void _E009()
	{
		_E003(EMenuType.Hideout);
	}

	[CompilerGenerated]
	private void _E00A()
	{
		_E003(EMenuType.Exit);
	}

	[CompilerGenerated]
	private void _E00B()
	{
		_E003(EMenuType.Logout);
	}

	[CompilerGenerated]
	private void _E00C()
	{
		ScreenController.CloseScreen();
	}

	[CompilerGenerated]
	private void _E00D()
	{
		this.m__E004.OnMatchingTypeUpdate -= _E001;
	}
}
