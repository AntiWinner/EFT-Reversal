using UnityEngine;

public class FirstPersonAuthority : StateMachineBehaviour
{
	public AnimationCurve WeightCurve;

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.SetLayerWeight(layerIndex, WeightCurve.Evaluate(stateInfo.normalizedTime));
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.SetLayerWeight(layerIndex, WeightCurve.Evaluate(1f));
	}
}
