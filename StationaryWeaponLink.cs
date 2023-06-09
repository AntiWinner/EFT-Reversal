using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.Interactive;
using EFT.InventoryLogic;
using UnityEngine;

public class StationaryWeaponLink : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public Vector3 pos;

		internal StationaryWeapon _E000(StationaryWeapon x1, StationaryWeapon x2)
		{
			if ((x1.OperatorPosition - pos).magnitude > (x2.OperatorPosition - pos).magnitude)
			{
				return x2;
			}
			return x1;
		}
	}

	public StationaryWeapon Weapon;

	public Vector3 InitialDir;

	public float CosAngleBase;

	private Weapon m__E000;

	[CompilerGenerated]
	private int _E001;

	[CompilerGenerated]
	private bool _E002;

	public int CurOwner
	{
		[CompilerGenerated]
		get
		{
			return _E001;
		}
		[CompilerGenerated]
		set
		{
			_E001 = value;
		}
	}

	public bool BadLoad
	{
		[CompilerGenerated]
		get
		{
			return _E002;
		}
		[CompilerGenerated]
		private set
		{
			_E002 = value;
		}
	}

	public bool Init()
	{
		CurOwner = -1;
		List<StationaryWeapon> list = LocationScene.GetAllObjects<StationaryWeapon>().ToList();
		if (list.Count == 0)
		{
			BadLoad = true;
			return false;
		}
		Vector3 pos = base.transform.position;
		StationaryWeapon stationaryWeapon = list.Aggregate((StationaryWeapon x1, StationaryWeapon x2) => ((x1.OperatorPosition - pos).magnitude > (x2.OperatorPosition - pos).magnitude) ? x2 : x1);
		if ((pos - stationaryWeapon.OperatorPosition).sqrMagnitude > 1f)
		{
			BadLoad = true;
			return false;
		}
		Weapon = stationaryWeapon;
		InitialDir = -_E39C.NormalizeFastSelf(Weapon.WeaponTransform.rotation * Vector3.up);
		float num = Mathf.Max(Weapon.YawLimit.x, Weapon.YawLimit.y);
		float num2 = Mathf.Min(Weapon.YawLimit.x, Weapon.YawLimit.y);
		float num3 = (num - num2) / 2f * 1.15f;
		CosAngleBase = Mathf.Cos(num3 * ((float)Math.PI / 180f));
		return true;
	}

	public bool IsGrenade()
	{
		return Weapon.Animation == StationaryWeapon.EStationaryAnimationType.AGS_17;
	}

	public bool IsFree(int id)
	{
		if (CurOwner == id)
		{
			return true;
		}
		if (Weapon.Locked)
		{
			return false;
		}
		if (CurOwner < 0)
		{
			return true;
		}
		return false;
	}

	public void ClearOwner()
	{
		CurOwner = -1;
	}

	public bool HaveAmmo()
	{
		this.m__E000 = Weapon.Item as Weapon;
		if (this.m__E000 == null)
		{
			return false;
		}
		return this.m__E000.GetCurrentMagazineCount() > 0;
	}

	public string AmmoInfo()
	{
		this.m__E000 = Weapon.Item as Weapon;
		if (this.m__E000 == null)
		{
			return _ED3E._E000(28419);
		}
		return string.Format(_ED3E._E000(28463), this.m__E000.Id, this.m__E000.GetCurrentMagazineCount());
	}
}
