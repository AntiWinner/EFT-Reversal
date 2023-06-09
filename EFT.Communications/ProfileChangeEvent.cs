using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace EFT.Communications;

[Serializable]
public sealed class ProfileChangeEvent
{
	private enum EAuxiliaryTypes
	{
		ProfileUnlock,
		LockTrader,
		AssortmentLockRule,
		ForgetItems
	}

	[JsonProperty("_id")]
	public string Id;

	public string MessageId;

	public ENotificationType Type;

	public string Entity;

	public double? Value;

	public bool Redeemed;

	public string EntityName
	{
		get
		{
			switch (Type)
			{
			case ENotificationType.TraderStanding:
			case ENotificationType.TraderSalesSum:
			case ENotificationType.UnlockTrader:
			case ENotificationType.TraderStandingDelta:
			case ENotificationType.TraderSalesSumDelta:
				return (Entity + _ED3E._E000(114050)).Localized();
			case ENotificationType.AssortmentUnlockRule:
			case ENotificationType.ExamineItems:
				return (Entity + _ED3E._E000(182596)).Localized();
			case ENotificationType.ProfileLevel:
			case ENotificationType.ProfileLockTimer:
			case ENotificationType.ProfileExperienceDelta:
				return null;
			default:
				if (!string.IsNullOrEmpty(Entity))
				{
					return Entity.Localized();
				}
				return null;
			}
		}
	}

	public string Description
	{
		get
		{
			Enum @enum = Type;
			string text = Value?.ToString();
			Enum enum2 = @enum;
			Enum enum3;
			if (enum2 != null && (enum3 = enum2) is ENotificationType)
			{
				switch ((int)(ENotificationType)(object)enum3)
				{
				case 39:
					@enum = (Enum)((Value > 0.0) ? ((object)ENotificationType.AssortmentUnlockRule) : ((object)EAuxiliaryTypes.AssortmentLockRule));
					break;
				case 40:
					@enum = (Enum)((Value > 0.0) ? ((object)ENotificationType.ExamineItems) : ((object)EAuxiliaryTypes.ForgetItems));
					break;
				case 43:
					@enum = (Enum)((Value > 0.0) ? ((object)ENotificationType.UnlockTrader) : ((object)EAuxiliaryTypes.LockTrader));
					break;
				case 44:
					@enum = (Enum)((Value > 0.0) ? ((object)ENotificationType.ProfileLockTimer) : ((object)EAuxiliaryTypes.ProfileUnlock));
					break;
				case 36:
					text = _E72F.GetLevel((int)Value.Value).ToString();
					break;
				case 46:
				case 47:
				case 48:
				case 49:
				case 50:
					if (Value.Value.Positive())
					{
						text = _ED3E._E000(29692) + text;
					}
					break;
				}
			}
			string text2 = (_ED3E._E000(167683) + @enum.ToString()).Localized();
			List<string> list = new List<string>();
			string entityName = EntityName;
			if (!string.IsNullOrEmpty(entityName))
			{
				list.Add(entityName);
			}
			if (!string.IsNullOrEmpty(text))
			{
				list.Add(text);
			}
			if (list.Any())
			{
				string format = text2;
				object[] args = list.ToArray();
				text2 = string.Format(format, args);
			}
			return text2;
		}
	}

	public event Action OnUpdate;

	public void Redeem()
	{
		Redeemed = true;
		this.OnUpdate?.Invoke();
	}

	public void Restore()
	{
		Redeemed = false;
		this.OnUpdate?.Invoke();
	}
}
