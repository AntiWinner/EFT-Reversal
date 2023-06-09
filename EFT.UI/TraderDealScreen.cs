using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class TraderDealScreen : UIElement
{
	private sealed class _E000 : _EC63
	{
		private readonly TraderDealScreen _E002;

		public _E000(TraderDealScreen screen)
		{
			_E002 = screen;
		}

		public override void Show()
		{
			_E002._E008(raiseEvents: true);
		}
	}

	public enum ETraderMode
	{
		Purchase,
		Sale
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public Profile profile;

		public TraderDealScreen _003C_003E4__this;

		internal void _E000()
		{
			profile.OnTraderLoyaltyChanged -= _003C_003E4__this._E004;
		}

		internal void _E001()
		{
			bool active = _003C_003E4__this._E1BB.CurrentAssortment == null && _003C_003E4__this._E1BB.AssortmentLoading;
			_003C_003E4__this._E002(active);
		}

		internal void _E002()
		{
			_003C_003E4__this._E1BB.Info.OnLoyaltyChanged -= _003C_003E4__this._E007;
			_003C_003E4__this._E19B.OnTraderAssortmentChanged -= _003C_003E4__this._E005;
			_003C_003E4__this._traderGridView.OnFilterChanged -= _003C_003E4__this._E00D;
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public _E8AF assortment;

		public TraderDealScreen _003C_003E4__this;

		internal bool _E000(Item queryItem)
		{
			if (assortment.LoyalLevelItems.TryGetValue(queryItem.Id, out var value))
			{
				return _003C_003E4__this.m__E005.SelectedLevel == value;
			}
			return false;
		}
	}

	public const int DEFAULT_GRID_WIDTH = 10;

	public const int DEFAULT_GRID_HEIGHT = 25;

	private const int _E1FC = 0;

	[SerializeField]
	private FilterTab[] _levelTabs;

	[SerializeField]
	private FilterTab _allItemsTab;

	[SerializeField]
	private FilterTab _eliteLevelTab;

	[SerializeField]
	private TradingGridView _traderGridView;

	[SerializeField]
	private TradingGridView _stashGridView;

	[SerializeField]
	private BarterSchemePanel _barterSchemePanel;

	[SerializeField]
	private TradingTable _tradingTable;

	[SerializeField]
	private GameObject _sortPanel;

	[SerializeField]
	private GridSortPanel _gridSortPanel;

	[SerializeField]
	private DefaultUIButton _dealButton;

	[SerializeField]
	private GameObject _traderGridLoader;

	[SerializeField]
	private GameObject _userGridLoader;

	[SerializeField]
	private GameObject _transactionLoader;

	[SerializeField]
	private GameObject _equivalentSumObject;

	[SerializeField]
	private Image _equivalentSumImage;

	[SerializeField]
	private TextMeshProUGUI _equivalentSumValue;

	[SerializeField]
	private ScrollRect _traderScroll;

	[SerializeField]
	private ScrollRect _stashScroll;

	private _E8B2 _E1BB;

	private ETraderMode _E1FD;

	private _EAED _E1FE;

	private _EAA0 _E1FF;

	private ItemUiContext _E089;

	private _E935 _E19B;

	private _EC68 m__E005;

	private _E9F5 _E200;

	private int? _E201;

	private bool _E000 => this.m__E005.SelectedLevel == 0;

	private void Awake()
	{
		_dealButton.OnClick.AddListener(delegate
		{
			switch (_E1FD)
			{
			case ETraderMode.Purchase:
				_E1BB.CurrentAssortment.Purchase().HandleExceptions();
				break;
			case ETraderMode.Sale:
				_E1BB.CurrentAssortment.Sell();
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		});
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.PageUp))
		{
			_traderScroll.normalizedPosition = Vector2.up;
		}
		if (Input.GetKeyDown(KeyCode.PageDown))
		{
			_traderScroll.normalizedPosition = Vector2.zero;
		}
	}

	public void Show(_E8B2 trader, Profile profile, _EAED stashController, ETraderMode mode, ItemUiContext itemUiContext, _E935 questController)
	{
		ShowGameObject();
		_E1BB = trader;
		_E1FD = mode;
		_E1FE = stashController;
		_E1FF = stashController.Inventory.Stash;
		_E089 = itemUiContext;
		_E19B = questController;
		Tab[] levelTabs = _levelTabs;
		Tab[] array = _EC68._E000.SelectTabs(levelTabs, _allItemsTab, _eliteLevelTab, trader.Info.MaxLoyaltyLevel);
		this.m__E005 = new _EC68(array, _allItemsTab, setAsLastSibling: false);
		this.m__E005.Reset();
		_E000 controller = new _E000(this);
		levelTabs = array;
		for (int i = 0; i < levelTabs.Length; i++)
		{
			levelTabs[i].Init(controller);
		}
		profile.OnTraderLoyaltyChanged += _E004;
		UI.AddDisposable(delegate
		{
			profile.OnTraderLoyaltyChanged -= _E004;
		});
		_dealButton.gameObject.SetActive(value: false);
		if (_E1BB.CurrentAssortment == null)
		{
			UI.SubscribeEvent(_E1BB.AssortmentChanged, _E000);
			_E00C();
			FilterTab[] levelTabs2 = _levelTabs;
			for (int i = 0; i < levelTabs2.Length; i++)
			{
				levelTabs2[i]._E001(active: false);
			}
		}
		else
		{
			_E000();
			_E007();
		}
		_E002(_E1BB.AssortmentLoading);
		_E006();
		UI.SubscribeEvent(_E1BB.AssortmentLoadingChanged, delegate
		{
			bool active = _E1BB.CurrentAssortment == null && _E1BB.AssortmentLoading;
			_E002(active);
		});
		_E1BB.Info.OnLoyaltyChanged += _E007;
		_E19B.OnTraderAssortmentChanged += _E005;
		_traderGridView.OnFilterChanged += _E00D;
		UI.AddDisposable(delegate
		{
			_E1BB.Info.OnLoyaltyChanged -= _E007;
			_E19B.OnTraderAssortmentChanged -= _E005;
			_traderGridView.OnFilterChanged -= _E00D;
		});
	}

	private void _E000()
	{
		_E003();
		UI.BindEvent(_E1BB.CurrentAssortment.AssortmentUpdated, _E001);
		UI.BindEvent(_E1BB.CurrentAssortment.PreparedSumChanged, _E00A);
		UI.SubscribeEvent(_E1BB.CurrentAssortment.SelectedItemChanged, _E00B);
		UI.SubscribeEvent(_E1BB.CurrentAssortment.ValidityChanged, _E00B);
		UI.SubscribeEvent(_E1BB.CurrentAssortment.TransactionChanged, _E00B);
	}

	private void _E001()
	{
		_E00B();
		_E008(raiseEvents: false);
	}

	private void _E002(bool active)
	{
		_traderGridLoader.SetActive(active);
		_userGridLoader.SetActive(active);
	}

	private void _E003()
	{
		if (_traderGridView.gameObject.activeSelf)
		{
			Debug.LogError(_ED3E._E000(260829));
			return;
		}
		_E004(_E1BB.Info);
		if (_E1FD == ETraderMode.Purchase)
		{
			_barterSchemePanel.Show(_E1BB.CurrentAssortment, _E1FE, _E1FF.Grid);
		}
		else
		{
			_tradingTable.Show(_E1BB.CurrentAssortment, _E1FE, _E089);
		}
		_stashGridView.Show(_E1FF.Grid, new _ECA3(_E1FF, _E1FD, ETradingSide.Player), _E1BB.CurrentAssortment, _E1FE, _E089);
	}

	private void _E004(Profile._E001 traderInfo)
	{
		if (!(traderInfo.Id != _E1BB.Settings.Id))
		{
			_E007();
			if (!_E1BB.AssortmentLoading)
			{
				_E008(raiseEvents: true);
			}
		}
	}

	private void _E005()
	{
		_E1BB.RefreshAssortment(createIfNotExists: true, ignoreTimeout: true).HandleExceptions();
	}

	private void _E006()
	{
		if (_sortPanel == null)
		{
			return;
		}
		_EAED obj;
		if ((obj = _E1FE) == null)
		{
			_sortPanel.SetActive(value: false);
		}
		else if (obj.IsAllowedToSort(_E1FE.Root))
		{
			_sortPanel.SetActive(value: true);
			if (_gridSortPanel != null)
			{
				_gridSortPanel.Show(obj, _E1FE.Root);
			}
		}
		else
		{
			_sortPanel.SetActive(value: false);
		}
	}

	private void _E007()
	{
		this.m__E005.Reset();
		int num = this.m__E005.SelectedLevel;
		if (num > _E1BB.Info.LoyaltyLevel)
		{
			num = 0;
		}
		this.m__E005.Show(num, sendCallback: false);
		this.m__E005.UpdateInteractableTabs(_E1BB.Info.LoyaltyLevel);
	}

	private void _E008(bool raiseEvents)
	{
		if (_E1BB.CurrentAssortment != null)
		{
			IReadOnlyCollection<Item> items = _E009();
			int num = items.ItemsCollectionHashSum();
			if (num != _E201)
			{
				_E201 = num;
				_traderGridView.Hide();
				_traderGridView.Show(items, new _ECA3(_E1FF, _E1FD, ETradingSide.Trader), _E1BB.CurrentAssortment, _E1FE, _E089, raiseEvents);
			}
		}
	}

	private IReadOnlyCollection<Item> _E009()
	{
		_E8AF assortment = _E1BB.CurrentAssortment;
		_EAA0 obj = (_EAA0)assortment.TraderController.RootItem;
		if (_E200 == null)
		{
			_E200 = new _E9F5(_ED3E._E000(124538), 10, 25, canStretchVertically: true, Array.Empty<ItemFilter>(), obj);
		}
		List<Item> list = _EB1B.Sort(obj.Grid.Items);
		if (this._E000)
		{
			return list;
		}
		int value;
		return list.Where((Item queryItem) => assortment.LoyalLevelItems.TryGetValue(queryItem.Id, out value) && this.m__E005.SelectedLevel == value).ToList();
	}

	private void _E00A()
	{
		_E00B();
		_E8B2._E000 preparedSum = _E1BB.CurrentAssortment.PreparedSum;
		_equivalentSumObject.SetActive(preparedSum.Amount > 0 && _E1FD == ETraderMode.Sale);
		_equivalentSumImage.sprite = EFTHardSettings.Instance.StaticIcons.GetSmallCurrencySign(preparedSum.CurrencyId);
		_equivalentSumValue.text = preparedSum.Amount.ToString();
	}

	private void _E00B()
	{
		_transactionLoader.SetActive(_E1BB.CurrentAssortment.TransactionInProgress);
		switch (_E1FD)
		{
		case ETraderMode.Purchase:
			_dealButton.gameObject.SetActive(!_E1BB.AssortmentLoading && _E1BB.CurrentAssortment.IsValid && !_E1BB.CurrentAssortment.TransactionInProgress && _E1BB.CurrentAssortment.SelectedItem != null && _E1BB.CurrentAssortment.SelectedItem.BuyRestrictionCheck && !_E1BB.CurrentAssortment.SelectedItem.IsEmptyStack);
			break;
		case ETraderMode.Sale:
			_dealButton.gameObject.SetActive(!_E1BB.AssortmentLoading && !_E1BB.CurrentAssortment.TransactionInProgress && _E1BB.CurrentAssortment.PreparedSum.Amount > 0);
			break;
		}
	}

	private void _E00C()
	{
		_dealButton.gameObject.SetActive(value: false);
		if (_traderGridView.gameObject.activeSelf)
		{
			_traderGridView.Hide();
			_stashGridView.Hide();
			if (_E1FD == ETraderMode.Purchase)
			{
				_barterSchemePanel.Close();
			}
			else
			{
				_tradingTable.Close();
			}
		}
	}

	public override void Close()
	{
		base.Close();
		_E201 = null;
		_E00C();
		this.m__E005?.TryHide();
	}

	private void _E00D()
	{
		_E00E(_traderScroll);
	}

	private void _E00E(ScrollRect scroll)
	{
		scroll.verticalNormalizedPosition = 1f;
		scroll.horizontalNormalizedPosition = 0f;
	}

	public void FullClose()
	{
		_traderGridView.ClearFilterChoice();
		_stashGridView.ClearFilterChoice();
		this.m__E005?.Close();
		_E00E(_traderScroll);
		_E00E(_stashScroll);
	}

	[CompilerGenerated]
	private void _E00F()
	{
		switch (_E1FD)
		{
		case ETraderMode.Purchase:
			_E1BB.CurrentAssortment.Purchase().HandleExceptions();
			break;
		case ETraderMode.Sale:
			_E1BB.CurrentAssortment.Sell();
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}
}
