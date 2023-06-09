using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace EFT.UI;

public class MasteringContainer<TSkillView> : UIElement where TSkillView : IUIView
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public DropDownBox sortMethod;

		public _EC79<KeyValuePair<string, _E750>, TSkillView> skillPanels;

		public KeyValuePair<string, _E750>[] skills;

		public DropDownBox filterMethod;

		public TMP_InputField inputField;

		public Func<KeyValuePair<string, _E750>, bool> _003C_003E9__5;

		public Func<KeyValuePair<string, _E750>, bool> _003C_003E9__6;

		internal void _E000()
		{
			switch (sortMethod.CurrentIndex)
			{
			case 0:
				skillPanels.UpdateOrder(skills);
				break;
			case 1:
				skillPanels.OrderByDescending((KeyValuePair<string, _E750> skill) => skill.Value.LevelProgress);
				break;
			case 2:
				skillPanels.OrderByDescending((KeyValuePair<string, _E750> skill) => skill.Value.LastCall);
				break;
			}
		}

		internal void _E001()
		{
			if (filterMethod.CurrentIndex == 0)
			{
				skillPanels.FilterBy((KeyValuePair<string, _E750> skill) => MasteringContainer<TSkillView>._E000(skill, filterMethod, inputField.text));
			}
			else
			{
				skillPanels.FilterBy((KeyValuePair<string, _E750> skill) => MasteringContainer<TSkillView>._E000(skill, filterMethod, inputField.text));
			}
		}

		internal bool _E002(KeyValuePair<string, _E750> skill)
		{
			return MasteringContainer<TSkillView>._E000(skill, filterMethod, inputField.text);
		}

		internal bool _E003(KeyValuePair<string, _E750> skill)
		{
			return MasteringContainer<TSkillView>._E000(skill, filterMethod, inputField.text);
		}

		internal void _E004(string arg)
		{
			_E001 CS_0024_003C_003E8__locals0 = new _E001
			{
				CS_0024_003C_003E8__locals1 = this,
				arg = arg
			};
			skillPanels.FilterBy((KeyValuePair<string, _E750> skill) => MasteringContainer<TSkillView>._E000(skill, CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1.filterMethod, CS_0024_003C_003E8__locals0.arg));
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public string arg;

		public _E000 CS_0024_003C_003E8__locals1;

		internal bool _E000(KeyValuePair<string, _E750> skill)
		{
			return MasteringContainer<TSkillView>._E000(skill, CS_0024_003C_003E8__locals1.filterMethod, arg);
		}
	}

	[SerializeField]
	private Transform _skillsContainer;

	[SerializeField]
	private TSkillView _skillViewTemplate;

	protected void Show(KeyValuePair<string, _E750>[] skills, DropDownBox sortMethod, DropDownBox filterMethod, TMP_InputField inputField, _EAE6 inventoryController, Action<KeyValuePair<string, _E750>, TSkillView> showAction)
	{
		ShowGameObject();
		_EC79<KeyValuePair<string, _E750>, TSkillView> skillPanels = UI.AddViewList(skills, _skillViewTemplate, _skillsContainer, showAction);
		UI.BindEvent(sortMethod.OnValueChanged, delegate
		{
			switch (sortMethod.CurrentIndex)
			{
			case 0:
				skillPanels.UpdateOrder(skills);
				break;
			case 1:
				skillPanels.OrderByDescending((KeyValuePair<string, _E750> skill) => skill.Value.LevelProgress);
				break;
			case 2:
				skillPanels.OrderByDescending((KeyValuePair<string, _E750> skill) => skill.Value.LastCall);
				break;
			}
		});
		UI.BindEvent(filterMethod.OnValueChanged, delegate
		{
			if (filterMethod.CurrentIndex == 0)
			{
				skillPanels.FilterBy((KeyValuePair<string, _E750> skill) => _E000(skill, filterMethod, inputField.text));
			}
			else
			{
				skillPanels.FilterBy((KeyValuePair<string, _E750> skill) => _E000(skill, filterMethod, inputField.text));
			}
		});
		inputField.onValueChanged.AddListener(delegate(string arg)
		{
			skillPanels.FilterBy((KeyValuePair<string, _E750> skill) => _E000(skill, filterMethod, arg));
		});
	}

	private static bool _E000(KeyValuePair<string, _E750> skill, DropDownBox filterMethod, string weaponName)
	{
		if (_E001(skill, filterMethod))
		{
			return _E002(skill, weaponName);
		}
		return false;
	}

	private static bool _E001(KeyValuePair<string, _E750> skill, DropDownBox filterMethod)
	{
		if (filterMethod.CurrentIndex != 0)
		{
			return MasteringScreen.GetItemTemplateText(skill.Key) == MasteringScreen.ItemTemplates.ElementAt(filterMethod.CurrentIndex - 1);
		}
		return true;
	}

	private static bool _E002(KeyValuePair<string, _E750> skill, string weaponName)
	{
		return (skill.Key + _ED3E._E000(70087)).Localized().IndexOf(weaponName, 0, StringComparison.CurrentCultureIgnoreCase) != -1;
	}
}
