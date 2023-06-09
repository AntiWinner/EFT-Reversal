using System;
using System.Linq;
using ChatShared;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI.Chat;

public class MessageView : UIElement, IPointerClickHandler, IEventSystemHandler
{
	[SerializeField]
	private TextMeshProUGUI _senderName;

	[SerializeField]
	private ClickableUIText _senderMessage;

	[SerializeField]
	private TextMeshProUGUI _timestampLabel;

	[SerializeField]
	private GameObject _quoteGameObject;

	[SerializeField]
	private TextMeshProUGUI _quotedName;

	[SerializeField]
	private TextMeshProUGUI _quotedMessage;

	[SerializeField]
	private ChatSpecialIcon _specialIcon;

	protected _E464 DialogueChatMessage;

	private UpdatableChatMember _E2E4;

	private Action<_E464, Vector2> _E2E5;

	private Action<_E464> _E2E6;

	private Action<string> _E2E7;

	private const float _E2E8 = 0.3f;

	private float _E2E9;

	private bool _E2EA;

	private static readonly EMessageType[] _E2EB = new EMessageType[9]
	{
		EMessageType.AuctionMessage,
		EMessageType.FleamarketMessage,
		EMessageType.InsuranceReturn,
		EMessageType.NpcTraderMessage,
		EMessageType.QuestFail,
		EMessageType.QuestStart,
		EMessageType.QuestSuccess,
		EMessageType.MessageWithItems,
		EMessageType.InitialSupport
	};

	public void Show(_E464 chatMessage, UpdatableChatMember chatMember, [CanBeNull] _E465 quote, Action<_E464, Vector2> onMessageClicked, Action<_E464> onMessageDoubleClicked, Action<string> onOfferSelected)
	{
		ShowGameObject();
		DialogueChatMessage = chatMessage;
		_E2E4 = chatMember;
		_E2E5 = onMessageClicked;
		_E2E6 = onMessageDoubleClicked;
		_E2E7 = onOfferSelected;
		if (_quoteGameObject != null)
		{
			_quoteGameObject.SetActive(quote != null);
			if (quote != null)
			{
				_quotedName.gameObject.SetActive(!string.IsNullOrEmpty(quote.QuotedNickname));
				_quotedName.text = quote.QuotedNickname;
				_quotedMessage.text = quote.QuotedText;
			}
		}
		SetSenderName();
		SetSenderMessage(chatMessage);
		DateTime localDateTime = chatMessage.LocalDateTime;
		_timestampLabel.text = localDateTime.ToString(((_E5AD.Now.ToLocalTime() - localDateTime).TotalDays < 1.0) ? _ED3E._E000(230912) : _ED3E._E000(230753));
		if (_specialIcon != null && chatMember.Info != null)
		{
			_specialIcon.Show(chatMember.Info.MemberCategory);
		}
	}

	protected void SetSenderName()
	{
		if (!(_senderName == null))
		{
			if (_E2EB.Contains(DialogueChatMessage.Type))
			{
				_senderName.text = (_E2E4.Id + _ED3E._E000(114050)).Localized();
			}
			else if (_E2E4.Info != null)
			{
				string text = ((_E2E4.Info.Level > 0) ? (_ED3E._E000(253257) + _E2E4.Info.Level + _ED3E._E000(11164)) : string.Empty);
				_senderName.text = _E2E4.LocalizedNickname + text;
				_senderName.gameObject.SetActive(_E2E4.HasNickname);
			}
		}
	}

	protected void SetSenderMessage(_E464 message)
	{
		if (!(_senderMessage == null))
		{
			_senderMessage.SetText(message.ParsedText(), OnPointerClick, _E2E7);
		}
	}

	public virtual void UpdateMessageStatus()
	{
	}

	private void Update()
	{
		if (_E2EA)
		{
			_E2E9 += Time.deltaTime;
		}
		if (_E2E9 >= 0.3f)
		{
			_E2E9 = 0f;
			_E2EA = false;
		}
	}

	public void OnPointerClick([NotNull] PointerEventData eventData)
	{
		if (DialogueChatMessage != null)
		{
			if (eventData.button == PointerEventData.InputButton.Right)
			{
				_E2E5(DialogueChatMessage, eventData.position);
			}
			if (_E2EA && _E2E9 < 0.3f)
			{
				_E2E6(DialogueChatMessage);
			}
			if (eventData.button == PointerEventData.InputButton.Left && !_E2EA)
			{
				_E2EA = true;
			}
		}
	}

	public override void Close()
	{
		base.Close();
		DialogueChatMessage = null;
	}
}
