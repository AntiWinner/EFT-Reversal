using EFT;
using UnityEngine;
using UnityEngine.AI;

public abstract class AReserveWayAction : MonoBehaviour
{
	protected ReserveWayResult _cuResult;

	private float _E000;

	private bool _E001;

	public abstract Vector3 GoTo { get; }

	public abstract Vector3 LookShootTo { get; }

	public abstract ReserveWayResult ManualUpdate(BotOwner owner);

	public abstract void RefreshData();

	public abstract void RefreshBot();

	public abstract void DrawGizmos();

	public void SetCurrentUser(BotOwner bot)
	{
		if (bot == null)
		{
			SetFree();
		}
		else
		{
			RefreshBot();
		}
	}

	public virtual void SetFree()
	{
	}

	public abstract void AutoFix();

	public abstract void ComeTo(BotOwner bot);

	public virtual float TimeToUse(BotOwner owner)
	{
		return owner.Settings.FileSettings.Patrol.RESERVE_TIME_STAY;
	}

	public virtual void SetLeaveUser(BotOwner owner)
	{
	}

	protected bool CheckDist(BotOwner bot)
	{
		if (_E000 < Time.time)
		{
			_E000 = Time.time + 3f;
			Vector3 vector = bot.Position - base.transform.position;
			float y = vector.y;
			vector.y = 0f;
			float sqrMagnitude = vector.sqrMagnitude;
			_E001 = sqrMagnitude < 1.4f && Mathf.Abs(y) < 0.3f;
			return _E001;
		}
		return _E001;
	}

	protected void CheckPoint(Vector3 point, string data)
	{
		NavMeshPath navMeshPath = new NavMeshPath();
		bool flag = true;
		flag = NavMesh.CalculatePath(point, base.transform.parent.position, -1, navMeshPath);
		if (flag)
		{
			flag = navMeshPath.status == NavMeshPathStatus.PathComplete;
		}
		if (!flag)
		{
			Debug.LogError(data + _ED3E._E000(2779) + base.gameObject.name + _ED3E._E000(2814) + base.transform.parent.name);
		}
	}

	protected void CheckWayFromParent(string nameInfo, Vector3 from)
	{
		NavMeshPath navMeshPath = new NavMeshPath();
		if (NavMesh.CalculatePath(base.transform.parent.position, from, -1, navMeshPath) && navMeshPath.status != 0)
		{
			Debug.LogError(nameInfo + _ED3E._E000(2801) + base.gameObject.name);
			AutoFix();
		}
	}
}
