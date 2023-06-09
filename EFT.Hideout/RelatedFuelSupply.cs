using System;
using System.Collections.Generic;

namespace EFT.Hideout;

[Serializable]
public sealed class RelatedFuelSupply : RelatedData
{
	public List<_E837> Data = new List<_E837>();

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

	public override EPanelType Type => EPanelType.FuelSupply;

	public override object Value => Data;
}
