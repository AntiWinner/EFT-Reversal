using System;
using System.Collections.Generic;
using EFT.Communications;
using EFT.HealthSystem;
using EFT.InventoryLogic;
using EFT.Quests;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT.UI;

[Serializable]
public sealed class StaticIcons : ScriptableObject
{
	[Serializable]
	public sealed class ItemTypeToSpriteDictionary : SerializableEnumDictionary<EItemType, Sprite>
	{
	}

	[Serializable]
	public sealed class QuestTypeToSpriteDictionary : SerializableEnumDictionary<_E937.EQuestType, Sprite>
	{
	}

	[Serializable]
	public sealed class QuestIconTypeToSpriteDictionary : SerializableEnumDictionary<EQuestIconType, Sprite>
	{
	}

	[Serializable]
	public sealed class BuffIdToSpriteDictionary : SerializableEnumDictionary<EBuffId, Sprite>
	{
	}

	[Serializable]
	public sealed class SkillIdToSpriteDictionary : SerializableEnumDictionary<ESkillId, Sprite>
	{
	}

	[Serializable]
	public sealed class StatGroupIdToSpriteDictionary : SerializableEnumDictionary<EStatGroupId, Sprite>
	{
	}

	[Serializable]
	public sealed class DamageResultToSpriteDictionary : SerializableEnumDictionary<DamageStats.EDamageResult, Sprite>
	{
	}

	[Serializable]
	public sealed class NotificationIconTypeToSpriteDictionary : SerializableEnumDictionary<ENotificationIconType, Sprite>
	{
	}

	[Serializable]
	public sealed class GestureToSpriteDictionary : SerializableEnumDictionary<EGesture, Sprite>
	{
	}

	[Serializable]
	public sealed class ItemAttributeIdToSpriteDictionary : SerializableEnumDictionary<EItemAttributeId, Sprite>
	{
	}

	[Serializable]
	public sealed class HealthFactorTypeToSpriteDictionary : SerializableEnumDictionary<EHealthFactorType, Sprite>
	{
	}

	[Serializable]
	public sealed class DamageEffectTypeToSpriteDictionary : SerializableEnumDictionary<EDamageEffectType, Sprite>
	{
	}

	[Serializable]
	public sealed class StimulatorBuffTypeToSpriteDictionary : SerializableEnumDictionary<EStimulatorBuffType, Sprite>
	{
	}

	[Serializable]
	public sealed class CurrencyTypeToSpriteDictionary : SerializableEnumDictionary<ECurrencyType, Sprite>
	{
	}

	[Serializable]
	public sealed class TraderDialogLineSpriteDictionary : SerializableEnumDictionary<_E8B6.EDialogLiteIconType, Sprite>
	{
	}

	[Serializable]
	public sealed class EffectSprites : ISerializationCallbackReceiver
	{
		public Sprite Painkiller;

		public Sprite Tremor;

		public Sprite Fracture;

		public Sprite Exhaustion;

		public Sprite TunnelVision;

		public Sprite Dehydration;

		public Sprite Contusion;

		public Sprite Wound;

		public Sprite LightBleeding;

		public Sprite HeavyBleeding;

		public Sprite Pain;

		public Sprite Berserk;

		public Sprite Stun;

		public Sprite StimulatorBuff;

		public Sprite StimulatorDebuff;

		public Sprite ChronicStaminaFatigue;

		public Sprite Encumbered;

		public Sprite OverEncumbered;

		public Sprite Intoxication;

		public Sprite LethalIntoxication;

		public Sprite ImmunityPreventedContamination;

		public Sprite HalloweenBuff;

		public Sprite MildMusclePain;

		public Sprite SevereMusclePain;

