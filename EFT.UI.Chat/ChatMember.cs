using System;
using ChatShared;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.Chat;

public class ChatMember : UIElement, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	[SerializeField]
	private TextMeshProUGUI _playerNameLabel;

	[SerializeField]
	private ChatSpecialIcon _specialIcon;

	[SerializeField]
	private CanvasGroup _canvasGroup;

	[SerializeField]
	private Image _backgroundImage;

	[SerializeField]
	private GameObject _muteIcon;

	[SerializeField]
	private GameObject _banIcon;

	[SerializeField]
	private Color _defaultImageColor;

	[SerializeField]
	private Color _hoveredImageColor;

	[SerializeField]
	private Color _chosenHoverImageColor;

	[SerializeField]
	private Color _chosenImageColor;

	private UpdatableChatMember _E2C9;

	private Action<Vector2> _E06D;

	private bool _E2CA;

	public void Show(UpdatableChatMember member, UpdatableChatMember playerMember, bool active, Action<Vector2> onClick)
	{
		ShowGameObject();
		_E2C9 = member;
		_E06D = onClick;
		if (member.Info == null)
		{
			_playerNameLabel.text = _ED3E._E000(230658);
		}
		else
		{
			_playerNameLabel.text = member.LocalizedNickname + _ED3E._E000(253257) + member.Info.Level + _ED3E._E000(11164);
			if (member.Id == playerMember.Id)
			{
				_playerNameLabel.text = _ED3E._E000(230713) + _playerNameLabel.text + _ED3E._E000(230710);
			}
			_specialIcon.Show(member.Info.MemberCategory);
			_E000(member.Info.Banned);
			UI.AddDisposable(_E2C9.OnPlayerBanStatusChanged.Subscribe(_E000));
		}
		UpdateIgnoreStatus();
		UI.BindEvent(member.OnIgnoreStatusChanged, UpdateIgnoreStatus);
		_canvasGroup.SetUnlockStatus(active);
	}

	public void MarkAsChosen(bool state)
	{
		_E2CA = state;
		_backgroundImage.color = (state ? _chosenImageColor : _defaultImageColor);
	}

	public void OnPointerEnter([NotNull] PointerEventData eventData)
	{
		_backgroundImage.color = (_E2CA ? _chosenHoverImageColor : _hoveredImageColor);
	}

	public void OnPointerExit([NotNull] PointerEventData eventData)
	{
		_backgroundImage.color = (_E2CA ? _chosenImageColor : _defaultImageColor);
	}

	public void OnPointerClick([NotNull] PointerEventData eventData)
	{
		_E06D(eventData.position);
	}

	public void UpdateIgnoreStatus()
	{
		_muteIcon.SetActive(_E2C9.Info.Ignored);
	}

	private void _E000(bool value)
	{
		if (_banIcon != null)
		{
			_banIcon.SetActive(value);
		}
	}
}
