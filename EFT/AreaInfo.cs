using System;

namespace EFT;

[Serializable]
public class AreaInfo
{
	public bool active;

	public int type;

	public int level;

	public int? completeTime;

	public bool constructing;

	public bool passiveBonusesEnabled;

	public string lastRecipe;

	public _E5EA[] bonuses;

	public _E818[] slots;

	public EAreaType AreaType => (EAreaType)type;
}
