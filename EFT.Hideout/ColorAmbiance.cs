using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace EFT.Hideout;

public sealed class ColorAmbiance : AmbianceObject<Color>
{
	[SerializeField]
	private Light[] _lights;

	public override async Task<bool> PerformInteraction(ELightStatus status)
	{
		if (!(await base.PerformInteraction(status)))
		{
			return false;
		}
		Color value = Patterns[status].Value;
		Light[] lights = _lights;
		for (int i = 0; i < lights.Length; i++)
		{
			lights[i].color = value;
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
