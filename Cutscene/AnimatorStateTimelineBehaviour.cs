using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Cutscene;

public class AnimatorStateTimelineBehaviour : StateMachineBehaviour
{
	public int stateHash;

	public AnimatorStateTimelineData data;

	private _E453 m__E000;

	private int m__E001;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		_E001(animator);
		_E000(animator, stateInfo, layerIndex);
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		_E001(animator);
		_E000(animator, stateInfo, layerIndex);
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		_E001(animator);
		this.m__E001 = 0;
		this.m__E000.OnStateExit(-1f, stateInfo.length);
	}

	private void _E000(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		int num = (int)stateInfo.normalizedTime;
		float num2 = stateInfo.normalizedTime - (float)num;
		if (num != this.m__E001)
		{
			if (num2 > 0f)
			{
				this.m__E000.OnStateUpdate(0f, stateInfo.length);
			}
			this.m__E001 = num;
		}
		this.m__E000.OnStateUpdate(num2, stateInfo.length);
	}

	private void _E001(Animator animator)
	{
		if (this.m__E000 == null)
		{
			this.m__E000 = new _E453(animator.gameObject, _E453.ESyncTime.AudioSystem, data.events.Where((AnimationEvent e) => e.stateHash == stateHash).ToArray());
		}
	}

	[CompilerGenerated]
	private bool _E002(AnimationEvent e)
	{
		return e.stateHash == stateHash;
	}
}
