using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.Counters;
using EFT.InputSystem;
using EFT.InventoryLogic;
using EFT.UI.Gestures;
using EFT.UI.Screens;
using EFT.UI.Settings;
using UnityEngine;

namespace EFT.UI;

public sealed class BattleUIScreen : EftScreen<BattleUIScreen._E000, BattleUIScreen>
{
	public new abstract class _E000 : _EC92._E000<_E000, BattleUIScreen>
	{
		public readonly GamePlayerOwner Owner;

		private new Func<int> m__E000;

		public override EEftScreenType ScreenType => EEftScreenType.BattleUI;

		protected override EStateSwitcher UnrestrictedFrameRate => EStateSwitcher.Enabled;

		protected override EStateSwitcher MenuChatBarVisibility => EStateSwitcher.Disabled;

		protected override EStateSwitcher TaskBarButtonsVisibility => EStateSwitcher.Disabled;

		protected override EStateSwitcher IgnorePlayerInput => EStateSwitcher.Disabled;

		protected override EStateSwitcher ShowEnvironment => EStateSwitcher.Disabled;

		protected override EStateSwitcher EnvironmentOverlay => EStateSwitcher.Disabled;

		protected override EStateSwitcher ShowEnvironmentCamera => EStateSwitcher.Disabled;

		protected override EStateSwitcher CameraBlur => EStateSwitcher.Disabled;

		public virtual bool AllowQuickPanel => true;

		public virtual bool AllowHealthPanel => true;

		public virtual bool AllowStancePanel => true;

		public bool AzimuthActive => m__E000 != null;

		public bool AmmoSelectorAvailable => !(_E002?.AmmoSelector.IsShown ?? true);

		public abstract void SubscribeOnMalfunctions(Action<Weapon> func);

		public abstract void UnsubscribeFromMalfunctions(Action<Weapon> func);

		protected _E000(GamePlayerOwner owner)
		{
			Owner = owner;
			m__E000 = null;
			base._E000();
		}

		public void ShowReloadPanel(Weapon weapon, List<Item> foundMags, _ECB1 insurance, Action<Item> selectHandler)
		{
			if (!base.Closed)
			{
				_E002._E002(weapon, foundMags, insurance, selectHandler);
			}
		}

		public void ShowAzimuth(Func<int> valueGetter)
		{
			m__E000 = valueGetter;
		}

		public void HideAzimuth()
		{
			m__E000 = null;
		}

		protected override bool ShowScreenAsPrevious()
		{
			if (m__E000 != null)
			{
				_E002.ShowAzimuth(m__E000);
			}
			return base.ShowScreenAsPrevious();
		}
	}

	public sealed class _E001 : _E000
	{
		private _E629 m__E001;

		private new _E554.Location _E002;

		private bool _E003 = true;

		protected override EShadingStateSwitcher ShadingType => EShadingStateSwitcher.Raid;

		public _E001(GamePlayerOwner owner, _E629 dateTime, _E554.Location location)
			: base(owner)
		{
			this.m__E001 = dateTime;
			_E002 = location;
		}

		protected override void ShowAction(BattleUIScreen screen)
		{
			Owner.Player.OnSenseChanged += _E001;
			if (_E003)
			{
				_E000();
				_E003 = false;
			}
		}

		private new void _E000()
		{
			Profile profile = Owner.Player.Profile;
			string text = profile.GetCorrectedNickname();
			string text2 = ((_E002 != null) ? (_E002._Id + _ED3E._E000(70087)) : _ED3E._E000(250474)).Localized();
			long raidNumber = profile.Stats.OverallCounters.GetLong(CounterTag.Sessions, CounterTag.Pmc) + 1;
			DateTime registrationDate = _E5AD.Now - _E5AD.LocalDateTimeFromUnixTime(profile.Info.RegistrationDate).TimeOfDay;
			if (Owner.Player.Side != EPlayerSide.Savage)
			{
				text = _ED3E._E000(250520).Localized() + _ED3E._E000(18502) + profile.Info.Level + _ED3E._E000(18502) + profile.Info.Side.ToString().ToUpper() + _ED3E._E000(18502) + _ED3E._E000(250516).Localized() + _ED3E._E000(18502) + text;
			}
			MonoBehaviourSingleton<PreloaderUI>.Instance.ShowRaidStartInfo(raidNumber, registrationDate, this.m__E001, Owner.Player.Side, text, _ED3E._E000(250509).Localized() + _ED3E._E000(10270) + text2);
		}

		private void _E001(bool value)
		{
			if (!base.Closed)
			{
				base._E002.ActionPanel.ShowPointer(value);
			}
		}

