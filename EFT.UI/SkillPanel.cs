using System.Linq;
using System.Runtime.CompilerServices;
using EFT.HealthSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class SkillPanel : UIElement
{
	[SerializeField]
	private CustomTextMeshProUGUI _level;

	[SerializeField]
	private CustomTextMeshProUGUI _name;

	[SerializeField]
	private BuffIcon _buffIconTemplate;

	[SerializeField]
	private Transform _buffsContainer;

	[SerializeField]
	private Image _initialProgressFill;

	[SerializeField]
	private SkillIcon _skillIcon;

	[SerializeField]
	private Sprite _normalFillSprite;

	[SerializeField]
	private Sprite _eliteFillSprite;

	[SerializeField]
	private CanvasGroup _alphaBlockedSkill;

	[SerializeField]
	private GameObject _effectivenessUp;

	[SerializeField]
	private GameObject _effectivenessDown;

	private SimpleTooltip _E02A;

	private _EC79<_E74F._E003, BuffIcon> _E1A9;

	private _E751 _E1A6;

	private _E9C4 _E0AE;

	public void Show(_E751 skill, _E9C4 healthController)
	{
		ShowGameObject();
		_E1A6 = skill;
		_E0AE = healthController;
		_alphaBlockedSkill.SetUnlockStatus(!_E1A6.Locked, setRaycast: false);
		_name.text = _E1A6.Id.ToString().Localized();
		_E02A = ItemUiContext.Instance.Tooltip;
		_skillIcon.Show(_E1A6, _E0AE, delegate(bool hover, PointerEventData eventData)
		{
			if (hover)
			{
				string text = "";
				if (_E7A3.InRaid && !_E1A6.IsEliteLevel)
				{
					if (_E1A6.Effectiveness > 1f)
					{
						text = string.Format(_ED3E._E000(259019), _ED3E._E000(259040).Localized(), (int)(_E1A6.Effectiveness * 100f));
					}
					else if (_E1A6.Effectiveness < 1f)
					{
						text = string.Format(_ED3E._E000(261136), _ED3E._E000(261165).Localized(), (int)(_E1A6.Effectiveness * 100f));
					}
				}
				_E02A.Show(string.Concat(_E1A6.Id, _ED3E._E000(69230)).Localized() + text);
			}
			else
			{
				_E02A.Close();
			}
		});
		UI.AddDisposable(_skillIcon);
		_E1A9 = UI.AddViewList(_E1A6.Buffs.Where((_E74F._E003 buff) => !buff.HidenForPlayers).ToArray(), _buffIconTemplate, _buffsContainer, delegate(_E74F._E003 buff, BuffIcon buffIcon)
		{
			buffIcon.Show(buff, _E1A6);
		});
		UI.AddDisposable(_E1A9);
		UI.AddDisposable(_E1A6.SkillLevelChanged.Bind(_E001));
		foreach (_E986 item in _E0AE.GetAllActiveEffects(EBodyPart.Head).OfType<_E9C0>().SelectMany((_E9C0 effect) => effect.ActiveBuffs.Where((_E986 x) => x.Active)))
		{
			_E000(item);
		}
		_E0AE.StimulatorBuffEvent += _E000;
		UI.AddDisposable(delegate
		{
			_E0AE.StimulatorBuffEvent -= _E000;
		});
	}

	private void _E000(_E986 buff)
	{
		if (buff.Settings.BuffType == EStimulatorBuffType.SkillRate && !(buff.Settings.SkillName != _E1A6.Id.ToString()))
		{
			_E001();
		}
	}

	private void _E001()
	{
		bool isEliteLevel = _E1A6.IsEliteLevel;
		int buff = _E1A6.Buff;
		bool flag = buff != 0;
		int summaryLevel = _E1A6.SummaryLevel;
		_level.text = (flag ? string.Format(_ED3E._E000(258997), _ED3E._E000(36568).Localized(), (buff > 0) ? _ED3E._E000(259025) : _ED3E._E000(259039), summaryLevel) : (isEliteLevel ? _ED3E._E000(258970).Localized() : string.Format(_ED3E._E000(168928), _ED3E._E000(36568).Localized(), summaryLevel)));
		_effectivenessDown.SetActive(_E1A6.Effectiveness < 1f && _E7A3.InRaid && !isEliteLevel);
		_effectivenessUp.SetActive(_E1A6.Effectiveness > 1f && _E7A3.InRaid && !isEliteLevel);
		_initialProgressFill.sprite = (isEliteLevel ? _eliteFillSprite : _normalFillSprite);
		_initialProgressFill.fillAmount = (isEliteLevel ? 1f : _E1A6.LevelProgress);
		_E1A9.UpdateItems(delegate(BuffIcon buffIcon)
		{
			buffIcon.UpdateBuff();
			buffIcon.UpdateVisibility();
		});
	}

	[CompilerGenerated]
	private void _E002(bool hover, PointerEventData eventData)
	{
		if (hover)
		{
			string text = "";
			if (_E7A3.InRaid && !_E1A6.IsEliteLevel)
			{
				if (_E1A6.Effectiveness > 1f)
				{
					text = string.Format(_ED3E._E000(259019), _ED3E._E000(259040).Localized(), (int)(_E1A6.Effectiveness * 100f));
				}
				else if (_E1A6.Effectiveness < 1f)
				{
					text = string.Format(_ED3E._E000(261136), _ED3E._E000(261165).Localized(), (int)(_E1A6.Effectiveness * 100f));
				}
			}
			_E02A.Show(string.Concat(_E1A6.Id, _ED3E._E000(69230)).Localized() + text);
		}
		else
		{
			_E02A.Close();
		}
	}

	[CompilerGenerated]
	private void _E003(_E74F._E003 buff, BuffIcon buffIcon)
	{
		buffIcon.Show(buff, _E1A6);
	}

	[CompilerGenerated]
	private void _E004()
	{
		_E0AE.StimulatorBuffEvent -= _E000;
	}
}
