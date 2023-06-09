using UnityEngine;

namespace EFT.UI;

public sealed class SkillLevelPanel : UIElement
{
	[SerializeField]
	private GameObject _eliteIcon;

	[SerializeField]
	private CustomTextMeshProUGUI _level;

	public void SetLevel(_E751 skill)
	{
		if (!(_eliteIcon == null))
		{
			bool isEliteLevel = skill.IsEliteLevel;
			_eliteIcon.SetActive(isEliteLevel);
			_level.gameObject.SetActive(!isEliteLevel);
			_level.text = skill.SummaryLevel.ToString();
		}
	}
}
