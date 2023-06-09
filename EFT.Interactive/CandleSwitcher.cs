using System;
using UnityEngine;

namespace EFT.Interactive;

public sealed class CandleSwitcher : MonoBehaviour, _EBFD
{
	private bool _E000 = true;

	public bool Enabled
	{
		get
		{
			return _E000;
		}
		set
		{
			_E000 = value;
			Switch(_E000 ? Turnable.EState.On : Turnable.EState.Off);
		}
	}

	public void Switch(Turnable.EState switchTo)
	{
		if (!_E000 && (switchTo == Turnable.EState.On || switchTo == Turnable.EState.TurningOn || switchTo == Turnable.EState.ConstantFlickering))
		{
			switchTo = Turnable.EState.Off;
		}
		switch (switchTo)
		{
		case Turnable.EState.On:
		case Turnable.EState.ConstantFlickering:
			base.gameObject.SmartEnable();
			break;
		case Turnable.EState.Off:
			base.gameObject.SmartDisable();
			break;
		default:
			throw new ArgumentOutOfRangeException(_ED3E._E000(212501), switchTo, null);
		case Turnable.EState.TurningOn:
		case Turnable.EState.TurningOff:
		case Turnable.EState.Destroyed:
			break;
		}
	}
}
