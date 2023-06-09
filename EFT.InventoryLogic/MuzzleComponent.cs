namespace EFT.InventoryLogic;

public class MuzzleComponent : _EB19
{
	public readonly _E9E5 Template;

	public MuzzleComponent(Item item, _E9E5 template)
		: base(item)
	{
		Template = template;
	}
}
