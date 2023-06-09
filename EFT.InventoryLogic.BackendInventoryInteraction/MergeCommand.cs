using System;
using Newtonsoft.Json;

namespace EFT.InventoryLogic.BackendInventoryInteraction;

[Serializable]
public sealed class MergeCommand : _EB70
{
	[JsonProperty("Action")]
	public string Action = _ED3E._E000(197724);

	[JsonProperty("item")]
	public string Item;

	[JsonProperty("with")]
	public string With;
}
