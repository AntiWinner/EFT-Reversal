using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.Interactive;
using UnityEngine;

public class ThrowGrenadePlace : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public Vector3 pos;

		internal Door _E000(Door x1, Door x2)
		{
			if ((x1.transform.position - pos).sqrMagnitude > (x2.transform.position - pos).sqrMagnitude)
			{
				return x2;
			}
			return x1;
		}
	}

	public float GrenadeMass = 0.6f;

	public float GrenadeForce = 11f;

	public Transform From;

	public Transform Target;

	public float AngleForThrow = 45f;

	public float ThrowForce = 45f;

	public Door Door;

	public bool IsOk;

	public Vector3 DoorPos;

	public bool HaveDoor;

	private readonly Vector3 m__E000 = new Vector3(0f, 1.5f, 0f);

	public _E269 GetData()
	{
		return new _E269(AngleForThrow, ThrowForce, Target.position - From.position, From.position, Target.position, alwaysGood: true);
	}

	public void Init(IEnumerable<Door> allDoors)
	{
		if (HaveDoor)
		{
			Vector3 pos = DoorPos;
			Door door = allDoors.Aggregate((Door x1, Door x2) => ((x1.transform.position - pos).sqrMagnitude > (x2.transform.position - pos).sqrMagnitude) ? x2 : x1);
			if ((DoorPos - door.transform.position).sqrMagnitude > 1f)
			{
				_E2E7.AILog(_ED3E._E000(15055), door);
			}
			Door = door;
		}
	}

	public bool IsValid(string id, bool withSubDoor)
	{
		if (withSubDoor)
		{
			Door = null;
			HaveDoor = false;
		}
		if (Target.position == Vector3.zero)
		{
			Debug.LogError(_ED3E._E000(15085) + id);
			return false;
		}
		if (From.position == Vector3.zero)
		{
			Debug.LogError(_ED3E._E000(15129) + id);
			return false;
		}
		if (!_E39C.IsOnNavMesh(From.position, 0.1f))
		{
			if (!Physics.Raycast(From.position + Vector3.up * 0.1f, Vector3.down, out var hitInfo, 2f, _E37B.TerrainLowPoly))
			{
				string text = _ED3E._E000(15110);
				if (base.transform.parent != null)
				{
					text = base.transform.parent.name;
				}
				Debug.LogError(string.Concat(_ED3E._E000(15160), From, _ED3E._E000(15195), id, _ED3E._E000(15191), base.gameObject.name, _ED3E._E000(15180), text));
				return false;
			}
			From.position = hitInfo.point;
		}
		Debug.Log(_ED3E._E000(15173) + GrenadeForce + _ED3E._E000(15220) + GrenadeMass);
		float maxPower = GrenadeForce / GrenadeMass;
		_E269 obj = null;
		List<_E269> list = new List<_E269>();
		foreach (AIGreandeAng value in Enum.GetValues(typeof(AIGreandeAng)))
		{
			_E269 obj2 = _E26B.CanThrowGrenade2(From.position + this.m__E000, Target.position, maxPower, value);
			if (obj2.CanThrow)
			{
				obj = obj2;
				break;
			}
			list.Add(obj2);
		}
		if (obj == null)
		{
			foreach (_E269 item in list)
			{
				if (withSubDoor)
				{
					Door door = _E000(item);
					if (door != null)
					{
						Door = door;
						Debug.LogWarning(_ED3E._E000(15205) + item.Ang);
						break;
					}
				}
			}
			HaveDoor = Door != null;
			if (HaveDoor)
			{
				DoorPos = Door.transform.position;
			}
			return false;
		}
		IsOk = true;
		AngleForThrow = obj.Ang;
		ThrowForce = obj.Force;
		return true;
	}

	public void OnDrawGizmosSelected()
	{
		DrawGizmos();
	}

	public void DrawGizmos()
	{
		Color color = new Color(0.5f, 0.9f, 0.1f);
		Gizmos.color = (IsOk ? color : Color.red);
		Gizmos.DrawSphere(From.position, 0.5f);
		Gizmos.color = (IsOk ? color : Color.red);
		Gizmos.DrawSphere(From.position + this.m__E000, 0.3f);
		Gizmos.color = (IsOk ? new Color(0.1f, 0.8f, 0.5f) : Color.red);
		Gizmos.DrawSphere(Target.position, 0.5f);
		Vector3 d = Target.position - From.position;
		if (d.sqrMagnitude > 0.1f)
		{
			Vector3 position = From.position + this.m__E000;
			Vector3 vector = _E39C.RotateVectorOnAngToZ(d, AngleForThrow);
			_E36B.DrawArrow(position, vector * 2f);
		}
	}

	public bool IsNowValid()
	{
		if (!IsOk)
		{
			return false;
		}
		if (!HaveDoor)
		{
			return true;
		}
		return Door.DoorState == EDoorState.Open;
	}

	private Door _E000(_E269 aiGreandeOutData)
	{
		GameObject hitsObject = aiGreandeOutData.HitsObject;
		if (hitsObject == null)
		{
			return null;
		}
		Door component = hitsObject.GetComponent<Door>();
		Debug.Log(string.Concat(_ED3E._E000(15240), hitsObject, _ED3E._E000(15286), aiGreandeOutData.CanThrow.ToString()));
		if (component != null)
		{
			return component;
		}
		Transform parent = hitsObject.transform.parent;
		if (parent != null)
		{
			component = parent.GetComponent<Door>();
			if (component != null)
			{
				return component;
			}
		}
		return null;
	}
}
