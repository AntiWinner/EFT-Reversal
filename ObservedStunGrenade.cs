using EFT;
using UnityEngine;

public class ObservedStunGrenade : StunGrenade
{
	protected override float PhysicsQuality => Grenade.PhysicsQualityForObserved;

	protected override _E383 GetVisibilityChecker()
	{
		return Grenade.GetVisibilityCheckerForObserved(this);
	}

	protected override void StartTimer()
	{
		this.StartBehaviourTimer(base.WeaponSource.GetExplDelay * 2f, _E000);
	}

	private void _E000()
	{
		Object.Destroy(base.gameObject);
	}

	protected override void OnDoneFromNet()
	{
		InvokeBlowUpEvent();
	}
}
