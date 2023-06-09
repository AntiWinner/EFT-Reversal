using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

namespace EFT.UI;

public sealed class ClothingRequirements : UIElement
{
	public sealed class _E000
	{
		public _E937 QuestTemplate;

		public float Width;
	}

	private sealed class _E001
	{
		public int? loyaltyLevel;

		public int? profileLevel;

		public float? standing;

		public IEnumerable<_EBE4> skillRequirements;

		public IEnumerable<_E000> questRequirements;
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public _EBE3 requirements;

		public int requirementsInCurrentRow;

		public _E001 requirementsInfo;

		public List<_E001> panelsCollection;

		public int currentPanelIndex;

		public Profile profile;

		public Profile._E001 trader;

		public _E934 quests;

		public Action<bool> onAvailableChanged;

		internal void _E000(float rowWidth, ref _E003 P_1)
		{
			P_1.rowAvailableWidth = rowWidth;
			requirementsInCurrentRow = 0;
			requirementsInfo.questRequirements = P_1.panelQuestRequirements;
			panelsCollection.Insert(currentPanelIndex, requirementsInfo);
			currentPanelIndex++;
			requirementsInfo = new _E001();
			P_1.panelQuestRequirements = new List<_E000>(4);
		}

		internal void _E001(_E001 item, SkillsAndLevelsRequirementsPanel view)
		{
			view.Show(profile, trader, quests, item.loyaltyLevel, item.profileLevel, item.standing, item.skillRequirements, item.questRequirements, onAvailableChanged);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _E003
	{
		public float rowAvailableWidth;

		public List<_E000> panelQuestRequirements;
	}

	[CompilerGenerated]
	private sealed class _E004
	{
		public int i;

		public _E002 CS_0024_003C_003E8__locals1;

		internal bool _E000(_E937 template)
		{
			return template.Id == CS_0024_003C_003E8__locals1.requirements.questRequirements[i];
		}
	}

	private const int _E0EC = 4;

	private const int _E0ED = 3;

	private const int _E0EE = 40;

	[SerializeField]
	private SkillsAndLevelsRequirementsPanel _skillsAndLevelsRow;

	[SerializeField]
	private Transform _skillsAndLevelsPanel;

	[SerializeField]
	private ItemRequirementsPanel _itemsPanel;

	public void Show(Profile profile, Profile._E001 trader, _E934 quests, _E9EF stashGrid, _EBE3 requirements, Action<bool> onAvailableChanged)
	{
		ShowGameObject();
		_E000(profile, trader, quests, requirements, onAvailableChanged);
		_itemsPanel.Show(stashGrid, requirements.itemRequirements, onAvailableChanged);
	}

	private void _E000(Profile profile, Profile._E001 trader, _E934 quests, _EBE3 requirements, Action<bool> onAvailableChanged)
	{
		int num = 0;
		int requirementsInCurrentRow = 0;
		_E001 requirementsInfo = new _E001();
		if (requirements.loyaltyLevel > 0)
		{
			requirementsInfo.loyaltyLevel = requirements.loyaltyLevel;
			requirementsInCurrentRow++;
			if (num == 0)
			{
				num = 1;
			}
		}
		if (requirements.profileLevel > 0)
		{
			requirementsInfo.profileLevel = requirements.profileLevel;
			requirementsInCurrentRow++;
			if (num == 0)
			{
				num = 1;
			}
		}
		if (requirements.standing > 0f)
		{
			requirementsInfo.standing = requirements.standing;
			requirementsInCurrentRow++;
			if (num == 0)
			{
				num = 1;
			}
		}
		_EBE4[] skillRequirements = requirements.skillRequirements;
		int num2 = ((skillRequirements != null) ? skillRequirements.Length : 0);
		if (num2 > 0)
		{
			num = ((num2 == 1 || num == 0) ? 1 : (num + Mathf.CeilToInt((float)num2 / 3f)));
		}
		string[] questRequirements = requirements.questRequirements;
		int num3 = ((questRequirements != null) ? questRequirements.Length : 0);
		num += num3;
		List<_E001> panelsCollection = new List<_E001>();
		int currentPanelIndex = 0;
		if (num == 1 && num2 == 0 && num3 == 0)
		{
			panelsCollection.Insert(0, requirementsInfo);
		}
		else
		{
			if (num2 > 0)
			{
				if (requirementsInCurrentRow > 0 && num2 > 1)
				{
					panelsCollection.Insert(currentPanelIndex, requirementsInfo);
					currentPanelIndex++;
					requirementsInCurrentRow = 0;
					requirementsInfo = new _E001();
				}
				List<_EBE4> list = new List<_EBE4>(4);
				if (requirements.skillRequirements != null)
				{
					for (int j = 0; j < requirements.skillRequirements.Length; j++)
					{
						list.Add(requirements.skillRequirements[j]);
						requirementsInCurrentRow++;
						if (requirementsInCurrentRow >= 3 || j + 1 == requirements.skillRequirements.Length)
						{
							requirementsInCurrentRow = 0;
							requirementsInfo.skillRequirements = list;
							panelsCollection.Insert(currentPanelIndex, requirementsInfo);
							currentPanelIndex++;
							requirementsInfo = new _E001();
							list = new List<_EBE4>(4);
						}
					}
				}
			}
			if (num3 > 0)
			{
				if (requirementsInCurrentRow > 0)
				{
					panelsCollection.Insert(currentPanelIndex, requirementsInfo);
					currentPanelIndex++;
					requirementsInCurrentRow = 0;
					requirementsInfo = new _E001();
				}
				_E003 obj = default(_E003);
				obj.panelQuestRequirements = new List<_E000>(4);
				float num4 = (obj.rowAvailableWidth = ((RectTransform)_skillsAndLevelsRow.transform).sizeDelta.x);
				bool flag = false;
				if (requirements.questRequirements != null)
				{
					IReadOnlyCollection<_E937> allProfileTemplates = _E93B.Instance.GetAllProfileTemplates(profile.Id);
					int i;
					_E002 CS_0024_003C_003E8__locals0;
					for (i = 0; i < requirements.questRequirements.Length; i++)
					{
						_E937 obj2 = allProfileTemplates.FirstOrDefault((_E937 template) => template.Id == requirements.questRequirements[i]) ?? new _E937
						{
							Id = requirements.questRequirements[i]
						};
						float num5 = Mathf.Ceil(_skillsAndLevelsRow.GetTextInfo(obj2.Name).lineInfo[0].length) + 1f;
						obj.rowAvailableWidth -= num5 + 40f;
						if (obj.rowAvailableWidth < 0f)
						{
							if (requirementsInCurrentRow > 0)
							{
								CS_0024_003C_003E8__locals0._E000(num4 - num5 - 40f, ref obj);
							}
							if (obj.rowAvailableWidth < 0f)
							{
								num5 = num4 - 40f;
								flag = true;
							}
						}
						obj.panelQuestRequirements.Add(new _E000
						{
							QuestTemplate = obj2,
							Width = num5
						});
						int num6 = requirementsInCurrentRow;
						requirementsInCurrentRow = num6 + 1;
						if (flag || requirements.questRequirements.Length - 1 == i)
						{
							CS_0024_003C_003E8__locals0._E000(num4, ref obj);
						}
					}
				}
			}
		}
		if (panelsCollection.Count > 0)
		{
			UI.AddViewList(panelsCollection, _skillsAndLevelsRow, _skillsAndLevelsPanel, delegate(_E001 item, SkillsAndLevelsRequirementsPanel view)
			{
				view.Show(profile, trader, quests, item.loyaltyLevel, item.profileLevel, item.standing, item.skillRequirements, item.questRequirements, onAvailableChanged);
			});
		}
	}

	public override void Close()
	{
		_skillsAndLevelsRow.Close();
		_itemsPanel.Close();
		base.Close();
	}
}
