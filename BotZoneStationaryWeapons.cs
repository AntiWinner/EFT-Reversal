using System;
using System.Collections.Generic;
using UnityEngine;

public class BotZoneStationaryWeapons : MonoBehaviour
{
	public StationaryWeaponLink[] Weapon = Array.Empty<StationaryWeaponLink>();

	public void Init()
	{
		StationaryWeaponLink[] weapon = Weapon;
		for (int i = 0; i < weapon.Length; i++)
		{
			weapon[i].Init();
		}
	}

	public void SetList(List<StationaryWeaponLink> list)
	{
		Weapon = list.ToArray();
	}

	public int GetCount()
	{
		if (Weapon == null)
		{
			return 0;
		}
		return Weapon.Length;
	}
}
