using System;

namespace EFT.Interactive;

public class LampSystem : ComponentSystem<LampController, LampSystem>
{
	protected override bool HasUpdate => true;

	protected override bool HasLateUpdate => false;

	protected override void UpdateComponent(LampController component)
	{
		component.ManualUpdate();
	}

	protected override void LateUpdateComponent(LampController component)
	{
		throw new NotImplementedException();
	}
}
