using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.Matchmaker;

public sealed class RaidReadyPlayerPanel : UIElement, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public RaidReadyPlayerPanel _003C_003E4__this;

		public _ECEF<_E551> invites;

		public _EC9B player;

		public Func<_E551, bool> _003C_003E9__1;

		internal void _E000()
		{
			_003C_003E4__this.SetInviteSentStatus(invites.Any((_E551 invite) => invite.To == player.AccountId));
		}

		internal bool _E001(_E551 invite)
		{
			return invite.To == player.AccountId;
		}
	}

	[CompilerGenerated]
	private Action<_EC9B, Vector2> _E296;

	[SerializeField]
	private Image _backgroundImage;

	[SerializeField]
	private Color _defaultImageColor;

	[SerializeField]
	private Color _yourImageColor;

	[SerializeField]
	private Color _selectedImageColor;

	[SerializeField]
	private ChatSpecialIcon _icon;

	[SerializeField]
	private CustomTextMeshProUGUI _nameLabel;

	[SerializeField]
	private CustomTextMeshProUGUI _sideLabel;

	[SerializeField]
	private CustomTextMeshProUGUI _statusLabel;

	[SerializeField]
	private GameObject _inviteSentGameObject;

	[SerializeField]
	private GameObject _lookingForGroupGameObject;

	private _EC9B _E094;

	private bool _E297;

	private bool _E298;

	public event Action<_EC9B, Vector2> OnPlayerClick
	{
		[CompilerGenerated]
		add
		{
			Action<_EC9B, Vector2> action = _E296;
			Action<_EC9B, Vector2> action2;
			do
			{
				action2 = action;
				Action<_EC9B, Vector2> value2 = (Action<_EC9B, Vector2>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E296, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<_EC9B, Vector2> action = _E296;
			Action<_EC9B, Vector2> action2;
			do
			{
				action2 = action;
				Action<_EC9B, Vector2> value2 = (Action<_EC9B, Vector2>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E296, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public void Show(_EC9B player, bool yourPanel, bool friendPanel, _ECEF<_E551> invites)
	{
		ShowGameObject();
		_E094 = player;
		_E297 = yourPanel;
		_E298 = friendPanel;
		_nameLabel.text = player.GetCorrectedNickname();
		_icon.Show((player.Info.Side != EPlayerSide.Savage) ? player.Info.MemberCategory : EMemberCategory.Default);
		_sideLabel.text = _ED3E._E000(253257) + player.Info.Level + _ED3E._E000(235128) + player.Info.Side.LocalizedShort(EStringCase.Upper) + _ED3E._E000(11164);
		string text = (_E297 ? _ED3E._E000(235125) : (friendPanel ? _ED3E._E000(233757) : string.Empty));
		if (!string.IsNullOrEmpty(text))
		{
			_statusLabel.text = _ED3E._E000(54246) + text.Localized() + _ED3E._E000(27308);
			_statusLabel.gameObject.SetActive(value: true);
		}
		else
		{
			_statusLabel.gameObject.SetActive(value: false);
		}
		_E000();
		UI.BindEvent(player.InfoChanged, SetLookingForGroupStatus);
		UI.BindEvent(invites.ItemsChanged, delegate
		{
			SetInviteSentStatus(invites.Any((_E551 invite) => invite.To == player.AccountId));
		});
	}

	public void SetLookingForGroupStatus()
	{
		if (!_inviteSentGameObject.activeSelf)
		{
			_lookingForGroupGameObject.SetActive(_E094.LookingForGroup);
		}
	}

	public void SetInviteSentStatus(bool value)
	{
		if (_E094.LookingForGroup)
		{
			_lookingForGroupGameObject.SetActive(!value);
		}
		_inviteSentGameObject.SetActive(value);
	}

	private void _E000()
	{
		_backgroundImage.color = ((_E297 || _E298) ? _yourImageColor : _defaultImageColor);
	}

	public void OnPointerEnter([NotNull] PointerEventData eventData)
	{
		_backgroundImage.color = _selectedImageColor;
	}

	public void OnPointerExit([NotNull] PointerEventData eventData)
	{
		_E000();
	}

	public void OnPointerClick([NotNull] PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right || eventData.button == PointerEventData.InputButton.Left)
		{
			_E296?.Invoke(_E094, eventData.position);
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
	}

	public void OnPointerDown(PointerEventData eventData)
	{
	}
}
