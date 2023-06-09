using EFT.UI.Screens;
using EFT.UI.Settings;

namespace EFT.UI;

public sealed class LoginUI : MonoBehaviourSingleton<LoginUI>
{
	public QueueScreen QueueScreen;

	public LoginScreen LoginScreen;

	public ValidateDeviceIdScreen ValidateDeviceIdScreen;

	public WelcomeScreen WelcomeScreen;

	public SetNicknameScreen SetNicknameScreen;

	public SideSelectionScreen SideSelectionScreen;

	public RestorePasswordScreen RestorePasswordScreen;

	public ProfileLoadingScreen ProfileLoadingScreen;

	public override void Awake()
	{
		base.Awake();
		_EC92 instance = _EC92.Instance;
		instance.RegisterScreen(EEftScreenType.Login, LoginScreen);
		instance.RegisterScreen(EEftScreenType.ValidateDeviceId, ValidateDeviceIdScreen);
		instance.RegisterScreen(EEftScreenType.Welcome, WelcomeScreen);
		instance.RegisterScreen(EEftScreenType.SetNickname, SetNicknameScreen);
		instance.RegisterScreen(EEftScreenType.SelectPlayerSide, SideSelectionScreen);
		instance.RegisterScreen(EEftScreenType.RestorePassword, RestorePasswordScreen);
		instance.RegisterScreen(EEftScreenType.ProfileLoading, ProfileLoadingScreen);
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		_EC92 instance = _EC92.Instance;
		instance.ReleaseScreen(EEftScreenType.Login, LoginScreen);
		instance.ReleaseScreen(EEftScreenType.ValidateDeviceId, ValidateDeviceIdScreen);
		instance.ReleaseScreen(EEftScreenType.Welcome, WelcomeScreen);
		instance.ReleaseScreen(EEftScreenType.SetNickname, SetNicknameScreen);
		instance.ReleaseScreen(EEftScreenType.SelectPlayerSide, SideSelectionScreen);
		instance.ReleaseScreen(EEftScreenType.RestorePassword, RestorePasswordScreen);
		instance.ReleaseScreen(EEftScreenType.ProfileLoading, ProfileLoadingScreen);
	}
}
