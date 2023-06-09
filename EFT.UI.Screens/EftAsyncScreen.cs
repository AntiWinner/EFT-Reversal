using System;
using System.Threading.Tasks;

namespace EFT.UI.Screens;

public abstract class EftAsyncScreen<TController, TScreen> : EftScreen<TController, TScreen> where TController : _EC91<TController, TScreen> where TScreen : EftAsyncScreen<TController, TScreen>
{
	public override void Show(TController controller)
	{
		throw new NotImplementedException();
	}

	public abstract Task ShowAsync(TController controller);
}
