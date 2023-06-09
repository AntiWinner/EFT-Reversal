namespace EFT;

public sealed class FakeSkill : _E751
{
	private readonly int _overrideLevel;

	public FakeSkill(_E751 skill, int overrideLevel)
		: base(skill.SkillManager, skill.Id, skill.Class, skill.Actions, skill.Buffs)
	{
		_overrideLevel = overrideLevel;
	}

	public override int GetLevelForValue(float value)
	{
		return _overrideLevel;
	}
}
