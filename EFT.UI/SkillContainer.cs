using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT.UI;

public class SkillContainer<TSkillView> : UIElement where TSkillView : IUIView
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public DropDownBox sortMethod;

		public _EC6D<_E751, TSkillView> skillPanels;

		public _E74F skillManager;

		public DropDownBox filterMethod;

		public Func<_E751, bool> _003C_003E9__4;

		internal void _E000()
		{
			switch (sortMethod.CurrentIndex)
			{
			case 0:
				skillPanels.UpdateOrder(skillManager.DisplayList);
				break;
			case 1:
				skillPanels.OrderByDescending((_E751 skill) => skill.Level);
				break;
			case 2:
				skillPanels.OrderByDescending((_E751 skill) => skill.LastCall);
				break;
			}
		}

		internal void _E001()
		{
			if (filterMethod.CurrentIndex == 0)
			{
				skillPanels.FilterBy();
				return;
			}
			skillPanels.FilterBy((_E751 skill) => skill.Class == (ESkillClass)(filterMethod.CurrentIndex - 1));
		}

		internal bool _E002(_E751 skill)
		{
			return skill.Class == (ESkillClass)(filterMethod.CurrentIndex - 1);
		}
	}

	[SerializeField]
	private Transform _skillsContainer;

	[SerializeField]
	private TSkillView _skillViewTemplate;

	protected void Show(_E74F skillManager, DropDownBox sortMethod, DropDownBox filterMethod, Action<_E751, TSkillView> showAction)
	{
		ShowGameObject();
		_EC6D<_E751, TSkillView> skillPanels = UI.AddViewListAsync(skillManager.DisplayList, _skillViewTemplate, _skillsContainer, showAction);
		UI.BindEvent(sortMethod.OnValueChanged, delegate
		{
			switch (sortMethod.CurrentIndex)
			{
			case 0:
				skillPanels.UpdateOrder(skillManager.DisplayList);
				break;
			case 1:
				skillPanels.OrderByDescending((_E751 skill) => skill.Level);
				break;
			case 2:
				skillPanels.OrderByDescending((_E751 skill) => skill.LastCall);
				break;
			}
		});
		UI.BindEvent(filterMethod.OnValueChanged, delegate
		{
			if (filterMethod.CurrentIndex == 0)
			{
				skillPanels.FilterBy();
			}
			else
			{
				skillPanels.FilterBy((_E751 skill) => skill.Class == (ESkillClass)(filterMethod.CurrentIndex - 1));
			}
		});
	}
}
