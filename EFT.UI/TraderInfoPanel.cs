using System;
using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class TraderInfoPanel : UIElement
{
	private const double _E011 = 1.0;

	[SerializeField]
	private CustomTextMeshProUGUI _nickName;

	[SerializeField]
	private RankPanel _rankPanel;

	[SerializeField]
	private TraderAvatar _traderAvatar;

	[SerializeField]
	private Image _currency;

	[SerializeField]
	private CustomTextMeshProUGUI _money;

	[SerializeField]
	private CustomTextMeshProUGUI _discount;

	[SerializeField]
	private CustomTextMeshProUGUI _timeLeft;

	[SerializeField]
	private CustomTextMeshProUGUI _standing;

	[SerializeField]
	private GameObject _notAvailableIcon;

	[SerializeField]
	private Button _reloadButton;

	private _E8B2 _E1BB;

	private DateTime _E012;

	private Profile._E001 _E1FB;

	private _E5CB.TraderSettings _E203;

	private void Awake()
	{
		_reloadButton.onClick.AddListener(_E005);
	}

	public void Show(_E8B2 trader, _E935 questController)
	{
		ShowGameObject();
		_E1BB = trader;
		_E1FB = _E1BB.Info;
		_E203 = _E1FB.Settings;
		_traderAvatar.Show(_E1BB.Info, questController);
		UI.AddDisposable(_traderAvatar);
		_E001();
		_E1FB.OnLoyaltyChanged += _E000;
		_E1FB.OnStandingChanged += _E002;
		UI.AddDisposable(delegate
		{
			_E1FB.OnLoyaltyChanged -= _E000;
			_E1FB.OnStandingChanged -= _E002;
		});
	}

	private void _E000()
	{
		_rankPanel.Show(_E1FB.LoyaltyLevel, _E1FB.MaxLoyaltyLevel);
	}

	private void _E001()
	{
		float discount = _E203.Discount;
		_discount.text = ((discount > 0f) ? _ED3E._E000(29692) : "") + discount + _ED3E._E000(149464);
		_discount.color = ((discount > 0f) ? Color.red : ((discount < 0f) ? Color.cyan : Color.yellow));
		_notAvailableIcon.SetActive(!_E1FB.Available);
		_nickName.text = _E203.Nickname.Localized();
		_E000();
		_E002();
		_E003();
	}

	private void _E002()
	{
		_standing.text = string.Format(_ED3E._E000(260804), _E1FB.Standing, _ECA1.GetStandingRating(_E1FB.Standing).Localized());
	}

	private void _E003()
	{
		long money = _E203.Currency switch
		{
			ECurrencyType.USD => _E203.BalanceUSD, 
			ECurrencyType.EUR => _E203.BalanceEUR, 
			ECurrencyType.RUB => _E203.BalanceRUB, 
			_ => throw new ArgumentOutOfRangeException(), 
		};
		_money.text = _ECA1.GetMoneyString(money);
		_currency.sprite = _ECA1.GetCurrencyIcon(_E203.Currency);
	}

	private void _E004()
	{
		bool assortmentUpdateTimeout = _E1BB.AssortmentUpdateTimeout;
		bool assortmentLoading = _E1BB.AssortmentLoading;
		_reloadButton.interactable = !assortmentUpdateTimeout && !assortmentLoading;
		_reloadButton.GetComponent<Animation>().enabled = assortmentLoading;
		if (!assortmentLoading)
		{
			_reloadButton.transform.localRotation = Quaternion.identity;
		}
	}

	private void Update()
	{
		DateTime utcNow = _E5AD.UtcNow;
		if (_E1FB != null && !((utcNow - _E012).TotalSeconds < 1.0))
		{
			_E012 = utcNow;
			TimeSpan timeLeft = _E5AD.UnixEpoch.AddSeconds(_E1FB.NextResupply) - utcNow;
			bool flag = timeLeft.Ticks <= 0;
			_timeLeft.SetMonospaceText((!flag) ? timeLeft.TraderFormat() : TimeSpan.Zero.TraderFormat());
			if (!_E1BB.AssortmentUpdateTimeout && !_E1BB.AssortmentLoading && (flag || Input.GetKeyDown(KeyCode.F5)))
			{
				_E005();
			}
			_E004();
		}
	}

	private void _E005()
	{
		_E1BB.RefreshAssortment(createIfNotExists: true, ignoreTimeout: false).HandleExceptions();
	}

	[CompilerGenerated]
	private void _E006()
	{
		_E1FB.OnLoyaltyChanged -= _E000;
		_E1FB.OnStandingChanged -= _E002;
	}
}
