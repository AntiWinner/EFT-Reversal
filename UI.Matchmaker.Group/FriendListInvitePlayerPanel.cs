using System.Linq;
using System.Runtime.CompilerServices;
using ChatShared;
using EFT.UI;
using EFT.UI.Matchmaker;
using UnityEngine;

namespace UI.Matchmaker.Group;

public sealed class FriendListInvitePlayerPanel : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _EC9B groupMember;

		internal bool _E000(_EC9B item)
		{
			return item.AccountId == groupMember.AccountId;
		}
	}

	[SerializeField]
	private RaidReadyList _friendsList;

	[SerializeField]
	private GroupPlayersList _groupPlayersList;

	private _EC99 _E006;

	private _ED07<_EC9B> _E007;

	private _ECEF<UpdatableChatMember> _E008;

	public void Show(string profileAid, _ECEF<UpdatableChatMember> friendsList, _EC99 matchmakerPlayersController)
	{
		UI.Dispose();
		ShowGameObject();
		_E006 = matchmakerPlayersController;
		_E008 = friendsList;
		_E007 = new _ED07<_EC9B>();
		UI.BindEvent(friendsList.ItemsChanged, _E001);
		UI.BindEvent(_E006.GroupPlayers.ItemsChanged, _E000);
		_friendsList.Show(_E007, matchmakerPlayersController.SentInvites, friendsList, profileAid);
		_friendsList.OnPlayerClick += RequestContextMenuForPlayer;
		_groupPlayersList.Show(matchmakerPlayersController.GroupPlayers, matchmakerPlayersController.GroupState, friendsList, profileAid);
		_groupPlayersList.OnPlayerClick += RequestContextMenuForPlayer;
		UI.AddDisposable(_friendsList);
		UI.AddDisposable(_groupPlayersList);
		UI.AddDisposable(delegate
		{
			_friendsList.OnPlayerClick -= RequestContextMenuForPlayer;
			_groupPlayersList.OnPlayerClick -= RequestContextMenuForPlayer;
		});
	}

	private void _E000()
	{
		_E001();
		_ECEF<_EC9B> groupPlayers = _E006.GroupPlayers;
		if (groupPlayers.Count == 1)
		{
			return;
		}
		foreach (_EC9B groupMember in groupPlayers)
		{
			_EC9B obj = _E007.FirstOrDefault((_EC9B item) => item.AccountId == groupMember.AccountId);
			if (obj != null)
			{
				_E007.Remove(obj);
			}
		}
	}

	private void _E001()
	{
		_ED07<_EC9B> obj = new _ED07<_EC9B>();
		foreach (UpdatableChatMember item in _E008)
		{
			obj.Add(new _EC9B(new _E550
			{
				AccountId = item.AccountId,
				Id = item.Id,
				Info = _E54F.GetFromChatMemberInfo(item.Info)
			}));
		}
		_E007.UpdateItems(obj);
	}

	public void RequestContextMenuForPlayer(_EC9B player, Vector2 position)
	{
		_E006.RequestContextMenuForPlayer(player, position);
	}

	[CompilerGenerated]
	private void _E002()
	{
		_friendsList.OnPlayerClick -= RequestContextMenuForPlayer;
		_groupPlayersList.OnPlayerClick -= RequestContextMenuForPlayer;
	}
}
