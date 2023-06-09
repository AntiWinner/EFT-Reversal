using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ChatShared;
using Comfort.Common;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.Chat;

public sealed class ChatCreateDialoguePanel : UIElement, IPointerClickHandler, IEventSystemHandler
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _E79D social;

		public ChatCreateDialoguePanel _003C_003E4__this;

		internal void _E000(UpdatableChatMember member, ChatMember view)
		{
			_E001 CS_0024_003C_003E8__locals0 = new _E001();
			CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1 = this;
			CS_0024_003C_003E8__locals0.member = member;
			CS_0024_003C_003E8__locals0.view = view;
			CS_0024_003C_003E8__locals0.view.Show(CS_0024_003C_003E8__locals0.member, social.PlayerMember, active: true, delegate
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
				CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1._003C_003E4__this._E000();
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
			CS_0024_003C_003E8__locals1._003C_003E4__this._E000();
		}
	}

	[SerializeField]
	private GameObject _captionPanel;

	[SerializeField]
	private Button _closeButton;

	[SerializeField]
	private ValidationInputField _dialogNameInputField;

	[SerializeField]
	private DefaultUIButton _createButton;

	[SerializeField]
	private DefaultUIButton _cancelButton;

	[SerializeField]
	private ChatMember _chatMemberTemplate;

	[SerializeField]
	private RectTransform _membersPlaceholder;

	private readonly List<UpdatableChatMember> _E2C6 = new List<UpdatableChatMember>();

	private _E79D _E2C7;

	private void Awake()
	{
		_captionPanel.AddComponent<UIDragComponent>().Init(base.RectTransform, putOnTop: true);
		_createButton.OnClick.AddListener(delegate
		{
			if (_dialogNameInputField.text.Length < 3)
			{
				_E857.DisplayWarningNotification(_ED3E._E000(230528));
			}
			else if (_E2C6.Count < 2)
			{
				_E857.DisplayWarningNotification(_ED3E._E000(230569));
			}
			else
			{
				Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.ChatSelect);
				_E2C7.CreateGroupDialogue(_dialogNameInputField.text, _E2C6.Select((UpdatableChatMember x) => x.Id).ToArray(), delegate
				{
					Debug.Log(_ED3E._E000(230596) + _dialogNameInputField.text + _ED3E._E000(230650));
				});
				Close();
			}
		});
		_dialogNameInputField.onValueChanged.AddListener(delegate
		{
			_E000();
		});
		_closeButton.onClick.AddListener(Close);
		_cancelButton.OnClick.AddListener(Close);
	}

	public void Show(_E79D social)
	{
		ShowGameObject();
		_E001();
		_E2C7 = social;
		UI.AddDisposable(new _EC71<UpdatableChatMember, ChatMember>(social.FriendsList, _chatMemberTemplate, _membersPlaceholder, delegate(UpdatableChatMember member, ChatMember view)
		{
			view.Show(member, social.PlayerMember, active: true, delegate
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
				_E000();
			});
		}));
		_E000();
		_cancelButton.SetHeaderText(_ED3E._E000(249950), 24);
	}

	private void _E000()
	{
		_createButton.Interactable = _E2C6.Count > 1 && _dialogNameInputField.text.Length > 2;
	}

	public void OnPointerClick([NotNull] PointerEventData eventData)
	{
		_E001();
	}

	private void _E001()
	{
		base.transform.SetAsLastSibling();
	}

	[CompilerGenerated]
	private void _E002()
	{
		if (_dialogNameInputField.text.Length < 3)
		{
			_E857.DisplayWarningNotification(_ED3E._E000(230528));
			return;
		}
		if (_E2C6.Count < 2)
		{
			_E857.DisplayWarningNotification(_ED3E._E000(230569));
			return;
		}
		Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.ChatSelect);
		_E2C7.CreateGroupDialogue(_dialogNameInputField.text, _E2C6.Select((UpdatableChatMember x) => x.Id).ToArray(), delegate
		{
			Debug.Log(_ED3E._E000(230596) + _dialogNameInputField.text + _ED3E._E000(230650));
		});
		Close();
	}

	[CompilerGenerated]
	private void _E003(IResult result)
	{
		Debug.Log(_ED3E._E000(230596) + _dialogNameInputField.text + _ED3E._E000(230650));
	}

	[CompilerGenerated]
	private void _E004(string arg)
	{
		_E000();
	}
}
