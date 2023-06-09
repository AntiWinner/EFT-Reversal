using System.Runtime.CompilerServices;

namespace EFT.InventoryLogic;

public class ThermalVisionComponent : _EB19, ITogglableComponentContainer<TogglableComponent>, ITogglableComponentContainer, IItemComponent
{
	public enum SelectablePalette
	{
		Fusion,
		Rainbow,
		WhiteHot,
		BlackHot
	}

	public readonly _E9EB Template;

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

	public ThermalVisionComponent(Item item, _E9EB template, TogglableComponent togglable)
		: base(item)
	{
		Template = template;
		_E00F = togglable;
	}
}
