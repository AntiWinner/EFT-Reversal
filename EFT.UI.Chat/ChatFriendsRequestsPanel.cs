using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Chat;

public sealed class ChatFriendsRequestsPanel : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public ChatFriendsRequestsPanel _003C_003E4__this;

		public _E79D social;

		public Action<_E467, Vector2> _003C_003E9__4;

		public Action<_E467, Vector2> _003C_003E9__5;

		internal void _E000(_E467 arg, FriendsInvitationView view)
		{
			view.Show(arg, delegate(_E467 invitation, Vector2 pos)
			{
				_003C_003E4__this._contextMenu.Show(pos, new _ECA8(social, null, invitation.From, invitation));
			});
		}

		internal void _E001(_E467 invitation, Vector2 pos)
		{
			_003C_003E4__this._contextMenu.Show(pos, new _ECA8(social, null, invitation.From, invitation));
		}

		internal void _E002(_E467 arg, FriendsInvitationView view)
		{
			view.Show(arg, delegate(_E467 invitation, Vector2 pos)
			{
				_003C_003E4__this._contextMenu.Show(pos, new _ECA8(social, null, invitation.To, invitation));
			});
		}

		internal void _E003(_E467 invitation, Vector2 pos)
		{
			_003C_003E4__this._contextMenu.Show(pos, new _ECA8(social, null, invitation.To, invitation));
		}
	}

	private const string _E2DD = "UI/Social/AllRequestsAccept";

	private const string _E2DE = "UI/Social/AllRequestsAccepted";

	[SerializeField]
	private Button _acceptAllButton;

	[SerializeField]
	private SimpleContextMenu _contextMenu;

	[SerializeField]
	private FriendsInvitationView _friendsInvitation;

	[SerializeField]
	private RectTransform _invitationsContainer;

	[SerializeField]
	private CustomTextMeshProUGUI _requestsLabel;

	private _E79D _E2C7;

	private int _E2DF = -1;

	public void Show(_E79D social)
	{
		ShowGameObject();
		_E2C7 = social;
		UI.AddDisposable(new _EC71<_E467, FriendsInvitationView>(social.InputFriendsInvitations.BindWhere((_E467 x) => x.Profile != null), _friendsInvitation, _invitationsContainer, delegate(_E467 arg, FriendsInvitationView view)
		{
			view.Show(arg, delegate(_E467 invitation, Vector2 pos)
			{
				_contextMenu.Show(pos, new _ECA8(social, null, invitation.From, invitation));
			});
		}));
		UI.AddDisposable(new _EC71<_E467, FriendsInvitationView>(social.OutputFriendsInvitations.BindWhere((_E467 x) => x.Profile != null), _friendsInvitation, _invitationsContainer, delegate(_E467 arg, FriendsInvitationView view)
		{
			view.Show(arg, delegate(_E467 invitation, Vector2 pos)
			{
				_contextMenu.Show(pos, new _ECA8(social, null, invitation.To, invitation));
			});
		}));
		UI.BindEvent(social.InputFriendsInvitations.ItemsChanged, _E003);
		UI.BindEvent(social.OutputFriendsInvitations.ItemsChanged, _E003);
		_E000();
		_acceptAllButton?.onClick.AddListener(_E001);
	}

	public override void Close()
	{
		base.Close();
		_acceptAllButton?.onClick.RemoveListener(_E001);
	}

	private void _E000()
	{
		_acceptAllButton.gameObject.SetActive(_E2C7.InputFriendsInvitations.Count > 0);
	}

	private void _E001()
	{
		ItemUiContext.Instance.ShowMessageWindow(_ED3E._E000(230786).Localized(), _E002, delegate
		{
		});
	}

	private async void _E002()
	{
		_acceptAllButton.gameObject.SetActive(value: false);
		Task task = _E2C7.AcceptAllFriendRequests();
		await task;
		if (task.IsCompleted)
		{
			_E000();
			ItemUiContext.Instance.ShowMessageWindow(_ED3E._E000(230871).Localized(), delegate
			{
			});
		}
	}

	private void _E003()
	{
		int num = _E2C7.InputFriendsInvitations.Count + _E2C7.OutputFriendsInvitations.Count;
		if (_E2DF != num)
		{
			_E2DF = num;
			_requestsLabel.text = _ED3E._E000(230822).Localized() + _ED3E._E000(54246) + num + _ED3E._E000(27308);
			_E000();
		}
	}
}
