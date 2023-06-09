using UnityEngine;

namespace EFT.Game.Spawning;

public interface ISpawnPointCollider
{
	bool Contains(Vector3 point);

	string DebugInfo();
}
