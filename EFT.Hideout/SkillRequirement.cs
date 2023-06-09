using System;
using Newtonsoft.Json;

namespace EFT.Hideout;

[Serializable]
public sealed class SkillRequirement : Requirement
{
	[JsonProperty("skillName")]
	public string SkillName;

	[JsonProperty("skillLevel")]
	public int SkillLevel;

	private _E74F _skillManager;

	private _E751 _realSkill;

	[JsonIgnore]
	public FakeSkill FakeSkill { get; private set; }

	private ESkillId _E000
	{
		get
		{
			Enum.TryParse<ESkillId>(SkillName, out var result);
			return result;
		}
	}

	public override ERequirementType Type => ERequirementType.Skill;

	public void Test(_E74F value)
	{
		if (_skillManager != value)
		{
			_skillManager = value;
			_realSkill = _skillManager.GetSkill(_E000);
		}
		if (FakeSkill == null)
		{
			FakeSkill = new FakeSkill(value.GetSkill(_E000), SkillLevel);
		}
		TestRequirement(_realSkill.Level, SkillLevel);
	}
}
