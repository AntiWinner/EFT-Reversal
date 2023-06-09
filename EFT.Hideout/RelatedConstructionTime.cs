using System;

namespace EFT.Hideout;

[Serializable]
public sealed class RelatedConstructionTime : RelatedData
{
	public float Data;

	public override bool IsActive => true;

	public override EPanelType Type => EPanelType.ConstructionTime;

	public override object Value => Data;
}
