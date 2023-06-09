using System;
using UnityEngine;

public class FloatingObjectManager : ComponentSystem<FloatingObject, FloatingObjectManager>
{
	protected override bool HasUpdate => true;

	protected override bool HasLateUpdate => false;

	protected override void UpdateComponent(FloatingObject component)
	{
		component.ManualUpdate(Time.deltaTime);
	}

	protected override void LateUpdateComponent(FloatingObject component)
	{
		throw new NotImplementedException();
	}
}
