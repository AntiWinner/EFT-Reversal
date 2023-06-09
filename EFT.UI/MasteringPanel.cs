using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class MasteringPanel : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public MasteringPanel _003C_003E4__this;

		public KeyValuePair<string, _E750> skill;

		internal void _E000()
		{
			_003C_003E4__this._level.text = _ED3E._E000(36568).Localized() + (skill.Value.Level + 1);
			_003C_003E4__this._initialProgressFill.sprite = _003C_003E4__this._masteringFillSprite;
			_003C_003E4__this._initialProgressFill.fillAmount = skill.Value.LevelProgress;
			_003C_003E4__this._masteringIcon.UpdateProgress(skill.Value);
		}
	}

	[SerializeField]
	private CustomTextMeshProUGUI _level;

	[SerializeField]
	private CustomTextMeshProUGUI _name;

	[SerializeField]
	private CustomTextMeshProUGUI _type;

	[SerializeField]
	private Image _initialProgressFill;

	[SerializeField]
	private MasteringIcon _masteringIcon;

	[SerializeField]
	private Sprite _masteringFillSprite;

	public void Show(KeyValuePair<string, _E750> skill)
	{
		ShowGameObject();
		_name.text = (skill.Key + _ED3E._E000(70087)).Localized();
		_type.text = MasteringScreen.GetItemTemplateText(skill.Key);
		_masteringIcon.Show(skill, null);
		UI.AddDisposable(_masteringIcon);
		UI.BindEvent(skill.Value.SkillLevelChanged, delegate
		{
			_level.text = _ED3E._E000(36568).Localized() + (skill.Value.Level + 1);
			_initialProgressFill.sprite = _masteringFillSprite;
			_initialProgressFill.fillAmount = skill.Value.LevelProgress;
			_masteringIcon.UpdateProgress(skill.Value);
		});
	}
}
