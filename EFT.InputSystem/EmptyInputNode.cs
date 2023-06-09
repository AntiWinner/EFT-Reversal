namespace EFT.InputSystem;

public class EmptyInputNode : InputNode
{
	public void AddChildNode(InputNode node)
	{
		if (!_children.Contains(node))
		{
			_children.Add(node);
		}
	}

	public void RemoveChildNode(InputNode node)
	{
		_children.Remove(node);
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		return ETranslateResult.Ignore;
	}

	protected override ECursorResult ShouldLockCursor()
	{
		return ECursorResult.Ignore;
	}

	protected override void TranslateAxes(ref float[] axes)
	{
	}
}
