using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using ChatShared;
using Comfort.Common;
using EFT.Hideout;
using EFT.InputSystem;
using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using EFT.UI.Insurance;
using EFT.UI.Ragfair;
using EFT.UI.Screens;
using EFT.UI.WeaponModding;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class ItemUiContext : UIInputNode
{
	private sealed class _E000
	{
		private static int m__E000;

		public readonly Item Item;

		public readonly Window<_EC7C> Window;

		public readonly Type WindowType;

		public readonly int Index;

		public _E000(Item item, Window<_EC7C> window, Type windowType)
		{
			Item = item;
			Window = window;
			WindowType = windowType;
			Index = m__E000;
			m__E000++;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public _EB68 itemContext;

		public ItemUiContext _003C_003E4__this;

		public _EC4E<EItemInfoButton> contextInteractions;

		internal void _E000(InfoWindow window, Action setPriority, Action onClosed)
		{
			window.Show(itemContext, _003C_003E4__this._E084, _003C_003E4__this._E08A, setPriority, _003C_003E4__this, onClosed, contextInteractions);
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public List<_EA12> ammo;

		public string ammoTemplateId;

		public _EA6A magazine;

		public ItemUiContext _003C_003E4__this;

		internal async Task<bool> _E000(_EA40 container)
		{
			container.GetAllAssembledItems(ammo);
			List<_EA12> list = (from ammoStack in ammo
				where ammoStack.TemplateId == ammoTemplateId && !(ammoStack.Parent.Container is Slot)
				select ammoStack into item
				orderby item.SpawnedInSession, item.StackObjectsCount
				select item).ToList();
			ammo.Clear();
			foreach (_EA12 item in list)
			{
				int stackObjectsCount;
				do
				{
					stackObjectsCount = item.StackObjectsCount;
					if (stackObjectsCount == 0)
					{
						break;
					}
					_ECD7 operationResult = magazine.Apply(_003C_003E4__this._E084, item, int.MaxValue, simulate: true);
					if (operationResult.Failed)
					{
						return true;
					}
					if ((await _003C_003E4__this._E084.TryRunNetworkTransaction(operationResult)).Failed)
					{
						return true;
					}
					if (magazine.MaxCount == magazine.Count)
					{
						return true;
					}
				}
				while (stackObjectsCount != item.StackObjectsCount);
			}
			return false;
		}

		internal bool _E001(_EA12 ammoStack)
		{
			if (ammoStack.TemplateId == ammoTemplateId)
			{
				return !(ammoStack.Parent.Container is Slot);
			}
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _E004
	{
		public Item targetItem;

		public List<Item> sourcesBuffer;

		public Item currentSource;

		public ItemUiContext _003C_003E4__this;

		public bool inventoryChanged;

		internal bool _E000()
		{
			return targetItem.StackObjectsCount >= targetItem.StackMaxSize;
		}

		internal async Task _E001(Item container)
		{
			if (container == null || _E000())
			{
				return;
			}
			int num = 0;
			sourcesBuffer.Clear();
			container.GetAllAssembledItemsNonAlloc(sourcesBuffer);
			sourcesBuffer.RemoveAll((Item x) => !targetItem.IsSameItem(x) || x.Parent.Container is Slot);
			sourcesBuffer.Sort((Item x, Item y) => x.StackObjectsCount.CompareTo(y.StackObjectsCount));
			while (!_E000() && num < sourcesBuffer.Count)
			{
				currentSource = sourcesBuffer[num++];
				_ECD7 operationResult = _EB29.TransferOrMerge(currentSource, targetItem, _003C_003E4__this._E084, simulate: true);
				if (!operationResult.Failed)
				{
					await _003C_003E4__this._E084.TryRunNetworkTransaction(operationResult);
					if (inventoryChanged)
					{
						Debug.LogError(_ED3E._E000(246558));
						break;
					}
				}
			}
		}

		internal bool _E002(Item x)
		{
			if (targetItem.IsSameItem(x))
			{
				return x.Parent.Container is Slot;
			}
			return true;
		}

		internal void _E003(_EAFF args)
		{
			inventoryChanged |= args.Item != targetItem && args.Item != currentSource;
		}

		internal void _E004(_EAF3 args)
		{
			if (args.Status == CommandStatus.Succeed)
			{
				inventoryChanged |= args.Item != targetItem && args.Item != currentSource;
			}
		}
	}

	[CompilerGenerated]
	private sealed class _E007
	{
		public List<Item> itemsBuffer;

		public Item targetItem;

		internal bool _E000(Item container)
		{
			if (container == null)
			{
				return false;
			}
			itemsBuffer.Clear();
			container.GetAllAssembledItemsNonAlloc(itemsBuffer);
			itemsBuffer.RemoveAll((Item x) => !targetItem.IsSameItem(x) || x.Parent.Container is Slot);
			if (itemsBuffer.Count > 0)
			{
				itemsBuffer.Clear();
				return true;
			}
			return false;
		}

		internal bool _E001(Item x)
		{
			if (targetItem.IsSameItem(x))
			{
				return x.Parent.Container is Slot;
			}
			return true;
		}
	}

	[CompilerGenerated]
	private sealed class _E008
	{
		public Item item;

		public Action callback;

		internal void _E000(IResult result)
		{
			if (result.Succeed)
			{
				_E857.DisplayMessageNotification(_ED3E._E000(246598).Localized() + item.ShortName.Localized() + _ED3E._E000(246646) + item.ExamineExperience + _ED3E._E000(246642));
			}
			callback?.Invoke();
		}
	}

	[CompilerGenerated]
	private sealed class _E009
	{
		public TagComponent tagComponent;

		public ItemUiContext _003C_003E4__this;

		public Action<string, int> _003C_003E9__1;

		internal void _E000(EditTagWindow window, Action onSelect, Action onClose)
		{
			window.Show(tagComponent, onSelect, onClose, delegate(string tagName, int tagColor)
			{
				_003C_003E4__this._E084.TryRunNetworkTransaction(tagComponent.Set(tagName, tagColor, simulate: true));
			});
		}

		internal void _E001(string tagName, int tagColor)
		{
			_003C_003E4__this._E084.TryRunNetworkTransaction(tagComponent.Set(tagName, tagColor, simulate: true));
		}
	}

	[CompilerGenerated]
	private sealed class _E00B
	{
		public _EB68 itemContext;

		public ItemUiContext _003C_003E4__this;

		internal void _E000(InsuranceWindow window, Action setPriority, Action onClosed)
		{
			window.Show(itemContext.Item, _003C_003E4__this._E085.Inventory, _003C_003E4__this._E084, _003C_003E4__this.Session.InsuranceCompany, _003C_003E4__this._messageWindow, _003C_003E4__this._E085.Skills, setPriority, _003C_003E4__this.WeaponPreviewPool, onClosed);
		}
	}

	[CompilerGenerated]
	private sealed class _E00C
	{
		public _E8D0 repairController;

		public _EB68 itemContext;

		public ItemUiContext _003C_003E4__this;

		internal void _E000(RepairWindow window, Action setPriority, Action onClosed)
		{
			window.Show(repairController, itemContext, _003C_003E4__this._E085.Inventory, _003C_003E4__this._messageWindow, _003C_003E4__this._E084, _003C_003E4__this.Session.Repairers, _003C_003E4__this._E085.Skills, setPriority, _003C_003E4__this.WeaponPreviewPool, onClosed);
		}
	}

	[CompilerGenerated]
	private sealed class _E00E
	{
		public ItemUiContext _003C_003E4__this;

		public _E9C4 healthController;

		public _EA48 foodDrink;

		public int maxValue;

		internal void _E000(int amount)
		{
			_003C_003E4__this.SplitDialog.Hide();
			healthController.ApplyItem(foodDrink, EBodyPart.Common, (float)amount / (float)maxValue);
		}

		internal void _E001()
		{
			_003C_003E4__this.SplitDialog.Hide();
		}
	}

	[CompilerGenerated]
	private sealed class _E010
	{
		public _EA40 item;

		public _EB68 itemContext;

		public ItemUiContext _003C_003E4__this;

		public bool searchButtonDisplay;

		internal void _E000(GridWindow window, Action setPriority, Action onClosed)
		{
			window.Show(item, itemContext, _003C_003E4__this._E084, setPriority, onClosed, _003C_003E4__this, searchButtonDisplay);
		}
	}

	[CompilerGenerated]
	private sealed class _E012
	{
		public ItemUiContext _003C_003E4__this;

		public _EA6A foundMagazine;

		public Slot magazineSlot;

		internal void _E000(IResult result)
		{
			_003C_003E4__this._E084.TryRunNetworkTransaction(_EB29.Move(foundMagazine, new _EB20(magazineSlot), _003C_003E4__this._E084, simulate: true));
		}
	}

	[CompilerGenerated]
	private sealed class _E013
	{
		public _EA6A currentMagazine;

		internal _EB22 _E000(_E9EF grid)
		{
			return grid.FindLocationForItem(currentMagazine);
		}
	}

	[CompilerGenerated]
	private sealed class _E015
	{
		public ItemUiContext _003C_003E4__this;

		public Mod mod;

		public Func<Slot, bool> _003C_003E9__2;

		internal bool _E000(Weapon weapon)
		{
			return _003C_003E4__this._E084.Examined(weapon);
		}

		internal bool _E001(Slot slot)
		{
			return slot.CanAccept(mod);
		}
	}

	[CompilerGenerated]
	private sealed class _E01B
	{
		public ItemUiContext _003C_003E4__this;

		public Slot magazineSlot;

		internal bool _E000(_EA6A mag)
		{
			if (mag.Count > 0 && !(mag.GetRootItem() is Weapon) && _003C_003E4__this._E084.Examined(mag))
			{
				return magazineSlot.CanAccept(mag);
			}
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _E01C
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
	private sealed class _E01D
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
	private sealed class _E01E<_E0B8> where _E0B8 : Window<_EC7C>, IShowable
	{
		public _E000 windowData;

		public ItemUiContext _003C_003E4__this;

		public _E0B8 window;

		internal void _E000()
		{
			_E002(windowData);
		}

		internal void _E001()
		{
			_003C_003E4__this._E083.Remove(windowData);
			UnityEngine.Object.Destroy(window.gameObject);
		}

		internal void _E002(_E000 data)
		{
			if (_003C_003E4__this._E083.LastOrDefault() != data)
			{
				_003C_003E4__this._E083.Remove(data);
				_003C_003E4__this._E083.Add(data);
			}
			data.Window.transform.SetAsLastSibling();
		}
	}

	[SerializeField]
	private RectTransform _contextMenuArea;

	[SerializeField]
	private SimpleTooltip _simpleTooltip;

	[SerializeField]
	private ItemTooltip _itemTooltip;

	[SerializeField]
	private PriceTooltip _priceTooltip;

	[SerializeField]
	private SkillTooltip _skillTooltip;

	[SerializeField]
	private MasteringTooltip _masteringTooltip;

	[SerializeField]
	private MultiLineTooltip _multiLineTooltip;

	[SerializeField]
	private RectTransform _infoWindowsContainer;

	[SerializeField]
	private SelectItemContextMenu _selectItemMenu;

	[SerializeField]
	private InfoWindow _infoWindowTemplate;

	[SerializeField]
	private RepairWindow _repairWindowTemplate;

	[SerializeField]
	private InsuranceWindow _insuranceWindowTemplate;

	[SerializeField]
	private ItemsListWindow _itemsListWindow;

	[SerializeField]
	private WeaponPreviewPool _weaponPreviewPool;

	[SerializeField]
	private SplitDialog _splitDialogTemplate;

	[SerializeField]
	private MessageWindow _messageWindow;

	[SerializeField]
	private MessageWindow _scrolledMessageWindow;

	[SerializeField]
	private GroupInviteWindow _groupInviteWindow;

	[SerializeField]
	private RaidInviteWindow _raidInviteWindow;

	[SerializeField]
	private SimpleContextMenu _contextMenu;

	[SerializeField]
	private GridWindow _gridWindowTemplate;

	[SerializeField]
	private EditTagWindow _editTagWindowTemplate;

	[SerializeField]
	private AcceptQuestChangeWindow _acceptQuestChangeWindow;

	[SerializeField]
	private PlayersInviteWindow _playersInviteWindow;

	[SerializeField]
	private ClothingConfirmationWindow _clothingConfirmation;

	[SerializeField]
	private HandoverExchangeableItemsWindow _handoverItemsWindow;

	[SerializeField]
	private CaptchaHandler _captchaHandler;

	[CompilerGenerated]
	private Action<IEnumerable<string>> _E07C;

	[CompilerGenerated]
	private Action<IEnumerable<string>> _E07D;

	[CompilerGenerated]
	private Action<_E992> _E07E;

	[CompilerGenerated]
	private Action<bool> _E07F;

	[SerializeField]
	public Transform DragLayer;

	private readonly HashSet<_EB68> _E080 = new HashSet<_EB68>();

	private _EB69 _E081;

	private readonly Dictionary<ECommand, EBoundItem> _E082 = new Dictionary<ECommand, EBoundItem>(7, _E3A5<ECommand>.EqualityComparer)
	{
		{
			ECommand.SelectFastSlot4,
			EBoundItem.Item4
		},
		{
			ECommand.SelectFastSlot5,
			EBoundItem.Item5
		},
		{
			ECommand.SelectFastSlot6,
			EBoundItem.Item6
		},
		{
			ECommand.SelectFastSlot7,
			EBoundItem.Item7
		},
		{
			ECommand.SelectFastSlot8,
			EBoundItem.Item8
		},
		{
			ECommand.SelectFastSlot9,
			EBoundItem.Item9
		},
		{
			ECommand.SelectFastSlot0,
			EBoundItem.Item10
		}
	};

	private readonly List<_E000> _E083 = new List<_E000>();

	private _EAED _E084;

	private Profile _E085;

	private _E9C4 _E086;

	private _EA40[] _E087;

	private _ECB1 _E088;

	private _ECBD _E089;

	private _E8B2 _E08A;

	private SplitDialog _E08B;

	private ECursorResult _E08C;

	private _EC4E<EItemInfoButton> _E08D;

	[CompilerGenerated]
	private static ItemUiContext _E08E;

	[CompilerGenerated]
	private _EC53 _E08F;

	[CompilerGenerated]
	private _E796 m__E00A;

	[CompilerGenerated]
	private EItemUiContextType _E090;

	[CompilerGenerated]
	private _EB68 _E091;

	private bool _E092;

	public static ItemUiContext Instance
	{
		[CompilerGenerated]
		get
		{
			return _E08E;
		}
		[CompilerGenerated]
		private set
		{
			_E08E = value;
		}
	}

	public RectTransform ContextMenuArea => _contextMenuArea;

	public _EC53 ContextInteractionsSwitcher
	{
		[CompilerGenerated]
		get
		{
			return _E08F;
		}
		[CompilerGenerated]
		private set
		{
			_E08F = value;
		}
	}

	public _E796 Session
	{
		[CompilerGenerated]
		get
		{
			return this.m__E00A;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E00A = value;
		}
	}

	public EItemUiContextType ContextType
	{
		[CompilerGenerated]
		get
		{
			return _E090;
		}
		[CompilerGenerated]
		private set
		{
			_E090 = value;
		}
	}

	public bool SortAvailable
	{
		get
		{
			if (ContextType != EItemUiContextType.InventoryScreen)
			{
				return ContextType == EItemUiContextType.ProfileEditor;
			}
			return true;
		}
	}

	public SplitDialog SplitDialog
	{
		get
		{
			if (_E08B == null)
			{
				_E08B = UnityEngine.Object.Instantiate(_splitDialogTemplate, _infoWindowsContainer);
			}
			return _E08B;
		}
	}

	private _EB68 _E000
	{
		[CompilerGenerated]
		get
		{
			return _E091;
		}
		[CompilerGenerated]
		set
		{
			_E091 = value;
		}
	}

	public SimpleContextMenu ContextMenu => _contextMenu;

	public WeaponPreviewPool WeaponPreviewPool => _weaponPreviewPool;

	public SimpleTooltip Tooltip => _simpleTooltip;

	public ItemTooltip ItemTooltip => _itemTooltip;

	public PriceTooltip PriceTooltip => _priceTooltip;

	public ItemsListWindow ItemsListWindow => _itemsListWindow;

	public SkillTooltip SkillTooltip => _skillTooltip;

	public MasteringTooltip MasteringTooltip => _masteringTooltip;

	public MultiLineTooltip MultiLineTooltip => _multiLineTooltip;

	public HandoverExchangeableItemsWindow HandoverItemsWindow => _handoverItemsWindow;

	public bool MessageWindowActive => _messageWindow.gameObject.activeSelf;

	private bool _E001 => _scrolledMessageWindow.gameObject.activeSelf;

	private _EA40[] _E002
	{
		get
		{
			IEnumerable<_EA40> collections = _E084.Inventory.Equipment.GetCollections(_EB0B.AllSlotNames);
			IEnumerable<_EA40> enumerable = _E087;
			return collections.Concat(enumerable ?? Enumerable.Empty<_EA40>()).ToArray();
		}
	}

	public _E794 CaptchaHandler => _captchaHandler;

	public event Action<IEnumerable<string>> OnContextMenuRedraw
	{
		[CompilerGenerated]
		add
		{
			Action<IEnumerable<string>> action = _E07C;
			Action<IEnumerable<string>> action2;
			do
			{
				action2 = action;
				Action<IEnumerable<string>> value2 = (Action<IEnumerable<string>>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E07C, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<IEnumerable<string>> action = _E07C;
			Action<IEnumerable<string>> action2;
			do
			{
				action2 = action;
				Action<IEnumerable<string>> value2 = (Action<IEnumerable<string>>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E07C, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<IEnumerable<string>> OnContextMenuClose
	{
		[CompilerGenerated]
		add
		{
			Action<IEnumerable<string>> action = _E07D;
			Action<IEnumerable<string>> action2;
			do
			{
				action2 = action;
				Action<IEnumerable<string>> value2 = (Action<IEnumerable<string>>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E07D, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<IEnumerable<string>> action = _E07D;
			Action<IEnumerable<string>> action2;
			do
			{
				action2 = action;
				Action<IEnumerable<string>> value2 = (Action<IEnumerable<string>>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E07D, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<_E992> OnEffectsChange
	{
		[CompilerGenerated]
		add
		{
			Action<_E992> action = _E07E;
			Action<_E992> action2;
			do
			{
				action2 = action;
				Action<_E992> value2 = (Action<_E992>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E07E, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<_E992> action = _E07E;
			Action<_E992> action2;
			do
			{
				action2 = action;
				Action<_E992> value2 = (Action<_E992>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E07E, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<bool> OnInventoryLocked
	{
		[CompilerGenerated]
		add
		{
			Action<bool> action = _E07F;
			Action<bool> action2;
			do
			{
				action2 = action;
				Action<bool> value2 = (Action<bool>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E07F, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<bool> action = _E07F;
			Action<bool> action2;
			do
			{
				action2 = action;
				Action<bool> value2 = (Action<bool>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E07F, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public void Inspect(_EB68 itemContext, _EC4E<EItemInfoButton> contextInteractions)
	{
		if (itemContext.ViewType != EItemViewType.Empty)
		{
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuInspectorWindowOpen);
			if (contextInteractions == null)
			{
				contextInteractions = GetItemContextInteractions(itemContext, null);
			}
			_E00C(itemContext, _infoWindowTemplate, delegate(InfoWindow window, Action setPriority, Action onClosed)
			{
				window.Show(itemContext, _E084, _E08A, setPriority, this, onClosed, contextInteractions);
			});
		}
	}

	public Dictionary<string, int> FindCompatibleAmmo(_EA6A magazine)
	{
		List<Item> list = new List<Item>();
		_E084.Inventory.Stash.GetAllAssembledItemsNonAlloc(list);
		_E084.Inventory.Equipment.GetAllAssembledItemsNonAlloc(list);
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		foreach (Item item in list)
		{
			if (item is _EA12 obj && magazine.CheckCompatibility(obj) && !(obj.Parent.Container is Slot))
			{
				string templateId = obj.TemplateId;
				if (dictionary.TryGetValue(templateId, out var value))
				{
					dictionary[templateId] = value + obj.StackObjectsCount;
				}
				else
				{
					dictionary.Add(templateId, obj.StackObjectsCount);
				}
			}
		}
		return dictionary;
	}

	public async Task LoadAmmoByType(_EA6A magazine, string ammoTemplateId, Action callback = null)
	{
		_E002 obj = new _E002();
		obj.ammoTemplateId = ammoTemplateId;
		obj.magazine = magazine;
		obj._003C_003E4__this = this;
		obj.ammo = new List<_EA12>();
		if (!(await obj._E000(_E084.Inventory.Stash)))
		{
			await obj._E000(_E084.Inventory.Equipment);
		}
		callback?.Invoke();
		if (Singleton<GUISounds>.Instantiated)
		{
			Singleton<GUISounds>.Instance.PlayUILoadSound();
		}
	}

	public void CheckMagazine(_EA6A magazine)
	{
		_E084.InventoryCheckMagazine(magazine, notify: true);
	}

	public async void TopUpItem(Item targetItem)
	{
		_E004 CS_0024_003C_003E8__locals0 = new _E004();
		CS_0024_003C_003E8__locals0.targetItem = targetItem;
		CS_0024_003C_003E8__locals0._003C_003E4__this = this;
		if (CS_0024_003C_003E8__locals0.targetItem.StackMaxSize < 2)
		{
			throw new ArgumentException(CS_0024_003C_003E8__locals0.targetItem.Name + _ED3E._E000(246583));
		}
		if (CS_0024_003C_003E8__locals0._E000())
		{
			throw new ArgumentException(CS_0024_003C_003E8__locals0.targetItem.Name + _ED3E._E000(246613));
		}
		CS_0024_003C_003E8__locals0.currentSource = null;
		CS_0024_003C_003E8__locals0.inventoryChanged = false;
		_E084.RefreshItemEvent += delegate(_EAFF args)
		{
			CS_0024_003C_003E8__locals0.inventoryChanged |= args.Item != CS_0024_003C_003E8__locals0.targetItem && args.Item != CS_0024_003C_003E8__locals0.currentSource;
		};
		_E084.RemoveItemEvent += delegate(_EAF3 args)
		{
			if (args.Status == CommandStatus.Succeed)
			{
				CS_0024_003C_003E8__locals0.inventoryChanged |= args.Item != CS_0024_003C_003E8__locals0.targetItem && args.Item != CS_0024_003C_003E8__locals0.currentSource;
			}
		};
		CS_0024_003C_003E8__locals0.sourcesBuffer = new List<Item>();
		await CS_0024_003C_003E8__locals0._E001(_E084.Inventory.Stash);
		await CS_0024_003C_003E8__locals0._E001(_E084.Inventory.Equipment);
		_E084.RefreshItemEvent -= delegate(_EAFF args)
		{
			CS_0024_003C_003E8__locals0.inventoryChanged |= args.Item != CS_0024_003C_003E8__locals0.targetItem && args.Item != CS_0024_003C_003E8__locals0.currentSource;
		};
		_E084.RemoveItemEvent -= delegate(_EAF3 args)
		{
			if (args.Status == CommandStatus.Succeed)
			{
				CS_0024_003C_003E8__locals0.inventoryChanged |= args.Item != CS_0024_003C_003E8__locals0.targetItem && args.Item != CS_0024_003C_003E8__locals0.currentSource;
			}
		};
	}

	public async Task<IResult> UnpackItem(Item targetItem)
	{
		return await Session.Unpack(targetItem.Id, _E085.Id);
	}

	public bool HasSourcesForTopUp(Item targetItem)
	{
		_E007 obj = new _E007();
		obj.targetItem = targetItem;
		obj.itemsBuffer = new List<Item>();
		if (!obj._E000(_E084.Inventory.Stash))
		{
			return obj._E000(_E084.Inventory.Equipment);
		}
		return true;
	}

	public void Examine(Item item, Action callback = null)
	{
		_E084.Examine(item, delegate(IResult result)
		{
			if (result.Succeed)
			{
				_E857.DisplayMessageNotification(_ED3E._E000(246598).Localized() + item.ShortName.Localized() + _ED3E._E000(246646) + item.ExamineExperience + _ED3E._E000(246642));
			}
			callback?.Invoke();
		});
	}

	public void ExternalRagfairSearch(_ECC4 ragfairSearch)
	{
		Session.RagFair.ExternalRagfairSearch(ragfairSearch);
	}

	public bool IsInWishList(string templateId)
	{
		return Session.RagFair.IsInWishList(templateId);
	}

	public void AddToWishList(Item item, Action callback)
	{
		_E089.AddToWishList(item.TemplateId, callback);
	}

	public void RemoveFromWishList(Item item, Action callback)
	{
		_E089.RemoveFromWishList(item.TemplateId, callback);
	}

	public void EditBuild(Weapon weapon)
	{
		if (!_E7A3.InRaid)
		{
			new EditBuildScreen._E000(weapon, _E084, Session).ShowScreen(EScreenState.Queued);
		}
	}

	public void Insure(_ECB4 item)
	{
		_E088.AddItemToInsuranceQueue(item);
	}

	public void Uncover(_ECB4 item)
	{
		_E088.RemoveItemFromInsuranceQueue(item);
	}

	public void ModWeapon(Item item)
	{
		if (!_E7A3.InRaid)
		{
			new WeaponModdingScreen._E000(item, _E084, this._E002).ShowScreen(EScreenState.Queued);
		}
	}

	public void EditTag(_EB68 itemContext, TagComponent tagComponent)
	{
		if (_editTagWindowTemplate != null)
		{
			_E00C(itemContext, _editTagWindowTemplate, delegate(EditTagWindow window, Action onSelect, Action onClose)
			{
				window.Show(tagComponent, onSelect, onClose, delegate(string tagName, int tagColor)
				{
					_E084.TryRunNetworkTransaction(tagComponent.Set(tagName, tagColor, simulate: true));
				});
			});
		}
		else
		{
			Debug.LogWarning(_ED3E._E000(246184));
		}
	}

	public void ResetTag(TagComponent tagComponent)
	{
		_E084.TryRunNetworkTransaction(tagComponent.Set(string.Empty, 0, simulate: true));
	}

	public void ToggleItem(TogglableComponent togglable)
	{
		_E084.TryRunNetworkTransaction(togglable.Set(!togglable.On, simulate: true));
	}

	public async Task ThrowItem(Item item)
	{
		if (!_E084.CanThrow(item))
		{
			return;
		}
		string id = (_E084.Examined(item) ? item.ShortName : _ED3E._E000(193009));
		string description = _ED3E._E000(246632).Localized() + _ED3E._E000(18502) + id.Localized() + _ED3E._E000(246665);
		if (await ShowMessageWindow(out var _, description))
		{
			_ECD9<bool> obj = _E084.TryThrowItem(item, null, silent: true);
			bool flag = obj.Failed;
			IEnumerable<_EAE9> destroyedItems = default(IEnumerable<_EAE9>);
			if (flag)
			{
				flag = await obj.Error.TryShowDestroyItemsDialog(out destroyedItems);
			}
			if (flag)
			{
				_E084.ThrowItem(item, destroyedItems);
			}
		}
	}

	private void _E000(Item item, EBoundItem bindIndex)
	{
		if (item.Owner == _E084)
		{
			_E084.TryRunNetworkTransaction(_EB4F.Run(_E084, this._E000.Item, bindIndex, simulate: true));
		}
	}

	public void OpenInsuranceWindow(_EB68 itemContext)
	{
		if (!(_insuranceWindowTemplate == null))
		{
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuInspectorWindowOpen);
			_E00C(itemContext, _insuranceWindowTemplate, delegate(InsuranceWindow window, Action setPriority, Action onClosed)
			{
				window.Show(itemContext.Item, _E085.Inventory, _E084, Session.InsuranceCompany, _messageWindow, _E085.Skills, setPriority, WeaponPreviewPool, onClosed);
			});
		}
	}

	public void OpenRepairWindow(_E8D0 repairController, _EB68 itemContext)
	{
		Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuInspectorWindowOpen);
		_E00C(itemContext, _repairWindowTemplate, delegate(RepairWindow window, Action setPriority, Action onClosed)
		{
			window.Show(repairController, itemContext, _E085.Inventory, _messageWindow, _E084, Session.Repairers, _E085.Skills, setPriority, WeaponPreviewPool, onClosed);
		});
	}

	public void UseItem(Item item)
	{
		if (item == null)
		{
			return;
		}
		if (!(item is _EA48 obj))
		{
			if (item is _EA72 obj2)
			{
				_EA72 item2 = obj2;
				_E086.ApplyItem(item2, EBodyPart.Common);
			}
		}
		else
		{
			_EA48 foodDrink = obj;
			_E001(_E086, foodDrink);
		}
	}

	public void UseAll(Item item)
	{
		if (item is _EA48 item2)
		{
			_E086.ApplyItem(item2, EBodyPart.Common);
		}
	}

	public void HealAll(_EA72 meds)
	{
		while (_E084.Inventory.AllPlayerItems.Contains(meds) && _E086.ApplyItem(meds, EBodyPart.Common))
		{
		}
	}

	public async Task Uninstall(Item item, bool putInStash = false)
	{
		await RunWithSound(_E084, item, QuickFindAppropriatePlace(item, _E084, putInStash));
	}

	private void _E001(_E9C4 healthController, _EA48 foodDrink)
	{
		int maxValue = (int)foodDrink.FoodDrinkComponent.MaxResource;
		int minValue = (int)((float)maxValue - foodDrink.FoodDrinkComponent.HpPercent + 1f);
		SplitDialog.Show(_ED3E._E000(246217).Localized().ToUpper(), 1, maxValue, minValue, maxValue, maxValue, Input.mousePosition, delegate(int amount)
		{
			SplitDialog.Hide();
			healthController.ApplyItem(foodDrink, EBodyPart.Common, (float)amount / (float)maxValue);
		}, delegate
		{
			SplitDialog.Hide();
		}, SplitDialog.ESplitDialogType.Int);
	}

	public async Task EquipItem(Item item)
	{
		_EB20 obj = _E084.FindSlotToPickUp(item);
		if (obj != null)
		{
			await RunWithSound(_E084, item, _EB29.Move(item, obj, _E084, simulate: true));
		}
		else
		{
			_E857.DisplayWarningNotification(_ED3E._E000(246295).Localized());
		}
	}

	public void FoldItem(Item item)
	{
		EItemUiContextType contextType = ContextType;
		if ((uint)(contextType - 4) > 1u && _EB29.CanFold(item, out var foldable))
		{
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuStock);
			_E084.TryRunNetworkTransaction(_EB29.Fold(foldable, !foldable.Folded, _E084.ID, simulate: true));
		}
	}

	public void RecodeItem(Item item)
	{
		if (_EB29.CanRecode(item, out var recodable))
		{
			_E084.TryRunNetworkTransaction(_EB29.Recode(recodable, !recodable.IsEncoded, _E084.ID, simulate: true));
		}
	}

	public void OpenItem(_EA40 item, _EB68 itemContext, bool searchButtonDisplay = true)
	{
		if (item is _EA66 obj && obj.Lockable.Locked)
		{
			_E857.DisplayWarningNotification(_ED3E._E000(246213).Localized());
			return;
		}
		AudioClip itemClip = Singleton<GUISounds>.Instance.GetItemClip(item.ItemSound, EInventorySoundType.open);
		if (itemClip != null)
		{
			Singleton<GUISounds>.Instance.PlaySound(itemClip);
		}
		else
		{
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuOpenContainer);
		}
		_E002(itemContext, delegate(GridWindow window, Action setPriority, Action onClosed)
		{
			window.Show(item, itemContext, _E084, setPriority, onClosed, this, searchButtonDisplay);
		});
	}

	private void _E002(_EB68 itemContext, Action<GridWindow, Action, Action> showAction)
	{
		if (_gridWindowTemplate != null)
		{
			_E00C(itemContext, _gridWindowTemplate, showAction);
		}
		else
		{
			Debug.LogWarning(_ED3E._E000(246257));
		}
	}

	private bool _E003(Weapon weapon)
	{
		if (weapon.MalfState.State != Weapon.EMalfunctionState.Feed)
		{
			return false;
		}
		if (!_E084.HasKnownMalfunction(weapon))
		{
			_E084.ExamineMalfunction(weapon);
		}
		return true;
	}

	public async Task UnloadWeapon(Weapon weapon)
	{
		_EA6A currentMagazine = weapon.GetCurrentMagazine();
		if (currentMagazine == null || _E003(weapon))
		{
			return;
		}
		_EB0B equipment = _E084.Inventory.Equipment;
		bool flag = equipment.Contains(currentMagazine);
		if (!flag && _E087 == null)
		{
			Debug.LogError(_ED3E._E000(246660));
			return;
		}
		object obj;
		if (_E087 != null)
		{
			obj = (flag ? equipment.ToEnumerable().Concat(_E087) : _E087.Concat(equipment.ToEnumerable()));
		}
		else
		{
			IEnumerable<_EA40> enumerable = equipment.ToEnumerable();
			obj = enumerable;
		}
		IEnumerable<_EA40> targets = (IEnumerable<_EA40>)obj;
		_ECD8<_EB2E> obj2 = _EB29.QuickFindAppropriatePlace(currentMagazine, _E084, targets, _EB29.EMoveItemOrder.UnloadWeapon, simulate: true);
		bool flag2 = obj2.Succeeded;
		if (flag2)
		{
			flag2 = (await RunWithSound(_E084, currentMagazine, obj2)).Succeed;
		}
		if (!flag2)
		{
			if (!_E7A3.InRaid)
			{
				_E857.DisplayWarningNotification(_ED3E._E000(246295).Localized());
			}
			else if (_E084.CanThrow(currentMagazine))
			{
				_E084.ThrowItem(currentMagazine, null, null, downDirection: true);
			}
		}
	}

	public void ReloadWeapon(Weapon weapon, IEnumerable<_EA40> collections)
	{
		Slot magazineSlot = weapon.GetMagazineSlot();
		if (_E003(weapon))
		{
			return;
		}
		_EA6A foundMagazine = _E005(magazineSlot, collections);
		if (foundMagazine != null)
		{
			_EA6A currentMagazine = weapon.GetCurrentMagazine();
			if (currentMagazine != null && !_E084.Examined(currentMagazine))
			{
				_E857.DisplaySingletonWarningNotification(_ED3E._E000(161844).Localized());
			}
			else if (currentMagazine != null)
			{
				_EB22 obj = (from grid in _E084.Inventory.Equipment.GetPrioritizedGridsForUnloadedObject(backpackIncluded: true)
					select grid.FindLocationForItem(currentMagazine) into g
					where g != null
					orderby g.Grid.GridWidth.Value * g.Grid.GridHeight.Value
					select g).FirstOrDefault();
				if (obj != null)
				{
					_E084.TryRunNetworkTransaction(_EB29.Swap(currentMagazine, obj, foundMagazine, new _EB20(magazineSlot), _E084, simulate: true));
					return;
				}
				if (!_E7A3.InRaid)
				{
					_E857.DisplayWarningNotification(_ED3E._E000(246295).Localized());
					return;
				}
				_E084.ThrowItem(currentMagazine, null, delegate
				{
					_E084.TryRunNetworkTransaction(_EB29.Move(foundMagazine, new _EB20(magazineSlot), _E084, simulate: true));
				});
			}
			else
			{
				_E084.TryRunNetworkTransaction(_EB29.Move(foundMagazine, new _EB20(magazineSlot), _E084, simulate: true));
			}
		}
		else
		{
			_E857.DisplayWarningNotification(_ED3E._E000(246331).Localized());
		}
	}

	public async Task LoadWeapon(Weapon weapon, _EA40[] collections)
	{
		if (weapon.GetCurrentMagazine() == null)
		{
			Slot magazineSlot = weapon.GetMagazineSlot();
			_EA6A obj = _E005(magazineSlot, collections);
			if (obj == null)
			{
				_E857.DisplayWarningNotification(_ED3E._E000(246783).Localized());
				return;
			}
			_EB20 to = new _EB20(magazineSlot);
			await RunWithSound(_E084, obj, _EB29.Move(obj, to, _E084, simulate: true));
		}
	}

	public async Task InstallMod(Mod mod, _EA40[] collections)
	{
		if (mod == null)
		{
			throw new Exception(_ED3E._E000(252274));
		}
		IEnumerable<Slot> source = (from weapon in mod.GetSuitableWeapons(collections)
			where _E084.Examined(weapon)
			select weapon).SelectMany((Weapon weapon) => weapon.AllSlots);
		foreach (Slot item in source.Where((Slot slot) => slot.CanAccept(mod)))
		{
			if (item.ContainedItem == mod)
			{
				return;
			}
			_EB20 obj = new _EB20(item);
			if (item.ContainedItem is Mod mod2)
			{
				if (QuickFindAppropriatePlace(mod2, _E084).Succeeded)
				{
					_EB22 obj2 = ((_E084.Inventory.Stash != null) ? _E084.Inventory.Stash.Grids : _E084.Inventory.Equipment.GetPrioritizedGridsForLoot(mod2).ToArray()).FindLocationForItem(mod2);
					bool flag = obj2 != null;
					if (flag)
					{
						flag = (await _E084.TryRunNetworkTransaction(_EB29.Swap(mod, obj, mod2, obj2, _E084, simulate: true))).Succeed;
					}
					if (flag)
					{
						return;
					}
				}
			}
			else if ((await RunWithSound(_E084, mod, _EB29.Move(mod, obj, _E084, simulate: true))).Succeed)
			{
				return;
			}
		}
		_E857.DisplayWarningNotification(_ED3E._E000(248859).Localized());
	}

	public async Task UnloadAmmo(Item item)
	{
		if (item == null)
		{
			return;
		}
		IResult result;
		if (!(item is Weapon weapon))
		{
			if (!(item is _EA6A obj))
			{
				if (!(item is AmmoBox ammoBox))
				{
					return;
				}
				AmmoBox ammoContainer = ammoBox;
				result = await _E084.UnloadAmmoInstantly(ammoContainer);
			}
			else
			{
				_EA6A magazine = obj;
				result = await _E084.UnloadMagazine(magazine);
			}
		}
		else
		{
			Weapon weapon2 = weapon;
			result = await _E084.UnloadMagazine(weapon2.GetCurrentMagazine());
		}
		if (result.Failed)
		{
			_E857.DisplayWarningNotification(result.Error.ToString());
		}
	}

	public async Task Disassemble(Item item)
	{
		List<_ECD7> list = _E004(item, simulate: true);
		foreach (_ECD7 item2 in list)
		{
			if (!item2.Failed)
			{
				await _E084.TryRunNetworkTransaction(item2);
			}
		}
	}

	private List<_ECD7> _E004(Item item, bool simulate)
	{
		Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuWeaponDisassemble);
		Weapon obj = (item as Weapon) ?? throw new ArgumentException(_ED3E._E000(246357));
		List<_ECD7> list = new List<_ECD7>();
		foreach (Mod item3 in obj.Mods.Where((Mod x) => _E084.Examined(x)))
		{
			_ECD7 item2 = QuickFindAppropriatePlace(item3, _E084, !_E7A3.InRaid, displayWarnings: false, simulate: false);
			if (item2.Succeeded)
			{
				list.Add(item2);
			}
		}
		if (simulate)
		{
			list.RollBack();
		}
		return list;
	}

	public async Task QuickEquip(Item item)
	{
		if (_E084.IsItemEquipped(item))
		{
			_E857.DisplayWarningNotification(_ED3E._E000(248895).Localized());
			return;
		}
		_EB20 obj = _E084.FindSlotToPickUp(item);
		if (obj != null)
		{
			await RunWithSound(_E084, item, _EB29.Move(item, obj, _E084, simulate: true));
		}
		else
		{
			_E857.DisplayWarningNotification(_ED3E._E000(248864).Localized());
		}
	}

	public static async Task<IResult> RunWithSound(_EB1E itemController, Item item, _ECD7 operationResult, [CanBeNull] Callback callback = null)
	{
		IResult obj = await itemController.TryRunNetworkTransaction(operationResult, callback);
		if (obj.Succeed)
		{
			PlayOperationSound(item, operationResult.Value);
		}
		return obj;
	}

	public static void PlayOperationSound(Item item, _EB2D operationResult)
	{
		ItemAddress itemAddress = ((operationResult is _EB3B obj) ? obj.To : ((operationResult is _EB47 obj2) ? obj2.To : ((operationResult is _EB3D obj3) ? obj3.TargetItem.Parent : ((operationResult is _EB49 obj4) ? obj4.TargetItem.Parent : null))));
		Item item2 = ((itemAddress is _EB20 obj5) ? obj5.Slot.ParentItem : ((itemAddress is _EB21 obj6) ? obj6.StackSlot.ParentItem : null));
		if (item2 is Weapon && item is Mod)
		{
			if (item is _EA6A)
			{
				Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuInstallMag);
				return;
			}
			EModClass modClass = ItemViewFactory.GetModClass(item.GetType());
			if (Singleton<GUISounds>.Instance.PlayItemSound(modClass))
			{
				return;
			}
		}
		Singleton<GUISounds>.Instance.PlayItemSound(item.ItemSound, (item2 is _EB0B || item2 is _EA6A) ? EInventorySoundType.use : EInventorySoundType.drop);
	}

	public _ECD7 QuickFindAppropriatePlace(Item item, _EB1E controller, bool forcePutInStash = false, bool displayWarnings = true, bool simulate = true)
	{
		_EB29.EMoveItemOrder eMoveItemOrder = _EB29.EMoveItemOrder.MoveToAnotherSide;
		IEnumerable<_EA40> enumerable;
		if (item.Parent.GetOwner().OwnerType == EOwnerType.Mail || forcePutInStash)
		{
			enumerable = (controller.Root ?? _E084.Root).ToEnumerable();
			eMoveItemOrder |= _EB29.EMoveItemOrder.IgnoreItemParent;
			goto IL_00d0;
		}
		_EB0B equipment = _E084.Inventory.Equipment;
		_EA98 sortingTable = _E084.Inventory.SortingTable;
		int num;
		if (!_E087.IsNullOrEmpty())
		{
			if (item.IsChildOf(equipment))
			{
				num = 1;
				goto IL_00a7;
			}
			if (sortingTable != null)
			{
				num = (item.IsChildOf(sortingTable) ? 1 : 0);
				if (num != 0)
				{
					goto IL_00a7;
				}
			}
			else
			{
				num = 0;
			}
		}
		else
		{
			num = 0;
		}
		IEnumerable<_EA40> enumerable2 = equipment.ToEnumerable();
		IEnumerable<_EA40> enumerable3 = enumerable2;
		goto IL_00b7;
		IL_00d0:
		_ECD8<_EB2E> obj = _EB29.QuickFindAppropriatePlace(item, controller, enumerable, eMoveItemOrder, simulate);
		if (obj.Failed && displayWarnings)
		{
			string text = ((obj.Error is InventoryError inventoryError) ? inventoryError.GetLocalizedDescription() : obj.Error.ToString());
			Debug.LogWarningFormat(_ED3E._E000(246385), item, text);
			_E857.DisplayWarningNotification(text.Localized());
		}
		return obj;
		IL_00b7:
		enumerable = enumerable3;
		if (num != 0 && equipment.Contains(item))
		{
			enumerable = enumerable.Concat(equipment.ToEnumerable());
		}
		goto IL_00d0;
		IL_00a7:
		enumerable3 = _E087.First().ToEnumerable();
		goto IL_00b7;
	}

	public _ECD7 QuickMoveToSortingTable(Item item, bool simulate = true)
	{
		_EA98 sortingTable = _E084.Inventory.SortingTable;
		if (sortingTable == null)
		{
			return new _ECD2(_ED3E._E000(246429));
		}
		if (!sortingTable.IsVisible)
		{
			return new _ECD2(_ED3E._E000(246462));
		}
		_EB22 obj = sortingTable.Grid.FindLocationForItem(item);
		if (obj == null)
		{
			return new _E9FA(item);
		}
		return _EB29.Move(item, obj, _E084, simulate);
	}

	private _EA6A _E005(Slot magazineSlot, IEnumerable<_EA40> collections)
	{
		return (_E7A3.InRaid ? _E084.GetReachableItemsOfType<_EA6A>() : collections.GetTopLevelItems().OfType<_EA6A>()).OrderByDescending((_EA6A mag) => mag.Count).FirstOrDefault((_EA6A mag) => mag.Count > 0 && !(mag.GetRootItem() is Weapon) && _E084.Examined(mag) && magazineSlot.CanAccept(mag));
	}

	public _EC4E<EItemInfoButton> GetItemContextInteractions(_EB68 itemContext, Action closeAction)
	{
		closeAction = closeAction ?? new Action(ContextMenu.Close);
		switch ((int)itemContext.ViewType)
		{
		case 3:
			return new _EC52(itemContext, this, this._E002, closeAction);
		case 8:
			return new _EC5B(itemContext, this, closeAction);
		case 9:
			return new _EC5C(itemContext, this, closeAction);
		case 13:
			if (!(itemContext is _ECB2 itemContext2))
			{
				break;
			}
			return new _ECB3(_E088, itemContext2, this, _E084, closeAction);
		case 4:
			return new _EC5A(itemContext, this, closeAction);
		case 5:
			return new _EC59(itemContext, this, closeAction);
		case 6:
			return new _EC58(itemContext, this, closeAction);
		case 2:
		case 14:
			return new _EC51(itemContext, this, closeAction);
		case 18:
			return new _EC55(itemContext, this, closeAction);
		case 10:
		case 12:
			return new _ECBF(_E089, itemContext, this, closeAction);
		case 15:
			return new _EC57(itemContext, this, closeAction);
		case 1:
		case 7:
		case 11:
		case 16:
		case 19:
			return new _EC4F();
		}
		throw new ArgumentOutOfRangeException();
	}

	public ItemView CreateSlotItemView(Item item, _EB68 sourceContext, _EAED inventoryController, IItemOwner itemOwner, _E74F skills)
	{
		itemOwner = itemOwner ?? item.Owner;
		EItemUiContextType contextType = ContextType;
		if (contextType == EItemUiContextType.InsuranceScreen)
		{
			return InsuranceSlotItemView.Create(item, sourceContext, inventoryController, itemOwner, this, skills, _E088);
		}
		return SlotItemView.Create(item, sourceContext, inventoryController, itemOwner, this, skills, _E088);
	}

	public ItemView CreateItemView(Item item, _EB68 sourceContext, ItemRotation rotation, _EB1E itemController, IItemOwner itemOwner, FilterPanel filterPanel, _EC9E container, bool slotView, bool canSelect, bool searched)
	{
		EItemViewType viewType = sourceContext.ViewType;
		itemOwner = itemOwner ?? item.Owner;
		if (sourceContext is _EB65 sourceContext2 && !slotView)
		{
			return ModdingSelectableItemView.Create(item, sourceContext2, rotation, itemController, itemOwner, filterPanel, container, this, _E088, canSelect);
		}
		if (sourceContext is _EB66 obj)
		{
			switch ((int)viewType)
			{
			case 10:
				return RagfairSelectableItemView.Create(item, obj, rotation, itemController, itemOwner, filterPanel, container, this, _E088);
			case 14:
			case 15:
				return SelectableItemView.Create(item, obj, rotation, itemController, itemOwner, filterPanel, container, this, _E088);
			case 11:
				return HandoverItemView.Create(item, obj, rotation, itemController, itemOwner, filterPanel, container, this, _E088);
			case 16:
				return CaptchaGridItemView.Create(item, obj, rotation, itemController, itemOwner, this);
			case 19:
				return DropdownSelectableItemView.Create(item, obj, rotation, itemController, itemOwner, filterPanel, container, this, _E088);
			case 18:
				if (slotView)
				{
					break;
				}
				goto default;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
		switch (viewType)
		{
		case EItemViewType.Inventory:
		case EItemViewType.ScavInventory:
		case EItemViewType.TransferPlayer:
		case EItemViewType.TransferTrader:
		case EItemViewType.Ragfair:
			return GridItemView.Create(item, sourceContext, rotation, itemController, itemOwner, filterPanel, container, this, _E088, searched);
		case EItemViewType.NewOffer:
			return RagfairNewOfferItemView.Create(item, (_ECA2)sourceContext, rotation, itemController, itemOwner, filterPanel, container, this, _E088, canSelect);
		case EItemViewType.Insurance:
			return InsuranceItemView.Create(item, sourceContext, rotation, itemController, itemOwner, filterPanel, container, this, _E088);
		case EItemViewType.TradingPlayer:
			return TradingPlayerItemView.Create(item, sourceContext, rotation, _E08A, itemController, itemOwner, filterPanel, container, ((_ECA3)sourceContext).TraderMode, canSelect: false, canDrag: true, ETradingItemViewType.Person, this, _E088, slotView);
		case EItemViewType.TradingTrader:
			return TradingItemView.Create(item, sourceContext, rotation, _E08A, itemController, itemOwner, filterPanel, container, ((_ECA3)sourceContext).TraderMode, canSelect, !slotView, ETradingItemViewType.Person, this, _E088, slotView);
		case EItemViewType.TradingSell:
			return TradingPlayerItemView.Create(item, sourceContext, rotation, _E08A, itemController, itemOwner, filterPanel, container, TraderDealScreen.ETraderMode.Sale, canSelect: false, canDrag: false, ETradingItemViewType.TradingTable, this, _E088, slotView);
		case EItemViewType.Handbook:
		case EItemViewType.Hideout:
		case EItemViewType.WeaponModdingInteractable:
			return HideoutItemView.Create(item, sourceContext, rotation, itemController, this, _E088, slotView);
		case EItemViewType.Quest:
			return QuestItemView.Create(item, sourceContext, rotation, itemController, itemOwner, filterPanel, container, this, _E088);
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	public static void Init(ItemUiContext instance)
	{
		Instance = instance;
	}

	private void _E006()
	{
		_simpleTooltip.Close();
		_itemTooltip.Close();
		_priceTooltip.Close();
		_itemsListWindow.Close();
		_skillTooltip.Close();
		_masteringTooltip.Close();
		_multiLineTooltip.Close();
	}

	public void Configure(_EAED inventoryController, Profile profile, _E796 backEndSession, _ECB1 insurance, [CanBeNull] _ECBD ragfair, [CanBeNull] _E8B2 trader, _E9C4 healthController, [CanBeNull] _EA40[] rightPanelItems, EItemUiContextType contextType, ECursorResult contextCursor)
	{
		ShowGameObject();
		if (_E084 != null)
		{
			_E084.ActiveEventsChanged -= _E009;
			_E084.OnLockedStatusChange -= _E00B;
		}
		if (_E086 != null)
		{
			_E086.EffectStartedEvent -= _E00A;
			_E086.EffectResidualEvent -= _E00A;
		}
		Session = backEndSession;
		_E088 = insurance;
		_E089 = ragfair;
		_E084 = inventoryController;
		_E085 = profile;
		_E08A = trader;
		_E086 = healthController;
		_E087 = rightPanelItems;
		ContextType = contextType;
		_E08C = contextCursor;
		ContextInteractionsSwitcher = new _EC53(_E084, _E086, _E088, _E089, this);
		if (_E084 != null)
		{
			_E084.ActiveEventsChanged += _E009;
			_E084.OnLockedStatusChange += _E00B;
		}
		if (_E086 != null)
		{
			_E086.EffectStartedEvent += _E00A;
			_E086.EffectResidualEvent += _E00A;
		}
		_captchaHandler.Init(Session);
	}

	public void RedrawContextMenus(IEnumerable<string> templateIds)
	{
		_E07C?.Invoke(templateIds);
	}

	public void ShowContextMenu(_EB68 itemContext, Vector2 position)
	{
		if (_E084.SearchOperations.Count <= 0)
		{
			_EA91 obj = itemContext.Item as _EA91;
			if (obj == null || obj.SearchOperations.Count <= 0)
			{
				_E08D = GetItemContextInteractions(itemContext, null);
				ContextMenu.Show(position, _E08D, null, itemContext.Item);
				return;
			}
		}
		_E857.DisplayWarningNotification(_ED3E._E000(246435).Localized());
	}

	public void InitSpecificationPanel(ItemSpecificationPanel panel, _EB68 itemContext, _EC4E<EItemInfoButton> contextInteractions)
	{
		panel.Show(itemContext, _E085, _E084, this._E002, _E085.Skills, WeaponPreviewPool, this, contextInteractions, _simpleTooltip);
	}

	public Task<bool> ShowMessageWindow(out _EC7B windowContext, string description, string caption = null, bool forceShow = false)
	{
		TaskCompletionSource<bool> taskSource = new TaskCompletionSource<bool>();
		windowContext = _E007(_messageWindow, description, delegate
		{
			taskSource.SetResult(result: true);
		}, delegate
		{
			taskSource.SetResult(result: false);
		}, caption, 0f, forceShow);
		return taskSource.Task;
	}

	public _EC7B ShowMessageWindow(string description, [CanBeNull] Action acceptAction, Action cancelAction = null, string caption = null, float time = 0f, bool forceShow = false, TextAlignmentOptions alignment = TextAlignmentOptions.Center)
	{
		_messageWindow.MessageAlignment = alignment;
		return _E007(_messageWindow, description, acceptAction, cancelAction, caption, time, forceShow);
	}

	public _EC7B ShowInviteInGroupWindow(_E551 groupInvite)
	{
		_E07D?.Invoke(null);
		return _groupInviteWindow.Show(groupInvite);
	}

	public _EC7B ShowInviteInRaidWindow(_E796 session, RaidSettings raidSettings, _EB61 inventoryController, _E550 player)
	{
		_E07D?.Invoke(null);
		return _raidInviteWindow.Show(session, player, raidSettings, inventoryController);
	}

	public Task<bool> ShowScrolledMessageWindow(out _EC7B windowContext, string description, string caption = null, bool forceShow = false)
	{
		TaskCompletionSource<bool> taskSource = new TaskCompletionSource<bool>();
		windowContext = _E007(_scrolledMessageWindow, description, delegate
		{
			taskSource.SetResult(result: true);
		}, delegate
		{
			taskSource.SetResult(result: false);
		}, caption, 0f, forceShow);
		return taskSource.Task;
	}

	public _EC7B ShowScrolledMessageWindow(string description, [CanBeNull] Action acceptAction, Action cancelAction = null, string caption = null, float time = 0f, bool forceShow = false)
	{
		return _E007(_scrolledMessageWindow, description, acceptAction, cancelAction, caption, time, forceShow);
	}

	private _EC7B _E007(MessageWindow windowTemplate, string description, [CanBeNull] Action acceptAction, Action cancelAction = null, string caption = null, float time = 0f, bool forceShow = false)
	{
		if (MessageWindowActive)
		{
			_messageWindow.Decline();
		}
		if (this._E001)
		{
			_scrolledMessageWindow.Decline();
		}
		if (_E7A3.InRaid && !forceShow)
		{
			acceptAction?.Invoke();
			_EC7B obj = new _EC7B();
			obj.Accept();
			return obj;
		}
		_E07D?.Invoke(null);
		return windowTemplate.Show(caption ?? _ED3E._E000(250339).Localized(), description, acceptAction, cancelAction, time);
	}

	public void RegisterView(_EB68 itemContext)
	{
		if (_E092)
		{
			throw new Exception(_ED3E._E000(246523));
		}
		if (itemContext is _EB69 obj)
		{
			_E081 = obj;
			_E008();
			return;
		}
		_E080.Add(itemContext);
		if (_E081 == null)
		{
			return;
		}
		try
		{
			itemContext.CheckAccept(_E081);
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}

	private void Update()
	{
		if (_E081 != null && (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt) || Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl) || Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift) || Input.GetKeyUp(KeyCode.LeftAlt) || Input.GetKeyUp(KeyCode.RightAlt)))
		{
			_EB69 obj = _E081;
			_E081 = null;
			_E008();
			_E081 = obj;
			_E008();
		}
	}

	public void UnregisterView(_EB68 itemContext)
	{
		if (_E092)
		{
			throw new Exception(_ED3E._E000(246523));
		}
		if (itemContext is _EB69 obj && _E081 == obj)
		{
			_E081 = null;
			_E008();
		}
		else
		{
			_E080.Remove(itemContext);
		}
	}

	private void _E008()
	{
		_E092 = true;
		foreach (_EB68 item in _E080)
		{
			try
			{
				item.CheckAccept(_E081);
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
		}
		_E092 = false;
	}

	private void _E009(_EAF1 activeEvent)
	{
		if (activeEvent is _EAF3)
		{
			_E07D?.Invoke(new string[1] { activeEvent.Item.Id });
		}
		RedrawContextMenus(null);
	}

	private void _E00A(_E992 effect)
	{
		_E07E?.Invoke(effect);
	}

	private void _E00B(bool locked)
	{
		_E07F?.Invoke(locked);
	}

	private void _E00C<_E0B8>(_EB68 itemContext, _E0B8 template, Action<_E0B8, Action, Action> show) where _E0B8 : Window<_EC7C>, IShowable
	{
		_E01E<_E0B8> CS_0024_003C_003E8__locals0 = new _E01E<_E0B8>();
		CS_0024_003C_003E8__locals0._003C_003E4__this = this;
		if (!base.gameObject.activeSelf)
		{
			return;
		}
		Item item = itemContext.Item;
		Type typeFromHandle = typeof(_E0B8);
		Window<_EC7C> window = null;
		_E000 obj = null;
		Window<_EC7C> window2 = null;
		_E000 obj2 = null;
		int num = 0;
		int num2 = 0;
		foreach (_E000 item2 in _E083)
		{
			num++;
			if (window == null)
			{
				window = item2.Window;
			}
			if (obj2 == null || obj2.Index < item2.Index)
			{
				obj2 = item2;
			}
			if (!(item2.WindowType != typeFromHandle))
			{
				num2++;
				if (item2.Item == item)
				{
					obj = item2;
				}
				if (window2 == null)
				{
					window2 = item2.Window;
				}
			}
		}
		if (obj != null)
		{
			CS_0024_003C_003E8__locals0._E002(obj);
			return;
		}
		if (num2 >= 4 && window2 != null)
		{
			window2.Close();
		}
		else if (num >= 8 && window != null)
		{
			window.Close();
		}
		CS_0024_003C_003E8__locals0.window = UnityEngine.Object.Instantiate(template, _infoWindowsContainer, worldPositionStays: false);
		CS_0024_003C_003E8__locals0.windowData = new _E000(item, CS_0024_003C_003E8__locals0.window, typeFromHandle);
		_E083.Add(CS_0024_003C_003E8__locals0.windowData);
		show(CS_0024_003C_003E8__locals0.window, delegate
		{
			CS_0024_003C_003E8__locals0._E002(CS_0024_003C_003E8__locals0.windowData);
		}, delegate
		{
			CS_0024_003C_003E8__locals0._003C_003E4__this._E083.Remove(CS_0024_003C_003E8__locals0.windowData);
			UnityEngine.Object.Destroy(CS_0024_003C_003E8__locals0.window.gameObject);
		});
		RectTransform rectTransform = (RectTransform)CS_0024_003C_003E8__locals0.window.transform;
		LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
		if (obj2 != null)
		{
			RectTransform rectTransform2 = (RectTransform)obj2.Window.transform;
			Vector2 vector = rectTransform2.anchoredPosition + rectTransform2.GetTopLeftToPivotDelta();
			Vector2 vector2 = new Vector2(vector.x + 40f, vector.y - 40f);
			rectTransform.anchoredPosition = vector2 - rectTransform.GetTopLeftToPivotDelta();
		}
		else
		{
			rectTransform.anchoredPosition = new Vector2((float)Screen.width / 2f, (float)Screen.height / 2f).Divide((Vector2)rectTransform.lossyScale);
		}
	}

	public _EC7B ShowChangeQuestConfirm(_EC7A arguments)
	{
		return _acceptQuestChangeWindow.Show(arguments);
	}

	public _EC7C ShowPlayersInviteWindow(string profileId, _ECEF<UpdatableChatMember> friendsList, _EC99 matchmakerPlayersController)
	{
		return _playersInviteWindow.Show(profileId, friendsList, matchmakerPlayersController);
	}

	public void ShowClothingConfirmation(string suiteName, Profile profile, Profile._E001 trader, _E934 quests, _EBE3 requirements, _E9EF stashGrid, Action unlockAction)
	{
		if (_clothingConfirmation != null)
		{
			_clothingConfirmation.Show(suiteName, profile, trader, quests, requirements, stashGrid, unlockAction, null);
		}
	}

	private bool _E00D()
	{
		if (_E083.IsNullOrEmpty())
		{
			return false;
		}
		_E083.Last().Window.Close();
		return true;
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		switch (command)
		{
		case ECommand.MakeScreenshot:
			MonoBehaviourSingleton<PreloaderUI>.Instance.MakeScreenshot();
			return ETranslateResult.Block;
		case ECommand.Escape:
			if (ContextMenu.gameObject.activeSelf)
			{
				ContextMenu.Close();
				return ETranslateResult.Block;
			}
			if (MessageWindowActive)
			{
				_messageWindow.Decline();
				return ETranslateResult.Block;
			}
			if (_groupInviteWindow.gameObject.activeSelf)
			{
				_groupInviteWindow.Close();
				return ETranslateResult.Block;
			}
			if (_raidInviteWindow.gameObject.activeSelf)
			{
				_raidInviteWindow.Close();
				return ETranslateResult.Block;
			}
			if (this._E001)
			{
				_scrolledMessageWindow.Decline();
				return ETranslateResult.Block;
			}
			if (_E00D())
			{
				return ETranslateResult.Block;
			}
			if (_E08B != null && _E08B.gameObject.activeSelf)
			{
				_E08B.Hide();
				return ETranslateResult.Block;
			}
			if (_clothingConfirmation != null && _clothingConfirmation.gameObject.activeSelf)
			{
				_clothingConfirmation.Close();
				return ETranslateResult.Block;
			}
			if (ItemsListWindow.gameObject.activeSelf)
			{
				ItemsListWindow.Close();
				return ETranslateResult.Block;
			}
			if (HandoverItemsWindow.gameObject.activeSelf)
			{
				HandoverItemsWindow.Close();
				return ETranslateResult.Block;
			}
			if (_playersInviteWindow.gameObject.activeSelf)
			{
				_playersInviteWindow.Close();
				return ETranslateResult.Block;
			}
			break;
		case ECommand.EnterHideout:
			if (MessageWindowActive)
			{
				_messageWindow.Accept();
				return ETranslateResult.Block;
			}
			if (this._E001)
			{
				_scrolledMessageWindow.Accept();
				return ETranslateResult.Block;
			}
			if (_E08B != null && _E08B.gameObject.activeSelf)
			{
				_E08B.Accept();
				return ETranslateResult.Block;
			}
			break;
		case ECommand.ThrowItem:
			if (MessageWindowActive)
			{
				return ETranslateResult.Block;
			}
			if (this._E000 == null)
			{
				return ETranslateResult.Block;
			}
			GetItemContextInteractions(this._E000, null).ExecuteInteraction(EItemInfoButton.Discard);
			return ETranslateResult.Block;
		default:
			if (MessageWindowActive)
			{
				return ETranslateResult.Block;
			}
			if (_E082.ContainsKey(command) && this._E000 != null)
			{
				_E000(this._E000.Item, _E082[command]);
			}
			break;
		}
		return ETranslateResult.Ignore;
	}

	protected override void TranslateAxes(ref float[] axes)
	{
	}

	protected override ECursorResult ShouldLockCursor()
	{
		return _E08C;
	}

	public void RegisterCurrentItemContext(_EB68 itemContext)
	{
		this._E000 = itemContext;
	}

	public void UnregisterCurrentItemContext(_EB68 itemContext)
	{
		if (this._E000 == itemContext)
		{
			this._E000 = null;
		}
	}

	public async Task<(Item, bool)> SelectItem(IEnumerable<Item> availableItems, Func<Item, string> getDetails, bool showEmptyCell, SelectingItemView itemTemplate, RectTransform elementPosition, Vector2 offset, EContextPriorDirection direction = EContextPriorDirection.BottomRightToLeft)
	{
		return await _selectItemMenu.GetItem(availableItems, getDetails, showEmptyCell, itemTemplate, elementPosition, offset, direction);
	}

	public void CloseSelectItemMenu()
	{
		_selectItemMenu.Close();
	}

	public void CloseAllWindows()
	{
		UI.Dispose();
		_E08C = ECursorResult.Ignore;
		_E07D?.Invoke(null);
		_E00E();
		if (MessageWindowActive)
		{
			_messageWindow.Decline();
		}
		if (this._E001)
		{
			_scrolledMessageWindow.Decline();
		}
		if (_itemsListWindow.gameObject.activeSelf)
		{
			_itemsListWindow.Close();
		}
		if (_handoverItemsWindow.gameObject.activeSelf)
		{
			_handoverItemsWindow.Close();
		}
		if (_playersInviteWindow.gameObject.activeSelf)
		{
			_playersInviteWindow.Close();
		}
		if (_clothingConfirmation != null && _clothingConfirmation.gameObject.activeSelf)
		{
			_clothingConfirmation.Close();
		}
		if (ContextMenu.gameObject.activeSelf)
		{
			ContextMenu.Close();
		}
		if (_E08B != null)
		{
			_E08B.Hide();
		}
		if (_itemTooltip.Displayed)
		{
			_itemTooltip.Close();
		}
		if (_priceTooltip.Displayed)
		{
			_priceTooltip.Close();
		}
		if (_simpleTooltip.Displayed)
		{
			_simpleTooltip.Close();
		}
		_E006();
	}

	private void _E00E()
	{
		for (int num = _E083.Count - 1; num >= 0; num--)
		{
			_E083.ElementAt(num).Window.Close();
		}
	}

	public override void Close()
	{
		CloseAllWindows();
	}

	[CompilerGenerated]
	private bool _E00F(Mod x)
	{
		return _E084.Examined(x);
	}
}
