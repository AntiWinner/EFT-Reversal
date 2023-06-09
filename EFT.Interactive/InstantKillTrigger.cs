using System.Runtime.CompilerServices;

namespace EFT.Interactive;

public class InstantKillTrigger : DamageTrigger
{
	[CompilerGenerated]
	private readonly string _E003 = _ED3E._E000(212677);

	public override string Description
	{
		[CompilerGenerated]
		get
		{
			return _E003;
		}
	}

	protected override bool IsStatic => true;

	protected override void ProceedDamage(Player player, BodyPartCollider bodyPart)
	{
		bodyPart.ProceedInstantKill();
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
