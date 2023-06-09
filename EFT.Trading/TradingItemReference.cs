using System;
using EFT.InventoryLogic;
using Newtonsoft.Json;

namespace EFT.Trading;

[Serializable]
public struct TradingItemReference
{
	[JsonProperty("Item")]
	public Item Item;

	[JsonProperty("Count")]
	public int Count;

	[JsonProperty("SchemeId")]
	public int SchemeId;
}
