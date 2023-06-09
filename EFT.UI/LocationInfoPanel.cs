using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class LocationInfoPanel : UIElement
{
	[SerializeField]
	private CustomTextMeshProUGUI _name;

	[SerializeField]
	private GameObject _lockedPanel;

	[SerializeField]
	private CustomTextMeshProUGUI _availableOnLevelLabel;

	[SerializeField]
	private GameObject _watchIntro;

	[SerializeField]
	private Sprite _defaultImage;

	[SerializeField]
	private Image _banner;

	[SerializeField]
	private CustomTextMeshProUGUI _description;

	[SerializeField]
	private CustomTextMeshProUGUI _area;

	[SerializeField]
	private CustomTextMeshProUGUI _playTime;

	[SerializeField]
	private CustomTextMeshProUGUI _difficulty;

	[SerializeField]
	private CustomTextMeshProUGUI _players;

	[SerializeField]
	private LocationWarningPanel _warningPanel;

	[SerializeField]
	private GameObject _bottomPanel;

	public void Set([CanBeNull] _E554.Location location, ESideType chosenSideType, int playerLevel)
	{
		ShowGameObject();
		_bottomPanel.SetActive(location != null);
		if (location == null)
		{
			_name.text = _ED3E._E000(249071).Localized();
			_description.text = _ED3E._E000(249062);
			_lockedPanel.SetActive(value: false);
			_watchIntro.SetActive(value: false);
			return;
		}
		_name.text = (location._Id + _ED3E._E000(70087)).Localized();
		bool flag = location.Locked || playerLevel < location.RequiredPlayerLevel || (chosenSideType == ESideType.Savage && location.DisabledForScav);
		_lockedPanel.SetActive(flag);
		_availableOnLevelLabel.text = _E000(location, playerLevel, chosenSideType);
		_watchIntro.SetActive(!flag && !_E38D.DisabledForNow);
		_description.text = (location._Id + _ED3E._E000(114100)).Localized();
		_area.text = location.Area + _ED3E._E000(249096);
		_playTime.text = location.AveragePlayTime + _ED3E._E000(249094);
		_players.text = location.MinPlayers + _ED3E._E000(29690) + location.MaxPlayers;
		_warningPanel.Set(location.Rules);
		int num = location.AveragePlayerLevel - playerLevel;
		if (num < -20)
		{
			_difficulty.text = _ED3E._E000(249091);
		}
		else if (num < -10)
		{
			_difficulty.text = _ED3E._E000(249146);
		}
		else if (num < 0)
		{
			_difficulty.text = _ED3E._E000(249143);
		}
		else if (num < 15)
		{
			_difficulty.text = _ED3E._E000(249134);
		}
		else
		{
			_difficulty.text = _ED3E._E000(249131);
		}
		_banner.sprite = _E001(location);
	}

	private string _E000(_E554.Location location, int playerLevel, ESideType side)
	{
		if (location.Locked)
		{
			return _ED3E._E000(249122).Localized();
		}
		if (playerLevel < location.RequiredPlayerLevel)
		{
			return string.Format(_ED3E._E000(249170).Localized(), location.RequiredPlayerLevel);
		}
		if (side == ESideType.Savage && location.DisabledForScav)
		{
			return _ED3E._E000(249209).Localized();
		}
		return string.Empty;
	}

	private Sprite _E001(_E554.Location location)
	{
		return _E905.Pop<Sprite>(_ED3E._E000(249233) + location.Id) ?? _defaultImage;
	}
}
