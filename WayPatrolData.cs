using System;
using UnityEngine;

[Serializable]
public class WayPatrolData
{
	public int PatrolFrom;

	public int PatrolTo;

	public WayPatrolPoints[] Paths;

	public static PatrolPoint FindPoint(BotZone zone, int patrolFromId)
	{
		PatrolWay[] patrolWays = zone.PatrolWays;
		for (int i = 0; i < patrolWays.Length; i++)
		{
			foreach (PatrolPoint point in patrolWays[i].Points)
			{
				if (point.Id != patrolFromId)
				{
					continue;
				}
				return point;
			}
		}
		return null;
	}

	public WayPatrolData(int from, int to, WayPatrolPoints[] paths)
	{
		Paths = paths;
		PatrolFrom = from;
		PatrolTo = to;
	}

	public void SetAllAvailable()
	{
		for (int i = 0; i < Paths.Length; i++)
		{
			Paths[i].IsAvailable = true;
		}
	}

	public void Check(BotZone zone)
	{
		WayPatrolPoints[] paths = Paths;
		foreach (WayPatrolPoints wayPatrolPoints in paths)
		{
			PatrolPoint patrolPoint = FindPoint(zone, PatrolFrom);
			if (patrolPoint == null)
			{
				Debug.LogError(string.Format(_ED3E._E000(27902), patrolPoint, PatrolFrom));
			}
			else if (wayPatrolPoints != null)
			{
				if (wayPatrolPoints.WayPoints == null)
				{
					Debug.LogError(string.Format(_ED3E._E000(27956), zone));
					continue;
				}
				float magnitude = (patrolPoint.position - wayPatrolPoints.WayPoints[0]).magnitude;
				if (magnitude > 0.5f)
				{
					Debug.LogError(string.Format(_ED3E._E000(28024), PatrolFrom, patrolPoint, magnitude));
				}
				PatrolPoint patrolPoint2 = FindPoint(zone, PatrolTo);
				if (patrolPoint2 != null)
				{
					float magnitude2 = (patrolPoint2.position - wayPatrolPoints.WayPoints[wayPatrolPoints.WayPoints.Length - 1]).magnitude;
					if (magnitude2 > 0.5f)
					{
						Debug.LogError(string.Format(_ED3E._E000(28112), PatrolTo, patrolPoint2, magnitude2));
					}
				}
				else
				{
					Debug.LogError(_ED3E._E000(28204));
				}
			}
			else
			{
				Debug.LogError(string.Format(_ED3E._E000(28283), zone));
			}
		}
	}
}
