using System.Runtime.CompilerServices;
using EFT.UI.Matchmaker;

namespace EFT.UI.Screens;

public abstract class MatchmakerEftScreen<TController, TScreen> : EftScreen<TController, TScreen> where TController : _EC8F<TController, TScreen> where TScreen : EftScreen<TController, TScreen>
{
	protected _EC99 MatchmakerPlayersController;

	public override void Show(TController controller)
	{
		MatchmakerPlayersController = controller.MatchmakerPlayersController;
		MatchmakerPlayersController.OnInviteAccepted += InviteAcceptedHandler;
		MatchmakerPlayersController.OnMatchingTypeUpdate += MatchingTypeUpdateHandler;
		UI.AddDisposable(delegate
		{
			MatchmakerPlayersController.OnInviteAccepted -= InviteAcceptedHandler;
			MatchmakerPlayersController.OnMatchingTypeUpdate -= MatchingTypeUpdateHandler;
		});
	}

	protected virtual void InviteAcceptedHandler()
	{
		ScreenController.CloseScreen();
	}

	protected virtual void MatchingTypeUpdateHandler(EMatchingType matchingType)
	{
		if (matchingType == EMatchingType.GroupPlayer)
		{
			ScreenController.CloseScreen();
		}
	}

	[CompilerGenerated]
	private void _E000()
	{
		MatchmakerPlayersController.OnInviteAccepted -= InviteAcceptedHandler;
		MatchmakerPlayersController.OnMatchingTypeUpdate -= MatchingTypeUpdateHandler;
	}
}
