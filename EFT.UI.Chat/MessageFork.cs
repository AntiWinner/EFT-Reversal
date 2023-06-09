using System;
using ChatShared;
using UnityEngine;

namespace EFT.UI.Chat;

public class MessageFork : UIElement
{
	[SerializeField]
	private MessageView _yourChatMessage;

	[SerializeField]
	private MessageView _opponentChatMessage;

	[SerializeField]
	private MessageView _systemMessage;

	[SerializeField]
	private AttachmentMessageView _attachmentMessage;

	public void Show(_ECA6 data)
	{
		_E464 message = data.Message;
		_E79D socialNetwork = data.SocialNetwork;
		EMessageType dialogueType = data.DialogueType;
		EMessageViewType messageType = data.MessageType;
		UpdatableChatMember member = data.Member;
		_E465 quote = data.Quote;
		Action<_E464> itemsTransfer = data.ItemsTransfer;
		Action<_E464, Vector2> contextMenuClicked = data.ContextMenuClicked;
		Action<_E464> messageDoubleClicked = data.MessageDoubleClicked;
		Action<string> offerSelected = data.OfferSelected;
		ShowGameObject();
		switch (messageType)
		{
		case EMessageViewType.YourMessage:
			_yourChatMessage.Show(message, member, quote, contextMenuClicked, messageDoubleClicked, offerSelected);
			break;
		case EMessageViewType.OpponentMessage:
			_opponentChatMessage.Show(message, member, quote, contextMenuClicked, messageDoubleClicked, offerSelected);
			break;
		case EMessageViewType.SystemMessage:
			if (dialogueType == EMessageType.SystemMessage || message.Params == null)
			{
				_attachmentMessage.Show(message.ToSystemMessage(), socialNetwork.SystemMember, quote, contextMenuClicked, messageDoubleClicked, itemsTransfer, offerSelected);
			}
			else
			{
				_systemMessage.Show(message.ToSystemMessageWithParams(), socialNetwork.SystemMember, quote, contextMenuClicked, messageDoubleClicked, offerSelected);
			}
			break;
		case EMessageViewType.TraderMessage:
			_attachmentMessage.Show(message, member, quote, contextMenuClicked, messageDoubleClicked, itemsTransfer, offerSelected);
			break;
		default:
			throw new ArgumentOutOfRangeException(messageType.ToString(), messageType, null);
		}
	}

	public override void Close()
	{
		_systemMessage.Close();
		_attachmentMessage.Close();
		_yourChatMessage.Close();
		_opponentChatMessage.Close();
		base.Close();
	}
}
