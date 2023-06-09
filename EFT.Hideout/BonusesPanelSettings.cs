using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EFT.Hideout;

public sealed class BonusesPanelSettings : SerializedScriptableObject
{
	public Dictionary<EBonusType, Sprite> BonusTypeMap;

	public Dictionary<ESkillClass, Sprite> SkillTypeMap;

	public Dictionary<ESkillId, Sprite> SkillIdMap;

	public Sprite this[Enum status]
	{
		get
		{
			if (status != null)
			{
				if (status is EBonusType eBonusType)
				{
					EBonusType key = eBonusType;
					return BonusTypeMap[key];
				}
				if (status is ESkillClass)
				{
					return BonusTypeMap[EBonusType.SkillGroupLevelingBoost];
				}
				if (status is ESkillId)
				{
					return BonusTypeMap[EBonusType.SkillLevelingBoost];
				}
			}
			return null;
		}
	}
}
