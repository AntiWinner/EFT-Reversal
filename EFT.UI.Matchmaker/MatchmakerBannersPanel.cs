using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.Counters;
using EFT.UI.Tutorial;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.Matchmaker;

public sealed class MatchmakerBannersPanel : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _E554.Location._E004 banner;

		public MatchmakerBannersPanel _003C_003E4__this;

		internal void _E000(Result<Texture2D> result)
		{
			_003C_003E4__this._E001(banner.id + _ED3E._E000(70087), banner.id + _ED3E._E000(114100), result.Succeed ? result.Value : null, selected: false);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public Sprite sprite;

		internal void _E000()
		{
			UnityEngine.Object.DestroyImmediate(sprite);
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public MatchmakerBannersPanel _003C_003E4__this;

		public MatchmakerBanner bannerImage;

		public _EC9C currentBanner;

		internal void _E000(bool visible)
		{
			if (!visible && !_003C_003E4__this._E2B1 && _003C_003E4__this._E2B0[_003C_003E4__this._E2B4].Banner != bannerImage)
			{
				_003C_003E4__this._E003(_003C_003E4__this._E2B0[_003C_003E4__this._E2B4], visible: false);
			}
			_003C_003E4__this._E003(currentBanner, visible);
			if (!_003C_003E4__this._E2B2)
			{
				_003C_003E4__this._E2B2 = true;
			}
			else if (visible)
			{
				_003C_003E4__this._E2B1 = true;
			}
		}
	}

	private const float _E2AF = 10f;

	public KeyBannerGenerator KeyBannerGenerator;

	[SerializeField]
	private ToggleGroup _toggleGroup;

	[SerializeField]
	private MatchmakerBanner _bannerImageTemplate;

	[SerializeField]
	private BannerPageToggle _bannerPageTemplate;

	[SerializeField]
	private Sprite _defaultImage;

	[SerializeField]
	private Sprite _savageSprite;

	[SerializeField]
	private CustomTextMeshProUGUI _bannerHeader;

	[SerializeField]
	private CustomTextMeshProUGUI _bannerDescription;

	[SerializeField]
	private RectTransform _bannerPagesPlaceholder;

	[SerializeField]
	private RectTransform _bannerImagesPlaceholder;

	private readonly List<_EC9C> _E2B0 = new List<_EC9C>();

	private bool _E2B1;

	private bool _E2B2;

	private float _E2B3;

	private int _E2B4;

	private int _E000
	{
		get
		{
			_E2B4++;
			_E2B4 %= _E2B0.Count;
			return _E2B4;
		}
	}

	public void Show(_E554.Location location, ESideType side, _E34D stats, Action<string, Callback<Texture2D>> textureLoader)
	{
		_E2B3 = Time.time;
		_E2B2 = false;
		ShowGameObject();
		_E554.Location._E004[] banners = location.Banners;
		foreach (Transform item in _bannerPagesPlaceholder)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
		bool flag = stats.OverallCounters.GetAllLong(CounterTag.Sessions) < Singleton<_E5CB>.Instance.SessionsToShowHotKeys;
		if (flag)
		{
			Texture2D blackTexture = Texture2D.blackTexture;
			Rect rect = new Rect(0f, 0f, blackTexture.width, blackTexture.height);
			MatchmakerBanner matchmakerBanner = _E002(_ED3E._E000(234091), _ED3E._E000(234128), Sprite.Create(Texture2D.blackTexture, rect, Vector2.zero), selected: true);
			KeyBannerGenerator.GetKeyBindingBanner(matchmakerBanner.transform);
		}
		if (side == ESideType.Savage)
		{
			_E002(_ED3E._E000(234171), _ED3E._E000(234148), _savageSprite, !flag);
		}
		_E002(location._Id + _ED3E._E000(70087), location._Id + _ED3E._E000(114100), _E000(location), !flag && side != ESideType.Savage);
		_E554.Location._E004[] array = banners;
		foreach (_E554.Location._E004 obj in array)
		{
			_E554.Location._E004 banner = obj;
			textureLoader(_ED3E._E000(234188) + obj.pic.path, delegate(Result<Texture2D> result)
			{
				_E001(banner.id + _ED3E._E000(70087), banner.id + _ED3E._E000(114100), result.Succeed ? result.Value : null, selected: false);
			});
		}
		_bannerHeader.gameObject.SetActive(value: true);
	}

	private Sprite _E000(_E554.Location location)
	{
		return _E905.Pop<Sprite>(_ED3E._E000(249233) + location.Id) ?? _defaultImage;
	}

	private void Update()
	{
		if (!(_E2B3 + 10f > Time.time) && !_E2B1 && _E2B0.Count > 0)
		{
			_E2B3 = Time.time;
			_E003(_E2B0.ElementAt(_E2B4), visible: false);
			_E003(_E2B0.ElementAt(this._E000), visible: true);
		}
	}

	private void _E001(string bannerName, string description, [CanBeNull] Texture2D texture, bool selected)
	{
		Sprite sprite = ((texture != null) ? Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f) : null);
		if (sprite != null)
		{
			UI.AddDisposable(delegate
			{
				UnityEngine.Object.DestroyImmediate(sprite);
			});
		}
		_E002(bannerName, description, sprite, selected);
	}

	private MatchmakerBanner _E002(string bannerName, string description, Sprite sprite, bool selected)
	{
		MatchmakerBanner bannerImage = UnityEngine.Object.Instantiate(_bannerImageTemplate);
		(bannerImage.transform as RectTransform).SetRectTransformParent(_bannerImagesPlaceholder);
		bannerImage.Init(bannerName, description, sprite);
		BannerPageToggle bannerPageToggle = UnityEngine.Object.Instantiate(_bannerPageTemplate, _bannerPagesPlaceholder, worldPositionStays: true);
		_EC9C currentBanner = new _EC9C(bannerImage, bannerPageToggle);
		bannerPageToggle.Init(_toggleGroup, bannerImage, delegate(bool visible)
		{
			if (!visible && !_E2B1 && _E2B0[_E2B4].Banner != bannerImage)
			{
				_E003(_E2B0[_E2B4], visible: false);
			}
			_E003(currentBanner, visible);
			if (!_E2B2)
			{
				_E2B2 = true;
			}
			else if (visible)
			{
				_E2B1 = true;
			}
		});
		UI.AddDisposable(bannerPageToggle);
		UI.AddDisposable(bannerImage);
		_E003(currentBanner, selected);
		_E2B0.Add(currentBanner);
		return bannerImage;
	}

	private void _E003(_EC9C bannerWithToggle, bool visible)
	{
		if (visible)
		{
			MatchmakerBanner banner = bannerWithToggle.Banner;
			if (!string.IsNullOrEmpty(banner.BannerName))
			{
				_bannerHeader.text = banner.BannerName.Localized();
			}
			if (!string.IsNullOrEmpty(banner.BannerDescription))
			{
				string text = banner.BannerDescription.Localized();
				if (string.IsNullOrEmpty(text) || text.Equals(banner.BannerDescription))
				{
					_bannerDescription.text = string.Empty;
					_bannerDescription.gameObject.SetActive(value: false);
				}
				else
				{
					_bannerDescription.text = text;
					_bannerDescription.gameObject.SetActive(value: true);
				}
			}
			else
			{
				_bannerDescription.gameObject.SetActive(value: false);
			}
		}
		bannerWithToggle.Banner.SetSelected(visible);
		bannerWithToggle.Toggle.SetSprite(visible);
	}

	public override void Close()
	{
		base.Close();
		_E2B0.Clear();
		_E2B4 = 0;
		_E2B1 = false;
		_E2B3 = 0f;
		_toggleGroup.SetAllTogglesOff();
	}
}
