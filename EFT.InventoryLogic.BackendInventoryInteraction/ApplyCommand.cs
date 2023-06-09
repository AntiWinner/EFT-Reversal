using System;
using Newtonsoft.Json;

namespace EFT.InventoryLogic.BackendInventoryInteraction;

[Serializable]
public sealed class ApplyCommand : _EB70
{
	[JsonProperty("Action")]
	public string Action = _ED3E._E000(197729);

	[JsonProperty("item")]
	public string Item;

	[JsonProperty("target")]
	public string Target;
}
