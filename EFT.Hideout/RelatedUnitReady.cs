using System;

namespace EFT.Hideout;

[Serializable]
public sealed class RelatedUnitReady : _E839
{
	public override EPanelType Type => EPanelType.UnitReady;
}
