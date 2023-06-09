using System.Collections.Generic;
using EFT;
using UnityEngine;

public class AIPlaceInfoTagillaAmbush : AIPlaceInfo
{
	public enum EPointSearchType
	{
		Random = 1,
		Nearest
	}

	[SerializeField]
	private readonly List<Transform> _ambushPoints = new List<Transform>();

	[SerializeField]
	private readonly EPointSearchType _pointSearchType = EPointSearchType.Nearest;

	[SerializeField]
	private readonly float _maxAmbushDistance = 100f;

	protected override void OnEnterPlace(Player player)
	{
		_E000(player, state: true);
		base.OnEnterPlace(player);
	}

	protected override void OnLeavePlace(Player player)
	{
		_E000(player, state: false);
		base.OnLeavePlace(player);
	}

	private void _E000(Player player, bool state)
	{
		_E620 obj = _E620.FindBotControllerEditorOnly();
		_ = _maxAmbushDistance;
		_ = _maxAmbushDistance;
		foreach (BotOwner botOwner in obj.Bots.BotOwners)
		{
			_ = botOwner;
		}
	}

	private Transform _E001(BotOwner owner)
	{
		EPointSearchType pointSearchType = _pointSearchType;
		if (pointSearchType != EPointSearchType.Random && pointSearchType == EPointSearchType.Nearest)
		{
			Transform transform = _ambushPoints[0];
			float num = owner.Position.SqrDistance(transform.position);
			for (int i = 1; i < _ambushPoints.Count; i++)
			{
				float num2 = owner.Position.SqrDistance(_ambushPoints[i].position);
				if (num2 < num)
				{
					num = num2;
					transform = _ambushPoints[i];
				}
			}
			return transform;
		}
		return _ambushPoints[_E39D.RandomInclude(0, _ambushPoints.Count - 1)];
	}

	private void _E002(BotOwner ambushOwner, bool state)
	{
		if (ambushOwner.Memory.GoalEnemy == null)
		{
			_E001(ambushOwner);
		}
	}

	private void OnDrawGizmos()
	{
		if (!DebugBotData.Instance.Gizmos.DrawTagillaAmbushPlace)
		{
			return;
		}
		Gizmos.color = Color.magenta;
		Gizmos.DrawSphere(base.transform.position, 0.6f);
		foreach (Transform ambushPoint in _ambushPoints)
		{
			if (ambushPoint != null)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawSphere(ambushPoint.position, 0.3f);
				Gizmos.DrawLine(base.transform.position, ambushPoint.transform.position);
			}
		}
	}
}
