using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class SkillTooltip : Tooltip
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _E751 skill;

		public SkillTooltip _003C_003E4__this;

		public _EC79<_E74F._E003, BuffPanel> buffPanels;

		internal void _E000(_E74F._E003 buff, BuffPanel buffPanel)
		{
			buffPanel.Show(skill, buff);
		}

		internal void _E001()
		{
			bool isEliteLevel = skill.IsEliteLevel;
			_003C_003E4__this._levelPanel.SetLevel(skill);
			_003C_003E4__this._initialProgressFill.sprite = (isEliteLevel ? _003C_003E4__this._eliteFillSprite : _003C_003E4__this._normalFillSprite);
			_003C_003E4__this._initialProgressFill.fillAmount = (isEliteLevel ? 1f : skill.LevelProgress);
			_003C_003E4__this._effectivenessDown.SetActive(skill.Effectiveness < 1f && _E7A3.InRaid && !isEliteLevel);
			_003C_003E4__this._effectivenessUp.SetActive(skill.Effectiveness > 1f && _E7A3.InRaid && !isEliteLevel);
			_003C_003E4__this._level.text = (isEliteLevel ? _ED3E._E000(258970).Localized() : (_ED3E._E000(36568).Localized() + skill.SummaryLevel));
			_003C_003E4__this._progress.text = (isEliteLevel ? (_ED3E._E000(258924) + _ED3E._E000(258972).Localized() + _ED3E._E000(59467)) : string.Format(_ED3E._E000(261239), skill.LevelProgress * (float)skill.LevelExp, skill.LevelExp));
			buffPanels.UpdateItems(delegate(BuffPanel buffPanel)
			{
				buffPanel.UpdateBuff();
				buffPanel.UpdateVisibility();
			});
		}
	}

	[SerializeField]
	private SkillClassIcon _classIcon;

	[SerializeField]
	private CustomTextMeshProUGUI _name;

	[SerializeField]
	private SkillLevelPanel _levelPanel;

	[SerializeField]
	private CustomTextMeshProUGUI _description;

	[SerializeField]
	private Image _initialProgressFill;

	[SerializeField]
	private CustomTextMeshProUGUI _level;

	[SerializeField]
	private CustomTextMeshProUGUI _progress;

	[SerializeField]
	private Transform _buffContainer;

	[SerializeField]
	private BuffPanel _buffPanelTemplate;

	[SerializeField]
	private Sprite _normalFillSprite;

	[SerializeField]
	private Sprite _eliteFillSprite;

	[SerializeField]
	private GameObject _effectivenessUp;

	[SerializeField]
	private GameObject _effectivenessDown;

	public void Show(_E751 skill, Vector2 position)
	{
		Show(new Vector2(25f, -25f));
		_name.text = skill.Id.ToStringNoBox().Localized();
		string text = "";
		if (_E7A3.InRaid && !skill.IsEliteLevel)
		{
			if (skill.Effectiveness > 1f)
			{
				text = _ED3E._E000(261215) + _ED3E._E000(259040).Localized() + _ED3E._E000(12201) + skill.Effectiveness.ToString(_ED3E._E000(45972)) + _ED3E._E000(224448);
			}
			else if (skill.Effectiveness < 1f)
			{
				text = _ED3E._E000(261195) + _ED3E._E000(261165).Localized() + _ED3E._E000(12201) + skill.Effectiveness.ToString(_ED3E._E000(45972)) + _ED3E._E000(224448);
			}
		}
		_description.text = string.Concat(skill.Id, _ED3E._E000(69230)).Localized() + text;
		_classIcon.Set(skill.Class);
		_EC79<_E74F._E003, BuffPanel> buffPanels = UI.AddViewList(skill.Buffs, _buffPanelTemplate, _buffContainer, delegate(_E74F._E003 buff, BuffPanel buffPanel)
		{
			buffPanel.Show(skill, buff);
		});
		UI.BindEvent(skill.SkillLevelChanged, delegate
		{
			bool isEliteLevel = skill.IsEliteLevel;
			_levelPanel.SetLevel(skill);
			_initialProgressFill.sprite = (isEliteLevel ? _eliteFillSprite : _normalFillSprite);
			_initialProgressFill.fillAmount = (isEliteLevel ? 1f : skill.LevelProgress);
			_effectivenessDown.SetActive(skill.Effectiveness < 1f && _E7A3.InRaid && !isEliteLevel);
			_effectivenessUp.SetActive(skill.Effectiveness > 1f && _E7A3.InRaid && !isEliteLevel);
			_level.text = (isEliteLevel ? _ED3E._E000(258970).Localized() : (_ED3E._E000(36568).Localized() + skill.SummaryLevel));
			_progress.text = (isEliteLevel ? (_ED3E._E000(258924) + _ED3E._E000(258972).Localized() + _ED3E._E000(59467)) : string.Format(_ED3E._E000(261239), skill.LevelProgress * (float)skill.LevelExp, skill.LevelExp));
			buffPanels.UpdateItems(delegate(BuffPanel buffPanel)
			{
				buffPanel.UpdateBuff();
				buffPanel.UpdateVisibility();
			});
		});
		Display();
	}
}
