using UnityEngine;

namespace EFT.UI;

public class ServicesScreen : UIElement
{
	[SerializeField]
	private ServicesListView _servicesListView;

	public void Show(_E8B2 trader, Profile profile, _EAED inventoryController, _E9C4 healthController, _E934 quests, ItemUiContext context, _E796 session)
	{
		ShowGameObject();
		_servicesListView.Show(trader, profile, inventoryController, healthController, quests, context, session);
	}

	public bool CheckAvailableServices(_E8B2 trader)
	{
		return _servicesListView.CheckAvailableServices(trader);
	}

	public override void Close()
	{
		_servicesListView.Close();
		base.Close();
	}
}
