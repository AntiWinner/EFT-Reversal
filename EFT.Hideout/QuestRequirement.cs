using System;
using Newtonsoft.Json;

namespace EFT.Hideout;

[Serializable]
public sealed class QuestRequirement : Requirement
{
	[JsonProperty("questId")]
	public string QuestId;

	public override ERequirementType Type => ERequirementType.QuestComplete;
}