		protected override void CloseAction(bool moveForward)
		{
			Owner.Player.OnSenseChanged -= _E001;
		}

		public override void SubscribeOnMalfunctions(Action<Weapon> func)
		{
			Owner.Player._E0DE.ExamineMalfunctionEvent += func;
		}

		public override void UnsubscribeFromMalfunctions(Action<Weapon> func)
		{
			Owner.Player._E0DE.ExamineMalfunctionEvent -= func;
		}
	}

	public sealed class _E002 : _E000
	{
		private _EC94<EEftScreenType> _E004;

		private readonly HideoutPlayerOwner _E005;

		private bool _E006;

		public override bool AllowQuickPanel => _E006;

		public override bool AllowHealthPanel => false;

		public override bool AllowStancePanel => _E006;

		protected override EShadingStateSwitcher ShadingType => EShadingStateSwitcher.Hideout;

		public override bool KeyScreen => true;

		public override void SubscribeOnMalfunctions(Action<Weapon> func)
		{
			_E005.HideoutPlayer.ShootingRangeInventory.ExamineMalfunctionEvent += func;
		}

		public override void UnsubscribeFromMalfunctions(Action<Weapon> func)
		{
			_E005.HideoutPlayer.ShootingRangeInventory.ExamineMalfunctionEvent -= func;
		}

		public _E002(HideoutPlayerOwner owner)
			: base(owner)
		{
			_E005 = owner;
		}

		public override void ShowScreen(EScreenState screenState)
		{
			_E002();
			base.ShowScreen(screenState);
		}

		protected override bool ShowScreenAsPrevious()
		{
			_E002();
			_E004?.ShowScreenAsPrevious();
			return base.ShowScreenAsPrevious();
		}

		protected override void ShowAction(BattleUIScreen screen)
		{
			_E001();
		}

		protected override Task<bool> CloseCurrentScreen(bool forced)
		{
			if (_EC92.Instance.CheckCurrentScreen(EEftScreenType.Hideout))
			{
				_E004 = base._E009;
				return Task.FromResult(result: true);
			}
			return base.CloseCurrentScreen(forced);
		}

		protected override void CloseAction(bool moveForward)
		{
			_E003();
			if (moveForward)
			{
				_E004?.CloseSelf(forced: true);
			}
		}

		private void _E000(bool status)
		{
			_E006 = status;
			base._E002._E00F(_E006);
			_E001();
		}

		private void _E001()
		{
			base._E002.ActionPanel.ShowPointer(!_E005.InShootingRange);
		}

		private new void _E002()
		{
			_E006 = _E005.InShootingRange;
			_E005.OnShootingRangeStatusChange += _E000;
		}

		private void _E003()
		{
			_E005.OnShootingRangeStatusChange -= _E000;
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public BattleUIScreen _003C_003E4__this;

		public Weapon weapon;

		public Action<Item> selectHandler;

		internal void _E000()
		{
			if (!_003C_003E4__this.ScreenController.Closed)
			{
				Item selectedMagazine = _003C_003E4__this.AmmoSelector.GetSelectedMagazine();
				_003C_003E4__this.AmmoSelector.Hide();
				_003C_003E4__this._E010(visible: false);
				_003C_003E4__this._E011(weapon, show: false);
				selectHandler(selectedMagazine);
			}
		}
	}

	[SerializeField]
	public AmmoSelector AmmoSelector;

	[SerializeField]
	private CharacterHealthPanel _characterHealthPanel;

	[SerializeField]
	private BattleStancePanel _battleStancePanel;

	[SerializeField]
	private GesturesQuickPanel _gesturesPanel;

	[SerializeField]
	private BattleUIPmcCount _pmcCount;

	[SerializeField]
	private InventoryScreenQuickAccessPanel _quickAccessPanel;

	[SerializeField]
	private UsingPanel _usingPanel;

	[SerializeField]
	private ActionPanel _actionPanel;

	[SerializeField]
	private AmmoCountPanel _ammoCountPanel;

	[SerializeField]
	private AzimuthPanel _azimuthPanel;

	[SerializeField]
	private OpticCratePanel _opticCratePanel;

	private new bool m__E000;

	private Player m__E001;

	private _E000 m__E002;

	public ActionPanel ActionPanel => _actionPanel;

	private EVisibilityMode _E003 => Singleton<_E7DE>.Instance.Game.Settings.QuickSlotsVisibility;

	private EVisibilityMode _E004 => Singleton<_E7DE>.Instance.Game.Settings.StaminaVisibility;

	private EVisibilityMode _E005 => Singleton<_E7DE>.Instance.Game.Settings.HealthVisibility;

