using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class MasteringTooltip : Tooltip
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public MasteringTooltip _003C_003E4__this;

		public _E750 value;

		internal void _E000()
		{
			_003C_003E4__this._initialProgressFill.fillAmount = value.LevelProgress;
			_003C_003E4__this._level.text = _ED3E._E000(36568).Localized() + (value.Level + 1);
			_003C_003E4__this._progress.text = string.Format(_ED3E._E000(171743), value.LevelProgress * (float)value.LevelingThreshold, value.LevelingThreshold);
		}
	}

	[SerializeField]
	private CustomTextMeshProUGUI _name;

	[SerializeField]
	private CustomTextMeshProUGUI _type;

	[SerializeField]
	private Image _initialProgressFill;

	[SerializeField]
	private CustomTextMeshProUGUI _level;

	[SerializeField]
	private CustomTextMeshProUGUI _progress;

	public void Show(KeyValuePair<string, _E750> mastering)
	{
		Show(new Vector2(25f, -25f));
		var (text2, value) = mastering;
		_name.text = (text2 + _ED3E._E000(70087)).Localized();
		_type.text = MasteringScreen.GetItemTemplateText(text2);
		UI.BindEvent(value.SkillLevelChanged, delegate
		{
			_initialProgressFill.fillAmount = value.LevelProgress;
			_level.text = _ED3E._E000(36568).Localized() + (value.Level + 1);
			_progress.text = string.Format(_ED3E._E000(171743), value.LevelProgress * (float)value.LevelingThreshold, value.LevelingThreshold);
		});
	}
}
