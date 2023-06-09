using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ChatShared;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Chat;

public sealed class ChatMessageSendBlock : UIElement
{
	[SerializeField]
	private DefaultUIButton _receiveAllButton;

	[SerializeField]
	private Image _backgroundImage;

	[SerializeField]
	private GameObject _enterMessagePanel;

	[SerializeField]
	private GameObject _messageSendBlockPanel;

	[SerializeField]
	private CustomTextMeshProUGUI _messageBlockLabel;

	private _E466 _E2CC;

	private Action _E2C1;

	private float _E2CD;

	private _E79D _E105;

	private void Awake()
	{
		_receiveAllButton.OnClick.AddListener(delegate
		{
			_E2C1?.Invoke();
		});
		_E001();
	}

	public void Show(_E79D social, _E466 dialogue, Action onTransferAll)
	{
		UI.Dispose();
		_E105 = social;
		_E2CC = dialogue;
		_E2CD = 0f;
		UI.AddDisposable(social.OnLastMessageUpdatedGlobal.Subscribe(_E004));
		if (social.SelectedDialogue == _E2CC)
		{
			if (_E2CC.Type == EMessageType.GlobalChat)
			{
				_messageSendBlockPanel.SetActive(value: false);
				_enterMessagePanel.SetActive(value: true);
				return;
			}
			_E2C1 = onTransferAll;
			UI.AddDisposable(_E2CC.ChatMessages.ItemsChanged.Bind(_E002));
			UI.AddDisposable(_E2CC.OnDialogueAttachmentsChanged.Subscribe(_E002));
			_E003();
			_E2CC.Profile.OnIgnoreStatusChanged.Bind(_E003);
		}
	}

	public void CloseMessageSendBlock(_E466 dialogue)
	{
		if (dialogue == null || _E2CC == dialogue)
		{
			_messageSendBlockPanel.SetActive(value: false);
			_enterMessagePanel.SetActive(value: false);
			_E001();
		}
	}

	private void _E000()
	{
		base.enabled = true;
		_backgroundImage.enabled = true;
	}

	private void _E001()
	{
		base.enabled = false;
		_backgroundImage.enabled = false;
	}

	private void _E002()
	{
		bool anyRewardMessage = _E2CC.AnyRewardMessage;
		_receiveAllButton.gameObject.SetActive(anyRewardMessage);
		if (anyRewardMessage)
		{
			_E464 message2 = (from message in _E2CC.ChatMessages
				where message.DisplayRewardStatus
				orderby message.UtcDateTime.AddSeconds(message.MaxStorageTime) descending
				select message).FirstOrDefault();
			_E005(message2);
			_E000();
		}
		else
		{
			_E001();
		}
	}

	private void _E003()
	{
		string error;
		bool flag = _E2CC.CanSendMessage(_E105, out error);
		_messageSendBlockPanel.SetActive(!flag);
		_enterMessagePanel.SetActive(flag);
		if (!flag)
		{
			_messageBlockLabel.text = error.Localized();
		}
	}

	private void _E004(KeyValuePair<_E466, _E464> dialogue)
	{
		var (obj3, obj4) = dialogue;
		if (obj3 == _E2CC && obj4.DisplayRewardStatus)
		{
			_E005(obj4);
			if (!base.enabled)
			{
				_E000();
			}
			if (!_receiveAllButton.gameObject.activeSelf)
			{
				_receiveAllButton.gameObject.SetActive(value: true);
			}
		}
	}

	private void _E005([CanBeNull] _E464 message)
	{
		float num = Time.time + 1f;
		if (message != null)
		{
			float num2 = (float)(message.UtcDateTime.AddSeconds(message.MaxStorageTime) - _E5AD.UtcNow).TotalSeconds + 1f;
			num = Time.time + num2;
		}
		if (num > _E2CD)
		{
			_E2CD = num;
		}
	}

	private void Update()
	{
		if (Time.time < _E2CD)
		{
			return;
		}
		if (_E2CC == null || !_E2CC.AnyRewardMessage)
		{
			_E001();
			if (_receiveAllButton != null)
			{
				_receiveAllButton.gameObject.SetActive(value: false);
			}
		}
		else
		{
			_E2CD = Time.time + 1f;
		}
	}

	[CompilerGenerated]
	private void _E006()
	{
		_E2C1?.Invoke();
	}
}
