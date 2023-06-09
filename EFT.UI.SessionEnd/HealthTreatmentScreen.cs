using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using EFT.InputSystem;
using EFT.UI.Screens;
using UnityEngine;

namespace EFT.UI.SessionEnd;

public sealed class HealthTreatmentScreen : EftScreen<HealthTreatmentScreen._E000, HealthTreatmentScreen>
{
	public new sealed class _E000 : _EC92._E000<_E000, HealthTreatmentScreen>
	{
		[CompilerGenerated]
		private new Action m__E000;

		[CompilerGenerated]
		private Action<bool> _E001;

		public readonly Profile Profile;

		public readonly _EAED InventoryController;

		public readonly _E981 HealthController;

		public readonly ItemUiContext Context;

		public readonly _E796 Session;

		public readonly _E8B2 Trader;

		public override EEftScreenType ScreenType => EEftScreenType.HealthTreatment;

		protected override bool MainEnvironment => false;

		public override bool KeyScreen => true;

		protected override EStateSwitcher TaskBarButtonsVisibility => EStateSwitcher.Disabled;

		protected override EStateSwitcher MenuChatBarVisibility => EStateSwitcher.Disabled;

		public event Action OnShowNextScreen
		{
			[CompilerGenerated]
			add
			{
				Action action = this.m__E000;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Combine(action2, value);
					action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action action = this.m__E000;
				Action action2;
				do
				{
					action2 = action;
					Action value2 = (Action)Delegate.Remove(action2, value);
					action = Interlocked.CompareExchange(ref this.m__E000, value2, action2);
				}
				while ((object)action != action2);
			}
		}

		public event Action<bool> OnCloseInProgress
		{
			[CompilerGenerated]
			add
			{
				Action<bool> action = _E001;
				Action<bool> action2;
				do
				{
					action2 = action;
					Action<bool> value2 = (Action<bool>)Delegate.Combine(action2, value);
					action = Interlocked.CompareExchange(ref _E001, value2, action2);
				}
				while ((object)action != action2);
			}
			[CompilerGenerated]
			remove
			{
				Action<bool> action = _E001;
				Action<bool> action2;
				do
				{
					action2 = action;
					Action<bool> value2 = (Action<bool>)Delegate.Remove(action2, value);
					action = Interlocked.CompareExchange(ref _E001, value2, action2);
				}
				while ((object)action != action2);
			}
		}

		public _E000(Profile profile, _EAED inventoryController, _E981 healthController, ItemUiContext context, _E796 session)
		{
			Profile = profile;
			InventoryController = inventoryController;
			HealthController = healthController;
			Context = context;
			Session = session;
			Trader = Session.Medic;
		}

		public void ShowNextScreen()
		{
			this.m__E000?.Invoke();
		}

		protected override async Task<bool> CloseScreenInterruption(bool moveForward)
		{
			_E001?.Invoke(obj: true);
			await Session.FlushOperationQueue();
			bool result = await base.CloseScreenInterruption(moveForward);
			_E001?.Invoke(obj: false);
			return result;
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private Task<bool> _E000(bool moveForward)
		{
			return base.CloseScreenInterruption(moveForward);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public _E000 controller;

		public HealthTreatmentScreen _003C_003E4__this;

		internal void _E000()
		{
			controller.OnCloseInProgress -= _003C_003E4__this._E000;
		}
	}

	[SerializeField]
	private HealthTreatmentServiceView _healthTreatmentView;

	[SerializeField]
	private DefaultUIButton _nextButton;

	[SerializeField]
	private DefaultUIButton _backButton;

	private void Awake()
	{
		_nextButton.OnClick.AddListener(delegate
		{
			ScreenController.ShowNextScreen();
		});
		_backButton.OnClick.AddListener(delegate
		{
			ScreenController.CloseScreen();
		});
	}

	public static bool IsAvailable(Profile profile, _E981 healthController, Profile._E001 trader)
	{
		bool flag = false;
		foreach (_EC83 healthObserver in HealthTreatmentServiceView.GetHealthObservers(profile, healthController, trader))
		{
			flag = flag || healthObserver.Active;
			healthObserver.Dispose();
		}
		return flag;
	}

	public override void Show(_E000 controller)
	{
		Show(controller.Profile, controller.InventoryController, controller.HealthController, controller.Context, controller.Session);
		controller.OnCloseInProgress += _E000;
		UI.AddDisposable(delegate
		{
			controller.OnCloseInProgress -= _E000;
		});
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		return ETranslateResult.Ignore;
	}

	private void Show(Profile profile, _EAED inventoryController, _E981 healthController, ItemUiContext uiContext, _E796 session)
	{
		UI.Dispose();
		ShowGameObject();
		UI.AddDisposable(_healthTreatmentView);
		_healthTreatmentView.Show(ScreenController.Trader, profile, inventoryController, healthController, null, uiContext, session);
		_E000(blocked: false);
	}

	private void _E000(bool blocked)
	{
		_nextButton.Interactable = !blocked;
		_backButton.Interactable = !blocked;
	}

	public override void Close()
	{
		_healthTreatmentView.Close();
		base.Close();
	}

	[CompilerGenerated]
	private void _E001()
	{
		ScreenController.ShowNextScreen();
	}

	[CompilerGenerated]
	private void _E002()
	{
		ScreenController.CloseScreen();
	}
}
