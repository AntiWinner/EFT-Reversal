using System;
using Newtonsoft.Json;

namespace EFT.InventoryLogic.BackendInventoryInteraction;

[Serializable]
public sealed class SplitCommand : _EB70
{
	[JsonProperty("Action")]
	public string Action = _ED3E._E000(197670);

	[JsonProperty("item")]
	public string Item;

	[JsonProperty("container")]
	public _EB60._E000 Container;

	[JsonProperty("count")]
	public int Count;
}
