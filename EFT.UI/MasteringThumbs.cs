using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine.EventSystems;

namespace EFT.UI;

public sealed class MasteringThumbs : MasteringContainer<MasteringIcon>
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public KeyValuePair<string, _E750> skill;

		public MasteringThumbs _003C_003E4__this;

		internal void _E000(bool hover, PointerEventData eventData)
		{
			if (hover)
			{
				_003C_003E4__this.m__E000.Show(skill);
			}
			else
			{
				_003C_003E4__this.m__E000.Close();
			}
		}
	}

	private MasteringTooltip m__E000;

	public void Show(KeyValuePair<string, _E750>[] skills, DropDownBox sortMethod, DropDownBox filterMethod, TMP_InputField inputField, _EAE6 inventoryController)
	{
		this.m__E000 = ItemUiContext.Instance.MasteringTooltip;
		Show(skills, sortMethod, filterMethod, inputField, inventoryController, delegate(KeyValuePair<string, _E750> skill, MasteringIcon skillIcon)
		{
			skillIcon.Show(skill, delegate(bool hover, PointerEventData eventData)
			{
				if (hover)
				{
					this.m__E000.Show(skill);
				}
				else
				{
					this.m__E000.Close();
				}
			});
		});
	}

	[CompilerGenerated]
	private void _E000(KeyValuePair<string, _E750> skill, MasteringIcon skillIcon)
	{
		skillIcon.Show(skill, delegate(bool hover, PointerEventData eventData)
		{
			if (hover)
			{
				this.m__E000.Show(skill);
			}
			else
			{
				this.m__E000.Close();
			}
		});
	}
}
