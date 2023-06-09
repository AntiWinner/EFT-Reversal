using Newtonsoft.Json;

namespace EFT.Hideout;

public sealed class QteResult
{
	[JsonProperty("Energy")]
	public float Energy { get; private set; }

	[JsonProperty("Hydration")]
	public float Hydration { get; private set; }

	[JsonProperty("RewardsRange")]
	public QteEffect[] Effects { get; private set; }
}
