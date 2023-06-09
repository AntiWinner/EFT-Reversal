using System;
using Newtonsoft.Json;

namespace EFT.InventoryLogic.BackendInventoryInteraction;

[Serializable]
public sealed class UnbindCommand : _EB6F
{
	[JsonProperty("Action")]
	public string Action = _ED3E._E000(225253);

	[JsonProperty("item")]
	public string Item;

	[JsonProperty("index")]
	public EBoundItem Index;
}
