using System.Runtime.CompilerServices;

namespace EFT.InventoryLogic;

public class NightVisionComponent : _EB19, ITogglableComponentContainer<TogglableComponent>, ITogglableComponentContainer, IItemComponent
{
	public enum EMask
	{
		Thermal,
		Anvis,
		Binocular,
		GasMask,
		OldMonocular
	}

	public readonly _E9E6 Template;

	[CompilerGenerated]
	private readonly TogglableComponent _E00F;

	public TogglableComponent Togglable
	{
		[CompilerGenerated]
		get
		{
			return _E00F;
		}
	}

	ITogglableComponent ITogglableComponentContainer.Togglable => Togglable;

	public NightVisionComponent(Item item, _E9E6 template, TogglableComponent togglable)
		: base(item)
	{
		Template = template;
		_E00F = togglable;
	}
}
