using System;
using Newtonsoft.Json;

namespace EFT.Hideout;

[Serializable]
public sealed class ResourceRequirement : Requirement
{
	[JsonProperty("templateId")]
	public string TemplateId;

	[JsonProperty("resource")]
	public int Resource;

	public override ERequirementType Type => ERequirementType.Resource;
}
