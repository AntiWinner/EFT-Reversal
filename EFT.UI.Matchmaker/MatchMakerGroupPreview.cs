using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT.UI.Matchmaker;

public class MatchMakerGroupPreview : UIElement
{
	[SerializeField]
	private List<ComradeView> _comradesPositions;

	[SerializeField]
	private MatchMakerPlayerPreview _playerPreviewTemplate;

	private string _E29C;

	private _EC99 _E29D;

	private List<MatchMakerPlayerPreview> _E03A = new List<MatchMakerPlayerPreview>();

	private Func<_EC9B, bool, _EC98> _E29E;

	private RaidSettings _E29F;

	public void Show(string mainProfileAid, _EC99 matchmakerPlayersController, RaidSettings raidSettings, Func<_EC9B, bool, _EC98> interactionsFactory)
	{
		UI.Dispose();
		ShowGameObject();
		_E29C = mainProfileAid;
		_E29D = matchmakerPlayersController;
		_E29F = raidSettings;
		_E29E = interactionsFactory;
		UI.AddDisposable(matchmakerPlayersController.GroupPlayers.ItemsChanged.Bind(_E000));
	}

	private void _E000()
	{
		List<_EC9B> list = _E29D.GroupPlayers.Where((_EC9B player) => player.AccountId != _E29C).ToList();
		for (int num = _E03A.Count - 1; num >= list.Count; num--)
		{
			_E002(num);
		}
		for (int i = 0; i < list.Count && i < _comradesPositions.Count; i++)
		{
			if (!_E29D.CanShowGroupPreview)
			{
				break;
			}
			MatchMakerPlayerPreview matchMakerPlayerPreview = _E001(i);
			_EC9B obj = list[i];
			if (matchMakerPlayerPreview.PlayerAid != obj.AccountId)
			{
				matchMakerPlayerPreview.Show(_E29D, _E29F, obj, _E29E(obj, arg2: true)).HandleExceptions();
				_comradesPositions[i].TogglePlaceholder(active: false);
			}
			else
			{
				_comradesPositions[i].TogglePlaceholder(active: false);
			}
		}
	}

	private MatchMakerPlayerPreview _E001(int index)
	{
		if (_E03A.Count > index)
		{
			return _E03A[index];
		}
		MatchMakerPlayerPreview matchMakerPlayerPreview = UnityEngine.Object.Instantiate(_playerPreviewTemplate, _comradesPositions[index].transform, worldPositionStays: false);
		_E03A.Insert(index, matchMakerPlayerPreview);
		return matchMakerPlayerPreview;
	}

	private MatchMakerPlayerPreview _E002(int index)
	{
		MatchMakerPlayerPreview matchMakerPlayerPreview = _E03A[index];
		matchMakerPlayerPreview.Close();
		_comradesPositions[index].TogglePlaceholder(active: true);
		return matchMakerPlayerPreview;
	}

	public override void Close()
	{
		while (_E03A.Any())
		{
			MatchMakerPlayerPreview matchMakerPlayerPreview = _E002(0);
			_E03A.Remove(matchMakerPlayerPreview);
			UnityEngine.Object.DestroyImmediate(matchMakerPlayerPreview.GameObject);
		}
		base.Close();
	}

	[CompilerGenerated]
	private bool _E003(_EC9B player)
	{
		return player.AccountId != _E29C;
	}
}
