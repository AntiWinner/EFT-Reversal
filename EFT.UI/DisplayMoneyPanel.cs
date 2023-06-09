using System.Collections.Generic;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT.UI;

public sealed class DisplayMoneyPanel : UIElement
{
	[SerializeField]
	private CustomTextMeshProUGUI _roubles;

	[SerializeField]
	private CustomTextMeshProUGUI _euros;

	[SerializeField]
	private CustomTextMeshProUGUI _dollars;

	public void Show(IEnumerable<Item> inventoryItems)
	{
		ShowGameObject();
		_EC6C moneyTuple = TradingPlayerPanel.GetMoneyTuple(_EB0E.GetMoneySums(inventoryItems));
		_roubles.text = moneyTuple.Rubles;
		_euros.text = moneyTuple.Euros;
		_dollars.text = moneyTuple.Dollars;
	}
}
