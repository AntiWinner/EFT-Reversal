using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Matchmaker;

public sealed class MatchmakerBanner : UIElement
{
	[SerializeField]
	private Image _bannerImage;

	[SerializeField]
	private CanvasGroup _imageCanvasGroup;

	[SerializeField]
	private CanvasGroup _bannerCanvasGroup;

	[HideInInspector]
	public string BannerName;

	[HideInInspector]
	public string BannerDescription;

	private const float _E293 = 2f;

	private bool _E294;

	private Coroutine _E1AC;

	public void Init(string bannerName, string description, Sprite sprite)
	{
		BannerName = bannerName;
		BannerDescription = description;
		_bannerImage.sprite = sprite;
	}

	public void SetSelected(bool value)
	{
		if (_E294 != !value)
		{
			return;
		}
		_E294 = value;
		if (_E294)
		{
			_bannerCanvasGroup.alpha = 1f;
		}
		_imageCanvasGroup.SoftChange(_E294, ref _E1AC, delegate
		{
			if (!_E294)
			{
				_bannerCanvasGroup.alpha = 0f;
			}
			_E1AC = null;
		}, 2f);
	}

	public override void Close()
	{
		if (_E1AC != null)
		{
			StaticManager.KillCoroutine(_E1AC);
			_E1AC = null;
		}
		base.Close();
		Object.DestroyImmediate(base.gameObject);
	}

	[CompilerGenerated]
	private void _E000()
	{
		if (!_E294)
		{
			_bannerCanvasGroup.alpha = 0f;
		}
		_E1AC = null;
	}
}
