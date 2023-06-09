using UnityEngine;

namespace DefaultNamespace;

[_E505]
public class LayerWeightStateController : StateMachineBehaviour, IStateBehaviour
{
	public float StartValue;

	public float EndValue;

	public AnimationCurve LayerWeight;

	private _E564 m__E000;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		_E000(animator);
		animator.SetLayerWeight(2, StartValue);
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		_E000(animator);
		float weight = LayerWeight.Evaluate(stateInfo.normalizedTime);
		animator.SetLayerWeight(2, weight);
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		_E000(animator);
		animator.SetLayerWeight(2, EndValue);
	}

	private void _E000(Animator animator)
	{
		if (this.m__E000 == null)
		{
			this.m__E000 = _E564.Create();
		}
		this.m__E000.SetAnimator(animator);
	}
}
