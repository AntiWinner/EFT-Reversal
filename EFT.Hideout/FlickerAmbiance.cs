using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EFT.Visual;
using UnityEngine;

namespace EFT.Hideout;

public sealed class FlickerAmbiance : AmbianceObject<AnimationCurve>
{
	[SerializeField]
	private List<Flicker> _flickers;

	public override async Task<bool> PerformInteraction(ELightStatus status)
	{
		if (!(await base.PerformInteraction(status)))
		{
			return false;
		}
		AnimationCurve value = Patterns[status].Value;
		foreach (Flicker flicker in _flickers)
		{
			flicker.SetCurve(value);
		}
		return true;
	}

	[DebuggerHidden]
	[CompilerGenerated]
	private Task<bool> _E000(ELightStatus status)
	{
		return base.PerformInteraction(status);
	}
}
