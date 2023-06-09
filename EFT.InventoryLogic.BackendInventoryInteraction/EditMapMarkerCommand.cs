using System;
using Newtonsoft.Json;

namespace EFT.InventoryLogic.BackendInventoryInteraction;

[Serializable]
public sealed class EditMapMarkerCommand : _EB6F
{
	[JsonProperty("Action")]
	public string Action = _ED3E._E000(197821);

	[JsonProperty("item")]
	public string Item;

	[JsonProperty("X")]
	public int X;

	[JsonProperty("Y")]
	public int Y;

	[JsonProperty("mapMarker")]
	public MapMarker MapMarker;
}
