using System;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT.InventoryLogic;

public sealed class FoldableComponent : _EB19
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _E9D9 template;

		internal bool _E000(Slot x)
		{
			return x.ID == template.FoldedSlot;
		}
	}

	[CanBeNull]
	public readonly Slot FoldedSlot;

	[_E63C]
	public bool Folded;

	[NonSerialized]
	public readonly _ECEC OnChanged = new _ECEC();

	private readonly _E9D9 m__E000;

	public int SizeReduceRight => this.m__E000.SizeReduceRight;

	public bool CanBeFolded
	{
		get
		{
			if (FoldedSlot != null)
			{
				return FoldedSlot.ContainedItem != null;
			}
			return true;
		}
	}

	public void SetFolded(bool value)
	{
		Folded = value;
		OnChanged.Invoke();
		Item.UpdateAttributes();
	}

	public FoldableComponent(Item item, _E9D9 template)
		: base(item)
	{
		this.m__E000 = template;
		if (!string.IsNullOrEmpty(template.FoldedSlot))
		{
			Slot slot = ((_EA40)Item).Slots.FirstOrDefault((Slot x) => x.ID == template.FoldedSlot);
			if (slot == null)
			{
				Debug.LogErrorFormat(_ED3E._E000(215382), template.FoldedSlot, Item.Id, Item.Template._name);
			}
			FoldedSlot = slot;
		}
	}
}
