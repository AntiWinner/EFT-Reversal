using System;
using Newtonsoft.Json;

namespace EFT.InventoryLogic.BackendInventoryInteraction;

[Serializable]
public sealed class TransferCommand : _EB70
{
	[JsonProperty("Action")]
	public string Action = _ED3E._E000(197696);

	[JsonProperty("item")]
	public string Item;

	[JsonProperty("with")]
	public string With;

	[JsonProperty("count")]
	public int Count;
}
