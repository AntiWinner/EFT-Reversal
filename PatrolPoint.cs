using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EFT;
using UnityEngine;
using UnityEngine.AI;

public class PatrolPoint : MonoBehaviour, IPositionPoint
{
	public int Id;

	public bool CanUseByBoss = true;

	public PatrolWay PatrolWay;

	public bool ShallSit;

	public PatrolPointType PatrolPointType;

	public AReserveWayAction ActionData;

	public PointWithLookSides PointWithLookSides;

	public bool SubManual;

	public List<PatrolPoint> subPoints;

	[CompilerGenerated]
	private BotOwner m__E000;

	public BotOwner Owner
	{
		[CompilerGenerated]
		get
		{
			return this.m__E000;
		}
		[CompilerGenerated]
		private set
		{
			this.m__E000 = value;
		}
	}

	public Vector3 position => base.transform.position;

	public Vector3 Position => base.transform.position;

	public bool HaveLookSide => LookIndexes > 0;

	public int LookIndexes
	{
		get
		{
			if (PointWithLookSides == null)
			{
				return -1;
			}
			return PointWithLookSides.Directions.Count;
		}
	}

	private void Awake()
	{
		subPoints = _E000();
	}

	public Vector3 PositionForTest()
	{
		if (ActionData != null)
		{
			return ActionData.GoTo;
		}
		return Position;
	}

	public Vector3 LookDir(int index)
	{
		return PointWithLookSides.Directions[index];
	}

	public void SetOwner(BotOwner owner)
	{
		if (owner != null)
		{
			foreach (PatrolPoint point in owner.PatrollingData.Way.Points)
			{
				if (point.Owner == Owner)
				{
					point.SetOwner(null);
				}
			}
		}
		if (ActionData != null && Owner != null && owner == null)
		{
			ActionData.SetLeaveUser(Owner);
		}
		Owner = owner;
		if (ActionData != null)
		{
			ActionData.SetCurrentUser(Owner);
		}
	}

	public int GetOwnerId()
	{
		if (Owner == null)
		{
			return -1;
		}
		return Owner.Id;
	}

	public void CreateSubPoints(PatrolWay way)
	{
		int num = 6;
		_E001();
		List<PatrolPoint> list = _E000();
		if (SubManual)
		{
			PointWithLookSides component = GetComponent<PointWithLookSides>();
			if (component != null)
			{
				PointWithLookSides = component;
				component.Refresh();
			}
			subPoints = new List<PatrolPoint>();
			foreach (Transform item in base.transform)
			{
				if (NavMesh.SamplePosition(item.position + Vector3.up, out var hit, 1.5f, -1))
				{
					item.position = hit.position;
				}
				else
				{
					string text = ((item.parent != null) ? item.parent.name : _ED3E._E000(9239));
					Debug.LogError(_ED3E._E000(9225) + item.name + _ED3E._E000(9261) + text);
				}
				PatrolPoint component2 = item.GetComponent<PatrolPoint>();
				NavMeshPath navMeshPath = new NavMeshPath();
				if (NavMesh.CalculatePath(Position, item.position, -1, navMeshPath))
				{
					if (navMeshPath.status != 0)
					{
						Debug.LogError(_ED3E._E000(9295) + base.gameObject.name + _ED3E._E000(9319));
					}
					else
					{
						float magnitude = (navMeshPath.corners[0] - Position).magnitude;
						float magnitude2 = (navMeshPath.corners[navMeshPath.corners.Length - 1] - item.position).magnitude;
						if (magnitude > 0.7f)
						{
							Debug.LogError(string.Format(_ED3E._E000(9399), magnitude, item.gameObject.name, base.gameObject.name));
						}
						if (magnitude2 > 0.7f)
						{
							Debug.LogError(string.Format(_ED3E._E000(9425), magnitude2, item.gameObject.name, base.gameObject.name));
						}
					}
				}
				else
				{
					Debug.LogError(_ED3E._E000(9295) + base.gameObject.name + _ED3E._E000(9456));
				}
				if (component2 != null)
				{
					subPoints.Add(component2);
					PointWithLookSides componentInChildren = component2.GetComponentInChildren<PointWithLookSides>();
					bool flag = false;
					bool flag2 = false;
					if (componentInChildren != null)
					{
						flag = true;
						component2.PointWithLookSides = componentInChildren;
					}
					PointWithLookSides component3 = component2.GetComponent<PointWithLookSides>();
					if (component3 != null)
					{
						flag2 = true;
						component2.PointWithLookSides = component3;
					}
					if (flag && flag2)
					{
						Debug.LogError(_ED3E._E000(9472) + base.gameObject.name + _ED3E._E000(9526));
					}
					if (component2.PointWithLookSides != null)
					{
						component2.PointWithLookSides.Refresh();
					}
				}
			}
		}
		else
		{
			foreach (PatrolPoint item2 in list)
			{
				Object.DestroyImmediate(item2.gameObject);
			}
			subPoints = new List<PatrolPoint>();
			_E002(1.7f * way.CoefSubPoints);
			_E002(2.5f * way.CoefSubPoints);
			_E002(3.4f * way.CoefSubPoints);
			if (subPoints.Count <= num)
			{
				Debug.LogWarning(_ED3E._E000(9562) + base.gameObject.name);
				_E002(1.7f * way.CoefSubPoints, 1f);
				_E002(2.5f * way.CoefSubPoints, 1f);
			}
		}
		if (subPoints.Count <= num)
		{
			int num2 = 4;
			if (subPoints.Count <= num2)
			{
				Debug.LogError(string.Format(_ED3E._E000(9622), num2, base.gameObject.name));
			}
			else
			{
				Debug.LogWarning(string.Format(_ED3E._E000(9703), num, base.gameObject.name));
			}
		}
	}

