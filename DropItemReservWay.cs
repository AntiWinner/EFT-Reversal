using EFT;
using UnityEngine;
using UnityEngine.AI;

public class DropItemReservWay : AReserveWayAction
{
	public Transform lookToPoint;

	private readonly bool _E005 = true;

	private bool _E002;

	private float _E004;

	public override Vector3 GoTo => base.transform.position;

	public override Vector3 LookShootTo => lookToPoint.position;

	public override ReserveWayResult ManualUpdate(BotOwner bot)
	{
		if (_E002 && _E004 < Time.time)
		{
			_E002 = false;
			bot.ItemDropper.RefreshItemToDrop();
			bot.ItemDropper.TryDoDrop();
		}
		if (bot.PatrollingData.Status == PatrolStatus.stay)
		{
			_cuResult = ReserveWayResult.stay;
		}
		else
		{
			_cuResult = ReserveWayResult.move;
		}
		return _cuResult;
	}

	public override void RefreshData()
	{
		CheckWayFromParent(_ED3E._E000(2942), base.transform.position);
		if (NavMesh.SamplePosition(base.transform.position, out var hit, 1f, -1))
		{
			base.transform.position = hit.position;
		}
		_ = lookToPoint == null;
		CheckPoint(base.transform.position, _ED3E._E000(2896));
	}

	public override void RefreshBot()
	{
		_cuResult = ReserveWayResult.move;
	}

	public override float TimeToUse(BotOwner owner)
	{
		return owner.Settings.FileSettings.Patrol.RESERVE_LOOT_TIME_STAY;
	}

	public override void ComeTo(BotOwner bot)
	{
		if (_E005)
		{
			_cuResult = ReserveWayResult.drop;
			_E004 = Time.time + 0.3f;
			_E002 = true;
		}
		else
		{
			_cuResult = ReserveWayResult.stay;
		}
	}

	public override void AutoFix()
	{
		base.transform.localPosition = Vector3.zero;
		if (NavMesh.SamplePosition(base.transform.position, out var hit, 1f, -1))
		{
			base.transform.position = hit.position;
		}
	}

	public override void DrawGizmos()
	{
		if (_E005)
		{
			Gizmos.color = new Color(0.5f, 0.1f, 0.7f, 0.9f);
		}
		else
		{
			Gizmos.color = new Color(0.5f, 0.1f, 0.4f, 0.9f);
		}
		Gizmos.DrawCube(lookToPoint.position, new Vector3(1f, 4f, 1f) * 0.2f);
		Gizmos.DrawCube(lookToPoint.position, new Vector3(1f, 1f, 4f) * 0.2f);
		Vector3 vector = base.transform.position + Vector3.up * 1.6f;
		Gizmos.DrawCube(vector, new Vector3(1f, 1f, 1f) * 0.3f);
		Gizmos.DrawCube(base.transform.position, new Vector3(1f, 1f, 1f) * 0.4f);
		Gizmos.DrawLine(vector, lookToPoint.position);
	}
}
