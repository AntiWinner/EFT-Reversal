using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using EFT.UI.Ragfair;
using UnityEngine;

namespace EFT.UI;

public sealed class ItemTooltip : SimpleTooltip
{
	[SerializeField]
	private GameObject _itemObject;

	[SerializeField]
	private RagfairOfferItemView _itemView;

	public RagfairOfferItemView ItemView => _itemView;

	public void Show(string text, float delay, Offer offer, Item item, _EAED inventoryController, ItemUiContext itemUiContext, _ECB1 insuranceCompany)
	{
		Show(text, null, delay);
		_itemObject.gameObject.SetActive(value: true);
		_itemView.Show(offer, item, ItemRotation.Horizontal, expanded: true, inventoryController, inventoryController, itemUiContext, insuranceCompany);
	}

	public override void Close()
	{
		base.Close();
		_itemObject.gameObject.SetActive(value: false);
	}
}
