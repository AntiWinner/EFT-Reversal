using System;

namespace EFT.InventoryLogic;

public class TogglableComponent : _EB19, ITogglableComponent, IItemComponent
{
	[_E63C]
	public bool On;

	[NonSerialized]
	public _ECEC OnChanged = new _ECEC();

	public float LastToggleTime;

	bool ITogglableComponent.On => On;

	public TogglableComponent(Item item)
		: base(item)
	{
	}

	public void Toggle()
	{
		Set(!On);
	}

	public void ToggleSilent()
	{
		Set(!On, simulate: false, silent: true);
	}

	public _ECD8<_EB4E> Set(bool value, bool simulate = false, bool silent = false)
	{
		if (On == value)
		{
			return new _ECD2(_ED3E._E000(215978) + value);
		}
		if (!simulate)
		{
			On = value;
			if (!silent)
			{
				OnChanged.Invoke();
			}
		}
		return new _EB4E(this, Item?.Parent, value);
	}
}
