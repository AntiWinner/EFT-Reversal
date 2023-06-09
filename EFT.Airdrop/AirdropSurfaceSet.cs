using System;

namespace EFT.Airdrop;

[Serializable]
public class AirdropSurfaceSet
{
	public BaseBallistic.ESurfaceSound Surface;

	public SoundBank LandingSoundBank;

	public bool IsFull => LandingSoundBank != null;
}
