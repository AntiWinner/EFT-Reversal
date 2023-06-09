using System.Globalization;
using EFT.Quests;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class QuestRequirementView : MonoBehaviour
{
	[SerializeField]
	private Image _icon;

	[SerializeField]
	private CustomTextMeshProUGUI _text;

	[SerializeField]
	private Image _checkMark;

	[SerializeField]
	private SpriteMap _icons;

	[SerializeField]
	private ColorMap _colors;

	public void Init(_E933 quest, Condition condition)
	{
		bool flag = quest.ConditionHandlers[condition].Test();
		_text.color = (flag ? _colors[_ED3E._E000(261907)] : _colors[_ED3E._E000(261914)]);
		_checkMark.gameObject.SetActive(flag);
		if (condition is ConditionSkill conditionSkill)
		{
			_text.text = conditionSkill.target.Localized() + _ED3E._E000(18502) + condition.value.ToString(CultureInfo.InvariantCulture);
		}
		else if (condition is _E351 obj)
		{
			_text.text = condition.value.ToString(CultureInfo.InvariantCulture) + _ED3E._E000(18502) + obj.area.LocalizeAreaName();
		}
		else if (condition is ConditionQuest conditionQuest)
		{
			CustomTextMeshProUGUI text = _text;
			string text2;
			if (conditionQuest.availableAfter <= 0)
			{
				text2 = conditionQuest.name.ParseLocalization();
			}
			else
			{
				CustomTextMeshProUGUI text3 = _text;
				string text5 = (text3.text = text3.text + _ED3E._E000(2540) + _ED3E._E000(261892).Localized());
				text2 = text5;
			}
			text.text = text2;
		}
		else
		{
			_text.text = condition.value.ToString(CultureInfo.InvariantCulture);
		}
		_icon.sprite = _icons[condition.GetType().Name];
		if (_icon.sprite == null)
		{
			Debug.LogWarning(_ED3E._E000(261930) + condition.GetType());
		}
	}
}
