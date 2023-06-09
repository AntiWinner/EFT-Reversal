using System;
using System.Collections;
using System.Collections.Generic;

namespace EFT.Hideout;

[Serializable]
public sealed class RelatedBonuses : RelatedData, IEnumerable<_E5EA>, IEnumerable
{
	public List<_E5EA> Data = new List<_E5EA>();

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

	public override EPanelType Type => EPanelType.Bonuses;

	public override object Value => Data;

	public IEnumerator<_E5EA> GetEnumerator()
	{
		return Data.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
