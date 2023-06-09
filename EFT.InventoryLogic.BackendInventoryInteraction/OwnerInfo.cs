using System;
using Newtonsoft.Json;

namespace EFT.InventoryLogic.BackendInventoryInteraction;

[Serializable]
public sealed class OwnerInfo
{
	[JsonProperty("id")]
	public string Id;

	[JsonProperty("type")]
	public EOwnerType Type;
}
