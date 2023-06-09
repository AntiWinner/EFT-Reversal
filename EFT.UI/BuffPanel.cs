using UnityEngine;

namespace EFT.UI;

public sealed class BuffPanel : BuffThumb
{
	[SerializeField]
	private BuffIcon _icon;

	[SerializeField]
	private CustomTextMeshProUGUI _description;

	public void Show(_E751 skill, _E74F._E003 buff)
	{
		ShowGameObject();
		Skill = skill;
		Buff = buff;
		_icon.Show(buff, skill);
		UI.AddDisposable(_icon);
	}

	public void UpdateBuff()
	{
		bool flag = Buff.BuffType == _E74F.EBuffType.Elite || (Skill.IsEliteLevel && Buff.BuffType == _E74F.EBuffType.Switching);
		_description.text = (flag ? _ED3E._E000(258237) : "") + BuffIcon.GetBuffDescription(Buff);
		_icon.UpdateBuff();
	}
}
