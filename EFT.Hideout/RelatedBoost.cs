using System;
using System.Collections.Generic;

namespace EFT.Hideout;

[Serializable]
public sealed class RelatedBoost : RelatedData
{
	public List<Requirement> Data = new List<Requirement>();

	public override bool IsActive
	{
		get
		{
			if (Data != null)
			{
				return Data.Count > 0;
			}
			return false;
		}
	}

	public override EPanelType Type => EPanelType.Boost;

	public override object Value => Data;
}
