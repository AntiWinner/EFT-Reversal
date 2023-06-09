using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.InventoryLogic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.DragAndDrop;

public class GridView : MonoBehaviour, _EC9E, _E640, _E63F, _E641, _E647, _E648, _E64E
{
	[CompilerGenerated]
	private sealed class _E001
	{
		public _EB22 gridItemAddress;

		public GridView _003C_003E4__this;

		internal bool _E000(ItemView x)
		{
			return _003C_003E4__this.Grid.GetItemLocation(x.Item) == gridItemAddress.LocationInGrid;
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public _EB22 gridItemAddress;

		public GridView _003C_003E4__this;

		internal bool _E000(ItemView x)
		{
			return _003C_003E4__this.Grid.GetItemLocation(x.Item) == gridItemAddress.LocationInGrid;
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public ItemView itemView;

		internal bool _E000(KeyValuePair<Item, LocationInGrid> item)
		{
			return item.Key == itemView.Item;
		}
	}

	[CompilerGenerated]
	private sealed class _E004
	{
		public KeyValuePair<Item, LocationInGrid> pair;

		internal bool _E000(ItemView visibleItem)
		{
			return visibleItem.Item != pair.Key;
		}
	}

	[CompilerGenerated]
	private sealed class _E005
	{
		public _EB47 _003CsplitResult_003E5__2;

		public _EB49 _003CtransferResult_003E5__3;

		public GridView _003C_003E4__this;

		internal void _E000(int count)
		{
			_003C_003E4__this.m__E005.SplitDialog.Hide();
			_003CsplitResult_003E5__2.ExecuteWithNewCount(_003C_003E4__this.m__E004, count);
		}

		internal void _E001(int count)
		{
			_003C_003E4__this.m__E005.SplitDialog.Hide();
			_003C_003E4__this.m__E004.TryRunNetworkTransaction(_003CtransferResult_003E5__3.ExecuteWithNewCount(count, simulate: true));
		}
	}

	[CompilerGenerated]
	private sealed class _E007
	{
		public Item item;

		public Func<ItemView, bool> check;

		internal bool _E000(ItemView view)
		{
			if (view.Item == item)
			{
				return check(view);
			}
			return false;
		}
	}

	[SerializeField]
	private Image _highlightPanel;

	[SerializeField]
	private bool _nonInteractable;

	[SerializeField]
	protected FilterPanel _filterPanel;

	[SerializeField]
	private bool _drawDebugGizmos;

	[CompilerGenerated]
	private _EB68 m__E000;

	private bool m__E001;

	public _E9EF Grid;

	internal readonly List<ItemView> _E002 = new List<ItemView>();

	protected readonly _E3A4 CompositeDisposable = new _E3A4();

	private IItemOwner m__E003;

	private _EB1E m__E004;

	private ItemUiContext m__E005;

	private _EB24? m__E006;

	private RectTransform m__E007;

	protected static readonly Color InvalidOperationColor = new Color(0.68f, 0f, 0f, 0.57f);

	protected static readonly Color ValidMoveColor = new Color(0.06f, 0.38f, 0.06f, 0.57f);

	private static readonly Color m__E008 = new Color(0f, 0.16f, 0.48f, 0.57f);

	private static readonly Color m__E009 = new Color(0.69f, 0.66f, 0f, 0.57f);

	private IEnumerable<KeyValuePair<Item, LocationInGrid>> m__E00A;

	private Rect m__E00B;

	protected _EB68 SourceContext
	{
		[CompilerGenerated]
		get
		{
			return this.m__E000;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E000 = value;
		}
	}

	public bool IsMagnified
	{
		private get
		{
			if (!(Grid is _E9F6))
			{
				return this.m__E001;
			}
			return false;
		}
		set
		{
			this.m__E001 = value;
		}
	}

	public void Show(_E9EF grid, [NotNull] _EB68 parentContext, [NotNull] _EB1E itemController, ItemUiContext itemUiContext, FilterPanel filterPanel = null, bool magnify = false)
	{
		if (this.m__E007 == null)
		{
			this.m__E007 = (RectTransform)base.transform;
		}
		if (magnify)
		{
			IsMagnified = true;
		}
		base.gameObject.SetActive(value: true);
		SourceContext = parentContext;
		Grid = grid;
		this.m__E004 = itemController;
		this.m__E003 = Grid.ParentItem.Parent.GetOwner();
		this.m__E003.RegisterView(this);
		this.m__E005 = itemUiContext;
		if (filterPanel != null)
		{
			_filterPanel = filterPanel;
		}
		if (_filterPanel != null)
		{
			_filterPanel.Init();
		}
		this.m__E006 = null;
		CompositeDisposable.SubscribeState(Grid.GridHeight, delegate(int gridHeight)
		{
			_E000(gridHeight, Grid.GridWidth.Value);
		});
		CompositeDisposable.BindState(Grid.GridWidth, delegate(int gridWidth)
		{
			_E000(Grid.GridHeight.Value, gridWidth);
		});
		if (this.m__E004 != null)
		{
			this.m__E004.OnItemFound += delegate(Item item)
			{
				if (_filterPanel != null)
				{
					_filterPanel.RegisterItem(item);
				}
			};
			CompositeDisposable.AddDisposable(delegate
			{
				this.m__E004.OnItemFound -= delegate(Item item)
				{
					if (_filterPanel != null)
					{
						_filterPanel.RegisterItem(item);
					}
				};
			});
		}
		_E001();
	}

	private void _E000(int gridHeight, int gridWidth)
	{
		_E313 cellPixelSize = ItemViewFactory.GetCellPixelSize(new _E313(gridWidth, gridHeight));
		LayoutElement component = GetComponent<LayoutElement>();
		if (component != null)
		{
			component.minWidth = cellPixelSize.X;
			component.minHeight = cellPixelSize.Y;
		}
		this.m__E007.sizeDelta = cellPixelSize;
		MagnifyIfPossible();
	}

	private void _E001()
	{
		PrepareItems();
		foreach (ItemView item in this._E002)
		{
			item.UpdateRemoveError();
		}
		_E007();
		_E003();
	}

	protected virtual void PrepareItems()
	{
		IEnumerable<KeyValuePair<Item, LocationInGrid>> enumerable;
		if (!(Grid is _E9F6 obj))
		{
			IEnumerable<KeyValuePair<Item, LocationInGrid>> containedItems = Grid.ContainedItems;
			enumerable = containedItems;
		}
		else
		{
			IEnumerable<KeyValuePair<Item, LocationInGrid>> containedItems = obj.GetItemsForPlayer(this.m__E004.ID);
			enumerable = containedItems;
		}
		this.m__E00A = enumerable;
		foreach (var (item2, locationInGrid2) in this.m__E00A)
		{
			if (_filterPanel != null && locationInGrid2.isSearched)
			{
				_filterPanel.RegisterItem(item2);
			}
			if (!IsMagnified)
			{
				_E005(_E004(item2, locationInGrid2, this.m__E005), locationInGrid2);
			}
		}
	}

	private void _E002()
	{
		foreach (ItemView item in this._E002)
		{
			item.Kill();
		}
		this._E002.Clear();
	}

	public async Task TransferAllItems()
	{
		if (this.m__E003.OwnerType != EOwnerType.Mail)
		{
			return;
		}
		for (Item key = this.m__E00A.FirstOrDefault().Key; key != null; key = this.m__E00A.FirstOrDefault().Key)
		{
			_ECD7 operationResult = this.m__E005.QuickFindAppropriatePlace(key, this.m__E004);
			bool flag = operationResult.Failed || !this.m__E004.CanExecute(operationResult.Value);
			if (!flag)
			{
				flag = (await this.m__E004.TryRunNetworkTransaction(operationResult)).Failed;
			}
			if (flag)
			{
				break;
			}
			await Task.Yield();
		}
	}

	private void _E003()
	{
		foreach (_EAF1 item in this.m__E003.SelectEvents(null))
		{
			if (item is _EAF2 obj)
			{
				if (obj.To.Container == Grid)
				{
					_EB22 gridItemAddress2 = (_EB22)obj.To;
					ItemView itemView = _E009(obj.Item, (ItemView x) => Grid.GetItemLocation(x.Item) == gridItemAddress2.LocationInGrid);
					if (itemView == null)
					{
						itemView = _E004(obj.Item, gridItemAddress2.LocationInGrid, this.m__E005, obj.To.GetOwnerOrNull());
						_E005(itemView, gridItemAddress2.LocationInGrid);
					}
					itemView.IsBeingAdded.Value = true;
				}
			}
			else if (item is _EAF3 obj2)
			{
				if (obj2.From.Container == Grid)
				{
					_EB22 gridItemAddress = (_EB22)obj2.From;
					ItemView itemView2 = _E009(obj2.Item, (ItemView x) => Grid.GetItemLocation(x.Item) == gridItemAddress.LocationInGrid);
					if (itemView2 == null)
					{
						itemView2 = _E004(obj2.Item, gridItemAddress.LocationInGrid, this.m__E005);
						_E005(itemView2, gridItemAddress.LocationInGrid);
					}
					itemView2.IsBeingRemoved.Value = true;
				}
			}
			else if (item is _EB01 obj3)
			{
				ItemView itemView3 = _E009(obj3.Item, (ItemView _) => true);
				if (itemView3 != null)
				{
					itemView3.IsBeingDrained.Value = true;
				}
			}
			else if (item is _EAF6 obj4)
			{
				ItemView itemView4 = _E009(obj4.Item, (ItemView _) => true);
				if (itemView4 != null)
				{
					itemView4.SetBeingExaminedState(obj4);
				}
			}
			else if (item is _EAF7 obj5)
			{
				ItemView itemView5 = _E009(obj5.TargetItem, (ItemView _) => true);
				if (itemView5 != null)
				{
					itemView5.SetLoadMagazineStatus(obj5);
				}
				ItemView itemView6 = _E009(obj5.Item, (ItemView _) => true);
				if (itemView6 != null)
				{
					itemView6.SetLoadAmmoStatus(obj5);
				}
			}
			else if (item is _EAF8 obj6)
			{
				ItemView itemView7 = _E009(obj6.FromItem, (ItemView _) => true);
				if (itemView7 != null)
				{
					itemView7.SetUnloadMagazineStatus(obj6);
				}
			}
			else if (item is _EB09 obj7)
			{
				ItemView itemView8 = _E009(obj7.Item, (ItemView _) => true);
				if (itemView8 != null)
				{
					itemView8.SetLoadAmmoStatus(obj7);
				}
			}
		}
	}

	protected void MagnifyIfPossible()
	{
		MagnifyIfPossible(this.m__E00B, force: true);
	}

	public void MagnifyIfPossible(Rect rect, bool force)
	{
		if (!IsMagnified || Grid == null)
		{
			return;
		}
		this.m__E00B = rect;
		Vector3 vector = base.transform.InverseTransformPoint(rect.min);
		Vector3 vector2 = base.transform.InverseTransformPoint(rect.max) - vector;
		int x = Mathf.RoundToInt(vector.x / 63f);
		int num = Mathf.RoundToInt((0f - vector.y) / 63f);
		int width = Mathf.RoundToInt(vector2.x / 63f);
		int num2 = Mathf.RoundToInt(vector2.y / 63f);
		_EB24 obj = new _EB24(x, num - num2, width, num2);
		if (!force && this.m__E006.Equals(obj))
		{
			return;
		}
		this.m__E006 = obj;
		List<KeyValuePair<Item, LocationInGrid>> source = Grid.GetItemsInRect(obj).ToList();
		for (int num3 = this._E002.Count - 1; num3 >= 0; num3--)
		{
			ItemView itemView = this._E002[num3];
			if (!itemView.BeingDragged && !source.Any((KeyValuePair<Item, LocationInGrid> item) => item.Key == itemView.Item))
			{
				itemView.Kill();
				this._E002.Remove(itemView);
			}
		}
		foreach (var (item3, location) in source.Where((KeyValuePair<Item, LocationInGrid> pair) => this._E002.All((ItemView visibleItem) => visibleItem.Item != pair.Key)))
		{
			_E005(_E004(item3, location, this.m__E005), location);
		}
	}

	private void OnDrawGizmos()
	{
		if (!_drawDebugGizmos)
		{
			return;
		}
		for (int i = 0; i < Grid.GridHeight.Value; i++)
		{
			for (int j = 0; j < Grid.GridWidth.Value; j++)
			{
				if (Grid.LayoutBuffer[i * Grid.GridWidth.Value + j])
				{
					_E313 cellPixelSize = ItemViewFactory.GetCellPixelSize(new _E313(j, i));
					Vector3 vector = new Vector3((float)cellPixelSize.X + 31f, (float)(-cellPixelSize.Y) - 31f);
					Gizmos.DrawIcon(base.transform.position + vector, _ED3E._E000(236892), allowScaling: true);
				}
			}
		}
	}

	public void OnItemAdded(_EAF2 eventArgs)
	{
		if (eventArgs.To.Container != Grid)
		{
			return;
		}
		Item item = eventArgs.Item;
		LocationInGrid locationInGrid = ((_EB22)eventArgs.To).LocationInGrid;
		switch (eventArgs.Status)
		{
		case CommandStatus.Begin:
		{
			if (this.m__E004 != null && this.m__E004.CanSearch && eventArgs.ProfileId != this.m__E004.ID)
			{
				locationInGrid.isSearched = false;
			}
			if (locationInGrid.isSearched)
			{
				ItemView itemView3 = _E009(item, (ItemView iv) => !iv.IsSearched);
				if (itemView3 != null)
				{
					this._E002.Remove(itemView3);
					itemView3.Kill();
				}
			}
			ItemView itemView4 = _E004(item, ((_EB22)eventArgs.To).LocationInGrid, this.m__E005, eventArgs.To.GetOwnerOrNull());
			itemView4.IsBeingAdded.Value = true;
			_E005(itemView4, ((_EB22)eventArgs.To).LocationInGrid);
			break;
		}
		case CommandStatus.Succeed:
		{
			if (_filterPanel != null && locationInGrid.isSearched)
			{
				_filterPanel.RegisterItem(item);
			}
			ItemView itemView2 = _E009(item, (ItemView view) => view.IsBeingAdded.Value);
			if (itemView2 != null)
			{
				itemView2.IsBeingAdded.Value = false;
			}
			break;
		}
		case CommandStatus.Failed:
		{
			ItemView itemView = _E009(item, (ItemView view) => view.IsBeingAdded.Value);
			if (itemView != null)
			{
				this._E002.Remove(itemView);
				itemView.Kill();
			}
			break;
		}
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	public void OnItemRemoved(_EAF3 eventArgs)
	{
		if (eventArgs.From.Container != Grid)
		{
			return;
		}
		Item item = eventArgs.Item;
		switch (eventArgs.Status)
		{
		case CommandStatus.Begin:
		{
			ItemView itemView3 = _E009(item, (ItemView _) => true);
			if (itemView3 != null)
			{
				itemView3.IsBeingRemoved.Value = true;
			}
			break;
		}
		case CommandStatus.Succeed:
		{
			if (_filterPanel != null)
			{
				_filterPanel.UnregisterItem(item);
			}
			ItemView itemView2 = _E009(item, (ItemView view) => view.IsBeingRemoved.Value);
			if (itemView2 != null)
			{
				this._E002.Remove(itemView2);
				itemView2.Kill();
			}
			break;
		}
		case CommandStatus.Failed:
		{
			ItemView itemView = _E009(item, (ItemView view) => view.IsBeingRemoved.Value);
			if (itemView != null)
			{
				itemView.IsBeingRemoved.Value = false;
				DragCancelled();
			}
			break;
		}
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	public void OnRefreshItem(_EAFF eventArgs)
	{
		if (eventArgs.Container == Grid)
		{
			ItemView itemView = _E009(eventArgs.Item, (ItemView _) => true);
			if (itemView != null)
			{
				SetItemViewPosition(itemView, Grid.ItemCollection[itemView.Item]);
			}
		}
	}

	public void OnRefreshContainer(_EB00 args)
	{
		if (args.Container == Grid)
		{
			if (_filterPanel != null)
			{
				_filterPanel.ClearAll();
			}
			_E002();
			MagnifyIfPossible();
			_E001();
		}
	}

	public void OnDiscoverItem(_EB07 eventArgs)
	{
		if (eventArgs.To.Container == Grid)
		{
			Singleton<GUISounds>.Instance.PlayItemSound(eventArgs.Item.ItemSound, EInventorySoundType.drop);
		}
	}

	protected void SetItemViewPosition(ItemView itemView, LocationInGrid location)
	{
		RectTransform obj = (RectTransform)itemView.transform;
		Vector2 vector = new Vector2(0f, 1f);
		obj.localScale = Vector3.one;
		obj.pivot = vector;
		obj.anchorMin = vector;
		obj.anchorMax = vector;
		obj.localPosition = Vector3.zero;
		obj.anchoredPosition = new Vector2(location.x * 63, -location.y * 63);
	}

	private ItemView _E004(Item item, LocationInGrid location, ItemUiContext itemUiContext, IItemOwner itemOwner = null)
	{
		return itemUiContext.CreateItemView(item, SourceContext, location.r, this.m__E004, itemOwner, _filterPanel, this, slotView: false, canSelect: true, location.isSearched);
	}

	private void _E005(ItemView itemView, LocationInGrid location)
	{
		itemView.transform.SetParent(base.transform);
		SetItemViewPosition(itemView, location);
		this._E002.Add(itemView);
	}

	private void _E006()
	{
		if (!_highlightPanel.gameObject.activeSelf)
		{
			_highlightPanel.gameObject.SetActive(value: true);
		}
	}

	private void _E007()
	{
		_highlightPanel.gameObject.SetActive(value: false);
	}

	protected virtual Color GetHighlightColor(_EB69 itemContext, _ECD7 possibleOperation, [CanBeNull] _EB68 targetItemContext)
	{
		if (possibleOperation.Failed)
		{
			return InvalidOperationColor;
		}
		if (possibleOperation.Value is _EB3B || possibleOperation.Value is _EB4B)
		{
			return ValidMoveColor;
		}
		if (possibleOperation.Value is _EB3D || possibleOperation.Value is _EB47 || possibleOperation.Value is _EB49)
		{
			return GridView.m__E009;
		}
		if (possibleOperation.Value is _EB4A)
		{
			return GridView.m__E008;
		}
		return InvalidOperationColor;
	}

	public void HighlightItemViewPosition(_EB69 itemContext, _EB68 targetItemContext, bool preview)
	{
		if (preview)
		{
			return;
		}
		CanAccept(itemContext, targetItemContext, out var operation);
		_E006();
		InventoryError inventoryError = operation.Error as InventoryError;
		if (inventoryError is _EA6A._E002 && _E008(targetItemContext) is _EA6A obj)
		{
			this.m__E004.StrictCheckMagazine(obj, status: true);
			obj.RaiseRefreshEvent();
		}
		if (targetItemContext != null)
		{
			targetItemContext.InventoryError = inventoryError;
		}
		InventoryError inventoryError2 = inventoryError;
		if (inventoryError2 != null)
		{
			if (!(inventoryError2 is _E9FE) && !(inventoryError2 is _EA02) && !(inventoryError2 is _E9EF._E001) && !(inventoryError2 is _E9EF._E000) && !(inventoryError2 is _EB29._E001) && !(inventoryError2 is Slot._E008) && !(inventoryError2 is _E9FB) && !(inventoryError2 is _E9FA))
			{
				if (!(inventoryError2 is _EB29._E000 obj2))
				{
					inventoryError2 = inventoryError2;
					InventoryError error = inventoryError2;
					this.m__E005.Tooltip.ShowInventoryError(error);
				}
				else
				{
					_EB29._E000 obj3 = obj2;
					if (this.m__E004 is _EAED obj4 && !obj4.HasKnownMalfunction(obj3.Weapon))
					{
						obj4.ExamineMalfunction(obj3.Weapon);
					}
					this.m__E005.Tooltip.ShowInventoryError(obj3);
				}
			}
			else if (this.m__E005.Tooltip.isActiveAndEnabled)
			{
				this.m__E005.Tooltip.Close();
			}
		}
		if (operation.Succeeded && operation.Value is _EB30 obj5 && obj5.ItemsDestroyRequired)
		{
			this.m__E005.Tooltip.ShowWarning(new _EA0B(itemContext.Item, obj5.ItemsToDestroy));
		}
		_highlightPanel.color = GetHighlightColor(itemContext, operation, targetItemContext);
		RectTransform rectTransform = _highlightPanel.rectTransform;
		rectTransform.localScale = Vector3.one;
		rectTransform.pivot = new Vector2(0f, 1f);
		rectTransform.anchorMin = new Vector2(0f, 1f);
		rectTransform.anchorMax = new Vector2(0f, 1f);
		rectTransform.localPosition = Vector3.zero;
		_E313 obj6 = itemContext.Item.CalculateRotatedSize(itemContext.ItemRotation);
		LocationInGrid locationInGrid = CalculateItemLocation(itemContext);
		int x = locationInGrid.x;
		int y = locationInGrid.y;
		int value = x + obj6.X;
		int value2 = y + obj6.Y;
		x = Mathf.Clamp(x, 0, Grid.GridWidth.Value);
		y = Mathf.Clamp(y, 0, Grid.GridHeight.Value);
		value = Mathf.Clamp(value, 0, Grid.GridWidth.Value);
		value2 = Mathf.Clamp(value2, 0, Grid.GridHeight.Value);
		rectTransform.anchoredPosition = new Vector2(x * 63, -y * 63);
		rectTransform.sizeDelta = new Vector2((value - x) * 63, (value2 - y) * 63);
	}

	public virtual bool CanAccept(_EB69 itemContext, _EB68 targetItemContext, out _ECD7 operation)
	{
		operation = default(_ECD7);
		if (Grid == null)
		{
			return false;
		}
		if (_nonInteractable)
		{
			return false;
		}
		Item item = itemContext.Item;
		LocationInGrid locationInGrid = CalculateItemLocation(itemContext);
		Item item2 = _E008(targetItemContext);
		_EB22 obj = new _EB22(Grid, locationInGrid);
		ItemAddress itemAddress = itemContext.ItemAddress;
		if (itemAddress == null)
		{
			return false;
		}
		if (itemAddress.Container == Grid && Grid.GetItemLocation(item) == locationInGrid)
		{
			return false;
		}
		bool partialTransferOnly = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
		if (!item.CheckAction(obj))
		{
			return false;
		}
		operation = ((item2 != null) ? this.m__E004.ExecutePossibleAction(itemContext, item2, partialTransferOnly, simulate: true) : this.m__E004.ExecutePossibleAction(itemContext, obj, partialTransferOnly, simulate: true));
		return operation.Succeeded;
	}

	public void DisableHighlight()
	{
		if (this.m__E005 != null && this.m__E005.Tooltip.isActiveAndEnabled)
		{
			this.m__E005.Tooltip.Close();
		}
		_E007();
	}

	public LocationInGrid CalculateItemLocation(_EB69 itemContext)
	{
		RectTransform rectTransform = base.transform.RectTransform();
		Vector2 size = rectTransform.rect.size;
		Vector2 pivot = rectTransform.pivot;
		Vector2 vector = size * pivot;
		Vector2 vector2 = rectTransform.InverseTransformPoint(itemContext.ItemPosition);
		vector2 += vector;
		_E313 obj = itemContext.Item.CalculateRotatedSize(itemContext.ItemRotation);
		int num = 63;
		vector2 /= (float)num;
		vector2.y = (float)Grid.GridHeight.Value - vector2.y;
		vector2.y -= obj.Y;
		return new LocationInGrid(Mathf.Clamp(Mathf.RoundToInt(vector2.x), 0, Grid.GridWidth.Value), Mathf.Clamp(Mathf.RoundToInt(vector2.y), 0, Grid.GridHeight.Value), itemContext.ItemRotation);
	}

	private Item _E008(_EB68 targetItemContext)
	{
		if (targetItemContext == null || !targetItemContext.Searched)
		{
			return null;
		}
		return targetItemContext.Item;
	}

	public void DragStarted()
	{
	}

	public void DragCancelled()
	{
	}

	public bool CanDrag(_EB68 itemContext)
	{
		if (itemContext.DragAvailable && !this.m__E003.Locked)
		{
			return itemContext.Item.CheckAction(null);
		}
		return false;
	}

	public virtual async Task AcceptItem(_EB69 itemContext, _EB68 targetItemContext)
	{
		if (!CanAccept(itemContext, targetItemContext, out var operation) || !(await operation.Value.TryShowDestroyItemsDialog()))
		{
			return;
		}
		if (itemContext.Item is _EA12 ammo)
		{
			Item item = _E008(targetItemContext);
			if (item != null)
			{
				if (item is _EA6A obj)
				{
					_EA6A obj2 = obj;
					int loadCount = _E00F(obj2, ammo);
					this.m__E004.LoadMagazine(ammo, obj2, loadCount).HandleExceptions();
					return;
				}
				if (item is Weapon weapon)
				{
					Weapon weapon2 = weapon;
					if (weapon2.SupportsInternalReload)
					{
						_EA6A currentMagazine = weapon2.GetCurrentMagazine();
						if (currentMagazine != null)
						{
							int num = _E00F(currentMagazine, ammo);
							if (num != 0)
							{
								this.m__E004.LoadWeaponWithAmmo(weapon2, ammo, num).HandleExceptions();
								return;
							}
						}
					}
					else
					{
						Weapon weapon3 = weapon;
						if (weapon3.IsMultiBarrel)
						{
							int ammoCount = _E010(weapon3, ammo);
							this.m__E004.LoadMultiBarrelWeapon(weapon3, ammo, ammoCount).HandleExceptions();
							return;
						}
					}
				}
			}
		}
		bool flag = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
		if (operation.Failed || !this.m__E004.CanExecute(operation.Value))
		{
			itemContext.DragCancelled();
			return;
		}
		_EB2D value = operation.Value;
		if (value == null)
		{
			goto IL_033d;
		}
		if (!(value is _EB47 obj3))
		{
			if (!(value is _EB49 obj4))
			{
				goto IL_033d;
			}
			_EB49 obj5 = obj4;
			itemContext.DragCancelled();
			if (obj5.Count > 1 && flag)
			{
				this.m__E005.SplitDialog.Show(_ED3E._E000(197696).Localized(), obj5.Count, itemContext.CursorPosition, delegate(int count)
				{
					this.m__E005.SplitDialog.Hide();
					this.m__E004.TryRunNetworkTransaction(obj5.ExecuteWithNewCount(count, simulate: true));
				}, delegate
				{
					this.m__E005.SplitDialog.Hide();
				});
			}
			else
			{
				this.m__E004.RunNetworkTransaction(obj5);
			}
		}
		else
		{
			_EB47 obj6 = obj3;
			itemContext.DragCancelled();
			if (obj6.Count > 1 && flag)
			{
				this.m__E005.SplitDialog.Show(_ED3E._E000(197670).Localized(), obj6.Count, itemContext.CursorPosition, delegate(int count)
				{
					this.m__E005.SplitDialog.Hide();
					obj6.ExecuteWithNewCount(this.m__E004, count);
				}, delegate
				{
					this.m__E005.SplitDialog.Hide();
				});
			}
			else
			{
				this.m__E004.RunNetworkTransaction(obj6);
			}
		}
		goto IL_0354;
		IL_033d:
		this.m__E004.RunNetworkTransaction(operation.Value);
		goto IL_0354;
		IL_0354:
		ItemUiContext.PlayOperationSound(itemContext.Item, operation.Value);
	}

	[CanBeNull]
	private ItemView _E009(Item item, Func<ItemView, bool> check)
	{
		return this._E002.Find((ItemView view) => view.Item == item && check(view));
	}

	public virtual void Hide()
	{
		if (Grid == null)
		{
			return;
		}
		CompositeDisposable.Dispose();
		_E002();
		this.m__E003?.UnregisterView(this);
		this.m__E003 = null;
		if (_filterPanel != null)
		{
			IEnumerable<KeyValuePair<Item, LocationInGrid>> enumerable;
			if (!(Grid is _E9F6 obj))
			{
				IEnumerable<KeyValuePair<Item, LocationInGrid>> containedItems = Grid.ContainedItems;
				enumerable = containedItems;
			}
			else
			{
				IEnumerable<KeyValuePair<Item, LocationInGrid>> containedItems = obj.GetItemsForPlayer(this.m__E004.ID);
				enumerable = containedItems;
			}
			foreach (KeyValuePair<Item, LocationInGrid> item in enumerable)
			{
				_filterPanel.UnregisterItem(item.Key);
			}
			_filterPanel.Hide();
		}
		this.m__E004 = null;
		this.m__E005 = null;
		SourceContext = null;
		Grid = null;
		base.gameObject.SetActive(value: false);
	}

	[CompilerGenerated]
	private void _E00A(int gridHeight)
	{
		_E000(gridHeight, Grid.GridWidth.Value);
	}

	[CompilerGenerated]
	private void _E00B(int gridWidth)
	{
		_E000(Grid.GridHeight.Value, gridWidth);
	}

	[CompilerGenerated]
	private void _E00C(Item item)
	{
		if (_filterPanel != null)
		{
			_filterPanel.RegisterItem(item);
		}
	}

	[CompilerGenerated]
	private void _E00D()
	{
		this.m__E004.OnItemFound -= delegate(Item item)
		{
			if (_filterPanel != null)
			{
				_filterPanel.RegisterItem(item);
			}
		};
	}

	[CompilerGenerated]
	private bool _E00E(KeyValuePair<Item, LocationInGrid> pair)
	{
		return this._E002.All((ItemView visibleItem) => visibleItem.Item != pair.Key);
	}

	[CompilerGenerated]
	internal static int _E00F(_EA6A mag, _EA12 ammo)
	{
		int num = mag.MaxCount - mag.Count;
		if (ammo.StackObjectsCount <= num)
		{
			return ammo.StackObjectsCount;
		}
		return num;
	}

	[CompilerGenerated]
	internal static int _E010(Weapon weapon, _EA12 ammo)
	{
		int freeChamberSlotsCount = weapon.FreeChamberSlotsCount;
		if (ammo.StackObjectsCount <= freeChamberSlotsCount)
		{
			return ammo.StackObjectsCount;
		}
		return freeChamberSlotsCount;
	}

	[CompilerGenerated]
	private void _E011()
	{
		this.m__E005.SplitDialog.Hide();
	}

	[CompilerGenerated]
	private void _E012()
	{
		this.m__E005.SplitDialog.Hide();
	}
}
