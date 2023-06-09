using System;
using Newtonsoft.Json;

namespace EFT.Interactive;

[Serializable]
public class LootPointTemplate
{
	[JsonProperty("id")]
	public string Id;

	[JsonProperty("systemName")]
	public string Name;

	[JsonProperty("filterInclusive")]
	public string[] FilterInclusive;

	[JsonProperty("filterExclusive")]
	public string[] FilterExclusive;
}
