using TMPro;
using UnityEngine;

namespace EFT.UI.Insurance;

public class InsuredItemPanel : UIElement
{
	[SerializeField]
	private TextMeshProUGUI _itemNameLabel;

	[SerializeField]
	private TextMeshProUGUI _insurerNamePanel;

	[SerializeField]
	private TradeItemType _itemType;

	public void Show(string itemName, string insurerName, EItemType itemType)
	{
		ShowGameObject();
		_itemNameLabel.text = itemName.Localized();
		_insurerNamePanel.text = insurerName;
		_itemType.Show(itemType);
	}
}
