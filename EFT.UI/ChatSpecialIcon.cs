using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class ChatSpecialIcon : UIElement
{
	[SerializeField]
	private CustomTextMeshProUGUI _specialLabel;

	[SerializeField]
	private Image _icon;

	private ChatSpecialIconSettings _E0B5;

	public void Show(EMemberCategory category, string playerName = null)
	{
		if (_specialLabel != null && !string.IsNullOrEmpty(playerName))
		{
			_specialLabel.text = playerName;
		}
		if (_icon == null)
		{
			HideGameObject();
			return;
		}
		ShowGameObject();
		if (_E0B5 == null)
		{
			_E0B5 = _E3A2.Load<ChatSpecialIconSettings>(_ED3E._E000(250600));
		}
		ChatSpecialIconSettings.IconsData dataByMemberCategory = _E0B5.GetDataByMemberCategory(category);
		if (dataByMemberCategory != null)
		{
			if (_specialLabel != null)
			{
				_specialLabel.color = dataByMemberCategory.IconColor;
			}
			if (_icon != null)
			{
				_icon.sprite = dataByMemberCategory.IconSprite;
			}
		}
	}
}
