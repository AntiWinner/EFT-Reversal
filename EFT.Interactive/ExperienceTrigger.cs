using UnityEngine;

namespace EFT.Interactive;

public sealed class ExperienceTrigger : TriggerWithId
{
	[SerializeField]
	private int _experience;

	private void Awake()
	{
		base.gameObject.layer = LayerMask.NameToLayer(_ED3E._E000(25347));
	}

	protected override void TriggerEnter(Player player)
	{
		base.TriggerEnter(player);
		player.SpecialPlaceVisited(base.Id, _experience);
	}
}
