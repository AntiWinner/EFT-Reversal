using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace EFT.Hideout;

[Serializable]
public sealed class AreaRequirement : Requirement
{
	[JsonProperty("areaType")]
	public EAreaType AreaType;

	[JsonProperty("requiredLevel")]
	public int RequiredLevel;

	[JsonIgnore]
	public AreaData AreaData { get; private set; }

	public override ERequirementType Type => ERequirementType.Area;

	public void Test(Dictionary<EAreaType, AreaData> areaData)
	{
		AreaData = areaData[AreaType];
		if (AreaData != null)
		{
			TestRequirement(AreaData.CurrentLevel, RequiredLevel);
		}
	}
}
