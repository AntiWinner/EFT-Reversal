using EFT;

public class ObservedSmokeGrenade : SmokeGrenade
{
	protected override float PhysicsQuality => Grenade.PhysicsQualityForObserved;

	protected override _E383 GetVisibilityChecker()
	{
		return Grenade.GetVisibilityCheckerForObserved(this);
	}

	protected override void OnDoneFromNet()
	{
		RemoveRigidbody();
	}

	protected override void RemoveRigidBodyOnVelocityDrop(Throwable grenade)
	{
	}
}
