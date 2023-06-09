using UnityEngine;

namespace EFT.Game.Spawning;

public interface ISpawnPoint
{
	string Id { get; }

	string Name { get; }

	Vector3 Position { get; }

	Quaternion Rotation { get; }

	EPlayerSideMask Sides { get; }

	ESpawnCategoryMask Categories { get; }

	string Infiltration { get; }

	string BotZoneName { get; }

	bool IsSnipeZone { get; }

	float DelayToCanSpawnSec { get; }

	float NextBornTime { get; set; }

	ISpawnPointCollider Collider { get; }

	void Dispose();
}
