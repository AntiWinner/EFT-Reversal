namespace EFT.InventoryLogic;

public class BarrelComponent : _EB19
{
	public readonly _E9D1 Template;

	public BarrelComponent(Item item, _E9D1 template)
		: base(item)
	{
		Template = template;
	}
}
