using System.Collections.Generic;
using TMPro;

namespace EFT.UI;

public sealed class MasteringList : MasteringContainer<MasteringPanel>
{
	public void Show(KeyValuePair<string, _E750>[] skills, DropDownBox sortMethod, DropDownBox filterMethod, TMP_InputField inputField, _EAE6 inventoryController)
	{
		Show(skills, sortMethod, filterMethod, inputField, inventoryController, delegate(KeyValuePair<string, _E750> skill, MasteringPanel skillView)
		{
			skillView.Show(skill);
		});
	}
}
