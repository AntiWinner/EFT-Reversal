using UnityEngine;

namespace EFT;

public sealed class Sounds : ScriptableObject
{
	public SurfaceSet[] Sets;

	public SoundBank Gear;

	public SoundBank GearFast;

	public AudioClip TinnitusSound;

	public AudioClip FractureSound;

	public AudioClip FaceShieldOn;

	public AudioClip FaceShieldOff;

	public AudioClip NightVisionOn;

	public AudioClip NightVisionOff;

	public AudioClip ThermalVisionOn;

	public AudioClip ThermalVisionOff;

	public AudioClip PropIn;

	public AudioClip PropOut;

	public SurfaceSet GetSurfaceSet(BaseBallistic.ESurfaceSound surfaceSound)
	{
		SurfaceSet[] sets = Sets;
		foreach (SurfaceSet surfaceSet in sets)
		{
			if (surfaceSound == surfaceSet.Surface)
			{
				return surfaceSet;
			}
		}
		return null;
	}
}
