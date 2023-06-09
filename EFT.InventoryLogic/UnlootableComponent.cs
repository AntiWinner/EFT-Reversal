namespace EFT.InventoryLogic;

public class UnlootableComponent : _EB19
{
	public readonly IUnlootableComponentTemplate Template;

	public UnlootableComponent(Item item, IUnlootableComponentTemplate template)
		: base(item)
	{
		Template = template;
	}

	public bool IsUnlootableFrom(IContainer container)
	{
		if (!container.ID.Contains(Template.SlotName))
		{
			return false;
		}
		if (container.ParentItem.Owner is _EAEB obj)
		{
			return Template.Side.CheckSide(obj.Side);
		}
		return false;
	}
}
