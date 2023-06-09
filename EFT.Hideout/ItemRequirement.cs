using System;
using System.Collections.Generic;
using System.Linq;
using Comfort.Common;
using EFT.InventoryLogic;
using Newtonsoft.Json;

namespace EFT.Hideout;

[Serializable]
public class ItemRequirement : Requirement, IExchangeRequirement
{
	private int _notEmptyCompoundItems;

	private Item _item;

	private int _baseCount = 1;

	public override ERequirementType Type => ERequirementType.Item;

	[JsonProperty("templateId")]
	public string TemplateId { get; private set; }

	[JsonProperty("count")]
	public int BaseCount
	{
		get
		{
			return _baseCount;
		}
		private set
		{
			_baseCount = value;
			IntCount = value;
		}
	}

	public double PreciseCount => IntCount;

	[JsonProperty("isFunctional")]
	public bool OnlyFunctional { get; private set; }

	[JsonProperty("isEncoded")]
	public bool IsEncoded { get; private set; }

	[JsonIgnore]
	public int UserItemsCount { get; private set; }

	[JsonIgnore]
	public int IntCount { get; private set; } = 1;


	[JsonIgnore]
	public Item Item => _item ?? (_item = Singleton<_E63B>.Instance.CreateItem(new MongoID(newProcessId: false), TemplateId, null));

	[JsonIgnore]
	public string ItemName => (TemplateId + _ED3E._E000(182596)).Localized();

	public float CountModifier
	{
		set
		{
			int num = Convert.ToInt32(Math.Max((float)BaseCount * (1f - value), 1f));
			if (IntCount != num)
			{
				IntCount = num;
				Retest();
			}
		}
	}

	public bool IsSuitableItem(Item item)
	{
		if (item.TemplateId != TemplateId)
		{
			return false;
		}
		if (OnlyFunctional && item is Weapon weapon && weapon.MissingVitalParts.Any())
		{
			return false;
		}
		if (item is _EA85 obj && Item is _EA85)
		{
			bool num = obj.IsEncoded();
			bool isEncoded = IsEncoded;
			return num == isEncoded;
		}
		return true;
	}

	public void Retest()
	{
		if (UserItemsCount >= IntCount && _notEmptyCompoundItems > 0 && UserItemsCount - _notEmptyCompoundItems < IntCount)
		{
			base.Error = new _EA07();
		}
		else
		{
			base.Error = null;
		}
		TestRequirement(UserItemsCount, IntCount);
	}

	public void Test(IEnumerable<Item> value)
	{
		UserItemsCount = 0;
		_notEmptyCompoundItems = 0;
		foreach (Item item in value)
		{
			if (IsSuitableItem(item))
			{
				UserItemsCount += item.StackObjectsCount;
				if (item is _EA40 obj && !obj.IsEmpty)
				{
					_notEmptyCompoundItems++;
				}
			}
		}
		Retest();
	}
}
