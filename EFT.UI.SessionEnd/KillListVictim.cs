using UnityEngine;

namespace EFT.UI.SessionEnd;

public sealed class KillListVictim : UIElement
{
	private const string _E23A = "Headshot";

	private const string _E23B = "Killed";

	private const string _E23C = "Unknown weapon";

	[SerializeField]
	private CustomTextMeshProUGUI _number;

	[SerializeField]
	private CustomTextMeshProUGUI _timestamp;

	[SerializeField]
	private CustomTextMeshProUGUI _name;

	[SerializeField]
	private CustomTextMeshProUGUI _level;

	[SerializeField]
	private CustomTextMeshProUGUI _faction;

	[SerializeField]
	private CustomTextMeshProUGUI _status;

	[SerializeField]
	private GameObject _separator;

	[SerializeField]
	private Color _pmcColor;

	[SerializeField]
	private Color _scavColor;

	public void Show(VictimStats victim, bool knownName, int number)
	{
		ShowGameObject();
		_separator.SetActive(number > 1);
		bool flag = victim.Side == EPlayerSide.Savage;
		knownName = knownName || flag;
		_number.text = number.ToString();
		_timestamp.text = victim.Time.ToString(_ED3E._E000(72296));
		_name.text = (knownName ? victim.GetCorrectedNickname() : _ED3E._E000(252232));
		_level.text = (flag ? _ED3E._E000(254434) : (knownName ? victim.Level.ToString() : _ED3E._E000(91186)));
		if (flag)
		{
			_faction.text = victim.Role.GetScavRoleKey().Localized(EStringCase.Upper);
			_faction.color = _scavColor;
		}
		else
		{
			_faction.text = victim.Side.LocalizedShort(EStringCase.Upper);
			_faction.color = _pmcColor;
		}
		string text = (string.IsNullOrEmpty(victim.Weapon) ? _ED3E._E000(254493) : victim.Weapon).Localized();
		string text2 = ((!victim.Distance.IsZero()) ? (_ED3E._E000(10270) + victim.Distance.ToString(_ED3E._E000(215469)) + _ED3E._E000(254476).Localized()) : string.Empty);
		_status.text = ((!knownName) ? _ED3E._E000(252232) : ((victim.BodyPart == EBodyPart.Head) ? (_ED3E._E000(260834) + _ED3E._E000(254505).Localized() + _ED3E._E000(254514) + text + text2 + _ED3E._E000(27308)) : (_ED3E._E000(260834) + _ED3E._E000(254523).Localized() + _ED3E._E000(254514) + text + _ED3E._E000(10270) + victim.BodyPart.LocalizedShort(EStringCase.Lower) + text2 + _ED3E._E000(27308))));
	}
}
