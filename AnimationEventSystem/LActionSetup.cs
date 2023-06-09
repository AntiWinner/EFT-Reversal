using System;
using UnityEngine;

namespace AnimationEventSystem;

[Serializable]
public class LActionSetup
{
	public float StartThreshold = 0.9f;

	public AnimationCurve LayerWeight;

	[SerializeField]
	private int _stateHash;

	public int StateHash
	{
		get
		{
			return _stateHash;
		}
		set
		{
			_stateHash = value;
		}
	}

	public LActionSetup(AnimationCurve animationCurve, float startThreshold)
	{
		LayerWeight = ((animationCurve == null) ? new AnimationCurve() : new AnimationCurve(animationCurve.keys));
		StartThreshold = startThreshold;
	}
}
