using Comfort.Common;
using EFT.UI;

namespace EFT.InputSystem;

public class UIInputRoot : InputNode
{
	internal void _E000(InputNode node)
	{
		_children.Add(node);
		ItemUiContext instance = ItemUiContext.Instance;
		if (instance != null)
		{
			_children.Remove(instance);
			_children.Add(instance);
		}
		if (MonoBehaviourSingleton<PreloaderUI>.Instantiated)
		{
			ConsoleScreen console = MonoBehaviourSingleton<PreloaderUI>.Instance.Console;
			if (console != null)
			{
				_children.Remove(console);
				_children.Add(console);
			}
		}
	}

	internal void _E001(InputNode node)
	{
		_children.Remove(node);
	}

	protected override ECursorResult ShouldLockCursor()
	{
		if (!Singleton<AbstractGame>.Instantiated)
		{
			return ECursorResult.Ignore;
		}
		return ECursorResult.LockCursor;
	}

	protected override void TranslateAxes(ref float[] axes)
	{
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		return ETranslateResult.Ignore;
	}
}
