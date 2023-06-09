using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.Quests;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace EFT.UI;

public class SkillsAndLevelsRequirementsPanel : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public Profile profile;

		public SkillsAndLevelsRequirementsPanel _003C_003E4__this;

		public _E934 quests;

		internal void _E000(_EBE4 skillRequirement, RequiredSkillOrLevel view)
		{
			_E001 CS_0024_003C_003E8__locals0 = new _E001
			{
				skillRequirement = skillRequirement
			};
			_E751 obj = profile.Skills.Skills.FirstOrDefault((_E751 skill) => skill.Id.ToString() == CS_0024_003C_003E8__locals0.skillRequirement.skillName);
			if (obj != null)
			{
				_003C_003E4__this._E000(view, string.Format(_ED3E._E000(53834), CS_0024_003C_003E8__locals0.skillRequirement.skillName, CS_0024_003C_003E8__locals0.skillRequirement.skillLevel), CS_0024_003C_003E8__locals0.skillRequirement.skillLevel <= obj.Level, _003C_003E4__this._skillIcon);
			}
			else
			{
				view.HideGameObject();
			}
		}

		internal void _E001(ClothingRequirements._E000 questTemplate, RequiredSkillOrLevel view)
		{
			_E933 quest = quests.GetQuest(questTemplate.QuestTemplate.Id);
			_003C_003E4__this._E000(view, questTemplate.QuestTemplate.Name, quest != null && quest.QuestStatus == EQuestStatus.Success, _003C_003E4__this._questIcon, questTemplate.Width);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public _EBE4 skillRequirement;

		internal bool _E000(_E751 skill)
		{
			return skill.Id.ToString() == skillRequirement.skillName;
		}
	}

	[SerializeField]
	private RequiredSkillOrLevel _loyalty;

	[SerializeField]
	private RequiredSkillOrLevel _level;

	[SerializeField]
	private RequiredSkillOrLevel _standing;

	[SerializeField]
	private RequiredSkillOrLevel _skillTemplate;

	[SerializeField]
	private Sprite _skillIcon;

	[SerializeField]
	private Sprite _questIcon;

	private bool _E00B;

	private Action<bool> _E0EF;

	public TMP_TextInfo GetTextInfo(string text)
	{
		return _skillTemplate.Text.GetTextInfo(text);
	}

	public void Show(Profile profile, Profile._E001 trader, _E934 quests, int? loyaltyLevel, int? profileLevel, float? standing, [CanBeNull] IEnumerable<_EBE4> skillRequirements, [CanBeNull] IEnumerable<ClothingRequirements._E000> questRequirements, Action<bool> onAvailableChanged)
	{
		ShowGameObject();
		_E00B = true;
		_E0EF = onAvailableChanged;
		if (loyaltyLevel.HasValue)
		{
			_E000(_loyalty, loyaltyLevel.Value.ToString(), loyaltyLevel.Value <= trader.LoyaltyLevel);
		}
		if (profileLevel.HasValue)
		{
			_E000(_level, profileLevel.Value.ToString(), profileLevel.Value <= profile.Info.Level);
		}
		if (standing.HasValue)
		{
			_E000(_standing, standing.Value.ToString(_ED3E._E000(253692)), (double)standing.Value <= trader.Standing);
		}
		if (skillRequirements != null)
		{
			UI.AddViewList(skillRequirements, _skillTemplate, base.transform, delegate(_EBE4 skillRequirement, RequiredSkillOrLevel view)
			{
				_E751 obj = profile.Skills.Skills.FirstOrDefault((_E751 skill) => skill.Id.ToString() == skillRequirement.skillName);
				if (obj != null)
				{
					_E000(view, string.Format(_ED3E._E000(53834), skillRequirement.skillName, skillRequirement.skillLevel), skillRequirement.skillLevel <= obj.Level, _skillIcon);
				}
				else
				{
					view.HideGameObject();
				}
			});
		}
		if (questRequirements != null)
		{
			UI.AddViewList(questRequirements, _skillTemplate, base.transform, delegate(ClothingRequirements._E000 questTemplate, RequiredSkillOrLevel view)
			{
				_E933 quest = quests.GetQuest(questTemplate.QuestTemplate.Id);
				_E000(view, questTemplate.QuestTemplate.Name, quest != null && quest.QuestStatus == EQuestStatus.Success, _questIcon, questTemplate.Width);
			});
		}
	}

	private void _E000(RequiredSkillOrLevel requirement, string message, bool available, Sprite icon = null, float width = -1f)
	{
		requirement.Show(message, available, icon, width);
		if (!available && _E00B)
		{
			_E00B = false;
			_E0EF(obj: false);
		}
	}

	public override void Close()
	{
		_loyalty.Close();
		_level.Close();
		_standing.Close();
		_E0EF = null;
		base.Close();
	}
}
