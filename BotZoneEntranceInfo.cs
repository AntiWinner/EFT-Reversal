using System.Collections.Generic;
using UnityEngine;

public class BotZoneEntranceInfo : MonoBehaviour
{
	public List<BotZoneEntrance> EntraceList = new List<BotZoneEntrance>();

	private Dictionary<int, List<BotZoneEntrance>> _E000 = new Dictionary<int, List<BotZoneEntrance>>();

	public void Init()
	{
		foreach (BotZoneEntrance entrace in EntraceList)
		{
			if (_E000.TryGetValue(entrace.ConnectedAreaId, out var value))
			{
				value.Add(entrace);
				continue;
			}
			value = new List<BotZoneEntrance>();
			_E000.Add(entrace.ConnectedAreaId, value);
		}
	}

	public void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		foreach (BotZoneEntrance entrace in EntraceList)
		{
			entrace.DrawGizmosSelected();
		}
	}

	public BotZoneEntrance GetClosest(Vector3 pos, int areaId)
	{
		if (_E000.TryGetValue(areaId, out var value))
		{
			BotZoneEntrance result = null;
			float num = float.MaxValue;
			{
				foreach (BotZoneEntrance item in value)
				{
					float sqrMagnitude = (item.CenterPoint - pos).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						result = item;
					}
				}
				return result;
			}
		}
		return null;
	}
}
