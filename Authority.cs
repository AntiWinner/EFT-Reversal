using UnityEngine;

public class Authority : StateMachineBehaviour, IStateBehaviour
{
	public PlayerBones PlayerBones;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (PlayerBones == null)
		{
			PlayerBones = animator.gameObject.GetComponent<PlayerBones>();
		}
	}
}
