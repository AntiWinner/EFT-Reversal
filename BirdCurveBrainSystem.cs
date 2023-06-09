using System;
using EFT.Animals;
using UnityEngine;

public class BirdCurveBrainSystem : ComponentSystem<BirdCurveBrain, BirdCurveBrainSystem>
{
	protected override bool HasUpdate => true;

	protected override bool HasLateUpdate => false;

	protected override void UpdateComponent(BirdCurveBrain component)
	{
		component.ManualUpdate(Time.deltaTime);
	}

	protected override void LateUpdateComponent(BirdCurveBrain component)
	{
		throw new NotImplementedException();
	}
}
