using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Diz.Binding;
using Newtonsoft.Json;

namespace EFT.Quests;

public abstract class Condition : IUpdatable<Condition>
{
	[JsonProperty("visibilityConditions")]
	public Condition[] VisibilityConditions = Array.Empty<Condition>();

	public string parentId;

	private int _identity;

	public _ED07<Condition> ChildConditions = new _ED07<Condition>();

	[JsonProperty("oneSessionOnly")]
	public bool resetOnSessionEnd;

	[JsonProperty]
	public string id { get; protected set; }

	[DefaultValue(1)]
	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public float value { get; set; }

	[DefaultValue(ECompareMethod.MoreOrEqual)]
	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public ECompareMethod compareMethod { get; protected set; }

	[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
	public int index { get; protected set; }

	[JsonProperty("dynamicLocale")]
	public bool DynamicLocale { get; protected set; }

	[JsonIgnore]
	public bool IsNecessary => string.IsNullOrEmpty(parentId);

	[JsonIgnore]
	public virtual string FormattedDescription => id.Localized();

	public int GetIdentity()
	{
		if (_identity == 0)
		{
			_identity = CalculateIdentity(IdentityFields().ToArray());
		}
		return _identity;
	}

	protected virtual List<object> IdentityFields()
	{
		return new List<object> { GetType() };
	}

	public static int CalculateIdentity(object[] fields)
	{
		List<int> list = fields.Select((object field) => field?.GetHashCode() ?? 0).ToList();
		list.Sort();
		return list.Aggregate(17, (int current, int hashCode) => current * 23 + hashCode);
	}

	public override string ToString()
	{
		return $"{GetType()}:({id}) {compareMethod} {value}";
	}

	public bool Compare(Condition other)
	{
		return id == other.id;
	}

	public void UpdateFromAnotherItem(Condition other)
	{
		value = other.value;
		compareMethod = other.compareMethod;
		index = other.index;
		parentId = other.parentId;
	}
}