	public override void Show(_E000 controller)
	{
		this.m__E002 = controller;
		Show(controller.Owner);
	}

	private void Show(GamePlayerOwner owner)
	{
		UI.Dispose();
		this.m__E001 = owner.Player;
		ShowGameObject();
		_E000();
		UIEventSystem.Instance.Disable();
		_characterHealthPanel.Show(this.m__E001.HealthController);
		_battleStancePanel.Show(this.m__E001);
		_gesturesPanel.Show(this.m__E001);
		_pmcCount.ShowGameObject();
		ActionPanel.Show(owner);
		_usingPanel.Init(this.m__E001.HealthController, this.m__E001._E0DE);
		_E00F(visible: false);
	}

	private void _E000()
	{
		this.m__E002.SubscribeOnMalfunctions(_E006);
		this.m__E001.HandsChangingEvent += _E009;
		this.m__E001.HandsChangedEvent += _E008;
		this.m__E001.Physical.OverweightIncreased += _E00A;
		this.m__E001.Physical.Stamina.OnChanged += _E00C;
		this.m__E001.Physical.HandsStamina.OnChanged += _E00C;
		this.m__E001.Physical.OnSprintStateChangedEvent += _E00D;
		this.m__E001.MovementContext.OnSmoothedPoseLevelChange += delegate
		{
			_E00C();
		};
		this.m__E001.OnSpeedChangedEvent += delegate
		{
			_E00C();
		};
		this.m__E001.MovementContext.OnMaxSpeedChangedEvent += delegate
		{
			_E00C();
		};
		this.m__E001.HealthController.ApplyDamageEvent += delegate
		{
			_E00A();
		};
		this.m__E001.HealthController.EffectRemovedEvent += _E001;
		this.m__E001.OnSightChangedEvent += _E003;
		this.m__E001.OnStartQuickdrawPistol += _E004;
		this.m__E001.HealthController.EffectStartedEvent += _E005;
		if (this.m__E001.VoipController != null)
		{
			UI.BindState(this.m__E001.VoipController.HasInteraction, _E00E);
			UI.BindState(this.m__E001.VoipController.Status, delegate(EVoipControllerStatus status)
			{
				if (status == EVoipControllerStatus.Blocked)
				{
					_E857.DisplayWarningNotification(_ED3E._E000(250500).Localized());
				}
			});
			this.m__E001.VoipController.AbuseNotification += delegate
			{
				_E857.DisplayWarningNotification(_ED3E._E000(250543).Localized());
			};
			UI.AddDisposable(delegate
			{
				this.m__E001.VoipController.AbuseNotification -= delegate
				{
					_E857.DisplayWarningNotification(_ED3E._E000(250543).Localized());
				};
			});
		}
		UI.AddDisposable(delegate
		{
			this.m__E002.UnsubscribeFromMalfunctions(_E006);
			this.m__E001.HandsChangingEvent -= _E009;
			this.m__E001.HandsChangedEvent -= _E008;
			this.m__E001.Physical.OverweightIncreased -= _E00A;
			this.m__E001.Physical.Stamina.OnChanged -= _E00C;
			this.m__E001.Physical.HandsStamina.OnChanged -= _E00C;
			this.m__E001.Physical.OnSprintStateChangedEvent -= _E00D;
			this.m__E001.MovementContext.OnSmoothedPoseLevelChange -= delegate
			{
				_E00C();
			};
			this.m__E001.OnSpeedChangedEvent -= delegate
			{
				_E00C();
			};
			this.m__E001.MovementContext.OnMaxSpeedChangedEvent -= delegate
			{
				_E00C();
			};
			this.m__E001.HealthController.ApplyDamageEvent -= delegate
			{
				_E00A();
			};
			this.m__E001.HealthController.EffectRemovedEvent -= _E001;
			this.m__E001.HealthController.EffectStartedEvent -= _E005;
			this.m__E001.OnSightChangedEvent -= _E003;
			this.m__E001.OnStartQuickdrawPistol -= _E004;
		});
	}

	private void _E001(_E992 effect)
	{
		if (!_characterHealthPanel.AutoHide)
		{
			_E00B(initialVisibility: true);
		}
	}

	public void ShowAmmoCountZeroingPanel(string message, string details = null)
	{
		_E000 screenController = ScreenController;
		if (screenController != null && !screenController.AzimuthActive)
		{
			_ammoCountPanel.Show(message, details);
		}
	}

	public void ShowFireMode(Weapon.EFireMode fireMode)
	{
		_E000 screenController = ScreenController;
		if (screenController != null && !screenController.AzimuthActive)
		{
			_ammoCountPanel.ShowFireMode(fireMode);
		}
	}

