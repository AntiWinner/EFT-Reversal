using UnityEngine;

namespace EFT.Interactive;

public sealed class PlaceItemTrigger : TriggerWithId
{
	[SerializeField]
	private GameObject _beaconDummy;

	public GameObject BeaconDummy => _beaconDummy;

	public void SetBeaconDummy(GameObject dummy)
	{
		_beaconDummy = dummy;
	}

	protected override void TriggerEnter(Player player)
	{
		player.OnPlaceItemTriggerChanged(this);
	}

	protected override void TriggerExit(Player player)
	{
		player.OnPlaceItemTriggerChanged(null);
	}
}
