using Newtonsoft.Json;

namespace EFT.InventoryLogic.BackendInventoryInteraction;

public interface ICommandWithItem
{
	[JsonIgnore]
	string ItemTemplate { get; }
}
