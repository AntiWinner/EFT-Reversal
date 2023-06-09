using System;
using System.Linq;
using System.Runtime.CompilerServices;
using ChatShared;
using Comfort.Common;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.Chat;

public sealed class DialogueView : UIElement, IPointerExitHandler, IEventSystemHandler, IPointerEnterHandler, IPointerClickHandler
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
	private TextMeshProUGUI _playerNameLabel;

	[SerializeField]
	private TextMeshProUGUI _lastMessageLabel;

	[SerializeField]
	private TextMeshProUGUI _timeStamp;

	[SerializeField]
	private GameObject _newMessagesObject;

	[SerializeField]
	private TextMeshProUGUI _newMessagesCount;

	[SerializeField]
	private GameObject _newAttachmentsMessagesObject;

	[SerializeField]
	private TextMeshProUGUI _newAttachmentsMessagesCount;

	[SerializeField]
	private ChatSpecialIcon _specialIcon;

	[SerializeField]
	private Toggle _pinButton;

	[SerializeField]
	private Image _pinImage;

	[SerializeField]
	private Sprite _pinnedSprite;

	[SerializeField]
	private Sprite _unpinnedSprite;

	[SerializeField]
	private Button _settingsButton;

	[SerializeField]
	private Image _backgroundImage;

	[SerializeField]
	private Toggle _dialogueToggle;

	[SerializeField]
	private Image _dialogueIcon;

	[SerializeField]
	private Sprite _singleDialogueSprite;

	[SerializeField]
	private Sprite _groupDialogueSprite;

	[SerializeField]
	private Sprite _systemDialogueSprite;

	[SerializeField]
	private Color _defaultImageColor;

	[SerializeField]
	private Color _hoverImageColor;

	[SerializeField]
	private Color _selectedImageColor;

	[SerializeField]
	private Color _defaultHeaderColor;

	[SerializeField]
	private Color _selectedHeaderColor;

	[SerializeField]
	private Color _defaultTimestampColor;

	[SerializeField]
	private Color _defaultMessageColor;

	[SerializeField]
	private Color _selectedMessageColor;

	private Action<bool, _E466> _E2CE;

	private Action<_E466> _E2C1;

	private Action<_E466, Vector2> _E2CF;

	private Action<bool, _E466, DialogueView> _E2D0;

	private Action<_E466> _E2D1;

	private Action<Vector2> _E2D2;

	private bool _E2D3;

	private _E79D _E2C7;

	private _E466 _E2CC;

	private UpdatableChatMember _E2D4;

	private ChatMessageSendBlock _E2D5;

	private CustomTextMeshProUGUI _E2D6;

	private int _E000
	{
		set
		{
			_E2CC.New = value;
			_newMessagesObject.SetActive(_E2CC.New > 0);
			_newMessagesCount.text = _E2CC.New.SubstringIfNecessary();
		}
	}

	private int _E001
	{
		set
		{
			_E2CC.AttachmentsNew = value;
			_newAttachmentsMessagesObject.SetActive(_E2CC.AttachmentsNew > 0);
			_newAttachmentsMessagesCount.text = _E2CC.AttachmentsNew.SubstringIfNecessary();
		}
	}

	private void Awake()
	{
		_settingsButton.onClick.AddListener(delegate
		{
			_E2D2(_settingsButton.transform.position);
		});
	}

	public void Show(_ECA4 data, ToggleGroup toggleGroup)
	{
		UI.Dispose();
		_E00C();
		_E2C7 = data.SocialNetwork;
		_E2CC = data.Dialogue;
		_E2CE = data.OnPinned;
		_E2C1 = data.OnTransferAll;
		_E2D0 = data.OnDialogueSelected;
		_E2D1 = data.OnDialogueRemoved;
		_E2CF = data.OnContextMenuCalled;
		_E2D2 = data.OnChannelListCalled;
		_E2D5 = data.ChatMessageSendBlock;
		_E2D6 = data.UsersCountLabel;
		_E2D4 = _E2C7.PlayerMember;
		_dialogueToggle.group = toggleGroup;
		ShowGameObject();
		_settingsButton.gameObject.SetActive(_E2CC.Type == EMessageType.GlobalChat);
		_E464 message = _E2CC.Message ?? _E2CC.ChatMessages.Last();
		_E008(message);
		this._E000 = _E2CC.New;
		this._E001 = _E2CC.AttachmentsNew;
		UI.AddDisposable(_E79D.IgnoringYouList.ItemsChanged.Subscribe(_E006));
		UI.AddDisposable(_E2C7.FriendsList.ItemsChanged.Subscribe(_E006));
		UI.AddDisposable(_E2CC.OnDialogueSelected.Subscribe(_E009));
		UI.AddDisposable(_E2CC.OnDialogueRemoved.Subscribe(_E00A));
		if (_E2CC == _E2C7.SelectedDialogue)
		{
			if (_E2CC.MessagesLoaded)
			{
				_E009(value: true);
			}
			else
			{
				_E2C7.SetSelectedStatus(_E2CC, isSelected: true);
			}
		}
		UI.AddDisposable(_E2CC.OnLastMessageUpdated.Subscribe(_E007));
		UI.AddDisposable(_E2CC.ActiveUsers.ItemsChanged.Bind(_E000));
		UI.AddDisposable(_E7AD._E010.AddLocaleUpdateListener(_E005));
		_pinButton.Set(_E2CC.Pinned);
		_pinImage.sprite = (_E2CC.Pinned ? _pinnedSprite : _unpinnedSprite);
		_E002();
		UI.SubscribeEvent(_pinButton.onValueChanged, _E00B);
		UI.SubscribeEvent(_dialogueToggle.onValueChanged, _E001);
	}

	private void _E000()
	{
		if (_E2CC == _E2C7.SelectedDialogue && _E2CC.Type != EMessageType.UserMessage)
		{
			_E2D6.text = _E2CC.ActiveUsers.Count.ToString();
		}
	}

	private void _E001(bool selected)
	{
		if (selected)
		{
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.ChatSelect);
		}
		if (_E2C7.SelectedDialogue == _E2CC != selected)
		{
			_E2C7.SetSelectedStatus(_E2CC, selected);
		}
	}

	private void _E002()
	{
		_playerNameLabel.text = _E2C7.GetDialogueHeaderByType(_E2CC).SubstringIfNecessary(14);
		switch (_E2CC.Type)
		{
		case EMessageType.UserMessage:
			_dialogueIcon.sprite = _singleDialogueSprite;
			_specialIcon.Show(_E2CC.Profile?.Info.MemberCategory ?? EMemberCategory.Default);
			break;
		case EMessageType.NpcTraderMessage:
		case EMessageType.FleamarketMessage:
		{
			string id2 = _E2CC.Message.Member.Id;
			if (Singleton<_E5CB>.Instance.TradersSettings.TryGetValue(id2, out var traderSettings))
			{
				_E2CC.Profile = UpdatableChatMember.FindOrCreate(id2, delegate(string id)
				{
					UpdatableChatMember updatableChatMember = new UpdatableChatMember(id);
					updatableChatMember.UpdateFromTrader(traderSettings);
					return updatableChatMember;
				});
				_specialIcon.Show(_E2CC.Profile.Info.MemberCategory);
				traderSettings.GetAndAssignAvatar(_dialogueIcon, base.CancellationToken).HandleExceptions();
			}
			else
			{
				_dialogueIcon.sprite = _systemDialogueSprite;
				_specialIcon.Close();
			}
			break;
		}
		case EMessageType.GroupChatMessage:
			_dialogueIcon.sprite = _groupDialogueSprite;
			_specialIcon.Show(EMemberCategory.Group);
			break;
		case EMessageType.SystemMessage:
			_dialogueIcon.sprite = _systemDialogueSprite;
			_specialIcon.Close();
			break;
		case EMessageType.GlobalChat:
			_dialogueIcon.sprite = _groupDialogueSprite;
			_specialIcon.Show(EMemberCategory.Group);
			break;
		default:
			_specialIcon.Show(_E2CC.Profile.Info.MemberCategory);
			Debug.LogError(_ED3E._E000(230726) + _E2CC.Type);
			break;
		}
	}

	private void _E003(bool value)
	{
		_dialogueToggle.isOn = value;
	}

	private void _E004()
	{
		_E2D3 = false;
		_backgroundImage.color = _defaultImageColor;
		_playerNameLabel.color = _defaultHeaderColor;
		_lastMessageLabel.color = _defaultMessageColor;
		_timeStamp.color = _defaultTimestampColor;
		_E2D5.CloseMessageSendBlock(_E2CC);
	}

	private void _E005()
	{
		_E008(_E2CC.Message);
	}

	private void _E006()
	{
		_E2D5.Show(_E2C7, _E2CC, delegate
		{
			_E2C1?.Invoke(_E2CC);
		});
	}

	private void _E007(_E464 message)
	{
		if (!_E2D3)
		{
			if (message.HasRewards)
			{
				this._E001 = _E2CC.AttachmentsNew;
			}
			else
			{
				this._E000 = _E2CC.New;
			}
		}
		_E008(message);
	}

	private void _E008(_E464 message)
	{
		string startingValue = string.Empty;
		if (message.Type != EMessageType.SystemMessage)
		{
			startingValue = ((!(_E2CC.DeleteTime.GetValueOrDefault(DateTime.MinValue) < message.UtcDateTime)) ? string.Empty : ((message.Member == _E2D4) ? (_ED3E._E000(230771).Localized() + message.Text) : message.Text));
		}
		_lastMessageLabel.text = ((_E2CC.DeleteTime.GetValueOrDefault(DateTime.MinValue) < message.UtcDateTime) ? message.ParsedText(startingValue, EViewRule.MessageHeader).SubstringIfNecessary(30) : string.Empty);
		DateTime localDateTime = message.LocalDateTime;
		string text = localDateTime.ToString(((_E5AD.Now.ToLocalTime() - localDateTime).TotalDays >= 1.0) ? _ED3E._E000(230753) : _ED3E._E000(230761));
		_timeStamp.text = text;
	}

	private void _E009(bool value)
	{
		_E2D0?.Invoke(value, _E2CC, this);
		_E2D3 = value;
		_backgroundImage.color = (value ? _selectedImageColor : _defaultImageColor);
		_playerNameLabel.color = (value ? _selectedHeaderColor : _defaultHeaderColor);
		_lastMessageLabel.color = (value ? _selectedMessageColor : _defaultMessageColor);
		_timeStamp.color = (value ? _selectedMessageColor : _defaultTimestampColor);
		_E003(value);
		if (value)
		{
			_E006();
			_E000();
			this._E000 = 0;
			this._E001 = 0;
			if (!_E2CC.MessagesLoaded && !_E2CC.Updating)
			{
				_E2C7.SetSelectedStatus(_E2CC, isSelected: true);
			}
		}
	}

	private void _E00A()
	{
		_E2D1?.Invoke(_E2CC);
		_E004();
	}

	private void _E00B(bool state)
	{
		_pinImage.sprite = (state ? _pinnedSprite : _unpinnedSprite);
		_E2CE?.Invoke(state, _E2CC);
	}

	public void OnPointerEnter([NotNull] PointerEventData eventData)
	{
		if (!_E2D3)
		{
			_backgroundImage.color = _hoverImageColor;
		}
	}

	public void OnPointerExit([NotNull] PointerEventData eventData)
	{
		if (!_E2D3)
		{
			_backgroundImage.color = _defaultImageColor;
		}
	}

	public void OnPointerClick([NotNull] PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right)
		{
			_E2CF(_E2CC, eventData.position);
		}
	}

	private void _E00C()
	{
		_E2D0 = null;
		_E2D1 = null;
		_E003(value: false);
		_E009(value: false);
	}

	public override void Close()
	{
		base.Close();
		_E00C();
	}

	[CompilerGenerated]
	private void _E00D()
	{
		_E2D2(_settingsButton.transform.position);
	}

	[CompilerGenerated]
	private void _E00E()
	{
		_E2C1?.Invoke(_E2CC);
	}
}
