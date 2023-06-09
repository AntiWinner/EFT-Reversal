using EFT;
using UnityEngine;

public class SpecialAnimationBehaviour : StateMachineBehaviour
{
	public IKSettings IKDisable;

	public bool EnableSpecialAnimation = true;

	public float TresholdTime = 1f;

	public bool LeftArmReverseSnatching;

	private bool _E000;

	public Player Player;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		_E000 = false;
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (!_E000)
		{
			_E000 = true;
		}
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (!(TresholdTime >= 1f) && stateInfo.normalizedTime > TresholdTime)
		{
			OnStateExit(animator, stateInfo, layerIndex);
		}
	}
}
