using System;
using UnityEngine;

namespace EFT.Game.Spawning;

[Serializable]
public class SpawnPoint : ISpawnPoint
{
	public string Id;

	public string Name;

	public Vector3 Position;

	public Quaternion Rotation;

	public EPlayerSideMask Sides;

	public ESpawnCategoryMask Categories;

	public string Infiltration;

	public BotZone BotZone;

	public float DelayToCanSpawnSec = 4f;

	public string BotZoneName
	{
		get
		{
			if (!(BotZone != null))
			{
				return "";
			}
			return BotZone.NameZone;
		}
	}

	public ISpawnPointCollider Collider { get; set; }

	string ISpawnPoint.Id => Id;

	string ISpawnPoint.Name => Name;

	Vector3 ISpawnPoint.Position => Position;

	Quaternion ISpawnPoint.Rotation => Rotation;

	EPlayerSideMask ISpawnPoint.Sides => Sides;

	ESpawnCategoryMask ISpawnPoint.Categories => Categories;

	string ISpawnPoint.Infiltration => Infiltration;

	string ISpawnPoint.BotZoneName
	{
		get
		{
			if (!(BotZone != null))
			{
				return null;
			}
			return BotZone.NameZone;
		}
	}

	bool ISpawnPoint.IsSnipeZone
	{
		get
		{
			if (BotZone != null)
			{
				return BotZone.SnipeZone;
			}
			return false;
		}
	}

	float ISpawnPoint.DelayToCanSpawnSec => DelayToCanSpawnSec;

	public float NextBornTime { get; set; }

	public void Dispose()
	{
		if (BotZone != null)
		{
			BotZone.CoverPoints = null;
			BotZone.AmbushPoints = null;
			BotZone.BushPoints = null;
			if (BotZone.ZoneManualInfo != null)
			{
				BotZone.ZoneManualInfo.Points = null;
			}
			BotZone.ClearPools();
		}
		BotZone = null;
	}
}
