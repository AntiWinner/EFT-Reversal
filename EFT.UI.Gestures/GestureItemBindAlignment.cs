using UnityEngine;

namespace EFT.UI.Gestures;

public sealed class GestureItemBindAlignment : GesturesBindAlignment
{
	[SerializeField]
	private float _hotkeySpacing = 3f;

	public override void AlignToCenter()
	{
		base.AlignToCenter();
		Vector2 vector = new Vector2(0.5f, 0f);
		BindRoot.anchorMin = vector;
		BindRoot.anchorMax = vector;
		BindRoot.pivot = new Vector2(0.5f, 1f);
		BindRoot.anchoredPosition = new Vector2(0.5f * HotkeyImage.rectTransform.rect.width, 0f - _hotkeySpacing);
	}

	public override void AlignToLeft()
	{
		base.AlignToLeft();
		Vector2 vector = new Vector2(1f, 0.5f);
		BindRoot.anchorMin = vector;
		BindRoot.anchorMax = vector;
		BindRoot.pivot = new Vector2(0f, 0.5f);
		BindRoot.anchoredPosition = new Vector2(_hotkeySpacing, 0f);
	}

	public override void AlignToRight()
	{
		base.AlignToRight();
		Vector2 vector = new Vector2(0f, 0.5f);
		BindRoot.anchorMin = vector;
		BindRoot.anchorMax = vector;
		BindRoot.pivot = new Vector2(1f, 0.5f);
		BindRoot.anchoredPosition = new Vector2(0f - _hotkeySpacing, 0f);
	}
}
