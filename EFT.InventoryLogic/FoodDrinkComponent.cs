namespace EFT.InventoryLogic;

public sealed class FoodDrinkComponent : _EB19
{
	[_E63C]
	public float HpPercent;

	private readonly _E9DA _E000;

	public float MaxResource => _E000.MaxResource;

	public float RelativeResource => HpPercent / MaxResource * 100f;

	public FoodDrinkComponent(Item item, _E9DA template)
		: base(item)
	{
		_E000 = template;
		HpPercent = template.MaxResource;
	}
}
