using System.Collections.Generic;
using JetBrains.Annotations;

namespace EFT.InventoryLogic;

public interface IContainer
{
	string ID { get; }

	IEnumerable<Item> Items { get; }

	ItemFilter[] Filters { get; set; }

	Item ParentItem { get; }

	EParentMergeType MergeContainerWithChildren { get; }

	int GetHashSum();

	[CanBeNull]
	Item FindItem(string itemId);
}
