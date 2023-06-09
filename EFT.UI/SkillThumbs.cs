using System.Runtime.CompilerServices;
using UnityEngine.EventSystems;

namespace EFT.UI;

public sealed class SkillThumbs : SkillContainer<SkillIcon>
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _E9C4 healthController;

		public SkillThumbs _003C_003E4__this;

		internal void _E000(_E751 skill, SkillIcon skillIcon)
		{
			_E001 CS_0024_003C_003E8__locals0 = new _E001
			{
				CS_0024_003C_003E8__locals1 = this,
				skill = skill
			};
			skillIcon.Show(CS_0024_003C_003E8__locals0.skill, healthController, delegate(bool hover, PointerEventData eventData)
			{
				if (hover)
				{
					CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1._003C_003E4__this.m__E000.Show(CS_0024_003C_003E8__locals0.skill, eventData.position);
				}
				else
				{
					CS_0024_003C_003E8__locals0.CS_0024_003C_003E8__locals1._003C_003E4__this.m__E000.Close();
				}
			});
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public _E751 skill;

		public _E000 CS_0024_003C_003E8__locals1;

		internal void _E000(bool hover, PointerEventData eventData)
		{
			if (hover)
			{
				CS_0024_003C_003E8__locals1._003C_003E4__this.m__E000.Show(skill, eventData.position);
			}
			else
			{
				CS_0024_003C_003E8__locals1._003C_003E4__this.m__E000.Close();
			}
		}
	}

	private SkillTooltip m__E000;

	public void Show(_E74F skills, DropDownBox sortMethod, DropDownBox filterMethod, _E9C4 healthController)
	{
		this.m__E000 = ItemUiContext.Instance.SkillTooltip;
		Show(skills, sortMethod, filterMethod, delegate(_E751 skill, SkillIcon skillIcon)
		{
			skillIcon.Show(skill, healthController, delegate(bool hover, PointerEventData eventData)
			{
				if (hover)
				{
					this.m__E000.Show(skill, eventData.position);
				}
				else
				{
					this.m__E000.Close();
				}
			});
		});
	}

	public override void Close()
	{
		if (this.m__E000 != null && this.m__E000.gameObject.activeSelf)
		{
			this.m__E000.Close();
		}
		base.Close();
	}
}
