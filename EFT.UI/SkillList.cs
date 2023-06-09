using System.Runtime.CompilerServices;

namespace EFT.UI;

public class SkillList : SkillContainer<SkillPanel>
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _E9C4 healthController;

		internal void _E000(_E751 skill, SkillPanel skillView)
		{
			skillView.Show(skill, healthController);
		}
	}

	public void Show(_E74F skills, DropDownBox sortMethod, DropDownBox filterMethod, _E9C4 healthController)
	{
		Show(skills, sortMethod, filterMethod, delegate(_E751 skill, SkillPanel skillView)
		{
			skillView.Show(skill, healthController);
		});
	}
}
