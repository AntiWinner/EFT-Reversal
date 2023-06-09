using System;
using System.Collections.Generic;
using Cutscene;

namespace EFT.GlobalEvents;

[Serializable]
public class ReactionOnEvent
{
	public List<string> animationBoolNames = new List<string>();

	public List<string> animationTriggerNames = new List<string>();

	public NPCGlobalEventsReacting.AnimationInt animationInt;

	public AnimationTrack lipSyncData = new AnimationTrack();

	public bool clearReactionQueue;
}
