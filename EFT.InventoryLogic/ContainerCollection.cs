using System.Collections.Generic;
using JetBrains.Annotations;

namespace EFT.InventoryLogic;

public abstract class ContainerCollection : Item
{
	public abstract IEnumerable<IContainer> Containers { get; }

	[CanBeNull]
	public abstract IContainer GetContainer(string containerId);

	public abstract Item FindItem(string itemId);

	protected ContainerCollection(string id, ItemTemplate template)
		: base(id, template)
	{
	}
}
