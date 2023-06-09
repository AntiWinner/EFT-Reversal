using System;
using UnityEngine;

namespace EFT;

public sealed class ShellExtractionData : MonoBehaviour
{
	[Serializable]
	public struct ExtractionSettings
	{
		public Vector2 ShellsForceXRange;

		public Vector2 ShellsForceYRange;

		public Vector2 ShellsForceZRange;

		public Vector2 ShellsRotationRangeX;

		public Vector2 ShellsRotationRangeY;

		public Vector2 ShellsRotationRangeZ;

		public float ShellForceMultiplier;

		public Vector3 GetRandomForce()
		{
			return new Vector3(UnityEngine.Random.Range(ShellsForceXRange.x, ShellsForceXRange.y), UnityEngine.Random.Range(ShellsForceYRange.x, ShellsForceYRange.y), UnityEngine.Random.Range(ShellsForceZRange.x, ShellsForceZRange.y));
		}

		public Vector3 GetRandomRotation()
		{
			return new Vector3(UnityEngine.Random.Range(ShellsRotationRangeX.x, ShellsRotationRangeX.y), UnityEngine.Random.Range(ShellsRotationRangeY.x, ShellsRotationRangeY.y), UnityEngine.Random.Range(ShellsRotationRangeZ.x, ShellsRotationRangeZ.y));
		}
	}

	public ExtractionSettings ShotSettings;

	public ExtractionSettings MisfireSettings;

	public ExtractionSettings JamSettings;

	public ExtractionSettings PatronExtractionSettings;

	public Vector3 GetMisfireRotationVector()
	{
		return MisfireSettings.GetRandomRotation();
	}

	public Vector3 GetMisfireAdditionalForce()
	{
		return MisfireSettings.GetRandomForce();
	}

	public float GetMisfireShellForceMultiplier()
	{
		return MisfireSettings.ShellForceMultiplier;
	}

	public Vector3 GetShotRotationVector()
	{
		return ShotSettings.GetRandomRotation();
	}

	public Vector3 GetShotAdditionalForce()
	{
		return ShotSettings.GetRandomForce();
	}

	public float GetShotShellForceMultiplier()
	{
		return ShotSettings.ShellForceMultiplier;
	}

	public Vector3 GetJamRotationVector()
	{
		return JamSettings.GetRandomRotation();
	}

	public Vector3 GetJamAdditionalForce()
	{
		return JamSettings.GetRandomForce();
	}

	public float GetJamShellForceMultiplier()
	{
		return JamSettings.ShellForceMultiplier;
	}

	public Vector3 PatronExtractionVector()
	{
		return PatronExtractionSettings.GetRandomRotation();
	}

	public Vector3 PatronExtractionAdditionalForce()
	{
		return PatronExtractionSettings.GetRandomForce();
	}

	public float PatronExtractionForceMultiplier()
	{
		return PatronExtractionSettings.ShellForceMultiplier;
	}
}
