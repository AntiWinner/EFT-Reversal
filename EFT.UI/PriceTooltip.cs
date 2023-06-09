using EFT.InventoryLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class PriceTooltip : SimpleTooltip
{
	private const string _E178 = "Trader can't buy this item";

	private const string _E179 = "The item has been sold";

	[SerializeField]
	private Image _currencyIcon;

	[SerializeField]
	private TextMeshProUGUI _price;

	[SerializeField]
	private Color _priceColor;

	[SerializeField]
	private Color _unbuyableColor;

	public void Show(EOwnerType ownerType, string text, int price, string currencyTemplate)
	{
		Show(text);
		bool flag = string.IsNullOrEmpty(currencyTemplate);
		bool flag2 = !flag && price > 0;
		_currencyIcon.gameObject.SetActive(flag2);
		_price.color = (flag2 ? _priceColor : _unbuyableColor);
		if (flag2)
		{
			_price.text = price.ToString();
			_currencyIcon.sprite = EFTHardSettings.Instance.StaticIcons.GetSmallCurrencySign(currencyTemplate);
			_currencyIcon.SetNativeSize();
		}
		else if (flag)
		{
			_price.text = ((ownerType == EOwnerType.Trader) ? _ED3E._E000(248257).Localized() : _ED3E._E000(248286).Localized());
		}
		else
		{
			_price.text = string.Empty;
		}
	}
}