		[NonSerialized]
		public readonly Dictionary<Type, Sprite> EffectIcons = new Dictionary<Type, Sprite>(30);

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			EffectIcons.Clear();
			EffectIcons.Add(typeof(_E9B1), Painkiller);
			EffectIcons.Add(typeof(_E9B4), Tremor);
			EffectIcons.Add(typeof(_E9A2), Fracture);
			EffectIcons.Add(typeof(_E9A4), Exhaustion);
			EffectIcons.Add(typeof(_E9B6), TunnelVision);
			EffectIcons.Add(typeof(_E9A3), Dehydration);
			EffectIcons.Add(typeof(_E9AC), Contusion);
			EffectIcons.Add(typeof(_E9B5), Wound);
			EffectIcons.Add(typeof(_E99F), LightBleeding);
			EffectIcons.Add(typeof(_E9A0), HeavyBleeding);
			EffectIcons.Add(typeof(_E9B0), Pain);
			EffectIcons.Add(typeof(_E9AA), Berserk);
			EffectIcons.Add(typeof(_E9B3), Stun);
			EffectIcons.Add(typeof(_E9B9), ChronicStaminaFatigue);
			EffectIcons.Add(typeof(_E9B7), Encumbered);
			EffectIcons.Add(typeof(_E9B8), OverEncumbered);
			EffectIcons.Add(typeof(_E986), StimulatorBuff);
			EffectIcons.Add(typeof(_E985), StimulatorDebuff);
			EffectIcons.Add(typeof(_E9A6), Intoxication);
			EffectIcons.Add(typeof(_E9A7), LethalIntoxication);
			EffectIcons.Add(typeof(_E9BA), ImmunityPreventedContamination);
			EffectIcons.Add(typeof(_E9BB), HalloweenBuff);
			EffectIcons.Add(typeof(_E9C2), MildMusclePain);
			EffectIcons.Add(typeof(_E9C3), SevereMusclePain);
		}
	}

	public ItemTypeToSpriteDictionary ItemTypeSprites = new ItemTypeToSpriteDictionary();

	public QuestTypeToSpriteDictionary QuestTypeSprites = new QuestTypeToSpriteDictionary();

	public QuestIconTypeToSpriteDictionary QuestIconTypeSprites = new QuestIconTypeToSpriteDictionary();

	public BuffIdToSpriteDictionary BuffIdSprites = new BuffIdToSpriteDictionary();

	public SkillIdToSpriteDictionary SkillIdSprites = new SkillIdToSpriteDictionary();

	public StatGroupIdToSpriteDictionary StatGroupSprites = new StatGroupIdToSpriteDictionary();

	public DamageResultToSpriteDictionary DamageResultSprites = new DamageResultToSpriteDictionary();

	public NotificationIconTypeToSpriteDictionary NotificationSprites = new NotificationIconTypeToSpriteDictionary();

	public GestureToSpriteDictionary GestureSprites = new GestureToSpriteDictionary();

	public TraderDialogLineSpriteDictionary DialogLineSprites = new TraderDialogLineSpriteDictionary();

	[Header("Attributes")]
	public ItemAttributeIdToSpriteDictionary ItemAttributeSprites = new ItemAttributeIdToSpriteDictionary();

	public HealthFactorTypeToSpriteDictionary HealEffectSprites = new HealthFactorTypeToSpriteDictionary();

	public DamageEffectTypeToSpriteDictionary DamageEffectSprites = new DamageEffectTypeToSpriteDictionary();

	public StimulatorBuffTypeToSpriteDictionary StimulatorBuffSprites = new StimulatorBuffTypeToSpriteDictionary();

	[Header("Currencies")]
	public CurrencyTypeToSpriteDictionary CurrencyTypeSmallSprites = new CurrencyTypeToSpriteDictionary();

	public CurrencyTypeToSpriteDictionary CurrencyTypeBigSprites = new CurrencyTypeToSpriteDictionary();

	public Sprite BarterSign;

	[Header("Misc")]
	public Sprite YesCheck;

	public Sprite NoCheck;

	public Sprite TogglableOn;

	public Sprite TogglableOff;

	[Header("Health Effects")]
	public EffectSprites EffectIcons;

	[CanBeNull]
	public Sprite GetItemTypeIcon(EItemType type)
	{
		if (!ItemTypeSprites.TryGetValue(type, out var value))
		{
			return null;
		}
		return value;
	}

	[CanBeNull]
	public Sprite GetSmallCurrencySign(string templateId, bool nullable = false)
	{
		if (!(templateId == _ED3E._E000(229113)))
		{
			if (!(templateId == _ED3E._E000(229121)))
			{
				if (templateId == _ED3E._E000(229154))
				{
					return GetSmallCurrencySign(ECurrencyType.EUR);
				}
				if (!nullable)
				{
					return BarterSign;
				}
				return null;
			}
			return GetSmallCurrencySign(ECurrencyType.USD);
		}
		return GetSmallCurrencySign(ECurrencyType.RUB);
	}

	public Sprite GetBigCurrencySign(string templateId)
	{
		if (!(templateId == _ED3E._E000(229113)))
		{
			if (!(templateId == _ED3E._E000(229121)))
			{
				if (templateId == _ED3E._E000(229154))
				{
					return CurrencyTypeBigSprites[ECurrencyType.EUR];
				}
				return BarterSign;
			}
			return CurrencyTypeBigSprites[ECurrencyType.USD];
		}
		return CurrencyTypeBigSprites[ECurrencyType.RUB];
	}

	public Sprite GetSmallCurrencySign(ECurrencyType currency)
	{
		return CurrencyTypeSmallSprites[currency];
	}

	[CanBeNull]
	public Sprite GetQuestIcon(Condition condition)
	{
		if (condition != null)
		{
			if (condition is ConditionCounterCreator conditionCounterCreator)
			{
				ConditionCounterCreator conditionCounterCreator2 = conditionCounterCreator;
				return QuestTypeSprites[conditionCounterCreator2.type];
			}
			if (condition is ConditionHandoverItem || condition is ConditionFindItem)
			{
				return QuestTypeSprites[_E937.EQuestType.PickUp];
			}
			if (condition is ConditionTraderStanding)
			{
				return QuestTypeSprites[_E937.EQuestType.Standing];
			}
			if (condition is ConditionVisitPlace)
			{
				return QuestTypeSprites[_E937.EQuestType.Exploration];
			}
			if (condition is ConditionStatisticsCounter || condition is ConditionQuest || condition is ConditionLevel)
			{
				return QuestTypeSprites[_E937.EQuestType.Levelling];
			}
			if (condition is ConditionSkill)
			{
				return QuestTypeSprites[_E937.EQuestType.Skill];
			}
			if (condition is ConditionTraderLoyalty)
			{
				return QuestTypeSprites[_E937.EQuestType.Loyalty];
			}
			if (condition is ConditionExperience)
			{
				return QuestTypeSprites[_E937.EQuestType.Experience];
			}
			if (condition is ConditionSellItemToTrader)
			{
				return QuestTypeSprites[_E937.EQuestType.Merchant];
			}
			if (condition is ConditionLeaveItemAtLocation)
			{
				return QuestTypeSprites[_E937.EQuestType.Levelling];
			}
			if (condition is ConditionWeaponAssembly)
			{
				return QuestTypeSprites[_E937.EQuestType.WeaponAssembly];
			}
			if (condition is _E351)
			{
				return QuestTypeSprites[_E937.EQuestType.Levelling];
			}
		}
		Debug.LogFormat(_ED3E._E000(253723), condition.GetType());
		return null;
	}

	public Sprite GetAttributeIcon(Enum id)
	{
		Sprite value = null;
		if (id == null)
		{
			goto IL_00be;
		}
		if (!(id is EItemAttributeId eItemAttributeId))
		{
			if (!(id is EHealthFactorType eHealthFactorType))
			{
				if (!(id is EDamageEffectType eDamageEffectType))
				{
					if (!(id is EStimulatorBuffType eStimulatorBuffType))
					{
						goto IL_00be;
					}
					EStimulatorBuffType key = eStimulatorBuffType;
					StimulatorBuffSprites.TryGetValue(key, out value);
				}
				else
				{
					EDamageEffectType key2 = eDamageEffectType;
					DamageEffectSprites.TryGetValue(key2, out value);
				}
			}
			else
			{
				EHealthFactorType key3 = eHealthFactorType;
				HealEffectSprites.TryGetValue(key3, out value);
			}
		}
		else
		{
			EItemAttributeId key4 = eItemAttributeId;
			ItemAttributeSprites.TryGetValue(key4, out value);
		}
		goto IL_00d6;
		IL_00d6:
		if (value == null)
		{
			Debug.LogError(string.Format(_ED3E._E000(253739), id));
		}
		return value;
		IL_00be:
		Debug.LogError(_ED3E._E000(253752));
		goto IL_00d6;
	}

	public Sprite GetStatGroupIcon(EStatGroupId groupId)
	{
		if (StatGroupSprites.TryGetValue(groupId, out var value) && value != null)
		{
			return value;
		}
		Debug.LogError(_ED3E._E000(253770) + groupId);
		return null;
	}
}
