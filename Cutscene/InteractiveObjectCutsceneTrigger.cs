using System;
using EFT.Interactive;
using UnityEngine;

namespace Cutscene;

public class InteractiveObjectCutsceneTrigger : BaseCutsceneTrigger
{
	[Serializable]
	private class StartCutsceneCondition
	{
		public EDoorState prevObjectState;

		public EDoorState nextObjectState;

		public bool IsPassingCondition(EDoorState prevState, EDoorState nextState)
		{
			if (prevState == prevObjectState)
			{
				return nextState == nextObjectState;
			}
			return false;
		}
	}

	[SerializeField]
	private WorldInteractiveObject interactiveObject;

	[SerializeField]
	private StartCutsceneCondition condition;

	protected override void Awake()
	{
		base.Awake();
		interactiveObject.OnDoorStateChanged += _E000;
	}

	private void _E000(WorldInteractiveObject obj, EDoorState prevState, EDoorState nextState)
	{
		if (condition.IsPassingCondition(prevState, nextState))
		{
			_E001();
		}
	}

	private void _E001()
	{
		if (!(interactiveObject.InteractingPlayer == null) && interactiveObject.InteractingPlayer.IsYourPlayer)
		{
			CallStartCutscene(interactiveObject.InteractingPlayer);
		}
	}
}
