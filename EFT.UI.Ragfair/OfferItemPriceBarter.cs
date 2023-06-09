using EFT.InventoryLogic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI.Ragfair;

public sealed class OfferItemPriceBarter : UIElement, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	[SerializeField]
	private GameObject _separator;

	[SerializeField]
	private GameObject _barterIcon;

	[SerializeField]
	private TextMeshProUGUI _requirementName;

	private IExchangeRequirement _E033;

	private ItemTooltip _E02A;

	private Offer _E354;

	private _EAED _E092;

	private ItemUiContext _E089;

	private _ECB1 _E35C;

	private bool _E355;

	public void Show(IExchangeRequirement requirement, ItemTooltip tooltip, Offer offer, _EAED inventoryController, ItemUiContext itemUiContext, _ECB1 insuranceCompany, int index, bool expanded)
	{
		_E033 = requirement;
		_E02A = tooltip;
		_E354 = offer;
		_E092 = inventoryController;
		_E089 = itemUiContext;
		_E35C = insuranceCompany;
		_E355 = expanded;
		_barterIcon.SetActive(!_E355);
		_requirementName.gameObject.SetActive(_E355);
		if (_E355)
		{
			_requirementName.text = requirement.ItemName.Localized() + _ED3E._E000(54246) + _ECA1.GetMoneyString(requirement.IntCount) + _ED3E._E000(27308);
		}
		if (_separator != null)
		{
			_separator.SetActive(index > 0);
		}
		ShowGameObject();
	}

	public void OnPointerEnter([NotNull] PointerEventData eventData)
	{
		Item item = _E033.Item;
		DogtagComponent itemComponent = item.GetItemComponent<DogtagComponent>();
		string text = ((!(_E033 is HandoverRequirement handoverRequirement)) ? (_E033.ItemName + _ED3E._E000(242203) + _E033.IntCount + _ED3E._E000(242193)) : ((itemComponent != null) ? (_ED3E._E000(200879).Localized() + _ED3E._E000(255020) + handoverRequirement.Level + _ED3E._E000(18502) + _ED3E._E000(255032).Localized() + ((handoverRequirement.Side != EDogtagExchangeSide.Any) ? (_ED3E._E000(10270) + handoverRequirement.Side) : "").ToUpper()) : (handoverRequirement.ItemName + _ED3E._E000(242203) + _E033.IntCount + _ED3E._E000(242193))));
		if (item is _EA40 && _E033.OnlyFunctional)
		{
			text = text + _ED3E._E000(216105) + _ED3E._E000(242163).Localized() + _ED3E._E000(27308);
		}
		_E02A.Show(text, 0.5f, _E354, item, _E092, _E089, _E35C);
	}

	public void OnPointerExit([NotNull] PointerEventData eventData)
	{
		_E02A.Close();
	}

	public void OnPointerClick([NotNull] PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right)
		{
			if (_E02A.gameObject.activeSelf)
			{
				_E02A.Close();
			}
			if (!(_E02A.ItemView == null))
			{
				_E089.ShowContextMenu(_E02A.ItemView.ItemContext, eventData.pressPosition);
			}
		}
	}
}
