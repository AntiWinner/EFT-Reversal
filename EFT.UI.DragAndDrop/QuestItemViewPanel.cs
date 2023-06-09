using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using EFT.Quests;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.DragAndDrop;

public sealed class QuestItemViewPanel : UIElement
{
	private enum EPanelViewType
	{
		None,
		Quest,
		QuestFoundInRaid,
		FoundInRaid
	}

	private const string _E2BB = "Item found in raid";

	private const string _E2BC = "QUEST ITEM";

	private const string _E2BD = "Item is related to an active {0} quest";

	private const string _E2BE = "Item fits the active {0} quest requirements";

	private const string _E2BF = "Item that has been found in raid for the {0} quest";

	[SerializeField]
	private Image _questIconImage;

	[SerializeField]
	private Sprite _questItemSprite;

	[SerializeField]
	private Sprite _foundInRaidSprite;

	[SerializeField]
	private TextMeshProUGUI _questItemLabel;

	[CanBeNull]
	private SimpleTooltip _E02A;

	private string _E2C0;

	private void Awake()
	{
		HoverTrigger orAddComponent = base.gameObject.GetOrAddComponent<HoverTrigger>();
		orAddComponent.OnHoverStart += delegate
		{
			if (!(_E02A == null) && !string.IsNullOrEmpty(_E2C0))
			{
				_E02A.Show(_E2C0);
			}
		};
		orAddComponent.OnHoverEnd += delegate
		{
			if (!(_E02A == null) && !string.IsNullOrEmpty(_E2C0))
			{
				_E02A.Close();
			}
		};
	}

	public void Show(Profile profile, Item item, [CanBeNull] SimpleTooltip tooltip)
	{
		HideGameObject();
		bool questItem = item.QuestItem;
		if (!questItem && !item.MarkedAsSpawnedInSession && !(item is Weapon))
		{
			return;
		}
		if (_questItemLabel != null)
		{
			_questItemLabel.gameObject.SetActive(questItem);
			if (questItem)
			{
				_questItemLabel.text = _ED3E._E000(236116).Localized();
			}
		}
		_E02A = tooltip;
		switch (_E000(profile, item))
		{
		case EPanelViewType.Quest:
		case EPanelViewType.QuestFoundInRaid:
			ShowGameObject();
			_questIconImage.sprite = _questItemSprite;
			break;
		case EPanelViewType.FoundInRaid:
			ShowGameObject();
			_questIconImage.sprite = _foundInRaidSprite;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		case EPanelViewType.None:
			break;
		}
	}

	private EPanelViewType _E000(Profile profile, Item item)
	{
		if (_E001(profile.QuestsData, item, out var questWithItem, out var conditionItem))
		{
			string arg = _ED3E._E000(236111) + questWithItem.Name + _ED3E._E000(59467);
			if (item.QuestItem)
			{
				_E2C0 = string.Format(_ED3E._E000(236159).Localized(), arg);
				return EPanelViewType.Quest;
			}
			if (item is Weapon weapon && conditionItem is ConditionWeaponAssembly condition && _EAE7.IsWeaponFitsCondition(weapon, condition))
			{
				_E2C0 = string.Format(_ED3E._E000(236182).Localized(), arg);
				return EPanelViewType.Quest;
			}
			if (item.MarkedAsSpawnedInSession)
			{
				_E2C0 = string.Format(_ED3E._E000(236202).Localized(), arg);
				return EPanelViewType.QuestFoundInRaid;
			}
		}
		if (item.MarkedAsSpawnedInSession)
		{
			_E2C0 = _ED3E._E000(236277).Localized();
			return EPanelViewType.FoundInRaid;
		}
		return EPanelViewType.None;
	}

	private static bool _E001(IEnumerable<_E932> quests, Item item, out _E937 questWithItem, out ConditionItem conditionItem)
	{
		questWithItem = null;
		conditionItem = null;
		foreach (_E932 quest in quests)
		{
			if (quest.Status != EQuestStatus.Started || quest.Template == null)
			{
				continue;
			}
			foreach (KeyValuePair<EQuestStatus, _E91B> condition in quest.Template.Conditions)
			{
				_E39D.Deconstruct(condition, out var _, out var value);
				foreach (Condition item2 in value)
				{
					if (!quest.CompletedConditions.Contains(item2.id) && item2 is ConditionItem conditionItem2 && conditionItem2.target.Contains(item.TemplateId))
					{
						questWithItem = quest.Template;
						conditionItem = conditionItem2;
						return true;
					}
				}
			}
		}
		return false;
	}

	[CompilerGenerated]
	private void _E002(PointerEventData arg)
	{
		if (!(_E02A == null) && !string.IsNullOrEmpty(_E2C0))
		{
			_E02A.Show(_E2C0);
		}
	}

	[CompilerGenerated]
	private void _E003(PointerEventData arg)
	{
		if (!(_E02A == null) && !string.IsNullOrEmpty(_E2C0))
		{
			_E02A.Close();
		}
	}
}
