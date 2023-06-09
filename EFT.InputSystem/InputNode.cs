using System.Collections.Generic;

namespace EFT.InputSystem;

public abstract class InputNode : InputNodeAbstract
{
	public enum ETranslateResult
	{
		Ignore,
		Block,
		BlockAll
	}

	private static readonly HashSet<ECommand> _E02A = new HashSet<ECommand>
	{
		ECommand.MakeScreenshot,
		ECommand.ShowConsole,
		ECommand.ToggleInventory,
		ECommand.ToggleTalk,
		ECommand.StopTalk
	};

	public void RecacheChildren()
	{
		_children.Clear();
		for (int i = 0; i < base.transform.childCount; i++)
		{
			InputNode component = base.transform.GetChild(i).GetComponent<InputNode>();
			if (!(component == null))
			{
				_children.Add(component);
				component.RecacheChildren();
			}
		}
	}

	protected override void TranslateInput(List<ECommand> commands, ref float[] axes, ref ECursorResult shouldLockCursor)
	{
		base.TranslateInput(commands, ref axes, ref shouldLockCursor);
		int num = 0;
		while (num < commands.Count)
		{
			switch (TranslateCommand(commands[num]))
			{
			case ETranslateResult.Block:
				commands.RemoveAt(num);
				continue;
			case ETranslateResult.BlockAll:
				break;
			default:
				num++;
				continue;
			}
			commands.Clear();
			break;
		}
		if (axes != null)
		{
			TranslateAxes(ref axes);
		}
		ECursorResult eCursorResult = ShouldLockCursor();
		if (shouldLockCursor < eCursorResult)
		{
			shouldLockCursor = eCursorResult;
		}
	}

	protected static ETranslateResult GetDefaultBlockResult(ECommand command)
	{
		if (!_E02A.Contains(command))
		{
			return ETranslateResult.Block;
		}
		return ETranslateResult.Ignore;
	}

	protected abstract ETranslateResult TranslateCommand(ECommand command);

	protected abstract void TranslateAxes(ref float[] axes);

	protected abstract ECursorResult ShouldLockCursor();
}
