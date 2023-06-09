using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.UI.Utilities.LightScroller;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Ragfair;

public sealed class OfferViewList : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _ECBB offers;

		public OfferViewList _003C_003E4__this;

		public _EBAC originalNodes;

		public Func<_EBAB, bool> _003C_003E9__8;

		internal void _E000()
		{
			int num;
			int num2;
			if (offers.Count > 0 && _003C_003E4__this._E32D != EViewListType.MyOffers)
			{
				num = ((_003C_003E4__this._E32D != EViewListType.WeaponBuild) ? 1 : 0);
				if (num != 0)
				{
					num2 = ((_003C_003E4__this._E36A.LastOrDefault() != _003C_003E4__this._E370) ? 1 : 0);
					goto IL_005c;
				}
			}
			else
			{
				num = 0;
			}
			num2 = 0;
			goto IL_005c;
			IL_00ec:
			int num3;
			bool flag = (byte)num3 != 0;
			int num4;
			if (num4 == 0 || !flag)
			{
				_003C_003E4__this._E36A.Remove(_003C_003E4__this._E371);
			}
			if (num4 != 0 && !flag)
			{
				_003C_003E4__this._E36A.Insert(0, _003C_003E4__this._E371);
			}
			return;
			IL_005c:
			bool flag2 = (byte)num2 != 0;
			if (num == 0 || flag2)
			{
				_003C_003E4__this._E36A.Remove(_003C_003E4__this._E370);
			}
			if (flag2)
			{
				_003C_003E4__this._E36A.Add(_003C_003E4__this._E370);
			}
			if (!_003C_003E4__this._E328.Available)
			{
				num4 = ((offers.Count > 0) ? 1 : 0);
				if (num4 != 0)
				{
					num3 = ((_003C_003E4__this._E36A.FirstOrDefault() == _003C_003E4__this._E371) ? 1 : 0);
					goto IL_00ec;
				}
			}
			else
			{
				num4 = 0;
			}
			num3 = 0;
			goto IL_00ec;
		}

		internal void _E001()
		{
			offers.AllItemsRemoved += _003C_003E4__this._E002;
		}

		internal void _E002()
		{
			originalNodes.ItemAdded -= _003C_003E4__this._E003;
		}

		internal void _E003(NodeBaseView view, string id)
		{
			if (_003C_003E4__this._E32D == EViewListType.AllOffers && (_003C_003E4__this._E328.CancellableFilters.IsActive(EFilterType.FilterSearch) || _003C_003E4__this._E328.CancellableFilters.IsActive(EFilterType.OfferId)))
			{
				if (_003C_003E4__this._E21A != null)
				{
					_003C_003E4__this._E21A.DeselectView();
				}
				_003C_003E4__this._E21A = view;
				return;
			}
			if (_003C_003E4__this._E32D == EViewListType.WishList && _003C_003E4__this._E328.CancellableFilters.IsActive(EFilterType.FilterSearch) && _003C_003E4__this._E328.FilterRule.FilterSearchId != id)
			{
				if (_003C_003E4__this._E21A != null)
				{
					_003C_003E4__this._E21A.DeselectView();
				}
				_003C_003E4__this._E21A = view;
			}
			_003C_003E4__this._E00A(id, dropPage: true);
		}

		internal void _E004(NodeBaseView view, string id)
		{
			if (!(_003C_003E4__this._E21A != null) && !(_003C_003E4__this._E372 == string.Empty) && (view.Node.Data.Id == _003C_003E4__this._E372 || view.Node.Children.Flatten((_EBAB child) => child.Children).Any((_EBAB child) => child.Data.Id == _003C_003E4__this._E372)))
			{
				_003C_003E4__this._E00E(new _ECBC(_ECBD.EUpdatePagesStatus.DoNotUpdate, id));
			}
		}

		internal bool _E005(_EBAB child)
		{
			return child.Data.Id == _003C_003E4__this._E372;
		}

		internal void _E006()
		{
			_003C_003E4__this._browseCategoriesPanel.OnFiltered -= _003C_003E4__this._E001;
		}
	}

	private const int _E33D = 3;

	private const string _E367 = "ragfair/No offers has been found in {0} category. Select another category.";

	[SerializeField]
	private FiltersPanel _filtersPanel;

	[SerializeField]
	private RagfairCategoriesPanel _browseCategoriesPanel;

	[SerializeField]
	private CancellableFiltersPanel _cancellableFiltersPanel;

	[SerializeField]
	private GameObject _loader;

	[SerializeField]
	private LightScroller _scroller;

	[SerializeField]
	private UIElement _cellViewPrefab;

	[SerializeField]
	private UIElement _updateDataPrefab;

	[SerializeField]
	private RagfairAvailabilityWarning _availabilityWarningPrefab;

	[SerializeField]
	private GameObject _notFoundObject;

	[SerializeField]
	private TextMeshProUGUI _notFoundLabel;

	[SerializeField]
	private MassPurchasePanel _massPurchasePanel;

	[SerializeField]
	private Button _refreshButton;

	private _ECBD _E328;

	private EViewListType _E32D;

	private _EBAC _E368;

	private readonly Dictionary<string, int> _E369 = new Dictionary<string, int>();

	private _ED01<Offer, _ECC5> _E36A;

	private OffersUpdatePanel _E36B;

	private NodeBaseView _E21A;

	private Action<Offer> _E36C;

	private Action<string> _E1CD;

	private _EBA8 _E081;

	private string _E289;

	private Dictionary<string, Profile._E001> _E154;

	private ItemUiContext _E089;

	private _EAED _E092;

	private _ECB1 _E35C;

	private _E79D _E2C7;

	private CaptchaHandler _E36D;

	private Action<Offer, Action> _E35D;

	private Action<bool, Dictionary<_E7D3, int>> _E35E;

	private Action<Offer, Action> _E360;

	private float _E070;

	private bool _E072 = true;

	private _ECC4 _E36E;

	private readonly _E3A4 _E36F = new _E3A4();

	private readonly _ECC7 _E370 = new _ECC7();

	private readonly _ECC8 _E371 = new _ECC8();

	private string _E372 = string.Empty;

	private bool _E000
	{
		get
		{
			if (!_E328.IsBuildFilterActive)
			{
				if (_E36E != null)
				{
					return _E36E.Type == EFilterType.BuildItems;
				}
				return false;
			}
			return true;
		}
	}

	private int _E001 => _E328.FilterRule.Page;

	private int _E002
	{
		get
		{
			if (!(_E21A != null) || !_E369.ContainsKey(_E21A.Node.Data.Id))
			{
				return 0;
			}
			return _E369[_E21A.Node.Data.Id];
		}
	}

	private void Awake()
	{
		_refreshButton.onClick.AddListener(_E009);
	}

	public void Show(EViewListType type, _EBA8 handbook, string profileId, _ECBB offers, Dictionary<string, Profile._E001> traders, ItemUiContext itemUiContext, _ECBD ragfair, _EAED inventoryController, _ECB1 insuranceCompany, _E79D social, Action<Offer, Action> onRenew, Action<bool, Dictionary<_E7D3, int>> onPurchase, Action<Offer, Action> onRemove, Action<Offer> onClose, Action<string> onFindById, _ECC4 currentSearch = null)
	{
		UI.Dispose();
		UI.AddDisposable(_E36F);
		UI.AddDisposable(_browseCategoriesPanel);
		_E32D = type;
		_E081 = handbook;
		_E289 = profileId;
		_E154 = traders;
		_E089 = itemUiContext;
		_E092 = inventoryController;
		_E35C = insuranceCompany;
		_E2C7 = social;
		_E35D = onRenew;
		_E35E = onPurchase;
		_E360 = onRemove;
		_E328 = ragfair;
		_E36C = onClose;
		_E1CD = onFindById;
		_E36E = currentSearch;
		_E015(!_E328.GettingOffers);
		if (this._E000)
		{
			if (_E32D == EViewListType.AllOffers)
			{
				_E32D = EViewListType.WeaponBuild;
				offers = _E328.WeaponBuildsFiltered;
			}
		}
		else if (_E32D == EViewListType.WeaponBuild)
		{
			_E32D = EViewListType.AllOffers;
			offers = _E328.Offers;
		}
		_E328.OnFilterRuleChanged += _E00B;
		if (_cancellableFiltersPanel != null)
		{
			_cancellableFiltersPanel.Show(ragfair);
		}
		if (_notFoundObject != null)
		{
			_notFoundObject.SetActive(value: false);
		}
		ShowGameObject();
		_E328.OnGettingOffersProcessing += _E015;
		_E328.OnYourOfferSold += _E006;
		_E000(offers);
		if (_E32D == EViewListType.AllOffers)
		{
			_E328.OnFindById += _E007;
		}
		_E328.CancellableFilters.ItemAdded += _E004;
		_E328.CancellableFilters.ItemRemoved += _E005;
		FilterRule filterRule = _E328.FilterRule;
		filterRule.ViewListType = _E32D;
		_E00E(new _ECBC(_ECBD.EUpdatePagesStatus.DoNotUpdate, filterRule.HandbookId));
		_E328.ForceSetFilterRule(filterRule);
		UI.BindState(Singleton<_E7DE>.Instance.Game.Settings.RagfairLinesCount, _E013);
		_E014();
	}

	public void ClearIdFilter()
	{
		_browseCategoriesPanel.ClearIdFilter();
	}

	private void _E000(_ECBB offers)
	{
		_E36F.Dispose();
		offers.AllItemsRemoved += _E002;
		_E36F.AddDisposable(offers.ItemsChanged.Subscribe(delegate
		{
			int num;
			int num2;
			if (offers.Count > 0 && _E32D != EViewListType.MyOffers)
			{
				num = ((_E32D != EViewListType.WeaponBuild) ? 1 : 0);
				if (num != 0)
				{
					num2 = ((_E36A.LastOrDefault() != _E370) ? 1 : 0);
					goto IL_005c;
				}
			}
			else
			{
				num = 0;
			}
			num2 = 0;
			goto IL_005c;
			IL_00ec:
			int num3;
			bool flag2 = (byte)num3 != 0;
			int num4;
			if (num4 == 0 || !flag2)
			{
				_E36A.Remove(_E371);
			}
			if (num4 != 0 && !flag2)
			{
				_E36A.Insert(0, _E371);
			}
			return;
			IL_005c:
			bool flag3 = (byte)num2 != 0;
			if (num == 0 || flag3)
			{
				_E36A.Remove(_E370);
			}
			if (flag3)
			{
				_E36A.Add(_E370);
			}
			if (!_E328.Available)
			{
				num4 = ((offers.Count > 0) ? 1 : 0);
				if (num4 != 0)
				{
					num3 = ((_E36A.FirstOrDefault() == _E371) ? 1 : 0);
					goto IL_00ec;
				}
			}
			else
			{
				num4 = 0;
			}
			num3 = 0;
			goto IL_00ec;
		}));
		_E36A = new _ED01<Offer, _ECC5>(offers, _E008, null);
		_scroller.Show(_E36A, _E010, (_ECC5 offerData) => offerData.DataType, _E011);
		_E36F.AddDisposable(delegate
		{
			offers.AllItemsRemoved += _E002;
		});
		_E36F.AddDisposable(_E36A);
		_E36F.AddDisposable(_scroller);
		bool flag = _E32D == EViewListType.WishList;
		_EBAC originalNodes = (flag ? _E328.Wishlist : offers.Nodes);
		if (flag)
		{
			originalNodes.ItemAdded += _E003;
			_E36F.AddDisposable(delegate
			{
				originalNodes.ItemAdded -= _E003;
			});
		}
		_E368 = new _EBAC(originalNodes);
		_EBAC filteredNodes = new _EBAC(_E368);
		if (_E32D == EViewListType.WeaponBuild)
		{
			Dictionary<string, int> includedItems = ((_E36E?.Value is BuildItemSearchValue buildItemSearchValue && buildItemSearchValue.BuildItems.Count > 0) ? buildItemSearchValue.BuildItems : ((!_E328.CancellableFilters.IsActive(EFilterType.BuildItems)) ? new Dictionary<string, int>() : _E328.FilterRule.BuildItems));
			_massPurchasePanel.Show(includedItems, _E328, _E35E);
		}
		else
		{
			_massPurchasePanel.Close();
		}
		_filtersPanel.Show(_E328, _E32D);
		if (_browseCategoriesPanel.gameObject.activeSelf)
		{
			_browseCategoriesPanel.Close();
		}
		if (_E32D == EViewListType.WeaponBuild)
		{
			return;
		}
		_browseCategoriesPanel.Show(string.Empty, _E328, _E081, originalNodes, _E368, filteredNodes, _E089.ContextMenu, _E32D, EWindowType.Default, delegate(NodeBaseView view, string id)
		{
			if (_E32D == EViewListType.AllOffers && (_E328.CancellableFilters.IsActive(EFilterType.FilterSearch) || _E328.CancellableFilters.IsActive(EFilterType.OfferId)))
			{
				if (_E21A != null)
				{
					_E21A.DeselectView();
				}
				_E21A = view;
			}
			else
			{
				if (_E32D == EViewListType.WishList && _E328.CancellableFilters.IsActive(EFilterType.FilterSearch) && _E328.FilterRule.FilterSearchId != id)
				{
					if (_E21A != null)
					{
						_E21A.DeselectView();
					}
					_E21A = view;
				}
				_E00A(id, dropPage: true);
			}
		}, delegate(NodeBaseView view, string id)
		{
			if (!(_E21A != null) && !(_E372 == string.Empty) && (view.Node.Data.Id == _E372 || view.Node.Children.Flatten((_EBAB child) => child.Children).Any((_EBAB child) => child.Data.Id == _E372)))
			{
				_E00E(new _ECBC(_ECBD.EUpdatePagesStatus.DoNotUpdate, id));
			}
		}, _E1CD).HandleExceptions();
		_browseCategoriesPanel.OnFiltered += _E001;
		UI.AddDisposable(delegate
		{
			_browseCategoriesPanel.OnFiltered -= _E001;
		});
	}

	private void _E001()
	{
		_E21A = null;
	}

	private void _E002()
	{
		_E36A.Remove(_E370);
	}

	private void _E003(_EBAB node)
	{
		_E00E(new _ECBC(_ECBD.EUpdatePagesStatus.Default, node.Data.Id));
		_E00A(node.Data.Id, dropPage: true);
	}

	private void _E004(_ECC0 filter)
	{
		_E36E = null;
	}

	private void _E005(_ECC0 filter)
	{
		if (_E36E != null && _E36E.Type == filter.Type)
		{
			_E36E = null;
		}
	}

	private void _E006(Offer offer, int count, string handbookId)
	{
		if (!(offer.User.Id != _E289))
		{
			_EBAB obj = _E328.Offers.Nodes[handbookId];
			if (obj != null && offer.CurrentItemCount - count <= 0)
			{
				obj.ChangeChildrenCount(-1);
			}
		}
	}

	private void _E007([CanBeNull] Offer offer, object id, bool reset)
	{
		if (_E32D != 0)
		{
			return;
		}
		_notFoundObject.SetActive(offer == null && !reset);
		if (offer == null)
		{
			if (reset)
			{
				_browseCategoriesPanel.SetInputFieldText(string.Empty);
				return;
			}
			_notFoundLabel.text = string.Format(_ED3E._E000(241875).Localized(), id);
			_browseCategoriesPanel.SetInputFieldText(_ED3E._E000(60677) + id);
		}
		else
		{
			_browseCategoriesPanel.SetInputFieldText(_ED3E._E000(60677) + id);
			_E00E(new _ECBC(1, _E372));
		}
	}

	private bool _E008(Offer offer, out _ECC5 convertedItem)
	{
		_E154.TryGetValue(offer.User?.Id ?? string.Empty, out var value);
		convertedItem = new _ECC6
		{
			Handbook = _E081,
			ProfileId = _E289,
			Offer = offer,
			TraderInfo = value,
			ItemUiContext = _E089,
			Ragfair = _E328,
			InventoryController = _E092,
			InsuranceCompany = _E35C,
			SocialNetwork = _E2C7,
			OnRenew = _E35D,
			OnPurchase = _E35E,
			OnExpired = _E00F,
			OnRemove = _E360
		};
		if (offer.User != null && offer.User.MemberType != EMemberCategory.Trader)
		{
			return (offer.EndTime - _E5AD.UtcNow).TotalSeconds.Positive();
		}
		return true;
	}

	private void Update()
	{
		if (!_E072)
		{
			_E070 += Time.deltaTime;
			if (_E070 >= 3f)
			{
				_E072 = true;
				_refreshButton.interactable = true;
			}
		}
		if (Input.GetKeyDown(KeyCode.F5))
		{
			_E009();
		}
		if (Input.GetKeyDown(KeyCode.PageUp))
		{
			_scroller.SetScrollPosition(0f);
		}
		if (Input.GetKeyDown(KeyCode.PageDown))
		{
			_scroller.SetScrollPosition(1f);
		}
		if (Input.GetKeyDown(KeyCode.F1))
		{
			_E328.ClearCachedInfo();
		}
	}

	private void _E009()
	{
		if (_E072)
		{
			_E070 = 0f;
			_E072 = false;
			_refreshButton.interactable = false;
			_E328.ClearCachedInfo();
			_E00B(_ECBD.ESetFilterSource.FilterWindow, clear: true, updateCategories: true);
		}
	}

	private void _E00A(string id, bool dropPage)
	{
		_scroller.SetScrollPosition(0f);
		bool flag = _E328.CancellableFilters.IsActive(EFilterType.FilterSearch);
		if (!flag || _E32D != EViewListType.WishList || !(_E328.FilterRule.FilterSearchId != id))
		{
			FilterRule filterRule = _E328.FilterRule;
			filterRule.ViewListType = _E32D;
			if (dropPage)
			{
				filterRule.Page = 0;
			}
			if (!flag)
			{
				filterRule.HandbookId = id;
			}
			_E328.SetFilterRule(filterRule, clear: true, _E36E?.Type.IsIdBased() ?? true, _ECBD.ESetFilterSource.IdSelected);
		}
	}

	private async void _E00B(_ECBD.ESetFilterSource source, bool clear, bool updateCategories)
	{
		if (_E328 == null)
		{
			return;
		}
		if (this._E000)
		{
			if (_E32D == EViewListType.AllOffers)
			{
				_E32D = EViewListType.WeaponBuild;
				_E000(_E328.WeaponBuildsFiltered);
			}
		}
		else if (_E32D == EViewListType.WeaponBuild)
		{
			_E32D = EViewListType.AllOffers;
			_E000(_E328.Offers);
		}
		if (_E328.FilterRule.OfferId > 0)
		{
			return;
		}
		switch (_E32D)
		{
		case EViewListType.AllOffers:
			if (clear)
			{
				_E328.ClearOffers();
			}
			await _E00C(updateCategories);
			break;
		case EViewListType.WishList:
			if (_E328.Wishlist.Count <= 0)
			{
				_E328.ClearOffers();
				break;
			}
			if (clear)
			{
				_E328.ClearOffers();
			}
			await _E00C(updateCategories);
			break;
		case EViewListType.WeaponBuild:
			if (clear)
			{
				_E328.ClearOffers();
			}
			await _E00C(updateCategories, _E328.FilterWeaponBuildOffers);
			break;
		case EViewListType.MyOffers:
			_E328.FilterMyOffers();
			_E00E(new _ECBC(_E328.MyOffersCount, _E372));
			break;
		case EViewListType.Handbook:
			throw new ArgumentOutOfRangeException();
		default:
			throw new ArgumentOutOfRangeException();
		case EViewListType.RequirementsWindow:
			break;
		}
	}

	private async Task _E00C(bool updateCategories, Action afterGetOffers = null)
	{
		_ECBD obj = _E328;
		_ECBA obj2 = new _ECBA(_E328, updateCategories);
		Result<_ECBC> result = await obj2.Execute();
		if (result.Succeed)
		{
			_E00E(result.Value);
		}
		else
		{
			_E00D(obj2.HandbookId);
		}
		afterGetOffers?.Invoke();
		if (_E328 == null)
		{
			obj.ForceNextUpdate = true;
		}
	}

	private void _E00D(string failedCategoryId)
	{
		NodeBaseView nodeBaseView = _browseCategoriesPanel.SelectNode(failedCategoryId);
		if (nodeBaseView != null)
		{
			nodeBaseView.DeselectView();
		}
		if (_E21A != null)
		{
			_browseCategoriesPanel.SelectNode(_E21A.Node.Data.Id);
		}
		if (_E328 != null)
		{
			_E328.ForceNextUpdate = true;
		}
	}

	private void _E00E(_ECBC offersData)
	{
		if (_E328 == null)
		{
			return;
		}
		if (_E328.FilterRule.OfferId != 0L)
		{
			_browseCategoriesPanel.SetInputFieldText(_ED3E._E000(60677) + _E328.FilterRule.OfferId);
		}
		if (_E21A != null)
		{
			_E21A.DeselectView();
		}
		NodeBaseView nodeBaseView = null;
		_E372 = ((_E328.FilterRule.HandbookId != string.Empty) ? _E328.FilterRule.HandbookId : offersData.SelectedCategory);
		if (!string.IsNullOrEmpty(_E372))
		{
			nodeBaseView = _browseCategoriesPanel.SelectNode(_E372);
		}
		_E21A = nodeBaseView;
		if (_E32D == EViewListType.MyOffers || _E32D == EViewListType.WeaponBuild)
		{
			return;
		}
		if (offersData.UpdatePagesStatus != 0)
		{
			_E012(_E36B);
			return;
		}
		if (_E32D == EViewListType.AllOffers)
		{
			_notFoundObject.SetActive(offersData.Count <= 0);
			_notFoundLabel.text = string.Format(_ED3E._E000(241889).Localized(), _E328.CategoryName.Localized());
		}
		if (!string.IsNullOrEmpty(offersData.SelectedCategory))
		{
			_EBAB obj = _E081[offersData.SelectedCategory];
			if (obj == null)
			{
				return;
			}
			obj.Count = offersData.Count;
			obj.CountUpdatedCall();
			_E369[obj.Data.Id] = ((offersData.Count > _E328.OffersPerPageCount) ? Mathf.CeilToInt((float)offersData.Count / (float)_E328.OffersPerPageCount) : 0);
		}
		_E012(_E36B);
	}

	public void UpdateOffersForced()
	{
		_E070 = 0f;
		_E072 = true;
		_E009();
	}

	private void _E00F(Offer offer)
	{
		_E39B.LogRagfair(_ED3E._E000(242004) + offer.IntId + _ED3E._E000(11164));
		_E36C?.Invoke(offer);
		_E328.RemoveOfferFromList(offer.Id);
	}

	private UIElement _E010(_ECC5 offerData)
	{
		return offerData.DataType switch
		{
			ERagFairOfferDataType.Warning => _availabilityWarningPrefab, 
			ERagFairOfferDataType.OfferData => _cellViewPrefab, 
			ERagFairOfferDataType.UpdateData => _updateDataPrefab, 
			_ => throw new Exception(_ED3E._E000(241988)), 
		};
	}

	private void _E011(_ECC5 offerData, UIElement dataView)
	{
		if (offerData is _ECC6 data && dataView is OfferView offerView)
		{
			offerView.Show(_E32D, data);
			return;
		}
		if (offerData is _ECC7 && dataView is OffersUpdatePanel offersUpdatePanel)
		{
			if (_E32D == EViewListType.MyOffers)
			{
				return;
			}
			_E36B = offersUpdatePanel;
			_E012(_E36B);
		}
		if (offerData is _ECC8 && dataView is RagfairAvailabilityWarning ragfairAvailabilityWarning && _E32D != EViewListType.MyOffers)
		{
			ragfairAvailabilityWarning.Show(_E328);
		}
	}

	private void _E012(OffersUpdatePanel view)
	{
		if (view == null)
		{
			return;
		}
		view.Show(_E328.OffersPerPageCount, this._E001, this._E002, delegate
		{
			FilterRule filterRule2 = _E328.FilterRule;
			filterRule2.ViewListType = _E32D;
			filterRule2.Page++;
			_E328.SetFilterRule(filterRule2, clear: false);
		}, delegate(int arg)
		{
			if (this._E001 != arg)
			{
				FilterRule filterRule = _E328.FilterRule;
				filterRule.ViewListType = _E32D;
				filterRule.Page = arg;
				_E328.SetFilterRule(filterRule, clear: true);
			}
		});
	}

	private void _E013(int offersPerPage)
	{
		int page = _E328.OffersPerPageCount * this._E001 / offersPerPage;
		int value = _E328.OffersPerPageCount * this._E002 / offersPerPage;
		_E328.ClearCachedInfo();
		if (_E21A != null)
		{
			_E369[_E21A.Node.Data.Id] = value;
		}
		FilterRule filterRule = _E328.FilterRule;
		filterRule.Page = page;
		_E328.ForceNextUpdate = true;
		_E328.SetFilterRule(filterRule, clear: true);
	}

	private void _E014()
	{
		if (_E36E != null && _E36E.Type.IsIdBased())
		{
			return;
		}
		switch (_E32D)
		{
		case EViewListType.AllOffers:
			if (_E328.FilterRule.OfferId == 0L)
			{
				_E00A(_E328.LastSelectedIds[_E32D], dropPage: false);
			}
			break;
		case EViewListType.WeaponBuild:
			_E00A(_E328.LastSelectedIds[_E32D], dropPage: false);
			break;
		case EViewListType.MyOffers:
			if (_E328.MyOffersCount <= 0)
			{
				_E328.FilterMyOffers();
			}
			else
			{
				_E00A(_E328.LastSelectedIds[_E32D], dropPage: false);
			}
			break;
		case EViewListType.WishList:
			if (_E328.FilterRule.OfferId > 0)
			{
				break;
			}
			if (_E328.Wishlist.Count == 0)
			{
				_E328.ClearOffers();
				_E00A(string.Empty, dropPage: true);
			}
			else if (_E328.FilterRule.AnyIdSearch)
			{
				var (id, obj2) = (from x in _E328.Wishlist
					orderby x.Value.Data.Order, x.Value.Data.Name
					select x).LastOrDefault((KeyValuePair<string, _EBAB> x) => x.Value.Count > 0);
				if (obj2 != null)
				{
					_E00A(id, dropPage: false);
				}
			}
			else
			{
				string text2 = _E328.LastSelectedIds[_E32D];
				string id2 = (string.IsNullOrEmpty(text2) ? _E328.Wishlist.Last().Key : text2);
				_E00A(id2, dropPage: false);
			}
			break;
		case EViewListType.RequirementsWindow:
		case EViewListType.Handbook:
			throw new ArgumentOutOfRangeException(_ED3E._E000(124643), _E32D, null);
		default:
			throw new ArgumentOutOfRangeException(_ED3E._E000(124643), _E32D, null);
		}
	}

	private void _E015(bool active)
	{
		_loader.SetActive(!active);
	}

	public override void Close()
	{
		if (_E328 != null)
		{
			_E328.OnGettingOffersProcessing -= _E015;
			_E328.OnYourOfferSold -= _E006;
			_E328.CancellableFilters.ItemAdded -= _E004;
			_E328.CancellableFilters.ItemRemoved -= _E005;
			_E328.OnFilterRuleChanged -= _E00B;
			_E328.OnFindById -= _E007;
		}
		_E328 = null;
		_browseCategoriesPanel.Close();
		_E368?.Clear();
		_filtersPanel.Close();
		if (_cancellableFiltersPanel != null)
		{
			_cancellableFiltersPanel.Close();
		}
		_massPurchasePanel.Close();
		base.Close();
	}

	[CompilerGenerated]
	private void _E016()
	{
		FilterRule filterRule = _E328.FilterRule;
		filterRule.ViewListType = _E32D;
		filterRule.Page++;
		_E328.SetFilterRule(filterRule, clear: false);
	}

	[CompilerGenerated]
	private void _E017(int arg)
	{
		if (this._E001 != arg)
		{
			FilterRule filterRule = _E328.FilterRule;
			filterRule.ViewListType = _E32D;
			filterRule.Page = arg;
			_E328.SetFilterRule(filterRule, clear: true);
		}
	}
}