	public void ShowAmmoDetails(int ammoCount, int maxAmmoCount, int mastering, string details, bool foldingMechanimWeapon)
	{
		string message = (foldingMechanimWeapon ? AmmoCountPanel.GetAmmoCountByLevelForFoldingMechanismWeapon(ammoCount, maxAmmoCount) : AmmoCountPanel.GetAmmoCountByLevel(ammoCount, maxAmmoCount, mastering));
		_E000 screenController = ScreenController;
		if (screenController != null && screenController.AzimuthActive)
		{
			HideAzimuth();
		}
		_ammoCountPanel.Show(message, details);
	}

	public void ShowAzimuth(Func<int> valueGetter)
	{
		ScreenController?.ShowAzimuth(valueGetter);
		_azimuthPanel.Show(valueGetter);
		_ammoCountPanel.Hide();
	}

	public void HideAzimuth()
	{
		ScreenController?.HideAzimuth();
		_azimuthPanel.Hide();
	}

	private void _E002(Weapon weapon, List<Item> foundMags, _ECB1 insurance, Action<Item> selectHandler)
	{
		_E010(visible: true);
		Vector2 screenPosition = _E011(weapon, show: true);
		AmmoSelector.ShowAcceptableMags(foundMags, this.m__E001._E0DE, screenPosition, insurance, delegate
		{
			if (!ScreenController.Closed)
			{
				Item selectedMagazine = AmmoSelector.GetSelectedMagazine();
				AmmoSelector.Hide();
				_E010(visible: false);
				_E011(weapon, show: false);
				selectHandler(selectedMagazine);
			}
		});
	}

	private void _E003(SightComponent sightComponent)
	{
		if (sightComponent != null)
		{
			float currentOpticZoom = sightComponent.GetCurrentOpticZoom();
			if (currentOpticZoom > 1f || sightComponent.HasCurrentZoomGreaterThenOne())
			{
				string message = string.Format(_ED3E._E000(250477), currentOpticZoom);
				_opticCratePanel.Show(message);
			}
		}
	}

	private void _E004()
	{
		_quickAccessPanel.ShowQuickdrawGlowForPistols(show: false, 0f);
	}

	private void _E005(_E992 effect)
	{
		if (effect is _E993)
		{
			_E00A();
		}
	}

	private void _E006(Weapon weapon)
	{
		_E007(150).HandleExceptions();
	}

	private async Task _E007(int delayInMilliseconds)
	{
		await Task.Delay(delayInMilliseconds);
		float duration = Singleton<_E5CB>.Instance.Malfunction.TimeToQuickdrawPistol - (float)delayInMilliseconds / 1000f;
		_quickAccessPanel.ShowQuickdrawGlowForPistols(show: true, duration);
		_E009();
	}

	private void _E008(_E6C7 handsController)
	{
		_quickAccessPanel.RefreshBoundSlotSelectView(this.m__E001.HandsController.Item);
	}

	private void _E009()
	{
		if (base.gameObject.activeSelf && this._E003 != EVisibilityMode.Never && ScreenController.AllowQuickPanel && !(this.m__E001.HandsController == null))
		{
			_quickAccessPanel.AnimatedShow(this._E003 == EVisibilityMode.Autohide);
			_E008(this.m__E001.HandsController);
		}
	}

	private void _E00A()
	{
		_E00B(initialVisibility: true);
	}

	private void _E00B(bool initialVisibility)
	{
		if (base.gameObject.activeSelf)
		{
			if (this._E005 == EVisibilityMode.Never || !ScreenController.AllowHealthPanel)
			{
				_characterHealthPanel.AnimatedHide();
			}
			else if (this._E005 == EVisibilityMode.Always || _characterHealthPanel.AnyCriticalEffects)
			{
				_characterHealthPanel.AnimatedShow(autohide: false);
			}
			else if (initialVisibility)
			{
				_characterHealthPanel.AnimatedShow(autohide: true);
			}
			else
			{
				_characterHealthPanel.AnimatedHide();
			}
		}
	}

	private void _E00C()
	{
		if (!this.m__E000 && base.gameObject.activeSelf && this._E004 != EVisibilityMode.Never && ScreenController.AllowStancePanel)
		{
			_battleStancePanel.AnimatedShow(this._E004 == EVisibilityMode.Autohide);
		}
	}

	private void _E00D(bool sprinting)
	{
		this.m__E000 = sprinting;
		_E00E(sprinting);
	}

