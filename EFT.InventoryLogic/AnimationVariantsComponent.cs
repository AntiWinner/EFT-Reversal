namespace EFT.InventoryLogic;

public class AnimationVariantsComponent : _EB19
{
	private readonly IAnimationVariantsComponentTemplate _E000;

	public int VariantsNumber => _E000.AnimationVariantsNumber;

	public AnimationVariantsComponent(Item item, IAnimationVariantsComponentTemplate template)
		: base(item)
	{
		_E000 = template;
	}
}
