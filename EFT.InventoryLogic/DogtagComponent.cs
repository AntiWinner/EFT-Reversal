using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Comfort.Common;

namespace EFT.InventoryLogic;

public sealed class DogtagComponent : _EB19
{
	public const string BearDogtagsTemplate = "59f32bb586f774757e1e8442";

	public const string UsecDogtagsTemplate = "59f32c3b86f77472a31742f0";

	public string GroupId;

	[_E63C]
	public string AccountId = "";

	[_E63C]
	public string ProfileId = "";

	[_E63C]
	public string Nickname = "";

	[_E63C]
	public EPlayerSide Side;

	[_E63C]
	public int Level;

	[_E63C]
	public DateTime Time;

	[_E63C]
	public string Status = "";

	[_E63C]
	public string KillerAccountId = "";

	[_E63C]
	public string KillerProfileId = "";

	[_E63C]
	public string KillerName = "";

	[_E63C]
	public string WeaponName = "";

	public DogtagComponent(Item item)
		: base(item)
	{
		Item.Attributes.Add(new _EB10(EItemAttributeId.Nickname)
		{
			Name = EItemAttributeId.Nickname.GetName(),
			StringValue = () => Nickname,
			DisplayType = () => EItemAttributeDisplayType.Compact
		});
		Item.Attributes.Add(new _EB10(EItemAttributeId.Side)
		{
			Name = EItemAttributeId.Side.GetName(),
			StringValue = () => Side.ToString().Localized().ToUpper(),
			DisplayType = () => EItemAttributeDisplayType.Compact
		});
		Item.Attributes.Add(new _EB10(EItemAttributeId.Level)
		{
			Name = EItemAttributeId.Level.GetName(),
			StringValue = () => Level.ToString(),
			DisplayType = () => EItemAttributeDisplayType.Compact
		});
		Item.Attributes.Add(new _EB10(EItemAttributeId.DeathTime)
		{
			Name = EItemAttributeId.DeathTime.GetName(),
			StringValue = () => Time.ToString(CultureInfo.CurrentCulture),
			DisplayType = () => EItemAttributeDisplayType.Compact
		});
		Item.Attributes.Add(new _EB10(EItemAttributeId.Status)
		{
			Name = EItemAttributeId.Status.GetName(),
			StringValue = delegate
			{
				string correctedProfileNickname = Singleton<_E7DE>.Instance.Game.Controller.GetCorrectedProfileNickname(KillerProfileId, KillerName);
				return Status.Localized() + correctedProfileNickname.Transliterate();
			},
			DisplayType = () => EItemAttributeDisplayType.Compact
		});
		Item.Attributes.Add(new _EB10(EItemAttributeId.Weapon)
		{
			Name = EItemAttributeId.Weapon.GetName(),
			StringValue = () => WeaponName.Localized(),
			DisplayType = () => EItemAttributeDisplayType.Compact
		});
	}

	[CompilerGenerated]
	private string _E000()
	{
		return Nickname;
	}

	[CompilerGenerated]
	private string _E001()
	{
		return Side.ToString().Localized().ToUpper();
	}

	[CompilerGenerated]
	private string _E002()
	{
		return Level.ToString();
	}

	[CompilerGenerated]
	private string _E003()
	{
		return Time.ToString(CultureInfo.CurrentCulture);
	}

	[CompilerGenerated]
	private string _E004()
	{
		string correctedProfileNickname = Singleton<_E7DE>.Instance.Game.Controller.GetCorrectedProfileNickname(KillerProfileId, KillerName);
		return Status.Localized() + correctedProfileNickname.Transliterate();
	}

	[CompilerGenerated]
	private string _E005()
	{
		return WeaponName.Localized();
	}
}
