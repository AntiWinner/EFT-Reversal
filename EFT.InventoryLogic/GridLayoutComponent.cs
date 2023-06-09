namespace EFT.InventoryLogic;

public class GridLayoutComponent : _EB19
{
	public readonly _E9DB Template;

	public GridLayoutComponent(Item item, _E9DB template)
		: base(item)
	{
		Template = template;
	}
}
