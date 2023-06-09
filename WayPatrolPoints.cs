using System;
using UnityEngine;

[Serializable]
public class WayPatrolPoints
{
	public int Id;

	public Vector3[] WayPoints;

	public bool IsAvailable = true;

	public bool CanRun;

	public WayPatrolPoints(int id, Vector3[] way, bool canRun)
	{
		WayPoints = way;
		Id = id;
		CanRun = canRun;
	}
}
