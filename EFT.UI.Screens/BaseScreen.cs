using System;
using System.Runtime.CompilerServices;

namespace EFT.UI.Screens;

public abstract class BaseScreen<TController, TScreen, TType> : UIScreen where TController : _EC95<TType>._E000<TController, TScreen> where TScreen : BaseScreen<TController, TScreen, TType> where TType : struct, Enum
{
	protected TController ScreenController;

	[CompilerGenerated]
	private bool _E0AE;

	public bool Destroyed
	{
		[CompilerGenerated]
		get
		{
			return _E0AE;
		}
		[CompilerGenerated]
		private set
		{
			_E0AE = value;
		}
	}

	public abstract void Show(TController controller);

	internal void _E000(TController controller)
	{
		ScreenController = controller;
	}

	public override void Close()
	{
		base.Close();
		ScreenController = null;
	}

	protected override void OnDestroy()
	{
		Destroyed = true;
		base.OnDestroy();
	}
}
