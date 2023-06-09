using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT;
using EFT.Interactive;
using UnityEngine;
using UnityEngine.AI;

public class SitReservWay : AReserveWayAction
{
	public Transform lookToPoint;

	public bool ShallLoot = true;

	public string LootableContainerId;

	private bool _E00A;

	private bool _E00B;

	private bool m__E002;

	private bool _E00C;

	private float _E004;

	[CompilerGenerated]
	private LootableContainer _E00D;

	public LootableContainer Container
	{
		[CompilerGenerated]
		get
		{
			return _E00D;
		}
		[CompilerGenerated]
		private set
		{
			_E00D = value;
		}
	}

	public override Vector3 GoTo => base.transform.position;

	public override Vector3 LookShootTo => lookToPoint.position;

	public override ReserveWayResult ManualUpdate(BotOwner bot)
	{
		if (this.m__E002 && _E004 < Time.time)
		{
			this.m__E002 = false;
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

	public override void SetLeaveUser(BotOwner bot)
	{
		_E000(bot);
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
			Debug.LogError(_ED3E._E000(13355) + base.gameObject.name);
		}
		if (lookToPoint == null)
		{
			Debug.LogError(_ED3E._E000(13387) + base.gameObject.name);
		}
		CheckPoint(base.transform.position, _ED3E._E000(13465));
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
		_E001();
		if (ShallLoot)
		{
			_cuResult = ReserveWayResult.looting;
			if (_E00B && Container.DoorState == EDoorState.Shut)
			{
				_E004 = Time.time + 0.2f;
				this.m__E002 = true;
			}
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

	public void SetLootId(string lootableContainerId)
	{
		LootableContainerId = lootableContainerId;
	}

	public override void DrawGizmos()
	{
		if (ShallLoot)
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

	private void _E000(BotOwner bot)
	{
		this.m__E002 = false;
		_ = _E00B;
	}

	private void _E001()
	{
		if (_E00A)
		{
			return;
		}
		_E00A = false;
		IEnumerable<LootableContainer> allObjects = LocationScene.GetAllObjects<LootableContainer>();
		if (LootableContainerId.Length > 0)
		{
			Container = allObjects.FirstOrDefault((LootableContainer x) => x.Id == LootableContainerId);
			_E00B = Container != null;
			if (!_E00B && !_E00C)
			{
				_E00C = true;
			}
		}
	}

	[CompilerGenerated]
	private bool _E002(LootableContainer x)
	{
		return x.Id == LootableContainerId;
	}
}
