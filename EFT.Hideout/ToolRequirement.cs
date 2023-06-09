using System;

namespace EFT.Hideout;

[Serializable]
public sealed class ToolRequirement : ItemRequirement
{
	public override ERequirementType Type => ERequirementType.Tool;
}
