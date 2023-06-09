using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.InputSystem;
using EFT.UI.Screens;
using UnityEngine;

namespace EFT.UI;

public sealed class TraderScreensGroup : EftScreen<TraderScreensGroup._E000, TraderScreensGroup>, _E640, _E63F, _E641, _E647
{
	public new sealed class _E000 : _EC92._E000<_E000, TraderScreensGroup>
	{
		public readonly _E8B2 Trader;

		public readonly Profile Profile;

		public readonly _EAED StashController;

		public readonly _E9C4 HealthController;

		public readonly _E935 QuestController;

		public readonly _E796 Session;

		public override EEftScreenType ScreenType => EEftScreenType.Trader;

		public _E000(_E8B2 trader, Profile profile, _EAED stashController, _E9C4 healthController, _E935 questController, _E796 session)
		{
			Trader = trader;
			Profile = profile;
			StashController = stashController;
			HealthController = healthController;
			QuestController = questController;
			Session = session;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public TraderScreensGroup _003C_003E4__this;

		public _E794 captchaHandler;

		public _E8B2 trader;

		public Profile profile;

		public _EAED stashController;

		public _E935 questController;

		public _E9C4 healthController;

		internal void _E000()
		{
			_003C_003E4__this._E005.OnSessionError -= _003C_003E4__this._E000;
			captchaHandler.Deactivate();
		}

		internal void _E001(TraderDealScreen x)
		{
			x.Show(trader, profile, stashController, TraderDealScreen.ETraderMode.Purchase, _003C_003E4__this.m__E000, questController);
		}

		internal void _E002(TraderDealScreen x)
		{
			x.Show(trader, profile, stashController, TraderDealScreen.ETraderMode.Sale, _003C_003E4__this.m__E000, questController);
		}

		internal void _E003(QuestsScreen x)
		{
			x.Show(_003C_003E4__this._E005, stashController, _003C_003E4__this._E004, trader);
		}

		internal void _E004(ServicesScreen x)
		{
			x.Show(trader, profile, stashController, healthController, questController.Quests, _003C_003E4__this.m__E000, _003C_003E4__this._E005);
		}
	}

	[SerializeField]
	private DefaultUIButton _closeButton;

	[SerializeField]
	private Tab _buyTab;

	[SerializeField]
	private Tab _sellTab;

	[SerializeField]
	private Tab _tasksTab;

	[SerializeField]
	private Tab _servicesTab;

	[SerializeField]
	private TraderDealScreen _traderDealScreen;

	[SerializeField]
	private QuestsScreen _questsScreen;

	[SerializeField]
	private ServicesScreen _servicesScreen;

	[SerializeField]
	private _EC67 _tabs;

	[SerializeField]
	private TraderInfoPanel _traderPanel;

	[SerializeField]
	private TradingPlayerPanel _playerPanel;

	[SerializeField]
	private GameObject _alpha;

	private new ItemUiContext m__E000;

	private _EAED m__E001;

	private _E8B2 m__E002;

	private Profile m__E003;

	private _E935 _E004;

	private _E796 _E005;

	private _EAED _E006;

	private void Awake()
	{
		_closeButton.OnClick.AddListener(delegate
		{
			ScreenController.CloseScreen();
		});
		_tabs = new _EC67(new Tab[4] { _buyTab, _sellTab, _tasksTab, _servicesTab }, _buyTab);
		_alpha.SetActive(value: false);
	}

	public override void Show(_E000 controller)
	{
		Show(controller.Trader, controller.Profile, controller.StashController, controller.HealthController, controller.QuestController, controller.Session);
	}

	private void Show(_E8B2 trader, Profile profile, _EAED stashController, _E9C4 healthController, _E935 questController, _E796 session)
	{
		ShowGameObject();
		this.m__E002 = trader;
		this.m__E001 = stashController;
		this.m__E003 = profile;
		_E005 = session;
		_E005.OnSessionError += _E000;
		if (!this.m__E002.AssortmentLoading && !this.m__E002.AssortmentUpdateTimeout)
		{
			trader.RefreshAssortment(createIfNotExists: true, ignoreTimeout: false).HandleExceptions();
		}
		_E004 = questController;
		_playerPanel.Set(this.m__E003, this.m__E002.Info);
		_traderPanel.Show(this.m__E002, _E004);
		this.m__E000 = ItemUiContext.Instance;
		this.m__E000.Configure(stashController, profile, _E005, _E005.InsuranceCompany, _E005.RagFair, trader, null, new _EA40[1] { this.m__E001.Inventory.Stash }, EItemUiContextType.TraderScreen, ECursorResult.ShowCursor);
		_E794 captchaHandler = this.m__E000.CaptchaHandler;
		captchaHandler.Activate();
		UI.AddDisposable(delegate
		{
			_E005.OnSessionError -= _E000;
			captchaHandler.Deactivate();
		});
		_buyTab.Init(new _EC66<TraderDealScreen>(_traderDealScreen, delegate(TraderDealScreen x)
		{
			x.Show(trader, profile, stashController, TraderDealScreen.ETraderMode.Purchase, this.m__E000, questController);
		}));
		_sellTab.Init(new _EC66<TraderDealScreen>(_traderDealScreen, delegate(TraderDealScreen x)
		{
			x.Show(trader, profile, stashController, TraderDealScreen.ETraderMode.Sale, this.m__E000, questController);
		}));
		_tasksTab.Init(new _EC66<QuestsScreen>(_questsScreen, delegate(QuestsScreen x)
		{
			x.Show(_E005, stashController, _E004, trader);
		}));
		_servicesTab.Init(new _EC66<ServicesScreen>(_servicesScreen, delegate(ServicesScreen x)
		{
			x.Show(trader, profile, stashController, healthController, questController.Quests, this.m__E000, _E005);
		}));
		CanvasGroup component = _servicesTab.GetComponent<CanvasGroup>();
		if (component != null)
		{
			bool flag = _servicesScreen.CheckAvailableServices(trader);
			component.alpha = (flag ? 1f : 0.3f);
			Tab servicesTab = _servicesTab;
			bool interactable = (component.interactable = flag);
			servicesTab.Interactable = interactable;
		}
		stashController.RegisterView(this);
		this.m__E002.Info.OnStandingChanged += _E002;
		this.m__E002.Info.OnSalesSumChanged += _E002;
		this.m__E002.Info.OnLoyaltyChanged += _E001;
		_E006 = stashController;
		_tabs.Show(_buyTab);
	}

	private void _E000(EBackendErrorCode errorCode, string errorMessage)
	{
		if (errorCode == EBackendErrorCode.TraderDisabled)
		{
			this.m__E002.Info.SetDisabled(disabled: true);
			_EC92.Instance.TryReturnToRootScreen();
		}
	}

	public void OnRefreshItem(_EAFF eventArgs)
	{
		if (eventArgs.Item is _EA76)
		{
			_playerPanel.Set(this.m__E003, this.m__E002.Info);
		}
	}

	public void OnItemAdded(_EAF2 eventArgs)
	{
		if (eventArgs.Item is _EA76)
		{
			_playerPanel.Set(this.m__E003, this.m__E002.Info);
		}
	}

	public void OnItemRemoved(_EAF3 eventArgs)
	{
		if (eventArgs.Item is _EA76)
		{
			_playerPanel.Set(this.m__E003, this.m__E002.Info);
		}
	}

	private void _E001()
	{
		this.m__E002.RefreshAssortment(createIfNotExists: false, ignoreTimeout: true).HandleExceptions();
		_playerPanel.Set(this.m__E003, this.m__E002.Info);
	}

	private void _E002()
	{
		_playerPanel.UpdateStats(this.m__E002.Info);
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (command.IsCommand(ECommand.Escape))
		{
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuEscape);
			ScreenController.CloseScreen();
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
		_tabs.TryHide();
		_traderPanel.Close();
		_E006.UnregisterView(this);
		_traderDealScreen.FullClose();
		this.m__E002.Info.OnStandingChanged -= _E002;
		this.m__E002.Info.OnSalesSumChanged -= _E002;
		this.m__E002.Info.OnLoyaltyChanged -= _E001;
		base.Close();
	}

	protected override void OnDestroy()
	{
		_EC92.Instance.ReleaseScreen(EEftScreenType.Trader, this);
		base.OnDestroy();
	}

	[CompilerGenerated]
	private void _E003()
	{
		ScreenController.CloseScreen();
	}
}
