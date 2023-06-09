using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EFT.PrefabSettings;
using UnityEngine;

namespace EFT.Interactive;

public class PlayersWithImmuneToSniperFireCollector : MonoBehaviour
{
	[SerializeField]
	private FlareEventType flareTypeForImmune = FlareEventType.ExitActivate;

	private _E89C m__E000 = new _E89C();

	private Action m__E001;

	private HashSet<string> m__E002 = new HashSet<string>();

	private bool _E003 = true;

	private Coroutine _E004;

	private readonly float _E005 = 20f;

	private void Awake()
	{
		this.m__E001 = _EBAF.Instance.SubscribeOnEvent(delegate(_EBB9 callback)
		{
			_E000(callback);
		});
	}

	private void OnDestroy()
	{
		this.m__E001?.Invoke();
		this.m__E001 = null;
		if (_E004 != null)
		{
			StopCoroutine(_E004);
			_E004 = null;
		}
	}

	private void _E000(_EBB9 invokedEvent)
	{
		if (invokedEvent.FlareEventType != flareTypeForImmune)
		{
			return;
		}
		switch (invokedEvent.ZoneEventType)
		{
		case _EBB9.EZoneEventType.PlayerEnteredZone:
			if (GamePlayerOwner.MyPlayer != null && GamePlayerOwner.MyPlayer.ProfileId == invokedEvent.PlayerProfileID && _E003)
			{
				_E857.DisplayNotification(this.m__E000);
				_E003 = false;
				_E004 = StartCoroutine(_E001());
			}
			break;
		case _EBB9.EZoneEventType.FiredPlayerAddedInShotList:
		case _EBB9.EZoneEventType.PlayerByPartyAddedInShotList:
			this.m__E002.Add(invokedEvent.PlayerProfileID);
			break;
		}
	}

	private IEnumerator _E001()
	{
		float num = 0f;
		while (num <= 1f)
		{
			num += Time.deltaTime / _E005;
			yield return null;
		}
		_E003 = true;
		_E004 = null;
	}

	public bool IsPlayerImmuneForFire(string profileID)
	{
		return this.m__E002.Contains(profileID);
	}

	[CompilerGenerated]
	private void _E002(_EBB9 callback)
	{
		_E000(callback);
	}
}
