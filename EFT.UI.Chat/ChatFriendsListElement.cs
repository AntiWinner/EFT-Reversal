using UnityEngine;

namespace EFT.UI.Chat;

public sealed class ChatFriendsListElement : UIElement
{
	[SerializeField]
	private CustomTextMeshProUGUI _nicknameLabel;

	public void Show(string nickname)
	{
		_nicknameLabel.text = nickname;
	}
}
