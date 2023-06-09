using System;
using Newtonsoft.Json;

namespace EFT.InventoryLogic.BackendInventoryInteraction;

[Serializable]
public sealed class CreateMapMarkerCommand : _EB6F
{
	[JsonProperty("Action")]
	public string Action = _ED3E._E000(197773);

	[JsonProperty("item")]
	public string Item;

	[JsonProperty("mapMarker")]
	public MapMarker MapMarker;
}
