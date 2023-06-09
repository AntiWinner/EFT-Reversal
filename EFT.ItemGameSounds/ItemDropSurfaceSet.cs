using System;
using JsonType;

namespace EFT.ItemGameSounds;

[Serializable]
public class ItemDropSurfaceSet
{
	public BaseBallistic.ESurfaceSound Surface;

	public SoundBank PistolDropSoundBank;

	public SoundBank RiffleDropSoundBank;

	public SoundBank SubGunDropSoundBank;

	public SoundBank GetBank(EItemDropSoundType dropSoundType)
	{
		return dropSoundType switch
		{
			EItemDropSoundType.Pistol => PistolDropSoundBank, 
			EItemDropSoundType.SubMachineGun => SubGunDropSoundBank, 
			EItemDropSoundType.Rifle => RiffleDropSoundBank, 
			_ => throw new ArgumentOutOfRangeException("dropSoundType", dropSoundType, "There is no associated sound bank for this type, you need to add!"), 
		};
	}
}
