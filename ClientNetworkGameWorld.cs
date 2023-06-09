using EFT;
using EFT.Interactive;
using EFT.InventoryLogic;
using UnityEngine;

internal class ClientNetworkGameWorld : ClientGameWorld
{
	public override LootItem CreateLootWithRigidbody(GameObject lootObject, Item item, string objectName, GameWorld gameWorld, bool randomRotation, string[] validProfiles, out BoxCollider objectCollider, bool forceLayerSetup = false, bool syncable = true, bool performPickUpValidation = true, float makeVisibleAfterDelay = 0f)
	{
		if (syncable)
		{
			return LootItem.CreateLootWithRigidbody<ObservedLootItem>(lootObject, item, objectName, gameWorld, randomRotation, validProfiles, out objectCollider, forceLayerSetup, performPickUpValidation, makeVisibleAfterDelay);
		}
		return base.CreateLootWithRigidbody(lootObject, item, objectName, gameWorld, randomRotation, validProfiles, out objectCollider, forceLayerSetup, syncable: true, performPickUpValidation, makeVisibleAfterDelay);
	}

	protected override _E334 CreateGrenadeFactory()
	{
		return new _E3CF();
	}

	protected override bool IsLocalGame()
	{
		return false;
	}
}
