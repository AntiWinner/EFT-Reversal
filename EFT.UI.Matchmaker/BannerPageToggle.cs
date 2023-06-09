using System;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Matchmaker;

public sealed class BannerPageToggle : Toggle, IDisposable
{
	[SerializeField]
	private Image _image;

	[SerializeField]
	private Sprite _onSprite;

	[SerializeField]
	private Sprite _offSprite;

	private Action<bool> m__E000;

	public void Init(ToggleGroup toggleGroup, MatchmakerBanner banner, Action<bool> onSelected)
	{
		this.m__E000 = onSelected;
		base.group = toggleGroup;
		onValueChanged.AddListener(_E000);
	}

	private void _E000(bool value)
	{
		this.m__E000(value);
		SetSprite(value);
	}

	public void SetSprite(bool active)
	{
		_image.sprite = (active ? _onSprite : _offSprite);
	}

	public void Dispose()
	{
		base.group.UnregisterToggle(this);
		onValueChanged.RemoveListener(_E000);
		UnityEngine.Object.DestroyImmediate(base.gameObject);
	}
}
