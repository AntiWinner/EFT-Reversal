using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ChatShared;
using Comfort.Common;
using EFT.UI.Utilities.LightScroller;
using UnityEngine;

namespace EFT.UI.Chat;

public sealed class MessagesContainer : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _E5CB.TraderSettings traderSettings;

		internal UpdatableChatMember _E000(string id)
		{
			UpdatableChatMember updatableChatMember = new UpdatableChatMember(id);
			updatableChatMember.UpdateFromTrader(traderSettings);
			return updatableChatMember;
		}
	}

	[SerializeField]
	private LightScroller _scroller;

	[SerializeField]
	private MessageFork _cellViewPrefab;

	[SerializeField]
	private GameObject _loader;

	[SerializeField]
	private SimpleContextMenu _contextMenu;

	[SerializeField]
	private BanTimeWindow _banTimeWindow;

	[SerializeField]
	private ChatMembersPanel _chatMembersPanel;

	private _E79D _E2C7;

	private ChatScreen _E2EC;

	private _E466 _E2CC;

	private _ED01<_E464, _ECA6> _E2ED;

	private IEnumerable<_E464> _E2EE;

	private Action<string> _E2E7;

	private Action<_E464> _E2C2;

	public override void Display()
	{
		base.Display();
		_E000();
	}

	public void Show(_E79D social, ChatScreen screen, _E466 dialogue, Action<_E464> onItemsTransferClicked, Action<string> onOfferSelected)
	{
		UI.Dispose();
		_E2C7 = social;
		_E2EC = screen;
		_E2CC = dialogue;
		_E2C2 = onItemsTransferClicked;
		_E2E7 = onOfferSelected;
		_E2ED = new _ED01<_E464, _ECA6>(dialogue.ChatMessages, _E001, _ECA6.Comparer);
		if (_E2CC.Updating)
		{
			_loader.SetActive(value: true);
		}
		UI.AddDisposable(dialogue.OnLastMessageUpdated.Subscribe(delegate
		{
			_E004();
		}));
		UI.SubscribeEvent(dialogue.OnDialogueLoadingStatus, delegate(bool isLoading)
		{
			_loader.SetActive(isLoading);
		});
		UI.AddDisposable(_scroller);
		UI.AddDisposable(_E2ED);
		_scroller.Show(_E2ED, (_ECA6 messageData) => _cellViewPrefab, (_ECA6 messageData) => EMessageType.UserMessage, delegate(_ECA6 messageData, MessageFork view)
		{
			view.Show(messageData);
		});
		_E004();
		Display();
	}

	private void _E000()
	{
		if (_E2CC.IsMultiUsersDialog)
		{
			if (_chatMembersPanel.gameObject.activeSelf)
			{
				_chatMembersPanel.Close();
			}
			_chatMembersPanel.Show(_E2C7, _E2CC.ActiveUsers);
		}
		else
		{
			_chatMembersPanel.Close();
		}
	}

	private bool _E001(_E464 info, out _ECA6 messageData)
	{
		_E465 quote = null;
		if (info.ReplyTo != null)
		{
			quote = ((info.ReplyTo.Type == EMessageType.SystemMessage) ? new _E465(string.Empty, info.ReplyTo.SystemText.SubstringIfNecessary(50)) : new _E465((info.ReplyTo.Member != null) ? info.ReplyTo.Member.LocalizedNickname : string.Empty, info.ReplyTo.Text.SubstringIfNecessary(50)));
		}
		EMessageViewType messageType = EMessageViewType.OpponentMessage;
		if (info.Member.Id == _E2C7.PlayerMember.Id)
		{
			messageType = EMessageViewType.YourMessage;
		}
		else if (info.Type == EMessageType.SystemMessage && (info.Params != null || info.Type == EMessageType.SystemMessage))
		{
			messageType = EMessageViewType.SystemMessage;
		}
		else if (info.Type == EMessageType.InsuranceReturn || info.Type == EMessageType.QuestFail || info.Type == EMessageType.QuestStart || info.Type == EMessageType.QuestSuccess || info.Type == EMessageType.MessageWithItems || info.Type == EMessageType.FleamarketMessage || info.Type == EMessageType.InitialSupport)
		{
			messageType = EMessageViewType.TraderMessage;
		}
		messageData = new _ECA6
		{
			LocalDateTime = info.LocalDateTime,
			Message = info,
			SocialNetwork = _E2C7,
			DialogueType = _E2CC.Type,
			MessageType = messageType,
			Member = _E002(info),
			Quote = quote,
			ItemsTransfer = _E2C2,
			ContextMenuClicked = _E003,
			MessageDoubleClicked = _E2EC.SetQuotedMessage,
			OfferSelected = _E2E7
		};
		return true;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.PageUp))
		{
			_scroller.SetScrollPosition(1f);
		}
		if (Input.GetKeyDown(KeyCode.PageDown))
		{
			_E004();
		}
		if (_E2CC != null && _E2ED.Any() && _E2C7?.SelectedDialogue == _E2CC && !_E2CC.FullyRead && !_E2CC.Updating && _scroller.NormalizedScrollPosition > 0.99f)
		{
			_ECA6 obj = _E2ED.Last();
			if (obj == null)
			{
				Debug.LogError(_ED3E._E000(230974));
			}
			else
			{
				_E2C7.UpdateDialogMessages(_E2CC, obj.Message.UtcDateTime.ToUnixTime());
			}
		}
	}

	private UpdatableChatMember _E002(_E464 message)
	{
		switch (message.Type)
		{
		case EMessageType.NpcTraderMessage:
		case EMessageType.FleamarketMessage:
		{
			if (Singleton<_E5CB>.Instance.TradersSettings.TryGetValue(message.Member.Id, out var traderSettings))
			{
				return UpdatableChatMember.FindOrCreate(message.Member.Id, delegate(string id)
				{
					UpdatableChatMember updatableChatMember = new UpdatableChatMember(id);
					updatableChatMember.UpdateFromTrader(traderSettings);
					return updatableChatMember;
				});
			}
			return _E2C7.SystemMember;
		}
		case EMessageType.SystemMessage:
			return _E2C7.SystemMember;
		default:
			return message.Member;
		}
	}

	private void _E003(_E464 message, Vector2 pos)
	{
		_contextMenu.Show(pos, new _ECA9(_E2EC, _E2C7, _banTimeWindow, _E2CC, message));
	}

	private void _E004()
	{
		_scroller.SetScrollPosition(0f);
	}

	public override void Close()
	{
		base.Close();
		_chatMembersPanel.Close();
	}

	[CompilerGenerated]
	private void _E005(_E464 message)
	{
		_E004();
	}

	[CompilerGenerated]
	private void _E006(bool isLoading)
	{
		_loader.SetActive(isLoading);
	}

	[CompilerGenerated]
	private MessageFork _E007(_ECA6 messageData)
	{
		return _cellViewPrefab;
	}
}
