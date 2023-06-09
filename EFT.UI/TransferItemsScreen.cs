using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EFT.InputSystem;
using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using EFT.UI.Screens;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace EFT.UI;

public sealed class TransferItemsScreen : EftScreen<TransferItemsScreen._E000, TransferItemsScreen>
{
	public new sealed class _E000 : _EC92._E000<_E000, TransferItemsScreen>
	{
		public readonly IEnumerable<_E464> Messages;

		public readonly _E9EF Grid;

		public readonly _E9C4 HealthController;

		public readonly _EAED InventoryController;

		public readonly _EAA0 PlayerStash;

		public readonly _E796 Session;

		public readonly DateTime? ExpirationTime;

		public override EEftScreenType ScreenType => EEftScreenType.TransferItems;

		protected override EStateSwitcher MenuChatBarVisibility => EStateSwitcher.Disabled;

		public _E000(IEnumerable<_E464> messages, _E9EF grid, _E9C4 healthController, _EAED inventoryController, _EAA0 playerStash, _E796 session, DateTime? expirationTime = null)
		{
			Messages = messages;
			Grid = grid;
			HealthController = healthController;
			InventoryController = inventoryController;
			PlayerStash = playerStash;
			Session = session;
			ExpirationTime = expirationTime;
		}

		protected override async Task<bool> CloseScreenInterruption(bool moveForward)
		{
			bool flag = _E002 != null;
			if (flag)
			{
				flag = !(await _E002._E001());
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

	private new const float m__E000 = 12f;

	private const string m__E001 = "UI/TransferScreen/PackageHasExpired";

	private const string m__E002 = "You still have untransfered items. Are you sure you want to end the transfer? You will be able to return to this process later.";

	[SerializeField]
	private SimpleStashPanel _stashPanel;

	[SerializeField]
	private GridView _itemsToTransferGridView;

	[SerializeField]
	private DefaultUIButton _closeButton;

	[SerializeField]
	private DefaultUIButton _acceptButton;

	[SerializeField]
	private DefaultUIButton _receiveAllButton;

	[SerializeField]
	private GameObject _expirationTimePanel;

	[SerializeField]
	private TextMeshProUGUI _expirationLabel;

	private _ECF5<bool> m__E003 = new _ECF5<bool>(initialValue: false);

	private ItemUiContext _E004;

	private IEnumerable<_E464> _E005;

	private _EAED _E006;

	private _EA40 _E007;

	private DateTime? _E008;

	private ItemView[] _E009;

	private _EC3D _E00A;

	private Action _E00B;

	private List<ItemView> _E00C => _itemsToTransferGridView._E002;

	private void Awake()
	{
		_acceptButton.OnClick.AddListener(delegate
		{
			ScreenController.CloseScreen();
		});
		_receiveAllButton.OnClick.AddListener(_E002);
	}

	private void Update()
	{
		if (!_E008.HasValue)
		{
			_expirationTimePanel.gameObject.SetActive(value: false);
			return;
		}
		TimeSpan timeSpan = _E008.Value - _E5AD.UtcNow;
		bool flag = timeSpan.TotalHours < 12.0;
		_expirationTimePanel.gameObject.SetActive(flag);
		if (flag)
		{
			if (timeSpan.TotalSeconds < 1.0)
			{
				ScreenController.CloseScreenForced();
				_E004.ShowMessageWindow(_ED3E._E000(255018).Localized(), null, null, _ED3E._E000(255046).Localized());
			}
			else
			{
				_expirationLabel.SetMonospaceText(timeSpan.RagfairDateFormatLong());
			}
		}
	}

	public override void Show(_E000 controller)
	{
		Show(controller.Messages, controller.Grid, controller.HealthController, controller.InventoryController, controller.PlayerStash, controller.Session, controller.ExpirationTime);
	}

	private void Show(IEnumerable<_E464> messages, [CanBeNull] _E9EF grid, _E9C4 healthController, _EAED controller, _EAA0 playerStash, _E796 session, DateTime? expirationTime)
	{
		Profile profile = session.Profile;
		_E005 = messages;
		_E006 = controller;
		_E007 = playerStash;
		_E008 = expirationTime;
		_E004 = ItemUiContext.Instance;
		Item[] array = _E005.SelectMany((_E464 message) => message?.Items?.Stash?.Grid.Items).ToArray();
		foreach (_EA91 item in array.OfType<_EA91>())
		{
			item.UncoverAll(profile.Id);
		}
		if (grid == null)
		{
			_E00A = new _EC3D(array, 10, 25, isFlexible: true);
			grid = _E00A.Grid;
		}
		_E004.Configure(controller, profile, session, session.InsuranceCompany, null, null, healthController, new _EA40[1] { _E007 }, EItemUiContextType.TransferItemsScreen, ECursorResult.ShowCursor);
		_E00B = this.m__E003.Bind(_E000);
		ShowGameObject();
		_closeButton.gameObject.SetActive(value: false);
		_stashPanel.Configure(playerStash, _E006, EItemViewType.TransferPlayer);
		_stashPanel.Show();
		_itemsToTransferGridView.Show(grid, new _EB64(grid.ParentItem, EItemViewType.TransferTrader), _E006, _E004);
		_E009 = new ItemView[_E00C.Count];
		_E00C.CopyTo(_E009);
	}

	private void _E000(bool isReceiving)
	{
		_acceptButton.Interactable = !isReceiving;
		_receiveAllButton.Interactable = !isReceiving;
	}

	private Task<bool> _E001()
	{
		if (this.m__E003.Value)
		{
			return Task.FromResult(result: false);
		}
		if (_E00C.Count == 0)
		{
			return Task.FromResult(result: true);
		}
		TaskCompletionSource<bool> taskSource = new TaskCompletionSource<bool>();
		_E004.ShowMessageWindow(_ED3E._E000(255095).Localized(), delegate
		{
			taskSource.SetResult(result: true);
		}, delegate
		{
			taskSource.SetResult(result: false);
		});
		return taskSource.Task;
	}

	private async void _E002()
	{
		if (!this.m__E003.Value)
		{
			this.m__E003.Value = true;
			_receiveAllButton.Interactable = false;
			await _itemsToTransferGridView.TransferAllItems();
			this.m__E003.Value = false;
			_receiveAllButton.Interactable = true;
		}
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (command == ECommand.Escape)
		{
			ScreenController.CloseScreen();
			return ETranslateResult.Block;
		}
		return InputNode.GetDefaultBlockResult(command);
	}

	public override void Close()
	{
		if (_E005 != null)
		{
			foreach (_E464 item in _E005)
			{
				item.UpdateRewardStatus?.Invoke();
			}
		}
		if (_E00A != null)
		{
			_E00A.Dispose();
			_E00A = null;
		}
		_stashPanel.Close();
		_itemsToTransferGridView.Hide();
		_E00B?.Invoke();
		base.Close();
	}

	[CompilerGenerated]
	private void _E003()
	{
		ScreenController.CloseScreen();
	}
}
