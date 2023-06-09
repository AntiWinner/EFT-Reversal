using System;
using Newtonsoft.Json;

namespace EFT.InventoryLogic.BackendInventoryInteraction;

[Serializable]
public sealed class HealCommand : _EB70
{
	[JsonProperty("Action")]
	public string Action = _ED3E._E000(197791);

	[JsonProperty("item")]
	public string Item;

	[JsonProperty("part")]
	public EBodyPart Part;

	[JsonProperty("count")]
	public int Count;

	[JsonProperty("time")]
	public int Time;
}
