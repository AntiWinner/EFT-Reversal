using System;
using EFT;
using EFT.InventoryLogic;

[Serializable]
public class AggressorStats : IProfileDataContainer
{
	public string AccountId;

	public string ProfileId;

	public string MainProfileNickname;

	public string Name;

	public EPlayerSide Side;

	public EBodyPart BodyPart;

	public EHeadSegment? HeadSegment;

	public string WeaponName;

	public EMemberCategory Category;

	string IProfileDataContainer.ProfileId => ProfileId;

	string IProfileDataContainer.Nickname => Name;

	EPlayerSide IProfileDataContainer.Side => Side;

	public AggressorStats()
	{
	}

	public AggressorStats(string accountId, string profileId, string name, string mainCharacterNickName, EPlayerSide side, EBodyPart part, string weaponName, EMemberCategory memberCategory, EHeadSegment? headSegment)
	{
		AccountId = accountId;
		ProfileId = profileId;
		Name = name;
		MainProfileNickname = mainCharacterNickName;
		Side = side;
		BodyPart = part;
		WeaponName = weaponName;
		Category = memberCategory;
		HeadSegment = headSegment;
	}

	public override string ToString()
	{
		return string.Format(_ED3E._E000(58637), _ED3E._E000(58661), ProfileId, _ED3E._E000(58719), Name, _ED3E._E000(58708), Side, _ED3E._E000(58705), BodyPart, _ED3E._E000(58698), WeaponName);
	}
}
