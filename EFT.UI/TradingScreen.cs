using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.InputSystem;
using EFT.UI.Ragfair;
using EFT.UI.Screens;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT.UI;

public sealed class TradingScreen : EftScreen<TradingScreen._E000, TradingScreen>
{
	public enum ETradingScreenTab
	{
		Merchants = 1,
		FleaMarket
	}

	public new abstract class _E000 : _EC92._E000<_E000, TradingScreen>
	{
		public readonly _E796 Session;

		public readonly _E9C4 HealthController;

		public readonly _EAED InventoryController;

		public readonly _E935 QuestController;

		public readonly _ECC4 RagfairSearch;

		public abstract ETradingScreenTab Tab { get; }

		protected override EStateSwitcher MenuChatBarVisibility => EStateSwitcher.Enabled;

		protected override EStateSwitcher TaskBarButtonsVisibility => EStateSwitcher.Enabled;

		protected _E000(_E796 session, _E9C4 healthController, _EAED inventoryController, _E935 questController, _ECC4 ragfairSearch)
		{
			Session = session;
			HealthController = healthController;
			InventoryController = inventoryController;
			QuestController = questController;
			RagfairSearch = ragfairSearch;
		}
	}

	public sealed class _E001 : _E000
	{
		public override EEftScreenType ScreenType => EEftScreenType.Traders;

		public override ETradingScreenTab Tab => ETradingScreenTab.Merchants;

		public _E001(_E796 session, _E9C4 healthController, _EAED inventoryController, _E935 questController)
			: base(session, healthController, inventoryController, questController, null)
		{
		}
	}

	public sealed class _E002 : _E000
	{
		public override EEftScreenType ScreenType => EEftScreenType.FleaMarket;

		public override ETradingScreenTab Tab => ETradingScreenTab.FleaMarket;

		public _E002(_E796 session, _E9C4 healthController, _EAED inventoryController, _E935 questController, _ECC4 ragfairSearch)
			: base(session, healthController, inventoryController, questController, ragfairSearch)
		{
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public string traderId;

		internal bool _E000(TraderPanel e)
		{
			return e.TraderId.Equals(traderId);
		}
	}

	[SerializeField]
	private DefaultUIButton _exitButton;

	[SerializeField]
	private UIAnimatedToggleSpawner _merchantsToggle;

	[SerializeField]
	private UIAnimatedToggleSpawner _ragfairToggle;

	[SerializeField]
	private GameObject _ragfairLockIcon;

	[SerializeField]
	private UIAnimatedToggleSpawner _auctionToggle;

	[SerializeField]
	private MerchantsList _merchantsList;

	[SerializeField]
	private RagfairScreen _ragfairScreen;

	[SerializeField]
	private HoverTooltipArea _tooltipArea;

	[SerializeField]
	private GameObject _loader;

	[SerializeField]
	public TraderScreensGroup TraderScreensGroup;

	private new _E796 m__E000;

	private _E9C4 m__E001;

	private _EAED m__E002;

	private _E935 m__E003;

	private Profile m__E004;

	private _ECBD m__E005;

	private _ECC4 m__E006;

	private _EC6B m__E007;

	private _EC6B m__E008;

	private DateTime m__E009;

	public override void Show(_E000 controller)
	{
		Show(controller.Session, controller.HealthController, controller.InventoryController, controller.QuestController, controller.Session.Profile, controller.RagfairSearch, controller.Tab);
	}

	private void Awake()
	{
		_EC92.Instance.RegisterScreen(EEftScreenType.Trader, TraderScreensGroup);
		_children.Add(_merchantsList);
		_children.Add(_ragfairScreen);
		_exitButton.OnClick.AddListener(delegate
		{
			ScreenController.CloseScreen();
		});
		if (this.m__E007 == null)
		{
			this.m__E007 = new _EC6B(_merchantsList, _merchantsToggle.SpawnedObject, _E001);
		}
		if (this.m__E008 == null)
		{
			this.m__E008 = new _EC6B(_ragfairScreen, _ragfairToggle.SpawnedObject, _E000);
		}
		_merchantsToggle.SpawnedObject.onValueChanged.AddListener(delegate(bool arg)
		{
			if (arg)
			{
				_E007(ETradingScreenTab.Merchants);
			}
		});
		_ragfairToggle.SpawnedObject.onValueChanged.AddListener(delegate(bool arg)
		{
			if (arg)
			{
				_E007(ETradingScreenTab.FleaMarket);
			}
		});
		_auctionToggle.SpawnedObject.onValueChanged.AddListener(delegate
		{
		});
		_auctionToggle.SetActive(active: false);
	}

