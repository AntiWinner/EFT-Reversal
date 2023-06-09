using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.InputSystem;
using EFT.UI.Screens;
using UnityEngine;

namespace EFT.UI.Settings;

public sealed class SettingsScreen : EftScreen<SettingsScreen._E000, SettingsScreen>
{
	public enum ESettingsGroup
	{
		Screen,
		Game,
		Sound,
		Control,
		PostFX
	}

	public new class _E000 : _EC92._E000<_E000, SettingsScreen>
	{
		public new sealed class _E000
		{
			public _E7EF Game;

			public _E7E0 Sound;

			public _E7E3 PostFx;

			public _E7EB Graphics;

			public _E7F2 Control;

			public static SettingsScreen._E000._E000 CopyFromManager(_E7DE manager)
			{
				return new SettingsScreen._E000._E000
				{
					Game = manager.Game.Settings.Clone(),
					Sound = manager.Sound.Settings.Clone(),
					PostFx = manager.PostFx.Settings.Clone(),
					Graphics = manager.Graphics.Settings.Clone(),
					Control = manager.Control.Settings.Clone()
				};
			}

			public bool HasSameSettings(_E7DE manager)
			{
				if (manager.Game.Settings.HasSameSettings(Game) && manager.Sound.Settings.HasSameSettings(Sound) && manager.PostFx.Settings.HasSameSettings(PostFx) && manager.Graphics.Settings.HasSameSettings(Graphics))
				{
					return manager.Control.Settings.HasSameSettings(Control);
				}
				return false;
			}

			public async Task ApplyTo(_E7DE manager)
			{
				await manager.Game.TakeSettingsFrom(Game);
				await manager.Sound.TakeSettingsFrom(Sound);
				await manager.PostFx.TakeSettingsFrom(PostFx);
				await manager.Graphics.TakeSettingsFrom(Graphics);
				await manager.Control.TakeSettingsFrom(Control);
			}
		}

		public readonly _E796 BackEndSession;

		public _E000 TempSettings;

		public _ECAA GesturesStorage;

		public ESettingsGroup CurrentGroup;

		public bool IgnoreScreenInterruptionCheck;

		private new _E7DE m__E000;

		public override EEftScreenType ScreenType => EEftScreenType.Settings;

		public _E7DE OriginalSettings => this.m__E000;

		protected override EStateSwitcher MenuChatBarVisibility => EStateSwitcher.LastState;

		protected override EStateSwitcher TaskBarButtonsVisibility => EStateSwitcher.LastState;

		public _E000(_E796 backEndSession)
		{
			BackEndSession = backEndSession;
			CurrentGroup = ESettingsGroup.Game;
		}

		public void InitSettings(_E7DE settingsManager)
		{
			this.m__E000 = settingsManager;
			_E001();
		}

		protected override async Task<bool> CloseScreenInterruption(bool moveForward)
		{
			if (IgnoreScreenInterruptionCheck)
			{
				IgnoreScreenInterruptionCheck = false;
				return true;
			}
			if (!TempSettings.HasSameSettings(this.m__E000) || GesturesStorage.IsChanged)
			{
				base._E002._E007(delegate
				{
					SaveSettings().HandleExceptions();
					CloseScreenForced();
				}, delegate
				{
					_E000();
					CloseScreenForced();
				});
				return false;
			}
			if (!(await base.CloseScreenInterruption(moveForward)))
			{
				return false;
			}
			_E000();
			return true;
		}

		public async Task SaveSettings()
		{
			await TempSettings.ApplyTo(this.m__E000);
			await _E77F.ReloadLocale(BackEndSession);
			if (GesturesStorage.IsChanged)
			{
				GesturesStorage.Save();
			}
		}

		public void TakeFromDefault()
		{
			SettingsScreen.m__E001[CurrentGroup].Tab.TakeSettingsFrom(this.m__E000);
		}

		private new void _E000()
		{
			if (TempSettings != null)
			{
				if (this.m__E000.Game.Settings.TryForceApplyDiff(TempSettings.Game))
				{
					_E7AD._E010.UpdateApplicationLanguage();
				}
				this.m__E000.Sound.Settings.TryForceApplyDiff(TempSettings.Sound);
				this.m__E000.PostFx.Settings.TryForceApplyDiff(TempSettings.PostFx);
				this.m__E000.Graphics.Settings.TryForceApplyDiff(TempSettings.Graphics);
				this.m__E000.Control.Settings.TryForceApplyDiff(TempSettings.Control);
				GesturesStorage.Rollback();
			}
		}

		private void _E001()
		{
			if (TempSettings == null)
			{
				TempSettings = SettingsScreen._E000._E000.CopyFromManager(this.m__E000);
			}
			if (GesturesStorage == null)
			{
				GesturesStorage = new _ECAA(autoSave: false, BackEndSession.Profile.Info.Side);
			}
		}

		[CompilerGenerated]
		private new void _E002()
		{
			SaveSettings().HandleExceptions();
			CloseScreenForced();
		}

		[CompilerGenerated]
		private void _E003()
		{
			_E000();
			CloseScreenForced();
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private Task<bool> _E004(bool moveForward)
		{
			return base.CloseScreenInterruption(moveForward);
		}
	}

	public sealed class _E001 : _E000
	{
		protected override EShadingStateSwitcher ShadingType => EShadingStateSwitcher.Default;

		protected override EStateSwitcher MenuChatBarVisibility => EStateSwitcher.Disabled;

		protected override EStateSwitcher TaskBarButtonsVisibility => EStateSwitcher.LastState;

		protected override EStateSwitcher ShowEnvironment => EStateSwitcher.Enabled;

		protected override EStateSwitcher EnvironmentOverlay => EStateSwitcher.Enabled;

		protected override EStateSwitcher ShowEnvironmentCamera => EStateSwitcher.Enabled;

		public _E001(_E796 backEndSession)
			: base(backEndSession)
		{
		}
	}

	private readonly struct _E002
	{
		public readonly SettingsTab Tab;

		public readonly UIAnimatedToggleSpawner Toggle;

		public _E002(SettingsTab tab, UIAnimatedToggleSpawner toggle)
		{
			Tab = tab;
			Toggle = toggle;
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public ESettingsGroup key;

		public _E002 value;

		public SettingsScreen _003C_003E4__this;

		internal void _E000(bool isOn)
		{
			if (isOn)
			{
				_003C_003E4__this._E004(key);
			}
		}

		internal void _E001()
		{
			value.Tab.OnLoadingInProgress -= _003C_003E4__this._E001;
		}
	}

	[SerializeField]
	private GameObject _loader;

	[SerializeField]
	private DefaultUIButton _saveButton;

	[SerializeField]
	private DefaultUIButton _backButton;

	[SerializeField]
	private DefaultUIButton _defaultButton;

	[Space]
	[SerializeField]
	private UIAnimatedToggleSpawner _gameButton;

	[SerializeField]
	private UIAnimatedToggleSpawner _graphicsButton;

	[SerializeField]
	private UIAnimatedToggleSpawner _postFXButton;

	[SerializeField]
	private UIAnimatedToggleSpawner _soundButton;

	[SerializeField]
	private UIAnimatedToggleSpawner _controlsButton;

	[Space]
	[SerializeField]
	private GameSettingsTab _gameSettingsScreen;

	[SerializeField]
	private GraphicsSettingsTab _graphicsSettingsScreen;

	[SerializeField]
	private PostFXSettingsTab _postFXSettingsScreen;

	[SerializeField]
	private SoundSettingsTab _soundSettingsScreen;

	[SerializeField]
	private ControlSettingsTab _controlsSettingsTabScreen;

	private new SettingsTab m__E000;

	private static readonly Dictionary<ESettingsGroup, _E002> m__E001 = new Dictionary<ESettingsGroup, _E002>(5, _E3A5<ESettingsGroup>.EqualityComparer);

	private void Awake()
	{
		_backButton.OnClick.AddListener(delegate
		{
			ScreenController.CloseScreen();
		});
		_saveButton.OnClick.AddListener(_E008);
		_defaultButton.OnClick.AddListener(_E006);
		SettingsScreen.m__E001.Clear();
		SettingsScreen.m__E001.Add(ESettingsGroup.Game, new _E002(_gameSettingsScreen, _gameButton));
		SettingsScreen.m__E001.Add(ESettingsGroup.Screen, new _E002(_graphicsSettingsScreen, _graphicsButton));
		SettingsScreen.m__E001.Add(ESettingsGroup.PostFX, new _E002(_postFXSettingsScreen, _postFXButton));
		SettingsScreen.m__E001.Add(ESettingsGroup.Sound, new _E002(_soundSettingsScreen, _soundButton));
		SettingsScreen.m__E001.Add(ESettingsGroup.Control, new _E002(_controlsSettingsTabScreen, _controlsButton));
		foreach (KeyValuePair<ESettingsGroup, _E002> item in SettingsScreen.m__E001)
		{
			var (key, value) = item;
			value.Toggle.SpawnedObject.onValueChanged.AddListener(delegate(bool isOn)
			{
				if (isOn)
				{
					_E004(key);
				}
			});
			value.Tab.OnLoadingInProgress += _E001;
			UI.AddDisposable(delegate
			{
				value.Tab.OnLoadingInProgress -= _E001;
			});
		}
	}

	public override void Show(_E000 controller)
	{
		ScreenController.InitSettings(Singleton<_E7DE>.Instance);
		Show();
		_E005(status: true);
		_E003(ScreenController.CurrentGroup);
	}

	private void Show()
	{
		ShowGameObject();
		_E796 backEndSession = ScreenController.BackEndSession;
		_E72F info = backEndSession.Profile.Info;
		_gameSettingsScreen.Show(ScreenController.TempSettings.Game, backEndSession);
		_graphicsSettingsScreen.Show(ScreenController.TempSettings.Graphics);
		_postFXSettingsScreen.Show(ScreenController.TempSettings.PostFx, ScreenController);
		_soundSettingsScreen.Show(ScreenController.TempSettings.Sound, info, backEndSession);
		_controlsSettingsTabScreen.Show(ScreenController.TempSettings.Control, ScreenController.TempSettings.Sound, ScreenController.GesturesStorage, info);
		ScreenController.OriginalSettings.Game.Settings.BlockGroupInvites.Subscribe(_E000);
	}

	private void _E000(bool toggleEnabled)
	{
		ScreenController.BackEndSession.SendClientProfileSettings(ScreenController.OriginalSettings.Game.Settings.BlockGroupInvites.Value, null);
	}

	private void _E001(bool inProgress)
	{
		_E005(!inProgress);
	}

	private void _E002()
	{
		_gameSettingsScreen.Close();
		_graphicsSettingsScreen.Close();
		_postFXSettingsScreen.Close();
		_soundSettingsScreen.Close();
		_controlsSettingsTabScreen.Close();
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (command.IsCommand(ECommand.Escape) && _controlsSettingsTabScreen.CanPressEscape)
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

	public override void Close()
	{
		_gameSettingsScreen.Close();
		_graphicsSettingsScreen.Close();
		_postFXSettingsScreen.Close();
		_soundSettingsScreen.Close();
		_controlsSettingsTabScreen.Close();
		base.Close();
	}

	private void _E003(ESettingsGroup desiredGroup)
	{
		(SettingsScreen.m__E001.TryGetValue(desiredGroup, out var value) ? value.Toggle : _gameButton)._E001 = true;
		_E004(desiredGroup);
	}

	private void _E004(ESettingsGroup group)
	{
		if (this.m__E000 != null)
		{
			this.m__E000.IsSelected = false;
		}
		ScreenController.CurrentGroup = group;
		this.m__E000 = SettingsScreen.m__E001[group].Tab;
		this.m__E000.IsSelected = true;
	}

	private void _E005(bool status)
	{
		_loader.SetActive(!status);
		_saveButton.Interactable = status;
	}

	private void _E006()
	{
		ItemUiContext.Instance.ShowMessageWindow(_ED3E._E000(234741).Localized(), delegate
		{
			_E002();
			ScreenController.TakeFromDefault();
			Show();
			_E003(ScreenController.CurrentGroup);
		}, delegate
		{
		}, null, 0f, forceShow: true);
	}

	private void _E007(Action accept, Action cancel)
	{
		ItemUiContext.Instance.ShowMessageWindow(_ED3E._E000(234774).Localized(), accept, cancel, null, 0f, forceShow: true);
	}

	private async void _E008()
	{
		_E005(status: false);
		await ScreenController.SaveSettings();
		ScreenController.CloseScreenForced();
	}

	[CompilerGenerated]
	private void _E009()
	{
		ScreenController.CloseScreen();
	}

	[CompilerGenerated]
	private void _E00A()
	{
		_E002();
		ScreenController.TakeFromDefault();
		Show();
		_E003(ScreenController.CurrentGroup);
	}
}
