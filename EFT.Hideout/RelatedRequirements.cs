using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EFT.Hideout;

[Serializable]
public sealed class RelatedRequirements : RelatedData, IEnumerable<Requirement>, IEnumerable
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

	public override EPanelType Type => EPanelType.Requirements;

	public override object Value => Data;

	public IEnumerator<Requirement> GetEnumerator()
	{
		return Data.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public IEnumerable<Type> GetFilteredTypeList()
	{
		return from x in typeof(RelatedRequirements).Assembly.GetTypes()
			where !x.IsAbstract
			where !x.IsGenericTypeDefinition
			where typeof(Requirement).IsAssignableFrom(x)
			select x;
	}
}
