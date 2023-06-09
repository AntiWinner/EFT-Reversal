using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ChatShared;
using UnityEngine;

namespace EFT.UI.Chat;

public sealed class ChatMembersPanel : UIElement
{
	private sealed class _E000 : IComparer<UpdatableChatMember>
	{
		public static readonly _E000 Instance = new _E000();

		public int Compare(UpdatableChatMember x, UpdatableChatMember y)
		{
			if (x == null || y == null)
			{
				return -1;
			}
			int num = (int)((x.Info != null) ? x.Info.MemberCategory : ((EMemberCategory)(-1)));
			int num2 = (int)((y.Info != null) ? y.Info.MemberCategory : ((EMemberCategory)(-1)));
			if (num == num2 && num != -1)
			{
				return string.CompareOrdinal(x.Info.Nickname, y.Info.Nickname);
			}
			return num2 - num;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public _E79D social;

		public ChatMembersPanel _003C_003E4__this;

		internal void _E000(UpdatableChatMember member, ChatMember view)
		{
			_E002 CS_0024_003C_003E8__locals0 = new _E002
			{
				CS_0024_003C_003E8__locals1 = this,
				member = member
			};
			view.Show(CS_0024_003C_003E8__locals0.member, social.PlayerMember, active: true, delegate(Vector2 pos)
			{
				CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1._003C_003E4__this._contextMenu.Show(pos, new _ECA8(CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1.social, CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1._003C_003E4__this._banTimeWindow, CS_0024_003C_003E8__locals0.member, CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1.social.GetInvitationByProfile(CS_0024_003C_003E8__locals0.member)));
			});
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public UpdatableChatMember member;

		public _E001 CS_0024_003C_003E8__locals1;

		internal void _E000(Vector2 pos)
		{
			CS_0024_003C_003E8__locals1._003C_003E4__this._contextMenu.Show(pos, new _ECA8(CS_0024_003C_003E8__locals1.social, CS_0024_003C_003E8__locals1._003C_003E4__this._banTimeWindow, member, CS_0024_003C_003E8__locals1.social.GetInvitationByProfile(member)));
		}
	}

	[SerializeField]
	private SimpleContextMenu _contextMenu;

	[SerializeField]
	private BanTimeWindow _banTimeWindow;

	[SerializeField]
	private ChatMember _chatMemberTemplate;

	[SerializeField]
	private RectTransform _membersPlaceholder;

	private _E79D _E2C7;

	private _ED07<UpdatableChatMember> _E2CB;

	public void Show(_E79D social, _ED07<UpdatableChatMember> chatMembers)
	{
		UI.Dispose();
		ShowGameObject();
		_E2C7 = social;
		_E2CB = chatMembers;
		UI.AddDisposable(new _EC6F<UpdatableChatMember, ChatMember>(_E2CB, _E000.Instance, _chatMemberTemplate, _membersPlaceholder, delegate(UpdatableChatMember member, ChatMember view)
		{
			view.Show(member, social.PlayerMember, active: true, delegate(Vector2 pos)
			{
				_contextMenu.Show(pos, new _ECA8(social, _banTimeWindow, member, social.GetInvitationByProfile(member)));
			});
		}));
	}

	public override void Close()
	{
		base.Close();
		_E2C7?.SearchedFriendsList.Clear();
		_E2C7 = null;
		if (_contextMenu.gameObject.activeSelf)
		{
			_contextMenu.Close();
		}
	}
}
