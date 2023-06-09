using System;
using Newtonsoft.Json;

namespace EFT.InventoryLogic.BackendInventoryInteraction;

[Serializable]
public sealed class MoveCommand : _EB70
{
	[JsonProperty("Action")]
	public string Action = _ED3E._E000(197673);

	[JsonProperty("item")]
	public string Item;

	[JsonProperty("to")]
	public _EB60._E000 To;
}
