using System;
using EFT.InventoryLogic;

namespace EFT.UI.DragAndDrop;

public class AutoResizableGridView : GridView, _EC9F
{
	public void FitGridForItem(Item item)
	{
		if (Grid != null && item != null && (Grid.CanStretchVertically || Grid.CanStretchHorizontally))
		{
			LocationInGrid locationInGrid = Grid.FindFreeSpace(item);
			_E313 obj = item.CalculateCellSize();
			if (locationInGrid.r == ItemRotation.Vertical)
			{
				ref int x = ref obj.X;
				ref int y = ref obj.Y;
				int y2 = obj.Y;
				int x2 = obj.X;
				x = y2;
				y = x2;
			}
			int value = Grid.GridWidth.Value;
			int value2 = Grid.GridHeight.Value;
			_E313 obj2 = default(_E313);
			obj2.X = (Grid.CanStretchHorizontally ? Math.Max(locationInGrid.x + obj.X, value) : value);
			obj2.Y = (Grid.CanStretchVertically ? Math.Max(locationInGrid.y + obj.Y, value2) : value2);
			_E313 obj3 = obj2;
			if (obj3.X > value || obj3.Y > value2)
			{
				Grid.ClampSize(obj3.Y, obj3.X);
			}
		}
	}
}
