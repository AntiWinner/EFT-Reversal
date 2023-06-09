using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ChatShared;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.Chat;

public sealed class ChatInvitePlayersPanel : UIElement, IPointerClickHandler, IEventSystemHandler
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _E79D social;

		public _E466 selectedDialogue;

		public ChatInvitePlayersPanel _003C_003E4__this;

		internal void _E000(UpdatableChatMember member, ChatMember view)
		{
			_E001 CS_0024_003C_003E8__locals0 = new _E001();
			CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1 = this;
			CS_0024_003C_003E8__locals0.member = member;
			CS_0024_003C_003E8__locals0.view = view;
			CS_0024_003C_003E8__locals0.view.Show(CS_0024_003C_003E8__locals0.member, social.PlayerMember, !selectedDialogue.ActiveUsers.Contains(CS_0024_003C_003E8__locals0.member), delegate
			{
				if (CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1._003C_003E4__this._E2C6.Contains(CS_0024_003C_003E8__locals0.member))
				{
					CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1._003C_003E4__this._E2C6.Remove(CS_0024_003C_003E8__locals0.member);
					CS_0024_003C_003E8__locals0.view.MarkAsChosen(state: false);
				}
				else
				{
					CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1._003C_003E4__this._E2C6.Add(CS_0024_003C_003E8__locals0.member);
					CS_0024_003C_003E8__locals0.view.MarkAsChosen(state: true);
				}
			});
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public UpdatableChatMember member;

		public ChatMember view;

		public _E000 CS_0024_003C_003E8__locals1;

		internal void _E000(Vector2 pos)
		{
			if (CS_0024_003C_003E8__locals1._003C_003E4__this._E2C6.Contains(member))
			{
				CS_0024_003C_003E8__locals1._003C_003E4__this._E2C6.Remove(member);
				view.MarkAsChosen(state: false);
			}
			else
			{
				CS_0024_003C_003E8__locals1._003C_003E4__this._E2C6.Add(member);
				view.MarkAsChosen(state: true);
			}
		}
	}

	[SerializeField]
	private GameObject _captionPanel;

	[SerializeField]
	private Button _closeButton;

	[SerializeField]
	private DefaultUIButton _inviteButtonSpawner;

	[SerializeField]
	private ChatMember _chatMemberTemplate;

	[SerializeField]
	private RectTransform _membersPlaceholder;

	private readonly List<UpdatableChatMember> _E2C6 = new List<UpdatableChatMember>();

	private _E79D _E2C7;

	private _E466 _E2C8;

	private void Awake()
	{
		_captionPanel.AddComponent<UIDragComponent>().Init(base.RectTransform, putOnTop: true);
		_inviteButtonSpawner.OnClick.AddListener(delegate
		{
			if (_E2C6.Count > 0)
			{
				_E2C7.InviteToDialogue(_E2C8, _E2C6, delegate
				{
					Debug.Log(_ED3E._E000(230628));
				});
				Close();
			}
		});
		_closeButton.onClick.AddListener(Close);
	}

	public void Show(_E79D social, _E466 selectedDialogue)
	{
		ShowGameObject();
		_E000();
		_E2C7 = social;
		_E2C8 = selectedDialogue;
		_E2C6.Clear();
		UI.AddDisposable(new _EC71<UpdatableChatMember, ChatMember>(social.FriendsList, _chatMemberTemplate, _membersPlaceholder, delegate(UpdatableChatMember member, ChatMember view)
		{
			view.Show(member, social.PlayerMember, !selectedDialogue.ActiveUsers.Contains(member), delegate
			{
				if (_E2C6.Contains(member))
				{
					_E2C6.Remove(member);
					view.MarkAsChosen(state: false);
				}
				else
				{
					_E2C6.Add(member);
					view.MarkAsChosen(state: true);
				}
			});
		}));
	}

	public void OnPointerClick([NotNull] PointerEventData eventData)
	{
		_E000();
	}

	private void _E000()
	{
		base.transform.SetAsLastSibling();
	}

	[CompilerGenerated]
	private void _E001()
	{
		if (_E2C6.Count > 0)
		{
			_E2C7.InviteToDialogue(_E2C8, _E2C6, delegate
			{
				Debug.Log(_ED3E._E000(230628));
			});
			Close();
		}
	}
}
