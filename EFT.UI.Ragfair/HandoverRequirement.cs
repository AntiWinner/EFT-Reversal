using System;
using Comfort.Common;
using EFT.InventoryLogic;

namespace EFT.UI.Ragfair;

[Serializable]
public sealed class HandoverRequirement : IExchangeRequirement
{
	public int Level;

	public EDogtagExchangeSide Side;

	public Offer Offer { private get; set; }

	public string ItemName => (TemplateId + _ED3E._E000(182596)).Localized();

	public Item Item
	{
		get
		{
			if (_E000 != null)
			{
				return _E000;
			}
			_EAA0 obj = Singleton<_E63B>.Instance.CreateFakeStash();
			new _EB1E(obj, (Offer != null) ? Offer.Id : Guid.NewGuid().ToString(), _ED3E._E000(231960), canBeLocalized: false, EOwnerType.RagFairRequirement);
			_E000 = Singleton<_E63B>.Instance.GetPresetItem(TemplateId);
			obj.Grid.Add(_E000);
			return _E000;
		}
	}

	private Item _E000 { get; set; }

	public string TemplateId { get; }

	public int IntCount { get; }

	public double PreciseCount { get; }

	public bool OnlyFunctional { get; }

	public bool IsEncoded { get; }

	public HandoverRequirement()
	{
	}

	public HandoverRequirement(string template, double itemCount, bool onlyFunctional, bool isEncoded = false)
	{
		TemplateId = template;
		IntCount = (int)Math.Ceiling(itemCount);
		PreciseCount = itemCount;
		OnlyFunctional = onlyFunctional;
		IsEncoded = isEncoded;
	}
}
