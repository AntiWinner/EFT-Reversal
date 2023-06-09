using Comfort.Common;
using EFT.Quests;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class StatView : UIElement
{
	[SerializeField]
	private Image _icon;

	[SerializeField]
	private CustomTextMeshProUGUI _value;

	[SerializeField]
	private CustomTextMeshProUGUI _name;

	[SerializeField]
	private SpriteMap _icons;

	[SerializeField]
	private ColorMap _colors;

	public void Show(_E936 reward)
	{
		ShowGameObject();
		_icon.sprite = _icons[reward.type];
		if (_icon.sprite == null)
		{
			Debug.LogError(string.Concat(_ED3E._E000(261957), reward.type, _ED3E._E000(27308)));
		}
		string target = reward.target;
		string text = string.Empty;
		switch (reward.type)
		{
		case ERewardType.Skill:
			text = target.Localized();
			break;
		case ERewardType.TraderStanding:
		case ERewardType.TraderStandingReset:
		case ERewardType.TraderStandingRestore:
		{
			if (Singleton<_E5CB>.Instance.TradersSettings.TryGetValue(target, out var value))
			{
				text = value.Nickname.Localized();
			}
			else
			{
				Debug.LogError(_ED3E._E000(261986) + target);
			}
			break;
		}
		}
		_name.gameObject.SetActive(!string.IsNullOrEmpty(text));
		_name.text = text;
		float num = reward.value;
		string format;
		if (reward.type == ERewardType.Skill)
		{
			format = _ED3E._E000(262077).Localized();
			num /= 100f;
		}
		else
		{
			format = ((num > 0f) ? _ED3E._E000(149385) : _ED3E._E000(55940));
		}
		_value.text = string.Format(format, num);
		_value.color = ((num >= 0f) ? _colors[_ED3E._E000(261907)] : _colors[_ED3E._E000(261914)]);
		_value.gameObject.SetActive(!string.IsNullOrEmpty(_value.text));
	}
}
