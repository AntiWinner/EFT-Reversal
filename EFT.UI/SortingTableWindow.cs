using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class SortingTableWindow : Window<_EC7C>, _E640, _E63F, _E641
{
	private const string m__E000 = "SortingTable/SellFromSortingTableConfirmation{0}";

	private const string m__E001 = "SortingTable/TransferFromSortingTableError";

	private const int m__E002 = 7;

	[SerializeField]
	private GridView _grid;

	[SerializeField]
	private Button _sellAllButton;

	[SerializeField]
	private CanvasGroup _sellButtonCanvasGroup;

	[SerializeField]
	private GameObject _blockScreen;

	private _E8B0 m__E003;

	private _EA98 m__E004;

	private _EAED m__E005;

	private IItemOwner m__E006;

	private ItemUiContext m__E007;

	private _EB63<_EA98> m__E008;

	private Action m__E009;

	private bool m__E00A;

	private bool m__E00B;

	private Task<bool> m__E00C;

	public bool IsVisible => this.m__E004?.IsVisible ?? false;

	protected override void Awake()
	{
		base.Awake();
		_sellAllButton.onClick.AddListener(delegate
		{
			_E001(_ED3E._E000(246019), delegate
			{
				_grid.Hide();
				_E000();
			});
		});
	}

	public void Show(_EB63<_EA98> sortingTableContext, _E796 session, _EAED inventoryController, ItemUiContext uiContext, Action onClosed, Action afterClosed)
	{
		Show(onClosed);
		this.m__E003 = new _E8B0(session);
		this.m__E004 = sortingTableContext.CastItem;
		this.m__E005 = inventoryController;
		this.m__E007 = uiContext;
		this.m__E008 = sortingTableContext;
		this.m__E009 = afterClosed;
		this.m__E006 = this.m__E004.Grid.ParentItem.Parent.GetOwner();
		this.m__E006.RegisterView(this);
		if (this.m__E003.Ready)
		{
			_E006();
		}
		else
		{
			this.m__E003.OnReadyStatusChanged += _E005;
		}
		_E000();
	}

	private void _E000()
	{
		this.m__E004.ClampSize(7, 7);
		_grid.Show(this.m__E004.Grid, this.m__E008, this.m__E005, this.m__E007, null, magnify: true);
	}

	private void _E001(string messageFormat, Action callback)
	{
	}

	private async Task _E002(IEnumerable<Item> sellingItems, Action callback)
	{
		this.m__E00A = true;
		_sellAllButton.interactable = false;
		_blockScreen.SetActive(value: true);
		int price = 0;
		await this.m__E003.SellItems(sellingItems, price);
		await Task.Yield();
		await _E003();
		this.m__E004.ClampSize(7, 7);
		this.m__E00A = false;
		_blockScreen.SetActive(value: false);
		_sellAllButton.interactable = true;
		callback();
	}

	private async Task _E003()
	{
		_EAED obj = ((this.m__E005.MainStorage == null) ? (this.m__E004.CurrentAddress.GetOwnerOrNull() as _EAED) : this.m__E005);
		if (obj == null)
		{
			MonoBehaviourSingleton<PreloaderUI>.Instance.ShowErrorScreen(null, _ED3E._E000(246092).Localized());
			return;
		}
		for (Item item = this.m__E004.Grid.Items.FirstOrDefault(); item != null; item = this.m__E004.Grid.Items.FirstOrDefault())
		{
			_ECD7 operationResult = this.m__E007.QuickFindAppropriatePlace(item, obj, forcePutInStash: true, displayWarnings: false);
			bool failed = operationResult.Failed;
			if (!failed)
			{
				failed = (await obj.TryRunNetworkTransaction(operationResult)).Failed;
			}
			if (failed)
			{
				MonoBehaviourSingleton<PreloaderUI>.Instance.ShowErrorScreen(null, _ED3E._E000(246092).Localized());
				break;
			}
		}
	}

	private async Task<bool> _E004()
	{
		this.m__E00B = true;
		if (!this.m__E00A)
		{
			await _E003();
		}
		else
		{
			await TasksExtensions.WaitUntil(() => !this.m__E00A);
		}
		bool result = !this.m__E004.Grid.Items.Any();
		this.m__E00B = false;
		return result;
	}

	public void OnItemAdded(_EAF2 obj)
	{
		_E006();
	}

	public void OnItemRemoved(_EAF3 obj)
	{
		_E006();
	}

	private void _E005()
	{
		_E006();
	}

	private void _E006()
	{
	}

	private void _E007()
	{
		base.RectTransform.CorrectPositionResolution();
	}

	public Task<bool> TryClose()
	{
		if (this.m__E00C == null || this.m__E00C.IsCompleted)
		{
			this.m__E00C = _E00B();
		}
		return this.m__E00C;
	}

	public override void Close()
	{
		TryClose().HandleExceptions();
	}

	[CompilerGenerated]
	private void _E008()
	{
		_E001(_ED3E._E000(246019), delegate
		{
			_grid.Hide();
			_E000();
		});
	}

	[CompilerGenerated]
	private void _E009()
	{
		_grid.Hide();
		_E000();
	}

	[CompilerGenerated]
	private bool _E00A()
	{
		return !this.m__E00A;
	}

	[CompilerGenerated]
	private async Task<bool> _E00B()
	{
		if (this.m__E00B)
		{
			return false;
		}
		if (this.m__E004.Grid.Items.Any() && !(await _E004()))
		{
			return false;
		}
		_grid.Hide();
		this.m__E006.UnregisterView(this);
		this.m__E003.OnReadyStatusChanged -= _E005;
		this.m__E004.ClampSize(0, 0);
		base.Close();
		this.m__E009?.Invoke();
		return true;
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private void _E00C()
	{
		base.Close();
	}
}
