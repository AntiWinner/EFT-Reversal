using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using EFT.UI.Matchmaker;
using EFT.UI.Screens;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class GroupPanel : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _ECF5<_EC9A> group;

		public GroupPanel _003C_003E4__this;

		public _ECEF<_EC9B> groupPlayers;

		internal void _E000(_EC9B player, GroupMemberView playerPanel)
		{
			_E001 CS_0024_003C_003E8__locals0 = new _E001();
			CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1 = this;
			CS_0024_003C_003E8__locals0.playerPanel = playerPanel;
			CS_0024_003C_003E8__locals0.playerPanel.Show(player, group.Value.Owner);
			CS_0024_003C_003E8__locals0.playerPanel.OnGroupMemberClick += _003C_003E4__this.ShowContextMenu;
			_003C_003E4__this.UI.AddDisposable(delegate
			{
				CS_0024_003C_003E8__locals0.playerPanel.OnGroupMemberClick -= CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1._003C_003E4__this.ShowContextMenu;
			});
		}

		internal void _E001()
		{
			_003C_003E4__this.m__E006.OnRaidReadyStatusChanged -= _003C_003E4__this._E006;
			_003C_003E4__this.m__E006.OnMatchingTypeUpdate -= _003C_003E4__this._E005;
			_EC92.Instance.OnScreenChanged -= _003C_003E4__this._E001;
			groupPlayers.ItemAdded -= _003C_003E4__this._E003;
			groupPlayers.ItemRemoved -= _003C_003E4__this._E004;
			groupPlayers.AllItemsRemoved -= _003C_003E4__this._E002;
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public GroupMemberView playerPanel;

		public _E000 CS_0024_003C_003E8__locals1;

		internal void _E000()
		{
			playerPanel.OnGroupMemberClick -= CS_0024_003C_003E8__locals1._003C_003E4__this.ShowContextMenu;
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public _EC9B raidPlayer;

		internal bool _E000(_EC9B p)
		{
			return p.AccountId != raidPlayer.AccountId;
		}
	}

	private const string _E0FF = "InRaidButton";

	[CompilerGenerated]
	private Action _E100;

	[SerializeField]
	private SimpleContextMenu _contextMenu;

	[SerializeField]
	private Transform _groupListContainer;

	[SerializeField]
	private Transform _emptyMemberContainer;

	[SerializeField]
	private GroupMemberView _groupMemberTemplate;

	[SerializeField]
	private EmptyMemberView _emptyMemberTemplate;

	[SerializeField]
	private Button _inRaidButton;

	[SerializeField]
	private TextMeshProUGUI _inRaidButtonLabel;

	[SerializeField]
	private GameObject _groupPanelBlocker;

	private readonly List<EmptyMemberView> _E101 = new List<EmptyMemberView>();

	private _EC99 m__E006;

	private _ECEF<_EC9B> _E102;

	private bool _E103;

	private bool _E104;

	private _EC71<_EC9B, GroupMemberView> _E03A;

	private _E79D _E105;

	private bool _E000
	{
		get
		{
			if (!_E104 && _E103)
			{
				return this.m__E006.Enabled;
			}
			return false;
		}
	}

	public event Action RaidReadyButtonPressed
	{
		[CompilerGenerated]
		add
		{
			Action action = _E100;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E100, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = _E100;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E100, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public void Awake()
	{
		_inRaidButton.onClick.AddListener(_E008);
	}

	public void Show(_EC99 matchmakerPlayersController, _E79D socialNetwork)
	{
		UI.Dispose();
		ShowGameObject();
		this.m__E006 = matchmakerPlayersController;
		_E105 = socialNetwork;
		_E102 = new _ECEF<_EC9B>(this.m__E006.GroupPlayers);
		_ECEF<_EC9B> groupPlayers = matchmakerPlayersController.GroupPlayers;
		_ECF5<_EC9A> group = matchmakerPlayersController.GroupState;
		_E03A = UI.AddDisposable(new _EC71<_EC9B, GroupMemberView>(_E102, _groupMemberTemplate, _groupListContainer, delegate(_EC9B player, GroupMemberView playerPanel)
		{
			playerPanel.Show(player, group.Value.Owner);
			playerPanel.OnGroupMemberClick += ShowContextMenu;
			UI.AddDisposable(delegate
			{
				playerPanel.OnGroupMemberClick -= ShowContextMenu;
			});
		}));
		UI.AddDisposable(group.Value?.Owner.Bind(_E000));
		_E009();
		this.m__E006.OnRaidReadyStatusChanged += _E006;
		this.m__E006.OnMatchingTypeUpdate += _E005;
		_EC92.Instance.OnScreenChanged += _E001;
		groupPlayers.ItemAdded += _E003;
		groupPlayers.ItemRemoved += _E004;
		groupPlayers.AllItemsRemoved += _E002;
		_E006(this.m__E006.Group?.Owner.Value, this.m__E006.LeaderRaidReadyStatus);
		UI.AddDisposable(delegate
		{
			this.m__E006.OnRaidReadyStatusChanged -= _E006;
			this.m__E006.OnMatchingTypeUpdate -= _E005;
			_EC92.Instance.OnScreenChanged -= _E001;
			groupPlayers.ItemAdded -= _E003;
			groupPlayers.ItemRemoved -= _E004;
			groupPlayers.AllItemsRemoved -= _E002;
		});
	}

	private void _E000(_EC9B leader)
	{
		foreach (var (obj2, groupMemberView2) in _E03A)
		{
			if (obj2.AccountId == leader?.AccountId)
			{
				groupMemberView2.transform.SetAsFirstSibling();
			}
		}
	}

	private void _E001(EEftScreenType eftScreen)
	{
		_E104 = eftScreen == EEftScreenType.Insurance;
		_inRaidButton.gameObject.SetActive(this._E000);
	}

	public void SetGroupsAvailability(bool available)
	{
		_groupPanelBlocker.SetActive(!available);
	}

	private void _E002()
	{
		_E102.Clear();
	}

	private void _E003(_EC9B raidPlayer)
	{
		if (_E102.Count <= 5 && _E102.All((_EC9B p) => p.AccountId != raidPlayer.AccountId))
		{
			_E102.Add(raidPlayer);
		}
	}

	private void _E004(_EC9B raidPlayer)
	{
		_E102.Remove(raidPlayer);
	}

	private void _E005(EMatchingType _)
	{
		_E007();
	}

	private void _E006(_EC9B player, bool _)
	{
		_E007();
	}

	private void _E007()
	{
		_EC9B obj = this.m__E006.Group.Owner?.Value;
		if (obj == null)
		{
			_E103 = false;
			_inRaidButton.gameObject.SetActive(this._E000);
		}
		else
		{
			_E103 = obj.IsReady;
			_inRaidButton.gameObject.SetActive(this._E000);
			_inRaidButtonLabel.SetText(string.Format(_ED3E._E000(252026), _ED3E._E000(252008).Localized(EStringCase.Upper), this.m__E006.TotalReadyGroupPlayers, this.m__E006.GroupPlayers.Count));
		}
	}

	private void _E008()
	{
		_E100?.Invoke();
	}

	private void _E009()
	{
		foreach (EmptyMemberView item in _E101)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
		_E101.Clear();
		for (int i = 0; i < 5; i++)
		{
			EmptyMemberView emptyMemberView = UnityEngine.Object.Instantiate(_emptyMemberTemplate, _emptyMemberContainer);
			emptyMemberView.Show(_E105.FriendsList, this.m__E006);
			_E101.Add(emptyMemberView);
		}
	}

	public void ShowContextMenu(_E550 player, PointerEventData eventData)
	{
		switch (eventData.button)
		{
		case PointerEventData.InputButton.Left:
			ItemUiContext.Instance.ShowPlayersInviteWindow(this.m__E006.CurrentProfileAid, _E105.FriendsList, this.m__E006);
			break;
		case PointerEventData.InputButton.Right:
			_contextMenu.Show(eventData.position, this.m__E006.GetContextInteractions(player));
			break;
		}
	}
}