	public Vector3? GetLookDirection(int index, bool wasCutted)
	{
		if (index < 0 || LookIndexes < 0)
		{
			return null;
		}
		if (LookIndexes <= index)
		{
			return null;
		}
		return PointWithLookSides.Directions[index];
	}

	public void SetWay(PatrolWay patrolWay)
	{
		PatrolWay = patrolWay;
	}

	public void CheckData(PatrolWay way, BoxCollider[] bounds)
	{
		if (!_E39D.InBounds(Position, bounds))
		{
			Debug.LogError(_ED3E._E000(9932) + base.name + _ED3E._E000(9922) + _E079.ParentsLog(base.gameObject));
		}
		if (way.PatrolType == PatrolType.reserved)
		{
			AReserveWayAction componentInChildren = GetComponentInChildren<AReserveWayAction>();
			if (componentInChildren == null)
			{
				Debug.LogError(_ED3E._E000(10002) + base.gameObject.name);
				return;
			}
			ActionData = componentInChildren;
			ActionData.RefreshData();
		}
	}

	public bool IsFreeFor(BotOwner botOwner = null)
	{
		if (Owner == null)
		{
			return true;
		}
		if (botOwner == null)
		{
			return false;
		}
		return botOwner == Owner;
	}

	public void TryAutoFixActionData()
	{
		if (ActionData != null)
		{
			ActionData.AutoFix();
		}
	}

	public PatrolPoint GetPosByIndex(int index)
	{
		return subPoints[index];
	}

	private List<PatrolPoint> _E000()
	{
		PointWithLookSides = null;
		ActionData = null;
		List<PatrolPoint> list = new List<PatrolPoint>();
		List<GameObject> list2 = new List<GameObject>();
		foreach (Transform item in base.transform)
		{
			PatrolPoint component = item.GetComponent<PatrolPoint>();
			if (component != null)
			{
				list.Add(component);
				continue;
			}
			PointWithLookSides component2 = item.GetComponent<PointWithLookSides>();
			if (component2 != null)
			{
				PointWithLookSides = component2;
				PointWithLookSides.Refresh();
				continue;
			}
			AReserveWayAction component3 = item.GetComponent<AReserveWayAction>();
			if (component3 != null)
			{
				ActionData = component3;
			}
			else
			{
				list2.Add(item.gameObject);
			}
		}
		if (list2.Count > 0)
		{
			Debug.LogError(_ED3E._E000(10018) + base.gameObject.name + _ED3E._E000(10097) + list2.Count);
			GameObject gameObject;
			PointWithLookSides pointWithLookSides;
			if (PointWithLookSides == null)
			{
				gameObject = new GameObject(_ED3E._E000(10084));
				pointWithLookSides = gameObject.AddComponent<PointWithLookSides>();
				gameObject.transform.SetParent(base.transform, worldPositionStays: false);
				gameObject.transform.localPosition = Vector3.zero;
			}
			else
			{
				pointWithLookSides = PointWithLookSides;
				gameObject = PointWithLookSides.gameObject;
			}
			foreach (GameObject item2 in list2)
			{
				item2.transform.SetParent(gameObject.transform, worldPositionStays: true);
				Vector3 v = item2.transform.position - base.transform.position;
				if (v.sqrMagnitude <= 1E-06f)
				{
					Debug.LogError(_ED3E._E000(10082) + item2.gameObject.name);
				}
				v.y = 0f;
				v = _E39C.NormalizeFastSelf(v);
				pointWithLookSides.Directions.Add(v);
			}
			PointWithLookSides = pointWithLookSides;
		}
		return list;
	}

