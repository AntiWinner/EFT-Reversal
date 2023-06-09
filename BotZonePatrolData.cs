using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class BotZonePatrolData : MonoBehaviour
{
	private const float m__E000 = 0.1f;

	private const float m__E001 = 3f;

	private const float m__E002 = 3f;

	public bool DrawGizmos;

	public List<WayPatrolData> WaysAsList = new List<WayPatrolData>();

	private _E111 m__E003;

	private readonly _E2A7<int, int, WayPatrolData> m__E004 = new _E2A7<int, int, WayPatrolData>();

	public void Init(_E111 dangerAreas, BotZone zone)
	{
		this.m__E003 = dangerAreas;
		foreach (WayPatrolData waysAs in WaysAsList)
		{
			this.m__E004.Add(waysAs.PatrolFrom, waysAs.PatrolTo, waysAs);
			for (int i = 0; i < waysAs.Paths.Length; i++)
			{
				WayPatrolPoints wayPatrolPoints = waysAs.Paths[i];
				wayPatrolPoints.IsAvailable = true;
				_E000(wayPatrolPoints, zone, waysAs.PatrolFrom, waysAs.PatrolTo);
			}
		}
		this.m__E003.OnDangerAreaCreated += _E006;
		this.m__E003.OnDangerAreaRemoved += _E002;
	}

	public async Task AddWays(PatrolPoint from, PatrolPoint to, List<NavMeshObstacle> obstacles, List<CustomNavigationPoint> coverPoints)
	{
		await Task.Yield();
		WayPatrolPoints wayPatrolPoints = await _E007(from.Position, to.Position, obstacles, coverPoints);
		List<WayPatrolPoints> list = new List<WayPatrolPoints>();
		List<WayPatrolPoints> list2 = new List<WayPatrolPoints>();
		await Task.Yield();
		if (wayPatrolPoints != null)
		{
			await Task.Yield();
			list.Add(wayPatrolPoints);
			Vector3[] way = wayPatrolPoints.WayPoints.Reverse().ToArray();
			list2.Add(new WayPatrolPoints(Guid.NewGuid().GetHashCode(), way, wayPatrolPoints.CanRun));
			WayPatrolPoints wayPatrolPoints2 = await _E007(from.Position, to.Position, obstacles, coverPoints);
			if (wayPatrolPoints2 != null)
			{
				await Task.Yield();
				list.Add(wayPatrolPoints2);
				way = wayPatrolPoints2.WayPoints.Reverse().ToArray();
				list2.Add(new WayPatrolPoints(Guid.NewGuid().GetHashCode(), way, wayPatrolPoints2.CanRun));
				WayPatrolPoints wayPatrolPoints3 = await _E007(from.Position, to.Position, obstacles, coverPoints);
				if (wayPatrolPoints3 != null)
				{
					await Task.Yield();
					list.Add(wayPatrolPoints3);
					way = wayPatrolPoints3.WayPoints.Reverse().ToArray();
					list2.Add(new WayPatrolPoints(Guid.NewGuid().GetHashCode(), way, wayPatrolPoints3.CanRun));
				}
			}
		}
		else
		{
			await Task.Yield();
			NavMeshPath navMeshPath = new NavMeshPath();
			bool flag = NavMesh.CalculatePath(from.Position, to.Position, -1, navMeshPath);
			if (!flag)
			{
				Debug.LogError(string.Format(_ED3E._E000(28351), from, to, obstacles.Count, flag, navMeshPath.status));
			}
		}
		await Task.Yield();
		_E00A(obstacles);
		WayPatrolData item = new WayPatrolData(from.Id, to.Id, list.ToArray());
		WaysAsList.Add(item);
		WayPatrolData item2 = new WayPatrolData(to.Id, from.Id, list2.ToArray());
		WaysAsList.Add(item2);
	}

	public bool TryGetWay(int from, int to, out WayPatrolData poitns)
	{
		return this.m__E004.TryGet(from, to, out poitns);
	}

	public void CheckCurZone(BotZone zone)
	{
		foreach (WayPatrolData waysAs in WaysAsList)
		{
			waysAs.Check(zone);
		}
	}

	private void _E000(WayPatrolPoints path, BotZone zone, int patrolFrom, int patrolTo)
	{
	}

	private void _E001()
	{
		foreach (WayPatrolData waysAs in WaysAsList)
		{
			if (this.m__E003.ActiveAreas.Count == 0)
			{
				waysAs.SetAllAvailable();
				continue;
			}
			foreach (_E112 activeArea in this.m__E003.ActiveAreas)
			{
				_E005(activeArea, waysAs);
			}
		}
	}

	private void _E002(_E112 obj)
	{
		_E003();
	}

	private void _E003()
	{
		if (this.m__E003.ActiveAreas.Count == 0)
		{
			foreach (WayPatrolData waysAs in WaysAsList)
			{
				waysAs.SetAllAvailable();
			}
		}
		foreach (WayPatrolData waysAs2 in WaysAsList)
		{
			for (int i = 0; i < waysAs2.Paths.Length; i++)
			{
				WayPatrolPoints wayPatrolPoints = waysAs2.Paths[i];
				if (wayPatrolPoints.IsAvailable)
				{
					continue;
				}
				bool flag = false;
				foreach (_E112 activeArea in this.m__E003.ActiveAreas)
				{
					if (_E004(wayPatrolPoints, activeArea))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					wayPatrolPoints.IsAvailable = true;
				}
			}
		}
	}

	private bool _E004(WayPatrolPoints path, _E112 activeArea)
	{
		for (int i = 0; i < path.WayPoints.Length; i++)
		{
			if ((path.WayPoints[i] - activeArea.Point).sqrMagnitude < activeArea.SRad)
			{
				return true;
			}
		}
		return false;
	}

	private void _E005(_E112 activeArea, WayPatrolData wayPatrolData)
	{
		for (int i = 0; i < wayPatrolData.Paths.Length; i++)
		{
			WayPatrolPoints wayPatrolPoints = wayPatrolData.Paths[i];
			if (wayPatrolPoints.IsAvailable && _E004(wayPatrolPoints, activeArea))
			{
				wayPatrolPoints.IsAvailable = false;
				break;
			}
		}
	}

	private void _E006(_E112 obj)
	{
		_E001();
	}

	private async Task<WayPatrolPoints> _E007(Vector3 from, Vector3 to, List<NavMeshObstacle> obstaclesCollect, List<CustomNavigationPoint> coverPoints)
	{
		await Task.Yield();
		WayPatrolPoints wayPatrolPoints = null;
		NavMeshPath navMeshPath = new NavMeshPath();
		if (NavMesh.CalculatePath(from, to, -1, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
		{
			if (navMeshPath.corners.Length <= 1)
			{
				Debug.LogError(string.Format(_ED3E._E000(28392), from, to));
				return null;
			}
			Vector3[] way = navMeshPath.corners.ToArray();
			bool canRun = _E008(way, coverPoints);
			wayPatrolPoints = new WayPatrolPoints(Guid.NewGuid().GetHashCode(), way, canRun);
			await Task.Yield();
			for (int i = 0; i < wayPatrolPoints.WayPoints.Length - 1; i++)
			{
				Vector3 from2 = wayPatrolPoints.WayPoints[i];
				Vector3 to2 = wayPatrolPoints.WayPoints[i + 1];
				NavMeshObstacle item = _E00B(from2, to2);
				obstaclesCollect.Add(item);
			}
		}
		return wayPatrolPoints;
	}

	private bool _E008(Vector3[] way, List<CustomNavigationPoint> coverPoints)
	{
		if (NavTriangle.CalculatePathLength(way) > 3f)
		{
			int num = 0;
			for (int i = 0; i < way.Length - 1; i++)
			{
				Vector3 p = way[i];
				Vector3 p2 = way[i + 1];
				int num2 = _E009(p, p2, coverPoints);
				num += num2;
			}
			if ((float)num <= 3f)
			{
				return true;
			}
		}
		return false;
	}

	private int _E009(Vector3 p1, Vector3 p2, List<CustomNavigationPoint> coverPoints)
	{
		int num = 0;
		foreach (CustomNavigationPoint coverPoint in coverPoints)
		{
			if ((_E079.GetProjectionPoint(coverPoint.BasePosition, p1, p2) - coverPoint.BasePosition).magnitude < 0.1f)
			{
				num++;
			}
		}
		return num;
	}

	private void _E00A(List<NavMeshObstacle> obstacles)
	{
		foreach (NavMeshObstacle item in obstacles.ToList())
		{
			item.gameObject.SetActive(value: false);
			UnityEngine.Object.DestroyImmediate(item.gameObject);
		}
		obstacles.Clear();
	}

	private NavMeshObstacle _E00B(Vector3 from, Vector3 to)
	{
		Vector3 vector;
		Vector3 vector2;
		if (from.z < to.z)
		{
			vector = from;
			vector2 = to;
		}
		else
		{
			vector = to;
			vector2 = from;
		}
		GameObject obj = new GameObject(_ED3E._E000(28294));
		NavMeshObstacle navMeshObstacle = obj.AddComponent<NavMeshObstacle>();
		Vector3 from2 = vector - vector2;
		float magnitude = from2.magnitude;
		obj.transform.position = (vector + vector2) / 2f;
		navMeshObstacle.carving = true;
		navMeshObstacle.size = new Vector3(magnitude * 0.8f, 3f, 3f);
		from2.y = 0f;
		float y = Vector3.Angle(from2, Vector3.right);
		obj.transform.rotation = Quaternion.Euler(new Vector3(0f, y, 0f));
		return navMeshObstacle;
	}

	private void OnDrawGizmosSelected()
	{
		if (!DrawGizmos || WaysAsList == null)
		{
			return;
		}
		foreach (WayPatrolData waysAs in WaysAsList)
		{
			WayPatrolPoints[] paths = waysAs.Paths;
			foreach (WayPatrolPoints way in paths)
			{
				_E00C(way);
			}
		}
	}

	private void _E00C(WayPatrolPoints way)
	{
		Vector3 vector = Vector3.up * 0.1f;
		Gizmos.color = ((!way.IsAvailable) ? Color.red : (way.CanRun ? Color.green : Color.blue));
		for (int i = 0; i < way.WayPoints.Length - 1; i++)
		{
			Vector3 vector2 = way.WayPoints[i];
			Gizmos.DrawLine(to: way.WayPoints[i + 1] + vector, from: vector2 + vector);
		}
	}

	public void Dispose()
	{
		this.m__E003.OnDangerAreaCreated -= _E006;
		this.m__E003.OnDangerAreaRemoved -= _E002;
	}
}
