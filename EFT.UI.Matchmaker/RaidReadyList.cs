using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using ChatShared;
using TMPro;
using UnityEngine;

namespace EFT.UI.Matchmaker;

public sealed class RaidReadyList : UIElement
{
	private sealed class _E000 : IComparer<_EC9B>
	{
		public static readonly _E000 Instance = new _E000();

		public int Compare(_EC9B x, _EC9B y)
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
		public IList<UpdatableChatMember> friends;

		public string profileAid;

		public _ECEF<_E551> invites;

		public RaidReadyList _003C_003E4__this;

		internal void _E000(_EC9B player, RaidReadyPlayerPanel playerPanel)
		{
			_E002 CS_0024_003C_003E8__locals0 = new _E002();
			CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1 = this;
			CS_0024_003C_003E8__locals0.playerPanel = playerPanel;
			bool friendPanel = friends.Select((UpdatableChatMember x) => x.AccountId).Contains(player.AccountId);
			CS_0024_003C_003E8__locals0.playerPanel.Show(player, player.AccountId == profileAid, friendPanel, invites);
			_003C_003E4__this.PlayerPanels.Add(player.AccountId, CS_0024_003C_003E8__locals0.playerPanel);
			CS_0024_003C_003E8__locals0.playerPanel.OnPlayerClick += _003C_003E4__this._E000;
			_003C_003E4__this.UI.AddDisposable(delegate
			{
				CS_0024_003C_003E8__locals0.playerPanel.OnPlayerClick -= CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1._003C_003E4__this._E000;
			});
			_003C_003E4__this._E002();
			_003C_003E4__this._E004();
		}

		internal void _E001()
		{
			_003C_003E4__this._E07E.OnRemove -= _003C_003E4__this._E001;
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public RaidReadyPlayerPanel playerPanel;

		public _E001 CS_0024_003C_003E8__locals1;

		internal void _E000()
		{
			playerPanel.OnPlayerClick -= CS_0024_003C_003E8__locals1._003C_003E4__this._E000;
		}
	}

	[CompilerGenerated]
	private sealed class _E003
	{
		public string profileAid;

		internal bool _E000(_EC9B x)
		{
			return x.AccountId == profileAid;
		}
	}

	[CompilerGenerated]
	private Action<_EC9B, Vector2> _E296;

	[SerializeField]
	private TMP_InputField _inputField;

	[SerializeField]
	private RectTransform _groupReadyPlayersContainer;

	[SerializeField]
	private RaidReadyPlayerPanel _playerPanelTemplate;

	public Dictionary<string, RaidReadyPlayerPanel> PlayerPanels = new Dictionary<string, RaidReadyPlayerPanel>();

	private _EC71<_EC9B, RaidReadyPlayerPanel> _E07E;

	private IList<UpdatableChatMember> _E2A5;

	private string _E29C;

	private _ED07<_EC9B> _E2A6;

	private string _E2A7;

	public event Action<_EC9B, Vector2> OnPlayerClick
	{
		[CompilerGenerated]
		add
		{
			Action<_EC9B, Vector2> action = _E296;
			Action<_EC9B, Vector2> action2;
			do
			{
				action2 = action;
				Action<_EC9B, Vector2> value2 = (Action<_EC9B, Vector2>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E296, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<_EC9B, Vector2> action = _E296;
			Action<_EC9B, Vector2> action2;
			do
			{
				action2 = action;
				Action<_EC9B, Vector2> value2 = (Action<_EC9B, Vector2>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E296, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	private void Awake()
	{
		_inputField.onValueChanged.AddListener(delegate(string arg)
		{
			_E2A7 = arg;
			_E004();
		});
	}

	public void Show(_ED07<_EC9B> playersList, _ECEF<_E551> invites, IList<UpdatableChatMember> friends, string profileAid)
	{
		ShowGameObject();
		_E2A5 = friends;
		_E29C = profileAid;
		_E2A6 = playersList;
		_E07E = UI.AddDisposable(new _EC71<_EC9B, RaidReadyPlayerPanel>(playersList, _playerPanelTemplate, _groupReadyPlayersContainer, delegate(_EC9B player, RaidReadyPlayerPanel playerPanel)
		{
			bool friendPanel = friends.Select((UpdatableChatMember x) => x.AccountId).Contains(player.AccountId);
			playerPanel.Show(player, player.AccountId == profileAid, friendPanel, invites);
			PlayerPanels.Add(player.AccountId, playerPanel);
			playerPanel.OnPlayerClick += _E000;
			UI.AddDisposable(delegate
			{
				playerPanel.OnPlayerClick -= _E000;
			});
			_E002();
			_E004();
		}));
		_E002();
		_E07E.OnRemove += _E001;
		UI.AddDisposable(delegate
		{
			_E07E.OnRemove -= _E001;
		});
	}

	private void _E000(_EC9B player, Vector2 position)
	{
		_E296?.Invoke(player, position);
	}

	private void _E001(_EC9B player)
	{
		PlayerPanels.Remove(player.AccountId);
	}

	private void _E002()
	{
		_E07E?.OrderBy((_EC9B x) => x, RaidReadyList._E000.Instance);
		foreach (KeyValuePair<string, RaidReadyPlayerPanel> item in PlayerPanels.Where((KeyValuePair<string, RaidReadyPlayerPanel> x) => _E2A5.Select((UpdatableChatMember y) => y.AccountId).Contains(x.Key)))
		{
			item.Value.transform.SetAsFirstSibling();
		}
		if (PlayerPanels.ContainsKey(_E29C))
		{
			PlayerPanels[_E29C].transform.SetAsFirstSibling();
		}
	}

	private string _E003(string profileAid)
	{
		_EC9B obj = _E2A6.FirstOrDefault((_EC9B x) => x.AccountId == profileAid);
		if (obj == null)
		{
			return string.Empty;
		}
		return obj.Info.Nickname;
	}

	private void _E004()
	{
		if (string.IsNullOrEmpty(_E2A7))
		{
			foreach (KeyValuePair<string, RaidReadyPlayerPanel> playerPanel in PlayerPanels)
			{
				playerPanel.Value.ShowGameObject();
			}
			return;
		}
		foreach (KeyValuePair<string, RaidReadyPlayerPanel> playerPanel2 in PlayerPanels)
		{
			if (_E003(playerPanel2.Key).IndexOf(_E2A7, StringComparison.OrdinalIgnoreCase) >= 0)
			{
				playerPanel2.Value.ShowGameObject();
			}
			else
			{
				playerPanel2.Value.HideGameObject();
			}
		}
	}

	public override void Close()
	{
		PlayerPanels.Clear();
		base.Close();
	}

	[CompilerGenerated]
	private void _E005(string arg)
	{
		_E2A7 = arg;
		_E004();
	}

	[CompilerGenerated]
	private bool _E006(KeyValuePair<string, RaidReadyPlayerPanel> x)
	{
		return _E2A5.Select((UpdatableChatMember y) => y.AccountId).Contains(x.Key);
	}
}
