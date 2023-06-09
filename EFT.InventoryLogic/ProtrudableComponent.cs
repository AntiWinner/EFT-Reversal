namespace EFT.InventoryLogic;

public class ProtrudableComponent : _EB19
{
	public ProtrudableComponent(Item item)
		: base(item)
	{
	}

	public bool IsProtruding()
	{
		FoldableComponent itemComponent = Item.GetItemComponent<FoldableComponent>();
		if (itemComponent != null)
		{
			return !itemComponent.Folded;
		}
		return true;
	}
}
