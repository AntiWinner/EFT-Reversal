using EFT;
using UnityEngine;

public class ObservedGrenade : Grenade
{
	protected override float PhysicsQuality => Grenade.PhysicsQualityForObserved;

	protected override _E383 GetVisibilityChecker()
	{
		return Grenade.GetVisibilityCheckerForObserved(this);
	}

	protected override void StartTimer()
	{
		this.StartBehaviourTimer(base.WeaponSource.GetExplDelay * 3f, _E000);
	}

	private void _E000()
	{
		Debug.LogWarning(_ED3E._E000(45265));
		Object.Destroy(base.gameObject);
	}

	protected override void OnDoneFromNet()
	{
		base.OnDoneFromNet();
		InvokeBlowUpEvent();
	}

	protected override void ProcessContactExplodeCollision(float impulse)
	{
	}
}
