using UnityEngine;

public class MuzzleSystem : ComponentSystem<MuzzleManager, MuzzleSystem>
{
	private Camera _E000;

	protected override bool HasUpdate => true;

	protected override bool HasLateUpdate => true;

	protected override void UpdateComponent(MuzzleManager component)
	{
		component.ManualUpdate();
	}

	protected override void LateUpdate()
	{
		_E000 = _E8A8.Instance.Camera;
		if (_E000 == null)
		{
			_E000 = Camera.main;
		}
		base.LateUpdate();
	}

	protected override void LateUpdateComponent(MuzzleManager component)
	{
		component.ManualLateUpdate();
		component.LateUpdateMuzzleEffectsValues(_E000);
	}
}
