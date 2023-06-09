using EFT.Ballistics;
using UnityEngine;

public class BaseBallistic : MonoBehaviour
{
	public enum ESurfaceSound
	{
		Concrete,
		Metal,
		Wood,
		Soil,
		Grass,
		Asphalt,
		Glass,
		Gravel,
		MetalThin,
		Puddle,
		Slate,
		Tile,
		WoodThick,
		Swamp,
		Garbage,
		GarbageMetal,
		Plastic,
		WholeGlass,
		WoodThin
	}

	[HideInInspector]
	public ESurfaceSound SurfaceSound;

	public virtual ESurfaceSound GetSurfaceSound(Vector3 position)
	{
		return SurfaceSound;
	}

	public virtual BallisticCollider Get(Vector3 pos)
	{
		return null;
	}

	public void Associate(MaterialType typeOfMaterial)
	{
		switch (typeOfMaterial)
		{
		case MaterialType.Asphalt:
			SurfaceSound = ESurfaceSound.Asphalt;
			break;
		case MaterialType.Concrete:
		case MaterialType.Stone:
			SurfaceSound = ESurfaceSound.Concrete;
			break;
		case MaterialType.Tile:
			SurfaceSound = ESurfaceSound.Tile;
			break;
		case MaterialType.GarbageMetal:
			SurfaceSound = ESurfaceSound.GarbageMetal;
			break;
		case MaterialType.MetalThin:
			SurfaceSound = ESurfaceSound.MetalThin;
			break;
		case MaterialType.Chainfence:
		case MaterialType.Grate:
		case MaterialType.MetalThick:
			SurfaceSound = ESurfaceSound.Metal;
			break;
		case MaterialType.Gravel:
			SurfaceSound = ESurfaceSound.Gravel;
			break;
		case MaterialType.Mud:
		case MaterialType.Soil:
		case MaterialType.SoilForest:
			SurfaceSound = ESurfaceSound.Soil;
			break;
		case MaterialType.Pebbles:
			SurfaceSound = ESurfaceSound.Slate;
			break;
		case MaterialType.WoodThick:
			SurfaceSound = ESurfaceSound.WoodThick;
			break;
		case MaterialType.WoodThin:
			SurfaceSound = ESurfaceSound.WoodThin;
			break;
		case MaterialType.Cardboard:
		case MaterialType.GarbagePaper:
			SurfaceSound = ESurfaceSound.Wood;
			break;
		case MaterialType.GrassHigh:
		case MaterialType.GrassLow:
			SurfaceSound = ESurfaceSound.Grass;
			break;
		case MaterialType.Glass:
		case MaterialType.GlassShattered:
			SurfaceSound = ESurfaceSound.Glass;
			break;
		case MaterialType.Water:
		case MaterialType.WaterPuddle:
			SurfaceSound = ESurfaceSound.Puddle;
			break;
		case MaterialType.Swamp:
			SurfaceSound = ESurfaceSound.Swamp;
			break;
		case MaterialType.Plastic:
			SurfaceSound = ESurfaceSound.Plastic;
			break;
		case MaterialType.Body:
		case MaterialType.Fabric:
		case MaterialType.GenericSoft:
		case MaterialType.Tyre:
		case MaterialType.Rubber:
		case MaterialType.GenericHard:
		case MaterialType.BodyArmor:
			break;
		}
	}

	public virtual void TakeSettingsFrom(BaseBallistic collider)
	{
		SurfaceSound = collider.SurfaceSound;
	}
}
