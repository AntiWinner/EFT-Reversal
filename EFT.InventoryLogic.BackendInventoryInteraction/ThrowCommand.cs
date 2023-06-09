using System;
using Newtonsoft.Json;

namespace EFT.InventoryLogic.BackendInventoryInteraction;

[Serializable]
public sealed class ThrowCommand : _EB6F
{
	[JsonProperty("Action")]
	public string Action = _ED3E._E000(201275);

	[JsonProperty("item")]
	public string Item;
}
