using System.Collections.Generic;
using EFT;
using UnityEngine;
using UnityEngine.AI;

public class ShootReservWay : AReserveWayAction
{
	public List<Transform> TargetToShoot;

	public float StayPeriod = 7f;

	public float ShootPeriod = 3f;

	private float _E006;

	private bool _E007;

	private bool _E008;

	private Vector3 _E009;

	public override Vector3 GoTo => base.transform.position;

	public override Vector3 LookShootTo => _E009;

	public override ReserveWayResult ManualUpdate(BotOwner bot)
	{
		if (_E007)
		{
			if (bot.PatrollingData.Status != PatrolStatus.stay)
			{
				_cuResult = ReserveWayResult.move;
			}
			if (_E006 < Time.time)
			{
				float num = (_E008 ? StayPeriod : ShootPeriod);
				_E006 = Time.time + num;
				_E008 = !_E008;
				_cuResult = (_E008 ? ReserveWayResult.shoot : ReserveWayResult.stay);
			}
		}
		else
		{
			_cuResult = ReserveWayResult.move;
		}
		return _cuResult;
	}

	public override void ComeTo(BotOwner bot)
	{
		_E009 = TargetToShoot.RandomElement().position;
		_E007 = true;
		_cuResult = ReserveWayResult.stay;
	}

	public override void AutoFix()
	{
		base.transform.localPosition = Vector3.zero;
		if (NavMesh.SamplePosition(base.transform.position, out var hit, 1f, -1))
		{
			base.transform.position = hit.position;
		}
	}

	public override void RefreshData()
	{
		CheckWayFromParent(_ED3E._E000(2923), base.transform.position);
		if (NavMesh.SamplePosition(base.transform.position, out var hit, 1f, -1))
		{
			base.transform.position = hit.position;
		}
		else
		{
			Debug.LogError(_ED3E._E000(2971) + base.gameObject.name + _ED3E._E000(2980));
		}
		if (TargetToShoot == null || TargetToShoot.Count < 1)
		{
			Debug.LogError(_ED3E._E000(3015) + base.gameObject.name);
		}
		else
		{
			List<Transform> list = new List<Transform>();
			foreach (Transform item in TargetToShoot)
			{
				if (item != null)
				{
					list.Add(item);
				}
				else
				{
					Debug.LogError(_ED3E._E000(3045) + base.gameObject.name + _ED3E._E000(13312));
				}
			}
			TargetToShoot = list;
		}
		CheckPoint(base.transform.position, _ED3E._E000(13369));
	}

	public override void RefreshBot()
	{
		_E007 = false;
		_cuResult = ReserveWayResult.stay;
		_E006 = 0f;
	}

	public override void DrawGizmos()
	{
		foreach (Transform item in TargetToShoot)
		{
			Vector3 position = item.position;
			Gizmos.color = new Color(0.2f, 0.8f, 0.4f, 0.9f);
			Gizmos.DrawCube(position, new Vector3(1f, 4f, 1f) * 0.2f);
			Gizmos.DrawCube(position, new Vector3(4f, 1f, 1f) * 0.2f);
			Gizmos.DrawCube(position, new Vector3(1f, 1f, 4f) * 0.2f);
			Vector3 vector = base.transform.position + Vector3.up * 1.6f;
			Gizmos.DrawCube(vector, new Vector3(1f, 1f, 1f) * 0.3f);
			Gizmos.DrawCube(base.transform.position, new Vector3(1f, 1f, 1f) * 0.4f);
			Gizmos.DrawLine(vector, position);
		}
	}
}
