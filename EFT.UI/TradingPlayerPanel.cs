using System.Collections.Generic;
using System.Globalization;
using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class TradingPlayerPanel : UIElement
{
	[SerializeField]
	private CustomTextMeshProUGUI _level;

	[SerializeField]
	private Image _levelIcon;

	[SerializeField]
	private CustomTextMeshProUGUI _nickname;

	[SerializeField]
	private DisplayMoneyPanel _moneyCountPanel;

	[SerializeField]
	private GameObject _nextRankPanel;

	[SerializeField]
	private Image _currentLevelCurrencyIcon;

	[SerializeField]
	private Image _nextLevelCurrencyIcon;

	[SerializeField]
	private RankPanel _currentRank;

	[SerializeField]
	private RankPanel _nextRank;

	[SerializeField]
	private CustomTextMeshProUGUI _currentLevel;

	[SerializeField]
	private CustomTextMeshProUGUI _nextLevel;

	[SerializeField]
	private CustomTextMeshProUGUI _currentStanding;

	[SerializeField]
	private CustomTextMeshProUGUI _nextStanding;

	[SerializeField]
	private CustomTextMeshProUGUI _currentMoney;

	[SerializeField]
	private CustomTextMeshProUGUI _nextMoney;

	private const string _E205 = "#54c1ff";

	private const string _E17B = "#c40000";

	public static _EC6C GetMoneyTuple(Dictionary<ECurrencyType, int> moneySums)
	{
		int num = moneySums[ECurrencyType.USD];
		int num2 = moneySums[ECurrencyType.EUR];
		int num3 = moneySums[ECurrencyType.RUB];
		NumberFormatInfo numberFormatInfo = new NumberFormatInfo
		{
			NumberGroupSeparator = _ED3E._E000(18502)
		};
		return new _EC6C(num3.ToString(_ED3E._E000(59493), numberFormatInfo), num2.ToString(_ED3E._E000(59493), numberFormatInfo), num.ToString(_ED3E._E000(59493), numberFormatInfo));
	}

	public void Set(Profile profile, Profile._E001 traderInfo)
	{
		ShowGameObject();
		PlayerLevelPanel.SetLevelIcon(_levelIcon, profile.Info.Level);
		_nickname.text = profile.GetCorrectedNickname() + _ED3E._E000(54246) + _ED3E._E000(261024).Localized() + _ED3E._E000(27308);
		_moneyCountPanel.Show(profile.Inventory.Stash.Grid.Items);
		Sprite smallCurrencySign = EFTHardSettings.Instance.StaticIcons.GetSmallCurrencySign(traderInfo.Settings.Currency);
		_currentLevelCurrencyIcon.sprite = smallCurrencySign;
		_nextLevelCurrencyIcon.sprite = smallCurrencySign;
		UpdateStats(traderInfo);
	}

	public void UpdateStats(Profile._E001 traderInfo)
	{
		_currentStanding.text = traderInfo.Standing.ToString(_ED3E._E000(253692));
		_currentMoney.text = _ECA1.GetMoneyString(traderInfo.SalesSum) + _ED3E._E000(54246) + _ED3E._E000(261079).Localized() + _ED3E._E000(27308);
		_level.text = traderInfo.ProfileLevel.ToString();
		_currentLevel.text = traderInfo.ProfileLevel.ToString();
		_currentRank.Show(traderInfo.LoyaltyLevel, traderInfo.MaxLoyaltyLevel);
		_E5CB.TraderLoyaltyLevel nextLoyalty;
		bool flag = traderInfo.TryGetNextLoyalty(out nextLoyalty);
		_nextRankPanel.SetActive(flag);
		if (flag)
		{
			bool flag2 = traderInfo.ProfileLevel >= nextLoyalty.MinProfileLevel;
			bool flag3 = traderInfo.Standing >= nextLoyalty.MinStanding;
			bool flag4 = traderInfo.SalesSum >= nextLoyalty.MinSalesSum;
			_nextRank.Show(traderInfo.LoyaltyLevel + 1, traderInfo.MaxLoyaltyLevel);
			_nextLevel.text = string.Format(_ED3E._E000(224491), nextLoyalty.MinProfileLevel, flag2 ? _ED3E._E000(261000) : _ED3E._E000(261008));
			_nextStanding.text = string.Format(_ED3E._E000(261067), nextLoyalty.MinStanding, flag3 ? _ED3E._E000(261000) : _ED3E._E000(261008));
			_nextMoney.text = string.Format(_ED3E._E000(261101), _ECA1.GetMoneyString(nextLoyalty.MinSalesSum), flag4 ? _ED3E._E000(261000) : _ED3E._E000(261008), _ED3E._E000(261079).Localized());
		}
	}
}
