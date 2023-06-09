using Newtonsoft.Json;

namespace EFT.Hideout;

public sealed class QteEffect
{
	public enum EQteRewardType
	{
		Skill,
		HealthEffect,
		MusclePain,
		GymArmTrauma
	}

	public enum EQteResultType
	{
		None,
		Exit
	}

	public struct SkillExperienceMultiplierData
	{
		[JsonProperty("level")]
		public int level;

		[JsonProperty("multiplier")]
		public float value;
	}

	[JsonProperty("Type")]
	public EQteRewardType Type { get; private set; }

	[JsonProperty("SkillId")]
	public ESkillId Skill { get; private set; }

	[JsonProperty("levelMultipliers")]
	public SkillExperienceMultiplierData[] SkillExpMultiplierData { get; private set; }

	[JsonProperty("Time")]
	public float Duration { get; private set; }

	[JsonProperty("Weight")]
	public float Weight { get; private set; }

	[JsonProperty("Result")]
	public EQteResultType Result { get; private set; }
}
