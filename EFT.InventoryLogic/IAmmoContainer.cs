using System.Collections.Generic;
using JetBrains.Annotations;

namespace EFT.InventoryLogic;

public interface IAmmoContainer
{
	StackSlot Cartridges { get; }

	int Count { get; }

	IEnumerable<IContainer> Containers { get; }

	[CanBeNull]
	IContainer GetContainer(string containerId);

	_EA12 GetBulletAtPosition(int index);
}
