using System;
using System.Collections.Generic;
using Comfort.Common;

namespace EFT.UI.Ragfair;

[Serializable]
public struct FilterRule
{
	public EViewListType ViewListType;

	public int Page;

	public int CurrencyType;

	public ESortType SortType;

	public bool SortDirection;

	public int PriceFrom;

	public int PriceTo;

	public int QuantityFrom;

	public int QuantityTo;

	public int ConditionFrom;

	public int ConditionTo;

	public bool OneHourExpiration;

	public bool RemoveBartering;

	public int OfferOwnerType;

	public bool OnlyFunctional;

	public long OfferId;

	public string FilterSearchId;

	public string LinkedSearchId;

	public string NeededSearchId;

	[NonSerialized]
	public string BuildName;

	public Dictionary<string, int> BuildItems;

	public int BuildCount;

	private string _handbookId;

	public string HandbookId
	{
		get
		{
			if (ViewListType != 0 || !Singleton<_E5CB>.Instantiated)
			{
				return _handbookId;
			}
			if (string.IsNullOrEmpty(_handbookId) && string.IsNullOrEmpty(LinkedSearchId) && string.IsNullOrEmpty(NeededSearchId) && string.IsNullOrEmpty(FilterSearchId) && (BuildItems == null || BuildItems.Count == 0))
			{
				return Singleton<_E5CB>.Instance.Handbook.defaultCategory;
			}
			return _handbookId;
		}
		set
		{
			if (!string.IsNullOrEmpty(value))
			{
				_E39B.LogRagfair(_ED3E._E000(232383) + value + _ED3E._E000(197354) + (value + _ED3E._E000(70087)).Localized());
			}
			_handbookId = value;
		}
	}

	public BuildItemSearchValue BuildSearch => new BuildItemSearchValue
	{
		BuildName = BuildName,
		BuildItems = BuildItems,
		BuildCount = BuildCount
	};

	public bool AnyIdSearch
	{
		get
		{
			if (string.IsNullOrEmpty(LinkedSearchId) && string.IsNullOrEmpty(NeededSearchId))
			{
				return !string.IsNullOrEmpty(FilterSearchId);
			}
			return true;
		}
	}

	public bool ShowFilterChange(FilterRule rule)
	{
		if (CurrencyType == rule.CurrencyType && SortType == rule.SortType && SortDirection == rule.SortDirection && PriceFrom == rule.PriceFrom && PriceTo == rule.PriceTo && QuantityFrom == rule.QuantityFrom && QuantityTo == rule.QuantityTo && ConditionFrom == rule.ConditionFrom && ConditionTo == rule.ConditionTo && OneHourExpiration == rule.OneHourExpiration && RemoveBartering == rule.RemoveBartering && OfferOwnerType == rule.OfferOwnerType && OnlyFunctional == rule.OnlyFunctional && !(FilterSearchId != rule.FilterSearchId) && !(LinkedSearchId != rule.LinkedSearchId) && !(NeededSearchId != rule.NeededSearchId) && BuildItems == rule.BuildItems)
		{
			return BuildCount != rule.BuildCount;
		}
		return true;
	}

	public void ClearBuildFilter()
	{
		BuildName = string.Empty;
		BuildItems = new Dictionary<string, int>();
		BuildCount = 0;
	}

	public override string ToString()
	{
		return string.Concat(_ED3E._E000(232360), ViewListType, _ED3E._E000(232358), Page, _ED3E._E000(232415), CurrencyType, _ED3E._E000(232403), SortType, _ED3E._E000(232384), SortDirection.ToString(), _ED3E._E000(232434), PriceFrom, _ED3E._E000(232416), PriceTo, _ED3E._E000(242708), QuantityFrom, _ED3E._E000(242693), QuantityTo, _ED3E._E000(242740), ConditionFrom, _ED3E._E000(242726), ConditionTo, _ED3E._E000(242774), OneHourExpiration.ToString(), _ED3E._E000(242766), RemoveBartering.ToString(), _ED3E._E000(242815), OfferOwnerType, _ED3E._E000(242794), OnlyFunctional.ToString(), _ED3E._E000(242847), _handbookId, _ED3E._E000(242830), FilterSearchId, _ED3E._E000(242817), LinkedSearchId, _ED3E._E000(242868), NeededSearchId);
	}
}
