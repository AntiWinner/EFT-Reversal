using UnityEngine;

[_E506(typeof(_E33D))]
public class AnimatorStateDebugger : StateMachineBehaviour
{
	public string Name;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateEnter(animator, stateInfo, layerIndex);
		Debug.LogFormat(_ED3E._E000(58380), Time.frameCount, Name, stateInfo.normalizedTime);
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateExit(animator, stateInfo, layerIndex);
		Debug.LogFormat(_ED3E._E000(58401), Time.frameCount, Name, stateInfo.normalizedTime);
	}

	public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
	{
		base.OnStateMachineEnter(animator, stateMachinePathHash);
		Debug.LogFormat(_ED3E._E000(58485), Time.frameCount, Name);
	}

	public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
	{
		base.OnStateMachineExit(animator, stateMachinePathHash);
		Debug.LogFormat(_ED3E._E000(58504), Time.frameCount, Name);
	}
}
