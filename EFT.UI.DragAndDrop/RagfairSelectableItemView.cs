using EFT.InventoryLogic;

namespace EFT.UI.DragAndDrop;

public sealed class RagfairSelectableItemView : SelectableItemView
{
	public new static RagfairSelectableItemView Create(Item item, _EB66 sourceContext, ItemRotation rotation, _EB1E itemController, IItemOwner itemOwner, FilterPanel filterPanel, _EC9E container, ItemUiContext itemUiContext, _ECB1 insurance)
	{
		RagfairSelectableItemView obj = (RagfairSelectableItemView)ItemViewFactory.CreateFromPool<RagfairSelectableItemView>(_ED3E._E000(236382)).NewSelectableItemView(item, sourceContext, rotation, itemController, itemOwner, filterPanel, container, itemUiContext, insurance);
		obj.Init();
		return obj;
	}

	protected override void SetAvailability(bool available, string tooltip)
	{
		base.SelectedMarkBack.gameObject.SetActive(available);
		base.SetAvailability(available, tooltip);
	}
}
