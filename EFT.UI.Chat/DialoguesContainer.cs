using System;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.UI.Utilities.LightScroller;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Chat;

public sealed class DialoguesContainer : UIElement
{
	private enum EDialogType
	{
		None,
		Trader,
		User
	}

	[CompilerGenerated]
	private sealed class _E000
	{
		public DialoguesContainer _003C_003E4__this;

		public _ED01<_E466, _ECA4> dialogsList;

		internal void _E000()
		{
			_003C_003E4__this._noDialogsPlaceholder.SetActive(!dialogsList.Any());
		}

		internal DialogueView _E001(_ECA4 dialog)
		{
			return _003C_003E4__this._cellViewPrefab;
		}

		internal void _E002(_ECA4 item, DialogueView view)
		{
			view.Show(item, _003C_003E4__this._E2D7);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public DialoguesContainer _003C_003E4__this;

		public _E466 dialogue;

		internal void _E000(_E466 view, Vector2 pos)
		{
			_003C_003E4__this._dialogueListContextMenu.Show(pos, new _ECA7(_003C_003E4__this._E2C7, _003C_003E4__this._invitePlayersPanel, dialogue));
		}

		internal void _E001(Vector2 pos)
		{
			_003C_003E4__this._channelContextMenu.Show(_EC74.ScaledPosition(pos), new _EC4B(_003C_003E4__this._E2C7));
		}
	}

	[SerializeField]
	private LightScroller _scroller;

	[SerializeField]
	private DialogueView _cellViewPrefab;

	[SerializeField]
	private ChatMessageSendBlock _chatMessageSendBlock;

	[SerializeField]
	private ChatInvitePlayersPanel _invitePlayersPanel;

	[SerializeField]
	private GameObject _loader;

	[SerializeField]
	private GameObject _noDialogsPlaceholder;

	[SerializeField]
	private SimpleContextMenu _dialogueListContextMenu;

	[SerializeField]
	private SimpleContextMenu _channelContextMenu;

	[SerializeField]
	private CustomTextMeshProUGUI _dialogueHeaderLabel;

	[SerializeField]
	private CustomTextMeshProUGUI _dialogueUsersCountLabel;

	private ToggleGroup _E2D7;

	private _E79D _E2C7;

	private Action<_E466> _E2C1;

	private Action<_E466, DialogueView> _E03D;

	private Action<_E466> _E2D8;

	public void Show(_E79D social, Action<_E466> onTransferAll, Action<_E466, DialogueView> onSelected, Action<_E466> onRemoved)
	{
		UI.Dispose();
		_E2D7 = base.gameObject.GetOrAddComponent<ToggleGroup>();
		_E2D7.allowSwitchOff = false;
		ShowGameObject();
		_E2C7 = social;
		_E2C1 = onTransferAll;
		_E03D = onSelected;
		_E2D8 = onRemoved;
		_ED01<_E466, _ECA4> dialogsList = new _ED01<_E466, _ECA4>(social.PinnedDialogues, _E000, _ECA4.Comparer);
		dialogsList.AddSource(social.UnpinnedDialogues);
		UI.AddDisposable(dialogsList);
		UI.AddDisposable(_scroller);
		UI.AddDisposable(dialogsList.ItemsChanged.Bind(delegate
		{
			_noDialogsPlaceholder.SetActive(!dialogsList.Any());
		}));
		_scroller.Show(dialogsList, (_ECA4 dialog) => _cellViewPrefab, (_ECA4 dialog) => EDialogType.None, delegate(_ECA4 item, DialogueView view)
		{
			view.Show(item, _E2D7);
		});
	}

	private bool _E000(_E466 dialogue, out _ECA4 dialogueData)
	{
		dialogueData = new _ECA4
		{
			UsersCountLabel = _dialogueUsersCountLabel,
			Pinned = dialogue.Pinned,
			LocalDateTime = dialogue.Message.LocalDateTime,
			OnDialogueSelected = _E001,
			OnDialogueRemoved = _E002,
			ChatMessageSendBlock = _chatMessageSendBlock,
			SocialNetwork = _E2C7,
			Dialogue = dialogue,
			OnPinned = _E003,
			OnTransferAll = _E2C1,
			OnContextMenuCalled = delegate(_E466 view, Vector2 pos)
			{
				_dialogueListContextMenu.Show(pos, new _ECA7(_E2C7, _invitePlayersPanel, dialogue));
			},
			OnChannelListCalled = delegate(Vector2 pos)
			{
				_channelContextMenu.Show(_EC74.ScaledPosition(pos), new _EC4B(_E2C7));
			}
		};
		return true;
	}

	private void Update()
	{
		if (_E2C7 == null || _E2C7.FullyRead || _E2C7.MessageListUpdating || !(_scroller.NormalizedScrollPosition <= 0f))
		{
			return;
		}
		_loader.SetActive(value: true);
		_E2C7.UpdateDialogueList(delegate
		{
			if (!(this == null))
			{
				_loader.SetActive(value: false);
			}
		});
	}

	private void _E001(bool arg, _E466 dialogue, DialogueView view)
	{
		if (arg)
		{
			_E03D?.Invoke(dialogue, view);
			_dialogueHeaderLabel.text = _E2C7.GetDialogueHeaderByType(dialogue).SubstringIfNecessary(50);
		}
	}

	private void _E002(_E466 dialogue)
	{
		_E2D8?.Invoke(dialogue);
		_dialogueHeaderLabel.text = _ED3E._E000(230815).Localized();
	}

	private void _E003(bool status, _E466 item)
	{
		if (status)
		{
			_E2C7.PinDialog(item);
		}
		else
		{
			_E2C7.UnpinDialog(item);
		}
	}

	public override void Close()
	{
		_E2D7.allowSwitchOff = true;
		base.Close();
	}

	[CompilerGenerated]
	private void _E004()
	{
		if (!(this == null))
		{
			_loader.SetActive(value: false);
		}
	}
}
