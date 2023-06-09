using UnityEngine;

namespace EFT.UI;

public class PlayerExperiencePanel : UIElement
{
	[SerializeField]
	private PlayerLevelPanel _currentLevel;

	[SerializeField]
	private PlayerLevelPanel _nextLevel;

	[SerializeField]
	private CustomTextMeshProUGUI _currentExperience;

	[SerializeField]
	private CustomTextMeshProUGUI _remainingExperience;

	[SerializeField]
	private CustomTextMeshProUGUI _newExperience;

	[SerializeField]
	private TwoValueBar _experienceBar;

	public void Set(int baseTotal, int targetExperience)
	{
		int obj = targetExperience - baseTotal;
		int level = _E72F.GetLevel(targetExperience);
		int experience = _E72F.GetExperience(level);
		int experience2 = _E72F.GetExperience(level + 1);
		int num = targetExperience - experience;
		_currentLevel.Set(level, ESideType.Pmc);
		_nextLevel.Set(level + 1, ESideType.Pmc);
		_currentExperience.text = string.Format(_ED3E._E000(248062) + targetExperience.ToThousandsString(), _ED3E._E000(248034).Localized());
		_remainingExperience.text = string.Format(_ED3E._E000(248062) + (experience2 - targetExperience).ToThousandsString(), _ED3E._E000(248091).Localized());
		_newExperience.text = _ED3E._E000(29692) + obj.ToThousandsString();
		int num2 = baseTotal - experience;
		_experienceBar.SetBase(((num2 >= 0) ? ((float)num2) : 0f) / (float)(experience2 - experience));
		_experienceBar.SetNew((float)num / (float)(experience2 - experience));
	}
}
