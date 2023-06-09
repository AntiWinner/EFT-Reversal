using System;

namespace EFT;

[Serializable]
public class SurfaceSet
{
	public BaseBallistic.ESurfaceSound Surface;

	public SoundBank RunSoundBank;

	public SoundBank SprintSoundBank;

	public SoundBank StopSoundBank;

	public SoundBank LandingSoundBank;

	public SoundBank TurnSoundBank;

	public SoundBank DuckSoundBank;

	public SoundBank ProneSoundBank;

	public SoundBank ProneDropSoundBank;

	public SoundBank JumpSoundBank;

	public bool IsFull
	{
		get
		{
			if (RunSoundBank != null && SprintSoundBank != null && StopSoundBank != null && LandingSoundBank != null && TurnSoundBank != null && DuckSoundBank != null && ProneSoundBank != null && ProneDropSoundBank != null)
			{
				return JumpSoundBank != null;
			}
			return false;
		}
	}
}
