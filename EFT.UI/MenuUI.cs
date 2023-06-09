using EFT.UI.Matchmaker;
using EFT.UI.Screens;

namespace EFT.UI;

public sealed class MenuUI : MonoBehaviourSingleton<MenuUI>
{
	public TradingScreen TradingScreen;

	public MatchMakerSideSelectionScreen MatchMakerSideSelectionScreen;

	public MatchMakerSelectionLocationScreen MatchMakerSelectionLocationScreen;

	public MatchmakerKeyAccessScreen MatchmakerKeyAccessScreen;

	public MatchmakerMapPointsScreen MatchmakerMapPoints;

	public MatchMakerAcceptScreen MatchMakerAcceptScreen;

	public MatchmakerInsuranceScreen MatchmakerInsuranceScreen;

	public MatchmakerTimeHasCome MatchmakerTimeHasCome;

	public MatchmakerOfflineRaidScreen MatchmakerOfflineRaidScreen;

	public MatchmakerFinalCountdown MatchmakerFinalCountdown;

	public OperationQueueIndicator OperationQueueIndicator;

	public override void Awake()
	{
		base.Awake();
		_EC92 instance = _EC92.Instance;
		instance.RegisterScreen(EEftScreenType.Traders, TradingScreen);
		instance.RegisterScreen(EEftScreenType.FleaMarket, TradingScreen);
		instance.RegisterScreen(EEftScreenType.SelectRaidSide, MatchMakerSideSelectionScreen);
		instance.RegisterScreen(EEftScreenType.SelectLocation, MatchMakerSelectionLocationScreen);
		instance.RegisterScreen(EEftScreenType.KeyAccess, MatchmakerKeyAccessScreen);
		instance.RegisterScreen(EEftScreenType.MapPoints, MatchmakerMapPoints);
		instance.RegisterScreen(EEftScreenType.MatchMakerAccept, MatchMakerAcceptScreen);
		instance.RegisterScreen(EEftScreenType.Insurance, MatchmakerInsuranceScreen);
		instance.RegisterScreen(EEftScreenType.TimeHasCome, MatchmakerTimeHasCome);
		instance.RegisterScreen(EEftScreenType.OfflineRaid, MatchmakerOfflineRaidScreen);
		instance.RegisterScreen(EEftScreenType.FinalCountdown, MatchmakerFinalCountdown);
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		_EC92 instance = _EC92.Instance;
		instance.ReleaseScreen(EEftScreenType.Traders, TradingScreen);
		instance.ReleaseScreen(EEftScreenType.FleaMarket, TradingScreen);
		instance.ReleaseScreen(EEftScreenType.SelectRaidSide, MatchMakerSideSelectionScreen);
		instance.ReleaseScreen(EEftScreenType.SelectLocation, MatchMakerSelectionLocationScreen);
		instance.ReleaseScreen(EEftScreenType.KeyAccess, MatchmakerKeyAccessScreen);
		instance.ReleaseScreen(EEftScreenType.MapPoints, MatchmakerMapPoints);
		instance.ReleaseScreen(EEftScreenType.MatchMakerAccept, MatchMakerAcceptScreen);
		instance.ReleaseScreen(EEftScreenType.Insurance, MatchmakerInsuranceScreen);
		instance.ReleaseScreen(EEftScreenType.TimeHasCome, MatchmakerTimeHasCome);
		instance.ReleaseScreen(EEftScreenType.OfflineRaid, MatchmakerOfflineRaidScreen);
		instance.ReleaseScreen(EEftScreenType.FinalCountdown, MatchmakerFinalCountdown);
	}
}
