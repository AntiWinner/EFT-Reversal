using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using EFT.InventoryLogic;
using EFT.UI.Health;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT.UI;

public sealed class ItemsPanel : UIElement
{
	public enum EItemsTab
	{
		Gear,
		Health
	}

	public sealed class _E000 : _EC64<ItemsPanel>
	{
		private new readonly _EC60 m__E000;

		private readonly _EAED _E001;

		private readonly _E9C4 _E002;

		private readonly _E74F _E003;

		private readonly _ECB1 _E004;

		private readonly EItemsTab _E005;

		[CanBeNull]
		private readonly SortingTableWindow _E006;

		private readonly EItemViewType _E007;

		public _E000(ItemsPanel itemsTab, _EC60 rightPanel, _EAED inventoryController, _E9C4 health, _E74F skills, _ECB1 insurance, [CanBeNull] SortingTableWindow sortingTable, EItemsTab currentTab, EItemViewType viewType)
			: base(itemsTab)
		{
			this.m__E000 = rightPanel;
			_E001 = inventoryController;
			_E002 = health;
			_E003 = skills;
			_E004 = insurance;
			_E005 = currentTab;
			_E006 = sortingTable;
			_E007 = viewType;
		}

		public override void Show()
		{
			base._E000.Show(this.m__E000, _E001, _E002, _E003, _E004, _E005, _E007).HandleExceptions();
		}

		public override async Task<bool> TryHide()
		{
			bool flag = _E006?.IsVisible ?? false;
			if (flag)
			{
				flag = !(await _E006.TryClose());
			}
			if (flag)
			{
				return false;
			}
			return await base.TryHide();
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private new Task<bool> _E000()
		{
			return base.TryHide();
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public bool wasClosed;

		public ItemsPanel _003C_003E4__this;

		public _EB63<_EB0B> equipmentContext;

		internal void _E000()
		{
			wasClosed = true;
		}

		internal void _E001()
		{
			_003C_003E4__this._E18D?.Invoke();
		}

		internal void _E002()
		{
			_003C_003E4__this._E089.UnregisterView(equipmentContext);
		}
	}

	[SerializeField]
	private HealthParametersPanel _healthParametersPanel;

	[SerializeField]
	private EquipmentTab _equipmentPanel;

	[SerializeField]
	private InventoryScreenHealthPanel _healthPanel;

	[SerializeField]
	private ContainersPanel _containers;

	[SerializeField]
	private InventoryScreenQuickAccessPanel _quickUse;

	[SerializeField]
	private DefaultUIButton _backButton;

	[CompilerGenerated]
	private Action _E18D;

	public bool SplitInFrames = true;

	private EItemsTab _E18E;

	private _EA40[] _E07B;

	private _EAED _E092;

	private ItemUiContext _E089;

	private _E74F _E135;

	private _EAE7 _E156;

	private _ECB1 _E18F;

	private _E9C4 _E190;

	[CanBeNull]
	private _EC60 _E191;

	public event Action OnBackButtonClick
	{
		[CompilerGenerated]
		add
		{
			Action action = _E18D;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E18D, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E18D;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E18D, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public async Task Show(_EC60 rightPanel, _EAED inventoryController, _E9C4 health, _E74F skills, _ECB1 insurance, EItemsTab currentTab, EItemViewType viewType)
	{
		UI.Dispose();
		_E191 = rightPanel;
		_E092 = inventoryController;
		_E156 = _E092.Inventory;
		_E190 = health;
		_E089 = ItemUiContext.Instance;
		_E135 = skills;
		_E18F = insurance;
		_E18E = currentTab;
		_E092.StopProcesses();
		_E089.CloseAllWindows();
		if (base.gameObject.activeSelf)
		{
			return;
		}
		ShowGameObject();
		bool wasClosed = false;
		UI.AddDisposable(delegate
		{
			wasClosed = true;
		});
		if (_backButton != null)
		{
			UI.SubscribeEvent(_backButton.OnClick, delegate
			{
				_E18D?.Invoke();
			});
		}
		_EB63<_EB0B> equipmentContext = new _EB63<_EB0B>(_E156.Equipment, viewType);
		_E089.RegisterView(equipmentContext);
		UI.AddDisposable(delegate
		{
			_E089.UnregisterView(equipmentContext);
		});
		switch (_E18E)
		{
		case EItemsTab.Gear:
			_equipmentPanel.Show(equipmentContext, _E092, _E135, _E18F);
			_healthParametersPanel.Show(_E190, _E156, _E135);
			break;
		case EItemsTab.Health:
			_healthPanel.Show(_E190, _E156, _E135);
			break;
		}
		if (SplitInFrames)
		{
			await Task.Yield();
			if (wasClosed)
			{
				return;
			}
		}
		_containers.Show(equipmentContext, _E092, _E135, _E18F);
		if (SplitInFrames)
		{
			await Task.Yield();
			if (wasClosed)
			{
				return;
			}
		}
		if (_quickUse != null)
		{
			_quickUse.Show(_E092, _E089);
			_quickUse.RefreshSelection(_E092.ItemInHands);
		}
		_E191?.Show(_E092, _E18E);
	}

	public override void Close()
	{
		_containers.Close();
		_E191?.Close();
		if (_quickUse != null)
		{
			_quickUse.Hide();
		}
		switch (_E18E)
		{
		case EItemsTab.Gear:
			_equipmentPanel.Hide();
			_healthParametersPanel.Close();
			break;
		case EItemsTab.Health:
			_healthPanel.Close();
			break;
		}
		base.Close();
	}
}
