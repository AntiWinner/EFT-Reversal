using UnityEngine;

namespace EFT.Game.Spawning;

public struct SpawnSphereParams : ISpawnColliderParams
{
	public Vector3 Center;

	public float Radius;
}
