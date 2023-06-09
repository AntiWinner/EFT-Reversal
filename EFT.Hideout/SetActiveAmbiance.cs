using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace EFT.Hideout;

public sealed class SetActiveAmbiance : AmbianceObject<bool>
{
	public override async Task<bool> PerformInteraction(ELightStatus status)
	{
		if (!(await base.PerformInteraction(status)))
		{
			return false;
		}
		bool value = Patterns[status].Value;
		base.gameObject.SetActive(value);
		return true;
	}

	[DebuggerHidden]
	[CompilerGenerated]
	private Task<bool> _E000(ELightStatus status)
	{
		return base.PerformInteraction(status);
	}
}
