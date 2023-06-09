using EFT;
using UnityEngine;

public class AuthorityBlendValue : StateMachineBehaviour
{
	[HideInInspector]
	public Player.ValueBlender Blender;

	[HideInInspector]
	public Player.ValueBlender Temporary_Right_Hand_Copy;

	public float Speed = 4f;

	public float Target = 1f;

	public bool RightHand;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (Blender != null)
		{
			Blender.Speed = Speed;
			Blender.Target = Target;
			Temporary_Right_Hand_Copy.Speed = Speed;
			Temporary_Right_Hand_Copy.Target = (RightHand ? 1 : 0);
		}
	}
}
