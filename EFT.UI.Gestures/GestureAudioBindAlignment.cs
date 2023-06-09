using TMPro;
using UnityEngine;

namespace EFT.UI.Gestures;

public sealed class GestureAudioBindAlignment : GesturesBindAlignment
{
	[SerializeField]
	private CustomTextMeshProUGUI _situationLabel;

	public override void AlignToCenter()
	{
		base.AlignToCenter();
		_situationLabel.alignment = TextAlignmentOptions.Left;
		BindRoot.pivot = new Vector2(0.5f, 0.5f);
		BindRoot.anchoredPosition += 0.5f * HotkeyImage.rectTransform.rect.width * Vector2.right;
	}

	public override void AlignToLeft()
	{
		base.AlignToLeft();
		_situationLabel.alignment = TextAlignmentOptions.Left;
		BindRoot.pivot = new Vector2(0f, 0.5f);
	}

	public override void AlignToRight()
	{
		base.AlignToRight();
		_situationLabel.alignment = TextAlignmentOptions.Right;
		BindRoot.pivot = new Vector2(1f, 0.5f);
	}
}
