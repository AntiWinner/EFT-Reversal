using System;

namespace EFT.Hideout;

[Serializable]
public sealed class RelatedDescription : _E839
{
	public override EPanelType Type => EPanelType.Description;
}
