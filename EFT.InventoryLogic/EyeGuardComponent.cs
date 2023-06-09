namespace EFT.InventoryLogic;

public class EyeGuardComponent : _EB19
{
	public readonly _EA19 Template;

	public EyeGuardComponent(Item item, _EA19 template)
		: base(item)
	{
		Template = template;
	}
}
