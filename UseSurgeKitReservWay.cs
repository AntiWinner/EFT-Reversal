using System.Collections.Generic;
using EFT;
using UnityEngine;
using UnityEngine.AI;

public class UseSurgeKitReservWay : AReserveWayAction
{
	public Transform lookToPoint;

	public float ChaceToUse100 = 50f;

	public float ChaceToSit100 = 50f;

	private bool _E002;

	private EMedsType _E00E;

	private bool _E00F;

	private float _E004;

	private readonly List<EMedsType> _E010 = new List<EMedsType>();

	public override Vector3 GoTo => base.transform.position;

	public override Vector3 LookShootTo => lookToPoint.position;

	public override ReserveWayResult ManualUpdate(BotOwner bot)
	{
		if (_E002 && _E004 < Time.time)
		{
			_E002 = false;
			_E004 = Time.time + 30f;
			switch (_E00E)
			{
			case EMedsType.surgialKit:
				bot.Medecine.SurgicalKit.SetRandomPartToHeal();
				bot.Medecine.SurgicalKit.ApplyToCurrentPart();
				break;
			case EMedsType.firstAidKit:
				bot.Medecine.FirstAid.SetRandomPartToHeal();
				bot.Medecine.FirstAid.TryApplyToCurrentPart();
				break;
			case EMedsType.stimulator:
				bot.Medecine.Stimulators.TryApply();
				break;
			}
		}
		if (bot.PatrollingData.Status == PatrolStatus.stay)
		{
			_cuResult = ReserveWayResult.stay;
		}
		else
		{
			_cuResult = ReserveWayResult.move;
		}
		bot.SetPose(_E00F ? 0f : 1f);
		return _cuResult;
	}

	public override void RefreshData()
	{
		CheckWayFromParent(_ED3E._E000(13449), base.transform.position);
		if (NavMesh.SamplePosition(base.transform.position, out var hit, 1f, -1))
		{
			base.transform.position = hit.position;
		}
		_ = lookToPoint == null;
		CheckPoint(base.transform.position, _ED3E._E000(13487));
	}

	public override void RefreshBot()
	{
		_cuResult = ReserveWayResult.move;
	}

	public override float TimeToUse(BotOwner owner)
	{
		return owner.Settings.FileSettings.Patrol.RESERVE_USE_SURGE_TIME_STAY;
	}

	public override void ComeTo(BotOwner bot)
	{
		_E00F = _E39D.IsTrue100(ChaceToSit100);
		if (bot.Settings.FileSettings.Patrol.RESERV_CAN_USE_MEDS && _E39D.IsTrue100(ChaceToUse100))
		{
			_E004 = Time.time + 0.3f;
			_E002 = true;
			_E010.Clear();
			if (bot.Medecine.SurgicalKit.HaveSmth2Use)
			{
				_E010.Add(EMedsType.surgialKit);
			}
			if (bot.Medecine.FirstAid.HaveSmth2Use)
			{
				_E010.Add(EMedsType.firstAidKit);
			}
			if (bot.Medecine.Stimulators.HaveSmt)
			{
				_E010.Add(EMedsType.stimulator);
			}
			_E00E = _E010.RandomElement();
		}
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

	public override void DrawGizmos()
	{
		Gizmos.color = new Color(0.5f, 0.1f, 0.4f, 0.9f);
		Gizmos.DrawCube(lookToPoint.position, new Vector3(1f, 4f, 1f) * 0.2f);
		Gizmos.DrawCube(lookToPoint.position, new Vector3(1f, 1f, 4f) * 0.2f);
		Vector3 vector = base.transform.position + Vector3.up * 1.6f;
		Gizmos.DrawCube(vector, new Vector3(1f, 1f, 1f) * 0.3f);
		Gizmos.DrawCube(base.transform.position, new Vector3(1f, 1f, 1f) * 0.4f);
		Gizmos.DrawLine(vector, lookToPoint.position);
	}
}