	private void _E001()
	{
		LookDirections[] componentsInChildren = GetComponentsInChildren<LookDirections>();
		foreach (LookDirections lookDirections in componentsInChildren)
		{
			float magnitude = (lookDirections.Position - base.transform.position).magnitude;
			if (magnitude > 12f)
			{
				Debug.LogError(_ED3E._E000(10134) + magnitude + _ED3E._E000(10170) + base.gameObject.name + _ED3E._E000(10152));
			}
			if (NavMesh.SamplePosition(lookDirections.Position, out var hit, 5f, -1))
			{
				if (hit.distance > 0.5f)
				{
					Debug.LogError(_ED3E._E000(10203) + hit.distance + _ED3E._E000(10170) + base.gameObject.name + _ED3E._E000(10229));
				}
				NavMeshPath path = new NavMeshPath();
				if (NavMesh.CalculatePath(lookDirections.Position, hit.position, -1, path))
				{
					lookDirections.transform.position = hit.position;
					lookDirections.NormalizeLookSide();
					if (lookDirections.GetComponent<PatrolPoint>() == null)
					{
						lookDirections.gameObject.AddComponent<PatrolPoint>();
						GameObject obj = new GameObject(_ED3E._E000(10217));
						PointWithLookSides pointWithLookSides = obj.AddComponent<PointWithLookSides>();
						obj.transform.SetParent(lookDirections.gameObject.transform, worldPositionStays: false);
						pointWithLookSides.Directions = new List<Vector3>();
						pointWithLookSides.Directions.Add(lookDirections.dir);
					}
				}
				else
				{
					Debug.LogError(_ED3E._E000(8220) + base.gameObject.name + _ED3E._E000(10152));
				}
			}
			else
			{
				Debug.LogError(_ED3E._E000(8243) + base.gameObject.name + _ED3E._E000(10152));
			}
		}
		componentsInChildren = GetComponentsInChildren<LookDirections>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Object.DestroyImmediate(componentsInChildren[i]);
		}
	}

	private void _E002(float offset, float yOffset = 0f)
	{
		Vector3 v = new Vector3(_E39D.Random(-1f, 1f), 0f, _E39D.Random(-1f, 1f));
		v = _E39C.NormalizeFastSelf(v) * offset;
		for (int i = 0; i < 4; i++)
		{
			v = _E39C.RotateOnAngUp(v, _E39D.Random(70f, 110f));
			NavMeshHit hit = default(NavMeshHit);
			Vector3 vector = position + v;
			if (NavMesh.SamplePosition(vector, out hit, 0.3f + yOffset, -1))
			{
				NavMeshPath navMeshPath = new NavMeshPath();
				if (NavMesh.CalculatePath(position, vector, -1, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete && navMeshPath.CalculatePathLength() < offset * 2f)
				{
					Debug.DrawRay(hit.position, Vector3.up * 2f, Color.green, 5f);
					GameObject obj = new GameObject(_ED3E._E000(8271) + (base.transform.childCount + 1));
					PatrolPoint item = obj.AddComponent<PatrolPoint>();
					obj.transform.SetParent(base.transform);
					obj.transform.position = hit.position;
					subPoints.Add(item);
				}
			}
		}
	}

	private void _E003()
	{
		Gizmos.color = new Color(0f, 1f, 1f, 0.9f);
		_E395.DrawCube(base.transform.position, base.transform.rotation, new Vector3(1f, 4f, 1f) * 0.2f);
	}

	private void _E004()
	{
		if (_E366.NoDrawSatelites)
		{
			return;
		}
		Gizmos.color = new Color(1f, 0.9215686f, 0.01568628f, 0.9f);
		if (subPoints == null)
		{
			return;
		}
		foreach (PatrolPoint subPoint in subPoints)
		{
			if (subPoint != null)
			{
				_E395.DrawCube(subPoint.transform.position, base.transform.rotation, new Vector3(1f, 4f, 1f) * 0.14f);
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (_E366.AlwaysDrawPatrolPoints)
		{
			_E006();
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (!_E366.AlwaysDrawPatrolPoints)
		{
			_E006();
		}
		_E005();
	}

	private void _E005()
	{
		if (Owner != null)
		{
			Gizmos.color = new Color(0.8f, 0.2f, 0.4f, 0.7f);
			for (int i = 0; i < Owner.Id; i++)
			{
				Gizmos.DrawSphere(base.transform.position + Vector3.up * (1f + 0.2f * (float)i), 0.1f);
			}
		}
	}

	private void _E006()
	{
		if (ActionData != null)
		{
			ActionData.DrawGizmos();
		}
		_E003();
		_E004();
		if (Physics.Raycast(base.transform.position, Vector3.down, out var hitInfo, 50f))
		{
			Gizmos.DrawLine(base.transform.position, hitInfo.point);
		}
	}
}
