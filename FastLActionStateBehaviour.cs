using System;
using UnityEngine;

[Serializable]
public class FastLActionStateBehaviour : _E4EF
{
	public float StartTreshold;

	public AnimationCurve LayerWeight;

	private LActionState._E000 _context;

	private LActionState._E000 _E000
	{
		get
		{
			if (_context == null)
			{
				_context = new LActionState._E000(LayerWeight, StartTreshold);
			}
			return _context;
		}
	}

	public override _E4EF Clone()
	{
		FastLActionStateBehaviour fastLActionStateBehaviour = new FastLActionStateBehaviour();
		fastLActionStateBehaviour.StartTreshold = StartTreshold;
		fastLActionStateBehaviour.LayerWeight = new AnimationCurve(LayerWeight.keys);
		fastLActionStateBehaviour.LayerWeight.postWrapMode = LayerWeight.postWrapMode;
		fastLActionStateBehaviour.LayerWeight.preWrapMode = LayerWeight.preWrapMode;
		return fastLActionStateBehaviour;
	}

	public override void OnStateEnter(_E502 fastAnimatorProcessor, in AnimatorStateInfoWrapper layerInfo, int layerIndex)
	{
		_E000.OnStateEnter(fastAnimatorProcessor, layerInfo, layerIndex);
	}

	public override void OnStateUpdate(_E502 fastAnimatorProcessor, in AnimatorStateInfoWrapper layerInfo, int layerIndex)
	{
		_E000.OnStateUpdate(fastAnimatorProcessor, layerInfo, layerIndex);
	}

	public override void OnStateExit(_E502 fastAnimatorProcessor, in AnimatorStateInfoWrapper layerInfo, int layerIndex)
	{
		_E000.OnStateExit(fastAnimatorProcessor, layerInfo, layerIndex);
	}
}
