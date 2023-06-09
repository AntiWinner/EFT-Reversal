using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.InputSystem;
using EFT.UI.Screens;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Ragfair;

public sealed class RagfairScreen : UIScreen
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public RagfairScreen _003C_003E4__this;

		public Offer offer;

		public Action callback;

		internal void _E000()
		{
			_003C_003E4__this._E089.RenewOffer(offer, _003C_003E4__this._renewOfferWindow.Prioritized, _003C_003E4__this._renewOfferWindow.HoursCount);
			callback?.Invoke();
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public RagfairScreen _003C_003E4__this;

		public Offer offer;

		public Action callback;

		internal void _E000()
		{
			_003C_003E4__this._E089.RemoveOffer(offer);
			offer.EndTime = (offer.AvailableTimePassed ? _E5AD.UtcNow.AddHours(_ECBD.Settings.offerDurationTimeInHourAfterRemove) : _E5AD.UtcNow);
			callback?.Invoke();
		}
	}

	[SerializeField]
	private DefaultUIButton _addOfferButton;

	[SerializeField]
	private GameObject _offerButtonLockIcon;

	[SerializeField]
	private Button _filterButton;

	[SerializeField]
	private RagfairFilterWindow _filterWindow;

	[SerializeField]
	private AddOfferWindow _addOfferWindowTemplate;

	[SerializeField]
	private RectTransform _requirementsWindowContainer;

	[SerializeField]
	private RectTransform _addOfferContainer;

	[SerializeField]
	private TextMeshProUGUI _myRagfairRating;

	[SerializeField]
	private GameObject _positiveReputation;

	[SerializeField]
	private GameObject _negativeReputation;

	[SerializeField]
	private GameObject _myOffersCount;

	[SerializeField]
	private TextMeshProUGUI _myOffersCurrentCount;

	[SerializeField]
	private TextMeshProUGUI _myOffersMaxCount;

	[SerializeField]
	private OfferViewList _offersListTemplate;

	[SerializeField]
	private RectTransform _offersListContainer;

	[SerializeField]
	private DisplayMoneyPanel _moneyCountPanel;

	[SerializeField]
	private RenewOfferWindow _renewOfferWindow;

	[SerializeField]
	private HandoverRagfairMoneyWindow _handoverMoneyWindow;

	[SerializeField]
	private UIAnimatedToggleSpawner _allOffersToggle;

	[SerializeField]
	private UIAnimatedToggleSpawner _wishListToggle;

	[SerializeField]
	private UIAnimatedToggleSpawner _myOffersToggle;

	private ItemUiContext _E0C9;

	private OfferViewList _E0CA;

	private OfferViewList _E0CB;

	private OfferViewList _E0CC;

	private AddOfferWindow _E0CD;

	private HandoverExchangeableItemsWindow _E0CE;

	private _E796 _E031;

	private Profile _E085;

	private _EAED _E084;

	private _EA40[] _E0CF;

	private Dictionary<string, Profile._E001> _E0D0;

	private _ECB1 _E088;

	private _E79D _E096;

	private _ECBD _E089;

	private _EBA8 _E097;

	private _ECC4 _E0D1;

	private OfferViewList _E0D2;

	private int _E0D3;

	private bool _E0D4;

	private bool _E000 => _E089.CancellableFilters.IsActive(EFilterType.BuildItems);

	private void Awake()
	{
		_E0CA = UnityEngine.Object.Instantiate(_offersListTemplate, _offersListContainer);
		_E0CB = UnityEngine.Object.Instantiate(_offersListTemplate, _offersListContainer);
		_E0CC = UnityEngine.Object.Instantiate(_offersListTemplate, _offersListContainer);
		_allOffersToggle.SpawnedObject.OnMouseDown += delegate
		{
			_E0CA.ClearIdFilter();
			_E0CB.ClearIdFilter();
			_E0CC.ClearIdFilter();
		};
		_wishListToggle.SpawnedObject.OnMouseDown += delegate
		{
			_E0CA.ClearIdFilter();
			_E0CB.ClearIdFilter();
			_E0CC.ClearIdFilter();
		};
		_myOffersToggle.SpawnedObject.OnMouseDown += delegate
		{
			_E0CA.ClearIdFilter();
			_E0CB.ClearIdFilter();
			_E0CC.ClearIdFilter();
		};
		_allOffersToggle.SpawnedObject.onValueChanged.AddListener(delegate(bool arg)
		{
			if (arg)
			{
				_E001();
			}
		});
		_wishListToggle.SpawnedObject.onValueChanged.AddListener(delegate(bool arg)
		{
			if (arg)
			{
				_E005(_E0CB, _E089.Offers, EViewListType.WishList);
			}
		});
		_myOffersToggle.SpawnedObject.onValueChanged.AddListener(delegate(bool arg)
		{
			if (arg)
			{
				_E005(_E0CC, _E089.MyOffersFiltered, EViewListType.MyOffers);
			}
		});
		_addOfferButton.OnClick.AddListener(async delegate
		{
			if (_E0CD == null)
			{
				_E0CD = UnityEngine.Object.Instantiate(_addOfferWindowTemplate, _addOfferContainer);
			}
			else if (_E0CD.gameObject.activeSelf)
			{
				return;
			}
			_addOfferButton.Interactable = false;
			await _E0CD.Show(_E084, _E0CF, _requirementsWindowContainer, _E031, _E089, _E097, _E0C9, _E0C9.WeaponPreviewPool);
			_addOfferButton.Interactable = true;
		});
		_filterButton.onClick.AddListener(delegate
		{
			_filterWindow.Show(_E089);
		});
	}

	public void Show(Profile profile, _EAED inventoryController, _EA40[] lootItems, _E9C4 healthController, _E796 session, _ECC4 search)
	{
		_E0C9 = ItemUiContext.Instance;
		_E0CE = _E0C9.HandoverItemsWindow;
		_E031 = session;
		_E085 = profile;
		_E084 = inventoryController;
		_E0CF = lootItems;
		_E0D0 = profile.TradersInfo;
		_E088 = session.InsuranceCompany;
		_E096 = session.SocialNetwork;
		_E089 = session.RagFair;
		_E097 = Singleton<_EBA8>.Instance;
		ShowGameObject();
		_E000(_allOffersToggle);
		_E0C9.Configure(inventoryController, profile, session, _E088, _E089, null, healthController, new _EA40[1] { inventoryController.Inventory.Stash }, EItemUiContextType.RagfairScreen, ECursorResult.ShowCursor);
		_E794 captchaHandler = _E0C9.CaptchaHandler;
		captchaHandler.Activate();
		UI.AddDisposable(captchaHandler.Deactivate);
		_E008();
		_E089.OnMoneySpend += _E008;
		_E089.OnRatingUpdated += _E009;
		_E089.CancellableFilters.ItemAdded += _E011;
		_E089.CancellableFilters.ItemRemoved += _E012;
		_E089.OnFindById += _E004;
		_E089.OnFilterRuleChanged += _E010;
		UI.AddDisposable(_E089.OnFilteredOffersCountChanged.Subscribe(_E007));
		UI.AddDisposable(_E089.OnStatusChanged.Subscribe(_E003));
		_E002(_E089.Status);
		_E009(_E089.MyRating, _E089.IsRatingGrowing);
		_E0D1 = search;
		if (_E0D1 != null)
		{
			_E089.SetSearch(_E0D1, sendBackendData: true);
		}
		_ECC4 obj = _E0D1;
		if (obj == null || obj.Type != 0)
		{
			if (search != null && search.Type == EFilterType.BuildItems)
			{
				_E005(_E0CA, _E089.WeaponBuildsFiltered, EViewListType.WeaponBuild);
			}
			else
			{
				_E005(_E0CA, _E089.Offers, EViewListType.AllOffers);
			}
		}
	}

	private void _E000(UIAnimatedToggleSpawner toggle)
	{
		_allOffersToggle.ToggleSilently(toggle == _allOffersToggle);
		_wishListToggle.ToggleSilently(toggle == _wishListToggle);
		_myOffersToggle.ToggleSilently(toggle == _myOffersToggle);
	}

	private void _E001()
	{
		EViewListType type = (_E0D4 ? EViewListType.WeaponBuild : EViewListType.AllOffers);
		_E005(_E0CA, _E089.Offers, type);
	}

	private void _E002(_ECBD.ERagFairStatus status)
	{
		bool flag = status == _ECBD.ERagFairStatus.Available;
		if (_E0CD != null && _E0CD.gameObject.activeSelf)
		{
			_E0CD.Close();
		}
		if (_filterWindow != null && _filterWindow.gameObject.activeSelf)
		{
			_filterWindow.Close();
		}
		_offerButtonLockIcon.SetActive(!flag);
		if (!flag)
		{
			_addOfferButton.SetDisabledTooltip(_E089.GetFormattedStatusDescription);
			_myOffersToggle.SpawnableToggle.SetDisabledTooltip(_E089.GetFormattedStatusDescription);
			_E089.CancellableFilters.RemoveFilter(EFilterType.OfferOwnerType, sendCallback: false);
		}
		_addOfferButton.Interactable = flag;
		_myOffersToggle.SpawnableToggle.Interactable = flag;
		_myOffersCount.SetActive(flag);
	}

	private void _E003(_ECBD.ERagFairStatus status)
	{
		_E002(status);
		_E089.ClearCachedInfo();
		if (_E0D2 == _E0CC)
		{
			_E001();
		}
		else
		{
			_E0D2.UpdateOffersForced();
		}
		_E007(EViewListType.MyOffers);
	}

	private void _E004(Offer offer, object id, bool reset)
	{
		if (!(offer == null && reset))
		{
			_E005(_E0CA, _E089.Offers, EViewListType.AllOffers);
			_E000(_allOffersToggle);
		}
	}

	private void _E005(OfferViewList tabToOpen, _ECBB offers, EViewListType type, FilterRule? newRule = null)
	{
		_E0D4 = type == EViewListType.WeaponBuild || (_E0D4 && type != EViewListType.AllOffers);
		if (!(tabToOpen == _E0D2))
		{
			_filterButton.interactable = type != EViewListType.MyOffers;
			if (_E0D2 != null)
			{
				_E0D2.Close();
			}
			if (type != 0)
			{
				_E089.CancellableFilters.RemoveFilter(EFilterType.OfferId, sendCallback: false);
				_E089.CancellableFilters.RemoveFilter(EFilterType.FilterSearch, sendCallback: false);
				_E089.CancellableFilters.RemoveFilter(EFilterType.LinkedSearch, sendCallback: false);
				_E089.CancellableFilters.RemoveFilter(EFilterType.NeededSearch, sendCallback: false);
				_E089.CancellableFilters.RemoveFilter(EFilterType.BuildItems, sendCallback: false);
			}
			if (!newRule.HasValue)
			{
				_E089.ApplyRule(type);
			}
			else
			{
				_E089.AddSearchesInRule(newRule.Value, setRule: false);
			}
			_E0D2 = tabToOpen;
			_E0D2.Show(type, _E097, _E085.Id, offers, _E0D0, _E0C9, _E089, _E084, _E088, _E096, _E00A, _E00B, _E00E, _E00F, _E006, _E0D1);
			if (_filterWindow.gameObject.activeSelf)
			{
				_filterWindow.Close();
			}
		}
	}

	private void _E006(string id)
	{
		_E089.ApplyRule(EViewListType.AllOffers);
		_E089.SetSearch(new _ECC4(EFilterType.OfferId, id), sendBackendData: false);
	}

	private void _E007(EViewListType type)
	{
		if (type != EViewListType.MyOffers)
		{
			return;
		}
		_E0D3 = _E089.GetMaxOffersCount(_E089.MyRating);
		_myOffersCurrentCount.text = _E089.MyOffersCount.ToString();
		_myOffersMaxCount.text = _E0D3.ToString();
		if (_E089.Available)
		{
			bool interactable = _E0D3 > _E089.MyOffersCount;
			if (_E0D3 <= _E089.MyOffersCount)
			{
				_addOfferButton.SetDisabledTooltip(_ED3E._E000(242038));
			}
			_addOfferButton.Interactable = interactable;
		}
	}

	private void _E008()
	{
		_moneyCountPanel.Show(_E085.Inventory.Stash.Grid.Items);
	}

	private void _E009(float rating, bool isRatingGrowing)
	{
		_myRagfairRating.text = rating.ToString(_ED3E._E000(56089));
		_myRagfairRating.color = ((rating < 0f) ? Color.red : Color.white);
		_positiveReputation.SetActive(isRatingGrowing);
		_negativeReputation.SetActive(!isRatingGrowing);
		_E007(EViewListType.MyOffers);
	}

	private void _E00A(Offer offer, Action callback)
	{
		_renewOfferWindow.Show(_E084, _E089, offer, offer.EndTime - _E5AD.UtcNow, delegate
		{
			_E089.RenewOffer(offer, _renewOfferWindow.Prioritized, _renewOfferWindow.HoursCount);
			callback?.Invoke();
		});
	}

	private void _E00B(bool displayInputField, Dictionary<_E7D3, int> offers)
	{
		if (offers.All((KeyValuePair<_E7D3, int> x) => x.Key.OnlyMoney))
		{
			_handoverMoneyWindow.Show(_E085.Inventory, displayInputField, offers, delegate(_E7D8 commodities)
			{
				_E00C(commodities).HandleExceptions();
			}, delegate
			{
			});
		}
		else
		{
			_E0CE.Show(EExchangeableWindowType.Ragfair, offers, displayInputField, _E085, _E084, delegate(_E7D8 commodities)
			{
				_E00C(commodities).HandleExceptions();
			}, delegate
			{
			});
		}
	}

	private async Task _E00C(_E7D8 newPurchases)
	{
		IResult result = await _E089.Purchase(newPurchases);
		if (result.ErrorCode == 214)
		{
			return;
		}
		if (result.Succeed)
		{
			_E00D(newPurchases);
			return;
		}
		int errorCode = result.ErrorCode;
		if (errorCode == 1503 || (uint)(errorCode - 1506) <= 1u)
		{
			_E0D2.UpdateOffersForced();
		}
	}

	private void _E00D(_E7D8 newPurchases)
	{
		if (newPurchases.Count >= 1)
		{
			_E857.DisplayNotification(new _E890(newPurchases));
		}
		foreach (_E7D7 newPurchase in newPurchases)
		{
			_E7D3 exchangeable = newPurchase.Exchangeable;
			int count = newPurchase.Count;
			string id = _E089.Offers.Nodes[exchangeable.Item.TemplateId].Data.Id;
			int num = exchangeable.CurrentItemCount - count;
			exchangeable.BuyRestrictionCurrent += count;
			Offer offer = exchangeable as Offer;
			if (offer != null)
			{
				offer.Item.StackObjectsCount -= count;
			}
			if (num <= 0 && exchangeable.BuyRestrictionMax <= 0 && exchangeable.MemberType != EMemberCategory.Trader)
			{
				_EBAB obj = _E089.Offers.Nodes[id];
				if (obj != null)
				{
					obj.ChangeChildrenCount(-1);
					_E089.RemoveOfferFromList(exchangeable.Id);
				}
			}
			_E089.OfferSold(offer, count, id);
		}
	}

	private void _E00E(Offer offer, Action callback)
	{
		ItemUiContext.Instance.ShowMessageWindow(_ED3E._E000(242063).Localized(), delegate
		{
			_E089.RemoveOffer(offer);
			offer.EndTime = (offer.AvailableTimePassed ? _E5AD.UtcNow.AddHours(_ECBD.Settings.offerDurationTimeInHourAfterRemove) : _E5AD.UtcNow);
			callback?.Invoke();
		}, delegate
		{
		});
	}

	private void _E00F(Offer offer)
	{
		if (_renewOfferWindow.gameObject.activeSelf && _renewOfferWindow.Offer == offer)
		{
			_renewOfferWindow.Close();
		}
		if (_handoverMoneyWindow.gameObject.activeSelf && _handoverMoneyWindow.Goods.ContainsKey(offer))
		{
			_handoverMoneyWindow.Close();
		}
		if (_E0CE.gameObject.activeSelf && _E0CE.Goods.ContainsKey(offer))
		{
			_E0CE.Close();
		}
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (command.IsCommand(ECommand.Escape))
		{
			if (_renewOfferWindow.gameObject.activeSelf)
			{
				_renewOfferWindow.Close();
				return ETranslateResult.Block;
			}
			if (_handoverMoneyWindow.gameObject.activeSelf)
			{
				_handoverMoneyWindow.Close();
				return ETranslateResult.Block;
			}
			if (_filterWindow.gameObject.activeSelf)
			{
				_filterWindow.Close();
				return ETranslateResult.Block;
			}
			if (_E0CD != null && _E0CD.gameObject.activeSelf)
			{
				_E0CD.Close();
				return ETranslateResult.Block;
			}
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuEscape);
			Close();
			return ETranslateResult.Ignore;
		}
		return InputNode.GetDefaultBlockResult(command);
	}

	private void _E010(_ECBD.ESetFilterSource source, bool clear, bool updateCategories)
	{
		if (!string.IsNullOrEmpty(_E089.FilterRule.FilterSearchId) || !string.IsNullOrEmpty(_E089.FilterRule.LinkedSearchId) || !string.IsNullOrEmpty(_E089.FilterRule.NeededSearchId) || _E089.FilterRule.OfferId > 0)
		{
			_E005(_E0CA, _E089.Offers, EViewListType.AllOffers);
			_E000(_allOffersToggle);
		}
	}

	private void _E011(_ECC0 filter)
	{
		_E0D1 = null;
		if (filter.Type == EFilterType.OfferId)
		{
			_E005(_E0CA, _E089.Offers, EViewListType.AllOffers, _E089.FilterRule);
			_E000(_allOffersToggle);
		}
	}

	private void _E012(_ECC0 filter)
	{
		if (_E0D1?.Type == filter.Type)
		{
			_E0D1 = null;
		}
	}

	public override void Close()
	{
		base.Close();
		if (_E0D2 != null)
		{
			_E0D2.Close();
			_E0D2 = null;
		}
		_E0CA.Close();
		_E0CC.Close();
		if (_E0CD != null)
		{
			if (_E0CD.gameObject.activeSelf)
			{
				_E0CD.Close();
			}
			UnityEngine.Object.Destroy(_E0CD.gameObject);
			_E0CD = null;
		}
		if (_E0CE != null && _E0CE.gameObject.activeSelf)
		{
			_E0CE.Close();
		}
		if (_filterWindow.gameObject.activeSelf)
		{
			_filterWindow.Close();
		}
		_E089.OnMoneySpend -= _E008;
		_E089.OnRatingUpdated -= _E009;
		_E089.CancellableFilters.ItemAdded -= _E011;
		_E089.CancellableFilters.ItemRemoved -= _E012;
		_E089.OnFindById -= _E004;
		_E089.OnFilterRuleChanged -= _E010;
		_E089.ClearSavedFilters();
		if (this._E000)
		{
			_E089.CancellableFilters.Restore();
		}
		_E089.ForceNextUpdate = true;
		_E0D4 = false;
		_ECC4 obj = _E0D1;
		if (obj != null && obj.Type == EFilterType.BuildItems)
		{
			_E089.ExternalRagfairSearch(null);
		}
	}

	[CompilerGenerated]
	private void _E013()
	{
		_E0CA.ClearIdFilter();
		_E0CB.ClearIdFilter();
		_E0CC.ClearIdFilter();
	}

	[CompilerGenerated]
	private void _E014(bool arg)
	{
		if (arg)
		{
			_E001();
		}
	}

	[CompilerGenerated]
	private void _E015(bool arg)
	{
		if (arg)
		{
			_E005(_E0CB, _E089.Offers, EViewListType.WishList);
		}
	}

	[CompilerGenerated]
	private void _E016(bool arg)
	{
		if (arg)
		{
			_E005(_E0CC, _E089.MyOffersFiltered, EViewListType.MyOffers);
		}
	}

	[CompilerGenerated]
	private async void _E017()
	{
		if (_E0CD == null)
		{
			_E0CD = UnityEngine.Object.Instantiate(_addOfferWindowTemplate, _addOfferContainer);
		}
		else if (_E0CD.gameObject.activeSelf)
		{
			return;
		}
		_addOfferButton.Interactable = false;
		await _E0CD.Show(_E084, _E0CF, _requirementsWindowContainer, _E031, _E089, _E097, _E0C9, _E0C9.WeaponPreviewPool);
		_addOfferButton.Interactable = true;
	}

	[CompilerGenerated]
	private void _E018()
	{
		_filterWindow.Show(_E089);
	}

	[CompilerGenerated]
	private void _E019(_E7D8 commodities)
	{
		_E00C(commodities).HandleExceptions();
	}
}
