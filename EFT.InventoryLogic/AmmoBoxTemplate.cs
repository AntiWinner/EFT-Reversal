using System;

namespace EFT.InventoryLogic;

[Serializable]
public class AmmoBoxTemplate : ItemTemplate
{
	public int magAnimationIndex;

	public StackSlot[] StackSlots = new StackSlot[0];
}
