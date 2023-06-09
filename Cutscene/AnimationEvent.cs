using System;

namespace Cutscene;

[Serializable]
public class AnimationEvent
{
	public int stateHash;

	public float time;

	public AnimationTrack fire;

	public float duration => fire?.duration ?? 0f;

	public string hint => fire?.hint ?? _ED3E._E000(71383);
}
