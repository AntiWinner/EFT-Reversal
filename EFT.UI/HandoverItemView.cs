using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;

namespace EFT.UI;

public sealed class HandoverItemView : BaseSelectableItemView
{
	public static HandoverItemView Create(Item item, _EB66 sourceContext, ItemRotation rotation, _EB1E itemController, IItemOwner itemOwner, FilterPanel filterPanel, _EC9E container, ItemUiContext itemUiContext, _ECB1 insurance)
	{
		HandoverItemView handoverItemView = ItemViewFactory.CreateFromPool<HandoverItemView>(_ED3E._E000(253695))._E000(item, sourceContext, rotation, itemController, itemOwner, filterPanel, container, itemUiContext, insurance);
		handoverItemView.Init();
		return handoverItemView;
	}

	private HandoverItemView _E000(Item item, _EB66 sourceContext, ItemRotation rotation, _EB1E itemController, IItemOwner itemOwner, FilterPanel filterPanel, _EC9E container, ItemUiContext itemUiContext, _ECB1 insurance)
	{
		NewBaseSelectableItemView(item, sourceContext, rotation, itemController, itemOwner, filterPanel, container, itemUiContext, insurance);
		return this;
	}

	protected override void SetAvailability(bool available, string tooltip)
	{
	}
}
