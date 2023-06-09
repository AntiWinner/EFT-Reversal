using Newtonsoft.Json;

namespace EFT.Quests;

public class ConditionLaunchFlare : Condition
{
	[JsonProperty("target")]
	public string zoneID;
}
