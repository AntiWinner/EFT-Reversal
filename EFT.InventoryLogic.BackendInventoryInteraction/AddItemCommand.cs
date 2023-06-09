using System;
using Newtonsoft.Json;

namespace EFT.InventoryLogic.BackendInventoryInteraction;

[Serializable]
public sealed class AddItemCommand : _EB6F
{
	[JsonProperty("Action")]
	public string Action = _ED3E._E000(201175);

	[JsonProperty("item")]
	public string Item;

	[JsonProperty("container")]
	public _EB60._E000 Container;
}
