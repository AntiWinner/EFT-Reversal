using System;

namespace EFT.Hideout;

[Serializable]
public sealed class RelatedProduction : RelatedData
{
	public _E828[] Data;

	public override bool IsActive
	{
		get
		{
			if (Data != null)
			{
				return Data.Length != 0;
			}
			return false;
		}
	}

	public override EPanelType Type => EPanelType.Production;

	public override object Value => Data;
}
