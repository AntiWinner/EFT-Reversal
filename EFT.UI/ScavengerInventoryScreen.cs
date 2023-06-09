using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.InputSystem;
using EFT.InventoryLogic;
using EFT.UI.Screens;
using UnityEngine;

namespace EFT.UI;

public sealed class ScavengerInventoryScreen : EftScreen<ScavengerInventoryScreen._E000, ScavengerInventoryScreen>
{
	public new sealed class _E000 : _EC92._E000<_E000, ScavengerInventoryScreen>
	{
		[CompilerGenerated]
		private new sealed class _E000
		{
			public TaskCompletionSource<bool> taskSource;

			internal void _E000()
			{
				taskSource.SetResult(result: true);
			}

			internal void _E001()
			{
				taskSource.SetResult(result: false);
			}
		}

		[CompilerGenerated]
		private new Action m__E000;

		public readonly _EAED InventoryController;

		public readonly _E9C4 HealthController;

		public readonly _EAA0 PlayerStash;

		public readonly _E796 Session;

		public readonly ItemUiContext UiContext;

		public override EEftScreenType ScreenType => EEftScreenType.ScavInventory;

		protected override bool MainEnvironment => false;

		public override bool KeyScreen => true;

		public bool HasItems => InventoryController.Inventory.AllRealPlayerItems.Any();

		protected override EStateSwitcher TaskBarButtonsVisibility => EStateSwitcher.Disabled;

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

		public _E000(_EAED inventoryController, _E9C4 healthController, _EAA0 playerStash, _E796 session)
		{
			InventoryController = inventoryController;
			HealthController = healthController;
			PlayerStash = playerStash;
			Session = session;
			UiContext = ItemUiContext.Instance;
		}

		public void ShowNextScreen()
		{
			this.m__E000?.Invoke();
		}

		protected override async Task<bool> CloseScreenInterruption(bool moveForward)
		{
			bool flag = _E002 != null && _E002._E005;
			if (flag)
			{
				flag = !(await _E002._sortingTable.TryClose());
			}
			if (flag)
			{
				return false;
			}
			if (!moveForward || !HasItems)
			{
				return await base.CloseScreenInterruption(moveForward);
			}
			TaskCompletionSource<bool> taskSource = new TaskCompletionSource<bool>();
			UiContext.ShowMessageWindow(_ED3E._E000(248378).Localized(), delegate
			{
				taskSource.SetResult(result: true);
			}, delegate
			{
				taskSource.SetResult(result: false);
			});
			flag = await taskSource.Task;
			if (flag)
			{
				flag = await base.CloseScreenInterruption(moveForward: true);
			}
			return flag;
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private Task<bool> _E000(bool moveForward)
		{
			return base.CloseScreenInterruption(moveForward);
		}
	}

	[SerializeField]
	private ItemsPanel _itemsPanel;

	[SerializeField]
	private SimpleStashPanel _simpleStashPanel;

	[SerializeField]
	private DefaultUIButton _backButton;

	[SerializeField]
	private DefaultUIButton _nextButton;

	[SerializeField]
	private CustomTextMeshProUGUI _scavCaption;

	[SerializeField]
	private SortingTableWindow _sortingTable;

	private new ItemUiContext m__E000;

	private _EAED m__E001;

	private _EAED m__E002;

	private _E796 m__E003;

	private _E784 _E004;

	private bool _E005
	{
		get
		{
			if (_sortingTable != null)
			{
				return _sortingTable.IsVisible;
			}
			return false;
		}
	}

	private void Awake()
	{
		_backButton.OnClick.AddListener(delegate
		{
			ScreenController.CloseScreen();
		});
		_nextButton.OnClick.AddListener(delegate
		{
			ScreenController.ShowNextScreen();
		});
	}

	public override void Show(_E000 controller)
	{
		Show(controller.InventoryController, controller.HealthController, controller.PlayerStash, controller.Session, controller.UiContext);
	}

	private void Show(_EAED inventoryController, _E9C4 healthController, _EAA0 playerStash, _E796 session, ItemUiContext uiContext)
	{
		UI.Dispose();
		this.m__E001 = inventoryController;
		this.m__E002 = playerStash.Parent.GetOwner() as _EAED;
		this.m__E003 = session;
		this.m__E000 = uiContext;
		_simpleStashPanel.OnSortingTableTabSelected += _E000;
		_simpleStashPanel.Configure(playerStash, inventoryController, EItemViewType.ScavInventory);
		uiContext.Configure(inventoryController, session.Profile, session, session.InsuranceCompany, null, null, healthController, new _EA40[1] { playerStash }, EItemUiContextType.ScavengerInventoryScreen, ECursorResult.ShowCursor);
		UI.AddDisposable(_itemsPanel);
		_backButton.gameObject.SetActive(!ScreenController.Root);
		ShowGameObject();
		_scavCaption.text = _ED3E._E000(248381).Localized();
		Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.BackpackOpen);
		_itemsPanel.Show(_simpleStashPanel, inventoryController, healthController, session.Profile.Skills, session.InsuranceCompany, ItemsPanel.EItemsTab.Gear, EItemViewType.ScavInventory).HandleExceptions();
	}

	private void _E000()
	{
		if (this.m__E002 == null || this.m__E001 == null)
		{
			return;
		}
		_EA98 sortingTable = this.m__E002.Inventory.SortingTable;
		if (sortingTable == null)
		{
			_simpleStashPanel.ChangeSortingTableTabState(isVisible: false);
			return;
		}
		_sortingTable.Show(new _EB63<_EA98>(sortingTable, EItemViewType.ScavInventory), this.m__E003, this.m__E001, this.m__E000, _sortingTable.Close, delegate
		{
			_simpleStashPanel.ChangeSortingTableTabState(isVisible: false);
		});
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (command.IsCommand(ECommand.Escape) && _E005)
		{
			_sortingTable.Close();
			return InputNode.GetDefaultBlockResult(command);
		}
		if (!ScreenController.Root && (command.IsCommand(ECommand.Escape) || command.IsCommand(ECommand.ToggleInventory)))
		{
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuEscape);
			ScreenController.CloseScreen();
		}
		return InputNode.GetDefaultBlockResult(command);
	}

	public override void Close()
	{
		_simpleStashPanel.OnSortingTableTabSelected -= _E000;
		base.Close();
	}

	[CompilerGenerated]
	private void _E001()
	{
		ScreenController.CloseScreen();
	}

	[CompilerGenerated]
	private void _E002()
	{
		ScreenController.ShowNextScreen();
	}

	[CompilerGenerated]
	private void _E003()
	{
		_simpleStashPanel.ChangeSortingTableTabState(isVisible: false);
	}
}
