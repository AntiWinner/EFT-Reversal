using ChatShared;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class EmptyMemberView : ButtonFeedback
{
	[SerializeField]
	private Image _bgImage;

	[SerializeField]
	private Sprite _bgIdleSprite;

	[SerializeField]
	private Sprite _bgHoverSprite;

	[SerializeField]
	private Image _borderImage;

	[SerializeField]
	private Sprite _borderIdleSprite;

	[SerializeField]
	private Sprite _borderHoverSprite;

	[SerializeField]
	private Image _plusImage;

	[SerializeField]
	private Color _idlePlusColor;

	[SerializeField]
	private Color _hoverPlusColor;

	private _EC99 _E006;

	private _ECEF<UpdatableChatMember> _E0FC;

	public void Show(_ECEF<UpdatableChatMember> friendsList, _EC99 matchmakerPlayersController)
	{
		ShowGameObject();
		_E0FC = friendsList;
		_E006 = matchmakerPlayersController;
		_E000(hover: false);
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		base.OnPointerClick(eventData);
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			ItemUiContext.Instance.ShowPlayersInviteWindow(_E006.CurrentProfileAid, _E0FC, _E006);
		}
	}

	private void _E000(bool hover)
	{
		_bgImage.sprite = (hover ? _bgHoverSprite : _bgIdleSprite);
		_borderImage.sprite = (hover ? _borderHoverSprite : _borderIdleSprite);
		_plusImage.color = (hover ? _hoverPlusColor : _idlePlusColor);
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		base.OnPointerEnter(eventData);
		_E000(hover: true);
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		base.OnPointerExit(eventData);
		_E000(hover: false);
	}
}
