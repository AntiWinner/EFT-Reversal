using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.PrefabSettings;
using UnityEngine;

namespace EFT.Interactive;

public class FlareShootDetectorZone : MonoBehaviour
{
	[SerializeField]
	private string zoneID;

	[SerializeField]
	private FlareEventType flareTypeForHandle;

	[SerializeField]
	private List<PhysicsTriggerHandler> _triggerHandlers = new List<PhysicsTriggerHandler>();

	private HashSet<string> m__E000 = new HashSet<string>();

	private Action m__E001;

	private void Awake()
	{
		this.m__E001 = _EBAF.Instance.SubscribeOnEvent(delegate(_EBBA flareEvent)
		{
			_E001(flareEvent);
		});
		foreach (PhysicsTriggerHandler triggerHandler in _triggerHandlers)
		{
			triggerHandler.OnTriggerEnter += _E000;
		}
	}

	private void OnDestroy()
	{
		this.m__E001?.Invoke();
		this.m__E001 = null;
	}

	private void _E000(Collider collider)
	{
		Player playerByCollider = Singleton<GameWorld>.Instance.GetPlayerByCollider(collider);
		if (!(playerByCollider == null))
		{
			_EBAF.Instance.CreateCommonEvent<_EBB9>().Invoke(flareTypeForHandle, _EBB9.EZoneEventType.PlayerEnteredZone, playerByCollider.ProfileId, zoneID);
		}
	}

	private void _E001(_EBBA flareEvent)
	{
		if (flareEvent.FlareEventType != flareTypeForHandle)
		{
			return;
		}
		bool flag = false;
		foreach (PhysicsTriggerHandler triggerHandler in _triggerHandlers)
		{
			if (!triggerHandler.trigger.bounds.Contains(flareEvent.PlayerPosOnFiredFlare))
			{
				continue;
			}
			flag = true;
			break;
		}
		if (!flag)
		{
			return;
		}
		this.m__E000.Add(flareEvent.FiredPlayer.ProfileId);
		_EBC9 instance = Singleton<GameWorld>.Instance;
		_EBAF.Instance.CreateCommonEvent<_EBB9>().Invoke(flareTypeForHandle, _EBB9.EZoneEventType.FiredPlayerAddedInShotList, flareEvent.FiredPlayer.ProfileId, zoneID);
		if (string.IsNullOrEmpty(flareEvent.FiredPlayer.GroupId))
		{
			return;
		}
		foreach (_E5B4 item in instance.GroupPlayers(flareEvent.FiredPlayer.GroupId, flareEvent.FiredPlayer.TeamId))
		{
			if (!(item.ProfileId == flareEvent.FiredPlayer.ProfileId))
			{
				this.m__E000.Add(item.ProfileId);
				_EBAF.Instance.CreateCommonEvent<_EBB9>().Invoke(flareTypeForHandle, _EBB9.EZoneEventType.PlayerByPartyAddedInShotList, item.ProfileId, zoneID);
			}
		}
	}

	[CompilerGenerated]
	private void _E002(_EBBA flareEvent)
	{
		_E001(flareEvent);
	}
}
