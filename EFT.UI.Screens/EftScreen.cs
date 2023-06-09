namespace EFT.UI.Screens;

public abstract class EftScreen<TController, TScreen> : BaseScreen<TController, TScreen, EEftScreenType> where TController : _EC92._E000<TController, TScreen> where TScreen : EftScreen<TController, TScreen>
{
}
