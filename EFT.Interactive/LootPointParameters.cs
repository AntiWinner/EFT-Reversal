using System;
using JsonType;
using Newtonsoft.Json;

namespace EFT.Interactive;

[Serializable]
public class LootPointParameters
{
	public string Id;

	public bool Enabled;

	public bool useGravity = true;

	public bool randomRotation;

	public float ChanceModifier;

	public bool IsAlwaysSpawn;

	public ELootRarity Rarity;

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string[] LootSets;

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string[] FilterInclusive;

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string[] FilterExclusive;

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public LootableContainerParameters[] FilterExtended;

	public ClassVector3 Position;

	public ClassVector3 Rotation;

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public WeightedLootPointSpawnPosition[] GroupPositions;

	public bool IsStatic;

	public bool IsContainer;

	public bool IsGroupPosition => GroupPositions != null;
}
