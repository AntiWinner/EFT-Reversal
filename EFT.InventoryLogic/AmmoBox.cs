using System;
using System.Collections.Generic;

namespace EFT.InventoryLogic;

[Serializable]
public class AmmoBox : ContainerCollection, IAmmoContainer
{
	public StackSlot Cartridges { get; protected set; }

	public int Count => Cartridges.Count;

	public int MaxCount => Cartridges.MaxCount;

	public override bool CanSellOnRagfair
	{
		get
		{
			if (base.CanSellOnRagfair)
			{
				return Count > 0;
			}
			return false;
		}
	}

	public override bool MergesWithChildren => true;

	public override IEnumerable<IContainer> Containers
	{
		get
		{
			yield return Cartridges;
		}
	}

	public override List<EItemInfoButton> ItemInteractionButtons
	{
		get
		{
			List<EItemInfoButton> itemInteractionButtons = base.ItemInteractionButtons;
			itemInteractionButtons.Add(EItemInfoButton.UnloadAmmo);
			return itemInteractionButtons;
		}
	}

	public override Item FindItem(string itemId)
	{
		if (Id == itemId)
		{
			return this;
		}
		return Cartridges.FindItem(itemId);
	}

	public AmmoBox(string id, AmmoBoxTemplate template)
		: base(id, template)
	{
		Cartridges = new StackSlot(template.StackSlots[0], this);
	}

	public override IContainer GetContainer(string containerId)
	{
		if (Cartridges == null || !(Cartridges.ID == containerId))
		{
			return null;
		}
		return Cartridges;
	}

	public _EA12 GetBulletAtPosition(int index)
	{
		return (_EA12)Cartridges.GetItemAtPosition(index);
	}

	public override int GetHashSum()
	{
		int num = base.GetHashSum();
		if (Cartridges == null)
		{
			return num;
		}
		foreach (Item item in Cartridges.Items)
		{
			num = num * 23 + item.GetHashSum();
		}
		return num;
	}
}
