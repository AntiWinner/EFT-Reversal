using System.Runtime.CompilerServices;
using ChatShared;
using UI.Matchmaker.Group;
using UnityEngine;

namespace EFT.UI;

public sealed class PlayersInviteWindow : Window<_EC7C>
{
	[SerializeField]
	private FriendListInvitePlayerPanel _friendListInvitePlayerPanel;

	[SerializeField]
	private SimpleContextMenu _contextMenu;

	private _EC99 m__E000;

	public _EC7C Show(string profileAid, _ECEF<UpdatableChatMember> friendsList, _EC99 matchmakerPlayersController)
	{
		ShowGameObject();
		_EC7C result = Show(Close);
		_friendListInvitePlayerPanel.Show(profileAid, friendsList, matchmakerPlayersController);
		this.m__E000 = matchmakerPlayersController;
		this.m__E000.OnPlayerContextRequest += _E000;
		UI.AddDisposable(delegate
		{
			this.m__E000.OnPlayerContextRequest -= _E000;
		});
		return result;
	}

	private void _E000(_EC9B raidPlayer, Vector2 position)
	{
		_contextMenu.Show(position, this.m__E000.GetContextInteractions(raidPlayer));
	}

	[CompilerGenerated]
	private void _E001()
	{
		this.m__E000.OnPlayerContextRequest -= _E000;
	}
}
