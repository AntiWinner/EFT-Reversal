using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using ChatShared;
using UnityEngine;

namespace EFT.UI.Matchmaker;

public sealed class GroupPlayersList : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public IList<UpdatableChatMember> friends;

		public string profileAid;

		public _ECF5<_EC9A> group;

		public GroupPlayersList _003C_003E4__this;

		internal void _E000(_EC9B player, GroupPlayerPanel playerPanel)
		{
			_E001 CS_0024_003C_003E8__locals0 = new _E001();
			CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1 = this;
			CS_0024_003C_003E8__locals0.playerPanel = playerPanel;
			bool friendPanel = friends.Select((UpdatableChatMember x) => x.AccountId).Contains(player.AccountId);
			CS_0024_003C_003E8__locals0.playerPanel.Show(player, friendPanel, profileAid, group.Value?.Owner);
			CS_0024_003C_003E8__locals0.playerPanel.OnPlayerClick += _003C_003E4__this._E001;
			_003C_003E4__this.UI.AddDisposable(delegate
			{
				CS_0024_003C_003E8__locals0.playerPanel.OnPlayerClick -= CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1._003C_003E4__this._E001;
			});
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public GroupPlayerPanel playerPanel;

		public _E000 CS_0024_003C_003E8__locals1;

		internal void _E000()
		{
			playerPanel.OnPlayerClick -= CS_0024_003C_003E8__locals1._003C_003E4__this._E001;
		}
	}

	[CompilerGenerated]
	private Action<_EC9B, Vector2> _E296;

	[SerializeField]
	private RectTransform _groupPlayersContainer;

	[SerializeField]
	private GroupPlayerPanel _groupPlayerPanelTemplate;

	private _EC71<_EC9B, GroupPlayerPanel> _E03A;

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

	public void Show(_ECEF<_EC9B> playersList, _ECF5<_EC9A> group, IList<UpdatableChatMember> friends, string profileAid)
	{
		ShowGameObject();
		_E03A = UI.AddDisposable(new _EC71<_EC9B, GroupPlayerPanel>(playersList, _groupPlayerPanelTemplate, _groupPlayersContainer, delegate(_EC9B player, GroupPlayerPanel playerPanel)
		{
			bool friendPanel = friends.Select((UpdatableChatMember x) => x.AccountId).Contains(player.AccountId);
			playerPanel.Show(player, friendPanel, profileAid, group.Value?.Owner);
			playerPanel.OnPlayerClick += _E001;
			UI.AddDisposable(delegate
			{
				playerPanel.OnPlayerClick -= _E001;
			});
		}));
		UI.AddDisposable(group.Value?.Owner.Bind(_E000));
	}

	private void _E000(_EC9B leader)
	{
		foreach (var (obj2, groupPlayerPanel2) in _E03A)
		{
			if (obj2.AccountId == leader?.AccountId)
			{
				groupPlayerPanel2.transform.SetAsFirstSibling();
			}
		}
	}

	private void _E001(_EC9B player, Vector2 position)
	{
		_E296?.Invoke(player, position);
	}
}
