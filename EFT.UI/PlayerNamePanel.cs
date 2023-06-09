using UnityEngine;

namespace EFT.UI;

public class PlayerNamePanel : MonoBehaviour
{
	public const string MAIN_CHARACTER_DESCRIPTION = "YOUR MAIN CHARACTER";

	[SerializeField]
	private CustomTextMeshProUGUI _name;

	[SerializeField]
	private CustomTextMeshProUGUI _description;

	[SerializeField]
	private ChatSpecialIcon _icon;

	public void Set(Profile profile)
	{
		Set(profile.Info.Side != EPlayerSide.Savage, profile.Info.MemberCategory, profile.GetCorrectedNickname());
	}

	public void Set(bool showDetails, EMemberCategory category, string nickname, int textSize = 23)
	{
		_description.text = ((!showDetails) ? string.Empty : _ED3E._E000(248118).Localized());
		_name.text = nickname;
		_name.fontSize = textSize;
		_icon.Show(showDetails ? category : EMemberCategory.Default);
	}
}