	private void _E000()
	{
		_EA40[] lootItems = new _EA40[1] { this.m__E002.Inventory.Stash };
		_ragfairScreen.Show(this.m__E004, this.m__E002, lootItems, this.m__E001, this.m__E000, this.m__E006);
	}

	private void _E001()
	{
		_merchantsList.Show(this.m__E000.Traders.Where((_E8B2 trader) => !trader.Settings.AvailableInRaid), this.m__E003, this.m__E002, this.m__E001, this.m__E000, this.m__E004);
	}

	private void Show(_E796 session, _E9C4 healthController, _EAED inventoryController, _E935 questController, Profile profile, _ECC4 ragfairSearch, ETradingScreenTab tab)
	{
		this.m__E000 = session;
		this.m__E001 = healthController;
		this.m__E002 = inventoryController;
		this.m__E003 = questController;
		this.m__E004 = profile;
		this.m__E006 = ragfairSearch;
		bool flag = tab == ETradingScreenTab.FleaMarket;
		if (flag && this.m__E008 == null)
		{
			this.m__E008 = new _EC6B(_ragfairScreen, _ragfairToggle.SpawnedObject, _E000);
		}
		_ragfairToggle.ToggleSilently(flag);
		_merchantsToggle.ToggleSilently(!flag);
		this.m__E005 = this.m__E000.RagFair;
		ShowGameObject();
		_loader.SetActive(value: false);
		_E006(tab).Show();
		this.m__E005.CancellableFilters.ItemAdded += _E004;
		this.m__E005.CancellableFilters.ItemRemoved += _E005;
		UI.AddDisposable(this.m__E005.OnStatusChanged.Bind(delegate
		{
			_E002();
		}, this.m__E005.Status));
	}

	private void _E002()
	{
		bool flag = !this.m__E005.Disabled;
		if (!flag)
		{
			_tooltipArea.SetMessageText(this.m__E005.GetFormattedStatusDescription);
			_ragfairToggle.ToggleSilently(show: false);
		}
		_ragfairToggle.SetActive(flag);
		_ragfairLockIcon.SetActive(value: false);
	}

	[CanBeNull]
	internal TraderPanel _E003(string traderId)
	{
		return GetComponentsInChildren<TraderPanel>().FirstOrDefault((TraderPanel e) => e.TraderId.Equals(traderId));
	}

	private void _E004(_ECC0 filter)
	{
		this.m__E006 = null;
	}

	private void _E005(_ECC0 filter)
	{
		if (this.m__E006 != null && this.m__E006.Type == filter.Type)
		{
			this.m__E006 = null;
		}
	}

	private _EC6B _E006(ETradingScreenTab tabType)
	{
		return tabType switch
		{
			ETradingScreenTab.FleaMarket => this.m__E008, 
			ETradingScreenTab.Merchants => this.m__E007, 
			_ => null, 
		};
	}

	private void _E007(ETradingScreenTab tabType)
	{
		switch (tabType)
		{
		case ETradingScreenTab.Merchants:
			new _E001(ScreenController.Session, ScreenController.HealthController, ScreenController.InventoryController, ScreenController.QuestController).ShowScreen(EScreenState.Queued);
			break;
		case ETradingScreenTab.FleaMarket:
			new _E002(ScreenController.Session, ScreenController.HealthController, ScreenController.InventoryController, ScreenController.QuestController, ScreenController.RagfairSearch).ShowScreen(EScreenState.Queued);
			break;
		}
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (command.IsCommand(ECommand.Escape))
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
		if (_merchantsList.gameObject.activeSelf)
		{
			_merchantsList.Close();
		}
		if (_ragfairScreen.gameObject.activeSelf)
		{
			_ragfairScreen.Close();
		}
		if (this.m__E005 != null)
		{
			this.m__E005.CancellableFilters.ItemAdded -= _E004;
			this.m__E005.CancellableFilters.ItemRemoved -= _E005;
		}
		base.Close();
		_merchantsToggle.ToggleSilently(show: false);
		_ragfairToggle.ToggleSilently(show: false);
	}

	[CompilerGenerated]
	private void _E008()
	{
		ScreenController.CloseScreen();
	}

	[CompilerGenerated]
	private void _E009(bool arg)
	{
		if (arg)
		{
			_E007(ETradingScreenTab.Merchants);
		}
	}

	[CompilerGenerated]
	private void _E00A(bool arg)
	{
		if (arg)
		{
			_E007(ETradingScreenTab.FleaMarket);
		}
	}

	[CompilerGenerated]
	private void _E00B(_ECBD.ERagFairStatus status)
	{
		_E002();
	}
}
