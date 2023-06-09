using System.Runtime.CompilerServices;
using EFT.MovingPlatforms;
using UnityEngine;

namespace EFT.Interactive;

public sealed class PlatformImpactTrigger : DamageTrigger
{
	[SerializeField]
	private Locomotive Locomotive;

	[SerializeField]
	private AnimationCurve DamagePerSpeed;

	[CompilerGenerated]
	private readonly string _E003 = _ED3E._E000(212866);

	public override string Description
	{
		[CompilerGenerated]
		get
		{
			return _E003;
		}
	}

	protected override bool IsStatic => false;

	protected override void ProceedDamage(Player player, BodyPartCollider bodyPart)
	{
		float num = DamagePerSpeed.Evaluate(Locomotive.TravelSpeed);
		if (num >= 1f)
		{
			bodyPart.ProceedPlatformImpact(num);
		}
	}

	protected override void AddPenalty(Player player)
	{
	}

	protected override void RemovePenalty(Player player)
	{
	}

	protected override void PlaySound()
	{
	}
}
