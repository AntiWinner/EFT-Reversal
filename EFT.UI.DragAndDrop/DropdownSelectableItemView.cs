using EFT.InventoryLogic;

namespace EFT.UI.DragAndDrop;

public sealed class DropdownSelectableItemView : SelectableItemView
{
	public new static DropdownSelectableItemView Create(Item item, _EB66 sourceContext, ItemRotation rotation, _EB1E itemController, IItemOwner itemOwner, FilterPanel filterPanel, _EC9E container, ItemUiContext itemUiContext, _ECB1 insurance)
	{
		return ItemViewFactory.CreateFromPool<DropdownSelectableItemView>(_ED3E._E000(235664))._E000(item, sourceContext, rotation, itemController, itemOwner, filterPanel, container, itemUiContext, insurance);
	}

	private DropdownSelectableItemView _E000(Item item, _EB66 sourceContext, ItemRotation rotation, _EB1E itemController, IItemOwner itemOwner, FilterPanel filterPanel, _EC9E container, ItemUiContext itemUiContext, _ECB1 insurance)
	{
		NewSelectableItemView(item, sourceContext, rotation, itemController, itemOwner, filterPanel, container, itemUiContext, insurance);
		Init();
		return this;
	}
}
