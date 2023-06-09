using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Ragfair;

public sealed class RagfairFilterWindow : UIElement
{
	private enum EManageInputFieldType
	{
		InfinityOverflow,
		HundredOverflow,
		DependencyClamp
	}

	[CompilerGenerated]
	private sealed class _E000
	{
		public EManageInputFieldType type;

		public TMP_InputField currentField;

		public TMP_InputField dependField;

		internal void _E000(string arg)
		{
			if (arg.Length <= 0 || arg == _ED3E._E000(59488))
			{
				return;
			}
			if (int.TryParse(arg, out var result))
			{
				int num = result;
				switch (type)
				{
				case EManageInputFieldType.InfinityOverflow:
					if (result <= 0)
					{
						currentField.text = _ED3E._E000(59488);
						return;
					}
					if (dependField.text.ParseToInt() > 0)
					{
						num = Mathf.Clamp(result, dependField.text.ParseToInt(), int.MaxValue);
					}
					break;
				case EManageInputFieldType.DependencyClamp:
					if (dependField.text.ParseToInt() > 0)
					{
						num = Mathf.Clamp(result, 0, dependField.text.ParseToInt());
					}
					break;
				case EManageInputFieldType.HundredOverflow:
					if (dependField.text.ParseToInt() > 0)
					{
						num = Mathf.Clamp(result, dependField.text.ParseToInt(), 100);
					}
					break;
				default:
					throw new ArgumentOutOfRangeException(_ED3E._E000(124643), type, null);
				}
				if (num != result)
				{
					currentField.text = num.ToString();
				}
			}
			else
			{
				_E39B.LogRagfair(_ED3E._E000(242908));
				currentField.text = arg.Remove(arg.Length - 1);
			}
		}
	}

	private const string _E327 = "∞";

	[SerializeField]
	private GameObject _captionPanel;

	[SerializeField]
	private Button _closeButton;

	[SerializeField]
	private DropDownBox _currencyDropdown;

	[SerializeField]
	private TMP_InputField _estimatedPriceFrom;

	[SerializeField]
	private TMP_InputField _estimatedPriceTo;

	[SerializeField]
	private CanvasGroup _priceFromGroup;

	[SerializeField]
	private CanvasGroup _priceToGroup;

	[SerializeField]
	private TMP_InputField _quantityFrom;

	[SerializeField]
	private TMP_InputField _quantityTo;

	[SerializeField]
	private TMP_InputField _conditionFrom;

	[SerializeField]
	private TMP_InputField _conditionTo;

	[SerializeField]
	private Image _currencyImage;

	[SerializeField]
	private UpdatableToggle _hourExpiration;

	[SerializeField]
	private UpdatableToggle _removeBartering;

	[SerializeField]
	private DropDownBox _displayOwnerDropdown;

	[SerializeField]
	private UpdatableToggle _onlyFunctional;

	[SerializeField]
	private UpdatableToggle _rememberSelected;

	[SerializeField]
	private DefaultUIButton _applyButton;

	[SerializeField]
	private DefaultUIButton _resetButton;

	private _ECBD _E328;

