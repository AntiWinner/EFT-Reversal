using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.Chat;

public class FriendsInvitationView : UIElement, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	[SerializeField]
	private TextMeshProUGUI _playerNameLabel;

	[SerializeField]
	private Image _backgroundImage;

	[SerializeField]
	private Image _friendIcon;

	[SerializeField]
	private ChatSpecialIcon _icon;

	[SerializeField]
	private Sprite _inputInvitation;

	[SerializeField]
	private Sprite _outputInvitation;

	[SerializeField]
	private Color _defaultImageColor;

	[SerializeField]
	private Color _selectedImageColor;

	private _E467 _E2E0;

	private Action<_E467, Vector2> _E06D;

	public void Show(_E467 invitation, Action<_E467, Vector2> onClick)
	{
		ShowGameObject();
		_E2E0 = invitation;
		_E06D = onClick;
		_playerNameLabel.text = invitation.Profile.LocalizedNickname;
		_icon.Show(invitation.Profile.Info.MemberCategory);
		_friendIcon.sprite = (invitation.InputInvitation ? _inputInvitation : _outputInvitation);
	}

	public void OnPointerEnter([NotNull] PointerEventData eventData)
	{
		_backgroundImage.color = _selectedImageColor;
	}

	public void OnPointerExit([NotNull] PointerEventData eventData)
	{
		_backgroundImage.color = _defaultImageColor;
	}

	public void OnPointerClick([NotNull] PointerEventData eventData)
	{
		_E06D(_E2E0, eventData.position);
	}
}
