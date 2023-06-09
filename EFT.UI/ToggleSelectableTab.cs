namespace EFT.UI;

public sealed class ToggleSelectableTab : Tab
{
	protected override bool CanHandlePointerClick => Interactable;

	public override void HandlePointerClick(bool isSelectedNow)
	{
		base.HandlePointerClick(isSelectedNow);
		if (isSelectedNow)
		{
			Deselect().HandleExceptions();
		}
		else
		{
			Select();
		}
	}
}
