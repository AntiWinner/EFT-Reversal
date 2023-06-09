using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.InputSystem;
using EFT.UI.Insurance;
using EFT.UI.Screens;
using UnityEngine;

namespace EFT.UI.Matchmaker;

public sealed class MatchmakerInsuranceScreen : MatchmakerEftScreen<MatchmakerInsuranceScreen._E000, MatchmakerInsuranceScreen>
{
	private enum EInsuranceTab
	{
		Insured,
		ToInsure
	}

	public new sealed class _E000 : _EC8F<_E000, MatchmakerInsuranceScreen>
	{
		public readonly _E9C4 HealthController;

		public readonly _EAED InventoryController;

		public readonly _E796 Session;

		public override EEftScreenType ScreenType => EEftScreenType.Insurance;

		protected override EStateSwitcher ShowEnvironment => EStateSwitcher.Enabled;

		protected override EStateSwitcher ShowEnvironmentCamera => EStateSwitcher.Enabled;

		public override bool KeyScreen => true;

		public _E000(_E9C4 healthController, _EAED inventoryController, _E796 session, _EC99 matchmakerPlayersController)
			: base(matchmakerPlayersController)
		{
			HealthController = healthController;
			InventoryController = inventoryController;
			Session = session;
		}

		protected override void CloseAction(bool moveForward)
		{
			Session.FlushOperationQueue().HandleExceptions();
		}

		protected override async Task<bool> CloseScreenInterruption(bool moveForward)
		{
			bool flag = moveForward;
			if (flag)
			{
				flag = (await Session.FlushOperationQueue()).Failed;
			}
			if (flag)
			{
				return false;
			}
			return await base.CloseScreenInterruption(moveForward);
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private Task<bool> _E000(bool moveForward)
		{
			return base.CloseScreenInterruption(moveForward);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public MatchmakerInsuranceScreen _003C_003E4__this;

		public List<_ECB4> allItemsToInsure;

		public Callback _003C_003E9__1;

		internal void _E000()
		{
			_003C_003E4__this.m__E004.InsureItems(allItemsToInsure, delegate(IResult result)
			{
				if (result.Succeed)
				{
					_003C_003E4__this.m__E002.TryHide().HandleExceptions();
					_003C_003E4__this.m__E002.Show(_003C_003E4__this._insuredTab);
				}
			});
		}

		internal void _E001(IResult result)
		{
			if (result.Succeed)
			{
				_003C_003E4__this.m__E002.TryHide().HandleExceptions();
				_003C_003E4__this.m__E002.Show(_003C_003E4__this._insuredTab);
			}
		}
	}

	[SerializeField]
	private Tab _insuredTab;

	[SerializeField]
	private Tab _toInsureTab;

	[SerializeField]
	private ItemsToInsureScreen _itemsToInsureScreen;

	[SerializeField]
	private InsuredItemsScreen _insuredItemsScreen;

	[SerializeField]
	private DefaultUIButton _readyButton;

	[SerializeField]
	private DefaultUIButton _nextButton;

	[SerializeField]
	private DefaultUIButton _insureButton;

	[SerializeField]
	private DefaultUIButton _backButton;

	private new ItemUiContext m__E000;

	private Dictionary<EInsuranceTab, Tab> m__E001;

	private _EC67 m__E002;

	private _E796 m__E003;

	private _ECB1 m__E004;

	private void Awake()
	{
		this.m__E001 = new Dictionary<EInsuranceTab, Tab>
		{
			{
				EInsuranceTab.Insured,
				_insuredTab
			},
			{
				EInsuranceTab.ToInsure,
				_toInsureTab
			}
		};
		this.m__E002 = new _EC67(this.m__E001.Values.ToArray(), _toInsureTab);
		_toInsureTab.Init(new _EC65(_itemsToInsureScreen));
		_insuredTab.Init(new _EC65(_insuredItemsScreen));
		_insureButton.OnClick.AddListener(_E002);
		_insureButton.SetHeaderText(_ED3E._E000(233748), 36);
		_nextButton.OnClick.AddListener(delegate
		{
			((_EC90<_E000, MatchmakerInsuranceScreen>)ScreenController)._E000();
		});
		_backButton.OnClick.AddListener(delegate
		{
			ScreenController.CloseScreen();
		});
		_readyButton.OnClick.AddListener(delegate
		{
			ScreenController._E000();
		});
	}

	public override void Show(_E000 controller)
	{
		base.Show(controller);
		Show(controller.HealthController, controller.InventoryController, controller.Session);
	}

	private void Show(_E9C4 healthController, _EAED controller, _E796 session)
	{
		ShowGameObject();
		this.m__E003 = session;
		this.m__E004 = session.InsuranceCompany;
		Profile profile = session.Profile;
		this.m__E000 = ItemUiContext.Instance;
		this.m__E000.Configure(controller, profile, this.m__E003, this.m__E004, null, null, healthController, null, EItemUiContextType.InsuranceScreen, ECursorResult.ShowCursor);
		_itemsToInsureScreen.Show(profile, controller, controller.Inventory.Equipment, this.m__E004, this.m__E000);
		_insuredItemsScreen.Show(this.m__E004);
		_toInsureTab.Deselect().HandleExceptions();
		_insuredTab.Deselect().HandleExceptions();
		this.m__E002.Show(_toInsureTab);
		MatchmakerPlayersController.OnRaidReadyStatusChanged += _E000;
		_itemsToInsureScreen.OnInsuranceAvailableChanged += _E001;
		UI.AddDisposable(delegate
		{
			MatchmakerPlayersController.OnRaidReadyStatusChanged -= _E000;
			_itemsToInsureScreen.OnInsuranceAvailableChanged -= _E001;
		});
	}

	private void _E000(_EC9B player, bool _)
	{
		if (!(player.AccountId == MatchmakerPlayersController.CurrentProfileAid) && !MatchmakerPlayersController.LeaderRaidReadyStatus)
		{
			ScreenController.CloseScreen();
		}
	}

	private void _E001(bool available)
	{
		_insureButton.Interactable = available;
	}

	private void _E002()
	{
		List<_ECB4> allItemsToInsure = this.m__E004.AllItemsToInsure;
		if (allItemsToInsure.Count <= 0)
		{
			return;
		}
		if (this.m__E004.ItemsPrice(allItemsToInsure) > this.m__E004.SelectedInsurer.Settings.Insurance.MinPayment)
		{
			_E003(delegate
			{
				this.m__E004.InsureItems(allItemsToInsure, delegate(IResult result)
				{
					if (result.Succeed)
					{
						this.m__E002.TryHide().HandleExceptions();
						this.m__E002.Show(_insuredTab);
					}
				});
			}, this.m__E004.AllItemsToInsure.Count);
		}
		else
		{
			_E857.DisplayWarningNotification(_ED3E._E000(233747).Localized());
		}
	}

	private void _E003(Action acceptAction, int itemsToInsureCount)
	{
		ItemUiContext.Instance.ShowMessageWindow(string.Format(_ED3E._E000(233810).Localized(), itemsToInsureCount.ToString()), acceptAction, delegate
		{
		});
	}

	public override void Close()
	{
		_itemsToInsureScreen.Close();
		_insuredItemsScreen.Close();
		base.Close();
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

	[CompilerGenerated]
	private void _E004()
	{
		((_EC90<_E000, MatchmakerInsuranceScreen>)ScreenController)._E000();
	}

	[CompilerGenerated]
	private void _E005()
	{
		ScreenController.CloseScreen();
	}

	[CompilerGenerated]
	private void _E006()
	{
		ScreenController._E000();
	}

	[CompilerGenerated]
	private void _E007()
	{
		MatchmakerPlayersController.OnRaidReadyStatusChanged -= _E000;
		_itemsToInsureScreen.OnInsuranceAvailableChanged -= _E001;
	}
}
