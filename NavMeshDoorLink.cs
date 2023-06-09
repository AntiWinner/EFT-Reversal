using System.Runtime.CompilerServices;
using EFT;
using EFT.Interactive;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshDoorLink : MonoBehaviour
{
	public int Id;

	public BotOwner Opening;

	public Vector3 Close1;

	public Vector3 Close2_Normal;

	public Vector3 Close2_Breach;

	public Vector3 Open1;

	public Vector3 Open2;

	public Vector3 MidOpen;

	public Vector3 MidClose;

	public NavMeshObstacle Carver;

	public NavMeshObstacle Carver_2;

	public bool CanBeCarved;

	private _E076 m__E000;

	private _E076 m__E001;

	private _E620 m__E002;

	private bool m__E003;

	private bool _E004;

	private float _E005;

	[CompilerGenerated]
	private Door _E006;

	public Door Door
	{
		[CompilerGenerated]
		get
		{
			return _E006;
		}
		[CompilerGenerated]
		private set
		{
			_E006 = value;
		}
	}

	public _E076 SegmentOpen
	{
		get
		{
			if (this.m__E000 == null)
			{
				this.m__E000 = new _E076(Open1, Open2);
			}
			return this.m__E000;
		}
	}

	public _E076 SegmentClose
	{
		get
		{
			if (this.m__E001 == null)
			{
				this.m__E001 = new _E076(Close1, Close2_Normal);
			}
			return this.m__E001;
		}
	}

	private void Awake()
	{
		MidOpen = (Open1 + Open2) / 2f;
		MidClose = (Close1 + Close2_Normal) / 2f;
	}

	public void SetDoor(Door door, bool withSubscribe)
	{
		Door = door;
		if (withSubscribe && CanBeCarved)
		{
			if (door.DoorState == EDoorState.Open)
			{
				NavMeshObstacle carver_ = Carver_2;
				bool carving = (Carver.carving = true);
				carver_.carving = carving;
			}
			else
			{
				NavMeshObstacle carver_2 = Carver_2;
				bool carving = (Carver.carving = false);
				carver_2.carving = carving;
			}
			Door.OnDoorStateChanged += _E000;
		}
	}

	public void Init(_E620 bc)
	{
		CanBeCarved = Carver != null && Carver_2 != null;
		this.m__E002 = bc;
	}

	public void ManualUpdate()
	{
		if (_E004 && !Carver.carving && !Carver_2.carving)
		{
			_E001();
		}
	}

	public bool ShallInteract()
	{
		if (!CanBeCarved)
		{
			return true;
		}
		if (!Carver.carving)
		{
			return !Carver_2.carving;
		}
		return false;
	}

	public void TryCreateCrave()
	{
		Vector3 vector = Close2_Normal - Close1;
		Vector3 from = Close2_Breach - Close1;
		Vector3 position = (Close1 + Close2_Normal) / 2f;
		Vector3 position2 = (Close1 + Close2_Breach) / 2f;
		GameObject gameObject = new GameObject(_ED3E._E000(15275));
		GameObject gameObject2 = new GameObject(_ED3E._E000(15322));
		NavMeshObstacle navMeshObstacle = gameObject.AddComponent<NavMeshObstacle>();
		gameObject.AddComponent<NavMeshObstacleWrap>().NavMeshObstacle = navMeshObstacle;
		NavMeshObstacle navMeshObstacle2 = gameObject2.AddComponent<NavMeshObstacle>();
		gameObject2.AddComponent<NavMeshObstacleWrap>().NavMeshObstacle = navMeshObstacle2;
		gameObject.transform.SetParent(base.transform);
		gameObject2.transform.SetParent(base.transform);
		from.y = (vector.y = 0f);
		float num = Vector3.Angle(vector, Vector3.right);
		if (vector.z < 0f)
		{
			num = 360f - num;
		}
		num = 0f - num;
		float num2 = Vector3.Angle(from, Vector3.right);
		if (from.z < 0f)
		{
			num2 = 360f - num2;
		}
		num2 = 0f - num2;
		gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, num, 0f));
		gameObject2.transform.rotation = Quaternion.Euler(new Vector3(0f, num2, 0f));
		_E36B.DebugArrow(Close1, vector, Color.red, 2f);
		navMeshObstacle.size = new Vector3(vector.magnitude, 1f, 0.1f);
		navMeshObstacle2.size = new Vector3(from.magnitude, 1f, 0.1f);
		gameObject.transform.position = position;
		gameObject2.transform.position = position2;
		Carver = navMeshObstacle;
		Carver_2 = navMeshObstacle2;
		CanBeCarved = Carver != null && Carver_2 != null;
	}

	public void CheckAfterCreatedCarver()
	{
		Vector3 rhs = Close2_Normal - Close1;
		Vector3 vector = Open2 - Open1;
		Vector3 vector2 = _E39C.Rotate90(vector, _E39C.SideTurn.right);
		Vector3 vector3 = ((!(Vector3.Dot(vector2, rhs) > 0f)) ? _E39C.NormalizeFastSelf(-vector2) : _E39C.NormalizeFastSelf(vector2));
		Vector3 sourcePosition = Close1 + vector + vector3 * 0.4f;
		Vector3 sourcePosition2 = Close1 - vector + vector3 * 0.4f;
		_E002();
		if (!NavMesh.SamplePosition(sourcePosition, out var hit, 2f, -1) || !NavMesh.SamplePosition(sourcePosition2, out var hit2, 2f, -1))
		{
			return;
		}
		_E36B.DebugPoint(hit.position, Color.yellow, 0.5f, 2f);
		_E36B.DebugPoint(hit2.position, Color.green, 0.5f, 2f);
		float magnitude = (hit2.position - hit.position).magnitude;
		NavMeshPath navMeshPath = new NavMeshPath();
		if (!NavMesh.CalculatePath(hit.position, hit2.position, -1, navMeshPath))
		{
			return;
		}
		if (navMeshPath.status == NavMeshPathStatus.PathComplete)
		{
			float num = Mathf.Abs(navMeshPath.CalculatePathLength() - magnitude);
			navMeshPath.DrawPathDebug(Color.white, 2f);
			if (num < 4f)
			{
				if ((Carver != null) & (Carver_2 != null))
				{
					CanBeCarved = true;
					NavMeshObstacle carver = Carver;
					bool flag2 = (Carver_2.enabled = true);
					carver.enabled = flag2;
				}
			}
			else
			{
				Debug.LogWarning(_ED3E._E000(15306) + num + _ED3E._E000(25470) + Door.gameObject.name + _ED3E._E000(15346) + Door.Id);
			}
		}
		else
		{
			Debug.LogWarning(string.Concat(_ED3E._E000(15337), navMeshPath.status, _ED3E._E000(25470), Door.gameObject.name, _ED3E._E000(15346), Door.Id));
		}
	}

	private void _E000(WorldInteractiveObject obj, EDoorState prevstate, EDoorState nextstate)
	{
		if (CanBeCarved)
		{
			if (nextstate == EDoorState.Open)
			{
				_E004 = true;
				_E001();
				return;
			}
			_E004 = false;
			NavMeshObstacle carver_ = Carver_2;
			bool carving = (Carver.carving = false);
			carver_.carving = carving;
		}
	}

	private void _E001()
	{
		if (!(_E005 < Time.time))
		{
			return;
		}
		_E005 = Time.time + 1f;
		BotOwner botOwner = this.m__E002.ClosestBotToPoint(MidOpen);
		if (botOwner != null)
		{
			if ((botOwner.Position - MidOpen).sqrMagnitude > 4f)
			{
				_E003();
			}
		}
		else
		{
			_E003();
		}
	}

	private void _E002()
	{
		CanBeCarved = false;
		if (Carver != null && Carver_2 != null)
		{
			NavMeshObstacle carver_ = Carver_2;
			bool flag2 = (Carver.enabled = false);
			carver_.enabled = flag2;
		}
	}

	private void _E003()
	{
		_E004 = false;
		NavMeshObstacle carver_ = Carver_2;
		bool carving = (Carver.carving = true);
		carver_.carving = carving;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Vector3 vector = Open1 + Vector3.up;
		Vector3 to = Open2 + Vector3.up;
		Gizmos.DrawLine(Open1, vector);
		Gizmos.DrawLine(Open2, to);
		Gizmos.DrawLine(vector, to);
		Gizmos.color = Color.green;
		Vector3 vector2 = Close1 + Vector3.up;
		Vector3 to2 = Close2_Normal + Vector3.up;
		Vector3 to3 = Close2_Breach + Vector3.up;
		Gizmos.DrawLine(Close1, vector2);
		Gizmos.DrawLine(Close2_Normal, to2);
		Gizmos.DrawLine(Close2_Breach, to3);
		Gizmos.DrawLine(vector2, to2);
		Gizmos.DrawLine(vector2, to3);
	}
}
