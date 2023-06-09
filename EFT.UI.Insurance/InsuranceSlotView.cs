using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;

namespace EFT.UI.Insurance;

public class InsuranceSlotView : SlotView
{
	protected override ItemView CreateItemViewKernel(Item item, IItemOwner itemOwner)
	{
		return InsuranceSlotItemView.Create(item, base.ParentItemContext, InventoryController, itemOwner ?? ItemOwner, ItemUiContext, Skills, InsuranceCompany);
	}
}
