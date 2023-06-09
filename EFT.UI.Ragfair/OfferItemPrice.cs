using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Ragfair;

public sealed class OfferItemPrice : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public ItemTooltip tooltip;

		public Offer offer;

		public _EAED inventoryController;

		public ItemUiContext itemUiContext;

		public _ECB1 insuranceCompany;

		public bool expanded;
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public int i;

		public _E000 CS_0024_003C_003E8__locals1;

		internal void _E000(IExchangeRequirement item, OfferItemPriceBarter view)
		{
			view.Show(item, CS_0024_003C_003E8__locals1.tooltip, CS_0024_003C_003E8__locals1.offer, CS_0024_003C_003E8__locals1.inventoryController, CS_0024_003C_003E8__locals1.itemUiContext, CS_0024_003C_003E8__locals1.insuranceCompany, i, CS_0024_003C_003E8__locals1.expanded);
			i++;
		}
	}

	[SerializeField]
	private OfferItemPriceBarter _barterRequirementView;

	[SerializeField]
	private RectTransform _barterRequirementsContainer;

	[SerializeField]
	private TextMeshProUGUI _priceLabel;

	[SerializeField]
	private Image _priceIcon;

	[SerializeField]
	private GameObject _estimatedPriceObject;

	[SerializeField]
	private TextMeshProUGUI _estimatedPriceLabel;

	[SerializeField]
	private TextMeshProUGUI _packLabel;

	[SerializeField]
	private GridLayoutGroup _gridLayout;

	private _EC79<IExchangeRequirement, OfferItemPriceBarter> _E357;

	public void Show(Offer offer, ItemTooltip tooltip, _EAED inventoryController, ItemUiContext itemUiContext, _ECB1 insuranceCompany, bool expanded)
	{
		if (offer.NotAvailable)
		{
			return;
		}
		ShowGameObject();
		bool onlyMoney = offer.OnlyMoney;
		_priceLabel.gameObject.SetActive(onlyMoney);
		_estimatedPriceObject.gameObject.SetActive(!onlyMoney);
		_priceIcon.gameObject.SetActive(onlyMoney);
		if (onlyMoney)
		{
			_priceLabel.text = offer.Requirements[0].IntCount.FormatSeparate(_ED3E._E000(18502));
			string templateId = offer.Requirements[0].TemplateId;
			_priceIcon.sprite = EFTHardSettings.Instance.StaticIcons.GetBigCurrencySign(templateId);
			ECurrencyType currencyTypeById = _EA10.GetCurrencyTypeById(templateId);
			_EA10.CurrencyColors.TryGetValue(currencyTypeById, out var value);
			_priceIcon.color = value;
		}
		else
		{
			_estimatedPriceLabel.text = _ED3E._E000(27312) + _ED3E._E000(243535).Localized() + _ED3E._E000(18502) + offer.SummaryCost.FormatSeparate(_ED3E._E000(18502));
			int i = 0;
			_E357?.Dispose();
			_E357 = UI.AddViewList(offer.Requirements, _barterRequirementView, _barterRequirementsContainer, delegate(IExchangeRequirement item, OfferItemPriceBarter view)
			{
				view.Show(item, tooltip, offer, inventoryController, itemUiContext, insuranceCompany, i, expanded);
				i++;
			});
			if (_gridLayout != null)
			{
				_gridLayout.constraintCount = Mathf.Clamp(i, 0, 3);
			}
		}
		_barterRequirementsContainer.gameObject.SetActive(!onlyMoney);
		_packLabel.text = ((offer.SellInOnePiece && offer.Item.StackObjectsCount > 1) ? string.Format(_ED3E._E000(243565).Localized(), offer.Item.StackObjectsCount) : _ED3E._E000(243580).Localized());
	}
}