	private void Awake()
	{
		_applyButton.OnClick.AddListener(delegate
		{
			int currentIndex = _currencyDropdown.CurrentIndex;
			int currentIndex2 = _displayOwnerDropdown.CurrentIndex;
			FilterRule filterRule = _E328.FilterRule;
			filterRule.CurrencyType = currentIndex;
			filterRule.PriceFrom = ((currentIndex > 0) ? _estimatedPriceFrom.text.ParseToInt() : 0);
			filterRule.PriceTo = ((currentIndex > 0) ? _estimatedPriceTo.text.ParseToInt() : 0);
			filterRule.QuantityFrom = _quantityFrom.text.ParseToInt();
			filterRule.QuantityTo = _quantityTo.text.ParseToInt();
			filterRule.ConditionFrom = _conditionFrom.text.ParseToInt();
			filterRule.ConditionTo = _conditionTo.text.ParseToInt();
			filterRule.OneHourExpiration = _hourExpiration.isOn;
			filterRule.RemoveBartering = _removeBartering.isOn;
			filterRule.OfferOwnerType = currentIndex2;
			filterRule.OnlyFunctional = _onlyFunctional.isOn;
			filterRule.OfferId = 0L;
			_E328.AddSearchesInRule(filterRule, setRule: true);
			if (_rememberSelected.isOn)
			{
				_E328.SaveRuleAsDefault();
			}
			Close();
		});
		_resetButton.OnClick.AddListener(delegate
		{
			switch (_E328.FilterRule.ViewListType)
			{
			case EViewListType.AllOffers:
				_E328.AddSearchesInRule(_E328.DefaultFilterRule, setRule: true);
				_E328.SaveRuleAsDefault();
				break;
			case EViewListType.WishList:
				_E328.AddSearchesInRule(_E328.WishListFilterRule, setRule: true);
				_E328.SaveRuleAsDefault();
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case EViewListType.MyOffers:
			case EViewListType.WeaponBuild:
				break;
			}
			Close();
		});
		_captionPanel.AddComponent<UIDragComponent>().Init(base.RectTransform, putOnTop: false);
		_closeButton.onClick.AddListener(Close);
		_E001(_estimatedPriceFrom, _estimatedPriceTo, EManageInputFieldType.DependencyClamp);
		_E001(_estimatedPriceTo, _estimatedPriceFrom, EManageInputFieldType.InfinityOverflow);
		_E001(_quantityFrom, _quantityTo, EManageInputFieldType.DependencyClamp);
		_E001(_quantityTo, _quantityFrom, EManageInputFieldType.InfinityOverflow);
		_E001(_conditionFrom, _conditionTo, EManageInputFieldType.DependencyClamp);
		_E001(_conditionTo, _conditionFrom, EManageInputFieldType.HundredOverflow);
	}

	public void Show(_ECBD ragfair)
	{
		ShowGameObject();
		_E328 = ragfair;
		FilterRule filterRule = _E328.FilterRule;
		_currencyDropdown.Show((from x in new string[1] { _ED3E._E000(242863) }.Concat(Enum.GetNames(typeof(ECurrencyType)))
			select x.Localized()).ToArray());
		_currencyDropdown.UpdateValue(filterRule.CurrencyType);
		_E000(filterRule.CurrencyType > 0);
		UI.AddDisposable(_currencyDropdown.OnValueChanged.Subscribe(delegate
		{
			_E000(_currencyDropdown.CurrentIndex > 0);
			_currencyImage.gameObject.SetActive(_currencyDropdown.CurrentIndex > 0);
			if (_currencyDropdown.CurrentIndex > 0)
			{
				Sprite smallCurrencySign = EFTHardSettings.Instance.StaticIcons.GetSmallCurrencySign((ECurrencyType)(_currencyDropdown.CurrentIndex - 1));
				_currencyImage.sprite = smallCurrencySign;
			}
		}));
		_currencyImage.gameObject.SetActive(filterRule.CurrencyType > 0);
		_estimatedPriceFrom.text = filterRule.PriceFrom.ToString();
		_estimatedPriceTo.text = filterRule.PriceTo.ToString();
		_quantityTo.text = filterRule.QuantityTo.ToString();
		_quantityFrom.text = filterRule.QuantityFrom.ToString();
		_conditionFrom.text = filterRule.ConditionFrom.ToString();
		_conditionTo.text = filterRule.ConditionTo.ToString();
		_estimatedPriceFrom.onEndEdit.Invoke(_estimatedPriceFrom.text);
		_estimatedPriceTo.onEndEdit.Invoke(_estimatedPriceTo.text);
		_quantityTo.onEndEdit.Invoke(_quantityTo.text);
		_quantityFrom.onEndEdit.Invoke(_quantityFrom.text);
		_conditionFrom.onEndEdit.Invoke(_conditionFrom.text);
		_conditionTo.onEndEdit.Invoke(_conditionTo.text);
		_hourExpiration.UpdateValue(filterRule.OneHourExpiration, sendCallback: false);
		_removeBartering.UpdateValue(filterRule.RemoveBartering, sendCallback: false);
		_displayOwnerDropdown.Show(((IReadOnlyList<string>)Enum.GetNames(typeof(EOfferOwnerType))).Localized(EStringCase.None).ToArray());
		bool available = _E328.Available;
		_displayOwnerDropdown.UpdateValue(available ? filterRule.OfferOwnerType : 0);
		_displayOwnerDropdown.Interactable = available;
		if (!available)
		{
			_displayOwnerDropdown.SetDisabledTooltip(_E328.GetFormattedStatusDescription);
		}
		_onlyFunctional.UpdateValue(filterRule.OnlyFunctional, sendCallback: false);
		_rememberSelected.isOn = false;
	}

