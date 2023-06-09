using System;

namespace EFT.GlobalEvents;

[Serializable]
public class EventFilterAnimationRelation
{
	public BaseEventFilter eventFilter;

	public ReactionOnEvent reaction;
}
