using System;
using Newtonsoft.Json;

namespace EFT.InventoryLogic.BackendInventoryInteraction;

[Serializable]
public sealed class RecodeCommand : _EB6F
{
	[JsonProperty("Action")]
	public string Action = _ED3E._E000(197738);

	[JsonProperty("item")]
	public string Item;

	[JsonProperty("value")]
	public bool Value;
}
