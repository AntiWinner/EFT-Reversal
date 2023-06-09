using UnityEngine;

namespace EFT.UI;

public class TraderTooltip : MonoBehaviour
{
	[SerializeField]
	private CustomTextMeshProUGUI _nickname;

	[SerializeField]
	private CustomTextMeshProUGUI _location;

	[SerializeField]
	private CustomTextMeshProUGUI _description;

	[SerializeField]
	private CustomTextMeshProUGUI _loyaltyLevel;

	[SerializeField]
	private CustomTextMeshProUGUI _standing;

	[SerializeField]
	private CustomTextMeshProUGUI _moneySpent;

	[SerializeField]
	private GameObject _nextLevelContainer;

	[SerializeField]
	private CustomTextMeshProUGUI _standingRequired;

	[SerializeField]
	private CustomTextMeshProUGUI _moneySpentRequired;

	[SerializeField]
	private CustomTextMeshProUGUI _playerLevelRequired;

	[SerializeField]
	private GameObject _standingMet;

	[SerializeField]
	private GameObject _moneySpentMet;

	[SerializeField]
	private GameObject _playerLevelMet;

	[SerializeField]
	private GameObject _detailsPanel;

	[SerializeField]
	private GameObject _lockedPanel;

	[SerializeField]
	private CustomTextMeshProUGUI _lockedText;

	[SerializeField]
	private GameObject _separator;

	private const string m__E000 = "traders/trader_is_locked";

	private const string _E001 = "#747b7e";

	private const string _E002 = "#54c1ff";

	private const string _E003 = "#c40000";

	public void Show(Profile._E001 traderInfo)
	{
		base.gameObject.SetActive(value: true);
		_E5CB.TraderSettings settings = traderInfo.Settings;
		bool available = traderInfo.Available;
		bool disabled = traderInfo.Disabled;
		_detailsPanel.SetActive(available);
		_separator.SetActive(available);
		_lockedPanel.SetActive(!available);
		_lockedText.text = ((disabled && !available) ? _ED3E._E000(186065).Localized() : _ED3E._E000(260857).Localized());
		_nickname.text = settings.Nickname.Localized();
		_location.text = _ED3E._E000(260834) + _ED3E._E000(260894).Localized() + _ED3E._E000(260887) + settings.Location.Localized();
		_description.text = settings.Description.Localized();
		string text = _ECA1.GetStandingRating(traderInfo.Standing).Localized();
		string moneyString = _ECA1.GetMoneyString(traderInfo.SalesSum);
		string currencyString = _EA10.GetCurrencyString(settings.Currency);
		_loyaltyLevel.text = string.Format(_ED3E._E000(260878), _ED3E._E000(260914).Localized(), _ED3E._E000(260957), traderInfo.LoyaltyLevel);
		_standing.text = string.Format(_ED3E._E000(260949), _ED3E._E000(260983).Localized(), _ED3E._E000(260957), traderInfo.Standing, text);
		_moneySpent.text = _ED3E._E000(260968).Localized() + _ED3E._E000(260966) + moneyString + _ED3E._E000(18502) + currencyString + _ED3E._E000(59467);
		_E5CB.TraderLoyaltyLevel nextLoyalty;
		bool flag = traderInfo.TryGetNextLoyalty(out nextLoyalty);
		_nextLevelContainer.SetActive(flag);
		if (flag)
		{
			bool active = traderInfo.Standing >= nextLoyalty.MinStanding;
			bool flag2 = traderInfo.SalesSum >= nextLoyalty.MinSalesSum;
			bool flag3 = traderInfo.ProfileLevel >= nextLoyalty.MinProfileLevel;
			text = _ECA1.GetStandingRating(nextLoyalty.MinStanding).Localized();
			moneyString = _ECA1.GetMoneyString(nextLoyalty.MinSalesSum);
			string text2 = (flag2 ? _ED3E._E000(261000) : _ED3E._E000(261008));
			string arg = (flag3 ? _ED3E._E000(261000) : _ED3E._E000(261008));
			_standingRequired.text = string.Format(_ED3E._E000(260949), _ED3E._E000(260983).Localized(), _ED3E._E000(260957), nextLoyalty.MinStanding, text);
			_moneySpentRequired.text = _ED3E._E000(260992).Localized() + _ED3E._E000(261049) + text2 + _ED3E._E000(59465) + moneyString + _ED3E._E000(18502) + currencyString + _ED3E._E000(59467);
			_playerLevelRequired.text = string.Format(_ED3E._E000(260878), _ED3E._E000(261043).Localized(), arg, nextLoyalty.MinProfileLevel);
			_standingMet.SetActive(active);
			_moneySpentMet.SetActive(flag2);
			_playerLevelMet.SetActive(flag3);
		}
		_E000(Input.mousePosition);
	}

	private void Update()
	{
		_E000(Input.mousePosition);
	}

	private void _E000(Vector2 position)
	{
		position.x = Mathf.Clamp(position.x, 0f, Screen.width);
		position.y = Mathf.Clamp(position.y, 0f, Screen.height);
		base.transform.position = position + new Vector2(15f, 15f);
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}
}
