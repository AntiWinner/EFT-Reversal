using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ChatShared;
using Comfort.Common;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.Ragfair;

public sealed class OfferView : UIElement, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	private const int _E358 = 85;

	[SerializeField]
	private GameObject _checkboxPanel;

	[SerializeField]
	private GameObject _selectedMark;

	[SerializeField]
	private Image _minimizeButton;

	[SerializeField]
	private Sprite _minimizedSprite;

	[SerializeField]
	private Sprite _expandedSprite;

	[SerializeField]
	private GameObject _buttonsContainer;

	[SerializeField]
	private DefaultUIButton _purchaseButton;

	[SerializeField]
	private DefaultUIButton _removeButton;

	[SerializeField]
	private TextMeshProUGUI _offerId;

	[SerializeField]
	private GameObject _exchangeOffer;

	[SerializeField]
	private MerchantInfoView _merchantInfoView;

	[SerializeField]
	private CanvasGroup _merchantCanvasGroup;

	[SerializeField]
	private OfferItemDescription _descriptionShrunk;

	[SerializeField]
	private OfferItemDescription _descriptionExpanded;

	[SerializeField]
	private OfferItemPrice _priceShrunk;

	[SerializeField]
	private OfferItemPrice _priceExpanded;

	[SerializeField]
	private GameObject _lockedButton;

	[SerializeField]
	private CanvasGroup _canvasGroup;

	[SerializeField]
	private HoverTooltipAreaClick _hoverTooltipArea;

	[SerializeField]
	private Button _renewButton;

	[SerializeField]
	private Image _disabledPanel;

	[SerializeField]
	private Image _selectedBackground;

	[SerializeField]
	private GameObject _notAvailableButton;

	[SerializeField]
	private GameObject _outOfStockButton;

	[SerializeField]
	private GameObject _expirationTimePanel;

	[SerializeField]
	private TextMeshProUGUI _expirationLabel;

	[SerializeField]
	private TextMeshProUGUI _createdTimeLabel;

	[SerializeField]
	private GameObject _availableTimePanel;

	[SerializeField]
	private TextMeshProUGUI _availableTimeLabel;

	[SerializeField]
	private GameObject _loader;

	[SerializeField]
	private Image[] _backgroundImages;

	[SerializeField]
	private Image _timeLeftImage;

	[SerializeField]
	private Image _borderImage;

	[SerializeField]
	private Image _backgroundIdImage;

	[SerializeField]
	private Sprite _myIdSprite;

	[SerializeField]
	private Sprite _traderIdSprite;

	private readonly Dictionary<Image, Color32> _E359 = new Dictionary<Image, Color32>();

	private readonly Dictionary<Image, Color32> _E35A = new Dictionary<Image, Color32>();

	private int _E35B;

	private EViewListType _E186;

	private _EBA8 _E081;

	private string _E289;

	private Profile._E001 _E1BB;

	private _ECBD _E328;

	private _EAED _E092;

	private _ECB1 _E35C;

	private ItemUiContext _E089;

	private _E79D _E2C7;

	private Action<Offer, Action> _E35D;

	private Action<bool, Dictionary<_E7D3, int>> _E35E;

	private Action<Offer> _E35F;

	private Action<Offer, Action> _E360;

	private Color32 _E128;

	private Color32 _E361;

	private DateTime _E362;

	private double _E363 = 1.0;

	private bool _E364;

	private bool _E365;

	private bool? _E355;

	[CompilerGenerated]
	private Offer _E1D4;

	[CompilerGenerated]
	private _ECC6 _E366;

	private static Color32 _E000 => new Color32(80, 80, 80, 65);

	private static Color32 _E001 => new Color32(60, 147, 192, 30);

	private static Color32 _E002 => new Color32(11, 106, 130, byte.MaxValue);

	private static Color32 _E003 => new Color32(197, 195, 178, byte.MaxValue);

	private static Color32 _E004 => new Color32(211, 0, 0, 30);

	private static Color32 _E005 => new Color32(byte.MaxValue, 0, 0, 60);

	private static Color32 _E006 => new Color32(189, 0, 0, byte.MaxValue);

	private static Color32 _E007 => new Color32(byte.MaxValue, 0, 0, 60);

	private static Color32 _E008 => new Color32(30, 15, 15, 80);

	private TimeSpan _E009 => this._E00C.EndTime - _E5AD.UtcNow;

	private TimeSpan _E00A => _E5AD.UtcNow - this._E00C.StartTime;

	private bool _E00B
	{
		get
		{
			if (this._E00C != null && !this._E00C.NotAvailable)
			{
				return this._E00C.MemberType == EMemberCategory.Trader;
			}
			return false;
		}
	}

	private Offer _E00C
	{
		[CompilerGenerated]
		get
		{
			return _E1D4;
		}
		[CompilerGenerated]
		set
		{
			_E1D4 = value;
		}
	}

	private _ECC6 _E00D
	{
		[CompilerGenerated]
		get
		{
			return _E366;
		}
		[CompilerGenerated]
		set
		{
			_E366 = value;
		}
	}

	private bool _E00E => this._E00C.TotalItemCount <= 0;

	private bool _E00F => _E186 == EViewListType.WeaponBuild;

	private bool _E010
	{
		get
		{
			if (this._E00C == null || this._E00C.NotAvailable)
			{
				return false;
			}
			return _E289 == this._E00C.User.Id;
		}
	}

	private void Awake()
	{
		_offerId.gameObject.AddComponent<ClickTrigger>().Init(delegate(PointerEventData eventData)
		{
			_ECBD ragfair = (this._E010 ? null : _E328);
			_ECBE contextInteractions2 = new _ECBE(delegate
			{
			}, this._E00C, ragfair);
			_E089.ContextMenu.Show(eventData.position, contextInteractions2);
		});
		_merchantInfoView.gameObject.AddComponent<ClickTrigger>().Init(delegate(PointerEventData eventData)
		{
			if (this._E00B || this._E00C.NotAvailable || this._E00E)
			{
				OnPointerClick(null);
			}
			else
			{
				UpdatableChatMember updatableChatMember = UpdatableChatMember.FindOrCreate(this._E00C.User.Id, (string id) => new UpdatableChatMember(id));
				updatableChatMember.UpdateFromMerchant(this._E00C.User);
				_E467 invitationByProfile = _E2C7.GetInvitationByProfile(updatableChatMember);
				_ECA8 contextInteractions = new _ECA8(_E2C7, null, updatableChatMember, invitationByProfile);
				_E089.ContextMenu.Show(eventData.position, contextInteractions);
			}
		});
		_purchaseButton.OnClick.AddListener(_E001);
		_removeButton.OnClick.AddListener(delegate
		{
			_E360?.Invoke(this._E00C, delegate
			{
				_E007();
				_E008();
			});
		});
		_renewButton.onClick.AddListener(delegate
		{
			_E35D?.Invoke(this._E00C, delegate
			{
				_E007();
				_E008();
			});
		});
	}

	public void Show(EViewListType viewType, _ECC6 data)
	{
		if (this._E00C != null || this._E00C == data.Offer)
		{
			return;
		}
		_E355 = null;
		this._E00D = data;
		this._E00C = data.Offer;
		_E186 = viewType;
		if (this._E00C != null)
		{
			if (!this._E00C.NotAvailable)
			{
				base.gameObject.name = this._E00C.Name.Localized();
			}
			_E081 = data.Handbook;
			_E289 = data.ProfileId;
			_E1BB = data.TraderInfo;
			_E328 = data.Ragfair;
			_E089 = data.ItemUiContext;
			_E092 = data.InventoryController;
			_E35C = data.InsuranceCompany;
			_E2C7 = data.SocialNetwork;
			_E35B = Singleton<_E5CB>.Instance.RagFair.delaySinceOfferAdd;
			_E328.OnOfferProcessing += _E005;
			_E328.OnYourOfferSold += _E004;
			this._E00C.OnSelectToPurchase += _E003;
			_E35D = data.OnRenew;
			_E35E = data.OnPurchase;
			_E35F = data.OnExpired;
			_E360 = data.OnRemove;
			_E000();
		}
	}

	private void _E000()
	{
		_E005(new string[1] { this._E00C.Id }, finished: true);
		_E007();
		_checkboxPanel.SetActive(this._E00F);
		_offerId.text = (this._E00C.NotAvailable ? string.Empty : this._E00C.IntId.FormatSeparate(_ED3E._E000(2540)));
		if (this._E00C.NotAvailable)
		{
			_exchangeOffer.SetActive(value: false);
		}
		else
		{
			_exchangeOffer.SetActive(!this._E00C.OnlyMoney);
		}
		_E006();
		_E008();
		ShowGameObject();
		Image[] backgroundImages = _backgroundImages;
		foreach (Image image in backgroundImages)
		{
			image.color = _E359[image];
		}
		if (this._E00B)
		{
			_merchantInfoView.Show(_E1BB);
		}
		else if (!this._E00C.NotAvailable)
		{
			_merchantInfoView.Show(_E328, this._E00C.User, this._E010);
		}
		else
		{
			_merchantInfoView.Show(null);
		}
		this._E00C.AvailableTimePassed = _E00A();
		_backgroundIdImage.gameObject.SetActive(this._E010);
		_E00B(!this._E00E && this._E00C.CanBeBought && this._E00D.Expanded);
		_merchantCanvasGroup.alpha = ((!this._E00C.NotAvailable) ? 1 : 0);
		_selectedBackground.gameObject.SetActive(this._E00F && _E328.IsSelected(this._E00C));
		_E002();
		_E003(this._E00C, _E328.IsSelected(this._E00C));
	}

	private void _E001()
	{
		int value = ((!this._E00C.SellInOnePiece) ? 1 : this._E00C.Item.StackObjectsCount);
		_E35E?.Invoke(arg1: true, new Dictionary<_E7D3, int> { { this._E00C, value } });
	}

	private void _E002()
	{
		_disabledPanel.gameObject.SetActive(this._E00C.NotAvailable || this._E00E);
		_disabledPanel.color = (this._E00C.NotAvailable ? OfferView._E008 : OfferView._E007);
		_notAvailableButton.SetActive(this._E00C.NotAvailable);
		_outOfStockButton.SetActive(!this._E00C.NotAvailable && this._E00E);
	}

	private void _E003(Offer offer, bool value)
	{
		if (this._E00C == offer)
		{
			_selectedMark.SetActive(value);
			_selectedBackground.gameObject.SetActive(value);
		}
	}

	private void _E004(Offer offer, int count, string handbookId)
	{
		if (this._E00C != null && !(this._E00C.Id != offer.Id))
		{
			_E00C();
			_E000();
			if (this._E00C.CurrentItemCount <= 0 && _E328.IsSelected(offer))
			{
				_E328.SwitchOfferSelection(offer);
			}
		}
	}

	private void _E005(IEnumerable<string> offerIds, bool finished)
	{
		if ((bool)this && !(_loader == null) && offerIds.Contains(this._E00C.Id))
		{
			_loader.gameObject.SetActive(!finished);
			_buttonsContainer.gameObject.SetActive(finished);
		}
	}

	private void _E006()
	{
		_E128 = (this._E010 ? OfferView._E001 : OfferView._E000);
		_E361 = new Color32(_E128.r, _E128.g, _E128.b, 85);
		_E359.Clear();
		_E35A.Clear();
		Image[] backgroundImages = _backgroundImages;
		foreach (Image key in backgroundImages)
		{
			_E359.Add(key, _E128);
			_E35A.Add(key, _E361);
		}
		Color color = _borderImage.color;
		_borderImage.color = new Color(color.r, color.g, color.b, 85f);
		_borderImage.gameObject.SetActive(this._E010);
		_backgroundIdImage.sprite = (this._E00B ? _traderIdSprite : (this._E010 ? _myIdSprite : null));
		_borderImage.color = (this._E010 ? OfferView._E002 : OfferView._E000);
	}

	private void _E007()
	{
		if (this._E00C.NotAvailable)
		{
			return;
		}
		if (!this._E00B && this._E00A.TotalSeconds < (double)_E35B)
		{
			_E363 = 1.0;
			return;
		}
		TimeSpan timeSpan = this._E009;
		int num = (int)timeSpan.TotalHours;
		int num2 = (int)timeSpan.TotalMinutes;
		int num3 = (int)timeSpan.TotalSeconds;
		if (num >= 5)
		{
			_E363 = Mathf.Max(num3 - num * 3600, 1);
		}
		else if (num >= 1)
		{
			_E363 = Mathf.Max(num3 - num2 * 60, 1);
		}
		else
		{
			_E363 = 1.0;
		}
	}

	private void Update()
	{
		if (!((_E5AD.UtcNow - _E362).TotalSeconds < _E363))
		{
			_E362 = _E5AD.UtcNow;
			if (this._E00C != null && !this._E00C.NotAvailable)
			{
				_E007();
				_E008();
			}
		}
	}

	private void _E008()
	{
		if (!this._E00C.NotAvailable)
		{
			if (!this._E00B && this._E00C.Expired)
			{
				_E35F?.Invoke(this._E00C);
				return;
			}
			TimeSpan timeSpan = this._E009;
			double totalHours = timeSpan.TotalHours;
			_E364 = totalHours < 10.0;
			this._E00C.AvailableTimePassed = _E00A();
			_expirationLabel.SetMonospaceText(_ED3E._E000(243593) + timeSpan.RagfairDateFormatShort() + _ED3E._E000(243593));
			_expirationLabel.color = (_E364 ? OfferView._E006 : OfferView._E003);
			_createdTimeLabel.text = this._E00A.TimePassedFormat();
			bool flag = totalHours >= (double)_ECBD.Settings.offerDurationTimeInHourAfterRemove;
			_removeButton.gameObject.SetActive(this._E010 && flag);
			_E009();
			_E359[_timeLeftImage] = (_E364 ? OfferView._E004 : _E128);
			_E35A[_timeLeftImage] = (_E364 ? OfferView._E005 : _E361);
			_E00E(_timeLeftImage, _E365);
		}
	}

	private void _E009()
	{
		TimeSpan value = _ECBD.Settings.DefaultDuration.MultiplyByPercent(this._E00C.RenewPercent);
		int num = (this._E00C.EndTime - _E5AD.UtcNow).Add(TimeSpan.FromHours(1.0)).CompareTo(value);
		_renewButton.gameObject.SetActive(this._E010 && _E364 && num <= 0);
	}

	private bool _E00A()
	{
		if (this._E00C == null)
		{
			return false;
		}
		bool flag = !this._E00C.NotAvailable;
		bool flag2 = !this._E00C.Expired;
		if (!flag)
		{
			flag2 = true;
			_hoverTooltipArea.Init(_E089.Tooltip, _ED3E._E000(243588));
		}
		else if (this._E00B)
		{
			if (_E1BB != null)
			{
				if (this._E00C.LimitsReached)
				{
					flag = false;
					_hoverTooltipArea.Init(_E089.Tooltip, _ED3E._E000(243621));
				}
				else if (!_E1BB.Available)
				{
					flag = false;
					_hoverTooltipArea.Init(_E089.Tooltip, _E1BB.Disabled ? _ED3E._E000(186065) : _ED3E._E000(243681));
				}
				else
				{
					flag = _E1BB.LoyaltyLevel >= this._E00C.LoyaltyLevel;
					if (!flag)
					{
						_hoverTooltipArea.Init(_E089.Tooltip, string.Format(_ED3E._E000(241707).Localized(), this._E00C.LoyaltyLevel, _E1BB.LoyaltyLevel), rawText: true);
					}
				}
			}
			else
			{
				flag = false;
				_hoverTooltipArea.Init(_E089.Tooltip, _ED3E._E000(241820) + this._E00C.User.Id);
			}
		}
		else
		{
			flag2 = this._E00A.TotalSeconds >= (double)_E35B;
			_availableTimeLabel.text = (this._E00C.StartTime.AddSeconds(_E35B) - _E5AD.UtcNow).RagfairDateFormatLong();
		}
		if (this._E00C.Locked)
		{
			flag = false;
			_hoverTooltipArea.Init(_E089.Tooltip, _ED3E._E000(241805));
		}
		_hoverTooltipArea.enabled = !flag;
		_hoverTooltipArea.CanvasGroup.blocksRaycasts = !flag;
		_availableTimePanel.SetActive(!flag2 && !this._E00B);
		_expirationTimePanel.SetActive(flag2);
		_canvasGroup.SetUnlockStatus(flag && flag2);
		_lockedButton.SetActive(!this._E00C.NotAvailable && !this._E010 && (!flag || !flag2));
		_purchaseButton.gameObject.SetActive(flag && flag2 && !this._E010 && !this._E00E);
		if (!flag2 && this._E00F && _E328.IsSelected(this._E00C))
		{
			_E328.SwitchOfferSelection(this._E00C);
		}
		return flag2;
	}

	private void _E00B(bool expanded)
	{
		if (_E355 != expanded)
		{
			this._E00D.Expanded = expanded;
			_E355 = expanded;
			_minimizeButton.sprite = (expanded ? _expandedSprite : _minimizedSprite);
			_createdTimeLabel.gameObject.SetActive(expanded);
			OfferItemDescription obj = (expanded ? _descriptionExpanded : _descriptionShrunk);
			OfferItemPrice offerItemPrice = (expanded ? _priceExpanded : _priceShrunk);
			OfferItemDescription offerItemDescription = (expanded ? _descriptionShrunk : _descriptionExpanded);
			OfferItemPrice obj2 = (expanded ? _priceShrunk : _priceExpanded);
			offerItemDescription.Close();
			obj2.Close();
			if (this._E00C.NotAvailable || this._E00E)
			{
				_availableTimePanel.SetActive(value: false);
				_expirationTimePanel.SetActive(value: false);
				_purchaseButton.gameObject.SetActive(value: false);
			}
			if (!this._E00C.NotAvailable)
			{
				offerItemPrice.Show(this._E00C, _E089.ItemTooltip, _E092, _E089, _E35C, expanded);
			}
			else
			{
				offerItemPrice.Close();
			}
			obj.Show(_E081, this._E00C, expanded, _E092, _E089, _E35C);
			_merchantInfoView.SetExpandedStatus(expanded);
		}
	}

	private void _E00C()
	{
		if (_descriptionExpanded.gameObject.activeSelf)
		{
			_descriptionExpanded.UpdateItemStackCountLabel();
		}
		else if (_descriptionShrunk.gameObject.activeSelf)
		{
			_descriptionShrunk.UpdateItemStackCountLabel();
		}
	}

	public void OnPointerEnter([NotNull] PointerEventData eventData)
	{
		if (this._E00C != null && !this._E00C.NotAvailable && !this._E00E)
		{
			_E00D(hovered: true, bordered: true);
		}
	}

	public void OnPointerExit([NotNull] PointerEventData eventData)
	{
		if (this._E00C != null)
		{
			_E00D(hovered: false, this._E010);
		}
	}

	private void _E00D(bool hovered, bool bordered)
	{
		_E365 = hovered;
		_borderImage.gameObject.SetActive(bordered);
		Image[] backgroundImages = _backgroundImages;
		foreach (Image image in backgroundImages)
		{
			_E00E(image, hovered);
		}
	}

	private void _E00E(Image image, bool hovered)
	{
		if (_E35A.ContainsKey(image) && _E359.ContainsKey(image))
		{
			image.color = (hovered ? _E35A[image] : _E359[image]);
		}
	}

	public void OnPointerClick([CanBeNull] PointerEventData eventData)
	{
		if (this._E00C.CanBeBought && !this._E00E)
		{
			if (this._E00F)
			{
				_E328.SwitchOfferSelection(this._E00C);
			}
			else if (!this._E00C.Expired)
			{
				_E00B(!this._E00D.Expanded);
			}
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuContextMenu);
		}
	}

	public override void Close()
	{
		_E355 = null;
		_E00D(hovered: false, bordered: false);
		if (_E328 != null)
		{
			_E328.OnOfferProcessing -= _E005;
			_E328.OnYourOfferSold -= _E004;
		}
		if (this._E00C != null)
		{
			this._E00C.OnSelectToPurchase -= _E003;
			this._E00C = null;
		}
		_descriptionShrunk.Close();
		_descriptionExpanded.Close();
		_priceShrunk.Close();
		_priceExpanded.Close();
		_merchantInfoView.Close();
		base.Close();
	}

	[CompilerGenerated]
	private void _E00F(PointerEventData eventData)
	{
		_ECBD ragfair = (this._E010 ? null : _E328);
		_ECBE contextInteractions = new _ECBE(delegate
		{
		}, this._E00C, ragfair);
		_E089.ContextMenu.Show(eventData.position, contextInteractions);
	}

	[CompilerGenerated]
	private void _E010(PointerEventData eventData)
	{
		if (this._E00B || this._E00C.NotAvailable || this._E00E)
		{
			OnPointerClick(null);
			return;
		}
		UpdatableChatMember updatableChatMember = UpdatableChatMember.FindOrCreate(this._E00C.User.Id, (string id) => new UpdatableChatMember(id));
		updatableChatMember.UpdateFromMerchant(this._E00C.User);
		_E467 invitationByProfile = _E2C7.GetInvitationByProfile(updatableChatMember);
		_ECA8 contextInteractions = new _ECA8(_E2C7, null, updatableChatMember, invitationByProfile);
		_E089.ContextMenu.Show(eventData.position, contextInteractions);
	}

	[CompilerGenerated]
	private void _E011()
	{
		_E360?.Invoke(this._E00C, delegate
		{
			_E007();
			_E008();
		});
	}

	[CompilerGenerated]
	private void _E012()
	{
		_E007();
		_E008();
	}

	[CompilerGenerated]
	private void _E013()
	{
		_E35D?.Invoke(this._E00C, delegate
		{
			_E007();
			_E008();
		});
	}

	[CompilerGenerated]
	private void _E014()
	{
		_E007();
		_E008();
	}
}
