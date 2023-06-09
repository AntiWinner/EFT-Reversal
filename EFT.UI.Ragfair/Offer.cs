using System;
using System.Linq;
using Comfort.Common;
using EFT.InventoryLogic;
using Newtonsoft.Json;
using UnityEngine;

namespace EFT.UI.Ragfair;

public sealed class Offer : _E7D3
{
	public class _E000
	{
		public string Id;

		public string Nickname;

		public float Rating;

		public EMemberCategory MemberType;

		public string Avatar;

		public bool IsRatingGrowing;

		public string CorrectedNickname => Singleton<_E7DE>.Instance.Game.Controller.GetCorrectedProfileNickname(Id, Nickname);
	}

	private const int RENEW_MODIFIER = 25;

	public long IntId;

	public int ItemsCost;

	public int RequirementsCost;

	public DateTime StartTime;

	public string Name;

	public string ShortName;

	public int LoyaltyLevel;

	public bool Locked;

	public bool UnlimitedCount;

	public int SummaryCost;

	[JsonIgnore]
	public _E000 User;

	[JsonIgnore]
	public bool NotAvailable;

	public int TotalItemCount => Item.StackObjectsCount;

	[JsonIgnore]
	public bool OnlyMoney
	{
		get
		{
			if (Requirements != null && Requirements.Length == 1)
			{
				return _EA10.IsCurrencyId(Requirements[0].TemplateId);
			}
			return false;
		}
	}

	public string Id { get; set; }

	public EMemberCategory MemberType => User.MemberType;

	public Item Item { get; set; }

	public IExchangeRequirement[] Requirements { get; set; }

	public bool SellInOnePiece { get; set; }

	public DateTime EndTime { get; set; }

	public int BuyRestrictionMax { get; set; }

	[JsonIgnore]
	public int BuyRestrictionCurrent { get; set; }

	public bool Expired => (EndTime - _E5AD.UtcNow).TotalSeconds.Negative();

	public bool LimitsReached
	{
		get
		{
			if (BuyRestrictionMax > 0)
			{
				return BuyRestrictionCurrent >= BuyRestrictionMax;
			}
			return false;
		}
	}

	public bool CanBeBought
	{
		get
		{
			if (!NotAvailable && !Expired)
			{
				return !LimitsReached;
			}
			return false;
		}
	}

	public int RenewPercent
	{
		get
		{
			int stackObjectsCount = Item.StackObjectsCount;
			int num = 0;
			if (Requirements.Any())
			{
				num = ((Requirements[0].Item is _EA12) ? (stackObjectsCount / 120) : (stackObjectsCount - 1));
			}
			return 25 * num;
		}
	}

	public int CurrentItemCount
	{
		get
		{
			if (BuyRestrictionMax <= 0)
			{
				return Item.StackObjectsCount;
			}
			return Mathf.Min(Item.StackObjectsCount, BuyRestrictionMax - BuyRestrictionCurrent);
		}
	}

	public ECurrencyType MoneyType => _EA10.GetCurrencyTypeById(Requirements[0].TemplateId);

	public bool AvailableTimePassed { get; set; }

	public event Action<Offer, bool> OnSelectToPurchase;

	public static Offer CreateUnavailableOffer(string templateId)
	{
		Item item = Singleton<_E63B>.Instance.CreateItem(new MongoID(newProcessId: false), templateId, null);
		return new Offer
		{
			Item = item,
			NotAvailable = true,
			Name = item.Name,
			ShortName = item.ShortName
		};
	}

	public void SelectToPurchase()
	{
		this.OnSelectToPurchase?.Invoke(this, arg2: true);
	}

	public void DeselectFromPurchase()
	{
		this.OnSelectToPurchase?.Invoke(this, arg2: false);
	}

	public override string ToString()
	{
		return string.Format(_ED3E._E000(231943), Id, StartTime);
	}
}
