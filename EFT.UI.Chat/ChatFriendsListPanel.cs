using System.Runtime.CompilerServices;
using ChatShared;
using UnityEngine;

namespace EFT.UI.Chat;

public sealed class ChatFriendsListPanel : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _E79D social;

		public ChatFriendsListPanel _003C_003E4__this;

		internal void _E000(UpdatableChatMember member, ChatMember view)
		{
			_E001 CS_0024_003C_003E8__locals0 = new _E001
			{
				CS_0024_003C_003E8__locals1 = this,
				member = member
			};
			view.Show(CS_0024_003C_003E8__locals0.member, social.PlayerMember, active: true, delegate(Vector2 pos)
			{
				CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1._003C_003E4__this._contextMenu.Show(pos, new _ECA8(CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1.social, null, CS_0024_003C_003E8__locals0.member, CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1.social.GetInvitationByProfile(CS_0024_003C_003E8__locals0.member)));
			});
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public UpdatableChatMember member;

		public _E000 CS_0024_003C_003E8__locals1;

		internal void _E000(Vector2 pos)
		{
			CS_0024_003C_003E8__locals1._003C_003E4__this._contextMenu.Show(pos, new _ECA8(CS_0024_003C_003E8__locals1.social, null, member, CS_0024_003C_003E8__locals1.social.GetInvitationByProfile(member)));
		}
	}

	[SerializeField]
	private SimpleContextMenu _contextMenu;

	[SerializeField]
	private ChatMember _chatMember;

	[SerializeField]
	private RectTransform _membersContainer;

	[SerializeField]
	private CustomTextMeshProUGUI _friendsLabel;

	private _E79D _E2C7;

	private int _E2D9 = -1;

	public void Show(_E79D social)
	{
		ShowGameObject();
		_E2C7 = social;
		UI.AddDisposable(new _EC71<UpdatableChatMember, ChatMember>(social.FriendsList, _chatMember, _membersContainer, delegate(UpdatableChatMember member, ChatMember view)
		{
			view.Show(member, social.PlayerMember, active: true, delegate(Vector2 pos)
			{
				_contextMenu.Show(pos, new _ECA8(social, null, member, social.GetInvitationByProfile(member)));
			});
		}));
		UI.BindEvent(social.FriendsList.ItemsChanged, _E000);
	}

	private void _E000()
	{
		if (_E2D9 != _E2C7.FriendsList.Count)
		{
			_E2D9 = _E2C7.FriendsList.Count;
			_friendsLabel.text = _ED3E._E000(230691).Localized() + _ED3E._E000(54246) + _E2D9 + _ED3E._E000(27308);
		}
	}
}
