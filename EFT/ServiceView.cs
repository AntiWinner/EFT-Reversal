using EFT.UI;

namespace EFT;

public abstract class ServiceView : UIElement
{
	public abstract bool CheckAvailable(_E8B2 trader);

	public abstract void Show(_E8B2 trader, Profile profile, _EAED controller, _E981 healthController, _E934 quests, ItemUiContext context, _E796 session);

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}
}
