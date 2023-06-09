using EFT.HandBook;
using EFT.Hideout;
using EFT.UI.Chat;
using EFT.UI.Screens;
using EFT.UI.Settings;
using EFT.UI.WeaponModding;

namespace EFT.UI;

public sealed class CommonUI : MonoBehaviourSingleton<CommonUI>
{
	public MenuScreen MenuScreen;

	public ReconnectionScreen ReconnectionScreen;

	public TransferItemsScreen TransferItemsScreen;

	public InventoryScreen InventoryScreen;

	public ScavengerInventoryScreen ScavengerInventoryScreen;

	public WeaponModdingScreen WeaponModdingScreen;

	public EditBuildScreen EditBuildScreen;

	public SettingsScreen SettingsScreen;

	public HandbookScreen HandbookScreen;

	public ChatScreen ChatScreen;

	public HideoutScreenRear HideoutScreenRear;

	public HideoutScreenOverlay HideoutScreenOverlay;

	public TraderDialogScreen TraderDialogScreen;

	public override void Awake()
	{
		base.Awake();
		_EC92 instance = _EC92.Instance;
		instance.RegisterScreen(EEftScreenType.MainMenu, MenuScreen);
		instance.RegisterScreen(EEftScreenType.Reconnect, ReconnectionScreen);
		instance.RegisterScreen(EEftScreenType.TransferItems, TransferItemsScreen);
		instance.RegisterScreen(EEftScreenType.Inventory, InventoryScreen);
		instance.RegisterScreen(EEftScreenType.ScavInventory, ScavengerInventoryScreen);
		instance.RegisterScreen(EEftScreenType.WeaponModding, WeaponModdingScreen);
		instance.RegisterScreen(EEftScreenType.EditBuild, EditBuildScreen);
		instance.RegisterScreen(EEftScreenType.Settings, SettingsScreen);
		instance.RegisterScreen(EEftScreenType.Handbook, HandbookScreen);
		instance.RegisterScreen(EEftScreenType.Hideout, HideoutScreenRear);
		instance.RegisterScreen(EEftScreenType.TraderDialog, TraderDialogScreen);
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		_EC92 instance = _EC92.Instance;
		instance.ReleaseScreen(EEftScreenType.MainMenu, MenuScreen);
		instance.ReleaseScreen(EEftScreenType.Reconnect, ReconnectionScreen);
		instance.ReleaseScreen(EEftScreenType.TransferItems, TransferItemsScreen);
		instance.ReleaseScreen(EEftScreenType.Inventory, InventoryScreen);
		instance.ReleaseScreen(EEftScreenType.ScavInventory, ScavengerInventoryScreen);
		instance.ReleaseScreen(EEftScreenType.WeaponModding, WeaponModdingScreen);
		instance.ReleaseScreen(EEftScreenType.EditBuild, EditBuildScreen);
		instance.ReleaseScreen(EEftScreenType.Settings, SettingsScreen);
		instance.ReleaseScreen(EEftScreenType.Handbook, HandbookScreen);
		instance.ReleaseScreen(EEftScreenType.Hideout, HideoutScreenRear);
		instance.ReleaseScreen(EEftScreenType.TraderDialog, TraderDialogScreen);
	}
}
