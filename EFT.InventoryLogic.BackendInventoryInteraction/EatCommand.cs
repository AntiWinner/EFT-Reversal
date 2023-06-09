using System;
using Newtonsoft.Json;

namespace EFT.InventoryLogic.BackendInventoryInteraction;

[Serializable]
public sealed class EatCommand : _EB70
{
	[JsonProperty("Action")]
	public string Action = _ED3E._E000(197780);

	[JsonProperty("item")]
	public string Item;

	[JsonProperty("count")]
	public int Count;

	[JsonProperty("time")]
	public int Time;
}
