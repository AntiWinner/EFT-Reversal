using System;
using System.Collections;
using System.Collections.Generic;

namespace EFT.Hideout;

[Serializable]
public sealed class RelatedImprovement : RelatedData, IEnumerable<_E81B>, IEnumerable
{
	public List<_E81B> Data = new List<_E81B>();

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

	public override EPanelType Type => EPanelType.Improvement;

	public override object Value => Data;

	public IEnumerator<_E81B> GetEnumerator()
	{
		return Data.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
