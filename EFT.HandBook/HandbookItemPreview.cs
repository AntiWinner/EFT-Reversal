using EFT.InventoryLogic;
using EFT.UI;
using UnityEngine;

namespace EFT.HandBook;

public sealed class HandbookItemPreview : UIElement
{
	[SerializeField]
	private ItemSpecificationPanel _itemSpecificationPanel;

	private ItemUiContext _E089;

	public void Show(ItemUiContext itemUiContext)
	{
		_E089 = itemUiContext;
	}

	public void ChooseEntity(Item item)
	{
		if (_itemSpecificationPanel.gameObject.activeSelf)
		{
			_itemSpecificationPanel.Close();
		}
		UI.AddDisposable(_itemSpecificationPanel.Close);
		ShowGameObject();
		_EB64 itemContext = new _EB64(item, EItemViewType.Handbook);
		_E089.InitSpecificationPanel(_itemSpecificationPanel, itemContext, new _EBA9(itemContext, _E089));
	}

	public void ClearEntity()
	{
		if (_itemSpecificationPanel != null)
		{
			_itemSpecificationPanel.Close();
		}
		HideGameObject();
	}
}
