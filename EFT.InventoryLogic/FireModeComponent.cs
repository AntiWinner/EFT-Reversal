using System;
using System.Runtime.CompilerServices;

namespace EFT.InventoryLogic;

public class FireModeComponent : _EB19
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public Weapon.EFireMode fireMode;

		internal bool _E000(Weapon.EFireMode mode)
		{
			return mode == fireMode;
		}
	}

	private readonly _E9D8 m__E000;

	[NonSerialized]
	public _ECEC OnChanged = new _ECEC();

	[_E63C]
	public Weapon.EFireMode FireMode;

	public Weapon.EFireMode[] AvailableEFireModes => this.m__E000.AvailablEFireModes;

	public int BurstShotsCount => this.m__E000.BurstShotsCount;

	public void SetFireMode(Weapon.EFireMode fireMode)
	{
		if (Array.FindIndex(this.m__E000.AvailablEFireModes, (Weapon.EFireMode mode) => mode == fireMode) >= 0)
		{
			FireMode = fireMode;
		}
		OnChanged.Invoke();
		Item.UpdateAttributes();
	}

	public FireModeComponent(Item item, _E9D8 template)
		: base(item)
	{
		this.m__E000 = template;
		FireMode = this.m__E000.AvailablEFireModes[0];
	}

	public Weapon.EFireMode GetNextFireMode()
	{
		int i = 0;
		Weapon.EFireMode[] availablEFireModes;
		for (availablEFireModes = this.m__E000.AvailablEFireModes; i < availablEFireModes.Length && availablEFireModes[i] != FireMode; i++)
		{
		}
		i = (i + 1) % availablEFireModes.Length;
		return availablEFireModes[i];
	}

	public Weapon.EFireMode GetForceAutoFireMode()
	{
		Weapon.EFireMode[] availablEFireModes = this.m__E000.AvailablEFireModes;
		int num = Array.IndexOf(availablEFireModes, Weapon.EFireMode.fullauto);
		if (num >= 0)
		{
			return availablEFireModes[num];
		}
		num = Array.IndexOf(availablEFireModes, Weapon.EFireMode.burst);
		if (num >= 0)
		{
			return availablEFireModes[num];
		}
		num = Array.IndexOf(availablEFireModes, Weapon.EFireMode.doublet);
		if (num >= 0)
		{
			return availablEFireModes[num];
		}
		num = Array.IndexOf(availablEFireModes, Weapon.EFireMode.doubleaction);
		if (num >= 0)
		{
			return availablEFireModes[num];
		}
		return FireMode;
	}
}
