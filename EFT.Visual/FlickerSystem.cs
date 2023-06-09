using System;

namespace EFT.Visual;

public class FlickerSystem : ComponentSystem<Flicker, FlickerSystem>
{
	protected override bool HasUpdate => true;

	protected override bool HasLateUpdate => false;

	protected override void UpdateComponent(Flicker component)
	{
		component.ManualUpdate();
	}

	protected override void LateUpdateComponent(Flicker component)
	{
		throw new NotImplementedException();
	}
}
