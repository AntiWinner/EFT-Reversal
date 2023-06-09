using System;
using Diz.Binding;

namespace EFT;

[Serializable]
public class VictimStats : IUpdatable<VictimStats>, IProfileDataContainer
{
	public string AccountId;

	public string ProfileId;

	public string Name;

	public EPlayerSide Side;

	public TimeSpan Time;

	public int Level;

	public EBodyPart BodyPart;

	public string Weapon;

	public float Distance;

	public WildSpawnType Role;

	string IProfileDataContainer.ProfileId => ProfileId;

	string IProfileDataContainer.Nickname => Name;

	EPlayerSide IProfileDataContainer.Side => Side;

	public VictimStats()
	{
	}

	public VictimStats(string accountId, string profileId, string name, EPlayerSide side, TimeSpan time, int level, EBodyPart part, string weapon, EDamageType damageType, float distance, WildSpawnType role)
	{
		AccountId = accountId;
		ProfileId = profileId;
		Name = name;
		Side = side;
		Time = time;
		Level = level;
		BodyPart = part;
		Weapon = weapon ?? string.Empty;
		Distance = ((damageType == EDamageType.Bullet) ? distance : 0f);
		Role = role;
	}

	public bool Compare(VictimStats other)
	{
		return this == other;
	}

	public void UpdateFromAnotherItem(VictimStats other)
	{
		AccountId = other.AccountId;
		ProfileId = other.ProfileId;
		Name = other.Name;
		Side = other.Side;
		Time = other.Time;
		Level = other.Level;
		BodyPart = other.BodyPart;
		Weapon = other.Weapon;
		Distance = other.Distance;
	}
}
