using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EFT.Interactive;
using EFT.Utilities;
using UnityEngine;

namespace EFT.Hideout;

public sealed class LampControllerAmbiance : InteractiveAmbianceObject<LampControllerAmbiance.LampControllerPattern>
{
	[Serializable]
	public sealed class LampControllerPattern
	{
		public bool Active = true;

		public RandomBetweenFloats Flickering;

		[Range(0f, 100f)]
		public float FlickeringChance;
	}

	public LampController AffectedObject;

	public override async Task<bool> PerformInteraction(ELightStatus status)
	{
		if (!(await base.PerformInteraction(status)) || !base.gameObject.activeInHierarchy || !Patterns.TryGetValue(status, out var value))
		{
			return false;
		}
		if (value.Value.Active)
		{
			AffectedObject.Switch(Turnable.EState.On);
		}
		else if ((float)UnityEngine.Random.Range(0, 100) < value.Value.FlickeringChance)
		{
			AffectedObject.Switch(Turnable.EState.ConstantFlickering);
			AffectedObject.FlickeringFreq = value.Value.Flickering.Result;
		}
		else
		{
			AffectedObject.Switch(Turnable.EState.Off);
		}
		return true;
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private Task<bool> _E000(ELightStatus status)
	{
		return base.PerformInteraction(status);
	}
}
