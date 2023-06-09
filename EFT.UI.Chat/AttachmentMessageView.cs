using System;
using System.Runtime.CompilerServices;
using ChatShared;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Chat;

public sealed class AttachmentMessageView : MessageView
{
	[SerializeField]
	private DefaultUIButton _transferButtonSpawner;

	[SerializeField]
	private LocalizedText _receivedLabel;

	[SerializeField]
	private LocalizedText _outOfTimeLabel;

	[SerializeField]
	private TextMeshProUGUI _timeToGetLabel;

	[SerializeField]
	private Image _messageIcon;

	[SerializeField]
	private Sprite _insuranceIcon;

	[SerializeField]
	private Sprite _questIcon;

	[SerializeField]
	private Sprite _systemIcon;

	[SerializeField]
	private TextMeshProUGUI _messageDescriptionLabel;

	[SerializeField]
	private Color _insuranceTextColor;

	[SerializeField]
	private Color _questTextColor;

	[SerializeField]
	private Color _systemTextColor;

	[SerializeField]
	private Image _messageBackground;

	[SerializeField]
	private GameObject _insuranceLimiter;

	[SerializeField]
	private GameObject _messageAttachmentsObject;

	[SerializeField]
	private Color _insuranceColor;

	[SerializeField]
	private Color _questColor;

	[SerializeField]
	private Color _systemColor;

	private Action _E2C1;

	private Action<_E464> _E2C2;

	private DateTime _E2C3;

	private TimeSpan _E2C4;

	private long _E2C5;

	private DateTime _E000 => DialogueChatMessage.UtcDateTime.Add(_E2C4);

	private bool _E001 => this._E000 > _E5AD.UtcNow;

	private bool _E002 => DialogueChatMessage.DisplayRewardStatus;

	private void Awake()
	{
		_transferButtonSpawner.OnClick.AddListener(delegate
		{
			_E2C2?.Invoke(DialogueChatMessage);
		});
	}

	public void Show(_E464 chatMessage, UpdatableChatMember member, [CanBeNull] _E465 quote, Action<_E464, Vector2> onMessageClicked, Action<_E464> onMessageDoubleClicked, Action<_E464> onItemsTransfer, Action<string> onOfferSelected)
	{
		UI.Dispose();
		Show(chatMessage, member, quote, onMessageClicked, onMessageDoubleClicked, onOfferSelected);
		UI.AddDisposable(_E7AD._E010.AddLocaleUpdateListener(_E003));
		_E2C2 = onItemsTransfer;
		_E2C3 = DateTime.MinValue;
		if (!(_messageAttachmentsObject == null))
		{
			_messageAttachmentsObject.SetActive(DialogueChatMessage.HasRewards);
			_insuranceLimiter.SetActive(DialogueChatMessage.HasRewards);
			if (chatMessage.HasRewards)
			{
				_E2C5 = chatMessage.MaxStorageTime;
				_E2C4 = TimeSpan.FromSeconds((_E2C5 > 0) ? _E2C5 : int.MaxValue);
				UI.AddDisposable(DialogueChatMessage.UpdateRewardStatus.Bind(_E002));
				_E000(chatMessage.Type);
			}
		}
	}

	private void _E000(EMessageType type)
	{
		switch (type)
		{
		case EMessageType.InsuranceReturn:
			_messageBackground.color = _insuranceColor;
			_messageDescriptionLabel.color = _insuranceTextColor;
			_messageIcon.sprite = _insuranceIcon;
			break;
		case EMessageType.FleamarketMessage:
		case EMessageType.SystemMessage:
		case EMessageType.MessageWithItems:
		case EMessageType.InitialSupport:
			_messageBackground.color = _systemColor;
			_messageDescriptionLabel.color = _systemTextColor;
			_messageIcon.sprite = _systemIcon;
			break;
		case EMessageType.QuestStart:
		case EMessageType.QuestFail:
		case EMessageType.QuestSuccess:
			_messageBackground.color = _questColor;
			_messageDescriptionLabel.color = _questTextColor;
			_messageIcon.sprite = _questIcon;
			break;
		default:
			throw new ArgumentOutOfRangeException(_ED3E._E000(124643), type, null);
		}
		_messageIcon.SetNativeSize();
		_E001(type);
	}

	private void _E001(EMessageType type)
	{
		if (!(_messageDescriptionLabel == null))
		{
			switch (type)
			{
			case EMessageType.InsuranceReturn:
				_messageDescriptionLabel.text = _ED3E._E000(230445).Localized();
				break;
			case EMessageType.QuestStart:
				_messageDescriptionLabel.text = _ED3E._E000(230494).Localized();
				break;
			case EMessageType.QuestFail:
				_messageDescriptionLabel.text = _ED3E._E000(230482).Localized();
				break;
			case EMessageType.QuestSuccess:
				_messageDescriptionLabel.text = _ED3E._E000(230469).Localized();
				break;
			case EMessageType.FleamarketMessage:
			case EMessageType.SystemMessage:
			case EMessageType.MessageWithItems:
			case EMessageType.InitialSupport:
				_messageDescriptionLabel.text = _ED3E._E000(255046).Localized();
				break;
			default:
				throw new ArgumentOutOfRangeException(_ED3E._E000(124643), type, null);
			}
		}
	}

	private void Update()
	{
		if (DialogueChatMessage == null)
		{
			return;
		}
		DateTime utcNow = _E5AD.UtcNow;
		DateTime dateTime = _E2C3.AddSeconds(1.0);
		if (!(utcNow < dateTime) && !(utcNow == _E2C3) && _E2C5 != 0L)
		{
			_E2C3 = utcNow;
			if (this._E001)
			{
				TimeSpan timeSpan = this._E000 - utcNow;
				_timeToGetLabel.text = ((timeSpan.Days <= 0) ? string.Format(_ED3E._E000(230553), timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds) : string.Format(_ED3E._E000(230523), timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds));
			}
			_E002();
		}
	}

	private void OnEnable()
	{
		if (DialogueChatMessage != null)
		{
			_E002();
		}
	}

	private void _E002()
	{
		_timeToGetLabel.gameObject.SetActive(this._E001 && this._E002 && _E2C5 > 0);
		bool flag = !this._E001 && _E2C5 > 0;
		_outOfTimeLabel.gameObject.SetActive(flag);
		_receivedLabel.gameObject.SetActive(!this._E002 && !flag);
		_transferButtonSpawner.gameObject.SetActive(this._E002 && this._E001);
	}

	private void _E003()
	{
		SetSenderName();
		SetSenderMessage(DialogueChatMessage);
		_E001(DialogueChatMessage.Type);
	}

	public override void UpdateMessageStatus()
	{
		if (_messageAttachmentsObject != null)
		{
			_E002();
		}
	}

	[CompilerGenerated]
	private void _E004()
	{
		_E2C2?.Invoke(DialogueChatMessage);
	}
}
