using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ChatShared;
using Comfort.Common;
using EFT.Communications;
using EFT.InputSystem;
using EFT.UI.Screens;
using JetBrains.Annotations;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.Chat;

public sealed class ChatScreen : UIScreen, IPointerClickHandler, IEventSystemHandler
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public ChatScreen _003C_003E4__this;

		public _E464[] messages;

		public List<_E464> messagesWithItems;

		public DateTime expirationTime;

		internal void _E000(IEnumerable<ProfileChangeEvent> events)
		{
			_003C_003E4__this._E096.RedeemProfileRewards(events).HandleExceptions();
			events.ForEach(delegate(ProfileChangeEvent evt)
			{
				evt.Redeem();
			});
			messages.ForEach(delegate(_E464 message)
			{
				message.UpdateRewardStatus?.Invoke();
			});
		}

		internal void _E001()
		{
			if (messagesWithItems.Any())
			{
				_003C_003E4__this._E0B6(messagesWithItems, expirationTime);
			}
		}
	}

	private const float _E0B4 = 80f;

	[SerializeField]
	private GameObject _chatScreenObject;

	[SerializeField]
	private GameObject _captionPanel;

	[SerializeField]
	private ChatMessageSendBlock _inputPanel;

	[SerializeField]
	private ChatMembersPanel _chatMembersPanel;

	[SerializeField]
	private Button _closeButton;

	[SerializeField]
	private SwitchableParamsButton _switchableParamsButton;

	[SerializeField]
	private Button _friendsButton;

	[SerializeField]
	private CustomTextMeshProUGUI _friendsButtonLabel;

	[SerializeField]
	private Button _createGroupDialog;

	[SerializeField]
	private GameObject _quoteGameObject;

	[SerializeField]
	private CustomTextMeshProUGUI _quoteLabel;

	[SerializeField]
	private RectTransform _messagesContainerParent;

	[SerializeField]
	private MessagesContainer _messagesContainerTemplate;

	[SerializeField]
	private DialoguesContainer _dialoguesContainer;

	[SerializeField]
	private ChatFriendsPanel _chatFriendsPanel;

	[SerializeField]
	private ChatCreateDialoguePanel _chatCreateDialoguePanel;

	[SerializeField]
	private ValidationInputField _chatInputField;

	[SerializeField]
	private DefaultUIButton _sendButton;

	[SerializeField]
	private ProfileEventsWindow _profileEventsWindow;

	[SerializeField]
	private UiElementBlocker _friendsBlocker;

	private _E79D _E096;

	private _E72F _E0B5;

	private Action<IEnumerable<_E464>, DateTime?> _E0B6;

	private Action<string> _E0B7;

	private DateTime _E0B8;

	private int _E0B9 = -1;

	private int _E0BA = -1;

	private readonly Dictionary<_E466, MessagesContainer> _E0BB = new Dictionary<_E466, MessagesContainer>();

	private GameObject _E000 => _chatScreenObject;

	private _E464 _E001 => new _E464
	{
		_id = string.Empty,
		LocalDateTime = _E5AD.Now,
		UtcDateTime = _E5AD.UtcNow,
		Text = _chatInputField.text,
		Type = _E096.SelectedDialogue.Type,
		Member = _E096.PlayerMember,
		ReplyTo = _E096.ReplyToMessage
	};

	private void Awake()
	{
		_captionPanel.AddComponent<UIDragComponent>().Init(base.RectTransform, putOnTop: true);
		_closeButton.onClick.AddListener(Close);
		_createGroupDialog.onClick.AddListener(delegate
		{
			if (_chatCreateDialoguePanel.gameObject.activeSelf)
			{
				_chatCreateDialoguePanel.Close();
			}
			else
			{
				_chatCreateDialoguePanel.Show(_E096);
			}
		});
		_friendsButton.onClick.AddListener(delegate
		{
			if (_chatFriendsPanel.gameObject.activeSelf)
			{
				_chatFriendsPanel.Close();
			}
			else
			{
				_chatFriendsPanel.Show(_E096);
			}
		});
		_E79D.MuteChatSounds = _E762.GetBool(_ED3E._E000(230700));
		_switchableParamsButton.AddSubscription(delegate
		{
			_E79D.MuteChatSounds = !_E79D.MuteChatSounds;
			_E762.SetBool(_ED3E._E000(230700), _E79D.MuteChatSounds);
		}, _E79D.MuteChatSounds);
		_sendButton.OnClick.AddListener(_E007);
		if (MonoBehaviourSingleton<PreloaderUI>.Instantiated)
		{
			MonoBehaviourSingleton<PreloaderUI>.Instance.SetCanvasAsParent(this._E000);
		}
	}

	public void Show(_E79D social, _E72F profileInfo, Action<IEnumerable<_E464>, DateTime?> onTransferItems, Action<string> onOfferSelected)
	{
		ShowGameObject();
		CorrectPosition();
		_E096 = social;
		_E0B5 = profileInfo;
		_E0B6 = onTransferItems;
		_E0B7 = onOfferSelected;
		_E096.SetChatScreenOpenStatus(status: true);
		_dialoguesContainer.Show(_E096, _E001, _E003, _E005);
		UI.BindEvent(_E096.OnDialogueSelectedGlobal, _E002, _E096.SelectedDialogue);
		UI.BindEvent(_E096.FriendsList.ItemsChanged, _E009);
		UI.BindEvent(_E096.InputFriendsInvitations.ItemsChanged, _E009);
		UI.BindEvent(_E096.OutputFriendsInvitations.ItemsChanged, _E009);
		_E0B5.OnBanChanged += _E000;
		_E000();
		UI.AddDisposable(delegate
		{
			_E0B5.OnBanChanged -= _E000;
		});
		SetQuotedMessage(null);
	}

	private void _E000(EBanType banType = EBanType.Friends)
	{
		if (banType == EBanType.Friends)
		{
			if (_E0B5.GetBan(EBanType.Friends) != null)
			{
				_friendsBlocker.StartBlock(_ED3E._E000(140972));
			}
			else
			{
				_friendsBlocker.RemoveBlock();
			}
		}
	}

	private void Update()
	{
		bool flag = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
		if (Input.GetKeyDown(KeyCode.Return) && !flag && _chatInputField.text.Length > 0)
		{
			_chatInputField.text = _chatInputField.text.Remove(_chatInputField.text.Length - 1);
			_E007();
		}
		_E006();
	}

	private void _E001(_E466 dialog)
	{
		_E096.AllAttachmentsFromDialog(dialog._id, _E004);
	}

	private void _E002(_E466 dialog)
	{
		_quoteGameObject.SetActive(value: false);
		_quoteLabel.text = string.Empty;
		if (dialog != null)
		{
			return;
		}
		foreach (KeyValuePair<_E466, MessagesContainer> item in _E0BB)
		{
			item.Value.HideGameObject();
		}
		_inputPanel.CloseMessageSendBlock(null);
		_chatMembersPanel.Close();
	}

	private void _E003(_E466 dialogue, DialogueView dialogueView)
	{
		_E008();
		foreach (KeyValuePair<_E466, MessagesContainer> item in _E0BB)
		{
			item.Value.HideGameObject();
		}
		if (_E0BB.TryGetValue(dialogue, out var value))
		{
			value.Display();
		}
		else
		{
			MessagesContainer messagesContainer = UnityEngine.Object.Instantiate(_messagesContainerTemplate, _messagesContainerParent);
			messagesContainer.transform.SetAsFirstSibling();
			messagesContainer.Show(_E096, this, dialogue, delegate(_E464 chatMessage)
			{
				_E004(new _E464[1] { chatMessage });
			}, delegate(string arg)
			{
				Close();
				_E0B7(arg);
			});
			_E0BB.Add(dialogue, messagesContainer);
		}
		_E096.UpdateReadMessages();
	}

	private void _E004(_E464[] messages)
	{
		_E000 CS_0024_003C_003E8__locals0 = new _E000();
		CS_0024_003C_003E8__locals0._003C_003E4__this = this;
		CS_0024_003C_003E8__locals0.messages = messages;
		_E464[] messages2 = CS_0024_003C_003E8__locals0.messages;
		if (messages2 == null || !messages2.Any())
		{
			return;
		}
		CS_0024_003C_003E8__locals0.messagesWithItems = new List<_E464>();
		List<ProfileChangeEvent> list = new List<ProfileChangeEvent>();
		CS_0024_003C_003E8__locals0.expirationTime = DateTime.MaxValue;
		bool flag = CS_0024_003C_003E8__locals0.messages.Length == 1;
		_E464[] messages3 = CS_0024_003C_003E8__locals0.messages;
		foreach (_E464 obj in messages3)
		{
			if (obj.AnyItems)
			{
				CS_0024_003C_003E8__locals0.messagesWithItems.Add(obj);
			}
			ProfileChangeEvent[] profileChangeEvents = obj.ProfileChangeEvents;
			IEnumerable<ProfileChangeEvent> collection;
			if (!flag)
			{
				collection = profileChangeEvents.Where((ProfileChangeEvent evt) => !evt.Redeemed);
			}
			else
			{
				IEnumerable<ProfileChangeEvent> enumerable = profileChangeEvents;
				collection = enumerable;
			}
			list.AddRange(collection);
			CS_0024_003C_003E8__locals0.expirationTime = new DateTime(Math.Min(CS_0024_003C_003E8__locals0.expirationTime.Ticks, obj.ExpirationTime.Ticks));
		}
		if (list.Any((ProfileChangeEvent evt) => !evt.Redeemed))
		{
			_profileEventsWindow.Show(list, delegate(IEnumerable<ProfileChangeEvent> events)
			{
				CS_0024_003C_003E8__locals0._003C_003E4__this._E096.RedeemProfileRewards(events).HandleExceptions();
				events.ForEach(delegate(ProfileChangeEvent evt)
				{
					evt.Redeem();
				});
				CS_0024_003C_003E8__locals0.messages.ForEach(delegate(_E464 message)
				{
					message.UpdateRewardStatus?.Invoke();
				});
			}).OnClose += delegate
			{
				if (CS_0024_003C_003E8__locals0.messagesWithItems.Any())
				{
					CS_0024_003C_003E8__locals0._003C_003E4__this._E0B6(CS_0024_003C_003E8__locals0.messagesWithItems, CS_0024_003C_003E8__locals0.expirationTime);
				}
			};
		}
		else
		{
			CS_0024_003C_003E8__locals0._E001();
		}
	}

	private void _E005(_E466 dialogue)
	{
		if (_E0BB.TryGetValue(dialogue, out var value))
		{
			_E0BB.Remove(dialogue);
			value.Close();
			UnityEngine.Object.DestroyImmediate(value.gameObject);
		}
	}

	public void SetQuotedMessage([CanBeNull] _E464 message)
	{
		_E096.SetReplyToMessage(message);
		_quoteGameObject.SetActive(message != null);
		if (message != null && message.Type == EMessageType.SystemMessage)
		{
			_quoteLabel.text = ((message.Params != null) ? (_ED3E._E000(227668) + string.Format(message.Params.Action.ToString().Localized(), message.Params.Member.LocalizedNickname).SubstringIfNecessary(70)) : string.Empty);
		}
		else
		{
			_quoteLabel.text = ((message != null) ? (_ED3E._E000(227668) + message.Member.LocalizedNickname + _ED3E._E000(12201) + message.Text.SubstringIfNecessary(70)) : string.Empty);
		}
		if (message != null)
		{
			_E008();
		}
	}

	private void _E006()
	{
		if (_E096 != null)
		{
			DateTime utcNow = _E5AD.UtcNow;
			DateTime dateTime = _E0B8.AddSeconds(80.0);
			if (!(utcNow < dateTime) && !(utcNow == _E0B8))
			{
				_E096.ReadDialogues();
				_E0B8 = utcNow;
			}
		}
	}

	private void _E007()
	{
		if (!string.IsNullOrEmpty(_chatInputField.text) && _E096.SelectedDialogue != null)
		{
			_E096.SendMessage(_E096.SelectedDialogue, _E096.SelectedDialogue.Type, this._E001, delegate
			{
			});
			SetQuotedMessage(null);
			_chatInputField.text = string.Empty;
			_E008();
		}
	}

	private void _E008()
	{
		_chatInputField.ActivateInputField();
		_chatInputField.Select();
	}

	private void _E009()
	{
		int count = _E096.InputFriendsInvitations.Count;
		if (_E0B9 != _E096.FriendsList.Count || _E0BA != count)
		{
			_E0B9 = _E096.FriendsList.Count;
			_E0BA = count;
			_friendsButtonLabel.text = ((count <= 0) ? (_ED3E._E000(230691).Localized(EStringCase.Upper) + _ED3E._E000(12201) + _E0B9) : (_ED3E._E000(230691).Localized(EStringCase.Upper) + _ED3E._E000(12201) + _E0B9 + _ED3E._E000(230747) + count + _ED3E._E000(163322)));
		}
	}

	public void OnPointerClick([NotNull] PointerEventData eventData)
	{
		_E00A();
	}

	private void _E00A()
	{
		base.transform.SetAsLastSibling();
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (command.IsCommand(ECommand.Escape))
		{
			if (_E096.ReplyToMessage != null)
			{
				SetQuotedMessage(null);
			}
			else if (_chatCreateDialoguePanel.gameObject.activeSelf)
			{
				_chatCreateDialoguePanel.Close();
			}
			else if (_chatFriendsPanel.gameObject.activeSelf)
			{
				_chatFriendsPanel.Close();
			}
			else
			{
				Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuEscape);
				Close();
			}
			return ETranslateResult.BlockAll;
		}
		return InputNode.GetDefaultBlockResult(command);
	}

	public override void Close()
	{
		_dialoguesContainer.Close();
		_messagesContainerTemplate.Close();
		_chatCreateDialoguePanel.Close();
		_chatFriendsPanel.Close();
		if (_E096 != null)
		{
			_E096.ReadDialogues();
			_E096.SetChatScreenOpenStatus(status: false);
		}
		MonoBehaviourSingleton<PreloaderUI>.Instance.MenuTaskBar.ChatToggle.isOn = false;
		base.Close();
	}

	[CompilerGenerated]
	private void _E00B()
	{
		if (_chatCreateDialoguePanel.gameObject.activeSelf)
		{
			_chatCreateDialoguePanel.Close();
		}
		else
		{
			_chatCreateDialoguePanel.Show(_E096);
		}
	}

	[CompilerGenerated]
	private void _E00C()
	{
		if (_chatFriendsPanel.gameObject.activeSelf)
		{
			_chatFriendsPanel.Close();
		}
		else
		{
			_chatFriendsPanel.Show(_E096);
		}
	}

	[CompilerGenerated]
	private void _E00D()
	{
		_E0B5.OnBanChanged -= _E000;
	}

	[CompilerGenerated]
	private void _E00E(_E464 chatMessage)
	{
		_E004(new _E464[1] { chatMessage });
	}

	[CompilerGenerated]
	private void _E00F(string arg)
	{
		Close();
		_E0B7(arg);
	}
}
