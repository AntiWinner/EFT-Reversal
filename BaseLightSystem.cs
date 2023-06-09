using System;
using UnityEngine;

public class BaseLightSystem : ComponentSystem<BaseLight, BaseLightSystem>
{
	protected override bool HasUpdate => true;

	protected override bool HasLateUpdate => false;

	protected override void UpdateComponent(BaseLight component)
	{
		component.ManualUpdate(Time.deltaTime);
	}

	protected override void LateUpdateComponent(BaseLight component)
	{
		throw new NotImplementedException();
	}
}
