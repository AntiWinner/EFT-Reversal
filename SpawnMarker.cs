using System;
using EFT;
using EFT.Game.Spawning;
using UnityEngine;

public sealed class SpawnMarker : MonoBehaviour
{
	public ESpawnCategory Category;

	public EPlayerSide Side;

	public string GroupId;

	public string Infiltration;

	public string BotZoneName;

	public SpawnPointMarker SpawnPointMarker;

	public static SpawnMarker Create(SpawnPointMarker spawnPointMarker, ESpawnCategory category, EPlayerSide side, string groupId, string infiltration, string botZoneName)
	{
		SpawnMarker spawnMarker = new GameObject(string.Format(_ED3E._E000(55351), side)).AddComponent<SpawnMarker>();
		spawnMarker.SpawnPointMarker = spawnPointMarker;
		spawnMarker.transform.position = spawnPointMarker.SpawnPoint.Position;
		spawnMarker.transform.rotation = spawnPointMarker.SpawnPoint.Rotation;
		spawnMarker.Category = category;
		spawnMarker.Side = side;
		spawnMarker.GroupId = groupId;
		spawnMarker.Infiltration = infiltration;
		spawnMarker.BotZoneName = botZoneName;
		return spawnMarker;
	}

	public static SpawnMarker Create(ISpawnPoint spawnPoint, ESpawnCategory category, EPlayerSide side, string groupId, string infiltration, string botZoneName)
	{
		SpawnMarker spawnMarker = new GameObject(string.Format(_ED3E._E000(55351), side)).AddComponent<SpawnMarker>();
		spawnMarker.transform.position = spawnPoint.Position;
		spawnMarker.transform.rotation = spawnPoint.Rotation;
		spawnMarker.Category = category;
		spawnMarker.Side = side;
		spawnMarker.GroupId = groupId;
		spawnMarker.Infiltration = infiltration;
		spawnMarker.BotZoneName = botZoneName;
		return spawnMarker;
	}

	private void OnDrawGizmos()
	{
		Color color2 = (Gizmos.color = Side switch
		{
			EPlayerSide.Usec => Color.red, 
			EPlayerSide.Bear => Color.green, 
			EPlayerSide.Savage => Color.blue, 
			_ => throw new ArgumentOutOfRangeException(), 
		});
		Vector3 direction = base.transform.TransformDirection(Vector3.up) * 20f;
		Gizmos.DrawRay(base.transform.position, direction);
		color2.a = 0.5f;
		Gizmos.color = color2;
		_E395.DrawWireCube(base.transform.position, base.transform.rotation, new Vector3(1f, 0.01f, 1f));
		Gizmos.color = Color.white;
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.DrawLine(Vector3.zero, Vector3.forward * 2f * 0.3f);
		Gizmos.DrawLine(Vector3.forward * 2f * 0.3f, Vector3.forward * 1.5f * 0.3f + Vector3.left * 0.5f * 0.3f);
		Gizmos.DrawLine(Vector3.zero + Vector3.forward * 2f * 0.3f, Vector3.forward * 1.5f * 0.3f + Vector3.right * 0.5f * 0.3f);
	}
}