	private void _E000(bool status)
	{
		_estimatedPriceFrom.interactable = status;
		_estimatedPriceTo.interactable = status;
		_priceFromGroup.SetUnlockStatus(status);
		_priceToGroup.SetUnlockStatus(status);
	}

	private static void _E001(TMP_InputField currentField, TMP_InputField dependField, EManageInputFieldType type)
	{
		currentField.onEndEdit.AddListener(delegate(string arg)
		{
			if (arg.Length > 0 && !(arg == _ED3E._E000(59488)))
			{
				if (int.TryParse(arg, out var result))
				{
					int num = result;
					switch (type)
					{
					case EManageInputFieldType.InfinityOverflow:
						if (result <= 0)
						{
							currentField.text = _ED3E._E000(59488);
							return;
						}
						if (dependField.text.ParseToInt() > 0)
						{
							num = Mathf.Clamp(result, dependField.text.ParseToInt(), int.MaxValue);
						}
						break;
					case EManageInputFieldType.DependencyClamp:
						if (dependField.text.ParseToInt() > 0)
						{
							num = Mathf.Clamp(result, 0, dependField.text.ParseToInt());
						}
						break;
					case EManageInputFieldType.HundredOverflow:
						if (dependField.text.ParseToInt() > 0)
						{
							num = Mathf.Clamp(result, dependField.text.ParseToInt(), 100);
						}
						break;
					default:
						throw new ArgumentOutOfRangeException(_ED3E._E000(124643), type, null);
					}
					if (num != result)
					{
						currentField.text = num.ToString();
					}
				}
				else
				{
					_E39B.LogRagfair(_ED3E._E000(242908));
					currentField.text = arg.Remove(arg.Length - 1);
				}
			}
		});
	}

	public override void Close()
	{
		_currencyDropdown.Hide();
		_displayOwnerDropdown.Hide();
		base.Close();
	}

	[CompilerGenerated]
	private void _E002()
	{
		int currentIndex = _currencyDropdown.CurrentIndex;
		int currentIndex2 = _displayOwnerDropdown.CurrentIndex;
		FilterRule filterRule = _E328.FilterRule;
		filterRule.CurrencyType = currentIndex;
		filterRule.PriceFrom = ((currentIndex > 0) ? _estimatedPriceFrom.text.ParseToInt() : 0);
		filterRule.PriceTo = ((currentIndex > 0) ? _estimatedPriceTo.text.ParseToInt() : 0);
		filterRule.QuantityFrom = _quantityFrom.text.ParseToInt();
		filterRule.QuantityTo = _quantityTo.text.ParseToInt();
		filterRule.ConditionFrom = _conditionFrom.text.ParseToInt();
		filterRule.ConditionTo = _conditionTo.text.ParseToInt();
		filterRule.OneHourExpiration = _hourExpiration.isOn;
		filterRule.RemoveBartering = _removeBartering.isOn;
		filterRule.OfferOwnerType = currentIndex2;
		filterRule.OnlyFunctional = _onlyFunctional.isOn;
		filterRule.OfferId = 0L;
		_E328.AddSearchesInRule(filterRule, setRule: true);
		if (_rememberSelected.isOn)
		{
			_E328.SaveRuleAsDefault();
		}
		Close();
	}

	[CompilerGenerated]
	private void _E003()
	{
		switch (_E328.FilterRule.ViewListType)
		{
		case EViewListType.AllOffers:
			_E328.AddSearchesInRule(_E328.DefaultFilterRule, setRule: true);
			_E328.SaveRuleAsDefault();
			break;
		case EViewListType.WishList:
			_E328.AddSearchesInRule(_E328.WishListFilterRule, setRule: true);
			_E328.SaveRuleAsDefault();
			break;
		default:
			throw new ArgumentOutOfRangeException();
		case EViewListType.MyOffers:
		case EViewListType.WeaponBuild:
			break;
		}
		Close();
	}

	[CompilerGenerated]
	private void _E004()
	{
		_E000(_currencyDropdown.CurrentIndex > 0);
		_currencyImage.gameObject.SetActive(_currencyDropdown.CurrentIndex > 0);
		if (_currencyDropdown.CurrentIndex > 0)
		{
			Sprite smallCurrencySign = EFTHardSettings.Instance.StaticIcons.GetSmallCurrencySign((ECurrencyType)(_currencyDropdown.CurrentIndex - 1));
			_currencyImage.sprite = smallCurrencySign;
		}
	}
}
