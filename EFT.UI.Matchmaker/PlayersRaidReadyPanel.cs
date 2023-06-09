using System.Runtime.CompilerServices;
using ChatShared;
using UnityEngine;

namespace EFT.UI.Matchmaker;

public sealed class PlayersRaidReadyPanel : UIElement
{
	[SerializeField]
	private RaidReadyList _raidReadyList;

	[SerializeField]
	private GroupPlayersList _groupPlayersList;

	private _EC99 _E006;

	public void Show(string profileAid, _ECEF<UpdatableChatMember> friendsList, _EC99 matchmakerPlayersController, bool showRaidReady = true)
	{
		UI.Dispose();
		ShowGameObject();
		_E006 = matchmakerPlayersController;
		if (showRaidReady)
		{
			_raidReadyList.Show(matchmakerPlayersController.RaidReadyPlayers, matchmakerPlayersController.SentInvites, friendsList, profileAid);
			_raidReadyList.OnPlayerClick += RequestContextMenuForPlayer;
		}
		_groupPlayersList.Show(matchmakerPlayersController.GroupPlayers, matchmakerPlayersController.GroupState, friendsList, profileAid);
		_groupPlayersList.OnPlayerClick += RequestContextMenuForPlayer;
		UI.AddDisposable(_raidReadyList);
		UI.AddDisposable(_groupPlayersList);
		UI.AddDisposable(delegate
		{
			_raidReadyList.OnPlayerClick -= RequestContextMenuForPlayer;
			_groupPlayersList.OnPlayerClick -= RequestContextMenuForPlayer;
		});
	}

	public void RequestContextMenuForPlayer(_EC9B player, Vector2 position)
	{
		_E006.RequestContextMenuForPlayer(player, position);
	}

	[CompilerGenerated]
	private void _E000()
	{
		_raidReadyList.OnPlayerClick -= RequestContextMenuForPlayer;
		_groupPlayersList.OnPlayerClick -= RequestContextMenuForPlayer;
	}
}