	private void _E00E(bool noAutoHide)
	{
		if (base.gameObject.activeSelf && this._E004 != EVisibilityMode.Never && ScreenController.AllowStancePanel)
		{
			if (noAutoHide)
			{
				_battleStancePanel.AnimatedShow(autohide: false);
			}
			else if (this._E004 == EVisibilityMode.Autohide)
			{
				_battleStancePanel.AnimatedHide(3f);
			}
		}
	}

	private void _E00F(bool visible)
	{
		if (!visible)
		{
			_opticCratePanel.Hide();
			_ammoCountPanel.Hide();
		}
		bool num = (visible || this._E003 == EVisibilityMode.Always) && this._E003 != EVisibilityMode.Never && ScreenController.AllowQuickPanel;
		bool num2 = (visible || this._E004 == EVisibilityMode.Always) && this._E004 != EVisibilityMode.Never && ScreenController.AllowStancePanel;
		if (ScreenController.AllowQuickPanel)
		{
			_quickAccessPanel.Show(this.m__E001._E0DE, ItemUiContext.Instance);
			_quickAccessPanel.RefreshSelection(this.m__E001.HandsController.Item);
		}
		else
		{
			_quickAccessPanel.Hide();
		}
		if (num2)
		{
			_E00C();
		}
		else
		{
			_battleStancePanel.AnimatedHide();
		}
		_E00B(visible);
		if (num)
		{
			_E009();
		}
		else
		{
			_quickAccessPanel.AnimatedHide();
		}
	}

	private void _E010(bool visible)
	{
		visible = (visible || this._E003 == EVisibilityMode.Always) && ScreenController.AllowQuickPanel;
		if (visible)
		{
			_quickAccessPanel.AnimatedShow(autohide: false);
		}
		else
		{
			_quickAccessPanel.AnimatedHide();
		}
	}

	private Vector2 _E011(Weapon weapon, bool show)
	{
		return _quickAccessPanel.ShowArrowForWeaponSlot(weapon, show);
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		return ETranslateResult.Ignore;
	}

	protected override ECursorResult ShouldLockCursor()
	{
		return ECursorResult.LockCursor;
	}

	public override void Close()
	{
		this.m__E000 = false;
		ActionPanel.ShowPointer(b: false);
		_battleStancePanel.Close();
		_characterHealthPanel.Close();
		_quickAccessPanel.Hide();
		ActionPanel.Hide();
		_gesturesPanel.Close();
		_pmcCount.Close();
		_opticCratePanel.Close();
		_azimuthPanel.Close();
		_ammoCountPanel.Close();
		AmmoSelector.Close();
		UIEventSystem.Instance.Enable();
		base.Close();
	}

	[CompilerGenerated]
	private void _E012()
	{
		this.m__E001.VoipController.AbuseNotification -= delegate
		{
			_E857.DisplayWarningNotification(_ED3E._E000(250543).Localized());
		};
	}

	[CompilerGenerated]
	private void _E013()
	{
		this.m__E002.UnsubscribeFromMalfunctions(_E006);
		this.m__E001.HandsChangingEvent -= _E009;
		this.m__E001.HandsChangedEvent -= _E008;
		this.m__E001.Physical.OverweightIncreased -= _E00A;
		this.m__E001.Physical.Stamina.OnChanged -= _E00C;
		this.m__E001.Physical.HandsStamina.OnChanged -= _E00C;
		this.m__E001.Physical.OnSprintStateChangedEvent -= _E00D;
		this.m__E001.MovementContext.OnSmoothedPoseLevelChange -= delegate
		{
			_E00C();
		};
		this.m__E001.OnSpeedChangedEvent -= delegate
		{
			_E00C();
		};
		this.m__E001.MovementContext.OnMaxSpeedChangedEvent -= delegate
		{
			_E00C();
		};
		this.m__E001.HealthController.ApplyDamageEvent -= delegate
		{
			_E00A();
		};
		this.m__E001.HealthController.EffectRemovedEvent -= _E001;
		this.m__E001.HealthController.EffectStartedEvent -= _E005;
		this.m__E001.OnSightChangedEvent -= _E003;
		this.m__E001.OnStartQuickdrawPistol -= _E004;
	}

	[CompilerGenerated]
	private void _E014(float _, int __)
	{
		_E00C();
	}

	[CompilerGenerated]
	private void _E015(float _, float __, int ___)
	{
		_E00C();
	}

	[CompilerGenerated]
	private void _E016(float _, float __, float ___, int ____)
	{
		_E00C();
	}

	[CompilerGenerated]
	private void _E017(EBodyPart _, float __, _EC23 ___)
	{
		_E00A();
	}
}
