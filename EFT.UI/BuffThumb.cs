namespace EFT.UI;

public class BuffThumb : UIElement
{
	protected _E74F._E003 Buff;

	protected _E751 Skill;

	public void UpdateVisibility()
	{
		bool flag = (Buff.BuffType == _E74F.EBuffType.Elite && !Skill.IsEliteLevel) || (Buff.BuffType == _E74F.EBuffType.Plebian && Skill.IsEliteLevel) || Skill.SummaryLevel == 0;
		base.gameObject.SetActive(!flag);
	}
}
